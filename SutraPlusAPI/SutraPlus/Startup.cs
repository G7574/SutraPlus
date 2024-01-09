using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog.Extensions.Logging;
//using Serilog;
//using Serilog.Events;
//using Serilog.Formatting.Json;
using SutraPlus.Filters;
using SutraPlus_DAL.Data;
using SutraPlus_DAL.Models;
using System.Text;

namespace SutraPlus
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
           // Log.Logger = new LoggerConfiguration()
           //.MinimumLevel.Warning()
           //.MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
           //.MinimumLevel.Override("System", LogEventLevel.Warning)
           //.MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
           //.Enrich.FromLogContext()
           //.WriteTo.RollingFile(new JsonFormatter(), "logs/log-{Date}.txt")
           //.CreateLogger();
            //Schedul.Configuration = configuration;
            ////JobScheduler.Start();
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                };
            });
            //app.UseAuthentication();
            //services.AddNodeServices();
            // Add converter to DI 
            //services.AddSingleton(typeof(IConverter), new BasicConverter(new PdfTools()));
            // Add service and create Policy with options 
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                  builder => builder.AllowAnyMethod()
                                    .AllowAnyHeader()
                                    .WithHeaders("authorization", "accept", "content-type", "origin")
                                    .AllowCredentials()
                                    .SetIsOriginAllowed(_ => true));
            });
            services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
                c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            });
            services.AddResponseCompression();
            services.AddDataProtection();
            services.AddRouting();
            services.AddResponseCompression();
            services.AddDataProtection();
            services.AddHttpContextAccessor();
            services.AddSingleton(new DBContext(new SqlDataAccess(Configuration.GetSection("ConnectionStrings:DefaultConnection").Value)));
            services.AddDbContext<MasterDBContext>(options =>
            options.UseSqlServer(Configuration.GetSection("ConnectionStrings:DefaultConnection").Value).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
            , ServiceLifetime.Transient); // 
            //TODO: inject logger 

            //services.AddMvc(options =>
            //{

            //    options.Filters.Add<RuntimeManagerFilter>();
            //})
            //    .AddJsonOptions(options =>
            //    {
            //        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            //    })
            //    .SetCompatibilityVersion(CompatibilityVersion.Latest);
            services.AddMvc(options =>
            {
                options.Filters.Add<RuntimeManagerFilter>();
            })
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            })
            .SetCompatibilityVersion(CompatibilityVersion.Latest);


            // Get options from app settings
            //var jwtAppSettingOptions = Configuration.GetSection(nameof(JwtIssuerOptions));

            // Configure JwtIssuerOptions 
            //services.AddLogging(loggingBuilder =>
            //{
            //    loggingBuilder.ClearProviders();
            //    loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
            //    loggingBuilder.AddNLog();
            //});
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider svp)
        {
            //Utilities.HostingEnvironment = env;
            //System.Web.HttpContext.Configure(app.ApplicationServices.GetRequiredService<Microsoft.AspNetCore.Http.IHttpContextAccessor>());

            //app.UseCors("AllowAll");
            app.UseCors(b => b.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                IdentityModelEventSource.ShowPII = true;
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                //c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            });
            //app.UseAuthentication();

            //app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            //app.UseEnyimMemcached();

            //CacheFactory.Instance.Initalize(LogFactory, Configuration);

            app.UseAuthentication();
            app.UseResponseCompression();

            app.UseCors("AllowAll");
            app.UseRouting();
            //app.UseAuthorization();
            //app.MapControllers();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
             name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            //Helper.EmailApi.Initialize(env, Configuration, LogFactory);
        }
    }
}
