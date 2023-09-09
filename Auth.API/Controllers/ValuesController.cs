using Auth.Core.Authentication.Identity;
using Auth.Core.Entities.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;
using static Auth.API.Controllers.ValuesController;

namespace Auth.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ValuesController : ControllerBase
	{
		public class ValuesData
		{
			public int Id { get; set; }
			public string Name { get; set; } = string.Empty;
		}

		[HasPermission(Permission.ViewProduct)]
		[HttpGet("getValues")]
		public async Task<ActionResult<List<ValuesData>>> GetValues()
		{
			return new List<ValuesData>()
			{
				new ValuesData{Id = 1, Name="value1"},
				new ValuesData{Id = 2, Name="value2"},
				new ValuesData{Id = 3, Name="value3"},
			};
		}


		[Authorize]
		[HttpGet("getValue")]
		public async Task<ActionResult<ValuesData>> GetValue()
		{
			return new ValuesData { Id = 1, Name = "value1" };			
		}

	}






}
