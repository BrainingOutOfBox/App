using System.Net.Http;

namespace Method635.App.Forms.RestAccess
{
    public interface IHttpClientService
    {
        HttpResponseMessage GetCall(string endpoint);
        HttpResponseMessage PutCall(object parameter, string endpoint);
        HttpResponseMessage PostCall(object parameter, string endpoint);    
    }
}