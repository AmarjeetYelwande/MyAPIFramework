using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyCompany.NetCore.Tests.Common;
using System;
using System.Collections.Generic;
using MyCompany.NetCore.Framework.Helpers.Common;
using MyCompany.NetCore.Framework.Helpers.Validation;
using TechTalk.SpecFlow;

namespace MyCompany.NetCore.Tests.FeatureSteps
{
    [Binding]
    internal class CommonStep
    {
        private CommonMethods SetParameters { get; }
        private Dictionary<string, object> ResponseData { get; set; }
        private ScenarioContext ScenarioContext { get; }
        private string JavaWebToken { get; set; }
        public CommonStep(ScenarioContext scenarioContext, CommonMethods setParameters)
        {
            ScenarioContext = scenarioContext;
            SetParameters = setParameters;
        }

        [Given(@"I have the endpoint for resource (.*)")]
        public void SetEndpoint(string application)
        {
            SetParameters.SetEndpoint(EndPoint.GetEndpoint(application));
        }

        [Given(@"I have (.*) for endpoint")]
        public void GivenIHaveForEndpoint(string resource)
        {
            SetParameters.SetResource(Resource.GetResource(resource));
        }

        [Given(@"I set api request method to (.*)")]
        public void GivenISetApiRequestMethodTo(string apiMethod)
        {
            SetParameters.SetApiMethod(apiMethod);
        }

        [Given(@"I have generated token for OAuthtwo using (.*) with parameters (.*)")]
        public void GenerateOAuth2(string oauthEndpoint, string oAuthParameters)
        {
            SetParameters.GenerateOAuth2Token(oauthEndpoint, oAuthParameters);
        }

        [Given(@"I have set (.*) for the request with header parameters (.*)")]
        public void SetHeader(string authorizationType, string headerParameters)
        {
            string authType = authorizationType.ToUpper();
            SetParameters.SetHeaders(authType, headerParameters);
        }

        [Given(@"I have parameters for constructing Endpoint (.*)")]
        public void SetEndpointParameters(string endpointParameters)
        {
            SetParameters.SetEndPointParameters(endpointParameters);
        }

        [Given(@"I have data for post request in (.*)")]
        public void SetPostData(string payloadId)
        {
            SetParameters.SetPostOrPutData(payloadId);
        }

        [When(@"I send request")]
        public void SendRequest()
        {
            ResponseData = SetParameters.SetRequestAndGetResponse();
        }

        [Then(@"The response status code should be (.*) with standard description")]
        public void GetResponseCode(string expectedResponseCode)
        {
            string actualResponseCode = ResponseData["StatusCode"].ToString();

            Assert.AreEqual(expectedResponseCode, actualResponseCode,
            ($"Received response code :  {actualResponseCode} does not match with expected code :  {expectedResponseCode}"));

            string actualResponseDescription = ResponseData["StatusDescription"].ToString();
            string expectedResponseDescription = RestResponseCode.StatusCodeToMessageMapping[(int)ResponseData["StatusCode"]].ToString();

            Assert.AreEqual(expectedResponseDescription, actualResponseDescription,
            ($"Received response description :  {actualResponseDescription} does not match with expected response description :  {expectedResponseDescription}"));
        }

        [Then(@"The Response body should contain expected data for (.*) call")]
        public void VerifyResponseBody(string api)
        {
            //use api string reference to store response values in code and assert response against it
            string responseContent = ResponseData["SourceCode"].ToString();
            Assert.IsTrue(JsonUtilities.ValidateJsonContentAgainstSchema(responseContent),
                $"Schema validation failed for response");
        }

        [Then(@"The response should be received in (.*) milliseconds")]
        public void VerifyRequestResponseTime(double maxResponseTime)
        {
            double actualResponseTime = Convert.ToDouble(ResponseData["ResponseTime"]);
            Assert.IsTrue(actualResponseTime < maxResponseTime, $"Response time taken by request " +
                $"{actualResponseTime } in ms exceeded maximum allowed time of {maxResponseTime} in ms");
        }

        [Given(@"I want to generate JWToken for my application")]
        public void SetParametersForJsonWebToken()
        {
            Console.WriteLine("Generating Json Web Token....");
        }        

        [When(@"I generate the JWToken with parameters (.*)")]
        public void GenerateJwToken(string jsonWebTokenParameters)
        {
            JavaWebToken = SetParameters.SetJwtParametersAndGetJwToken(jsonWebTokenParameters);
        }

        [Then(@"I get well formed JWToken which I can verify for its integrity")]
        public void VerifyJWToken()
        {
            var isTokenValid = SetParameters.CheckValidityOfJwToken(JavaWebToken);
            Assert.IsTrue(isTokenValid, $"Generated JWToken is not valid");
        }

        [AfterScenario]
        public void CleanUpTestObjects()
        {
            try
            {
                // Add your cleanup code here
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
}
