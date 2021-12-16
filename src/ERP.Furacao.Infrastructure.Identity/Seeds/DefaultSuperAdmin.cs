using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;
using ERP.Furacao.Application.Enums;
using ERP.Furacao.Infrastructure.Identity.Models;

namespace ERP.Furacao.Infrastructure.Identity.Seeds
{
    public static class DefaultSuperAdmin
    {
        public static async Task SeedAsync(UserManager<IdentityApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Default User
            var defaultUser = new IdentityApplicationUser
            {
                UserName = "superadmin",
                Email = "superadmin@gmail.com",
                FirstName = "Mukesh",
                LastName = "Murugan",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };

            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "123Pa$$word!");
                    await userManager.AddToRoleAsync(defaultUser, RoleEnum.Basic.ToString());
                    await userManager.AddToRoleAsync(defaultUser, RoleEnum.Moderator.ToString());
                    await userManager.AddToRoleAsync(defaultUser, RoleEnum.Admin.ToString());
                    await userManager.AddToRoleAsync(defaultUser, RoleEnum.SuperAdmin.ToString());
                }

            }
        }
    }
}
