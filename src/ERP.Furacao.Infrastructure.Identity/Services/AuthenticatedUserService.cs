using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using ERP.Furacao.Application.Services;

namespace ERP.Furacao.Infrastructure.Identity.Services
{
    public class AuthenticatedUserService : IAuthenticatedUserService
    {
        public AuthenticatedUserService(IHttpContextAccessor httpContextAccessor)
        {
            UserId = httpContextAccessor.HttpContext?.User?.FindFirstValue("uid");
        }

        public string UserId { get; }
    }
}
