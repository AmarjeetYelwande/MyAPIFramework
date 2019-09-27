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
        private RequestParameters _requestparameters { get; set; }
        private static TestContext _testContext { get; set; }

        string authheader = "Authorization";        

        public CommonMethods(ScenarioContext scenarioContext)
        {
            _requestparameters = new RequestParameters();
            _testContext = scenarioContext.ScenarioContainer.Resolve<TestContext>();
        }
    
        internal void SetEndpoint(string application)
        {
            _requestparameters.SetUri(application);
        }

        internal void SetResource(string resource)
        {
            _requestparameters.SetResource(resource);
        }
        internal void SetEndPointParameters(string parameters)
        {
            Dictionary<string, string> allparams = new Dictionary<string, string>();
            string rawparameters = parameters.Replace(" And ", ",");
            var allparameters = rawparameters.Split(',');

            foreach (var key in allparameters)
            {
                try
                {
                    string value = _testContext.Properties["Parameter:" + key].ToString() ?? "";
                    allparams.Add(key, value);
                }
                catch { }
            }

            string endpointparameters = Helper.DictionaryToQueryParametersList(allparams);
            _requestparameters.AddURIParameters(endpointparameters);
        }
        internal void GenerateOAuth2Token(string oAuthendpoint, string oAuthParameters)
        {
            string OAuthParams = oAuthParameters.Replace(" And ", ",");
            var oauthparams = OAuthParams.Split(',');

            Dictionary<string, string> allparams = new Dictionary<string, string>();

            foreach (var key in oauthparams)
            {
                try
                {
                    string value = _testContext.Properties["Parameter:" + key].ToString() ?? "";
                    allparams.Add(key, value);
                }
                catch { }
            }

            string authserver = _testContext.Properties["Parameter:authserver"].ToString();
            string requeststring = EndPoint.GetEndpoint(authserver);
            string parameters = Helper.DictionaryToQueryParametersList(allparams);
            Console.WriteLine($"Printing endpoint and parameters for oauth2 request {requeststring + parameters}");
            var Oauth2Token = new OAuth2();
            string OAuthToken = Oauth2Token.GetOAuthToken(requeststring + parameters);
            _requestparameters.SetHeaders(authheader, "Bearer " + OAuthToken);
        }

        internal void SetPostOrPutData(string postdataid)
        {
            string postdata = PostData.GetPostData(postdataid);
            _requestparameters.SetRequestData(postdata);
        }

        internal void SetAPIMethod(string apimethod)
        {
            _requestparameters.SetAPIRequestMethod(apimethod);
        }
        internal void SetHeaders(string authtype, string headerparameters)
        {
            string rawheaderparameters = headerparameters.Replace(" And ", ",");
            var arrayheaders = rawheaderparameters.Split(',');

            if (authtype.Equals("jwt", StringComparison.InvariantCultureIgnoreCase))
            {
                string authority = _testContext.Properties["Parameter:authority"].ToString();
                string brand = _testContext.Properties["Parameter:brand"].ToString();
                string uid = _testContext.Properties["Parameter:customeruid"].ToString();
                _requestparameters.SetHeaders(authheader, JsonWebToken.GetJWToken(authority, brand, uid));
            }
            else if (authtype.Equals("basic", StringComparison.InvariantCultureIgnoreCase))
            {
                string clientid = _testContext.Properties["Parameter:clientid"].ToString();
                string clientsecret = _testContext.Properties["Parameter:clientsecret"].ToString();
                string EncodedAuthString = BasicAuthentication.GetBasicAuthString(clientid, clientsecret);
                _requestparameters.SetHeaders(authheader, "Basic " + EncodedAuthString);
            }

            foreach (string authparameter in arrayheaders)
            {
                try
                {
                    var parametervalue = _testContext.Properties["Parameter:" + authparameter].ToString();
                    if (parametervalue != null)
                    {
                        _requestparameters.SetHeaders(authparameter, parametervalue);
                    }
                }
                catch { }
            }
            _requestparameters.SetHeaders("x-transaction-id", Helper.GenerateTransactionId());
        }

        internal bool CheckValidityOfJWToken(string jwtoken)
        {
            string jwtokentoverify = jwtoken.Replace("Bearer ", "");
            string[] tokensection = jwtokentoverify.Split('.');
            var header = tokensection[0];
            string headerJson = Encoding.UTF8.GetString(Convert.FromBase64String(header));
            string headerData = JsonConvert.DeserializeObject(headerJson).ToString();
            bool isHeaderValid = headerData.Contains("RS256");

            if (isHeaderValid){return true;} return false;
        }

        internal string SetJWTParametersAndGetJWToken(string jwtokenparameters)
        {
            string authority = _testContext.Properties["Parameter:brand"].ToString();
            string brand = _testContext.Properties["Parameter:brand"].ToString();
            string uid = _testContext.Properties["Parameter:customeruid"].ToString();
            string jwtoken = JsonWebToken.GetJWToken(authority, brand, uid);
            Console.WriteLine($"Generated json token is {jwtoken} Verify the token on JWT.io website");
            return jwtoken;
        }

        public Dictionary<string, object> SetRequestAndGetResponse()
        {
            var request = new Request();
            var response = new APIResponse();
            request.SetAPIRequest(_requestparameters);
            return response.GetResponse(request);
        }
    }
}
