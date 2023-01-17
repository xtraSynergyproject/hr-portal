using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.ViewModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public interface IApplicationAccessBusiness : IBusinessBase<ApplicationAccessViewModel, ApplicationAccess>
    {
        Task CreateAccessLog(HttpRequest request, AccessLogTypeEnum type);
        Task CreateLogin(HttpRequest request,  UserViewModel user);
    }
}
