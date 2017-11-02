using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATC
{

    class ATCLoginResult
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
        public static ATCService shared = new ATCService();

        public List<ATC> onlineATCs = new List<ATC>();

        //public ATCLoginResult enable(string name, string id, bool isTryingToEnableNew)
        //{
        //    if (id != null)
        //    {
        //        if (onlineATCs.Exists(atc => atc.id == id))
        //        {
        //            return new ATCLoginResult(false, "This ATC is already online.", null);
        //        } else
        //        {
        //            return new ATCLoginResult(true, null, new ATC(id, name));
        //        }
        //    } else
        //    {
        //        if (isTryingToEnableNew)
        //        {
                    
        //        } else
        //        {

        //        }
        //    }
        //}

        //string getNewATCid()
        //{
        //    string id = "00";
        //    while (DatabaseService.ATCExists(id))
        //    {
        //        int intID = Convert.ToInt32(id);
        //        intID++;
        //        id = intID.ToString();
        //    }
        //    while (DatabaseService)
        //}
    }
}
