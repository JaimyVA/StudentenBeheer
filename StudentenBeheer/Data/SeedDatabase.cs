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
                //ApplicationUser user = null;
                context.Database.EnsureCreated();

                ApplicationUser Beheerder = null;
                ApplicationUser Docent1 = null;
                ApplicationUser Student1 = null;

                if (!context.Users.Any())
                {
                    ApplicationUser dummy = new ApplicationUser { Id = "-", Firstname = "-", Lastname = "-", UserName = "-", Email = "?@?.?"};
                    context.Users.Add(dummy);
                    context.SaveChanges();

                    Beheerder = new ApplicationUser
                    {
                        UserName = "Beheerder",
                        Firstname = "Jaimy",
                        Lastname = "Van Audenhove",
                        Email = "System.administrator@studentenbeheer.be",
                        EmailConfirmed = true
                    };

                    Docent1 = new ApplicationUser
                    {
                        UserName = "Docent1",
                        Firstname = "Meneer",
                        Lastname = "Docent",
                        Email = "System.User@studentenbeheer.be",
                        EmailConfirmed = true
                    };

                    Student1 = new ApplicationUser
                    {
                        UserName = "Student1",
                        Firstname = "Student",
                        Lastname = "Test",
                        Email = "System.User@studentenbeheer.be",
                        EmailConfirmed = true
                    };

                    userManager.CreateAsync(Beheerder, "Abc!12345");
                    userManager.CreateAsync(Docent1, "Abc!12345");
                    userManager.CreateAsync(Student1, "Abc!12345");
                }

                if (!context.Roles.Any())
                {
                    context.Roles.AddRange(

                            new IdentityRole { Id = "Beheerder", Name = "Beheerder", NormalizedName = "beheerder" },
                            new IdentityRole { Id = "Docent", Name = "Docent", NormalizedName = "docent" },
                            new IdentityRole { Id = "Student", Name = "Student", NormalizedName = "student" }
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
                                   Name = "Jaimy",
                                   Lastname = "Van Audenhove",
                                   Birthday = DateTime.Now,
                                   GenderId = 'M',
                                   UserId = Student1.Id,
                                   Deleted = DateTime.MaxValue
                               },
                               new Student
                               {
                                   Name = "Karen",
                                   Lastname = "Oudaert",
                                   Birthday = DateTime.Now,
                                   GenderId = 'F',
                                   Deleted = DateTime.Now
                               }
                        );
                    context.SaveChanges();

                }

                if (!context.Docent.Any())
                {
                    context.Docent.AddRange(

                          new Docent
                          {
                              FirstName = "Docent1",
                              LastName = "Docent1",
                              Birthday = DateTime.Now,
                              GenderId = 'M',
                              UserId = Docent1.Id,
                              Email = "Docent1@ehb.be",
                              DeletedAt = DateTime.MaxValue
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

                if (!context.Docenten_modules.Any())
                {
                    context.Docenten_modules.AddRange(

                        new Docenten_modules
                        {
                            ModuleId = 1,
                            DocentId = 1
                        });
                }

                if (Beheerder != null && Docent1 != null && Student1 != null)
                {
                    context.UserRoles.AddRange(

                        new IdentityUserRole<string> { UserId = Beheerder.Id, RoleId = "Beheerder" },
                        new IdentityUserRole<string> { UserId = Docent1.Id, RoleId = "Docent" },
                        new IdentityUserRole<string> { UserId = Student1.Id, RoleId = "Student" }

                        );

                    context.SaveChanges();
                }
            }
        }
    }

}

