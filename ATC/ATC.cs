using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATC
{
    public delegate void ATCConnectionDelegate(Signal signal, ATC atc);
    public delegate void UserConnectionDelegate(Signal signal);
    public class ATC: Transmitter
    {
        public ATC(string id, string name) : base(id) {
            ATCNameService.Save(name, id);
        }

        private List<ATC> connectedATCs = new List<ATC>();
        private List<User> connectedUsers = new List<User>();
        private List<Connection> connections = new List<Connection>();
        private List<User> waitingForTheResponceFrom = new List<User>();
        private Dictionary<string, bool> isGoingToMakeOutgoingCall = new Dictionary<string, bool>();

        public void connect(User user)
        {
            user.sendSignal += new UserConnectionDelegate(handle);
            connectedUsers.Add(user);
        }

        public void connect(ATC atc)
        {
            connectedATCs.Add(atc);
            atc.connect(this);
        }

        void connectUserToATC(User user, Connection connection)
        {

            ATC atc = connectedATCs.Find(x => x.id == connection.destinationNumber[0].ToString());

            if (atc == null)
            {
                foreach(ATC anotherATC in connectedATCs)
                {
                    anotherATC.connectUserToATC(user, connection);
                }
            } else
            {
                string connectionID = atc.handleIncomingConnection(this, getUserId(connection.destinationNumber), connection.id, user.GlobalID);
                connection.ConnectATC(atc, connectionID);

            }

        }

        public string handleIncomingConnection(ATC atc, string destinationNumber, string connectionID, string callerID)
        {
            Connection connection = new Connection(getConnectionID());
            connection.ConnectATC(atc, connectionID);
            connection.destinationNumber = destinationNumber;
            connectUserWithIncommingCaller(connection, atc, callerID);
            return connection.id;
        }

        string getConnectionID()
        {
            string id = "000";
            while(connections.Exists(c => c.id == id))
            {
                int intID = Convert.ToInt32(id);
                intID++;
                id = intID.ToString();
            }
            return id;
        }

        string incrementStringID(string id)
        {
            int length = id.Length;
            string newId = (Convert.ToInt32(id) + 1).ToString();
            while(newId.Length != length)
            {
                newId.Insert(0, "0");
            }
            return newId;
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
                            }

                        }
                        else
                        {
                            Connection newConnection = new Connection(getConnectionID(), sender);
                            connections.Add(newConnection);
                            sender.handle(new Signal((Transmitter)this, SignalType.tone, "Please enter destination number..."));
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
                atc.handleSignalFromATC(new Signal((Transmitter)this, SignalType.offline), connection.connectedATCs[atc]);
            }
            else
            {
                if (connections.Exists(c => c.users.Exists(x => x.id == connection.destinationNumber)))
                {
                    atc.handleSignalFromATC(new Signal((Transmitter)this, SignalType.busy), connection.connectedATCs[atc]);
                }
                else
                {
                    atc.handleSignalFromATC(new Signal((Transmitter)this, SignalType.call, user.id), connection.connectedATCs[atc]);
                    user.handle(new Signal((Transmitter)this, SignalType.call, callerID));
                    waitingForTheResponceFrom.Add(user);
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
                            }
                            else
                            {
                                if (connections.Exists(c => c.users.Exists(x => x.id == connection.destinationNumber)))
                                {
                                    sender.handle(new Signal((Transmitter)this, SignalType.busy));
                                }
                                else
                                {
                                    sender.handle(new Signal((Transmitter)this, SignalType.call, user.id));
                                    user.handle(new Signal((Transmitter)this, SignalType.call, sender.id));
                                    waitingForTheResponceFrom.Add(user);
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
            string atcID = getATCid(signal.sender.id);

            bool isSignalLocal = atcID == this.id;

            foreach(ATC connectedATC in connection.connectedATCs.Keys)
            {
                if (isSignalLocal || connectedATC.id != atcID)
                {
                    connectedATC.handleSignalFromATC(signal, connection.connectedATCs[connectedATC]);
                }
            }

            foreach(User connectedUser in connection.users)
            {
                if (isSignalLocal && connectedUser.id != signal.sender.id)
                {
                    connectedUser.handle(signal);
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
            } else
            {
                ATC atcToRemove = connection.connectedATCs.Keys.ToList().Find(a => a.id == atcID);
                connection.connectedATCs.Remove(atcToRemove);
            }

            Signal cancelSignal = new Signal((Transmitter)this, SignalType.cancel, signal.sender.id);

            foreach (User user in connection.users)
            {
                user.handle(cancelSignal);
            }
            foreach (ATC connectedATC in connection.connectedATCs.Keys)
            {
                connectedATC.handleSignalFromATC(signal, connection.connectedATCs[connectedATC]);
            }

            if (connection.users.Count + connection.connectedATCs.Keys.Count == 1)
            {
                Signal connectionClosedSignal = new Signal((Transmitter)this, SignalType.tone, "Connection closed.");

                if (connection.users.Count == 1)
                {

                } else
                {
                    ATC lastATC = connection.connectedATCs.Keys.First();
                    lastATC.handleSignalFromATC(connectionClosedSignal, connection.connectedATCs[lastATC]);
                }

                connections.Remove(connection);
            }
        }
    }
}
