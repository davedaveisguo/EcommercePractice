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
            // if user not exist
            if (!userManager.Users.Any())
            {
                var user = new AppUser
                {
                    DisplayName = "Bob",
                    Email = "bob@gmail.com",
                    UserName = "yuhua@hotmail.com",
                    Address = new Address
                    {
                        FirstName = "Bob",
                        LastName = "Guo",
                        Street = "99 the stree",
                        City = "Winsor",
                        State = "ON",
                        ZipCode = "3242432"
                    }


                };
                await userManager.CreateAsync(user, "Pa$$w0rd");
            }

        }
    }
}