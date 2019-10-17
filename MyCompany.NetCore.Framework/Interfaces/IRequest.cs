using System.Net;

namespace MyCompany.NetCore.Framework.Interfaces
{
    public interface IRequest
    {
        void SetUri(string endpoint);
        void AddUriParameters(string parameters);        
        void SetResource(string resource);
        void SetRequestData(string requestPayload);
        void SetApiRequestMethod(string desiredMethod);
        void SetCache(string cacheTime);
        void SetAuthentication(bool isAuthentication);
        void SetUserName(string username);
        void SetPassword(string password);
        void SetContentType(string contentType);
        void SetProxyServer(WebProxy proxyServer);
        void SetAuthenticationProtocol(string desiredProtocol);
        void SetHeaders(string headerKey, string headerValue);
        void SetCustomHeader(string desireCustomHeader);
    }
}