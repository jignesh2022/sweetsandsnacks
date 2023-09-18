using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Serilog;
using SweetsAndSnacks.Data;
using SweetsAndSnacks.Middlewares;
using SweetsAndSnacks.Models;

namespace SweetsAndSnacks
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);           
            
            var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .Enrich.FromLogContext()
                .CreateLogger();
            builder.Logging.ClearProviders();
            builder.Logging.AddSerilog(logger);
                       

            var connectionstring = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<DBContext>(options => {
                options.UseMySql(connectionstring, ServerVersion.AutoDetect(connectionstring));
             
            });

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 8;             
                //options.Password.RequiredUniqueChars = 2;
            })
            .AddEntityFrameworkStores<DBContext>();

            builder.Services.ConfigureApplicationCookie(options =>
                {
                    options.LoginPath = "/Account/Home/Login";
                    options.AccessDeniedPath = "/Account/Home/AccessDenied";
                }
            );

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddTransient<ExceptionHandlingMiddleware>();
                        

            var app = builder.Build();

            
            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            //app.UseStatusCodePagesWithReExecute("/Home/Error");
            

            app.UseHttpsRedirection();
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en-IN")
                
            });
            app.UseStaticFiles();

            app.UseAuthentication();          

            app.UseRouting();

            app.UseAuthorization();

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            /*app.MapControllerRoute(
                name: "administration",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );*/

            app.MapControllerRoute(
                name: "account",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            

            app.Run();
        }
    }
}