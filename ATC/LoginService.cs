using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ATC
{
    class LoginResult
    {
        public string error;
        public bool isSuccessfull;
        public User user;

        public LoginResult(bool isSuccessfull, string error, User user)
        {
            this.error = error;
            this.isSuccessfull = isSuccessfull;
            this.user = user;
        }
    }
    class LoginService
    {
        
        public static LoginResult login(string id, string password, ATC atc)
        {
            try
            {

                int idINT = Convert.ToInt32(id);

                if (idINT > 999 || idINT < 0)
                {
                    return new LoginResult(false, "Unacceptable id.", null);
                }

                Dictionary<string, string> db = DatabaseService.Load(atc.id);

                if (db == null)
                {
                    return LoginService.signUp(atc, id, password);
                }

                if (!db.ContainsKey(id) || db[id] == null)
                {
                    return LoginService.signUp(atc, id, password);
                }

                string passwordHash = db[id];

                if (HashService.VerifyMd5Hash(password,passwordHash))
                {
                    User user = connectUser(id, atc);
                    if (user != null)
                    {
                        return new LoginResult(true, null, user);
                    } else
                    {
                        return new LoginResult(false, "User is already connected.", null);
                    }
                    
                } else
                {
                    return new LoginResult(false, "Incorrect password.", null);
                }


            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                return new LoginResult(false, "Error occured while processing your input. Try again.", null);
            }
        }
        static LoginResult signUp(ATC atc, string id, string password)
        {
            if (DatabaseService.Save(atc.id, id, HashService.GetMd5Hash(password)))
            {
                User user = connectUser(id, atc);
                if (user != null)
                {
                    return new LoginResult(true, null, user);
                }
                else
                {
                    return new LoginResult(false, "User is already connected.", null);
                }
            }
            else
            {
                return new LoginResult(false, "Error occured while signing up.", null);
            }
        }

        static User connectUser(string id, ATC atc)
        {
            User user = new User(id, atc);
            if (atc.connect(user))
            {
                return user;
            } else
            {
                return null;
            }
            
        }

    }
}
