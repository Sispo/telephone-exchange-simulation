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
        public UserScreen screen;
        public ATC currentATC;

        public event LogDelegate logEvent;
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
            if (screen != null && !screen.IsDisposed && currentATC != null)
            {
                sendSignal(new Signal((Transmitter)this, type, message));
            } else
            {
                disconnect();
            }
        }

        public void log(string message)
        {
            if (screen != null && !screen.IsDisposed && logEvent != null)
            {
                logEvent(message);
            } else
            {
                disconnect();
            }
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
                        isTryingToConnect = false;
                        log($"{signal.sender.id}: User is busy. Try again later.");
                        break;
                    case SignalType.offline:
                        isTryingToConnect = false;
                        log($"{signal.sender.id}: User is offline. Try again later.");
                        break;
                    case SignalType.cancel:
                        isTryingToConnect = false;
                        log($"{signal.sender.id}: {signal.message} disconnected.");
                        break;
                }
            }
            else if (signal.sender is User)
            {
                User sender = signal.sender as User;

                if (signal.type == SignalType.message)
                {
                    log($"{sender.GlobalID}: {signal.message}");
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
            sendSignal -= logOutgoingSignal;
            logEvent = null;
            if (sendSignal != null)
            {
                sendSignal(new Signal(this, SignalType.cancel));
            }
            if (currentATC != null)
            {
                this.currentATC.disconnect(this);
            }
        }
    }
}
