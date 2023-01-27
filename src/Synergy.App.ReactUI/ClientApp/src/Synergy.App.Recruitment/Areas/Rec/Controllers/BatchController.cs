using Synergy.App.Business;
using Synergy.App.Common;
using CMS.UI.Utility;
using Synergy.App.ViewModel;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.UI.Web.Areas.Rec.Controllers
{
    [Area("Rec")]
    public class BatchController : Controller
    {

        private IBatchBusiness _batchBusiness;
        private IJobAdvertisementBusiness _jobAddBusiness;
        private IMasterBusiness _masterBusiness;      
        private IListOfValueBusiness _lovBusiness;
        private IRecTaskBusiness _recTaskBusiness;
        private readonly IPortalBusiness _portalBusiness;
        private readonly IPushNotificationBusiness _notificationBusiness;
        private readonly IUserBusiness _userBusiness;
        private readonly IUserContext _userContext;
        private readonly IManpowerRecruitmentSummaryBusiness _manpowerBusiness;
        public BatchController(IBatchBusiness batchBusiness, IPortalBusiness portalBusiness, IRecTaskBusiness recTaskBusiness, IManpowerRecruitmentSummaryBusiness manpowerBusiness,
            IJobAdvertisementBusiness jobAddBusiness, IListOfValueBusiness lovBusiness, IPushNotificationBusiness notiificationBusiness, IUserBusiness userBusiness, IUserContext userContext, IMasterBusiness masterBusiness)
        {
            _batchBusiness = batchBusiness;
            _portalBusiness = portalBusiness;
            _jobAddBusiness = jobAddBusiness;
            _lovBusiness = lovBusiness;
            _notificationBusiness = notiificationBusiness;
            _userBusiness = userBusiness;
            _userContext = userContext;
            _masterBusiness = masterBusiness;
            _recTaskBusiness = recTaskBusiness;
            _manpowerBusiness = manpowerBusiness;
        }




        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        //public async Task<JsonResult> GetBatchList()
        //{
        //    var data = await _BatchBusiness.GetList();
        //    var res = from d in data
        //              where d.Status != StatusEnum.Inactive
        //              select d;
        //    return Json(res);
        //}
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
       

    public async Task<ActionResult> ReadData([DataSourceRequest] DataSourceRequest request,string jobIdAdvertismentId,BatchTypeEnum batchtype,string orgId,string batchId)
        {
            //var model = _batchBusiness.GetList(x=>x.JobAdvertisementId== jobIdAdvertismentId);
            var model = await _batchBusiness.GetBatchData(jobIdAdvertismentId, batchtype, orgId);
            if (batchId.IsNotNullAndNotEmpty() && batchId != "null")
            {
                model = model.Where(x => x.Id == batchId).ToList();
            }
            else
            {
                model = model.Where(x => x.BatchStatusCode == "Draft").ToList();
            }
            //if (batchtype==BatchTypeEnum.WorkerAppointment) 
            //{
            //    model = await _batchBusiness.GetWorkerBatchData(batchtype);
            //}
            var dsResult = model.ToDataSourceResult(request);
            return Json(dsResult);
        }
        public async Task<ActionResult> ReadBatchHmData([DataSourceRequest] DataSourceRequest request, string jobAdvertismentId,string orgId, string hmId, BatchTypeEnum batchtype)
        {
            //var model = _batchBusiness.GetList(x=>x.JobAdvertisementId== jobIdAdvertismentId);
            IList<BatchViewModel> model1 = await _batchBusiness.GetBatchHmData(jobAdvertismentId,orgId,hmId, batchtype,null);
            var model = model1.Where(x => x.BatchStatusCode == "PendingwithHM").ToList(); 
            var dsResult = model.ToDataSourceResult(request);
            return Json(dsResult);
        }
        public async Task<ActionResult> ReadCloseBatchHmData([DataSourceRequest] DataSourceRequest request, string jobAdvertismentId, string orgId, string hmId, BatchTypeEnum batchtype,string batchId)
        {
            IList<BatchViewModel> model1 = await _batchBusiness.GetBatchHmData(jobAdvertismentId, orgId, hmId, batchtype, batchId);
            //var model = model1.Where(x => x.BatchStatusCode == "Close").ToList();
            var dsResult = model1.ToDataSourceResult(request);
            return Json(dsResult);
        }
        public async Task<IActionResult>  Create(string jobAdvertisementId,BatchTypeEnum batchType,string orgId,string taskId)
        {
            var model = new BatchViewModel();

            model.DataAction = DataActionEnum.Create;
            model.JobId = jobAdvertisementId.IsNotNullAndNotEmpty()? jobAdvertisementId:null;
            model.BatchType = batchType;
            model.OrganizationId = orgId;
            //if (batchType== BatchTypeEnum.WorkerAppointment) 
            //{               
            //    model.Name = await _batchBusiness.GenerateNextBatchName("WB-") ;
            //}
            //if (batchType == BatchTypeEnum.ShortlistByHr)
            //{
            //var jobName = await _masterBusiness.GetJobNameById(jobAdvertisementId); 
            //var OrgName = await _masterBusiness.GetOrgNameById(orgId);
            //model.Name = await _batchBusiness.GenerateNextBatchNameUsingOrg(/*OrgName.Name +"_"+*/jobName.Name+"_");
            // }


            var status = await _lovBusiness.GetSingle(x => x.ListOfValueType == "BatchStatus" && x.Code == "Draft");
            model.BatchStatus = status.Id;
            if (jobAdvertisementId.IsNotNullAndNotEmpty() && orgId.IsNotNullAndNotEmpty())
            {
                var manpower = await _manpowerBusiness.GetList(x => x.JobId == jobAdvertisementId && x.OrganizationId == orgId);
                if (manpower.Count == 0)
                {
                    ViewBag.Message = "Please Create Manpower Summary";
                }
                else
                {
                    ViewBag.Message = "";
                }
            }
            if (taskId.IsNotNullAndNotEmpty())
            {
                var task = await _recTaskBusiness.GetSingleById(taskId);
                var status1 = await _lovBusiness.GetSingle(x => x.ListOfValueType == "BatchStatus" && x.Code == "PendingwithHM");
                model.BatchStatus = status1.Id;
                if (task.TextValue5.IsNotNullAndNotEmpty())
                {
                    var Batch = await _batchBusiness.GetSingleById(task.TextValue5);
                    if (Batch != null)
                    {

                        Batch.DataAction = DataActionEnum.Edit;
                        return View("Manage", Batch);
                    }
                }
            }
           

            return View("Manage", model);
        }
        public async Task<IActionResult> GenerateBatchNo(string JobId,string OrgId) 
        {
            var Name = "";
            if (JobId.IsNotNullAndNotEmpty() && OrgId.IsNotNullAndNotEmpty())
            {
                var jobName = await _masterBusiness.GetJobNameById(JobId);
                var OrgName = await _masterBusiness.GetOrgNameById(OrgId);
                Name = await _batchBusiness.GenerateNextBatchNameUsingOrg(OrgName.Name + "_" + jobName.Name + "_");
            }
            return Json(new { success = true ,result = Name }) ;
        }
        public async Task<IActionResult> Edit(string Id,string status=null)
        {
            var Batch = await _batchBusiness.GetSingleById(Id);
            if (status== "PendingwithHM")
            {
                Batch.IsPending=true;
            }
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
            // Send Notification to selected Hm
            var batch = await _batchBusiness.GetSingleById(batchId);
            var user = await _userBusiness.GetSingleById(batch.HiringManager);
            var loggeduser = await _userBusiness.GetSingleById(_userContext.UserId);
            var notificationModel = new NotificationViewModel();
            notificationModel.Subject = batch.Name;
            notificationModel.Body = string.Concat("<div><h4>Hello ", user.Name, "</h4>You have received batch "+batch.Name+" to shortlist for interview</div> ");
            notificationModel.From = loggeduser.Email;
            notificationModel.ToUserId = user.Id;
            notificationModel.To = user.Email;
            var result = await _notificationBusiness.Create(notificationModel);

            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<ActionResult> UpdateBatchClose(string batchId, string code)
        {
            await _batchBusiness.UpdateStatus(batchId, code);
            var batchmodel = await _batchBusiness.GetSingleById(batchId);
            if (code == "Close")
            {
                // Send Notification for Close Batch
                //var notificationModel = new NotificationViewModel();
                //var touser = await _userBusiness.GetSingle(x => x.Id == batchmodel.CreatedBy && x.IsDeleted == false);
                //notificationModel.Subject = "Batch Closed";
                //notificationModel.Body = string.Concat("<div><h4>Hello ", touser.Name, "</h4>Batch Details:<br><br>Batch Name : ", batchmodel.Name,
                //                            "<br>Subject : ", batchmodel.Name, " Batch Closed", "<br><br>",
                //                            "</div> ");
                //notificationModel.From = _userContext.Email;
                //notificationModel.ToUserId = touser.Id;
                //notificationModel.To = touser.Email;
                //var result = await _notificationBusiness.Create(notificationModel);
                //if (result.IsSuccess)
                //{

                //}
            }
            return Json(new { success = true });
        }

            public async Task<IActionResult> GetApplicantCount(string Id)
        {
            var Batch = await _batchBusiness.GetBatchApplicantCount(Id);

            if (Batch != null)
            {

                return Json(new { success = true, count = Batch.NoOfApplication });
            }
            return Json(new { success = true, count = 0 });
        }
        public async Task<IActionResult> GetWorkerApplicantCount(string Id)
        {
            var Batch = await _batchBusiness.GetWorkerBatchApplicantCount(Id);

            if (Batch != null)
            {

                return Json(new { success = true, count = Batch.NoOfApplication });
            }
            return Json(new { success = true, count = 0 });
        }

    }
}