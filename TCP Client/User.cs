using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCP_Client
{
    class User
    {
        int age; 
        string name;
        string password;
        string nickname;

        public User()
        {
        }

        public void InitializeUser( int a, string n, string p, string nick)
        {
            age = a;
            name = n;
            password = p;
            nickname = nick;
        }


        public string getNickName()
        {
            return nickname;
        }
    }
}
