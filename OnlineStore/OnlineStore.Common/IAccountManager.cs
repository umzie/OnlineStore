using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OnlineStore.Contracts;

namespace OnlineStore.Common
{
    public interface IAccountManager
    {
        string ValidateUser(string username, string password);
        UserCredentials Register(UserCredentials register);
        UserCredentials GetUserInfo(string username);
    }
}
