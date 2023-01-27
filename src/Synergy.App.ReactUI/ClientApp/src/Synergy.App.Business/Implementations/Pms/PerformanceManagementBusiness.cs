using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using Hangfire;
using Humanizer;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Synergy.App.Business
{
    public class PerformanceManagementBusiness : BusinessBase<ServiceViewModel, NtsService>, IPerformanceManagementBusiness
    {
        private readonly IRepositoryQueryBase<ServiceViewModel> _queryRepo;
        private readonly IRepositoryQueryBase<StageViewModel> _queryStageRepo;
        private readonly IRepositoryQueryBase<IdNameViewModel> _queryRepo1;
        private readonly IRepositoryQueryBase<ProgramDashboardViewModel> _queryPDRepo;
        private readonly IRepositoryQueryBase<ProjectGanttTaskViewModel> _queryGantt;
        private readonly IRepositoryQueryBase<TeamWorkloadViewModel> _queryTWRepo;
        private readonly IRepositoryQueryBase<DashboardCalendarViewModel> _queryDCRepo;
        private readonly IRepositoryQueryBase<PerformanceDashboardViewModel> _queryProjDashRepo;
        private readonly IRepositoryQueryBase<ProjectDashboardChartViewModel> _queryProjDashChartRepo;
        private readonly IRepositoryQueryBase<TaskViewModel> _queryTaskRepo;
        private readonly IRepositoryQueryBase<MailViewModel> _queryMailTaskRepo;
        private readonly IRepositoryQueryBase<PerformanceDocumentViewModel> _queryPerDoc;
        private readonly IRepositoryQueryBase<PerformanceDocumentStageViewModel> _queryPerDocStage;
        private readonly IRepositoryQueryBase<GoalViewModel> _queryGoal;
        private readonly IRepositoryQueryBase<NoteTemplateViewModel> _queryNoteTemplate;
        private readonly ITaskBusiness _taskBusiness;
        private readonly IServiceBusiness _serviceBusiness;
        private readonly INoteBusiness _noteBusiness;
        private readonly ITableMetadataBusiness _tableMetadataBusiness;
        private readonly IUserContext _userContext;
        private readonly INtsTaskPrecedenceBusiness _ntsTaskPrecedenceBusiness;
        private readonly IHRCoreBusiness _hrCoreBusiness;
        private readonly IComponentResultBusiness _componentResultBusiness;
        private readonly ILOVBusiness _lovBusiness;
        private readonly ITemplateBusiness _templateBusiness;
        private readonly IStepTaskComponentBusiness _stepCompBusiness;
        private readonly IRepositoryQueryBase<PerformaceRatingViewModel> _queryPerformanceRatingRepo;
        private readonly IRepositoryQueryBase<PerformanceRatingItemViewModel> _queryPerformanceRatingitemRepo;
        private readonly ICmsBusiness _cmsBusiness;
        private readonly IRepositoryQueryBase<CompetencyViewModel> _queryComp;
        private readonly IUserHierarchyBusiness _userHierBusiness;
        private readonly IPerformanceManagementQueryBusiness _performanceManagementQueryBusiness;

        private readonly IRepositoryQueryBase<CompetencyCategoryViewModel> _queryCompeencyCategory;
        private readonly IServiceProvider _serviceProvider;
        //private readonly IHangfireScheduler _hangfireScheduler;
        public PerformanceManagementBusiness(IRepositoryBase<ServiceViewModel, NtsService> repo, IRepositoryQueryBase<ServiceViewModel> queryRepo,
            IRepositoryQueryBase<ProgramDashboardViewModel> queryPDRepo,
            IRepositoryQueryBase<StageViewModel> queryStageRepo,
            IRepositoryQueryBase<IdNameViewModel> queryRepo1,
            IRepositoryQueryBase<DashboardCalendarViewModel> queryDCRepo,
            IRepositoryQueryBase<ProjectGanttTaskViewModel> queryGantt,
             IRepositoryQueryBase<TeamWorkloadViewModel> queryTWRepo,
             IRepositoryQueryBase<PerformanceDashboardViewModel> queryProjDashRepo,
             IRepositoryQueryBase<ProjectDashboardChartViewModel> queryProjDashChartRepo,
             IPerformanceManagementQueryBusiness performanceManagementQueryBusiness,
             IRepositoryQueryBase<TaskViewModel> queryTaskRepo, INtsTaskPrecedenceBusiness ntsTaskPrecedenceBusiness, ITableMetadataBusiness tableMetadataBusiness,
            IMapper autoMapper, ITaskBusiness taskBusiness, INoteBusiness noteBusiness, IServiceBusiness serviceBusiness, IRepositoryQueryBase<MailViewModel> queryMailTaskRepo,
            IRepositoryQueryBase<PerformanceDocumentViewModel> queryPerDoc, IRepositoryQueryBase<GoalViewModel> queryGoal, IRepositoryQueryBase<PerformanceDocumentStageViewModel> queryPerDocStage
            , IHRCoreBusiness hrCoreBusiness, IUserContext userContext, IRepositoryQueryBase<NoteTemplateViewModel> queryNoteTemplate, IComponentResultBusiness componentResultBusiness, ILOVBusiness lovBusiness
            , ITemplateBusiness templateBusiness, IStepTaskComponentBusiness stepCompBusiness, IRepositoryQueryBase<PerformaceRatingViewModel> queryPerformaceRating,
            IRepositoryQueryBase<PerformanceRatingItemViewModel> queryPerformaceRatingitem, IRepositoryQueryBase<CompetencyViewModel> queryComp, ICmsBusiness cmsBusiness, IRepositoryQueryBase<CompetencyCategoryViewModel> queryComptetencyCategory
            , IUserHierarchyBusiness userHierBusiness
            , IServiceProvider serviceProvider
            //, IHangfireScheduler hangfireScheduler
            ) : base(repo, autoMapper)
        {
            _queryStageRepo = queryStageRepo;
            _queryRepo = queryRepo;
            _queryRepo1 = queryRepo1;
            _queryPDRepo = queryPDRepo;
            _queryDCRepo = queryDCRepo;
            _queryGantt = queryGantt;
            _queryTWRepo = queryTWRepo;
            _queryProjDashRepo = queryProjDashRepo;
            _queryProjDashChartRepo = queryProjDashChartRepo;
            _queryTaskRepo = queryTaskRepo;
            _taskBusiness = taskBusiness;
            _serviceBusiness = serviceBusiness;
            _noteBusiness = noteBusiness;
            _tableMetadataBusiness = tableMetadataBusiness;
            _ntsTaskPrecedenceBusiness = ntsTaskPrecedenceBusiness;
            _queryMailTaskRepo = queryMailTaskRepo;
            _queryPerDoc = queryPerDoc;
            _queryPerDocStage = queryPerDocStage;
            _hrCoreBusiness = hrCoreBusiness;
            _componentResultBusiness = componentResultBusiness;
            _queryGoal = queryGoal;
            _userContext = userContext;
            _queryNoteTemplate = queryNoteTemplate;
            _lovBusiness = lovBusiness;
            _templateBusiness = templateBusiness;
            _stepCompBusiness = stepCompBusiness;
            _queryComp = queryComp;
            _queryPerformanceRatingRepo = queryPerformaceRating;
            _queryPerformanceRatingitemRepo = queryPerformaceRatingitem;
            _cmsBusiness = cmsBusiness;
            _queryCompeencyCategory = queryComptetencyCategory;
            _userHierBusiness = userHierBusiness;
            _performanceManagementQueryBusiness = performanceManagementQueryBusiness;
            _serviceProvider = serviceProvider;
            //_hangfireScheduler = hangfireScheduler;

        }
        public async override Task<CommandResult<ServiceViewModel>> Create(ServiceViewModel model, bool autoCommit = true)
        {

            return CommandResult<ServiceViewModel>.Instance();
        }
        public async override Task<CommandResult<ServiceViewModel>> Edit(ServiceViewModel model, bool autoCommit = true)
        {
            return CommandResult<ServiceViewModel>.Instance();
        }
        private async Task<CommandResult<ServiceViewModel>> IsNameExists(ServiceViewModel model)
        {
            return CommandResult<ServiceViewModel>.Instance();
        }

        public async Task<IList<ProgramDashboardViewModel>> GetPerformanceData(string userId)
        {

            var queryData = await _performanceManagementQueryBusiness.GetPerformanceData(userId);

            foreach (var st in queryData)
            {
                var stages = await GetPerformanceStageData(userId, st.Id);
                st.StageList = new List<StageViewModel>();
                st.StageList.AddRange(stages);
            }

            return queryData;
        }

        public async Task<IList<StageViewModel>> GetPerformanceStageData(string userId, string performanceDocumentId)
        {

            var queryData = await _performanceManagementQueryBusiness.GetPerformanceStageData(userId, performanceDocumentId);
            foreach (var st in queryData)
            {
                var goalsComptency = await GetGoalandCompetencyCountByPerformanceAndStageId(performanceDocumentId, st.StageId, userId);
                if (goalsComptency.IsNotNull())
                {
                    st.Goals = goalsComptency.Where(x => x.TemplateCode == "PMS_GOAL").ToList().Count;
                    st.Competency = goalsComptency.Where(x => x.TemplateCode == "PMS_COMPENTENCY").ToList().Count;
                }
            }

            return queryData;
        }

        public async Task<IList<TeamWorkloadViewModel>> GetGoalandCompetencyCountByPerformanceAndStageId(string performanceId, string stageId, string userId)
        {
            var queryData = await _performanceManagementQueryBusiness.GetGoalandCompetencyCountByPerformanceAndStageId(performanceId, stageId, userId);
            return queryData;
        }



        public async Task<IList<ProgramDashboardViewModel>> GetPerformanceSharedData()
        {

            var queryData = await _performanceManagementQueryBusiness.GetPerformanceSharedData();

            return queryData;
        }


        public async Task<IList<ProjectGanttTaskViewModel>> ReadWBSTimelineGanttChartData(string userId, string performanceId, string userRole, List<string> projectIds = null, List<string> ownerIds = null, List<string> assigneeIds = null, List<string> tasksStatus = null, FilterColumnEnum? column = null, DateTime? startDate = null, DateTime? dueDate = null)
        {

            var queryData = await _performanceManagementQueryBusiness.ReadWBSTimelineGanttChartData(userId, performanceId, userRole, projectIds, ownerIds, assigneeIds, tasksStatus, column, startDate, dueDate);
            return queryData;
        }
        public async Task<IList<TreeViewViewModel>> GetInboxMenuItem(string id, string type, string parentId, string userRoleId, string userId, string userRoleIds, string stageName, string stageId, string projectId, string expandingList, string userroleCode)
        {
            var expObj = new List<TreeViewViewModel>();
            if (expandingList != null)
            {
                expObj = JsonConvert.DeserializeObject<List<TreeViewViewModel>>(expandingList);
                var obj = expObj.Where(x => x.id == id).FirstOrDefault();
                if (obj.IsNotNull())
                {
                    type = obj.Type;
                    parentId = obj.ParentId;
                    userRoleId = obj.UserRoleId;
                    projectId = obj.ProjectId;
                    stageId = obj.StageId;
                }
            }

            var list = new List<TreeViewViewModel>();
            if (id.IsNullOrEmpty())
            {
                var roles = userRoleIds.Split(",");
                var roleText = "";
                foreach (var i in roles)
                {
                    roleText += $"'{i}',";
                }
                roleText = roleText.Trim(',');


                var count = await _performanceManagementQueryBusiness.GetInboxMenuItemData(roleText, userId);


                var item = new TreeViewViewModel
                {
                    id = "INBOX",
                    Name = "Inbox",
                    DisplayName = "Inbox",
                    ParentId = null,
                    hasChildren = true,
                    expanded = true,
                    Type = "INBOX",
                };
                if (count != null)
                {
                    item.Name = item.DisplayName = $"Inbox ({count })";
                }
                list.Add(item);

            }
            else if (id == "INBOX")
            {
                var roles = userRoleIds.Split(",");
                var roleText = "";
                foreach (var item in roles)
                {
                    roleText += $"'{item}',";
                }
                roleText = roleText.Trim(',');

                list = await _performanceManagementQueryBusiness.GetInboxMenuItemIndex(roleText, userId);

                //expanded -> type= userrole - from coming list find type as userRole
                // if found then find the item in list as selcted item id
                //make expanded true

                var obj = expObj.Where(x => x.Type == "USERROLE").FirstOrDefault();
                if (obj.IsNotNull())
                {
                    var data = list.Where(x => x.id == obj.UserRoleId).FirstOrDefault();
                    if (data.IsNotNull())
                    {
                        data.expanded = true;
                    }
                }

            }
            else if (type == "USERROLE")
            {

                list = await _performanceManagementQueryBusiness.GetInboxMenuItemUserRole(id, userRoleId, userId);



                var obj = expObj.Where(x => x.Type == "PROJECTSTAGE").FirstOrDefault();
                if (obj.IsNotNull())
                {
                    var data = list.Where(x => x.id == obj.id).FirstOrDefault();
                    if (data.IsNotNull())
                    {
                        data.expanded = true;
                    }
                }

            }
            else if (type == "PROJECTSTAGE")
            {

                list = await _performanceManagementQueryBusiness.GetInboxMenuItemProjectStage(id, userRoleId, userId);


                var obj = expObj.Where(x => x.Type == "PROJECT").FirstOrDefault();
                if (obj.IsNotNull())
                {
                    var data = list.Where(x => x.id == obj.id).FirstOrDefault();
                    if (data.IsNotNull())
                    {
                        data.expanded = true;
                    }
                }

            }
            else if (type == "PROJECT")
            {



                list = await _performanceManagementQueryBusiness.GetInboxMenuItemProject(id, userId);


                var obj = expObj.Where(x => x.Type == "STAGE").FirstOrDefault();
                if (obj.IsNotNull())
                {
                    var data = list.Where(x => x.id == obj.id).FirstOrDefault();
                    if (data.IsNotNull())
                    {
                        data.expanded = true;
                    }
                }


            }

            else
            {

            }

            foreach (var item in list)
            {

                item.children = true;
                item.text = item.Name;
                item.parent = item.ParentId == null ? "#" : item.ParentId;
                item.a_attr = new { data_id = item.id, data_name = item.Name, data_type = item.Type };
            }

            return list;
        }
        public async Task<IList<TreeViewViewModel>> GetInboxMenuItemByUser(string id, string type, string parentId, string userRoleId, string userId, string userRoleIds, string stageName, string stageId, string performanceId, string expandingList, string userroleCode)
        {
            var expObj = new List<TreeViewViewModel>();
            if (expandingList != null)
            {
                expObj = JsonConvert.DeserializeObject<List<TreeViewViewModel>>(expandingList);
                var obj = expObj.Where(x => x.id == id).FirstOrDefault();
                if (obj.IsNotNull())
                {
                    type = obj.Type;
                    parentId = obj.ParentId;
                    userRoleId = obj.UserRoleId;
                    performanceId = obj.PerformanceId;
                    stageId = obj.StageId;
                }
            }

            var list = new List<TreeViewViewModel>();

            if (id.IsNullOrEmpty())
            {
                var roles = userRoleIds.Split(",");
                var roleText = "";
                foreach (var i in roles)
                {
                    roleText += $"'{i}',";
                }
                roleText = roleText.Trim(',');

                var count = await _performanceManagementQueryBusiness.GetInboxMenuItemByUserData(roleText, userId);

                var item = new TreeViewViewModel
                {
                    id = "INBOX",
                    Name = "Inbox",
                    DisplayName = "Inbox",
                    ParentId = null,
                    hasChildren = true,
                    expanded = true,
                    Type = "INBOX"
                };
                if (count != null)
                {
                    item.Name = item.DisplayName = $"Inbox ({count })";
                }
                list.Add(item);


                var item1 = new TreeViewViewModel
                {
                    id = "GENERAL",
                    Name = "General Task",
                    DisplayName = "General Task",
                    ParentId = null,
                    hasChildren = true,
                    expanded = true,
                    Type = "GENERAL"
                };
                if (count != null)
                {
                    item1.Name = item1.DisplayName = $"General Task";
                }
                list.Add(item1);

            }
            else if (id == "GENERAL")
            {


                var rcount = await _performanceManagementQueryBusiness.GetInboxMenuItemGeneral(userId);

                var scount = await _performanceManagementQueryBusiness.GetInboxMenuItemGeneralCount(userId);

                var item1 = new TreeViewViewModel
                {
                    id = "RECEIVED",
                    Name = "Received",
                    DisplayName = "Received",
                    ParentId = "GENERAL",
                    hasChildren = true,
                    expanded = true,
                    Type = "RECEIVED"
                };
                if (rcount != null)
                {
                    item1.Name = item1.DisplayName = $"Received ({rcount })";
                }

                list.Add(item1);

                var item2 = new TreeViewViewModel
                {
                    id = "SENT",
                    Name = "Sent",
                    DisplayName = "Sent",
                    ParentId = "GENERAL",
                    hasChildren = true,
                    expanded = true,
                    Type = "SENT"
                };
                if (scount != null)
                {
                    item2.Name = item2.DisplayName = $"Sent ({scount })";
                }

                list.Add(item2);
            }
            else if (id == "INBOX")
            {
                var roles = userRoleIds.Split(",");
                var roleText = "";
                foreach (var item in roles)
                {
                    roleText += $"'{item}',";
                }
                roleText = roleText.Trim(',');

                list = await _performanceManagementQueryBusiness.GetInboxMenuItemGeneralInbox(roleText, userId);
                //expanded -> type= userrole - from coming list find type as userRole
                // if found then find the item in list as selcted item id
                //make expanded true

                var obj = expObj.Where(x => x.Type == "USERROLE").FirstOrDefault();
                if (obj.IsNotNull())
                {
                    var data = list.Where(x => x.id == obj.UserRoleId).FirstOrDefault();
                    if (data.IsNotNull())
                    {
                        data.expanded = true;
                    }
                }

            }

            else if (type == "USERROLE")
            {


                list = await _performanceManagementQueryBusiness.GetInboxMenuItemGeneralUserRole(id, userRoleId, userId);



                var obj = expObj.Where(x => x.Type == "PERFORMANCE").FirstOrDefault();
                if (obj.IsNotNull())
                {
                    var data = list.Where(x => x.id == obj.id).FirstOrDefault();
                    if (data.IsNotNull())
                    {
                        data.expanded = true;
                    }
                }

            }

            else if (type == "PERFORMANCE")
            {
                list = await _performanceManagementQueryBusiness.GetInboxMenuItemGeneralPerformance(id, userRoleId, userId, stageId);


                var obj = expObj.Where(x => x.Type == "STAGETYPE").FirstOrDefault();
                if (obj.IsNotNull())
                {
                    var data = list.Where(x => x.id == obj.id).FirstOrDefault();
                    if (data.IsNotNull())
                    {
                        data.expanded = true;
                    }
                }


            }
            else if (type == "STAGETYPE")
            {

                list = await _performanceManagementQueryBusiness.GetInboxMenuItemGeneralStageType(id, userRoleId, userId, performanceId);


                var obj = expObj.Where(x => x.Type == "STAGE").FirstOrDefault();
                if (obj.IsNotNull())
                {
                    var data = list.Where(x => x.id == obj.id).FirstOrDefault();
                    if (data.IsNotNull())
                    {
                        data.expanded = true;
                    }
                }


            }
            else if (type == "STAGE")
            {


                list = await _performanceManagementQueryBusiness.GetInboxMenuItemGeneralStage(id, userId, stageId, performanceId);


                var obj = expObj.Where(x => x.Type == "STATUS").FirstOrDefault();
                if (obj.IsNotNull())
                {
                    var data = list.Where(x => x.id == obj.id).FirstOrDefault();
                    if (data.IsNotNull())
                    {
                        data.expanded = true;
                    }
                }


            }
            else if (type == "STATUS")
            {

                list = await _performanceManagementQueryBusiness.GetInboxMenuItemGeneralStatus(id, stageId);


                var obj = expObj.Where(x => x.Type == "TASK").FirstOrDefault();
                if (obj.IsNotNull())
                {
                    var data = list.Where(x => x.id == obj.id).FirstOrDefault();
                    if (data.IsNotNull())
                    {
                        data.expanded = true;
                    }
                }


            }
            foreach (var item in list)
            {

                item.children = true;
                item.text = item.Name;
                item.parent = item.ParentId == null ? "#" : item.ParentId;
                item.a_attr = new { data_id = item.id, data_name = item.Name, data_type = item.Type };
            }
            return list;
        }


        public async Task<IList<ProjectGanttTaskViewModel>> ReadPerformanceDashboardData(string userId, string performanceId, string userRole, string projectIds = null, string ownerIds = null, string assigneeIds = null, string tasksStatus = null, FilterColumnEnum? column = null, DateTime? startDate = null, DateTime? dueDate = null, string type = null, string stageId = null)
        {

            var queryData = await _performanceManagementQueryBusiness.ReadPerformanceDashboardData(userId, performanceId, userRole, projectIds, ownerIds, assigneeIds, tasksStatus, column, startDate, dueDate, type, stageId);

            return queryData;

        }
        public async Task<IList<ProjectGanttTaskViewModel>> ReadPerformanceDashboardTaskData(string userId, string performanceId, string userRole, string projectIds = null, string ownerIds = null, string assigneeIds = null, string tasksStatus = null, FilterColumnEnum? column = null, DateTime? startDate = null, DateTime? dueDate = null, string type = null, string stageId = null)
        {
            var queryData = await _performanceManagementQueryBusiness.ReadPerformanceDashboardTaskData(userId, performanceId, userRole, projectIds, ownerIds, assigneeIds, tasksStatus, column, startDate, dueDate, type, stageId);
            return queryData;

        }
        public async Task<IList<ProjectGanttTaskViewModel>> ReadMindMapData(string projectId)
        {
            var queryData = await _performanceManagementQueryBusiness.ReadMindMapData(projectId);
            return queryData;
        }
        public async Task<IList<ProjectGanttTaskViewModel>> ReadPerformanceTaskGridViewData(string userId, string performanceId, string objectiveId, string userRole, List<string> projectIds = null, List<string> ownerIds = null, List<string> assigneeIds = null, List<string> tasksStatus = null, FilterColumnEnum? column = null, DateTime? startDate = null, DateTime? dueDate = null, List<string> type = null, InboxTypeEnum? inboxType = null)
        {

            var queryData = await _performanceManagementQueryBusiness.ReadPerformanceTaskGridViewData(userId, performanceId, objectiveId, userRole, projectIds, ownerIds, assigneeIds, tasksStatus, column, startDate, dueDate, type, inboxType);
            if (performanceId.IsNotNull() && projectIds.IsNotNull() && projectIds.Count > 0)
            {
                queryData = queryData.Where(x => x.ParentId == projectIds[0]).ToList();
            }
            return queryData;

        }
        public async Task<IList<ProjectGanttTaskViewModel>> ReadPerformanceUserWorkloadGridViewData(string userIds, List<string> projectIds = null, List<string> ownerIds = null, List<string> assigneeIds = null, List<string> tasksStatus = null, FilterColumnEnum? column = null, DateTime? startDate = null, DateTime? dueDate = null)
        {

            var queryData = await _performanceManagementQueryBusiness.ReadPerformanceUserWorkloadGridViewData(userIds, projectIds, ownerIds, assigneeIds, tasksStatus, column, startDate, dueDate);

            return queryData;

        }
        public async Task<IList<ProjectGanttTaskViewModel>> ReadPerformanceTaskUserGridViewData(string userId, List<string> projectIds = null, List<string> ownerIds = null, List<string> assigneeIds = null, List<string> tasksStatus = null, FilterColumnEnum? column = null, DateTime? startDate = null, DateTime? dueDate = null, List<string> pmsTypes = null, List<string> stageIds = null)
        {

            var queryData = await _performanceManagementQueryBusiness.ReadPerformanceTaskUserGridViewData(userId, projectIds, ownerIds, assigneeIds, tasksStatus, column, startDate, dueDate, pmsTypes, stageIds);

            return queryData;

        }
        public async Task<IList<IdNameViewModel>> GetSubordinatesIdNameList()
        {
            var list = new List<IdNameViewModel>();
            //var position = await _hrCoreBusiness.GetPostionHierarchyParentId(null);
            //if (position != null && position.Id.IsNotNullAndNotEmpty())
            //{
            //    var positionchildList = await _hrCoreBusiness.GetPositionHierarchy(position.Id, 1);
            //    if (positionchildList != null && positionchildList.Count() > 0)
            //    {
            //        //list = positionchildList.ConvertAll(x => new IdNameViewModel
            //        //{
            //        //    Id = x.UserId,
            //        //    Name = x.DisplayName
            //        //}).Distinct().Where(x => x.Id.IsNotNullAndNotEmpty()).ToList();

            //        //list = positionchildList.Select(x => new IdNameViewModel
            //        //{
            //        //    Id = x.UserId,
            //        //    Name = x.DisplayName
            //        //}).Where(x => x.Id.IsNotNullAndNotEmpty()).Distinct().ToList();

            //        var list1 = positionchildList.GroupBy(x => new
            //        {
            //            Id = x.UserId,
            //            Name = x.DisplayName
            //        }).Select(g => g.FirstOrDefault()).Where(g => g.UserId.IsNotNullAndNotEmpty()).ToList();

            //        if (list1 != null && list1.Count > 0)
            //        {
            //            list = list1.Select(x => new IdNameViewModel
            //            {
            //                Id = x.UserId,
            //                Name = x.DisplayName
            //            }).ToList();
            //        }
            //    }
            //}
            var userlist = await GetUserHierarchy();
            list = userlist.Select(x => new IdNameViewModel { Id = x.Id, Name = x.Name }).ToList();
            return list;
        }

        public async Task<List<UserViewModel>> GetUserHierarchy()
        {

            var queryData = await _performanceManagementQueryBusiness.GetUserHierarchy();
            var list = queryData;
            return list;
        }

        public async Task<IList<ProjectGanttTaskViewModel>> ReadPerformanceObjectivesGridViewData(string userId, List<string> projectIds = null, List<string> ownerIds = null, List<string> assigneeIds = null, List<string> tasksStatus = null, FilterColumnEnum? column = null, DateTime? startDate = null, DateTime? dueDate = null, List<string> pmsTypes = null, List<string> stageIds = null, string statusCodes = null)
        {

            var queryData = await _performanceManagementQueryBusiness.ReadPerformanceObjectivesGridViewData(userId, projectIds, ownerIds, assigneeIds, tasksStatus, column, startDate, dueDate, pmsTypes, stageIds, statusCodes);

            if (projectIds.IsNotNull() && projectIds.Count > 0)
            {
                //queryData = queryData.Where(x=>x.s)
            }
            if (pmsTypes.IsNotNull() && pmsTypes.Count > 0)
            {
                queryData = queryData.Where(x => pmsTypes.Contains(x.Type)).ToList();
            }
            if (stageIds.IsNotNull() && stageIds.Count > 0)
            {
                queryData = queryData.Where(x => stageIds.Contains(x.ServiceId)).ToList();
            }
            if (ownerIds.IsNotNull() && ownerIds.Count > 0)
            {
                queryData = queryData.Where(x => ownerIds.Contains(x.OwnerUserId)).ToList();
            }
            if (assigneeIds.IsNotNull() && assigneeIds.Count > 0)
            {
                queryData = queryData.Where(x => assigneeIds.Contains(x.AssigneeUserId)).ToList();
            }
            if (tasksStatus.IsNotNull() && tasksStatus.Count > 0)
            {
                queryData = queryData.Where(x => tasksStatus.Contains(x.TaskStatusId)).ToList();
            }
            if (column.IsNotNull() && startDate.IsNotNull() && dueDate.IsNotNull())
            {
                if (column == FilterColumnEnum.DueDate)
                {
                    queryData = queryData.Where(x => startDate <= x.End && x.End < dueDate).ToList();
                }
                else
                {
                    queryData = queryData.Where(x => startDate <= x.Start && x.Start < dueDate).ToList();
                }
            }


            return queryData;

        }

        public async Task<IList<ProjectGanttTaskViewModel>> ReadEmployeePerformanceObjectivesData(string userId, List<string> projectIds = null, List<string> ownerIds = null, List<string> assigneeIds = null, List<string> tasksStatus = null, FilterColumnEnum? column = null, DateTime? startDate = null, DateTime? dueDate = null, List<string> pmsTypes = null, List<string> stageIds = null, string statusCodes = null)
        {

            var queryData = await _performanceManagementQueryBusiness.ReadEmployeePerformanceObjectivesData(userId, projectIds, ownerIds, assigneeIds, tasksStatus, column, startDate, dueDate, pmsTypes, stageIds, statusCodes);

            return queryData;

        }
        public async Task<IList<IdNameViewModel>> GetPerformanceSharedList(string userId)
        {
            var queryData = await _performanceManagementQueryBusiness.GetPerformanceSharedList(userId);

            return queryData;
        }
        public async Task<PerformanceDashboardViewModel> GetPerformanceDashboardDetails(string projectId, string stageId = null)
        {

            var queryData = await _performanceManagementQueryBusiness.GetPerformanceDashboardDetails(projectId, stageId);
            if (queryData != null)
            {
                queryData.TemplateUserType = queryData.UserId.IsNotNullAndNotEmpty() ? NtsUserTypeEnum.Owner : NtsUserTypeEnum.Shared;
            }

            var queryData1 = await _performanceManagementQueryBusiness.GetPerformanceDashboardDetailsData(projectId, stageId);
            //queryData.TaskCount = queryData1.Count();
            return queryData;
        }
        public async Task<List<ProjectDashboardChartViewModel>> GetTaskStatus(string userId, string projectId, string stageId = null)
        {


            var queryData = await _performanceManagementQueryBusiness.GetTaskStatus(userId, projectId, stageId);

            var list = new List<ProjectDashboardChartViewModel>();
            var data = new List<ProjectGanttTaskViewModel>();
            //var serviceDetails = await _serviceBusiness.GetSingleById(stageId);
            //if (serviceDetails.IsNotNull())
            //{
            //    var udfNoteTableId = serviceDetails.UdfNoteTableId;
            //    if (udfNoteTableId.IsNotNullAndNotEmpty())
            //    {
            //        var a = await GetTaskListByType("", projectId, userId, udfNoteTableId);
            //        var b = await GetStageTaskList("", projectId, userId, stageId);
            //        data.AddRange(a);
            //        data.AddRange(b);
            //    }
            //}

            var serviceDetails = await _serviceBusiness.GetSingle(x => x.UdfNoteTableId == stageId);
            if (serviceDetails.IsNotNull())
            {
                var udfNoteTableId = serviceDetails.UdfNoteTableId;
                if (udfNoteTableId.IsNotNullAndNotEmpty())
                {
                    var a = await GetTaskListByType("", projectId, userId, stageId);
                    var b = await GetStageTaskList("", projectId, userId, serviceDetails.Id);
                    data.AddRange(a);
                    data.AddRange(b);
                }
            }


            var list1 = stageId.IsNotNullAndNotEmpty() ? data.GroupBy(x => x.NtsStatusCode).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.TaskStatusId).FirstOrDefault() }).ToList() :
                queryData.GroupBy(x => x.NtsStatus).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.NtsStatusId).FirstOrDefault() }).ToList(); ;
            list = list1.Select(x => new ProjectDashboardChartViewModel { Type = x.Type, Value = x.Value, Id = x.Id }).ToList();
            return list;
        }

        public async Task<List<ProjectDashboardChartViewModel>> GetPerformanceServicesStatus(string userId, string projectId, string type, string stageId = null)
        {

            var queryData = await _performanceManagementQueryBusiness.GetPerformanceServicesStatus(userId, projectId, type, stageId);
            queryData = queryData.Where(x => x.TemplateCode == type).ToList();
            var list = new List<ProjectDashboardChartViewModel>();

            IList<TeamWorkloadViewModel> data = new List<TeamWorkloadViewModel>();
            var serviceDetails = await _serviceBusiness.GetSingleById(stageId);
            if (serviceDetails.IsNotNull())
            {
                var udfNoteTableId = serviceDetails.UdfNoteTableId;
                if (udfNoteTableId.IsNotNullAndNotEmpty())
                {
                    if (type == "PMS_GOAL")
                    {
                        data = await ReadManagerPerformanceGoalViewData(projectId, udfNoteTableId, userId);
                    }
                    else if (type == "PMS_COMPENTENCY")
                    {
                        data = await ReadPerformanceCompetencyStageViewData(projectId, udfNoteTableId, userId);
                    }
                    else if (type == "PMS_DEVELOPMENT")
                    {
                        data = await ReadPerformanceDevelopmentViewData(projectId, udfNoteTableId, userId);
                    }
                }
            }


            var list1 = stageId.IsNotNullAndNotEmpty() ? data.GroupBy(x => x.NtsStatus).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.NtsStatusId).FirstOrDefault() }).ToList() :
                queryData.GroupBy(x => x.NtsStatus).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.NtsStatusId).FirstOrDefault() }).ToList(); ;
            list = list1.Select(x => new ProjectDashboardChartViewModel { Type = x.Type, Value = x.Value, Id = x.Id }).ToList();
            return list;
        }
        public async Task<List<ProjectDashboardChartViewModel>> GetPerformanceServiceStatus(string userId, string projectId, string type, string stageId = null)
        {

            var queryData = await _performanceManagementQueryBusiness.GetPerformanceServiceStatus(userId, projectId, type, stageId);
            queryData = queryData.Where(x => x.TemplateCode == type).ToList();
            var list = new List<ProjectDashboardChartViewModel>();
            var list1 = queryData.GroupBy(x => x.NtsStatus).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.NtsStatusId).FirstOrDefault() }).ToList();
            list = list1.Select(x => new ProjectDashboardChartViewModel { Type = x.Type, Value = x.Value, Id = x.Id }).ToList();
            return list;
        }

        /// <summary>
        /// Code For Chart that Bring Requested by me
        /// </summary>
        /// <param name="TemplateID"></param>
        /// /// <param name="UserID"></param>
        /// <returns></returns>
        /// 


        public async Task<List<ProjectDashboardChartViewModel>> GetTaskStatusRequestedByMe(string TemplateID, string UserID)
        {
            var queryData = await _performanceManagementQueryBusiness.GetTaskStatusRequestedByMe(TemplateID, UserID);

            var list = new List<ProjectDashboardChartViewModel>();
            list = queryData.Select(x => new ProjectDashboardChartViewModel { Type = x.Type, Value = x.Value }).ToList();
            return list;
        }
        public async Task<List<ProjectDashboardChartViewModel>> GetTaskStatusAssigneduserid(string TemplateID, string UserID)
        {
            var queryData = await _performanceManagementQueryBusiness.GetTaskStatusAssigneduserid(TemplateID, UserID);

            var list = new List<ProjectDashboardChartViewModel>();
            list = queryData.Select(x => new ProjectDashboardChartViewModel { Type = x.Type, Value = x.Value }).ToList();
            return list;
        }

        public async Task<List<ProjectDashboardChartViewModel>> MdlassignUser(string TemplateID, string UserID)
        {

            var queryData = await _performanceManagementQueryBusiness.MdlassignUser(TemplateID, UserID);

            var list = new List<ProjectDashboardChartViewModel>();
            list = queryData.Select(x => new ProjectDashboardChartViewModel { Type = x.Type, AssigneeId = x.RefId }).ToList();
            return list;
        }

        public async Task<List<NtsTaskChartList>> GetGridList(string TemplateID, string UserID, List<string> assigneeIds = null, List<string> StatusIDs = null)
        {

            var queryData = await _performanceManagementQueryBusiness.GetGridList(TemplateID, UserID, assigneeIds, StatusIDs);
            return queryData;
        }

        public async Task<List<ProjectDashboardChartViewModel>> GetDatewiseTask(string TemplateID, string UserID, DateTime? FromDate = null, DateTime? ToDate = null)
        {

            var queryData = await _performanceManagementQueryBusiness.GetDatewiseTask(TemplateID, UserID, FromDate, ToDate);

            var list = new List<ProjectDashboardChartViewModel>();
            //  var list1 = queryData.GroupBy(x => x.NtsStatus).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.NtsStatusId).FirstOrDefault() }).ToList();
            list = queryData.Select(x => new ProjectDashboardChartViewModel { Type = x.Type, Days = x.Days.Days }).ToList();
            return list;
        }

        public async Task<IList<ProjectDashboardChartViewModel>> GetTaskType(string userId, string performanceId, string stageId = null)
        {

            //var queryData = await _performanceManagementQueryBusiness.GetTaskType(userId, performanceId, stageId);
            var queryData = await _performanceManagementQueryBusiness.GetTaskStatus(userId, performanceId, stageId);

            var list = new List<ProjectDashboardChartViewModel>();


            var data = new List<ProjectGanttTaskViewModel>();
            var serviceDetails = await _serviceBusiness.GetSingleById(stageId);
            if (serviceDetails.IsNotNull())
            {
                var udfNoteTableId = serviceDetails.UdfNoteTableId;
                if (udfNoteTableId.IsNotNullAndNotEmpty())
                {
                    //var a = await GetTaskListByType("", performanceId, userId, udfNoteTableId);
                    //var b = await GetStageTaskList("", performanceId, userId, stageId);
                    //data.AddRange(a);
                    //data.AddRange(b);
                }
            }

            var list1 = queryData.GroupBy(x => x.UserName).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.UserId).FirstOrDefault() }).ToList();
            list = list1.Select(x => new ProjectDashboardChartViewModel { Id = x.Id, Type = x.Type, Value = x.Value }).ToList();
            return list;
        }
        public async Task<IList<ProjectDashboardChartViewModel>> ReadPerformanceStageChartData(string userId, string projectId)
        {
            var queryData = await _performanceManagementQueryBusiness.ReadPerformanceStageChartData(userId, projectId);

            var list = new List<ProjectDashboardChartViewModel>();
            var list1 = queryData.GroupBy(x => x.ServiceStage).Select(group => new { Value = group.Count(), Type = (group.Key.IsNotNullAndNotEmpty() ? group.Key : group.Select(x => x.ProjectName).FirstOrDefault()), Id = group.Select(x => x.ParentId).FirstOrDefault() }).ToList();
            list = list1.Select(x => new ProjectDashboardChartViewModel { Id = x.Id, Type = x.Type, Value = x.Value }).ToList();
            return list;

        }

        public async Task<IList<IdNameViewModel>> GetPerformanceStageIdNameList(string userId, string projectId)
        {

            var queryData = await _performanceManagementQueryBusiness.GetPerformanceStageIdNameList(userId, projectId);

            var list = new List<IdNameViewModel>();
            list = queryData.GroupBy(x => x.ServiceStage).Select(group => new IdNameViewModel { Id = group.Select(x => x.Id).FirstOrDefault(), Name = (group.Key.IsNotNullAndNotEmpty() ? group.Key : group.Select(x => x.ProjectName).FirstOrDefault()) }).ToList();
            return list;

        }
        public async Task<IList<IdNameViewModel>> GetPerformanceObjectiveList(string userId, string projectId)
        {

            var queryData = await _performanceManagementQueryBusiness.GetPerformanceObjectiveList(userId, projectId);


            var list = new List<IdNameViewModel>();
            list = queryData.GroupBy(x => x.ServiceStage).Select(group => new IdNameViewModel { Id = group.Select(x => x.Id).FirstOrDefault(), Name = (group.Key.IsNotNullAndNotEmpty() ? group.Key : group.Select(x => x.ProjectName).FirstOrDefault()) }).ToList();
            return list;

        }
        public async Task<IList<IdNameViewModel>> GetPerformanceStageIdNameList(string userId, string projectId, List<string> ownerIds = null, List<string> types = null)
        {
            var queryData = await _performanceManagementQueryBusiness.GetPerformanceStageIdNameList(userId, projectId, ownerIds, types);

            var list = new List<IdNameViewModel>();
            list = queryData.GroupBy(x => x.ServiceStage).Select(group => new IdNameViewModel { Id = group.Select(x => x.Id).FirstOrDefault(), Name = (group.Key.IsNotNullAndNotEmpty() ? group.Key : group.Select(x => x.ProjectName).FirstOrDefault()) }).ToList();
            return list;

        }
        public async Task<IList<TaskViewModel>> ReadTaskOverdueData(string projectId)
        {
            var queryData = await _performanceManagementQueryBusiness.ReadTaskOverdueData(projectId);
            return queryData;

        }
        public async Task<IList<TeamWorkloadViewModel>> ReadProjectSubTaskViewData(string taskId, string id, string status)
        {

            var queryData = await _performanceManagementQueryBusiness.ReadProjectSubTaskViewData(taskId, id, status);
            if (status.IsNotNullAndNotEmpty() && id.IsNotNullAndNotEmpty())
            {
                if (status == "Pending")
                {
                    queryData = queryData.Where(x => x.TaskStatusCode == "TASK_STATUS_INPROGRESS" || x.TaskStatusCode == "TASK_STATUS_OVERDUE").ToList();
                }
                else if (status == "Completed")
                {
                    queryData = queryData.Where(x => x.TaskStatusCode == "TASK_STATUS_COMPLETE").ToList();

                }

            }
            //foreach(var item in queryData)
            //{
            //    var subtask = await _taskBusiness.GetList(x => x.ParentTaskId == item.TaskId);
            //    if (subtask.Count > 0)
            //    {
            //        item.HasSubFolders = true;
            //    }
            //}
            return queryData;
        }



        public async Task<ProgramDashboardViewModel> ReadProjectTotalTaskData(string projectId, string templatecode)
        {
            var queryData1 = await _performanceManagementQueryBusiness.ReadProjectTotalTaskData(projectId, templatecode);
            return queryData1;
        }

        public async Task<IList<TeamWorkloadViewModel>> ReadManagerProjectTaskViewData(string projectId, List<string> assigneeIds = null, List<string> statusIds = null, List<string> ownerIds = null, FilterColumnEnum? column = null, DateTime? startDate = null, DateTime? dueDate = null)
        {
            var queryData = await _performanceManagementQueryBusiness.ReadManagerProjectTaskViewData(projectId, assigneeIds, statusIds, ownerIds, column, startDate, dueDate);

            return queryData;
        }

        public async Task<IList<TeamWorkloadViewModel>> ReadPerformanceTaskViewData(string projectId, string performanceUser, List<string> assigneeIds = null, List<string> statusIds = null, List<string> ownerIds = null, FilterColumnEnum? column = null, DateTime? startDate = null, DateTime? dueDate = null, string type = null)
        {
            var queryData = await _performanceManagementQueryBusiness.ReadPerformanceTaskViewData(projectId, performanceUser, assigneeIds, statusIds, ownerIds, column, startDate, dueDate, type);
            return queryData;
        }



        public async Task<IList<ServiceViewModel>> ReadPerformanceDocumentStagesData(string performanceId, string stageId = null)
        {
            var queryData = await _performanceManagementQueryBusiness.ReadPerformanceDocumentStagesData(performanceId, stageId);
            return queryData;
        }

        public async Task<IList<ServiceViewModel>> GetPerformanceDocumentStageDataByServiceId(string performanceServiceId, string ownerUserId, string stageId = null)
        {
            var queryData = await _performanceManagementQueryBusiness.GetPerformanceDocumentStageDataByServiceId(performanceServiceId, ownerUserId, stageId);
            return queryData;
        }

        public async Task<IList<ServiceViewModel>> GetPerDocMasterStageDataByServiceId(string performanceServiceId, string ownerUserId, string stageId = null)
        {
            var queryData = await _performanceManagementQueryBusiness.GetPerDocMasterStageDataByServiceId(performanceServiceId, ownerUserId, stageId);
            return queryData;
        }

        public async Task<IList<TeamWorkloadViewModel>> GetAllApprovedGoalForManager(string performanceId)
        {
            var queryData = await _performanceManagementQueryBusiness.ReadGoalServiceData(performanceId);
            return queryData;
        }

        public async Task<IList<TeamWorkloadViewModel>> ReadManagerPerformanceGoalViewData(string performanceId, string stageId, string userId)
        {
            var queryData = await _performanceManagementQueryBusiness.ReadManagerPerformanceGoalViewData(performanceId, stageId, userId);
            return queryData;
        }
        public async Task<IList<TeamWorkloadViewModel>> ReadGoalServiceData(string performanceId)
        {
            var queryData = await _performanceManagementQueryBusiness.ReadGoalServiceData(performanceId);
            return queryData;
        }
        public async Task<IList<TeamWorkloadViewModel>> ReadPerformanceCompetencyStageViewData(string performanceId, string stageId, string userId)
        {

            var queryData = await _performanceManagementQueryBusiness.ReadPerformanceCompetencyStageViewData(performanceId, stageId, userId);
            return queryData;
        }


        public async Task<IList<TeamWorkloadViewModel>> ReadPerformanceCompetencyServiceData(string performanceId)
        {

            var queryData = await _performanceManagementQueryBusiness.ReadPerformanceCompetencyServiceData(performanceId);
            return queryData;
        }
        public async Task<IList<TeamWorkloadViewModel>> GetAllApprovedCompetenciesForManager(string performanceId)
        {
            var queryData = await _performanceManagementQueryBusiness.GetAllApprovedCompetenciesForManager(performanceId);
            return queryData;
        }
        public async Task<IList<TeamWorkloadViewModel>> ReadPerformanceDevelopmentViewData(string performanceId, string stageId, string userId)
        {

            var queryData = await _performanceManagementQueryBusiness.ReadPerformanceDevelopmentViewData(performanceId, stageId, userId);
            return queryData;
        }

        public async Task<IList<TeamWorkloadViewModel>> ReadPerformanceAllData(string performanceId, string stageId, string userId)
        {

            var queryData = await _performanceManagementQueryBusiness.ReadPerformanceAllData(performanceId, stageId, userId);
            return queryData;
        }

        public async Task<IList<TeamWorkloadViewModel>> ReadProjectStageViewData(string projectId)
        {

            var queryData = await _performanceManagementQueryBusiness.ReadProjectStageViewData(projectId);
            return queryData;
        }


        public async Task<IList<TeamWorkloadViewModel>> ReadManagerProjectTaskAssignedData(string projectId, string templatecode)
        {
            var queryData = await _performanceManagementQueryBusiness.ReadManagerProjectTaskAssignedData(projectId, templatecode);
            return queryData;
        }

        public async Task<IList<TeamWorkloadViewModel>> ReadManagerPerformanceTaskAssignedData(string projectId, string templatecode)
        {
            var queryData = await _performanceManagementQueryBusiness.ReadManagerPerformanceTaskAssignedData(projectId, templatecode);
            return queryData;
        }

        public async Task<IList<TeamWorkloadViewModel>> ReadProjectTaskAssignedData(string projectId, string templatecode)
        {
            var queryData = await _performanceManagementQueryBusiness.ReadProjectTaskAssignedData(projectId, templatecode);
            return queryData;
        }

        public async Task<IList<TeamWorkloadViewModel>> ReadManagerProjectTaskOwnerData(string projectId, string templatecode)
        {
            var queryData = await _performanceManagementQueryBusiness.ReadManagerProjectTaskOwnerData(projectId, templatecode);
            return queryData;
        }

        public async Task<IList<TeamWorkloadViewModel>> ReadProjectTaskOwnerData(string projectId, string templatecode)
        {
            var queryData = await _performanceManagementQueryBusiness.ReadProjectTaskOwnerData(projectId, templatecode);
            return queryData;
        }

        public async Task<IList<DashboardCalendarViewModel>> ReadPerformanceCalendarViewData(string projectId, List<string> assigneeIds = null, List<string> statusIds = null, List<string> ownerIds = null, FilterColumnEnum? column = null, DateTime? startDate = null, DateTime? dueDate = null, string performanceStageId = null)
        {

            var queryData = await _performanceManagementQueryBusiness.ReadPerformanceCalendarViewData(projectId, assigneeIds, statusIds, ownerIds, column, startDate, dueDate, performanceStageId);

            if (ownerIds.IsNotNull() && ownerIds.Count > 0)
            {
                queryData = queryData.Where(x => ownerIds.Contains(x.OwnerUserId)).ToList();
            }

            if (assigneeIds.IsNotNull() && assigneeIds.Count > 0)
            {
                queryData = queryData.Where(x => assigneeIds.Contains(x.AssigneeUserId)).ToList();
            }

            if (statusIds.IsNotNull() && statusIds.Count > 0)
            {
                queryData = queryData.Where(x => statusIds.Contains(x.TaskStatusId)).ToList();
            }
            if (column.IsNotNull() && startDate.IsNotNull() && dueDate.IsNotNull())
            {


                if ((ownerIds.IsNotNull() && ownerIds.Count > 0) || (assigneeIds.IsNotNull() && assigneeIds.Count > 0) || (statusIds.IsNotNull() && statusIds.Count > 0))
                {
                    if (column == FilterColumnEnum.DueDate)
                    {
                        queryData = queryData.Where(x => startDate <= x.End.Date && x.End.Date < dueDate).ToList();
                    }
                    else
                    {
                        queryData = queryData.Where(x => startDate <= x.Start.Date && x.Start.Date < dueDate).ToList();
                    }

                }
                else
                {
                    if (column == FilterColumnEnum.DueDate)
                    {

                        queryData = queryData.Where(x => startDate <= x.End.Date && x.End.Date < dueDate).ToList();
                    }
                    else
                    {
                        queryData = queryData.Where(x => startDate <= x.Start.Date && x.Start.Date < dueDate).ToList();
                    }
                }


            }

            return queryData;
        }


        public async Task<IList<PerformanceDocumentViewModel>> GetPerformanceDocumentList(string userId)
        {

            var queryData = await _performanceManagementQueryBusiness.GetPerformanceDocumentList(userId);
            var list = queryData.ToList();
            return list;
        }
        public async Task<IList<ProjectGanttTaskViewModel>> GetPerformanceDocumentTaskList()
        {

            var queryData = await _performanceManagementQueryBusiness.GetPerformanceDocumentTaskList();
            var list = queryData.ToList();
            return list;
        }

        public async Task<List<ProjectGanttTaskViewModel>> GetTaskListByType(string templateCodes, string pdmId = null, string ownerId = null, string stageId = null)
        {



            var queryData = await _performanceManagementQueryBusiness.GetTaskListByType(templateCodes, pdmId, ownerId, stageId);
            var list = queryData.ToList();
            return list;
        }

        public async Task<List<ProjectGanttTaskViewModel>> GetStageTaskList(string templateCodes, string pdmId = null, string ownerId = null, string stageId = null)
        {

            var queryData = await _performanceManagementQueryBusiness.GetStageTaskList(templateCodes, pdmId, ownerId, stageId);
            var list = queryData.ToList();
            return list;
        }
        public async Task<IList<IdNameViewModel>> GetPerformanceList(string userId, bool isProjectManager, string year = null)
        {
            var queryData = await _performanceManagementQueryBusiness.GetPerformanceList(userId, isProjectManager, year);
            var list = queryData.ToList();
            return list;
        }

        public async Task<IList<IdNameViewModel>> GetPDMList(string year = null)
        {

            var queryData = await _performanceManagementQueryBusiness.GetPDMList(year);
            var list = queryData.ToList();
            return list;
        }

        public async Task<ServiceViewModel> GetPDMDetails(string pdmId)
        {
            var queryData = await _performanceManagementQueryBusiness.GetPDMDetails(pdmId);
            return queryData;
        }

        public async Task<ServiceViewModel> GetPerformanceDetails(string projectId)
        {

            var queryData = await _performanceManagementQueryBusiness.GetPerformanceDetails(projectId);
            return queryData;
        }

        public async Task<IList<TeamWorkloadViewModel>> ReadProjectTeamWorkloadData(string projectId, string userId, bool isProjectManager = false)
        {
            var queryData = await _performanceManagementQueryBusiness.ReadProjectTeamWorkloadData(projectId, userId, isProjectManager);
            var list = queryData.ToList();
            return list;
        }

        public async Task<IList<TeamWorkloadViewModel>> ReadProjectTeamDataByUser(string projectId, string userId)
        {

            var queryData = await _performanceManagementQueryBusiness.ReadProjectTeamDataByUser(projectId, userId);
            var list = queryData.ToList();
            return list;
        }
        public async Task<IList<TeamWorkloadViewModel>> ReadProjectTeamDateData(string projectId)
        {
            var queryData = await _performanceManagementQueryBusiness.ReadProjectTeamDateData(projectId);
            var list = queryData.ToList();
            return list;
        }
        public async Task<IList<TeamWorkloadViewModel>> ReadProjectTeamDataByDate(string projectId, DateTime startDate, bool isProjectManager = false)
        {
            var queryData = await _performanceManagementQueryBusiness.ReadProjectTeamDataByDate(projectId, startDate, isProjectManager);
            var list = queryData.ToList();
            return list;
        }

        public async Task<IList<TreeViewViewModel>> GetWBSItemData(string id, string type, string parentId, string userRoleId, string userId, string userRoleIds, string stageName, string stageId, string projectId, string expandingList, string userroleCode)
        {
            var expObj = new List<TreeViewViewModel>();
            if (expandingList != null)
            {
                expObj = JsonConvert.DeserializeObject<List<TreeViewViewModel>>(expandingList);
                var obj = expObj.Where(x => x.id == id).FirstOrDefault();
                if (obj.IsNotNull())
                {
                    type = obj.Type;
                    parentId = obj.ParentId;
                    userRoleId = obj.UserRoleId;
                    projectId = obj.ProjectId;
                    stageId = obj.StageId;
                }
            }
            var list = new List<TreeViewViewModel>();
            var userRoleList = new List<TreeViewViewModel>();
            var projectStageList = new List<TreeViewViewModel>();
            var projectList = new List<TreeViewViewModel>();
            var pStageList = new List<TreeViewViewModel>();
            var stageList = new List<TreeViewViewModel>();
            var query = "";
            id = "INBOX";
            if (id == "INBOX")
            {
                var roles = userRoleIds.Split(",");
                var roleText = "";
                foreach (var item in roles)
                {
                    roleText += $"'{item}',";
                }
                roleText = roleText.Trim(',');
                userRoleList = await _performanceManagementQueryBusiness.GetWBSItemInboxData(roleText, userId);
            }

            if (userRoleList.IsNotNull())
            {
                foreach (var l in userRoleList)
                {
                    if (l.Type == "USERROLE")
                    {

                        projectStageList = await _performanceManagementQueryBusiness.GetWBSItemInboxUserRole(userId, l);

                        if (projectStageList.IsNotNull())
                        {
                            foreach (var p in projectStageList)
                            {
                                if (p.Type == "PROJECTSTAGE")
                                {

                                    projectList = await _performanceManagementQueryBusiness.GetWBSItemInboxProjectStage(userId, p);

                                    if (projectList.IsNotNull())
                                    {
                                        foreach (var pr in projectList)
                                        {
                                            if (pr.Type == "PROJECT")
                                            {



                                                pStageList = await _performanceManagementQueryBusiness.GetWBSItemInboxProject(userId, pr);

                                                if (pStageList.IsNotNull())
                                                {
                                                    foreach (var ps in pStageList)
                                                    {
                                                        if (ps.Type == "STAGE")
                                                        {


                                                            stageList = await _performanceManagementQueryBusiness.GetWBSItemInboxStage(userId, ps);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            list.AddRange(userRoleList);
            list.AddRange(projectStageList);
            list.AddRange(projectList);
            list.AddRange(pStageList);
            list.AddRange(stageList);
            return list;
        }

        public async Task<bool> CreateMindMap(string model)
        {
            var mappingParent = new List<IdNameViewModel>();
            var data = JsonConvert.DeserializeObject<IList<ProjectGanttTaskViewModel>>(model);
            foreach (var d in data)
            {
                if (d.Type == "Service")
                {
                    //add service
                    var serviceTemplate = new ServiceTemplateViewModel();
                    serviceTemplate.ActiveUserId = _repo.UserContext.UserId;
                    serviceTemplate.TemplateCode = "PROJECT_SUPER_SERVICE";
                    var service = await _serviceBusiness.GetServiceDetails(serviceTemplate);
                    service.ServiceSubject = d.Title;
                    service.OwnerUserId = d.OwnerUserId;
                    service.StartDate = d.Start;
                    service.DueDate = d.End;
                    service.RequestedByUserId = d.UserId;
                    service.DataAction = DataActionEnum.Create;
                    service.Json = "{}";
                    if (d.RefId.IsNullOrEmpty())
                    {
                        service.DataAction = DataActionEnum.Create;
                    }
                    else
                    {
                        service.Id = d.Id;
                        service.DataAction = DataActionEnum.Edit;
                    }
                    var res = await _serviceBusiness.ManageService(service);
                    mappingParent.Add(new IdNameViewModel { Id = d.Id, Name = res.Item.ServiceId, Code = "Service" });
                }
                else if (d.Type == "Stage")
                {
                    //add stage
                    //add service
                    var serviceTemplate = new ServiceTemplateViewModel();
                    serviceTemplate.ActiveUserId = _repo.UserContext.UserId;
                    serviceTemplate.TemplateCode = "PROJECT_ADHOC_SERVICE";
                    var service = await _serviceBusiness.GetServiceDetails(serviceTemplate);
                    service.ServiceSubject = d.Title;
                    service.OwnerUserId = d.OwnerUserId;
                    service.StartDate = d.Start;
                    service.DueDate = d.End;
                    service.RequestedByUserId = d.UserId;
                    service.DataAction = DataActionEnum.Create;
                    service.Json = "{}";
                    if (d.ParentId.IsNotNullAndNotEmpty())
                    {
                        service.ParentServiceId = mappingParent.Where(x => x.Id == d.ParentId).FirstOrDefault().Name;
                    }
                    if (d.RefId.IsNullOrEmpty())
                    {
                        service.DataAction = DataActionEnum.Create;
                    }
                    else
                    {
                        service.Id = d.Id;
                        service.DataAction = DataActionEnum.Edit;
                    }
                    var res = await _serviceBusiness.ManageService(service);
                    mappingParent.Add(new IdNameViewModel { Id = d.Id, Name = res.Item.ServiceId, Code = "Service" });
                    //if (d.Predeccessor.Count > 0)
                    //{
                    //    foreach (var id in d.Predeccessor)
                    //    {
                    //        var prec = new NTSSERVICEP
                    //        {
                    //            NtsTaskId = res.Item.ServiceId,
                    //            PrecedenceRelationshipType = PrecedenceRelationshipTypeEnum.FinishToStart,
                    //            PredecessorType = NtsTypeEnum.Service,
                    //            PredecessorId = id
                    //        };
                    //        var result = await _ntsTaskPrecedenceBusiness.Create(prec);
                    //    }
                    //}
                }
                else if (d.Type == "Task")
                {
                    //add Task
                    var taskTemplate = new TaskTemplateViewModel();
                    taskTemplate.ActiveUserId = _repo.UserContext.UserId;
                    taskTemplate.TemplateCode = "PROJECT_ADHOC_TASK";
                    var task = await _taskBusiness.GetTaskDetails(taskTemplate);
                    task.TaskSubject = d.Title;
                    task.OwnerUserId = d.OwnerUserId;
                    task.StartDate = d.Start;
                    task.DueDate = d.End;
                    task.AssignedToUserId = d.UserId;
                    task.DataAction = DataActionEnum.Create;
                    task.Json = "{}";
                    if (d.ParentId.IsNotNullAndNotEmpty())
                    {
                        task.ParentServiceId = mappingParent.Where(x => x.Id == d.ParentId).FirstOrDefault().Name;
                    }
                    if (d.RefId.IsNullOrEmpty())
                    {
                        task.DataAction = DataActionEnum.Create;
                    }
                    else
                    {
                        task.Id = d.Id;
                        task.DataAction = DataActionEnum.Edit;
                    }
                    var res = await _taskBusiness.ManageTask(task);
                    mappingParent.Add(new IdNameViewModel { Id = d.Id, Name = res.Item.TaskId, Code = "Task" });
                    if (d.Predeccessor.Count > 0)
                    {
                        foreach (var id in d.Predeccessor)
                        {
                            var predData = mappingParent.Where(x => x.Id == id).FirstOrDefault();
                            if (predData.IsNotNull())
                            {
                                var predType = NtsTypeEnum.Task;
                                if (predData.Code == "Task")
                                {
                                    predType = NtsTypeEnum.Task;
                                }
                                else if (predData.Code == "Service")
                                {
                                    predType = NtsTypeEnum.Service;
                                }

                                var prec = new NtsTaskPrecedenceViewModel
                                {
                                    NtsTaskId = res.Item.TaskId,
                                    PrecedenceRelationshipType = PrecedenceRelationshipTypeEnum.FinishToStart,
                                    PredecessorType = predType,//NtsTypeEnum.Task,
                                    PredecessorId = predData.Name
                                };
                                var result = await _ntsTaskPrecedenceBusiness.Create(prec);
                            }
                        }
                    }
                }
                else if (d.Type == "SubTask")
                {
                    //add Task
                    var taskTemplate = new TaskTemplateViewModel();
                    taskTemplate.ActiveUserId = _repo.UserContext.UserId;
                    taskTemplate.TemplateCode = "PROJECT_ADHOC_TASK";
                    var task = await _taskBusiness.GetTaskDetails(taskTemplate);
                    task.TaskSubject = d.Title;
                    task.OwnerUserId = d.OwnerUserId;
                    task.StartDate = d.Start;
                    task.DueDate = d.End;
                    task.AssignedToUserId = d.UserId;
                    task.DataAction = DataActionEnum.Create;
                    task.ParentTaskId = d.ParentId;
                    task.Json = "{}";
                    if (d.ParentId.IsNotNullAndNotEmpty())
                    {
                        task.ParentTaskId = mappingParent.Where(x => x.Id == d.ParentId).FirstOrDefault().Name;
                    }
                    if (d.RefId.IsNullOrEmpty())
                    {
                        task.DataAction = DataActionEnum.Create;
                    }
                    else
                    {
                        task.Id = d.Id;
                        task.DataAction = DataActionEnum.Edit;
                    }
                    var res = await _taskBusiness.ManageTask(task);
                    mappingParent.Add(new IdNameViewModel { Id = d.Id, Name = res.Item.TaskId, Code = "SubTask" });
                    if (d.Predeccessor.Count > 0)
                    {
                        foreach (var id in d.Predeccessor)
                        {
                            var predData = mappingParent.Where(x => x.Id == id).FirstOrDefault();
                            if (predData.IsNotNull())
                            {
                                var predType = NtsTypeEnum.Task;
                                if (predData.Code == "Task")
                                {
                                    predType = NtsTypeEnum.Task;
                                }
                                else if (predData.Code == "Service")
                                {
                                    predType = NtsTypeEnum.Service;
                                }

                                var prec = new NtsTaskPrecedenceViewModel
                                {
                                    NtsTaskId = res.Item.TaskId,
                                    PrecedenceRelationshipType = PrecedenceRelationshipTypeEnum.FinishToStart,
                                    PredecessorType = predType,
                                    PredecessorId = predData.Name
                                };
                                var result = await _ntsTaskPrecedenceBusiness.Create(prec);
                            }
                        }
                    }
                }
            }
            return true;
        }

        public async Task<IList<ProjectGanttTaskViewModel>> ReadProjectTask(string userId, string projectId, bool isProjectManager = false, List<string> projectIds = null, List<string> ownerIds = null, List<string> assigneeIds = null, List<string> tasksStatus = null, FilterColumnEnum? column = null, DateTime? startDate = null, DateTime? dueDate = null)
        {

            var queryData = await _performanceManagementQueryBusiness.ReadProjectTask(userId, projectId, isProjectManager, projectIds, ownerIds, assigneeIds, tasksStatus, column, startDate, dueDate);

            return queryData;


        }

        public Task<IList<IdNameViewModel>> GetPerformanceUserIdNameList(string projectId)
        {
            throw new NotImplementedException();
        }
        public async Task<IList<MailViewModel>> ReadEmailTaskData(string userId)
        {
            var queryData = await _performanceManagementQueryBusiness.ReadEmailTaskData(userId);
            var list = queryData.ToList();
            return list;
        }
        public async Task<IList<WBSViewModel>> ReadProjectTaskForEmailList(string projectId)
        {
            var queryData = await _performanceManagementQueryBusiness.ReadProjectTaskForEmailList(projectId);

            return queryData;


        }
        public async Task<IList<IdNameViewModel>> ReadPerformanceTaskUserData(string projectId)
        {

            var list = await _performanceManagementQueryBusiness.ReadPerformanceTaskUserData(projectId);
            return list;
        }

        public async Task<List<ProjectDashboardChartViewModel>> GetTaskStatusByTemplate(string templateId, string userId)
        {
            var queryData = await _performanceManagementQueryBusiness.GetTaskStatusByTemplate(templateId, userId);

            var list = new List<ProjectDashboardChartViewModel>();
            var list1 = queryData.GroupBy(x => x.NtsStatus).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.NtsStatusId).FirstOrDefault() }).ToList();
            list = list1.Select(x => new ProjectDashboardChartViewModel { Type = x.Type, Value = x.Value, Id = x.Id }).ToList();
            return list;
        }
        public async Task<List<ProjectDashboardChartViewModel>> GetTaskByUsers(string templateId, string userId)
        {

            var queryData = await _performanceManagementQueryBusiness.GetTaskByUsers(templateId, userId);

            var list = new List<ProjectDashboardChartViewModel>();
            var list1 = queryData.GroupBy(x => x.OwnerName).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.UserId).FirstOrDefault() }).ToList();
            list = list1.Select(x => new ProjectDashboardChartViewModel { Id = x.Id, Type = x.Type, Value = x.Value }).ToList();
            return list;
        }

        public async Task<List<ProjectDashboardChartViewModel>> GetSLADetails(string templateId, string userId, DateTime? FromDate = null, DateTime? ToDate = null)
        {

            var queryData = await _performanceManagementQueryBusiness.GetSLADetails(templateId, userId, FromDate, ToDate);

            var list = new List<ProjectDashboardChartViewModel>();
            //var list1 = queryData.GroupBy(x => x.NtsStatus).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.NtsStatusId).FirstOrDefault() }).ToList();
            list = queryData.Select(x => new ProjectDashboardChartViewModel { Type = x.Type, Days = x.Days.Days, ActualSLA = x.ActualSLA.Days }).ToList();
            return list;
        }

        public async Task<IList<ProjectGanttTaskViewModel>> ReadTaskGridViewData(string templateId, string userId, List<string> tasksStatus = null, List<string> ownerIds = null)
        {
            var queryData = await _performanceManagementQueryBusiness.ReadTaskGridViewData(templateId, userId, tasksStatus, ownerIds);
            return queryData;
        }

        public async Task<IList<IdNameViewModel>> GetTaskOwnerUsersList(string templateId, string userId)
        {
            var queryData = await _performanceManagementQueryBusiness.GetTaskOwnerUsersList(templateId, userId);
            return queryData;
        }
        public async Task<IList<IdNameViewModel>> GetTaskUsersList(string templateId)
        {
            var queryData = await _performanceManagementQueryBusiness.GetTaskUsersList(templateId);
            return queryData;
        }
        //For PM
        public async Task<List<ProjectDashboardChartViewModel>> GetPMTaskStatusByTemplate(string templateId)
        {
            var queryData = await _performanceManagementQueryBusiness.GetPMTaskStatusByTemplate(templateId);
            var list = new List<ProjectDashboardChartViewModel>();
            var list1 = queryData.GroupBy(x => x.NtsStatus).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.NtsStatusId).FirstOrDefault() }).ToList();
            list = list1.Select(x => new ProjectDashboardChartViewModel { Type = x.Type, Value = x.Value, Id = x.Id }).ToList();
            return list;
        }
        public async Task<List<ProjectDashboardChartViewModel>> GetPMTaskByUsers(string templateId)
        {


            var queryData = await _performanceManagementQueryBusiness.GetPMTaskByUsers(templateId);

            var list = new List<ProjectDashboardChartViewModel>();
            var list1 = queryData.GroupBy(x => x.UserName).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.UserId).FirstOrDefault() }).ToList();
            list = list1.Select(x => new ProjectDashboardChartViewModel { Id = x.Id, Type = x.Type, Value = x.Value }).ToList();
            return list;
        }

        public async Task<List<ProjectDashboardChartViewModel>> GetPMSLADetails(string templateId, DateTime? FromDate = null, DateTime? ToDate = null)
        {

            var queryData = await _performanceManagementQueryBusiness.GetPMSLADetails(templateId, FromDate, ToDate);

            var list = new List<ProjectDashboardChartViewModel>();
            //var list1 = queryData.GroupBy(x => x.NtsStatus).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.NtsStatusId).FirstOrDefault() }).ToList();
            list = queryData.Select(x => new ProjectDashboardChartViewModel { Type = x.Type, Days = x.Days.Days, ActualSLA = x.ActualSLA.Days }).ToList();
            return list;
        }

        public async Task<IList<ProjectGanttTaskViewModel>> ReadPMTaskGridViewData(string templateId, List<string> tasksStatus = null, List<string> userIds = null)
        {


            var queryData = await _performanceManagementQueryBusiness.ReadPMTaskGridViewData(templateId, tasksStatus, userIds);
            return queryData;
        }


        // For Group Template


        public async Task<List<ProjectDashboardChartViewModel>> GetGroupTemplate()
        {


            //  query = query.Replace("#RequesUserID#", UserID).Replace("#TemplateID#", TemplateID);
            var queryData = await _performanceManagementQueryBusiness.GetGroupTemplate();

            var list = new List<ProjectDashboardChartViewModel>();
            //  var list1 = queryData.GroupBy(x => x.NtsStatus).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.NtsStatusId).FirstOrDefault() }).ToList();
            list = queryData.Select(x => new ProjectDashboardChartViewModel { Id = x.Id, GroupName = x.GroupName }).ToList();
            return list;
        }






        public async Task<List<ProjectDashboardChartViewModel>> GetTaskStatusRequestedByMeGroup(string TemplateID, string UserID, string StatusLOV)
        {

            var queryData = await _performanceManagementQueryBusiness.GetTaskStatusRequestedByMeGroup(TemplateID, UserID, StatusLOV);

            var list = new List<ProjectDashboardChartViewModel>();
            //  var list1 = queryData.GroupBy(x => x.NtsStatus).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.NtsStatusId).FirstOrDefault() }).ToList();
            list = queryData.Select(x => new ProjectDashboardChartViewModel { Id = x.Id, Type = x.Type, Value = x.Value }).ToList();
            return list;
        }
        public async Task<List<ProjectDashboardChartViewModel>> GetTaskStatusAssigneduseridGroup(string TemplateID, string UserID, string StatusTemplateId, string StatusLOV)
        {

            var queryData = await _performanceManagementQueryBusiness.GetTaskStatusAssigneduseridGroup(TemplateID, UserID, StatusTemplateId, StatusLOV);

            var list = new List<ProjectDashboardChartViewModel>();
            //  var list1 = queryData.GroupBy(x => x.NtsStatus).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.NtsStatusId).FirstOrDefault() }).ToList();
            list = queryData.Select(x => new ProjectDashboardChartViewModel { Type = x.Type, Value = x.Value }).ToList();
            return list;
        }

        public async Task<List<ProjectDashboardChartViewModel>> MdlassignUserGroup(string TemplateID, string UserID)
        {

            var queryData = await _performanceManagementQueryBusiness.MdlassignUserGroup(TemplateID, UserID);

            var list = new List<ProjectDashboardChartViewModel>();
            //  var list1 = queryData.GroupBy(x => x.NtsStatus).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.NtsStatusId).FirstOrDefault() }).ToList();
            list = queryData.Select(x => new ProjectDashboardChartViewModel { Type = x.Type, AssigneeId = x.RefId }).ToList();
            return list;
        }

        public async Task<List<NtsTaskChartList>> GetGridListGroup(string TemplateID, string UserID, List<string> assigneeIds = null, List<string> StatusIDs = null)
        {

            //query = query.Replace("#RequesUserID#", UserID).Replace("#TemplateID#", TemplateID);
            var queryData = await _performanceManagementQueryBusiness.GetGridListGroup(TemplateID, UserID, assigneeIds, StatusIDs);

            //var list = new List<ProjectDashboardChartViewModel>();
            //  var list1 = queryData.GroupBy(x => x.NtsStatus).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.NtsStatusId).FirstOrDefault() }).ToList();
            //list = queryData.Select(x => new ProjectDashboardChartViewModel { Type = x.Type, Value = x.Value }).ToList();
            return queryData;
        }

        public async Task<List<ProjectDashboardChartViewModel>> GetDatewiseTaskGroup(string TemplateID, string UserID, DateTime? FromDate = null, DateTime? ToDate = null)
        {

            var queryData = await _performanceManagementQueryBusiness.GetDatewiseTaskGroup(TemplateID, UserID, FromDate, ToDate);


            var list = new List<ProjectDashboardChartViewModel>();
            //  var list1 = queryData.GroupBy(x => x.NtsStatus).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.NtsStatusId).FirstOrDefault() }).ToList();
            //list = queryData.Select(x => new ProjectDashboardChartViewModel { Type = x.Type, Days = x.Days.Days, ActualSLA = x.ActualSLA.Days }).ToList();
            list = queryData.Select(x => new ProjectDashboardChartViewModel { Type = x.Type, Days = x.Days.Days, ActualSLA = x.ActualSLA.Days }).ToList();

            return list;
        }

        //for project manager
        public async Task<List<ProjectDashboardChartViewModel>> GetTaskStatusRequestedByMeProjectGroup(string TemplateID, string StatusLOV = null)
        {


            var queryData = await _performanceManagementQueryBusiness.GetTaskStatusRequestedByMeProjectGroup(TemplateID, StatusLOV);

            var list = new List<ProjectDashboardChartViewModel>();
            var list1 = queryData.GroupBy(x => x.NtsStatus).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.Id).FirstOrDefault() }).ToList();
            list = list1.Select(x => new ProjectDashboardChartViewModel { Type = x.Type, Value = x.Value, Id = x.Id }).ToList();
            return list;
        }
        public async Task<List<ProjectDashboardChartViewModel>> GetChartByAssigneduserProjectGroup(string templateId, string StatusTemplateID = null, string StatusLOV = null)
        {


            var queryData = await _performanceManagementQueryBusiness.GetChartByAssigneduserProjectGroup(templateId, StatusTemplateID, StatusLOV);

            var list = new List<ProjectDashboardChartViewModel>();
            var list1 = queryData.GroupBy(x => x.OwnerName).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.UserId).FirstOrDefault() }).ToList();
            list = list1.Select(x => new ProjectDashboardChartViewModel { Id = x.Id, Type = x.Type, Value = x.Value }).ToList();
            return list;
        }

        public async Task<IList<ProjectGanttTaskViewModel>> GetGridListProjectGroup(string templateId, List<string> tasksStatus = null, List<string> ownerIds = null)
        {


            var queryData = await _performanceManagementQueryBusiness.GetGridListProjectGroup(templateId, tasksStatus, ownerIds);
            return queryData;
        }

        public async Task<IList<ProjectDashboardChartViewModel>> GetTaskStatusAssigneduseridProjectGroup(string templateId)
        {

            var queryData = await _performanceManagementQueryBusiness.GetTaskStatusAssigneduseridProjectGroup(templateId);

            var list = new List<ProjectDashboardChartViewModel>();
            //  var list1 = queryData.GroupBy(x => x.NtsStatus).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.NtsStatusId).FirstOrDefault() }).ToList();
            list = queryData.Select(x => new ProjectDashboardChartViewModel { Type = x.Type, Value = x.Value }).ToList();
            return list;
        }

        public async Task<IList<ProjectDashboardChartViewModel>> GetDatewiseTaskProjectGroup(string templateId, DateTime? FromDate, DateTime? ToDate)
        {


            var queryData = await _performanceManagementQueryBusiness.GetDatewiseTaskProjectGroup(templateId, FromDate, ToDate);




            var list = new List<ProjectDashboardChartViewModel>();
            //  var list1 = queryData.GroupBy(x => x.NtsStatus).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.NtsStatusId).FirstOrDefault() }).ToList();
            list = queryData.Select(x => new ProjectDashboardChartViewModel { Type = x.Type, Days = x.Days.Days, ActualSLA = x.ActualSLA.Days }).ToList();
            return list;
        }







        public async Task<List<ProjectDashboardChartViewModel>> GetTaskStatusByTemplateGroup(string templateId, string userId, string StatusLOV = null)
        {


            var queryData = await _performanceManagementQueryBusiness.GetTaskStatusByTemplateGroup(templateId, userId, StatusLOV);

            var list = new List<ProjectDashboardChartViewModel>();
            var list1 = queryData.GroupBy(x => x.NtsStatus).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.NtsStatusId).FirstOrDefault() }).ToList();
            list = list1.Select(x => new ProjectDashboardChartViewModel { Type = x.Type, Value = x.Value, Id = x.Id }).ToList();
            return list;
        }
        public async Task<List<ProjectDashboardChartViewModel>> GetTaskByUsersGroup(string templateId, string userId, string StatusTemplateID = null, string StatusLOV = null)
        {

            var queryData = await _performanceManagementQueryBusiness.GetTaskByUsersGroup(templateId, userId, StatusTemplateID, StatusLOV);

            var list = new List<ProjectDashboardChartViewModel>();
            var list1 = queryData.GroupBy(x => x.OwnerName).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.UserId).FirstOrDefault() }).ToList();
            list = list1.Select(x => new ProjectDashboardChartViewModel { Id = x.Id, Type = x.Type, Value = x.Value }).ToList();
            return list;
        }

        public async Task<IList<ProjectGanttTaskViewModel>> ReadTaskGridViewDataGroup(string templateId, string userId, List<string> tasksStatus = null, List<string> ownerIds = null)
        {
            var queryData = await _performanceManagementQueryBusiness.ReadTaskGridViewDataGroup(templateId, userId, tasksStatus, ownerIds);
            return queryData;
        }

        public async Task<IList<IdNameViewModel>> GetTaskOwnerUsersListGroup(string templateId, string userId)
        {
            var queryData = await _performanceManagementQueryBusiness.GetTaskOwnerUsersListGroup(templateId, userId);
            return queryData;
        }

        public async Task<IList<ProjectDashboardChartViewModel>> GetDatewiseSingleGroup(string templateId, string userId, DateTime? FromDate, DateTime? ToDate)
        {

            var queryData = await _performanceManagementQueryBusiness.GetDatewiseSingleGroup(templateId, userId, FromDate, ToDate);




            var list = new List<ProjectDashboardChartViewModel>();
            //  var list1 = queryData.GroupBy(x => x.NtsStatus).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.NtsStatusId).FirstOrDefault() }).ToList();
            list = queryData.Select(x => new ProjectDashboardChartViewModel { Type = x.Type, Days = x.Days.Days, ActualSLA = x.ActualSLA.Days }).ToList();
            return list;
        }

        public async Task<PerformanceDocumentViewModel> GetPerformanceDocumentDetails(string Id)
        {
            var queryData = await _performanceManagementQueryBusiness.GetPerformanceDocumentDetails(Id);
            return queryData;
        }
        public async Task<List<PerformanceDocumentViewModel>> GetPerformanceDocumentsList()
        {
            var queryData = await _performanceManagementQueryBusiness.GetPerformanceDocumentsList();
            return queryData;
        }
        public async Task<bool?> UpdatePerformanceDocumentMasterStatus(string id, PerformanceDocumentStatusEnum status)
        {
            var result = await _performanceManagementQueryBusiness.UpdatePerformanceDocumentMasterStatus(id, status);
            return result;
        }
        public async Task<bool?> UpdatePerformanceDocumentMasterStageStatus(string id, PerformanceDocumentStatusEnum status)
        {
            var result = await _performanceManagementQueryBusiness.UpdatePerformanceDocumentMasterStageStatus(id, status);
            return result;
        }

        public async Task<PerformanceDocumentViewModel> IsDocNameExist(string docName, string docId)
        {
            var queryData = await _performanceManagementQueryBusiness.IsDocNameExist(docName, docId);
            return queryData;
        }


        public async Task<IList<PerformanceDocumentStageViewModel>> GetPerformanceDocumentStageData(string parentNoteId, string noteId, string udfNoteId, bool isEnableReview = false)
        {
            var queryData = await _performanceManagementQueryBusiness.GetPerformanceDocumentStageData(parentNoteId, noteId, udfNoteId, isEnableReview);
            return queryData;
        }
        public async Task<PerformanceDocumentStageViewModel> GetPerformanceDocumentStage(string Id)
        {
            var queryData = await _performanceManagementQueryBusiness.GetPerformanceDocumentStage(Id);
            return queryData;
        }
        public async Task<IList<PerformanceDocumentViewModel>> GetPerformanceGradeRatingData(string parentNoteId, string udfNoteId, string noteId = null)
        {

            var queryData = await _performanceManagementQueryBusiness.GetPerformanceGradeRatingData(parentNoteId, udfNoteId, noteId);
            return queryData;
        }

        public async Task<IList<IdNameViewModel>> GetPerformanceGradeRatingList(string perRatingId)
        {
            //var ratdetails = await _performanceManagementQueryBusiness.GetPerformanceGradeRatingList(perRatingId);

            var queryData = await _performanceManagementQueryBusiness.GetPerformanceGradeRatingListData(perRatingId);
            return queryData;
        }
        //public async Task<IList<IdNameViewModel>> GetPerformanceGradeRatingList()
        //{
        //    var query = $@"SELECT gr.""Name"" as Name, gr.""NtsNoteId"" as Id
        //                    FROM  public.""NtsNote"" N
        //                    inner join cms.""N_General_PerformanceRatingItem"" gr on  N.""Id"" =gr.""NtsNoteId"" and N.""IsDeleted""=false
        //                    where N.""ParentNoteId""='{perRatingId}' and gr.""IsDeleted""=false ";

        //    var queryData = await _queryRepo1.ExecuteQueryList(query, null);
        //    return queryData;
        //}
        private async Task<PerformanceDocumentStageViewModel> GetPerformanceDocumentMasterById(string noteId)
        {

            var queryData = await _performanceManagementQueryBusiness.GetPerformanceDocumentMasterId(noteId);
            return queryData;
        }
        public async Task<PerformanceDocumentStageViewModel> GetPerformanceDocumentMasterStageById(string noteId)
        {
            var queryData = await _performanceManagementQueryBusiness.GetPerformanceDocumentMasterStageById(noteId);
            return queryData;
        }
        public async Task<PerformanceDocumentViewModel> GetPerformanceDocumentMasterByNoteId(string noteId)
        {
            var queryData = await _performanceManagementQueryBusiness.GetPerformanceDocumentMasterByNoteId(noteId);
            return queryData;
        }

        public async Task<PerformanceDocumentViewModel> GetPerformanceDocumentMasterByServiceId(string serviceId)
        {
            var queryData = await _performanceManagementQueryBusiness.GetPerformanceDocumentMasterByServiceId(serviceId);
            return queryData;
        }
        public async Task<PerformanceDocumentViewModel> GetPerformanceDocumentMasterByDocServiceId(string serviceId)
        {
            var queryData = await _performanceManagementQueryBusiness.GetPerformanceDocumentMasterByDocServiceId(serviceId);
            return queryData;
        }

        public async Task<PerformanceDocumentViewModel> GetPDMByServiceId(string serviceId)
        {
            var queryData = await _performanceManagementQueryBusiness.GetPDMByServiceId(serviceId);
            return queryData;
        }

        public async Task<IList<TreeViewViewModel>> GetDiagramMenuItem(string id, string type, string parentId, string userRoleId, string userId, string userRoleIds, string stageName, string stageId, string projectId, string expandingList, string userroleCode)
        {
            var expObj = new List<TreeViewViewModel>();
            if (expandingList != null)
            {
                expObj = JsonConvert.DeserializeObject<List<TreeViewViewModel>>(expandingList);
                var obj = expObj.Where(x => x.id == id).FirstOrDefault();
                if (obj.IsNotNull())
                {
                    type = obj.Type;
                    parentId = obj.ParentId;
                    userRoleId = obj.UserRoleId;
                    projectId = obj.ProjectId;
                    stageId = obj.StageId;
                }
            }

            var list = new List<TreeViewViewModel>();
            if (id.IsNullOrEmpty())
            {
                var roles = userRoleIds.Split(",");
                var roleText = "";
                foreach (var item in roles)
                {
                    roleText += $"'{item}',";
                }
                roleText = roleText.Trim(',');

                list = await _performanceManagementQueryBusiness.GetDiagramMenuItemData(roleText, userId);
                //expanded -> type= userrole - from coming list find type as userRole
                // if found then find the item in list as selcted item id
                //make expanded true

                var obj = expObj.Where(x => x.Type == "USERROLE").FirstOrDefault();
                if (obj.IsNotNull())
                {
                    var data = list.Where(x => x.id == obj.UserRoleId).FirstOrDefault();
                    if (data.IsNotNull())
                    {
                        data.expanded = true;
                    }
                }

            }
            else if (type == "USERROLE")
            {

                list = await _performanceManagementQueryBusiness.GetDiagramMenuItemUserRoleData(id, userRoleId, userId);



                var obj = expObj.Where(x => x.Type == "PERFORMANCETYPE").FirstOrDefault();
                if (obj.IsNotNull())
                {
                    var data = list.Where(x => x.id == obj.id).FirstOrDefault();
                    if (data.IsNotNull())
                    {
                        data.expanded = true;
                    }
                }

            }
            else if (type == "PERFORMANCETYPE")
            {
                list = await _performanceManagementQueryBusiness.GetDiagramMenuItemPerformanceType(id, userRoleId, userId);


                var obj = expObj.Where(x => x.Type == "PERFORMANCE").FirstOrDefault();
                if (obj.IsNotNull())
                {
                    var data = list.Where(x => x.id == obj.id).FirstOrDefault();
                    if (data.IsNotNull())
                    {
                        data.expanded = true;
                    }
                }

            }
            else if (type == "PERFORMANCE")
            {



                list = await _performanceManagementQueryBusiness.GetDiagramMenuItemPerformance(id, userId);


                var obj = expObj.Where(x => x.Type == "STAGE").FirstOrDefault();
                if (obj.IsNotNull())
                {
                    var data = list.Where(x => x.id == obj.id).FirstOrDefault();
                    if (data.IsNotNull())
                    {
                        data.expanded = true;
                    }
                }


            }

            return list;
        }

        public async Task<IList<PerformanceDiagramViewModel>> GetPerformanceDiagram(string performanceDocumentId)
        {
            var list = new List<PerformanceDiagramViewModel>();

            list = await _performanceManagementQueryBusiness.GetPerformanceDiagram(performanceDocumentId);

            var adhoc = list.Where(x => x.Type != "SERVICE").Select(x => new PerformanceDiagramViewModel
            {
                Id = x.Id + "_AdhocTask",
                Title = "Tasks",
                Description = x.Title,
                ReferenceId = x.Id,
                ParentId = x.Id,
                Type = "ADHOCROOT",
                TemplateType = TemplateTypeEnum.Service,
                NodeShape = NodeShapeEnum.Rectangle
            });

            var step = list.Where(x => x.Type != "SERVICE").Select(x => new PerformanceDiagramViewModel
            {
                Id = x.Id + "_StepTask",
                Title = "Activities",
                Description = x.Title,
                ReferenceId = x.Id,
                ParentId = x.Id,
                Type = "STEPROOT",
                TemplateType = TemplateTypeEnum.Service,
                NodeShape = NodeShapeEnum.Rectangle
            });
            list.AddRange(adhoc.ToList());

            list.AddRange(step.ToList());

            list.Add(new PerformanceDiagramViewModel
            {
                Id = "GOAL",
                Title = "Goal",
                Description = "Goal",
                ReferenceId = "GOAL_ROOT",
                ParentId = performanceDocumentId,
                Type = "GOAL_ROOT",
                TemplateType = TemplateTypeEnum.Custom,
                NodeShape = NodeShapeEnum.Rectangle
            });

            list.Add(new PerformanceDiagramViewModel
            {
                Id = "COMPETENCY",
                Title = "Competency",
                Description = "Competency",
                ReferenceId = "COMPENTENCY_ROOT",
                ParentId = performanceDocumentId,
                Type = "COMPENTENCY_ROOT",
                TemplateType = TemplateTypeEnum.Custom,
                NodeShape = NodeShapeEnum.Rectangle
            });

            list.Add(new PerformanceDiagramViewModel
            {
                Id = "DEVELOPMENT",
                Title = "Development",
                Description = "Development",
                ReferenceId = "DEVELOPMENT_ROOT",
                ParentId = performanceDocumentId,
                Type = "DEVELOPMENT_ROOT",
                TemplateType = TemplateTypeEnum.Custom,
                NodeShape = NodeShapeEnum.Rectangle
            });

            list.Add(new PerformanceDiagramViewModel
            {
                Id = "PEERREVIEW",
                Title = "Peer Review",
                Description = "Peer Review",
                ReferenceId = "PEERREVIEW_ROOT",
                ParentId = performanceDocumentId,
                Type = "PEERREVIEW_ROOT",
                TemplateType = TemplateTypeEnum.Custom,
                NodeShape = NodeShapeEnum.Rectangle
            });

            //list.Add(new PerformanceDiagramViewModel
            //{
            //    Id = "STEPTASK",
            //    Title = "Step Task",
            //    Description = "Step Task",
            //    ReferenceId = "STEPTASK_ROOT",
            //    ParentId = performanceDocumentId,
            //    Type = "STEPTASK_ROOT",
            //    TemplateType = TemplateTypeEnum.Custom,
            //    NodeShape = NodeShapeEnum.Rectangle
            //});


            var taskResult = await _performanceManagementQueryBusiness.GetPerformanceDiagramData(performanceDocumentId);


            if (taskResult.IsNotNull())
            {
                var taskList = taskResult.Select(x => new PerformanceDiagramViewModel()
                {
                    Id = x.Id + "_AdhocTask",
                    Title = x.Title,
                    Description = x.Title,
                    ReferenceId = x.ParentId,
                    ParentId = GetNodeParentId(x),
                    Type = x.Code,
                    TemplateType = TemplateTypeEnum.Task,
                    NodeShape = NodeShapeEnum.SquareWithHeader
                });

                //var subTaskList = taskList.Select(x => new  PerformanceDiagramViewModel()
                //{
                //    Id = x.Id + "_AdhocTask",
                //    Title = "Sub Tasks",
                //    Description = x.Title,
                //    ReferenceId = x.Id,
                //    ParentId = x.Id,
                //    Type = "SUBTASKROOT",
                //    TemplateType = TemplateTypeEnum.Service,
                //    NodeShape = NodeShapeEnum.Rectangle
                //});

                list.AddRange(taskList);
                //list.AddRange(subTaskList);
            }

            var stepRoots = new List<PerformanceDiagramViewModel>();
            foreach (var l in list)
            {
                if (l.Type == "STEPROOT" || l.Type == "SUBTASKROOT")
                {
                    var lst = list.Where(x => x.ParentId == l.Id).ToList();
                    if (lst.IsNotNull() && lst.Count == 0)
                    {
                        stepRoots.Add(l);
                    }
                }
            }

            list = list.Except(stepRoots).ToList();

            list = list.Where(x => !x.Id.Contains("AdhocTask_StepTask")).ToList();

            return list;
        }

        public static string GetNodeParentId(ProjectGanttTaskViewModel model)
        {
            if (model.Code == "PMS_GOAL_ADHOC_TASK" || model.Code == "PMS_COMPENTENCY_ADHOC_TASK" || model.Code == "PMS_DEVELOPMENT_ADHOC_TASK" || model.Code == "PMS_REVIEW_TASK")
            {
                return model.ParentId + "_AdhocTask";
            }
            else
            {
                return model.ParentId + "_StepTask";
            }
        }

        public async Task<IList<GoalViewModel>> GetGoalWeightageByPerformanceId(string Id, string stageId, string userId)
        {
            var queryData = await _performanceManagementQueryBusiness.GetGoalWeightageByPerformanceId(Id, stageId, userId);

            return queryData;
        }


        public async Task<IList<GoalViewModel>> GetCompentencyWeightageByPerformanceId(string Id, string stageId, string userId)
        {
            var queryData = await _performanceManagementQueryBusiness.GetCompentencyWeightageByPerformanceId(Id, stageId, userId);

            return queryData;
        }

        public async Task<IList<PerformanceDocumentViewModel>> GetPerformanceDocumentMappedUserData(string perDocId, string userid)
        {
            var queryData = await _performanceManagementQueryBusiness.GetPerformanceDocumentMappedUserData(perDocId, userid);
            return queryData;
        }

        public async Task<IList<IdNameViewModel>> GetPerDocMasMappedUserData(string perDocId)
        {
            var queryData = await _performanceManagementQueryBusiness.GetPerDocMasMappedUserData(perDocId);
            return queryData;
        }

        //private async Task<CommandResult<PerformanceDocumentViewModel>> ChangeStatusforDocumentMaster(PerformanceDocumentStatusEnum status, PerformanceDocumentViewModel model)
        //{
        //    await _performanceManagementQueryBusiness.ChangeStatusforDocumentMaster(status, model);
        //    model.DocumentStatus = status;
        //    return CommandResult<PerformanceDocumentViewModel>.Instance(model);
        //}

        //public async Task<CommandResult<PerformanceDocumentStageViewModel>> ChangeStatusforDocumentMasterStage(PerformanceDocumentStatusEnum status, PerformanceDocumentStageViewModel model)
        //{
        //    await _performanceManagementQueryBusiness.ChangeStatusforDocumentMasterStage(status, model);
        //    model.DocumentStageStatus = status;
        //    return CommandResult<PerformanceDocumentStageViewModel>.Instance(model);

        //    //var noteTempModel = new NoteTemplateViewModel();
        //    //noteTempModel.DataAction = DataActionEnum.Edit;
        //    //noteTempModel.ActiveUserId = _userContext.UserId;
        //    //noteTempModel.NoteId = model.NoteId;
        //    //noteTempModel.SetUdfValue = true;
        //    //var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
        //    ////var rowData1 = notemodel.ColumnList.ToDictionary(x => x.Name, x => x.Value);
        //    ////var k = rowData1.FirstOrDefault(x => x.Key == "DocumentStageStatus");
        //    ////if (k.IsNotNull() && k.Key.IsNotNullAndNotEmpty())
        //    ////{
        //    ////    rowData1[k.Key] = status;
        //    ////}
        //    //model.DocumentStageStatus = status;
        //    //notemodel.Json = JsonConvert.SerializeObject(model);
        //    //var result = await _noteBusiness.ManageNote(notemodel);
        //    //if (result.IsSuccess)
        //    //{
        //    //    return CommandResult<PerformanceDocumentStageViewModel>.Instance(model);
        //    //}
        //    //return CommandResult<PerformanceDocumentStageViewModel>.Instance(model, false, result.Messages);
        //}

        public async Task<CommandResult<PerformanceDocumentViewModel>> CreatePerDoc(PerformanceDocumentViewModel model)
        {
            var validateName = await IsDocNameExist(model.Name, model.Id);
            if (validateName != null)
            {
                return CommandResult<PerformanceDocumentViewModel>.Instance(model, false, "Name Already Exist");
            }

            var noteTempModel = new NoteTemplateViewModel();
            noteTempModel.DataAction = model.DataAction;
            noteTempModel.ActiveUserId = _userContext.UserId;
            noteTempModel.TemplateCode = "PERFORMANCE_DOCUMENT_MASTER";
            var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);

            model.LetterTemplate = HttpUtility.HtmlDecode(model.LetterTemplate);

            notemodel.Json = JsonConvert.SerializeObject(model);
            notemodel.NoteStatusCode = "NOTE_STATUS_COMPLETE";

            var result = await _noteBusiness.ManageNote(notemodel);
            if (result.IsSuccess)
            {
                return CommandResult<PerformanceDocumentViewModel>.Instance(model, result.IsSuccess, result.Messages);
            }
            return CommandResult<PerformanceDocumentViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        public async Task<CommandResult<PerformanceDocumentViewModel>> EditPerDoc(PerformanceDocumentViewModel model)
        {
            var validateName = await IsDocNameExist(model.Name, model.Id);
            if (validateName != null)
            {
                return CommandResult<PerformanceDocumentViewModel>.Instance(model, false, "Name Already Exist");
            }

            var noteTempModel = new NoteTemplateViewModel();
            noteTempModel.DataAction = DataActionEnum.Edit;
            noteTempModel.ActiveUserId = _userContext.UserId;
            noteTempModel.NoteId = model.NoteId;
            var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);

            model.LetterTemplate = HttpUtility.HtmlDecode(model.LetterTemplate);

            notemodel.Json = JsonConvert.SerializeObject(model);
            var result = await _noteBusiness.ManageNote(notemodel);
            if (result.IsSuccess)
            {
                return CommandResult<PerformanceDocumentViewModel>.Instance(model, result.IsSuccess, result.Messages);
            }
            return CommandResult<PerformanceDocumentViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async Task<CommandResult<PerformanceDocumentViewModel>> CreatePerGradeRating(PerformanceDocumentViewModel model)
        {

            var validateName = await IsRatingExist(model);
            if (validateName != null)
            {
                return CommandResult<PerformanceDocumentViewModel>.Instance(model, false, "Record Already Exist");
            }
            var noteTempModel = new NoteTemplateViewModel();
            noteTempModel.DataAction = model.DataAction;
            noteTempModel.ActiveUserId = _userContext.UserId;
            noteTempModel.TemplateCode = "PERFORMANCE_GRADE_RATING_PERCENTAGE";
            noteTempModel.ParentNoteId = model.ParentNoteId;
            var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);

            notemodel.Json = JsonConvert.SerializeObject(model);
            notemodel.NoteStatusCode = "NOTE_STATUS_COMPLETE";

            var result = await _noteBusiness.ManageNote(notemodel);
            if (result.IsSuccess)
            {
                return CommandResult<PerformanceDocumentViewModel>.Instance(model, result.IsSuccess, result.Messages);
            }
            return CommandResult<PerformanceDocumentViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        public async Task<CommandResult<PerformanceDocumentViewModel>> EditPerGradeRating(PerformanceDocumentViewModel model)
        {
            var validateName = await IsRatingExist(model);
            if (validateName != null)
            {
                return CommandResult<PerformanceDocumentViewModel>.Instance(model, false, "Record Already Exist");
            }
            var noteTempModel = new NoteTemplateViewModel();
            noteTempModel.DataAction = DataActionEnum.Edit;
            noteTempModel.ActiveUserId = _userContext.UserId;
            noteTempModel.NoteId = model.NoteId;
            var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);

            notemodel.Json = JsonConvert.SerializeObject(model);
            var result = await _noteBusiness.ManageNote(notemodel);
            if (result.IsSuccess)
            {
                return CommandResult<PerformanceDocumentViewModel>.Instance(model, result.IsSuccess, result.Messages);
            }
            return CommandResult<PerformanceDocumentViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async Task<PerformanceDocumentViewModel> IsRatingExist(PerformanceDocumentViewModel model)
        {
            var ratings = await GetPerformanceGradeRatingData(model.ParentNoteId, model.NoteId, model.Id);
            var exist = ratings.Where(x => x.RatingId == model.RatingId && x.GradeId == model.GradeId && x.Id != model.Id).FirstOrDefault();
            return exist;

        }
        public async Task<CommandResult<PerformanceDocumentStageViewModel>> IsStageNameExists(PerformanceDocumentStageViewModel model)
        {
            var errorList = new Dictionary<string, string>();
            var docStageModel = await GetPerformanceDocumentStageData(model.ParentNoteId, null, null);
            if (docStageModel.Count > 0)
            {
                var stagemodel = docStageModel.Select(x => x.Name == model.Name && x.Id != model.Id);
                foreach (var item in stagemodel)
                {
                    if (item)
                    {
                        errorList.Add("Name", "Name already exist.");
                    }
                }
            }
            if (errorList.Count > 0)
            {
                return CommandResult<PerformanceDocumentStageViewModel>.Instance(model, false, errorList);
            }
            return CommandResult<PerformanceDocumentStageViewModel>.Instance();
        }
        private async Task<CommandResult<PerformanceDocumentStageViewModel>> ValidateDate(PerformanceDocumentStageViewModel model)
        {
            var errorList = new Dictionary<string, string>();
            var docModel = await GetPerformanceDocumentDetails(model.ParentNoteId);
            if (model.StartDate > model.EndDate)
            {
                errorList.Add("Date", "Start date should be less than End Date");
            }
            if (model.StartDate < docModel.StartDate)
            {
                errorList.Add("StartDate", "Stage Start date should be greater than " + docModel.StartDate.ToDefaultDateFormat());
            }
            if (model.EndDate > docModel.EndDate)
            {
                errorList.Add("EndDate", "Stage End date should be less than " + docModel.EndDate.ToDefaultDateFormat());
            }
            if (errorList.Count > 0)
            {
                return CommandResult<PerformanceDocumentStageViewModel>.Instance(model, false, errorList);
            }
            return CommandResult<PerformanceDocumentStageViewModel>.Instance();
        }


        public async Task<CommandResult<PerformanceDocumentStageViewModel>> CreatePerDocStage(PerformanceDocumentStageViewModel model)
        {
            var validateName = await IsStageNameExists(model);
            if (!validateName.IsSuccess)
            {
                return CommandResult<PerformanceDocumentStageViewModel>.Instance(model, false, validateName.Messages);
            }

            var validateDate = await ValidateDate(model);
            if (!validateDate.IsSuccess)
            {
                return CommandResult<PerformanceDocumentStageViewModel>.Instance(model, false, validateDate.Messages);
            }

            var noteTempModel = new NoteTemplateViewModel();
            noteTempModel.DataAction = model.DataAction;
            noteTempModel.ActiveUserId = _userContext.UserId;
            noteTempModel.TemplateCode = "PERFORMACE_DOCUMENT_MASTER_STAGE";
            noteTempModel.ParentNoteId = model.ParentNoteId;
            var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);

            notemodel.Json = JsonConvert.SerializeObject(model);
            notemodel.NoteStatusCode = "NOTE_STATUS_COMPLETE";

            var result = await _noteBusiness.ManageNote(notemodel);
            if (result.IsSuccess)
            {
                return CommandResult<PerformanceDocumentStageViewModel>.Instance(model, result.IsSuccess, result.Messages);
            }
            return CommandResult<PerformanceDocumentStageViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async Task<CommandResult<PerformanceDocumentStageViewModel>> EditPerDocStage(PerformanceDocumentStageViewModel model)
        {
            var validateName = await IsStageNameExists(model);
            if (!validateName.IsSuccess)
            {
                return CommandResult<PerformanceDocumentStageViewModel>.Instance(model, false, validateName.Messages);
            }

            var validateDate = await ValidateDate(model);
            if (!validateDate.IsSuccess)
            {
                return CommandResult<PerformanceDocumentStageViewModel>.Instance(model, false, validateDate.Messages);
            }

            var noteTempModel = new NoteTemplateViewModel();
            noteTempModel.DataAction = DataActionEnum.Edit;
            noteTempModel.ActiveUserId = _userContext.UserId;
            noteTempModel.NoteId = model.NoteId;
            noteTempModel.ParentNoteId = model.ParentNoteId;
            var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);

            notemodel.Json = JsonConvert.SerializeObject(model);
            var result = await _noteBusiness.ManageNote(notemodel);
            if (result.IsSuccess)
            {
                return CommandResult<PerformanceDocumentStageViewModel>.Instance(model, result.IsSuccess, result.Messages);
            }
            return CommandResult<PerformanceDocumentStageViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        public async Task<bool> DeleteDocumentStage(string noteId)
        {
            await _performanceManagementQueryBusiness.DeleteDocumentStage(noteId);

            //await Delete(noteId);
            return true;
        }

        public async Task<List<IdNameViewModel>> GetPerformanceDocumentGoalTemplates()
        {
            var data = await _performanceManagementQueryBusiness.GetPerformanceDocumentGoalTemplates();

            //await Delete(noteId);
            return data;
        }

        public async Task<List<IdNameViewModel>> GetPerformanceDocumentCompetencyTemplates()
        {
            var data = await _performanceManagementQueryBusiness.GetPerformanceDocumentCompetencyTemplates();

            //await Delete(noteId);
            return data;
        }

        public async Task<List<IdNameViewModel>> GetEmployeeReviewTemplate()
        {
            var data = await _performanceManagementQueryBusiness.GetEmployeeReviewTemplate();


            return data;
        }
        public async Task<List<IdNameViewModel>> GetManagerReviewTemplate()
        {
            var data = await _performanceManagementQueryBusiness.GetManagerReviewTemplate();


            return data;
        }
        public async Task<List<IdNameViewModel>> GetPerformanceRatingsList()
        {
            var data = await _performanceManagementQueryBusiness.GetPerformanceRatingsList();
            return data;
        }

        public async Task<PerformanceDocumentStageViewModel> GetPerformanceDocumentStageByMaster(string parentServiceId, string docMasterStageId)
        {

            var queryData = await _performanceManagementQueryBusiness.GetPerformanceDocumentStageByMaster(parentServiceId, docMasterStageId);
            return queryData;
        }

        public async Task<PerformanceDocumentViewModel> GetPerformanceDocumentByMaster(string ownerUserId, string docMasterId)
        {
            var queryData = await _performanceManagementQueryBusiness.GetPerformanceDocumentByMaster(ownerUserId, docMasterId);
            return queryData;
        }

        public async Task<List<TeamWorkloadViewModel>> GetAllApprovedGoals(string ownerUserId, string docServiceId, string stageId)
        {

            var queryData = await _performanceManagementQueryBusiness.GetAllApprovedGoals(ownerUserId, docServiceId, stageId);
            return queryData;

        }
        public async Task<List<TeamWorkloadViewModel>> GetAllApprovedCompetencies(string ownerUserId, string docServiceId, string stageId)
        {

            var queryData = await _performanceManagementQueryBusiness.GetAllApprovedCompetencies(ownerUserId, docServiceId, stageId);
            return queryData;


        }

        public async Task<List<TeamWorkloadViewModel>> GetAllStageGoals(string ownerUserId, string docServiceId, string stageId, DateTime? startDate, DateTime? endDate)
        {

            var goals = await _performanceManagementQueryBusiness.GetAllStageCompetencies(ownerUserId, docServiceId, stageId, startDate, endDate);
            return goals;
        }
        public async Task<List<TeamWorkloadViewModel>> GetAllStageCompetencies(string ownerUserId, string docServiceId, string stageId, DateTime? startDate, DateTime? endDate)
        {
            var goals = await _performanceManagementQueryBusiness.GetAllStageCompetencies(ownerUserId, docServiceId, stageId, startDate, endDate);
            return goals;
        }

        public async Task<List<ServiceViewModel>> GetPerformanceDocumentServiceOwners(string docMasterId, string users)
        {
            var queryData = await _performanceManagementQueryBusiness.GetPerformanceDocumentServiceOwners(docMasterId, users);
            return queryData;
        }


        public async Task<CommandResult<PerformanceDocumentViewModel>> PublishDocumentMaster(string pdmId)
        {
            var model = await GetPerformanceDocumentDetails(pdmId);
            if (model.IsNotNull())
            {
                var result = await UpdatePerformanceDocumentMasterStatus(model.Id, PerformanceDocumentStatusEnum.Publishing);
                if (result.IsTrue())
                {
                    //await GeneratePerformanceDocument(pdmId);
                    var hangfireScheduler = _serviceProvider.GetService<IHangfireScheduler>();
                    await hangfireScheduler.Enqueue<HangfireScheduler>(x => x.GeneratePerformanceDocument(pdmId, _userContext.ToIdentityUser()));
                }
                return CommandResult<PerformanceDocumentViewModel>.Instance(model);
            }

            return CommandResult<PerformanceDocumentViewModel>.Instance(model, false, "Performance Document Does Not Available");
        }

        public async Task<bool> GeneratePerformanceDocumentStage(string docid, string userId, string parentServiceId)
        {
            var documentstage = await GetPerformanceDocumentStageData(docid, null, null);
            documentstage = documentstage.Where(x => x.DocumentStageStatus == PerformanceDocumentStatusEnum.Active && x.StartDate <= DateTime.Today).OrderBy(x => x.StartDate).ToList();
            foreach (var stage in documentstage)
            {
                var existingstage = await GetPerformanceDocumentStageByMaster(parentServiceId, stage.Id);
                if (existingstage == null)
                {
                    var serviceStageTempModel = new ServiceTemplateViewModel();
                    var performanceDocumentStageView = new PerformanceDocumentStageViewModel();
                    // var udfdic = new Dictionary<string, string>();
                    performanceDocumentStageView.Name = stage.Name;
                    performanceDocumentStageView.Description = stage.Description;
                    performanceDocumentStageView.StartDate = stage.StartDate;
                    performanceDocumentStageView.EndDate = stage.EndDate;
                    performanceDocumentStageView.Year = stage.Year;
                    performanceDocumentStageView.DocumentStageStatus = stage.DocumentStageStatus;
                    // performanceDocumentStageView.StageLinkId = stage.StageLinkId;
                    performanceDocumentStageView.DocumentMasterStageId = stage.Id;
                    //  udfdic.Add("Name",stage.Name);
                    // serviceStageTempModel.Udfs = udfdic;
                    serviceStageTempModel.ActiveUserId = _userContext.UserId;
                    serviceStageTempModel.TemplateCode = "PMS_PERFORMANCE_DOCUMENT_STAGE";
                    var serviceStagemodel = await _serviceBusiness.GetServiceDetails(serviceStageTempModel);
                    serviceStagemodel.ParentServiceId = parentServiceId;
                    serviceStagemodel.OwnerUserId = userId;
                    serviceStagemodel.DataAction = DataActionEnum.Create;
                    serviceStagemodel.ServiceStatusCode = "SERVICE_STATUS_INPROGRESS";
                    serviceStagemodel.ServiceSubject = stage.Name;
                    serviceStagemodel.ServiceDescription = stage.Description;
                    serviceStagemodel.Json = JsonConvert.SerializeObject(performanceDocumentStageView);
                    var stagecreate = await _serviceBusiness.ManageService(serviceStagemodel);
                    if (!stagecreate.IsSuccess)
                    {
                        return false;
                    }
                }

            }
            return true;
        }

        public async Task<CommandResult<PerformanceDocumentViewModel>> GeneratePerformanceDocument(string pdmId)
        {
            var model = await GetPerformanceDocumentDetails(pdmId);
            var mappedusers = "";
            if (model.IsNotNull())

            {
                var users = await GetPerformanceDocumentMappedUserData(model.NoteId, null);
                model.DocumentMasterId = model.Id;
                foreach (var user in users)
                {
                    // var exisiting = await _tableMetadataBusiness.GetTableDataByColumn("PMS_PERFORMANCE_DOCUMENT", null, "DocumentMasterId", model.Id);
                    mappedusers += user.OwnerUserId + "','";
                    var exisiting = await GetPerformanceDocumentByMaster(user.OwnerUserId, model.Id);
                    if (exisiting == null)
                    {


                        var serviceTempModel = new ServiceTemplateViewModel();


                        serviceTempModel.ActiveUserId = _userContext.UserId;
                        serviceTempModel.TemplateCode = "PMS_PERFORMANCE_DOCUMENT";
                        var servicemodel = await _serviceBusiness.GetServiceDetails(serviceTempModel);
                        servicemodel.OwnerUserId = user.OwnerUserId;
                        servicemodel.DataAction = DataActionEnum.Create;
                        servicemodel.ServiceStatusCode = "SERVICE_STATUS_INPROGRESS";
                        servicemodel.ServiceSubject = model.Name;
                        servicemodel.ServiceDescription = model.Description;
                        servicemodel.Json = JsonConvert.SerializeObject(model);
                        var servicecreate = await _serviceBusiness.ManageService(servicemodel);
                        if (servicecreate.IsSuccess)
                        {
                            //bool stagecreation = await GeneratePerformanceDocumentStage(model.NoteId, user.OwnerUserId, servicecreate.Item.ServiceId);
                            //if (!stagecreation)
                            //{
                            //    return CommandResult<PerformanceDocumentViewModel>.Instance(model, false, "Cannot Generate Stage Document");
                            //}
                        }
                    }

                    else
                    {
                        //bool stagecreation = await GeneratePerformanceDocumentStage(model.NoteId, user.OwnerUserId, exisiting.ServiceId);
                        //if (!stagecreation)
                        //{
                        //    return CommandResult<PerformanceDocumentViewModel>.Instance(model, false, "Cannot Generate Stage Document");
                        //}
                    }
                }
                mappedusers = mappedusers.TrimEnd(',');
                var servicecreatedusers = await GetPerformanceDocumentServiceOwners(model.Id, mappedusers);
                if (servicecreatedusers.Count > 0)
                {
                    foreach (var createdusers in servicecreatedusers)
                    {
                        var serviceTempModel = new ServiceTemplateViewModel();
                        serviceTempModel.ActiveUserId = _userContext.UserId;
                        serviceTempModel.DataAction = DataActionEnum.Edit;
                        serviceTempModel.ServiceId = createdusers.Id;
                        var servicemodel = await _serviceBusiness.GetServiceDetails(serviceTempModel);
                        servicemodel.OwnerUserId = createdusers.OwnerUserId;
                        servicemodel.ServiceStatusCode = "SERVICE_STATUS_CANCEL";
                        servicemodel.ServiceSubject = model.Name;
                        servicemodel.ServiceDescription = model.Description;
                        servicemodel.Json = JsonConvert.SerializeObject(model);
                        var serviceedited = await _serviceBusiness.ManageService(servicemodel);
                        if (!serviceedited.IsSuccess)
                        {
                            return CommandResult<PerformanceDocumentViewModel>.Instance(model);
                        }
                    }

                }


                await UpdatePerformanceDocumentMasterStatus(model.Id, PerformanceDocumentStatusEnum.Active);

            }
            return CommandResult<PerformanceDocumentViewModel>.Instance(model, true, "Success");
        }


        public async Task updateGoalWeightaged(string Id, string Weightage)
        {

            await _performanceManagementQueryBusiness.updateGoalWeightaged(Id, Weightage);

        }

        public async Task updateCompentancyWeightaged(string Id, string Weightage)
        {
            await _performanceManagementQueryBusiness.updateCompentancyWeightaged(Id, Weightage);

        }

        public async Task updateGoalRating(string Id, string RatingId, string type)
        {

            await _performanceManagementQueryBusiness.updateGoalRating(Id, RatingId, type);
        }

        public async Task updateCompentancyRating(string Id, string RatingId, string type)
        {
            await _performanceManagementQueryBusiness.updateCompentancyRating(Id, RatingId, type);

        }
        public async Task<IList<ServiceViewModel>> ReadPerformanceDocumentGoalData(string performanceId, string userId, string masterStageId)
        {
            var queryData = await _performanceManagementQueryBusiness.ReadPerformanceDocumentGoalData(performanceId, userId, masterStageId);
            return queryData;
        }
        public async Task<IList<ServiceViewModel>> ReadPerformanceDocumentCompetencyData(string performanceId, string userId, string masterStageId)
        {
            var queryData = await _performanceManagementQueryBusiness.ReadPerformanceDocumentCompetencyData(performanceId, userId, masterStageId);
            return queryData;
        }

        public async Task TriggerReviewGoal(ServiceTemplateViewModel viewModel)
        {
            try
            {
                DataRow stage = await _tableMetadataBusiness.GetTableDataByHeaderId(viewModel.TemplateId, viewModel.UdfNoteId);

                if (stage != null)
                {
                    string rowValue = stage["DocumentMasterStageId"].ToString();
                    var documentMaster = await GetPerformanceDocumentMasterById(rowValue);
                    var DocumentMasterData = await GetPerformanceDocumentMasterByNoteId(documentMaster.ParentNoteId);
                    //var master = await GetPerformanceDocumentMasterId(documentMaster.ParentNoteId);

                    var users = await GetPerformanceDocumentMappedUserData(documentMaster.ParentNoteId, null);

                    var masterList = await GetPerformanceGradeRatingData(documentMaster.ParentNoteId, null, null);
                    var master = masterList.FirstOrDefault();

                    var documentstagelist = await GetPerformanceDocumentStageData(documentMaster.ParentNoteId, null, rowValue);
                    var documentstage = documentstagelist.FirstOrDefault();
                    foreach (var user in users)
                    {

                        var goals = await _performanceManagementQueryBusiness.TriggerReviewGoal(user, viewModel);

                        // var linemanager = await _hrCoreBusiness.GetUserPerformanceDocumentInfo(user.OwnerUserId, viewModel.ParentServiceId);
                        // var line = linemanager.FirstOrDefault();
                        var line = await _hrCoreBusiness.GetUserLineManagerFromPerformanceHierarchy(user.OwnerUserId);

                        foreach (var goal in goals)
                        {
                            var existManagerTask = await _taskBusiness.GetSingle(x => x.ParentServiceId == goal.Id && x.TemplateId == documentstage.ManagerGoalStageTemplateId);

                            if (existManagerTask == null && line.IsNotNull() && line.ManagerUserId.IsNotNull())
                            {
                                var taskTempModel = new TaskTemplateViewModel();
                                var tasktemplatecode = await _templateBusiness.GetSingleById(documentstage.ManagerGoalStageTemplateId);
                                taskTempModel.TemplateCode = tasktemplatecode.Code;
                                var stepmodel = await _taskBusiness.GetTaskDetails(taskTempModel);
                                // for line manager line.ManagerUserId
                                stepmodel.AssignedToUserId = line.ManagerUserId;
                                stepmodel.OwnerUserId = viewModel.RequestedByUserId;
                                stepmodel.StartDate = viewModel.StartDate;
                                stepmodel.DueDate = viewModel.DueDate;
                                stepmodel.DataAction = DataActionEnum.Create;
                                stepmodel.ParentServiceId = goal.Id;
                                stepmodel.TaskSubject = stepmodel.TaskSubject.IsNotNullAndNotEmpty() ? stepmodel.TaskSubject : tasktemplatecode.DisplayName;
                                stepmodel.TaskStatusCode = "TASK_STATUS_INPROGRESS";
                                //stepmodel.Json = "{}";
                                dynamic exo = new System.Dynamic.ExpandoObject();
                                if (master.IsNotNull())
                                {
                                    ((IDictionary<String, Object>)exo).Add("RatingNoteId", master.NoteId);
                                }
                                ((IDictionary<String, Object>)exo).Add("DocumentMasterId", DocumentMasterData.Id);
                                ((IDictionary<String, Object>)exo).Add("DocumentMasterStageId", rowValue);
                                stepmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                                await _taskBusiness.ManageTask(stepmodel);

                            }

                            var existEmpTask = await _taskBusiness.GetSingle(x => x.ParentServiceId == goal.Id && x.TemplateId == documentstage.EmployeeGoalStageTemplateId);

                            if (existEmpTask == null /*&& line.IsNotNull()*/)
                            {
                                var taskTempModel = new TaskTemplateViewModel();
                                var tasktemplatecode = await _templateBusiness.GetSingleById(documentstage.EmployeeGoalStageTemplateId);
                                taskTempModel.TemplateCode = tasktemplatecode.Code;
                                var stepmodel = await _taskBusiness.GetTaskDetails(taskTempModel);
                                // for line manager line.ManagerUserId
                                stepmodel.AssignedToUserId = user.OwnerUserId;
                                stepmodel.OwnerUserId = viewModel.RequestedByUserId;
                                stepmodel.StartDate = viewModel.StartDate;
                                stepmodel.DueDate = viewModel.DueDate;
                                stepmodel.DataAction = DataActionEnum.Create;
                                stepmodel.ParentServiceId = goal.Id;
                                stepmodel.TaskSubject = stepmodel.TaskSubject.IsNotNullAndNotEmpty() ? stepmodel.TaskSubject : tasktemplatecode.DisplayName;
                                stepmodel.TaskStatusCode = "TASK_STATUS_INPROGRESS";
                                //stepmodel.Json = "{}";
                                dynamic exo = new System.Dynamic.ExpandoObject();
                                if (master.IsNotNull())
                                {
                                    ((IDictionary<String, Object>)exo).Add("RatingNoteId", master.NoteId);
                                }
                                ((IDictionary<String, Object>)exo).Add("DocumentMasterId", DocumentMasterData.Id);
                                ((IDictionary<String, Object>)exo).Add("DocumentMasterStageId", rowValue);
                                stepmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                                await _taskBusiness.ManageTask(stepmodel);
                            }


                        }

                        //Trigger Compentency




                        var compentency = await _performanceManagementQueryBusiness.TriggerReviewGoalData(user, viewModel);


                        foreach (var comp in compentency)
                        {
                            var existManagerTask = await _taskBusiness.GetSingle(x => x.ParentServiceId == comp.Id && x.TemplateId == documentstage.ManagerCompetencyStageTemplateId);

                            if (existManagerTask == null && line.IsNotNull() && line.ManagerUserId.IsNotNull())
                            {
                                var taskTempModel = new TaskTemplateViewModel();
                                var tasktemplatecode = await _templateBusiness.GetSingleById(documentstage.ManagerCompetencyStageTemplateId);
                                taskTempModel.TemplateCode = tasktemplatecode.Code;
                                var stepmodel = await _taskBusiness.GetTaskDetails(taskTempModel);
                                // for line manager line.ManagerUserId
                                stepmodel.AssignedToUserId = line.ManagerUserId;
                                stepmodel.OwnerUserId = viewModel.RequestedByUserId;
                                stepmodel.StartDate = viewModel.StartDate;
                                stepmodel.DueDate = viewModel.DueDate;
                                stepmodel.DataAction = DataActionEnum.Create;
                                stepmodel.ParentServiceId = comp.Id;
                                stepmodel.TaskSubject = stepmodel.TaskSubject.IsNotNullAndNotEmpty() ? stepmodel.TaskSubject : tasktemplatecode.DisplayName;
                                stepmodel.TaskStatusCode = "TASK_STATUS_INPROGRESS";
                                //stepmodel.Json = "{}";
                                dynamic exo = new System.Dynamic.ExpandoObject();
                                if (master.IsNotNull())
                                {
                                    ((IDictionary<String, Object>)exo).Add("RatingNoteId", master.NoteId);
                                }
                                ((IDictionary<String, Object>)exo).Add("DocumentMasterId", DocumentMasterData.Id);
                                ((IDictionary<String, Object>)exo).Add("DocumentMasterStageId", rowValue);
                                stepmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                                await _taskBusiness.ManageTask(stepmodel);
                            }

                            var existEmpTask = await _taskBusiness.GetSingle(x => x.ParentServiceId == comp.Id && x.TemplateId == documentstage.EmployeeCompetencyStageTemplateId);

                            if (existEmpTask == null /*&& line.IsNotNull()*/)
                            {
                                var taskTempModel = new TaskTemplateViewModel();
                                var tasktemplatecode = await _templateBusiness.GetSingleById(documentstage.EmployeeCompetencyStageTemplateId);
                                taskTempModel.TemplateCode = tasktemplatecode.Code;
                                var stepmodel = await _taskBusiness.GetTaskDetails(taskTempModel);
                                // for line manager line.ManagerUserId
                                stepmodel.AssignedToUserId = user.OwnerUserId;
                                stepmodel.OwnerUserId = viewModel.RequestedByUserId;
                                stepmodel.StartDate = viewModel.StartDate;
                                stepmodel.DueDate = viewModel.DueDate;
                                stepmodel.DataAction = DataActionEnum.Create;
                                stepmodel.ParentServiceId = comp.Id;
                                stepmodel.TaskSubject = stepmodel.TaskSubject.IsNotNullAndNotEmpty() ? stepmodel.TaskSubject : tasktemplatecode.DisplayName;
                                stepmodel.TaskStatusCode = "TASK_STATUS_INPROGRESS";
                                //stepmodel.Json = "{}";
                                dynamic exo = new System.Dynamic.ExpandoObject();
                                if (master.IsNotNull())
                                {
                                    ((IDictionary<String, Object>)exo).Add("RatingNoteId", master.NoteId);
                                }
                                ((IDictionary<String, Object>)exo).Add("DocumentMasterId", DocumentMasterData.Id);
                                ((IDictionary<String, Object>)exo).Add("DocumentMasterStageId", rowValue);
                                stepmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                                await _taskBusiness.ManageTask(stepmodel);
                            }

                        }
                    }

                }

            }
            catch (Exception ex)
            {

            }
        }

        public async Task CalculatePerformanceRating(string documentMasterId, string masterStageId)
        {
            var performanceId = "";
            var stageId = "";
            var userId = "";

            await _performanceManagementQueryBusiness.CalculatePerformanceRatingData(documentMasterId, masterStageId, performanceId, stageId, userId);
        }

        public async Task<IList<PerformanceDocumentViewModel>> GetPerformanceFinalReport(string documentMasterId, string departmentId = null, string userId = null, string stageId = null)
        {

            var result = await _performanceManagementQueryBusiness.GetPerformanceDocumentDetailsData(documentMasterId, departmentId, userId, stageId);
            return result;
        }

        public async Task<IList<PerformanceDocumentViewModel>> GetPerformanceDocumentDetailsData(string DocumentMasterId, string deptId = null, string userId = null, string pdmStageId = null)
        {

            var queryData = await _performanceManagementQueryBusiness.GetPerformanceDocumentDetailsData(DocumentMasterId, deptId, userId, pdmStageId);
            foreach (var data in queryData)
            {
                var comment = "";
                if (data.TotalGoalWeightage != 100)
                {
                    comment = comment + "Total goal weightage not equal to 100.</br>";
                }
                if (data.TotalCompetencyWeightage != 100)
                {
                    comment = comment + "Total competency weightage not equal to 100.</br>";
                }
                if (data.TotalGoal.ToSafeInt() != data.TotalGoalCompleted.ToSafeInt())
                {
                    comment = comment + "Complete/Approve all Goals.</br>";
                }
                if (data.TotalCompetency.ToSafeInt() != data.TotalCompetencyCompleted.ToSafeInt())
                {
                    comment = comment + "Complete/Approve all Competencies.</br>";
                }
                if (comment.IsNullOrEmpty())
                {
                    comment = "Ready";
                }
                data.ReadyForPerformanceRating = comment;
            }
            return queryData;
        }



        public async Task<PerformaceRatingViewModel> GetPerformanceRatingDetails(string Id)
        {
            var queryData = await _performanceManagementQueryBusiness.GetPerformanceRatingDetails(Id);
            return queryData;
        }
        public async Task<PerformanceRatingItemViewModel> GetPerformanceRatingItemDetails(string Id)
        {

            var queryData = await _performanceManagementQueryBusiness.GetPerformanceRatingItemDetails(Id);
            return queryData;
        }

        public async Task DeletePerformanceRating(string Id)
        {
            await _performanceManagementQueryBusiness.DeletePerformanceRating(Id);
        }


        public async Task DeletePerformanceRatingItem(string Id)
        {
            await _performanceManagementQueryBusiness.DeletePerformanceRatingItem(Id);
        }

        public async Task<List<PerformanceRatingItemViewModel>> GetPerformanceRatingItemList(string ParentNodeId)
        {
            var queryData = await _performanceManagementQueryBusiness.GetPerformanceRatingItemList(ParentNodeId);
            return queryData;
        }


        public async Task<List<PerformaceRatingViewModel>> GetPerformanceRatingList()
        {

            var queryData = await _performanceManagementQueryBusiness.GetPerformanceRatingList();
            return queryData;
        }

        public async Task<PerformanceRatingItemViewModel> IsRatingItemExist(string Parentid, string Name, string Id)
        {

            var queryData = await _performanceManagementQueryBusiness.IsRatingItemExist(Parentid, Name, Id);
            return queryData;
        }

        public async Task<PerformanceRatingItemViewModel> IsRatingItemCodeExist(string Parentid, string code, string Id)
        {
            var queryData = await _performanceManagementQueryBusiness.IsRatingItemCodeExist(Parentid, code, Id);
            return queryData;
        }


        public async Task<PerformaceRatingViewModel> IsRatingNameExist(string Name, string Id)
        {
            var queryData = await _performanceManagementQueryBusiness.IsRatingNameExist(Name, Id);
            return queryData;
        }


        public async Task<List<CompetencyCategoryViewModel>> GetPerformanceTaskCompetencyCategory()
        {
            var queryData = await _performanceManagementQueryBusiness.GetPerformanceTaskCompetencyCategory();
            return queryData;
        }
        public async Task<List<CompetencyViewModel>> GetPerformanceTaskCompetencyMaster(string templateCode, string categoryCode)
        {

            var queryData = await _performanceManagementQueryBusiness.GetPerformanceTaskCompetencyMaster(templateCode, categoryCode);
            return queryData;
        }
        public async Task<CompetencyCategoryViewModel> IsCompetencyCategoryNameExist(string Name, string Id)
        {
            var queryData = await _performanceManagementQueryBusiness.IsCompetencyCategoryNameExist(Name, Id);
            return queryData;
        }

        public async Task<CompetencyCategoryViewModel> IsCompetencyCategoryCodeExist(string Code, string Id)
        {
            var queryData = await _performanceManagementQueryBusiness.IsCompetencyCategoryCodeExist(Code, Id);
            return queryData;
        }


        public async Task<CompetencyCategoryViewModel> GetcompetencyCategoryDetails(string Id)
        {

            var queryData = await _performanceManagementQueryBusiness.GetcompetencyCategoryDetails(Id);
            return queryData;
        }

        public async Task<List<IdNameViewModel>> GetParentCompatencyCategory()
        {

            var queryData = await _performanceManagementQueryBusiness.GetParentCompatencyCategory();
            var list = new List<IdNameViewModel>();
            ////  var list1 = queryData.GroupBy(x => x.NtsStatus).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.NtsStatusId).FirstOrDefault() }).ToList();
            list = queryData.Select(x => new IdNameViewModel { Id = x.Id, Name = x.Name }).ToList();
            return queryData;
        }


        public async Task<CompetencyCategoryViewModel> IsParentAssignToCompetencyCategoryExist(string ParentId, string Id)
        {


            // var where = "";
            // if (Id.IsNotNullAndNotEmpty())
            // {
            //     where = $@" and N.""Id"" !='{Id}' ";
            // }
            // query = query.Replace("#IdWhere#", where);
            var queryData = await _performanceManagementQueryBusiness.IsParentAssignToCompetencyCategoryExist(ParentId, Id);
            return queryData;
        }


        public async Task DeleteCompetencyCategory(string Id)
        {

            await _performanceManagementQueryBusiness.DeleteCompetencyCategory(Id);
        }
        public async Task DeleteCompetency(string Id)
        {
            await _performanceManagementQueryBusiness.DeleteCompetency(Id);
        }
        public async Task DeleteGoal(string Id)
        {
            await _performanceManagementQueryBusiness.DeleteGoal(Id);
        }
        public async Task DeleteDevelopment(string Id)
        {
            await _performanceManagementQueryBusiness.DeleteDevelopment(Id);
        }
        public async Task DeletService(string Id)
        {
            await _performanceManagementQueryBusiness.DeletService(Id);
        }
        public async Task<IList<CompetencyCategoryViewModel>> GetCompetencyMaster()
        {

            var queryData = await _performanceManagementQueryBusiness.GetCompetencyMaster();
            return queryData;
        }

        public async Task<IList<CompetencyCategoryViewModel>> ReadCompetencyMasterJob(string noteid)
        {
            var queryData = await _performanceManagementQueryBusiness.ReadCompetencyMasterJob(noteid);
            return queryData;
        }

        public async Task<IList<CompetencyViewModel>> GetCompetencyData(string parentNoteId, string udfNoteId = null, string noteId = null)
        {
            var queryData = await _performanceManagementQueryBusiness.GetCompetencyData(parentNoteId, udfNoteId, noteId);
            return queryData;
        }

        public async Task<CommandResult<CompetencyViewModel>> CreateComp(CompetencyViewModel model)
        {
            var validateName = await IsCompNameExists(model);
            if (!validateName.IsSuccess)
            {
                return CommandResult<CompetencyViewModel>.Instance(model, false, validateName.Message);
            }

            var noteTempModel = new NoteTemplateViewModel();
            noteTempModel.DataAction = model.DataAction;
            noteTempModel.ActiveUserId = _userContext.UserId;
            noteTempModel.TemplateCode = "COMPETENCY_MASTER";
            noteTempModel.ParentNoteId = model.ParentNoteId;
            var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);

            notemodel.Json = JsonConvert.SerializeObject(model);
            notemodel.NoteStatusCode = "NOTE_STATUS_COMPLETE";

            var result = await _noteBusiness.ManageNote(notemodel);
            if (result.IsSuccess)
            {
                return CommandResult<CompetencyViewModel>.Instance(model, result.IsSuccess, result.Messages);
            }
            return CommandResult<CompetencyViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async Task<CommandResult<CompetencyViewModel>> EditComp(CompetencyViewModel model)
        {
            var validateName = await IsCompNameExists(model);
            if (!validateName.IsSuccess)
            {
                return CommandResult<CompetencyViewModel>.Instance(model, false, validateName.Message);
            }

            var noteTempModel = new NoteTemplateViewModel();
            noteTempModel.DataAction = DataActionEnum.Edit;
            noteTempModel.ActiveUserId = _userContext.UserId;
            noteTempModel.NoteId = model.NoteId;
            var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);

            notemodel.Json = JsonConvert.SerializeObject(model);
            var result = await _noteBusiness.ManageNote(notemodel);
            if (result.IsSuccess)
            {
                return CommandResult<CompetencyViewModel>.Instance(model, result.IsSuccess, result.Messages);
            }
            return CommandResult<CompetencyViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        public async Task<bool> DeleteComp(string Id)
        {

            await _performanceManagementQueryBusiness.DeleteComp(Id);
            return true;
        }

        public async Task<CommandResult<CompetencyViewModel>> IsCompNameExists(CompetencyViewModel model)
        {
            var ratings = await GetCompetencyData(model.ParentNoteId);
            var nameexist = ratings.Where(x => x.CompetencyName == model.CompetencyName && x.Id != model.Id);
            if (nameexist.Count() > 0)
            {
                return CommandResult<CompetencyViewModel>.Instance(model, false, "Name Already Exist");
            }
            var codeexist = ratings.Where(x => x.CompetencyCode == model.CompetencyCode && x.Id != model.Id);
            if (codeexist.Count() > 0)
            {
                return CommandResult<CompetencyViewModel>.Instance(model, false, "Code Already Exist");
            }

            return CommandResult<CompetencyViewModel>.Instance(model, true, "");
        }

        public async Task<List<CompetencyCategoryViewModel>> GetCompotencyDetails()
        {


            // var where = "";
            // if (Id.IsNotNullAndNotEmpty())
            // {
            //     where = $@" and N.""Id"" !='{Id}' ";
            // }
            // query = query.Replace("#IdWhere#", where);
            var queryData = await _performanceManagementQueryBusiness.GetCompotencyDetails();
            return queryData;
        }

        public async Task<List<IdNameViewModel>> GetAllPerformanceDocument()
        {

            // var where = "";
            // if (Id.IsNotNullAndNotEmpty())
            // {
            //     where = $@" and N.""Id"" !='{Id}' ";
            // }
            // query = query.Replace("#IdWhere#", where);
            var queryData = await _performanceManagementQueryBusiness.GetAllPerformanceDocument();
            return queryData;
        }

        public async Task<List<IdNameViewModel>> GetAllDepartment()
        {


            // var where = "";
            // if (Id.IsNotNullAndNotEmpty())
            // {
            //     where = $@" and N.""Id"" !='{Id}' ";
            // }
            // query = query.Replace("#IdWhere#", where);
            var queryData = await _performanceManagementQueryBusiness.GetAllDepartment();
            return queryData;
        }

        public async Task<IList<PerformanceDashboardViewModel>> GetPerformanceSummaryData(string filter)
        {
            var finalres = new List<PerformanceDashboardViewModel>();

            var userhier = await _userHierBusiness.GetPerformanceHierarchyUsers(_userContext.UserId);
            var queryData = await _performanceManagementQueryBusiness.GetPerformanceSummaryData(filter);

            if (_userContext.UserRoleCodes.IsNotNull() && _userContext.UserRoleCodes.Contains("PERFORMANCE_MANAGER"))
            {

            }
            else if (filter == "ALL")
            {
                var ids = new List<string>();
                ids = userhier.Select(x => x.UserId).Distinct().ToList();
                queryData = queryData.Where(x => ids.Contains(x.UserId)).ToList();
            }
            else if (filter == "DIRECT")
            {
                var ids = new List<string>();
                ids = userhier.Where(x => x.Type == "DIRECT").Select(x => x.UserId).Distinct().ToList();
                queryData = queryData.Where(x => ids.Contains(x.UserId)).ToList();
            }
            else if (filter == "INDIRECT")
            {
                var ids = new List<string>();
                ids = userhier.Where(x => x.Type == "INDIRECT").Select(x => x.UserId).Distinct().ToList();
                queryData = queryData.Where(x => ids.Contains(x.UserId)).ToList();
            }
            else
            {
                queryData = new List<PerformanceDashboardViewModel>();
            }



            var result = queryData.Select(x => x.Name).Distinct();
            foreach (var item in result)
            {
                var data = queryData.Where(x => x.Name == item).ToList();
                var performance = new PerformanceDashboardViewModel { Name = item };
                performance.InProgreessCount = data.Where(x => x.Status == "SERVICE_STATUS_INPROGRESS").GroupBy(x => x.UserId).Count();
                performance.OverDueCount = data.Where(x => x.Status == "SERVICE_STATUS_OVERDUE").GroupBy(x => x.UserId).Count();
                performance.CompletedCount = data.Where(x => x.Status == "SERVICE_STATUS_COMPLETE").GroupBy(x => x.UserId).Count();
                performance.CancelledCount = data.Where(x => x.Status == "SERVICE_STATUS_CANCEL").GroupBy(x => x.UserId).Count();
                performance.TotalCount = data.GroupBy(x => x.UserId).Count();
                finalres.Add(performance);
            }

            return finalres;
        }
        public async Task<IList<ServiceViewModel>> GetPerformanceSummaryDetail(string filter, string status, string service)
        {

            var queryData = await _performanceManagementQueryBusiness.GetPerformanceSummaryDetail(filter, status, service);
            var userhier = await _userHierBusiness.GetPerformanceHierarchyUsers(_userContext.UserId);
            if (_userContext.UserRoleCodes.IsNotNull() && _userContext.UserRoleCodes.Contains("PERFORMANCE_MANAGER"))
            {

            }
            else if (filter == "ALL")
            {
                var ids = userhier.Select(x => x.UserId).Distinct();
                queryData = queryData.Where(x => ids.Contains(x.OwnerUserId)).ToList();
            }
            else if (filter == "DIRECT")
            {
                var ids = userhier.Where(x => x.Type == "DIRECT").Select(x => x.UserId).Distinct();
                queryData = queryData.Where(x => ids.Contains(x.OwnerUserId)).ToList();
            }
            else if (filter == "INDIRECT")
            {
                var ids = userhier.Where(x => x.Type == "INDIRECT").Select(x => x.UserId).Distinct();
                queryData = queryData.Where(x => ids.Contains(x.OwnerUserId)).ToList();
            }
            else
            {
                queryData = new List<ServiceViewModel>();
            }

            return queryData;
        }


        public async Task<List<IdNameViewModel>> GetYearByUserId(string userId)
        {
            var queryData = await _performanceManagementQueryBusiness.GetYearByUserId(userId);
            return queryData;
        }


        public Task GetInboxMenuItem(string id, string userRoleCodes)
        {
            throw new NotImplementedException();
        }

        public Task GetInboxMenuItemByUser(string id, string userRoleCodes)
        {
            throw new NotImplementedException();
        }

        public async Task<List<IdNameViewModel>> GetRatingDetailsFromDocumentMaster(string Id)
        {
            var data = await _performanceManagementQueryBusiness.GetRatingDetailsFromDocumentMaster(Id);
            return data;
        }
        public async Task<List<IdNameViewModel>> GetParentGoalByDepartment(string departmentId)
        {
            var result = new List<IdNameViewModel>();
            var parent = await _hrCoreBusiness.GetParentOrgByOrg(departmentId);
            if (parent != null)
            {

                result = await _performanceManagementQueryBusiness.GetParentGoalByDepartment(departmentId);
            }
            return result;
        }
        public async Task<string> GetParentGoal(string goalId)
        {

            var result = await _performanceManagementQueryBusiness.GetParentGoal(goalId);

            var goals = result.Select(x => x.GoalName);
            var parentGoal = "";
            if (goals.Count() > 0)
            {
                parentGoal = string.Join(" >>", goals);
            }

            return parentGoal;
        }

        public async Task<List<IdNameViewModel>> GetDepartmentGoal(string departmentId)
        {

            var result = await _performanceManagementQueryBusiness.GetDepartmentGoal(departmentId);

            return result;
        }
        public async Task<List<IdNameViewModel>> GetPerformanceMasterByDepatment(string departmentId, string year)
        {

            var data = await _performanceManagementQueryBusiness.GetPerformanceMasterByDepatment(departmentId, year);

            return data;
        }

        public async Task<List<IdNameViewModel>> GetYearByDepartment(string departmentId)
        {
            var data = await _performanceManagementQueryBusiness.GetYearByDepartment(departmentId);

            return data;
        }

        public async Task<List<GoalViewModel>> GetDepartmentGoalByDepartment(string departmentId, string masterId)
        {

            var result = await _performanceManagementQueryBusiness.GetDepartmentGoalByDepartment(departmentId, masterId);

            return result;
        }
        public async Task<List<IdNameViewModel>> GetDepartmentBasedOnUser()
        {
            var result = new List<IdNameViewModel>();
            if (_userContext.IsSystemAdmin || _userContext.UserRoleCodes.Contains("PERFORMANCE_MANAGER"))
            {
                result = await _hrCoreBusiness.GetAllOrganisation();
            }
            else
            {

                result = await _performanceManagementQueryBusiness.GetDepartmentBasedOnUser();
            }

            return result;
        }
        public async Task TriggerAdhocTasksGoals(ServiceTemplateViewModel viewModel)
        {
            try
            {

                var DocumentMasterData = await GetPerformanceDocumentMasterByDocServiceId(viewModel.ParentServiceId);
                var documentstageList = await GetPerformanceDocumentStageData(DocumentMasterData.NoteId, null, null);
                var stage = documentstageList.Where(x => x.DocumentStageStatus == PerformanceDocumentStatusEnum.Active && x.StartDate <= DateTime.Today).OrderBy(x => x.StartDate).FirstOrDefault();
                if (stage != null)
                {
                    string rowValue = stage.Id;//stage["DocumentMasterStageId"].ToString();                    
                    var masterList = await GetPerformanceGradeRatingData(DocumentMasterData.NoteId, null, null);
                    var master = masterList.FirstOrDefault();

                    //var linemanager = await _hrCoreBusiness.GetUserPerformanceDocumentInfo(viewModel.OwnerUserId, viewModel.ParentServiceId);
                    //var line = linemanager.FirstOrDefault();
                    var line = await _hrCoreBusiness.GetUserLineManagerFromPerformanceHierarchy(viewModel.OwnerUserId);
                    var existManagerTask = await _taskBusiness.GetSingle(x => x.ParentServiceId == viewModel.ServiceId && x.TemplateId == stage.ManagerGoalStageTemplateId);
                    if (existManagerTask == null && line.IsNotNull() && line.ManagerUserId.IsNotNull())
                    {
                        var taskTempModel = new TaskTemplateViewModel();
                        var tasktemplatecode = await _templateBusiness.GetSingleById(stage.ManagerGoalStageTemplateId);
                        taskTempModel.TemplateCode = tasktemplatecode.Code;
                        var stepmodel = await _taskBusiness.GetTaskDetails(taskTempModel);
                        // for line manager line.ManagerUserId
                        stepmodel.AssignedToUserId = line.ManagerUserId;
                        stepmodel.OwnerUserId = viewModel.RequestedByUserId;
                        stepmodel.StartDate = viewModel.StartDate;
                        stepmodel.DueDate = viewModel.DueDate;
                        stepmodel.DataAction = DataActionEnum.Create;
                        stepmodel.ParentServiceId = viewModel.ServiceId;
                        stepmodel.TaskSubject = stepmodel.TaskSubject.IsNotNullAndNotEmpty() ? stepmodel.TaskSubject : tasktemplatecode.DisplayName;
                        stepmodel.TaskStatusCode = "TASK_STATUS_INPROGRESS";
                        //stepmodel.Json = "{}";
                        dynamic exo = new System.Dynamic.ExpandoObject();
                        if (master.IsNotNull())
                        {
                            ((IDictionary<String, Object>)exo).Add("RatingNoteId", master.NoteId);
                        }
                        ((IDictionary<String, Object>)exo).Add("DocumentMasterId", DocumentMasterData.Id);
                        ((IDictionary<String, Object>)exo).Add("DocumentMasterStageId", rowValue);
                        stepmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                        await _taskBusiness.ManageTask(stepmodel);

                    }

                    var existEmpTask = await _taskBusiness.GetSingle(x => x.ParentServiceId == viewModel.ServiceId && x.TemplateId == stage.EmployeeGoalStageTemplateId);
                    if (existEmpTask == null /*&& line.IsNotNull()*/)
                    {
                        var taskTempModel = new TaskTemplateViewModel();
                        var tasktemplatecode = await _templateBusiness.GetSingleById(stage.EmployeeGoalStageTemplateId);
                        taskTempModel.TemplateCode = tasktemplatecode.Code;
                        var stepmodel = await _taskBusiness.GetTaskDetails(taskTempModel);
                        // for line manager line.ManagerUserId
                        stepmodel.AssignedToUserId = viewModel.OwnerUserId;
                        stepmodel.OwnerUserId = viewModel.RequestedByUserId;
                        stepmodel.StartDate = viewModel.StartDate;
                        stepmodel.DueDate = viewModel.DueDate;
                        stepmodel.DataAction = DataActionEnum.Create;
                        stepmodel.ParentServiceId = viewModel.ServiceId;
                        stepmodel.TaskSubject = stepmodel.TaskSubject.IsNotNullAndNotEmpty() ? stepmodel.TaskSubject : tasktemplatecode.DisplayName;
                        stepmodel.TaskStatusCode = "TASK_STATUS_INPROGRESS";
                        //stepmodel.Json = "{}";
                        dynamic exo = new System.Dynamic.ExpandoObject();
                        if (master.IsNotNull())
                        {
                            ((IDictionary<String, Object>)exo).Add("RatingNoteId", master.NoteId);
                        }
                        ((IDictionary<String, Object>)exo).Add("DocumentMasterId", DocumentMasterData.Id);
                        ((IDictionary<String, Object>)exo).Add("DocumentMasterStageId", rowValue);
                        stepmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                        await _taskBusiness.ManageTask(stepmodel);
                    }


                }

            }
            catch (Exception ex)
            {

            }
        }
        public async Task TriggerAdhocTasksCompetency(ServiceTemplateViewModel viewModel)
        {
            try
            {
                var DocumentMasterData = await GetPerformanceDocumentMasterByDocServiceId(viewModel.ParentServiceId);
                var documentstageList = await GetPerformanceDocumentStageData(DocumentMasterData.NoteId, null, null);
                var stage = documentstageList.Where(x => x.DocumentStageStatus == PerformanceDocumentStatusEnum.Active && x.StartDate <= DateTime.Today).OrderBy(x => x.StartDate).FirstOrDefault();
                if (stage != null)
                {
                    string rowValue = stage.Id;
                    var masterList = await GetPerformanceGradeRatingData(DocumentMasterData.NoteId, null, null);
                    var master = masterList.FirstOrDefault();
                    //var documentstagelist = await GetPerformanceDocumentStageData(DocumentMasterData.NoteId, null, rowValue);
                    // var documentstage = documentstagelist.FirstOrDefault();
                    //var linemanager = await _hrCoreBusiness.GetUserPerformanceDocumentInfo(viewModel.OwnerUserId, viewModel.ParentServiceId);
                    //var line = linemanager.FirstOrDefault();
                    var line = await _hrCoreBusiness.GetUserLineManagerFromPerformanceHierarchy(viewModel.OwnerUserId);
                    //Trigger Compentency                   
                    var existManagerTask = await _taskBusiness.GetSingle(x => x.ParentServiceId == viewModel.ServiceId && x.TemplateId == stage.ManagerCompetencyStageTemplateId);
                    if (existManagerTask == null && line.IsNotNull() && line.ManagerUserId.IsNotNull())
                    {
                        var taskTempModel = new TaskTemplateViewModel();
                        var tasktemplatecode = await _templateBusiness.GetSingleById(stage.ManagerCompetencyStageTemplateId);
                        taskTempModel.TemplateCode = tasktemplatecode.Code;
                        var stepmodel = await _taskBusiness.GetTaskDetails(taskTempModel);
                        // for line manager line.ManagerUserId
                        stepmodel.AssignedToUserId = line.ManagerUserId;
                        stepmodel.OwnerUserId = viewModel.RequestedByUserId;
                        stepmodel.StartDate = viewModel.StartDate;
                        stepmodel.DueDate = viewModel.DueDate;
                        stepmodel.DataAction = DataActionEnum.Create;
                        stepmodel.ParentServiceId = viewModel.ServiceId;
                        stepmodel.TaskSubject = stepmodel.TaskSubject.IsNotNullAndNotEmpty() ? stepmodel.TaskSubject : tasktemplatecode.DisplayName;
                        stepmodel.TaskStatusCode = "TASK_STATUS_INPROGRESS";
                        //stepmodel.Json = "{}";
                        dynamic exo = new System.Dynamic.ExpandoObject();
                        if (master.IsNotNull())
                        {
                            ((IDictionary<String, Object>)exo).Add("RatingNoteId", master.NoteId);
                        }
                        ((IDictionary<String, Object>)exo).Add("DocumentMasterId", DocumentMasterData.Id);
                        ((IDictionary<String, Object>)exo).Add("DocumentMasterStageId", rowValue);
                        stepmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                        await _taskBusiness.ManageTask(stepmodel);
                    }

                    var existEmpTask = await _taskBusiness.GetSingle(x => x.ParentServiceId == viewModel.ServiceId && x.TemplateId == stage.EmployeeCompetencyStageTemplateId);
                    if (existEmpTask == null /*&& line.IsNotNull()*/)
                    {
                        var taskTempModel = new TaskTemplateViewModel();
                        var tasktemplatecode = await _templateBusiness.GetSingleById(stage.EmployeeCompetencyStageTemplateId);
                        taskTempModel.TemplateCode = tasktemplatecode.Code;
                        var stepmodel = await _taskBusiness.GetTaskDetails(taskTempModel);
                        // for line manager line.ManagerUserId
                        stepmodel.AssignedToUserId = viewModel.OwnerUserId;
                        stepmodel.OwnerUserId = viewModel.RequestedByUserId;
                        stepmodel.StartDate = viewModel.StartDate;
                        stepmodel.DueDate = viewModel.DueDate;
                        stepmodel.DataAction = DataActionEnum.Create;
                        stepmodel.ParentServiceId = viewModel.ServiceId;
                        stepmodel.TaskSubject = stepmodel.TaskSubject.IsNotNullAndNotEmpty() ? stepmodel.TaskSubject : tasktemplatecode.DisplayName;
                        stepmodel.TaskStatusCode = "TASK_STATUS_INPROGRESS";
                        //stepmodel.Json = "{}";
                        dynamic exo = new System.Dynamic.ExpandoObject();
                        if (master.IsNotNull())
                        {
                            ((IDictionary<String, Object>)exo).Add("RatingNoteId", master.NoteId);
                        }
                        ((IDictionary<String, Object>)exo).Add("DocumentMasterId", DocumentMasterData.Id);
                        ((IDictionary<String, Object>)exo).Add("DocumentMasterStageId", rowValue);
                        stepmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                        await _taskBusiness.ManageTask(stepmodel);
                    }
                }
            }
            catch (Exception ex)
            {

            }

        }


        public async Task<IList<IdNameViewModel>> GetDepartmentList()
        {
            var queryData = await _performanceManagementQueryBusiness.GetDepartmentList();
            return queryData;
        }

        public async Task MapDepartmentUser(NoteTemplateViewModel viewModel)
        {
            try
            {
                var udfData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(viewModel.Json);
                // Get All the  mapped users
                var existingUsers = await GetPerformanceDocumentMappedUserData(viewModel.NoteId, null);
                if (Convert.ToString(udfData["DepartmentId"]).IsNotNullAndNotEmpty())
                {
                    // Get All Users Of Department
                    var departmentUsers = await _hrCoreBusiness.GetUsersInfo(Convert.ToString(udfData["DepartmentId"]));
                    if (departmentUsers.Count > 0)
                    {
                        foreach (var user in departmentUsers)
                        {
                            if (existingUsers.Any(x => x.DepartmentId == Convert.ToString(udfData["DepartmentId"]) && x.OwnerUserId == user.UserId))
                            {
                                // department user already mapped to document Master
                                // Do Nothing
                            }
                            else
                            {
                                var noteTempModel = new NoteTemplateViewModel();
                                noteTempModel.DataAction = DataActionEnum.Create;
                                noteTempModel.ActiveUserId = _userContext.UserId;
                                noteTempModel.TemplateCode = "PERFORMANCE_DOCUMENT_MASTER_USERS";
                                noteTempModel.ParentNoteId = viewModel.NoteId;
                                var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                                notemodel.OwnerUserId = user.UserId;
                                notemodel.StartDate = DateTime.Now;
                                notemodel.NoteStatusCode = "NOTE_STATUS_COMPLETE";
                                //notemodel.Json = JsonConvert.SerializeObject(notemodel);
                                var result = await _noteBusiness.ManageNote(notemodel);
                            }
                        }
                    }
                }
                // remove All users which were mapped and now does not belong to this user
                var removeUserList = existingUsers.Where(x => x.DepartmentId != Convert.ToString(udfData["DepartmentId"]));
                if (removeUserList.IsNotNull())
                {
                    // existing user have another users
                    foreach (var user in removeUserList)
                    {
                        var deletemapuser = await _tableMetadataBusiness.DeleteTableDataByHeaderId("PERFORMANCE_DOCUMENT_MASTER_USERS", null, user.Id);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        public async Task<List<IdNameViewModel>> GetDepartmentListByOrganization(string organizationId)
        {
            var queryData = await _performanceManagementQueryBusiness.GetDepartmentListByOrganization(organizationId);
            return queryData;
        }

        public async Task<List<IdNameViewModel>> GetAllYearFromPerformanceMaster(string departmentId)
        {
            var queryData = await _performanceManagementQueryBusiness.GetAllYearFromPerformanceMaster(departmentId);
            return queryData;
        }

        public async Task<List<ServiceViewModel>> GetServiceListByPDMId(string pdmId, string templateCodes, string ownerUserId)
        {

            var result = await _performanceManagementQueryBusiness.GetServiceListByPDMId(pdmId, templateCodes, ownerUserId);
            return result;

        }
        public async Task TriggerReviewAdhocTasks(ServiceTemplateViewModel viewModel)
        {
            try
            {
                var EmployeeReviewTemplate = await _templateBusiness.GetSingle(x => x.Code == "PMS_EMPLOYEE_REVIEW");
                var ManagerReviewTemplate = await _templateBusiness.GetSingle(x => x.Code == "PMS_MANAGER_REVIEW");
                DataRow stage = await _tableMetadataBusiness.GetTableDataByHeaderId(viewModel.TemplateId, viewModel.UdfNoteId);
                if (stage != null)
                {
                    string rowValue = stage["DocumentMasterStageId"].ToString();
                    var documentMaster = await GetPerformanceDocumentMasterStageById(rowValue);
                    var DocumentMasterData = await GetPerformanceDocumentMasterByNoteId(documentMaster.ParentNoteId);
                    var documentstagelist = await GetPerformanceDocumentStageData(documentMaster.ParentNoteId, null, rowValue);
                    var documentstage = documentstagelist.FirstOrDefault();

                    var line = await _hrCoreBusiness.GetUserLineManagerFromPerformanceHierarchy(viewModel.OwnerUserId);
                    if (line.IsNotNull() && line.ManagerUserId.IsNotNullAndNotEmpty())
                    {
                        var existManagerTask = await _taskBusiness.GetSingle(x => x.ParentServiceId == viewModel.ServiceId && x.TemplateId == ManagerReviewTemplate.Id);
                        if (existManagerTask == null)
                        {
                            var taskTempModel = new TaskTemplateViewModel();
                            //var tasktemplatecode = await _templateBusiness.GetSingleById(documentstage.ManagerReviewTemplate);
                            taskTempModel.TemplateCode = ManagerReviewTemplate.Code;
                            var stepmodel = await _taskBusiness.GetTaskDetails(taskTempModel);
                            // for line manager line.ManagerUserId
                            stepmodel.AssignedToUserId = line.ManagerUserId;
                            stepmodel.OwnerUserId = viewModel.OwnerUserId;
                            stepmodel.RequestedByUserId = viewModel.RequestedByUserId;
                            stepmodel.StartDate = documentstage.ReviewStartDate;
                            stepmodel.DueDate = documentstage.EndDate;
                            stepmodel.AllowPastStartDate = true;
                            stepmodel.DataAction = DataActionEnum.Create;
                            stepmodel.ParentServiceId = viewModel.ServiceId;
                            stepmodel.TaskSubject = stepmodel.TaskSubject.IsNotNullAndNotEmpty() ? stepmodel.TaskSubject : ManagerReviewTemplate.DisplayName;
                            stepmodel.TaskStatusCode = "TASK_STATUS_INPROGRESS";
                            //stepmodel.Json = "{}";
                            dynamic exo = new System.Dynamic.ExpandoObject();
                            //if (master.IsNotNull())
                            //{
                            //    ((IDictionary<String, Object>)exo).Add("RatingNoteId", master.NoteId);
                            //}
                            if (viewModel.UdfNoteTableId.IsNotNull())
                            {
                                ((IDictionary<String, Object>)exo).Add("DocumentStageId", viewModel.UdfNoteTableId);
                            }
                           ((IDictionary<String, Object>)exo).Add("DocumentMasterId", DocumentMasterData.Id);
                            ((IDictionary<String, Object>)exo).Add("DocumentMasterStageId", rowValue);
                            stepmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                            await _taskBusiness.ManageTask(stepmodel);

                        }

                    }

                    var existEmpTask = await _taskBusiness.GetSingle(x => x.ParentServiceId == viewModel.ServiceId && x.TemplateId == EmployeeReviewTemplate.Id);
                    if (existEmpTask == null /*&& line.IsNotNull()*/)
                    {
                        var taskTempModel = new TaskTemplateViewModel();
                        //var tasktemplatecode = await _templateBusiness.GetSingleById(documentstage.EmployeeReviewTemplate);
                        taskTempModel.TemplateCode = EmployeeReviewTemplate.Code;
                        var stepmodel = await _taskBusiness.GetTaskDetails(taskTempModel);
                        // for line manager line.ManagerUserId
                        stepmodel.AssignedToUserId = viewModel.OwnerUserId;
                        stepmodel.OwnerUserId = viewModel.OwnerUserId;
                        stepmodel.RequestedByUserId = viewModel.RequestedByUserId;
                        stepmodel.StartDate = documentstage.ReviewStartDate;
                        stepmodel.DueDate = documentstage.EndDate;
                        stepmodel.AllowPastStartDate = true;
                        stepmodel.DataAction = DataActionEnum.Create;
                        stepmodel.ParentServiceId = viewModel.ServiceId;
                        stepmodel.TaskSubject = stepmodel.TaskSubject.IsNotNullAndNotEmpty() ? stepmodel.TaskSubject : EmployeeReviewTemplate.DisplayName;
                        stepmodel.TaskStatusCode = "TASK_STATUS_INPROGRESS";
                        //stepmodel.Json = "{}";
                        dynamic exo = new System.Dynamic.ExpandoObject();
                        //if (master.IsNotNull())
                        //{
                        //    ((IDictionary<String, Object>)exo).Add("RatingNoteId", master.NoteId);
                        //}
                        if (viewModel.UdfNoteTableId.IsNotNull())
                        {
                            ((IDictionary<String, Object>)exo).Add("DocumentStageId", viewModel.UdfNoteTableId);
                        }
                        ((IDictionary<String, Object>)exo).Add("DocumentMasterId", DocumentMasterData.Id);
                        ((IDictionary<String, Object>)exo).Add("DocumentMasterStageId", rowValue);
                        stepmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                        await _taskBusiness.ManageTask(stepmodel);
                    }


                }

            }
            catch (Exception ex)
            {

            }
        }

        public async Task<bool> GeneratePerformanceDocumentStages(string docMasterStageId)
        {
            var masterStage = await GetPerformanceDocumentMasterStageById(docMasterStageId);
            // update the status as publishing 
            // await UpdatePerformanceDocumentMasterStageStatus(masterStage.Id, PerformanceDocumentStatusEnum.Publishing);
            //var documentstage = await GetPerformanceDocumentStageData(masterStage.ParentNoteId, null, null);
            var users = await GetPerformanceDocumentMappedUserData(masterStage.ParentNoteId, null);
            foreach (var user in users)
            {
                var master = await GetPerformanceDocumentMasterByNoteId(masterStage.ParentNoteId);
                var document = await GetPerformanceDocumentByMaster(user.OwnerUserId, master.Id);
                var existingstage = await GetPerformanceDocumentStageByMaster(document.ServiceId, masterStage.Id);
                if (existingstage == null)
                {
                    var serviceStageTempModel = new ServiceTemplateViewModel();
                    var performanceDocumentStageView = new PerformanceDocumentStageViewModel();
                    // var udfdic = new Dictionary<string, string>();
                    performanceDocumentStageView.Name = masterStage.Name;
                    performanceDocumentStageView.Description = masterStage.Description;
                    performanceDocumentStageView.StartDate = masterStage.StartDate;
                    performanceDocumentStageView.EndDate = masterStage.EndDate;
                    performanceDocumentStageView.Year = masterStage.Year;
                    performanceDocumentStageView.DocumentStageStatus = masterStage.DocumentStageStatus;
                    // performanceDocumentStageView.StageLinkId = stage.StageLinkId;
                    performanceDocumentStageView.DocumentMasterStageId = masterStage.Id;
                    performanceDocumentStageView.DocumentMasterId = master.Id;
                    performanceDocumentStageView.DocumentId = document.Id;
                    //  udfdic.Add("Name",stage.Name);
                    // serviceStageTempModel.Udfs = udfdic;
                    serviceStageTempModel.ActiveUserId = _userContext.UserId;
                    serviceStageTempModel.TemplateCode = "PMS_PERFORMANCE_DOCUMENT_STAGE";
                    var serviceStagemodel = await _serviceBusiness.GetServiceDetails(serviceStageTempModel);
                    serviceStagemodel.ParentServiceId = document.ServiceId;
                    serviceStagemodel.OwnerUserId = user.OwnerUserId;
                    serviceStagemodel.DataAction = DataActionEnum.Create;
                    serviceStagemodel.ServiceStatusCode = "SERVICE_STATUS_INPROGRESS";
                    serviceStagemodel.ServiceSubject = masterStage.Name;
                    serviceStagemodel.ServiceDescription = masterStage.Description;
                    serviceStagemodel.Json = JsonConvert.SerializeObject(performanceDocumentStageView);
                    var stagecreate = await _serviceBusiness.ManageService(serviceStagemodel);
                    if (stagecreate.IsSuccess)
                    {
                        // await TriggerReviewAdhocTasks(stagecreate.Item);
                        existingstage = await GetPerformanceDocumentStageByMaster(document.ServiceId, masterStage.Id);

                    }
                    else
                    {
                        return false;
                    }

                }
                if (existingstage.IsNotNull())
                {
                    var serviceStageTempModel = new ServiceTemplateViewModel();
                    serviceStageTempModel.ServiceId = existingstage.ServiceId;
                    serviceStageTempModel.DataAction = DataActionEnum.Edit;
                    var serviceStagemodel = await _serviceBusiness.GetServiceDetails(serviceStageTempModel);
                    await TriggerReviewAdhocTasks(serviceStagemodel);
                }
            }

            // update status to Active
            // update the status as publishing 
            await UpdatePerformanceDocumentMasterStageStatus(masterStage.Id, PerformanceDocumentStatusEnum.Active);
            return true;
        }

        public Task<List<ProjectDashboardChartViewModel>> GetTaskStatus(string userId, string projectId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ServiceViewModel>> LoadWorkBooks(string userIds, string docId = null)
        {
            var task = await _performanceManagementQueryBusiness.GetPerformanceBookList(userIds, docId);

            foreach (var item in task)
            {
                item.key = item.Id;
                item.lazy = true;
                item.title = "";
                var flag = task.Any(x => x.Id == item.ParentServiceId);
                if (item.ParentServiceId.IsNotNull() && !flag)
                {
                    item.ParentServiceId = null;
                }
                //var list = await GetNextNoteSequenceNo(item.Id);
                //item.NextSequenceNo = list.Count + 1;                

            }
            return task.OrderBy(x => x.CreatedDate).ToList();
        }

        public async Task<ServiceTemplateViewModel> GetBookDetails(string serviceId)
        {
            var model = await _performanceManagementQueryBusiness.GetBookDetails(serviceId);
            model.BookItems = await GetBookList(serviceId, null, true);
            return model;
        }
        public async Task<List<NtsViewModel>> GetBookList(string serviceId, string templateId, bool includeitemDetails = false)
        {

            var list = await _performanceManagementQueryBusiness.GetBookList(serviceId, templateId, includeitemDetails);
            var root = list.FirstOrDefault(x => x.Level == 0);
            //if (list.Any(x => x.ItemType == ItemTypeEnum.StepTask))
            //{
            //    list.Add(new NtsViewModel
            //    {
            //        TemplateName = "Step Tasks",
            //        Subject = "Step Tasks",
            //        Level = 1,
            //        parentId = serviceId,
            //        SequenceOrder = -1,
            //        Id = "-1"
            //    });
            //}
            //list.Add(new NtsViewModel
            //{
            //    TemplateName = "Child Books",
            //    Subject = "Child Books",
            //    Level = 1,
            //    parentId = serviceId,
            //    SequenceOrder = -2,
            //    Id = "-2",
            //    ItemType = ItemTypeEnum.BookSection
            //});
            ProcessBookList(root, list, includeitemDetails);
            return list.OrderBy(x => x.Level).ThenBy(x => x.SequenceOrder).ThenBy(x => x.TemplateSequence).ThenBy(x => x.Sequence).ToList();
        }

        private void ProcessBookList(NtsViewModel root, List<NtsViewModel> list, bool includeitemDetails)
        {
            if (root != null)
            {
                var childs = list.Where(x => x.parentId == root.Id).OrderBy(x => x.CreatedDate);
                root.MaxChildSequence = 0;
                if (childs.Any())
                {
                    root.HasChild = true;
                    root.MaxChildSequence = childs.Count();
                    var i = 1;
                    foreach (var item in childs)
                    {
                        item.Level = root.Level + 1;
                        item.ItemNo = $"{root.ItemNo}.{i++}";
                        item.ParentNtsType = root.NtsType;
                        ProcessBookList(item, list, includeitemDetails);
                    }
                }
            }
        }
        public async Task<GoalViewModel> GetGoalDataById(string id)
        {

            return await _performanceManagementQueryBusiness.GetGoalDataById(id);

        }
        public async Task<CompetencyViewModel> GetCompetencyDataById(string id)
        {
            return await _performanceManagementQueryBusiness.GetCompetencyDataById(id);
        }
        public async Task<CompetencyViewModel> GetDevelopmentDataById(string id)
        {
            return await _performanceManagementQueryBusiness.GetCompetencyDataById(id);
        }

        public async Task<IList<NotificationViewModel>> GetNotificationsList(string refIds)
        {
            return await _performanceManagementQueryBusiness.GetNotificationsList(refIds);
        }
    }
}
