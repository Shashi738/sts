using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _repository;

        public AuthenticationService(IUserRepository repository)
        {
            _repository = repository;
        }

        public async Task<string> AuthenticateAndGenerateToken(User user)
        {
            //authenticate user

            var userFound = await _repository.FindUserAsync(user);
            
            if (!userFound)
            {
                return string.Empty;
            }

            //generate token

            user.Role = "Admin";
            var token = GenerateToken(user);

            return token;
        }

        private string GenerateToken(User user)
        {
            var securityTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(@"ca3gOZG/G8qy09itnwwl9MZl4IRNz8Ba1gwHehP9U9dx/hQHDjGRQYrCYdKLaHqn");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserName),
                    new Claim(ClaimTypes.Role, user.Role),                   
                }),
                Expires =  DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };

            var securityToken = securityTokenHandler.CreateToken(tokenDescriptor);
            return securityTokenHandler.WriteToken(securityToken);
        }
    }
}
