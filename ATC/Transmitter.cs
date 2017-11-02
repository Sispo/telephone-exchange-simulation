using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATC
{
    public class Transmitter
    {
        private string _id;
        public string id
        {
            get
            {
                return _id;
            }
        }

        public Transmitter(string id)
        {
            _id = id;
        }
    }
}
