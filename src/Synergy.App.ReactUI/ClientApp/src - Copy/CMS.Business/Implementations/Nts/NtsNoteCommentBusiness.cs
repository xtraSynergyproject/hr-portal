using AutoMapper;
using CMS.Common;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace CMS.Business
{
    public class NtsNoteCommentBusiness : BusinessBase<NtsNoteCommentViewModel, NtsNoteComment>,INtsNoteCommentBusiness
    {
        private readonly IRepositoryQueryBase<NtsNoteCommentViewModel> _queryRepo;
        private readonly INoteBusiness _noteBusiness;
        private readonly INtsNoteCommentUserBusiness _commentuserBusiness;
        public NtsNoteCommentBusiness(IRepositoryBase<NtsNoteCommentViewModel, NtsNoteComment> repo, IMapper autoMapper
            , IRepositoryQueryBase<NtsNoteCommentViewModel> queryRepo, INoteBusiness noteBusiness, INtsNoteCommentUserBusiness commentuserBusiness) : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;
            _noteBusiness = noteBusiness;
            _commentuserBusiness = commentuserBusiness;
        }

        public async override Task<CommandResult<NtsNoteCommentViewModel>> Create(NtsNoteCommentViewModel model)
        {


            // var data = _autoMapper.Map<UserRoleViewModel>(model);
            if (model.CommentToUserIds.IsNotNull())
            {
                if (model.CommentToUserIds.Contains("All"))
                {
                    model.CommentedTo = CommentToEnum.All;
                    var result = await base.Create(model);
                    await SendNotification(model);
                    return CommandResult<NtsNoteCommentViewModel>.Instance(model, result.IsSuccess, result.Messages);
                }
                else
                {
                    model.CommentedTo = CommentToEnum.User;
                    var result = await base.Create(model);
                    foreach (var user in model.CommentToUserIds)
                    {
                        var newmodel = new NtsNoteCommentUserViewModel
                        {
                            NtsNoteCommentId = result.Item.Id,
                            CommentToUserId = user,
                            DataAction = DataActionEnum.Create
                        };

                        await _commentuserBusiness.Create(newmodel);
                    }
                    await SendNotification(model);
                    return CommandResult<NtsNoteCommentViewModel>.Instance(model, result.IsSuccess, result.Messages);
                }


            }

            else
            {
                var result = await base.Create(model);
               // await SendNotification(model);
                return CommandResult<NtsNoteCommentViewModel>.Instance(model, result.IsSuccess, result.Messages);

            }
            //var result = await base.Create(model);
            //if (model.CommentToUserIds.IsNotNull())
            //{
            //    foreach (var user in model.CommentToUserIds)
            //    {
            //        var newmodel = new NtsNoteCommentUserViewModel
            //        {
            //            NtsNoteCommentId = result.Item.Id,
            //            CommentToUserId = user,
            //            DataAction = DataActionEnum.Create
            //        };

            //        await _commentuserBusiness.Create(newmodel);
            //    }
            //}
               
            
            //await SendNotification(model);
            //return CommandResult<NtsNoteCommentViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        private async Task SendNotification(NtsNoteCommentViewModel model)
        {
            var note = await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel { NoteId = model.NtsNoteId, DataAction = DataActionEnum.Read });
            note.PostComment = model.Comment;
            note.OwnerUserId = _repo.UserContext.UserId;
            var notificationTemplate = await _repo.GetSingle<NotificationTemplate, NotificationTemplate>(x => x.Code =="NOTE_COMMENT_NOTIFICATION_TEMPLATE" && x.NtsType == NtsTypeEnum.Note);
            if (model.CommentToUserIds.IsNotNull())
            {

                if (model.CommentToUserIds.Contains("All"))
                {
                    var userIds = await _noteBusiness.GetSharedList(model.NtsNoteId);
                    foreach (var user in userIds)
                    {
                        await _noteBusiness.SendNotification(note, notificationTemplate, user.Id);
                    }
                }
                else
                {
                    foreach (var user in model.CommentToUserIds)
                    {
                        await _noteBusiness.SendNotification(note, notificationTemplate, user);
                    }

                }
                //foreach (var user in model.CommentToUserIds)
                //{
                //    await _noteBusiness.SendNotification(note, notificationTemplate,user);
                //}
            }
            //else
            //{
            //    var list =await _noteBusiness.GetSharedList(model.NtsNoteId);
            //    foreach(var item in list)
            //    {
            //        await _noteBusiness.SendNotification(note, notificationTemplate, item.Id);
            //    }
            //}
        }
        public async override Task<CommandResult<NtsNoteCommentViewModel>> Edit(NtsNoteCommentViewModel model)
        {  
            var result = await base.Edit(model);           
            return CommandResult<NtsNoteCommentViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        

        public async Task<List<NtsNoteCommentViewModel>> GetSearchResult(string NoteId)
        {

            string query = @$"select n.""Id"" as Id,ub.""Name"" as CommentedByUserName,ub.""PhotoId"" as PhotoId,
n.""CommentedDate"" as CommentedDate,n.""Comment"" as Comment,case when n.""CommentedTo""=0 then 'All' else string_agg(ut.""Name"",'; ') end as CommentedToUserName
                              from public.""NtsNoteComment"" as n
join public.""User"" as ub ON ub.""Id"" = n.""CommentedByUserId"" and ub.""IsDeleted""=false
left join public.""NtsNoteCommentUser"" as nut ON nut.""NtsNoteCommentId"" = n.""Id""  and nut.""IsDeleted""=false
 left join public.""User"" as ut ON ut.""Id"" = nut.""CommentToUserId"" and ut.""IsDeleted""=false
left join public.""NtsNoteComment"" as p on p.""Id""=n.""ParentCommentId"" and p.""IsDeleted""=false
 where n.""NtsNoteId""='{NoteId}' AND n.""IsDeleted""= false and (nut.""Id"" is null or nut.""CommentToUserId""='{_repo.UserContext.UserId}' 
or n.""CommentedByUserId""='{_repo.UserContext.UserId}'  )
group by  n.""Id"",ub.""Name"",ub.""PhotoId"",
n.""CommentedDate"",n.""Comment"" ";
;
            var list = await _queryRepo.ExecuteQueryList<NtsNoteCommentViewModel>(query, null);
            //var result = new List<NtsNoteCommentViewModel>();
            //var firstLevelComments = list.Where(x => x.ParentCommentId == null).OrderByDescending(x => x.CreatedDate);
            //foreach (var item in firstLevelComments)
            //{                
            //    result.Add(item);
            //    AddReplyComments(result, list, item.Id);
            //}

          //  return result;
            return list;
        }

        public async Task<List<NtsNoteCommentViewModel>> GetCommentTree(string NoteId, string Id = null)
        {

            string query = "";
            if (Id == null)
            {
                query = @$"select distinct n.""Id"" as id,n.""Id"" as Id,ub.""Name"" as CommentedByUserName,ub.""PhotoId"" as PhotoId,n.""CommentedByUserId"",
n.""CommentedDate"" as CommentedDate,n.""Comment"" as Comment,n.""AttachmentId"" as AttachmentId,n.""ParentCommentId"" as ParentCommentId,
case when n.""CommentedTo""=0 then 'All' else string_agg(ut.""Name"",'; ') end as CommentedToUserName,f.""FileName""  as FileName,		
null as ParentId,true as hasChildren,true as expanded
                from public.""NtsNoteComment"" as n
join public.""User"" as ub ON ub.""Id"" = n.""CommentedByUserId"" and ub.""IsDeleted""=false
left join public.""NtsNoteCommentUser"" as nut ON nut.""NtsNoteCommentId"" = n.""Id"" and nut.""IsDeleted""=false
 left join public.""User"" as ut ON ut.""Id"" = nut.""CommentToUserId"" and ut.""IsDeleted""=false
left join public.""NtsNoteComment"" as p on p.""Id""=n.""ParentCommentId"" and p.""IsDeleted""=false
left join public.""File"" as f on f.""Id""=n.""AttachmentId"" and f.""IsDeleted""=false
 where n.""NtsNoteId""='{NoteId}' AND n.""IsDeleted""= false and n.""ParentCommentId"" is null and (nut.""Id"" is null or nut.""CommentToUserId""='{_repo.UserContext.UserId}' 
or n.""CommentedByUserId""='{_repo.UserContext.UserId}'  )
 group by n.""Id"",ub.""Name"",ub.""PhotoId"",
n.""CommentedDate"",n.""Comment"",f.""FileName"" ";

            }

            else
            {
                query = $@" with recursive cmn as(
select distinct n.""Id"",ub.""Name"" as CommentedByUserName,ub.""PhotoId"" as PhotoId,
n.""CommentedDate"" as CommentedDate,n.""Comment"" as Comment,ut.""Name"" as CommentedToUserName
,n.""AttachmentId"" as AttachmentId,n.""ParentCommentId"" as ParentCommentId,               
n.""ParentCommentId"" as ParentId,false as hasChildren,false as expanded,
	n.""CommentedTo""	as	CommentedTo	 ,f.""FileName""  as FileName ,n.""CommentedByUserId"" as CommentedByUserId
			   
			   from public.""NtsNoteComment"" as n
join public.""User"" as ub ON ub.""Id"" = n.""CommentedByUserId"" and ub.""IsDeleted""=false
left join public.""NtsNoteCommentUser"" as nut ON nut.""NtsNoteCommentId"" = n.""Id"" and nut.""IsDeleted""=false
 left join public.""User"" as ut ON ut.""Id"" = nut.""CommentToUserId"" and ut.""IsDeleted""=false
	left join public.""File"" as f on f.""Id""=n.""AttachmentId"" and f.""IsDeleted""=false
 where  n.""IsDeleted""= false and n.""ParentCommentId""='{Id}'
and (nut.""Id"" is null or n.""CommentedByUserId""='{_repo.UserContext.UserId}' or nut.""CommentToUserId""='{_repo.UserContext.UserId}')


	union
	
	select distinct n.""Id"" as Id,ub.""Name"" as CommentedByUserName,ub.""PhotoId"" as PhotoId,
n.""CommentedDate"" as CommentedDate,n.""Comment"" as Comment,ut.""Name"" as CommentedToUserName
,n.""AttachmentId"" as AttachmentId,n.""ParentCommentId"" as ParentCommentId,               
n.""ParentCommentId"" as ParentId,false as hasChildren,false as expanded,
	n.""CommentedTo""	as	CommentedTo	,f.""FileName""  as FileName	,n.""CommentedByUserId"" as CommentedByUserId   
			   
			   from public.""NtsNoteComment"" as n
join public.""User"" as ub ON ub.""Id"" = n.""CommentedByUserId"" and ub.""IsDeleted""=false
	join cmn as p on p.""Id""=n.""ParentCommentId""
left join public.""NtsNoteCommentUser"" as nut ON nut.""NtsNoteCommentId"" = n.""Id"" and nut.""IsDeleted""=false
 left join public.""User"" as ut ON ut.""Id"" = nut.""CommentToUserId"" and ut.""IsDeleted""=false
	left join public.""File"" as f on f.""Id""=n.""AttachmentId"" and f.""IsDeleted""=false
 where  n.""IsDeleted""= false
 and (nut.""Id"" is null or n.""CommentedByUserId""='{_repo.UserContext.UserId}' or nut.""CommentToUserId""='{_repo.UserContext.UserId}')


)select *,case when CommentedTo=0 then 'All' else string_agg(CommentedToUserName,'; ') end as CommentedToUserName
from cmn
group by ""Id"",CommentedByUserName,PhotoId,CommentedDate,Comment,CommentedToUserName,AttachmentId
,ParentCommentId,ParentId,hasChildren,expanded,CommentedTo,FileName,CommentedByUserId



";
            }
            var list = await _queryRepo.ExecuteQueryList<NtsNoteCommentViewModel>(query, null);
            return list;
        }


        private void AddReplyComments(List<NtsNoteCommentViewModel> result, List<NtsNoteCommentViewModel> list, string parentCommentId)
        {
            var reply = list.Where(x => x.ParentCommentId == parentCommentId).OrderByDescending(x => x.CreatedDate).ToList();
            foreach (var item in reply)
            {
               // item.ParentId = parentId;
                result.Add(item);
                AddReplyComments(result, list, item.Id);
            }
        }

        public async  Task<List<NtsNoteCommentViewModel>> GetAllCommentTree(string NoteId)
        {
            var list = new List<NtsNoteCommentViewModel>();
            var replylist = new List<NtsNoteCommentViewModel>();
            string query = "";
                query = @$"select distinct n.""Id"" as id,n.""Id"" as Id,ub.""Name"" as CommentedByUserName,ub.""PhotoId"" as PhotoId,n.""CommentedByUserId"",
n.""CommentedDate"" as CommentedDate,n.""Comment"" as Comment,n.""AttachmentId"" as AttachmentId,n.""ParentCommentId"" as ParentCommentId,
case when n.""CommentedTo""=0 then 'All' else string_agg(ut.""Name"",'; ') end as CommentedToUserName,f.""FileName""  as FileName,		
null as ParentId,true as hasChildren,true as expanded
                from public.""NtsNoteComment"" as n
join public.""User"" as ub ON ub.""Id"" = n.""CommentedByUserId"" and ub.""IsDeleted""=false
left join public.""NtsNoteCommentUser"" as nut ON nut.""NtsNoteCommentId"" = n.""Id"" and nut.""IsDeleted""=false
 left join public.""User"" as ut ON ut.""Id"" = nut.""CommentToUserId"" and ut.""IsDeleted""=false
left join public.""NtsNoteComment"" as p on p.""Id""=n.""ParentCommentId"" and p.""IsDeleted""=false
left join public.""File"" as f on f.""Id""=n.""AttachmentId"" and f.""IsDeleted""=false
 where n.""NtsNoteId""='{NoteId}' AND n.""IsDeleted""= false and n.""ParentCommentId"" is null and (nut.""Id"" is null or nut.""CommentToUserId""='{_repo.UserContext.UserId}' 
or n.""CommentedByUserId""='{_repo.UserContext.UserId}'  )
 group by n.""Id"",ub.""Name"",ub.""PhotoId"",
n.""CommentedDate"",n.""Comment"",f.""FileName"" ";
            var result = await _queryRepo.ExecuteQueryList<NtsNoteCommentViewModel>(query, null);
            list.AddRange(result);

            foreach(var p in list)
            {
                query = $@" with recursive cmn as(
select distinct n.""Id"",ub.""Name"" as CommentedByUserName,ub.""PhotoId"" as PhotoId,
n.""CommentedDate"" as CommentedDate,n.""Comment"" as Comment,ut.""Name"" as CommentedToUserName
,n.""AttachmentId"" as AttachmentId,n.""ParentCommentId"" as ParentCommentId,               
n.""ParentCommentId"" as ParentId,false as hasChildren,false as expanded,
	n.""CommentedTo""	as	CommentedTo	 ,f.""FileName""  as FileName ,n.""CommentedByUserId"" as CommentedByUserId
			   
			   from public.""NtsNoteComment"" as n
join public.""User"" as ub ON ub.""Id"" = n.""CommentedByUserId"" and ub.""IsDeleted""=false
left join public.""NtsNoteCommentUser"" as nut ON nut.""NtsNoteCommentId"" = n.""Id"" and nut.""IsDeleted""=false
 left join public.""User"" as ut ON ut.""Id"" = nut.""CommentToUserId"" and ut.""IsDeleted""=false
	left join public.""File"" as f on f.""Id""=n.""AttachmentId"" and f.""IsDeleted""=false
 where  n.""IsDeleted""= false and n.""ParentCommentId""='{p.id}'
and (nut.""Id"" is null or n.""CommentedByUserId""='{_repo.UserContext.UserId}' or nut.""CommentToUserId""='{_repo.UserContext.UserId}')


	union
	
	select distinct n.""Id"" as Id,ub.""Name"" as CommentedByUserName,ub.""PhotoId"" as PhotoId,
n.""CommentedDate"" as CommentedDate,n.""Comment"" as Comment,ut.""Name"" as CommentedToUserName
,n.""AttachmentId"" as AttachmentId,n.""ParentCommentId"" as ParentCommentId,               
n.""ParentCommentId"" as ParentId,false as hasChildren,false as expanded,
	n.""CommentedTo""	as	CommentedTo	,f.""FileName""  as FileName	,n.""CommentedByUserId"" as CommentedByUserId   
			   
			   from public.""NtsNoteComment"" as n
join public.""User"" as ub ON ub.""Id"" = n.""CommentedByUserId"" and ub.""IsDeleted""=false
	join cmn as p on p.""Id""=n.""ParentCommentId""
left join public.""NtsNoteCommentUser"" as nut ON nut.""NtsNoteCommentId"" = n.""Id"" and nut.""IsDeleted""=false
 left join public.""User"" as ut ON ut.""Id"" = nut.""CommentToUserId"" and ut.""IsDeleted""=false
	left join public.""File"" as f on f.""Id""=n.""AttachmentId"" and f.""IsDeleted""=false
 where  n.""IsDeleted""= false
 and (nut.""Id"" is null or n.""CommentedByUserId""='{_repo.UserContext.UserId}' or nut.""CommentToUserId""='{_repo.UserContext.UserId}')


)select *,case when CommentedTo=0 then 'All' else string_agg(CommentedToUserName,'; ') end as CommentedToUserName
from cmn
group by ""Id"",CommentedByUserName,PhotoId,CommentedDate,Comment,CommentedToUserName,AttachmentId
,ParentCommentId,ParentId,hasChildren,expanded,CommentedTo,FileName,CommentedByUserId



";
                var result1 = await _queryRepo.ExecuteQueryList<NtsNoteCommentViewModel>(query, null);
                replylist.AddRange(result1);
            }

            list.AddRange(replylist);
            return list.ToList();
        }
    }
}
