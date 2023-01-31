using Synergy.App.Business.Interface.BusinessScript.Note.General.Assessment;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.ViewModel;
using Hangfire;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business.Implementations.BusinessScript.Note.General.Assessment
{
    public class NoteGeneralAssessmentPostScript : INoteGeneralAssessmentPostScript
    {
        //private readonly IHangfireScheduler _hangfireScheduler;
        //private readonly IHangfireScheduler _hangfireScheduler;
        public NoteGeneralAssessmentPostScript(//IHangfireScheduler hangfireScheduler
            )
        {
            //_hangfireScheduler = hangfireScheduler;
        }
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
                var assessment = await _talentBusiness.GetAssessmentDetailsById(rowData1["SurveyId"].ToString());
                if (assessment.IsNotNull())
                {
                    if (assessment.AssessmentType == "OPEN_SURVEY")
                    {
                        //await _talentBusiness.GenerateDummySurveyDetails(viewModel.UdfNoteTableId, Convert.ToInt32(rowData1["SurveyDummyCount"].ToString()));
                        var hangfireScheduler = sp.GetService<IHangfireScheduler>();
                        await hangfireScheduler.Enqueue<HangfireScheduler>(x => x.GenerateDummySurveyDetails(viewModel.UdfNoteTableId, Convert.ToInt32(rowData1["SurveyDummyCount"].ToString()), uc.ToIdentityUser(), null));
                    }
                    else
                    {
                        await _talentBusiness.ExecuteSurveyForUsers(viewModel.NoteId, uc.PortalId);
                    }
                }

            }
            return CommandResult<NoteTemplateViewModel>.Instance(viewModel);
        }
    }
}
