using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Newtonsoft.Json.Serialization;
using PFCWebAPP.Repositories.BackOps;
using PFCWebAPP.Repositories.Common.Interfaces;
using PFCWebAPP.Repositories.Common.ServiceProviders;
using PFCWebAPP.Repositories;
using PFCWebAPP.Utilities;
using System.IO.Compression;
using PFCWebAPP.Filters;
using PFCWebAPP.DatabaseContext;
using PFCWebAPP.Repositories.BackOps.ServiceProviders;
using PFCWebAPP.Repositories.BackOps.Interfaces;
using PFCWebAPP.Repositories.Configure;
using PFCWebAPP.Repositories.Configure.Interfaces;
using PFCWebAPP.Repositories.Configure.ServiceProviders;
using PFCWebAPP.Repositories.PriceList.Interfaces;
using PFCWebAPP.Repositories.PriceList.ServiceProviders;
using PFCWebAPP.Repositories.PriceList;
using PFCWebAPP.Repositories.Common;

namespace PFCWebAPP.Extensions
{
    public static class ServiceExtensions
    {


        // MVC Extensions
        public static IServiceCollection AddMvcConfigurations(this IServiceCollection services)
        {

            // Add services to the container.

            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddControllersWithViews().AddRazorOptions(options =>
            {
                options.ViewLocationFormats.Add("/{0}.cshtml");

            });

            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.IgnoreNullValues = true;// removing the null value properties the Object
            });
            services.AddControllers().AddNewtonsoftJson();
            services.AddControllersWithViews().AddNewtonsoftJson();
            services.AddRazorPages().AddNewtonsoftJson();

            services.AddControllers(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true); // to avoid some model validation changes in ASP.Net 6 (Non Nullable Reference types)


            //services.AddMvc(option =>
            //{
            //    option.Filters.Add(typeof(PFCExceptionFilter));
            //    option.ModelMetadataDetailsProviders.Add(new DisplayProvider());
            //})
            //.AddNewtonsoftJson(option => option.SerializerSettings.ContractResolver = new DefaultContractResolver());

            return services;
        }


        //DependencyInjection
        public static IServiceCollection AddDependencyInjectionConfigureServices(this IServiceCollection services)
        {



            services.AddTransient<PFCAuthFilter>();
            services.AddTransient<PFCAPIAuthFilter>();
            services.AddTransient<PFCExceptionFilter>();
            services.AddTransient<PFCAPIExceptionFilter>();
            services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient<ILoggingProvider, LoggingProvider>();
            services.AddTransient<IBackOpsProvider, BackOpsProvider>();
            services.AddTransient<IBackOpsRepository, BackOpsRepository>();
            services.AddTransient<ISqlHelper, SqlHelper>();
            services.AddTransient<ICommonProvider, CommonProvider>();
            services.AddTransient<ICommonRepository, CommonRepository>();
            services.AddTransient<IConfigureRepository, ConfigureRepository>();
            services.AddTransient<IConfigureProvider, ConfigureProvider>();
            services.AddTransient<IPriceListProvider, PriceListProvider>();
            services.AddTransient<IPriceListRepository, PriceListRepository>();
            services.AddTransient<IMailSenderProvider, MailSenderProvider>();
            services.AddTransient<INotificationProvider, NotificationProvider>();
            return services;
        }


        public static IServiceCollection AddSessionAndCookiesConfigurations(this IServiceCollection services)
        {


            services.AddMvc().AddSessionStateTempDataProvider();
            services.AddRazorPages().AddSessionStateTempDataProvider();
            services.AddControllersWithViews().AddSessionStateTempDataProvider();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(60);//We can set Time here 
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            

            //services.ConfigureApplicationCookie(options =>
            //{
            //    options.Cookie.Name = "AspNetCore.Session";//We can set Time here 
            //    options.ExpireTimeSpan = TimeSpan.FromMinutes(1);
            //    options.SlidingExpiration = true;
            //});



            return services;
        }


        public static IServiceCollection AddCacheConfigurations(this IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddDistributedMemoryCache();


            return services;
        }

        public static IServiceCollection AddDataBaseConfigurations(this IServiceCollection services)
        {
            //Database Connection
            services.AddDbContext<PFCDBContext>(options =>
            {
                //var connectionString = builder.Configuration.GetConnectionString("ConnectionString");
                options.UseSqlServer(AppConfig.ConnectionString,
                   // x => x.MigrationsHistoryTable("MigrationsHistory", "dbo")
                   sqlServerOptionsAction: x =>
                   {
                      // x.EnableRetryOnFailure(maxRetryCount: 10,maxRetryDelay: TimeSpan.FromSeconds(30),errorNumbersToAdd: null);
                       x.MigrationsHistoryTable("MigrationsHistory", "dbo");
                     //  x.CommandTimeout = 180;
                       
                   });
                    

            });
            


            return services;
        }

        public static IServiceCollection AddCorsConfigurations(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowAnyOrigin();
                });
            });

            //services.AddCors(options =>
            //{
            //    options.AddDefaultPolicy(
            //        policy =>
            //        {
            //            policy.WithOrigins("http://SE.com",
            //                                "http://www.SEAPI.com").AllowAnyHeader()
            //                                              .AllowAnyMethod(); ; //.WithMethods("PUT", "DELETE", "GET");

            //        });
            //});

            return services;
        }

        public static IServiceCollection AddResponseCompressionConfigurations(this IServiceCollection services)
        {
            services.AddResponseCompression();
            services.Configure<GzipCompressionProviderOptions>
            (options =>
            {
                options.Level = CompressionLevel.Fastest;
            });
            return services;
        }

        public static IServiceCollection AddOtherConfigurations(this IServiceCollection services)
        {
            
            return services;

        }

        public class DisplayProvider : IDisplayMetadataProvider
        {
            public void CreateDisplayMetadata(DisplayMetadataProviderContext context)
            {
                if (context.Key.ModelType == typeof(string))
                    context.DisplayMetadata.ConvertEmptyStringToNull = false;
            }
        }




    }
}
