using Auth.Core.Entities.Authentication;
using Auth.Core.Entities.DTOs;
using Auth.Core.Entities.Identity;
using Auth.Core.Exceptions;
using Auth.Core.Interfaces.Authentication.Identity;
using Auth.Core.Interfaces.Authentication.Jwt;
using Auth.Core.Interfaces.Repositories;
using AutoMapper.Execution;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Infrastructure.Repositories
{
    public class AuthRepository : IAuthRepository
	{

		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;
		private readonly IJwtProvider _jwtProvider;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly IServiceScopeFactory _serviceScopeFactory;


		public AuthRepository(RoleManager<IdentityRole> roleManager,UserManager<AppUser> userManager, IServiceScopeFactory serviceScopeFactory,
			SignInManager<AppUser> signInManager, IJwtProvider jwtProvider, IHttpContextAccessor httpContextAccessor)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_jwtProvider = jwtProvider;
			_httpContextAccessor = httpContextAccessor;
			_serviceScopeFactory = serviceScopeFactory;
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

				if (!Guid.TryParse(loggedInUser.Id, out Guid parsedMemberId))
				{
					throw new NotFoundException("user", loggedInUser.Email);
				}
				using IServiceScope scope = _serviceScopeFactory.CreateScope();
				IPermissionsService permissionsService = scope.ServiceProvider
					.GetRequiredService<IPermissionsService>();

				await permissionsService.GetPermissionAsync(parsedMemberId);

				return res;
			}
			return new UserDto();
		}

		public async Task<UserDto> RefreshToken(string token)
		{
			var userDto = new UserDto();

			// get the user of this token
			var user = await _userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

			if (user is null)
			{
				throw new UnauthorizedException("Invalid Token", 401);
			}

			var refreshToken = user.RefreshTokens.Single(t => t.Token == token);

			if (!refreshToken.IsActive)
			{
				throw new UnauthorizedException("Inactive Token", 401);
			}

			// revoke the old refreshtoken
			refreshToken.RevokedOn = DateTime.UtcNow.ToLocalTime();

			// create new refresh token
			var newRefreshToken = GenerateRefreshToken();

			// add the new refresh token to database as new record
			user.RefreshTokens.Add(newRefreshToken);
			await _userManager.UpdateAsync(user);

			var res = await CreateUserObject(user);

			SetRefreshTokenInCookie(res.RefreshToken, res.RefreshTokenExpiration);

			return res;

		}

		public async Task<UserDto> Register(RegisterDto registerDto)
		{
			if (await _userManager.Users.AnyAsync(x => x.Email == registerDto.Email))
			{
				throw new BadRequestException("Email taken", 400);
			}
			if (await _userManager.Users.AnyAsync(x => x.UserName == registerDto.Username))
			{
				throw new BadRequestException("Username taken", 400);
			}

			var user = new AppUser
			{
				DisplayName = registerDto.DisplayName,
				Email = registerDto.Email,
				UserName = registerDto.Username,
			};

			var result = await _userManager.CreateAsync(user, registerDto.Password);
			if (result.Succeeded)
			{
				var res = await CreateUserObject(user);
				SetRefreshTokenInCookie(res.RefreshToken, res.RefreshTokenExpiration);
				return res;
			}

			throw new BadRequestException("Problem registering user", 400);

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
			if (user.RefreshTokens != null && user.RefreshTokens.Any(t => t.IsActive))
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
