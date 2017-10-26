using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATC
{
    public enum SignalType { phone, tone, number, busy, offline, call, cancel, message }
    public class Signal
    {
        private SignalType _type;
        private string _message;

        public SignalType type
        {
            get
            {
                return _type;
            }
        }

        public string message
        {
            get
            {
                return _message;
            }
        }

        public Signal(SignalType type)
        {
            _type = type;
            _message = null;
        }

        public Signal(SignalType type, string message)
        {
            _type = type;
            _message = message;
        }
    }
}
