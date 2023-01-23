using Synergy.App.Business.Interface.BusinessScript.Task.General.CaseManagement;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business.Implementations.BusinessScript.Task.General.CaseManagement
{
    public class TaskGeneralCaseManagementPreScript : ITaskGeneralCaseManagementPreScript
    {
        /// <summary>
        /// Stop all action of task for Freezed Performance Document Master
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="udf"></param>
        /// <param name="uc"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
    
        public async Task<CommandResult<TaskTemplateViewModel>> ChangeTaskAssignee(TaskTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {        
            var _serviceBusiness = sp.GetService<IServiceBusiness>();
            var _noteBusiness = sp.GetService<INoteBusiness>();
            var _tableMetadataBusiness = sp.GetService<ITableMetadataBusiness>();
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
             
                var vendorId = rowdata["VendorUserId"].ToString();

                if (vendorId.IsNotNullAndNotEmpty())
                {
                    var vendor = await _tableMetadataBusiness.GetTableDataByColumn("CSM_VENDOR", "", "Id", vendorId);
                    if (vendor.IsNotNull())
                    {
                        var userId = Convert.ToString(vendor["VendorUserId"]);
                        viewModel.AssignedToUserId = userId;
                    }

                }
            }
           
                return CommandResult<TaskTemplateViewModel>.Instance(viewModel);
        }

       
    }
}
