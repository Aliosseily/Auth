using Auth.Core.Entities.Authentication;
using Auth.Infrastructure.Persistence.EntityModels.Authentication;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Infrastructure.MappingConfig
{
	public class MappingDbToModel: Profile
	{
		public MappingDbToModel()
		{
			SourceMemberNamingConvention = new PascalCaseNamingConvention();
			DestinationMemberNamingConvention = new PascalCaseNamingConvention();
			AllowNullCollections = true;


			MapRefreshToken();
		}


		private void MapRefreshToken()
		{
			CreateMap<TRefreshToken, RefreshToken>();
		}


	}
}
