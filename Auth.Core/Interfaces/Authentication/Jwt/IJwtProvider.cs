using Auth.Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Core.Interfaces.Authentication.Jwt
{
    public interface IJwtProvider
    {
        string Generate(AppUser user);
    }
}
