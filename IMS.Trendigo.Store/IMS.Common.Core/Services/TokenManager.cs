using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using IMS.Common.Core.DTO;
using IMS.Store.Common.Extensions;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.KeyVault.Models;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Rest.Azure;
using Newtonsoft.Json;

namespace IMS.Common.Core.Services
{
    public class TokenManager
    {
        public static async Task<TokenDTO> generateToken(string deviceId)
        {
            TokenDTO token = new TokenDTO();

            token.jti = Guid.NewGuid().ToString();

            KeyVaultClient keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(TokenManager.GetToken));

            KeyBundle keyBundle = await keyVaultClient.GetKeyAsync(ConfigurationManager.AppSettings["IMS.Service.Token.VaultBaseURL"], ConfigurationManager.AppSettings["IMS.Service.Token.KeyName"]);
            var header = JsonConvert.SerializeObject(new Dictionary<string, string>()
            {
                { JwtHeaderParameterNames.Alg, "RS512" },
                { JwtHeaderParameterNames.Typ, "JWT" },
                { JwtHeaderParameterNames.Kid, keyBundle.KeyIdentifier.Identifier}
            });

            var claims = JsonConvert.SerializeObject(new Dictionary<string, object>()
            {
                { JwtRegisteredClaimNames.Aud, ConfigurationManager.AppSettings["IMS.Service.Token.Claims.Aud"] },
                { JwtRegisteredClaimNames.Exp, new EPOCHHelper().ConvertToTimestamp(DateTime.Now.AddDays(Convert.ToInt32(ConfigurationManager.AppSettings["IMS.Service.Token.Claims.Exp"])))},
                { JwtRegisteredClaimNames.Jti, token.jti },
                { JwtRegisteredClaimNames.Iat, new EPOCHHelper().ConvertToTimestamp(DateTime.Now) },
                { JwtRegisteredClaimNames.Iss, ConfigurationManager.AppSettings["IMS.Service.Token.Claims.Iss"]},
                { JwtRegisteredClaimNames.Sub, deviceId }
            });

            string base64Header = Base64UrlTextEncoder.Encode(System.Text.Encoding.UTF8.GetBytes(header));
            string base64Payload = Base64UrlTextEncoder.Encode(System.Text.Encoding.UTF8.GetBytes(claims));
            var bytesToDigest = Encoding.UTF8.GetBytes(base64Header + "." + base64Payload);
            var hasher = new SHA512CryptoServiceProvider();
            var digest = hasher.ComputeHash(bytesToDigest);
            var signature = await keyVaultClient.SignAsync(keyBundle.Key.Kid, "RS512", digest);
            token.signedJwt = base64Header + "." + base64Payload + "." + Base64UrlTextEncoder.Encode(signature.Result);

            return token;
        }

        public static async Task<bool> validateToken(string token)
        {
            KeyVaultClient keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(TokenManager.GetToken));

            string jwtKid = "https://trendigo-test.vault.azure.net/keys/Trendigo-Test-Key/5b62df1945d740038e5c41fef090eb66";  // En vrai, il faut extraire le kid du jwt.

            string[] splittedJwt = token.Split('.');
            string toDigest = splittedJwt[0] + '.' + splittedJwt[1]; // header + . + payload
            byte[] signature = Base64UrlTextEncoder.Decode(splittedJwt[2]);
            var bytesToDigest = Encoding.UTF8.GetBytes(toDigest);
            var hasher = new SHA512CryptoServiceProvider(); // SHA512 vu qu'on utilise RS512 comme algo dans le token
            var digest = hasher.ComputeHash(bytesToDigest);
            return await keyVaultClient.VerifyAsync(jwtKid, "RS512", digest, signature);
        }

        public static async Task<string> GetToken(string authority, string resource, string scope)
        {
            string clientId = ConfigurationManager.AppSettings["IMS.Service.Token.ClientId"];
            string secret = ConfigurationManager.AppSettings["IMS.Service.Token.Secret"];
            ClientCredential cc = new ClientCredential(clientId, secret);
            var context = new AuthenticationContext(authority);
            var result = await context.AcquireTokenAsync(resource, cc);
            return result.AccessToken;
        }
    }
}
