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
using System.Collections.Generic;
using System.Collections;
using Hangfire.Storage.Monitoring;
using System.Threading;
using System.Linq;
using AutoMapper.Configuration.Annotations;
using static Org.BouncyCastle.Math.EC.ECCurve;

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
            var hangfireOption = new PostgreSqlStorageOptions();
            hangfireOption.SchemaName = "HangfireIip";
            //JobStorage.Current = new PostgreSqlStorage(Configuration.GetConnectionString("HangfirePostgreConnection"));
            //services.AddHangfire(x => x.UsePostgreSqlStorage(Configuration.GetConnectionString("HangfirePostgreConnection")));
            services.AddHangfire(x => x.UsePostgreSqlStorage(
                Configuration.GetConnectionString("HangfirePostgreConnection"), // connection string
                hangfireOption// no overload without options, so just pass the default or configured options
            ));
            //services.AddHangfireServer();
            services.AddHangfireServer(options =>
            {
                options.Queues = new[] { "alpha", "beta", "default" };
            });

            /// tusdotnet Start
            //services.AddCors();            
           
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
            var options = new BackgroundJobServerOptions { WorkerCount = 20 };
           // if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
           // {
           //     options = new BackgroundJobServerOptions { WorkerCount = Environment.ProcessorCount * 2 };
           // }
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
                var monitor = JobStorage.Current.GetMonitoringApi();
                var toDelete = new List<string>();
                foreach (QueueWithTopEnqueuedJobsDto queue in monitor.Queues())
                {
                    for (var i = 0; i < Math.Ceiling(queue.Length / 1000d); i++)
                    {
                        monitor.EnqueuedJobs(queue.Name, 1000 * i, 1000)
                            .ForEach(x => toDelete.Add(x.Key));
                    }
                }
                foreach (var jobId in toDelete)
                {
                    BackgroundJob.Delete(jobId);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("exception in clearing hangfire jobs "+ex.ToString());
            }           
            try
            {
                RecurringJob.AddOrUpdate<Synergy.App.Business.HangfireScheduler>(x => x.SocialMediaKeywordDataMigrationToElasticDB1(), Cron.HourInterval(5));
                RecurringJob.AddOrUpdate<Synergy.App.Business.HangfireScheduler>(x => x.TrendingSocialDataMigrationToElasticDB1(), Cron.HourInterval(5));
                RecurringJob.AddOrUpdate<Synergy.App.Business.HangfireScheduler>(x => x.SocialMediaKeywordDataMigrationToElasticDB2(), Cron.HourInterval(1));
                RecurringJob.AddOrUpdate<Synergy.App.Business.HangfireScheduler>(x => x.TrendingSocialDataMigrationToElasticDB2(), Cron.HourInterval(1));

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
       
    }
}
