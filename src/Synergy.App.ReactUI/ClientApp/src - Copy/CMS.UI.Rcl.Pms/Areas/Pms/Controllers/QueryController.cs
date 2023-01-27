using CMS.Business;
using CMS.Common;
using CMS.UI.ViewModel;
using CMS.UI.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Web.Areas.Pms.Controllers
{
    [Route("pms/query")]
    [ApiController]
    public class QueryController : Controller
    {
        private readonly IServiceProvider _serviceProvider;

        private AuthSignInManager<ApplicationIdentityUser> _customUserManager;

        public QueryController(AuthSignInManager<ApplicationIdentityUser> customUserManager
            , IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

        }
        [HttpGet]
        [Route("GetUserPerformanceDocumentInfo/{userId}/{PerformanceId}")]
        public async Task<IActionResult> GetUserPerformanceDocumentInfo(string userId, string PerformanceId,string masterStageId)
        {
            var _hRCoreBusiness = _serviceProvider.GetService<IHRCoreBusiness>();
            var _performanceManagementBusiness = _serviceProvider.GetService<IPerformanceManagementBusiness>();
            var _fileBusiness = _serviceProvider.GetService<IFileBusiness>();
            try
            {
                var performanceDocs = await _hRCoreBusiness.GetUserPerformanceDocumentInfo(userId, PerformanceId,masterStageId);
                var performanceDoc = performanceDocs.FirstOrDefault();
                if (performanceDoc != null)
                {
                    //foreach (var performance in performanceDoc)
                    //{                      
                    var stagedata = await _performanceManagementBusiness.ReadPerformanceDocumentStagesData(performanceDoc.PerformanceDocumentId);
                    if (stagedata != null)
                    {
                        performanceDoc.PDStage = stagedata.Where(x => x.ServiceStatusCode == "SERVICE_STATUS_INPROGRESS").FirstOrDefault();
                    }
                    performanceDoc.Goals = await _performanceManagementBusiness.ReadPerformanceDocumentGoalData(performanceDoc.PerformanceDocumentId, userId,masterStageId);
                    performanceDoc.Competency = await _performanceManagementBusiness.ReadPerformanceDocumentCompetencyData(performanceDoc.PerformanceDocumentId, userId, masterStageId);
                    //}
                    if (performanceDoc.SponsorLogoId.IsNotNullAndNotEmpty())
                    {
                        var spbytes = await _fileBusiness.GetFileByte(performanceDoc.SponsorLogoId);
                        if (spbytes.Length > 0)
                        {
                            performanceDoc.SponsorLogo = Convert.ToBase64String(spbytes, 0, spbytes.Length);
                        }

                    }
                }
                performanceDoc.CurrentDateText = DateTime.Today.ToDefaultDateFormat();

                return Ok(performanceDoc);
            }
            catch (Exception)
            {

                throw;
            }

        }

        [HttpGet]
        [Route("GetPerformanceDocumentServiceStepTask/{serviceId}")]
        public async Task<IActionResult> GetPerformanceDocumentServiceStepTask(string serviceId)
        {
            try
            {
                var _componentResultBusiness = _serviceProvider.GetService<IComponentResultBusiness>();
                var stepTaskList = await _componentResultBusiness.GetStepTaskList(serviceId);
                return Ok(stepTaskList);
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpGet]
        [Route("GetPerformanceDocumentServiceStepTaskByUser/{userId}/{PerformanceId}")]
        public async Task<IActionResult> GetPerformanceDocumentServiceStepTaskByUser(string userId, string PerformanceId, string masterStageId)
        {
            var _hRCoreBusiness = _serviceProvider.GetService<IHRCoreBusiness>();
            var _performanceManagementBusiness = _serviceProvider.GetService<IPerformanceManagementBusiness>();
            var stepTaskList = new List<TaskViewModel>();
            var _componentResultBusiness = _serviceProvider.GetService<IComponentResultBusiness>();
            try
            {
                var performanceDocs = await _hRCoreBusiness.GetUserPerformanceDocumentInfo(userId, PerformanceId,masterStageId);
                var performanceDoc = performanceDocs.FirstOrDefault();
                if (performanceDoc != null)
                {
                    //foreach (var performance in performanceDoc)
                    //{                      
                    var stagedata = await _performanceManagementBusiness.ReadPerformanceDocumentStagesData(performanceDoc.PerformanceDocumentId);
                    if (stagedata != null)
                    {
                        performanceDoc.PDStage = stagedata.Where(x => x.ServiceStatusCode == "SERVICE_STATUS_INPROGRESS").FirstOrDefault();
                    }
                    performanceDoc.Goals = await _performanceManagementBusiness.ReadPerformanceDocumentGoalData(performanceDoc.PerformanceDocumentId, userId,masterStageId);
                    performanceDoc.Competency = await _performanceManagementBusiness.ReadPerformanceDocumentCompetencyData(performanceDoc.PerformanceDocumentId, userId, masterStageId);
                    if (performanceDoc.Goals != null && performanceDoc.Goals.Count > 0)
                    {
                        foreach (var goal in performanceDoc.Goals)
                        {
                            var tasklist = await _componentResultBusiness.GetStepTaskList(goal.Id);
                            if (tasklist != null && tasklist.Count > 0)
                            {
                                stepTaskList.AddRange(tasklist);
                            }
                        }
                    }
                    if (performanceDoc.Competency != null && performanceDoc.Competency.Count > 0)
                    {
                        foreach (var comp in performanceDoc.Competency)
                        {
                            var tasklist = await _componentResultBusiness.GetStepTaskList(comp.Id);
                            if (tasklist != null && tasklist.Count > 0)
                            {
                                stepTaskList.AddRange(tasklist);
                            }
                        }
                    }
                }
                //Test Data for FastReport
                if (false)
                {
                    stepTaskList.Add(new TaskViewModel {ParentServiceId= "add36aba-3b09-4842-bd45-62ea89dfc15f", SequenceOrder=1,TaskSubject="StepTask01 Goal01",TaskStatusName="Completed",StartDate=DateTime.Now.AddDays(-5), DueDate=DateTime.Now.AddDays(-3),AssigneeUserName="Step user01",CompletedDate=DateTime.Now.AddDays(-1) });
                    stepTaskList.Add(new TaskViewModel {ParentServiceId= "add36aba-3b09-4842-bd45-62ea89dfc15f", SequenceOrder=2,TaskSubject= "StepTask02 Goal01", TaskStatusName="In Progress",StartDate=DateTime.Now.AddDays(-5), DueDate=DateTime.Now.AddDays(1),AssigneeUserName="Step user02",CompletedDate=null });
                    stepTaskList.Add(new TaskViewModel {ParentServiceId= "e0b3a34e-0681-42d2-a1ea-77a33ccb8afc", SequenceOrder=1,TaskSubject="StepTask01 Goal02",TaskStatusName="Overdue",StartDate=DateTime.Now.AddDays(-5), DueDate=DateTime.Now.AddDays(1),AssigneeUserName="Step user01",CompletedDate=null });
                    stepTaskList.Add(new TaskViewModel {ParentServiceId= "e0b3a34e-0681-42d2-a1ea-77a33ccb8afc", SequenceOrder=2,TaskSubject= "StepTask02 Goal02", TaskStatusName="Completed",StartDate=DateTime.Now.AddDays(-5), DueDate=DateTime.Now.AddDays(1),AssigneeUserName="Step user01",CompletedDate=null });
                    stepTaskList.Add(new TaskViewModel {ParentServiceId= "957e72d3-65b2-4a03-b04b-272b48da6130", SequenceOrder=1,TaskSubject="StepTask01 Compt01",TaskStatusName="Completed",StartDate=DateTime.Now.AddDays(-5), DueDate=DateTime.Now.AddDays(1),AssigneeUserName="Step user01",CompletedDate=null });
                    stepTaskList.Add(new TaskViewModel {ParentServiceId= "1", SequenceOrder=1,TaskSubject="StepTask01",TaskStatusName="Completed",StartDate=DateTime.Now.AddDays(-5), DueDate=DateTime.Now.AddDays(1),AssigneeUserName="Step user01",CompletedDate=null });
                    stepTaskList.Add(new TaskViewModel {ParentServiceId="1",SequenceOrder=1,TaskSubject="StepTask01",TaskStatusName="Completed",StartDate=DateTime.Now.AddDays(-5), DueDate=DateTime.Now.AddDays(1),AssigneeUserName="Step user01",CompletedDate=null });
                }
                //stepTaskList = await _componentResultBusiness.GetStepTaskList(serviceId);
                return Ok(stepTaskList);
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpGet]
        [Route("GetLetterTemplateDetails/{userId}/{PerformanceId}")]
        public async Task<IActionResult> GetLetterTemplateDetails(string userId, string PerformanceId, string masterStageId)
        {
            var _hRCoreBusiness = _serviceProvider.GetService<IHRCoreBusiness>();
            var _userBusiness = _serviceProvider.GetService<IUserBusiness>();
            var _companyBusiness = _serviceProvider.GetService<ICompanyBusiness>();
            var _fileBusiness = _serviceProvider.GetService<IFileBusiness>();

            try
            {
                var employeeName = "User01";
                var jobTitle = "Officer";
                var departmentName = "Finance";
                var gradeName = "G-10";
                var basicSalary = "1500";
                var performanceRating = "4";
                var bonus = "800";
                var increment = "600";
                var effectiveDate = System.DateTime.Today;
                var performanceDocument = "Performance Document";
                var performanceDocumentYear = "2021";
                var performanceDocumentStartDate = System.DateTime.Today;
                var performanceDocumentEndDate = System.DateTime.Today;

                var performanceDocs = await _hRCoreBusiness.GetUserLetterTemplateDetails(userId, PerformanceId, masterStageId);
                var performanceDoc = performanceDocs.FirstOrDefault();
                if (performanceDoc != null)
                {
                    employeeName = performanceDoc.PersonFullName;
                    jobTitle = performanceDoc.JobName;
                    departmentName = performanceDoc.DepartmentName;
                    gradeName = performanceDoc.GradeName;
                    basicSalary = performanceDoc.BasicSalary;
                    performanceRating = performanceDoc.PDFinalRatingRounded;
                    bonus = performanceDoc.PDBonus;
                    increment = performanceDoc.PDIncrement;
                    //effectiveDate = System.DateTime.Today;
                    performanceDocument = performanceDoc.PerformanceDocumentName;
                    performanceDocumentYear = performanceDoc.PDYear;
                    performanceDocumentStartDate = performanceDoc.PDStartDate.IsNotNull()? performanceDoc.PDStartDate.Value: System.DateTime.Today;
                    performanceDocumentEndDate = performanceDoc.PDEndDate.IsNotNull() ? performanceDoc.PDEndDate.Value : System.DateTime.Today;
                }

                var code = @"<p><strong>Dear {{EmployeeName}},</strong></p>
                             <p>Job Title : {{JobTitle}}</p>
                             <p>Department : {{DepartmentName}}</p>
                             <p>Grade : {{GradeName}}</p>
                             <p>Basic Salary : {{BasicSalary}}</p>
                             <p>Performance Rating : {{PerformanceRating}}</p>
                             <p>Bonus : {{Bonus}}</p>
                             <p>Increment : {{Increment}}</p>
                             <p>Effective Date : {{EffectiveDate}}</p>
                             <p></p>
                             <p>Performance Document : {{PerformanceDocument}}</p>
                             <p>Performance Document Year : {{PerformanceDocumentYear}}</p>
                             <p>Performance Document Start Date : {{PerformanceDocumentStartDate}}</p>
                             <p>Performance Document End Date : {{PerformanceDocumentEndDate}}</p>
                                <p style='text-align: justify'>Thank you very much for your last letter. It was great to hear from you after so many months.Thank you very much for your last letter. It was great to hear from you after so many months.</p><p style='text-align: justify'>Thank you very much for your last letter. It was great to hear from you after so many months.</p><p>Thank you.</p>";

                code = code.Replace("{{EmployeeName}}", employeeName);
                code = code.Replace("{{JobTitle}}", jobTitle);
                code = code.Replace("{{DepartmentName}}", departmentName);
                code = code.Replace("{{GradeName}}", gradeName);
                code = code.Replace("{{BasicSalary}}", basicSalary);
                code = code.Replace("{{PerformanceRating}}", performanceRating);
                code = code.Replace("{{Bonus}}", bonus);
                code = code.Replace("{{Increment}}", increment);
                code = code.Replace("{{EffectiveDate}}", effectiveDate.ToShortDateString());
                code = code.Replace("{{PerformanceDocument}}", performanceDocument);
                code = code.Replace("{{PerformanceDocumentYear}}", performanceDocumentYear);
                code = code.Replace("{{PerformanceDocumentStartDate}}", performanceDocumentStartDate.ToShortDateString());
                code = code.Replace("{{PerformanceDocumentEndDate}}", performanceDocumentEndDate.ToShortDateString());

                var userDetails = await _userBusiness.GetSingleById(userId);
                var companayDetails = await _companyBusiness.GetSingleById(userDetails.CompanyId);
                var header = "";
                var footer = "";
                if (companayDetails != null && companayDetails.LetterHeaderId.IsNotNullAndNotEmpty())
                {
                    var bytes = await _fileBusiness.GetFileByte(companayDetails.LetterHeaderId);
                    header = Convert.ToBase64String(bytes, 0, bytes.Length);
                    //header = String.Format("data:image/png;base64,{0}", base64String);
                    //header = base64String;
                    //Stream headerstream = new MemoryStream(bytes);
                    //header = Image.FromStream(headerstream).ToString();
                }
                //header = "https://localhost:44389/Cms/document/getimagemongo/8b836d83-a1f2-4ddc-90b0-ccc659755b2a";
                if (companayDetails != null && companayDetails.LetterFooterId.IsNotNullAndNotEmpty())
                {
                    var bytes = await _fileBusiness.GetFileByte(companayDetails.LetterHeaderId);
                    string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
                    footer = base64String;
                }
                var list = new List<ReportViewModel> { { new ReportViewModel { Name = "Letter Template", HtmlCode = code, Header = header, Footer = footer } } };
                return Ok(list);
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpGet]
        [Route("HeaderData/{userid}")]
        public async Task<IActionResult> HeaderData(string userid)
        {
            try
            {
                var item = new IdNameViewModel { Id = "1", Name = "sd" };
                return Ok(item);
            }
            catch (Exception)
            {

                throw;
            }
        }
        [HttpGet]
        [Route("DetailData/{headerId}")]
        public async Task<IActionResult> DetailData(string headerId)
        {
            try
            {
                var list = new List<IdNameViewModel> { { new IdNameViewModel { Id = "1", Name = "sdsdsd" } } };
                return Ok(list);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }


}
