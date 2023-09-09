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

				// Define a dictionary to map exception types to status codes
				var exceptionStatusCodes = new Dictionary<Type, int>
				{
					{ typeof(UnauthorizedException), (int)HttpStatusCode.Unauthorized },
					{ typeof(NotFoundException), (int)HttpStatusCode.NotFound },
					{ typeof(BadRequestException), (int)HttpStatusCode.BadRequest },
				};

				int statusCode = (int)HttpStatusCode.InternalServerError; // Default status code

				// Check if the exception type is in the dictionary
				if (exceptionStatusCodes.ContainsKey(ex.GetType()))
				{
					statusCode = exceptionStatusCodes[ex.GetType()];
				}

				var response = _env.IsDevelopment()
					? new AppException(statusCode, ex.Message, ex.StackTrace?.ToString() ?? string.Empty)
					: new AppException(statusCode, "Server Error");

				var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
				var json = JsonSerializer.Serialize(response, options);

				context.Response.StatusCode = statusCode;
				await context.Response.WriteAsync(json);
			}
		}
	}
}
