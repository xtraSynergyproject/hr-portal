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
    public class TaskGeneralCSCPostScript : ITaskGeneralCSCPostScript
    {
        public async Task<CommandResult<TaskTemplateViewModel>> SetProvisionCertificateTrue(TaskTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            if (viewModel.TaskStatusCode == "TASK_STATUS_COMPLETE")
            {
                var _serviceBusiness = sp.GetService<IServiceBusiness>();
                var _noteBusiness = sp.GetService<INoteBusiness>();
                var servicedata = await _serviceBusiness.GetSingleById(viewModel.ParentServiceId);
                var notemodel = await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel()
                {
                    NoteId = servicedata.UdfNoteId,
                    DataAction = DataActionEnum.Edit,
                    SetUdfValue = true,
                });
                var rowdata = notemodel.ColumnList.ToDictionary(x => x.Name, x => x.Value);
                if (rowdata.ContainsKey("ProvisionalCertificate"))
                {
                    rowdata["ProvisionalCertificate"] = true;
                }

                var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowdata);
                var update = await _noteBusiness.EditNoteUdfTable(notemodel, data1, notemodel.UdfNoteTableId);
            }
            return CommandResult<TaskTemplateViewModel>.Instance(viewModel);
        }
        public async Task<CommandResult<TaskTemplateViewModel>> SetCertificateTrue(TaskTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            if (viewModel.TaskStatusCode == "TASK_STATUS_COMPLETE")
            {
                var _serviceBusiness = sp.GetService<IServiceBusiness>();
                var _noteBusiness = sp.GetService<INoteBusiness>();
                var servicedata = await _serviceBusiness.GetSingleById(viewModel.ParentServiceId);
                var notemodel = await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel()
                {
                    NoteId = servicedata.UdfNoteId,
                    DataAction = DataActionEnum.Edit,
                    SetUdfValue = true,
                });
                var rowdata = notemodel.ColumnList.ToDictionary(x => x.Name, x => x.Value);
                if (rowdata.ContainsKey("Certificate"))
                {
                    rowdata["Certificate"] = true;
                }

                var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowdata);
                var update = await _noteBusiness.EditNoteUdfTable(notemodel, data1, notemodel.UdfNoteTableId);
            }
            return CommandResult<TaskTemplateViewModel>.Instance(viewModel);
        }


    }
}
