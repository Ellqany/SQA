using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace SQA.Models.Identity
{
    public class IdentitySeedData
    {
        public static async void EnsurePopulated(IApplicationBuilder app)
        {
            string Password = "@ALm2123Pq";
            UserManager<User> userManager = app.ApplicationServices
            .GetRequiredService<UserManager<User>>();
            List<User> users = new List<User>
            {
                new User()
                {
                    Name = "Ahmed",
                    UserName = "Ahmed",
                    Email = "Ahmed@yahoo.com",
                    Gender = "Male",
                    Department = "zxcas"
                },
                new User()
                {
                    Name = "Ali",
                    UserName = "Ali",
                    Email = "Ali@yahoo.com",
                    Gender = "Male",
                    Department = "zxcas"
                },
                new User()
                {
                    Name = "Hassan",
                    UserName = "Hassan",
                    Email = "Hassan@yahoo.com",
                    Gender = "Male",
                    Department = "zxcas"
                },
                new User()
                {
                    Name = "Yahia",
                    UserName = "Yahia",
                    Email = "Yahia@yahoo.com",
                    Gender = "Male",
                    Department = "zxcasd1"
                },
                new User()
                {
                    Name = "Hady",
                    UserName = "Hady",
                    Email = "Hady@yahoo.com",
                    Gender = "Male",
                    Department = "zxcasd2"
                },
                new User()
                {
                    Name = "Ayaad",
                    UserName = "Ayaad",
                    Email = "Ayaad@yahoo.com",
                    Gender = "Male",
                    Department = "zxcasd3"
                },
                new User()
                {
                    Name = "Badran",
                    UserName = "Badran",
                    Email = "Badran@yahoo.com",
                    Gender = "Male",
                    Department = "zxcasd3"
                },
                new User()
                {
                    Name = "Ahmed Ali",
                    UserName = "AhmedAli",
                    Email = "AhmedAli@yahoo.com",
                    Gender = "Male",
                    Department = "zxcas"
                },
                new User()
                {
                    Name = "Ali Hassan",
                    UserName = "AliHassan",
                    Email = "AliHassan@yahoo.com",
                    Gender = "Male",
                    Department = "zxcas"
                },
                new User()
                {
                    Name = "Hossam",
                    UserName = "Hossam",
                    Email = "Hossam@yahoo.com",
                    Gender = "Male",
                    Department = "zxcas"
                },
                new User()
                {
                    Name = "Shady",
                    UserName = "Shady",
                    Email = "Shady@yahoo.com",
                    Gender = "Male",
                    Department = "zxcasd1"
                },
                new User()
                {
                    Name = "Adel",
                    UserName = "Adel",
                    Email = "Adel@yahoo.com",
                    Gender = "Male",
                    Department = "zxcasd2"
                },
                new User()
                {
                    Name = "Nagy",
                    UserName = "Nagy",
                    Email = "Nagy@yahoo.com",
                    Gender = "Male",
                    Department = "zxcasd3"
                },
                new User()
                {
                    Name = "Hamdy",
                    UserName = "Hamdy",
                    Email = "Hamdy@yahoo.com",
                    Gender = "Male",
                    Department = "zxcasd3"
                }
            };
            foreach (var user in users)
            {
                IdentityResult result = await userManager.CreateAsync(user, Password);
            }
        }

    }
}
