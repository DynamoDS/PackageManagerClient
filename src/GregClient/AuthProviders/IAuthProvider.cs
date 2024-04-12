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
        //TODO ideally would not force the use of restsharp just to set auth.
        /// <summary>
        /// This method should sign the request and params as OAuth1.
        /// OAuth parameters should be added to the query string.
        /// </summary>
        /// <param name="m"></param>
        /// <param name="client"></param>
        void SignRequest(ref RestRequest m, RestClient client);

        void Logout();
        bool Login();
    }

    /// <summary>
    /// Implement this interface to use OAuth2 for authentication with Dynamo Package Manager.
    /// </summary>
    public interface IOAuth2AuthProvider : IAuthProvider
    {
        //TODO ideally would not force the use of restsharp just to set auth.

        /// <summary>
        /// This method should add the JWT access token to the Authorization header.
        /// Package manager expects a header in the form: Authorization: Bearer [accesstoken]
        /// </summary>
        /// <param name="m"></param>
        /// <param name="client"></param>
        new void SignRequest(ref RestRequest m, RestClient client);
    }

    /// <summary>
    /// Implement this interface to provide direct access to the current bearer token. 
    /// </summary>
    public interface IOAuth2AccessTokenProvider
    {
        string GetAccessToken();
    }

    /// <summary>
    /// Implement this interface to get user information.
    /// </summary>
    public interface IOAuth2UserIDProvider
    {
        /// <summary>
        /// Gets the User ID.
        /// </summary>
        public string UserId { get; }
    }
}