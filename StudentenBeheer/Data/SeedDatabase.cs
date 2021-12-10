using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudentenBeheer.Areas.Identity.Data;
using StudentenBeheer.Models;

namespace StudentenBeheer.Data
{

    public static class SeedDatabase
    {

        public static void Initialize(IServiceProvider serviceProvider, UserManager<ApplicationUser> userManager)
        {
            using (var context = new ApplicationContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<ApplicationContext>>()))
            {
                ApplicationUser user = null;
                context.Database.EnsureCreated();

                if (!context.Users.Any())
                {
                    user = new ApplicationUser
                    {
                        UserName = "Admin",
                        Firstname = "Jaimy",
                        Lastname = "Van Audenhove",
                        Email = "System.administrator@studentenbeheer.be",
                        EmailConfirmed = true
                    };

                    userManager.CreateAsync(user, "AdminJaimy123_");
                }

                if (!context.Roles.Any())
                {

                    context.Roles.AddRange(

                            new IdentityRole { Id = "User", Name = "User", NormalizedName = "user" },
                            new IdentityRole { Id = "Admin", Name = "Admin", NormalizedName = "admin" }

                            );

                    context.SaveChanges();
                }


                if (!context.Gender.Any() || !(context.Student.Any()))
                {
                    context.Gender.AddRange(

                       new Gender
                       {

                           ID = 'M',
                           Name = "Male"
                       },

                       new Gender
                       {

                           ID = 'F',
                           Name = "Female"
                       },

                       new Gender
                       {
                           ID = 'X',
                           Name = "Unspecified"
                       }

                   );
                    context.SaveChanges();

                    context.Student.AddRange(

                               new Student
                               {
                                   Name = "Test",
                                   Lastname = "Test",
                                   Birthday = DateTime.Now,
                                   GenderId = 'X',
                                   Deleted = DateTime.MaxValue


                               },
                               new Student
                               {
                                   Name = "Karen",
                                   Lastname = "Oudaert",
                                   Birthday = DateTime.Now,
                                   GenderId = 'F',
                                   Deleted = DateTime.MaxValue


                               }
                        );
                    context.SaveChanges();

                }

                if (!context.Module.Any())
                {
                    context.Module.AddRange(

                    new Module
                    {
                        Name = "Programmeren",
                        Omschrijving = "Programmeren is het schrijven van een computerprogramma, een concrete reeks instructies die een computer kan uitvoeren. Dit is de taak van een softwareontwikkelaar of programmeur. Programmeren wordt in het algemeen niet direct in machinetaal gedaan, maar in een programmeertaal. De programmacode die wordt geschreven heet broncode en wordt door een assembler, compiler of interpreter omgezet in machinecode. Met name in het verleden werd voor programmeren ook coderen gebruikt.",
                        Deleted = DateTime.MaxValue
                    },
                     new Module
                     {
                         Name = "Talen",
                         Omschrijving = "Taal is een middel tot communicatie waarmee met een woordenschat en grammaticaregels een boodschap kan worden overgebracht. Deze boodschap kan gesproken zijn of via tekens (geschreven taal en gebarentaal). De term taal kan betrekking hebben op het systeem als geheel waarvan de tekens de individuele bouwstenen vormen, of op slechts een of enkele van de tekens afzonderlijk.",
                         Deleted = DateTime.Now
                     }

                    );
                    context.SaveChanges();
                }

                if (user != null)
                {
                    context.UserRoles.AddRange(

                        new IdentityUserRole<string> { UserId = user.Id, RoleId = "Admin" }
                        );

                    context.SaveChanges();
                }
            }
        }
    }

}

