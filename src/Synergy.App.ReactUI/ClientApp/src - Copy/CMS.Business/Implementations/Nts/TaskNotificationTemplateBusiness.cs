using AutoMapper;
using CMS.Common;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;

namespace CMS.Business
{
    public class TaskNotificationTemplateBusiness : BusinessBase<TaskNotificationTemplateViewModel, TaskNotificationTemplate>, ITaskNotificationTemplateBusiness
    {
        public TaskNotificationTemplateBusiness(IRepositoryBase<TaskNotificationTemplateViewModel, TaskNotificationTemplate> repo, IMapper autoMapper) : base(repo, autoMapper)
        {

        }

        public async override Task<CommandResult<TaskNotificationTemplateViewModel>> Create(TaskNotificationTemplateViewModel model)
        {
            model.Body = HttpUtility.HtmlDecode(model.Body);
            var validateName = await IsNameExists(model);

            if (!validateName.IsSuccess)
            {
                return CommandResult<TaskNotificationTemplateViewModel>.Instance(model, false, validateName.Messages);
            }
            if (model.ParentTaskNotificationTemplateId.IsNotNullAndNotEmpty())
            {
                model.Subject = null;
                model.Body = null;
                model.SmsText = null;
            }
            var result = await base.Create(model);
            if (!result.IsSuccess)
            {
                return CommandResult<TaskNotificationTemplateViewModel>.Instance(model, false, result.Messages);
            }
            return CommandResult<TaskNotificationTemplateViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<TaskNotificationTemplateViewModel>> Edit(TaskNotificationTemplateViewModel model)
        {
            model.Body = HttpUtility.HtmlDecode(model.Body);
            var validateName = await IsNameExists(model);

            if (!validateName.IsSuccess)
            {
                return CommandResult<TaskNotificationTemplateViewModel>.Instance(model, false, validateName.Messages);
            }
            if (model.ParentTaskNotificationTemplateId.IsNotNullAndNotEmpty())
            {
                model.Subject = null;
                model.Body = null;
                model.SmsText = null;
            }
            var result = await base.Edit(model);
            if (!result.IsSuccess)
            {
                return CommandResult<TaskNotificationTemplateViewModel>.Instance(model, false, result.Messages);
            }
            return CommandResult<TaskNotificationTemplateViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        private async Task<CommandResult<TaskNotificationTemplateViewModel>> IsNameExists(TaskNotificationTemplateViewModel model)
        {
            var errorList = new Dictionary<string, string>();
            if (model.Name.IsNullOrEmpty())
            {
                errorList.Add("Name", "Name is required.");
            }
            else
            {
                var name = await _repo.GetSingle<TaskNotificationTemplateViewModel, TaskNotificationTemplate>(x => x.Name.ToLower() == model.Name.ToLower() && x.Id != model.Id && x.IsTemplate == model.IsTemplate);
                if (name != null)
                {
                    errorList.Add("Name", "Name already exist.");

                }
            }
            if (model.Code.IsNullOrEmpty())
            {
                errorList.Add("Code", "Code is required.");
            }
            else
            {
                var name = await _repo.GetSingle<TaskNotificationTemplateViewModel, TaskNotificationTemplate>(x => x.Code.ToLower() == model.Code.ToLower() && x.Id != model.Id && x.IsTemplate == model.IsTemplate);
                if (name != null)
                {
                    errorList.Add("Code", "Code already exist.");

                }
            }
            if (model.NotificationTo.ToString().IsNullOrEmpty())
            {
                errorList.Add("NotificationTo", "Notification to is required.");
            }
            if (model.NotificationActionId.IsNullOrEmpty())
            {
                errorList.Add("NotificationAction", "Notification action is required.");
            }
            if (errorList.Count > 0)
            {
                return CommandResult<TaskNotificationTemplateViewModel>.Instance(model, false, errorList);
            }

            return CommandResult<TaskNotificationTemplateViewModel>.Instance();
        }
        public async Task<List<TaskNotificationTemplateViewModel>> GetListByTaskTemplate(string taskTemplateId)
        {
            var model = await _repo.GetList(x => x.TaskTemplateId == taskTemplateId, x => x.ParentTaskNotificationTemplate);

            foreach (var item in model)
            {
                if (item.ParentTaskNotificationTemplate != null)
                {
                    item.Subject = item.ParentTaskNotificationTemplate.Subject;
                }
            }
            return model;
        }
    }
}
