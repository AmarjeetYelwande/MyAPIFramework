using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyCompany.NetCore.Helpers.Authentication;
using MyCompany.NetCore.Helpers.Common;
using MyCompany.NetCore.Operation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using TechTalk.SpecFlow;

namespace MyCompany.NetCore.Tests.Common
{
    class CommonMethods
    {
        private RequestParameters RequestParameters { get; set; }
        private static TestContext TestContext { get; set; }

        private readonly string _authHeader = "Authorization";        

        public CommonMethods(ScenarioContext scenarioContext)
        {
            RequestParameters = new RequestParameters();
            TestContext = scenarioContext.ScenarioContainer.Resolve<TestContext>();
        }
    
        internal void SetEndpoint(string application)
        {
            RequestParameters.SetUri(application);
        }

        internal void SetResource(string resource)
        {
            RequestParameters.SetResource(resource);
        }
        internal void SetEndPointParameters(string parameters)
        {
            var allParameters = new Dictionary<string, string>();
            string rawparameters = parameters.Replace(" And ", ",");
            var allparameters = rawparameters.Split(',');

            foreach (var key in allparameters)
            {
                try
                {
                    string value = TestContext.Properties["Parameter:" + key].ToString() ?? "";
                    allParameters.Add(key, value);
                }
                catch { }
            }

            string endpointParameters = Helper.DictionaryToQueryParametersList(allParameters);
            RequestParameters.AddURIParameters(endpointParameters);
        }
        internal void GenerateOAuth2Token(string oAuthendpoint, string oAuthParameters)
        {
            string oAuthParams = oAuthParameters.Replace(" And ", ",");
            var oauthParameters = oAuthParams.Split(',');

            Dictionary<string, string> allParameters = new Dictionary<string, string>();

            foreach (var key in oauthParameters)
            {
                try
                {
                    string value = TestContext.Properties["Parameter:" + key].ToString() ?? "";
                    allParameters.Add(key, value);
                }
                catch { }
            }

            string authorizationServer = TestContext.Properties["Parameter:authserver"].ToString();
            string requestString = EndPoint.GetEndpoint(authorizationServer);
            string parameters = Helper.DictionaryToQueryParametersList(allParameters);
            Console.WriteLine($"Printing endpoint and parameters for oauth2 request {requestString + parameters}");
            var Oauth2Token = new OAuth2();
            string OAuthToken = Oauth2Token.GetOAuthToken(requestString + parameters);
            RequestParameters.SetHeaders(_authHeader, "Bearer " + OAuthToken);
        }

        internal void SetPostOrPutData(string postdataid)
        {
            string postData = PostData.GetPostData(postdataid);
            RequestParameters.SetRequestData(postData);
        }

        internal void SetApiMethod(string apimethod)
        {
            RequestParameters.SetAPIRequestMethod(apimethod);
        }
        internal void SetHeaders(string authorizationType, string headerParameters)
        {
            string rawHeaderParameters = headerParameters.Replace(" And ", ",");
            var arrayHeaders = rawHeaderParameters.Split(',');

            if (authorizationType.Equals("jwt", StringComparison.InvariantCultureIgnoreCase))
            {
                string authority = TestContext.Properties["Parameter:authority"].ToString();
                string brand = TestContext.Properties["Parameter:brand"].ToString();
                string uid = TestContext.Properties["Parameter:customeruid"].ToString();
                RequestParameters.SetHeaders(_authHeader, JsonWebToken.GetJWToken(authority, brand, uid));
            }
            else if (authorizationType.Equals("basic", StringComparison.InvariantCultureIgnoreCase))
            {
                string clientId = TestContext.Properties["Parameter:clientid"].ToString();
                string clientSecret = TestContext.Properties["Parameter:clientsecret"].ToString();
                string encodedAuthString = BasicAuthentication.GetBasicAuthString(clientId, clientSecret);
                RequestParameters.SetHeaders(_authHeader, "Basic " + encodedAuthString);
            }

            foreach (var authorizationParameter in arrayHeaders)
            {
                try
                {
                    var parameterValue = TestContext.Properties["Parameter:" + authorizationParameter].ToString();
                    if (parameterValue != null)
                    {
                        RequestParameters.SetHeaders(authorizationParameter, parameterValue);
                    }
                }
                catch { }
            }
            RequestParameters.SetHeaders("x-transaction-id", Helper.GenerateTransactionId());
        }

        internal bool CheckValidityOfJwToken(string jsonWebToken)
        {
            string jsonWebTokenToVerify = jsonWebToken.Replace("Bearer ", "");
            string[] tokenSection = jsonWebTokenToVerify.Split('.');
            var header = tokenSection[0];
            string headerJson = Encoding.UTF8.GetString(Convert.FromBase64String(header));
            string headerData = JsonConvert.DeserializeObject(headerJson).ToString();
            bool isHeaderValid = headerData.Contains("RS256");

            if (isHeaderValid){return true;} return false;
        }

        internal string SetJwtParametersAndGetJwToken(string jsonTokenParameters)
        {
            string authority = TestContext.Properties["Parameter:brand"].ToString();
            string brand = TestContext.Properties["Parameter:brand"].ToString();
            string uid = TestContext.Properties["Parameter:customeruid"].ToString();
            string jsonWebToken = JsonWebToken.GetJWToken(authority, brand, uid);
            Console.WriteLine($"Generated json token is {jsonWebToken} Verify the token on JWT.io website");
            return jsonWebToken;
        }

        public Dictionary<string, object> SetRequestAndGetResponse()
        {
            var request = new Request();
            var response = new APIResponse();
            request.SetAPIRequest(RequestParameters);
            return response.GetResponse(request);
        }
    }
}
