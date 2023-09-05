using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Core.Entities.DTOs
{
	public class RegisterDto
	{
		[Required]
		public string DisplayName { get; set; } = string.Empty;

		[Required]
		public string Username { get; set; } = string.Empty;

		[Required]
		[EmailAddress]
		public string Email { get; set; } = string.Empty;

		[Required]
		[RegularExpression("(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{4,8}$", ErrorMessage = "Password must be complex")]
		public string Password { get; set; } = string.Empty;
	}
}
