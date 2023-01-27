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
    public class NtsNoteCommentBusiness : BusinessBase<NtsNoteCommentViewModel, NtsNoteComment>,INtsNoteCommentBusiness
    {
        private readonly IRepositoryQueryBase<NtsNoteCommentViewModel> _queryRepo;
        private readonly INoteBusiness _noteBusiness;
        private readonly INtsNoteCommentUserBusiness _commentuserBusiness;
        private readonly INtsQueryBusiness _ntsQueryBusiness;
        public NtsNoteCommentBusiness(IRepositoryBase<NtsNoteCommentViewModel, NtsNoteComment> repo, IMapper autoMapper
            , IRepositoryQueryBase<NtsNoteCommentViewModel> queryRepo, INoteBusiness noteBusiness, INtsNoteCommentUserBusiness commentuserBusiness
             , INtsQueryBusiness ntsQueryBusiness) : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;
            _noteBusiness = noteBusiness;
            _commentuserBusiness = commentuserBusiness;
            _ntsQueryBusiness = ntsQueryBusiness;
        }

        public async override Task<CommandResult<NtsNoteCommentViewModel>> Create(NtsNoteCommentViewModel model, bool autoCommit = true)
        {


            // var data = _autoMapper.Map<UserRoleViewModel>(model);
            if (model.CommentToUserIds.IsNotNull())
            {
                if (model.CommentToUserIds.Contains("All"))
                {
                    model.CommentedTo = CommentToEnum.All;
                    var result = await base.Create(model,autoCommit);
                    await SendNotification(model);
                    return CommandResult<NtsNoteCommentViewModel>.Instance(model, result.IsSuccess, result.Messages);
                }
                else
                {
                    model.CommentedTo = CommentToEnum.User;
                    var result = await base.Create(model,autoCommit);
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
                var result = await base.Create(model,autoCommit);
               // await SendNotification(model);
                return CommandResult<NtsNoteCommentViewModel>.Instance(model, result.IsSuccess, result.Messages);

            }
            //var result = await base.Create(model,autoCommit);
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
        public async override Task<CommandResult<NtsNoteCommentViewModel>> Edit(NtsNoteCommentViewModel model, bool autoCommit = true)
        {  
            var result = await base.Edit(model,autoCommit);           
            return CommandResult<NtsNoteCommentViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        

        public async Task<List<NtsNoteCommentViewModel>> GetSearchResult(string NoteId)
        {

  
            var list = await _ntsQueryBusiness.GetSearchResultData(NoteId);
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

            var list = await _ntsQueryBusiness.GetCommentTreeData(NoteId, Id);
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
         
            var result = await _ntsQueryBusiness.GetAllCommentTreeData(NoteId);
            list.AddRange(result);

            foreach(var p in list)
            {
        
                var result1 = await _ntsQueryBusiness.GetAllCommentTreeData1(p.id);
                replylist.AddRange(result1);
            }

            list.AddRange(replylist);
            return list.ToList();
        }
    }
}
