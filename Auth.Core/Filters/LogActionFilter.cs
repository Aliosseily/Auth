using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Core.Filters
{
	public class LogActionFilter : IActionFilter
	{
		public void OnActionExecuted(ActionExecutedContext context)
		{
			var username = context.HttpContext.User.Identity.Name;
			var actionName = context.HttpContext.Request.Method;

			var log = Log
				.ForContext("Username", username)
				.ForContext("ActionName", actionName);

			if (context.Exception != null)
			{
				Log.Error(context.Exception, "User: {Username}, Action: {ActionName}, Endpoint: {Endpoint}",
				username, actionName, context.HttpContext.Request.Path);
			}
			else
			{
				log.Information("User: {Username}, Action: {ActionName}, Endpoint: {Endpoint}",
					username, actionName, context.HttpContext.Request.Path);
			}

		}

		public void OnActionExecuting(ActionExecutingContext context)
		{
		}
	}
}
