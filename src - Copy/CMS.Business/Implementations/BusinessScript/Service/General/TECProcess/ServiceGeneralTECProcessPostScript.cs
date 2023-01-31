
using CMS.Business.Interface.BusinessScript.Service.General.TECProcess;
using CMS.Common;
using CMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Business.Implementations.BusinessScript.Service.General.TECProcess
{
    public class ServiceGeneralTECProcessPostScript : IServiceGeneralTECProcessPostScript
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="udf"></param>
        /// <param name="uc"></param>
        /// <param name="sp"></param>
        /// <returns></returns>    
       
        public async Task<CommandResult<ServiceTemplateViewModel>> CreateBookPageMapping(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _serviceBusiness = sp.GetService<IServiceBusiness>();
            if (viewModel.ServiceStatusCode=="SERVICE_STATUS_INPROGRESS" && viewModel.DataAction==DataActionEnum.Create) 
            {
                var pageservice = await _serviceBusiness.GetSingleById(viewModel.ServiceId);
                var bookservice = await _serviceBusiness.GetSingleById(viewModel.ParentServiceId);
                var Count = await _serviceBusiness.GetBookAllPagesByBookId(bookservice.UdfNoteTableId);
                await _serviceBusiness.CreateBookPageMapping(pageservice.UdfNoteTableId, bookservice.UdfNoteTableId, Count.Count()+1);
            }
            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }


    }
}
