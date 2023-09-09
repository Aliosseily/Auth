using Auth.Core.Entities.DTOs;
using Auth.Core.Interfaces.Repositories;
using Auth.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Core.Services
{
	public class AuthService : IAuthService
	{

		private readonly IAuthRepository _repository;

		public AuthService(IAuthRepository repository)
		{
			_repository = repository;
		}


		public async Task<UserDto> Login(LoginDto loginDto)
		{
			return await _repository.Login(loginDto);
		}

		public async Task<bool> Logout()
		{
			return await _repository.Logout();
		}

		public async Task<UserDto> RefreshToken(string refreshToken)
		{
			return await _repository.RefreshToken(refreshToken);
		}

		public async Task<UserDto> Register(RegisterDto registerDto)
		{
			return await _repository.Register(registerDto);
		}
	}
}
