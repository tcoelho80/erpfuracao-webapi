using Microsoft.AspNetCore.Identity;
using System;

namespace ERP.Furacao.Infrastructure.Identity.Models
{
    public class IdentityApplicationToken : IdentityUserToken<string>
    {
        public virtual DateTime Created { get; set; }
    }
}
