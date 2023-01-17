using Synergy.App.Business.Interface.BusinessScript.Task.General.SwacchSanjy;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business.Implementations.BusinessScript.Task.General.SwacchSanjy
{
    public class TaskGeneralSwacchSanjyPreScript : ITaskGeneralSwacchSanjyPreScript
    {
        /// <summary>
        /// Stop all action of task for Freezed Performance Document Master
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="udf"></param>
        /// <param name="uc"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
    
        public async Task<CommandResult<TaskTemplateViewModel>> ChangeVerifyIssueTaskAssignee(TaskTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _lovBusiness = sp.GetService<ILOVBusiness>();            
            var _tableMetadataBusiness = sp.GetService<ITableMetadataBusiness>();
            var _teamBusiness = sp.GetService<ITeamBusiness>();
            var _userBusiness = sp.GetService<IUserBusiness>();
            var _serviceBusiness = sp.GetService<IServiceBusiness>();
            var _noteBusiness = sp.GetService<INoteBusiness>();
            var servicedata = await _serviceBusiness.GetSingleById(viewModel.ParentServiceId);
            var serviceData = await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel()
            {
                NoteId = servicedata.UdfNoteId,
                DataAction = DataActionEnum.View,
                SetUdfValue = true,
            });
            var rowdata = serviceData.ColumnList.ToDictionary(x => x.Name, x => x.Value);
            if (viewModel.TaskStatusCode == "TASK_STATUS_DRAFT" || viewModel.TaskStatusCode == "TASK_STATUS_INPROGRESS")
            {               
             
                var FacilityId = rowdata["FacilityId"].ToString();

                if (FacilityId.IsNotNullAndNotEmpty())
                {
                    var facility=await _tableMetadataBusiness.GetTableDataByColumn("SWACHH_SANJY_FACILITY","","Id", FacilityId);
                    if (facility.IsNotNull())
                    {
                        var FacilityLocationId = Convert.ToString(facility["FacilityLocationId"]);
                        var FacilityLocation = await _tableMetadataBusiness.GetTableDataByColumn("SS_FACILITY_LOCATION", "", "Id", FacilityLocationId);
                        if (FacilityLocation.IsNotNull())
                        {
                            var LocationId = Convert.ToString(FacilityLocation["YathraLocationId"]);
                            var lov = await _lovBusiness.GetSingleById(LocationId);
                            if (lov.Code == "SS_Baltal")
                            {
                                var team = await _teamBusiness.GetSingle(x => x.Code == "NODAL_OFFICER_BALTAL");
                                if (team.IsNotNull())
                                {
                                    var assigntotype = await _lovBusiness.GetSingle(x => x.LOVType == "TASK_ASSIGN_TO_TYPE" && x.Code == "TASK_ASSIGN_TO_TEAM");
                                    var temUser = await _teamBusiness.GetSingle<TeamUserViewModel, TeamUser>(x => x.IsTeamOwner == true && x.TeamId == team.Id);
                                    viewModel.AssignedToTypeId = assigntotype.Id;
                                    viewModel.AssignedToTypeCode = assigntotype.Code;
                                    viewModel.AssignedToTeamId = team.Id;
                                    viewModel.AssignedToUserId = temUser.UserId;
                                }

                            }
                            else if (lov.Code == "SS_Pahalgam")
                            {
                                var team = await _teamBusiness.GetSingle(x => x.Name == "NODAL_OFFICER_PAHALGAM");
                                if (team.IsNotNull())
                                {
                                    var assigntotype = await _lovBusiness.GetSingle(x => x.LOVType == "TASK_ASSIGN_TO_TYPE" && x.Code == "TASK_ASSIGN_TO_TEAM");
                                    var temUser = await _teamBusiness.GetSingle<TeamUserViewModel, TeamUser>(x => x.IsTeamOwner == true && x.TeamId == team.Id);
                                    viewModel.AssignedToTypeId = assigntotype.Id;
                                    viewModel.AssignedToTypeCode = assigntotype.Code;
                                    viewModel.AssignedToTeamId = team.Id;
                                    viewModel.AssignedToUserId = temUser.UserId;
                                }
                            }
                        }
                    }


                   
                }
            }
           
                return CommandResult<TaskTemplateViewModel>.Instance(viewModel);
        }

       
    }
}
