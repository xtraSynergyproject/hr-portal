using CMS.Business.Interface.BusinessScript.Note.General.Assessment;
using CMS.Common;
using CMS.Data.Model;
using CMS.UI.ViewModel;
using Hangfire;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Business.Implementations.BusinessScript.Note.General.Assessment
{
    public class NoteGeneralAssessmentPostScript : INoteGeneralAssessmentPostScript
    {
        public async Task<CommandResult<NoteTemplateViewModel>> ManageSurveyUsers(NoteTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            if (viewModel.NoteStatusCode == "NOTE_STATUS_INPROGRESS")
            {
                //

                var _noteBusiness = sp.GetService<INoteBusiness>();

                var noteTempModel = new NoteTemplateViewModel();

                noteTempModel.NoteId = viewModel.NoteId;
                noteTempModel.SetUdfValue = true;
                var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                var rowData1 = notemodel.ColumnList.ToDictionary(x => x.Name, x => x.Value);
                var SurveyStatus = rowData1.ContainsKey("SurveyStatus") ? Convert.ToString(rowData1["SurveyStatus"]) : "";
                if (SurveyStatus.IsNotNull())
                {
                    rowData1["SurveyStatus"] = (int)((SurveyStatusEnum)Enum.Parse(typeof(SurveyStatusEnum), SurveyStatusEnum.ScheduleInprogress.ToString()));//.ScheduleInprogress;
                    var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData1);
                    var update = await _noteBusiness.EditNoteUdfTable(notemodel, data1, notemodel.UdfNoteTableId);

                }
                //BackgroundJob.Enqueue<HangfireScheduler>(x => x.ExecuteSurveyForUsers(viewModel.NoteId, uc.PortalId, uc.ToIdentityUser()));
                var _talentBusiness = sp.GetService<ITalentAssessmentBusiness>();
                await _talentBusiness.ExecuteSurveyForUsers(viewModel.NoteId, uc.PortalId);
            }
            return CommandResult<NoteTemplateViewModel>.Instance(viewModel);
        }
    }
}
