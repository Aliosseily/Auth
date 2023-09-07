using Auth.Core.Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Core.Interfaces.Services
{
	public interface IAuthService
	{
		Task<UserDto> Login(LoginDto loginDto);
		Task<UserDto> Register(RegisterDto registerDto);
		Task<UserDto> RefreshToken(string refreshToken);
	}
}
