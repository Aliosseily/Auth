using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Core.Authentication.filters
{
	public class PermissionAuthorizationPolicyProvider : DefaultAuthorizationPolicyProvider
	{
		public PermissionAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options)
			: base(options)
		{

		}
		// We are going to override this method to see if a policy with this policyName already exists 
		// the policyName is going to come from our HasPermissionAttribute 

		// This is hpw we can define automatically missing policies for when ypi define new permission enum values and you don't 
		// even have to think about it
		public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
		{
			var policy = await base.GetPolicyAsync(policyName);
			if (policy is not null)
			{
				return policy;
			}

			// this is going to handle building our authorization policy and the next time this method is called, our authorization
			// policy is already going to be exist and we don't need to create a new one 
			return new AuthorizationPolicyBuilder()
					.AddRequirements(new PermissionRequirement(policyName)).Build();

		}
	}
}
