﻿using RestSharp;

namespace Greg.Requests
{
    /// <summary>
    /// Request for getting latest versions of all packages published by the current user.
    /// </summary>
    public class GetUserPackages : PackageReferenceRequest
    {
        private readonly Method httpMethod = Method.GET;

        /// <summary>
        /// GET request for fetching latest versions of all packages published by the current user.
        /// Authentication is required to identify user.
        /// </summary>
        public GetUserPackages()
        {
            this.ForceAuthentication = true;

            httpMethod = Method.GET;
        }

        /// <summary>
        /// Path used to call remote package manager API endpoint.
        /// </summary>
        public override string Path
        {
            get { return "user/latest_packages"; }
        }

        /// <summary>
        /// Http method used to make remote package manager API call.
        /// </summary>
        public override Method HttpMethod
        {
            get { return httpMethod; }
        }

        /// <summary>
        /// This function can be used to build request for POST/PUT request types which requires some additional headers to be added to the request.
        /// </summary>
        internal override void Build(ref RestRequest request)
        {
        }
    }
}
