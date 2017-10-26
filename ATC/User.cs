using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATC
{
    public delegate void LogDelegate(string message);
    public class User
    {
        private string _id;
        private ATC currentATC;

        public event LogDelegate log;
        
        public string id
        {
            get
            {
                return _id;
            }
        }

        public User(string id, ATC atc)
        {
            _id = id;
            currentATC = atc;
        }

        public void receive(Signal signal)
        {

        }

        public void send(Signal signal)
        {

        }
    }
}
