using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Core.Exceptions
{
	public class NotFoundException : ApplicationException
	{
		public NotFoundException(string name, object key)
			: base($"The Entity {name} with identifier {key} was not found.")
		{
		}
	}
}
