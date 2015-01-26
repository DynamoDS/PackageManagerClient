using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Greg.AuthProviders
{
    public enum LoginState
    {
        LoggedOut, LoggingIn, RequestingUserData, LoggedIn
    }
}
