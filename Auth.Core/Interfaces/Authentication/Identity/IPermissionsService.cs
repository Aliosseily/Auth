using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Core.Interfaces.Authentication.Identity
{
    public interface IPermissionsService
    {
        Task<HashSet<string>> GetPermissionAsync(Guid userId);
    }
}
