using Synergy.App.Repository;
using Synergy.App.ViewModel;
using Synergy.App.Common;
using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Synergy.App.DataModel;
using System.Data;

namespace Synergy.App.Business
{
    public class NtsBusiness : INtsBusiness
    {
        private readonly ITaskBusiness _taskBusiness;
        private readonly IServiceBusiness _serviceBusiness;
        private readonly INtsServiceCommentBusiness _serviceCommentBusiness;
        private readonly IUserBusiness _userBusiness;
        private readonly IRepositoryQueryBase<ServiceViewModel> _repositoryQueryBase;
        private readonly IRepositoryBase<ServiceViewModel, NtsService> _repo;
        private readonly INtsTaskCommentBusiness _taskCommentBusiness;
        private readonly IPushNotificationBusiness _pushNotificationBusiness;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILOVBusiness _lOVBusiness;
        private readonly INtsQueryBusiness _ntsQueryBusiness;
        public NtsBusiness(ITaskBusiness taskBusiness, IUserBusiness userBusiness, INtsTaskCommentBusiness taskCommentBusiness,
            IPushNotificationBusiness pushNotificationBusiness,
            IServiceBusiness serviceBusiness, INtsServiceCommentBusiness serviceCommentBusiness
            , IRepositoryQueryBase<ServiceViewModel> repositoryQueryBase, ILOVBusiness lOVBusiness
            , IRepositoryBase<ServiceViewModel, NtsService> repo, IServiceProvider serviceProvider
            , INtsQueryBusiness ntsQueryBusiness)
        {
            _taskBusiness = taskBusiness;
            _userBusiness = userBusiness;
            _taskCommentBusiness = taskCommentBusiness;
            _pushNotificationBusiness = pushNotificationBusiness;
            _serviceBusiness = serviceBusiness;
            _serviceCommentBusiness = serviceCommentBusiness;
            _repositoryQueryBase = repositoryQueryBase;
            _repo = repo;
            _serviceProvider = serviceProvider;
            _lOVBusiness = lOVBusiness;
            _ntsQueryBusiness = ntsQueryBusiness;
        }
        public async Task<IList<NTSMessageViewModel>> GetAttachedReplies(string userId, string taskId)
        {
            var list = new List<NTSMessageViewModel>();

            var task = await _taskBusiness.GetSingleById(taskId);
            if (task != null)
            {
                var owner = await _userBusiness.GetSingleById(task.OwnerUserId);
                var assignee = await _userBusiness.GetSingleById(task.AssignedToUserId);
                list.Add(new NTSMessageViewModel
                {
                    Body = task.TaskDescription,
                    // CC = task.c,
                    From = owner.Name,
                    FromEmail = owner.Email,
                    //SentDate = task.CreatedDate.ToString(),
                    Subject = task.TaskSubject,
                    To = assignee != null ? assignee.Name : null,
                    ToEmail = assignee != null ? assignee.Email : null,
                    Type = "Task"
                });
            }
            var comments = await _taskCommentBusiness.GetList(x => x.NtsTaskId == taskId);
            if (comments != null)
            {
                foreach (var data in comments)
                {
                    var owner = await _userBusiness.GetSingleById(data.CommentedByUserId);
                    var userIds = await _taskCommentBusiness.GetTakCommentUserList(taskId);
                    var commentuser = userIds.FirstOrDefault();
                    if (commentuser != null)
                    {
                        var assignee = await _userBusiness.GetSingleById(commentuser.Id);

                        list.Add(new NTSMessageViewModel
                        {
                            Body = data.Comment,
                            // CC = task.c,
                            From = owner.Name,
                            FromEmail = owner.Email,
                            //SentDate = data.CommentedDate.ToString(),                        
                            To = assignee != null ? assignee.Name : null,
                            ToEmail = assignee != null ? assignee.Email : null,
                            Type = "Comment"
                        });
                    }
                }
            }
            var notifications = await _pushNotificationBusiness.GetList(x => x.ReferenceTypeId == taskId && x.ReferenceType == Common.ReferenceTypeEnum.NTS_Task);
            if (notifications != null)
            {
                foreach (var data in notifications)
                {
                    //var ToUser = await _userBusiness.GetSingleById(data.ToUserId);
                    list.Add(new NTSMessageViewModel
                    {
                        Body = data.Body,
                        CC = data.CC,
                        From = data.From,
                        //SentDate = data.CreatedDate.ToString(),
                        Subject = data.Subject,
                        To = data.To,
                        Type = "Notification"
                    });
                }
            }
            return list.OrderByDescending(x => x.SentDate).ToList();
        }



