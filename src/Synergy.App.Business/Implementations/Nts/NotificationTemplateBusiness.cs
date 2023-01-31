using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;

namespace Synergy.App.Business
{
    public class NotificationTemplateBusiness : BusinessBase<NotificationTemplateViewModel, NotificationTemplate>, INotificationTemplateBusiness
    {
        public NotificationTemplateBusiness(IRepositoryBase<NotificationTemplateViewModel, NotificationTemplate> repo, IMapper autoMapper) : base(repo, autoMapper)
        {

        }

        public async override Task<CommandResult<NotificationTemplateViewModel>> Create(NotificationTemplateViewModel model, bool autoCommit = true)
        {
            //var body = model.Body;
            model.Body = HttpUtility.HtmlDecode(model.Body);

            var validateName = await IsNameExists(model);

            if (!validateName.IsSuccess)
            {
                return CommandResult<NotificationTemplateViewModel>.Instance(model, false, validateName.Messages);
            }
            var result = await base.Create(model,autoCommit);
            if (!result.IsSuccess)
            {
                return CommandResult<NotificationTemplateViewModel>.Instance(model, false, result.Messages);
            }
            return CommandResult<NotificationTemplateViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<NotificationTemplateViewModel>> Edit(NotificationTemplateViewModel model, bool autoCommit = true)
        {
            model.Body = HttpUtility.HtmlDecode(model.Body);
            var validateName = await IsNameExists(model);
            if (!validateName.IsSuccess)
            {
                return CommandResult<NotificationTemplateViewModel>.Instance(model, false, validateName.Messages);
            }
            var result = await base.Edit(model,autoCommit);
            if (!result.IsSuccess)
            {
                return CommandResult<NotificationTemplateViewModel>.Instance(model, false, result.Messages);
            }
            return CommandResult<NotificationTemplateViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        private async Task<CommandResult<NotificationTemplateViewModel>> IsNameExists(NotificationTemplateViewModel model)
        {
            var errorList = new Dictionary<string, string>();
            if (model.Name.IsNullOrEmpty())
            {
                errorList.Add("Name", "Name is required.");
            }
            if (model.Code.IsNullOrEmpty())
            {
                errorList.Add("Code", "Code is required.");
            }
            if (model.ParentNotificationTemplateId.IsNullOrEmpty())
            {
                if (model.NotificationTo.ToString().IsNullOrEmpty())
                {
                    errorList.Add("NotificationTo", "Notification to is required.");
                }
                if (model.NotificationActionId.IsNullOrEmpty())
                {
                    errorList.Add("NotificationAction", "Notification action is required.");
                }
            }

            if (errorList.Count > 0)
            {
                return CommandResult<NotificationTemplateViewModel>.Instance(model, false, errorList);
            }

            return CommandResult<NotificationTemplateViewModel>.Instance();
        }

        public async Task<List<NotificationTemplateViewModel>> GetListByTemplate(string templateId, NtsTypeEnum ntsType)
        {
            var model = await _repo.GetList(x => x.TemplateId == templateId || (x.NtsType == ntsType && x.AutoApplyOnAllTemplates), x => x.ParentNotificationTemplate);

            foreach (var item in model)
            {
                if (item.ParentNotificationTemplate != null)
                {
                    item.Subject = item.ParentNotificationTemplate.Subject;
                }
            }
            return model;
        }
    }
}
