using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RestSharp;

namespace Greg.AuthProviders
{
    public class BasicProvider : IAuthProvider
    {
        void IAuthProvider.SignRequest(ref RestRequest m, RestClient client)
        {
            client.Authenticator = new SimpleAuthenticator("username", "test", "password", "e0jlZfJfKS");
        }
    }
}
