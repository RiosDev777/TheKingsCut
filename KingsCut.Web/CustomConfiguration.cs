using Microsoft.EntityFrameworkCore;
using KingsCut.Web.Data;
using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using KingsCut.Web.Services;
using KingsCut.Web.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace KingsCut.Web
{
    public static class CustomConfiguration
    {
        public static WebApplicationBuilder AddCustomBuilderConfiguration(this WebApplicationBuilder builder)
        {
            // Data Context
            builder.Services.AddDbContext<DataContext>(configuration =>
            {
                configuration.UseSqlServer(builder.Configuration.GetConnectionString("MyConnection"));
            });



            builder.Services.AddNotyf(config =>
            {
                config.DurationInSeconds = 10;
                config.IsDismissable = true;
                config.Position = NotyfPosition.BottomRight;
            });

                //services

                AddServices(builder);

                //Identity and Access Managment
                AddIAM(builder);
                //TOAST NOTIFICATION

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
                //Esta parte en desarrollo no es tan relevante, pero en la parte de despliegue es importante activarlos para generar seguridad en la contraseña
                conf.Password.RequireDigit = false;
                conf.Password.RequiredUniqueChars = 0;
                conf.Password.RequireLowercase = false;
                conf.Password.RequireUppercase = false;
                conf.Password.RequireNonAlphanumeric = false;
                conf.Password.RequiredLength = 0;
            }).AddEntityFrameworkStores<DataContext>() //Almacenado de sesion
            .AddDefaultTokenProviders();   //Aqui iria cualquier tipo de token si se llega a usar

            //Configuracion de la cookie
            builder.Services.ConfigureApplicationCookie(conf =>
            {
                conf.Cookie.Name = "Auth";
                conf.ExpireTimeSpan = TimeSpan.FromDays(5); //tiempo de duracion de la cookies
                conf.LoginPath = "Account/Login";
                conf.AccessDeniedPath = "Account/NotAuthorized";
            });
        }

        public static void AddServices(WebApplicationBuilder builder) 
        { 

            builder.Services.AddScoped<IProductsService, ProductService>();
            builder.Services.AddScoped<IServicesServices, ServiceService>();
        }
               

        public static WebApplication AddCustomWebAppConfiguration(this WebApplication app)
        {
            app.UseNotyf();

            return app;
        }

    }
}
