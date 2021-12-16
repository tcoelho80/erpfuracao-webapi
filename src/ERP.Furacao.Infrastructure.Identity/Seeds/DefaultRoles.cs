using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using ERP.Furacao.Application.Enums;

namespace ERP.Furacao.Infrastructure.Identity.Seeds
{
    public static class DefaultRoles
    {
        public static async Task SeedAsync(RoleManager<IdentityRole> roleManager)
        {
            //Seed Roles
            await roleManager.CreateAsync(new IdentityRole(RoleEnum.SuperAdmin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(RoleEnum.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(RoleEnum.Moderator.ToString()));
            await roleManager.CreateAsync(new IdentityRole(RoleEnum.Basic.ToString()));
        }
    }
}
