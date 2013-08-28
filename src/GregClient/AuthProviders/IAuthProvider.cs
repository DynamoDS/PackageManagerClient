using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Text;
using Greg.Requests;
using RestSharp;

namespace Greg
{
    public interface IAuthProvider
    {
        /// <summary>
        /// Get authentication data for a request
        /// </summary>
        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        void SignRequest(ref RestRequest m, RestClient client);
    }
}
