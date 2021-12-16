using Microsoft.AspNetCore.Identity;

namespace ERP.Furacao.Application.DTOs.Account
{
    public class ApplicationUserRequest : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}