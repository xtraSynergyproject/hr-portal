using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace Synergy.App.Business
{
    public class NtsTaskCommentBusiness : BusinessBase<NtsTaskCommentViewModel, NtsTaskComment>, INtsTaskCommentBusiness
    {
        private readonly IRepositoryQueryBase<NtsTaskCommentViewModel> _queryRepo;
        private readonly ITaskBusiness _taskBusiness;
        private readonly INtsTaskCommentUserBusiness _commentuserBusiness;
        private readonly INtsQueryBusiness _ntsQueryBusiness;
        public NtsTaskCommentBusiness(IRepositoryBase<NtsTaskCommentViewModel, NtsTaskComment> repo, IMapper autoMapper, IRepositoryQueryBase<NtsTaskCommentViewModel> queryRepo, ITaskBusiness taskBusiness, INtsTaskCommentUserBusiness commentuserBusiness
            , INtsQueryBusiness ntsQueryBusiness) : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;
            _taskBusiness = taskBusiness;
            _commentuserBusiness = commentuserBusiness;
            _ntsQueryBusiness = ntsQueryBusiness;


        }

        public async override Task<CommandResult<NtsTaskCommentViewModel>> Create(NtsTaskCommentViewModel model, bool autoCommit = true)
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
                    var result = await base.Create(model,autoCommit);
                    await SendNotification(model);
                    return CommandResult<NtsTaskCommentViewModel>.Instance(model, result.IsSuccess, result.Messages);
                }
                else
                {
                    model.CommentedTo = CommentToEnum.User;
                    var result = await base.Create(model,autoCommit);
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
                var result = await base.Create(model,autoCommit);
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

        public async override Task<CommandResult<NtsTaskCommentViewModel>> Edit(NtsTaskCommentViewModel model, bool autoCommit = true)
        {
            var result = await base.Edit(model,autoCommit);
            return CommandResult<NtsTaskCommentViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }


        public async Task<List<NtsTaskCommentViewModel>> GetSearchResult(string TaskId)
        {
       
            var list = await _ntsQueryBusiness.GetSearchCommentResultData(TaskId);
           return list;
        }

        public async Task<List<NtsTaskCommentViewModel>> GetCommentTree(string TaskId, string Id = null)
        {
            var list = await _ntsQueryBusiness.GetTaskCommentTreeData(TaskId, Id);
            return list;
        }



        public async Task<List<IdNameViewModel>> GetTakCommentUserList(string TaskId)
        {
            var list = await _ntsQueryBusiness.GetTaskCommentUserListData(TaskId);
             return list;
          
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
            var result = await _ntsQueryBusiness.GetAllTaskCommentTreeData(taskId);
            list.AddRange(result);

            foreach (var p in list)
            {
                var result1 = await _ntsQueryBusiness.GetAllTaskCommentTreeData1(taskId);
                replylist.AddRange(result1);


            }
            list.AddRange(replylist);
            return list;
        }
    }
}
