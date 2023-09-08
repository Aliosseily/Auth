using Auth.Core.Entities.Identity;
using Auth.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Auth.Core.Authentication.Jwt;
using Auth.Core.Interfaces.Authentication.Jwt;
using Auth.Core.Authentication.Identity;
using Auth.Core.Interfaces.Authentication.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Auth.API.Extensions
{
    public static class IdentityServiceExtentions
	{
		public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
		{
			services.AddIdentity<AppUser, IdentityRole>(opt =>
			{
				opt.Password.RequireNonAlphanumeric = false;
			})
			.AddEntityFrameworkStores<AuthContext>() // this registers our users store and role stores with our application
			.AddSignInManager<SignInManager<AppUser>>();

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:SecretKey"]));

			services.AddAuthentication(options => {
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
			})
				.AddJwtBearer(option =>
				{
					option.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuerSigningKey = true,
						IssuerSigningKey = key,
						ValidateIssuer= false,
						ValidateAudience= false,
						// jwt give the token a small duration after expiration to use it, so after token is expired jwt give it a short
						// time we can use it before preventing us
						// using ClockSkew we till jwt at the moment token is expired, if you got a request with expired token you
						// return unauthorized
						ClockSkew = TimeSpan.Zero
					};
				});


			services.AddScoped<IJwtProvider, JwtProvider>();

			return services;
		}


	}
}
