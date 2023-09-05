using Auth.Core.Entities.Authentication;
using Auth.Core.Entities.DTOs;
using Auth.Core.Entities.Identity;
using Auth.Core.Exceptions;
using Auth.Core.Interfaces.Authentication;
using Auth.Core.Interfaces.Repositories;
using AutoMapper.Execution;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Infrastructure.Repositories
{
	public class AuthRepository : IAuthRepository
	{

		private readonly UserManager<AppUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly SignInManager<AppUser> _signInManager;
		private readonly IJwtProvider _jwtProvider;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public AuthRepository(RoleManager<IdentityRole> roleManager,UserManager<AppUser> userManager,
			SignInManager<AppUser> signInManager, IJwtProvider jwtProvider, IHttpContextAccessor httpContextAccessor)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_roleManager = roleManager;
			_jwtProvider = jwtProvider;
			_httpContextAccessor = httpContextAccessor;
		}

		public Task<UserDto> GetCurrentUser()
		{
			throw new NotImplementedException();
		}

		public async Task<UserDto> Login(LoginDto loginDto)
		{
			var loggedInUser = await _userManager.FindByEmailAsync(loginDto.Email);
			if (loggedInUser == null)
			{
				throw new NotFoundException(nameof(UserDto), loginDto.Email);
			}
			// the third parameter false, is what to do in case of failure, should we lock the user out, NO.
			var result = await _signInManager.CheckPasswordSignInAsync(loggedInUser, loginDto.Password, false);

			if (result.Succeeded)
			{
				var res = await CreateUserObject(loggedInUser);
				if (!string.IsNullOrEmpty(res.RefreshToken))
				{
					SetRefreshTokenInCookie(res.RefreshToken, res.RefreshTokenExpiration);
				}

				return res;
			}
			return new UserDto();
		}

		public Task<UserDto> RefreshToken()
		{
			throw new NotImplementedException();
		}

		public Task<UserDto> Register(RegisterDto registerDto)
		{
			throw new NotImplementedException();
		}



		private async Task<UserDto> CreateUserObject(AppUser user)
		{
			var res = new UserDto
			{
				DisplayName = user.DisplayName,
				Image = "This will be a user image",
				Token = _jwtProvider.Generate(user),
				Username = user.UserName,
			};
			if (user.RefreshTokens.Any(t => t.IsActive))
			{
				var activeRefreshToken = user.RefreshTokens.FirstOrDefault(t => t.IsActive);
				res.RefreshToken = activeRefreshToken?.Token;
				res.RefreshTokenExpiration = activeRefreshToken.ExpiresOn;
			}
			else
			{
				var refreshToken = GenerateRefreshToken();
				res.RefreshToken = refreshToken?.Token;
				res.RefreshTokenExpiration = refreshToken.ExpiresOn;
				user.RefreshTokens.Add(refreshToken);
				await _userManager.UpdateAsync(user);
			}

			return res;


		}

		private RefreshToken GenerateRefreshToken()
		{
			var randomNumber = new byte[32];

			using var generator = new RNGCryptoServiceProvider();

			generator.GetBytes(randomNumber);

			return new RefreshToken
			{
				Token = Convert.ToBase64String(randomNumber),
				ExpiresOn = DateTime.UtcNow.ToLocalTime().AddDays(4), // refresh token will be expired after 4 days
				CreatedOn = DateTime.UtcNow.ToLocalTime()
			};
		}

		private void SetRefreshTokenInCookie(string refreshToken, DateTime expires)
		{
			var cookieOptions = new CookieOptions
			{
				HttpOnly = true,
				Expires = expires.ToLocalTime(), // expiration expires date same as token expiration date
			};
			var httpContext = _httpContextAccessor.HttpContext;
			if (httpContext is not null)
			{
				httpContext.Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
			}

		}


	}
}
