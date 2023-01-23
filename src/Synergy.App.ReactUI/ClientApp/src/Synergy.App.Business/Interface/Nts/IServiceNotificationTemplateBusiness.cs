﻿using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public interface IServiceNotificationTemplateBusiness : IBusinessBase<ServiceNotificationTemplateViewModel, ServiceNotificationTemplate>
    {
        Task<List<ServiceNotificationTemplateViewModel>> GetListByServiceTemplate(string serviceTemplateId);
    }
}