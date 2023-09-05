using Auth.Core.Entities.Identity;
using Auth.Core.Interfaces.Authentication;
using Auth.Core.OptionsSetup;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Infrastructure.Authentication
{
	public class JwtProvider : IJwtProvider
	{
		private readonly JwtOptions _options;

		public JwtProvider(IOptions<JwtOptions> options)
		{
			_options = options.Value;
		}

		public string Generate(AppUser user)
		{
			// claims to be sent back to the user in the token on every request
			var claims = new List<Claim>
			{
				//new Claim(ClaimTypes.NameIdentifier, user.Id),
				//new Claim(ClaimTypes.Name, user.UserName),
				//new Claim(ClaimTypes.Email, user.Email),
				new Claim("id", user.Id.ToString()),
				new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
				new Claim(JwtRegisteredClaimNames.Email, user.Email),
				new Claim(JwtRegisteredClaimNames.Name, user.DisplayName)
			};


			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)); //should be 12 characters at least

			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);


			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(claims),
				Expires = DateTime.Now.AddMinutes(_options.DurationInMinutes),
				SigningCredentials = creds
			};

			var tokenHandler = new JwtSecurityTokenHandler();

			var token = tokenHandler.CreateToken(tokenDescriptor);

			return tokenHandler.WriteToken(token);

		}
	}
}
