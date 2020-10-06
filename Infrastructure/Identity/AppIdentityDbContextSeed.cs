using System.Linq;
using System.Threading.Tasks;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUsersAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user =new AppUser
                {
                    DisplayName = "Mahmoud",
                    Email = "mahmoud@test.com",
                    UserName = "mahmoud@test.com",
                    Address = new Address
                    {
                        FirstName = "Mahmoud",
                        LastName = "Elmaghraby",
                        Street = "10th street ",
                        City = "Abu Hammad",
                        State = "Ash Sharqia",
                        Zipcode ="90210"
                    }
                };

                await userManager.CreateAsync(user, "Pa$$w0rd");
            }
        }
    }
}