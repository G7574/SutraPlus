using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using NLog.Web;
using System.Text;

namespace SutraPlus
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // NLog: setup the Logger first to catch all errors
            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            //var builder = WebApplication.CreateBuilder(args);
            //var app = builder.Build();
            try
            {

                logger.Debug("init main");
                //builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
                //{
                //    options.RequireHttpsMetadata = false;
                //    options.SaveToken = true;
                //    options.TokenValidationParameters = new TokenValidationParameters()
                //    {
                //        ValidateIssuer = true,
                //        ValidateAudience = true,
                //        ValidAudience = builder.Configuration["Jwt:Audience"],
                //        ValidIssuer = builder.Configuration["Jwt:Issuer"],
                //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                //    };
                //});

                //app.UseAuthentication();
                CreateWebHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                //NLog: catch setup errors
                logger.Error(ex, "Stopped program because of exception");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                NLog.LogManager.Shutdown();
            }
        }


        //shivaji
        //public static IWebHostBuilder CreateWebHostBuilder(string[] args) => WebHost.CreateDefaultBuilder(args)
        //.UseStartup<Startup>()
        //.ConfigureLogging(logging =>
        //{
        //    logging.ClearProviders();
        //    logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
        //})
        //.UseNLog();  // NLog: setup NLog for Dependency injection


        //pratik change
        public static IHostBuilder CreateWebHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(webBuilder =>
    {
        webBuilder.UseStartup<Startup>();
    })
    .ConfigureLogging(logging =>
    {
        logging.ClearProviders();
        logging.SetMinimumLevel(LogLevel.Trace);
    })
        .UseNLog();

    }
}