﻿using Microsoft.EntityFrameworkCore;
using KingsCut.Web.Data;
using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using KingsCut.Web.Services;
using KingsCut.Web.Data.Entities;
using Microsoft.AspNetCore.Identity;
using KingsCut.Web.Data.Seeders;
using KingsCut.Web.Helper;

namespace KingsCut.Web
{
    public static class CustomConfiguration
    {
        public static WebApplicationBuilder AddCustomBuilderConfiguration(this WebApplicationBuilder builder)
        {
            // Data Context
            builder.Services.AddDbContext<DataContext>(configuration =>
            {
                configuration.UseSqlServer(builder.Configuration.GetConnectionString("LocalConnection"));
            });

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddNotyf(config =>
            {
                config.DurationInSeconds = 10;
                config.IsDismissable = true;
                config.Position = NotyfPosition.BottomRight;
            });

            //Services
            AddServices(builder);

            //Identity and Access Management
            AddIAM(builder);

            //PAM: Privileged access Management

            //Toast Notification
            builder.Services.AddNotyf(config => 
            { 
                config.DurationInSeconds = 10; 
                config.IsDismissable = true; 
                config.Position = NotyfPosition.BottomRight; 
            });

            return builder;

        }

        private static void AddIAM(WebApplicationBuilder builder)
        {
            builder.Services.AddIdentity<User, IdentityRole>(conf =>
            {
                conf.User.RequireUniqueEmail = true;
                conf.Password.RequireDigit = false;
                conf.Password.RequiredUniqueChars = 0;
                conf.Password.RequireLowercase = false;
                conf.Password.RequireUppercase = false;
                conf.Password.RequireNonAlphanumeric = false;
                conf.Password.RequiredLength = 4;
            }).AddEntityFrameworkStores<DataContext>() 
            .AddDefaultTokenProviders();   

            builder.Services.ConfigureApplicationCookie(conf =>
            {
                conf.Cookie.Name = "Auth";
                conf.ExpireTimeSpan = TimeSpan.FromDays(5); 
                conf.LoginPath = "/Account/Login";
                conf.AccessDeniedPath = "/Account/NotAuthorized";
            });
        }

        public static void AddServices(WebApplicationBuilder builder) 
        { 
            //Services
            builder.Services.AddScoped<IProductsService, ProductService>();
            builder.Services.AddScoped<IRolesService, RolesService>();
            builder.Services.AddScoped<IServicesServices, ServiceService>();
            builder.Services.AddTransient<SeedDb>();
            builder.Services.AddScoped<IUsersService, UsersService>();


            // Helpers
            builder.Services.AddScoped<ICombosHelper, CombosHelper>();
            builder.Services.AddScoped<IConverterHelper, ConverterHelper>();
        }
               

        public static WebApplication AddCustomWebAppConfiguration(this WebApplication app)
        {
            app.UseNotyf();

            SeedData(app);

            return app;
        }

        private static void SeedData(WebApplication app)
        {
            IServiceScopeFactory scopeFactory = app.Services.GetService<IServiceScopeFactory>();

            using (IServiceScope scope = scopeFactory.CreateScope())
            {
                SeedDb service = scope.ServiceProvider.GetService<SeedDb>();
                service!.SeedAsync().Wait();
            }
        }
    }
}
