using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.ViewModel;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.UI.Web.Areas.Recruitment.Controllers
{
    [Area("Recruitment")]
    public class ContractOfferForEmploymentController : Controller
    {

        private IBatchBusiness _batchBusiness;
        private readonly IPortalBusiness _portalBusiness;
        private readonly IRecruitmentElementBusiness _recruitmentElementBusiness;
        private readonly IApplicationBusiness _applicationBusiness;


        public ContractOfferForEmploymentController(IBatchBusiness batchBusiness, IPortalBusiness portalBusiness, IRecruitmentElementBusiness recruitmentElementBusiness,
            IApplicationBusiness applicationBusiness)
        {
            _batchBusiness = batchBusiness;
            _portalBusiness = portalBusiness;
            _recruitmentElementBusiness = recruitmentElementBusiness;
            _applicationBusiness = applicationBusiness;
        }

        
        public async Task<IActionResult> Index(string applicationId)
        {
            var offerdetails = await _applicationBusiness.GetOfferDetails(applicationId);
            offerdetails.ContractYear = Convert.ToDateTime(offerdetails.ContractStartDate).Year;
            offerdetails.FullName = String.Concat(offerdetails.FirstName, ' ', offerdetails.MiddleName, ' ', offerdetails.LastName);
            offerdetails.CandJoiningDate = offerdetails.JoiningNotLaterThan;            
            return View("Index", offerdetails);

            // return View();
        }
        [HttpGet]
        
        public async Task<JsonResult> GetBatchList()
        {
           // var data = await _batchBusiness.GetList();
            List<BatchViewModel> res = new List<BatchViewModel>();
            res.Add(new BatchViewModel { BatchStatus = "Draft" });
            res.Add(new BatchViewModel { BatchStatus = "Pending with HM" });
            res.Add(new BatchViewModel { BatchStatus = "Close" });

            return Json(res);
        }
        public async Task<JsonResult> GetJobIdList()
        {
           // var data = await _batchBusiness.GetList();
            List< BatchViewModel> res = new List<BatchViewModel>();
           // res.Add(new BatchViewModel { JobId = "3110"});
           // res.Add(new BatchViewModel { JobId = "9444" });
           // res.Add(new BatchViewModel { JobId = "7777" });

            return Json(res);
        }
       

    public async Task<ActionResult> ReadData([DataSourceRequest] DataSourceRequest request,string jobIdAdvertismentId)
        {
            //var model = _batchBusiness.GetList(x=>x.JobAdvertisementId== jobIdAdvertismentId);
            var model = await _batchBusiness.GetBatchData(jobIdAdvertismentId);
           
            var dsResult = model.ToDataSourceResult(request);
            return Json(dsResult);
        }
        public IActionResult Create(string jobAdvertisementId)
        {
            return View("Manage", new BatchViewModel
            {
                DataAction = DataActionEnum.Create,
                JobAdvertisementId = jobAdvertisementId,
                
                
            });
        }
        public async Task<IActionResult> Edit(string Id)
        {
            var Batch = await _batchBusiness.GetSingleById(Id);

            if (Batch != null)
            {

                Batch.DataAction = DataActionEnum.Edit;
                return View("Manage", Batch);
            }
            return View("Manage", new BatchViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Manage(BatchViewModel model)
        {
            if (ModelState.IsValid)
            {

                if (model.DataAction == DataActionEnum.Create)
                {
                    var result = await _batchBusiness.Create(model);
                    if (result.IsSuccess)
                    {
                        ViewBag.Success = true;

                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
                else if (model.DataAction == DataActionEnum.Edit)
                {
                    var result = await _batchBusiness.Edit(model);
                    if (result.IsSuccess)
                    {
                        ViewBag.Success = true;

                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
            }

            return View("Manage", model);
        }
        public async Task<IActionResult> Delete(string id)
        {
            await _batchBusiness.Delete(id);
            return View("Index", new BatchViewModel());
        }

        [HttpPost]
        public async Task<ActionResult> UpdateStatus(string batchId,string code)
        {
            await _batchBusiness.UpdateStatus(batchId, code);
            return Json(new { success = true });

        }

    }
}