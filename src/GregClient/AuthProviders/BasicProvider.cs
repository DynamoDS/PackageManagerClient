using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RestSharp;

namespace Greg.AuthProviders
{
    public class BasicProvider : IAuthProvider
    {
        private readonly string _usernameKey;
        private readonly string _username;
        private readonly string _passwordKey;
        private readonly string _password;

        public BasicProvider(string usernameKey, string username, string passwordKey, string password)
        {
            _usernameKey = usernameKey;
            _username = username;
            _passwordKey = passwordKey;
            _password = password;
        }

        public void SignRequest(ref RestRequest m, RestClient client)
        {
            client.Authenticator = new SimpleAuthenticator(_usernameKey, _username, _passwordKey, _password);
        }

        public void Logout()
        {
        }

        public bool Login()
        {
            return true;
        }

        public LoginState LoginState
        {
            get { return LoginState.LoggedIn; }
        }

        public string Username
        {
            get { return _username; }
        }

        public event Func<object, bool> RequestLogin;
        public event Action<LoginState> LoginStateChanged;
    }
}
