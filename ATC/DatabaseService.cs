using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace ATC
{
    class DatabaseService
    {
        public static bool Save(string atcName, string id, string passwordHash)
        {
            Dictionary<string, string> db = DatabaseService.Load(atcName);

            if (db == null)
            {
                db = new Dictionary<string, string>;
            }

            db[id] = passwordHash;

            BinaryFormatter bf = new BinaryFormatter();

            FileStream fsout = new FileStream($"{atcName}.dat", FileMode.Create, FileAccess.Write, FileShare.None);

            try
            {
                using (fsout)
                {
                    bf.Serialize(fsout, db);
                    return true;
                }
            }
            catch (Exception err)
            {
                Console.WriteLine("Error occured:" + err.Message);
                return false;
            }
            finally
            {
                if (fsout != null)
                {
                    fsout.Close();
                }
            }
        }
        public static Dictionary<string, string> Load(string atcName)
        {
            BinaryFormatter bf = new BinaryFormatter();

            FileStream fsin = new FileStream($"{atcName}.dat", FileMode.Open, FileAccess.Read, FileShare.None);
            try
            {
                using (fsin)
                {
                    return (Dictionary<string, string>)bf.Deserialize(fsin);
                }
            }
            catch (Exception err)
            {
                Console.WriteLine("Error occured:" + err.Message);
                return null;
            }
            finally
            {
                if (fsin != null)
                {
                    fsin.Close();
                }
            }
        }
    }
}
