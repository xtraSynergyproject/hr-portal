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
    public class ServiceNotificationTemplateBusiness : BusinessBase<ServiceNotificationTemplateViewModel, ServiceNotificationTemplate>, IServiceNotificationTemplateBusiness
    {
        public ServiceNotificationTemplateBusiness(IRepositoryBase<ServiceNotificationTemplateViewModel, ServiceNotificationTemplate> repo, IMapper autoMapper) : base(repo, autoMapper)
        {

        }

        public async override Task<CommandResult<ServiceNotificationTemplateViewModel>> Create(ServiceNotificationTemplateViewModel model)
        {
            model.Body = HttpUtility.HtmlDecode(model.Body);
            var validateName = await IsNameExists(model);

            if (!validateName.IsSuccess)
            {
                return CommandResult<ServiceNotificationTemplateViewModel>.Instance(model, false, validateName.Messages);
            }
            if (model.ParentServiceNotificationTemplateId.IsNotNullAndNotEmpty())
            {
                model.Subject = null;
                model.Body = null;
                model.SmsText = null;
            }
            var result = await base.Create(model);
            if (!result.IsSuccess)
            {
                return CommandResult<ServiceNotificationTemplateViewModel>.Instance(model, false, result.Messages);
            }
            return CommandResult<ServiceNotificationTemplateViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<ServiceNotificationTemplateViewModel>> Edit(ServiceNotificationTemplateViewModel model)
        {
            model.Body = HttpUtility.HtmlDecode(model.Body);
            var validateName = await IsNameExists(model);

            if (!validateName.IsSuccess)
            {
                return CommandResult<ServiceNotificationTemplateViewModel>.Instance(model, false, validateName.Messages);
            }
            if (model.ParentServiceNotificationTemplateId.IsNotNullAndNotEmpty())
            {
                model.Subject = null;
                model.Body = null;
                model.SmsText = null;
            }
            var result = await base.Edit(model);
            if (!result.IsSuccess)
            {
                return CommandResult<ServiceNotificationTemplateViewModel>.Instance(model, false, result.Messages);
            }
            return CommandResult<ServiceNotificationTemplateViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        private async Task<CommandResult<ServiceNotificationTemplateViewModel>> IsNameExists(ServiceNotificationTemplateViewModel model)
        {
            var errorList = new Dictionary<string, string>();
            if (model.Name.IsNullOrEmpty())
            {
                errorList.Add("Name", "Name is required.");
            }
            else
            {
                var name = await _repo.GetSingle<ServiceNotificationTemplateViewModel, ServiceNotificationTemplate>(x => x.Name.ToLower() == model.Name.ToLower() && x.Id != model.Id && x.IsTemplate == model.IsTemplate);
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
                var name = await _repo.GetSingle<ServiceNotificationTemplateViewModel, ServiceNotificationTemplate>(x => x.Code.ToLower() == model.Code.ToLower() && x.Id != model.Id && x.IsTemplate == model.IsTemplate);
                if (name != null)
                {
                    errorList.Add("Code", "Code already exist.");
                }
            }
            if (model.NotificationTo.ToString().IsNullOrEmpty())
            {
                errorList.Add("NotificationTo", "Notification to is required.");
            }
            if (model.NotificationActionId.ToString().IsNullOrEmpty())
            {
                errorList.Add("NotificationAction", "Notification action is required.");
            }
            if (errorList.Count > 0)
            {
                return CommandResult<ServiceNotificationTemplateViewModel>.Instance(model, false, errorList);
            }

            return CommandResult<ServiceNotificationTemplateViewModel>.Instance();
        }
        public async Task<List<ServiceNotificationTemplateViewModel>> GetListByServiceTemplate(string serviceTemplateId)
        {
            var model = await _repo.GetList(x => x.ServiceTemplateId == serviceTemplateId, x => x.ParentServiceNotificationTemplate);
            
            foreach(var item in model)
            {
                if (item.ParentServiceNotificationTemplate!=null)
                {
                    item.Subject = item.ParentServiceNotificationTemplate.Subject;
                }
            }
            return model;
        }
    }
}
