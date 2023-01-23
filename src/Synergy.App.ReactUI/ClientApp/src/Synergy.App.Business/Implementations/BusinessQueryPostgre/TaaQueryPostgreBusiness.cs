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
    public class TaaQueryPostgreBusiness : BusinessBase<NoteViewModel, NtsNote>, ITaaQueryBusiness
    {
        IUserContext _uc;
        private readonly IRepositoryQueryBase<NoteViewModel> _queryRepo;
        private readonly ITableMetadataBusiness _tableMetadataBusiness;
        public TaaQueryPostgreBusiness(IRepositoryBase<NoteViewModel, NtsNote> repo
            , IMapper autoMapper
            , IUserContext uc
            , ITableMetadataBusiness tableMetadataBusiness
            , IRepositoryQueryBase<NoteViewModel> queryRepo) : base(repo, autoMapper)
        {
            _uc = uc;
            _queryRepo = queryRepo;
            _tableMetadataBusiness = tableMetadataBusiness;
        }

        public async Task<List<IdNameViewModel>> GetDepartmentList()
        {
            var companyquery = Helper.OrganizationMapping(_uc.UserId, _uc.CompanyId, _uc.LegalEntityId);
            string query = $@"{companyquery} select d.""Id"" as Id ,d.""DepartmentName"" as Name
                            from cms.""N_CoreHR_HRDepartment"" as d 
                            join  ""Department"" as dept on dept.""DepartmentId""=d.""Id""
                            where d.""IsDeleted""=false and d.""CompanyId""='{_repo.UserContext.CompanyId}'
                            ";
            //query = query.Replace("#comp#", companyquery);
            var queryData = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return queryData;
        }

        public async Task<List<IdNameViewModel>> GetShiftPatternList()
        {

            var query =
                $@"SELECT rd.""Id"" as Id ,rd.""Name"" as Name FROM cms.""N_TAA_RosterDutyTemplate"" as rd
                    where rd.""IsDeleted""=false and rd.""CompanyId""='{_repo.UserContext.CompanyId}'
                        ";
            var queryData = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return queryData;
        }

        public async Task<RosterScheduleViewModel> GetExistingShiftPattern(string userId, string rostarDate)
        {

            var query =
                $@"SELECT rd.""NtsNoteId"" as NoteId ,rd.* FROM cms.""N_TAA_RosterSchedule"" as rd
                    where rd.""IsDeleted""=false and rd.""UserId""='{userId}' and substring(rd.""RosterDate"",0,11)='{rostarDate}' and rd.""CompanyId""='{_repo.UserContext.CompanyId}'
                        ";
            var queryData = await _queryRepo.ExecuteQuerySingle<RosterScheduleViewModel>(query, null);
            return queryData;
        }

        public async Task<RosterDutyTemplateViewModel> GetRosterDutyTemplateById(string Id)
        {
            var query =
                $@"SELECT rd.* FROM cms.""N_TAA_RosterDutyTemplate"" as rd
                      where rd.""Id""='{Id}' and rd.""IsDeleted""=false and rd.""CompanyId""='{_repo.UserContext.CompanyId}' ";

            var queryData = await _queryRepo.ExecuteQuerySingle<RosterDutyTemplateViewModel>(query, null);

            return queryData;
        }

        public async Task<List<RosterScheduleViewModel>> GetRosterSchedulerList(string orgId, DateTime sun, DateTime mon, DateTime tue, DateTime wed, DateTime thu, DateTime fri, DateTime sat, string LegalEntityCode)
        {
            var timeDiff = LegalEntityCode.ServerToLocalTimeDiff();
            var query = $@"select u.""Id"" as UserId,p.""Id"" as PersonId,p.""IqamahNoNationalId"" as SponsorshipNo,p.""PersonNo"" as PersonNo
,j.""JobTitle"" as JobName,n.""NationalityName"" as Nationality,d.""DepartmentName"" as OrganizationName
,c.""EffectiveEndDate"" as ContractEndDate,c.""ContractRenewable"" as ContractRenewable,sp.""Code"" as Sponsor
,a.""Id"" as AssignmentId,d.""Id"",p.""PersonFullName"" as EmployeeName
,case when count(case when nts.""LeaveStartDate"" <= '{sat}' and '{sat}' <= nts.""LeaveEndDate"" THEN 'L' END)>0 THEN 'L' END  as Sunday,
            case when count(case when nts.""LeaveStartDate"" <= '{sun}' and '{sun}' <= nts.""LeaveEndDate"" THEN 'L' END)>0 THEN 'L' END  as Monday,
            case when count(case when nts.""LeaveStartDate"" <= '{mon}' and '{mon}' <= nts.""LeaveEndDate"" THEN 'L' END)>0 THEN 'L' END  as Tuesday,
            case when count(case when nts.""LeaveStartDate"" <= '{tue}' and '{tue}' <= nts.""LeaveEndDate"" THEN 'L' END)>0 THEN 'L' END  as Wednesday,
            case when count(case when nts.""LeaveStartDate"" <= '{wed}' and '{wed}' <= nts.""LeaveEndDate"" THEN 'L' END)>0 THEN 'L' END  as Thursday,
            case when count(case when nts.""LeaveStartDate"" <= '{thu}' and '{thu}' <= nts.""LeaveEndDate"" THEN 'L' END)>0 THEN 'L' END  as Friday,
            case when count(case when nts.""LeaveStartDate"" <= '{fri}' and '{fri}' <= nts.""LeaveEndDate"" THEN 'L' END)>0 THEN 'L' END  as Saturday,
			            
coalesce(substring(rs1.""DraftDuty1StartTime"", 0, 6) || '-' || substring(rs1.""DraftDuty1EndTime"", 0, 6),'') || coalesce('<br/>' || substring(rs1.""DraftDuty2StartTime"", 0, 6) || '-' || substring(rs1.""DraftDuty2EndTime"", 0, 6),'') || coalesce('<br/>' || substring(rs1.""DraftDuty3StartTime"", 0, 6) || '-' || substring(rs1.""DraftDuty3EndTime"", 0, 6),'') || coalesce(case when rs1.""DraftDuty1FallsNextDay""='true' or rs1.""DraftDuty2FallsOnNextDay""='true' or rs1.""DraftDuty3FallsNextDay""='true' then '(D+)' else ' ' end,'') as SundayText,
coalesce(substring(rs2.""DraftDuty1StartTime"", 0, 6) || '-' || substring(rs2.""DraftDuty1EndTime"", 0, 6),'') || coalesce('<br/>' || substring(rs2.""DraftDuty2StartTime"", 0, 6) || '-' || substring(rs2.""DraftDuty2EndTime"", 0, 6),'') || coalesce('<br/>' || substring(rs2.""DraftDuty3StartTime"", 0, 6) || '-' || substring(rs2.""DraftDuty3EndTime"", 0, 6),'') || coalesce(case when rs2.""DraftDuty1FallsNextDay""='true' or rs2.""DraftDuty2FallsOnNextDay""='true' or rs2.""DraftDuty3FallsNextDay""='true' then '(D+)' else ' ' end,'') as MondayText,   
coalesce(substring(rs3.""DraftDuty1StartTime"", 0, 6) || '-' || substring(rs3.""DraftDuty1EndTime"", 0, 6),'') || coalesce('<br/>' || substring(rs3.""DraftDuty2StartTime"", 0, 6) || '-' || substring(rs3.""DraftDuty2EndTime"", 0, 6),'') || coalesce('<br/>' || substring(rs3.""DraftDuty3StartTime"", 0, 6) || '-' || substring(rs3.""DraftDuty3EndTime"", 0, 6),'') || coalesce(case when rs3.""DraftDuty1FallsNextDay""='true' or rs3.""DraftDuty2FallsOnNextDay""='true' or rs3.""DraftDuty3FallsNextDay""='true' then '(D+)' else ' ' end,'') as TuesdayText,  
coalesce(substring(rs4.""DraftDuty1StartTime"", 0, 6) || '-' || substring(rs4.""DraftDuty1EndTime"", 0, 6),'') || coalesce('<br/>' || substring(rs4.""DraftDuty2StartTime"", 0, 6) || '-' || substring(rs4.""DraftDuty2EndTime"", 0, 6),'') || coalesce('<br/>' || substring(rs4.""DraftDuty3StartTime"", 0, 6) || '-' || substring(rs4.""DraftDuty3EndTime"", 0, 6),'') || coalesce(case when rs4.""DraftDuty1FallsNextDay""='true' or rs4.""DraftDuty2FallsOnNextDay""='true' or rs4.""DraftDuty3FallsNextDay""='true' then '(D+)' else ' ' end,'') as WednesdayText,
coalesce(substring(rs5.""DraftDuty1StartTime"", 0, 6) || '-' || substring(rs5.""DraftDuty1EndTime"", 0, 6),'') || coalesce('<br/>' || substring(rs5.""DraftDuty2StartTime"", 0, 6) || '-' || substring(rs5.""DraftDuty2EndTime"", 0, 6),'') || coalesce('<br/>' || substring(rs5.""DraftDuty3StartTime"", 0, 6) || '-' || substring(rs5.""DraftDuty3EndTime"", 0, 6),'') || coalesce(case when rs5.""DraftDuty1FallsNextDay""='true' or rs5.""DraftDuty2FallsOnNextDay""='true' or rs5.""DraftDuty3FallsNextDay""='true' then '(D+)' else ' ' end,'') as ThursdayText, 
coalesce(substring(rs6.""DraftDuty1StartTime"", 0, 6) || '-' || substring(rs6.""DraftDuty1EndTime"", 0, 6),'') || coalesce('<br/>' || substring(rs6.""DraftDuty2StartTime"", 0, 6) || '-' || substring(rs6.""DraftDuty2EndTime"", 0, 6),'') || coalesce('<br/>' || substring(rs6.""DraftDuty3StartTime"", 0, 6) || '-' || substring(rs6.""DraftDuty3EndTime"", 0, 6),'') || coalesce(case when rs6.""DraftDuty1FallsNextDay""='true' or rs6.""DraftDuty2FallsOnNextDay""='true' or rs6.""DraftDuty3FallsNextDay""='true' then '(D+)' else ' ' end,'') as FridayText,   
coalesce(substring(rs7.""DraftDuty1StartTime"", 0, 6) || '-' || substring(rs7.""DraftDuty1EndTime"", 0, 6),'') || coalesce('<br/>' || substring(rs7.""DraftDuty2StartTime"", 0, 6) || '-' || substring(rs7.""DraftDuty2EndTime"", 0, 6),'') || coalesce('<br/>' || substring(rs7.""DraftDuty3StartTime"", 0, 6) || '-' || substring(rs7.""DraftDuty3EndTime"", 0, 6),'') || coalesce(case when rs7.""DraftDuty1FallsNextDay""='true' or rs7.""DraftDuty2FallsOnNextDay""='true' or rs7.""DraftDuty3FallsNextDay""='true' then '(D+)' else ' ' end,'') as SaturdayText, 


rs1.""DraftTotalHours"" as SundayTotalHours,
            rs2.""DraftTotalHours"" as MondayTotalHours,
            rs3.""DraftTotalHours"" as TuesdayTotalHours,
            rs4.""DraftTotalHours"" as WednesdayTotalHours,
            rs5.""DraftTotalHours"" as ThursdayTotalHours,
            rs6.""DraftTotalHours"" as FridayTotalHours,
            rs7.""DraftTotalHours"" as SaturdayTotalHours,

            rs1.""DraftRosterDutyType"" as DayOff1,
            rs2.""DraftRosterDutyType"" as DayOff2,
            rs3.""DraftRosterDutyType"" as DayOff3,
            rs4.""DraftRosterDutyType"" as DayOff4,
            rs5.""DraftRosterDutyType"" as DayOff5,
            rs6.""DraftRosterDutyType"" as DayOff6,
            rs7.""DraftRosterDutyType"" as DayOff7,
             
            rs1.""PublishStatus"" as RostarStatus1,
            rs2.""PublishStatus"" as RostarStatus2,
            rs3.""PublishStatus"" as RostarStatus3,
            rs4.""PublishStatus"" as RostarStatus4,
            rs5.""PublishStatus"" as RostarStatus5,
            rs6.""PublishStatus"" as RostarStatus6,
            rs7.""PublishStatus"" as RostarStatus7,

            rs1.""IsDraft"" as Draft1,
            rs2.""IsDraft"" as Draft2,
            rs3.""IsDraft"" as Draft3,
            rs4.""IsDraft"" as Draft4,
            rs5.""IsDraft"" as Draft5,
            rs6.""IsDraft"" as Draft6,
            rs7.""IsDraft"" as Draft7,

            case when rs1.""Duty1StartTime"" is not null then 1 else 0 end as Count1,
            case when rs2.""Duty1StartTime"" is not null then 1 else 0 end as Count2,
            case when rs3.""Duty1StartTime"" is not null then 1 else 0 end as Count3,
            case when rs4.""Duty1StartTime"" is not null then 1 else 0 end as Count4,
            case when rs5.""Duty1StartTime"" is not null then 1 else 0 end as Count5,
            case when rs6.""Duty1StartTime"" is not null then 1 else 0 end as Count6,
            case when rs7.""Duty1StartTime"" is not null then 1 else 0 end as Count7	

from cms.""N_CoreHR_HRDepartment"" as d
join cms.""N_CoreHR_HRAssignment"" as a on a.""DepartmentId""=d.""Id"" and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRJob"" as j on a.""JobId""=j.""Id"" and j.""IsDeleted""=false and j.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRPerson"" as p on a.""EmployeeId""=p.""Id"" and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""User"" as u on u.""Id""=p.""UserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_CoreHR_HRNationality"" as n on n.""Id""=p.""NationalityId"" and n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}'
--left join cms.""N_CoreHR_HRSection"" as s on s.""Id""=p.""NationalityId"" and n.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_CoreHR_HRContract"" as c on c.""EmployeeId""=p.""Id"" and c.""IsDeleted""=false and c.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_CoreHR_HRSponsor"" as sp on sp.""Id""=c.""SponsorId"" and sp.""IsDeleted""=false and sp.""CompanyId""='{_repo.UserContext.CompanyId}'

left join cms.""N_TAA_RosterSchedule"" as rs1 on u.""Id""=rs1.""UserId"" and rs1.""IsDeleted""=false and rs1.""RosterDate""='{sun}' and rs1.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_TAA_RosterSchedule"" as rs2 on u.""Id""=rs2.""UserId"" and rs2.""IsDeleted""=false and rs2.""RosterDate""='{mon}' and rs2.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_TAA_RosterSchedule"" as rs3 on u.""Id""=rs3.""UserId"" and rs3.""IsDeleted""=false and rs3.""RosterDate""='{tue}' and rs3.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_TAA_RosterSchedule"" as rs4 on u.""Id""=rs4.""UserId"" and rs4.""IsDeleted""=false and rs4.""RosterDate""='{wed}' and rs4.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_TAA_RosterSchedule"" as rs5 on u.""Id""=rs5.""UserId"" and rs5.""IsDeleted""=false and rs5.""RosterDate""='{thu}' and rs5.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_TAA_RosterSchedule"" as rs6 on u.""Id""=rs6.""UserId"" and rs6.""IsDeleted""=false and rs6.""RosterDate""='{fri}' and rs6.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_TAA_RosterSchedule"" as rs7 on u.""Id""=rs7.""UserId"" and rs7.""IsDeleted""=false and rs7.""RosterDate""='{sat}' and rs7.""CompanyId""='{_repo.UserContext.CompanyId}'
left join (
select al.""LeaveStartDate"",al.""LeaveEndDate"", nts.""OwnerUserId""
from public.""NtsService"" as nts
join cms.""N_Leave_AnnualLeave"" as al on al.""Id""=nts.""UdfNoteTableId"" and al.""IsDeleted""=false and al.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""LOV"" as lov on lov.""Id""=nts.""ServiceStatusId"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_repo.UserContext.CompanyId}'	  
where lov.""Code"" not in ('SERVICE_STATUS_DRAFT' , 'SERVICE_STATUS_CANCEL') and nts.""CompanyId""='{_repo.UserContext.CompanyId}' and nts.""IsDeleted""=false and nts.""CompanyId""='{_repo.UserContext.CompanyId}'
	  
union 	  
select al.""LeaveStartDate"",al.""LeaveEndDate"", nts.""OwnerUserId""
from public.""NtsService"" as nts
join cms.""N_Leave_MaternityLeave"" as al on al.""Id""=nts.""UdfNoteTableId"" and al.""IsDeleted""=false and al.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""LOV"" as lov on lov.""Id""=nts.""ServiceStatusId"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_repo.UserContext.CompanyId}'	  
where lov.""Code"" not in ('SERVICE_STATUS_DRAFT' , 'SERVICE_STATUS_CANCEL') and nts.""CompanyId""='{_repo.UserContext.CompanyId}' and nts.""IsDeleted""=false
	  
union 	  
select al.""LeaveStartDate"",al.""LeaveEndDate"", nts.""OwnerUserId""
from public.""NtsService"" as nts 
join cms.""N_Leave_PaternityLeave"" as al on al.""Id""=nts.""UdfNoteTableId"" and al.""IsDeleted""=false and al.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""LOV"" as lov on lov.""Id""=nts.""ServiceStatusId"" and lov.""IsDeleted""=false	and lov.""CompanyId""='{_repo.UserContext.CompanyId}'  
where lov.""Code"" not in ('SERVICE_STATUS_DRAFT' , 'SERVICE_STATUS_CANCEL') and nts.""CompanyId""='{_repo.UserContext.CompanyId}' and nts.""IsDeleted""=false
	  
union 	  
 select al.""LeaveStartDate"",al.""LeaveEndDate"", nts.""OwnerUserId""
from public.""NtsService"" as nts 
join cms.""N_Leave_CompassionatelyLeave"" as al on al.""Id""=nts.""UdfNoteTableId"" and al.""IsDeleted""=false and al.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""LOV"" as lov on lov.""Id""=nts.""ServiceStatusId"" and lov.""IsDeleted""=false	and lov.""CompanyId""='{_repo.UserContext.CompanyId}'  
where lov.""Code"" not in ('SERVICE_STATUS_DRAFT' , 'SERVICE_STATUS_CANCEL') and nts.""CompanyId""='{_repo.UserContext.CompanyId}' and nts.""IsDeleted""=false
	   
union 	  
select al.""LeaveStartDate"",al.""LeaveEndDate"", nts.""OwnerUserId""
from public.""NtsService"" as nts 
join cms.""N_Leave_HajjLeave"" as al on al.""Id""=nts.""UdfNoteTableId"" and al.""IsDeleted""=false and al.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""LOV"" as lov on lov.""Id""=nts.""ServiceStatusId"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_repo.UserContext.CompanyId}'	  
where lov.""Code"" not in ('SERVICE_STATUS_DRAFT' , 'SERVICE_STATUS_CANCEL') and nts.""CompanyId""='{_repo.UserContext.CompanyId}' and nts.""IsDeleted""=false
	  

	  
union 	  
select al.""LeaveStartDate"",al.""LeaveEndDate"", nts.""OwnerUserId""
from public.""NtsService"" as nts 
join cms.""N_Leave_LeaveExamination"" as al on al.""Id""=nts.""UdfNoteTableId"" and al.""IsDeleted""=false and al.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""LOV"" as lov on lov.""Id""=nts.""ServiceStatusId"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_repo.UserContext.CompanyId}'	   
where lov.""Code"" not in ('SERVICE_STATUS_DRAFT' , 'SERVICE_STATUS_CANCEL') and nts.""CompanyId""='{_repo.UserContext.CompanyId}' and nts.""IsDeleted""=false
	  
union 	  
select al.""LeaveStartDate"",al.""LeaveEndDate"", nts.""OwnerUserId""
from public.""NtsService"" as nts 
join cms.""N_Leave_MarriageLeave"" as al on al.""Id""=nts.""UdfNoteTableId"" and al.""IsDeleted""=false  and al.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""LOV"" as lov on lov.""Id""=nts.""ServiceStatusId"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_repo.UserContext.CompanyId}'	   
where lov.""Code"" not in ('SERVICE_STATUS_DRAFT' , 'SERVICE_STATUS_CANCEL') and nts.""CompanyId""='{_repo.UserContext.CompanyId}' and nts.""IsDeleted""=false

union 	  
select al.""LeaveStartDate"",al.""LeaveEndDate"", nts.""OwnerUserId""
from public.""NtsService"" as nts 
join cms.""N_Leave_PlannedUnpaidLeave"" as al on al.""Id""=nts.""UdfNoteTableId"" and al.""IsDeleted""=false and al.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""LOV"" as lov on lov.""Id""=nts.""ServiceStatusId"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_repo.UserContext.CompanyId}'	  
where lov.""Code"" not in ('SERVICE_STATUS_DRAFT' , 'SERVICE_STATUS_CANCEL')  and nts.""CompanyId""='{_repo.UserContext.CompanyId}' and nts.""IsDeleted""=false

union 	  
select al.""LeaveStartDate"",al.""LeaveEndDate"", nts.""OwnerUserId""
from public.""NtsService"" as nts 
join cms.""N_Leave_SickLeave"" as al on al.""Id""=nts.""UdfNoteTableId"" and al.""IsDeleted""=false and al.""CompanyId""='{_repo.UserContext.CompanyId}'	
join public.""LOV"" as lov on lov.""Id""=nts.""ServiceStatusId"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_repo.UserContext.CompanyId}'	   
where lov.""Code"" not in ('SERVICE_STATUS_DRAFT' , 'SERVICE_STATUS_CANCEL')  and nts.""CompanyId""='{_repo.UserContext.CompanyId}' and nts.""IsDeleted""=false

union 	  
select al.""LeaveStartDate"",al.""LeaveEndDate"", nts.""OwnerUserId""
from public.""NtsService"" as nts 
join cms.""N_Leave_UndertimeLeave"" as al on al.""Id""=nts.""UdfNoteTableId"" and al.""IsDeleted""=false and al.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""LOV"" as lov on lov.""Id""=nts.""ServiceStatusId"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_repo.UserContext.CompanyId}'	  
where lov.""Code"" not in ('SERVICE_STATUS_DRAFT' , 'SERVICE_STATUS_CANCEL') and nts.""CompanyId""='{_repo.UserContext.CompanyId}' and nts.""IsDeleted""=false

union 	  
select al.""LeaveStartDate"",al.""LeaveEndDate"", nts.""OwnerUserId""
from public.""NtsService"" as nts 
join cms.""N_Leave_UnpaidLeave"" as al on al.""Id""=nts.""UdfNoteTableId"" and al.""IsDeleted""=false and al.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""LOV"" as lov on lov.""Id""=nts.""ServiceStatusId"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_repo.UserContext.CompanyId}'	   
where lov.""Code"" not in ('SERVICE_STATUS_DRAFT' , 'SERVICE_STATUS_CANCEL') and nts.""CompanyId""='{_repo.UserContext.CompanyId}') 
               as nts on nts.""OwnerUserId""=u.""Id""
where d.""IsDeleted""=false #Org#			   
group by 	u.""Id"",p.""Id"",p.""IqamahNoNationalId"",p.""PersonNo""
,j.""JobTitle"",n.""NationalityName"",d.""DepartmentName""
,c.""EffectiveEndDate"",c.""ContractRenewable"",sp.""Code""
,a.""Id"",d.""Id"",p.""PersonFullName"",u.""Name"",rs1.""IsDraft""
,rs2.""IsDraft""
,rs3.""IsDraft""
,rs4.""IsDraft""
,rs5.""IsDraft""
,rs6.""IsDraft""
,rs7.""IsDraft""
,rs1.""DraftRosterDutyType""
,rs2.""DraftRosterDutyType""
,rs3.""DraftRosterDutyType""
,rs4.""DraftRosterDutyType""
,rs5.""DraftRosterDutyType""
,rs6.""DraftRosterDutyType""
,rs7.""DraftRosterDutyType""
,rs1.""DraftTotalHours""
,rs2.""DraftTotalHours""
,rs3.""DraftTotalHours""
,rs4.""DraftTotalHours""
,rs5.""DraftTotalHours""
,rs6.""DraftTotalHours""
,rs7.""DraftTotalHours""
,rs1.""Duty1StartTime""
,rs2.""Duty1StartTime""
,rs3.""Duty1StartTime""
,rs4.""Duty1StartTime""
,rs5.""Duty1StartTime""
,rs6.""Duty1StartTime""
,rs7.""Duty1StartTime""
,rs1.""DraftDuty1StartTime"",rs1.""DraftDuty2StartTime"",rs1.""DraftDuty3StartTime"",rs1.""DraftDuty1EndTime"",rs1.""DraftDuty2EndTime"",rs1.""DraftDuty3EndTime""
,rs2.""DraftDuty1StartTime"",rs2.""DraftDuty2StartTime"",rs2.""DraftDuty3StartTime"",rs2.""DraftDuty1EndTime"",rs2.""DraftDuty2EndTime"",rs2.""DraftDuty3EndTime""
,rs3.""DraftDuty1StartTime"",rs3.""DraftDuty2StartTime"",rs3.""DraftDuty3StartTime"",rs3.""DraftDuty1EndTime"",rs3.""DraftDuty2EndTime"",rs3.""DraftDuty3EndTime""
,rs4.""DraftDuty1StartTime"",rs4.""DraftDuty2StartTime"",rs4.""DraftDuty3StartTime"",rs4.""DraftDuty1EndTime"",rs4.""DraftDuty2EndTime"",rs4.""DraftDuty3EndTime""
,rs5.""DraftDuty1StartTime"",rs5.""DraftDuty2StartTime"",rs5.""DraftDuty3StartTime"",rs5.""DraftDuty1EndTime"",rs5.""DraftDuty2EndTime"",rs5.""DraftDuty3EndTime""
,rs6.""DraftDuty1StartTime"",rs6.""DraftDuty2StartTime"",rs6.""DraftDuty3StartTime"",rs6.""DraftDuty1EndTime"",rs6.""DraftDuty2EndTime"",rs6.""DraftDuty3EndTime""
,rs7.""DraftDuty1StartTime"",rs7.""DraftDuty2StartTime"",rs7.""DraftDuty3StartTime"",rs7.""DraftDuty1EndTime"",rs7.""DraftDuty2EndTime"",rs7.""DraftDuty3EndTime""

,rs1.""DraftDuty1FallsNextDay"",rs1.""DraftDuty2FallsOnNextDay"",rs1.""DraftDuty3FallsNextDay""
,rs2.""DraftDuty1FallsNextDay"",rs2.""DraftDuty2FallsOnNextDay"",rs2.""DraftDuty3FallsNextDay""
,rs3.""DraftDuty1FallsNextDay"",rs3.""DraftDuty2FallsOnNextDay"",rs3.""DraftDuty3FallsNextDay""
,rs4.""DraftDuty1FallsNextDay"",rs4.""DraftDuty2FallsOnNextDay"",rs4.""DraftDuty3FallsNextDay""
,rs5.""DraftDuty1FallsNextDay"",rs5.""DraftDuty2FallsOnNextDay"",rs5.""DraftDuty3FallsNextDay""
,rs6.""DraftDuty1FallsNextDay"",rs6.""DraftDuty2FallsOnNextDay"",rs6.""DraftDuty3FallsNextDay""
,rs7.""DraftDuty1FallsNextDay"",rs7.""DraftDuty2FallsOnNextDay"",rs7.""DraftDuty3FallsNextDay""
,rs1.""PublishStatus"" 
, rs2.""PublishStatus""
 ,rs3.""PublishStatus""
 ,rs4.""PublishStatus""
 ,rs5.""PublishStatus""
 ,rs6.""PublishStatus""
 ,rs7.""PublishStatus""
";



            var whereorg = "";
            if (orgId.IsNotNullAndNotEmpty() && orgId != "0")
            {
                whereorg = $@" and d.""Id""='{orgId}'";
            }
            else
            {
                var legalorgids = await GetDepartmentList();
                var orgids1 = string.Join("','", legalorgids.Select(x => x.Id));
                var orgids = "'" + string.Join("','", legalorgids.Select(x => x.Id)) + "'";
                whereorg = $@" and d.""Id"" in ({orgids})";
            }
            query = query.Replace("#Org#", whereorg);
            var result = await _queryRepo.ExecuteQueryList<RosterScheduleViewModel>(query, null);
            return result;
        }

        public async Task<List<RosterScheduleViewModel>> GetRosterScheduleByRosterDate(string orgId, DateTime start, DateTime end)
        {
            var query = $@"
            select rs1.*,rs1.""NtsNoteId"" as NoteId
            from cms.""N_CoreHR_HRDepartment"" as d
            join cms.""N_CoreHR_HRAssignment"" as a on a.""DepartmentId""=d.""Id"" and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'
            join cms.""N_CoreHR_HRJob"" as j on a.""JobId""=j.""Id"" and j.""IsDeleted""=false and j.""CompanyId""='{_repo.UserContext.CompanyId}'
            join cms.""N_CoreHR_HRPerson"" as p on a.""EmployeeId""=p.""Id"" and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
            join public.""User"" as u on u.""Id""=p.""UserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
            join cms.""N_TAA_RosterSchedule"" as rs1 on u.""Id""=rs1.""UserId"" and rs1.""IsDeleted""=false and rs1.""CompanyId""='{_repo.UserContext.CompanyId}'
            where rs1.""RosterDate"" >='{start}' and rs1.""RosterDate""<='{end}' and d.""CompanyId""='{_repo.UserContext.CompanyId}' #ORG# ";

            var whereorg = "";
            if (orgId.IsNotNullAndNotEmpty() && orgId != "All")
            {
                whereorg = $@" and d.""Id""='{orgId}'";
            }
            query = query.Replace("#ORG#", whereorg);
            var queryData = await _queryRepo.ExecuteQueryList<RosterScheduleViewModel>(query, null);

            return queryData;
        }

        public async Task<List<RosterTimeLineViewModel>> GetRosterTimeList(string orgId, DateTime sun, DateTime sat)
        {
            var query = $@"
            select p.""Id"" as PersonId,'ON' as Title,false as IsAllDay,rs1.""RosterDate"" as RosterDate,
            rs1.""Duty1StartTime"" as Duty1StartTime,rs1.""Duty1EndTime"" as Duty1EndTime,'R' as Type, 1 as TypeVal
from cms.""N_CoreHR_HRDepartment"" as d
join cms.""N_CoreHR_HRAssignment"" as a on a.""DepartmentId""=d.""Id"" and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRJob"" as j on a.""JobId""=j.""Id"" and j.""IsDeleted""=false and j.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRPerson"" as p on a.""EmployeeId""=p.""Id"" and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""User"" as u on u.""Id""=p.""UserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_TAA_RosterSchedule"" as rs1 on u.""Id""=rs1.""UserId"" and rs1.""IsDeleted""=false  and rs1.""CompanyId""='{_repo.UserContext.CompanyId}'
where rs1.""RosterDate"" is not null and rs1.""Duty1StartTime"" is not null and rs1.""Duty1EndTime"" is not null and '{sun}'<=rs1.""RosterDate"" and rs1.""RosterDate""<='{sat}'
 and rs1.""PublishStatus""='1' and d.""CompanyId""='{_repo.UserContext.CompanyId}' and d.""IsDeleted""=false #Org#    
union

select p.""Id"" as PersonId,'ON' as Title,false as IsAllDay,rs1.""RosterDate"" as RosterDate,
            rs1.""Duty2StartTime"" as Duty1StartTime,rs1.""Duty2EndTime"" as Duty1EndTime,'R' as Type, 1 as TypeVal
from cms.""N_CoreHR_HRDepartment"" as d
join cms.""N_CoreHR_HRAssignment"" as a on a.""DepartmentId""=d.""Id"" and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRJob"" as j on a.""JobId""=j.""Id"" and j.""IsDeleted""=false and j.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRPerson"" as p on a.""EmployeeId""=p.""Id"" and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""User"" as u on u.""Id""=p.""UserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_TAA_RosterSchedule"" as rs1 on u.""Id""=rs1.""UserId"" and rs1.""IsDeleted""=false  and rs1.""CompanyId""='{_repo.UserContext.CompanyId}'
where rs1.""RosterDate"" is not null and rs1.""Duty2StartTime"" is not null and rs1.""Duty2EndTime"" is not null and '{sun}'<=rs1.""RosterDate"" and rs1.""RosterDate""<='{sat}'
and rs1.""PublishStatus""='1' and rs1.""PublishStatus""='1' and d.""CompanyId""='{_repo.UserContext.CompanyId}' and d.""IsDeleted""=false #Org#    
union

select p.""Id"" as PersonId,'ON' as Title,false as IsAllDay,rs1.""RosterDate"" as RosterDate,
            rs1.""Duty3StartTime"" as Duty1StartTime,rs1.""Duty3EndTime"" as Duty1EndTime,'R' as Type, 1 as TypeVal
from cms.""N_CoreHR_HRDepartment"" as d
join cms.""N_CoreHR_HRAssignment"" as a on a.""DepartmentId""=d.""Id"" and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRJob"" as j on a.""JobId""=j.""Id"" and j.""IsDeleted""=false and j.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRPerson"" as p on a.""EmployeeId""=p.""Id"" and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""User"" as u on u.""Id""=p.""UserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_TAA_RosterSchedule"" as rs1 on u.""Id""=rs1.""UserId"" and rs1.""IsDeleted""=false  and rs1.""CompanyId""='{_repo.UserContext.CompanyId}'
where rs1.""RosterDate"" is not null and rs1.""Duty3StartTime"" is not null and rs1.""Duty3EndTime"" is not null and '{sun}'<=rs1.""RosterDate"" and rs1.""RosterDate""<='{sat}'
and rs1.""PublishStatus""='1' and d.""CompanyId""='{_repo.UserContext.CompanyId}' and d.""IsDeleted""=false #Org#    

union

select p.""Id"" as PersonId,'ON' as Title,false as IsAllDay,rs1.""AttendanceDate"" as RosterDate,
            rs1.""Duty1StartTime"" as Duty1StartTime,rs1.""Duty1EndTime"" as Duty1EndTime,'A' as Type, 2 as TypeVal
from cms.""N_CoreHR_HRDepartment"" as d
join cms.""N_CoreHR_HRAssignment"" as a on a.""DepartmentId""=d.""Id"" and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRJob"" as j on a.""JobId""=j.""Id"" and j.""IsDeleted""=false and j.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRPerson"" as p on a.""EmployeeId""=p.""Id"" and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""User"" as u on u.""Id""=p.""UserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_TAA_Attendance"" as rs1 on u.""Id""=rs1.""UserId"" and rs1.""IsDeleted""=false and rs1.""CompanyId""='{_repo.UserContext.CompanyId}' 
where rs1.""AttendanceDate"" is not null and rs1.""Duty1StartTime"" is not null and rs1.""Duty1EndTime"" is not null and '{sun}'<=rs1.""AttendanceDate"" and rs1.""AttendanceDate""<='{sat}'
 and d.""CompanyId""='{_repo.UserContext.CompanyId}' and d.""IsDeleted""=false #Org#    

union

select p.""Id"" as PersonId,'ON' as Title,false as IsAllDay,rs1.""AttendanceDate"" as RosterDate,
            rs1.""Duty2StartTime"" as Duty1StartTime,rs1.""Duty2EndTime"" as Duty1EndTime,'A' as Type, 2 as TypeVal
from cms.""N_CoreHR_HRDepartment"" as d
join cms.""N_CoreHR_HRAssignment"" as a on a.""DepartmentId""=d.""Id"" and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRJob"" as j on a.""JobId""=j.""Id"" and j.""IsDeleted""=false and j.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRPerson"" as p on a.""EmployeeId""=p.""Id"" and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""User"" as u on u.""Id""=p.""UserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_TAA_Attendance"" as rs1 on u.""Id""=rs1.""UserId"" and rs1.""IsDeleted""=false  and rs1.""CompanyId""='{_repo.UserContext.CompanyId}'
where rs1.""AttendanceDate"" is not null and rs1.""Duty2StartTime"" is not null and rs1.""Duty2EndTime"" is not null and '{sun}'<=rs1.""AttendanceDate"" and rs1.""AttendanceDate""<='{sat}'
 and d.""CompanyId""='{_repo.UserContext.CompanyId}' and d.""IsDeleted""=false #Org#    

union

select p.""Id"" as PersonId,'ON' as Title,false as IsAllDay,rs1.""AttendanceDate"" as RosterDate,
            rs1.""Duty3StartTime"" as Duty1StartTime,rs1.""Duty3EndTime"" as Duty1EndTime,'A' as Type, 2 as TypeVal
from cms.""N_CoreHR_HRDepartment"" as d
join cms.""N_CoreHR_HRAssignment"" as a on a.""DepartmentId""=d.""Id"" and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRJob"" as j on a.""JobId""=j.""Id"" and j.""IsDeleted""=false and j.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRPerson"" as p on a.""EmployeeId""=p.""Id"" and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""User"" as u on u.""Id""=p.""UserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_TAA_Attendance"" as rs1 on u.""Id""=rs1.""UserId"" and rs1.""IsDeleted""=false  and rs1.""CompanyId""='{_repo.UserContext.CompanyId}'
where rs1.""AttendanceDate"" is not null and rs1.""Duty3StartTime"" is not null and rs1.""Duty3EndTime"" is not null and '{sun}'<=rs1.""AttendanceDate"" and rs1.""AttendanceDate""<='{sat}'
 and d.""CompanyId""='{_repo.UserContext.CompanyId}' and d.""IsDeleted""=false  #Org#         ";

            var whereorg = "";
            if (orgId.IsNotNullAndNotEmpty() && orgId != "All")
            {
                whereorg = $@" and d.""Id""='{orgId}'";
            }
            query = query.Replace("#Org#", whereorg);
            var list = await _queryRepo.ExecuteQueryList<RosterTimeLineViewModel>(query, null);
            return list;
        }

        public async Task<List<IdNameViewModel>> GetPersonListByOrganizationHerarchy(string orgId)
        {
            var query = $@"select distinct p.""Id"",concat(p.""PersonFullName"",p.""SponsorshipNo"") as Name
from cms.""N_CoreHR_HRDepartment"" as d
left join(
WITH RECURSIVE Department AS(
                                 select d.""Id"" as ""Id"",d.""Id"" as ""ParentId"",'Parent' as Type
                                from cms.""N_CoreHR_HRDepartment"" as d
                                where d.""IsDeleted""=false #Org#


                              union all

                                 select distinct d.""Id"" as Id,h.""ParentDepartmentId"" as ""ParentId"",'Child' as Type
                                from cms.""N_CoreHR_HRDepartmentHierarchy"" as h
                                join cms.""N_CoreHR_HRDepartment"" as d on h.""DepartmentId"" = d.""Id"" and d.""IsDeleted""=false
                                join Department ns on h.""ParentDepartmentId"" = ns.""Id""
                                where  h.""IsDeleted""=false
                             )
                            select ""ParentId"" from Department  where Type = 'Child'

) as cdept on cdept.""ParentId""=d.""Id""
join cms.""N_CoreHR_HRAssignment"" as a on a.""DepartmentId""=d.""Id"" and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRPerson"" as p on a.""EmployeeId""=p.""Id"" and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""User"" as u on p.""UserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
  where d.""IsDeleted""=false and d.""CompanyId""='{_repo.UserContext.CompanyId}' #Org#";
            var whereorg = "";
            if (orgId.IsNotNullAndNotEmpty() && orgId != "All")
            {
                whereorg = $@"and d.""Id""='{orgId}'";
            }
            query = query.Replace("#Org#", whereorg);
            var list = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return list;


        }

        public async Task<List<RosterScheduleViewModel>> GetDistinctNotcalculatedRosterDateList(DateTime rosterDate)
        {
            var match = $@"Select r.* from 
cms.""N_TAA_RosterSchedule"" as r 
join public.""User"" as u on u.""Id""=r.""UserId"" and u.""IsDeleted""=false  and u.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRPerson"" as p on p.""UserId""=u.""Id"" and p.""IsDeleted""=false  and p.""CompanyId""='{_repo.UserContext.CompanyId}'
 where r.""RosterDate""::Date<='{rosterDate}'::Date and p.""BiometricId"" is not null   
                and (r.""IsAttendanceCalculated"" is  null or r.""IsAttendanceCalculated""='false') and r.""CompanyId""='{_repo.UserContext.CompanyId}'  and r.""IsDeleted""=false
";
            var list = await _queryRepo.ExecuteQueryList<RosterScheduleViewModel>(match, null);
            //var match = @"MATCH (r:TAA_RosterSchedule{IsDeleted:0,Status:'Active'})
            //            -[:R_RosterSchedule_User]->(u:ADM_User{IsDeleted:0,Status:'Active'})
            //            match (u)-[:R_User_PersonRoot]->(pr:HRS_PersonRoot)
            //            match (pr)<-[:R_PersonRoot]-(p:HRS_Person{IsDeleted:0,Status:'Active',IsLatest:true})
            //            where r.RosterDate<={RosterDate} and p.BiometricId is not null  
            //            and (r.IsAttendanceCalculated is  null or r.IsAttendanceCalculated=false) 
            //            return r";
            //var list = ExecuteCypherList<TAA_RosterSchedule>(match, new Dictionary<string, object> { { "RosterDate", rosterDate } });
            return list;
        }

        public async Task<List<RosterScheduleViewModel>> GetPublishedRostersList(DateTime rosterDate)
        {
            //var cypher = @"MATCH (r:TAA_RosterSchedule{IsDeleted:0,Status:'Active',PublishStatus:'Published',RosterDate:{RosterDate}})
            //            -[:R_RosterSchedule_User]->(u:ADM_User{IsDeleted:0,Status:'Active'})
            //            -[:R_User_PersonRoot]->(pr:HRS_PersonRoot)<-[:R_PersonRoot]-(p:HRS_Person{IsDeleted:0,Status:'Active',IsLatest:true})
            //            match (pr)<-[:R_UserInfo_PersonRoot]-(ui:CLK_UserInfo{IsDeleted:0,Status:'Active'})
            //            where  p.BiometricId is not null 
            //            return r,u.Id as UserId,p.BiometricId as BiometricId";
            var cypher = $@"select r.*,u.""Id"" as UserId,p.""BiometricId"" as BiometricId from
cms.""N_TAA_RosterSchedule"" as r 
join public.""User"" as u on u.""Id""=r.""UserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRPerson"" as p on p.""UserId""=u.""Id"" and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CLK_UserInfo"" as ui on ui.""PersonId""=p.""Id"" and ui.""IsDeleted""=false and ui.""CompanyId""='{_repo.UserContext.CompanyId}'
where p.""BiometricId"" is not null  and r.""RosterDate""='{rosterDate}' and r.""CompanyId""='{_repo.UserContext.CompanyId}' and r.""IsDeleted""=false 
";
            return await _queryRepo.ExecuteQueryList<RosterScheduleViewModel>(cypher, null);
        }

        public async Task<IList<RosterScheduleViewModel>> GetPublishedRostersForAttendance(DateTime rosterDate)
        {
            //var cypher = @"MATCH (r:TAA_RosterSchedule{IsDeleted:0,Status:'Active'})
            //            -[:R_RosterSchedule_User]->(u:ADM_User{IsDeleted:0,Status:'Active'})
            //            -[:R_User_PersonRoot]->(pr:HRS_PersonRoot)<-[:R_PersonRoot]-(p:HRS_Person{IsDeleted:0,Status:'Active',IsLatest:true})
            //            match (pr)-[:R_PersonRoot_LegalEntity_OrganizationRoot]->(or:HRS_OrganizationRoot)
            //            <-[:R_LegalEntity_OrganizationRoot]-(le:ADM_LegalEntity)
            //            where r.PublishStatus='Published' and r.RosterDate={RosterDate} and p.BiometricId is not null  
            //           and (p.IsAttendanceCalculated is  null or p.IsAttendanceCalculated=false)
            //            return r,u.Id as UserId,p.BiometricId as BiometricId,le.LegalEntityCode as LegalEntityCode ";
            var cypher = $@"select r.*,u.""Id"" as UserId,p.""BiometricId"" as BiometricId,le.""Code"" as LegalEntityCode 
ms.""N_TAA_RosterSchedule"" as r 
join public.""User"" as u on u.""Id""=r.""UserId"" and u.""IsDeleted""=false  and u.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRPerson"" as p on p.""UserId""=u.""Id"" and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""LegalEntity"" as le on le.""Id""=p.""LegalEntityId"" and le.""IsDeleted""=false  and le.""CompanyId""='{_repo.UserContext.CompanyId}'
  where r.""PublishStatus""='1' and r.""RosterDate""='{rosterDate}' and p.""BiometricId"" is not null  
                 and (p.""IsAttendanceCalculated"" is  null or p.""IsAttendanceCalculated""=false)  and r.""CompanyId""='{_repo.UserContext.CompanyId}' and r.""IsDeleted""=false 
";
            var result = await _queryRepo.ExecuteQueryList<RosterScheduleViewModel>(cypher, null);
            return result;
        }

        public async Task<AttendanceViewModel> GetAttendanceSingleForPersonandDate(string personId, DateTime date)
        {
            var query = $@"select n.*,u.""Id"" as UserId  from cms.""N_TAA_Attendance"" as n
join public.""User"" as u on u.""Id""=n.""UserId""  and u.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_CoreHR_HRPerson"" as p on p.""UserId""=u.""Id"" and p.""IsDeleted""=false and p.""Id""='{personId}' and p.""CompanyId""='{_repo.UserContext.CompanyId}'
where n.""AttendanceDate""='{date}' and n.""CompanyId""='{_repo.UserContext.CompanyId}' and n.""IsDeleted""=false and u.""IsDeleted""=false";
            //            var cypher = @"match (n:TAA_Attendance) where date(datetime(n.AttendanceDate)) = date(datetime({date}))  with n match (n)-[:R_Attendance_User]-(u:ADM_User)
            //match(u)-[:R_User_PersonRoot]-(pr:HRS_PersonRoot{Id:{personId}, IsDeleted: 0,CompanyId: {CompanyId}})
            //return n, u.Id as UserId";
            //var prms = new Dictionary<string, object>
            //{
            //    { "CompanyId", CompanyId },
            //    { "Status", StatusEnum.Active.ToString() },
            //    { "date", date },
            //    { "personId", personId },

            //};
            var result = await _queryRepo.ExecuteQuerySingle<AttendanceViewModel>(query, null);
            return result;
        }

        public async Task<List<IdNameViewModel>> GetDepartmentIdNameList()
        {
            var companyquery = Helper.OrganizationMapping(_uc.UserId, _uc.CompanyId, _uc.LegalEntityId);
            string query = $@"{companyquery} select d.""Id"" as Id ,d.""DepartmentName"" as Name
                            from cms.""N_CoreHR_HRDepartment"" as d
                            join  ""Department"" as dept on dept.""DepartmentId""=d.""Id"" --and dept.""IsDeleted""=false  and dept.""CompanyId""='{_repo.UserContext.CompanyId}'
                            where d.""IsDeleted""=false  and d.""CompanyId""='{_repo.UserContext.CompanyId}'
                            ";

            var querydata = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return querydata;
        }

        public async Task<List<AttendanceViewModel>> GetAttendanceList(string orgId, DateTime? date)
        {
            var query = $@"select u.""Id"" as UserId,hp.""Id"" as PersonId,hp.""SponsorshipNo"" as SponsorshipNo,
                        hd.""DepartmentName"" as OrganizationName,
                        rs.""Duty1Enabled"" as RosterDuty1Enabled, rs.""Duty1StartTime"" as RosterDuty1StartTime,
                        rs.""Duty1EndTime"" as RosterDuty1EndTime, rs.""Duty1FallsNextDay"" as RosterDuty1FallsNextDay,
                        rs.""Duty2Enabled"" as RosterDuty2Enabled,rs.""Duty2StartTime"" as RosterDuty2StartTime,rs.""Duty2EndTime"" as RosterDuty2EndTime,
                        rs.""Duty2FallsNextDay"" as RosterDuty2FallsNextDay, rs.""Duty3Enabled"" as RosterDuty3Enabled, rs.""Duty3StartTime"" as RosterDuty3StartTime,
                        rs.""Duty3EndTime"" as RosterDuty3EndTime, rs.""Duty3FallsNextDay"" as RosterDuty3FallsNextDay,
                        hp.""PersonNo"" as PersonNo,
                        hj.""JobTitle"" as JobName,
                        n.""EmployeeComments"" as EmployeeComments, n.""OverrideComments"" as OverrideComments, s.""Id"" as ServiceId,
                        lovat.""Name"" as SystemAttendance,lovlt.""Name"" as AttendanceLeaveType,lovoa.""Name"" as OverrideAttendance,
                        assi.""Id"" as AssignmentId,
                        CONCAT(coalesce(CONCAT(rs.""Duty1StartTime"",' - ',rs.""Duty1EndTime""),''),coalesce(case when rs.""Duty1FallsNextDay""='true' then '(D+)' else '' end,''),'<br/>',coalesce(CONCAT(rs.""Duty2StartTime"",' - ',rs.""Duty2EndTime""),''),coalesce(case when rs.""Duty2FallsNextDay""='true' then '(D+)' else '' end,''),'<br/>',coalesce(CONCAT(rs.""Duty3StartTime"",' - ',rs.""Duty3EndTime""),''),coalesce(case when rs.""Duty3FallsNextDay""='true' then '(D+)' else '' end,''),coalesce(case when (select lovrdt.""Code"" from public.""LOV"" as lovrdt where lovrdt.""Id""=rs.""RosterDutyTypeId"")='DAY_OFF' then ' DayOff ' end,''),coalesce(case when (select lovrdt.""Code"" from public.""LOV"" as lovrdt where lovrdt.""Id""=rs.""RosterDutyTypeId"")='PUBLIC_HOLIDAY' then ' Public Holiday ' end,'')) as RosterText,
                        CONCAT(coalesce(CONCAT(n.""Duty1StartTime"",coalesce(case when n.""Duty1FallsPreviousDay""='true' then '(D-)' else '' end,''),' - ',n.""Duty1EndTime""),''),coalesce(case when n.""Duty1FallsNextDay""='true' then '(D+)' else '' end,''),'<br/>',coalesce(CONCAT(n.""Duty2StartTime"",coalesce(case when n.""Duty2FallsPreviousDay""='true' then '(D-)' else '' end,''),' - ',n.""Duty2EndTime""),''),coalesce(case when n.""Duty2FallsNextDay""='true' then '(D+)' else '' end,''),'<br/>',coalesce(CONCAT(n.""Duty3StartTime"",coalesce(case when n.""Duty3FallsPreviousDay""='true' then '(D-)' else '' end,''),' - ',n.""Duty3EndTime""),''),coalesce(case when n.""Duty3FallsNextDay""='true' then '(D+)' else '' end,'')) as ActualText,
                        case when rs.""TotalHours"" is not null THEN rs.""TotalHours"" ELSE '00.00' END as RosterHours,
                        case when n.""TotalHours"" is not null THEN n.""TotalHours"" ELSE '00.00' END as ActualHours,
                        case when n.""SystemOTHours"" is not null THEN n.""SystemOTHours"" ELSE '00.00' END as SystemOTHoursText,
                        case when n.""OverrideOTHours"" is not null THEN n.""OverrideOTHours"" ELSE '00.00' END as OverrideOTHoursText,
                        case when n.""SystemDeductionHours"" is not null THEN n.""SystemDeductionHours"" ELSE '00.00' END as SystemDeductionHoursText,
                        case when n.""OverrideDeductionHours"" is not null THEN n.""OverrideDeductionHours"" ELSE '00.00' END as OverrideDeductionHoursText,
                        case when n.""Id"" is not null THEN n.""Id"" ELSE '' END as ""Id"",
                        case when n.""IsOverridden"" is null THEN 'false' ELSE n.""IsOverridden"" END as IsOverridden,
                        case when (select lovp.""Code"" from public.""LOV"" as lovp where lovp.""Id""=n.""ApprovalStatus"")='APPROVED' THEN true ELSE false END as Approved,
                        CONCAT( hp.""FirstName"",' ',hp.""LastName"") as EmployeeName
                        
                        from cms.""N_CoreHR_HRDepartment"" as hd
                        join cms.""N_CoreHR_HRAssignment"" as assi on assi.""DepartmentId""=hd.""Id"" and assi.""IsDeleted""=false and assi.""CompanyId""='{_repo.UserContext.CompanyId}'
                        join cms.""N_CoreHR_HRPerson"" as hp on hp.""Id""=assi.""EmployeeId"" and hp.""IsDeleted""=false and hp.""CompanyId""='{_repo.UserContext.CompanyId}'
                        join public.""User"" as u on u.""Id""=hp.""UserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
                        left join cms.""N_CoreHR_HRJob"" as hj on hj.""Id""=assi.""JobId"" and hj.""IsDeleted""=false and hj.""CompanyId""='{_repo.UserContext.CompanyId}'
                        left join cms.""N_TAA_Attendance"" as n on n.""UserId""=u.""Id"" and n.""AttendanceDate""::Date='{date}'::Date and n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}' and n.""IsDeleted""=false
                        left join public.""LOV"" as lovat on lovat.""Id""=n.""AttendanceTypeId"" and lovat.""IsDeleted""=false and lovat.""CompanyId""='{_repo.UserContext.CompanyId}'
                        left join public.""LOV"" as lovlt on lovlt.""Id""=n.""AttendanceLeaveTypeId"" and lovlt.""IsDeleted""=false and lovlt.""CompanyId""='{_repo.UserContext.CompanyId}'
                        left join public.""LOV"" as lovoa on lovoa.""Id""=n.""OverrideAttendanceId"" and lovoa.""IsDeleted""=false and lovoa.""CompanyId""='{_repo.UserContext.CompanyId}'
                        left join public.""LOV"" as lovpps on lovpps.""Id""=n.""PayrollPostedStatusId"" and lovpps.""IsDeleted""=false and lovpps.""CompanyId""='{_repo.UserContext.CompanyId}'
                        left join public.""LOV"" as lovas on lovas.""Id""=n.""ApprovalStatus"" and lovas.""IsDeleted""=false and lovas.""CompanyId""='{_repo.UserContext.CompanyId}'
                        left join cms.""N_TAA_OverrideOverTimeRequest"" as ort on ort.""ParentId""=n.""Id"" and ort.""IsDeleted""=false and ort.""CompanyId""='{_repo.UserContext.CompanyId}'
                        left join public.""NtsService"" as s on s.""UdfNoteId""=ort.""NtsNoteId"" and s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}'
                        left join cms.""N_TAA_RosterSchedule"" as rs on rs.""UserId""=u.""Id"" and rs.""IsDeleted""=false and rs.""RosterDate""::Date='{date}'::Date and rs.""CompanyId""='{_repo.UserContext.CompanyId}'
                        where hd.""IsDeleted""=false #OrgWhere#

                        ";
            var orgwhere = "";
            if (orgId.IsNotNullAndNotEmpty())
            {
                orgwhere = $@"and hd.""Id""='{orgId}' ";
            }
            query = query.Replace("#OrgWhere#", orgwhere);
            var result = await _queryRepo.ExecuteQueryList<AttendanceViewModel>(query, null);
            return result;
        }

        public async Task<List<AccessLogViewModel>> GetAccessLogs(DateTime startDate, DateTime endDate, string users)
        {
            var accesslogquery = $@" select clk.* from cms.""N_CLK_AccessLog"" as clk 
                                        where clk.""IsDeleted""=false and clk.""PunchingTime"">='{startDate}' and clk.""PunchingTime""<='{endDate}'  and clk.""CompanyId""='{_repo.UserContext.CompanyId}'
                                        #UserAccessWhere# ";
            var useraccesswhere = "";
            if (users.IsNotNullAndNotEmpty())
            {
                useraccesswhere = @" and clk.""UserId"" IN (" + users + ") ";
            }
            accesslogquery = accesslogquery.Replace("#UserAccessWhere#", useraccesswhere);
            var userLogs = await _queryRepo.ExecuteQueryList<AccessLogViewModel>(accesslogquery, null);
            return userLogs;
        }

        public async Task<AttendanceViewModel> GetAttendanceDetailsById(string attendanceId)
        {
            var query = $@" select n.* from cms.""N_TAA_Attendance"" as n 
                            where n.""Id""='{attendanceId}' and n.""CompanyId""='{_repo.UserContext.CompanyId}'";
            var result = await _queryRepo.ExecuteQuerySingle<AttendanceViewModel>(query, null);
            return result;
        }

        public async Task<List<AttendanceToPayrollViewModel>> GetUserDetailsList(string orgId)
        {
            var query = $@" select distinct u.""Id"" as UserId, CONCAT(u.""Name"",' (',u.""Email"",') ') as UserNameWithEmail,hp.""Id"" as PersonId,CONCAT(hp.""FirstName"",' ',hp.""LastName"") as EmployeeName,hp.""SponsorshipNo"" as SponsorshipNo,hp.""PersonNo"" as PersonNo
                        from cms.""N_CoreHR_HRDepartment"" as hd
                        join cms.""N_CoreHR_HRAssignment"" as assi on assi.""DepartmentId""=hd.""Id"" and assi.""IsDeleted""=false and assi.""CompanyId""='{_repo.UserContext.CompanyId}'
                        join cms.""N_CoreHR_HRPerson"" as hp on hp.""Id""=assi.""EmployeeId"" and hp.""IsDeleted""=false and hp.""CompanyId""='{_repo.UserContext.CompanyId}'
                        join public.""User"" as u on u.""Id""=hp.""UserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
                        left join cms.""N_CoreHR_HRJob"" as hj on hj.""Id""=assi.""JobId"" and hj.""IsDeleted""=false and hj.""CompanyId""='{_repo.UserContext.CompanyId}'
                        where hd.""IsDeleted""=false and hd.""CompanyId""='{_repo.UserContext.CompanyId}' #OrgWhere#
                        order by EmployeeName
                        ";
            var orgwhere = "";
            if (orgId.IsNotNullAndNotEmpty())
            {
                orgwhere = $@" and hd.""Id""='{orgId}' ";
            }
            query = query.Replace("#OrgWhere#", orgwhere);
            var result = await _queryRepo.ExecuteQueryList<AttendanceToPayrollViewModel>(query, null);
            return result;
        }

        public async Task<List<AttendanceToPayrollViewModel>> GetAttendanceDetails(string orgId, DateTime first, DateTime last)
        {
            var query1 = $@" select u.""Id"" as UserId,n.""AttendanceDate"" as AttendanceDate,case when n.""Id"" is not null then n.""Id"" else '' end as Id,
                            CONCAT(coalesce(case when n.""OverrideAttendanceId"" is not null then substring(lovoa.""Name"", 1, 1) when n.""AttendanceTypeId"" is not null then substring(lovat.""Name"", 1, 1) else '' end, ''),
	                        coalesce(case when n.""OverrideOTHours"" is not null THEN CONCAT(' OT:', substring(n.""OverrideOTHours"", 1, 5))  when n.""SystemOTHours"" is not null then CONCAT(' OT:', substring(n.""SystemOTHours"", 1, 5)) else '' END, ''),
	                        coalesce(case when n.""OverrideDeductionHours"" is not null THEN CONCAT(' D:', substring(n.""OverrideDeductionHours"", 1, 5))  when n.""SystemDeductionHours"" is not null then CONCAT(' D:', substring(n.""SystemDeductionHours"", 1, 5)) else '' END, '')) as Day1,
                            lovpps.""Name"" as PayrollPostedStatus,
                            case when lovpps.""Code"" = 'SUBMITTED' or lovpps.""Code"" = 'POSTED' then true else false end as P1,
                            case when n.""OverrideOTHours"" is not null THEN n.""OverrideOTHours"" else n.""SystemOTHours"" END as OT1,
                            case when n.""OverrideDeductionHours"" is not null THEN n.""OverrideDeductionHours"" else n.""SystemDeductionHours"" END as D1
                            from cms.""N_CoreHR_HRDepartment"" as hd
                            join cms.""N_CoreHR_HRAssignment"" as assi on assi.""DepartmentId""=hd.""Id"" and assi.""IsDeleted""=false and assi.""CompanyId""='{_repo.UserContext.CompanyId}'
                            join cms.""N_CoreHR_HRPerson"" as hp on hp.""Id""=assi.""EmployeeId"" and hp.""IsDeleted""=false and hp.""CompanyId""='{_repo.UserContext.CompanyId}'
                            join public.""User"" as u on u.""Id""=hp.""UserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
                            join cms.""N_TAA_Attendance"" as n on n.""UserId""=u.""Id"" and n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}'
                            left join cms.""N_CoreHR_HRJob"" as hj on hj.""Id""=assi.""JobId"" and hj.""IsDeleted""=false and hj.""CompanyId""='{_repo.UserContext.CompanyId}'
                            left join public.""LOV"" as lovat on lovat.""Id""=n.""AttendanceTypeId"" and lovat.""IsDeleted""=false and lovat.""CompanyId""='{_repo.UserContext.CompanyId}'
                            left join public.""LOV"" as lovlt on lovlt.""Id""=n.""AttendanceLeaveTypeId"" and lovlt.""IsDeleted""=false and lovlt.""CompanyId""='{_repo.UserContext.CompanyId}'
                            left join public.""LOV"" as lovoa on lovoa.""Id""=n.""OverrideAttendanceId"" and lovoa.""IsDeleted""=false and lovoa.""CompanyId""='{_repo.UserContext.CompanyId}'
                            left join public.""LOV"" as lovpps on lovpps.""Id""=n.""PayrollPostedStatusId"" and lovpps.""IsDeleted""=false and lovpps.""CompanyId""='{_repo.UserContext.CompanyId}'
                            left join public.""LOV"" as lovas on lovas.""Id""=n.""ApprovalStatus"" and lovas.""IsDeleted""=false and lovas.""CompanyId""='{_repo.UserContext.CompanyId}'
                            where hd.""IsDeleted""=false and hd.""CompanyId""='{_repo.UserContext.CompanyId}' #FirstLastDateWhere# #OrgWhere#
                            ";
            var firstlastdatewhere = "";
            if (first.IsNotNull() && last.IsNotNull())
            {
                //firstlastdatewhere = $@" and '{first}'::Date<=n.""AttendanceDate""::Date<='{last}'::Date ";
                firstlastdatewhere = $@" and (n.""AttendanceDate""::Date BETWEEN '{first}'::Date AND '{last}'::Date) ";
            }

            var orgwhere = "";
            if (orgId.IsNotNullAndNotEmpty())
            {
                orgwhere = $@" and hd.""Id""='{orgId}' ";
            }

            query1 = query1.Replace("#OrgWhere#", orgwhere);
            query1 = query1.Replace("#FirstLastDateWhere#", firstlastdatewhere);
            var result1 = await _queryRepo.ExecuteQueryList<AttendanceToPayrollViewModel>(query1, null);
            return result1;
        }

        public async Task<List<AttendanceToPayrollViewModel>> LeaveQuery(List<TemplateViewModel> templateList, string users)
        {
            var leavequery = "";
            var i = 1;
            foreach (var item in templateList.Where(x => x.UdfTableMetadataId != null))
            {
                var tableMeta = await _tableMetadataBusiness.GetSingle(x => x.Id == item.UdfTableMetadataId);
                if (item.Code == "UndertimeLeave" || item.Code == "LeaveAdjustment" || item.Code == "AnnualLeaveEncashment"
                    || item.Code == "LeaveHandoverService" || item.Code == "LEAVE_CANCEL" || item.Code == "RETURN_TO_WORK" || item.Code == "LeaveAccrual")
                {

                }
                else
                {
                    if (i != 1)
                    {
                        leavequery += " union ";
                    }
                    leavequery = @$" {leavequery} select u.""Id"" as UserId,substring(tr.""Name"", 9) as Leave,
                                lvt.""LeaveStartDate"" as StartDate,lvt.""LeaveEndDate"" as EndDate
                                from public.""User"" as u
                                join public.""NtsService"" as s on s.""OwnerUserId""=u.""Id"" and s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}'
                                join public.""LOV"" as lov on lov.""Id""=s.""ServiceStatusId"" and lov.""IsDeleted""=false and lov.""Code""='SERVICE_STATUS_COMPLETE' and lov.""CompanyId""='{_repo.UserContext.CompanyId}'
                                join public.""Template"" as tr on tr.""Id""=s.""TemplateId"" and tr.""IsDeleted""=false and tr.""CompanyId""='{_repo.UserContext.CompanyId}'
			                    join public.""TemplateCategory"" as tc on tc.""Id""=tr.""TemplateCategoryId"" and tc.""Code""='Leave' and tc.""IsDeleted""=false  and tc.""CompanyId""='{_repo.UserContext.CompanyId}'
                                join cms.""{tableMeta.Name}"" as lvt on lvt.""NtsNoteId""=s.""UdfNoteId"" and lvt.""IsDeleted""=false  and lvt.""CompanyId""='{_repo.UserContext.CompanyId}'
                                Where u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'  #UserWhere#
                                ";
                    i++;
                }

            }
            var userwhere = "";
            if (users.IsNotNullAndNotEmpty())
            {
                userwhere = @" and u.""Id"" in (" + users + ") ";
            }
            leavequery = leavequery.Replace("#UserWhere#", userwhere);
            var leave = await _queryRepo.ExecuteQueryList<AttendanceToPayrollViewModel>(leavequery, null);
            return leave;
        }

        public async Task<AttendanceViewModel> GetAttendancebyIdandDate(DateTime payRollDate, string payRollRunId)
        {
            //var cypher = @"match (n:TAA_Attendance) where datetime(n.AttendanceDate) <= datetime({date}) and n.PayrollPostedStatus = 'Submitted' with n match (n)-[:R_Attendance_User]-(u:ADM_User)
            //match(u)-[:R_User_PersonRoot]-(pr:HRS_PersonRoot{ IsDeleted: 0,Status: {Status},CompanyId: {CompanyId}})-[:R_PersonRoot]-(p:HRS_Person)-[:R_Person_Nationality]->(nation:HRS_Nationality) where datetime(p.EffectiveStartDate) <= datetime({date}) <= datetime(p.EffectiveEndDate)
            //match(pr)<-[:R_PayrollRun_PersonRoot]-(payR:PAY_PayrollRun{Id:{payRollRunId}})
            //optional match (n)<-[:R_Service_Reference{ReferenceTypeCode:'TAA_Attendance'}]-(s:NTS_Service)-[:R_Service_Status_ListOfValue]->(g:GEN_ListOfValue)
            //with n,u, g, s, pr, nation
            //where (g.Code in ['COMPLETED', 'CANCELED'] or g.Code is null)
            //with n,u,g,s,pr, nation
            //match (pr)<-[:R_SalaryInfoRoot_PersonRoot]-(psr:PAY_SalaryInfoRoot)
            // match(psr)<-[psrr:R_SalaryElementInfo_SalaryInfoRoot]-(ps:PAY_SalaryElementInfo) where datetime(ps.EffectiveStartDate) <= datetime({date}) <= datetime(ps.EffectiveEndDate)
            // match(ps)-[:R_SalaryElementInfo_ElementRoot]->(pe:PAY_ElementRoot{ IsDeleted: 0,CompanyId: 1 })<-[:R_ElementRoot]-(e:PAY_Element{ IsDeleted: 0,CompanyId: 1 })
            //where e.Code = 'BASIC' and datetime(e.EffectiveStartDate) <= datetime(n.AttendanceDate) <= datetime(e.EffectiveEndDate)
            //set n.PayrollPostedStatus = 'Posted' return 1";
            //await _queryRepo.ExecuteCommand(cypher, null);

            var referenceType = (int)ReferenceTypeEnum.TAA_Attendance;
            var query = $@" select n.* from cms.""N_TAA_Attendance"" as n
                        left join public.""LOV"" as lov on lov.""Id""=n.""PayrollPostedStatusId"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_repo.UserContext.CompanyId}'
                        join public.""User"" as u on u.""Id""=n.""UserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
                        join cms.""N_CoreHR_HRPerson"" as p on p.""UserId""=u.""Id"" and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
                        join cms.""N_CoreHR_HRNationality"" as nation on nation.""Id""=p.""NationalityId"" and nation.""IsDeleted""=false  and nation.""CompanyId""='{_repo.UserContext.CompanyId}'
                        join cms.""N_PayrollHR_PayrollRunPerson"" as pr on pr.""PersonId""=p.""Id"" and pr.""IsDeleted""=false and pr.""CompanyId""='{_repo.UserContext.CompanyId}'
                        join cms.""N_PayrollHR_PayrollRun"" as pay on pay.""Id""=pr.""PayrollRunId"" and pay.""IsDeleted""=false and pay.""CompanyId""='{_repo.UserContext.CompanyId}'
                        --left join public.""NtsService"" as s on s.""ReferenceId""=n.""Id"" and s.""ReferenceType""= '{referenceType}' and s.""IsDeleted""=false  and s.""CompanyId""='{_repo.UserContext.CompanyId}'
                        --left join public.""LOV"" as lovs on lovs.""Id""=s.""ServiceStatusId"" and lovs.""IsDeleted""=false and lovs.""CompanyId""='{_repo.UserContext.CompanyId}'
                        join cms.""N_PayrollHR_SalaryInfo"" as psr on psr.""PersonId""=p.""Id"" and psr.""IsDeleted""=false and psr.""CompanyId""='{_repo.UserContext.CompanyId}'
                        join public.""NtsNote"" as note on note.""ParentNoteId""=psr.""NtsNoteId"" and note.""IsDeleted""=false and note.""CompanyId""='{_repo.UserContext.CompanyId}'
                        join cms.""N_PayrollHR_SalaryElementInfo"" as sei on sei.""NtsNoteId""=note.""Id"" and sei.""IsDeleted""=false  and sei.""CompanyId""='{_repo.UserContext.CompanyId}'
                        and sei.""EffectiveStartDate""::Date<='{payRollDate}'::Date and '{payRollDate}'::Date<=sei.""EffectiveEndDate""::Date
                        join cms.""N_PayrollHR_PayrollElement"" as e on e.""Id""=sei.""ElementId"" and e.""IsDeleted""=false  and e.""CompanyId""='{_repo.UserContext.CompanyId}'
                        and e.""EffectiveStartDate""::Date<=n.""AttendanceDate""::Date and n.""AttendanceDate""::Date<=e.""EffectiveEndDate""::Date
                        where n.""IsDeleted""=false and lov.""Code""='SUBMITTED' and e.""ElementCode""='BASIC' and pay.""Id""='{payRollRunId}' and n.""CompanyId""='{_repo.UserContext.CompanyId}'
                        limit 1
                ";
            var querydata = await _queryRepo.ExecuteQuerySingle<AttendanceViewModel>(query, null);
            return querydata;

        }

        public async Task UpdateAttendance(string id, string payrollPostedStatusId)
        {
            var query1 = $@"Update cms.""N_TAA_Attendance"" 
                                set ""PayrollPostedStatusId""='{payrollPostedStatusId}'
                                where ""Id""='{id}' ";
            await _queryRepo.ExecuteCommand(query1, null);
        }

        public async Task<List<AttendanceViewModel>> GetPostAttendanceToPayrollDetails(string personIds, DateTime startDate, DateTime endDate)
        {
            var attendancequery = $@" select n.*
                                                from cms.""N_TAA_Attendance"" as n
                                                join public.""User"" as u on u.""Id""=n.""UserId"" and u.""IsDeleted""=false  and u.""CompanyId""='{_repo.UserContext.CompanyId}'
                                                where n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}' and n.""IsCalculated""='True' and n.""PayrollPostedStatusId"" is null #FirstLastDateWhere# #UserWhere#
                                            ";
            string users = null;
            var userIds = personIds.Split(",");
            foreach (var j in userIds)
            {
                if (j.IsNotNullAndNotEmpty())
                {
                    users += $"'{j}',";
                }
            }
            users = users.Trim(',');

            var userwhere = "";
            if (users.IsNotNullAndNotEmpty())
            {
                userwhere = @" and u.""Id"" in (" + users + ") ";
            }
            var firstlastdatewhere = "";
            if (startDate.IsNotNull() && endDate.IsNotNull())
            {
                firstlastdatewhere = $@" and (n.""AttendanceDate""::Date BETWEEN '{startDate}'::Date AND '{endDate}'::Date) ";
            }
            attendancequery = attendancequery.Replace("#UserWhere#", userwhere);
            attendancequery = attendancequery.Replace("#FirstLastDateWhere#", firstlastdatewhere);
            var result = await _queryRepo.ExecuteQueryList<AttendanceViewModel>(attendancequery, null);
            return result;
        }

        public async Task<bool?> UpdateAttendanceAfterApproval(string attendanceIds)
        {
            var updatequery = $@" update cms.""N_TAA_Attendance""
                                                set ""PayrollPostedStatusId"" = (select lov.""Id"" as lovId from public.""LOV"" as lov 
                                                where lov.""LOVType""='PayrollPostedStatus' and lov.""Code""='SUBMITTED' and lov.""IsDeleted""=false)
                                                where ""IsDeleted""=false #AttendanceWhere#
                                                returning true
                                            ";
            var attendancewhere = "";
            if (attendanceIds.IsNotNullAndNotEmpty())
            {
                attendancewhere = @" and ""Id"" in (" + attendanceIds + ") ";
            }
            updatequery = updatequery.Replace("#AttendanceWhere#", attendancewhere);
            var updateresult = await _queryRepo.ExecuteScalar<bool?>(updatequery, null);
            return updateresult;
        }

        public async Task<List<string>> CheckAnyPendingAttendanceApproval(string personIds, DateTime startDate, DateTime endDate, string users)
        {
            var query = $@" select CONCAT(' (',s.""ServiceNo"",') ','Service is Pending for Approval for User : ',u.""Name"") as Message
                            from cms.""N_TAA_Attendance"" as n
                            join public.""User"" as u on u.""Id""=n.""UserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
                            join public.""NtsService"" as s on s.""OwnerUserId""=u.""Id"" and s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}'
                            join public.""LOV"" as lov on lov.""Id""=s.""ServiceStatusId"" and lov.""IsDeleted""=false and (lov.""Code""<>'SERVICE_STATUS_COMPLETE' or lov.""Code""<>'SERVICE_STATUS_CLOSE')  and lov.""CompanyId""='{_repo.UserContext.CompanyId}'
                            join public.""Template"" as tr on tr.""Id""=s.""TemplateId"" and tr.""IsDeleted""=false and tr.""CompanyId""='{_repo.UserContext.CompanyId}'
			                join public.""TemplateCategory"" as tc on tc.""Id""=tr.""TemplateCategoryId"" and tc.""Code""='Leave' and tc.""IsDeleted""=false and tc.""CompanyId""='{_repo.UserContext.CompanyId}'
                            where n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}' and n.""IsCalculated""='True' and n.""PayrollPostedStatusId"" is null #FirstLastDateWhere# #UserWhere#
                            
                            union
                            select CONCAT('System is Waiting for Biometric Entry for User : ',u.""Name"") as Message
                            from cms.""N_TAA_Attendance"" as n
                            join public.""User"" as u on u.""Id""=n.""UserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
                            where n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}' and n.""IsCalculated""<>'True' #FirstLastDateWhere# #UserWhere#

                            ";

            var userwhere = "";
            if (users.IsNotNullAndNotEmpty())
            {
                userwhere = @" and u.""Id"" in (" + users + ") ";
            }
            var firstlastdatewhere = "";
            if (startDate.IsNotNull() && endDate.IsNotNull())
            {
                firstlastdatewhere = $@" and (n.""AttendanceDate""::Date BETWEEN '{startDate}'::Date AND '{endDate}'::Date) ";
            }
            query = query.Replace("#UserWhere#", userwhere);
            query = query.Replace("#FirstLastDateWhere#", firstlastdatewhere);
            var result = await _queryRepo.ExecuteScalarList<string>(query, null);
            return result;
        }

        public async Task<List<AttendanceViewModel>> GetAttendanceListByDate(DateTime attendanceDate)
        {
            var cypher = $@"Select a.*,u.""Id"" as UserId,p.""BiometricId"" as BiometricId
from cms.""N_TAA_Attendance"" as a 
join public.""User"" as u on u.""Id""=a.""UserId"" and a.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRPerson"" as p on p.""UserId""=u.""Id"" and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
where a.""AttendanceDate""='{attendanceDate}' and a.""CompanyId""='{_repo.UserContext.CompanyId}' and a.""IsDeleted""=false
";
            //var cypher = @"MATCH (a:TAA_Attendance{AttendanceDate:{AttendanceDate},IsDeleted:0,Status:'Active'})
            //-[:R_Attendance_User]-(u:ADM_User{IsDeleted:0})-[:R_User_PersonRoot]->(pr:HRS_PersonRoot)
            //<-[:R_PersonRoot]-(p:HRS_Person{IsDeleted:0,Status:'Active',IsLatest:true})
            //return a,u.Id as UserId,p.BiometricId as BiometricId";
            var result = await _queryRepo.ExecuteQueryList<AttendanceViewModel>(cypher, null);
            return result;
        }

        public async Task<List<AttendanceViewModel>> GetTotalOtAndDeduction(string userId, DateTime startDate, DateTime endDate)
        {
            var query = $@"select coalesce(n.""OverrideOTHours"",n.""SystemOTHours"") as OTHours, coalesce(n.""OverrideDeductionHours"", n.""SystemDeductionHours"") as DeductionHours
                            from cms.""N_TAA_Attendance"" as n
                            join public.""User"" as u on u.""Id""=n.""UserId"" and u.""IsDeleted""=false and u.""Id""='{userId}' and u.""CompanyId""='{_repo.UserContext.CompanyId}'
                            left join public.""LOV"" as lov on lov.""Id""=n.""PayrollPostedStatusId"" and lov.""IsDeleted""=false and lov.""Code""='POSTED' and lov.""CompanyId""='{_repo.UserContext.CompanyId}'
                            where n.""IsDeleted""=false and n.""IsCalculated""='True' and n.""CompanyId""='{_repo.UserContext.CompanyId}'
                                and n.""AttendanceDate""::Date>='{startDate}'::Date and n.""AttendanceDate""::Date<='{endDate}'::Date ";
            var list = await _queryRepo.ExecuteQueryList<AttendanceViewModel>(query, null);
            return list;

        }

        public async Task<IList<TimeinTimeoutDetailsViewModel>> GetTimeinTimeOutDetails(string Empid, DateTime? Datefrom, DateTime? DateTo, DateTime MonthSearch, string Type)
        {



            //var selectQry = $@"SELECT A.""AttendanceDate"" as Date,CONCAT( cast( R.""Duty1StartTime""::TIMESTAMP::Time as text),'_',cast( R.""Duty1EndTime""::TIMESTAMP::Time as text)) as Roster,
            //CONCAT(cast(A.""Duty1StartTime""::TIMESTAMP::Time as text), '_', cast(A.""Duty1EndTime""::TIMESTAMP::Time as text)) as Actual,A.""EmployeeComments"",A.""OverrideComments"" from cms.""N_TAA_Attendance"" as A inner join cms.""N_TAA_RosterSchedule""  as R on A.""AttendanceDate""::TIMESTAMP::DATE=R.""RosterDate""::TIMESTAMP::DATE  and  A.""UserId""=R.""UserId"" where  A.""UserId""='{Empid}' and  #fromTo#";

            var selectQry = $@"SELECT A.""AttendanceDate""
as Date,
CONCAT(substring (cast(R.""Duty1StartTime"" as text),0,6), '_', substring(cast(R.""Duty1EndTime"" as text),0,6)) as Roster,
CONCAT(substring(cast(R.""Duty2StartTime"" as text), 0, 6), '_', substring(cast(R.""Duty2EndTime"" as text), 0, 6)) as Duty2Roster,
CONCAT(substring(cast(R.""Duty3StartTime"" as text), 0, 6), '_', substring(cast(R.""Duty3EndTime"" as text), 0, 6)) as Duty3Roster,

CONCAT(substring(cast(A.""Duty1StartTime"" as text), 0, 6), '_', substring(cast(A.""Duty1EndTime"" as text), 0, 6)) as Actual,
CONCAT(substring(cast(A.""Duty2StartTime"" as text), 0, 6), '_', substring(cast(A.""Duty2EndTime"" as text), 0, 6)) as Duty2Actual,
CONCAT(substring(cast(A.""Duty3StartTime"" as text), 0, 6), '_', substring(cast(A.""Duty3EndTime"" as text), 0, 6)) as Duty3Actual,
A.""EmployeeComments"",A.""OverrideComments""
from cms.""N_TAA_Attendance"" as A 
left join cms.""N_TAA_RosterSchedule""
as R on A.""AttendanceDate""::TIMESTAMP::DATE = R.""RosterDate""::TIMESTAMP::DATE and R.""CompanyId""='{_repo.UserContext.CompanyId}'
and A.""UserId"" = R.""UserId"" and R.""IsDeleted""=false
where A.""UserId"" = '{Empid}' and A.""CompanyId""='{_repo.UserContext.CompanyId}' and A.""IsDeleted""=false and  #fromTo#";


            if (Type == "Manual")

            {
                string Qr = $@"A.""AttendanceDate""::TIMESTAMP::DATE>='{Datefrom.ToYYYY_MM_DD_DateFormat()}'::TIMESTAMP::DATE and A.""AttendanceDate""::TIMESTAMP::DATE<= '{DateTo.ToYYYY_MM_DD_DateFormat()}'::TIMESTAMP::DATE";

                selectQry = selectQry.Replace("#fromTo#", Qr);
            }
            else if (Type == "Monthly")
            {

                var DaysInMonth = DateTime.DaysInMonth(MonthSearch.Year, MonthSearch.Month);
                var lastDay = new DateTime(MonthSearch.Year, MonthSearch.Month, DaysInMonth);

                string Qr = $@"A.""AttendanceDate""::TIMESTAMP::DATE>='{MonthSearch.ToYYYY_MM_DD_DateFormat()}'::TIMESTAMP::DATE and A.""AttendanceDate""::TIMESTAMP::DATE<= '{lastDay.ToYYYY_MM_DD_DateFormat()}'::TIMESTAMP::DATE";

                selectQry = selectQry.Replace("#fromTo#", Qr);
            }



            var queryData = await _queryRepo.ExecuteQueryList<TimeinTimeoutDetailsViewModel>(selectQry, null);

            return queryData;
        }

        public async Task<List<TimePermissionAttendanceViewModel>> GetReportTimePermissionList(List<TemplateViewModel> templateList)
        {
            //cypher = string.Concat(@"match (tr:NTS_TemplateMaster{IsDeleted:0,Status: 'Active'})  
            //                    where tr.Code in ['TIME_PERMISSION_PERSONAL_UAE','TIME_PERMISSION_BUSINESS_UAE','TIME_PERMISSION_PERSONAL_KSA'
            //                    ,'TIME_PERMISSION_BUSINESS_KSA']
            //                    match(tr) < -[:R_TemplateRoot]-(t: NTS_Template{ IsDeleted: 0,Status: 'Active'})  
            //                    match(t) < -[:R_Service_Template] - (s:NTS_Service{ IsDeleted: 0,Status: 'Active',CompanyId: {CompanyId}})
            //                    match(s) -[:R_Service_Owner_User]->(u:ADM_User{ IsDeleted: 0,Status: 'Active',CompanyId: {CompanyId}})
            //                    where not s.TemplateAction in ['Draft','Cancel']
            //                     match(s) < -[:R_ServiceFieldValue_Service] - (nfv1: NTS_ServiceFieldValue{ IsDeleted: 0})
            //                    -[:R_ServiceFieldValue_TemplateField]->(tf1{ FieldName: 'hours',IsDeleted: 0})
            //                    match(s) < -[:R_ServiceFieldValue_Service] - (nfv2: NTS_ServiceFieldValue{ IsDeleted: 0})
            //                    -[:R_ServiceFieldValue_TemplateField]->(tf2{ FieldName: 'date',IsDeleted: 0})
            //                    optional match(s) < -[:R_ServiceFieldValue_Service] - (nfv3: NTS_ServiceFieldValue{ IsDeleted: 0})
            //                    -[:R_ServiceFieldValue_TemplateField]->(tf3{ FieldName: 'type',IsDeleted: 0})
            //                     optional match(s) < -[:R_ServiceFieldValue_Service] - (nfv4: NTS_ServiceFieldValue{ IsDeleted: 0})
            //                   -[:R_ServiceFieldValue_TemplateField]->(tf4{ FieldName: 'reason',IsDeleted: 0})
            //                    optional match(s)-[:R_Service_Status_ListOfValue]-(ns:GEN_ListOfValue{IsDeleted:0})     
            //                    optional match(lov:GEN_ListOfValue) where lov.Id = toInt( nfv4.Code) and nfv4.Code is not null
            //                    RETURN s.ServiceNo as ServiceNo, s.Id as ServiceId,nfv1.Code as Hours
            //                    ,toString(date(datetime(nfv2.Code)))+'T00:00:00.0000000' as Date,u.Id as UserId, 
            //                    tr.Name as Name, ns.Name as Status, nfv3.Code as TimePermissionType, lov.Name as TimePermissionReason,
            //                    u.UserName as ServiceOwner,tr.Code as LeaveTypeCode,s.TemplateAction as TemplateAction");            


            var selectQry = "";
            var i = 1;
            foreach (var item in templateList.Where(x => x.UdfTableMetadataId != null))
            {
                var tableMeta = await _tableMetadataBusiness.GetSingle(x => x.Id == item.UdfTableMetadataId);

                if (item.Code == "TimePermissionPersonal" || item.Code == "TimePermissionBusiness")
                {
                    if (i != 1)
                    {
                        selectQry += " union ";
                    }

                    selectQry = @$" {selectQry} Select s.""ServiceNo"", s.""Id"" as ServiceId, udf.""Hours"" as Hours,  udf.""Date""::TIMESTAMP::DATE as FromDate, u.""Id"" as UserId, u.""Name"" as ServiceOwner,
                            t.""DisplayName"" as Name, tpt.""Name"" as TimePermissionTypes, t.""Code"" as LeaveTypeCode, lov.""Name"" as ServiceStatus
                            From cms.""{tableMeta.Name}"" as udf
                            Join public.""NtsNote"" as n on udf.""NtsNoteId""=n.""Id"" and n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Left Join public.""NtsService"" as s on n.""Id""=s.""UdfNoteId"" and s.""IsDeleted""=false  and s.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Left Join public.""Template"" as t on s.""TemplateId""=t.""Id""  and t.""CompanyId""='{_repo.UserContext.CompanyId}' and t.""IsDeleted""=false
                            Left Join public.""Template"" as UdfTemplate on s.""UdfTemplateId""=UdfTemplate.""Id"" and UdfTemplate.""IsDeleted""=false and UdfTemplate.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Left join public.""ServiceTemplate"" as st on st.""TemplateId""=t.""Id"" and st.""IsDeleted""=false  and st.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Left Join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Left Join public.""LOV"" as tpt on tpt.""Id""=udf.""TimePermissionType"" and tpt.""CompanyId""='{_repo.UserContext.CompanyId}' and  tpt.""IsDeleted""=false                           
                            Left Join public.""LOV"" as lov on s.""ServiceStatusId""=lov.""Id"" and lov.""Code"" not in ('SERVICE_STATUS_DRAFT','SERVICE_STATUS_CANCEL') and lov.""CompanyId""='{_repo.UserContext.CompanyId}'  and  udf.""IsDeleted""=false       ";
                    i++;
                }

            }

            var querydata = await _queryRepo.ExecuteQueryList<TimePermissionAttendanceViewModel>(selectQry, null);

            return querydata;
        }

        public async Task<List<EmployeeServiceViewModel>> GetReportBusinessTripList(List<TemplateViewModel> templateList)
        {
            //var cypher = string.Concat(@"match (tr:NTS_TemplateMaster{IsDeleted:0,Status: 'Active'})  
            //                    where tr.Code in ['BUSINESS_TRIP_SERVICE_UAE','BUSINESS_TRIP_SERVICE']
            //                    match(tr) < -[:R_TemplateRoot]-(t: NTS_Template{ IsDeleted: 0,Status: 'Active'})  
            //                    match(t) < -[:R_Service_Template] - (s: NTS_Service{ IsDeleted: 0,Status: 'Active',CompanyId: {CompanyId}})
            //                    match(s) -[:R_Service_Owner_User]->(u: ADM_User{ IsDeleted: 0,Status: 'Active',CompanyId: {CompanyId}})
            //                    where not s.TemplateAction in ['Draft','Cancel']
            //                     match(s) < -[:R_ServiceFieldValue_Service] - (nfv1: NTS_ServiceFieldValue{ IsDeleted: 0})
            //                    -[:R_ServiceFieldValue_TemplateField]->(tf1{ FieldName: 'startDate',IsDeleted: 0})
            //                    match(s) < -[:R_ServiceFieldValue_Service] - (nfv2: NTS_ServiceFieldValue{ IsDeleted: 0})
            //                    -[:R_ServiceFieldValue_TemplateField]->(tf2{ FieldName: 'endDate',IsDeleted: 0})

            //                    optional match(s)-[:R_Service_Status_ListOfValue]-(ns:GEN_ListOfValue{IsDeleted:0})     

            //                    RETURN s.ServiceNo as ServiceNo, s.Id as ServiceId,nfv1.Code as StartDate, nfv2.Code as EndDate,u.Id as UserId, 
            //                    tr.Name as Name, ns.Name as Status, u.UserName as ServiceOwner,tr.Code as LeaveTypeCode,s.TemplateAction as TemplateAction");            



            var selectQry = "";
            var i = 1;
            foreach (var item in templateList.Where(x => x.UdfTableMetadataId != null))
            {
                var tableMeta = await _tableMetadataBusiness.GetSingle(x => x.Id == item.UdfTableMetadataId);

                if (item.Code == "BuisnessTrip")
                {
                    if (i != 1)
                    {
                        selectQry += " union ";
                    }

                    selectQry = @$" {selectQry} Select s.""ServiceNo"", s.""Id"" as ServiceId,  udf.""BusinessTripStartDate""::TIMESTAMP::DATE as StartDate,   udf.""BusinessTripEndDate"" ::TIMESTAMP::DATE as EndDate, u.""Id"" as UserId, u.""Name"" as ServiceOwner,
                            t.""DisplayName"" as Name, t.""Code"" as LeaveTypeCode, lov.""Name"" as ServiceStatus
                            From cms.""{tableMeta.Name}"" as udf
                            Join public.""NtsNote"" as n on udf.""NtsNoteId""=n.""Id"" and n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Left Join public.""NtsService"" as s on n.""Id""=s.""UdfNoteId"" and s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Left Join public.""Template"" as t on s.""TemplateId""=t.""Id"" and t.""CompanyId""='{_repo.UserContext.CompanyId}' and t.""IsDeleted""=false
                            Left Join public.""Template"" as UdfTemplate on s.""UdfTemplateId""=UdfTemplate.""Id"" and UdfTemplate.""IsDeleted""=false and UdfTemplate.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Left join public.""ServiceTemplate"" as st on st.""TemplateId""=t.""Id"" and st.""IsDeleted""=false and st.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Left Join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""CompanyId""='{_repo.UserContext.CompanyId}'  and u.""IsDeleted""=false                                                    
                            Left Join public.""LOV"" as lov on s.""ServiceStatusId""=lov.""Id"" and lov.""Code"" not in ('SERVICE_STATUS_DRAFT','SERVICE_STATUS_CANCEL') and lov.""CompanyId""='{_repo.UserContext.CompanyId}' 
where  udf.""IsDeleted""=false and udf.""CompanyId""='{_repo.UserContext.CompanyId}'";
                    i++;
                }

            }

            var querydata = await _queryRepo.ExecuteQueryList<EmployeeServiceViewModel>(selectQry, null);
            return querydata;
        }
        public async Task CloseOldRosters(DateTime date)
        {
            date = date.AddDays(-10);
            //var cypher = @"match (r:TAA_RosterSchedule) where r.RosterDate<{Date}  set r.IsAttendanceCalculated=true";
            //ExecuteCypherWithoutResult(cypher, new Dictionary<string, object> { { "Date", date } });

            var cypher = $@"update cms.""N_TAA_RosterSchedule"" as r set r.""IsAttendanceCalculated""=true where r.""RosterDate""<'{date}'  ";
            await _queryRepo.ExecuteCommand(cypher, null);
        }
        public async Task<List<SalaryInfoViewModel>> GetAllEmployeesForAttendance()
        {
            // var prms = new Dictionary<string, object>();
            // prms.Add("ESD", DateTime.Today);
            //var match = string.Concat(@"MATCH(pr:HRS_PersonRoot{ IsDeleted: 0})<-[:R_User_PersonRoot]-(u:ADM_User{IsDeleted:0})
            //optional match (pr)<-[:R_SalaryInfoRoot_PersonRoot]-(sir:PAY_SalaryInfoRoot)
            //optional match (sir)<-[:R_SalaryInfoRoot]-(si:PAY_SalaryInfo)
            //where si.EffectiveStartDate<={ESD}<=si.EffectiveEndDate
            //return u.Id as UserId,coalesce(si.TakeAttendanceFromTAA,false) as TakeAttendanceFromTAA");
            var match = string.Concat($@"select u.""Id"" as UserId,coalesce(si.""UseTimeAndAttendanceModule"",'false') as TakeAttendanceFromTAA
from public.""User"" as u 
join cms.""N_CoreHR_HRPerson"" as p on p.""UserId""=u.""Id"" and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_PayrollHR_SalaryInfo"" as si on si.""PersonId""=p.""Id"" and si.""IsDeleted""=false and si.""CompanyId""='{_repo.UserContext.CompanyId}' where u.""CompanyId""='{_repo.UserContext.CompanyId}' and u.""IsDeleted""=false
");
            return await _queryRepo.ExecuteQueryList<SalaryInfoViewModel>(match, null);
        }

        public async Task<List<AccessLogViewModel>> GetAccessLogDetail(DateTime accessEnd, DateTime accessStart)
        {
            var match = string.Concat($@"select a.* 
from public.""User"" as u 
join cms.""N_CLK_AccessLog"" as a on a.""UserId""=u.""Id"" and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'
where a.""PunchingTime"" >'{accessStart}' and  a.""PunchingTime"" <'{accessEnd}' and u.""Id""!=null and u.""CompanyId""='{_repo.UserContext.CompanyId}' and u.""IsDeleted""=false
");
            return await _queryRepo.ExecuteQueryList<AccessLogViewModel>(match, null);
        }

        public async Task<List<AttendanceViewModel>> GetAttendanceListByDate(List<string> orgId, List<string> personId, DateTime? fromdate, DateTime? todate, List<string> empStatus, string payrollRunId = null)
        {
            var query = $@"select distinct u.""Email"",p.""PersonFullName"" as EmployeeName,
u.""Id"" as UserId,p.""Id"" as PersonId,p.""SponsorshipNo"" as SponsorshipNo,
 p.""PersonNo"" as PersonNo,j.""JobTitle"" as JobName,d.""DepartmentName"" as OrganizationName,lovs.""Name"" as EmployeeStatus,

coalesce(rs2.""AttendanceDate"",rs1.""RosterDate"") as AttDate,rs1.""ShiftPatternName"" as ShiftPatternName,
lovat.""Name"" as SystemAttendance,
coalesce(rs2.""OverrideOTHours"",rs2.""SystemOTHours"") as SystemOTHours,
coalesce(rs2.""OverrideDeductionHours"",rs2.""SystemDeductionHours"") as SystemDeductionHours,
         rs2.""EmployeeComments"" as EmployeeComments, rs2.""OverrideComments"" as OverrideComments,  

coalesce(substring(rs1.""Duty1StartTime"", 0, 6) || '-' || substring(rs1.""Duty1EndTime"", 0, 6),'') || coalesce('<br/>' || substring(rs1.""Duty2StartTime"", 0, 6) || '-' || substring(rs1.""Duty2EndTime"", 0, 6),'') || coalesce('<br/>' || substring(rs1.""Duty3StartTime"", 0, 6) || '-' || substring(rs1.""Duty3EndTime"", 0, 6),'') || coalesce(case when rs1.""RosterDutyTypeId""='2' then rs1.""RosterDutyTypeId"" end,'') as RosterText,
coalesce(substring(rs2.""Duty1StartTime"", 0, 6) || '-' || substring(rs2.""Duty1EndTime"", 0, 6),'') || coalesce('<br/>' || substring(rs2.""Duty2StartTime"", 0, 6) || '-' || substring(rs2.""Duty2EndTime"", 0, 6),'') || coalesce('<br/>' || substring(rs2.""Duty3StartTime"", 0, 6) || '-' || substring(rs2.""Duty3EndTime"", 0, 6),'') as ActualText,
case when rs1.""TotalHours"" is not null THEN substring(rs1.""TotalHours"", 0, 5) ELSE '0.0' END as RosterHours,
case when rs2.""TotalHours"" is not null THEN substring(rs2.""TotalHours"", 0, 5) ELSE '0.0' END as ActualHours,
case when rs2.""AttendanceDate"" is not null THEN rs2.""AttendanceDate"" ELSE rs1.""RosterDate"" END as AttendanceDate,
case when rs2.""Id"" is null THEN '0' ELSE rs2.""Id"" END as Id 
,rs2.""Duty1StartTime"" as Duty1StartTime,rs2.""Duty1EndTime"" as Duty1EndTime,
            case when rs2.""IsOverridden"" is null THEN 'false' ELSE rs2.""IsOverridden"" END as IsOverridden,
            case when rs2.""IsApproved"" is null THEN 'false' ELSE rs2.""IsApproved"" END as Approved
--order by rs1.""RosterDate"" desc,rs2.""AttendanceDate"" desc
 from cms.""N_CoreHR_HRDepartment"" as d
            join cms.""N_CoreHR_HRAssignment"" as a on a.""DepartmentId""=d.""Id"" and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'
            join cms.""N_CoreHR_HRJob"" as j on a.""JobId""=j.""Id"" and j.""IsDeleted""=false and j.""CompanyId""='{_repo.UserContext.CompanyId}'
            join cms.""N_CoreHR_HRPerson"" as p on a.""EmployeeId""=p.""Id"" and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
            join public.""User"" as u on u.""Id""=p.""UserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_TAA_RosterSchedule"" as rs1 on u.""Id""=rs1.""UserId"" and rs1.""IsDeleted""=false  and rs1.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_TAA_Attendance"" as rs2 on u.""Id""=rs2.""UserId"" and rs2.""IsDeleted""=false and rs2.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_CoreHR_HRNationality"" as n on n.""Id""=p.""NationalityId"" and n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}'
--left join cms.""N_CoreHR_HRSection"" as s on s.""Id""=p.""NationalityId"" and n.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_CoreHR_HRContract"" as c on c.""EmployeeId""=p.""Id"" and c.""IsDeleted""=false and c.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_CoreHR_HRSponsor"" as sp on sp.""Id""=c.""SponsorId"" and sp.""IsDeleted""=false and sp.""CompanyId""='{_repo.UserContext.CompanyId}'
left join public.""LOV"" as lovs on lovs.""Id""=p.""PersonalStatusId"" and lovs.""IsDeleted""=false and lovs.""CompanyId""='{_repo.UserContext.CompanyId}'
left join public.""LOV"" as lovat on lovat.""Id""=rs2.""AttendanceTypeId"" and lovat.""IsDeleted""=false and lovat.""CompanyId""='{_repo.UserContext.CompanyId}'
where rs1.""RosterDate"" >='{fromdate}' and rs1.""RosterDate""<='{todate}' and d.""CompanyId""='{_repo.UserContext.CompanyId}' and d.""IsDeleted""=false  #where#";


            var where = "";
            var orgIds = "";
            if (orgId.Count > 0)
            {
                orgIds = "'" + string.Join("','", orgId) + "'";
                where = $@"and d.""Id"" in ({orgIds}) ";
            }
            var personIds = "";
            if (personId.Count > 0)
            {
                personIds = "'" + string.Join("','", personId) + "'";
                where += $@"and p.""Id"" in ({personIds})";
            }
            var empStatuss = "";
            if (empStatus.Count > 0)
            {
                empStatuss = "'" + string.Join("','", empStatus) + "'";
                where += $@"and p.""PersonalStatusId"" in ({empStatuss})";
            }

            //if (orgId.IsNotNullAndNotEmpty())
            //{
            //    where = $@"and d.""Id"" in ({orgIds}) ";
            //}
            //if (personId.IsNotNullAndNotEmpty())
            //{
            //    where += $@"and p.""Id"" in ({personId})";
            //}

            //if (empStatus.IsNotNullAndNotEmpty())
            //{
            //    where += $@"and p.""PersonalStatusId"" in ({empStatus})";
            //}
            query = query.Replace("#where#", where);
            var result = await _queryRepo.ExecuteQueryList<AttendanceViewModel>(query, null);
            return result;
        }

        public async Task<string> getUnderTimeHours(string id)
        {
            var query = $@"select udf.""Hours""
from public.""User"" as u 
join public.""NtsService"" as s on s.""OwnerUserId""=u.""Id"" and s.""Id""='{id}' and s.""IsDeleted""=false and u.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}' 
join public.""Template"" as tr on tr.""Id""=s.""TemplateId"" and tr.""Code"" in ('UndertimeLeave','UNDERTIME_REQUEST_AH') and tr.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_Leave_UndertimeLeave"" as udf on udf.""NtsNoteId""=s.""UdfNoteId"" and udf.""CompanyId""='{_repo.UserContext.CompanyId}' and udf.""IsDeleted""=false
where u.""CompanyId""='{_repo.UserContext.CompanyId}' and u.""IsDeleted""=false
";
            //var cypher = @"match(a:ADM_User{IsDeleted:0})<-[:R_Service_Owner_User]-(s:NTS_Service{IsDeleted:0, Id:{ServiceId}})
            //            -[:R_Service_Template]->(t:NTS_Template{IsDeleted:0,Status:'Active'})-[:R_TemplateRoot]->(tr:NTS_TemplateMaster{IsDeleted:0,Status:'Active'})
            //              where tr.Code in ['UNDERTIME_REQUEST','UNDERTIME_REQUEST_AH']
            //             match (s)<-[:R_ServiceFieldValue_Service]-(nfv:NTS_ServiceFieldValue)-[:R_ServiceFieldValue_TemplateField]->(tf{FieldName:'hours',IsDeleted:0})
            //            return nfv.Code";
            var hours = await _queryRepo.ExecuteScalar<string>(query, null);
            return hours;
        }

        public async Task<LeaveBalanceSheetViewModel> GetLeaveBalanceData(string userId, string leaveTypeCode, int year)
        {
            var Query = $@"SELECT lbs.*,u.""Id"" as UserId,lt.""Id"" as LeaveTypeId,lt.""Code"" as LeaveTypeCode 
FROM cms.""N_TAA_LeaveBalanceSheet"" as lbs
join public.""User"" as u on u.""Id""=lbs.""UserId"" and u.""Id""='{userId}' and u.""CompanyId""='{_repo.UserContext.CompanyId}'  and u.""IsDeleted""=false
join cms.""N_TAA_LeaveType"" as lt on lbs.""LeaveTypeId""=lt.""Id"" and lt.""Code""='{leaveTypeCode}' and lt.""CompanyId""='{_repo.UserContext.CompanyId}'  and lt.""IsDeleted""=false
where lbs.""Year""='{year}'  and lbs.""CompanyId""='{_repo.UserContext.CompanyId}'  and lbs.""IsDeleted""=false";

            //var cypher = @"MATCH (l:TAA_LeaveBalanceSheet{IsDeleted:0,Year:{Year}}) 
            //match(l)-[:R_LeaveBalanceSheet_User]->(u:ADM_User{IsDeleted:0,Id:{UserId}})
            //match(l)-[:R_LeaveBalanceSheet_LeaveType]->(lt:TAA_LeaveType{IsDeleted:0,Code:{LeaveTypeCode}})
            //return l,u.Id as UserId,lt.Id as LeaveTypeId,lt.Code as LeaveTypeCode";

            var model = await _queryRepo.ExecuteQuerySingle<LeaveBalanceSheetViewModel>(Query, null);// ExecuteCypher<LeaveBalanceSheetViewModel>(cypher, parameters);
            return model;
        }

        public async Task<IdNameViewModel> GetLeaveTypeByCode(string code)
        {
            var query = $@"select ""Id"",""Name"" from cms.""N_TAA_LeaveType"" where ""Code""='{code}' and ""IsDeleted""=false and ""CompanyId""='{_repo.UserContext.CompanyId}'";
            var result = await _queryRepo.ExecuteQuerySingle<IdNameViewModel>(query, null);
            return result;
        }

        public async Task<IList<LeaveViewModel>> GetAllAnnualLeaveTransactions(string userId)
        {
            var cypher = $@"select s.""Id"" as ServiceId,s.""ServiceNo"" as ServiceNo,s.""ServiceSubject"",s.""ServiceDescription"",u.""Id"" as UserId,tr.""Code"" as LeaveTypeCode,tr.""Name"" as LeaveType, null as LeaveStartDate
            ,null as LeaveEndDate,lov.""Code"" as LeaveStatus
            ,lovs.""Code"" as AddDeduct,coalesce(cast(la.""AdjustmentWorkingDays"" as DOUBLE PRECISION),0.0) as Adjustment
			from public.""User"" as u 
			join public.""NtsService"" as s on s.""OwnerUserId""=u.""Id"" and s.""CompanyId""='{_repo.UserContext.CompanyId}'  and s.""IsDeleted""=false
join public.""LOV"" as lov on lov.""Id""=s.""ServiceStatusId"" and lov.""CompanyId""='{_repo.UserContext.CompanyId}'  and lov.""IsDeleted""=false
			join public.""Template"" as tr on tr.""Id""=s.""TemplateId"" and tr.""CompanyId""='{_repo.UserContext.CompanyId}'  and tr.""IsDeleted""=false
			join public.""TemplateCategory"" as tc on tc.""Id""=tr.""TemplateCategoryId"" and tc.""Code""='Leave'  and tc.""CompanyId""='{_repo.UserContext.CompanyId}'  and tc.""IsDeleted""=false
			left join cms.""N_Leave_LeaveAdjustment"" as la on la.""Id""=s.""UdfNoteTableId""  and la.""CompanyId""='{_repo.UserContext.CompanyId}'  and la.""IsDeleted""=false
left join public.""LOV"" as lovs on lovs.""Id""=la.""AddOrDeductId"" and lovs.""CompanyId""='{_repo.UserContext.CompanyId}'  and lovs.""IsDeleted""=false
		    where tr.""Code"" ='LeaveAdjustment'  and u.""CompanyId""='{_repo.UserContext.CompanyId}'  and u.""IsDeleted""=false
and u.""Id""='{userId}'
union
--select s.""Id"" as ServiceId,s.""ServiceNo"" as ServiceNo,s.""ServiceSubject"",s.""ServiceDescription"", u.""Id"" as UserId,tr.""Code"" as LeaveTypeCode,tr.""Name"" as LeaveType, la.""LeaveStartDate"" as LeaveStartDate
--            ,la.""LeaveEndDate"" as LeaveEndDate,lov.""Code"" as LeaveStatus
--            ,'' as AddDeduct,0.0 as Adjustment
--
--            from public.""User"" as u
--            join public.""NtsService"" as s on s.""OwnerUserId""=u.""Id""
--join public.""LOV"" as lov on lov.""Id""=s.""ServiceStatusId""
--			join public.""Template"" as tr on tr.""Id""=s.""TemplateId""
--			join public.""TemplateCategory"" as tc on tc.""Id""=tr.""TemplateCategoryId"" and tc.""Code""='Leave'
--
--            left join cms.""N_Leave_AccuralLeave"" as la on la.""Id""=s.""UdfNoteTableId"" 
--left join public.""LOV"" as lovs on lovs.""Id""=la.""AddOrDeductId""
--		    where tr.""Code"" ='AccuralLeave'
--and u.""Id""='{userId}'
--union 
select s.""Id"" as ServiceId,s.""ServiceNo"" as ServiceNo,s.""ServiceSubject"",s.""ServiceDescription"", u.""Id"" as UserId,tr.""Code"" as LeaveTypeCode,tr.""Name"" as LeaveType, la.""LeaveStartDate"" as LeaveStartDate
            ,la.""LeaveEndDate"" as LeaveEndDate,lov.""Code"" as LeaveStatus
            ,'' as AddDeduct,0.0 as Adjustment

            from public.""User"" as u
            join public.""NtsService"" as s on s.""OwnerUserId""=u.""Id"" and s.""CompanyId""='{_repo.UserContext.CompanyId}'  and s.""IsDeleted""=false
join public.""LOV"" as lov on lov.""Id""=s.""ServiceStatusId"" and lov.""CompanyId""='{_repo.UserContext.CompanyId}'  and lov.""IsDeleted""=false
			join public.""Template"" as tr on tr.""Id""=s.""TemplateId"" and tr.""CompanyId""='{_repo.UserContext.CompanyId}'  and tr.""IsDeleted""=false
			join public.""TemplateCategory"" as tc on tc.""Id""=tr.""TemplateCategoryId"" and tc.""Code""='Leave' and tc.""CompanyId""='{_repo.UserContext.CompanyId}'  and tc.""IsDeleted""=false

            left join cms.""N_Leave_AnnualLeave"" as la on la.""Id""=s.""UdfNoteTableId""  and la.""CompanyId""='{_repo.UserContext.CompanyId}'  and la.""IsDeleted""=false

		    where tr.""Code"" ='AnnualLeave' and u.""CompanyId""='{_repo.UserContext.CompanyId}'  and u.""IsDeleted""=false
and u.""Id""='{userId}'
union 
select s.""Id"" as ServiceId,s.""ServiceNo"" as ServiceNo,s.""ServiceSubject"",s.""ServiceDescription"", u.""Id"" as UserId,tr.""Code"" as LeaveTypeCode,tr.""Name"" as LeaveType, null as LeaveStartDate
            ,null as LeaveEndDate,lov.""Code"" as LeaveStatus
            ,'' as AddDeduct,0.0 as Adjustment

            from public.""User"" as u
            join public.""NtsService"" as s on s.""OwnerUserId""=u.""Id"" and s.""CompanyId""='{_repo.UserContext.CompanyId}'  and s.""IsDeleted""=false
join public.""LOV"" as lov on lov.""Id""=s.""ServiceStatusId"" and lov.""CompanyId""='{_repo.UserContext.CompanyId}'  and lov.""IsDeleted""=false
			join public.""Template"" as tr on tr.""Id""=s.""TemplateId"" and tr.""CompanyId""='{_repo.UserContext.CompanyId}'  and tr.""IsDeleted""=false
			join public.""TemplateCategory"" as tc on tc.""Id""=tr.""TemplateCategoryId"" and tc.""Code""='Leave' and tc.""CompanyId""='{_repo.UserContext.CompanyId}'  and tc.""IsDeleted""=false

            left join cms.""N_Leave_AnnualLeaveEncashment"" as la on la.""Id""=s.""UdfNoteTableId""   and la.""CompanyId""='{_repo.UserContext.CompanyId}'  and la.""IsDeleted""=false
--left join public.""LOV"" as lovs on lovs.""Id""=la.""AddOrDeductId""
		    where tr.""Code"" ='AnnualLeaveEncashment'  and u.""CompanyId""='{_repo.UserContext.CompanyId}'  and u.""IsDeleted""=false
and u.""Id""='{userId}'
union 
select s.""Id"" as ServiceId,s.""ServiceNo"" as ServiceNo,s.""ServiceSubject"",s.""ServiceDescription"", u.""Id"" as UserId,tr.""Code"" as LeaveTypeCode,tr.""Name"" as LeaveType, la.""LeaveStartDate"" as LeaveStartDate
            ,la.""LeaveEndDate"" as LeaveEndDate,lov.""Code"" as LeaveStatus
            ,'' as AddDeduct,0.0 as Adjustment

            from public.""User"" as u
            join public.""NtsService"" as s on s.""OwnerUserId""=u.""Id"" and s.""CompanyId""='{_repo.UserContext.CompanyId}'  and s.""IsDeleted""=false
join public.""LOV"" as lov on lov.""Id""=s.""ServiceStatusId"" and lov.""CompanyId""='{_repo.UserContext.CompanyId}'  and lov.""IsDeleted""=false
			join public.""Template"" as tr on tr.""Id""=s.""TemplateId"" and tr.""CompanyId""='{_repo.UserContext.CompanyId}'  and tr.""IsDeleted""=false
			join public.""TemplateCategory"" as tc on tc.""Id""=tr.""TemplateCategoryId"" and tc.""Code""='Leave' and tc.""CompanyId""='{_repo.UserContext.CompanyId}'  and tc.""IsDeleted""=false

            left join cms.""N_Leave_UndertimeLeave"" as la on la.""Id""=s.""UdfNoteTableId""   and la.""CompanyId""='{_repo.UserContext.CompanyId}'  and la.""IsDeleted""=false
--left join public.""LOV"" as lovs on lovs.""Id""=la.""AddOrDeductId""
		    where tr.""Code"" ='UndertimeLeave'  and u.""CompanyId""='{_repo.UserContext.CompanyId}'  and u.""IsDeleted""=false
and u.""Id""='{userId}'
--union 
--select s.""Id"" as ServiceId,s.""ServiceNo"" as ServiceNo,s.""ServiceSubject"",s.""ServiceDescription"", u.""Id"" as UserId,tr.""Code"" as LeaveTypeCode,tr.""Name"" as LeaveType, la.""LeaveStartDate"" as LeaveStartDate
--            ,la.""LeaveEndDate"" as LeaveEndDate,lov.""Code"" as LeaveStatus
--            ,'' as AddDeduct,0.0 as Adjustment
--
--            from public.""User"" as u
--            join public.""NtsService"" as s on s.""OwnerUserId""=u.""Id""
--join public.""LOV"" as lov on lov.""Id""=s.""ServiceStatusId""
--			join public.""Template"" as tr on tr.""Id""=s.""TemplateId""
--			join public.""TemplateCategory"" as tc on tc.""Id""=tr.""TemplateCategoryId"" and tc.""Code""='Leave'
--
--            left join cms.""N_Leave_AnnualLeaveADV"" as la on la.""Id""=s.""UdfNoteTableId"" 
--left join public.""LOV"" as lovs on lovs.""Id""=la.""AddOrDeductId""
--		    where tr.""Code"" ='ANNUAL_LEAVE_ADV'
--and u.""Id""='{userId}'
--union 
--select s.""Id"" as ServiceId,s.""ServiceNo"" as ServiceNo,s.""ServiceSubject"",s.""ServiceDescription"", u.""Id"" as UserId,tr.""Code"" as LeaveTypeCode,tr.""Name"" as LeaveType, la.""LeaveStartDate"" as LeaveStartDate
--            ,la.""LeaveEndDate"" as LeaveEndDate,lov.""Code"" as LeaveStatus
--            ,'' as AddDeduct,0.0 as Adjustment
--
--            from public.""User"" as u
--            join public.""NtsService"" as s on s.""OwnerUserId""=u.""Id""
--join public.""LOV"" as lov on lov.""Id""=s.""ServiceStatusId""
--			join public.""Template"" as tr on tr.""Id""=s.""TemplateId""
--			join public.""TemplateCategory"" as tc on tc.""Id""=tr.""TemplateCategoryId"" and tc.""Code""='Leave'
--
--            left join cms.""N_Leave_AnnualLeaveHD"" as la on la.""Id""=s.""UdfNoteTableId"" 
--left join public.""LOV"" as lovs on lovs.""Id""=la.""AddOrDeductId""
--		    where tr.""Code"" ='ANNUAL_LEAVE_HD'
--and u.""Id""='{userId}'
";
            //var cypher = @"match (u:ADM_User{Id:{UserId},IsDeleted:0,Status:'Active'})
            //<-[:R_Service_Owner_User]-(s:NTS_Service{IsDeleted:0,Status:'Active'})
            //-[:R_Service_Template]->(t:NTS_Template{IsDeleted:0,Status:'Active'})
            //-[:R_TemplateRoot]->(tr:NTS_TemplateMaster{IsDeleted:0,Status:'Active'})
            //-[:R_TemplateMaster_TemplateCategory]->(tc:NTS_TemplateCategory{Code:'LEAVE_REQUEST',IsDeleted: 0,Status:'Active'})
            //optional match (s)<-[:R_ServiceFieldValue_Service]-(nfv:NTS_ServiceFieldValue)
            //-[:R_ServiceFieldValue_TemplateField]->(ttf:NTS_TemplateField)
            //with s,tr,u,apoc.map.fromPairs(COLLECT([ttf.FieldName,nfv.Code])) as udf
            //where tr.Code in ['LEAVE_ADJUSTMENT','LEAVE_ACCRUAL','ANNUAL_LEAVE','ANNUAL_LEAVE_HD','ANNUAL_LEAVE_ADV','UNDERTIME_REQUEST','ANNUAL_LEAVE_ENCASHMENT_KSA']
            //return s,u.Id as UserId,tr.Code as LeaveTypeCode,tr.Name as LeaveType, udf.startDate as LeaveStartDate
            //,udf.endDate as LeaveEndDate,s.TemplateAction as LeaveStatus
            //,udf.addDeduct as AddDeduct,udf.adjustment as Adjustment";
            //var parameters = new Dictionary<string, object> {

            //    { "UserId",userId },

            //    { "Status",StatusEnum.Active },

            //};

            var model = await _queryRepo.ExecuteQueryList<LeaveViewModel>(cypher, null); /*ExecuteCypherList<LeaveViewModel>(cypher, parameters).ToList();*/
            return model;
        }

        public async Task<CalendarViewModel> GetCalendarDetails(string userId)
        {
            var query = $@"SELECT pc.* FROM  cms.""N_PayrollHR_PayrollCalendar"" as pc 
join cms.""N_PayrollHR_SalaryInfo"" as psi on psi.""PayCalendarId"" = pc.""Id"" and psi.""IsDeleted""=false and psi.""CompanyId""='{_repo.UserContext.CompanyId}'  
join cms.""N_CoreHR_HRPerson"" as p on psi.""PersonId"" = p.""Id"" and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'  
join public.""User"" as u on u.""Id""=p.""UserId"" and u.""Id""='{userId}' and u.""CompanyId""='{_repo.UserContext.CompanyId}'  and u.""IsDeleted""=false
where  pc.""CompanyId""='{_repo.UserContext.CompanyId}'  and pc.""IsDeleted""=false
";

            var payCalendar = await _queryRepo.ExecuteQuerySingle<CalendarViewModel>(query, null);
            return payCalendar;
        }

        public async Task<List<CalendarHolidayViewModel>> GetHolidayDetails(CalendarViewModel payCalendar)
        {
            var query = $@"select h.* from cms.""N_PayrollHR_PayrollCalendar"" as pc 
join cms.""N_PayrollHR_CalendarHoliday"" as h on h.""CalendarId""=pc.""Id"" and pc.""Id""='{ payCalendar.Id}' and h.""CompanyId""='{_repo.UserContext.CompanyId}'  and h.""IsDeleted""=false
where  pc.""CompanyId""='{_repo.UserContext.CompanyId}' and pc.""IsDeleted""=false";
            //cypher = @"match(c:PAY_Calendar{Id:{Id}})<-[:R_CalendarHoliday_Calendar]-(h:PAY_CalendarHoliday{Status:'Active'}) return h";
            //prms = new Dictionary<string, object> { { "Id", payCalendar.Id } };
            //var payCalendarHolidays = ExecuteCypherList<PAY_CalendarHoliday>(cypher, prms);
            var payCalendarHolidays = await _queryRepo.ExecuteQueryList<CalendarHolidayViewModel>(query, null);
            return payCalendarHolidays;
        }

        public async Task<List<LeaveDetailViewModel>> LeaveDetails(List<TemplateViewModel> templateList, string userId)
        {
            var selectQry = "";
            var i = 1;
            foreach (var item in templateList.Where(x => x.UdfTableMetadataId != null))
            {
                var tableMeta = await _tableMetadataBusiness.GetSingle(x => x.Id == item.UdfTableMetadataId);

                if (item.Code == "LeaveAdjustment" || item.Code == "LeaveAccrual" || item.Code == "LEAVE_CANCEL" || item.Code == "RETURN_TO_WORK" || item.Code == "AnnualLeaveEncashment")
                {

                }
                else
                {
                    if (i != 1)
                    {
                        selectQry += " union ";
                    }
                    if (item.Code == "UndertimeLeave")
                    {
                        selectQry = @$" {selectQry} Select s.""ServiceNo"", u.""Id"" as UserId, udf.""LeaveStartDate""::TIMESTAMP::DATE as StartDate,  udf.""LeaveEndDate""::TIMESTAMP::DATE  as EndDate, 
                            t.""DisplayName"" as LeaveType, null as CalendarDuration, null as WorkingDuration,
                            t.""Code"" as LeaveTypeCode, lov.""Name"" as ServiceStatus, lov.""Code"" as ServiceStatusCode
                            From cms.""{tableMeta.Name}"" as udf 
                            Join public.""NtsNote"" as n on udf.""NtsNoteId""=n.""Id"" and n.""IsDeleted""=false  and n.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Left Join public.""NtsService"" as s on n.""Id""=s.""UdfNoteId"" and s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Left Join public.""Template"" as t on s.""TemplateId""=t.""Id"" and t.""CompanyId""='{_repo.UserContext.CompanyId}' and t.""IsDeleted""=false
                            Left Join public.""Template"" as UdfTemplate on s.""UdfTemplateId""=UdfTemplate.""Id"" and UdfTemplate.""IsDeleted""=false and UdfTemplate.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Left join public.""ServiceTemplate"" as st on st.""TemplateId""=t.""Id"" and st.""IsDeleted""=false  and st.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Left Join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""Id""='{userId}'  and u.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Left Join public.""LOV"" as lov on s.""ServiceStatusId""=lov.""Id"" and lov.""Code"" not in ('SERVICE_STATUS_DRAFT','SERVICE_STATUS_CANCEL') and lov.""CompanyId""='{_repo.UserContext.CompanyId}'
where udf.""IsDeleted""=false and udf.""CompanyId""='{_repo.UserContext.CompanyId}'";

                    }
                    else if (item.Code == "LeaveHandoverService")
                    {
                        selectQry = @$" {selectQry} Select s.""ServiceNo"", u.""Id"" as UserId, null as StartDate,  null  as EndDate, 
                            t.""DisplayName"" as LeaveType, null as CalendarDuration, null as WorkingDuration,
                            t.""Code"" as LeaveTypeCode, lov.""Name"" as ServiceStatus, lov.""Code"" as ServiceStatusCode
                            From cms.""{tableMeta.Name}"" as udf
                            Join public.""NtsNote"" as n on udf.""NtsNoteId""=n.""Id"" and n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Left Join public.""NtsService"" as s on n.""Id""=s.""UdfNoteId"" and s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Left Join public.""Template"" as t on s.""TemplateId""=t.""Id"" and t.""CompanyId""='{_repo.UserContext.CompanyId}' and t.""IsDeleted""=false
                            Left Join public.""Template"" as UdfTemplate on s.""UdfTemplateId""=UdfTemplate.""Id"" and UdfTemplate.""IsDeleted""=false and UdfTemplate.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Left join public.""ServiceTemplate"" as st on st.""TemplateId""=t.""Id"" and st.""IsDeleted""=false and st.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Left Join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""Id""='{userId}' and u.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Left Join public.""LOV"" as lov on s.""ServiceStatusId""=lov.""Id"" and lov.""Code"" not in ('SERVICE_STATUS_DRAFT','SERVICE_STATUS_CANCEL') and lov.""CompanyId""='{_repo.UserContext.CompanyId}'
where udf.""IsDeleted""=false and udf.""CompanyId""='{_repo.UserContext.CompanyId}'";

                    }
                    else
                    {
                        selectQry = @$" {selectQry} Select s.""ServiceNo"", u.""Id"" as UserId, udf.""LeaveStartDate""::TIMESTAMP::DATE as StartDate,  udf.""LeaveEndDate""::TIMESTAMP::DATE  as EndDate, 
                            t.""DisplayName"" as LeaveType, udf.""LeaveDurationCalendarDays"" as CalendarDuration, udf.""LeaveDurationWorkingDays"" as WorkingDuration,
                            t.""Code"" as LeaveTypeCode, lov.""Name"" as ServiceStatus, lov.""Code"" as ServiceStatusCode
                            From cms.""{tableMeta.Name}"" as udf
                            Join public.""NtsNote"" as n on udf.""NtsNoteId""=n.""Id"" and n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Left Join public.""NtsService"" as s on n.""Id""=s.""UdfNoteId"" and s.""CompanyId""='{_repo.UserContext.CompanyId}' and s.""IsDeleted""=false
                            Left Join public.""Template"" as t on s.""TemplateId""=t.""Id"" and t.""CompanyId""='{_repo.UserContext.CompanyId}' and t.""IsDeleted""=false
                            Left Join public.""Template"" as UdfTemplate on s.""UdfTemplateId""=UdfTemplate.""Id"" and UdfTemplate.""IsDeleted""=false and UdfTemplate.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Left join public.""ServiceTemplate"" as st on st.""TemplateId""=t.""Id"" and st.""IsDeleted""=false  and st.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Left Join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""Id""='{userId}' and u.""CompanyId""='{_repo.UserContext.CompanyId}' and u.""IsDeleted""=false 
                            Left Join public.""LOV"" as lov on s.""ServiceStatusId""=lov.""Id"" and lov.""Code"" not in ('SERVICE_STATUS_DRAFT','SERVICE_STATUS_CANCEL') and lov.""CompanyId""='{_repo.UserContext.CompanyId}'
where udf.""IsDeleted""=false and udf.""CompanyId""='{_repo.UserContext.CompanyId}'";

                    }
                    i++;
                }

            }

            var querydata = await _queryRepo.ExecuteQueryList<LeaveDetailViewModel>(selectQry, null);
            return querydata;

        }

        public async Task<List<LeaveDetailViewModel>> GetAllLeaveEncashmentDuration(DateTime startDate, DateTime endDate)
        {
            //throw new NotImplementedException();

            //var prms = new Dictionary<string, object> { { "SD", startDate }, { "ED", endDate } };
            //var cypher = string.Concat(@"match (u:ADM_User{ IsDeleted:0,Status:'Active'})
            //-[:R_User_PersonRoot]-(pr:HRS_PersonRoot{ IsDeleted: 0,Status:'Active'})
            //match (u)<-[:R_Service_Owner_User]-(s:NTS_Service{IsDeleted:0})
            //match (s)-[:R_Service_Template]->(t:NTS_Template{IsDeleted:0,Status:'Active'})
            //match (t)-[:R_TemplateRoot]->(tr:NTS_TemplateMaster{IsDeleted:0,Status:'Active'})
            //match (tr)-[:R_TemplateMaster_TemplateCategory]
            //->(tc:NTS_TemplateCategory{Code:'LEAVE_REQUEST',IsDeleted: 0,Status:'Active'})
            //match(s)-[:R_Service_Status_ListOfValue]->(lv:GEN_ListOfValue{IsDeleted:0})           
            //where not s.TemplateAction in ['Draft','Cancel']
            //and tr.Code in ['ANNUAL_LEAVE_ENCASHMENT_KSA']
            //match (s)<-[:R_ServiceFieldValue_Service]-(nfv:NTS_ServiceFieldValue)
            //-[:R_ServiceFieldValue_TemplateField]->(ttf:NTS_TemplateField)
            //with s,pr,u,apoc.map.fromPairs(COLLECT([ttf.FieldName,nfv.Code])) as udf
            //where {SD}<=udf.effectiveDate<={ED}
            //return s.Id as ServiceId,pr.Id as PersonId,u.Id as UserId
            //,sum(coalesce(toFloat(udf.adjustment),0.0)) as Adjustment");
            //return ExecuteCypherList<LeaveDetailViewModel>(cypher, prms).ToList();

            var selectQry = @$" Select s.""Id"" as ServiceId, p.""Id"" as PersonId,u.""Id"" as UserId,sum(coalesce(0.0,0.0)) as Adjustment
                            From cms.""N_Leave_AnnualLeaveEncashment"" as udf
                            Join public.""NtsNote"" as n on udf.""NtsNoteId""=n.""Id"" and n.""IsDeleted""=false and  n.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Left Join public.""NtsService"" as s on n.""Id""=s.""UdfNoteId"" and s.""IsDeleted""=false  and  s.""CompanyId""='{_repo.UserContext.CompanyId}' 
                            Left Join public.""Template"" as t on s.""TemplateId""=t.""Id"" and  t.""CompanyId""='{_repo.UserContext.CompanyId}' and t.""IsDeleted""=false 
                            Left Join public.""Template"" as UdfTemplate on s.""UdfTemplateId""=UdfTemplate.""Id"" and UdfTemplate.""IsDeleted""=false and  UdfTemplate.""CompanyId""='{_repo.UserContext.CompanyId}' 
                            Left join public.""ServiceTemplate"" as st on st.""TemplateId""=t.""Id"" and st.""IsDeleted""=false  and  st.""CompanyId""='{_repo.UserContext.CompanyId}' 
                            Left Join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and  u.""CompanyId""='{_repo.UserContext.CompanyId}' 
                            Left Join cms.""N_CoreHR_HRPerson"" as p on p.""UserId""=u.""Id"" and p.""IsDeleted""=false and  p.""CompanyId""='{_repo.UserContext.CompanyId}' 
                            Left Join public.""LOV"" as lov on s.""ServiceStatusId""=lov.""Id"" and lov.""Code"" not in ('SERVICE_STATUS_DRAFT','SERVICE_STATUS_CANCEL')   and  lov.""CompanyId""='{_repo.UserContext.CompanyId}' 
                            where udf.""IsDeleted""=false and  udf.""CompanyId""='{_repo.UserContext.CompanyId}' 
                            group by s.""Id"",p.""Id"",u.""Id""
                            ";

            var querydata = await _queryRepo.ExecuteQueryList<LeaveDetailViewModel>(selectQry, null);
            return querydata;
        }

        public async Task<List<LeaveDetailViewModel>> GetAllLeavesWithDuration(List<TemplateViewModel> templateList)
        {

            var selectQry = "";
            var i = 1;
            foreach (var item in templateList.Where(x => x.UdfTableMetadataId != null))
            {
                var tableMeta = await _tableMetadataBusiness.GetSingle(x => x.Id == item.UdfTableMetadataId);
                if (item.Code == "LeaveAdjustment" || item.Code == "LeaveAccrual" || item.Code == "LEAVE_CANCEL" || item.Code == "RETURN_TO_WORK"
                    || item.Code == "AnnualLeaveEncashment" || item.Code == "LeaveHandoverService" || item.Code == "UndertimeLeave")
                {
                }
                else
                {
                    if (i != 1)
                    {
                        selectQry += " union ";
                    }
                    selectQry = @$" {selectQry} Select p.""Id"" as PersonId,u.""Id"" as UserId, udf.""LeaveStartDate""::TIMESTAMP::DATE as StartDate, udf.""LeaveEndDate""::TIMESTAMP::DATE as EndDate
                                , t.""DisplayName"" as LeaveType, udf.""LeaveDurationCalendarDays"" as CalendarDuration, udf.""LeaveDurationCalendarDays"" as Duration, udf.""LeaveDurationWorkingDays"" as WorkingDuration,
                            t.""Code"" as LeaveTypeCode, lov.""Name"" as ServiceStatus
                            From cms.""{tableMeta.Name}"" as udf
                            Join public.""NtsNote"" as n on udf.""NtsNoteId""=n.""Id"" and n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Left Join public.""NtsService"" as s on n.""Id""=s.""UdfNoteId"" and s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Left Join public.""Template"" as t on s.""TemplateId""=t.""Id"" and t.""CompanyId""='{_repo.UserContext.CompanyId}' and t.""IsDeleted""=false
                            Left Join public.""Template"" as UdfTemplate on s.""UdfTemplateId""=UdfTemplate.""Id"" and UdfTemplate.""IsDeleted""=false and UdfTemplate.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Left join public.""ServiceTemplate"" as st on st.""TemplateId""=t.""Id"" and st.""IsDeleted""=false and st.""CompanyId""='{_repo.UserContext.CompanyId}' 
                            Left Join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Left Join cms.""N_CoreHR_HRPerson"" as p on p.""UserId""=u.""Id"" and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Left Join public.""LOV"" as lov on s.""ServiceStatusId""=lov.""Id"" and lov.""Code"" not in ('SERVICE_STATUS_DRAFT','SERVICE_STATUS_CANCEL') and lov.""CompanyId""='{_repo.UserContext.CompanyId}'  and lov.""IsDeleted""=false
where  udf.""CompanyId""='{_repo.UserContext.CompanyId}'  and udf.""IsDeleted""=false";
                    i++;
                }
            }
            var querydata = await _queryRepo.ExecuteQueryList<LeaveDetailViewModel>(selectQry, null);
            return querydata;
        }

        public async Task<List<LeaveDetailViewModel>> GetAllSickLeaveDuration(string userId)
        {
            var query = $@"select 
u.""Id"" as UserId ,nts.""TemplateCode"" as LeaveType
  ,al.""LeaveStartDate"" as StartDate,al.""LeaveEndDate"" as EndDate,al.""LeaveDurationWorkingDays"" as Duration
    from public.""User"" as u
join public.""NtsService"" as nts on nts.""OwnerUserId""=u.""Id"" and nts.""IsDeleted""=false and nts.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_Leave_SickLeave"" as al on al.""Id""=nts.""UdfNoteTableId"" and al.""IsDeleted""=false and al.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""LOV"" as lov on lov.""Id""=nts.""ServiceStatusId"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_repo.UserContext.CompanyId}'	  
where lov.""Code"" not in ('SERVICE_STATUS_DRAFT' , 'SERVICE_STATUS_CANCEL') and u.""IsDeleted""=false and u.""Id""='{userId}' and u.""CompanyId""='{_repo.UserContext.CompanyId}'";

            var list = await _queryRepo.ExecuteQueryList<LeaveDetailViewModel>(query, null);
            return list;
        }

        public async Task<List<LeaveDetailViewModel>> GetAllUnpaidLeaveDuration(string userId, List<TemplateViewModel> templateList)
        {
            var selectQry = "";
            var i = 1;
            foreach (var item in templateList.Where(x => x.UdfTableMetadataId != null))
            {
                var tableMeta = await _tableMetadataBusiness.GetSingle(x => x.Id == item.UdfTableMetadataId);

                if (item.Code == "UNPAID_L" || item.Code == "UNA_ABSENT")
                {
                    if (i != 1)
                    {
                        selectQry += " union ";
                    }
                    selectQry = @$" {selectQry} Select u.""Id"" as UserId, udf.""LeaveStartDate""::TIMESTAMP::DATE as StartDate, udf.""LeaveEndDate""::TIMESTAMP::DATE as EndDate, t.""DisplayName"" as LeaveType, udf.""LeaveDurationCalendarDays"" as CalendarDuration, udf.""LeaveDurationWorkingDays"" as WorkingDuration,
                            t.""Code"" as LeaveTypeCode, lov.""Name"" as ServiceStatus
                            From cms.""{tableMeta.Name}"" as udf
                            Join public.""NtsNote"" as n on udf.""NtsNoteId""=n.""Id"" and n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Left Join public.""NtsService"" as s on n.""Id""=s.""UdfNoteId"" and s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Left Join public.""Template"" as t on s.""TemplateId""=t.""Id"" and t.""IsDeleted""=false and t.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Left Join public.""Template"" as UdfTemplate on s.""UdfTemplateId""=UdfTemplate.""Id"" and UdfTemplate.""IsDeleted""=false and UdfTemplate.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Left join public.""ServiceTemplate"" as st on st.""TemplateId""=t.""Id"" and st.""IsDeleted""=false and st.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Left Join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""Id""='{userId}' and u.""CompanyId""='{_repo.UserContext.CompanyId}' and u.""IsDeleted""=false
                            Left Join public.""LOV"" as lov on s.""ServiceStatusId""=lov.""Id"" and lov.""Code"" not in ('SERVICE_STATUS_DRAFT','SERVICE_STATUS_CANCEL') and udf.""CompanyId""='{_repo.UserContext.CompanyId}' and udf.""IsDeleted""=false ";
                }
                i++;
            }

            var querydata = await _queryRepo.ExecuteQueryList<LeaveDetailViewModel>(selectQry, null);
            return querydata;

        }

        public async Task<List<LeaveDetailViewModel>> GetAllUnpaidLeaveDurationIncludingPlannedUnpaidLeave(string userId)
        {
            // throw new NotImplementedException();
            var query = $@"select u.""Id"" as UserId,l.""LeaveStartDate"" as StartDate,
l.""LeaveEndDate"" as EndDate,'UNPAID_L' as LeaveType,l.""LeaveDurationWorkingDays""::int as Duration
from public.""User"" as u
join public.""NtsService"" as nts on nts.""OwnerUserId""=u.""Id"" and nts.""IsDeleted""=false and nts.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""LOV"" as lv on nts.""ServiceStatusId""=lv.""Id"" and lv.""IsDeleted""=false and lv.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_Leave_UnpaidLeave"" as l on l.""Id""=nts.""UdfNoteTableId"" and l.""IsDeleted""=false and l.""CompanyId""='{_repo.UserContext.CompanyId}'
where u.""Id""='{userId}' and u.""IsDeleted""=false and lv.""Code"" not in ('SERVICE_STATUS_DRAFT','SERVICE_STATUS_CANCEL') and u.""CompanyId""='{_repo.UserContext.CompanyId}'

union

select u.""Id"" as UserId,l.""LeaveStartDate"" as StartDate,
l.""LeaveEndDate"" as EndDate,'PLANNED_UNPAID_L' as LeaveType,l.""LeaveDurationWorkingDays""::int as Duration
from public.""User"" as u
join public.""NtsService"" as nts on nts.""OwnerUserId""=u.""Id"" and nts.""IsDeleted""=false and nts.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""LOV"" as lv on nts.""ServiceStatusId""=lv.""Id"" and lv.""IsDeleted""=false and lv.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_Leave_PlannedUnpaidLeave"" as l on l.""Id""=nts.""UdfNoteTableId"" and l.""IsDeleted""=false and l.""CompanyId""='{_repo.UserContext.CompanyId}'
where u.""Id""='{userId}' and u.""IsDeleted""=false and lv.""Code"" not in ('SERVICE_STATUS_DRAFT','SERVICE_STATUS_CANCEL') and u.""CompanyId""='{_repo.UserContext.CompanyId}'

union

select u.""Id"" as UserId,l.""LeaveStartDate"" as StartDate,
l.""LeaveEndDate"" as EndDate,'UNA_ABSENT' as LeaveType,0 as Duration
from public.""User"" as u
join public.""NtsService"" as nts on nts.""OwnerUserId""=u.""Id"" and nts.""IsDeleted""=false  and nts.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""LOV"" as lv on nts.""ServiceStatusId""=lv.""Id"" and lv.""IsDeleted""=false  and lv.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_Leave_UNAAbsent"" as l on l.""Id""=nts.""UdfNoteTableId"" and l.""IsDeleted""=false and l.""CompanyId""='{_repo.UserContext.CompanyId}'
where u.""Id""='{userId}' and u.""IsDeleted""=false and lv.""Code"" not in ('SERVICE_STATUS_DRAFT','SERVICE_STATUS_CANCEL') and u.""CompanyId""='{_repo.UserContext.CompanyId}'";

            var list = await _queryRepo.ExecuteQueryList<LeaveDetailViewModel>(query, null);
            return list;
        }

        public async Task<List<LeaveViewModel>> GetAnnualLeaveDatedDurationForAccrual(string userId, List<TemplateViewModel> templateList)
        {
            var selectQry = "";
            var i = 1;
            foreach (var item in templateList.Where(x => x.UdfTableMetadataId != null))
            {
                var tableMeta = await _tableMetadataBusiness.GetSingle(x => x.Id == item.UdfTableMetadataId);
                if (i != 1)
                {
                    selectQry += " union ";
                }

                selectQry = @$" {selectQry} Select s.ServiceNo,u.Id as UserId,t.Code as LeaveTypeCode,t.Name as LeaveType
            ,case when t.Code='ANNUAL_LEAVE_ENCASHMENT_KSA' then udf.EffectiveDate else s.StartDate end as LeaveStartDate
            ,case when tr.Code='ANNUAL_LEAVE_ENCASHMENT_KSA' then udf.EffectiveDate else   s.DueDate end as LeaveEndDate
            ,udf.*,t.""Code"" as LeaveTypeCode, lov.""Name"" as ServiceStatus
                            From cms.""{tableMeta.Name}"" as udf
                            Join public.""NtsNote"" as n on udf.""NtsNoteId""=n.""Id"" and n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Left Join public.""NtsService"" as s on n.""Id""=s.""UdfNoteId"" and s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Left Join public.""Template"" as t on s.""TemplateId""=t.""Id"" and t.""CompanyId""='{_repo.UserContext.CompanyId}' and t.""IsDeleted""=false
                            Left Join public.""Template"" as UdfTemplate on s.""UdfTemplateId""=UdfTemplate.""Id"" and UdfTemplate.""IsDeleted""=false and UdfTemplate.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Left join public.""ServiceTemplate"" as st on st.""TemplateId""=t.""Id"" and st.""IsDeleted""=false  and st.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Left Join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""CompanyId""='{_repo.UserContext.CompanyId}' and u.""IsDeleted""=false
                            Left Join public.""LOV"" as lov on s.""ServiceStatusId""=lov.""Id"" and lov.""Code"" not in ('SERVICE_STATUS_DRAFT','SERVICE_STATUS_CANCEL') and lov.""CompanyId""='{_repo.UserContext.CompanyId}' and lov.""IsDeleted""=false
                             where udf.""IsDeleted""=false and u.""Id""='{userId}' and udf.""CompanyId""='{_repo.UserContext.CompanyId}'";
                i++;

            }

            var list = await _queryRepo.ExecuteQueryList<LeaveViewModel>(selectQry, null);
            return list;
        }

        public async Task<double> GetEntitlement(string leaveTypeCode, string userId)
        {
            var query = $@"select cr.""AnnualLeaveEntitlement""
            from cms.""N_CoreHR_HRPerson"" as p 
            join public.""User"" as u on u.""Id""=p.""UserId"" and u.""Id""='{userId}' and  u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
            join cms.""N_CoreHR_HRContract"" as cr on cr.""EmployeeId""=p.""Id"" and  cr.""IsDeleted""=false and cr.""CompanyId""='{_repo.UserContext.CompanyId}'
            where  p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
";

            //var cypher = @"MATCH (u:ADM_User{IsDeleted:0,Id:{UserId}}) 
            //            match(u)-[:R_User_PersonRoot]->(pr:HRS_PersonRoot{IsDeleted:0})
            //            match(pr)<-[:R_ContractRoot_PersonRoot]-(cr:HRS_ContractRoot{IsDeleted:0})
            //            match(cr)<-[:R_ContractRoot]-(c:HRS_Contract{IsDeleted:0,IsLatest:true})
            //            return c.AnnualLeaveEntitlement as AnnualLeaveEntitlement";
            //var parameters = new Dictionary<string, object> {
            //    { "UserId",userId }
            //};
            var value = await _queryRepo.ExecuteScalar<double>(query, null);
            return value;
            // throw new NotImplementedException();
        }

        public async Task<ContractViewModel> GetContractDetails(string userId)
        {
            var cypher = string.Concat(
            $@"select c.*,a.""DateOfJoin"" as DateOfJoin
                from public.""User"" as u
                join cms.""N_CoreHR_HRPerson"" as p on p.""UserId""=u.""Id"" and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
                join cms.""N_CoreHR_HRContract"" as c on c.""EmployeeId""=p.""Id"" and c.""IsDeleted""=false and c.""CompanyId""='{_repo.UserContext.CompanyId}'
                join cms.""N_CoreHR_HRAssignment"" as a on a.""EmployeeId""=p.""Id"" and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'
                where u.""IsDeleted""=false and u.""Id""='{userId}' and u.""CompanyId""='{_repo.UserContext.CompanyId}' ");

            var cm = await _queryRepo.ExecuteQuerySingle<ContractViewModel>(cypher, null);
            return cm;
        }

        public async Task<double> GetLeaveBalance(string userId, int year, string leaveTypeCode)
        {

            var query = $@"Select l.""ClosingBalance"" from cms.""N_TAA_LeaveBalanceSheet"" as l
                        Join cms.""N_TAA_LeaveType"" as lt on l.""LeaveTypeId""=lt.""Id"" and lt.""Code""='{leaveTypeCode}' and lt.""CompanyId""='{_repo.UserContext.CompanyId}'  and lt.""IsDeleted""=false 
                        Where l.""Year""='{year}' and l.""UserId""='{userId}' and l.""IsDeleted""=false and l.""CompanyId""='{_repo.UserContext.CompanyId}'";

            var val = await _queryRepo.ExecuteScalar<double>(query, null);
            return val;
        }
        public async Task<bool> IsOnPaidLeave(string userId, DateTime date)
        {
            //throw new NotImplementedException();
            //var prms = Helper.GenerateCypherParameterWithMandatoryValues(CompanyId, 0, StatusEnum.Active, "UserId", userId);
            //var cypher = @"match (u:ADM_User{Id:{UserId},IsDeleted:0,Status:'Active'})
            //<-[:R_Service_Owner_User]-(s:NTS_Service{IsDeleted:0,Status:'Active'})
            //-[:R_Service_Template]->(t:NTS_Template{IsDeleted:0,Status:'Active'})
            //-[:R_TemplateRoot]->(tr:NTS_TemplateMaster{IsDeleted:0,Status:'Active'})
            //-[:R_TemplateMaster_TemplateCategory]->(tc:NTS_TemplateCategory{Code:'LEAVE_REQUEST',IsDeleted: 0,Status:'Active'})
            //match(s)-[:R_Service_Status_ListOfValue]->(lv:GEN_ListOfValue{IsDeleted:0})    
            //with s,tr,lv 
            //optional match (s)<-[:R_ServiceFieldValue_Service]-(nfv:NTS_ServiceFieldValue{IsDeleted:0})-[:R_ServiceFieldValue_TemplateField]->(tf{FieldName:'startDate',IsDeleted:0})
            //with s,tr,lv,tf,nfv
            //optional match(s)<-[:R_ServiceFieldValue_Service] - (nfv1: NTS_ServiceFieldValue{IsDeleted:0}) -[:R_ServiceFieldValue_TemplateField] -> (tf1{FieldName:'endDate',IsDeleted:0})
            //with s,tr,lv,tf,nfv,tf1,nfv1
            //where not tr.Code  in ['PLANNED_UNPAID_L','UNPAID_L','LEAVE_ADJUSTMENT','LEAVE_ACCRUAL','LEAVE_CANCEL','RETURN_TO_WORK','ANNUAL_LEAVE_ENCASHMENT_KSA'] 
            //return s.ServiceNo as ServiceNo,s.Id as ServiceId,  nfv.Code as StartDate,nfv1.Code as EndDate,s.TemplateAction as LeaveStatusAction";
            //var list = ExecuteCypherList<LeaveDetailViewModel>(cypher, prms);
            //return list.Any(x => (x.LeaveStatusAction == NtsActionEnum.Complete || x.LeaveStatusAction == NtsActionEnum.Close)
            // && x.StartDate <= date && x.EndDate >= date);

            var tempCategory = await _repo.GetSingle<TemplateCategoryViewModel, TemplateCategory>(x => x.Code == "Leave" && x.TemplateType == TemplateTypeEnum.Service);
            var templateList = await _repo.GetList<TemplateViewModel, Template>(x => x.TemplateCategoryId == tempCategory.Id);
            var querydata = await this.LeaveDetails(templateList, userId);
            var result = querydata.Any(x => (x.ServiceStatusCode == "SERVICE_STATUS_COMPLETE" || x.ServiceStatusCode == "SERVICE_STATUS_CLOSE") && x.StartDate <= date && x.EndDate >= date);
            return result;

        }
        public async Task<AssignmentViewModel> GetAssignmentDetails(string userId)
        {
            var query = $@"select a.*,a.""DateOfJoin""::date as DTDateOfJoin from public.""User"" as u
                            join cms.""N_CoreHR_HRPerson"" as p on p.""UserId""=u.""Id"" and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
                            join cms.""N_CoreHR_HRAssignment"" as a on p.""Id""=a.""EmployeeId"" and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'
                            where u.""IsDeleted""=false and u.""Id""='{userId}' and u.""CompanyId""='{_repo.UserContext.CompanyId}'";


            var assignment = await _queryRepo.ExecuteQuerySingle<AssignmentViewModel>(query, null);
            return assignment;
        }

        public async Task<AssignmentViewModel> GetCurrentAnniversaryStartDateByUserId(string userId)
        {
            var cypher = $@"select a.*,a.""DateOfJoin""::date as DTDateOfJoin from public.""User"" as u
join cms.""N_CoreHR_HRPerson"" as p on p.""UserId""=u.""Id"" and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRAssignment"" as a on p.""Id""=a.""EmployeeId"" and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'
where u.""IsDeleted""=false and u.""Id""='{userId}' and u.""CompanyId""='{_repo.UserContext.CompanyId}'
           ";
            var assignment = await _queryRepo.ExecuteQuerySingle<AssignmentViewModel>(cypher, null);
            return assignment;

        }

        public async Task<ContractViewModel> GetContractByUser(string userId)
        {

            var query = $@"Select c.""EffectiveEndDate""::date From cms.""N_CoreHR_HRContract"" as c
                            Join cms.""N_CoreHR_HRPerson"" as p on p.""Id""=c.""EmployeeId""  and p.""CompanyId""='{_repo.UserContext.CompanyId}' and p.""IsDeleted""=false
                            Join public.""User"" as u on u.""Id""=p.""UserId"" and u.""Id""='{userId}'  and u.""CompanyId""='{_repo.UserContext.CompanyId}'  and u.""IsDeleted""=false
where c.""IsDeleted""=false and c.""CompanyId""='{_repo.UserContext.CompanyId}'";

            var result = await _queryRepo.ExecuteQuerySingle<ContractViewModel>(query, null);

            return result;
        }

        public async Task<ContractViewModel> GetTicketDetails(string userId)
        {
            var cypher = $@"select c.*,a.""DateOfJoin"" as DateOfJoin,s.""FlightTicketFrequency""::int as AirTicketInterval
from public.""User"" as u
join cms.""N_CoreHR_HRPerson"" as p on p.""UserId""=u.""Id"" and p.""IsDeleted""=false  and p.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRContract"" as c on p.""Id""=c.""EmployeeId"" and c.""IsDeleted""=false and c.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRAssignment"" as a on p.""Id""=a.""EmployeeId"" and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_PayrollHR_SalaryInfo"" as s on p.""Id""=s.""PersonId"" and s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}'
where u.""IsDeleted""=false and u.""Id""='{userId}' and u.""CompanyId""='{_repo.UserContext.CompanyId}'";

            var cm = await _queryRepo.ExecuteQuerySingle<ContractViewModel>(cypher, null);
            return cm;
        }

        public async Task<long> GetSickLeaveDetails(string userId)
        {
            var cypher = $@"select sum(coalesce(sal.""UnpaidLeavesNotInSystem""::int, 0)) as UnpaidLeavesNotInSystem
from public.""User"" as u
join cms.""N_CoreHR_HRPerson"" as p on p.""UserId""=u.""Id"" and p.""IsDeleted""=false and  p.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_PayrollHR_SalaryInfo"" as sal on sal.""PersonId""=p.""Id"" and sal.""IsDeleted""=false and  sal.""CompanyId""='{_repo.UserContext.CompanyId}'
where u.""IsDeleted""=false and u.""Id""='{userId}'  and u.""CompanyId""='{_repo.UserContext.CompanyId}'";

            var notInSystem = await _queryRepo.ExecuteScalar<long>(cypher, null);
            return notInSystem;
        }

        public async Task<double> GetUnpaidLeaveDetails(string userId)
        {
            var query = $@"Select sum(Cast(sal.""UnpaidLeavesNotInSystem"" as float)) as UnpaidLeavesNotInSystem
                            From cms.""N_PayrollHR_SalaryInfo"" as sal
                            Join cms.""N_CoreHR_HRPerson"" as p on p.""Id""=sal.""PersonId"" and p.""IsDeleted""=false  and  p.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Join public.""User"" as u on u.""Id""=p.""UserId"" and u.""IsDeleted""=false and u.""Id""='{userId}'  and  u.""CompanyId""='{_repo.UserContext.CompanyId}'
                            where sal.""IsDeleted""=false and  sal.""CompanyId""='{_repo.UserContext.CompanyId}' ";

            var notInSystem = await _queryRepo.ExecuteScalar<double>(query, null);
            return notInSystem;
        }

        public async Task<ServiceViewModel> LeaveAccrualServiceExists(string userId, DateTime startDate, DateTime endDate, IServiceBusiness serviceBusiness = null)
        {
            var cypher = $@"select nts.*
from public.""User"" as u
join public.""NtsService"" as nts on nts.""OwnerUserId""=u.""Id"" and nts.""IsDeleted""=false and nts.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_Leave_LeaveAccrual"" as l on l.""Id""=nts.""UdfNoteTableId"" and l.""IsDeleted""=false and l.""CompanyId""='{_repo.UserContext.CompanyId}'
where u.""Id""='{userId}'  and nts.""StartDate"">='{startDate}' and nts.""StartDate""<'{endDate}' and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}' limit 1";
            //var parameters = new Dictionary<string, object> {
            //    { "UserId",userId },
            //    { "Status",StatusEnum.Active },
            //    { "CompanyId",1 },
            //    { "SD",startDate },
            //    { "ED",endDate },
            //};
            var res = await _queryRepo.ExecuteQuerySingle<ServiceViewModel>(cypher, null);
            return res;
        }

        public async Task<List<LeaveDetailViewModel>> GetAllLeaves(List<TemplateViewModel> templateList)
        {
            var selectQry = "";
            var i = 1;
            foreach (var item in templateList.Where(x => x.UdfTableMetadataId != null))
            {
                var tableMeta = await _tableMetadataBusiness.GetSingle(x => x.Id == item.UdfTableMetadataId);

                if (item.Code == "LeaveAdjustment" || item.Code == "LEAVE_ACCRUAL" || item.Code == "LEAVE_CANCEL" || item.Code == "LeaveHandoverService" || item.Code == "AnnualLeaveEncashment" || item.Code == "UndertimeLeave" || item.Code == "RETURN_TO_WORK")

                {

                }
                else
                {
                    if (i != 1)
                    {
                        selectQry += " union ";
                    }

                    selectQry = @$" {selectQry} Select u.""Id"" as UserId, udf.""LeaveStartDate""::TIMESTAMP::DATE as StartDate,  udf.""LeaveEndDate""::TIMESTAMP::DATE  as EndDate, t.""DisplayName"" as LeaveType, udf.""LeaveDurationCalendarDays"" as CalendarDuration, udf.""LeaveDurationWorkingDays"" as WorkingDuration,
                            t.""Code"" as LeaveTypeCode, lov.""Name"" as ServiceStatus
                            From cms.""{tableMeta.Name}"" as udf
                            Join public.""NtsNote"" as n on udf.""NtsNoteId""=n.""Id"" and n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Left Join public.""NtsService"" as s on n.""Id""=s.""UdfNoteId"" and s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Left Join public.""Template"" as t on s.""TemplateId""=t.""Id"" and t.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Left Join public.""Template"" as UdfTemplate on s.""UdfTemplateId""=UdfTemplate.""Id"" and UdfTemplate.""IsDeleted""=false and UdfTemplate.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Left join public.""ServiceTemplate"" as st on st.""TemplateId""=t.""Id"" and st.""IsDeleted""=false and st.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Left Join public.""User"" as u on s.""OwnerUserId""=u.""Id"" and u.""CompanyId""='{_repo.UserContext.CompanyId}'
                            Left Join public.""LOV"" as lov on s.""ServiceStatusId""=lov.""Id"" and lov.""Code"" not in ('SERVICE_STATUS_DRAFT','SERVICE_STATUS_CANCEL') and lov.""CompanyId""='{_repo.UserContext.CompanyId}'
where  udf.""CompanyId""='{_repo.UserContext.CompanyId}' and udf.""IsDeleted""=false";
                    i++;
                }

            }

            var querydata = await _queryRepo.ExecuteQueryList<LeaveDetailViewModel>(selectQry, null);
            return querydata;
        }

        public async Task<PayrollRunViewModel> GetPayrollRunDetails(DateTime startDate, DateTime endDate)
        {
            var cypher = $@"select pay.*,pb.""Id"" as PayrollId
                    ,leg.""Id"" as LegalEntityId,pg.""Id"" as PayrollGroupId,
					pb.""PayrollStartDate"" as PayrollStartDate,pb.""PayrollEndDate"" as PayrollEndDate
                    ,pb.""AttendanceStartDate"" as AttendanceStartDate,pb.""AttendanceEndDate"" as AttendanceEndDate
                    ,pb.""YearMonth"" as YearMonth,pb.""RunType"" as RunType
from cms.""N_PayrollHR_PayrollRun"" as pay
join cms.""N_PayrollHR_PayrollBatch"" as pb on pb.""Id""=pay.""PayrollBatchId"" and pb.""IsDeleted""=false and pb.""CompanyId""='{_repo.UserContext.CompanyId}'
left join public.""LegalEntity"" as leg on leg.""Id""=pb.""LegalEntityId"" and leg.""IsDeleted""=false and leg.""Id""='{_repo.UserContext.LegalEntityId}' and leg.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_CoreHR_HRDepartment"" as d on d.""LegalEntityId""=leg.""Id"" and d.""IsDeleted""=false and d.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_PayrollHR_PayrollGroup"" as pg on pg.""Id""=pb.""PayrollGroupId"" and pg.""IsDeleted""=false and pg.""CompanyId""='{_repo.UserContext.CompanyId}'
where pb.""PayrollStartDate""::date='{startDate}' and pb.""PayrollEndDate""='{endDate}'
and pay.""IsDeleted""=false and pay.""CompanyId""='{_repo.UserContext.CompanyId}'";

            var vm = await _queryRepo.ExecuteQuerySingle<PayrollRunViewModel>(cypher, null);
            return vm;
        }

        public async Task<bool> IsDayOff(string personId, DateTime todayDate, string IsDayQueryCheck)
        {
            var query = $@"select case when((count(pch.""Id"") > 0) or cast(pc.""{IsDayQueryCheck}"" as boolean)) then true else false end as IsDayOff
                        from cms.""N_CoreHR_HRPerson"" as pr
                        join cms.""N_PayrollHR_SalaryInfo"" as ps on ps.""PersonId"" = pr.""Id"" and ps.""IsDeleted"" = false and ps.""CompanyId""='{_repo.UserContext.CompanyId}'
                        join cms.""N_PayrollHR_PayrollCalendar"" as pc on pc.""Id"" = ps.""PayCalendarId""  and pc.""IsDeleted"" = false and pc.""CompanyId""='{_repo.UserContext.CompanyId}'
                        left join cms.""N_PayrollHR_CalendarHoliday"" as pch on pch.""CalendarId"" = pc.""Id"" and pch.""IsDeleted"" = false
                        and pch.""FromDate""::Date <= '{todayDate}'::Date and '{todayDate}'::Date <= pch.""ToDate""::Date and pch.""CompanyId""='{_repo.UserContext.CompanyId}'
                        where pr.""IsDeleted"" = false and pr.""Id"" = '{personId}'  and pr.""IsDeleted""=false and pr.""CompanyId""='{_repo.UserContext.CompanyId}'
                        group by pch.""Id"",pc.""{IsDayQueryCheck}""
                        limit 1 ";
            var result = await _queryRepo.ExecuteScalar<bool>(query, null);
            return result;
        }

        public async Task<CalendarViewModel> GetCalendarDetailsByCalendarId(string calendarId)
        {
            var query = $@"select pc.* from cms.""N_PayrollHR_PayrollCalendar"" as pc 
                        where pc.""Id""='{calendarId}' and pc.""IsDeleted""=false and pc.""CompanyId""='{_repo.UserContext.CompanyId}'";


            var payCalendar = await _queryRepo.ExecuteQuerySingle<CalendarViewModel>(query, null);
            return payCalendar;
        }

        public async Task<List<CalendarHolidayViewModel>> GetHolidays()
        {

            var query = $@"Select ch.*, pr.""Id"" as PersonId, u.""Id"" as UserId
                        From cms.""N_PayrollHR_SalaryInfo"" as ps
                        Join cms.""N_CoreHR_HRPerson"" as pr on pr.""Id""=ps.""PersonId"" and pr.""IsDeleted""=false and pr.""CompanyId""='{_repo.UserContext.CompanyId}'
                        Join public.""User"" as u on u.""Id""=pr.""UserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
                        Join cms.""N_PayrollHR_PayrollCalendar"" as pc on pc.""Id""=ps.""PayCalendarId"" and pc.""IsDeleted""=false and pc.""CompanyId""='{_repo.UserContext.CompanyId}'
                        Join cms.""N_PayrollHR_CalendarHoliday"" as ch on ch.""CalendarId""=pc.""Id"" and ch.""CompanyId""='{_repo.UserContext.CompanyId}'  and ch.""IsDeleted""=false
where ps.""CompanyId""='{_repo.UserContext.CompanyId}'  and ps.""IsDeleted""=false";

            var querydata = await _queryRepo.ExecuteQueryList<CalendarHolidayViewModel>(query, null);
            return querydata;
        }

        public async Task<double> GetLeaveBalanceWithFutureEntitlementKSA(int year, string leaveTypeCode, string userId)
        {
            
            var query = $@"SELECT lbs.""ClosingBalance""
                        FROM cms.""N_TAA_LeaveBalanceSheet"" as lbs
                        join public.""User"" as u on u.""Id""=lbs.""UserId"" and u.""Id""='{userId}' and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
                        join cms.""N_TAA_LeaveType"" as lt on lbs.""LeaveTypeId""=lt.""Id"" and lt.""Code""='{leaveTypeCode}' and lt.""IsDeleted""=false and lt.""CompanyId""='{_repo.UserContext.CompanyId}'
                        where lbs.""Year""='{year}' and lbs.""CompanyId""='{_repo.UserContext.CompanyId}' and lbs.""IsDeleted""=false";

            var leaveBalance = await _queryRepo.ExecuteScalar<double>(query, null);

            return leaveBalance;
        }

        public async Task<double> GetLeaveBalanceWithFutureEntitlementUAE(int year, string leaveTypeCode, string userId)
        {
            
            var query = $@"SELECT lbs.""ClosingBalance""
                        FROM cms.""N_TAA_LeaveBalanceSheet"" as lbs 
                        join public.""User"" as u on u.""Id""=lbs.""UserId"" and u.""Id""='{userId}' and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
                        join cms.""N_TAA_LeaveType"" as lt on lbs.""LeaveTypeId""=lt.""Id"" and lt.""Code""='{leaveTypeCode}' and lt.""IsDeleted""=false and lt.""CompanyId""='{_repo.UserContext.CompanyId}'
                        where lbs.""Year""='{year}' and lbs.""CompanyId""='{_repo.UserContext.CompanyId}' and lbs.""IsDeleted""=false";
            

            var leaveBalance = await _queryRepo.ExecuteScalar<double>(query, null);

            return leaveBalance;

        }

        public async Task<List<TeamLeaveRequestViewModel>> GetTeamLeaveDetails(string organizationId, DateTime? date)
        {
            var query = $@"

select 
u.""Id"" as UserId,nts.""Id"" as Id,nts.""ServiceNo"" as ServiceNo,nts.""Id"" as ServiceId ,nts.""TemplateCode"" as Title,true as IsAllDay
  ,al.""LeaveStartDate"" as Start,al.""LeaveEndDate"" as End,lov.""Name"" as LeaveStatus,lov.""Code"" as LeaveStatusCode,
     p.""Id"" as PersonId,concat(p.""PersonFullName"",p.""SponsorshipNo"") as Name,p.""SponsorshipNo"" as SponsorshipNo,p.""PersonNo"" as PersonNo


from cms.""N_CoreHR_HRDepartment"" as d
join cms.""N_CoreHR_HRAssignment"" as a on a.""DepartmentId""=d.""Id"" and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRJob"" as j on a.""JobId""=j.""Id"" and j.""IsDeleted""=false and j.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRPerson"" as p on a.""EmployeeId""=p.""Id"" and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""User"" as u on u.""Id""=p.""UserId"" and u.""IsDeleted""=false  and u.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""NtsService"" as nts on nts.""OwnerUserId""=u.""Id"" and nts.""IsDeleted""=false and nts.""CompanyId""='{_repo.UserContext.CompanyId}'	
join cms.""N_Leave_AnnualLeave"" as al on al.""Id""=nts.""UdfNoteTableId"" and al.""IsDeleted""=false and al.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""LOV"" as lov on lov.""Id""=nts.""ServiceStatusId"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_repo.UserContext.CompanyId}'	  
where lov.""Code"" not in ('SERVICE_STATUS_DRAFT' , 'SERVICE_STATUS_CANCEL') and d.""CompanyId""='{_repo.UserContext.CompanyId}' and d.""IsDeleted""=false #Org#
	  
union 	  
select 
u.""Id"" as UserId,nts.""Id"" as Id,nts.""ServiceNo"" as ServiceNo,nts.""Id"" as ServiceId ,nts.""TemplateCode"" as Title,true as IsAllDay
  ,al.""LeaveStartDate"" as Start,al.""LeaveEndDate"" as End,lov.""Name"" as LeaveStatus,lov.""Code"" as LeaveStatusCode,
     p.""Id"" as PersonId,concat(p.""PersonFullName"",p.""SponsorshipNo"") as Name,p.""SponsorshipNo"" as SponsorshipNo,p.""PersonNo"" as PersonNo


from cms.""N_CoreHR_HRDepartment"" as d
join cms.""N_CoreHR_HRAssignment"" as a on a.""DepartmentId""=d.""Id"" and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRJob"" as j on a.""JobId""=j.""Id"" and j.""IsDeleted""=false and j.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRPerson"" as p on a.""EmployeeId""=p.""Id"" and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""User"" as u on u.""Id""=p.""UserId"" and u.""IsDeleted""=false  and u.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""NtsService"" as nts on nts.""OwnerUserId""=u.""Id"" and nts.""IsDeleted""=false and nts.""CompanyId""='{_repo.UserContext.CompanyId}'	
join cms.""N_Leave_MaternityLeave"" as al on al.""Id""=nts.""UdfNoteTableId"" and al.""IsDeleted""=false and al.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""LOV"" as lov on lov.""Id""=nts.""ServiceStatusId"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_repo.UserContext.CompanyId}'	  
where lov.""Code"" not in ('SERVICE_STATUS_DRAFT' , 'SERVICE_STATUS_CANCEL') and d.""CompanyId""='{_repo.UserContext.CompanyId}' and d.""IsDeleted""=false #Org#
	  
union 	  
select 
u.""Id"" as UserId,nts.""Id"" as Id,nts.""ServiceNo"" as ServiceNo,nts.""Id"" as ServiceId ,nts.""TemplateCode"" as Title,true as IsAllDay
  ,al.""LeaveStartDate"" as Start,al.""LeaveEndDate"" as End,lov.""Name"" as LeaveStatus,lov.""Code"" as LeaveStatusCode,
     p.""Id"" as PersonId,concat(p.""PersonFullName"",p.""SponsorshipNo"") as Name,p.""SponsorshipNo"" as SponsorshipNo,p.""PersonNo"" as PersonNo


from cms.""N_CoreHR_HRDepartment"" as d
join cms.""N_CoreHR_HRAssignment"" as a on a.""DepartmentId""=d.""Id"" and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRJob"" as j on a.""JobId""=j.""Id"" and j.""IsDeleted""=false and j.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRPerson"" as p on a.""EmployeeId""=p.""Id"" and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""User"" as u on u.""Id""=p.""UserId"" and u.""IsDeleted""=false  and u.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""NtsService"" as nts on nts.""OwnerUserId""=u.""Id"" and nts.""IsDeleted""=false and nts.""CompanyId""='{_repo.UserContext.CompanyId}'	
join cms.""N_Leave_PaternityLeave"" as al on al.""Id""=nts.""UdfNoteTableId"" and al.""IsDeleted""=false and al.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""LOV"" as lov on lov.""Id""=nts.""ServiceStatusId"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_repo.UserContext.CompanyId}'	  
where lov.""Code"" not in ('SERVICE_STATUS_DRAFT' , 'SERVICE_STATUS_CANCEL') and d.""CompanyId""='{_repo.UserContext.CompanyId}' and d.""IsDeleted""=false #Org#
	  
union 	  
select 
u.""Id"" as UserId,nts.""Id"" as Id,nts.""ServiceNo"" as ServiceNo,nts.""Id"" as ServiceId ,nts.""TemplateCode"" as Title,true as IsAllDay
  ,al.""LeaveStartDate"" as Start,al.""LeaveEndDate"" as End,lov.""Name"" as LeaveStatus,lov.""Code"" as LeaveStatusCode,
     p.""Id"" as PersonId,concat(p.""PersonFullName"",p.""SponsorshipNo"") as Name,p.""SponsorshipNo"" as SponsorshipNo,p.""PersonNo"" as PersonNo


from cms.""N_CoreHR_HRDepartment"" as d
join cms.""N_CoreHR_HRAssignment"" as a on a.""DepartmentId""=d.""Id"" and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRJob"" as j on a.""JobId""=j.""Id"" and j.""IsDeleted""=false and j.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRPerson"" as p on a.""EmployeeId""=p.""Id"" and p.""IsDeleted""=false  and p.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""User"" as u on u.""Id""=p.""UserId"" and u.""IsDeleted""=false  and u.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""NtsService"" as nts on nts.""OwnerUserId""=u.""Id"" and nts.""IsDeleted""=false and nts.""CompanyId""='{_repo.UserContext.CompanyId}'	
join cms.""N_Leave_CompassionatelyLeave"" as al on al.""Id""=nts.""UdfNoteTableId"" and al.""IsDeleted""=false and al.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""LOV"" as lov on lov.""Id""=nts.""ServiceStatusId"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_repo.UserContext.CompanyId}'	  
where lov.""Code"" not in ('SERVICE_STATUS_DRAFT' , 'SERVICE_STATUS_CANCEL') and d.""CompanyId""='{_repo.UserContext.CompanyId}' and d.""IsDeleted""=false #Org#
	   
union 	  
select 
u.""Id"" as UserId,nts.""Id"" as Id,nts.""ServiceNo"" as ServiceNo,nts.""Id"" as ServiceId ,nts.""TemplateCode"" as Title,true as IsAllDay
  ,al.""LeaveStartDate"" as Start,al.""LeaveEndDate"" as End,lov.""Name"" as LeaveStatus,lov.""Code"" as LeaveStatusCode,
     p.""Id"" as PersonId,concat(p.""PersonFullName"",p.""SponsorshipNo"") as Name,p.""SponsorshipNo"" as SponsorshipNo,p.""PersonNo"" as PersonNo


from cms.""N_CoreHR_HRDepartment"" as d
join cms.""N_CoreHR_HRAssignment"" as a on a.""DepartmentId""=d.""Id"" and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRJob"" as j on a.""JobId""=j.""Id"" and j.""IsDeleted""=false and j.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRPerson"" as p on a.""EmployeeId""=p.""Id"" and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""User"" as u on u.""Id""=p.""UserId"" and u.""IsDeleted""=false  and u.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""NtsService"" as nts on nts.""OwnerUserId""=u.""Id"" and nts.""IsDeleted""=false and nts.""CompanyId""='{_repo.UserContext.CompanyId}'	
join cms.""N_Leave_HajjLeave"" as al on al.""Id""=nts.""UdfNoteTableId"" and al.""IsDeleted""=false and al.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""LOV"" as lov on lov.""Id""=nts.""ServiceStatusId"" and lov.""IsDeleted""=false	and lov.""CompanyId""='{_repo.UserContext.CompanyId}'  
where lov.""Code"" not in ('SERVICE_STATUS_DRAFT' , 'SERVICE_STATUS_CANCEL') and d.""CompanyId""='{_repo.UserContext.CompanyId}' and d.""IsDeleted""=false #Org#
	  

	  
union 	  
select 
u.""Id"" as UserId,nts.""Id"" as Id,nts.""ServiceNo"" as ServiceNo,nts.""Id"" as ServiceId ,nts.""TemplateCode"" as Title,true as IsAllDay
  ,al.""LeaveStartDate"" as Start,al.""LeaveEndDate"" as End,lov.""Name"" as LeaveStatus,lov.""Code"" as LeaveStatusCode,
     p.""Id"" as PersonId,concat(p.""PersonFullName"",p.""SponsorshipNo"") as Name,p.""SponsorshipNo"" as SponsorshipNo,p.""PersonNo"" as PersonNo


from cms.""N_CoreHR_HRDepartment"" as d
join cms.""N_CoreHR_HRAssignment"" as a on a.""DepartmentId""=d.""Id"" and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRJob"" as j on a.""JobId""=j.""Id"" and j.""IsDeleted""=false and j.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRPerson"" as p on a.""EmployeeId""=p.""Id"" and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""User"" as u on u.""Id""=p.""UserId"" and u.""IsDeleted""=false  and u.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""NtsService"" as nts on nts.""OwnerUserId""=u.""Id"" and nts.""IsDeleted""=false and nts.""CompanyId""='{_repo.UserContext.CompanyId}'	
join cms.""N_Leave_LeaveExamination"" as al on al.""Id""=nts.""UdfNoteTableId"" and al.""IsDeleted""=false and al.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""LOV"" as lov on lov.""Id""=nts.""ServiceStatusId"" and lov.""IsDeleted""=false	and lov.""CompanyId""='{_repo.UserContext.CompanyId}'  
where lov.""Code"" not in ('SERVICE_STATUS_DRAFT' , 'SERVICE_STATUS_CANCEL') and d.""CompanyId""='{_repo.UserContext.CompanyId}' and d.""IsDeleted""=false #Org#
	  
union 	  
select 
u.""Id"" as UserId,nts.""Id"" as Id,nts.""ServiceNo"" as ServiceNo,nts.""Id"" as ServiceId ,nts.""TemplateCode"" as Title,true as IsAllDay
  ,al.""LeaveStartDate"" as Start,al.""LeaveEndDate"" as End,lov.""Name"" as LeaveStatus,lov.""Code"" as LeaveStatusCode,
     p.""Id"" as PersonId,concat(p.""PersonFullName"",p.""SponsorshipNo"") as Name,p.""SponsorshipNo"" as SponsorshipNo,p.""PersonNo"" as PersonNo


from cms.""N_CoreHR_HRDepartment"" as d
join cms.""N_CoreHR_HRAssignment"" as a on a.""DepartmentId""=d.""Id"" and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRJob"" as j on a.""JobId""=j.""Id"" and j.""IsDeleted""=false and j.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRPerson"" as p on a.""EmployeeId""=p.""Id"" and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""User"" as u on u.""Id""=p.""UserId"" and u.""IsDeleted""=false  and u.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""NtsService"" as nts on nts.""OwnerUserId""=u.""Id"" and nts.""IsDeleted""=false and nts.""CompanyId""='{_repo.UserContext.CompanyId}'	
join cms.""N_Leave_MarriageLeave"" as al on al.""Id""=nts.""UdfNoteTableId"" and al.""IsDeleted""=false and al.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""LOV"" as lov on lov.""Id""=nts.""ServiceStatusId"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_repo.UserContext.CompanyId}'	  
where lov.""Code"" not in ('SERVICE_STATUS_DRAFT' , 'SERVICE_STATUS_CANCEL') and d.""CompanyId""='{_repo.UserContext.CompanyId}' and d.""IsDeleted""=false #Org#

union 	  
select 
u.""Id"" as UserId,nts.""Id"" as Id,nts.""ServiceNo"" as ServiceNo,nts.""Id"" as ServiceId ,nts.""TemplateCode"" as Title,true as IsAllDay
  ,al.""LeaveStartDate"" as Start,al.""LeaveEndDate"" as End,lov.""Name"" as LeaveStatus,lov.""Code"" as LeaveStatusCode,
     p.""Id"" as PersonId,concat(p.""PersonFullName"",p.""SponsorshipNo"") as Name,p.""SponsorshipNo"" as SponsorshipNo,p.""PersonNo"" as PersonNo


from cms.""N_CoreHR_HRDepartment"" as d
join cms.""N_CoreHR_HRAssignment"" as a on a.""DepartmentId""=d.""Id"" and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRJob"" as j on a.""JobId""=j.""Id"" and j.""IsDeleted""=false and j.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRPerson"" as p on a.""EmployeeId""=p.""Id"" and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""User"" as u on u.""Id""=p.""UserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""NtsService"" as nts on nts.""OwnerUserId""=u.""Id"" and nts.""IsDeleted""=false and nts.""CompanyId""='{_repo.UserContext.CompanyId}'	
join cms.""N_Leave_PlannedUnpaidLeave"" as al on al.""Id""=nts.""UdfNoteTableId"" and al.""IsDeleted""=false  and al.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""LOV"" as lov on lov.""Id""=nts.""ServiceStatusId"" and lov.""IsDeleted""=false  and lov.""CompanyId""='{_repo.UserContext.CompanyId}'	  	  
where lov.""Code"" not in ('SERVICE_STATUS_DRAFT' , 'SERVICE_STATUS_CANCEL') and d.""CompanyId""='{_repo.UserContext.CompanyId}' and d.""IsDeleted""=false #Org#

union 	  
select 
u.""Id"" as UserId,nts.""Id"" as Id,nts.""ServiceNo"" as ServiceNo,nts.""Id"" as ServiceId ,nts.""TemplateCode"" as Title,true as IsAllDay
  ,al.""LeaveStartDate"" as Start,al.""LeaveEndDate"" as End,lov.""Name"" as LeaveStatus,lov.""Code"" as LeaveStatusCode,
     p.""Id"" as PersonId,concat(p.""PersonFullName"",p.""SponsorshipNo"") as Name,p.""SponsorshipNo"" as SponsorshipNo,p.""PersonNo"" as PersonNo


from cms.""N_CoreHR_HRDepartment"" as d
join cms.""N_CoreHR_HRAssignment"" as a on a.""DepartmentId""=d.""Id"" and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'	
join cms.""N_CoreHR_HRJob"" as j on a.""JobId""=j.""Id"" and j.""IsDeleted""=false and j.""CompanyId""='{_repo.UserContext.CompanyId}'	
join cms.""N_CoreHR_HRPerson"" as p on a.""EmployeeId""=p.""Id"" and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'	
join public.""User"" as u on u.""Id""=p.""UserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'	
join public.""NtsService"" as nts on nts.""OwnerUserId""=u.""Id"" and nts.""IsDeleted""=false and nts.""CompanyId""='{_repo.UserContext.CompanyId}'	
join cms.""N_Leave_SickLeave"" as al on al.""Id""=nts.""UdfNoteTableId"" and al.""IsDeleted""=false  and al.""CompanyId""='{_repo.UserContext.CompanyId}'	 
join public.""LOV"" as lov on lov.""Id""=nts.""ServiceStatusId"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_repo.UserContext.CompanyId}'	  
where lov.""Code"" not in ('SERVICE_STATUS_DRAFT' , 'SERVICE_STATUS_CANCEL') and d.""CompanyId""='{_repo.UserContext.CompanyId}' and d.""IsDeleted""=false #Org#

union 	  
select 
u.""Id"" as UserId,nts.""Id"" as Id,nts.""ServiceNo"" as ServiceNo,nts.""Id"" as ServiceId ,nts.""TemplateCode"" as Title,true as IsAllDay
  ,al.""LeaveStartDate"" as Start,al.""LeaveEndDate"" as End,lov.""Name"" as LeaveStatus,lov.""Code"" as LeaveStatusCode,
     p.""Id"" as PersonId,concat(p.""PersonFullName"",p.""SponsorshipNo"") as Name,p.""SponsorshipNo"" as SponsorshipNo,p.""PersonNo"" as PersonNo


from cms.""N_CoreHR_HRDepartment"" as d
join cms.""N_CoreHR_HRAssignment"" as a on a.""DepartmentId""=d.""Id"" and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRJob"" as j on a.""JobId""=j.""Id"" and j.""IsDeleted""=false and j.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRPerson"" as p on a.""EmployeeId""=p.""Id"" and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""User"" as u on u.""Id""=p.""UserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""NtsService"" as nts on nts.""OwnerUserId""=u.""Id"" and nts.""IsDeleted""=false and nts.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_Leave_UndertimeLeave"" as al on al.""Id""=nts.""UdfNoteTableId"" and al.""IsDeleted""=false and al.""CompanyId""='{_repo.UserContext.CompanyId}'	 
join public.""LOV"" as lov on lov.""Id""=nts.""ServiceStatusId"" and lov.""IsDeleted""=false  and lov.""CompanyId""='{_repo.UserContext.CompanyId}'	   
where lov.""Code"" not in ('SERVICE_STATUS_DRAFT' , 'SERVICE_STATUS_CANCEL') and d.""CompanyId""='{_repo.UserContext.CompanyId}' and d.""IsDeleted""=false #Org#

union 	  
select 
u.""Id"" as UserId,nts.""Id"" as Id,nts.""ServiceNo"" as ServiceNo,nts.""Id"" as ServiceId ,nts.""TemplateCode"" as Title,true as IsAllDay
  ,al.""LeaveStartDate"" as Start,al.""LeaveEndDate"" as End,lov.""Name"" as LeaveStatus,lov.""Code"" as LeaveStatusCode,
     p.""Id"" as PersonId,concat(p.""PersonFullName"",p.""SponsorshipNo"") as Name,p.""SponsorshipNo"" as SponsorshipNo,p.""PersonNo"" as PersonNo


from cms.""N_CoreHR_HRDepartment"" as d
join cms.""N_CoreHR_HRAssignment"" as a on a.""DepartmentId""=d.""Id"" and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRJob"" as j on a.""JobId""=j.""Id"" and j.""IsDeleted""=false and j.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_CoreHR_HRPerson"" as p on a.""EmployeeId""=p.""Id"" and p.""IsDeleted""=false  and p.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""User"" as u on u.""Id""=p.""UserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'	
join public.""NtsService"" as nts on nts.""OwnerUserId""=u.""Id"" and nts.""IsDeleted""=false and nts.""CompanyId""='{_repo.UserContext.CompanyId}'	
join cms.""N_Leave_UnpaidLeave"" as al on al.""Id""=nts.""UdfNoteTableId"" and al.""IsDeleted""=false and al.""CompanyId""='{_repo.UserContext.CompanyId}'	
join public.""LOV"" as lov on lov.""Id""=nts.""ServiceStatusId"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_repo.UserContext.CompanyId}'	   
where lov.""Code"" not in ('SERVICE_STATUS_DRAFT' , 'SERVICE_STATUS_CANCEL') and d.""CompanyId""='{_repo.UserContext.CompanyId}' and d.""IsDeleted""=false	 #Org#
 	";
            var whereorg = "";
            if (organizationId.IsNotNullAndNotEmpty() && organizationId != "All")
            {
                whereorg = $@"and d.""Id""='{organizationId}'";
            }
            query = query.Replace("#Org#", whereorg);

            var list = await _queryRepo.ExecuteQueryList<TeamLeaveRequestViewModel>(query, null);

            return list;
        }

        public async Task<LeaveTypeViewModel> GetLeaveTypeByCode(LeaveBalanceSheetViewModel viewModel)
        {
            var query = $@"select * from cms.""N_TAA_LeaveType"" where ""Code""='{viewModel.LeaveTypeCode}'";
            var leaveType = await _queryRepo.ExecuteQuerySingle<LeaveTypeViewModel>(query, null);
            return leaveType;
        }

    }
}
