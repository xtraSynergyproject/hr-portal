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
    public class NtsTaskCommentBusiness : BusinessBase<NtsTaskCommentViewModel, NtsTaskComment>, INtsTaskCommentBusiness
    {
        private readonly IRepositoryQueryBase<NtsTaskCommentViewModel> _queryRepo;
        private readonly ITaskBusiness _taskBusiness;
        private readonly INtsTaskCommentUserBusiness _commentuserBusiness;
        public NtsTaskCommentBusiness(IRepositoryBase<NtsTaskCommentViewModel, NtsTaskComment> repo, IMapper autoMapper, IRepositoryQueryBase<NtsTaskCommentViewModel> queryRepo, ITaskBusiness taskBusiness, INtsTaskCommentUserBusiness commentuserBusiness) : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;
            _taskBusiness = taskBusiness;
            _commentuserBusiness = commentuserBusiness;


        }

        public async override Task<CommandResult<NtsTaskCommentViewModel>> Create(NtsTaskCommentViewModel model)
        {

            // var data = _autoMapper.Map<UserRoleViewModel>(model);


            if (model.CommentToUserIds.IsNotNull())
            {
                if (model.CommentToUserIds.Contains("All"))
                {
                    //var userIds = await _taskBusiness.GetTaskUserList(model.NtsTaskId);
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
                    return CommandResult<NtsTaskCommentViewModel>.Instance(model, result.IsSuccess, result.Messages);
                }
                else
                {
                    model.CommentedTo = CommentToEnum.User;
                    var result = await base.Create(model);
                    foreach (var user in model.CommentToUserIds)
                    {
                        var newmodel = new NtsTaskCommentUserViewModel
                        {
                            NtsTaskCommentId = result.Item.Id,
                            CommentToUserId = user,
                            DataAction = DataActionEnum.Create
                        };
                        await _commentuserBusiness.Create(newmodel);
                    }
                    await SendNotification(model);
                    return CommandResult<NtsTaskCommentViewModel>.Instance(model, result.IsSuccess, result.Messages);
                }

            }

            else
            {
                var result = await base.Create(model);
                return CommandResult<NtsTaskCommentViewModel>.Instance(model, result.IsSuccess, result.Messages);
            }


        }
        private async Task SendNotification(NtsTaskCommentViewModel model)
        {
            var taskmodel = new TaskTemplateViewModel();
            taskmodel.TaskId = model.NtsTaskId;
            taskmodel.DataAction = DataActionEnum.Read;
            var task = await _taskBusiness.GetTaskDetails(taskmodel);
            task.PostComment = model.Comment;
            task.OwnerUserId = _repo.UserContext.UserId;
            var notificationTemplate = await _repo.GetSingle<NotificationTemplate, NotificationTemplate>(x => x.Code == "TASK_COMMENT_NOTIFICATION_TEMPLATE" && x.NtsType == NtsTypeEnum.Task);
            if (model.CommentToUserIds.IsNotNull())
            {
                if (model.CommentToUserIds.Contains("All"))
                {
                    var userIds = await _taskBusiness.GetTaskUserList(model.NtsTaskId);
                    foreach (var user in userIds)
                    {
                        await _taskBusiness.SendNotification(task, notificationTemplate, user.Id);
                    }
                }
                else
                {

                    foreach (var user in model.CommentToUserIds)
                    {
                        await _taskBusiness.SendNotification(task, notificationTemplate, user);
                    }
                }
            }
            //else
            //{
            //    var list = await _taskBusiness.GetSharedList(model.NtsTaskId);
            //    foreach (var item in list)
            //    {
            //        await _taskBusiness.SendNotification(task, notificationTemplate, item.Id);
            //    }
            //}
        }

        public async override Task<CommandResult<NtsTaskCommentViewModel>> Edit(NtsTaskCommentViewModel model)
        {
            var result = await base.Edit(model);
            return CommandResult<NtsTaskCommentViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }


        public async Task<List<NtsTaskCommentViewModel>> GetSearchResult(string TaskId)
        {

            //            string query = @$"select n.""Id"" as Id,ut.""Name"" as CommentedToUserName,ub.""Name"" as CommentedByUserName,ub.""PhotoId"" as PhotoId,
            //n.""CommentedDate"" as CommentedDate,n.""Comment"" as Comment
            //                              from public.""NtsTaskComment"" as n
            //join public.""User"" as ub ON ub.""Id"" = n.""CommentedByUserId"" and ub.""IsDeleted""=false
            // left join public.""User"" as ut ON ut.""Id"" = n.""CommentToUserId"" and ut.""IsDeleted""=false
            //left join public.""NtsTaskComment"" as p on p.""Id""=n.""ParentCommentId"" and p.""IsDeleted""=false
            // where n.""NtsTaskId""='{TaskId}' AND n.""IsDeleted""= false";
            string query = @$"select distinct n.""Id"" as Id,ub.""Name"" as CommentedByUserName,ub.""PhotoId"" as PhotoId,
n.""CommentedDate"" as CommentedDate,n.""Comment"" as Comment,case when n.""CommentedTo""=0 then 'All' else string_agg(ut.""Name"",'; ') end as CommentedToUserName
                              from public.""NtsTaskComment"" as n
join public.""User"" as ub ON ub.""Id"" = n.""CommentedByUserId"" and ub.""IsDeleted""=false
left join public.""NtsTaskCommentUser"" as nut ON nut.""NtsTaskCommentId"" = n.""Id""
 left join public.""User"" as ut ON ut.""Id"" = nut.""CommentToUserId"" and ut.""IsDeleted""=false
left join public.""NtsTaskComment"" as p on p.""Id""=n.""ParentCommentId"" and p.""IsDeleted""=false
 where n.""NtsTaskId""='{TaskId}' AND n.""IsDeleted""= false and (nut.""Id"" is null or nut.""CommentToUserId""='{_repo.UserContext.UserId}' 
or n.""CommentedByUserId""='{_repo.UserContext.UserId}'  )
group by n.""Id"",ub.""Name"",ub.""PhotoId"",
n.""CommentedDate"",n.""Comment"" ";
            var list = await _queryRepo.ExecuteQueryList<NtsTaskCommentViewModel>(query, null);
            //var result = new List<NtsTaskCommentViewModel>();
            //var firstLevelComments = list.Where(x => x.ParentCommentId == null).OrderByDescending(x => x.CreatedDate);
            //foreach (var item in firstLevelComments)
            //{                
            //    result.Add(item);
            //    AddReplyComments(result, list, item.Id);
            //}

            //return result;
            return list;
        }

        public async Task<List<NtsTaskCommentViewModel>> GetCommentTree(string TaskId, string Id = null)
        {

            string query = "";
            if (Id == null)
            {
                query = @$"select distinct n.""Id"" as id,n.""Id"" as Id,ub.""Name"" as CommentedByUserName,ub.""PhotoId"" as PhotoId,n.""CommentedByUserId"",
n.""CommentedDate"" as CommentedDate,n.""Comment"" as Comment,n.""AttachmentId"" as AttachmentId,n.""ParentCommentId"" as ParentCommentId,
case when n.""CommentedTo""=0 then 'All' else string_agg(ut.""Name"",'; ') end as CommentedToUserName,f.""FileName""  as FileName,		
null as ParentId,true as hasChildren,true as expanded
                from public.""NtsTaskComment"" as n
join public.""User"" as ub ON ub.""Id"" = n.""CommentedByUserId"" and ub.""IsDeleted""=false
left join public.""NtsTaskCommentUser"" as nut ON nut.""NtsTaskCommentId"" = n.""Id"" and nut.""IsDeleted""=false
 left join public.""User"" as ut ON ut.""Id"" = nut.""CommentToUserId"" and ut.""IsDeleted""=false
left join public.""NtsTaskComment"" as p on p.""Id""=n.""ParentCommentId"" and p.""IsDeleted""=false
left join public.""File"" as f on f.""Id""=n.""AttachmentId"" and f.""IsDeleted""=false
 where n.""NtsTaskId""='{TaskId}' AND n.""IsDeleted""= false and n.""ParentCommentId"" is null and (nut.""Id"" is null or nut.""CommentToUserId""='{_repo.UserContext.UserId}' 
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
			   
			   from public.""NtsTaskComment"" as n
join public.""User"" as ub ON ub.""Id"" = n.""CommentedByUserId"" and ub.""IsDeleted""=false
left join public.""NtsTaskCommentUser"" as nut ON nut.""NtsTaskCommentId"" = n.""Id"" and nut.""IsDeleted""=false
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
			   
			   from public.""NtsTaskComment"" as n
join public.""User"" as ub ON ub.""Id"" = n.""CommentedByUserId"" and ub.""IsDeleted""=false
	join cmn as p on p.""Id""=n.""ParentCommentId""
left join public.""NtsTaskCommentUser"" as nut ON nut.""NtsTaskCommentId"" = n.""Id"" and nut.""IsDeleted""=false
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
            var list = await _queryRepo.ExecuteQueryList<NtsTaskCommentViewModel>(query, null);
            return list;
        }



        public async Task<List<IdNameViewModel>> GetTakCommentUserList(string TaskId)
        {

            string query = @$"select ub.""CommentToUserId"" as Id
                                              from public.""NtsTaskComment"" as n
                join public.""NtsTaskComment"" as ub ON ub.""NtsTaskCommentId"" = n.""Id"" and ub.""IsDeleted""=false
                
                 where n.""NtsTaskId""='{TaskId}' AND n.""IsDeleted""= false";
            ;
            var list = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);

            return list;
            ///return list;
        }

        private void AddReplyComments(List<NtsTaskCommentViewModel> result, List<NtsTaskCommentViewModel> list, string parentCommentId)
        {
            var reply = list.Where(x => x.ParentCommentId == parentCommentId).OrderByDescending(x => x.CreatedDate).ToList();
            foreach (var item in reply)
            {
                // item.ParentId = parentId;
                result.Add(item);
                AddReplyComments(result, list, item.Id);
            }
        }

        public async Task<List<NtsTaskCommentViewModel>> GetAllCommentTree(string taskId)
        {
            var list = new List<NtsTaskCommentViewModel>();
            var replylist = new List<NtsTaskCommentViewModel>();
            string query = "";

            query = @$"select distinct n.""Id"" as id,n.""Id"" as Id,ub.""Name"" as CommentedByUserName,ub.""PhotoId"" as PhotoId,n.""CommentedByUserId"",
                        n.""CommentedDate"" as CommentedDate,n.""Comment"" as Comment,n.""AttachmentId"" as AttachmentId,n.""ParentCommentId"" as ParentCommentId,
                        case when n.""CommentedTo""=0 then 'All' else string_agg(ut.""Name"",'; ') end as CommentedToUserName,f.""FileName""  as FileName,		
                        null as ParentId,true as hasChildren,true as expanded
                                        from public.""NtsTaskComment"" as n
                        join public.""User"" as ub ON ub.""Id"" = n.""CommentedByUserId"" and ub.""IsDeleted""=false
                        left join public.""NtsTaskCommentUser"" as nut ON nut.""NtsTaskCommentId"" = n.""Id"" and nut.""IsDeleted""=false
                         left join public.""User"" as ut ON ut.""Id"" = nut.""CommentToUserId"" and ut.""IsDeleted""=false
                        left join public.""NtsTaskComment"" as p on p.""Id""=n.""ParentCommentId"" and p.""IsDeleted""=false
                        left join public.""File"" as f on f.""Id""=n.""AttachmentId"" and f.""IsDeleted""=false
                         where n.""NtsTaskId""='{taskId}' AND n.""IsDeleted""= false and n.""ParentCommentId"" is null and (nut.""Id"" is null or nut.""CommentToUserId""='{_repo.UserContext.UserId}' 
                        or n.""CommentedByUserId""='{_repo.UserContext.UserId}'  )
                         group by n.""Id"",ub.""Name"",ub.""PhotoId"",
                        n.""CommentedDate"",n.""Comment"",f.""FileName"" ";
            var result = await _queryRepo.ExecuteQueryList<NtsTaskCommentViewModel>(query, null);
            list.AddRange(result);

            foreach (var p in list)
            {
                query = $@" with recursive cmn as(
                            select distinct n.""Id"",ub.""Name"" as CommentedByUserName,ub.""PhotoId"" as PhotoId,
                            n.""CommentedDate"" as CommentedDate,n.""Comment"" as Comment,ut.""Name"" as CommentedToUserName
                            ,n.""AttachmentId"" as AttachmentId,n.""ParentCommentId"" as ParentCommentId,               
                            n.""ParentCommentId"" as ParentId,false as hasChildren,false as expanded,
	                            n.""CommentedTo""	as	CommentedTo	 ,f.""FileName""  as FileName ,n.""CommentedByUserId"" as CommentedByUserId
			   
			                               from public.""NtsTaskComment"" as n
                            join public.""User"" as ub ON ub.""Id"" = n.""CommentedByUserId"" and ub.""IsDeleted""=false
                            left join public.""NtsTaskCommentUser"" as nut ON nut.""NtsTaskCommentId"" = n.""Id"" and nut.""IsDeleted""=false
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
			   
			                               from public.""NtsTaskComment"" as n
                            join public.""User"" as ub ON ub.""Id"" = n.""CommentedByUserId"" and ub.""IsDeleted""=false
	                            join cmn as p on p.""Id""=n.""ParentCommentId""
                            left join public.""NtsTaskCommentUser"" as nut ON nut.""NtsTaskCommentId"" = n.""Id"" and nut.""IsDeleted""=false
                             left join public.""User"" as ut ON ut.""Id"" = nut.""CommentToUserId"" and ut.""IsDeleted""=false
	                            left join public.""File"" as f on f.""Id""=n.""AttachmentId"" and f.""IsDeleted""=false
                             where  n.""IsDeleted""= false
                             and (nut.""Id"" is null or n.""CommentedByUserId""='{_repo.UserContext.UserId}' or nut.""CommentToUserId""='{_repo.UserContext.UserId}')


                            )select *,case when CommentedTo=0 then 'All' else string_agg(CommentedToUserName,'; ') end as CommentedToUserName
                            from cmn
                            group by ""Id"",CommentedByUserName,PhotoId,CommentedDate,Comment,CommentedToUserName,AttachmentId
                            ,ParentCommentId,ParentId,hasChildren,expanded,CommentedTo,FileName,CommentedByUserId



                            ";
                var result1 = await _queryRepo.ExecuteQueryList<NtsTaskCommentViewModel>(query, null);
                replylist.AddRange(result1);


            }
            list.AddRange(replylist);
            return list;
        }
    }
}
