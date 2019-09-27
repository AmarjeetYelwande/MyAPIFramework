using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyCompany.NetCore.Helpers.Common;
using MyCompany.NetCore.Helpers.Validation;
using MyCompany.NetCore.Tests.Common;
using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;

namespace MyCompany.NetCore.Tests.FeatureSteps
{
    [Binding]
    class CommonStep
    {
        private CommonMethods _SetParameters { get; set; }
        private Dictionary<string, object> _responsedata { get; set; }
        private ScenarioContext _scenarioContext { get; set; }
        private string jwtoken { get; set; }
        public CommonStep(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [BeforeScenario]
        public void InitialiseScenarioObjects()
        {
            _SetParameters = new CommonMethods(_scenarioContext);
        }

        [Given(@"I have the endpoint for resource (.*)")]
        public void SetEndpoint(string application)
        {
            _SetParameters.SetEndpoint(EndPoint.GetEndpoint(application));
        }

        [Given(@"I have (.*) for endpoint")]
        public void GivenIHaveForEndpoint(string resource)
        {
            _SetParameters.SetResource(Resource.GetResource(resource));
        }

        [Given(@"I set api request method to (.*)")]
        public void GivenISetApiRequestMethodTo(string apimethod)
        {
            _SetParameters.SetAPIMethod(apimethod);
        }

        [Given(@"I have generated token for OAuthtwo using (.*) with parameters (.*)")]
        public void GenerateOAuth2(string OAuthendpoint, string OAuthParameters)
        {
            _SetParameters.GenerateOAuth2Token(OAuthendpoint, OAuthParameters);
        }

        [Given(@"I have set (.*) for the request with header parameters (.*)")]
        public void SetHeader(string authorisationtype, string headerparameters)
        {
            string authtype = authorisationtype.ToUpper();
            _SetParameters.SetHeaders(authtype, headerparameters);
        }

        [Given(@"I have parameters for constructing Endpoint (.*)")]
        public void SetEndpointParameters(string endpointparameters)
        {
            _SetParameters.SetEndPointParameters(endpointparameters);
        }

        [Given(@"I have data for post request in (.*)")]
        public void SetPostData(string payloadid)
        {
            _SetParameters.SetPostOrPutData(payloadid);
        }

        [When(@"I send request")]
        public void SendRequest()
        {
            _responsedata = _SetParameters.SetRequestAndGetResponse();
        }

        [Then(@"The response status code should be (.*) with standard description")]
        public void GetResponseCode(string expectedresponsecode)
        {
            string actualresponsecode = _responsedata["StatusCode"].ToString();

            Assert.AreEqual(expectedresponsecode, actualresponsecode,
            ($"Received response code :  {actualresponsecode} does not match with expected code :  {expectedresponsecode}"));

            string actualresponsedescription = _responsedata["StatusDescription"].ToString();
            string expectedrespoonsedescription = RestResponseCode.StatusCodeToMessageMapping[(int)_responsedata["StatusCode"]].ToString();

            Assert.AreEqual(expectedrespoonsedescription, actualresponsedescription,
            ($"Received response description :  {actualresponsedescription} does not match with expected response description :  {expectedrespoonsedescription}"));
        }

        [Then(@"The Response body should contain expected data for (.*) call")]
        public void VerifyResponseBody(string api)
        {
            //use api string reference to store response values in code and assert response against it
            string responsecontent = _responsedata["SourceCode"].ToString();
            Assert.IsTrue(JSONUtilities.ValidateJSONContentAgainstSchema(responsecontent),
                $"Schema validation failed for response");
        }

        [Then(@"The response should be received in (.*) milliseconds")]
        public void VerifyRequestResponseTime(double maxresponsetime)
        {
            double actualresponsetime = Convert.ToDouble(_responsedata["ResponseTime"]);
            Assert.IsTrue(actualresponsetime < maxresponsetime, $"Response time taken by request " +
                $"{actualresponsetime} in ms exceeded maximum allowed time of {maxresponsetime} in ms");
        }

        [Given(@"I want to generate JWToken for my application")]
        public void SetParametersForJWToken()
        {
            Console.WriteLine("Generating Json Web Token....");
        }        

        [When(@"I generate the JWToken with parameters (.*)")]
        public void GenerateJWToken(string jwtokenparameters)
        {
            jwtoken = _SetParameters.SetJWTParametersAndGetJWToken(jwtokenparameters);
        }

        [Then(@"I get well formed JWToken which I can verify for its integrity")]
        public void VerifyJWToken()
        {
            bool IsTokenValid = _SetParameters.CheckValidityOfJWToken(jwtoken);
            Assert.IsTrue(IsTokenValid, $"Generated JWToken is not valid");
        }

        [AfterScenario]
        public void CleanUpTestObjects()
        {
            try
            {

            }
            catch { }
        }
    }
}
