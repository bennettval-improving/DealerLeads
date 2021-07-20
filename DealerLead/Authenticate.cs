using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DealerLead.Entities;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.EntityFrameworkCore;

namespace DealerLead
{
    public static class Authenticate
    {
        private static readonly DealerLeadDbContext _context;

        static Authenticate()
        {
            _context = new DealerLeadDbContext();
        }

        public static async Task OnTokenValidate(TokenValidatedContext context)
        {
            var user = context.Principal;
            if (user != null && user.Claims != null && user.Claims.Count() > 0)
            {
                var oidString = user.Claims.FirstOrDefault(x => x.Type.Equals("http://schemas.microsoft.com/identity/claims/objectidentifier"))?.Value;
                var oid = Guid.Parse(oidString);
                var userExists = await _context.DealerLeadUser.AnyAsync(x => x.AzureADId.Equals(oid));
                if (!userExists && !oid.Equals(Guid.Empty)) 
                {
                    _context.Add(new DealerLeadUser { AzureADId = oid });
                    await _context.SaveChangesAsync();
                }
            }

            await Task.CompletedTask.ConfigureAwait(false);
        }

        public static Guid GetOid(ClaimsPrincipal user)
        {
            if (user != null && user.Claims != null && user.Claims.Count() > 0)
            {
                var oid = user.Claims.FirstOrDefault(x => x.Type.Equals("http://schemas.microsoft.com/identity/claims/objectidentifier")).Value;
                return Guid.Parse(oid);
            }
            else
                return Guid.Empty;
        }
    }
}
