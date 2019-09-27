using MyCompany.NetCore.Enumerators.Enum;
using MyCompany.NetCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;

namespace MyCompany.NetCore.Operation
{
    public class RequestParameters : IRequest
    {
        public Uri Url { get; private set; }
        public string Resource { get; private set; }
        public string DesiredMethod { get; private set; }
        public HTTPMethod Method { get; private set; }
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
        public void AddURIParameters(string parameters)
        {
            Url = new Uri(Url + parameters);
        }
        public void SetAPIRequestMethod(string desiredmethod)
        {
            DesiredMethod = desiredmethod.ToUpper();
            #region methodoptions
            switch (DesiredMethod)
            {
                case "GET":
                    Method = HTTPMethod.GET;
                    break;
                case "POST":
                    Method = HTTPMethod.POST;
                    break;
                case "DELETE":
                    Method = HTTPMethod.DELETE;
                    break;
                case "PUT":
                    Method = HTTPMethod.PUT;
                    break;
                case "PATCH":
                    Method = HTTPMethod.PATCH;
                    break;
                case "HEAD":
                    Method = HTTPMethod.HEAD;
                    break;
                case "OPTIONS":
                    Method = HTTPMethod.OPTIONS;
                    break;
                default:
                    Method = HTTPMethod.POST;
                    break;
            }
            #endregion
        }
        public void SetAuthentication(bool isauthentication)
        {
            Authentication = isauthentication ? true : false;
        }
        public void SetUserName(string username)
        {
            UserName = username;
        }
        public void SetPassword(string password)
        {
            Password = password;
        }
        public void SetAuthenticationProtocol(string desiredprotocol)
        {
            switch (desiredprotocol.ToUpper())
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
        public void SetCache(string cachetime)
        {
            CacheTime = (cachetime.Length > 0) ? cachetime : "0";
        }

        public void SetContentType(string contenttype)
        {
            ContentType = contenttype ?? "application/json;charset=utf-8";
        }

        public void SetProxyServer(WebProxy proxyserver)
        {            
            ProxyServer = proxyserver;
        }

        public void SetHeaders(string headerkey, string headervalue)
        {
            HeaderKeyValues.Add(headerkey, headervalue);
        }

        public void SetCustomHeader(string customheader)
        {
            CustomHeader = customheader ?? "";
        }

        public void SetRequestData(string requestpayload)
        {
            RequestData = requestpayload ?? "";
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