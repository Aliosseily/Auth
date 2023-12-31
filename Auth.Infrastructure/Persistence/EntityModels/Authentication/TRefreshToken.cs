﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Infrastructure.Persistence.EntityModels.Authentication
{
	// Owned means that this table is owned by another table (AppUser)
	// this will also add an autoincrement primary key Id to the table
	[Owned]
	public class TRefreshToken
	{
		public string Token { get; set; } = string.Empty;
		public DateTime ExpiresOn { get; set; }
		public DateTime CreatedOn { get; set; }
		public DateTime? RevokedOn { get; set; }
	}
}
