using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATC
{
    public enum ConnectionStatus { Started, DestinationSelected, Connected, TryingToAddAnotherUser }
    public class Connection
    {
        public string id;
        public ConnectionStatus status;
        public string destinationNumber;
        public List<User> users = new List<User>();
        public Dictionary<ATC, string> connectedATCs = new Dictionary<ATC, string>();

        public Connection(string id)
        {
            status = ConnectionStatus.Started;
            this.id = id;
        }
        public Connection(string id, User user)
        {
            users.Add(user);
            status = ConnectionStatus.Started;
            this.id = id;
        }

        public void ConnectATC(ATC atc, string connectionID)
        {
            connectedATCs[atc] = connectionID;
        }
    }
}
