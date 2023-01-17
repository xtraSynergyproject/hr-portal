using AutoMapper;
using CMS.Common;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Business
{
    public class TicketManagementBusiness : BusinessBase<NoteViewModel, NtsNote>, ITicketAssessmentBusiness
    {
        private readonly IRepositoryQueryBase<IdNameViewModel> _queryRepo;
        private readonly IRepositoryQueryBase<AssessmentQuestionsViewModel> _queryAssQues;
        private IUserContext _userContext;
        private IServiceBusiness _serviceBusiness;
        private INoteBusiness _noteBusiness;
        private readonly IServiceProvider _sp;
        private readonly IRepositoryQueryBase<AssessmentQuestionsOptionViewModel> _queryRepo1;
        private readonly IRepositoryQueryBase<AssignmentViewModel> _queryAssignment;
        public TicketManagementBusiness(IRepositoryBase<NoteViewModel, NtsNote> repo, IMapper autoMapper
            , IRepositoryQueryBase<AssessmentQuestionsOptionViewModel> queryRepo1, IServiceProvider sp,
            IUserContext userContext, IServiceBusiness serviceBusiness, INoteBusiness noteBusiness,
            IRepositoryQueryBase<IdNameViewModel> queryRepo, IRepositoryQueryBase<AssignmentViewModel> queryAssignment
           , IRepositoryQueryBase<AssessmentQuestionsViewModel> queryAssQues) : base(repo, autoMapper)
        {
            _queryRepo1 = queryRepo1;
            _sp = sp;
            _userContext = userContext;
            _serviceBusiness = serviceBusiness;
            _noteBusiness = noteBusiness;
            _queryRepo = queryRepo;
            _queryAssignment = queryAssignment;
            _queryAssQues = queryAssQues;
        }

        public async Task<List<HelpDeskViewModel>> GetRequestCounts(string type)
        {
            var query = "";
            if (type == "Category")
            {
                query = $@"select tc.""Id"" as Id, tc.""Name"" as Name,  Count(p1.""Id"") as InProgress, Count(p3.""Id"") as Draft, Count(p2.""Id"") as OverDue, Count(p4.""Id"") as Completed
                            from public.""Module"" as m
                              join public.""TemplateCategory"" as tc on tc.""ModuleId"" = m.""Id""
                              join public.""Template"" as t on t.""TemplateCategoryId"" = tc.""Id""
                             join public.""NtsService"" as s on s.""TemplateId"" = t.""Id""
                            --left join public.""LOV"" as p on p.""Id"" = s.""ServiceStatusId"" 
                            left join public.""LOV"" as p1 on p1.""Id"" = s.""ServiceStatusId"" and p1.""Code"" = 'SERVICE_STATUS_INPROGRESS'
                            --left join public.""LOV"" as p2 on p2.""Id"" = s.""ServiceStatusId"" 
                            left join public.""LOV"" as p3 on p3.""Id"" = s.""ServiceStatusId"" and p3.""Code"" = 'SERVICE_STATUS_DRAFT'
                            left join public.""LOV"" as p2 on p2.""Id"" = s.""ServiceStatusId"" and p2.""Code"" = 'SERVICE_STATUS_OVERDUE'
                            left join public.""LOV"" as p4 on p4.""Id"" = s.""ServiceStatusId"" and p4.""Code"" = 'SERVICE_STATUS_COMPLETE'
                            where m.""Code"" = 'TMS_MODULE' and t.""TemplateType"" = 6
                            GROUP BY  tc.""Name"", tc.""Id""";
            }
            else if (type == "Service")
            {
                query = $@"select t.""Id"" as Id, t.""DisplayName"" as Name,  Count(p1.""Id"") as InProgress, Count(p3.""Id"") as Draft, Count(p2.""Id"") as OverDue, Count(p4.""Id"") as Completed
                            from public.""Module"" as m
                              join public.""TemplateCategory"" as tc on tc.""ModuleId"" = m.""Id""
                              left join public.""Template"" as t on t.""TemplateCategoryId"" = tc.""Id""
                             left join public.""NtsService"" as s on s.""TemplateId"" = t.""Id""
                            --left join public.""LOV"" as p on p.""Id"" = s.""ServiceStatusId"" 
                            left join public.""LOV"" as p1 on p1.""Id"" = s.""ServiceStatusId"" and p1.""Code"" = 'SERVICE_STATUS_INPROGRESS'
                            --left join public.""LOV"" as p2 on p2.""Id"" = s.""ServiceStatusId"" 
                            left join public.""LOV"" as p3 on p3.""Id"" = s.""ServiceStatusId"" and p3.""Code"" = 'SERVICE_STATUS_DRAFT'
                            left join public.""LOV"" as p2 on p2.""Id"" = s.""ServiceStatusId"" and p2.""Code"" = 'SERVICE_STATUS_OVERDUE'
                            left join public.""LOV"" as p4 on p4.""Id"" = s.""ServiceStatusId"" and p4.""Code"" = 'SERVICE_STATUS_COMPLETE'
                            where m.""Code"" = 'TMS_MODULE' and t.""TemplateType"" = 6
                            GROUP BY  t.""Name"", t.""Id""";
            }
            else if (type == "Priority")
            {
                query = $@"select p.""Id"" as Id, p.""Name"" ,  Count(p1.""Id"") as InProgress, Count(p3.""Id"") as Draft, Count(p2.""Id"") as OverDue, Count(p4.""Id"") as Completed
                            from
                            public.""LOV"" as p
                             left join public.""NtsService"" as s on s.""ServicePriorityId"" = p.""Id""
                             left join public.""Template"" as t on t.""Id"" = s.""TemplateId""
                             left join public.""TemplateCategory"" as tc on tc.""Id"" = t.""TemplateCategoryId""
                            left join public.""Module"" as m on m.""Id"" = tc.""ModuleId""
                            left join public.""LOV"" as p1 on p1.""Id"" = s.""ServiceStatusId"" and p1.""Code"" = 'SERVICE_STATUS_INPROGRESS'
                            left join public.""LOV"" as p3 on p3.""Id"" = s.""ServiceStatusId"" and p3.""Code"" = 'SERVICE_STATUS_DRAFT'
                            left join public.""LOV"" as p2 on p2.""Id"" = s.""ServiceStatusId"" and p2.""Code"" = 'SERVICE_STATUS_OVERDUE'
                            left join public.""LOV"" as p4 on p4.""Id"" = s.""ServiceStatusId"" and p4.""Code"" = 'SERVICE_STATUS_COMPLETE'
                            where m.""Code"" = 'TMS_MODULE' --and t.""TemplateType"" = 6
                            GROUP BY  p.""Id""";
            } else if (type == "Owner")
            {
                query = $@"select u.""Id"" as Id,u.""Name"" as Name, Count(p1.""Id"") as InProgress, Count(p3.""Id"") as Draft, Count(p2.""Id"") as OverDue, Count(p4.""Id"") as Completed
                            from
                            public.""LOV"" as p
                             left join public.""NtsService"" as s on s.""ServicePriorityId"" = p.""Id""
                             left join public.""User"" as u on u.""Id"" = s.""OwnerUserId""
                             left join public.""Template"" as t on t.""Id"" = s.""TemplateId""
                             left join public.""TemplateCategory"" as tc on tc.""Id"" = t.""TemplateCategoryId""
                            left join public.""Module"" as m on m.""Id"" = tc.""ModuleId""
                            left join public.""LOV"" as p1 on p1.""Id"" = s.""ServiceStatusId"" and p1.""Code"" = 'SERVICE_STATUS_INPROGRESS'
                            left join public.""LOV"" as p3 on p3.""Id"" = s.""ServiceStatusId"" and p3.""Code"" = 'SERVICE_STATUS_DRAFT'
                            left join public.""LOV"" as p2 on p2.""Id"" = s.""ServiceStatusId"" and p2.""Code"" = 'SERVICE_STATUS_OVERDUE'
                            left join public.""LOV"" as p4 on p4.""Id"" = s.""ServiceStatusId"" and p4.""Code"" = 'SERVICE_STATUS_COMPLETE'
                            where m.""Code"" = 'TMS_MODULE' and t.""TemplateType"" = 6
                            GROUP BY  u.""Id"", u.""Name""
                        ";
            }
            return await _queryRepo.ExecuteQueryList<HelpDeskViewModel>(query, null);
        }

        public async Task<List<HelpDeskViewModel>> GetRequestCountsSLAViolation(string type)
        {
            var query = "";
            if (type == "Category")
            {
                query = $@"select tc.""Id"" as Id, tc.""Name"" as Name,  Count(p1.""Id"") as InProgress, Count(p3.""Id"") as Draft, Count(p2.""Id"") as OverDue, Count(p4.""Id"") as Completed
                            from public.""Module"" as m
                              join public.""TemplateCategory"" as tc on tc.""ModuleId"" = m.""Id""
                              join public.""Template"" as t on t.""TemplateCategoryId"" = tc.""Id""
                             join public.""NtsService"" as s on s.""TemplateId"" = t.""Id""
                            left join public.""LOV"" as p1 on p1.""Id"" = s.""ServiceStatusId"" and p1.""Code"" = 'SERVICE_STATUS_INPROGRESS'
                            left join public.""LOV"" as p3 on p3.""Id"" = s.""ServiceStatusId"" and p3.""Code"" = 'SERVICE_STATUS_DRAFT'
                            left join public.""LOV"" as p2 on p2.""Id"" = s.""ServiceStatusId"" and p2.""Code"" = 'SERVICE_STATUS_OVERDUE'
                            left join public.""LOV"" as p4 on p4.""Id"" = s.""ServiceStatusId"" and p4.""Code"" = 'SERVICE_STATUS_COMPLETE'
                            where m.""Code"" = 'TMS_MODULE' and t.""TemplateType"" = 6 and (s.""CompletedDate"" > s.""DueDate"" )
                            GROUP BY  tc.""Name"", tc.""Id""";
            }
            else if (type == "Service")
            {
                query = $@"select t.""Id"" as Id, t.""DisplayName"" as Name,  Count(p1.""Id"") as InProgress, Count(p3.""Id"") as Draft, Count(p2.""Id"") as OverDue, Count(p4.""Id"") as Completed
                            from public.""Module"" as m
                              join public.""TemplateCategory"" as tc on tc.""ModuleId"" = m.""Id""
                              left join public.""Template"" as t on t.""TemplateCategoryId"" = tc.""Id""
                             left join public.""NtsService"" as s on s.""TemplateId"" = t.""Id""
                            left join public.""LOV"" as p1 on p1.""Id"" = s.""ServiceStatusId"" and p1.""Code"" = 'SERVICE_STATUS_INPROGRESS'
                            left join public.""LOV"" as p3 on p3.""Id"" = s.""ServiceStatusId"" and p3.""Code"" = 'SERVICE_STATUS_DRAFT'
                            left join public.""LOV"" as p2 on p2.""Id"" = s.""ServiceStatusId"" and p2.""Code"" = 'SERVICE_STATUS_OVERDUE'
                            left join public.""LOV"" as p4 on p4.""Id"" = s.""ServiceStatusId"" and p4.""Code"" = 'SERVICE_STATUS_COMPLETE'
                            where m.""Code"" = 'TMS_MODULE' and t.""TemplateType"" = 6 and (s.""CompletedDate"" > s.""DueDate"" )
                            GROUP BY  t.""Name"", t.""Id""";
            }
            else if (type == "Priority")
            {
                query = $@"select p.""Id"" as Id, p.""Name"" ,  Count(p1.""Id"") as InProgress, Count(p3.""Id"") as Draft, Count(p2.""Id"") as OverDue, Count(p4.""Id"") as Completed
                            from
                            public.""LOV"" as p
                             left join public.""NtsService"" as s on s.""ServicePriorityId"" = p.""Id""
                             left join public.""Template"" as t on t.""Id"" = s.""TemplateId""
                             left join public.""TemplateCategory"" as tc on tc.""Id"" = t.""TemplateCategoryId""
                            left join public.""Module"" as m on m.""Id"" = tc.""ModuleId""
                            left join public.""LOV"" as p1 on p1.""Id"" = s.""ServiceStatusId"" and p1.""Code"" = 'SERVICE_STATUS_INPROGRESS'
                            left join public.""LOV"" as p3 on p3.""Id"" = s.""ServiceStatusId"" and p3.""Code"" = 'SERVICE_STATUS_DRAFT'
                            left join public.""LOV"" as p2 on p2.""Id"" = s.""ServiceStatusId"" and p2.""Code"" = 'SERVICE_STATUS_OVERDUE'
                            left join public.""LOV"" as p4 on p4.""Id"" = s.""ServiceStatusId"" and p4.""Code"" = 'SERVICE_STATUS_COMPLETE'
                           
                            where m.""Code"" = 'TMS_MODULE' and t.""TemplateType"" = 6 and (s.""CompletedDate"" > s.""DueDate"" )
                            GROUP BY  p.""Id""";
            }
            else if (type == "Owner")
            {
                query = $@"select u.""Id"" as Id,u.""Name"" as Name, Count(p1.""Id"") as InProgress, Count(p3.""Id"") as Draft, Count(p2.""Id"") as OverDue, Count(p4.""Id"") as Completed
                            from
                            public.""LOV"" as p
                             left join public.""NtsService"" as s on s.""ServicePriorityId"" = p.""Id""
                             left join public.""User"" as u on u.""Id"" = s.""OwnerUserId""
                             left join public.""Template"" as t on t.""Id"" = s.""TemplateId""
                             left join public.""TemplateCategory"" as tc on tc.""Id"" = t.""TemplateCategoryId""
                            left join public.""Module"" as m on m.""Id"" = tc.""ModuleId""
                            left join public.""LOV"" as p1 on p1.""Id"" = s.""ServiceStatusId"" and p1.""Code"" = 'SERVICE_STATUS_INPROGRESS'
                            left join public.""LOV"" as p3 on p3.""Id"" = s.""ServiceStatusId"" and p3.""Code"" = 'SERVICE_STATUS_DRAFT'
                            left join public.""LOV"" as p2 on p2.""Id"" = s.""ServiceStatusId"" and p2.""Code"" = 'SERVICE_STATUS_OVERDUE'
                            left join public.""LOV"" as p4 on p4.""Id"" = s.""ServiceStatusId"" and p4.""Code"" = 'SERVICE_STATUS_COMPLETE'
                            where m.""Code"" = 'TMS_MODULE' and t.""TemplateType"" = 6 and (s.""CompletedDate"" > s.""DueDate"" )
                            GROUP BY  u.""Id"", u.""Name""
                        ";
            }
            return await _queryRepo.ExecuteQueryList<HelpDeskViewModel>(query, null);
        }

        public async Task<List<HelpDeskViewModel>> GetChartData(string type)
        {
            var query = "";

            if (type == "thisweek")
            {
                query = $@"SELECT  to_char( s.""DueDate"", 'Day') AS ""Day"" , 
                         Count(p1.""Id"") as InProgress, Count(p3.""Id"") as Draft, Count(p2.""Id"") as OverDue, Count(p4.""Id"") as Completed
                                                    from public.""Module"" as m
                                                      join public.""TemplateCategory"" as tc on tc.""ModuleId"" = m.""Id""
                                                      join public.""Template"" as t on t.""TemplateCategoryId"" = tc.""Id""
                                                     join public.""NtsService"" as s on s.""TemplateId"" = t.""Id""
                                                    left join public.""LOV"" as p1 on p1.""Id"" = s.""ServiceStatusId"" and p1.""Code"" = 'SERVICE_STATUS_INPROGRESS'
                                                    left join public.""LOV"" as p3 on p3.""Id"" = s.""ServiceStatusId"" and p3.""Code"" = 'SERVICE_STATUS_DRAFT'
                                                    left join public.""LOV"" as p2 on p2.""Id"" = s.""ServiceStatusId"" and p2.""Code"" = 'SERVICE_STATUS_OVERDUE'
                                                    left join public.""LOV"" as p4 on p4.""Id"" = s.""ServiceStatusId"" and p4.""Code"" = 'SERVICE_STATUS_COMPLETE'
                                                    where m.""Code"" = 'TMS_MODULE' and t.""TemplateType"" = 6
                        and (s.""DueDate"" >= date_trunc('week', CURRENT_TIMESTAMP) and
                               s.""DueDate"" < date_trunc('week', CURRENT_TIMESTAMP  + interval '1 week')
                              )
                         GROUP BY  ""Day""
                        ";
            } else if (type == "lastweek")
            {
                query = $@"SELECT  to_char( s.""DueDate"", 'Day') AS ""Day"" , 
                         Count(p1.""Id"") as InProgress, Count(p3.""Id"") as Draft, Count(p2.""Id"") as OverDue, Count(p4.""Id"") as Completed
                                                    from public.""Module"" as m
                                                      join public.""TemplateCategory"" as tc on tc.""ModuleId"" = m.""Id""
                                                      join public.""Template"" as t on t.""TemplateCategoryId"" = tc.""Id""
                                                     join public.""NtsService"" as s on s.""TemplateId"" = t.""Id""
                                                    left join public.""LOV"" as p1 on p1.""Id"" = s.""ServiceStatusId"" and p1.""Code"" = 'SERVICE_STATUS_INPROGRESS'
                                                    left join public.""LOV"" as p3 on p3.""Id"" = s.""ServiceStatusId"" and p3.""Code"" = 'SERVICE_STATUS_DRAFT'
                                                    left join public.""LOV"" as p2 on p2.""Id"" = s.""ServiceStatusId"" and p2.""Code"" = 'SERVICE_STATUS_OVERDUE'
                                                    left join public.""LOV"" as p4 on p4.""Id"" = s.""ServiceStatusId"" and p4.""Code"" = 'SERVICE_STATUS_COMPLETE'
                                                    where m.""Code"" = 'TMS_MODULE' and t.""TemplateType"" = 6
                          
                        and (s.""DueDate"" >= date_trunc('week', CURRENT_TIMESTAMP  - interval '1 week') and
                               s.""DueDate"" < date_trunc('week', CURRENT_TIMESTAMP )
                              )
                         GROUP BY  ""Day""
                        ";
            }
            else if (type == "thismonth")
            {
                query = $@"SELECT  to_char( s.""DueDate"", 'Month') AS ""Day"" , 
                         Count(p1.""Id"") as InProgress, Count(p3.""Id"") as Draft, Count(p2.""Id"") as OverDue, Count(p4.""Id"") as Completed
                                                    from public.""Module"" as m
                                                      join public.""TemplateCategory"" as tc on tc.""ModuleId"" = m.""Id""
                                                      join public.""Template"" as t on t.""TemplateCategoryId"" = tc.""Id""
                                                     join public.""NtsService"" as s on s.""TemplateId"" = t.""Id""
                                                    left join public.""LOV"" as p1 on p1.""Id"" = s.""ServiceStatusId"" and p1.""Code"" = 'SERVICE_STATUS_INPROGRESS'
                                                    left join public.""LOV"" as p3 on p3.""Id"" = s.""ServiceStatusId"" and p3.""Code"" = 'SERVICE_STATUS_DRAFT'
                                                    left join public.""LOV"" as p2 on p2.""Id"" = s.""ServiceStatusId"" and p2.""Code"" = 'SERVICE_STATUS_OVERDUE'
                                                    left join public.""LOV"" as p4 on p4.""Id"" = s.""ServiceStatusId"" and p4.""Code"" = 'SERVICE_STATUS_COMPLETE'
                                                    where m.""Code"" = 'TMS_MODULE' and t.""TemplateType"" = 6
                        and (s.""DueDate"" >= date_trunc('month', CURRENT_TIMESTAMP) and
                               s.""DueDate"" < date_trunc('month', CURRENT_TIMESTAMP  + interval '1 month')
                              )
                         GROUP BY  ""Day""";
            }else if (type == "lastMonth")
            {
                query = $@"SELECT  to_char( s.""DueDate"", 'Month') AS ""Day"" , 
                 Count(p1.""Id"") as InProgress, Count(p3.""Id"") as Draft, Count(p2.""Id"") as OverDue, Count(p4.""Id"") as Completed
                                            from public.""Module"" as m
                                              join public.""TemplateCategory"" as tc on tc.""ModuleId"" = m.""Id""
                                              join public.""Template"" as t on t.""TemplateCategoryId"" = tc.""Id""
                                             join public.""NtsService"" as s on s.""TemplateId"" = t.""Id""
                                            left join public.""LOV"" as p1 on p1.""Id"" = s.""ServiceStatusId"" and p1.""Code"" = 'SERVICE_STATUS_INPROGRESS'
                                            left join public.""LOV"" as p3 on p3.""Id"" = s.""ServiceStatusId"" and p3.""Code"" = 'SERVICE_STATUS_DRAFT'
                                            left join public.""LOV"" as p2 on p2.""Id"" = s.""ServiceStatusId"" and p2.""Code"" = 'SERVICE_STATUS_OVERDUE'
                                            left join public.""LOV"" as p4 on p4.""Id"" = s.""ServiceStatusId"" and p4.""Code"" = 'SERVICE_STATUS_COMPLETE'
                                            where m.""Code"" = 'TMS_MODULE' and t.""TemplateType"" = 6
                          
                and (s.""DueDate"" >= date_trunc('month', CURRENT_TIMESTAMP  - interval '1 month') and
                       s.""DueDate"" < date_trunc('month', CURRENT_TIMESTAMP )
                      )
                 GROUP BY  ""Day""
                ";
            }
            return await _queryRepo.ExecuteQueryList<HelpDeskViewModel>(query, null);
        }

        public async Task<List<HelpDeskViewModel>> GetPendingTaskCountWithAssignee()
        {
            var query = $@"select u.""Name"" as Name, Count(p1.""Id"") as InProgress, Count(p3.""Id"") as Draft, Count(p2.""Id"") as OverDue, Count(p4.""Id"") as Completed
                            from public.""Module"" as m
                              join public.""TemplateCategory"" as tc on tc.""ModuleId"" = m.""Id""
                              join public.""Template"" as t on t.""TemplateCategoryId"" = tc.""Id""
                             join public.""NtsService"" as s on s.""TemplateId"" = t.""Id""
							 join public.""NtsTask"" as ta on ta.""ParentServiceId"" = s.""Id""
							 join public.""User"" as u on u.""Id"" = ta.""AssignedToUserId""
                            left join public.""LOV"" as p1 on p1.""Id"" = s.""ServiceStatusId"" and p1.""Code"" = 'SERVICE_STATUS_INPROGRESS'
							left join public.""LOV"" as p3 on p3.""Id"" = s.""ServiceStatusId"" and p3.""Code"" = 'SERVICE_STATUS_DRAFT'
                                            left join public.""LOV"" as p2 on p2.""Id"" = s.""ServiceStatusId"" and p2.""Code"" = 'SERVICE_STATUS_OVERDUE'
                                            left join public.""LOV"" as p4 on p4.""Id"" = s.""ServiceStatusId"" and p4.""Code"" = 'SERVICE_STATUS_COMPLETE'
                            where m.""Code"" = 'TMS_MODULE' and t.""TemplateType"" = 6
                            GROUP BY  u.""Name""";
            return await _queryRepo.ExecuteQueryList<HelpDeskViewModel>(query, null);
        }
        
        public async Task<List<HelpDeskViewModel>> GetServiceSLAVoilationInLast20Days()
        {
            var query = $@"SELECT   to_char( s.""CreatedDate"", 'DD') AS ""Day"", Count( p2.""Code"") as Violated,Count( p3.""Code"") as NonViolated
                        FROM public.""Module"" as m
                                                      join public.""TemplateCategory"" as tc on tc.""ModuleId"" = m.""Id""
                                                      join public.""Template"" as t on t.""TemplateCategoryId"" = tc.""Id""
                                                     join public.""NtsService"" as s on s.""TemplateId"" = t.""Id""
                         left join public.""LOV"" as p2 on p2.""Id"" = s.""ServiceStatusId"" and p2.""Code"" = 'SERVICE_STATUS_OVERDUE'
                          left join public.""LOV"" as p3 on p3.""Id"" = s.""ServiceStatusId"" and p3.""Code"" <> 'SERVICE_STATUS_OVERDUE'

                        where m.""Code"" = 'TMS_MODULE' and t.""TemplateType"" = 6
                         and (s.""CreatedDate"" >= CURRENT_TIMESTAMP  - interval '20 day' 
                                              ) and s.""IsDeleted"" = false
					  
					                          Group by 1, p2.""Code""";
            return await _queryRepo.ExecuteQueryList<HelpDeskViewModel>(query, null);
        } 
        
        
        public async Task<List<HelpDeskViewModel>> GetServiceSLAVoilationCompletedIn20Days()
        {
            var query = $@"SELECT   to_char( s.""DueDate"", 'DD') AS ""Day"", 0 as Voilated,Count( p2.""Code"") as NonVoilated
                                FROM public.""Module"" as m
                                                  join public.""TemplateCategory"" as tc on tc.""ModuleId"" = m.""Id""
                                                  join public.""Template"" as t on t.""TemplateCategoryId"" = tc.""Id""
                                                 join public.""NtsService"" as s on s.""TemplateId"" = t.""Id""
                     left join public.""LOV"" as p2 on p2.""Id"" = s.""ServiceStatusId"" and p2.""Code"" = 'SERVICE_STATUS_COMPLETE'
                    where m.""Code"" = 'TMS_MODULE' and t.""TemplateType"" = 6
                     and (s.""DueDate"" >= CURRENT_TIMESTAMP  - interval '20 day' 
                                          ) and s.""IsDeleted"" = false
					                      Group by 1, p2.""Code""";                                                                                                                                               
            return await _queryRepo.ExecuteQueryList<HelpDeskViewModel>(query, null);
        }

        public async Task<List<HelpDeskViewModel>> GetRequestForServiceChart(string type)
        {
            var query = $@"select tc.""Id"" as Id, tc.""Name"" as Name,tc.""Code"" as CategoryCode,sp.""Name"" as Priority,t.""Code"" as TemplateCode,t.""DisplayName"" as TemplateName,s.""DueDate"" as DueDate
-- Count(p1.""Id"") as InProgress, Count(p3.""Id"") as Draft, Count(p2.""Id"") as OverDue, Count(p4.""Id"") as Completed
                            from public.""Module"" as m
                              join public.""TemplateCategory"" as tc on tc.""ModuleId"" = m.""Id"" and m.""IsDeleted""=false and tc.""IsDeleted""=false
                              join public.""Template"" as t on t.""TemplateCategoryId"" = tc.""Id""  and t.""IsDeleted""=false
                             join public.""NtsService"" as s on s.""TemplateId"" = t.""Id""
                             join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId""  and sp.""IsDeleted""=false
                             join public.""LOV"" as p1 on p1.""Id"" = s.""ServiceStatusId"" and  p1.""Code"" in ('SERVICE_STATUS_INPROGRESS','SERVICE_STATUS_OVERDUE') and p1.""IsDeleted""=false  
                            --left join public.""LOV"" as p2 on p2.""Id"" = s.""ServiceStatusId""  and s.""IsDeleted""=false and p2.""IsDeleted""=false  
                            left join public.""LOV"" as p3 on p3.""Id"" = s.""ServiceStatusId"" and p3.""Code"" = 'SERVICE_STATUS_DRAFT'
                            left join public.""LOV"" as p2 on p2.""Id"" = s.""ServiceStatusId"" and p2.""Code"" = 'SERVICE_STATUS_OVERDUE'
                            --left join public.""LOV"" as p4 on p4.""Id"" = s.""ServiceStatusId"" and p4.""Code"" = 'SERVICE_STATUS_COMPLETE'
                            where m.""Code"" = 'TMS_MODULE' and t.""TemplateType"" = 6
                            --GROUP BY  tc.""Name"", tc.""Id""";
           
            return await _queryRepo.ExecuteQueryList<HelpDeskViewModel>(query, null);
        }
        public async Task<List<HelpDeskViewModel>> GetOverDueRequest()
        {
            var query = $@"select tc.""Id"" as Id, tc.""Name"" as Name,tc.""Code"" as CategoryCode,sp.""Name"" as Priority,t.""Code"" as TemplateCode,t.""DisplayName"" as TemplateName

                            from public.""Module"" as m
                              join public.""TemplateCategory"" as tc on tc.""ModuleId"" = m.""Id"" and m.""IsDeleted""=false and tc.""IsDeleted""=false
                              join public.""Template"" as t on t.""TemplateCategoryId"" = tc.""Id"" and t.""IsDeleted""=false
                             join public.""NtsService"" as s on s.""TemplateId"" = t.""Id"" and s.""IsDeleted""=false
                            left join public.""LOV"" as sp on sp.""Id"" = s.""ServicePriorityId"" and sp.""IsDeleted""=false
                            left join public.""LOV"" as p1 on p1.""Id"" = s.""ServiceStatusId"" and p1.""IsDeleted""=false  
                            where m.""Code"" = 'TMS_MODULE' and t.""TemplateType"" = 6 and s.""DueDate""::Date<'{DateTime.Now}'::Date and p1.""Code"" in ('SERVICE_STATUS_INPROGRESS','SERVICE_STATUS_OVERDUE')
                            ";

            return await _queryRepo.ExecuteQueryList<HelpDeskViewModel>(query, null);
        }
    }
}
