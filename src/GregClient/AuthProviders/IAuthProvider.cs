using System;
using Greg.AuthProviders;
using RestSharp;

namespace Greg
{
    public interface IAuthProvider
    {
        event Func<object, bool> RequestLogin;
        event Action<LoginState> LoginStateChanged;

        LoginState LoginState { get; }
        string Username { get; }

        void SignRequest(ref RestRequest m, RestClient client);

        void Logout();
        bool Login();
    }
}