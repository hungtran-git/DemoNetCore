using jwt_authentication_api_example.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace jwt_authentication_api_example.Helpers
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly AppSettings _appSettings;

        public JwtMiddleware(RequestDelegate next, IOptions<AppSettings> appSettings)
        {
            _next = next;
            _appSettings = appSettings.Value;
        }

        public async Task Invoke(HttpContext context, IUserService userService)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
                attachUserToContext(context, userService, token);

            await _next(context);
        }

        private void attachUserToContext(HttpContext context, IUserService userService, string token)
        {
            try
            {
                /*
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

                // attach user to context on successful jwt validation
                var user = userService.GetById(userId);
                if(user != null)
                    context.Items["User"] = user;
                */

                /*
                var tokenHandler = new JwtSecurityTokenHandler();

                var publicKey = _appSettings.RsaPublicKey.ToByteArray();
                using RSA rsa = RSA.Create();
                rsa.ImportRSAPublicKey(publicKey, out _);

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = new RsaSecurityKey(rsa),
                    CryptoProviderFactory = new CryptoProviderFactory()
                    {
                        CacheSignatureProviders = false
                    }
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

                // attach user to context on successful jwt validation
                var user = userService.GetById(userId);
                if (user != null)
                    context.Items["User"] = user;
                */

                var tokenHandler = new JwtSecurityTokenHandler();

                var publicKey = _appSettings.ECDsaPublicKey.ToByteArray();
                ECDsa ecdsa = new ECDsaCng(CngKey.Import(publicKey, CngKeyBlobFormat.EccPublicBlob))
                {
                    HashAlgorithm = CngAlgorithm.ECDsaP256
                };

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = new ECDsaSecurityKey(ecdsa),
                    CryptoProviderFactory = new CryptoProviderFactory()
                    {
                        CacheSignatureProviders = false
                    }
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

                // attach user to context on successful jwt validation
                var user = userService.GetById(userId);
                if (user != null)
                    context.Items["User"] = user;
            }
            catch (Exception e)
            {
                // do nothing if jwt validation fails
                // user is not attached to context so request won't have access to secure routes
            }
        }
    }
}
