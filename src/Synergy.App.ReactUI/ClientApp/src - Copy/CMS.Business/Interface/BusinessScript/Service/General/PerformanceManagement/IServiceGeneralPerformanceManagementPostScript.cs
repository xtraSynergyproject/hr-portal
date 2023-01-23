﻿using CMS.Common;
using CMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Business.Interface.BusinessScript.Service.General.PerformanceManagement
{
    public interface IServiceGeneralPerformanceManagementPostScript
    {
        Task<CommandResult<ServiceTemplateViewModel>> TriggerReviewGoal(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp);
    }
}