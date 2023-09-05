using Auth.Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Infrastructure.Persistence
{
	public class AuthContext: IdentityDbContext<AppUser>
	{

		public AuthContext(DbContextOptions<AuthContext> options)
			:base(options)
		{

		}


		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			builder.Entity<AppUser>().ToTable("Users");
			builder.Entity<IdentityRole>().ToTable("Roles");
			builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
			builder.Entity<IdentityUserClaim<string>>().ToTable("UserPermissions");
			builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
			builder.Entity<IdentityRoleClaim<string>>().ToTable("RolePermissions");
			builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");
		}
	}
}
