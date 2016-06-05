using GogoKit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Linq;

namespace Tickets
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IViagogoClient, ViagogoClient>(provider =>
            {
                return new ViagogoClient(
                    Configuration["ViagogoAPI:ClientID"],
                    Configuration["ViagogoAPI:ClientSecret"],
                    new System.Net.Http.Headers.ProductHeaderValue("Tickets"));
            });

            services.AddMvc(options =>
            {
                var jsonOutputFormater = (JsonOutputFormatter)options
                    .OutputFormatters
                    .FirstOrDefault(formatter => formatter is JsonOutputFormatter)
                    ?? new JsonOutputFormatter(new JsonSerializerSettings());

                jsonOutputFormater.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

                options.OutputFormatters.RemoveType<JsonOutputFormatter>();
                options.OutputFormatters.Add(jsonOutputFormater);
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseStatusCodePagesWithReExecute("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
