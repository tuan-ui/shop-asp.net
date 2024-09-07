namespace Data.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Data.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Validation;
    using System.Diagnostics;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Data.ShopDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Data.ShopDbContext context)
        {
            //CreateUser(context);
            CreatePage(context);
            CreateContactDetail(context);
            CreateConfigTitle(context);
        }
        //private void CreateUser(Data.ShopDbContext context)
        //{
        //    var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ShopDbContext()));

        //    var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ShopDbContext()));

        //    var user = new ApplicationUser()
        //    {
        //        UserName = "tuan",
        //        Email = "lethanhtuan900@gmail.com",
        //        EmailConfirmed = true,
        //        BirthDay = DateTime.Now,
        //        FullName = "Le Thanh Tuan"

        //    };
        //    if (manager.Users.Count(x => x.UserName == "tuan") == 0)
        //    {
        //        manager.Create(user, "123456");

        //        if (!roleManager.Roles.Any())
        //        {
        //            roleManager.Create(new IdentityRole { Name = "Admin" });
        //            roleManager.Create(new IdentityRole { Name = "User" });
        //        }

        //        var adminUser = manager.FindByEmail("lethanhtuan900@gmail.com");

        //        manager.AddToRoles(adminUser.Id, new string[] { "Admin", "User" });
        //    }

        //}
        private void CreatePage(ShopDbContext context)
        {
            if (context.Pages.Count() == 0)
            {
                try
                {
                    var page = new Page()
                    {
                        Name = "Giới thiệu",
                        Alias = "gioi-thieu",
                        Content = @"Lê Thanh Tuấn, luận văn tốt nghiệp ",
                        Status = true

                    };
                    context.Pages.Add(page);
                    context.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var eve in ex.EntityValidationErrors)
                    {
                        Trace.WriteLine($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation error.");
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Trace.WriteLine($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"");
                        }
                    }
                }

            }

        }
        private void CreateContactDetail(ShopDbContext context)
        {
            if (context.ContactDetails.Count() == 0)
            {
                try
                {
                    var contactDetail = new Data.Models.ContactDetail()
                    {
                        Name = "Trường Đại học công nghệ Sài Gòn",
                        Address = "180 Cao Lỗ, p4, q8",
                        Email = "lethanhtuan900@gmail.com",
                        Lat = 10.7377526,
                        Lng = 106.6760432,
                        Phone = "0366494098",
                        Website = "https://www.stu.edu.vn",
                        Other = "",
                        Status = true

                    };
                    context.ContactDetails.Add(contactDetail);
                    context.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var eve in ex.EntityValidationErrors)
                    {
                        Trace.WriteLine($"Entity of type \"{eve.Entry.Entity.GetType().Name}\" in state \"{eve.Entry.State}\" has the following validation error.");
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Trace.WriteLine($"- Property: \"{ve.PropertyName}\", Error: \"{ve.ErrorMessage}\"");
                        }
                    }
                }

            }
        }
        private void CreateConfigTitle(ShopDbContext context)
        {
            if (!context.SystemConfigs.Any(x => x.Code == "HomeTitle"))
            {
                context.SystemConfigs.Add(new SystemConfig()
                {
                    Code = "HomeTitle",
                    ValueString = "Trang chủ TuanPC",

                });
            }
            if (!context.SystemConfigs.Any(x => x.Code == "HomeMetaKeyword"))
            {
                context.SystemConfigs.Add(new SystemConfig()
                {
                    Code = "HomeMetaKeyword",
                    ValueString = "Trang chủ TuanPC",

                });
            }
            if (!context.SystemConfigs.Any(x => x.Code == "HomeMetaDescription"))
            {
                context.SystemConfigs.Add(new SystemConfig()
                {
                    Code = "HomeMetaDescription",
                    ValueString = "Trang chủ TuanPC",

                });
            }
        }
    }
}