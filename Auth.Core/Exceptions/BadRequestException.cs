using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Core.Exceptions
{
	public class BadRequestException : ApplicationException
	{
		public int StatusCode { get; }
		public BadRequestException(string message, int statusCode)
			: base(message)
		{
			StatusCode = statusCode;
		}

		public BadRequestException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
