using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Navicon.Mdm.ExternalServices.Configuration;
using Navicon.Mdm.ExternalServices.Configuration.Enumerations;
using Navicon.Mdm.ExternalServices.Infrastructure.DaData.Options;
using Navicon.Mdm.ExternalServices.Web.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using NLog;
using NLog.Web;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");

try
{
    var webApplicationBuilder = WebApplication.CreateBuilder(args);

    var hostType = webApplicationBuilder.Configuration.GetValue<HostType>("HostType");
    var authType = webApplicationBuilder.Configuration.GetValue<AuthType>("AuthType");

    logger.Info($"AuthType = '{authType}', HostType = '{hostType}'");

    // Setup cors
    webApplicationBuilder.Services.AddCors(options => options.AddPolicy("CorsPolicy", policy => policy.AllowAnyMethod().AllowAnyHeader().AllowCredentials()));

    // Setup authentication
    if (hostType == HostType.Selfhosted)
    {
        if (authType == AuthType.NTLM)
        {
            webApplicationBuilder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme).AddNegotiate();
        }
        else if (authType == AuthType.Kerberos)
        {
            throw new NotImplementedException("Kerberos authentication is not implemented right now.");
        }
    }
    else if (hostType == HostType.IIS)
    {
        webApplicationBuilder.Services.AddAuthentication(IISDefaults.AuthenticationScheme);
    }

    //webApplicationBuilder.Services.AddAuthorization(authorizationOptions => authorizationOptions.FallbackPolicy = authorizationOptions.DefaultPolicy);

    // Setup controllers
    webApplicationBuilder.Services.AddControllers().AddNewtonsoftJson(mvcNewtonsoftJsonOptions =>
    {
        mvcNewtonsoftJsonOptions.SerializerSettings.Converters.Add(new StringEnumConverter(new CamelCaseNamingStrategy(), true));
        mvcNewtonsoftJsonOptions.SerializerSettings.NullValueHandling = NullValueHandling.Include;
    });

    // Setup endpoints
    webApplicationBuilder.Services.AddEndpointsApiExplorer();

    // Setup swagger document generator
    webApplicationBuilder.Services.AddSwaggerGen();

    // Setup health checks
    webApplicationBuilder.Services.AddHealthChecks();

    // Setup caching
    webApplicationBuilder.Services.AddMemoryCache(options => options.TrackStatistics = true);

    // Setup NLog
    webApplicationBuilder.Logging.ClearProviders().SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
    webApplicationBuilder.Host.UseNLog();

    // Setup configuration files
    var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
    webApplicationBuilder.Configuration.SetBasePath(Directory.GetCurrentDirectory());
    webApplicationBuilder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    webApplicationBuilder.Configuration.AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true);

    // Setup configuration
    webApplicationBuilder.Services.Configure<ApplicationOptions>(options => webApplicationBuilder.Configuration.GetSection("Application").Bind(options));
    webApplicationBuilder.Services.Configure<EntityValidationInfrastructureOptions>(options => webApplicationBuilder.Configuration.GetSection("EntityValidationInfrastructure").Bind(options));

    // Setup options
    //webApplicationBuilder.Services.AddOptions();

    // Setup dependences
    webApplicationBuilder.Services.AddDependences();

    // Setup Kestrel
    webApplicationBuilder.WebHost.UseKestrel((webHostBuilderContext, kestrelServerOptions) =>
    {
        var kestrelLimits = webHostBuilderContext.Configuration.GetSection("Kestrel:Limits");

        kestrelServerOptions.Limits.MaxConcurrentConnections = kestrelLimits.GetValue<long?>("MaxConcurrentConnections");
        kestrelServerOptions.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(kestrelLimits.GetValue<int>("KeepAliveTimeout:Minutes"));
        kestrelServerOptions.Limits.MaxRequestBodySize = kestrelLimits.GetValue<long?>("MaxRequestBodySize");

        kestrelServerOptions.Limits.MinRequestBodyDataRate = new MinDataRate(
            bytesPerSecond: kestrelLimits.GetValue<int>("MinRequestBodyDataRate:BytesPerSecond"),
            gracePeriod: TimeSpan.FromSeconds(kestrelLimits.GetValue<int>("MinRequestBodyDataRate:GracePeriod:Seconds")));

        kestrelServerOptions.Limits.MinResponseDataRate = new MinDataRate(
            bytesPerSecond: kestrelLimits.GetValue<int>("MinResponseDataRate:BytesPerSecond"),
            gracePeriod: TimeSpan.FromSeconds(kestrelLimits.GetValue<int>("MinResponseDataRate:GracePeriod:Seconds")));
    });

    // Setup IIS (OutOfProcess)
    if (hostType == HostType.IIS)
    {
        webApplicationBuilder.WebHost.UseIISIntegration();
    }

    // Setup web application
    var webApplication = webApplicationBuilder.Build();

    if (!webApplication.Environment.IsDevelopment())
    {
        // Setup swagger object model and middleware
        webApplication.UseSwagger();

        // Setup swagger embedded UI tool
        webApplication.UseSwaggerUI();
    }

    webApplication.UseHttpsRedirection();
    webApplication.UseStaticFiles();
    webApplication.MapControllers();

    webApplication.Run();
}
catch (Exception ex)
{
    logger.Error(ex, "Stopped application because of exception.");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    LogManager.Shutdown();
}
