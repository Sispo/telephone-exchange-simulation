using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace ATC
{
    class ATCNameService
    {
        public static bool Save(string atcName, string id)
        {
            Dictionary<string, string> ATCNameServiceDataBase = ATCNameService.Load();

            if (ATCNameServiceDataBase == null)
            {
                ATCNameServiceDataBase = new Dictionary<string, string>();
            }

            ATCNameServiceDataBase[id] = atcName;

            BinaryFormatter bf = new BinaryFormatter();

            FileStream fsout = new FileStream($"ANS.dat", FileMode.Create, FileAccess.Write, FileShare.None);

            try
            {
                using (fsout)
                {
                    bf.Serialize(fsout, ATCNameServiceDataBase);
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

        public static string GetName(string id)
        {
            Dictionary<string, string> ATCNameServiceDataBase = Load();

            if (ATCNameServiceDataBase == null)
            {
                return null;
            }

            if (ATCNameServiceDataBase.ContainsKey(id))
            {
                return ATCNameServiceDataBase[id];
            }

            return null;
        }
        public static Dictionary<string, string> Load()
        {
            BinaryFormatter bf = new BinaryFormatter();
            string fileName = $"ANS.dat";
            if (File.Exists(fileName))
            {
                FileStream fsin = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.None);
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
            else
            {
                return null;
            }

        }

    }
}
