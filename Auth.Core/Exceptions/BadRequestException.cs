﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Core.Exceptions
{
	public class BadRequestException : ApplicationException
	{
		public BadRequestException(string message)
			: base(message)
		{
		}

		public BadRequestException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
