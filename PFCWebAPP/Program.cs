
//global using System; need to check once how to use global using
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Extensions.Options;
using NLog;
using NLog.Web;
using PFCWebAPP.Extensions;
using PFCWebAPP.Filters;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
try
{
    logger.Debug("Application Started");

    var builder = WebApplication.CreateBuilder(args);
    // NLog: Setup NLog for Dependency injection
    builder.Logging.ClearProviders();

    builder.Services.AddControllers(options =>
    {
        //options.InputFormatters.Add(new XmlSerializerInputFormatter(options));
        //options.OutputFormatters.Add(new XmlSerializerOutputFormatter());
        //options.Filters.Add(new PFCAPIAuthFilter("localhost", "postman.com"));
    }).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();


    builder.Services.AddMvcConfigurations();

    //runtime compilation
    var mvcBuilder = builder.Services.AddRazorPages();

    if (builder.Environment.IsDevelopment())
    {
        mvcBuilder.AddRazorRuntimeCompilation();
    }
    builder.Services.AddDependencyInjectionConfigureServices();
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddCacheConfigurations();
    builder.Services.AddSessionAndCookiesConfigurations();
    
    builder.Services.AddDataBaseConfigurations();
    builder.Services.AddCorsConfigurations();
    builder.Services.Configure<IISServerOptions>(options =>
    {
        options.AllowSynchronousIO = true;
    });

    //Static Files & assets;
    builder.WebHost.UseWebRoot("wwwroot");
    builder.WebHost.UseStaticWebAssets();
    builder.WebHost.ConfigureKestrel(serverOptions =>
    {
        serverOptions.AllowSynchronousIO = true;
    });

    //Enable compression
    builder.Services.AddResponseCompressionConfigurations();

    var app = builder.Build();

    //app.UseForwardedHeaders();

    if (app.Environment.IsDevelopment())
    {
        //app.UseDeveloperExceptionPage();
        // app.UseDatabaseErrorPage();
        app.UseStatusCodePagesWithRedirects("/Error/{0}");
        //app.UseStatusCodePages();
    }
    else
    {
        //app.UseExceptionHandler("/Error/Index");
        // app.UseExceptionHandler("/Error"); //UseExceptionHandler is the first middleware component added to the pipeline. Therefore, the Exception Handler Middleware catches any exceptions that occur in later calls.
        app.UseStatusCodePagesWithRedirects("/Error/{0}"); // handle unhandle Exception
        app.UseHsts(); // Strict-Transport-Security
    }

    app.UseHttpsRedirection(); //HTTPS Redirection Middleware (UseHttpsRedirection) redirects HTTP requests to HTTPS.

    app.UseStaticFiles(); // UseStaticFiles is called before UseCors. Apps that use JavaScript to retrieve static files cross site must call UseCors and Static File Middleware is called early in the pipeline  so that it can handle requests and short-circuit without going through the remaining components. The Static File Middleware provides no authorization checks. 

    app.UseCookiePolicy();

    app.UseRouting();

    app.UseRequestLocalization();//UseRequestLocalization must appear before any middleware that might check the request culture

    app.UseCors();//UseCors, UseAuthentication, and UseAuthorization must appear in the order shown. and UseCors must be called before UseResponseCaching

    //var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
    //app.UseCors(MyAllowSpecificOrigins);

    app.UseAuthorization();

    app.UseAuthentication();

    app.UseSession();//If the app uses session state, call Session Middleware after Cookie Policy Middleware and before MVC Middleware.

    app.UseResponseCompression();

    // app.UseResponseCaching();

    app.MapRazorPages();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Account}/{action=Index}/{id?}");

    app.Run();
}
catch (Exception exception)
{
    // NLog: catch setup errors
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    NLog.LogManager.Shutdown();
}
