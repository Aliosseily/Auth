using Auth.Core.Exceptions;
using Auth.Core.Interfaces.Authentication.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Core.Authentication.filters
{
	public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
	{
		// we can inject directly the IPermissionsService, beacuse we are going to be registering the
		// permission authorization handler as a singelton 

		private readonly IDistributedCache _distributedCache;
		public PermissionAuthorizationHandler(IDistributedCache distributedCache)
		{
			_distributedCache = distributedCache;
		}

		// this where we are going to take the permission on the permission requirement and see if the currently authenticated user 
		// has this permission and if so then, we can allow him continue the execution and access the Api that he is trying to access

		protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
		{
			// get the id of the currently authenticated user
			string? membrId = context.User.Claims.FirstOrDefault(x => x.Type == "id")?.Value;

			if (!Guid.TryParse(membrId, out Guid parsedMemberId))
			{
				throw new UnauthorizedException("Invalid token", 401);
			}

			var cacheKey = $"Permissions_{parsedMemberId}";
			var cachedPermissions = await _distributedCache.GetStringAsync(cacheKey);

			if (cachedPermissions is not null && cachedPermissions.Contains(requirement.Permission))
			{
				context.Succeed(requirement);
			}

		}
	}
}