        public async Task<IList<TreeViewViewModel>> GetNtsMenuList(string id, string userId, string email)
        {
            var list = new List<TreeViewViewModel>();

            if (id == null)
            {
                list.Add(new TreeViewViewModel
                {
                    id = "root",
                    Name = email,
                    DisplayName = email,
                    ParentId = null,
                    hasChildren = true,
                    expanded = true,
                    text = email,
                    parent = "#",
                    children = true,

                });

            }
            else if (id == "root")
            {
                list = new List<TreeViewViewModel>
            {
            new TreeViewViewModel
                {
                    id = "1",
                    Name = "Inbox",
                    DisplayName = "",
                    ParentId = "root",
                    PortalId = "",
                    ItemLevel = 0,
                    hasChildren = false,
                    Type = "",
                    IconCss = "fa fa-sm fa-envelope-open",
                    text="Inbox",
                    parent="root",
                    children=true,
                    icon="fa fa-sm fa-envelope-open"
                },
                new TreeViewViewModel
                {
                    id = "2",
                    Name = "Draft",
                    DisplayName = "",
                    ParentId = "root",
                    PortalId = "",
                    ItemLevel = 0,
                    hasChildren = false,
                    Type = "",
                    IconCss = "fa fa-sm fa-envelope-open-text",
                    text="Draft",
                    parent="root",
                    children=false,
                    icon="fa fa-sm fa-envelope-open-text"
                },
                new TreeViewViewModel
                {
                    id = "3",
                    Name = "Sent Items",
                    DisplayName = "",
                    ParentId = "root",
                    PortalId = "",
                    ItemLevel = 0,
                    hasChildren = false,
                    Type = "",
                    IconCss = "fa fa-sm fa-share-square",
                     text="Sent Items",
                    parent="root",
                    children=false,
                    icon="fa fa-sm fa-share-squar"
                },
                new TreeViewViewModel
                {
                    id = "4",
                    Name = "Deleted Items",
                    DisplayName = "",
                    ParentId = "root",
                    PortalId = "",
                    ItemLevel = 0,
                    hasChildren = false,
                    Type = "",
                    IconCss = "fa fa-sm fa-trash-alt",
                       text="Deleted Items",
                    parent="root",
                    children=false,
                    icon="fa fa-sm fa-trash-alt"
                },
                new TreeViewViewModel
                {
                    id = "6",
                    Name = "Archive",
                    DisplayName = "",
                    ParentId = "root",
                    PortalId = "",
                    ItemLevel = 0,
                    hasChildren = false,
                    Type = "",
                     IconCss = "fa fa-sm fa-archive",
                    text="Archive",
                    parent="root",
                    children=false,
                    icon="fa fa-sm fa-archive"
                },
                new TreeViewViewModel
                {
                    id = "7",
                    Name = "Conversation History",
                    DisplayName = "",
                    ParentId = "root",
                    PortalId = "",
                    ItemLevel = 0,
                    hasChildren = false,
                    Type = "",
                     IconCss = "fa fa-sm fa-history",
                       text="Conversation History",
                    parent="root",
                    children=false,
                    icon="fa fa-sm fa-history"
                },

                new TreeViewViewModel
                {
                    id = "8",
                    Name = "Junk Mail",
                    DisplayName = "",
                    ParentId = "root",
                    PortalId = "",
                    ItemLevel = 0,
                    hasChildren = false,
                    Type = "",
                     IconCss = "fa fa-sm fa-envelope-square",
                   text="Junk Mail",
                    parent="root",
                    children=false,
                    icon="fa fa-sm fa-envelope-square"
                },
                new TreeViewViewModel
                {
                    id = "9",
                    Name = "Outbox",
                    DisplayName = "",
                    ParentId = "root",
                    PortalId = "",
                    ItemLevel = 0,
                    hasChildren = false,
                    Type = "",
                     IconCss = "fa fa-sm fa-envelope-open",
                        text="Outbox",
                    parent="root",
                    children=false,
                    icon="fa fa-sm fa-envelope-open"
                },
                new TreeViewViewModel
                {
                    id = "10",
                    Name = "RSS Feeds",
                    DisplayName = "",
                    ParentId = "root",
                    PortalId = "",
                    ItemLevel = 0,
                    hasChildren = false,
                    Type = "",
                     IconCss = "fa fa-sm fa-rss-square",
                       text="RSS Feeds",
                    parent="root",
                    children=false,
                    icon="fa fa-sm fa-rss-square"
                },
                new TreeViewViewModel
                {
                    id = "11",
                    Name = "Scheduled",
                    DisplayName = "",
                    ParentId = "root",
                    PortalId = "",
                    ItemLevel = 0,
                    hasChildren = false,
                    Type = "",
                     IconCss = "fa fa-sm fa-calendar-alt",
                     text="Scheduled",
                    parent="root",
                    children=false,
                    icon="fa fa-sm fa-calendar-alt"
                }
            };
            }

            return list.ToList();
        }


