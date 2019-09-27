using System;
using System.Collections.Generic;
using System.IO;

namespace MyCompany.NetCore.Operation
{
    public class APIResponse
    {
        private Stream ResponseStream { get; set; }
        private string ResponseValue { get; set; }
        public Dictionary<string, object> ResponseValues { get; set; }
        public Dictionary<string, object> GetResponse(Request requestresponse)
        {
            Dictionary<string, object> ResponseValues = new Dictionary<string, object>();
            var response = requestresponse.Response;
            var responsetime = requestresponse.ResponseTime;
            
            try
            {
                ResponseStream = response.GetResponseStream();
                using (var reader = new StreamReader(ResponseStream))
                {                    
                    ResponseValue = reader.ReadToEnd();
                }
            }
            catch (Exception StreamProcessing)
            {
                Console.WriteLine($"Unable to process response stream due to error {StreamProcessing.Message}");
                throw;
            }

            finally
            {
                if (ResponseStream != null) ResponseStream.Dispose();
            }

            ResponseValues.Add("SourceCode", ResponseValue);
            ResponseValues.Add("StatusDescription", response.StatusDescription);
            ResponseValues.Add("StatusCode", (int)response.StatusCode);
            ResponseValues.Add("Headers", response.Headers.ToString());
            ResponseValues.Add("ResponseTime", responsetime);
            Console.WriteLine($"Received response is : {ResponseValue}");
            return ResponseValues;
        }
    }
}
