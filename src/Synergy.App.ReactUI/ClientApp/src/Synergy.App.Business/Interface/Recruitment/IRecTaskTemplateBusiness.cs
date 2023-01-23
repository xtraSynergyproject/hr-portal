using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public interface IRecTaskTemplateBusiness : IBusinessBase<RecTaskTemplateViewModel,RecTaskTemplate>
    {
        Task<CommandResult<RecTaskTemplateViewModel>> ManageCreate(RecTaskTemplateViewModel model, bool autoCommit = true);
        Task<CommandResult<RecTaskTemplateViewModel>> ManageEdit(RecTaskTemplateViewModel model, bool autoCommit = true);
        Task<IList<IdNameViewModel>> GetEmailSetting();
        Task<EmailViewModel> GetEmailSettingById(string id);
        Task<IList<IdNameViewModel>> GetTaskTemplateList();
        Task<IList<IdNameViewModel>> GetAdhocTaskTemplateList();
    }
}
