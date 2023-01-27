using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.ViewModel;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.UI.Web.Areas.Recruitment.Controllers
{
    [Area("Recruitment")]
    public class JobDescriptionController : ApplicationController
    {
        private readonly IJobDescriptionBusiness _jobDescriptionBusiness;
        private readonly IListOfValueBusiness _listOfValueBusiness;
        private readonly IJobDescriptionCriteriaBusiness _jobDescriptionCriteriaBusiness;

        public JobDescriptionController(IJobDescriptionBusiness jobDescriptionBusiness
            , IListOfValueBusiness listOfValueBusiness
            , IJobDescriptionCriteriaBusiness jobDescriptionCriteriaBusiness)
        {
            _jobDescriptionBusiness = jobDescriptionBusiness;
            _listOfValueBusiness = listOfValueBusiness;
            _jobDescriptionCriteriaBusiness = jobDescriptionCriteriaBusiness;
        }

        public ActionResult Index()
        {
            return View();
        }

        //public IActionResult JobDescription(string jobId, string orgId)
        //{
        //    return View("JobDescription", new JobDescriptionViewModel
        //    {
        //        JobId = jobId,
        //        OrganizationId = orgId,
        //        DataAction = DataActionEnum.Create,                
        //    });
        //}

        public async Task<IActionResult> ViewJobDescription(string jobId, string orgId, string taskStatus)
        {
            var jd = await _jobDescriptionBusiness.GetSingle(x => x.JobId == jobId);

                return View("ViewJobDescription", jd);
            
         
        }



        public async Task<IActionResult> EditJobDescription(string jobId, string orgId,string taskStatus)
        {
            var jd = await _jobDescriptionBusiness.GetSingle(x => x.JobId == jobId);

            if (jd != null)
            {
                jd.TaskStatus = taskStatus;
                jd.DataAction = DataActionEnum.Edit;
                return View("JobDescription", jd);
            }
            else
            {
                return View("JobDescription", new JobDescriptionViewModel
                {
                    JobId = jobId,
                   // OrganizationId = orgId,
                    DataAction = DataActionEnum.Create,
                    TaskStatus = taskStatus
                });
            }
            //return View("JobDescription", new JobDescriptionViewModel());
        }
        

        [HttpPost]
        public async Task<IActionResult> ManageJobDescription(JobDescriptionViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (model.DataAction == DataActionEnum.Create)
                {
                    var result = await _jobDescriptionBusiness.Create(model);
                    if (result.IsSuccess)
                    {
                        if (model.JobCriteria.IsNotNull())
                        {
                            var jobcriteria = JsonConvert.DeserializeObject<List<JobDescriptionCriteriaViewModel>>(model.JobCriteria);

                            var jc = new JobDescriptionCriteriaViewModel();
                            foreach (var a in jobcriteria)
                            {
                                a.JobDescriptionId = result.Item.Id;
                                a.Type = "Criteria";
                                jc = a;
                                await _jobDescriptionCriteriaBusiness.Create(jc);
                            }
                        }

                        if (model.Skills.IsNotNull())
                        {
                            var jobcriteria = JsonConvert.DeserializeObject<List<JobDescriptionCriteriaViewModel>>(model.Skills);
                            var jc = new JobDescriptionCriteriaViewModel();
                            foreach (var a in jobcriteria)
                            {
                                a.JobDescriptionId = result.Item.Id;
                                a.Type = "Skills";
                                jc = a;
                                await _jobDescriptionCriteriaBusiness.Create(jc);
                            }
                        }
                        if (model.OtherInformation.IsNotNull())
                        {
                            var jobcriteria = JsonConvert.DeserializeObject<List<JobDescriptionCriteriaViewModel>>(model.OtherInformation);
                            var jc = new JobDescriptionCriteriaViewModel();
                            foreach (var a in jobcriteria)
                            {
                                a.JobDescriptionId = result.Item.Id;
                                a.Type = "OtherInformation";
                                jc = a;
                                await _jobDescriptionCriteriaBusiness.Create(jc);
                            }
                        }
                        ViewBag.Success = true;
                        model.Id = result.Item.Id;
                        //return PopupRedirect("Menu Group created successfully", true);
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
                else if (model.DataAction == DataActionEnum.Edit)
                {
                    var result = await _jobDescriptionBusiness.Edit(model);
                    if (result.IsSuccess)
                    {
                        if (model.JobCriteria.IsNotNull())
                        {
                            var jobcriteria = JsonConvert.DeserializeObject<List<JobDescriptionCriteriaViewModel>>(model.JobCriteria);
                            var existingjobcriteria = await _jobDescriptionBusiness.GetJobDescCriteriaList("Criteria", model.Id);
                            if (existingjobcriteria.IsNotNull())
                            {
                                foreach (var p in existingjobcriteria)
                                {

                                    await _jobDescriptionCriteriaBusiness.Delete(p.Id);

                                }
                            }
                            if (jobcriteria.IsNotNull())
                            {
                                foreach (var p in jobcriteria)
                                {
                                    var res = await _jobDescriptionCriteriaBusiness.Create(new JobDescriptionCriteriaViewModel
                                    {
                                        JobDescriptionId = result.Item.Id,
                                        Type = "Criteria",
                                        Weightage = p.Weightage,
                                        CriteriaType = p.CriteriaType,
                                        Criteria = p.Criteria
                                    });

                                }
                            }


                            //var jc = new JobCriteriaViewModel();
                            //foreach (var a in jobcriteria)
                            //{
                            //    foreach(var ex in existingjobcriteria)
                            //    {
                            //        if (ex.Id == a.Id)
                            //        {
                            //            a.JobAdvertisementId = result.Item.Id;
                            //            a.Type = "Criteria";
                            //            jc = a;
                            //            if (jc.Id != "")
                            //            {
                            //                await _jobCriteriaBusiness.Edit(jc);
                            //            }
                            //            else
                            //            {
                            //                await _jobCriteriaBusiness.Create(jc);
                            //            }

                            //        }
                            //        else
                            //        {
                            //            await _jobCriteriaBusiness.Delete(ex.Id);
                            //        }
                            //    }

                            //}
                        }

                        if (model.Skills.IsNotNull())
                        {
                            var jobcriteria = JsonConvert.DeserializeObject<List<JobDescriptionCriteriaViewModel>>(model.Skills);
                            var existingjobcriteria = await _jobDescriptionBusiness.GetJobDescCriteriaList("Skills", model.Id);
                            if (existingjobcriteria.IsNotNull())
                            {
                                foreach (var p in existingjobcriteria)
                                {
                                    await _jobDescriptionCriteriaBusiness.Delete(p.Id);
                                }
                            }
                            if (jobcriteria.IsNotNull())
                            {
                                foreach (var p in jobcriteria)
                                {

                                    var res = await _jobDescriptionCriteriaBusiness.Create(new JobDescriptionCriteriaViewModel
                                    {
                                        JobDescriptionId = result.Item.Id,
                                        Type = "Skills",
                                        Weightage = p.Weightage,
                                        CriteriaType = p.CriteriaType,
                                        Criteria = p.Criteria
                                    });

                                }
                            }

                            //var jc = new JobCriteriaViewModel();
                            //foreach (var a in jobcriteria)
                            //{
                            //    a.JobAdvertisementId = result.Item.Id;
                            //    a.Type = "Skills";
                            //    jc = a;
                            //    if (jc.Id != "")
                            //    {
                            //        await _jobCriteriaBusiness.Edit(jc);
                            //    }
                            //    else
                            //    {
                            //        await _jobCriteriaBusiness.Create(jc);
                            //    }
                            //}
                        }
                        if (model.OtherInformation.IsNotNull())
                        {
                            var jobcriteria = JsonConvert.DeserializeObject<List<JobDescriptionCriteriaViewModel>>(model.OtherInformation);
                            var existingjobcriteria = await _jobDescriptionBusiness.GetJobDescCriteriaList("OtherInformation", model.Id);
                            if (existingjobcriteria.IsNotNull())
                            {
                                foreach (var p in existingjobcriteria)
                                {
                                    await _jobDescriptionCriteriaBusiness.Delete(p.Id);
                                }
                            }
                            if (jobcriteria.IsNotNull())
                            {
                                foreach (var p in jobcriteria)
                                {

                                    var res = await _jobDescriptionCriteriaBusiness.Create(new JobDescriptionCriteriaViewModel
                                    {
                                        JobDescriptionId = result.Item.Id,
                                        Type = "OtherInformation",
                                        Weightage = p.Weightage,
                                        CriteriaType = p.CriteriaType,
                                        Criteria = p.Criteria,
                                        ListOfValueTypeId = p.ListOfValueTypeId
                                    });

                                }
                            }

                            //var jc = new JobCriteriaViewModel();
                            //foreach (var a in jobcriteria)
                            //{
                            //    a.JobAdvertisementId = result.Item.Id;
                            //    a.Type = "OtherInformation";
                            //    jc = a;
                            //    if (jc.Id != "")
                            //    {
                            //        await _jobCriteriaBusiness.Edit(jc);
                            //    }
                            //    else
                            //    {
                            //        await _jobCriteriaBusiness.Create(jc);
                            //    }
                            //}
                        }

                        ViewBag.Success = true;
                        //return PopupRedirect("Menu Group edited successfully", true);                        
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
            }
            //return View("JobDescription", model);
            return Json(new { success = true });
        }       

        public async Task<IActionResult> Delete(string id)
        {
            await _jobDescriptionBusiness.Delete(id);            
            return Json(new { success = true });
        }

        public async Task<IActionResult> ReadJobDescCriteriaData([DataSourceRequest] DataSourceRequest request, string jobdescid)
        {
            var data = new List<JobDescriptionCriteriaViewModel>();
            ViewBag.JobCriterias = await _listOfValueBusiness.GetList(x => x.ListOfValueType == "CRITERIATYPE");
            if (jobdescid.IsNotNullAndNotEmpty())
            {
                var model = await _jobDescriptionBusiness.GetJobDescCriteriaList("Criteria", jobdescid);
                data = model.ToList();
            }
            var dsResult = data.ToDataSourceResult(request);
            return Json(dsResult);
        }
        public async Task<IActionResult> ReadDescSkillsData([DataSourceRequest] DataSourceRequest request, string jobdescid)
        {
            ViewBag.JobCriterias = await _listOfValueBusiness.GetList(x => x.ListOfValueType == "CRITERIATYPE");
            var data = new List<JobDescriptionCriteriaViewModel>();
            if (jobdescid.IsNotNullAndNotEmpty())
            {
                // var model =await _jobCriteriaBusiness.GetList(x => x.Type == "Skills" && x.JobAdvertisementId == jobadvtid);
                var model = await _jobDescriptionBusiness.GetJobDescCriteriaList("Skills", jobdescid);
                data = model.ToList();
            }
            var dsResult = data.ToDataSourceResult(request);
            return Json(dsResult);
        }
        public async Task<IActionResult> ReadDescInfoData([DataSourceRequest] DataSourceRequest request, string jobdescid)
        {
            ViewBag.JobCriterias = await _listOfValueBusiness.GetList(x => x.ListOfValueType == "CRITERIATYPE");
            var data = new List<JobDescriptionCriteriaViewModel>();
            if (jobdescid.IsNotNullAndNotEmpty())
            {
                // var model = _jobCriteriaBusiness.GetList(x => x.Type == "OtherInformation" && x.JobAdvertisementId == jobadvtid);
                var model = await _jobDescriptionBusiness.GetJobDescCriteriaList("OtherInformation", jobdescid);
                data = model.ToList();
            }
            //  var data = model.Result.ToList();
            var dsResult = data.ToDataSourceResult(request);
            return Json(dsResult);
        }
        public async Task<ActionResult> GetCriteriaNameById(string id, string type)
        {
            var data = await _listOfValueBusiness.GetList(x => x.ListOfValueType == type);
            var res = from d in data
                      where d.Status != StatusEnum.Inactive
                      select d;
            var name = res.Where(x => x.Id == id).Select(x => x.Name).FirstOrDefault();
            return Json(name);
        }
        public async Task<ActionResult> GetOtherLOVNameById(string id)
        {
            var data = await _listOfValueBusiness.GetList(x => x.Id == id);
            var res = from d in data
                      where d.Status != StatusEnum.Inactive
                      select d;
            var name = res.Where(x => x.Id == id).Select(x => x.Name).FirstOrDefault();
            return Json(name);
        }

        public IActionResult CreateList(string jobid, string lovtype, string id)
        {
            return View("ManageListOfValue", new ListOfValueViewModel
            {
                ReferenceTypeCode = ReferenceTypeEnum.REC_Job,
                ReferenceTypeId = jobid,
                Code = lovtype,
                Id = id,
                DataAction=DataActionEnum.Create
            }); ;
        }
        public IActionResult ViewListOfValue(string jobid)
        {
            return View("ViewListOfValue", new ListOfValueViewModel
            {
                ReferenceTypeCode = ReferenceTypeEnum.REC_Job,
                ReferenceTypeId = jobid,
                //Code = lovtype,
                // Id = id

            });
        }
        public async Task<IActionResult> EditList(string Id)
        {
            var ListOfValue = await _listOfValueBusiness.GetSingleById(Id);

            if (ListOfValue != null)
            {

                ListOfValue.DataAction = DataActionEnum.Edit;
                return View("ManageListOfValue", ListOfValue);
            }
            return View("ManageListOfValue", new ListOfValueViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> ManageListOfValue(ListOfValueViewModel model)
        {
            if (ModelState.IsValid)
            {
                string lovcode = model.Name;
                model.Code = String.Concat(lovcode.Where(c => !Char.IsWhiteSpace(c)));
                model.ListOfValueType = "LOV_TYPE";
                if (model.Id == null)
                {

                    var result = await _listOfValueBusiness.Create(model);

                    if (result.IsSuccess)
                    {
                        if (model.Json.IsNotNullAndNotEmpty())
                        {
                            var json = JsonConvert.DeserializeObject<List<ListOfValueViewModel>>(model.Json);
                            foreach (var item in json)
                            {
                                item.Code = String.Concat(item.Name.Where(c => !Char.IsWhiteSpace(c)));
                                item.ListOfValueType = result.Item.Code;
                                item.ReferenceTypeId = result.Item.ReferenceTypeId;
                                item.ReferenceTypeCode = result.Item.ReferenceTypeCode;
                                item.ParentId = result.Item.Id;
                                var r = await _listOfValueBusiness.Create(item);
                            }
                            return Json(new { success = true, lovtype = result.Item.Code, id = result.Item.Id });
                        }
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }

            }

            return View("ManageListOfValue", model);
        }
      
     
        public async Task<ActionResult> GetList([DataSourceRequest] DataSourceRequest request, string jobid, string lovtype)
        {
            var data = await _listOfValueBusiness.GetList(x => x.ReferenceTypeCode == ReferenceTypeEnum.REC_Job && x.ListOfValueType == lovtype && x.ReferenceTypeId == jobid && x.Status != StatusEnum.Inactive);
            return Json(data.ToDataSourceResult(request));
        }
    }
}