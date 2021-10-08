using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Events;
using System.Linq;

namespace CavedRockCode.Api
{
    public static class ConfigureRequestLogging
    {

        public static IApplicationBuilder UseCustomeRequestLogging(this IApplicationBuilder builder)
        {
            return builder.UseSerilogRequestLogging(options =>
            {
                options.GetLevel = ExcludedHealthChecks;
                options.EnrichDiagnosticContext = (IDiagnosticContext diagnosticContext, HttpContext context) =>
                {   
                    diagnosticContext.Set("RequestHost",context.Request.Host.Value);
                    diagnosticContext.Set("UserAgent",context.Request.Headers["User-Agent"]);
                };
            });

        }

        private static LogEventLevel ExcludedHealthChecks(HttpContext context, double _, Exception exception)
        => exception != null || context.Response.StatusCode > 499 ? LogEventLevel.Error
        : IsHealthCkeckEndpoint(context) ? LogEventLevel.Verbose : LogEventLevel.Information;


        private static bool IsHealthCkeckEndpoint(HttpContext context)
        {
            var userAgent = context.Request.Headers["User-Agent"].FirstOrDefault() ?? string.Empty;
            return context.Request.Path.Value.EndsWith("health", StringComparison.InvariantCultureIgnoreCase) ||
            userAgent.Contains("HealthCheck", StringComparison.InvariantCultureIgnoreCase);

        }
    }

}