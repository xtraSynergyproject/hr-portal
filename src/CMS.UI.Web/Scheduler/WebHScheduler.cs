using Synergy.App.Business;
using Synergy.App.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace CMS.Web
{
    public class WebHScheduler
    {
        private static IServiceProvider _services;

        public WebHScheduler(IServiceProvider services)
        {
            _services = services;
        }

        public async Task<bool> SendEmailUsingHangfire(string Id)
        {
            try
            {
                var _business = (IRecEmailBusiness)_services.GetService(typeof(IRecEmailBusiness));
                var model = await _business.GetSingleById(Id);
                var result = await _business.SendMail(model);
                return true;
            }
            catch (Exception ex)
            {
                //Console.Write(ex.ToString());
                //Console.ReadLine();
                return false;
            }

        }
        public async Task<bool> UpdateTaskAndServiceStatus()
        {
            try
            {
                var _business = (IRecTaskBusiness)_services.GetService(typeof(IRecTaskBusiness));
                var model = await _business.UpdateOverdueTaskAndServiceStatus(DateTime.Now);                
                return true;
            }
            catch (Exception ex)
            {
                //Console.Write(ex.ToString());
                //Console.ReadLine();
                return false;
            }

        }

        public async Task<bool> UpdateJobAdvertisementStatus()
        {
            try
            {
                var _business = (IJobAdvertisementBusiness)_services.GetService(typeof(IJobAdvertisementBusiness));
                await _business.UpdateJobAdvertisementStatus();
                return true;
            }
            catch (Exception ex)
            {
                //Console.Write(ex.ToString());
                //Console.ReadLine();
                return false;
            }

        }
    }
}
