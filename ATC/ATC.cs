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

        public ATC(string id, string name) : base(id) {
            ATCNameService.Save(name, id);
        }

        public List<ATC> connectedATCs = new List<ATC>();
        private List<User> connectedUsers = new List<User>();
        private List<Connection> connections = new List<Connection>();
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

        public void connect(User user)
        {
            user.sendSignal += new UserConnectionDelegate(handle);
            connectedUsers.Add(user);
            logEvent($"User {user.id} connected to ATC.");
        }

        public void disconnect(User user)
        {
            connectedUsers.Remove(user);
            logEvent($"User {user.id} removed from ATC.");
        }

        public void connect(ATC atc)
        {
            if (!connectedATCs.Exists(a => a.id == atc.id))
            {
                connectedATCs.Add(atc);
            }
            logEvent($"{atc.name} ATC connected to ATC.");
        }

        void connectUserToATC(User user, Connection connection)
        {
            string atcID = connection.destinationNumber[0].ToString() + connection.destinationNumber[1].ToString();
            ATC atc = connectedATCs.Find(x => x.id == atcID);

            if (atc == null)
            {
                logEvent($"User {user.id} requested ATC {ATCNameService.GetName(atcID)} ({atcID}) that is not connected to the ATC.");
                foreach (ATC anotherATC in connectedATCs)
                {
                    logEvent($"Requested ATC {anotherATC.name} to connect user {user.id} to ATC {ATCNameService.GetName(atcID)} ({atcID}).");
                    anotherATC.connectUserToATC(user, connection);
                }
            } else
            {
                string connectionID = atc.handleIncomingConnection(this, getUserId(connection.destinationNumber), connection.id, user.GlobalID);
                connection.ConnectATC(atc, connectionID);
                logEvent($"Connected User {user.id} to ATC {ATCNameService.GetName(atcID)} ({atcID}). Connection ID: {connection.id}");
            }

        }

        public string handleIncomingConnection(ATC atc, string destinationNumber, string connectionID, string callerID)
        {
            Connection connection = new Connection(getConnectionID());
            connection.ConnectATC(atc, connectionID);
            connection.destinationNumber = destinationNumber;
            connections.Add(connection);
            connectUserWithIncommingCaller(connection, atc, callerID);
            logEvent($"Handled incoming connection from ATC {atc.name}. Destination number: {destinationNumber}. Caller ID: {callerID}.");
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

        public void handleSignalFromATC(Signal signal, string connectionID)
        {
            Connection connection = connections.Find(c => c.id == connectionID);
            switch (signal.type)
            {
                case SignalType.message:
                    handleMessage(signal, connection);
                    break;
                case SignalType.cancel:
                    handleCancel(signal, connection);
                    break;
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
                                existingConnection.users.Add(sender);
                                existingConnection.status = ConnectionStatus.Connected;
                                logEvent($"User {sender.id} is now connected. Connection ID: {existingConnection.id}");
                            }

                        }
                        else
                        {
                            Connection newConnection = new Connection(getConnectionID(), sender);
                            connections.Add(newConnection);
                            sender.handle(new Signal((Transmitter)this, SignalType.tone, "Please enter destination number..."));
                            logEvent($"User {sender.id} is trying to connect. Waiting for the destination number.");
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
                            handleMessage(signal, connection);
                            break;
                        case SignalType.cancel:
                            handleCancel(signal, connection);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        void connectUserWithIncommingCaller(Connection connection, ATC atc, string callerID)
        {
            connection.status = ConnectionStatus.DestinationSelected;

            User user = connectedUsers.Find(x => x.id == connection.destinationNumber);

            if (user == null)
            {
                logEvent($"Sended ATC {atc.name} a signal. Requested User {connection.destinationNumber} is offline.");
                atc.handleSignalFromATC(new Signal((Transmitter)this, SignalType.offline), connection.connectedATCs[atc]);
            }
            else
            {
                if (connections.Exists(c => c.users.Exists(x => x.id == connection.destinationNumber)))
                {
                    logEvent($"Sended ATC {atc.name} a signal. Requested User {connection.destinationNumber} is busy.");
                    atc.handleSignalFromATC(new Signal((Transmitter)this, SignalType.busy), connection.connectedATCs[atc]);
                }
                else
                {
                    atc.handleSignalFromATC(new Signal((Transmitter)this, SignalType.call, user.id), connection.connectedATCs[atc]);
                    user.handle(new Signal((Transmitter)this, SignalType.call, callerID));
                    waitingForTheResponceFrom.Add(user);
                    logEvent($"Sended ATC {atc.name} a call signal. Waiting for the responce from User {user.id}.");
                }
            }
        }
        async void handleNumber(Signal signal, Connection connection)
        {

            if (signal.sender is User)
            {
                User sender = signal.sender as User;

                if (connection.status == ConnectionStatus.Started)
                {

                    string number = signal.message;

                    connection.destinationNumber += number;

                    if (connection.destinationNumber.Length == 3)
                    {

                        isGoingToMakeOutgoingCall[sender.id] = false;

                        await Task.Delay(2000);

                        if (!isGoingToMakeOutgoingCall[sender.id])
                        {
                            connection.status = ConnectionStatus.DestinationSelected;

                            User user = connectedUsers.Find(x => x.id == connection.destinationNumber);

                            if (user == null)
                            {
                                sender.handle(new Signal((Transmitter)this, SignalType.offline));
                                logEvent($"User {sender.id} received response. User {connection.destinationNumber} is offline.");
                            }
                            else
                            {
                                if (connections.Exists(c => c.users.Exists(x => x.id == connection.destinationNumber)))
                                {
                                    sender.handle(new Signal((Transmitter)this, SignalType.busy));
                                    logEvent($"User {sender.id} received response. User {connection.destinationNumber} is busy.");
                                }
                                else
                                {
                                    sender.handle(new Signal((Transmitter)this, SignalType.call, user.id));
                                    user.handle(new Signal((Transmitter)this, SignalType.call, sender.id));
                                    waitingForTheResponceFrom.Add(user);
                                    logEvent($"Sended user {sender.id} a call signal. Waiting for the responce from User {user.id}.");
                                }
                            }
                        } 

                        isGoingToMakeOutgoingCall.Remove(sender.id);

                    } else if (connection.destinationNumber.Length == 5)
                    {
                        isGoingToMakeOutgoingCall[sender.id] = true;

                        connectUserToATC(sender, connection);
                    }

                }
            }
        }

        void handleMessage(Signal signal, Connection connection)
        {
            if (signal.sender is User)
            {
                User sender = signal.sender as User;

                string atcID = getATCid(sender.GlobalID);

                bool isSignalLocal = atcID == this.id;

                foreach (ATC connectedATC in connection.connectedATCs.Keys)
                {
                    if (isSignalLocal || connectedATC.id != atcID)
                    {
                        connectedATC.handleSignalFromATC(signal, connection.connectedATCs[connectedATC]);
                        logEvent($"Sended ATC {connectedATC.name} a message signal. Connection ID: {connection.id}.");
                    }
                }

                foreach (User connectedUser in connection.users)
                {
                    if (!isSignalLocal || isSignalLocal && connectedUser.id != signal.sender.id)
                    {
                        connectedUser.handle(signal);
                        logEvent($"Sended User {connectedUser.id} a message signal. Connection ID: {connection.id}.");
                    }
                }
            }
        } 

        void handleCancel(Signal signal, Connection connection)
        {
            string atcID = getATCid(signal.sender.id);

            bool isSignalLocal = atcID == this.id;

            if (isSignalLocal)
            {
                connection.users.Remove(signal.sender as User);
                logEvent($"Removed User {signal.sender.id} from connection {connection.id}.");
            } else
            {
                ATC atcToRemove = connection.connectedATCs.Keys.ToList().Find(a => a.id == atcID);
                connection.connectedATCs.Remove(atcToRemove);
                logEvent($"Removed ATC {atcToRemove.name} from connection {connection.id}.");
            }

            Signal cancelSignal = new Signal((Transmitter)this, SignalType.cancel, signal.sender.id);

            foreach (User user in connection.users)
            {
                user.handle(cancelSignal);
                logEvent($"Sended User {user.id} a cancel signal. Connection ID: {connection.id}.");
            }
            foreach (ATC connectedATC in connection.connectedATCs.Keys)
            {
                connectedATC.handleSignalFromATC(signal, connection.connectedATCs[connectedATC]);
                logEvent($"Sended ATC {connectedATC.name} a cancel signal. Connection ID: {connection.id}.");
            }

            if (connection.users.Count + connection.connectedATCs.Keys.Count == 1)
            {
                Signal connectionClosedSignal = new Signal((Transmitter)this, SignalType.tone, "Connection closed.");

                if (connection.users.Count == 1)
                {
                    User lastUser = connection.users.First();
                    lastUser.handle(connectionClosedSignal);
                    logEvent($"Sended User {lastUser.id} a tone signal. Connection ID: {connection.id}. Connection Closed.");
                } else
                {
                    ATC lastATC = connection.connectedATCs.Keys.First();
                    lastATC.handleSignalFromATC(connectionClosedSignal, connection.connectedATCs[lastATC]);
                    logEvent($"Sended ATC {lastATC.name} a tone signal. Connection ID: {connection.id}. Connection Closed.");
                }

                connections.Remove(connection);
            }
        }
    }
}
