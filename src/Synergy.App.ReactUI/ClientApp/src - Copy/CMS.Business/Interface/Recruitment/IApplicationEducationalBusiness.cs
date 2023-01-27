using CMS.Data.Model;
using CMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CMS.Common;

namespace CMS.Business
{
    public interface IApplicationEducationalBusiness : IBusinessBase<ApplicationEducationalViewModel, ApplicationEducational>
    {
        Task<IList<ApplicationEducationalViewModel>> GetListByApplication(QualificationTypeEnum qualificationType,string candidateProfileId);
    }
}
