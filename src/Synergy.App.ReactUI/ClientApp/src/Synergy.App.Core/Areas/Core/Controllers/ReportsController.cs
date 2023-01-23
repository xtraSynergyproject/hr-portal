namespace CMS.UI.Web.Controllers
{
    using Microsoft.AspNetCore.Cors;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using Synergy.App.Common;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Mail;
     
    using Microsoft.Extensions.Configuration;

    [Route("api/reports")]

    public class ReportsController //: ReportsControllerBase
    {
        //readonly string reportsPath = string.Empty;
        //public ReportsController(IReportServiceConfiguration reportServiceConfiguration)
        //   : base(reportServiceConfiguration)
        //{
        //}



        //[HttpGet("reportlist")]
        //public IEnumerable<string> GetReports()
        //{
        //    return Directory
        //        .GetFiles(this.reportsPath)
        //        .Select(path =>
        //            Path.GetFileName(path));
        //}
        //protected override HttpStatusCode SendMailMessage(MailMessage mailMessage)
        //{
        //    throw new System.NotImplementedException("This method should be implemented in order to send mail messages");
        //    //using (var smtpClient = new SmtpClient("smtp01.mycompany.com", 25))
        //    //{
        //    //    smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
        //    //    smtpClient.EnableSsl = false;

        //    //    smtpClient.Send(mailMessage);
        //    //}
        //    //return HttpStatusCode.OK;
        //}
        //public static byte[] GetFromBase64(string base64)
        //{
        //    if (base64 != null)
        //    {
        //        // Convert base 64 string to byte[]
        //        byte[] imageBytes = Convert.FromBase64String(base64);
        //        return imageBytes;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

    }

    public class ConfigurationService
    {
        public Microsoft.Extensions.Configuration.IConfiguration Configuration { get; private set; }

        public IWebHostEnvironment Environment { get; private set; }
        private string configFileName = "appsettings.json";

        public ConfigurationService(IWebHostEnvironment environment)
        {
            this.Environment = environment;

            var configFileName = System.IO.Path.Combine(environment.ContentRootPath, this.configFileName);
            var config = new Microsoft.Extensions.Configuration.ConfigurationBuilder()
                            .AddJsonFile(configFileName, true)
                            .Build();
            //var defaultBuilder = Microsoft.AspNetCore.WebHost.CreateDefaultBuilder()
            //    .Build();
            //var config = defaultBuilder.Services.GetService<IConfiguration>();

            this.Configuration = config;
        }
    }
}