        public async Task UpdateNotStartedNts(DateTime dateTime)
        {
            //var query = @$"select s.* from public.""NtsService"" as s 
            //join public.""LOV"" as l on s.""ServiceStatusId""=l.""Id"" and l.""IsDeleted""=false
            //where s.""StartDate""<='{DateTime.Now.ToDatabaseDateFormat()}' and s.""IsDeleted""=false and l.""Code""='SERVICE_STATUS_NOTSTARTED'";
            //var serviceList = await _repositoryQueryBase.ExecuteQueryList<ServiceViewModel>(query, null);

            try
            {
                var serviceList = await _ntsQueryBusiness.UpdateNotStartedNtsData();
                foreach (var item in serviceList)
                {

                    var svm = new ServiceTemplateViewModel
                    {
                        ServiceId = item.Id,
                        DataAction = DataActionEnum.Edit,
                        ActiveUserId = _repo.UserContext.UserId
                    };
                    var service = await _serviceBusiness.GetServiceDetails(svm);
                    if (service != null && service.ServiceId != null)
                    {
                        service.ServiceStatusCode = "SERVICE_STATUS_INPROGRESS";
                        service.DataAction = DataActionEnum.Edit;
                        service.ActiveUserId = service.OwnerUserId;
                        await _serviceBusiness.ManageService(service);
                    }

                }



                var taskList = await _ntsQueryBusiness.UpdateNotStartedNtsData2();
                foreach (var item in taskList)
                {

                    var tvm = new TaskTemplateViewModel
                    {
                        TaskId = item.Id,
                        DataAction = DataActionEnum.Edit,
                        ActiveUserId = _repo.UserContext.UserId
                    };
                    var task = await _taskBusiness.GetTaskDetails(tvm);
                    if (task != null && task.TaskId != null)
                    {
                        task.TaskStatusCode = "TASK_STATUS_INPROGRESS";
                        task.DataAction = DataActionEnum.Edit;
                        task.ActiveUserId = task.AssignedToUserId;
                        await _taskBusiness.ManageTask(task);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

        }
        public async Task UpdateOverdueNts(DateTime dateTime)
        {
            try
            {
                var serviceList = await _ntsQueryBusiness.UpdateOverdueNtsData();
                foreach (var item in serviceList)
                {

                    try
                    {
                        var svm = new ServiceTemplateViewModel
                        {
                            ServiceId = item.Id,
                            DataAction = DataActionEnum.Edit,
                            ActiveUserId = _repo.UserContext.UserId
                        };
                        var service = await _serviceBusiness.GetServiceDetails(svm);
                        if (service != null && service.ServiceId != null)
                        {
                            service.ServiceStatusCode = "SERVICE_STATUS_OVERDUE";
                            service.DataAction = DataActionEnum.Edit;
                            service.ActiveUserId = svm.OwnerUserId;
                            await _serviceBusiness.ManageService(service);
                        }
                    }
                    catch (Exception)
                    {
                    }




                }

                var taskList = await _ntsQueryBusiness.UpdateOverdueNtsData1();
                foreach (var item in taskList)
                {
                    try
                    {
                        var tvm = new TaskTemplateViewModel
                        {
                            TaskId = item.Id,
                            DataAction = DataActionEnum.Edit,
                            ActiveUserId = _repo.UserContext.UserId,
                            TaskTemplateType=item.TaskType

                        };
                        var task = await _taskBusiness.GetTaskDetails(tvm);
                        if (task != null && task.TaskId != null)
                        {
                            task.TaskStatusCode = "TASK_STATUS_OVERDUE";
                            task.DataAction = DataActionEnum.Edit;
                            task.ActiveUserId = tvm.AssignedToUserId;
                            await _taskBusiness.ManageTask(task);
                        }
                    }
                    catch (Exception)
                    {

                    }

                }
            }
            catch (Exception)
            {
                throw;
            }


        }

        public async Task UpdateRating(NtsTypeEnum ntsType, string ntsId, string userId, int rating, string ratingComment)
        {
            var existingRate = await _repo.GetSingle<NtsRatingViewModel, NtsRating>(x => x.NtsType == ntsType && x.NtsId == ntsId && x.RatedByUserId == userId);
            if (existingRate == null)
            {
                await _repo.Create<NtsRatingViewModel, NtsRating>(new NtsRatingViewModel
                {
                    NtsType = ntsType,
                    NtsId = ntsId,
                    Rating = rating,
                    RatingComment = ratingComment,
                    RatedByUserId = userId
                });
            }
            else
            {
                existingRate.Rating = rating;
                existingRate.RatingComment = ratingComment;
                await _repo.Edit<NtsRatingViewModel, NtsRating>(existingRate);
            }
        }
        public async Task RemoveRating(NtsTypeEnum ntsType, string ntsId, string userId)
        {
            var existingRate = await _repo.GetSingle<NtsRatingViewModel, NtsRating>(x => x.NtsType == ntsType && x.NtsId == ntsId && x.RatedByUserId == userId);
            if (existingRate != null)
            {
                await _repo.Delete<NtsRatingViewModel, NtsRating>(existingRate.Id);
            }
        }
        public async Task DisbaleGrievenceReopenService(DateTime dateTime)
        {
            try
            {
                var serviceList = await _ntsQueryBusiness.DisbaleGrievenceReopenServiceData(dateTime);
                foreach (var item in serviceList)
                {
                    await _ntsQueryBusiness.DisbaleGrievenceReopenServiceData1();
                }
            }
            catch (Exception)
            {

            }

        }
        public async Task CancelCommunityHallBookingOnExpired(DateTime dateTime)
        {
            try
            {


                var taskList = await _ntsQueryBusiness.CancelCommunityHallBookingOnExpiredData(dateTime);
                foreach (var item in taskList)
                {
                    try
                    {
                        var svm = new TaskTemplateViewModel
                        {
                            TaskId = item.Id,
                            DataAction = DataActionEnum.Edit,
                            ActiveUserId = _repo.UserContext.UserId
                        };
                        var task = await _taskBusiness.GetTaskDetails(svm);
                        if (task != null && task.TaskId != null)
                        {
                            task.TaskStatusCode = "TASK_STATUS_REJECT";
                            task.DataAction = DataActionEnum.Edit;
                            task.ActiveUserId = svm.OwnerUserId;
                            task.RejectionReason = "Completion time limit for the task exceeded the limit of 2 days. So task is automatically rejected by the system.";
                            await _taskBusiness.ManageTask(task);
                        }
                    }
                    catch (Exception)
                    {

                    }
                }
            }
            catch (Exception)
            {
            }
        }
        public async Task SendNotificationForRentServices()
        {
            try
            {


                var _cmsBusiness = _serviceProvider.GetService<ICmsBusiness>();
                var _noteBusiness = _serviceProvider.GetService<INoteBusiness>();
                var _tableMetadataBusiness = _serviceProvider.GetService<ITableMetadataBusiness>();
                var list = await _cmsBusiness.GetDataListByTemplate("SN_NEW_RENTAL_PROPERTY", "");
                foreach (DataRow data in list.Rows)
                {
                    var udftabledata = await _tableMetadataBusiness.GetTableDataByColumn("RENTAL_PROPERTY", "", "Id", data["PropertyId"].ToString());
                    var service = await _serviceBusiness.GetSingle(x => x.UdfNoteId == data["NtsNoteId"].ToString());
                    var user = await _userBusiness.GetSingleById(service.OwnerUserId);
                    var Enddate = Convert.ToDateTime(data["EndDate"].ToString()).AddDays(-30);
                    if (data["EndDate"].ToString().IsNotNullAndNotEmpty())
                    {
                        if ((Convert.ToDateTime(data["EndDate"].ToString()).AddDays(-60)).Date > DateTime.Now.Date && (Convert.ToDateTime(data["EndDate"].ToString()).AddDays(-90)).Date <= DateTime.Now.Date)
                        {
                            if (data["Is90DaysNotificationSent"].ToString() != "true")
                            {
                                // Send 90 days Notification
                                var _notificationBusiness = _serviceProvider.GetService<INotificationBusiness>();
                                var notificationTemplateModel = await _repo.GetSingleGlobal<NotificationTemplateViewModel, NotificationTemplate>(x => x.Code == "SMARTCITY_RENT_90DAYS");
                                if (notificationTemplateModel.IsNotNull())
                                {
                                    if (udftabledata.IsNotNull())
                                    {
                                        notificationTemplateModel.Body = notificationTemplateModel.Body.Replace("{{PropertyNo}}", udftabledata["PropertyName"].ToString());
                                    }
                                    notificationTemplateModel.Body = notificationTemplateModel.Body.Replace("{{UserName}}", user.Name);
                                    notificationTemplateModel.Body = notificationTemplateModel.Body.Replace("{{EndDate}}", data["EndDate"].ToString());
                                    var viewModel = new NotificationViewModel()
                                    {
                                        To = user.Email,
                                        ToUserId = user.Id,
                                        //FromUserId = model.CreatedBy,
                                        Subject = notificationTemplateModel.Subject,
                                        Body = notificationTemplateModel.Body,
                                        SendAlways = true,
                                        NotifyByEmail = true,
                                        DynamicObject = data
                                    };

                                    await _notificationBusiness.Create(viewModel);

                                }
                                await UpdateRentService(data["NtsNoteId"].ToString(), "Is90DaysNotificationSent");
                            }
                        }
                        else if ((Convert.ToDateTime(data["EndDate"].ToString()).AddDays(-30)).Date > DateTime.Now.Date && (Convert.ToDateTime(data["EndDate"].ToString()).AddDays(-60)).Date <= DateTime.Now.Date)
                        {
                            if (data["Is60DaysNotificationSent"].ToString() != "true")
                            {
                                // Send 60 days Notification
                                var _notificationBusiness = _serviceProvider.GetService<INotificationBusiness>();
                                var notificationTemplateModel = await _repo.GetSingleGlobal<NotificationTemplateViewModel, NotificationTemplate>(x => x.Code == "SMARTCITY_RENT_60DAYS");
                                if (notificationTemplateModel.IsNotNull())
                                {
                                    if (udftabledata.IsNotNull())
                                    {
                                        notificationTemplateModel.Body = notificationTemplateModel.Body.Replace("{{PropertyNo}}", udftabledata["PropertyName"].ToString());
                                    }
                                    notificationTemplateModel.Body = notificationTemplateModel.Body.Replace("{{UserName}}", user.Name);
                                    notificationTemplateModel.Body = notificationTemplateModel.Body.Replace("{{EndDate}}", data["EndDate"].ToString());
                                    var viewModel = new NotificationViewModel()
                                    {
                                        To = user.Email,
                                        ToUserId = user.Id,
                                        //FromUserId = model.CreatedBy,
                                        Subject = notificationTemplateModel.Subject,
                                        Body = notificationTemplateModel.Body,
                                        SendAlways = true,
                                        NotifyByEmail = true,
                                        DynamicObject = data
                                    };

                                    await _notificationBusiness.Create(viewModel);
                                }
                                await UpdateRentService(data["NtsNoteId"].ToString(), "Is60DaysNotificationSent");
                            }
                        }
                        else if ((Convert.ToDateTime(data["EndDate"].ToString()).AddDays(-30)).Date <= DateTime.Now.Date)
                        {
                            if (data["Is30DaysNotificationSent"].ToString() != "true")
                            {
                                // Send 30 days Notification
                                var _notificationBusiness = _serviceProvider.GetService<INotificationBusiness>();
                                var notificationTemplateModel = await _repo.GetSingleGlobal<NotificationTemplateViewModel, NotificationTemplate>(x => x.Code == "SMARTCITY_RENT_30DAYS");
                                if (notificationTemplateModel.IsNotNull())
                                {
                                    if (udftabledata.IsNotNull())
                                    {
                                        notificationTemplateModel.Body = notificationTemplateModel.Body.Replace("{{PropertyNo}}", udftabledata["PropertyName"].ToString());
                                    }
                                    notificationTemplateModel.Body = notificationTemplateModel.Body.Replace("{{UserName}}", user.Name);
                                    notificationTemplateModel.Body = notificationTemplateModel.Body.Replace("{{EndDate}}", data["EndDate"].ToString());
                                    var viewModel = new NotificationViewModel()
                                    {
                                        To = user.Email,
                                        ToUserId = user.Id,
                                        //FromUserId = model.CreatedBy,
                                        Subject = notificationTemplateModel.Subject,
                                        Body = notificationTemplateModel.Body,
                                        SendAlways = true,
                                        NotifyByEmail = true,
                                        DynamicObject = data
                                    };

                                    await _notificationBusiness.Create(viewModel);
                                }
                                await UpdateRentService(data["NtsNoteId"].ToString(), "Is30DaysNotificationSent");
                            }
                        }
                    }

                }
            }
            catch (Exception)
            {
            }
        }

        public async Task UpdateRentService(string noteId, string ColumnName)
        {
            var query = $@"update cms.""N_SNC_RENT_MANAGEMENT_NewRentalProperty"" set ""{ColumnName}""=True, ""LastUpdatedDate""='{DateTime.Now}',""LastUpdatedBy""='{_repo.UserContext.UserId}' where ""NtsNoteId""='{noteId}'";
            await _repositoryQueryBase.ExecuteCommand(query, null);
            await _ntsQueryBusiness.UpdateRentServiceData(noteId, ColumnName);
        }

        public async Task UpdateRentalStatusForVacating()
        {
            try
            {


                var _cmsBusiness = _serviceProvider.GetService<ICmsBusiness>();
                var _serviceBusiness = _serviceProvider.GetService<IServiceBusiness>();
                var list = await _cmsBusiness.GetDataListByTemplate("SN_VACATING_RENTAL_PROPERTY", "");

                foreach (DataRow data in list.Rows)
                {
                    var servicedetails = await _serviceBusiness.GetSingle(x => x.UdfNoteTableId == data["Id"].ToString());
                    var servicestatus = await _lOVBusiness.GetSingleById(servicedetails.ServiceStatusId);

                    if (servicestatus.Code == "SERVICE_STATUS_COMPLETE")
                    {
                        if ((Convert.ToDateTime(data["ContractCancellationDate"].ToString())).Date == DateTime.Now.Date.AddDays(-1))
                        {
                            var reason = data["ReasonForVacating"].ToString();
                            var rentalstatus = await _lOVBusiness.GetSingle(x => x.Code == "RENTAL_STATUS_VACATED");
                            var rentalAgreementNumber = data["RentalAgreementNumber"].ToString();


                            await _ntsQueryBusiness.UpdateRentalStatusForVacatingData(rentalstatus.Id, rentalAgreementNumber, reason);



                        }
                    }
                }
            }
            catch (Exception)
            {

            }
        }
    }
}
