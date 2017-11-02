using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATC
{
    public class IdUtility
    {
        public static string IncrementStringID(string id)
        {
            int length = id.Length;
            string newId = (Convert.ToInt32(id) + 1).ToString();
            while (newId.Length != length)
            {
                newId = "0" + newId;
            }
            return newId;
        }
    }
}
