using AutoMapper;
using SecretSanta.Business;
using SecretSanta.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Data.Sqlite;

using Microsoft.Extensions.Configuration;
using SecretSanta.Business.Services;

namespace SecretSanta.Web
{
    public class Startup
    {
        private IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.EnableSensitiveDataLogging()
                       .UseSqlite(Configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IGiftService, GiftService>();
            services.AddScoped<IGroupService, GroupService>();

            System.Type profileType = typeof(AutomapperConfigurationProfile);
            System.Reflection.Assembly assembly = profileType.Assembly;
            services.AddAutoMapper(new[] { assembly });

            // services.AddMvc(opts => opts.EnableEndpointRouting = false);
            services.AddControllers();

            services.AddSwaggerDocument();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2007:Consider calling ConfigureAwait on the awaited task", Justification = "<Pending>")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>")]
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseOpenApi();
            //http://localhost/swagger
            app.UseSwaggerUi3();

            app.UseEndpoints(endpoint =>
            {
                endpoint.MapDefaultControllerRoute();
            });
        }
    }
}
