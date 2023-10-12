﻿using RestSharp;
using System.Net.Http;

namespace Greg.Requests
{
    /// <summary>
    /// Request for getting all votes by the current user.
    /// </summary>
    public class GetUserVotes : PackageReferenceRequest
    {
        /// <summary>
        /// GET request for fetching all votes by the current user.
        /// Authentication is required to identify user.
        /// </summary>
        public GetUserVotes()
        {
            this.ForceAuthentication = true;
        }
        /// <summary>
        /// Path used to call remote package manager API endpoint.
        /// </summary>
        public override string Path
        {
            get { return "user/votes"; }
        }

        /// <summary>
        /// Http method used to make remote package manager API call.
        /// </summary>
        public override HttpMethod HttpMethod
        {
            get { return HttpMethod.Get; }
        }

        /// <summary>
        /// This function can be used to build request for POST/PUT request types which requires some additional headers to be added to the request.
        /// </summary>
        internal override void Build(ref RestRequest request)
        {
        }
    }
}
