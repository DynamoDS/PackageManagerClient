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

        PackageManagerResponse Execute(PMRequest m);
        PackageManagerResponseBody ExecuteAndDeserialize(PMRequest m);
        PMResponseWithContentBody ExecuteAndDeserializeWithContent<dynamic>(PMRequest m);
    }
}
