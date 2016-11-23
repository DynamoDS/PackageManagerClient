using PackageManagerClient.AuthProviders;
using PackageManagerClient.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageManagerClient
{
    public interface IPackageManagerClient
    {
        IAuthProvider AuthProvider { get; }
        string BaseUrl { get; }

        CefResponse Execute(CefRequest m);
        CefResponseBody ExecuteAndDeserialize(CefRequest m);
        CefResponseWithContentBody ExecuteAndDeserializeWithContent<dynamic>(CefRequest m);
    }
}
