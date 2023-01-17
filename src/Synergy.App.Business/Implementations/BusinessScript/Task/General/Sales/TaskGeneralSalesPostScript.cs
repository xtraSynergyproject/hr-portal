using Synergy.App.Business.Interface.BusinessScript.Task.General.Sales;
using Synergy.App.Common;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business.Implementations.BusinessScript.Task.General.Sales
{
    public class TaskGeneralSalesPostScript : ITaskGeneralSalesPostScript
    {

        public async Task<CommandResult<TaskTemplateViewModel>> GenerateLicense(TaskTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {

            if (viewModel.TaskStatusCode == "TASK_STATUS_COMPLETE")
            {
                try
                {
                    var sb = sp.GetService<IServiceBusiness>();                   
                    if (viewModel.ParentServiceId.IsNotNullAndNotEmpty())
                    {

                        var svm =await sb.GetServiceDetails(new ServiceTemplateViewModel
                        {
                            ServiceId = viewModel.ParentServiceId,
                            DataAction = DataActionEnum.Read,
                            ActiveUserId= uc.UserId,
                            SetUdfValue=true
                        });
                        var lb = sp.GetService<ISalesBusiness>();
                       
                        var license =await lb.GenerateLicense(svm);
                        if (license != null && license.IsSuccess && license.Item != null)
                        {
                            var _noteBusiness = sp.GetService<INoteBusiness>();
                            var noteTempModel = new NoteTemplateViewModel();                          
                            noteTempModel.NoteId = svm.UdfNoteId;
                            noteTempModel.SetUdfValue = true;
                            var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                            var rowData1 = notemodel.ColumnList.ToDictionary(x => x.Name, x => x.Value);
                            rowData1["LicenseNote"] = license.Item.NoteId;
                            var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData1);
                            var update = await _noteBusiness.EditNoteUdfTable(notemodel, data1, notemodel.UdfNoteTableId);
                            //var tfvs = tb.GetTaskFieldValues(viewModel.Id);
                            //if (tfvs != null)
                            //{
                            //    var tfv = tfvs.FirstOrDefault(x => x.FieldName == "licenseNote");
                            //    if (tfv != null)
                            //    {
                            //        var tfvr = BusinessHelper.GetInstance<IRepositoryBase<NTS_TaskFieldValue>>();
                            //        var tfvm = tfvr.GetSingleById(tfv.Id);
                            //        if (tfvm != null)
                            //        {
                            //            tfvm.Code = license.Item.Id.ToString();
                            //            tfvm.Value = "License Details";
                            //            tfvm.LastUpdatedDate = DateTime.Now;
                            //            tfvr.Edit(tfvm);
                            //        }
                            //    }
                            //}
                            return CommandResult<TaskTemplateViewModel>.Instance(viewModel);
                        }


                    }

                }
                catch (Exception e)
                {
                    var messages = new Dictionary<string, string>();
                    messages.Add("Error","Error while calculating commission.");
                    return CommandResult<TaskTemplateViewModel>.Instance(viewModel, false, messages);
                   
                    
                }
              
            }
            return CommandResult<TaskTemplateViewModel>.Instance(viewModel);
        }

    }
}
