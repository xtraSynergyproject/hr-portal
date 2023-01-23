using AutoMapper.Data;
using CMS.Data.Repository;
using CMS.UI.Web.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Business.Test
{
    public class TestHelper
    {
        public static ServiceProvider Setup(string userId, string portalName = "", string legalEntityCode = "")
        {
            var serviceProvider = new ServiceCollection();
            var configuration = new AutoMapper.MapperConfiguration(cfg =>
            {
                ConfigureMapper(cfg);
            });


            var mapper = configuration.CreateMapper();
            var configs = new Dictionary<string, string>();
            configs.Add("PostgreConnection", "Host=95.111.235.64;Port=5432;User ID=postgres;Password=!Welcome123;Database=CMS;MaxPoolSize=50;CommandTimeout=120;");

            var c = new ConfigurationBuilder().AddInMemoryCollection(configs).Build();
            serviceProvider.AddSingleton<IConfiguration>(c);
            serviceProvider.AddSingleton(mapper);
            serviceProvider.AddDbContext<ApplicationDbContext>(option => option.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=aspnet-BRE.UI.Web-884B3809-77BC-4B31-A8DA-B7BAB5D43278;Trusted_Connection=True;MultipleActiveResultSets=true"));
            serviceProvider.AddEntityFrameworkNpgsql().AddDbContext<PostgreDbContext>(opt => opt.UseNpgsql("Host=95.111.235.64;Port=5432;User ID=postgres;Password=!Welcome123;Database=CMS;MaxPoolSize=50;CommandTimeout=120;"));
            serviceProvider.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            BusinessHelper.RegisterDependency(serviceProvider);
            return serviceProvider.BuildServiceProvider();
        }
        private static void ConfigureMapper(AutoMapper.IMapperConfigurationExpression cfg)
        {
            cfg.AddDataReaderMapping(false);
            var profile = new MappingProfile();
            profile.AddDataRecordMember();
            cfg.AddProfile(profile);

        }
    }
}
