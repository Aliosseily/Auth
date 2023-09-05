using Auth.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Auth.API.Extensions
{
	public static class ApplicationServiceExtensions
	{

		public static IServiceCollection AddApplicationServices(this IServiceCollection services,
			IConfiguration config)
		{
			services.AddDbContext<AuthContext>((serviceProvider, options) =>
			{
				var connectionString = config.GetConnectionString("AuthConnection");
				var serverVersion = ServerVersion.AutoDetect(connectionString);
				options.UseMySql(connectionString, serverVersion);
			});

			//services.AddTransient<ExceptionMiddleware>();

			return services;
		}

	}
}
