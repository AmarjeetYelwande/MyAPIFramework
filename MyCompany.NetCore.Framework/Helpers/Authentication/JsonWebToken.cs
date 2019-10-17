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
        public static string GetJsonWebToken(string authority, string brand, string Uid)
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
            var stringToken = Jose.JWT.Encode(GetPayload(authority, brand, Uid), rsa, Jose.JwsAlgorithm.RS256);
            
            return ("Bearer " + stringToken);
        }
        private static object GetPayload(string authority, string brand, string uid)
        {
            DateTime now = DateTime.UtcNow;
            DateTime issued = DateTime.Now;
            DateTime expire = DateTime.Now.AddHours(1);
            string currentDirectory = Directory.GetCurrentDirectory();
            string pathToJson = Path.Combine(currentDirectory, "Data","JWToken", "BrandIdentity.json");
            var rawJsonSchema = new StreamReader(pathToJson);
            var finalJson = rawJsonSchema.ReadToEnd();
            var payload = new JwtParser();
            payload = JwtParser.FromJson(finalJson);
            payload.iss = "https://idp." + authority + ".co.uk/v1/authorize";
            payload.iat = ToUnixTime(issued);
            payload.exp = ToUnixTime(expire);
            payload.brand = brand;
            if (uid != string.Empty) payload.identifierUid = uid;

            return payload;
        }
        private static long ToUnixTime(DateTime dateTime)
        {
            return (int)(dateTime.ToUniversalTime().Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }
    }
}