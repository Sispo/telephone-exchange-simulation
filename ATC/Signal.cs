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
        private Transmitter _sender;

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

        public Transmitter sender
        {
            get
            {
                return _sender;
            }
        }

        public Signal(Transmitter sender, SignalType type)
        {
            _type = type;
            _sender = sender;
            _message = null;
        }

        public Signal(Transmitter sender, SignalType type, string message)
        {
            _type = type;
            _message = message;
            _sender = sender;
        }
    }
}
