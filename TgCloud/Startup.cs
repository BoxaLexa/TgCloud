using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TgCloud
{
    /// <summary>
    /// Startup класс конфигурации хоста
    /// </summary>
    public class Startup
    {
        private readonly IConfigurationRoot configuration;

        /// <summary>
        /// Создает Startup
        /// </summary>
        /// <param name="configuration">конфигурация из config.json</param>
        public Startup(IConfigurationRoot configuration)
        {
            this.configuration = configuration;
        }

        /// <summary>
        /// Регистрирует сервисы в DI контейнер
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {

            var dbHost = configuration["DataBase:Host"];
            var dbPort = configuration["DataBase:Port"];
            var dbUsername = configuration["DataBase:UserName"];
            var dbPassword = configuration["DataBase:Password"];
            services.AddSingleton(configuration);
            services.AddCors();
            services.AddMvc(options =>
            {
                options.EnableEndpointRouting = false;
            });
            services.AddControllers();
            services.AddDbContext<ApplicationDbContext>(o =>
            {
                var aspNetDataBaseName = configuration["DataBase:AspNetDataBaseName"];
                o.UseNpgsql($"Host={dbHost};Port={dbPort};Database={aspNetDataBaseName};Username={dbUsername};Password={dbPassword}", o => o.EnableRetryOnFailure(2, TimeSpan.FromSeconds(10), new[] { "5" }));
            });

            services.AddSwaggerGen(options =>
            {
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });
        }

        /// <summary>
        /// Конфигурирует хост авторизации
        /// </summary>
        /// <param name="app"></param>
        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseCors(o => o.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMvc();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI();
        }
    }
}
