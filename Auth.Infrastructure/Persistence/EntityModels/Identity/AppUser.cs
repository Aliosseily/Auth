using Auth.Infrastructure.Persistence.EntityModels.Authentication;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Infrastructure.Persistence.EntityModels.Identity
{
	public class AppUser : IdentityUser
	{
		public string DisplayName { get; set; } = string.Empty;
		public List<RefreshToken>? RefreshTokens { get; set; }
	}
}
