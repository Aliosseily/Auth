using Auth.Core.Entities.Identity;
using Auth.Core.Interfaces.Authentication.Identity;
using Auth.Core.Interfaces.Authentication.Jwt;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Auth.Core.Authentication.Identity
{
	public class PermissionService : IPermissionsService
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly IDistributedCache _distributedCache;


		public PermissionService(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager,
				IDistributedCache distributedCache)
		{
			_userManager = userManager;

			_roleManager = roleManager;
			_distributedCache = distributedCache;
		}

		public async Task<HashSet<string>> GetPermissionAsync(Guid userId)
		{

			var cacheKey = $"Permissions_{userId}";
			//ar cachedPermissions = await _distributedCache.GetStringAsync(cacheKey);

			//if (cachedPermissions is not null)
			//{
			//	return JsonSerializer.Deserialize<HashSet<string>>(cachedPermissions);
			//}


			var user = await _userManager.FindByIdAsync(userId.ToString());

			if (user == null)
			{
				throw new ArgumentException("User not found");
			}

			var roles = await _userManager.GetRolesAsync(user);
			var permissions = new HashSet<string>();


			foreach (var roleName in roles)
			{
				var role = await _roleManager.FindByNameAsync(roleName);
				if (role != null)
				{
					var permissionsList = _roleManager.GetClaimsAsync(role).Result.Select(c => c.Value).ToHashSet<string>();
					permissions.UnionWith(permissionsList);
				}
			}

			var serializedPermissions = JsonSerializer.Serialize(permissions);
			//var cacheOptions = new DistributedCacheEntryOptions
			//{
			//	AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30) // Cache for 30 minutes
			//};

			await _distributedCache.SetStringAsync(cacheKey, serializedPermissions);

			return permissions;
		}
	}
}
