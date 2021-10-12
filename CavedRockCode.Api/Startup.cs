using System.Data;
using System.Data.SqlClient;
using CavedRockCode.Api.Domain;
using CavedRockCode.Api.Integrations;
using CavedRockCode.Api.Interfaces;
using CavedRockCode.Api.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Serilog;
namespace CavedRockCode.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            LogConfigurationDebugView();
            LogSettingsConfiguration();

            services.AddScoped<IProductLogic, ProductLogic>();
            services.AddScoped<IQuickOrderLogic, QuickOrderLogic>();
            services.AddSingleton<IOrderProcessingNotification, OrderProcessingNotification>();


            string connectionString = Configuration.GetConnectionString("Db");
            services.AddScoped<IDbConnection>(ServiceProvider => new SqlConnection(connectionString));
            services.AddScoped<ICavedRepository, CavedRepository>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CavedRockCode.Api", Version = "v1" });
            });


            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<CustomExceptionHandlingMiddleware>();
            //app.UseForwardedHeaders();
            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CavedRockCode.Api v1"));
            }
            app.UseCustomeRequestLogging();

            //app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }


        private void LogConfigurationDebugView()
        {
            string debigView = (Configuration as IConfigurationRoot).GetDebugView();
            Log.ForContext("ConfigurationDebug", debigView)
                .Information("Configuration Dump");
        }
        private void LogSettingsConfiguration()
        {
            var connectionString = Configuration.GetConnectionString("Db");
            string simpleProperty = Configuration.GetValue<string>("SimpleProperty");
            string nestedProperty = Configuration.GetValue<string>("Inventory:NestedProperty");
            Log.ForContext("Connection string", connectionString)
            .ForContext("Simple property", simpleProperty)
            .ForContext("Nested property", nestedProperty)
            .Information("Loaded configuration", connectionString);


        }
    }
}
