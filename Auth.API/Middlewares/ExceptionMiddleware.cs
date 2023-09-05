using Auth.Core.Exceptions;
using System.Net;
using System.Text.Json;

namespace Auth.API.Middlewares
{
	public class ExceptionMiddleware: IMiddleware
	{
		private readonly ILogger<ExceptionMiddleware> _logger;
		private readonly IHostEnvironment _env;

		public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
		{
			_logger = logger;
			_env = env;
		}

		public async Task InvokeAsync(HttpContext context, RequestDelegate next)
		{
			try
			{
				await next(context);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, ex.Message);
				context.Response.ContentType = "application/json";
				context.Response.StatusCode = (int)HttpStatusCode.InternalServerError; // this will set status code to 500

				// if we are in development modfe then we need to end back the full exception
				// with stack trace details
				var response = _env.IsDevelopment()
					? new AppException(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString() ?? string.Empty)
					: new AppException(context.Response.StatusCode, "Server Error");

				// this to ensure that our response json will be in camleCase TitleCase or PascalCase
				var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };


				var json = JsonSerializer.Serialize(response, options);

				context.Response.ContentType = "application/json";

				await context.Response.WriteAsync(json);

			}
		}
	}
}
