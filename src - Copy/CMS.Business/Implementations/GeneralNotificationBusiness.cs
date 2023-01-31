using AutoMapper;
using CMS.Common;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace CMS.Business
{
    //public class GeneralNotificationBusiness : BusinessBase<NotificationViewModel, Notification>, INotificationBusiness
    //{
    //    INotificationTemplateBusiness _noteNotificationTemplateBusiness;
    //    ITableMetadataBusiness _tableMetadataBusiness;
    //    public GeneralNotificationBusiness(IRepositoryBase<NotificationViewModel, Notification> repo, IMapper autoMapper,
    //        INotificationTemplateBusiness noteNotificationTemplateBusiness, ITableMetadataBusiness tableMetadataBusiness) : base(repo, autoMapper)
    //    {
    //        _noteNotificationTemplateBusiness = noteNotificationTemplateBusiness;
    //        _tableMetadataBusiness = tableMetadataBusiness;
    //    }

    //    //public async override Task<CommandResult<NotificationViewModel>> Create(NotificationViewModel model)
    //    //{
    //    //    var data = _autoMapper.Map<NotificationViewModel>(model);
    //    //    var result = await base.Create(data);
    //    //    if (result.IsSuccess)
    //    //    {
    //    //        var template = await _repo.GetSingleById<TemplateViewModel,Template>(model.TemplateId);
    //    //       // template.Json = model.Json;
    //    //        var tableResult=await _tableMetadataBusiness.ManageTemplateTable(template);
    //    //        if (tableResult.IsSuccess)
    //    //        {
    //    //            var notifcationList = new List<NoteNotificationTemplateViewModel>();
    //    //            notifcationList = await _noteNotificationTemplateBusiness.GetList();
    //    //            var notelist = notifcationList.Where(x => x.IsTemplate == true).ToList();
    //    //            foreach (var item in notelist)
    //    //            {
    //    //                var note = item;
    //    //                note.Id = null;
    //    //                note.NoteTemplateId = result.Item.Id;
    //    //                note.IsTemplate = false;
    //    //                var noteresult = await _noteNotificationTemplateBusiness.Create(note);
    //    //                if (!noteresult.IsSuccess)
    //    //                {
    //    //                    noteresult.Messages.Add("NoteTemplateName", "Note Template : Created.");
    //    //                    return CommandResult<NotificationViewModel>.Instance(model, noteresult.IsSuccess, noteresult.Messages);
    //    //                }
    //    //            }
    //    //        }
    //    //        else 
    //    //        {
    //    //            return CommandResult<NotificationViewModel>.Instance(model, tableResult.IsSuccess, tableResult.Messages);
    //    //        }
    //    //    }
    //    //    return CommandResult<NotificationViewModel>.Instance(model, result.IsSuccess, result.Messages);
    //    //}

    //    //public async override Task<CommandResult<NotificationViewModel>> Edit(NotificationViewModel model)
    //    //{
    //    //    var data = _autoMapper.Map<NotificationViewModel>(model);
    //    //    var result = await base.Edit(data);
    //    //    if (result.IsSuccess) 
    //    //    {
    //    //        var tableResult = await _tableMetadataBusiness.ManageTemplateTable(new TemplateViewModel { Id = model.TemplateId, Json = model.Json, TemplateType = TemplateTypeEnum.Note });
    //    //        if (!tableResult.IsSuccess) 
    //    //        {
    //    //            return CommandResult<NotificationViewModel>.Instance(model, tableResult.IsSuccess, tableResult.Messages);
    //    //        }
    //    //    }
    //    //    return CommandResult<NotificationViewModel>.Instance(model, result.IsSuccess, result.Messages);
    //    //}


    //}
}
