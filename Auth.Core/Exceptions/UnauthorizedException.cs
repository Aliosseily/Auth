using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Core.Exceptions
{
	public class UnauthorizedException: ApplicationException
	{
		public int StatusCode { get; }

		public UnauthorizedException(string message)
			:base(message)
		{

		}

		public UnauthorizedException(string message, int statusCode)
			: base(message)
		{
			StatusCode = statusCode;
		}

		public UnauthorizedException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
