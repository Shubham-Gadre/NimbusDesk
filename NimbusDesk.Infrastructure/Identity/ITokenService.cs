using System;
using System.Collections.Generic;
using System.Text;

namespace NimbusDesk.Infrastructure.Identity
{
    public interface ITokenService
    {
        string GenerateJwtToken(ApplicationUser user, IList<string> roles);
    }
}
