using System.Net;

namespace MyCompany.NetCore.Interfaces
{
    public interface IRequest
    {
        void SetUri(string endpoint);
        void AddURIParameters(string parameters);        
        void SetResource(string resource);
        void SetRequestData(string requestpayload);
        void SetAPIRequestMethod(string desiredmethod);
        void SetCache(string cachetime);
        void SetAuthentication(bool isauthentication);
        void SetUserName(string username);
        void SetPassword(string password);
        void SetContentType(string contenttype);
        void SetProxyServer(WebProxy proxyserver);
        void SetAuthenticationProtocol(string desiredprotocol);
        void SetHeaders(string headerkey, string headervalue);
        void SetCustomHeader(string desirecustomedheader);
    }
}