using Auth.Core.Entities.DTOs;
using Auth.Core.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Auth.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{

		private readonly IAuthService _service;

		public AuthController(IAuthService service)
		{
			_service = service;
		}

		[HttpPost("login")]
		public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
		{
			return await _service.Login(loginDto);
		}

		[HttpPost("register")]
		public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
		{
			return await _service.Register(registerDto);
		}

		[HttpGet("refreshToken")]
		public async Task<ActionResult<UserDto>> RefreshToken()
		{
			var refreshToken = Request.Cookies["refreshToken"];
			return await _service.RefreshToken(refreshToken);
		}


		[HttpPost("logout")]
		public async Task<ActionResult<bool>> Logout()
		{
			return await _service.Logout();
		}



	}
}
