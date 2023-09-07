using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Core.Services.Background
{
	public class CachePermissionsAtStartup : BackgroundService
	{
		protected override Task ExecuteAsync(CancellationToken stoppingToken)
		{
			throw new NotImplementedException();
		}
	}
}
