using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public class TicketManagementQueryPostgreBusiness : BusinessBase<NoteViewModel, NtsNote>, ITicketManagementQueryBusiness
    {
        IUserContext _uc;
        private readonly IRepositoryQueryBase<NoteViewModel> _queryRepo;
        public TicketManagementQueryPostgreBusiness(IRepositoryBase<NoteViewModel, NtsNote> repo
            , IMapper autoMapper
            , IUserContext uc
            , IRepositoryQueryBase<NoteViewModel> queryRepo) : base(repo, autoMapper)
        {
            _uc = uc;
            _queryRepo = queryRepo;
        }

        public async Task<List<HelpDeskViewModel>> GetRequestCountsByCategory()
        {
            var query = $@"select tc.""Id"" as Id, tc.""Name"" as Name,  Count(p1.""Id"") as InProgress, Count(p3.""Id"") as Draft, Count(p2.""Id"") as OverDue, Count(p4.""Id"") as Completed
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

            var result = await _queryRepo.ExecuteQueryList<HelpDeskViewModel>(query, null);
            return result;
        }

        public async Task<List<HelpDeskViewModel>> GetRequestCountsByService()
        {
            var query = $@"select t.""Id"" as Id, t.""DisplayName"" as Name,  Count(p1.""Id"") as InProgress, Count(p3.""Id"") as Draft, Count(p2.""Id"") as OverDue, Count(p4.""Id"") as Completed
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

            var result = await _queryRepo.ExecuteQueryList<HelpDeskViewModel>(query, null);
            return result;
        }

        public async Task<List<HelpDeskViewModel>> GetRequestCountsByPriority()
        {
            var query = $@"select p.""Id"" as Id, p.""Name"" ,  Count(p1.""Id"") as InProgress, Count(p3.""Id"") as Draft, Count(p2.""Id"") as OverDue, Count(p4.""Id"") as Completed
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

            var result = await _queryRepo.ExecuteQueryList<HelpDeskViewModel>(query, null);
            return result;
        }

        public async Task<List<HelpDeskViewModel>> GetRequestCountsByOwner()
        {
            var query = $@"select u.""Id"" as Id,u.""Name"" as Name, Count(p1.""Id"") as InProgress, Count(p3.""Id"") as Draft, Count(p2.""Id"") as OverDue, Count(p4.""Id"") as Completed
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

            var result = await _queryRepo.ExecuteQueryList<HelpDeskViewModel>(query, null);
            return result;
        }



        public async Task<List<HelpDeskViewModel>> GetRequestCountsSLAViolationByCategory()
        {
            var query = $@"select tc.""Id"" as Id, tc.""Name"" as Name,  Count(p1.""Id"") as InProgress, Count(p3.""Id"") as Draft, Count(p2.""Id"") as OverDue, Count(p4.""Id"") as Completed
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

            var result = await _queryRepo.ExecuteQueryList<HelpDeskViewModel>(query, null);
            return result;
        }
        public async Task<List<HelpDeskViewModel>> GetRequestCountsSLAViolationByService()
        {
            var query = $@"select t.""Id"" as Id, t.""DisplayName"" as Name,  Count(p1.""Id"") as InProgress, Count(p3.""Id"") as Draft, Count(p2.""Id"") as OverDue, Count(p4.""Id"") as Completed
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

            var result = await _queryRepo.ExecuteQueryList<HelpDeskViewModel>(query, null);
            return result;
        }
        public async Task<List<HelpDeskViewModel>> GetRequestCountsSLAViolationByPriority()
        {
            var query = $@"select p.""Id"" as Id, p.""Name"" ,  Count(p1.""Id"") as InProgress, Count(p3.""Id"") as Draft, Count(p2.""Id"") as OverDue, Count(p4.""Id"") as Completed
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

            var result = await _queryRepo.ExecuteQueryList<HelpDeskViewModel>(query, null);
            return result;
        }
        public async Task<List<HelpDeskViewModel>> GetRequestCountsSLAViolationByOwner()
        {
            var query = $@"select u.""Id"" as Id,u.""Name"" as Name, Count(p1.""Id"") as InProgress, Count(p3.""Id"") as Draft, Count(p2.""Id"") as OverDue, Count(p4.""Id"") as Completed
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

            var result = await _queryRepo.ExecuteQueryList<HelpDeskViewModel>(query, null);
            return result;
        }




        public async Task<List<HelpDeskViewModel>> GetChartDataThisWeek()
        {
            var query = $@"SELECT  to_char( s.""DueDate"", 'Day') AS ""Day"" , 
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

            var result = await _queryRepo.ExecuteQueryList<HelpDeskViewModel>(query, null);
            return result;
        }
        public async Task<List<HelpDeskViewModel>> GetChartDataLastWeek()
        {
            var query = $@"SELECT  to_char( s.""DueDate"", 'Day') AS ""Day"" , 
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

            var result = await _queryRepo.ExecuteQueryList<HelpDeskViewModel>(query, null);
            return result;
        }
        public async Task<List<HelpDeskViewModel>> GetChartDataThisMonth()
        {
            var query = $@"SELECT  to_char( s.""DueDate"", 'Month') AS ""Day"" , 
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

            var result = await _queryRepo.ExecuteQueryList<HelpDeskViewModel>(query, null);
            return result;
        }
        public async Task<List<HelpDeskViewModel>> GetChartDataLastMonth()
        {
            var query = $@"SELECT  to_char( s.""DueDate"", 'Month') AS ""Day"" , 
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

            var result = await _queryRepo.ExecuteQueryList<HelpDeskViewModel>(query, null);
            return result;
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
            var result = await _queryRepo.ExecuteQueryList<HelpDeskViewModel>(query, null);
            return result;
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
            var result = await _queryRepo.ExecuteQueryList<HelpDeskViewModel>(query, null);
            return result;
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
            var result = await _queryRepo.ExecuteQueryList<HelpDeskViewModel>(query, null);
            return result;
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

            var result = await _queryRepo.ExecuteQueryList<HelpDeskViewModel>(query, null);
            return result;
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

            var result = await _queryRepo.ExecuteQueryList<HelpDeskViewModel>(query, null);
            return result;
        }

    }
}
