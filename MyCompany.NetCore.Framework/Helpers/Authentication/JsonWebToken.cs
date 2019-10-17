using System;
using System.IO;
using System.Security.Cryptography;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;

namespace MyCompany.NetCore.Framework.Helpers.Authentication
{
    public static class JsonWebToken
    {
        public static string GetJWToken(string Authority, string Brand, string Uid)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string pathToFile = Path.Combine(currentDirectory, "Data","JWToken", "PrivateKey.pem");
            string privateKey = File.ReadAllText(pathToFile);
            RSAParameters rsaParams;

            using (var tr = new StringReader(privateKey))
            {
                AsymmetricCipherKeyPair keyPair;
                using (var reader = File.OpenText(pathToFile))
                    keyPair = (AsymmetricCipherKeyPair)new PemReader(reader).ReadObject();
                if (keyPair == null)
                {
                    throw new Exception("Could not read RSA private key");
                }
                var privateRsaParams = keyPair.Private as RsaPrivateCrtKeyParameters;
                rsaParams = DotNetUtilities.ToRSAParameters(privateRsaParams);
            }
            var rsa = new RSACryptoServiceProvider();
            rsa.ImportParameters(rsaParams);
            string stringtoken;
            stringtoken = Jose.JWT.Encode(GetPayload(Authority, Brand, Uid), rsa, Jose.JwsAlgorithm.RS256);
            
            return ("Bearer " + stringtoken);
        }
        private static object GetPayload(string Authority, string Brand, string Uid)
        {
            DateTime now = DateTime.UtcNow;
            DateTime issued = DateTime.Now;
            DateTime expire = DateTime.Now.AddHours(1);
            string currentDirectory = Directory.GetCurrentDirectory();
            string pathToJson = Path.Combine(currentDirectory, "Data","JWToken", "BrandIdentity.json");
            var rawjsonschema = new StreamReader(pathToJson);
            var finaljson = rawjsonschema.ReadToEnd();
            var payload = new JwtParser();
            payload = JwtParser.FromJson(finaljson);
            payload.iss = "https://idp." + Authority + ".co.uk/v1/authorize";
            payload.iat = ToUnixTime(issued);
            payload.exp = ToUnixTime(expire);
            payload.brand = Brand;
            if (Uid != string.Empty) payload.identifierUid = Uid;

            return payload;
        }
        private static long ToUnixTime(DateTime dateTime)
        {
            return (int)(dateTime.ToUniversalTime().Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }
    }
}