using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal class User
    {
        public User(string log, string pass) 
        {
            login = log;
            password = pass;
        }
        string login;
        public string Login
        {
            get { return login; }
            set { login = value; }
        }
        string password;
        public string Password
        {
            get { return password; }
            set { password = value; }
        }
        public List<Note> notes;
    }
}
