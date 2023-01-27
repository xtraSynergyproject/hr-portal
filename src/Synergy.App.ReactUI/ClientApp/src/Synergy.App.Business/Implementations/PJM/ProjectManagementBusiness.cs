using AutoMapper;
using Synergy.App.Common;
using Synergy.App.Common.Utilities;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using SpreadsheetLight;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace Synergy.App.Business
{
    public class ProjectManagementBusiness : BusinessBase<ServiceViewModel, NtsService>, IProjectManagementBusiness
    {
        private readonly IRepositoryQueryBase<ServiceViewModel> _queryRepo;
        private readonly IRepositoryQueryBase<IdNameViewModel> _queryRepo1;
        private readonly IRepositoryQueryBase<ProgramDashboardViewModel> _queryPDRepo;
        private readonly IRepositoryQueryBase<ProjectGanttTaskViewModel> _queryGantt;
        private readonly IRepositoryQueryBase<TeamWorkloadViewModel> _queryTWRepo;
        private readonly IRepositoryQueryBase<DashboardCalendarViewModel> _queryDCRepo;
        private readonly IRepositoryQueryBase<ProjectDashboardViewModel> _queryProjDashRepo;
        private readonly IRepositoryQueryBase<ProjectDashboardChartViewModel> _queryProjDashChartRepo;
        private readonly IRepositoryQueryBase<TaskViewModel> _queryTaskRepo;
        private readonly IRepositoryQueryBase<MailViewModel> _queryMailTaskRepo;
        private readonly ITaskBusiness _taskBusiness;
        private readonly IServiceBusiness _serviceBusiness;
        private readonly IProjectManagementQueryBusiness _projectManagementQueryBusiness;
        private readonly INtsTaskPrecedenceBusiness _ntsTaskPrecedenceBusiness;
        private readonly IUserHierarchyBusiness _userHierBusiness;
        IUserContext _userContext;
        private readonly IUserBusiness _userBusiness;
        public ProjectManagementBusiness(IRepositoryBase<ServiceViewModel, NtsService> repo, IRepositoryQueryBase<ServiceViewModel> queryRepo,
            IRepositoryQueryBase<ProgramDashboardViewModel> queryPDRepo,
            IRepositoryQueryBase<IdNameViewModel> queryRepo1,
            IRepositoryQueryBase<DashboardCalendarViewModel> queryDCRepo,
            IRepositoryQueryBase<ProjectGanttTaskViewModel> queryGantt,
             IRepositoryQueryBase<TeamWorkloadViewModel> queryTWRepo, IUserContext userContext,
             IRepositoryQueryBase<ProjectDashboardViewModel> queryProjDashRepo,
             IRepositoryQueryBase<ProjectDashboardChartViewModel> queryProjDashChartRepo,
             IRepositoryQueryBase<TaskViewModel> queryTaskRepo, INtsTaskPrecedenceBusiness ntsTaskPrecedenceBusiness,
            IMapper autoMapper, ITaskBusiness taskBusiness, IServiceBusiness serviceBusiness
            , IRepositoryQueryBase<MailViewModel> queryMailTaskRepo, IProjectManagementQueryBusiness projectManagementQueryBusiness
            , IUserBusiness userBusiness, IUserHierarchyBusiness userHierBusiness) : base(repo, autoMapper)
        {
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
            _ntsTaskPrecedenceBusiness = ntsTaskPrecedenceBusiness;
            _queryMailTaskRepo = queryMailTaskRepo;
            _userContext = userContext;
            _userBusiness = userBusiness;
            _userHierBusiness = userHierBusiness;
            _projectManagementQueryBusiness = projectManagementQueryBusiness;
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

        public async Task<IList<ProgramDashboardViewModel>> GetProjectData()
        {
            var queryData = await _projectManagementQueryBusiness.GetProjectData();
            return queryData;
        }

        public async Task<IList<ProgramDashboardViewModel>> GetProjectSharedData()
        {
            var queryData = await _projectManagementQueryBusiness.GetProjectSharedData();
            return queryData;
        }


        public async Task<IList<ProjectGanttTaskViewModel>> ReadWBSTimelineGanttChartData(string userId,string projectId,string userRole, List<string> projectIds = null, List<string> ownerIds = null, List<string> assigneeIds = null, List<string> tasksStatus = null, FilterColumnEnum? column = null, DateTime? startDate = null, DateTime? dueDate = null)
        {
            var queryData = await _projectManagementQueryBusiness.ReadWBSTimelineGanttChartData(userId, projectId, userRole, projectIds, ownerIds, assigneeIds, tasksStatus, column, startDate, dueDate);
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
            var query = "";
            if (id.IsNullOrEmpty())
            {
                var roles = userRoleIds.Split(",");
                var roleText = "";
                foreach (var i in roles)
                {
                    roleText += $"'{i}',";
                }
                roleText = roleText.Trim(',');

                var count = await _projectManagementQueryBusiness.GetInboxMenuItemcount(roleText, userId);

                var item = new TreeViewViewModel
                {
                    id = "INBOX",
                    Name = "Inbox",
                    DisplayName = "Inbox",
                    ParentId = null,
                    hasChildren = true,
                    expanded = true,
                    Type = "INBOX",
                    children = true,
                    text = $"Inbox ({count })",
                    parent = "#",
                    a_attr = new { data_id = "INBOX", data_type = "INBOX", data_name = $"Inbox ({count })", data_parentId = "#" },

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

                list = await _projectManagementQueryBusiness.GetInboxMenuItem(roleText, userId);
                //expanded -> type= userrole - from coming list find type as userRole
                // if found then find the item in list as selcted item id
                //make expanded true

                var obj = expObj.Where(x => x.Type == "PROJECTSTAGE").FirstOrDefault();
                if (obj.IsNotNull())
                {
                    var data = list.Where(x => x.id == obj.UserRoleId).FirstOrDefault();
                    if (data.IsNotNull())
                    {
                        data.expanded = true;
                    }
                }

            }
     //       else if (type == "USERROLE")
     //       {

     //           query = $@"Select usp.""InboxStageName"" ||' (' || nt.""Count""|| ')' as Name
     //           , usp.""InboxStageName"" as id, '{id}' as ParentId, 'PROJECTSTAGE' as Type,
     //           true as hasChildren, '{userRoleId}' as UserRoleId
     //           from public.""UserRole"" as ur
     //           join public.""UserRoleStageParent"" as usp on usp.""UserRoleId"" = ur.""Id"" and usp.""InboxCode""='PMT'
              
     //            left join(
     //               WITH RECURSIVE Nts AS(

     //            WITH RECURSIVE NtsService AS(
     //            SELECT s.""Id"",s.""Id"" as ""ServiceId"",usp.""InboxStageName""   FROM public.""NtsService"" as s
     //                    join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" 
					//	     join public.""UserRoleStageParent"" as usp on usp.""TemplateCode"" = tt.""Code"" and usp.""InboxCode""='PMT' 
					//	     where usp.""UserRoleId"" = '{userRoleId}' and s.""OwnerUserId""='{userId}'
					 
     //                   union all
     //                   SELECT s.""Id"", s.""Id"" as ""ServiceId"",ns.""InboxStageName"" FROM public.""NtsService"" as s
     //                   join NtsService ns on s.""ParentServiceId""=ns.""ServiceId""

     //                join public.""User"" as u on s.""OwnerUserId""=u.""Id""
                        
     //            )
     //            SELECT ""Id"",'Parent' as Level,""InboxStageName""  from NtsService


     //            union all

     //            select t.""Id"",'Child' as Level,nt.""InboxStageName"" 
     //                FROM  public.""NtsTask"" as t
     //                   join Nts as nt on t.""ParentServiceId"" =nt.""Id""  
	                             
     //               )
						
     //               SELECT count(""Id"") as ""Count"",""InboxStageName"" from Nts where Level='Child' group by ""InboxStageName""
					//) nt on usp.""InboxStageName""=nt.""InboxStageName""
     //           where ur.""Id"" = '{userRoleId}'
     //           Group By nt.""Count"",usp.""InboxStageName"", usp.""StageSequence""
     //           order by CAST(coalesce(usp.""StageSequence"", '0') AS integer) asc";

     //           list = await _queryRepo.ExecuteQueryList<TreeViewViewModel>(query, null);



     //           var obj = expObj.Where(x => x.Type == "PROJECTSTAGE").FirstOrDefault();
     //           if (obj.IsNotNull())
     //           {
     //               var data = list.Where(x => x.id == obj.id).FirstOrDefault();
     //               if (data.IsNotNull())
     //               {
     //                   data.expanded = true;
     //               }
     //           }

     //       }
            else if (type == "PROJECTSTAGE")
            {

                list = await _projectManagementQueryBusiness.GetProjectStageData3(id, userRoleId, userId);


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



                list = await _projectManagementQueryBusiness.GetProjectData3(id, userId);


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

        //    else if (type == "STAGE")
        //    {
        //        query = $@"select 'STAGE' as Type,s.""ServiceSubject"" ||' (' || COALESCE(t.""Count"",0)|| ')' as Name,
        //        s.""Id"" as id,
        //        true as hasChildren
        //        FROM public.""NtsService"" as s
        //        left join(
        //        WITH RECURSIVE Nts AS(

        //         WITH RECURSIVE NtsService AS(
        //         SELECT s.""Id"",s.""Id"" as ""ServiceId""  FROM public.""NtsService"" as s
                         
						    
						  //where s.""ParentServiceId""='{id}' 
					 
        //                union all
        //                SELECT s.""Id"", ns.""ServiceId"" FROM public.""NtsService"" as s
        //                join NtsService ns on s.""ParentServiceId""=ns.""Id""

        //             join public.""User"" as u on s.""OwnerUserId""=u.""Id""
                        
        //         )
        //         SELECT ""Id"",'Parent' as Level,""ServiceId""  from NtsService


        //         union all

        //         select t.""Id"",'Child' as Level,nt.""ServiceId"" 
        //             FROM  public.""NtsTask"" as t
        //                join Nts as nt on t.""ParentServiceId"" =nt.""Id""  
	                             
        //            )
	         
        //            SELECT count(""Id"") as ""Count"",""ServiceId"" from Nts where Level='Child' group by ""ServiceId""
                 
        //        ) t on s.""Id""=t.""ServiceId""
        //        where s.""ParentServiceId""='{id}'
        //        order by s.""SequenceOrder"" asc";

        //        list = await _queryRepo.ExecuteQueryList<TreeViewViewModel>(query, null);

        //    }
            else
            {

                //query = $@"select 'STATUS' as Type,s.""StatusLabel"" ||' (' || COALESCE(t.""Count"",0)|| ')' as Name,
                //'{parentId}' as StageId,s.""StatusCode"" as StatusCode,
                //false as hasChildren
                //FROM public.""UserRoleStageChild"" as s
                //--left join rec.""UserRoleStatusLabelCode"" as urs on urs.""StatusLabelId"" = s.""Id""
                //left join(
                //    select case when task.""TaskStatusCode""='OVERDUE' then 'INPROGRESS' else task.""TaskStatusCode"" end as TaskStatusCode,count(task.""Id"")  as ""Count""
                //    FROM public.""RecTask"" as s
                //    join public.""RecTask"" as task on  task.""ReferenceTypeId"" = s.""Id""
                //    join public.""RecTaskTemplate"" as tmp on tmp.""TemplateCode"" = task.""TemplateCode""
                //    join public.""User"" as au on  au.""Id"" = task.""AssigneeUserId""

                //    Where tmp.""TemplateCode""='{parentId}' and task.""AssigneeUserId"" = '{userId}'
                //    and s.""IsDeleted""=false and task.""IsDeleted""=false and tmp.""IsDeleted""=false and au.""IsDeleted""=false 

                //    group by TaskStatusCode  
                //) t on t.TaskStatusCode=ANY(s.""StatusCode"")
                //where s.""InboxStageId""='{stageId}'
                //order by s.""SequenceOrder"" asc";

                //list = await _queryRepo.ExecuteQueryList<TreeViewViewModel>(query, null);

            }
            return list;
        }
        public async Task<IList<TreeViewViewModel>> GetInboxMenuItemByUser(string id, string type, string parentId, string userRoleId, string userId, string userRoleIds, string stageName, string stageId, string projectId, string expandingList, string userroleCode)
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
            var query = "";
            if (id.IsNullOrEmpty())
            {
                var roles = userRoleIds.Split(",");
                var roleText = "";
                foreach (var i in roles)
                {
                    roleText += $"'{i}',";
                }
                roleText = roleText.Trim(',');

                var count = await _projectManagementQueryBusiness.GetInboxMenuItemByUsercount(roleText, userId);

                var item = new TreeViewViewModel
                {
                    id = "INBOX",
                    Name = "Inbox",
                    DisplayName = "Inbox",
                    ParentId = null,
                    hasChildren = true,
                    expanded = true,
                    Type = "INBOX",
                    children = true,
                    text = $"Inbox ({count })",
                    parent = "#",
                    a_attr = new { data_id = "INBOX", data_type = "INBOX", data_name = $"Inbox ({count })", data_parentId = "#" },

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

                list = await _projectManagementQueryBusiness.GetInboxMenuItemByUser(roleText, userId);
                //expanded -> type= userrole - from coming list find type as userRole
                // if found then find the item in list as selcted item id
                //make expanded true

                var obj = expObj.Where(x => x.Type == "PROJECTSTAGE").FirstOrDefault();
                if (obj.IsNotNull())
                {
                    var data = list.Where(x => x.id == obj.UserRoleId).FirstOrDefault();
                    if (data.IsNotNull())
                    {
                        data.expanded = true;
                    }
                }

            }
     //       else if (type == "USERROLE")
     //       {

     //           query = $@"Select usp.""InboxStageName"" ||' (' || nt.""Count""|| ')' as Name
     //           , usp.""InboxStageName"" as id, '{id}' as ParentId, 'PROJECTSTAGE' as Type,
     //           true as hasChildren, '{userRoleId}' as UserRoleId
     //           from public.""UserRole"" as ur
     //           join public.""UserRoleStageParent"" as usp on usp.""UserRoleId"" = ur.""Id"" and usp.""InboxCode""='PMT'
              
     //            left join(
     //               WITH RECURSIVE Nts AS(

     //            WITH RECURSIVE NtsService AS(
     //            SELECT s.""Id"",s.""Id"" as ""ServiceId"",usp.""InboxStageName""   FROM public.""NtsService"" as s
     //                    join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" 
					//	     join public.""UserRoleStageParent"" as usp on usp.""TemplateCode"" = tt.""Code"" and usp.""InboxCode""='PMT' 
					//	     where usp.""UserRoleId"" = '{userRoleId}' 
					 
     //                   union all
     //                   SELECT s.""Id"", s.""Id"" as ""ServiceId"",ns.""InboxStageName"" FROM public.""NtsService"" as s
     //                   join NtsService ns on s.""ParentServiceId""=ns.""ServiceId""

     //                join public.""User"" as u on s.""OwnerUserId""=u.""Id""
                        
     //            )
     //            SELECT ""Id"",'Parent' as Level,""InboxStageName""  from NtsService


     //            union all

     //            select t.""Id"",'Child' as Level,nt.""InboxStageName"" 
     //                FROM  public.""NtsTask"" as t
     //                   join Nts as nt on t.""ParentServiceId"" =nt.""Id""  
	    //                  join public.""User"" as u on t.""AssignedToUserId""=u.""Id""
	    //                where t.""AssignedToUserId"" = '{userId}'           
     //               )
						
     //               SELECT count(""Id"") as ""Count"",""InboxStageName"" from Nts where Level='Child' group by ""InboxStageName""
					//) nt on usp.""InboxStageName""=nt.""InboxStageName""
     //           where ur.""Id"" = '{userRoleId}'
     //           Group By nt.""Count"",usp.""InboxStageName"", usp.""StageSequence""
     //           order by CAST(coalesce(usp.""StageSequence"", '0') AS integer) asc";

     //           list = await _queryRepo.ExecuteQueryList<TreeViewViewModel>(query, null);



     //           var obj = expObj.Where(x => x.Type == "PROJECTSTAGE").FirstOrDefault();
     //           if (obj.IsNotNull())
     //           {
     //               var data = list.Where(x => x.id == obj.id).FirstOrDefault();
     //               if (data.IsNotNull())
     //               {
     //                   data.expanded = true;
     //               }
     //           }

     //       }
            else if (type == "PROJECTSTAGE")
            {

                list = await _projectManagementQueryBusiness.GetProjectStageData2(id, userRoleId, userId);


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



                list = await _projectManagementQueryBusiness.GetProjectData2(id, userId);


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
            //   public async Task<IList<TreeViewViewModel>> GetInboxMenuItemByUser(string id, string type, string parentId, string userRoleId, string userId, string userRoleIds, string stageName, string stageId, string projectId, string expandingList, string userroleCode)
            //   {
            //       var expObj = new List<TreeViewViewModel>();
            //       if (expandingList != null)
            //       {
            //           expObj = JsonConvert.DeserializeObject<List<TreeViewViewModel>>(expandingList);
            //           var obj = expObj.Where(x => x.id == id).FirstOrDefault();
            //           if (obj.IsNotNull())
            //           {
            //               type = obj.Type;
            //               parentId = obj.ParentId;
            //               userRoleId = obj.UserRoleId;
            //               projectId = obj.ProjectId;
            //               stageId = obj.StageId;
            //           }
            //       }

            //       var list = new List<TreeViewViewModel>();
            //       var query = "";
            //       if (id.IsNullOrEmpty())
            //       {
            //           var roles = userRoleIds.Split(",");
            //           var roleText = "";
            //           foreach (var i in roles)
            //           {
            //               roleText += $"'{i}',";
            //           }
            //           roleText = roleText.Trim(',');
            //           query = $@"  WITH RECURSIVE NtsService AS ( 
            //	     SELECT s.""ServiceSubject"" as ts,s.""Id"",'Parent'  as Type
            //                        FROM public.""NtsService"" as s
            //                        join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" 
            //	     join public.""UserRoleStageParent"" as usp on usp.""TemplateCode"" = tt.""Code"" and usp.""InboxCode""='PMT' 
            //	     and usp.""UserRoleId"" in ({roleText})

            //                        --left join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id""
            //	     --where s.""OwnerUserId""='{userId}'
            //	     union all
            //                        SELECT t.""TaskSubject"" as ts, s.""Id"",'Child'  as Type
            //                        FROM public.""NtsService"" as s
            //                        inner join NtsService ns on s.""ParentServiceId""=ns.""Id""
            //		 join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id"" 

            //                       where t.""AssignedToUserId""='{userId}'

            //                     --where s.""OwnerUserId""='{userId}'
            //                   )
            //                   SELECT count(ts) from NtsService where Type!='Parent'";
            //           var count = await _queryRepo.ExecuteScalar<long?>(query, null);

            //           var item = new TreeViewViewModel
            //           {
            //               id = "INBOX",
            //               Name = "Inbox",
            //               DisplayName = "Inbox",
            //               ParentId = null,
            //               hasChildren = true,
            //               expanded = true,
            //               Type = "INBOX"
            //           };
            //           if (count != null)
            //           {
            //               item.Name = item.DisplayName = $"Inbox ({count })";
            //           }
            //           list.Add(item);

            //       }
            //       else if (id == "INBOX")
            //       {
            //           var roles = userRoleIds.Split(",");
            //           var roleText = "";
            //           foreach (var item in roles)
            //           {
            //               roleText += $"'{item}',";
            //           }
            //           roleText = roleText.Trim(',');
            //           query = $@"Select distinct ur.""Id"" as id,ur.""Name"" ||' (' || nt.""Count""|| ')' as Name
            //           , 'INBOX' as ParentId, 'USERROLE' as Type,
            //           true as hasChildren, ur.""Id"" as UserRoleId
            //           from public.""UserRole"" as ur
            //           join public.""UserRoleStageParent"" as usp on usp.""UserRoleId"" = ur.""Id"" and usp.""InboxCode""='PMT'
            //           left join(
            //	 WITH RECURSIVE NtsService AS ( 
            //	     SELECT distinct t.""TaskSubject"" as ts,s.""Id"",usp.""UserRoleId"",'Parent'  as Type
            //                        FROM public.""NtsService"" as s
            //                        join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" 
            //	     join public.""UserRoleStageParent"" as usp on usp.""TemplateCode"" = tt.""Code"" and usp.""InboxCode""='PMT' 
            //	     left join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id""
            //	     where usp.""UserRoleId"" in ({roleText}) and t.""AssignedToUserId""='{userId}'
            //	     union all
            //                        SELECT distinct t.""TaskSubject"" as ts, s.""Id"",ns.""UserRoleId"",'Child'  as Type
            //                        FROM public.""NtsService"" as s
            //                        join NtsService ns on s.""ParentServiceId""=ns.""Id""
            //                        join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id"" 
            //                          where t.""AssignedToUserId""='{userId}'

            //                     --where s.""OwnerUserId""='{userId}'
            //                   )
            //                   SELECT count(ts) as ""Count"",""UserRoleId"" from NtsService where Type!='Parent' group by ""UserRoleId""
            //) nt on nt.""UserRoleId""=ur.""Id""
            //           where ur.""Id"" in ({roleText})
            //           --order by CAST(coalesce(usp.""StageSequence"", '0') AS integer) asc";
            //           list = await _queryRepo.ExecuteQueryList<TreeViewViewModel>(query, null);
            //           //expanded -> type= userrole - from coming list find type as userRole
            //           // if found then find the item in list as selcted item id
            //           //make expanded true

            //           var obj = expObj.Where(x => x.Type == "USERROLE").FirstOrDefault();
            //           if (obj.IsNotNull())
            //           {
            //               var data = list.Where(x => x.id == obj.UserRoleId).FirstOrDefault();
            //               if (data.IsNotNull())
            //               {
            //                   data.expanded = true;
            //               }
            //           }

            //       }
            //       else if (type == "USERROLE")
            //       {

            //           query = $@"Select usp.""InboxStageName"" ||' (' || nt.""Count""|| ')' as Name
            //           , usp.""InboxStageName"" as id, '{id}' as ParentId, 'PROJECTSTAGE' as Type,
            //           true as hasChildren, '{userRoleId}' as UserRoleId
            //           from public.""UserRole"" as ur
            //           join public.""UserRoleStageParent"" as usp on usp.""UserRoleId"" = ur.""Id"" and usp.""InboxCode""='PMT'

            //            left join(
            //	WITH RECURSIVE NtsService AS ( 
            //                       SELECT distinct t.""TaskSubject"" as ts,s.""Id"",usp.""InboxStageName"" ,'Parent'  as Type
            //                        FROM public.""NtsService"" as s
            //                        join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" 
            //	     join public.""UserRoleStageParent"" as usp on usp.""TemplateCode"" = tt.""Code"" and usp.""InboxCode""='PMT' 
            //	     left join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id""
            //	     where usp.""UserRoleId"" = '{userRoleId}' and t.""AssignedToUserId""='{userId}'
            //	     union all
            //                        SELECT distinct t.""TaskSubject"" as ts, s.""Id"",ns.""InboxStageName"",'Child'  as Type
            //                        FROM public.""NtsService"" as s
            //                        join NtsService ns on s.""ParentServiceId""=ns.""Id""
            //                        join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id""
            //                      where t.""AssignedToUserId""='{userId}'
            //                     --where s.""OwnerUserId""='{userId}'                        
            //               )
            //               SELECT count(ts) as ""Count"",""InboxStageName"" from NtsService where Type!='Parent' group by ""InboxStageName""
            //) nt on usp.""InboxStageName""=nt.""InboxStageName""
            //           where ur.""Id"" = '{userRoleId}'
            //           Group By nt.""Count"",usp.""InboxStageName"", usp.""StageSequence""
            //           order by CAST(coalesce(usp.""StageSequence"", '0') AS integer) asc";

            //           list = await _queryRepo.ExecuteQueryList<TreeViewViewModel>(query, null);



            //           var obj = expObj.Where(x => x.Type == "PROJECTSTAGE").FirstOrDefault();
            //           if (obj.IsNotNull())
            //           {
            //               var data = list.Where(x => x.id == obj.id).FirstOrDefault();
            //               if (data.IsNotNull())
            //               {
            //                   data.expanded = true;
            //               }
            //           }

            //       }
            //       else if (type == "PROJECTSTAGE")
            //       {
            //           query = $@"Select  usp.""TemplateShortName"" ||' (' || COALESCE(t.""Count"",0)|| ')'  as Name,
            //           usp.""TemplateCode""  as id, '{id}' as ParentId,                
            //           'PROJECT' as Type,'{userRoleId}' as UserRoleId,
            //           true as hasChildren
            //           from public.""UserRole"" as ur
            //           join public.""UserRoleStageParent"" as usp on usp.""UserRoleId"" = ur.""Id"" and usp.""InboxCode""='PMT'
            //           left join(
            //        WITH RECURSIVE NtsService AS ( 
            //               SELECT distinct t.""TaskSubject"" as ts,s.""Id"",usp.""TemplateCode"" ,'Parent'  as Type
            //                   FROM public.""NtsService"" as s
            //                   join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" 
            //	join public.""UserRoleStageParent"" as usp on usp.""TemplateCode"" = tt.""Code"" and usp.""InboxCode""='PMT' 
            //	left join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id""
            //	where usp.""UserRoleId"" = '{userRoleId}' and t.""AssignedToUserId""='{userId}'
            //	union all
            //                   SELECT distinct t.""TaskSubject"" as ts, s.""Id"",ns.""TemplateCode"",'Child'  as Type
            //                   FROM public.""NtsService"" as s
            //                   join NtsService ns on s.""ParentServiceId""=ns.""Id""
            //                   join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id""
            //                 where t.""AssignedToUserId""='{userId}'     
            //               --where s.""OwnerUserId""='{userId}'                        
            //               )
            //               SELECT count(ts) as ""Count"",""TemplateCode"" from NtsService where Type!='Parent' group by ""TemplateCode""

            //           ) t on usp.""TemplateCode""=t.""TemplateCode""

            //           where ur.""Id"" = '{userRoleId}' and usp.""InboxStageName"" = '{id}'
            //           Group By t.""Count"", usp.""TemplateCode"",usp.""TemplateShortName"", usp.""InboxStageName"", usp.""ChildSequence"",id
            //           order by CAST(coalesce(usp.""ChildSequence"", '0') AS integer) asc";
            //           list = await _queryRepo.ExecuteQueryList<TreeViewViewModel>(query, null);


            //           var obj = expObj.Where(x => x.Type == "PROJECT").FirstOrDefault();
            //           if (obj.IsNotNull())
            //           {
            //               var data = list.Where(x => x.id == obj.id).FirstOrDefault();
            //               if (data.IsNotNull())
            //               {
            //                   data.expanded = true;
            //               }
            //           }

            //       }
            //       else if (type == "PROJECT")
            //       {
            //           query = $@"select  s.""Id"" as id,
            //           s.""ServiceSubject"" ||' (' || case when t.""Count"" is not null then t.""Count"" else 0 end || ')' as Name,
            //           true as hasChildren,s.""Id"" as ProjectId,
            //           '{id}' as ParentId,'STAGE' as Type
            //           FROM public.""NtsService"" as s
            //           join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" 
            //            left join(
            //        WITH RECURSIVE NtsService AS ( 
            //               SELECT distinct t.""TaskSubject"" as ts,s.""Id"",s.""Id"" as ""ServiceId"" ,'Parent'  as Type
            //                   FROM public.""NtsService"" as s
            //                   join public.""Template"" as tt on tt.""Id"" =s.""TemplateId"" 
            //	left join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id""
            //	where tt.""Code""='{id}' and t.""AssignedToUserId""='{userId}'
            //	union all
            //                   SELECT distinct t.""TaskSubject"" as ts, s.""Id"",ns.""ServiceId"" as ""ServiceId"" ,'Child'  as Type
            //                   FROM public.""NtsService"" as s
            //                   join NtsService ns on s.""ParentServiceId""=ns.""Id""
            //                   join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id""
            //                 where t.""AssignedToUserId""='{userId}'     
            //               -- where s.""OwnerUserId""='{userId}'                        
            //               )
            //               SELECT count(ts) as ""Count"",""ServiceId"" from NtsService where Type!='Parent' group by ""ServiceId""

            //           ) t on s.""Id""=t.""ServiceId""

            //           Where tt.""Code""='{id}' --and s.""OwnerUserId"" = '{userId}' 
            //           GROUP BY s.""Id"", s.""ServiceSubject"",t.""Count""    ";


            //           list = await _queryRepo.ExecuteQueryList<TreeViewViewModel>(query, null);


            //           var obj = expObj.Where(x => x.Type == "STAGE").FirstOrDefault();
            //           if (obj.IsNotNull())
            //           {
            //               var data = list.Where(x => x.id == obj.id).FirstOrDefault();
            //               if (data.IsNotNull())
            //               {
            //                   data.expanded = true;
            //               }
            //           }


            //       }

            //       else if (type == "STAGE")
            //       {
            //           query = $@"select 'STAGE' as Type,s.""ServiceSubject"" ||' (' || COALESCE(t.""Count"",0)|| ')' as Name,
            //           s.""Id"" as id,
            //           true as hasChildren
            //           FROM public.""NtsService"" as s
            //           left join(
            //        WITH RECURSIVE NtsService AS ( 
            //               SELECT distinct t.""TaskSubject"" as ts,s.""Id"",s.""Id"" as ""ServiceId"" 
            //                   FROM public.""NtsService"" as s                      
            //	left join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id""
            //	where s.""ParentServiceId""='{id}' and t.""AssignedToUserId""='{userId}'
            //	union all
            //                   SELECT distinct t.""TaskSubject"" as ts, s.""Id"",ns.""ServiceId"" as ""ServiceId"" 
            //                   FROM public.""NtsService"" as s
            //                   join NtsService ns on s.""ParentServiceId""=ns.""Id""
            //                   join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id""
            //                where t.""AssignedToUserId""='{userId}'      
            //                --where s.""OwnerUserId""='{userId}'                        
            //               )
            //               SELECT count(ts) as ""Count"",""ServiceId"" from NtsService group by ""ServiceId""

            //           ) t on s.""Id""=t.""ServiceId""
            //           where s.""ParentServiceId""='{id}'
            //           order by s.""SequenceOrder"" asc";

            //           list = await _queryRepo.ExecuteQueryList<TreeViewViewModel>(query, null);

            //       }
            //       else
            //       {

            //           //query = $@"select 'STATUS' as Type,s.""StatusLabel"" ||' (' || COALESCE(t.""Count"",0)|| ')' as Name,
            //           //'{parentId}' as StageId,s.""StatusCode"" as StatusCode,
            //           //false as hasChildren
            //           //FROM public.""UserRoleStageChild"" as s
            //           //--left join rec.""UserRoleStatusLabelCode"" as urs on urs.""StatusLabelId"" = s.""Id""
            //           //left join(
            //           //    select case when task.""TaskStatusCode""='OVERDUE' then 'INPROGRESS' else task.""TaskStatusCode"" end as TaskStatusCode,count(task.""Id"")  as ""Count""
            //           //    FROM public.""RecTask"" as s
            //           //    join public.""RecTask"" as task on  task.""ReferenceTypeId"" = s.""Id""
            //           //    join public.""RecTaskTemplate"" as tmp on tmp.""TemplateCode"" = task.""TemplateCode""
            //           //    join public.""User"" as au on  au.""Id"" = task.""AssigneeUserId""

            //           //    Where tmp.""TemplateCode""='{parentId}' and task.""AssigneeUserId"" = '{userId}'
            //           //    and s.""IsDeleted""=false and task.""IsDeleted""=false and tmp.""IsDeleted""=false and au.""IsDeleted""=false 

            //           //    group by TaskStatusCode  
            //           //) t on t.TaskStatusCode=ANY(s.""StatusCode"")
            //           //where s.""InboxStageId""='{stageId}'
            //           //order by s.""SequenceOrder"" asc";

            //           //list = await _queryRepo.ExecuteQueryList<TreeViewViewModel>(query, null);

            //       }
            //       return list;
            //   }
            public async Task<IList<ProjectGanttTaskViewModel>> ReadMindMapData(string projectId)
        {



            var queryData = await _projectManagementQueryBusiness.ReadMindMapData(projectId);


            return queryData;
        }
        public async Task<IList<ProjectGanttTaskViewModel>> ReadProjectTaskGridViewData(string userId,string projectId,string userRole, string projectIds = null, string ownerIds = null, string assigneeIds = null, string tasksStatus = null, FilterColumnEnum? column = null, DateTime? startDate = null, DateTime? dueDate = null)
        {
            var queryData = await _projectManagementQueryBusiness.ReadProjectTaskGridViewData(userId, projectId, userRole, projectIds, ownerIds, assigneeIds, tasksStatus, column, startDate, dueDate);
            if (projectId.IsNotNull() && projectIds.IsNotNull())
            {
                List<string> pIds = projectIds.Split(',').ToList();
                //queryData = queryData.Where(x => x.ParentId == projectIds[0]).ToList();
                queryData = queryData.Where(x => pIds.Any(p=>p == x.ParentId)).ToList();
            }
            return queryData;

        }
        public async Task<IList<ProjectGanttTaskViewModel>> ReadProjectUserWorkloadGridViewData(string userId, string projectIds = null, string ownerIds = null, string assigneeIds = null, string tasksStatus = null, FilterColumnEnum? column = null, DateTime? startDate = null, DateTime? dueDate = null)
        {
            var users = await _userHierBusiness.GetUserHierarchyByCode("PROJECT_USER_HIERARCHY", userId);

            var queryData = await _projectManagementQueryBusiness.ReadProjectUserWorkloadGridViewData(userId, users, projectIds, ownerIds, assigneeIds, tasksStatus, column, startDate, dueDate);
            
            return queryData;

        }
        public async Task<IList<IdNameViewModel>> GetProjectSharedList(string userId)
        {

            var queryData = await _projectManagementQueryBusiness.GetProjectSharedList(userId);

            return queryData;
        }
        public async Task<ProjectDashboardViewModel> GetProjectDashboardDetails(string projectId)
        {
            

            var queryData = await _projectManagementQueryBusiness.GetPrjDashboardDetails(projectId);
            if (queryData!=null)
            {
                queryData.TemplateUserType = queryData.UserId.IsNotNullAndNotEmpty() ? NtsUserTypeEnum.Owner : NtsUserTypeEnum.Shared;
            }


            var queryData1 = await _projectManagementQueryBusiness.GetPrjTaskDetails(projectId);
            if (queryData!=null)
            {
                if (queryData1!=null && queryData1.Count()>0)
                {
                    queryData.TaskCount = queryData1.Count();
                }
                else
                {
                    queryData.TaskCount = 0;
                }
            }
            
            return queryData;
        }
        public async Task<List<ProjectDashboardChartViewModel>> GetTaskStatus(string projectId)
        {

            var queryData = await _projectManagementQueryBusiness.GetTaskStatus(projectId);

            var list = new List<ProjectDashboardChartViewModel>();
            var list1 = queryData.GroupBy(x => x.NtsStatus).Select(group => new { Value = group.Count(), Type = group.Key,Id= group.Select(x=>x.NtsStatusId).FirstOrDefault() }).ToList();
            list = list1.Select(x => new ProjectDashboardChartViewModel { Type = x.Type, Value = x.Value,Id = x.Id }).ToList();
            return list;
        }



        /// <summary>
        /// Code For Chart that Bring Requested by me
        /// </summary>
        /// <param name="TemplateID"></param>
        /// /// <param name="UserID"></param>
        /// <returns></returns>
        /// 


        public async Task<List<ProjectDashboardChartViewModel>> GetTaskStatusRequestedByMe(string TemplateID,string UserID)
        {
            var queryData = await _projectManagementQueryBusiness.GetTaskStatusRequestedByMe(TemplateID, UserID);

            var list = new List<ProjectDashboardChartViewModel>();
          //  var list1 = queryData.GroupBy(x => x.NtsStatus).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.NtsStatusId).FirstOrDefault() }).ToList();
            list = queryData.Select(x => new ProjectDashboardChartViewModel { Type =x.Type, Value = x.Value}).ToList();
            return list;
        }
        public async Task<List<ProjectDashboardChartViewModel>> GetTaskStatusAssigneduserid(string TemplateID, string UserID)
        {
            var queryData = await _projectManagementQueryBusiness.GetTaskStatusAssigneduserid(TemplateID, UserID);

            var list = new List<ProjectDashboardChartViewModel>();
            //  var list1 = queryData.GroupBy(x => x.NtsStatus).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.NtsStatusId).FirstOrDefault() }).ToList();
            list = queryData.Select(x => new ProjectDashboardChartViewModel { Type = x.Type, Value = x.Value }).ToList();
            return list;
        }

        public async Task<List<ProjectDashboardChartViewModel>> MdlassignUser(string TemplateID, string UserID)
        {



            var queryData = await _projectManagementQueryBusiness.MdlassignUser(TemplateID, UserID);

            var list = new List<ProjectDashboardChartViewModel>();
            //  var list1 = queryData.GroupBy(x => x.NtsStatus).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.NtsStatusId).FirstOrDefault() }).ToList();
            list = queryData.Select(x => new ProjectDashboardChartViewModel { Type = x.Type, AssigneeId = x.RefId }).ToList();
            return list;
        }

        public async Task<List<NtsTaskChartList>> GetGridList(string TemplateID, string UserID, string assigneeIds = null, string StatusIDs=null)
        {

            var queryData = await _projectManagementQueryBusiness.GetGridList(TemplateID, UserID, assigneeIds, StatusIDs);

            //var list = new List<ProjectDashboardChartViewModel>();
            //  var list1 = queryData.GroupBy(x => x.NtsStatus).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.NtsStatusId).FirstOrDefault() }).ToList();
            //list = queryData.Select(x => new ProjectDashboardChartViewModel { Type = x.Type, Value = x.Value }).ToList();
            return queryData;
        }

        public async Task<List<ProjectDashboardChartViewModel>> GetDatewiseTask(string TemplateID, string UserID, DateTime? FromDate=null, DateTime? ToDate=null)
        {

            var queryData = await _projectManagementQueryBusiness.GetDatewiseTask(TemplateID, UserID, FromDate, ToDate);

            var list = new List<ProjectDashboardChartViewModel>();
            //  var list1 = queryData.GroupBy(x => x.NtsStatus).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.NtsStatusId).FirstOrDefault() }).ToList();
            list = queryData.Select(x => new ProjectDashboardChartViewModel { Type = x.Type, Days = x.Days.Days }).ToList();
            return list;
        }

        public async Task<IList<ProjectDashboardChartViewModel>> GetTaskType(string projectId)
        {

            var queryData = await _projectManagementQueryBusiness.GetTaskType(projectId);

            var list = new List<ProjectDashboardChartViewModel>();
            var list1 = queryData.GroupBy(x => x.UserName).Select(group => new { Value = group.Count(), Type = group.Key,Id=group.Select(x=>x.UserId).FirstOrDefault() }).ToList();
            list = list1.Select(x => new ProjectDashboardChartViewModel {Id=x.Id, Type = x.Type, Value = x.Value }).ToList();
            return list;
        }
        public async Task<IList<ProjectDashboardChartViewModel>> ReadProjectStageChartData(string userId, string projectId)
        {

            var queryData = await _projectManagementQueryBusiness.ReadProjectStageChartData(userId, projectId);

            var list = new List<ProjectDashboardChartViewModel>();
            var list1=queryData.GroupBy(x => x.ServiceStage).Select( group =>new {Value =group.Count(),Type=(group.Key.IsNotNullAndNotEmpty()?group.Key: group.Select(x => x.ProjectName).FirstOrDefault()),Id=group.Select(x=>x.ParentId).FirstOrDefault() }).ToList();
            list = list1.Select(x => new ProjectDashboardChartViewModel {Id=x.Id, Type = x.Type, Value = x.Value }).ToList();
            return list;

        }

        public async Task<IList<IdNameViewModel>> GetProjectStageIdNameList(string userId, string projectId)
        {

            var queryData = await _projectManagementQueryBusiness.GetProjectStageIdNameList(userId, projectId);

            var list = new List<IdNameViewModel>();
            list = queryData.GroupBy(x => x.ServiceStage).Select(group => new IdNameViewModel { Id = group.Select(x=>x.ParentId).FirstOrDefault(), Name = (group.Key.IsNotNullAndNotEmpty() ? group.Key : group.Select(x => x.ProjectName).FirstOrDefault()) }).ToList();
            return list;

        }
        public async Task<IList<IdNameViewModel>> GetSubordinatesUserIdNameList()
        {
            var list = new List<IdNameViewModel>();
            if (_userContext.IsSystemAdmin)
            {

                var queryData = await _projectManagementQueryBusiness.GetSubordinatesUserIdNameListuserIfAdmin();
                list = queryData;
            }
            else
            {

                var queryData1 = await _projectManagementQueryBusiness.GetSubordinatesUserIdNameList();
                list = queryData1;
            }
            list = list.OrderBy(x => x.Name).ToList();
            var puser = list.Where(x => x.Id == _userContext.UserId).ToList();
            if (puser.Count == 0)
            {
                list.Insert(0, new IdNameViewModel { Id = _userContext.UserId, Name = _userContext.Name });
            }
            return list;
        }
        public async Task<IList<TaskViewModel>> ReadTaskOverdueData(string projectId)
        {
            var queryData = await _projectManagementQueryBusiness.ReadTaskOverdueData(projectId);
            return queryData;

        }
        public async Task<IList<TeamWorkloadViewModel>> ReadProjectSubTaskViewData(string taskId, List<string> assigneeIds = null, List<string> statusIds = null, List<string> ownerIds = null)
        {
            var queryData = await _projectManagementQueryBusiness.ReadProjectSubTaskViewData(taskId, assigneeIds, statusIds, ownerIds);
            return queryData;
        }

        public async Task<ProgramDashboardViewModel> ReadProjectTotalTaskDataOld(string projectId)
        {
            var queryData1 = await _projectManagementQueryBusiness.ReadProjectTotalTaskDataOld(projectId);
            return queryData1;
        }

        public async Task<ProgramDashboardViewModel> ReadProjectTotalTaskData(string projectId)
        {


            var queryData1 = await _projectManagementQueryBusiness.ReadProjectTotalTaskData(projectId);
            return queryData1;
        }

        public async Task<IList<TeamWorkloadViewModel>> ReadManagerProjectTaskViewData(string projectId, List<string> assigneeIds = null, List<string> statusIds = null, List<string> ownerIds = null, FilterColumnEnum? column = null, DateTime? startDate = null, DateTime? dueDate = null)
        {

            var queryData = await _projectManagementQueryBusiness.ReadManagerProjectTaskViewData(projectId, assigneeIds, statusIds, ownerIds, column, startDate, dueDate);
            return queryData;
        }

        public async Task<IList<TeamWorkloadViewModel>> ReadProjectTaskViewData(string projectId, List<string> assigneeIds = null, List<string> statusIds = null, List<string> ownerIds = null, FilterColumnEnum? column = null, DateTime? startDate = null, DateTime? dueDate = null)
        {
            var queryData = await _projectManagementQueryBusiness.ReadProjectTaskViewData(projectId, assigneeIds, statusIds, ownerIds, column, startDate, dueDate);
            return queryData;
        }

        public async Task<IList<TeamWorkloadViewModel>> ReadManagerProjectStageViewData(string projectId)
        {
            var queryData = await _projectManagementQueryBusiness.ReadManagerProjectStageViewData(projectId);
            return queryData;
        }

        public async Task<IList<TeamWorkloadViewModel>> ReadProjectStageViewData(string projectId)
        {
            var queryData = await _projectManagementQueryBusiness.ReadProjectStageViewData(projectId);
            return queryData.OrderBy(x=>x.Level).ToList();
        }

        public async Task<IList<TeamWorkloadViewModel>> ReadProjectTaskAssignedDataOld(string projectId)
        {
            var queryData = await _projectManagementQueryBusiness.ReadProjectTaskAssignedDataOld(projectId);
            return queryData;
        }

        public async Task<IList<TeamWorkloadViewModel>> ReadManagerProjectTaskAssignedData(string projectId)
        {
            var queryData = await _projectManagementQueryBusiness.ReadManagerProjectTaskAssignedData(projectId);
            return queryData;
        }

        public async Task<IList<TeamWorkloadViewModel>> ReadProjectTaskAssignedData(string projectId)
        {
            var queryData = await _projectManagementQueryBusiness.ReadProjectTaskAssignedData(projectId);
            return queryData;
        }

        public async Task<IList<TeamWorkloadViewModel>> ReadManagerProjectTaskOwnerData(string projectId)
        {

            var queryData = await _projectManagementQueryBusiness.ReadManagerProjectTaskOwnerData(projectId);
            return queryData;
        }

        public async Task<IList<TeamWorkloadViewModel>> ReadProjectTaskOwnerData(string projectId)
        {



            var queryData = await _projectManagementQueryBusiness.ReadProjectTaskOwnerData(projectId);
            return queryData;
        }

        public async Task<IList<DashboardCalendarViewModel>> ReadProjectCalendarViewData(string projectId, List<string> assigneeIds = null, List<string> statusIds = null, List<string> ownerIds = null, FilterColumnEnum? column = null, DateTime? startDate = null, DateTime? dueDate = null)
        {


            var queryData = await _projectManagementQueryBusiness.ReadProjectCalendarViewData(projectId, assigneeIds, statusIds, ownerIds, column, startDate, dueDate);


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
                        queryData = queryData.Where(x => startDate<= x.End.Date && x.End.Date< dueDate).ToList();
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
        public async Task<IList<DashboardCalendarViewModel>> ReadManagerProjectCalendarViewData(string projectId, List<string> assigneeIds = null, List<string> statusIds = null, List<string> ownerIds = null, FilterColumnEnum? column = null, DateTime? startDate = null, DateTime? dueDate = null)
        {

            var queryData = await _projectManagementQueryBusiness.ReadManagerProjectCalendarViewData(projectId, assigneeIds, statusIds, ownerIds, column, startDate, dueDate);
            return queryData;
        }
        
        public async Task<IList<IdNameViewModel>> GetProjectsLevel1(string userId, bool isProjectManager)
        {
            var queryData = new List<IdNameViewModel>();
            if (_userContext.IsSystemAdmin)
            {
                queryData = await _projectManagementQueryBusiness.GetPrjLevel1userIfAdmin();

            }
            else if (isProjectManager)
            {
                var users = await _userHierBusiness.GetUserHierarchyByCode("PROJECT_USER_HIERARCHY", userId);
               
                string userIds = null;
                if (users.IsNotNull() && users.Count > 0)
                {
                    var Ids = users.Select(x => x.UserId).ToList();

                    userIds = string.Join("','", Ids);
                }
                queryData = await _projectManagementQueryBusiness.GetPrjLevel1userIfProjectManager(userId, userIds);
            }
            else
            {
                queryData = await _projectManagementQueryBusiness.GetPrjLevel1(userId);
            }
            
            var list = queryData.ToList();
            return list;
        }
        public async Task<IList<IdNameViewModel>> GetProjectsLevel2(string userId, bool isProjectManager,string monthYear)
        {
            
            var queryData = new List<IdNameViewModel>();
            if (_userContext.IsSystemAdmin)
            {
                queryData = await _projectManagementQueryBusiness.GetPrjLevel2userIfAdmin(monthYear);

            }
            else if (isProjectManager)
            {
                var users = await _userHierBusiness.GetUserHierarchyByCode("PROJECT_USER_HIERARCHY", userId);

                string userIds = null;
                if (users.IsNotNull() && users.Count > 0)
                {
                    var Ids = users.Select(x => x.UserId).ToList();

                    userIds = string.Join("','", Ids);
                }
                queryData = await _projectManagementQueryBusiness.GetPrjLevel2userIfProjectManager(monthYear, userId, userIds);
            }
            else
            {
                queryData = await _projectManagementQueryBusiness.GetPrjLevel2(monthYear, userId);
            }
            
            var list = queryData.ToList();
            return list;
        }
        public async Task<IList<IdNameViewModel>> GetProjectsLevel3(string userId, bool isProjectManager, string monthYear, string status)
        {
            
            var queryData = new List<IdNameViewModel>();
            if (_userContext.IsSystemAdmin)
            {
                queryData = await _projectManagementQueryBusiness.GetPrjLevel3userIfAdmin(monthYear, status);

            }
            else if (isProjectManager)
            {
                var users = await _userHierBusiness.GetUserHierarchyByCode("PROJECT_USER_HIERARCHY", userId);

                string userIds = null;
                if (users.IsNotNull() && users.Count > 0)
                {
                    var Ids = users.Select(x => x.UserId).ToList();

                    userIds = string.Join("','", Ids);
                }
                queryData = await _projectManagementQueryBusiness.GetPrjLevel3userIfProjectManager(monthYear, status, userId, userIds);
            }
            else
            {
                queryData = await _projectManagementQueryBusiness.GetPrjLevel3(monthYear, status, userId);
            }
            
            var list = queryData.ToList();
            return list;
        }

        public async Task<IList<IdNameViewModel>> GetProjectsList(string userId, bool isProjectManager)
        {

            var queryData = await _projectManagementQueryBusiness.GetProjectsList(userId, isProjectManager);
            var list = queryData.ToList();
            return list;
        }

        public async Task<ServiceViewModel> GetProjectDetails(string projectId)
        {
            var queryData = await _projectManagementQueryBusiness.GetProjectDetails(projectId);
            return queryData;
        }

        public async Task<IList<TeamWorkloadViewModel>> ReadProjectTeamWorkloadData(string projectId, string userId, bool isProjectManager = false)
        {
            var queryData = await _projectManagementQueryBusiness.ReadProjectTeamWorkloadData(projectId, userId, isProjectManager);
            var list = queryData.ToList();
            return list;
        }

        public async Task<IList<TeamWorkloadViewModel>> ReadProjectTeamDataByUser(string projectId, string userId)
        {

            var queryData = await _projectManagementQueryBusiness.ReadProjectTeamDataByUser(projectId, userId);
            var list = queryData.ToList();
            return list;
        }
        public async Task<IList<TeamWorkloadViewModel>> ReadProjectTeamDateData(string projectId)
        {
            var queryData = await _projectManagementQueryBusiness.ReadProjectTeamDateData(projectId);
            var list = queryData.ToList();
            return list;
        }
        public async Task<IList<TeamWorkloadViewModel>> ReadProjectTeamDataByDate(string projectId, DateTime startDate, bool isProjectManager = false)
        {
            var queryData = await _projectManagementQueryBusiness.ReadProjectTeamDataByDate(projectId, startDate, isProjectManager);
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
            //var query = "";
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

                userRoleList = await _projectManagementQueryBusiness.GetUserRoleList(userId, roleText);
            }

            if (userRoleList.IsNotNull())
            {
                foreach (var l in userRoleList)
                {
                    if (l.Type == "USERROLE")
                    {



                        projectStageList = await _projectManagementQueryBusiness.GetUserRoleData(l, userId);

                        if (projectStageList.IsNotNull())
                        {
                            foreach (var p in projectStageList)
                            {
                                if (p.Type == "PROJECTSTAGE")
                                {

                                    projectList = await _projectManagementQueryBusiness.GetProjectStageData(userId, p);

                                    if (projectList.IsNotNull())
                                    {
                                        foreach (var pr in projectList)
                                        {
                                            if (pr.Type == "PROJECT")
                                            {



                                                pStageList = await _projectManagementQueryBusiness.GetProjectData(userId, pr);

                                                if (pStageList.IsNotNull())
                                                {
                                                    foreach (var ps in pStageList)
                                                    {
                                                        if (ps.Type == "STAGE")
                                                        {


                                                            stageList = await _projectManagementQueryBusiness.GetStageData(userId, ps);
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
                    if (d.RefId.IsNullOrEmpty()) {
                        service.DataAction = DataActionEnum.Create;
                    } else
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
                                if(predData.Code == "Task")
                                {
                                    predType = NtsTypeEnum.Task;
                                } else if (predData.Code == "Service")
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
        //  public async Task<IList<ProjectGanttTaskViewModel>> ReadProjectTaskGrid(string projectId)
        //  {
        //      var query = @$"select t.""Id"",t.""TaskSubject"" as ""Title"",t.""StartDate"" as ""Start"",t.""DueDate"" as ""End"",
        //                  t.""ServiceStage"" as ""ServiceStage"",""ParentId"",t.""UserName"" as ""UserName"",t.""OwnerName"" as ""OwnerName"",true as ""Summary"",
        //                  t.""Priority"",t.""NtsStatus"",pj.""ServiceSubject"" as ""ProjectName""
        //                  FROM public.""NtsService"" as s
        //                  join(
        //                  WITH RECURSIVE NtsService AS (
        //                  SELECT t.*, s.""Id"" as ""ServiceId"", s.""Id"" as ""ParentId"", u.""Name"" as ""UserName"", n.""Name"" as ""OwnerName"",'{projectId}'  as tmp,
        //                  sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus"", s.""ServiceSubject"" as ""ServiceStage""
        //                  FROM public.""NtsService"" as s
        //                  left join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id""
        //                  left join public.""LOV"" as sp on t.""TaskPriorityId""=sp.""Id""
        //                  left join public.""LOV"" as ss on t.""TaskStatusId""=ss.""Id""
        //                  left join public.""User"" as u on t.""AssignedToUserId""=u.""Id""
        //                   left join public.""User"" as n on t.""OwnerUserId""=n.""Id""

        //                  where s.""Id""='{projectId}' 
        //union all
        //                  SELECT t.*, s.""Id"" as ""ServiceId"", s.""Id"" as ""ParentId"", u.""Name"" as ""UserName"" , n.""Name"" as ""OwnerName"",'{projectId}' as tmp,
        //                  sp.""Name"" as ""Priority"", ss.""Name"" as ""NtsStatus"", s.""ServiceSubject"" as ""ServiceStage""
        //                  FROM public.""NtsService"" as s
        //                  join NtsService ns on s.""ParentServiceId""=ns.""ServiceId""
        //                  join public.""NtsTask"" as t on t.""ParentServiceId"" =s.""Id""
        //                  join public.""LOV"" as sp on t.""TaskPriorityId""=sp.""Id""
        //                  join public.""LOV"" as ss on t.""TaskStatusId""=ss.""Id""
        //                  join public.""User"" as u on t.""AssignedToUserId""=u.""Id""
        //                  join public.""User"" as n on t.""OwnerUserId""=n.""Id""
        //                  )
        //                   SELECT ""Id"",""TaskSubject"",""StartDate"",""DueDate"",""ParentId"",""UserName"",""OwnerName"",tmp,""Priority"",""NtsStatus"",
        //                  ""ServiceStage"" from NtsService where ""Id"" is not null ) t on t.tmp=s.""Id""
        //      left join public.""NtsService"" as pj on pj.""Id""='{projectId}' 
        //      where s.""Id""='{projectId}'
        //                  ";
        //      var queryData = await _queryGantt.ExecuteQueryList<ProjectGanttTaskViewModel>(query, null);
        //      return queryData;

        //  }
        public async Task<IList<ProjectGanttTaskViewModel>> ReadProjectTask(string userId, string projectId, bool isProjectManager = false, string projectIds = null, string ownerIds = null, string assigneeIds = null, string tasksStatus = null, FilterColumnEnum? column = null, DateTime? startDate = null, DateTime? dueDate = null)
        {
            var queryData = await _projectManagementQueryBusiness.ReadProjectTask(userId, projectId, isProjectManager, projectIds, ownerIds, assigneeIds, tasksStatus, column, startDate, dueDate);
            return queryData;
        }

        public async Task<IList<IdNameViewModel>> GetProjectUserIdNameList(string projectId)
        {
            var queryData = await _projectManagementQueryBusiness.GetProjectUserIdNameList(projectId);
            var list = new List<IdNameViewModel>();
            list = queryData.GroupBy(x => x.UserName).Select(group => new IdNameViewModel { Id = group.Select(x => x.UserId).FirstOrDefault(), Name = group.Key }).ToList();
            return list;
        }
        public async Task<IList<MailViewModel>> ReadEmailTaskData(string userId)
        {
            var queryData = await _projectManagementQueryBusiness.ReadEmailTaskData(userId);
            var list = queryData.ToList();
            return list;
        }
        public async Task<IList<WBSViewModel>> ReadProjectTaskForEmailList(string projectId)
        {
            var queryData = await _projectManagementQueryBusiness.ReadProjectTaskForEmailList(projectId);
            return queryData;


        }
        public async Task<IList<IdNameViewModel>> ReadProjectTaskUserData(string projectId)
        {
            var list = await _projectManagementQueryBusiness.ReadProjectTaskUserData(projectId);
            return list;
        }

        public async Task<List<ProjectDashboardChartViewModel>> GetTaskStatusByTemplate(string templateId, string userId)
        {

            var queryData = await _projectManagementQueryBusiness.GetTaskStatusByTemplate(templateId, userId);

            var list = new List<ProjectDashboardChartViewModel>();
            var list1 = queryData.GroupBy(x => x.NtsStatus).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.NtsStatusId).FirstOrDefault() }).ToList();
            list = list1.Select(x => new ProjectDashboardChartViewModel { Type = x.Type, Value = x.Value, Id = x.Id }).ToList();
            return list;
        }
        public async Task<List<ProjectDashboardChartViewModel>> GetTaskByUsers(string templateId, string userId)
        {

            var queryData = await _projectManagementQueryBusiness.GetTaskByUsers(templateId, userId);

            var list = new List<ProjectDashboardChartViewModel>();
            var list1 = queryData.GroupBy(x => x.OwnerName).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.UserId).FirstOrDefault() }).ToList();
            list = list1.Select(x => new ProjectDashboardChartViewModel { Id = x.Id, Type = x.Type, Value = x.Value }).ToList();
            return list;
        }

        public async Task<List<ProjectDashboardChartViewModel>> GetSLADetails(string templateId, string userId, DateTime? FromDate = null, DateTime? ToDate = null)
        {

            var queryData = await _projectManagementQueryBusiness.GetSLADetails(templateId, userId, FromDate, ToDate);

            var list = new List<ProjectDashboardChartViewModel>();
            //var list1 = queryData.GroupBy(x => x.NtsStatus).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.NtsStatusId).FirstOrDefault() }).ToList();
            list = queryData.Select(x => new ProjectDashboardChartViewModel { Type = x.Type, Days = x.Days.Days, ActualSLA = x.ActualSLA.Days  }).ToList();
            return list;
        }

        public async Task<IList<ProjectGanttTaskViewModel>> ReadTaskGridViewData(string templateId, string userId, string tasksStatus = null, string ownerIds = null)
        {
            var queryData = await _projectManagementQueryBusiness.ReadTaskGridViewData(templateId, userId, tasksStatus, ownerIds);
            return queryData;
        }

        public async Task<IList<IdNameViewModel>> GetTaskOwnerUsersList(string templateId, string userId)
        {
            var queryData = await _projectManagementQueryBusiness.GetTaskOwnerUsersList(templateId, userId);
            return queryData;
        }
        public async Task<IList<IdNameViewModel>> GetTaskUsersList(string templateId)
        {
            var queryData = await _projectManagementQueryBusiness.GetTaskUsersList(templateId);
            return queryData;
        }
        //For PM
        public async Task<List<ProjectDashboardChartViewModel>> GetPMTaskStatusByTemplate(string templateId)
        {

            var queryData = await _projectManagementQueryBusiness.GetPMTaskStatusByTemplate(templateId);

            var list = new List<ProjectDashboardChartViewModel>();
            var list1 = queryData.GroupBy(x => x.NtsStatus).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.NtsStatusId).FirstOrDefault() }).ToList();
            list = list1.Select(x => new ProjectDashboardChartViewModel { Type = x.Type, Value = x.Value, Id = x.Id }).ToList();
            return list;
        }
        public async Task<List<ProjectDashboardChartViewModel>> GetPMTaskByUsers(string templateId)
        {

            var queryData = await _projectManagementQueryBusiness.GetPMTaskByUsers(templateId);

            var list = new List<ProjectDashboardChartViewModel>();
            var list1 = queryData.GroupBy(x => x.UserName).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.UserId).FirstOrDefault() }).ToList();
            list = list1.Select(x => new ProjectDashboardChartViewModel { Id = x.Id, Type = x.Type, Value = x.Value }).ToList();
            return list;
        }

        public async Task<List<ProjectDashboardChartViewModel>> GetPMSLADetails(string templateId, DateTime? FromDate = null, DateTime? ToDate = null)
        {

            var queryData = await _projectManagementQueryBusiness.GetPMSLADetails(templateId, FromDate, ToDate);

            var list = new List<ProjectDashboardChartViewModel>();
            //var list1 = queryData.GroupBy(x => x.NtsStatus).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.NtsStatusId).FirstOrDefault() }).ToList();
            list = queryData.Select(x => new ProjectDashboardChartViewModel { Type = x.Type, Days = x.Days.Days, ActualSLA=x.ActualSLA.Days }).ToList();
            return list;
        }

        public async Task<IList<ProjectGanttTaskViewModel>> ReadPMTaskGridViewData(string templateId, string tasksStatus = null, string userIds = null)
        {

            var queryData = await _projectManagementQueryBusiness.ReadPMTaskGridViewData(templateId, tasksStatus, userIds);
            return queryData;
        }


        // For Group Template


        public async Task<List<ProjectDashboardChartViewModel>> GetGroupTemplate()
        {


            var queryData = await _projectManagementQueryBusiness.GetGroupTemplate();

            var list = new List<ProjectDashboardChartViewModel>();
            //  var list1 = queryData.GroupBy(x => x.NtsStatus).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.NtsStatusId).FirstOrDefault() }).ToList();
            list = queryData.Select(x => new ProjectDashboardChartViewModel { Id = x.Id, GroupName = x.GroupName }).ToList();
            return list;
        }






        public async Task<List<ProjectDashboardChartViewModel>> GetTaskStatusRequestedByMeGroup(string TemplateID, string UserID, string StatusLOV)
        {

            var queryData = await _projectManagementQueryBusiness.GetTaskStatusRequestedByMeGroup(TemplateID, UserID, StatusLOV);

            var list = new List<ProjectDashboardChartViewModel>();
            //  var list1 = queryData.GroupBy(x => x.NtsStatus).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.NtsStatusId).FirstOrDefault() }).ToList();
            list = queryData.Select(x => new ProjectDashboardChartViewModel { Id = x.Id, Type = x.Type, Value = x.Value }).ToList();
            return list;
        }
        public async Task<List<ProjectDashboardChartViewModel>> GetTaskStatusAssigneduseridGroup(string TemplateID, string UserID, string StatusTemplateId, string StatusLOV)
        {
            var queryData = await _projectManagementQueryBusiness.GetTaskStatusAssigneduseridGroup(TemplateID, UserID, StatusTemplateId, StatusLOV);
            var list = new List<ProjectDashboardChartViewModel>();
            //  var list1 = queryData.GroupBy(x => x.NtsStatus).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.NtsStatusId).FirstOrDefault() }).ToList();
            list = queryData.Select(x => new ProjectDashboardChartViewModel { Type = x.Type, Value = x.Value }).ToList();
            return list;
        }

        public async Task<List<ProjectDashboardChartViewModel>> MdlassignUserGroup(string TemplateID, string UserID)
        {
            var queryData = await _projectManagementQueryBusiness.MdlassignUserGroup(TemplateID, UserID);

            var list = new List<ProjectDashboardChartViewModel>();
            //  var list1 = queryData.GroupBy(x => x.NtsStatus).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.NtsStatusId).FirstOrDefault() }).ToList();
            list = queryData.Select(x => new ProjectDashboardChartViewModel { Type = x.Type, AssigneeId = x.RefId }).ToList();
            return list;
        }

        public async Task<List<NtsTaskChartList>> GetGridListGroup(string TemplateID, string UserID, string assigneeIds = null, string StatusIDs = null)
        {
            var queryData = await _projectManagementQueryBusiness.GetGridListGroup(TemplateID, UserID, assigneeIds, StatusIDs);

            //var list = new List<ProjectDashboardChartViewModel>();
            //  var list1 = queryData.GroupBy(x => x.NtsStatus).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.NtsStatusId).FirstOrDefault() }).ToList();
            //list = queryData.Select(x => new ProjectDashboardChartViewModel { Type = x.Type, Value = x.Value }).ToList();
            return queryData;
        }

        public async Task<List<ProjectDashboardChartViewModel>> GetDatewiseTaskGroup(string TemplateID, string UserID, DateTime? FromDate = null, DateTime? ToDate = null)
        {


            var queryData = await _projectManagementQueryBusiness.GetDatewiseTaskGroup(TemplateID, UserID, FromDate, ToDate);




            var list = new List<ProjectDashboardChartViewModel>();
            //  var list1 = queryData.GroupBy(x => x.NtsStatus).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.NtsStatusId).FirstOrDefault() }).ToList();
            list = queryData.Select(x => new ProjectDashboardChartViewModel { Type = x.Type, Days = x.Days.Days, ActualSLA = x.ActualSLA.Days }).ToList();
            return list;
        }

        //for project manager
        public async Task<List<ProjectDashboardChartViewModel>> GetTaskStatusRequestedByMeProjectGroup(string TemplateID, string StatusLOV = null)
        {

            var queryData = await _projectManagementQueryBusiness.GetTaskStatusRequestedByMeProjectGroup(TemplateID, StatusLOV);

            var list = new List<ProjectDashboardChartViewModel>();
            var list1 = queryData.GroupBy(x => x.NtsStatus).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.Id).FirstOrDefault() }).ToList();
            list = list1.Select(x => new ProjectDashboardChartViewModel { Type = x.Type, Value = x.Value, Id = x.Id }).ToList();
            return list;
        }
        public async Task<List<ProjectDashboardChartViewModel>> GetChartByAssigneduserProjectGroup(string templateId, string StatusTemplateID = null, string StatusLOV = null)
        {

            var queryData = await _projectManagementQueryBusiness.GetChartByAssigneduserProjectGroup(templateId, StatusTemplateID, StatusLOV);

            var list = new List<ProjectDashboardChartViewModel>();
            var list1 = queryData.GroupBy(x => x.OwnerName).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.UserId).FirstOrDefault() }).ToList();
            list = list1.Select(x => new ProjectDashboardChartViewModel { Id = x.Id, Type = x.Type, Value = x.Value }).ToList();
            return list;
        }

        public async Task<IList<ProjectGanttTaskViewModel>> GetGridListProjectGroup(string templateId, string tasksStatus = null, string ownerIds = null)
        {
            var queryData = await _projectManagementQueryBusiness.GetGridListProjectGroup(templateId, tasksStatus, ownerIds);
            return queryData;
        }

        public async Task<IList<ProjectDashboardChartViewModel>> GetTaskStatusAssigneduseridProjectGroup(string templateId)
        {
            var queryData = await _projectManagementQueryBusiness.GetTaskStatusAssigneduseridProjectGroup(templateId);

            var list = new List<ProjectDashboardChartViewModel>();
            //  var list1 = queryData.GroupBy(x => x.NtsStatus).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.NtsStatusId).FirstOrDefault() }).ToList();
            list = queryData.Select(x => new ProjectDashboardChartViewModel { Type = x.Type, Value = x.Value }).ToList();
            return list;
        }

        public async Task<IList<ProjectDashboardChartViewModel>> GetDatewiseTaskProjectGroup(string templateId, DateTime? FromDate, DateTime? ToDate)
        {
            var queryData = await _projectManagementQueryBusiness.GetDatewiseTaskProjectGroup(templateId, FromDate, ToDate);




            var list = new List<ProjectDashboardChartViewModel>();
            //  var list1 = queryData.GroupBy(x => x.NtsStatus).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.NtsStatusId).FirstOrDefault() }).ToList();
            list = queryData.Select(x => new ProjectDashboardChartViewModel { Type = x.Type, Days = x.Days.Days, ActualSLA = x.ActualSLA.Days }).ToList();
            return list;
        }



      



        public async Task<List<ProjectDashboardChartViewModel>> GetTaskStatusByTemplateGroup(string templateId, string userId, string StatusLOV = null)
        {
            var queryData = await _projectManagementQueryBusiness.GetTaskStatusByTemplateGroup(templateId, userId, StatusLOV);

            var list = new List<ProjectDashboardChartViewModel>();
            var list1 = queryData.GroupBy(x => x.NtsStatus).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.NtsStatusId).FirstOrDefault() }).ToList();
            list = list1.Select(x => new ProjectDashboardChartViewModel { Type = x.Type, Value = x.Value, Id = x.Id }).ToList();
            return list;
        }
        public async Task<List<ProjectDashboardChartViewModel>> GetTaskByUsersGroup(string templateId, string userId, string StatusTemplateID = null, string StatusLOV = null)
        {
            var queryData = await _projectManagementQueryBusiness.GetTaskByUsersGroup(templateId, userId, StatusTemplateID, StatusLOV);

            var list = new List<ProjectDashboardChartViewModel>();
            var list1 = queryData.GroupBy(x => x.OwnerName).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.UserId).FirstOrDefault() }).ToList();
            list = list1.Select(x => new ProjectDashboardChartViewModel { Id = x.Id, Type = x.Type, Value = x.Value }).ToList();
            return list;
        }

        public async Task<IList<ProjectGanttTaskViewModel>> ReadTaskGridViewDataGroup(string templateId, string userId, string tasksStatus = null, string ownerIds = null)
        {
            var queryData = await _projectManagementQueryBusiness.ReadTaskGridViewDataGroup(templateId, userId, tasksStatus, ownerIds);
            return queryData;
        }

        public async Task<IList<IdNameViewModel>> GetTaskOwnerUsersListGroup(string templateId, string userId)
        {
            var queryData = await _projectManagementQueryBusiness.GetTaskOwnerUsersListGroup(templateId, userId);
            return queryData;
        }

        public async Task<IList<ProjectDashboardChartViewModel>> GetDatewiseSingleGroup(string templateId, string userId, DateTime? FromDate, DateTime? ToDate)
        {
            var queryData = await _projectManagementQueryBusiness.GetDatewiseSingleGroup(templateId, userId, FromDate, ToDate);




            var list = new List<ProjectDashboardChartViewModel>();
            //  var list1 = queryData.GroupBy(x => x.NtsStatus).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.NtsStatusId).FirstOrDefault() }).ToList();
            list = queryData.Select(x => new ProjectDashboardChartViewModel { Type = x.Type, Days = x.Days.Days, ActualSLA = x.ActualSLA.Days }).ToList();
            return list;
        }


        public async Task<ProgramDashboardViewModel> GetPerformanceDashboard()
        {
            var queryData = await _projectManagementQueryBusiness.GetPerformanceDashboard();
            return queryData;
        }


        public async Task<List<ProjectDashboardChartViewModel>> GetProjectStatus()
        {

            var queryData = await _projectManagementQueryBusiness.GetProjectStatus();
            return queryData;
        }


        public async Task<IList<ProjectDashboardChartViewModel>> GetTopfiveProject()
        {
            var queryData = await _projectManagementQueryBusiness.GetTopfiveProject();
            var list = queryData.ToList();
            return list;
        }


        public async Task<IList<ProjectDashboardChartViewModel>> GetTimeLog()
        {
            var queryData = await _projectManagementQueryBusiness.GetTimeLog();
            var list = new List<ProjectDashboardChartViewModel>();
            //  var list1 = queryData.GroupBy(x => x.NtsStatus).Select(group => new { Value = group.Count(), Type = group.Key, Id = group.Select(x => x.NtsStatusId).FirstOrDefault() }).ToList();
            list = queryData.Select(x => new ProjectDashboardChartViewModel { Type = x.Type, Days =Convert.ToInt32( x.Days.TotalHours), ActualSLA = x.ActualSLA.Days,Count1=x.Value }).ToList();
            return list;
        }


        public async Task<IList<ProgramDashboardViewModel>> GetProjectwiseUsers()
        {
            var queryData = await _projectManagementQueryBusiness.GetProjectwiseUsers();
            return queryData;
        }



        public async Task<IList<ProgramDashboardViewModel>> GetTaskDetails()
        {
            var queryData = await _projectManagementQueryBusiness.GetTaskDetails();
            return queryData;
        }
        public async Task<List<ProjectDashboardChartViewModel>> GetProjecTaskStatus()
        {
            var queryData = await _projectManagementQueryBusiness.GetProjecTaskStatus();
            return queryData;
        }
        public async Task<List<TaskWorkTimeViewModel>> GetHourReportTaskData(string serviceId)
        {
            var result = await _projectManagementQueryBusiness.GetHourReportTaskData(serviceId);
            return result;

        }
        public async Task<List<TaskWorkTimeViewModel>> GetHourReportProjectData(string projectId, string assigneeId, string sdate, string edate)
        {
            var result = await _projectManagementQueryBusiness.GetHourReportProjectData(projectId, assigneeId, sdate, edate);
            return result;
        }
        public async Task<IList<ProjectGanttTaskViewModel>> ReadProjectTimelineData()
        {
            var queryData = await _projectManagementQueryBusiness.ReadProjectTimelineData();
            return queryData;
        }

        public async Task<MemoryStream> GetHourSpentReportDataExcel(List<TaskWorkTimeViewModel> model, string projectId, string assigneeId, string sdate, string edate)
        {
            var ms = new MemoryStream();          
            using (var sl = new SLDocument())
            {
                sl.AddWorksheet("Project Daily Work Report");

                SLPageSettings pageSettings = new SLPageSettings();
                pageSettings.ShowGridLines = true;
                sl.SetPageSettings(pageSettings);

                sl.SetColumnWidth("A", 20);
                sl.SetColumnWidth("B", 20);
                sl.SetColumnWidth("C", 20);
                sl.SetColumnWidth("D", 20);
                sl.SetColumnWidth("E", 20);
                sl.SetColumnWidth("F", 20);
                sl.SetColumnWidth("G", 20);
                sl.SetColumnWidth("H", 20);
                sl.SetColumnWidth("I", 20);
                sl.SetColumnWidth("J", 20);
                sl.SetColumnWidth("K", 20);
                sl.SetColumnWidth("L", 20);
                sl.SetColumnWidth("M", 20);
                sl.SetColumnWidth("N", 20);
                sl.SetColumnWidth("O", 20);
                sl.SetColumnWidth("P", 20);
                sl.SetColumnWidth("R", 20);

                sl.MergeWorksheetCells("A1", "B1");
                sl.SetCellValue("A1", "");
                sl.SetCellStyle("A1", "B1", ExcelHelper.GetHeaderRowCayanStyle(sl));

                sl.MergeWorksheetCells("I1", "J1");
                sl.SetCellValue("I1", DateTime.Today.ToDefaultDateFormat());
                sl.SetCellStyle("I1", "J1", ExcelHelper.GetHeaderRowDateStyle(sl));

                sl.MergeWorksheetCells("A2", "H3");
                sl.SetCellValue("A2", "Project Daily Work Report " + DateTime.Now.ToShortDateString());
                sl.SetCellStyle("A2", "H3", ExcelHelper.GetReportHeadingStyle(sl));

                int row = 5;
                sl.MergeWorksheetCells(string.Concat("A", row), string.Concat("A", row));
                sl.SetCellValue(string.Concat("A", row), "Project Name");
                sl.SetCellStyle(string.Concat("A", row), string.Concat("A", row), ExcelHelper.GetShiftEntryDateStyle(sl));

                sl.MergeWorksheetCells(string.Concat("B", row), string.Concat("B", row));
                sl.SetCellValue(string.Concat("B", row), "Task Name");
                sl.SetCellStyle(string.Concat("B", row), string.Concat("B", row), ExcelHelper.GetShiftEntryDateStyle(sl));

                sl.MergeWorksheetCells(string.Concat("C", row), string.Concat("C", row));
                sl.SetCellValue(string.Concat("C", row), "Task No.");
                sl.SetCellStyle(string.Concat("C", row), string.Concat("C", row), ExcelHelper.GetShiftEntryDateStyle(sl));

                sl.MergeWorksheetCells(string.Concat("D", row), string.Concat("D", row));
                sl.SetCellValue(string.Concat("D", row), "Start Date");
                sl.SetCellStyle(string.Concat("D", row), string.Concat("D", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                //
                sl.MergeWorksheetCells(string.Concat("E", row), string.Concat("E", row));
                sl.SetCellValue(string.Concat("E", row), "Assignee Name");
                sl.SetCellStyle(string.Concat("E", row), string.Concat("E", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                //
                sl.MergeWorksheetCells(string.Concat("F", row), string.Concat("F", row));
                sl.SetCellValue(string.Concat("F", row), "Due Date");
                sl.SetCellStyle(string.Concat("F", row), string.Concat("F", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                //
                sl.MergeWorksheetCells(string.Concat("G", row), string.Concat("G", row));
                sl.SetCellValue(string.Concat("G", row), "Task Status");
                sl.SetCellStyle(string.Concat("G", row), string.Concat("G", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                //
                sl.MergeWorksheetCells(string.Concat("H", row), string.Concat("H", row));
                sl.SetCellValue(string.Concat("H", row), "SLA(In Days)");
                sl.SetCellStyle(string.Concat("H", row), string.Concat("H", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                row++;
                var projectList =await GetHourReportProjectData(projectId,assigneeId,sdate,edate);
                foreach (var modelData in projectList)
                {                    
                    if (modelData.IsNotNull())
                    {
                        sl.MergeWorksheetCells(string.Concat("A", row), string.Concat("A", row));
                       sl.SetCellValue(string.Concat("A", row), modelData.ProjectName.IsNotNull() ? modelData.ProjectName : "");
                       sl.SetCellStyle(string.Concat("A", row), string.Concat("A", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                        sl.MergeWorksheetCells(string.Concat("B", row), string.Concat("B", row));
                        sl.SetCellValue(string.Concat("B", row), modelData.TaskName.IsNotNull() ? modelData.TaskName : "");
                        sl.SetCellStyle(string.Concat("B", row), string.Concat("B", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                        sl.MergeWorksheetCells(string.Concat("C", row), string.Concat("C", row));
                        sl.SetCellValue(string.Concat("C", row), modelData.TaskNo.IsNotNull() ? modelData.TaskNo : "");
                        sl.SetCellStyle(string.Concat("C", row), string.Concat("C", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                        sl.MergeWorksheetCells(string.Concat("D", row), string.Concat("D", row));
                        sl.SetCellValue(string.Concat("D", row), modelData.TaskStartDate.IsNotNull() ? modelData.TaskStartDate.ToDD_MMM_YYYY_HHMMSS() : "");
                        sl.SetCellStyle(string.Concat("D", row), string.Concat("D", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                        sl.MergeWorksheetCells(string.Concat("E", row), string.Concat("E", row));
                        sl.SetCellValue(string.Concat("E", row), modelData.AssigneeName.IsNotNull() ? modelData.AssigneeName : "");
                        sl.SetCellStyle(string.Concat("E", row), string.Concat("E", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                        sl.MergeWorksheetCells(string.Concat("F", row), string.Concat("F", row));
                        sl.SetCellValue(string.Concat("F", row), modelData.TaskDueDate.IsNotNull() ? modelData.TaskDueDate.ToDD_MMM_YYYY_HHMMSS() : "");
                        sl.SetCellStyle(string.Concat("F", row), string.Concat("F", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                        sl.MergeWorksheetCells(string.Concat("G", row), string.Concat("G", row));
                        sl.SetCellValue(string.Concat("G", row), modelData.TaskStatusName.IsNotNull() ? modelData.TaskStatusName : "");
                        sl.SetCellStyle(string.Concat("G", row), string.Concat("G", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                        sl.MergeWorksheetCells(string.Concat("H", row), string.Concat("H", row));
                        sl.SetCellValue(string.Concat("H", row), modelData.SLA.IsNotNull() ? modelData.SLA.ToString() : "");
                        sl.SetCellStyle(string.Concat("H", row), string.Concat("H", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                               
                        row++;
                        sl.MergeWorksheetCells(string.Concat("B", row), string.Concat("B", row));
                        sl.SetCellValue(string.Concat("B", row), "Work Start Time");
                        sl.SetCellStyle(string.Concat("B", row), string.Concat("B", row), ExcelHelper.GetShiftEntryDateStyle(sl));

                        sl.MergeWorksheetCells(string.Concat("C", row), string.Concat("C", row));
                        sl.SetCellValue(string.Concat("C", row), "Work End Time");
                        sl.SetCellStyle(string.Concat("C", row), string.Concat("C", row), ExcelHelper.GetShiftEntryDateStyle(sl));

                        sl.MergeWorksheetCells(string.Concat("D", row), string.Concat("D", row));
                        sl.SetCellValue(string.Concat("D", row), "Duration");
                        sl.SetCellStyle(string.Concat("D", row), string.Concat("D", row), ExcelHelper.GetShiftEntryDateStyle(sl));

                        sl.MergeWorksheetCells(string.Concat("E", row), string.Concat("E", row));
                        sl.SetCellValue(string.Concat("E", row), "Work Comment");
                        sl.SetCellStyle(string.Concat("E", row), string.Concat("E", row), ExcelHelper.GetShiftEntryDateStyle(sl));
                        row++;
                        var timeentry = model.Where(x => x.ProjectId == modelData.ProjectId && x.TaskId== modelData.TaskId).GroupBy(x => x.ProjectId).ToList();
                        foreach (var ans in timeentry)
                        {
                            foreach (var time in ans)
                            {
                                sl.MergeWorksheetCells(string.Concat("B", row), string.Concat("B", row));
                                sl.SetCellValue(string.Concat("B", row), time.WSDate.IsNotNull() ? time.WSDate.Value.ToDD_MMM_YYYY_HHMMSS() : "");
                                sl.SetCellStyle(string.Concat("B", row), string.Concat("B", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                                sl.MergeWorksheetCells(string.Concat("C", row), string.Concat("C", row));
                                sl.SetCellValue(string.Concat("C", row), time.WEDate.IsNotNull() ? time.WEDate.Value.ToDD_MMM_YYYY_HHMMSS() : "");
                                sl.SetCellStyle(string.Concat("C", row), string.Concat("C", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                                sl.MergeWorksheetCells(string.Concat("D", row), string.Concat("D", row));
                                sl.SetCellValue(string.Concat("D", row), time.Duration.IsNotNull() ? time.Duration.Value.ToString() : "");
                                sl.SetCellStyle(string.Concat("D", row), string.Concat("D", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));

                                sl.MergeWorksheetCells(string.Concat("E", row), string.Concat("E", row));
                                sl.SetCellValue(string.Concat("E", row), time.WorkComment.IsNotNull() ? time.WorkComment : "");
                                sl.SetCellStyle(string.Concat("E", row), string.Concat("E", row), ExcelHelper.GetTextAssessmentQuestionnaireStyle(sl));
                                row++;
                            }
                        }
                    }
                }
                sl.DeleteWorksheet("Sheet1");
                sl.SaveAs(ms);
            }
            ms.Position = 0;
            return ms;
        }

        public async Task<List<UserHierarchyChartViewModel>> GetProjectHierarchy(string parentId, int levelUpto,string nodeType)
        {
            var list = new List<UserHierarchyChartViewModel>();

            if (levelUpto <= 0)
            {
                var queryData = await _projectManagementQueryBusiness.GetProjectHierarchyDataForLevelUpto0();

                list.AddRange(queryData);

                var model = new UserHierarchyChartViewModel()
                {
                    Id = "-1",
                    Name = "ProjectManagement",
                    DirectChildCount = queryData.Count()
                };
                list.Insert(0, model);
            }
            else if (levelUpto == 1)
            {
                var queryData = await _projectManagementQueryBusiness.GetProjectHierarchyDataForLevel1(parentId);

                list = queryData;
            }
            else if (levelUpto == 2)
            {
                var queryData = await _projectManagementQueryBusiness.GetProjectHierarchyDataForLevel2(parentId);

                list = queryData;
            }

            else if (nodeType == "Project" || nodeType == "Stage")
            {
                var queryData = await _projectManagementQueryBusiness.GetProjectHierarchyDataForNodeTypeProjectnStage(parentId);

                list = queryData;
            }
            else if (nodeType == "TaskUser")
            {
                var queryData = await _projectManagementQueryBusiness.GetProjectHierarchyDataForNodeTypeTaskUser(parentId);

                list = queryData;
            }
            else if (nodeType == "TaskStatus")
            {
                var queryData = await _projectManagementQueryBusiness.GetProjectHierarchyDataForNodeTypeTaskStatus(parentId);

                list = queryData;
            }
            
            foreach (var x in list)
            {
                x.Count = list.Where(x => x.ParentId == x.Id).Count();
            }
            return list;
        }
    }
}
