using CMS.Business;
using CMS.Common;
using CMS.UI.ViewModel;
using CMS.Web.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace CMS.Web.Api.Areas.Recruitment.Controllers
{
    [Route("recruitment/query")]
    [ApiController]
    public class QueryController : ControllerBase
    {
        private readonly IServiceProvider _serviceProvider;

        private AuthSignInManager<ApplicationIdentityUser> _customUserManager;

        public QueryController(AuthSignInManager<ApplicationIdentityUser> customUserManager
            , IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

        }
        [HttpGet]
        [Route("GetPageForExecution")]
        public async Task<IActionResult> GetPageForExecution(string pageId)
        {
            try
            {
                var _pageBusiness = (IPageBusiness)_serviceProvider.GetService(typeof(IPageBusiness));
                var page = await _pageBusiness.GetPageForExecution(pageId);
                return Ok(page);
            }
            catch (Exception)
            {

                throw;
            }

        }
        [HttpGet]
        [Route("GetJobCategoryListOfValue")]
        public async Task<IActionResult> GetJobCategoryListOfValue()
        {
            try
            {
                var _lovBusiness = (IListOfValueBusiness)_serviceProvider.GetService(typeof(IListOfValueBusiness));
                var jcl = await _lovBusiness.GetList(x => x.ListOfValueType == "JOB_CATEGORY" && x.Status != StatusEnum.Inactive, x => x.Parent);
                return Ok(jcl.OrderBy(x => x.SequenceOrder).ToList());
            }
            catch (Exception)
            {

                throw;
            }

        }
        [HttpGet]
        [Route("GetJobCategoryListOfValueWithCount")]
        public async Task<IActionResult> GetJobCategoryListOfValueWithCount(string agencyId)
        {
            try
            {
                var _jobAdvertisementBusiness = (IJobAdvertisementBusiness)_serviceProvider.GetService(typeof(IJobAdvertisementBusiness));
                var data = await _jobAdvertisementBusiness.GetJobAdvertisementListWithCount(agencyId);
                return Ok(data);
            }
            catch (Exception)
            {

                throw;
            }

        }
        [HttpGet]
        [Route("GetJobAdvertisementList")]
        public async Task<IActionResult> GetJobAdvertisementList(string keyWord, string categoryId, string locationId, string manpowerType, string agencyId)
        {
            try
            {
                var _jobAdvertisementBusiness = (IJobAdvertisementBusiness)_serviceProvider.GetService(typeof(IJobAdvertisementBusiness));
                var data = await _jobAdvertisementBusiness.GetJobAdvertisementList(keyWord, categoryId, locationId, manpowerType, agencyId);
                return Ok(data);
            }
            catch (Exception)
            {

                throw;
            }

        }
        [HttpGet]
        [Route("GetManpowerTypeListOfValue")]
        public async Task<IActionResult> GetManpowerTypeListOfValue()
        {
            try
            {
                var _lovBusiness = (IListOfValueBusiness)_serviceProvider.GetService(typeof(IListOfValueBusiness));
                var data = await _lovBusiness.GetList(x => x.ListOfValueType == "LOV_MANPOWERTYPE" && x.Status != StatusEnum.Inactive);
                return Ok(data);
            }
            catch (Exception)
            {

                throw;
            }

        }
        [HttpGet]
        [Route("GetJobNameById")]
        public async Task<IActionResult> GetJobNameById(string jobId)
        {
            try
            {
                var _jobAdvertisementBusiness = (IJobAdvertisementBusiness)_serviceProvider.GetService(typeof(IJobAdvertisementBusiness));
                var page = await _jobAdvertisementBusiness.GetNameById(jobId);
                return Ok(page);
            }
            catch (Exception)
            {

                throw;
            }

        }
        [HttpGet]
        [Route("GetJobIdNameListByJobAdvertisement")]
        public async Task<IActionResult> GetJobIdNameListByJobAdvertisement(string jobId)
        {
            try
            {
                var _jobAdvertisementBusiness = (IJobAdvertisementBusiness)_serviceProvider.GetService(typeof(IJobAdvertisementBusiness));
                var page = await _jobAdvertisementBusiness.GetJobIdNameListByJobAdvertisement(jobId);
                return Ok(page);
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
