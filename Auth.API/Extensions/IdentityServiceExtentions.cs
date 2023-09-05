using Auth.Core.Entities.Identity;
using Auth.Core.Interfaces.Authentication;
using Auth.Infrastructure.Authentication;
using Auth.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;

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

			services.AddScoped<IJwtProvider, JwtProvider>();

			return services;
		}


	}
}
