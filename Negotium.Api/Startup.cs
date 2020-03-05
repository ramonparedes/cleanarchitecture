using System.Data.SqlClient;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Negotium.Api.Helpers;
using Negotium.Common;
using Negotium.Common.Interface;
using Negotium.Repository;
using Negotium.Service;

namespace Negotium.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddControllers().AddControllersAsServices();

            var appSettingsSection = Configuration.GetSection("AppSettings");
            var connectionStrings = Configuration.GetSection("ConnectionStrings");

            services.Configure<AppSettings>(appSettingsSection);
            services.Configure<ConnectionString>(connectionStrings);

            var appSettings = appSettingsSection.Get<AppSettings>();
            var connections = connectionStrings.Get<ConnectionString>();


            var conexion = connectionStrings.Get<ConnectionString>();
            var secrect = appSettings.Secret;

            services.AddDbContext<ApplicationContext>(options =>
            {
                var connection = new SqlConnection(connections.DefaultConnection);

                options.UseSqlServer(connection);
            });

            services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(conexion.DefaultConnection));
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped(typeof(IUnitOfWork),typeof(UnitOfWork));
   
            services.AddTransient<IUserService, UserService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
