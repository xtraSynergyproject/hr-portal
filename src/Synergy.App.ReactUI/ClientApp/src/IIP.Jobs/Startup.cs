using AutoMapper;
using AutoMapper.Data;
using Hangfire;
using Hangfire.Dashboard;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using Synergy.App.Business;
using Synergy.App.Repository;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using tusdotnet;
using tusdotnet.Models;
using tusdotnet.Models.Concatenation;
using tusdotnet.Models.Configuration;
using tusdotnet.Models.Expiration;
using tusdotnet.Stores;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using Hangfire.Storage;

namespace Synergy.App.Api
{
    public class Startup
    {
        IHttpContextAccessor _accessor;
        IApplicationBuilder _app;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

        }

        public IConfiguration Configuration { get; }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AnyOrigin", builder =>
                {
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });


            //services.AddSingleton(CreateTusConfiguration);
            //services.AddSingleton<ExpiredFilesCleanupService>();
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddHttpContextAccessor();
            // services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // services.AddSingleton<HangfireScheduler>(new HangfireScheduler(null, null, null, null));



            services.AddControllers().AddNewtonsoftJson

                (options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });
            services.Configure<KestrelServerOptions>(options =>
            {
                options.Limits.MaxRequestBodySize = int.MaxValue; // if don't set default value is: 30 MB
            });

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // For Identity
            services.AddIdentity<ApplicationIdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddSignInManager<AuthSignInManager<ApplicationIdentityUser>>()
                .AddDefaultTokenProviders();


            services.AddMvc().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                // options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            });


            //services.AddDefaultIdentity<ApplicationIdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            // .AddEntityFrameworkStores<ApplicationDbContext>()
            //  .AddSignInManager<AuthSignInManager<ApplicationIdentityUser>>();

            services.AddEntityFrameworkNpgsql().AddDbContext<PostgreDbContext>(opt =>
            opt.UseNpgsql(Configuration.GetConnectionString("PostgreConnection")));


            var configuration = new MapperConfiguration(cfg =>
            {
                ConfigureMapper(cfg);
            });


            var mapper = configuration.CreateMapper();
            services.AddSingleton(mapper);

            BusinessHelper.RegisterDependency(services);

            // Adding Authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })

            // Adding Jwt Bearer
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = Configuration["JWT:ValidAudience"],
                    ValidIssuer = Configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Secret"]))
                };
            });

            services.AddSwaggerGen(swagger =>
            {
                swagger.CustomSchemaIds(type => type.ToString());
                //This is to generate the Default UI of Swagger Documentation  
                swagger.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "ASP.NET 5 Web API",
                    Description = "Authentication and Authorization in ASP.NET 5 with JWT and Swagger"
                });
                // To Enable authorization using Swagger (JWT)  
                swagger.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' [space] and then your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
                });
                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}

                    }
                });
            });

            //JobStorage.Current = new PostgreSqlStorage(Configuration.GetConnectionString("HangfirePostgreConnection"));
            services.AddHangfire(x => x.UsePostgreSqlStorage(Configuration.GetConnectionString("HangfirePostgreConnection")));
            //services.AddHangfireServer();
            services.AddHangfireServer(options =>
            {
                options.Queues = new[] { "alpha", "beta", "default" };
            });

            /// tusdotnet Start
            //services.AddCors();            
            services.AddSingleton(CreateTusConfiguration);
            ////services.AddSingleton<ExpiredFilesCleanupService>();

            /// tusdotnet end            
            //services.AddSignalR();
            services.AddSignalR(hubOptions =>
            {
                hubOptions.EnableDetailedErrors = true;
                hubOptions.KeepAliveInterval = TimeSpan.FromMinutes(60);
            });

        }
        protected virtual void ConfigureMapper(IMapperConfigurationExpression cfg)
        {
            cfg.AddDataReaderMapping(false);
            var profile = new MappingProfile();
            profile.AddDataRecordMember();
            cfg.AddProfile(profile);

        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHttpContextAccessor accessor)
        {

            _app = app;
            //app.UseStaticHttpContext();
            app.UseForwardedHeaders();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //app.UseCors(builder => builder
            //   .AllowAnyHeader()
            //   .AllowAnyMethod()
            //   //.WithOrigins("http://localhost:44389")
            //   .AllowAnyOrigin()
            //   .AllowCredentials()
            //   .WithExposedHeaders(CorsHelper.GetExposedHeaders()));
            app.UseCors("AnyOrigin");
            //app.UseCors(options => options.AllowAnyOrigin());
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ASP.NET 5 Web API v1"));

            app.UseHttpsRedirection();

            app.UseRouting();

            var options = new BackgroundJobServerOptions { WorkerCount = 2 };
            app.UseHangfireServer(options);
            app.UseAuthentication();
            app.UseAuthorization();
            //app.UseEndpoints(endpoints => { endpoints.MapHub<ServiceHub>("/signalr"); });
            app.UseHangfireDashboard("/jobs", new DashboardOptions
            {
                Authorization = new[] { new HangfireAuthorizationFilter() }
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            try
            {
                using (var connection = JobStorage.Current.GetConnection())
                {
                    foreach (var recurringJob in connection.GetRecurringJobs())
                    {
                        Console.WriteLine("clearing hangfire job id :" + recurringJob.Id);
                        RecurringJob.RemoveIfExists(recurringJob.Id);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("exception in clearing hangfire jobs "+ex.ToString());
            }           
            try
            {
                BackgroundJob.Enqueue<Synergy.App.Business.HangfireScheduler>(x => x.PerformApiDataMigrationToElasticDB());
            }
            catch (Exception)
            {

            }
            try
            {
                BackgroundJob.Enqueue<Synergy.App.Business.HangfireScheduler>(x => x.ExecuteAlertRule());
            }
            catch (Exception)
            {

            }









            //app.UseTus(httpContext => Task.FromResult(httpContext.RequestServices.GetService<DefaultTusConfiguration>()));


            /// tusdotnet start
            app.UseDefaultFiles();
            app.UseStaticFiles();



            ////app.UseSimpleExceptionHandler();

            // httpContext parameter can be used to create a tus configuration based on current user, domain, host, port or whatever.
            // In this case we just return the same configuration for everyone.
            app.UseTus(httpContext => Task.FromResult(httpContext.RequestServices.GetService<DefaultTusConfiguration>()));

            // All GET requests to tusdotnet are forwarded so that you can handle file downloads.
            // This is done because the file's metadata is domain specific and thus cannot be handled 
            // in a generic way by tusdotnet.
            ////app.UseSimpleDownloadMiddleware(httpContext => Task.FromResult(httpContext.RequestServices.GetService<DefaultTusConfiguration>()));

            // Start cleanup job to remove incomplete expired files.
            ////var cleanupService = app.ApplicationServices.GetService<ExpiredFilesCleanupService>();
            ////cleanupService.Start();
            /// tusdotnet end
        }
        public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
        {
            public bool Authorize(DashboardContext context)
            {
                return true;
                //var httpContext = context.GetHttpContext();
                //return httpContext.User.Identity.IsAuthenticated;
            }
        }
        private DefaultTusConfiguration CreateTusConfiguration(IServiceProvider serviceProvider)
        {
            var logger = serviceProvider.GetService<ILoggerFactory>().CreateLogger<Startup>();
            string tusFiles = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "tusfiles");
            bool exists = System.IO.Directory.Exists(tusFiles);
            if (!exists)
                System.IO.Directory.CreateDirectory(tusFiles);
            //var tusFiles = @"C:\tusfiles\";
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

                        //if (!ctx.Metadata.ContainsKey("name") || ctx.Metadata["name"].HasEmptyValue)
                        //{
                        //    ctx.FailRequest("name metadata must be specified. ");
                        //}

                        //if (!ctx.Metadata.ContainsKey("contentType") || ctx.Metadata["contentType"].HasEmptyValue)
                        //{
                        //    ctx.FailRequest("contentType metadata must be specified. ");
                        //}

                        //uppy
                        if (!ctx.Metadata.ContainsKey("name") || ctx.Metadata["name"].HasEmptyValue)
                        {
                            ctx.FailRequest("name metadata must be specified. ");
                        }
                        //uppy
                        if (!ctx.Metadata.ContainsKey("type") || ctx.Metadata["type"].HasEmptyValue)
                        {
                            ctx.FailRequest("contentType metadata must be specified. ");
                        }

                        ////tus
                        //if (!ctx.Metadata.ContainsKey("filename") || ctx.Metadata["filename"].HasEmptyValue)
                        //{
                        //    ctx.FailRequest("name metadata must be specified. ");
                        //}
                        ////tus
                        //if (!ctx.Metadata.ContainsKey("filetype") || ctx.Metadata["filetype"].HasEmptyValue)
                        //{
                        //    ctx.FailRequest("contentType metadata must be specified. ");
                        //}

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
                        var fileNameMetadata = metadatas["name"];
                        var parentMetaData = metadatas["parentid"];
                        var parentId = parentMetaData.GetString(Encoding.UTF8);
                        var userMetaData = metadatas["userid"];
                        var userId = userMetaData.GetString(Encoding.UTF8);
                        var templateMetaData = metadatas["templateid"];
                        var templateId = templateMetaData.GetString(Encoding.UTF8);
                        var batchMetaData = metadatas["batchid"];
                        var bacthId = batchMetaData.GetString(Encoding.UTF8);
                        //tus
                        //var fileNameMetadata = metadatas["filename"];

                        //The target file name is encoded in Base64, so it needs to be decoded here
                        var fileName = fileNameMetadata.GetString(Encoding.UTF8);
                        var extensionName = Path.GetExtension(fileName);
                        //Convert the uploaded file to the actual target file
                        File.Move(Path.Combine(tusFiles, ctx.FileId), Path.Combine(tusFiles, $"{ctx.FileId}{extensionName}"));

                        ////Create file start
                        string newpath = System.IO.Path.Combine(tusFiles, $"{ctx.FileId}{extensionName}");
                        using (var serviceScope = _app.ApplicationServices.CreateScope())
                        {
                            var _documentBusiness = serviceScope.ServiceProvider.GetRequiredService<IDMSDocumentBusiness>();
                            var result = await _documentBusiness.CreateDocumentByUpload(newpath, fileName, extensionName, parentId, userId, templateId, bacthId);
                        }
                        ////Create file end
                    },
                    // Set an expiration time where incomplete files can no longer be updated.
                    // This value can either be absolute or sliding.
                    // Absolute expiration will be saved per file on create
                    // Sliding expiration will be saved per file on create and updated on each patch/update.

                }
            };
            defaultTusConfig.Expiration = new AbsoluteExpiration(TimeSpan.FromMinutes(5));
            return defaultTusConfig;
        }
    }
}
