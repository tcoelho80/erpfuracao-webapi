using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace ERP.Furacao.Infrastructure.Identity.Models
{
    public class IdentityApplicationUser : IdentityUser
    {
        public IdentityApplicationUser()
        {
            RefreshTokens = new List<IdentityRefreshToken<string>>();
            Claims = new List<IdentityUserClaim<string>>();
            Logins = new List<IdentityUserLogin<string>>();
            Tokens = new List<IdentityApplicationToken>();
            Roles = new List<IdentityUserRole<string>>();
        }

        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual ICollection<IdentityRefreshToken<string>> RefreshTokens { get; set; }
        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }
        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }
        public virtual ICollection<IdentityApplicationToken> Tokens { get; set; }
        public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }
    }
}
