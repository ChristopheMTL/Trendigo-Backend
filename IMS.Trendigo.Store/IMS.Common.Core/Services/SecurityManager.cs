using IMS.Common.Core.Data;
using IMS.Common.Core.Entities;
using IMS.Store.Common.Extensions;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Common.Core.Services
{
    public class SecurityManager
    {
        public TokenData GetToken(IMSUser ims_user)
        {
            return GetToken(ims_user.TransaxId);
        }
        public TokenData GetToken(string transaxUserId, bool gotoEkoPay = true)
        {
            //String TIMESTAMP = new EPOCHHelper().ConvertToTimestamp(DateTime.Now).ToString();

            TokenData accessToken = new TransaxHelper().GetToken(transaxUserId, ConfigurationManager.AppSettings["IMSPrivateKey"], gotoEkoPay);

            accessToken.admin = transaxUserId.ToString();

            return accessToken;

        }
    }

    public class JWTManager
    {
        private IMSEntities db = new IMSEntities();

        private const string Secret = "db3OIsj+BXE9NZDy0t8W3TcNekrF+2d/1sFnWG4HnV8TZY30iTOdtVWJG8abWvB1GlOgJuQZdcF2Luqm/hccMw==";

        public static string GenerateToken(string username, string deviceId, int expireDays = 30)
        {
            var complexUsername = username + (!string.IsNullOrEmpty(deviceId) ? ";" + deviceId : "");

            var symmetricKey = Convert.FromBase64String(Secret);
            var tokenHandler = new JwtSecurityTokenHandler();

            var now = DateTime.UtcNow;
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, complexUsername)
                }),

                Expires = now.AddDays(Convert.ToInt32(expireDays)),

                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(symmetricKey),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var stoken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(stoken);

            return token;
        }

        //public static bool ValidateToken(string token, out string username)
        //{
        //    username = null;

        //    var simplePrinciple = GetPrincipal(token);
        //    var identity = simplePrinciple.Identity as ClaimsIdentity;

        //    if (identity == null)
        //        return false;

        //    if (!identity.IsAuthenticated)
        //        return false;

        //    var usernameClaim = identity.FindFirst(ClaimTypes.Name);
        //    username = usernameClaim?.Value;

        //    if (string.IsNullOrEmpty(username))
        //        return false;

        //    // More validate to check whether username exists in system
        //    string[] complexUsername = username.Split(';');

            


        //    return true;
        //}

        //protected Task<IPrincipal> AuthenticateJwtToken(string token)
        //{
        //    string username;

        //    if (ValidateToken(token, out username))
        //    {
        //        // based on username to get more information from database 
        //        // in order to build local identity
        //        var claims = new List<Claim>
        //    {
        //        new Claim(ClaimTypes.Name, username)
        //        // Add more claims if needed: Roles, ...
        //    };

        //        var identity = new ClaimsIdentity(claims, "Jwt");
        //        IPrincipal user = new ClaimsPrincipal(identity);

        //        return Task.FromResult(user);
        //    }

        //    return Task.FromResult<IPrincipal>(null);
        //}

        //public static ClaimsPrincipal GetPrincipal(string token)
        //{
        //    try
        //    {
        //        var tokenHandler = new JwtSecurityTokenHandler();
        //        var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

        //        if (jwtToken == null)
        //            return null;

        //        var securityManager = new SecurityManager();

        //        //var symmetricKey = Convert.FromBase64String(Secret);
        //        var symmetricKey = Convert.FromBase64String(Decrypt("This is the public key", ASCIIEncoding.ASCII.GetBytes(token)));

        //        var validationParameters = new TokenValidationParameters()
        //        {
        //            RequireExpirationTime = true,
        //            ValidateIssuer = false,
        //            ValidateAudience = false,
        //            IssuerSigningKey = new SymmetricSecurityKey(symmetricKey)
        //        };

        //        SecurityToken securityToken;
        //        var principal = tokenHandler.ValidateToken(token, validationParameters, out securityToken);

        //        return principal;
        //    }
        //    catch (Exception)
        //    {
        //        //should write log
        //        return null;
        //    }
        //}

        private byte[] Encrypt(string publicKeyXML, string dataToDycript)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(publicKeyXML);

            return rsa.Encrypt(ASCIIEncoding.ASCII.GetBytes(dataToDycript), true);
        }

        public static string Decrypt(string publicPrivateKeyXML, byte[] encryptedData)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(publicPrivateKeyXML);

            return ASCIIEncoding.ASCII.GetString(rsa.Decrypt(encryptedData, true));
        }
    }
}
