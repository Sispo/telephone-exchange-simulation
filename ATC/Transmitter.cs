using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATC
{
    public class Transmitter: IEquatable<Transmitter>
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

        public bool Equals(Transmitter other)
        {
            return this.id == other.id;
        }

        public override int GetHashCode()
        {
            return id.GetHashCode();
        }
    }
}
