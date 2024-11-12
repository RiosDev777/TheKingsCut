using Microsoft.EntityFrameworkCore;
using KingsCut.Web.Data;
using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using KingsCut.Web.Services;


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



            builder.Services.AddNotyf(config =>
            {
                config.DurationInSeconds = 10;
                config.IsDismissable = true;
                config.Position = NotyfPosition.BottomRight;
            });

                //services

                AddServices(builder);

<<<<<<< Updated upstream
                //TOAST NOTIFICATION
=======
                //Identity and Access Managment
                AddIAM(builder);
>>>>>>> Stashed changes

            //PAM: Privileged access Management


                //TOAST NOTIFICATION
                builder.Services.AddNotyf(config => 
                { 
                    config.DurationInSeconds = 10; 
                    config.IsDismissable = true; 
                    config.Position = NotyfPosition.BottomRight; 
                });

            return builder;

        }

        public static void AddServices(WebApplicationBuilder builder) 
        { 

            builder.Services.AddScoped<IProductsService, ProductService>();
            builder.Services.AddScoped<IUsersServices, UserService>();
        }
               

        public static WebApplication AddCustomWebAppConfiguration(this WebApplication app)
        {
            app.UseNotyf();

            return app;
        }

    }
}
