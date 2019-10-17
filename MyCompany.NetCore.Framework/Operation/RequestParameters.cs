using System;
using System.Collections.Generic;
using System.Net;
using MyCompany.NetCore.Framework.Enumerators;
using MyCompany.NetCore.Framework.Interfaces;

namespace MyCompany.NetCore.Framework.Operation
{
    public class RequestParameters : IRequest
    {
        public Uri Url { get; private set; }
        public string Resource { get; private set; }
        public string DesiredMethod { get; private set; }
        public HttpMethod Method { get; private set; }
        public string ContentType { get; private set; }
        public WebProxy ProxyServer { get; private set; }
        public string Referer { get; private set; }
        public AuthProtocol Protocol { get; private set; }
        public string RequestData { get; private set; }
        public bool Authentication { get; private set; }
        public string UserName { get; private set; }
        public string Password { get; private set; }
        public string CacheTime { get; private set; }
        public string CustomHeader { get; private set; }
        public Dictionary<string, string> HeaderKeyValues { get; private set; }
            = new Dictionary<string, string>();
        public void SetUri(string endpoint)
        {
            Url = new Uri(endpoint);
        }
        public void AddUriParameters(string parameters)
        {
            Url = new Uri(Url + parameters);
        }
        public void SetApiRequestMethod(string desiredMethod)
        {
            DesiredMethod = desiredMethod.ToUpper();
            #region methodoptions
            switch (DesiredMethod)
            {
                case "GET":
                    Method = HttpMethod.GET;
                    break;
                case "POST":
                    Method = HttpMethod.POST;
                    break;
                case "DELETE":
                    Method = HttpMethod.DELETE;
                    break;
                case "PUT":
                    Method = HttpMethod.PUT;
                    break;
                case "PATCH":
                    Method = HttpMethod.PATCH;
                    break;
                case "HEAD":
                    Method = HttpMethod.HEAD;
                    break;
                case "OPTIONS":
                    Method = HttpMethod.OPTIONS;
                    break;
                default:
                    Method = HttpMethod.POST;
                    break;
            }
            #endregion
        }
        public void SetAuthentication(bool isAuthentication)
        {
            Authentication = isAuthentication ? true : false;
        }
        public void SetUserName(string username)
        {
            UserName = username;
        }
        public void SetPassword(string password)
        {
            Password = password;
        }
        public void SetAuthenticationProtocol(string desiredProtocol)
        {
            switch (desiredProtocol.ToUpper())
            {
                case "BASIC":
                    Protocol = AuthProtocol.BASIC;
                    break;
                case "NTLM":
                    Protocol = AuthProtocol.NTLM;
                    break;
                default:
                    Protocol = AuthProtocol.BASIC;
                    break;
            }
        }
        public void SetCache(string cacheTime)
        {
            CacheTime = (cacheTime.Length > 0) ? cacheTime : "0";
        }

        public void SetContentType(string contentType)
        {
            ContentType = contentType ?? "application/json;charset=utf-8";
        }

        public void SetProxyServer(WebProxy proxyServer)
        {            
            ProxyServer = proxyServer;
        }

        public void SetHeaders(string headerKey, string headerValue)
        {
            HeaderKeyValues.Add(headerKey, headerValue);
        }

        public void SetCustomHeader(string desireCustomHeader)
        {
            CustomHeader = desireCustomHeader ?? "";
        }

        public void SetRequestData(string requestPayload)
        {
            RequestData = requestPayload ?? "";
        }

        public void SetReferer(string referer)
        {
            Referer = referer ?? "";
        }
        public void SetResource(string resource)
        {
            Resource = resource.TrimStart('/').TrimStart('\\');
        }
    }
}