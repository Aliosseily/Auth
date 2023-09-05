using Auth.Core.Entities.Authentication;
using Microsoft.AspNetCore.Identity;


namespace Auth.Core.Entities.Identity
{
	public class AppUser : IdentityUser
	{
		public string DisplayName { get; set; } = string.Empty;
		public List<RefreshToken>? RefreshTokens { get; set; }
	}
}
