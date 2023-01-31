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
    public class NtsServiceCommentBusiness : BusinessBase<NtsServiceCommentViewModel, NtsServiceComment>, INtsServiceCommentBusiness
    {
        private readonly IRepositoryQueryBase<NtsServiceCommentViewModel> _queryRepo;
        private readonly IServiceBusiness _serviceBusiness;
        public NtsServiceCommentBusiness(IRepositoryBase<NtsServiceCommentViewModel, NtsServiceComment> repo, IMapper autoMapper,
           IServiceBusiness serviceBusiness, IRepositoryQueryBase<NtsServiceCommentViewModel> queryRepo) : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;
            _serviceBusiness = serviceBusiness;


        }

        public async override Task<CommandResult<NtsServiceCommentViewModel>> Create(NtsServiceCommentViewModel model)
        {

            // var data = _autoMapper.Map<UserRoleViewModel>(model);


            if (model.CommentToUserIds.IsNotNull())
            {
                if (model.CommentToUserIds.Contains("All"))
                {
                    //var userIds=await _serviceBusiness.GetServiceUserList(model.NtsServiceId);
                    //foreach (var user in userIds)
                    //{
                    //    var newmodel = new NtsServiceCommentUserViewModel
                    //    {
                    //        NtsServiceCommentId = result.Item.Id,
                    //        CommentToUserId = user.Id,
                    //        DataAction = DataActionEnum.Create
                    //    };

                    //    await base.Create<NtsServiceCommentUserViewModel, NtsServiceCommentUser>(newmodel);
                    //}
                    model.CommentedTo = CommentToEnum.All;
                    var result = await base.Create(model);
                    await SendNotification(model);
                    return CommandResult<NtsServiceCommentViewModel>.Instance(model, result.IsSuccess, result.Messages);
                }
                else
                {
                    model.CommentedTo = CommentToEnum.User;
                    var result = await base.Create(model);
                    foreach (var user in model.CommentToUserIds)
                    {
                        var newmodel = new NtsServiceCommentUserViewModel
                        {
                            NtsServiceCommentId = result.Item.Id,
                            CommentToUserId = user,
                            DataAction = DataActionEnum.Create
                        };

                        await base.Create<NtsServiceCommentUserViewModel, NtsServiceCommentUser>(newmodel);
                    }
                    await SendNotification(model);
                    return CommandResult<NtsServiceCommentViewModel>.Instance(model, result.IsSuccess, result.Messages);
                }


            }

            else
            {
                var result = await base.Create(model);
                //  await SendNotification(model);
                return CommandResult<NtsServiceCommentViewModel>.Instance(model, result.IsSuccess, result.Messages);

            }


        }

        private async Task SendNotification(NtsServiceCommentViewModel model)
        {
            var sermodel = new ServiceTemplateViewModel();
            sermodel.ServiceId = model.NtsServiceId;
            sermodel.DataAction = DataActionEnum.Read;

            var note = await _serviceBusiness.GetServiceDetails(sermodel);
            note.PostComment = model.Comment;
            note.OwnerUserId = _repo.UserContext.UserId;
            var notificationTemplate = await _repo.GetSingle<NotificationTemplate, NotificationTemplate>(x => x.Code == "SERVICE_COMMENT_NOTIFICATION_TEMPLATE" && x.NtsType == NtsTypeEnum.Service);
            if (model.CommentToUserIds.IsNotNull())
            {
                if (model.CommentToUserIds.Contains("All"))
                {
                    var userIds = await _serviceBusiness.GetServiceUserList(model.NtsServiceId);
                    foreach (var user in userIds)
                    {
                        await _serviceBusiness.SendNotification(note, notificationTemplate, user.Id);
                    }
                }
                else
                {
                    foreach (var user in model.CommentToUserIds)
                    {
                        await _serviceBusiness.SendNotification(note, notificationTemplate, user);
                    }

                }

            }
            //else
            //{
            //    var list = await _serviceBusiness.GetSharedList(model.NtsServiceId);
            //    foreach (var item in list)
            //    {
            //        await _serviceBusiness.SendNotification(note, notificationTemplate, item.Id);
            //    }
            //}
        }

        public async override Task<CommandResult<NtsServiceCommentViewModel>> Edit(NtsServiceCommentViewModel model)
        {
            var result = await base.Edit(model);
            return CommandResult<NtsServiceCommentViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }


        public async Task<List<NtsServiceCommentViewModel>> GetSearchResult(string ServiceId)
        {

            string query = @$"select distinct n.""Id"" as Id,ub.""Name"" as CommentedByUserName,ub.""PhotoId"" as PhotoId,n.""CommentedByUserId"",
n.""CommentedDate"" as CommentedDate,n.""Comment"" as Comment,n.""ParentCommentId"" as ParentCommentId,
case when n.""CommentedTo""=0 then 'All' else string_agg(ut.""Name"",'; ') end as CommentedToUserName
                from public.""NtsServiceComment"" as n
join public.""User"" as ub ON ub.""Id"" = n.""CommentedByUserId"" and ub.""IsDeleted""=false
left join public.""NtsServiceCommentUser"" as nut ON nut.""NtsServiceCommentId"" = n.""Id"" and nut.""IsDeleted""=false
 left join public.""User"" as ut ON ut.""Id"" = nut.""CommentToUserId"" and ut.""IsDeleted""=false
left join public.""NtsServiceComment"" as p on p.""Id""=n.""ParentCommentId"" and p.""IsDeleted""=false
 where n.""NtsServiceId""='{ServiceId}' AND n.""IsDeleted""= false and (nut.""Id"" is null or nut.""CommentToUserId""='{_repo.UserContext.UserId}' 
or n.""CommentedByUserId""='{_repo.UserContext.UserId}'  )
 group by n.""Id"",ub.""Name"",ub.""PhotoId"",
n.""CommentedDate"",n.""Comment"" ";

            var list = await _queryRepo.ExecuteQueryList<NtsServiceCommentViewModel>(query, null);
            //var result = new List<NtsServiceCommentViewModel>();
            //var firstLevelComments = list.Where(x => x.ParentCommentId == null).OrderByDescending(x => x.CreatedDate);
            //foreach (var item in firstLevelComments)
            //{
            //    result.Add(item);
            //    AddReplyComments(result, list, item.Id);
            //}

            //return result;
            return list;
        }


        public async Task<List<NtsServiceCommentViewModel>> GetCommentTree(string ServiceId, string Id = null)
        {

            string query = "";
            if (Id == null)
            {
                query = @$"select distinct n.""Id"" as id,n.""Id"" as Id,ub.""Name"" as CommentedByUserName,ub.""PhotoId"" as PhotoId,n.""CommentedByUserId"",
n.""CommentedDate"" as CommentedDate,n.""Comment"" as Comment,n.""AttachmentId"" as AttachmentId,n.""ParentCommentId"" as ParentCommentId,
case when n.""CommentedTo""=0 then 'All' else string_agg(ut.""Name"",'; ') end as CommentedToUserName,f.""FileName""  as FileName,		
null as ParentId,true as hasChildren,true as expanded
                from public.""NtsServiceComment"" as n
join public.""User"" as ub ON ub.""Id"" = n.""CommentedByUserId"" and ub.""IsDeleted""=false
left join public.""NtsServiceCommentUser"" as nut ON nut.""NtsServiceCommentId"" = n.""Id"" and nut.""IsDeleted""=false
 left join public.""User"" as ut ON ut.""Id"" = nut.""CommentToUserId"" and ut.""IsDeleted""=false
left join public.""NtsServiceComment"" as p on p.""Id""=n.""ParentCommentId"" and p.""IsDeleted""=false
left join public.""File"" as f on f.""Id""=n.""AttachmentId"" and f.""IsDeleted""=false
 where n.""NtsServiceId""='{ServiceId}' AND n.""IsDeleted""= false and n.""ParentCommentId"" is null and (nut.""Id"" is null or nut.""CommentToUserId""='{_repo.UserContext.UserId}' 
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
			   
			   from public.""NtsServiceComment"" as n
join public.""User"" as ub ON ub.""Id"" = n.""CommentedByUserId"" and ub.""IsDeleted""=false
left join public.""NtsServiceCommentUser"" as nut ON nut.""NtsServiceCommentId"" = n.""Id"" and nut.""IsDeleted""=false
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
			   
			   from public.""NtsServiceComment"" as n
join public.""User"" as ub ON ub.""Id"" = n.""CommentedByUserId"" and ub.""IsDeleted""=false
	join cmn as p on p.""Id""=n.""ParentCommentId""
left join public.""NtsServiceCommentUser"" as nut ON nut.""NtsServiceCommentId"" = n.""Id"" and nut.""IsDeleted""=false
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
            var list = await _queryRepo.ExecuteQueryList<NtsServiceCommentViewModel>(query, null);
            //var result = new List<NtsServiceCommentViewModel>();
            //var firstLevelComments = list.Where(x => x.ParentCommentId == null).OrderByDescending(x => x.CreatedDate);
            //foreach (var item in firstLevelComments)
            //{                
            //    result.Add(item);
            //    AddReplyComments(result, list, item.Id);
            //}

            //return result;
            return list;
        }

        private void AddReplyComments(List<NtsServiceCommentViewModel> result, List<NtsServiceCommentViewModel> list, string parentCommentId)
        {
            var reply = list.Where(x => x.ParentCommentId == parentCommentId).OrderByDescending(x => x.CreatedDate).ToList();
            foreach (var item in reply)
            {
                // item.ParentId = parentId;
                result.Add(item);
                AddReplyComments(result, list, item.Id);
            }
        }

        public async Task<List<NtsServiceCommentViewModel>> GetAllCommentTree(string serviceId)
        {
            var list = new List<NtsServiceCommentViewModel>();
            var replylist = new List<NtsServiceCommentViewModel>();
            string query = "";

            query = @$"select distinct n.""Id"" as id,n.""Id"" as Id,ub.""Name"" as CommentedByUserName,ub.""PhotoId"" as PhotoId,n.""CommentedByUserId"",
                            n.""CommentedDate"" as CommentedDate,n.""Comment"" as Comment,n.""AttachmentId"" as AttachmentId,n.""ParentCommentId"" as ParentCommentId,
                            case when n.""CommentedTo""=0 then 'All' else string_agg(ut.""Name"",'; ') end as CommentedToUserName,f.""FileName""  as FileName,		
                            null as ParentId,true as hasChildren,true as expanded
                                            from public.""NtsServiceComment"" as n
                            join public.""User"" as ub ON ub.""Id"" = n.""CommentedByUserId"" and ub.""IsDeleted""=false
                            left join public.""NtsServiceCommentUser"" as nut ON nut.""NtsServiceCommentId"" = n.""Id"" and nut.""IsDeleted""=false
                             left join public.""User"" as ut ON ut.""Id"" = nut.""CommentToUserId"" and ut.""IsDeleted""=false
                            left join public.""NtsServiceComment"" as p on p.""Id""=n.""ParentCommentId"" and p.""IsDeleted""=false
                            left join public.""File"" as f on f.""Id""=n.""AttachmentId"" and f.""IsDeleted""=false
                             where n.""NtsServiceId""='{serviceId}' AND n.""IsDeleted""= false and n.""ParentCommentId"" is null and (nut.""Id"" is null or nut.""CommentToUserId""='{_repo.UserContext.UserId}' 
                            or n.""CommentedByUserId""='{_repo.UserContext.UserId}'  )
                             group by n.""Id"",ub.""Name"",ub.""PhotoId"",
                            n.""CommentedDate"",n.""Comment"",f.""FileName"" ";
            var result = await _queryRepo.ExecuteQueryList<NtsServiceCommentViewModel>(query, null);
            list.AddRange(result);

            foreach (var p in list)
            {
                query = $@" with recursive cmn as(
                        select distinct n.""Id"",ub.""Name"" as CommentedByUserName,ub.""PhotoId"" as PhotoId,
                        n.""CommentedDate"" as CommentedDate,n.""Comment"" as Comment,ut.""Name"" as CommentedToUserName
                        ,n.""AttachmentId"" as AttachmentId,n.""ParentCommentId"" as ParentCommentId,               
                        n.""ParentCommentId"" as ParentId,false as hasChildren,false as expanded,
	                        n.""CommentedTo""	as	CommentedTo	 ,f.""FileName""  as FileName ,n.""CommentedByUserId"" as CommentedByUserId
			   
			                           from public.""NtsServiceComment"" as n
                        join public.""User"" as ub ON ub.""Id"" = n.""CommentedByUserId"" and ub.""IsDeleted""=false
                        left join public.""NtsServiceCommentUser"" as nut ON nut.""NtsServiceCommentId"" = n.""Id"" and nut.""IsDeleted""=false
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
			   
			                           from public.""NtsServiceComment"" as n
                        join public.""User"" as ub ON ub.""Id"" = n.""CommentedByUserId"" and ub.""IsDeleted""=false
	                        join cmn as p on p.""Id""=n.""ParentCommentId""
                        left join public.""NtsServiceCommentUser"" as nut ON nut.""NtsServiceCommentId"" = n.""Id"" and nut.""IsDeleted""=false
                         left join public.""User"" as ut ON ut.""Id"" = nut.""CommentToUserId"" and ut.""IsDeleted""=false
	                        left join public.""File"" as f on f.""Id""=n.""AttachmentId"" and f.""IsDeleted""=false
                         where  n.""IsDeleted""= false
                         and (nut.""Id"" is null or n.""CommentedByUserId""='{_repo.UserContext.UserId}' or nut.""CommentToUserId""='{_repo.UserContext.UserId}')


                        )select *,case when CommentedTo=0 then 'All' else string_agg(CommentedToUserName,'; ') end as CommentedToUserName
                        from cmn
                        group by ""Id"",CommentedByUserName,PhotoId,CommentedDate,Comment,CommentedToUserName,AttachmentId
                        ,ParentCommentId,ParentId,hasChildren,expanded,CommentedTo,FileName,CommentedByUserId



                        ";
                var result1 = await _queryRepo.ExecuteQueryList<NtsServiceCommentViewModel>(query, null);
                replylist.AddRange(result1);


            }
            list.AddRange(replylist);
            return list;
        }
    }
}
