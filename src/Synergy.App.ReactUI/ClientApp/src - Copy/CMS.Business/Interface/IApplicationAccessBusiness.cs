using CMS.Common;
using CMS.Data.Model;
using CMS.UI.ViewModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Business
{
    public interface IApplicationAccessBusiness : IBusinessBase<ApplicationAccessViewModel, ApplicationAccess>
    {
        Task CreateAccessLog(HttpRequest request, AccessLogTypeEnum type);
        Task CreateLogin(HttpRequest request,  UserViewModel user);
    }
}
