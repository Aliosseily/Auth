using Auth.Core.Entities.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Core.Interfaces.Repositories
{
	public interface IAuthRepository
	{
		Task<UserDto>Login(LoginDto loginDto);
		Task<UserDto> Register(RegisterDto registerDto);
		Task<UserDto> GetCurrentUser();
		Task<UserDto> RefreshToken(string refreshToken);

	}
}
