using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCP_Server
{
    class User
    {
        string alias;
        string password;
        string nickname;

        public User(string a, string p, string n)
        {
            alias = a;
            password = p;
            nickname = n;
        }


        public string getNickname()
        {
            return nickname;
        }

        public string getName()
        {
            return alias;
        }

        public string getPassword()
        {
            return password;
        }

        public void changePassword(string oldPassword, string newPassword)
        {
            if (oldPassword == password)
            {
                password = newPassword;
            }
        }
    }
}
