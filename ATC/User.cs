using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATC
{
    public delegate void LogDelegate(string message);
    public class User: Transmitter
    {
        private ATC currentATC;

        public event LogDelegate log;
        public event UserConnectionDelegate sendSignal;

        bool isTryingToConnect = false;
        bool isReceivingCall = false;

        public User(string id, ATC atc) : base(id)
        {
            currentATC = atc;
            sendSignal += logOutgoingSignal;
        }

        public string GlobalID
        {
            get
            {
                return currentATC.id + id;
            }
        }

        public void send(SignalType type, string message)
        {
            sendSignal(new Signal((Transmitter)this, type, message));
        }

        public void handle(Signal signal)
        {
            if (signal.sender is ATC)
            {
                switch (signal.type)
                {
                    case SignalType.call:

                        if (isTryingToConnect)
                        {
                            isTryingToConnect = false;
                            log($"{signal.sender.id}: Waiting for the responce from {signal.message}");
                        }
                        else
                        {
                            isReceivingCall = true;
                            log($"{signal.sender.id}: {signal.message} is calling you...");
                        }

                        break;
                    case SignalType.tone:
                        log($"{signal.sender.id}: {signal.message}");
                        break;
                    case SignalType.busy:
                        log($"{signal.sender.id}: User is busy. Try again later.");
                        break;
                    case SignalType.offline:
                        log($"{signal.sender.id}: User is offline. Try again later.");
                        break;
                    default:
                        break;
                }
            }
            else if (signal.sender is User)
            {
                User sender = signal.sender as User;

                if (signal.type == SignalType.message)
                {
                    log($"{sender.id}: {signal.message}");
                }
            }
        }

        void logOutgoingSignal(Signal signal)
        {
            switch (signal.type)
            {
                case SignalType.cancel:
                    log("Canceled call.");
                    isTryingToConnect = false;
                    break;
                case SignalType.message:
                    log($"{id}: {signal.message}");
                    break;
                case SignalType.number:
                    log(signal.message);
                    break;
                case SignalType.phone:
                    if (isReceivingCall)
                    {
                        isReceivingCall = false;
                        log("Accepted call.");
                    } else
                    {
                        log("Calling...");
                        isTryingToConnect = true;
                    }
                    break;
                default:
                    break;
            }
        }

        public void disconnect()
        {
            send(SignalType.cancel, null);
            this.currentATC.disconnect(this);
        }
    }
}
