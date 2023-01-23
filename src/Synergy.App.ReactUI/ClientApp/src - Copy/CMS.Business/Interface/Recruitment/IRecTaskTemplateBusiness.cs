using CMS.Common;
using CMS.Data.Model;
using CMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Business
{
    public interface IRecTaskTemplateBusiness : IBusinessBase<RecTaskTemplateViewModel,RecTaskTemplate>
    {
        Task<CommandResult<RecTaskTemplateViewModel>> ManageCreate(RecTaskTemplateViewModel model);
        Task<CommandResult<RecTaskTemplateViewModel>> ManageEdit(RecTaskTemplateViewModel model);
        Task<IList<IdNameViewModel>> GetEmailSetting();
        Task<EmailViewModel> GetEmailSettingById(string id);
        Task<IList<IdNameViewModel>> GetTaskTemplateList();
        Task<IList<IdNameViewModel>> GetAdhocTaskTemplateList();
    }
}
