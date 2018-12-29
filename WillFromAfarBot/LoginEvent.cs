using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WillFromAfarBot
{
    public class LoginEvent : EventArgs
    {
        private readonly LoginModel LoginInfo;

        public LoginEvent(LoginModel info)
        {
            LoginInfo = info;
        }

        public LoginModel GetLoginInfo()
        {
            return LoginInfo;
        }
    }
}
