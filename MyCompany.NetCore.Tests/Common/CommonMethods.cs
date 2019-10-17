using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using MyCompany.NetCore.Framework.Helpers.Authentication;
using MyCompany.NetCore.Framework.Helpers.Common;
using MyCompany.NetCore.Framework.Operation;
using TechTalk.SpecFlow;

namespace MyCompany.NetCore.Tests.Common
{
    public class CommonMethods
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
            string rawParameters = parameters.Replace(" And ", ",");
            var customParameters = rawParameters.Split(',');

            foreach (var key in customParameters)
            {
                try
                {
                    string value = TestContext.Properties["Parameter:" + key].ToString() ?? "";
                    allParameters.Add(key, value);
                }
                catch (Exception)
                {
                    // ignored
                }
            }

            string endpointParameters = Helper.DictionaryToQueryParametersList(allParameters);
            RequestParameters.AddUriParameters(endpointParameters);
        }
        internal void GenerateOAuth2Token(string oAuthEndPoint, string oAuthParameters)
        {
            string oAuthParams = oAuthParameters.Replace(" And ", ",");
            var oauthParameters = oAuthParams.Split(',');

            var allParameters = new Dictionary<string, string>();

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
            var oauth2Token = new OAuth2();
            string OAuthToken = oauth2Token.GetOAuthToken(requestString + parameters);
            RequestParameters.SetHeaders(_authHeader, "Bearer " + OAuthToken);
        }

        internal void SetPostOrPutData(string postDataId)
        {
            string postData = PostData.GetPostData(postDataId);
            RequestParameters.SetRequestData(postData);
        }

        internal void SetApiMethod(string apiMethod)
        {
            RequestParameters.SetApiRequestMethod(apiMethod);
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
                RequestParameters.SetHeaders(_authHeader, JsonWebToken.GetJsonWebToken(authority, brand, uid));
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
                catch (Exception)
                {
                    // ignored
                }
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

            return isHeaderValid;
        }

        internal string SetJwtParametersAndGetJwToken(string jsonTokenParameters)
        {
            string authority = TestContext.Properties["Parameter:brand"].ToString();
            string brand = TestContext.Properties["Parameter:brand"].ToString();
            string uid = TestContext.Properties["Parameter:customeruid"].ToString();
            string jsonWebToken = JsonWebToken.GetJsonWebToken(authority, brand, uid);
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
