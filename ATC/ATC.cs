using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATC
{
    public delegate void UserConnectionDelegate(Signal signal);
    public class ATC: Transmitter
    {

        public event LogDelegate logEvent;
        public event ActionDelegate connectionsChanged;

        public ATC(string id, string name) : base(id) {
            ATCNameService.Save(name, id);
        }

        public List<ATC> connectedATCs = new List<ATC>();
        public List<User> connectedUsers = new List<User>();
        public List<Connection> connections = new List<Connection>();
        private List<User> waitingForTheResponceFrom = new List<User>();
        private Dictionary<string, bool> isGoingToMakeOutgoingCall = new Dictionary<string, bool>();

        public string name
        {
            get
            {
                return $"{ATCNameService.GetName(this.id)} ({this.id})";
            }
        }

        public string nameExtended
        {
            get
            {
                return name + $" [{connections.Count}]";
            }
        }

        private void log(string message)
        {
            if (logEvent != null)
            {
                logEvent(message);
            }
        }

        public void disconnect()
        {
            logEvent = null;
            connectionsChanged = null;
            foreach(ATC a in connectedATCs)
            {
                a.disconnect(this);
            }
            for(int i = connectedUsers.Count - 1; i > -1; i--)
            {
                connectedUsers[i].handle(new Signal((Transmitter)this, SignalType.cancel, this.id));
                disconnect(connectedUsers[i]);
            }
        }

        public void disconnect(ATC atc)
        {
            for(int i = 0; i < connections.Count; i++)
            {
                if (connections[i].connectedATCs.ContainsKey(atc))
                {
                    handleCancel(new Signal((Transmitter)atc, SignalType.cancel, atc.id), connections[i],this.id);
                }
            }
            connectedATCs.Remove(atc);
        }

        public bool connect(User user)
        {
            if (!connectedUsers.Exists(u => u.id == user.id))
            {
                user.sendSignal += new UserConnectionDelegate(handle);
                connectedUsers.Add(user);
                log($"User {user.id} connected to ATC.");
                return true;
            }
            return false;
        }

        public void disconnect(User user)
        {
            user.sendSignal -= new UserConnectionDelegate(handle);
            user.currentATC = null;
            connectedUsers.Remove(user);
            log($"User {user.id} removed from ATC.");
        }

        public void connect(ATC atc)
        {
            if (!connectedATCs.Exists(a => a.id == atc.id))
            {
                connectedATCs.Add(atc);
            }
            log($"{atc.name} ATC connected to ATC.");
        }

        void connectUserToATC(User user, Connection connection)
        {
            string atcID = connection.destinationNumber[0].ToString() + connection.destinationNumber[1].ToString();
            ATC atc = connectedATCs.Find(x => x.id == atcID);

            if (atc == null)
            {
                log($"User {user.id} requested ATC {ATCNameService.GetName(atcID)} ({atcID}) that is not connected to the ATC.");
                foreach (ATC anotherATC in connectedATCs)
                {
                    log($"Requested ATC {anotherATC.name} to connect user {user.id} to ATC {ATCNameService.GetName(atcID)} ({atcID}).");
                    anotherATC.connectUserToATC(user, connection);
                }
            } else
            {
                string existingConnectionID = ATC.FindExistingConnectionID(this.id, connection, atc, this.id);
                if(existingConnectionID != null)
                {
                    atc.handleIncomingConnection(existingConnectionID, this, getUserId(connection.destinationNumber), connection.id, user.GlobalID);
                    log($"Connected User {user.id} to ATC {ATCNameService.GetName(atcID)} ({atcID}). Connection ID: {connection.id}");

                } else
                {
                    string connectionID = atc.handleIncomingConnection(this, getUserId(connection.destinationNumber), connection.id, user.GlobalID);
                    connection.ConnectATC(atc, connectionID);
                    log($"Connected User {user.id} to ATC {ATCNameService.GetName(atcID)} ({atcID}). Connection ID: {connection.id}");
                }
            }

        }

        static string FindExistingConnectionID(string connectionATCid, Connection connection, ATC desiredATC, string sourceATC)
        {
            foreach(ATC atc in connection.connectedATCs.Keys)
            {
                if (atc.id != sourceATC)
                {
                    if (atc.id == desiredATC.id)
                    {
                        return connection.connectedATCs[atc];
                    }
                    else
                    {
                        Connection lowerConnection = atc.connections.Find(c => c.id == connection.connectedATCs[atc]);
                        if (lowerConnection != null)
                        {
                            return ATC.FindExistingConnectionID(atc.id, lowerConnection, desiredATC, connectionATCid);
                        }
                    }
                }
            }

            return null;
        }

        public void handleIncomingConnection(string existingConnectionID, ATC atc, string destinationNumber, string connectionID, string callerID)
        {
            Connection connection = connections.Find(c => c.id == existingConnectionID);
          
            connection.destinationNumber = destinationNumber;
            connectionsChanged();
            connectUserWithIncommingCaller(connection, atc, callerID, true);
            log($"Handled incoming connection from ATC {atc.name}. Destination number: {destinationNumber}. Caller ID: {callerID}. This connection between these ATCs has existed before.");
        }

        public string handleIncomingConnection(ATC atc, string destinationNumber, string connectionID, string callerID)
        {
            Connection connection = new Connection(getConnectionID());
            connection.ConnectATC(atc, connectionID);
            connection.destinationNumber = destinationNumber;
            connections.Add(connection);
            connectionsChanged();
            connectUserWithIncommingCaller(connection, atc, callerID, false);
            log($"Handled incoming connection from ATC {atc.name}. Destination number: {destinationNumber}. Caller ID: {callerID}.");
            return connection.id;
        }

        string getConnectionID()
        {
            string id = "000";
            while(connections.Exists(c => c.id == id))
            {
                id = IdUtility.IncrementStringID(id);
            }
            return id;
        }

        string getATCid(string GlobalID)
        {
            return GlobalID[0].ToString() + GlobalID[1].ToString();
        }

        string getUserId(string GlobalID)
        {
            return GlobalID[2].ToString() + GlobalID[3].ToString() + GlobalID[4].ToString();
        }

        public void handleSignalFromATC(Signal signal, string atcID, string connectionID)
        {
            Connection connection = connections.Find(c => c.id == connectionID);

            if (connection != null)
            {
                switch (signal.type)
                {
                    case SignalType.busy:
                    case SignalType.offline:
                        handleIncomingBusyOffline(signal, connection, atcID);
                        break;
                    case SignalType.message:
                        handleMessage(signal, connection, atcID);
                        break;
                    case SignalType.cancel:
                        handleCancel(signal, connection, atcID);
                        break;
                }
            }
        }

        void handleIncomingTone(Signal signal, string atcID, string connectionID)
        {
            Connection connection = connections.Find(c => c.id == connectionID);

            string sourceAtcID = getATCid(signal.sender.id);

            foreach (User user in connection.users)
            {
                user.handle(new Signal((Transmitter)this, SignalType.tone, signal.message));
                log($"Sended User {user.id} a tone signal. Connection ID: {connection.id}.");
            }
            foreach (ATC connectedATC in connection.connectedATCs.Keys)
            {
                if (connectedATC.id != sourceAtcID && connectedATC.id != atcID)
                {
                    connectedATC.handleSignalFromATC(signal, this.id, connection.connectedATCs[connectedATC]);
                    log($"Sended ATC {connectedATC.name} a tone signal. Connection ID: {connection.id}.");
                }
            }

            connectionsChanged();
        }

        void handleIncomingBusyOffline(Signal signal, Connection connection, string atcID)
        {
            string sourceAtcID = getATCid(signal.sender.id);

            ATC atcToRemove = connection.connectedATCs.Keys.ToList().Find(a => a.id == sourceAtcID);
            if (atcToRemove != null)
            {
                connection.connectedATCs.Remove(atcToRemove);
                log($"Removed ATC {atcToRemove.name} from connection {connection.id}.");
            }

            Signal busyOfflineSignal = new Signal((Transmitter)this, signal.type, $"{ATCNameService.GetName(sourceAtcID)} ({sourceAtcID})");
            Signal cancelSignal = new Signal((Transmitter)this, SignalType.cancel, $"{ATCNameService.GetName(sourceAtcID)} ({sourceAtcID})");

            foreach (User user in connection.users)
            {
                user.handle(busyOfflineSignal);
                user.handle(cancelSignal);
                log($"Sended User {user.id} a cancel signal. Connection ID: {connection.id}.");
            }
            foreach (ATC connectedATC in connection.connectedATCs.Keys)
            {
                connectedATC.handleSignalFromATC(signal, this.id, connection.connectedATCs[connectedATC]);
                log($"Sended ATC {connectedATC.name} a cancel signal. Connection ID: {connection.id}.");
            }

            if (connection.users.Count + connection.connectedATCs.Keys.Count == 1)
            {
                Signal connectionClosedSignal = new Signal((Transmitter)this, SignalType.tone, "Connection closed.");

                if (connection.users.Count == 1)
                {
                    User lastUser = connection.users.First();
                    lastUser.handle(connectionClosedSignal);
                    log($"Sended User {lastUser.id} a tone signal. Connection ID: {connection.id}. Connection Closed.");
                }
                else
                {
                    ATC lastATC = connection.connectedATCs.Keys.First();
                    lastATC.handleSignalFromATC(connectionClosedSignal, this.id, connection.connectedATCs[lastATC]);
                    log($"Sended ATC {lastATC.name} a tone signal. Connection ID: {connection.id}. Connection Closed.");
                }

                connections.Remove(connection);
                connectionsChanged();
            }
        }

        public void handle(Signal signal)
        {
            if (signal.sender is User)
            {
                User sender = signal.sender as User;

                Connection connection = connections.Find(x => x.users.Exists(user => user.id == sender.id));

                if (connection == null)
                {
                    if (signal.type == SignalType.phone)
                    {
                        if (waitingForTheResponceFrom.Exists(x => x.id == sender.id))
                        {
                            Connection existingConnection = connections.Find(x => x.destinationNumber == sender.id);

                            if (existingConnection != null)
                            {
                                foreach (User user in existingConnection.users)
                                {
                                    user.handle(new Signal((Transmitter)this, SignalType.tone, $"You are now connected with user {sender.GlobalID}"));
                                }
                                foreach (ATC atc in existingConnection.connectedATCs.Keys)
                                {
                                    atc.handleIncomingTone(new Signal((Transmitter)this, SignalType.phone, $"You are now connected with user {sender.GlobalID}"), this.id, existingConnection.connectedATCs[atc]);
                                }
                                existingConnection.users.Add(sender);
                                existingConnection.status = ConnectionStatus.Connected;
                                log($"User {sender.id} is now connected. Connection ID: {existingConnection.id}");
                                waitingForTheResponceFrom.RemoveAll(x => x.id == sender.id);
                            } else
                            {
                                waitingForTheResponceFrom.RemoveAll(x => x.id == sender.id);
                            }

                        }
                        else
                        {
                            Connection newConnection = new Connection(getConnectionID(), sender);
                            connections.Add(newConnection);
                            connectionsChanged();
                            sender.handle(new Signal((Transmitter)this, SignalType.tone, "Please enter destination number..."));
                            log($"User {sender.id} is trying to connect. Waiting for the destination number.");
                        }
                    } else if (signal.type == SignalType.cancel)
                    {
                        if (waitingForTheResponceFrom.Exists(x => x.id == sender.id))
                        {
                            Connection existingConnection = connections.Find(x => x.destinationNumber == sender.id);

                            if (existingConnection != null)
                            {
                                foreach (User user in existingConnection.users)
                                {
                                    user.handle(new Signal((Transmitter)this, SignalType.tone, $"{sender.GlobalID} have canceled call."));
                                }
                                foreach (ATC atc in existingConnection.connectedATCs.Keys)
                                {
                                    atc.handleSignalFromATC(new Signal((Transmitter)this, SignalType.cancel, $"{sender.GlobalID} have canceled call."), this.id, existingConnection.connectedATCs[atc]);
                                }
                                log($"User {sender.id} canceled call. Abandoning connection. {existingConnection.id}");
                                waitingForTheResponceFrom.RemoveAll(x => x.id == sender.id);
                                connections.Remove(existingConnection);
                                connectionsChanged();
                            }
                            else
                            {
                                waitingForTheResponceFrom.RemoveAll(x => x.id == sender.id);
                            }

                        }
                    }
                }
                else
                {
                    switch (signal.type)
                    {
                        case SignalType.number:
                            handleNumber(signal, connection);
                            break;
                        case SignalType.message:
                            handleMessage(signal, connection, this.id);
                            break;
                        case SignalType.cancel:
                            handleCancel(signal, connection, this.id);
                            break;
                        case SignalType.phone:
                            handlePhoneSignalWithExistingConnection(sender, signal, connection);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        void handlePhoneSignalWithExistingConnection(User sender, Signal signal, Connection connection)
        {
            
            sender.handle(new Signal((Transmitter)this, SignalType.tone, "Please enter destination number..."));
            log($"User {sender.id} that already has a connection is trying to connect. Waiting for the destination number.");
        }

        void connectUserWithIncommingCaller(Connection connection, ATC atc, string callerID, bool existingConnection)
        {
            connection.status = ConnectionStatus.DestinationSelected;

            User user = connectedUsers.Find(x => x.id == connection.destinationNumber);

            if (user == null)
            {
                Signal offlineSignal = new Signal((Transmitter)this, SignalType.offline);

                log($"Sended ATC {atc.name} a signal. Requested User {connection.destinationNumber} is offline.");
                if (existingConnection)
                {
                    foreach(User u in connection.users)
                    {
                        u.handle(offlineSignal);
                        log($"User {u.id} received response. User {connection.destinationNumber} is offline.");
                    }

                    foreach(ATC a in connection.connectedATCs.Keys)
                    {
                        a.handleSignalFromATC(offlineSignal, this.id, connection.connectedATCs[a]);
                        log($"ATC {a.id} received response. User {connection.destinationNumber} is offline.");
                    }


                } else
                {
                    atc.handleSignalFromATC(new Signal((Transmitter)this, SignalType.offline), this.id, connection.connectedATCs[atc]);
                    log($"ATC {atc.id} received response. User {connection.destinationNumber} is offline.");
                    connections.Remove(connection);
                    connectionsChanged();
                }
            }
            else
            {
                if (connections.Exists(c => c.users.Exists(x => x.id == connection.destinationNumber)))
                {

                    Signal busySignal = new Signal((Transmitter)this, SignalType.busy);

                    if (existingConnection)
                    {

                        foreach (User u in connection.users)
                        {
                            u.handle(busySignal);
                            log($"User {u.id} received response. User {connection.destinationNumber} is busy.");
                        }

                        foreach (ATC a in connection.connectedATCs.Keys)
                        {
                            a.handleSignalFromATC(busySignal, this.id, connection.connectedATCs[a]);
                            log($"ATC {a.id} received response. User {connection.destinationNumber} is busy.");
                        }

                    } else
                    {
                        log($"Sended ATC {atc.name} a signal. Requested User {connection.destinationNumber} is busy.");
                        atc.handleSignalFromATC(busySignal, this.id, connection.connectedATCs[atc]);
                        connections.Remove(connection);
                        connectionsChanged();
                    }
                }
                else
                {

                    Signal callSignal = new Signal((Transmitter)this, SignalType.call, user.id);

                    if (!existingConnection)
                    {
                        atc.handleSignalFromATC(callSignal, this.id, connection.connectedATCs[atc]);
                    }
                    
                    user.handle(callSignal);
                    waitingForTheResponceFrom.Add(user);
                    log($"Sended ATC {atc.name} a call signal. Waiting for the responce from User {user.id}.");
                }
            }
        }
        async void handleNumber(Signal signal, Connection connection)
        {

            if (signal.sender is User)
            {
                User sender = signal.sender as User;

                if (connection.status == ConnectionStatus.Started || connection.status == ConnectionStatus.TryingToAddAnotherUser)
                {

                    string number = signal.message;

                    connection.destinationNumber += number;

                    if (connection.destinationNumber.Length == 3)
                    {

                        isGoingToMakeOutgoingCall[sender.id] = false;

                        await Task.Delay(2000);

                        if (isGoingToMakeOutgoingCall.ContainsKey(sender.id))
                        {
                            if (!isGoingToMakeOutgoingCall[sender.id])
                            {
                                connection.status = ConnectionStatus.DestinationSelected;

                                User user = connectedUsers.Find(x => x.id == connection.destinationNumber);

                                if (user == null)
                                {
                                    sender.handle(new Signal((Transmitter)this, SignalType.offline));
                                    connections.Remove(connection);
                                    connectionsChanged();
                                    log($"User {sender.id} received response. User {connection.destinationNumber} is offline.");
                                }
                                else
                                {
                                    if (connections.Exists(c => c.users.Exists(x => x.id == connection.destinationNumber)))
                                    {
                                        sender.handle(new Signal((Transmitter)this, SignalType.busy));
                                        connections.Remove(connection);
                                        connectionsChanged();
                                        log($"User {sender.id} received response. User {connection.destinationNumber} is busy.");
                                    }
                                    else
                                    {
                                        sender.handle(new Signal((Transmitter)this, SignalType.call, user.id));
                                        user.handle(new Signal((Transmitter)this, SignalType.call, sender.id));
                                        waitingForTheResponceFrom.Add(user);
                                        log($"Sended user {sender.id} a call signal. Waiting for the responce from User {user.id}.");
                                    }
                                }
                            }

                            isGoingToMakeOutgoingCall.Remove(sender.id);
                        }

                    } else if (connection.destinationNumber.Length == 5)
                    {
                        isGoingToMakeOutgoingCall[sender.id] = true;

                        connection.status = ConnectionStatus.DestinationSelected;

                        connectUserToATC(sender, connection);

                    }

                } else
                {
                    connection.status = ConnectionStatus.TryingToAddAnotherUser;
                    connection.destinationNumber = signal.message;
                }
            }
        }

        void handleMessage(Signal signal, Connection connection, string atcID)
        {
            if (signal.sender is User)
            {
                User sender = signal.sender as User;

                string sourceAtcID = getATCid(sender.GlobalID);

                bool isSignalLocal = sourceAtcID == this.id;

                foreach (ATC connectedATC in connection.connectedATCs.Keys)
                {
                    if (isSignalLocal || (connectedATC.id != sourceAtcID && connectedATC.id != atcID))
                    {
                        connectedATC.handleSignalFromATC(signal, this.id, connection.connectedATCs[connectedATC]);
                        log($"Sended ATC {connectedATC.name} a message signal. Connection ID: {connection.id}.");
                    }
                }

                foreach (User connectedUser in connection.users)
                {
                    if (!isSignalLocal || isSignalLocal && connectedUser.id != signal.sender.id)
                    {
                        connectedUser.handle(signal);
                        log($"Sended User {connectedUser.id} a message signal. Connection ID: {connection.id}.");
                    }
                }
            }
        } 

        void handleCancel(Signal signal, Connection connection, string atcID)
        {
            if (connection.status == ConnectionStatus.TryingToAddAnotherUser && signal.sender is User)
            {
                connection.status = ConnectionStatus.TryingToAddAnotherUser;
                connection.destinationNumber = "";
                (signal.sender as User).handle(new Signal((Transmitter)this, SignalType.tone, $"Destination number cleared."));

                if (isGoingToMakeOutgoingCall.ContainsKey(signal.sender.id))
                {
                    isGoingToMakeOutgoingCall.Remove(signal.sender.id);
                }

                return;
            }

            string sourceAtcID = "";

            if (signal.sender is User)
            {
                sourceAtcID = getATCid((signal.sender as User).GlobalID);
            } else
            {
                sourceAtcID = signal.sender.id;
            }

            bool isSignalLocal = sourceAtcID == this.id;

            if (isSignalLocal)
            {
                connection.users.Remove(signal.sender as User);
                log($"Removed User {signal.sender.id} from connection {connection.id}.");
            } else
            {
                ATC atcToRemove = connection.connectedATCs.Keys.ToList().Find(a => a.id == sourceAtcID);
                if (atcToRemove != null)
                {
                    connection.connectedATCs.Remove(atcToRemove);
                    log($"Removed ATC {atcToRemove.name} from connection {connection.id}.");
                }
                
            }

            Signal cancelSignal = new Signal((Transmitter)this, SignalType.cancel, isSignalLocal ? signal.sender.id : $"{ATCNameService.GetName(sourceAtcID)} ({sourceAtcID})");

            foreach (User user in connection.users)
            {
                user.handle(cancelSignal);
                log($"Sended User {user.id} a cancel signal. Connection ID: {connection.id}.");
            }
            foreach (ATC connectedATC in connection.connectedATCs.Keys)
            {
                if (connectedATC.id != atcID)
                {
                    connectedATC.handleSignalFromATC(signal, this.id, connection.connectedATCs[connectedATC]);
                    log($"Sended ATC {connectedATC.name} a cancel signal. Connection ID: {connection.id}.");
                }
            }

            if (connection.users.Count + connection.connectedATCs.Keys.Count == 1)
            {
                Signal connectionClosedSignal = new Signal((Transmitter)this, SignalType.tone, "Connection closed.");

                if (connection.users.Count == 1)
                {
                    User lastUser = connection.users.First();
                    lastUser.handle(connectionClosedSignal);
                    log($"Sended User {lastUser.id} a tone signal. Connection ID: {connection.id}. Connection Closed.");
                } else
                {
                    ATC lastATC = connection.connectedATCs.Keys.First();
                    if (lastATC.id != atcID)
                    {
                        lastATC.handleSignalFromATC(connectionClosedSignal, this.id, connection.connectedATCs[lastATC]);
                        log($"Sended ATC {lastATC.name} a tone signal. Connection ID: {connection.id}. Connection Closed.");
                    }
                }

                connections.Remove(connection);
                connectionsChanged();
            }
        }
    }
}
