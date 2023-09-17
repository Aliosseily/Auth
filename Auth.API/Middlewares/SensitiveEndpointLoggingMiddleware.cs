using Auth.Core.CustomAttributes;
using Microsoft.AspNetCore.Mvc.Controllers;
using Serilog;
using System.Security.Claims;

namespace Auth.API.Middlewares
{
	public class SensitiveEndpointLoggingMiddleware
	{
		private readonly RequestDelegate _next;

		public SensitiveEndpointLoggingMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			var username = context.User.Identity.Name;
			var actionName = context.Request.Method;
			// Check if the LogSensitiveActionAttribute is applied to the action method
			if (IsActionToBeLogged(context))
			{
				// Enrich the log event with custom properties
				var log = Log
					.ForContext("Username", username)
					.ForContext("ActionName", actionName);

				log.Information("User: {Username}, Action: {ActionName}, Endpoint: {Endpoint}",
					username, actionName, context.Request.Path);				
			}
			try
			{
				await _next(context);
			}
			catch (Exception ex)
			{
				//Log.Error(ex, "Error Occured!");
				Log.Error(ex, "User: {Username}, Action: {ActionName}, Endpoint: {Endpoint}",
				username, actionName, context.Request.Path);
			}

		}

		private bool IsActionToBeLogged(HttpContext context)
		{
			var endpoint = context.GetEndpoint();
			if (endpoint != null)
			{
				var controllerActionDescriptor = endpoint.Metadata.GetMetadata<ControllerActionDescriptor>();
				if (controllerActionDescriptor != null)
				{
					return controllerActionDescriptor.EndpointMetadata.OfType<LogSensitiveActionAttribute>().Any();
				}
			}
			return false;
		}


	}
}
