using AutoMapper;
using AutoMapper.Data;
using CMS.Business;
using CMS.Data.Repository;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Web.Scheduler
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
            services.AddControllersWithViews();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddSingleton(Configuration);
            services.AddEntityFrameworkNpgsql().AddDbContext<PostgreDbContext>(opt =>
  opt.UseNpgsql(Configuration.GetConnectionString("PostgreConnection")));
            var configuration = new MapperConfiguration(cfg =>
            {
                ConfigureMapper(cfg);
            });
            var mapper = configuration.CreateMapper();
            services.AddSingleton(mapper);

            BusinessHelper.RegisterDependency(services);

            services.AddHangfire(x => x.UsePostgreSqlStorage(Configuration.GetConnectionString("HangfirePostgreConnection")));
            services.AddHangfireServer();
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
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();


            app.UseAuthorization();
            app.UseHangfireDashboard();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHangfireDashboard();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            // app.UseHangfireDashboard();
          
            try
                {
                    RecurringJob.AddOrUpdate<CMS.Business.HangfireScheduler>(x => x.UpdateJobAdvertisementStatus(), Cron.Daily);
                }
                catch (Exception ex)
                {

                }

            try
                {
                    RecurringJob.AddOrUpdate<CMS.Business.HangfireScheduler>(x => x.UpdateTaskAndServiceStatus(), "0 */4 * * *");
                }
                catch (Exception ex)
                {

                }
            try
                {
                    RecurringJob.AddOrUpdate<CMS.Business.HangfireScheduler>(x => x.SendEmailSummary(), Cron.Daily(4));
                }
                catch (Exception ex)
                {

                }
            
        }
    }
}
