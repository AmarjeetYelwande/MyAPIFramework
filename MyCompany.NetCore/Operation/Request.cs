using MyCompany.NetCore.Enumerators.Enum;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Text;

namespace MyCompany.NetCore.Operation
{
    public class Request
    {
        public HttpWebResponse Response { get; private set; }
        public double ResponseTime { get; private set; }
        public (HttpWebResponse, double) SetAPIRequest(RequestParameters _requestparameters)
        {
            var webRequest = WebRequest.CreateHttp(_requestparameters.Url);
            webRequest.Method = _requestparameters.Method.ToString();
            webRequest.Host = _requestparameters.Url.Host;

            if (_requestparameters.HeaderKeyValues != null)
            {
                foreach (KeyValuePair<string, string> credentialpair in _requestparameters.HeaderKeyValues)
                {
                    webRequest.Headers[credentialpair.Key] = credentialpair.Value;
                }
            }

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            if (_requestparameters.Authentication)
            {
                var networkCredential = new NetworkCredential(_requestparameters.UserName, _requestparameters.Password);
                var myCredentialCache = new CredentialCache { { _requestparameters.Url, _requestparameters.Protocol.ToString(), networkCredential } };

                ServicePointManager.ServerCertificateValidationCallback +=
                    (sender, certificate, chain, sslPolicyErrors) => true;

                webRequest.PreAuthenticate = true;
                webRequest.Credentials = myCredentialCache;                

            }

            ServicePointManager.ServerCertificateValidationCallback +=
                (sender, certificate, chain, sslPolicyErrors) => true;
            ServicePointManager.ServerCertificateValidationCallback = (s, cert, chain, ssl) => true;

            if (_requestparameters.Method == HTTPMethod.POST 
               || _requestparameters.Method == HTTPMethod.PUT 
               || _requestparameters.Method == HTTPMethod.PATCH )
            {
                if (_requestparameters.RequestData != null)
                {
                    var data = Encoding.UTF8.GetBytes(_requestparameters.RequestData);
                    webRequest.ContentLength = data.Length;

                    using (Stream stream = webRequest.GetRequestStream())
                        {
                            GenerateStreamFromString(_requestparameters.RequestData).CopyTo(stream);
                        }
                }
                else { webRequest.ContentLength = 0; }
            }            

            WebProxy DefaultProxy = new WebProxy {UseDefaultCredentials = true};
            webRequest.Proxy = _requestparameters.ProxyServer ?? DefaultProxy;
            webRequest.Referer = _requestparameters.Referer ?? "";
            webRequest.ContentType = _requestparameters.ContentType ?? "application/json;charset=utf-8";

            HttpRequestCachePolicy requestPolicy;
            if (_requestparameters.CacheTime != "0")
            {
                int numberOfHours = Convert.ToInt16(_requestparameters.CacheTime);
                requestPolicy = new HttpRequestCachePolicy(HttpCacheAgeControl.MaxAge, TimeSpan.FromHours(numberOfHours));
            }
            else
            {
                requestPolicy = new HttpRequestCachePolicy(HttpRequestCacheLevel.NoCacheNoStore);
            }

            webRequest.CachePolicy = requestPolicy;
            webRequest.Timeout = 15000;
            var requeststopwatch = Stopwatch.StartNew();

            try
            {
                Response = (HttpWebResponse)webRequest.GetResponse();
                requeststopwatch.Stop();
                ResponseTime = requeststopwatch.Elapsed.TotalMilliseconds;
                Console.WriteLine($"Time taken by request to execute is : {ResponseTime} ms ");
            }

            catch (WebException requestFailure)
            {
                Console.WriteLine("Request failed. Printing parameters used to send request for debugging purpose");
                Console.WriteLine($"Request type : {webRequest.Method}");
                Console.WriteLine($"Header Values : {webRequest.Headers}");               
                Console.WriteLine($"Request URI : {webRequest.RequestUri}");
                Console.WriteLine($"Content length : {webRequest.ContentLength}");
                Console.WriteLine($"Failure message from response is : {requestFailure.Message}");
                requeststopwatch.Stop();
                ResponseTime = requeststopwatch.Elapsed.TotalMilliseconds;
                Console.WriteLine($"Time taken by request to execute is : {ResponseTime} ms ");
                Response = (HttpWebResponse)requestFailure.Response;
                Console.WriteLine($"Api Request failed due to error : {requestFailure.Message}");
            }
            return (Response, ResponseTime);
        }
        private static Stream GenerateStreamFromString(string inputstring)
        {
            MemoryStream streamM = new MemoryStream();
            StreamWriter writer = new StreamWriter(streamM);
            writer.Write(inputstring);
            writer.Flush();
            streamM.Position = 0;
            return streamM;
        }
    }
}
