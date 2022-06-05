using System;
using Microsoft.AspNetCore.Identity;
using MediArch.Models;

namespace MediArch.Data
{
    public static class MyIdentityDataInitializer
    {
        public static void SeedData(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);
        }

        public static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            roleManager.CreateRole("Owner");
            roleManager.CreateRole("Moderator");
            roleManager.CreateRole("Medic");
            roleManager.CreateRole("Pacient");
        }

        public static void CreateRole(this RoleManager<IdentityRole> roleManager, string roleName)
        {
            if (roleManager.RoleExistsAsync(roleName).Result)
            {
                return;
            }

            var role = new IdentityRole
            {
                Name = roleName
            };

            var roleResult = roleManager.CreateAsync(role).Result;

            if (!roleResult.Succeeded)
            {
                throw new Exception("Error creating role");
            }
        }

        public static void SeedUsers(UserManager<ApplicationUser> userManager)
        {
            //1 Owner
            if (userManager.FindByNameAsync("Owner@gmail.com").Result == null)
            {
                var user = new ApplicationUser()
                {
                    UserName = "Owner@gmail.com",
                    FirstName = "Gado",
                    LastName = "Moro",
                    BirthDate = new DateTime(1996, 09, 17),
                    Email = "Owner@gmail.com",
                    PhoneNumber = "0750000000",
                    ActiveAccount = true,
                    CreatedDate = new DateTime(2018, 04, 15)
                };

                //user.UserName = user.UserName.Encrypt();
                //user.Email = user.Email.Encrypt();
                user.FirstName = user.FirstName.Encrypt();
                user.LastName = user.LastName.Encrypt();
                user.PhoneNumber = user.PhoneNumber.Encrypt();

                IdentityResult result = userManager.CreateAsync(user, "Owner007!").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Owner").Wait();
                }
            }

            //3 Moderators
            if (userManager.FindByNameAsync("Moderator1@gmail.com").Result == null)
            {
                var user = new ApplicationUser()
                {
                    UserName = "Moderator1@gmail.com",
                    FirstName = "John",
                    LastName = "Sugar",
                    BirthDate = new DateTime(1996, 11, 25),
                    Email = "Moderator1@gmail.com",
                    PhoneNumber = "0750000001",
                    ActiveAccount = true,
                    CreatedDate = new DateTime(2018, 04, 15)
                };

                //user.UserName = user.UserName.Encrypt();
                //user.Email = user.Email.Encrypt();
                user.FirstName = user.FirstName.Encrypt();
                user.LastName = user.LastName.Encrypt();
                user.PhoneNumber = user.PhoneNumber.Encrypt();

                IdentityResult result = userManager.CreateAsync(user, "Moderator007!").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Moderator").Wait();
                }
            }

            if (userManager.FindByNameAsync("Moderator2@gmail.com").Result == null)
            {
                var user = new ApplicationUser()
                {
                    UserName = "Moderator2@gmail.com",
                    FirstName = "Axel",
                    LastName = "Pain",
                    BirthDate = new DateTime(1996, 11, 15),
                    Email = "Moderator2@gmail.com",
                    PhoneNumber = "0750000002",
                    ActiveAccount = true,
                    CreatedDate = new DateTime(2018, 04, 15)
                };

               // user.UserName = user.UserName.Encrypt();
                //user.Email = user.Email.Encrypt();
                user.FirstName = user.FirstName.Encrypt();
                user.LastName = user.LastName.Encrypt();
                user.PhoneNumber = user.PhoneNumber.Encrypt();

                IdentityResult result = userManager.CreateAsync(user, "Moderator007!").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Moderator").Wait();
                }
            }
            
            if (userManager.FindByNameAsync("Moderator3@gmail.com").Result == null)
            {
                var user = new ApplicationUser()
                {
                    UserName = "Moderator3@gmail.com",
                    FirstName = "Robin",
                    LastName = "Cloud",
                    BirthDate = new DateTime(1996, 07, 01),
                    Email = "Moderator3@gmail.com",
                    PhoneNumber = "0750000003",
                    ActiveAccount = true,
                    CreatedDate = new DateTime(2018, 04, 15)
                };

                //user.UserName = user.UserName.Encrypt();
                //user.Email = user.Email.Encrypt();
                user.FirstName = user.FirstName.Encrypt();
                user.LastName = user.LastName.Encrypt();
                user.PhoneNumber = user.PhoneNumber.Encrypt();

                IdentityResult result = userManager.CreateAsync(user, "Moderator007!").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Moderator").Wait();
                }
            }

            //5 Medics
            if (userManager.FindByNameAsync("Medic1@gmail.com").Result == null)
            {
                var user = new ApplicationUser()
                {
                    UserName = "Medic1@gmail.com",
                    FirstName = "Larry",
                    LastName = "Smith",
                    BirthDate = new DateTime(1986, 12, 18),
                    Email = "Medic1@gmail.com",
                    PhoneNumber = "0751000000",
                    Title = "Cardiologist",
                    CabinetAddress = "str. Decebal, Bl 374, Iasi",
                    ActiveAccount = true,
                    CreatedDate = new DateTime(2018, 05, 15)
                };

                //user.UserName = user.UserName.Encrypt();
                //user.Email = user.Email.Encrypt();
                user.FirstName = user.FirstName.Encrypt();
                user.LastName = user.LastName.Encrypt();
                user.PhoneNumber = user.PhoneNumber.Encrypt();
                user.Title = user.Title.Encrypt();
                user.CabinetAddress = user.CabinetAddress.Encrypt();

                IdentityResult result = userManager.CreateAsync(user, "Medic007!").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Medic").Wait();
                }
            }

            if (userManager.FindByNameAsync("Medic2@gmail.com").Result == null)
            {
                var user = new ApplicationUser()
                {
                    UserName = "Medic2@gmail.com",
                    FirstName = "Jessica",
                    LastName = "Adams",
                    BirthDate = new DateTime(1986, 07, 23),
                    Email = "Medic2@gmail.com",
                    PhoneNumber = "0751000001",
                    Title = "Allergist",
                    CabinetAddress = "str. Decebal, Bl 373, Iasi",
                    ActiveAccount = true,
                    CreatedDate = new DateTime(2018, 05, 15)
                };

                //user.UserName = user.UserName.Encrypt();
                //user.Email = user.Email.Encrypt();
                user.FirstName = user.FirstName.Encrypt();
                user.LastName = user.LastName.Encrypt();
                user.PhoneNumber = user.PhoneNumber.Encrypt();
                user.Title = user.Title.Encrypt();
                user.CabinetAddress = user.CabinetAddress.Encrypt();

                IdentityResult result = userManager.CreateAsync(user, "Medic007!").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Medic").Wait();
                }
            }

            if (userManager.FindByNameAsync("Medic3@gmail.com").Result == null)
            {
                var user = new ApplicationUser()
                {
                    UserName = "Medic3@gmail.com",
                    FirstName = "Sarah",
                    LastName = "Moore",
                    BirthDate = new DateTime(1986, 10, 29),
                    Email = "Medic3@gmail.com",
                    PhoneNumber = "0751000002",
                    Title = "Dermatologist",
                    CabinetAddress = "str. Decebal, Bl 372, Vaslui",
                    ActiveAccount = true,
                    CreatedDate = new DateTime(2018, 05, 15)
                };

               // user.UserName = user.UserName.Encrypt();
                //user.Email = user.Email.Encrypt();
                user.FirstName = user.FirstName.Encrypt();
                user.LastName = user.LastName.Encrypt();
                user.PhoneNumber = user.PhoneNumber.Encrypt();
                user.Title = user.Title.Encrypt();
                user.CabinetAddress = user.CabinetAddress.Encrypt();

                IdentityResult result = userManager.CreateAsync(user, "Medic007!").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Medic").Wait();
                }
            }

            if (userManager.FindByNameAsync("Medic4@gmail.com").Result == null)
            {
                var user = new ApplicationUser()
                {
                    UserName = "Medic4@gmail.com",
                    FirstName = "Jason",
                    LastName = "Walker",
                    BirthDate = new DateTime(1986, 06, 25),
                    Email = "Medic4@gmail.com",
                    PhoneNumber = "0751000003",
                    Title = "General Surgeons",
                    CabinetAddress = "str. Decebal, Bl 371, Piatra Neamt",
                    ActiveAccount = true,
                    CreatedDate = new DateTime(2018, 05, 15)
                };

                //user.UserName = user.UserName.Encrypt();
                //user.Email = user.Email.Encrypt();
                user.FirstName = user.FirstName.Encrypt();
                user.LastName = user.LastName.Encrypt();
                user.PhoneNumber = user.PhoneNumber.Encrypt();
                user.Title = user.Title.Encrypt();
                user.CabinetAddress = user.CabinetAddress.Encrypt();

                IdentityResult result = userManager.CreateAsync(user, "Medic007!").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Medic").Wait();
                }
            }

            if (userManager.FindByNameAsync("Medic5@gmail.com").Result == null)
            {
                var user = new ApplicationUser()
                {
                    UserName = "Medic5@gmail.com",
                    FirstName = "Susan",
                    LastName = "Lee",
                    BirthDate = new DateTime(1986, 10, 29),
                    Email = "Medic5@gmail.com",
                    PhoneNumber = "0751000004",
                    Title = "Family Physician",
                    CabinetAddress = "str. Decebal, Bl 370, Vaslui",
                    ActiveAccount = true,
                    CreatedDate = new DateTime(2018, 05, 15)
                };

                //user.UserName = user.UserName.Encrypt();
                //user.Email = user.Email.Encrypt();
                user.FirstName = user.FirstName.Encrypt();
                user.LastName = user.LastName.Encrypt();
                user.PhoneNumber = user.PhoneNumber.Encrypt();
                user.Title = user.Title.Encrypt();
                user.CabinetAddress = user.CabinetAddress.Encrypt();

                IdentityResult result = userManager.CreateAsync(user, "Medic007!").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Medic").Wait();
                }
            }
            if (userManager.FindByNameAsync("Medic6@gmail.com").Result == null)
            {
                var user = new ApplicationUser()
                {
                    UserName = "Medic6@gmail.com",
                    FirstName = "Lara",
                    LastName = "Anderson",
                    BirthDate = new DateTime(1985, 10, 29),
                    Email = "Medic6@gmail.com",
                    PhoneNumber = "0751000005",
                    Title = "Family Physician",
                    CabinetAddress = "str. Traian, Bl 234, Bucuresti",
                    ActiveAccount = true,
                    CreatedDate = new DateTime(2018, 05, 25)
                };

                //user.UserName = user.UserName.Encrypt();
                //user.Email = user.Email.Encrypt();
                user.FirstName = user.FirstName.Encrypt();
                user.LastName = user.LastName.Encrypt();
                user.PhoneNumber = user.PhoneNumber.Encrypt();
                user.Title = user.Title.Encrypt();
                user.CabinetAddress = user.CabinetAddress.Encrypt();

                IdentityResult result = userManager.CreateAsync(user, "Medic007!").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Medic").Wait();
                }
            }
            //10 Pacients
            if (userManager.FindByNameAsync("Pacient1@gmail.com").Result == null)
            {
                var user = new ApplicationUser()
                {
                    UserName = "Pacient1@gmail.com",
                    FirstName = "Mark",
                    LastName = "Taylor",
                    BirthDate = new DateTime(1996, 12, 19),
                    Email = "Pacient1@gmail.com",
                    PhoneNumber = "0752000000",
                    ActiveAccount = true,
                    CreatedDate = new DateTime(2018, 05, 16)
                };

                //user.UserName = user.UserName.Encrypt();
                //user.Email = user.Email.Encrypt();
                user.FirstName = user.FirstName.Encrypt();
                user.LastName = user.LastName.Encrypt();
                user.PhoneNumber = user.PhoneNumber.Encrypt();

                IdentityResult result = userManager.CreateAsync(user, "Pacient007!").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Pacient").Wait();
                }
            }

            if (userManager.FindByNameAsync("Pacient2@gmail.com").Result == null)
            {
                var user = new ApplicationUser()
                {
                    UserName = "Pacient2@gmail.com",
                    FirstName = "James",
                    LastName = "Lee",
                    BirthDate = new DateTime(1996, 06, 01),
                    Email = "Pacient2@gmail.com",
                    PhoneNumber = "0752000001",
                    ActiveAccount = true,
                    CreatedDate = new DateTime(2018, 05, 16)
                };

                //user.UserName = user.UserName.Encrypt();
                //user.Email = user.Email.Encrypt();
                user.FirstName = user.FirstName.Encrypt();
                user.LastName = user.LastName.Encrypt();
                user.PhoneNumber = user.PhoneNumber.Encrypt();

                IdentityResult result = userManager.CreateAsync(user, "Pacient007!").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Pacient").Wait();
                }
            }

            if (userManager.FindByNameAsync("Pacient3@gmail.com").Result == null)
            {
                var user = new ApplicationUser()
                {
                    UserName = "Pacient3@gmail.com",
                    FirstName = "James",
                    LastName = "Clark",
                    BirthDate = new DateTime(1996, 02, 04),
                    Email = "Pacient3@gmail.com",
                    PhoneNumber = "0752000002",
                    ActiveAccount = true,
                    CreatedDate = new DateTime(2018, 05, 16)
                };

                //user.UserName = user.UserName.Encrypt();
                //user.Email = user.Email.Encrypt();
                user.FirstName = user.FirstName.Encrypt();
                user.LastName = user.LastName.Encrypt();
                user.PhoneNumber = user.PhoneNumber.Encrypt();

                IdentityResult result = userManager.CreateAsync(user, "Pacient007!").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Pacient").Wait();
                }
            }

            if (userManager.FindByNameAsync("Pacient4@gmail.com").Result == null)
            {
                var user = new ApplicationUser()
                {
                    UserName = "Pacient4@gmail.com",
                    FirstName = "Mary",
                    LastName = "Hall",
                    BirthDate = new DateTime(1996, 01, 13),
                    Email = "Pacient4@gmail.com",
                    PhoneNumber = "0752000003",
                    ActiveAccount = true,
                    CreatedDate = new DateTime(2018, 05, 16)
                };

                //user.UserName = user.UserName.Encrypt();
                //user.Email = user.Email.Encrypt();
                user.FirstName = user.FirstName.Encrypt();
                user.LastName = user.LastName.Encrypt();
                user.PhoneNumber = user.PhoneNumber.Encrypt();

                IdentityResult result = userManager.CreateAsync(user, "Pacient007!").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Pacient").Wait();
                }
            }

            if (userManager.FindByNameAsync("Pacient5@gmail.com").Result == null)
            {
                var user = new ApplicationUser()
                {
                    UserName = "Pacient5@gmail.com",
                    FirstName = "Linda",
                    LastName = "Young",
                    BirthDate = new DateTime(1996, 10, 27),
                    Email = "Pacient5@gmail.com",
                    PhoneNumber = "0752000004",
                    ActiveAccount = true,
                    CreatedDate = new DateTime(2018, 06, 25)
                };

                //user.UserName = user.UserName.Encrypt();
                //user.Email = user.Email.Encrypt();
                user.FirstName = user.FirstName.Encrypt();
                user.LastName = user.LastName.Encrypt();
                user.PhoneNumber = user.PhoneNumber.Encrypt();

                IdentityResult result = userManager.CreateAsync(user, "Pacient007!").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Pacient").Wait();
                }
            }

            if (userManager.FindByNameAsync("Pacient6@gmail.com").Result == null)
            {
                var user = new ApplicationUser()
                {
                    UserName = "Pacient6@gmail.com",
                    FirstName = "Laura",
                    LastName = "Wilson",
                    BirthDate = new DateTime(1995, 12, 31),
                    Email = "Pacient6@gmail.com",
                    PhoneNumber = "0752000005",
                    ActiveAccount = true,
                    CreatedDate = new DateTime(2018, 06, 25)
                };

                //user.UserName = user.UserName.Encrypt();
                //user.Email = user.Email.Encrypt();
                user.FirstName = user.FirstName.Encrypt();
                user.LastName = user.LastName.Encrypt();
                user.PhoneNumber = user.PhoneNumber.Encrypt();

                IdentityResult result = userManager.CreateAsync(user, "Pacient007!").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Pacient").Wait();
                }
            }

            if (userManager.FindByNameAsync("Pacient7@gmail.com").Result == null)
            {
                var user = new ApplicationUser()
                {
                    UserName = "Pacient7@gmail.com",
                    FirstName = "Daniel",
                    LastName = "Bryan",
                    BirthDate = new DateTime(1996, 12, 30),
                    Email = "Pacient7@gmail.com",
                    PhoneNumber = "0752000006",
                    ActiveAccount = true,
                    CreatedDate = new DateTime(2018, 06, 25)
                };

                //user.UserName = user.UserName.Encrypt();
                //user.Email = user.Email.Encrypt();
                user.FirstName = user.FirstName.Encrypt();
                user.LastName = user.LastName.Encrypt();
                user.PhoneNumber = user.PhoneNumber.Encrypt();

                IdentityResult result = userManager.CreateAsync(user, "Pacient007!").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Pacient").Wait();
                }
            }

            if (userManager.FindByNameAsync("Pacient8@gmail.com").Result == null)
            {
                var user = new ApplicationUser()
                {
                    UserName = "Pacient8@gmail.com",
                    FirstName = "Linda",
                    LastName = "Carter",
                    BirthDate = new DateTime(1996, 04, 16),
                    Email = "Pacient8@gmail.com",
                    PhoneNumber = "0752000007",
                    ActiveAccount = true,
                    CreatedDate = new DateTime(2018, 06, 25)
                };

                //user.UserName = user.UserName.Encrypt();
               // user.Email = user.Email.Encrypt();
                user.FirstName = user.FirstName.Encrypt();
                user.LastName = user.LastName.Encrypt();
                user.PhoneNumber = user.PhoneNumber.Encrypt();

                IdentityResult result = userManager.CreateAsync(user, "Pacient007!").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Pacient").Wait();
                }
            }

            if (userManager.FindByNameAsync("Pacient9@gmail.com").Result == null)
            {
                var user = new ApplicationUser()
                {
                    UserName = "Pacient9@gmail.com",
                    FirstName = "Donald",
                    LastName = "Wilson",
                    BirthDate = new DateTime(1996, 10, 26),
                    Email = "Pacient9@gmail.com",
                    PhoneNumber = "0752000008",
                    ActiveAccount = true,
                    CreatedDate = new DateTime(2018, 06, 25)
                };

                //user.UserName = user.UserName.Encrypt();
                //user.Email = user.Email.Encrypt();
                user.FirstName = user.FirstName.Encrypt();
                user.LastName = user.LastName.Encrypt();
                user.PhoneNumber = user.PhoneNumber.Encrypt();

                IdentityResult result = userManager.CreateAsync(user, "Pacient007!").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Pacient").Wait();
                }
            }

            if (userManager.FindByNameAsync("Pacient10@gmail.com").Result == null)
            {
                var user = new ApplicationUser()
                {
                    UserName = "Pacient10@gmail.com",
                    FirstName = "William",
                    LastName = "Smith",
                    BirthDate = new DateTime(1996, 03, 19),
                    Email = "Pacient10@gmail.com",
                    PhoneNumber = "0752000009",
                    ActiveAccount = true,
                    CreatedDate = new DateTime(2018, 06, 15)
                };

                //user.UserName = user.UserName.Encrypt();
                //user.Email = user.Email.Encrypt();
                user.FirstName = user.FirstName.Encrypt();
                user.LastName = user.LastName.Encrypt();
                user.PhoneNumber = user.PhoneNumber.Encrypt();

                IdentityResult result = userManager.CreateAsync(user, "Pacient007!").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Pacient").Wait();
                }
            }

        }
    }
}
