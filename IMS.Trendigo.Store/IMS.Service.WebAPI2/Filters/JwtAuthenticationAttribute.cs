using IMS.Common.Core.Data;
using IMS.Common.Core.Services;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Filters;

namespace IMS.Service.WebAPI2.Filters
{
    public class JwtAuthenticationAttribute : Attribute, IAuthenticationFilter
    {
        private IMSEntities db = new IMSEntities();

        public string Realm { get; set; }
        public bool AllowMultiple => false;

        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            var request = context.Request;
            var authorization = request.Headers.Authorization;

            if (authorization == null)
            {
                context.ErrorResult = new AuthenticationFailureResult("Missing Jwt Token", request);
                return;
            }

            if (string.IsNullOrEmpty(authorization.Parameter))
            {
                context.ErrorResult = new AuthenticationFailureResult("Missing Jwt Token", request);
                return;
            }

            var token = authorization.Parameter;

            bool validToken = await ValidateToken(token);

            if (!validToken)
            {
                context.ErrorResult = new AuthenticationFailureResult("Invalid token", request);
            }

            context.Principal = null;

            //var principal = await AuthenticateJwtToken(token);

            //if (principal == null)
            //    context.ErrorResult = new AuthenticationFailureResult("Invalid token", request);

            //else
            //    context.Principal = principal;
        }

        protected Task<IPrincipal> AuthenticateJwtToken(string token)
        {
            bool validToken = ValidateToken(token).GetAwaiter().GetResult();

            if (validToken)   //, out var username
            {
                // based on username to get more information from database in order to build local identity
                var claims = new List<Claim>
                {
                    //new Claim(ClaimTypes.Name, username)
                    // Add more claims if needed: Roles, ...
                };

                var identity = new ClaimsIdentity(claims, "Jwt");
                IPrincipal user = new ClaimsPrincipal(identity);

                return Task.FromResult(user);
            }

            return Task.FromResult<IPrincipal>(null);
        }

        private async Task<bool> ValidateToken(string token) //, out string username
        {
            //username = null;

            //var simplePrinciple = GetPrincipal(token);
            //var identity = simplePrinciple?.Identity as ClaimsIdentity;

            //if (identity == null)
            //    return false;

            //if (!identity.IsAuthenticated)
            //    return false;

            //var usernameClaim = identity.FindFirst(ClaimTypes.Name);
            //username = usernameClaim?.Value;

            //if (string.IsNullOrEmpty(username))
            //    return false;

            //// More validate to check whether username exists in system
            //string[] complexUsername = username.Split(';');

            //string email = complexUsername[0];
            //string deviceId = complexUsername[1];

            //if (string.IsNullOrEmpty(email))
            //    return false;

            //if (new IMSEntities().AspNetUsers.FirstOrDefault(a => a.Email == email) == null)
            //    return false;

            //return true;
            var task = await TokenManager.validateToken(token);
            return task;
        }

        public static ClaimsPrincipal GetPrincipal(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

                if (jwtToken == null)
                    return null;

                var securityManager = new SecurityManager();

                var symmetricKey = Convert.FromBase64String("Trendigo123!");
                //var symmetricKey = Convert.FromBase64String(Decrypt("This is the public key", ASCIIEncoding.ASCII.GetBytes(token)));

                var validationParameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = false,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(symmetricKey)
                };

                SecurityToken securityToken;
                var principal = tokenHandler.ValidateToken(token, validationParameters, out securityToken);

                return principal;
            }
            catch (Exception)
            {
                //should write log
                return null;
            }
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            Challenge(context);
            return Task.FromResult(0);
        }

        private void Challenge(HttpAuthenticationChallengeContext context)
        {
            string parameter = null;

            if (!string.IsNullOrEmpty(Realm))
                parameter = "realm=\"" + Realm + "\"";

            context.ChallengeWith("Bearer", parameter);
        }
    }
}