using jwt_authentication_api_example.Entities;
using jwt_authentication_api_example.Helpers;
using jwt_authentication_api_example.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace jwt_authentication_api_example.Services
{
    public interface IUserService
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model);
        IEnumerable<User> GetAll();
        User GetById(int id);
    }

    public class UserService : IUserService
    {
        // users hardcoded for simplicity, store in a db with hashed passwords in production applications
        private List<User> _users = new List<User>
        {
            new User { Id = 1, FirstName = "Test", LastName = "User", Username = "test", Password = "test" }
        };

        private readonly AppSettings _appSettings;

        public UserService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public AuthenticateResponse Authenticate(AuthenticateRequest model)
        {
            var user = _users.SingleOrDefault(x => x.Username == model.Username && x.Password == model.Password);

            // return null if user not found
            if (user == null) return null;

            // authentication successful so generate jwt token
            var token = generateJwtToken(user);

            return new AuthenticateResponse(user, token);
        }

        public IEnumerable<User> GetAll()
        {
            return _users;
        }

        public User GetById(int id)
        {
            return _users.FirstOrDefault(x => x.Id == id);
        }

        // helper methods

        private string generateJwtToken(User user)
        {
            /*
            // HmacSha256Signature
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
            */

            /*
             
            // RsaSha256
            var privateKey = _appSettings.RsaPrivateKey.ToByteArray();
            using ECDsa ecdsa = ECDsa.Create();
            ecdsa.ImportECPrivateKey(privateKey, out _);

            var signingCredentials = new SigningCredentials(new ECDsaSecurityKey(ecdsa), SecurityAlgorithms.Ecd)
            {
                CryptoProviderFactory = new CryptoProviderFactory { CacheSignatureProviders = false }
            };

            var now = DateTime.Now;
            var unixTimeSeconds = new DateTimeOffset(now).ToUnixTimeSeconds();

            var jwt = new JwtSecurityToken(
                    claims: new Claim[] {
                        new Claim("id", user.Id.ToString())
                    },
                    notBefore: now,
                    expires: now.AddMinutes(30),
                    signingCredentials: signingCredentials
                );
            return new JwtSecurityTokenHandler().WriteToken(jwt);
             */

            // ECDsa
            var privateKey = _appSettings.ECDsaPrivateKey.ToByteArray();
            ECDsa ecdsa = new ECDsaCng(CngKey.Import(privateKey, CngKeyBlobFormat.EccPrivateBlob))
            {
                HashAlgorithm = CngAlgorithm.ECDsaP256
            };

            var signingCredentials = new SigningCredentials(
                    new ECDsaSecurityKey(ecdsa), SecurityAlgorithms.EcdsaSha256);

            var now = DateTime.Now;
            var unixTimeSeconds = new DateTimeOffset(now).ToUnixTimeSeconds();

            var jwt = new JwtSecurityToken(
                    claims: new Claim[] {
                        new Claim("id", user.Id.ToString())
                    },
                    notBefore: now,
                    expires: now.AddMinutes(30),
                    signingCredentials: signingCredentials
                );
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
