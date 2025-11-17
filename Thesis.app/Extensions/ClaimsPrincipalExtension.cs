using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Thesis.app.Extensions
{
    public static class ClaimsPrincipalExtension
    {
        public static int Id(this ClaimsPrincipal user)
        {
            return Int32.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        }
    }
}
