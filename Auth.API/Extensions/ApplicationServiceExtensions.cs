using Auth.Core.Authentication.filters;
using Auth.Core.Authentication.Identity;
using Auth.Core.Interfaces.Authentication.Identity;
using Auth.Core.Interfaces.Repositories;
using Auth.Core.Interfaces.Services;
using Auth.Core.Services;
using Auth.Infrastructure.Persistence;
using Auth.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Auth.API.Extensions
{
	public static class ApplicationServiceExtensions
	{

		public static IServiceCollection AddApplicationServices(this IServiceCollection services,
			IConfiguration config)
		{
			services.AddScoped<IPermissionsService, PermissionService>();
			services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();
			services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
			services.AddScoped<IAuthRepository, AuthRepository>();
			services.AddScoped<IAuthService, AuthService>();
			services.AddHttpContextAccessor();
			services.AddDistributedMemoryCache();

			services.AddDbContext<AuthContext>((serviceProvider, options) =>
			{
				var connectionString = config.GetConnectionString("AuthConnection");
				var serverVersion = ServerVersion.AutoDetect(connectionString);
				options.UseMySql(connectionString, serverVersion);
			});


			return services;
		}

	}
}
