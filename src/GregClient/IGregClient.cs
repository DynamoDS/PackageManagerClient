using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Greg.Requests;
using Greg.Responses;

namespace Greg
{
    public interface IGregClient
    {
        IAuthProvider AuthProvider { get; }
        string BaseUrl { get; }

        Response Execute(Request m);
        ResponseBody ExecuteAndDeserialize(Request m);
        ResponseWithContentBody<T> ExecuteAndDeserializeWithContent<T>(Request m);
    }
}
