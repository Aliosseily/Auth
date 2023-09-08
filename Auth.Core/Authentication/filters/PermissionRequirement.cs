﻿using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Core.Authentication.filters
{
	public class PermissionRequirement : IAuthorizationRequirement
	{
		//public string Permission { get; private set; }
		public string Permission { get; }


		public PermissionRequirement(string permission)
		{
			Permission = permission;
		}
	}
}
