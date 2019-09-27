using System;

namespace MyCompany.NetCore.Helpers.Authentication
{
    public static class BasicAuthentication
    {
        public static string GetBasicAuthString(string clientid, string clientsecret)
        {
            var ClientId = SetAuthString(clientid);
            var ClientSecret = SetAuthString(clientsecret);
            return EncodeToBase64(ClientId, ClientSecret);
        }
        private static string SetAuthString(string clientcredentials)
        {
            if (IsDigitsOnly(clientcredentials))
            {
                var random = new Random();
                var allcharacters = "abcdefghijklmnopqrstuvwxyz0123456789";
                var finalstring = new char[Int32.Parse(clientcredentials)];

                for (int i = 0; i < finalstring.Length; i++)
                {
                    finalstring[i] = allcharacters[random.Next(allcharacters.Length)];
                }
                return new string(finalstring);
            }
            else
            {
                return clientcredentials;
            }
        }
        private static bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }
            return true;
        }
        private static string EncodeToBase64(string clientid, string clientsecret)
        {
            var finalStringToEncode = clientid + ":" + clientsecret;
            var plainBytes = System.Text.Encoding.UTF8.GetBytes(finalStringToEncode);
            return Convert.ToBase64String(plainBytes);
        }
    }
}
