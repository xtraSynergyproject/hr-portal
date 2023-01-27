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
    public class NtsServiceCommentBusiness : BusinessBase<NtsServiceCommentViewModel, NtsServiceComment>, INtsServiceCommentBusiness
    {
        private readonly IRepositoryQueryBase<NtsServiceCommentViewModel> _queryRepo;
        private readonly IServiceBusiness _serviceBusiness;
        private readonly INtsQueryBusiness _ntsQueryBusiness;
        public NtsServiceCommentBusiness(IRepositoryBase<NtsServiceCommentViewModel, NtsServiceComment> repo, IMapper autoMapper,
           IServiceBusiness serviceBusiness, IRepositoryQueryBase<NtsServiceCommentViewModel> queryRepo, INtsQueryBusiness ntsQueryBusiness) : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;
            _serviceBusiness = serviceBusiness;
            _ntsQueryBusiness = ntsQueryBusiness;


        }

        public async override Task<CommandResult<NtsServiceCommentViewModel>> Create(NtsServiceCommentViewModel model, bool autoCommit = true)
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
                    var result = await base.Create(model,autoCommit);
                    await SendNotification(model);
                    return CommandResult<NtsServiceCommentViewModel>.Instance(model, result.IsSuccess, result.Messages);
                }
                else
                {
                    model.CommentedTo = CommentToEnum.User;
                    var result = await base.Create(model,autoCommit);
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
                var result = await base.Create(model,autoCommit);
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

        public async override Task<CommandResult<NtsServiceCommentViewModel>> Edit(NtsServiceCommentViewModel model, bool autoCommit = true)
        {
            var result = await base.Edit(model,autoCommit);
            return CommandResult<NtsServiceCommentViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }


        public async Task<List<NtsServiceCommentViewModel>> GetSearchResult(string ServiceId)
        {
           var list = await _ntsQueryBusiness.GetSearchResultData1(ServiceId);
           return list;
        }


        public async Task<List<NtsServiceCommentViewModel>> GetCommentTree(string ServiceId, string Id = null)
        {

            var list = await _ntsQueryBusiness.GetCommentTreeData1(ServiceId,Id);
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
           
            var result = await _ntsQueryBusiness.GetAllCommentTree(serviceId);
            list.AddRange(result);

            foreach (var p in list)
            {
           
                var result1 = await _ntsQueryBusiness.GetAllCommentTree1(p.id);
                replylist.AddRange(result1);


            }
            list.AddRange(replylist);
            return list;
        }
    }
}
