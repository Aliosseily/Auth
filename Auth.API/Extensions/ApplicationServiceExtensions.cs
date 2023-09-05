using Auth.Core.Interfaces.Repositories;
using Auth.Core.Interfaces.Services;
using Auth.Core.Services;
using Auth.Infrastructure.Persistence;
using Auth.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Auth.API.Extensions
{
	public static class ApplicationServiceExtensions
	{

		public static IServiceCollection AddApplicationServices(this IServiceCollection services,
			IConfiguration config)
		{

			services.AddScoped<IAuthRepository, AuthRepository>();
			services.AddScoped<IAuthService, AuthService>();
			services.AddHttpContextAccessor();

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
