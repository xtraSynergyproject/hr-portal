using Synergy.App.Business.Interface.BusinessScript.Task.General.CSC;
using Synergy.App.Common;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business.Implementations.BusinessScript.Task.General.CSC
{
    public class TaskGeneralCSCPreScript : ITaskGeneralCSCPreScript
    {
        /// <summary>
        /// Stop all action of task for Freezed Performance Document Master
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="udf"></param>
        /// <param name="uc"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
    
        public async Task<CommandResult<TaskTemplateViewModel>> ChangePaymentTaskAssignee(TaskTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _taskBusiness = sp.GetService<ITaskBusiness>();
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
               
               // var rowdata = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(viewModel.Json);
                var assigneeId = rowdata["ApplicantName"].ToString();

                if (assigneeId.IsNotNullAndNotEmpty())
                {
                    viewModel.AssignedToUserId = assigneeId;
                }
            }
            //if (viewModel.TaskStatusCode == "TASK_STATUS_COMPLETE")
            //{
                
            //    var paymentReferenceId = rowdata["PaymentReferenceNo"] != null? rowdata["PaymentReferenceNo"].ToString():"";
            //    if (paymentReferenceId.IsNullOrEmpty())
            //    {
            //        var errorList = new Dictionary<string, string>();
            //        errorList.Add("Validate", "Please make payement.");
            //        return CommandResult<TaskTemplateViewModel>.Instance(viewModel, false, errorList);
            //    }
            //}
                return CommandResult<TaskTemplateViewModel>.Instance(viewModel);
        }

       
    }
}
