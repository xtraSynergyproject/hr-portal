using Synergy.App.Business.Interface.BusinessScript.Task.General.IMS;
using Synergy.App.Common;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business.Implementations.BusinessScript.Task.General.IMS
{
    public class TaskGeneralIMSPreScript : ITaskGeneralIMSPreScript
    {

        /// <summary>
        /// ValidateApprovedRequisitionItems
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="udf"></param>
        /// <param name="uc"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
        public async Task<CommandResult<TaskTemplateViewModel>> ValidateApprovedRequisitionItems(TaskTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _serviceBusiness = sp.GetService<IServiceBusiness>();
            var _taskBusiness = sp.GetService<ITaskBusiness>();
            var _inventoryManagementBusiness = sp.GetService<IInventoryManagementBusiness>();

            var serviceData = await _serviceBusiness.GetSingleById(viewModel.ParentServiceId);
            bool flag = false;
            if (serviceData!=null)
            {
                var itemData = await _inventoryManagementBusiness.GetRequisistionItemsData(serviceData.UdfNoteTableId);
                foreach (var item in itemData)
                {
                    if (item.IsApproved==false)
                    {
                        flag = true;
                        break;
                    }

                }
            }

            if (flag)
            {
                var errorList = new Dictionary<string, string>();
                errorList.Add("Validation", "Please approve requisition all items.");
                return CommandResult<TaskTemplateViewModel>.Instance(viewModel, false, errorList);
            }
            return CommandResult<TaskTemplateViewModel>.Instance(viewModel);
        }
    }
}
