using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.WebUtility;
using Synergy.App.ViewModel;
//using Kendo.Mvc.Extensions;
//using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.UI.Web.Areas.TAS.Controllers
{
    [Area("TAS")]
    public class JobCapacityRiskController : ApplicationController
    {
        private readonly IUserContext _userContext;
        private readonly INoteBusiness _noteBusiness;
        private readonly ITableMetadataBusiness _tableMetadataBusiness;
        private readonly IHRCoreBusiness _hRCoreBusiness;
        private readonly ITalentAssessmentBusiness _talentAssessmentBusiness;
        public JobCapacityRiskController(IUserContext userContext, INoteBusiness noteBusiness, ITableMetadataBusiness tableMetadataBusiness,
            IHRCoreBusiness hRCoreBusiness, ITalentAssessmentBusiness talentAssessmentBusiness) 
        {
            _userContext = userContext;
            _noteBusiness = noteBusiness;
            _tableMetadataBusiness = tableMetadataBusiness;
            _hRCoreBusiness = hRCoreBusiness;
            _talentAssessmentBusiness = talentAssessmentBusiness;
        }
        public IActionResult Index()
        {
            var model = new CapacityRiskViewModel();
            return View(model);
        }
        
        public IActionResult UpdateCapacityRisk(string departmentId)
        {
            var model = new CapacityRiskViewModel();
            model.DepartmentId = departmentId;
            return View(model);
        }
        public async Task<JsonResult> GetDepartmentList(string departmentId)
        {
            var result = await _hRCoreBusiness.GetJobByDepartment(departmentId);
            return Json(result);
        }

        public async Task<ActionResult> ReadSearchData(/*[DataSourceRequest] DataSourceRequest request,*/ string departmentId)
        {
            var result = await _talentAssessmentBusiness.GetJobCapacityData(departmentId);
            var json = Json(result/*.ToDataSourceResult(request)*/);
            return json;
        }

        public async Task<ActionResult> ReadChartData([DataSourceRequest] DataSourceRequest request, string jobId, string departmentId)
        {
            var result = await _talentAssessmentBusiness.GetJobCapacityChartData(jobId, departmentId);
            var json = Json(result);
            return json;
        }

        [HttpPost]
        public async Task<IActionResult> ManageUpdateCapacityRisk(string json)
        {
            if (json.IsNotNull())
            {
                var list = JsonConvert.DeserializeObject<List<CapacityRiskViewModel>>(json);
                foreach (var item in list)
                {
                    var existing = await _tableMetadataBusiness.GetTableDataByColumn("JobCapacityRisk", "", "JobId", item.JobId);
                    
                    if (existing==null && (item.ExternalAvailability.IsNotNull() || item.InternalAvailability.IsNotNull()))
                    {
                        var noteTempModel = new NoteTemplateViewModel();
                        noteTempModel.DataAction = DataActionEnum.Create;
                        noteTempModel.ActiveUserId = _userContext.UserId;
                        noteTempModel.TemplateCode = "JobCapacityRisk";
                        var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                        notemodel.StartDate = DateTime.Now;
                        notemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                        dynamic exo = new System.Dynamic.ExpandoObject();

                        ((IDictionary<String, Object>)exo).Add("JobId", item.JobId);
                        ((IDictionary<String, Object>)exo).Add("ExternalAvailability", item.ExternalAvailability);
                        ((IDictionary<String, Object>)exo).Add("InternalAvailability", item.InternalAvailability);
                
                        notemodel.Json = JsonConvert.SerializeObject(exo);
                        var result = await _noteBusiness.ManageNote(notemodel);
                    }
                    else if (item.ExternalAvailability.IsNotNull() || item.InternalAvailability.IsNotNull())
                    {
                        var noteTempModel = new NoteTemplateViewModel();
                        noteTempModel.DataAction = DataActionEnum.Edit;
                        noteTempModel.ActiveUserId = _userContext.UserId;
                        noteTempModel.TemplateCode = "JobCapacityRisk";
                        noteTempModel.NoteId = existing["NtsNoteId"].ToString();
                        var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                        dynamic exo = new System.Dynamic.ExpandoObject();
                        ((IDictionary<String, Object>)exo).Add("ExternalAvailability", item.ExternalAvailability);
                        ((IDictionary<String, Object>)exo).Add("InternalAvailability", item.InternalAvailability);
                        ((IDictionary<String, Object>)exo).Add("JobId", item.JobId);

                        notemodel.Json = JsonConvert.SerializeObject(exo);
                        var result = await _noteBusiness.ManageNote(notemodel);
                    }


                }
            }
              

            return Json(new { success = true });



        }
    }
}
