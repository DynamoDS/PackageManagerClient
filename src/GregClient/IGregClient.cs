using RestSharp;
using Greg.Requests;
using Greg.Responses;

namespace Greg
{
    public interface IGregClient
    {
        IAuthProvider AuthProvider { get; }
        string BaseUrl { get; }

        RestRequest BuildRestRequest(Request m);

        Response Execute(Request m);
        ResponseBody ExecuteAndDeserialize(Request m);
        ResponseWithContentBody<T> ExecuteAndDeserializeWithContent<T>(Request m);
    }
}
