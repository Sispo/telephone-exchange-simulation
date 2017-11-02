using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATC
{

    public delegate void ActionDelegate();
    public class ATCLoginResult
    {
        public string error;
        public bool isSuccessfull;
        public ATC atc;

        public ATCLoginResult(bool isSuccessfull, string error, ATC atc)
        {
            this.error = error;
            this.isSuccessfull = isSuccessfull;
            this.atc = atc;
        }
    }
    class ATCService
    {

        public event ActionDelegate stateUpdated;

        public static ATCService shared = new ATCService();

        public List<ATC> onlineATCs = new List<ATC>();

        public ATCLoginResult Enable(string name, string id)
        {
            if (id != null)
            {
                if (onlineATCs.Exists(atc => atc.id == id))
                {
                    return new ATCLoginResult(false, "This ATC is already online.", null);
                }
                else
                {
                    return new ATCLoginResult(true, null, new ATC(id, name));
                }
            }
            else
            {
                string newATCid = getNewATCid();
                if (Convert.ToInt32(newATCid) > 99)
                {
                    return new ATCLoginResult(false, "Maximum number of ATCs is already in use.", null);
                }

                return new ATCLoginResult(true, null, new ATC(newATCid, name));
            }
        }

        public void connect(ATC atc)
        {
            foreach(ATC a in onlineATCs)
            {
                if (atc.id != a.id)
                {
                    a.connect(atc);
                    atc.connect(a);
                }
            }
            onlineATCs.Add(atc);
            atc.connectionsChanged += stateUpdated;
            stateUpdated();
        }

        public void disconnect(ATC atc)
        {
            onlineATCs.Remove(atc);
            stateUpdated();
        }

        string getNewATCid()
        {
            string id = "00";
            while (DatabaseService.ATCExists(id))
            {
                id = IdUtility.IncrementStringID(id);
            }
            return id;
        }
    }
}
