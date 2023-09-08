using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Core.Authentication.Identity
{
	public class HasPermissionAttribute : AuthorizeAttribute
	{
		// policy: permission is ti specify the policy that is required on the AuthorizeAttribute
		// so we are going to use this permission to specify a policy that needs to be fulfilled for an authenticated uder 
		// to be able to pass authorization
		public HasPermissionAttribute(/*string permission*/ Permission permission)
			: base(policy: permission.ToString())
		{

		}
	}
}
