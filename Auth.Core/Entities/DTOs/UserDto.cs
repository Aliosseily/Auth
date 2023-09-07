using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Auth.Core.Entities.DTOs
{
	public class UserDto // this is the class we return to user after loggedIn or register
	{
		public string DisplayName { get; set; } = string.Empty;
		public string Username { get; set; } = string.Empty;
		public string Token { get; set; } = string.Empty;
		public string Image { get; set; } = string.Empty;

		// To prevent return this property with the response
		[JsonIgnore]
		public string? RefreshToken { get; set; }
		public DateTime RefreshTokenExpiration { get; set; }


	}
}
