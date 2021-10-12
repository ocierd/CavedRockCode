using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using CavedRockCode.OrderProcessor.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace CavedRockCode.OrderProcessor
{
    public class Program
    {
        public static int Main(string[] args)
        {
            ConfigureLogging();

            try
            {
                Log.Information("Starting Order processor worker");
                CreateHostBuilder(args).Build().Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    string connectionString = hostContext.Configuration.GetConnectionString("Db");
                    services.AddSingleton<IDbConnection>(ServiceProvider => new SqlConnection(connectionString));
                    services.AddSingleton<IInventoryRepository, InventoryRepository>();
                    services.AddHostedService<Worker>();
                })
                .UseSerilog();

        private static void ConfigureLogging()
        {
            string serilogUrl = "http://seq-in-dc:5341";
#if DEBUG
            serilogUrl = "http://host.docker.internal:5341";
#endif
            string name = typeof(Program).Assembly.GetName().Name;
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .Enrich.WithProperty("MachineName", Environment.MachineName)
                .Enrich.WithProperty("assembly", name)
                .WriteTo.Seq(serilogUrl)
                .WriteTo.Console()
                .CreateLogger();
        }
    }
}
