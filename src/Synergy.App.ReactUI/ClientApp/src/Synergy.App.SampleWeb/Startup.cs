using ActiveQueryBuilder.Web.Core;
using AutoMapper;
using AutoMapper.Data;
using Synergy.App.WebUtility;
using DNTCaptcha.Core;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using Synergy.App.Business;
using Synergy.App.Repository;
using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;
 
using tusdotnet.Models;
using tusdotnet.Models.Concatenation;
using tusdotnet.Models.Configuration;
using tusdotnet.Models.Expiration;
using tusdotnet.Stores;

namespace Synergy.App.SampleWeb
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IWebHostEnvironment HostEnvironment { get; }
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            HostEnvironment = env;
        }



        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
                //options.HttpOnly = HttpOnlyPolicy.Always;
                //options.Secure = CookieSecurePolicy.Always;
                // you can add more options here and they will be applied to all cookies (middleware and manually created cookies)
            });
            // services.AddSession();
            services.AddSession(options =>
            {
                //options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            });
            services.AddAntiforgery(options =>
            {
                //options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            });
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddSingleton<IConfiguration>(Configuration);
            services.AddSingleton<IWebHelper, WebHelper>();
            var builder = services.AddControllersWithViews();
#if DEBUG

            if (HostEnvironment.IsDevelopment())
            {
                var path = HostEnvironment.ContentRootPath;

                var utility = Path.GetFullPath(Path.Combine(path, "..", "Synergy.App.WebUtility"));
                var chr = Path.GetFullPath(Path.Combine(path, "..", "Synergy.App.Hrms"));
                var core = Path.GetFullPath(Path.Combine(path, "..", "Synergy.App.Core"));
                var dms = Path.GetFullPath(Path.Combine(path, "..", "Synergy.App.Dms"));
                var egov = Path.GetFullPath(Path.Combine(path, "..", "Synergy.App.SmartCity"));
                var others = Path.GetFullPath(Path.Combine(path, "..", "Synergy.App.Others"));
                var pjm = Path.GetFullPath(Path.Combine(path, "..", "Synergy.App.ProjectManagement"));
                var pms = Path.GetFullPath(Path.Combine(path, "..", "Synergy.App.PerformanceManagement"));
                var rec = Path.GetFullPath(Path.Combine(path, "..", "Synergy.App.Recruitment"));
                var website = Path.GetFullPath(Path.Combine(path, "..", "Synergy.App.Website"));
                var inventory = Path.GetFullPath(Path.Combine(path, "..", "Synergy.App.Inventory"));

                builder.AddRazorRuntimeCompilation(options =>
                {
                    options.FileProviders.Add(new PhysicalFileProvider(chr));
                    options.FileProviders.Add(new PhysicalFileProvider(core));
                    options.FileProviders.Add(new PhysicalFileProvider(dms));
                    options.FileProviders.Add(new PhysicalFileProvider(egov));
                    options.FileProviders.Add(new PhysicalFileProvider(others));
                    options.FileProviders.Add(new PhysicalFileProvider(pjm));
                    options.FileProviders.Add(new PhysicalFileProvider(pms));
                    options.FileProviders.Add(new PhysicalFileProvider(rec));
                    options.FileProviders.Add(new PhysicalFileProvider(website));
                    options.FileProviders.Add(new PhysicalFileProvider(utility));
                    options.FileProviders.Add(new PhysicalFileProvider(inventory));
                });
            }
#endif
            builder.AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());
            builder.AddRazorOptions(options =>
            {
                options.ViewLocationFormats.Add("~/Areas/Core/Views/Account/{0}.cshtml");
                options.ViewLocationFormats.Add("~/Areas/Core/Views/Cms/{0}.cshtml");
                options.ViewLocationFormats.Add("~/Areas/Core/Views/CmsService/{0}.cshtml");
                options.ViewLocationFormats.Add("~/Areas/Core/Views/Company/{0}.cshtml");
                options.ViewLocationFormats.Add("~/Areas/Core/Views/Diagram/{0}.cshtml");
                options.ViewLocationFormats.Add("~/Areas/Core/Views/FileDownLoad/{0}.cshtml");
                options.ViewLocationFormats.Add("~/Areas/Core/Views/Home/{0}.cshtml");
                options.ViewLocationFormats.Add("~/Areas/Core/Views/Nts/{0}.cshtml");
                options.ViewLocationFormats.Add("~/Areas/Core/Views/Shared/{0}.cshtml");
                options.ViewLocationFormats.Add("~/Areas/Core/Views/User/{0}.cshtml");
            });

            services.AddMyRclServices(Configuration);
            services.AddDNTCaptcha(options =>
               // options.UseSessionStorageProvider() // -> It doesn't rely on the server or client's times. Also it's the safest one.
               // options.UseMemoryCacheStorageProvider() // -> It relies on the server's times. It's safer than the CookieStorageProvider.
               options.UseCookieStorageProvider() // -> It relies on the server and client's times. It's ideal for scalability, because it doesn't save anything in the server's memory.
                                                  // options.UseDistributedCacheStorageProvider() // --> It's ideal for scalability using `services.AddStackExchangeRedisCache()` for instance.
                 .WithEncryptionKey("This is Synergy Captcha!")
                   // Don't set this line (remove it) to use the installed system's fonts (FontName = "Tahoma").
                   // Or if you want to use a custom font, make sure that font is present in the wwwroot/fonts folder and also use a good and complete font!
                   // .UseCustomFont(Path.Combine(_env.WebRootPath, "fonts", "name.ttf")) 
                   // .AbsoluteExpiration(minutes: 7)
                   .ShowThousandsSeparators(false)
           );
            //services.AddKendo();
            services.AddMvc(options =>
            {
                options.EnableEndpointRouting = false;
                // options.ModelBinderProviders.Insert(0, new DateTimeBinderProvider(services));
                // options.RegisterDateTimeProvider(services);
                //options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            }
            )
                //.AddNewtonsoftJson(jsonOption => jsonOption.RegisterDateTimeConverter(services))
                .AddRazorRuntimeCompilation();
            BundleAndMinify(services);

            //services.TryAddSingleton<IReportServiceConfiguration>(sp =>
            //new ReportServiceConfiguration
            //{
            //    ReportingEngineConfiguration = sp.GetService<IConfiguration>(),

            //    HostAppId = "CmsReportTempData",
            //    Storage = new FileStorage(Path.Combine(sp.GetService<Microsoft.Extensions.Hosting.IHostEnvironment>().ContentRootPath, "Reports")),
            //    ReportResolver = new ReportTypeResolver()
            //        .AddFallbackResolver(new ReportFileResolver(
            //            Path.Combine(sp.GetService<Microsoft.Extensions.Hosting.IHostEnvironment>().ContentRootPath, "Reports")))
            //});


            services.AddSignalR();
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddEntityFrameworkNpgsql().AddDbContext<PostgreDbContext>(opt =>
        opt.UseNpgsql(Configuration.GetConnectionString("PostgreConnection")));

            services.AddDefaultIdentity<ApplicationIdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>()
                 .AddSignInManager<AuthSignInManager<ApplicationIdentityUser>>();
            //services.AddControllersWithViews();
            services.AddLocalization(opt =>
            {
                opt.ResourcesPath = "Resources";
            });

            //var  provider = new CustomRequestCultureProvider(async (HttpContext) => {
            //     await Task.Yield();
            //     return new ProviderCultureResult(WebExtension.GetCultureInfo(HttpContext.User));
            // });

            // services.Configure<RequestLocalizationOptions>(options =>
            // {
            //     options.RequestCultureProviders.Clear();
            //     options.RequestCultureProviders.Add(provider);
            // });

            IConfiguration qbConfig = null;
            //qbConfig = Configuration.GetSection("aspQueryBuilder"); // Uncomment this line to apply settings from the configuration file
            services.AddActiveQueryBuilder(qbConfig);


            var configuration = new MapperConfiguration(cfg =>
            {
                ConfigureMapper(cfg);
            });


            var mapper = configuration.CreateMapper();
            services.AddSingleton(mapper);


            services.AddMvcCore().AddAuthorization(options =>
            {
                options.AddPolicy("AuthorizeCMS", policy => policy.Requirements.Add(new AuthorizeCMS()));
            });

            services.AddRazorPages();
            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/LogIn";
                options.ExpireTimeSpan = TimeSpan.FromDays(2);
                options.SlidingExpiration = true;
            });

            services.Configure<KestrelServerOptions>(options =>
            {
                options.Limits.MaxRequestBodySize = int.MaxValue; // if don't set default value is: 30 MB
            });


            BusinessHelper.RegisterDependency(services);


            // services.AddHangfire(x => x.UsePostgreSqlStorage(Configuration.GetConnectionString("HangfirePostgreConnection")));
            JobStorage.Current = new PostgreSqlStorage(Configuration.GetConnectionString("HangfirePostgreConnection"));
            // Add the processing server as IHostedService
            //services.AddHangfireServer();

            /// tusdotnet Start
            //services.AddCors();
            //services.AddSingleton(CreateTusConfiguration);
            //services.AddSingleton<ExpiredFilesCleanupService>();
            /// tusdotnet End 
        }

        private void BundleAndMinify(IServiceCollection services)
        {
            services.AddWebOptimizer(pipeline =>
            {
                //// Creates a CSS and a JS bundle. Globbing patterns supported.
                //pipeline.AddCssBundle("/css/bundle.css", "css/*.css");
                //pipeline.AddJavaScriptBundle("/js/bundle.js", "js/plus.js", "js/minus.js");

                //// This bundle uses source files from the Content Root and uses a custom PrependHeader extension
                //pipeline.AddJavaScriptBundle("/js/scripts.js", "scripts/a.js", "wwwroot/js/plus.js")
                //        .UseContentRoot()
                //        .AddResponseHeader("x-test", "value");

                // This will minify any JS and CSS file that isn't part of any bundle
                pipeline.AddCssBundle("/css/bundle1.css",
                     "lib/bootstrap/dist/css/bootstrap.css",
      "css/floating-labels.css",
      "css/jquery-splitter.css",
                    //"lib/font-awesome/css/all.css",
                    //"lib/jquery-context-menu/dist/jquery.contextMenu.css",
                    "css/site.css",
                    "css/common.css"
                    // "css/elegant-icons-style.css",
                    //"css/style.css"
                    //"css/style-responsive.css",
                    //"css/line-icons.css",
                    //"lib/kendo/dist/css/kendo.bootstrap-v4.css",
                    //"css/jsgrid/jsgrid.css",
                    //"css/jsgrid/jsgrid-theme.css",
                    //"css/Jstree/style.css",
                    //"lib/duration-picker/dist/jquery-duration-picker.css"
                    );
                pipeline.AddCssBundle("/css/bundle2.css",
                     "css/style.css"
                    );
                pipeline.AddCssBundle("/css/bundle3.css",
                         "lib/jquery-context-menu/dist/jquery.contextMenu.css"
                  );

                //"css/elegant-icons-style.css",
                // "css/style.css"
                //"css/style-responsive.css",
                //"css/line-icons.css"
                //);
                //pipeline.MinifyCssFiles();
                //pipeline.MinifyJsFiles();



                //// AddFiles/AddBundle allow for custom pipelines
                //pipeline.AddBundle("/text.txt", "text/plain", "random/*.txt")
                //        .AdjustRelativePaths()
                //        .Concatenate()
                //        .FingerprintUrls()
                //        .MinifyCss();
            });
            //      services.AddWebOptimizer(pipeline =>
            //      {
            //          pipeline.AddCssBundle("/css/bundle1.css",
            //               "lib/bootstrap/dist/css/bootstrap.min.css",
            //"css/floating-labels.css",
            //"css/jquery-splitter.css",
            //"lib/font-awesome/css/all.min.css",
            //"lib/jquery-context-menu/dist/jquery.contextMenu.min.css",
            //"css/site.css",
            //"css/common.css",
            //"css/elegant-icons-style.css",
            //"css/style.css",
            //"css/style-responsive.css",
            //"css/line-icons.css",
            //"lib/kendo/dist/css/kendo.bootstrap-v4.min.css",
            //"css/jsgrid/jsgrid.min.css",
            //"css/jsgrid/jsgrid-theme.min.css",
            //"css/Jstree/style.min.css",
            //"lib/duration-picker/dist/jquery-duration-picker.min.css"
            //              );
            //      });
        }

        protected virtual void ConfigureMapper(IMapperConfigurationExpression cfg)
        {
            cfg.AddDataReaderMapping(false);
            var profile = new MappingProfile();
            profile.AddDataRecordMember();
            cfg.AddProfile(profile);

        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //  app.UseCookiePolicy(new CookiePolicyOptions
            //  {
            //      Secure = CookieSecurePolicy.Always
            //  });

            app.UseMiddleware<HttpModuleMiddleware>();
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            app.UseAuthentication();

            //Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("NDE4MzQ5QDMxMzgyZTM0MmUzMGdla0UydThJVnAxeUxlQ2pPcFNLcStzemZoR0ZjQjNCWE05cFI0RDJiVWs9;NDE4MzUwQDMxMzgyZTM0MmUzMG8xZDJORWxpbFdtejZuOHBwVCtrem9BNFhyRTNlZDlwcmd1MVNjQ09vNFE9;NDE4MzUxQDMxMzgyZTM0MmUzMEJuZ2pGM2FJbCtoNmUyR0dCWjdqejh2ek1mc2ROekdQUTkraUM2eVFxUEk9;NDE4MzUyQDMxMzgyZTM0MmUzMGt6VzJGVWVaelJnYkh2dG9qL0xqNzE4OFJ1MVZjWUJrK28xVEpLZy85Mjg9;NDE4MzUzQDMxMzgyZTM0MmUzMERQeStlSkdwc3lucVNGbkdwelVWQzJSaVZjd2ZXWWh1dG1GSy9WY0Z5SXM9;NDE4MzU0QDMxMzgyZTM0MmUzME1jQlB5MUI3QkwyUE1hSStVV1F5aTBzRDk0SnYxemZEU2pWTSs0UHNEVG89;NDE4MzU1QDMxMzgyZTM0MmUzMGpTcE5sd21lOXBFTUNGWkIrd0lDUE5jem9WdVpJK2xzRmNQbkZFWktBcXc9;NDE4MzU2QDMxMzgyZTM0MmUzMEVwRTRQUlZqdW5iL2JwWXZvVGdrM3hOd1YvUlJ3UXQzR3BSV08rZTBSczQ9;NDE4MzU3QDMxMzgyZTM0MmUzMG5OZnlmN2dxeDdOZkltTlN2ZnZhYlI4dUc3c25tVjRjdWFJditINkpKbXM9;NDE4MzU4QDMxMzgyZTM0MmUzMFFXUVhQMlorVWYyV2VhVjlqSzlKV0kyREp2K2VDN1NJeVNvM3ozWUJOWWs9");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }



            //app.UseMiddleware<LocalizationCustomMiddleware>();
            app.Use(async (context, next) =>
            {
                var ci = (CultureInfo)CultureInfo.GetCultureInfo(WebExtension.GetCultureName(context)).Clone();
                ci.DateTimeFormat = new DateTimeFormatInfo
                {
                    Calendar = new GregorianCalendar(),
                    ShortDatePattern = "yyyy-MM-dd",
                    LongDatePattern = "yyyy-MM-ddTHH:mm:ss.fff",
                    LongTimePattern = "HH:mm:ss.fff",
                    DateSeparator = "-"
                };
                //ci.DateTimeFormat.ShortDatePattern = "yyyy-MM-dd";
                //ci.DateTimeFormat.LongDatePattern = "yyyy-MM-ddTHH:mm:ss.fff";
                //ci.DateTimeFormat.LongTimePattern = "HH:mm:ss.fff";
                //ci.DateTimeFormat.DateSeparator = "-";
                context.Response.Headers.Add("X-Frame-Options", "SAMEORIGIN");
                context.Response.Headers.Add("X-Xss-Protection", "1; mode=block");
                context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                CultureInfo.CurrentCulture = ci;
                CultureInfo.CurrentUICulture = CultureInfo.CurrentCulture;
                await next();
            });

            app.UseHttpsRedirection();
            app.UseWebOptimizer();
            app.UseStaticFiles();

            app.UseStaticFiles(new StaticFileOptions
            {
                ServeUnknownFileTypes = true,
                DefaultContentType = "text/plain"
            });


            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();

            // Active Query Builder server requests handler.
            app.UseActiveQueryBuilder();
            app.UseEndpoints(endpoints =>
            {
                // endpoints.MapHangfireDashboard();
                endpoints.MapAreaControllerRoute("default", "Cms", "Cms/{controller=content}/{action=Index}/{id?}");
                endpoints.MapAreaControllerRoute("bre", "Bre", "Bre/{controller}/{action}/{id?}");
                endpoints.MapAreaControllerRoute("recruitment", "Recruitment", "Recruitment/{controller}/{action}/{id?}");
                endpoints.MapAreaControllerRoute("recruitment", "Recruitment", "Recruitment/{controller}/{action}/{id?}");
                endpoints.MapAreaControllerRoute("pay", "Pay", "Pay/{controller}/{action}/{id?}");
                endpoints.MapAreaControllerRoute("pjm", "PJM", "PJM/{controller}/{action}/{id?}");
                endpoints.MapAreaControllerRoute("pms", "Pms", "Pms/{controller}/{action}/{id?}");
                endpoints.MapAreaControllerRoute("chr", "CHR", "CHR/{controller}/{action}/{id?}");
                endpoints.MapAreaControllerRoute("dms", "DMS", "DMS/{controller}/{action}/{id?}");
                endpoints.MapAreaControllerRoute("tas", "Tas", "Tas/{controller}/{action}/{id?}");
                endpoints.MapAreaControllerRoute("vp", "VisionPioneer", "VisionPioneer/{controller}/{action}/{id?}");
                endpoints.MapAreaControllerRoute("tms", "Tms", "Tms/{controller}/{action}/{id?}");
                endpoints.MapAreaControllerRoute("inv", "Inv", "Inv/{controller}/{action}/{id?}");
                endpoints.MapAreaControllerRoute("taa", "TAA", "TAA/{controller}/{action}/{id?}");
                endpoints.MapAreaControllerRoute("lms", "LMS", "LMS/{controller}/{action}/{id?}");
                endpoints.MapAreaControllerRoute("tas", "TAS", "TAS/{controller}/{action}/{id?}");
                endpoints.MapAreaControllerRoute("clk", "Clk", "Clk/{controller}/{action}/{id?}");
                endpoints.MapAreaControllerRoute("portalAdmin", "PortalAdmin", "PortalAdmin/{controller}/{action}/{id?}");
                endpoints.MapAreaControllerRoute("property", "Property", "Property/{controller}/{action}/{id?}");
                endpoints.MapAreaControllerRoute("egov", "EGov", "EGov/{controller}/{action}/{id?}");
                endpoints.MapAreaControllerRoute("ims", "IMS", "IMS/{controller}/{action}/{id?}");
                endpoints.MapAreaControllerRoute("sps", "Sps", "Sps/{controller}/{action}/{id?}");
                endpoints.MapAreaControllerRoute("synergySolutions", "SynergySolutions", "SynergySolutions/{controller}/{action}/{id?}");
                endpoints.MapAreaControllerRoute("rec", "Rec", "Rec/{controller}/{action}/{id?}");
                endpoints.MapAreaControllerRoute("career", "Career", "Career/{controller}/{action}/{id?}");
                endpoints.MapAreaControllerRoute("core", "Core", "Core/{controller}/{action}/{id?}");
                endpoints.MapControllerRoute(name: "appDefault", pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();

            });



            // Configure the Localization middleware
            //app.UseRequestLocalization(new RequestLocalizationOptions
            //{
            //    DefaultRequestCulture = new RequestCulture(ci),
            //    SupportedCultures = new List<CultureInfo>
            //{
            //    ci,
            //},
            //    SupportedUICultures = new List<CultureInfo>
            //{
            //    ci,
            //}
            //});
            app.UseFastReport();
            app.UseMvc();


            //app.UseTus(httpContext => new DefaultTusConfiguration
            //{
            //    // This method is called on each request so different configurations can be returned per user, domain, path etc.
            //    // Return null to disable tusdotnet for the current request.

            //    // c:\tusfiles is where to store files
            //    Store = new TusDiskStore(@"C:\tusfiles\"),
            //    // On what url should we listen for uploads?
            //    UrlPath = "Cms/files",
            //    Events = new Events
            //    {
            //        OnFileCompleteAsync = async eventContext =>
            //        {
            //            ITusFile file = await eventContext.GetFileAsync();
            //            Dictionary<string, Metadata> metadata = await file.GetMetadataAsync(eventContext.CancellationToken);
            //            Stream content = await file.GetContentAsync(eventContext.CancellationToken);

            //            //await DoSomeProcessing(content, metadata);
            //        }
            //    }
            //});

            try
            {
                Console.WriteLine("===================== " + Path.Combine(AppContext.BaseDirectory, "MigrationScript"));
                BackgroundJob.Enqueue<Synergy.App.Business.HangfireScheduler>(x => x.IncrementalDBMigration());
            }
            catch (Exception e)
            {

            }

            ///tusdotnet start
            //app.UseDefaultFiles();
            //app.UseStaticFiles();

            //app.UseCors(builder => builder
            //.AllowAnyHeader()
            //.AllowAnyMethod()
            //.AllowAnyOrigin()
            //.WithExposedHeaders(tusdotnet.Helpers.CorsHelper.GetExposedHeaders())
            //);


            ////app.UseSimpleExceptionHandler();

            // httpContext parameter can be used to create a tus configuration based on current user, domain, host, port or whatever.
            // In this case we just return the same configuration for everyone.
            //app.UseTus(httpContext => Task.FromResult(httpContext.RequestServices.GetService<DefaultTusConfiguration>()));

            // All GET requests to tusdotnet are forwarded so that you can handle file downloads.
            // This is done because the file's metadata is domain specific and thus cannot be handled 
            // in a generic way by tusdotnet.
            ////app.UseSimpleDownloadMiddleware(httpContext => Task.FromResult(httpContext.RequestServices.GetService<DefaultTusConfiguration>()));

            // Start cleanup job to remove incomplete expired files.
            ////var cleanupService = app.ApplicationServices.GetService<ExpiredFilesCleanupService>();
            ////cleanupService.Start();

            //app.UseTus(httpContext => new DefaultTusConfiguration
            //{
            //    // This method is called on each request so different configurations can be returned per user, domain, path etc.
            //    // Return null to disable tusdotnet for the current request.

            //    // c:\tusfiles is where to store files
            //    Store = new TusDiskStore(@"C:\tusfiles\"),
            //    // On what url should we listen for uploads?
            //    UrlPath = "/files",
            //    //UrlPath = "cms/tempfiles",
            //    Events = new Events
            //    {
            //        OnFileCompleteAsync = async eventContext =>
            //        {
            //            ITusFile file = await eventContext.GetFileAsync();
            //            Dictionary<string, Metadata> metadata = await file.GetMetadataAsync(eventContext.CancellationToken);
            //            Stream content = await file.GetContentAsync(eventContext.CancellationToken);

            //            //await DoSomeProcessing(content, metadata);
            //        }
            //    }
            //});

            ///tusdotnet end
        }
        //tusdotnet
        private DefaultTusConfiguration CreateTusConfiguration(IServiceProvider serviceProvider)
        {
            var logger = serviceProvider.GetService<Microsoft.Extensions.Logging.ILoggerFactory>().CreateLogger<Startup>();
            var tusFiles = @"C:\tusfiles\";
            var defaultTusConfig = new DefaultTusConfiguration
            {
                UrlPath = "/files",
                Store = new TusDiskStore(tusFiles),
                MetadataParsingStrategy = MetadataParsingStrategy.AllowEmptyValues,
                Events = new Events
                {
                    OnBeforeCreateAsync = ctx =>
                    {
                        // Partial files are not complete so we do not need to validate
                        // the metadata in our example.
                        if (ctx.FileConcatenation is FileConcatPartial)
                        {
                            return Task.CompletedTask;
                        }
                        //uppy
                        //if (!ctx.Metadata.ContainsKey("name") || ctx.Metadata["name"].HasEmptyValue)
                        //{
                        //    ctx.FailRequest("name metadata must be specified. ");
                        //}
                        //uppy
                        //if (!ctx.Metadata.ContainsKey("type") || ctx.Metadata["type"].HasEmptyValue)
                        //{
                        //    ctx.FailRequest("contentType metadata must be specified. ");
                        //}
                        //tus
                        if (!ctx.Metadata.ContainsKey("filename") || ctx.Metadata["filename"].HasEmptyValue)
                        {
                            ctx.FailRequest("name metadata must be specified. ");
                        }
                        //tus
                        if (!ctx.Metadata.ContainsKey("filetype") || ctx.Metadata["filetype"].HasEmptyValue)
                        {
                            ctx.FailRequest("contentType metadata must be specified. ");
                        }

                        return Task.CompletedTask;
                    },
                    OnCreateCompleteAsync = ctx =>
                    {
                        logger.LogInformation($"Created file {ctx.FileId} using {ctx.Store.GetType().FullName}");
                        return Task.CompletedTask;
                    },
                    OnBeforeDeleteAsync = ctx =>
                    {
                        // Can the file be deleted? If not call ctx.FailRequest(<message>);
                        return Task.CompletedTask;
                    },
                    OnDeleteCompleteAsync = ctx =>
                    {
                        logger.LogInformation($"Deleted file {ctx.FileId} using {ctx.Store.GetType().FullName}");
                        return Task.CompletedTask;
                    },
                    OnFileCompleteAsync = async ctx =>
                    {
                        logger.LogInformation($"Upload of {ctx.FileId} completed using {ctx.Store.GetType().FullName}");
                        // If the store implements ITusReadableStore one could access the completed file here.
                        // The default TusDiskStore implements this interface:
                        //var file = await ctx.GetFileAsync();
                        //return Task.CompletedTask;

                        //Get upload file
                        var file = await ctx.GetFileAsync();
                        //Get upload file
                        var metadatas = await file.GetMetadataAsync(ctx.CancellationToken);

                        //Get the target file name in the above file metadata
                        //uppy
                        //var fileNameMetadata = metadatas["name"];
                        //tus
                        var fileNameMetadata = metadatas["filename"];

                        //The target file name is encoded in Base64, so it needs to be decoded here
                        var fileName = fileNameMetadata.GetString(Encoding.UTF8);
                        var extensionName = Path.GetExtension(fileName);
                        //Convert the uploaded file to the actual target file
                        File.Move(Path.Combine(tusFiles, ctx.FileId), Path.Combine(tusFiles, $"{ctx.FileId}{extensionName}"));
                    }
                },
                // Set an expiration time where incomplete files can no longer be updated.
                // This value can either be absolute or sliding.
                // Absolute expiration will be saved per file on create
                // Sliding expiration will be saved per file on create and updated on each patch/update.
                Expiration = new AbsoluteExpiration(TimeSpan.FromMinutes(5))
            };
            return defaultTusConfig;
        }
    }
}
