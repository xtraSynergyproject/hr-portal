
using AutoMapper;
using CMS.Common;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Business
{
   public class SuccessionPlanningBusiness: BusinessBase<NoteViewModel, NtsNote>, ISuccessionPlanningBusiness
    {
        private readonly IRepositoryQueryBase<SuccessionPlaningViewModel> _queryRepo1;
		private readonly IRepositoryQueryBase<SuccessionPlanningAssessmentViewModel> _queryAssessment;
		private readonly IRepositoryQueryBase<CompetencyFeedbackUserViewModel> _CompetencyFeedback;


		public SuccessionPlanningBusiness(IRepositoryBase<NoteViewModel, NtsNote> repo, IMapper autoMapper,IRepositoryQueryBase<SuccessionPlaningViewModel> queryRepo1,
			 IRepositoryQueryBase<SuccessionPlanningAssessmentViewModel> queryAssessment,
			 IRepositoryQueryBase<CompetencyFeedbackUserViewModel> CompetencyFeedback) : base(repo, autoMapper)
        {
			_queryRepo1 = queryRepo1;
			_queryAssessment = queryAssessment;
			_CompetencyFeedback = CompetencyFeedback;


		}

        public async Task<List<SuccessionPlaningViewModel>> GetSuccessionPlanings(string Module, string Employee, string Department, int? Month, int? year)
        {
			//            var query=$@"SELECT ""Id"" as ID, ""Name"" as Employee, null as ParentId, 1 as sequencess
			//	FROM public.""Module""  where ""IsDeleted""=false and ""Id""='60ec5b4a250bcb01854887d9'
			//	union
			//
			//	select ""Id"" as ID,""PersonFullName"" as Employee,'60ec5b4a250bcb01854887d9' as ParentId , 2 as sequencess
			//	from cms.""N_CoreHR_HRPerson"" 
			//	where ""IsDeleted""=false	
			//union
			//select null  as ID,j.""JobTitle""as ""Employee"",p.""Id"" as ParentId , 3 as sequencess from cms.""N_CoreHR_HRPerson""  as p
			//inner join cms.""N_CoreHR_HRAssignment"" as a
			//on a.""EmployeeId""=p.""Id""  and P.""IsDeleted""=false
			//inner join cms.""N_CoreHR_HRJob"" as j on j.""Id""=a.""JobId"" 
			//where j.""IsDeleted""=false
			//union
			//	select null as ID,'Assessment' as Employee,""Id"" as ParentId , 4 as sequencess
			//from cms.""N_CoreHR_HRPerson"" 
			//	where ""IsDeleted""=false";

			var query = $@"SELECT ""Id"" as ID, ""Name"" as Employee, null as ParentId, 1 as sequencess,'' as UserId
	FROM public.""Module""  where ""IsDeleted""=false and ""Id""='{Module}' and ""CompanyId""='{_repo.UserContext.CompanyId}'
	union
select  ""Id"" as ID,""DepartmentName"" as Employee,'{Module}' as ParentId , 2 as sequencess,'' as UserId
from cms.""N_CoreHR_HRDepartment"" where ""IsDeleted""=false  and ""Id""='{Department}' and ""CompanyId""='{_repo.UserContext.CompanyId}'

union


	select p.""Id"" as ID,""PersonFullName"" as Employee, a.""DepartmentId"" as ParentId , 3 as sequencess,'' as UserId
	from cms.""N_CoreHR_HRPerson""  as p
	inner join cms.""N_CoreHR_HRAssignment"" as a on a.""EmployeeId""=p.""Id""  and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'
	where p.""IsDeleted""=false	and a.""DepartmentId""='{Department}' and p.""CompanyId""='{_repo.UserContext.CompanyId}'	


    union
select null  as ID,j.""JobTitle""as ""Employee"",P.""Id"" as ParentId , 4 as sequencess,'' as UserId from cms.""N_CoreHR_HRPerson""  as p
inner join cms.""N_CoreHR_HRAssignment"" as a
on a.""EmployeeId""=p.""Id""  and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'	
inner join cms.""N_CoreHR_HRJob"" as j on j.""Id""=a.""JobId"" and j.""CompanyId""='{_repo.UserContext.CompanyId}' 
where j.""IsDeleted""=false  and a.""DepartmentId""='{Department}'	 and p.""CompanyId""='{_repo.UserContext.CompanyId}' and P.""IsDeleted""=false	
	
union

select null as ID,'AssessmentSet' as Employee, p.""Id"" as ParentId , 5 as sequencess, P.""UserId""
	from cms.""N_CoreHR_HRPerson""  as p
	inner join cms.""N_CoreHR_HRAssignment"" as a on a.""EmployeeId""=p.""Id""  and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'
	where p.""IsDeleted""=false	and a.""DepartmentId""='{Department}' and p.""CompanyId""='{_repo.UserContext.CompanyId}'	 

union

select null as ID,'Assessment' as Employee, P.""Id"" as ParentId , 6 as sequencess, P.""UserId""
	from cms.""N_CoreHR_HRPerson""  as p
	inner join cms.""N_CoreHR_HRAssignment"" as a on a.""EmployeeId""=p.""Id""  and a.""IsDeleted""=false and a.""CompanyId""='{_repo.UserContext.CompanyId}'
	where p.""IsDeleted""=false	and a.""DepartmentId""='{Department}' and p.""CompanyId""='{_repo.UserContext.CompanyId}'	
	order by sequencess";

			var result = await _queryRepo1.ExecuteQueryList<SuccessionPlaningViewModel>(query, null);
			if (year.IsNotNull())
			{
				result[0].Year = year;
			}
			if (Month.IsNotNull())
			{
				DateTime date = new DateTime(Convert.ToInt32(year),Convert.ToInt32(Month), 1);
				result[0].Month = date.ToString("MMM");
			}

			var cnt = await GetAssessmentListofUser(Convert.ToInt32(Month), Convert.ToInt32(year));
			var cntSet = await GetAssessmenSettListofUser(Convert.ToInt32(Month), Convert.ToInt32(year));
		//	if (cnt.Count > 0)
			{
				foreach (var item in result)
				{
					if (item.Employee == "Assessment")
					{

						var data = cnt.Where(x => x.UserId == item.UserId).ToList();
						if (data.Count > 0)
						{
							foreach (var item2 in data)
							{
								var date = item2.Date;

								switch (date.Day)
								{
									case 1:
										item.Day1 = item2.Count.ToString();
										item.Sa1 = item2.Status;
										break;
									case 2:
										item.Day2 = item2.Count.ToString();
										item.Sa2 = item2.Status;
										break;
									case 3:
										item.Day3 = item2.Count.ToString();
										item.Sa3 = item2.Status;
										break;
									case 4:
										item.Day4 = item2.Count.ToString();
										item.Sa4 = item2.Status;
										break;
									case 5:
										item.Day5 = item2.Count.ToString();
										item.Sa5 = item2.Status;
										break;
									case 6:
										item.Day6 = item2.Count.ToString();
										item.Sa6 = item2.Status;
										break;
									case 7:
										item.Day7 = item2.Count.ToString();
										item.Sa7 = item2.Status;
										break;
									case 8:
										item.Day8 = item2.Count.ToString();
										item.Sa8 = item2.Status;
										break;
									case 9:
										item.Day9 = item2.Count.ToString();
										item.Sa9 = item2.Status;
										break;
									case 10:
										item.Day10 = item2.Count.ToString();
										item.Sa10 = item2.Status;
										break;
									case 11:
										item.Day11 = item2.Count.ToString();
										item.Sa11 = item2.Status;
										break;
									case 12:
										item.Day12 = item2.Count.ToString();
										item.Sa12 = item2.Status;
										break;
									case 13:
										item.Day13 = item2.Count.ToString();
										item.Sa13 = item2.Status;
										break;
									case 14:
										item.Day14 = item2.Count.ToString();
										item.Sa14 = item2.Status;

										break;
									case 15:
										item.Day15 = item2.Count.ToString();
										item.Sa15 = item2.Status;
										break;
									case 16:
										item.Day16 = item2.Count.ToString();
										item.Sa16 = item2.Status;
										break;
									case 17:
										item.Day17 = item2.Count.ToString();
										item.Sa17 = item2.Status;
										break;
									case 18:
										item.Day18 = item2.Count.ToString();
										item.Sa18 = item2.Status;
										break;
									case 19:
										item.Day19 = item2.Count.ToString();
										item.Sa19 = item2.Status;
										break;
									case 20:
										item.Day20 = item2.Count.ToString();
										item.Sa20 = item2.Status;
										break;
									case 21:
										item.Day21 = item2.Count.ToString();
										item.Sa21 = item2.Status;
										break;
									case 22:
										item.Day22 = item2.Count.ToString();
										item.Sa22 = item2.Status;
										break;
									case 23:
										item.Day23 = item2.Count.ToString();
										item.Sa23 = item2.Status;
										break;
									case 24:
										item.Day24 = item2.Count.ToString();
										item.Sa24 = item2.Status;
										break;
									case 25:
										item.Day25 = item2.Count.ToString();
										item.Sa25 = item2.Status;
										break;
									case 26:
										item.Day26 = item2.Count.ToString();
										item.Sa26 = item2.Status;
										break;
									case 27:
										item.Day27 = item2.Count.ToString();
										item.Sa27 = item2.Status;
										break;
									case 28:
										item.Day28 = item2.Count.ToString();
										item.Sa28 = item2.Status;
										break;
									case 29:
										item.Day29 = item2.Count.ToString();
										item.Sa29 = item2.Status;
										break;
									case 30:
										item.Day30 = item2.Count.ToString();
										item.Sa30 = item2.Status;
										break;
									case 31:
										item.Day31 = item2.Count.ToString();
										item.Sa31 = item2.Status;
										break;
									default:
										break;
								}
							}
						}
						else { item.Day1 = "1";
							item.Sa1 = "Inprogress";
						}

					}
					else if (item.Employee == "AssessmentSet")
					{

						var data = cntSet.Where(x => x.UserId == item.UserId).ToList();
						if (data.Count > 0)
						{
							foreach (var item2 in data)
							{
								var date = item2.Date;

								switch (date.Day)
								{
									case 1:
										item.Day1 = item2.Count.ToString();
										item.Sa1 = item2.Status;
										break;
									case 2:
										item.Day2 = item2.Count.ToString();
										item.Sa2 = item2.Status;
										break;
									case 3:
										item.Day3 = item2.Count.ToString();
										item.Sa3 = item2.Status;
										break;
									case 4:
										item.Day4 = item2.Count.ToString();
										item.Sa4 = item2.Status;
										break;
									case 5:
										item.Day5 = item2.Count.ToString();
										item.Sa5 = item2.Status;
										break;
									case 6:
										item.Day6 = item2.Count.ToString();
										item.Sa6 = item2.Status;
										break;
									case 7:
										item.Day7 = item2.Count.ToString();
										item.Sa7 = item2.Status;
										break;
									case 8:
										item.Day8 = item2.Count.ToString();
										item.Sa8 = item2.Status;
										break;
									case 9:
										item.Day9 = item2.Count.ToString();
										item.Sa9 = item2.Status;
										break;
									case 10:
										item.Day10 = item2.Count.ToString();
										item.Sa10 = item2.Status;
										break;
									case 11:
										item.Day11 = item2.Count.ToString();
										item.Sa11 = item2.Status;
										break;
									case 12:
										item.Day12 = item2.Count.ToString();
										item.Sa12 = item2.Status;
										break;
									case 13:
										item.Day13 = item2.Count.ToString();
										item.Sa13 = item2.Status;
										break;
									case 14:
										item.Day14 = item2.Count.ToString();
										item.Sa14 = item2.Status;

										break;
									case 15:
										item.Day15 = item2.Count.ToString();
										item.Sa15 = item2.Status;
										break;
									case 16:
										item.Day16 = item2.Count.ToString();
										item.Sa16 = item2.Status;
										break;
									case 17:
										item.Day17 = item2.Count.ToString();
										item.Sa17 = item2.Status;
										break;
									case 18:
										item.Day18 = item2.Count.ToString();
										item.Sa18 = item2.Status;
										break;
									case 19:
										item.Day19 = item2.Count.ToString();
										item.Sa19 = item2.Status;
										break;
									case 20:
										item.Day20 = item2.Count.ToString();
										item.Sa20 = item2.Status;
										break;
									case 21:
										item.Day21 = item2.Count.ToString();
										item.Sa21 = item2.Status;
										break;
									case 22:
										item.Day22 = item2.Count.ToString();
										item.Sa22 = item2.Status;
										break;
									case 23:
										item.Day23 = item2.Count.ToString();
										item.Sa23 = item2.Status;
										break;
									case 24:
										item.Day24 = item2.Count.ToString();
										item.Sa24 = item2.Status;
										break;
									case 25:
										item.Day25 = item2.Count.ToString();
										item.Sa25 = item2.Status;
										break;
									case 26:
										item.Day26 = item2.Count.ToString();
										item.Sa26 = item2.Status;
										break;
									case 27:
										item.Day27 = item2.Count.ToString();
										item.Sa27 = item2.Status;
										break;
									case 28:
										item.Day28 = item2.Count.ToString();
										item.Sa28 = item2.Status;
										break;
									case 29:
										item.Day29 = item2.Count.ToString();
										item.Sa29 = item2.Status;
										break;
									case 30:
										item.Day30 = item2.Count.ToString();
										item.Sa30 = item2.Status;
										break;
									case 31:
										item.Day31 = item2.Count.ToString();
										item.Sa31 = item2.Status;
										break;
									default:
										break;
								}
							}
						}
						else { item.Day1 = "1";
							item.Sa1 = "Inprogress";
						}
					}

				}
			}


			return result;


		}





		public async Task<List<SuccessionPlaningViewModel>> GetAssessmentListofUser(int Month,int Year)
		{

			var query = $@" select s.""StartTime""::TIMESTAMP::Date as Date ,S.""UserId"",Count(*) as Count,LOV.""Name"" as Status FROM Public.""NtsService"" NS inner join  

	public.""NtsNote"" N on N.""Id""=NS.""UdfNoteId"" and N.""CompanyId""='{_repo.UserContext.CompanyId}' and N.""IsDeleted"" = false 
	inner join  cms.""N_TAS_UserAssessmentSchedule"" s on  N.""Id"" = s.""NtsNoteId"" and s.""CompanyId""='{_repo.UserContext.CompanyId}' and s.""IsDeleted"" = false
	inner join   cms.""N_TAS_Assessment"" A   on A.""Id"" = s.""AssessmentId"" and A.""IsDeleted"" = false and A.""CompanyId""='{_repo.UserContext.CompanyId}'

	inner join public.""LOV"" as LOV on NS.""ServiceStatusId""=LOV.""Id"" and LOV.""CompanyId""='{_repo.UserContext.CompanyId}' and LOV.""IsDeleted"" = false
	where extract(year from CAST(s.""StartTime"" AS DATE))={Year}  and extract(month from CAST(s.""StartTime"" AS DATE))={Month} and NS.""CompanyId""='{_repo.UserContext.CompanyId}' and NS.""IsDeleted"" = false
	group by Date,S.""UserId"",""Name""";


			var resuly = await _queryRepo1.ExecuteQueryList<SuccessionPlaningViewModel>(query, null);
			return resuly;
		}

		public async Task<List<SuccessionPlaningViewModel>> GetAssessmenSettListofUser(int Month, int Year)
		{

			var query = $@" select s.""CreatedDate""::TIMESTAMP::Date as Date ,S.""UserId"",Count(*) as Count,LOV.""Name"" as Status FROM Public.""NtsService"" NS inner join  

	public.""NtsNote"" N on N.""Id""=NS.""UdfNoteId"" and N.""CompanyId""='{_repo.UserContext.CompanyId}' and N.""IsDeleted"" = false
	inner join cms.""N_TAS_UserAssessmentSet"" s on  N.""Id"" = s.""NtsNoteId"" and s.""CompanyId""='{_repo.UserContext.CompanyId}' and s.""IsDeleted"" = false
	inner join  cms.""N_TAS_AssessmentSet"" A on A.""Id"" = s.""AssessmentSetId"" and A.""IsDeleted"" = false and A.""CompanyId""='{_repo.UserContext.CompanyId}' 

	inner join public.""LOV"" as LOV on NS.""ServiceStatusId""=LOV.""Id"" and LOV.""CompanyId""='{_repo.UserContext.CompanyId}' and LOV.""IsDeleted"" = false
	where extract(year from CAST(s.""CreatedDate"" AS DATE))={Year}  and extract(month from CAST(s.""CreatedDate"" AS DATE))={Month} and NS.""CompanyId""='{_repo.UserContext.CompanyId}' and NS.""IsDeleted"" = false
	group by Date,S.""UserId"",""Name""";


			var resuly = await _queryRepo1.ExecuteQueryList<SuccessionPlaningViewModel>(query, null);
			return resuly;
		}

		public async Task<SuccessionPlanningAssessmentViewModel> GetAssessmenSetByDateuserid(string UserId,DateTime date)
		{

			var query = $@" select A.* FROM Public.""NtsService"" NS inner join  

	public.""NtsNote"" N on N.""Id""=NS.""UdfNoteId""  and N.""CompanyId""='{_repo.UserContext.CompanyId}' and N.""IsDeleted"" = false
	inner join cms.""N_TAS_UserAssessmentSet"" s on  N.""Id"" = s.""NtsNoteId"" and s.""CompanyId""='{_repo.UserContext.CompanyId}' and s.""IsDeleted"" = false
	inner join  cms.""N_TAS_AssessmentSet"" A on A.""Id"" = s.""AssessmentSetId"" and A.""IsDeleted"" = false and A.""CompanyId""='{_repo.UserContext.CompanyId}'

	inner join public.""LOV"" as LOV on NS.""ServiceStatusId""=LOV.""Id"" and LOV.""CompanyId""='{_repo.UserContext.CompanyId}' and LOV.""IsDeleted"" = false
	where s.""UserId""='{UserId}' and s.""CreatedDate""::TIMESTAMP::Date= '{date}'::TIMESTAMP::Date and N.""CompanyId""='{_repo.UserContext.CompanyId}' and N.""IsDeleted"" = false";


			var resuly = await _queryRepo1.ExecuteQuerySingle<SuccessionPlanningAssessmentViewModel>(query, null);
			return resuly;
		}



		public async Task<SuccessionPlanningAssessmentViewModel> GetAssessmentByDateuserid(string UserId, DateTime date)
		{

			var query = $@" select A.* FROM Public.""NtsService"" NS inner join  

	public.""NtsNote"" N on N.""Id""=NS.""UdfNoteId"" and N.""CompanyId""='{_repo.UserContext.CompanyId}' and N.""IsDeleted"" = false 
	inner join cms.""N_TAS_UserAssessmentSchedule"" s on  N.""Id"" = s.""NtsNoteId"" and s.""CompanyId""='{_repo.UserContext.CompanyId}' and s.""IsDeleted"" = false
	inner join  cms.""N_TAS_Assessment"" A on A.""Id"" = s.""AssessmentId"" and A.""IsDeleted"" = false and A.""CompanyId""='{_repo.UserContext.CompanyId}' 

	inner join public.""LOV"" as LOV on NS.""ServiceStatusId""=LOV.""Id"" and LOV.""CompanyId""='{_repo.UserContext.CompanyId}' and LOV.""IsDeleted"" = false
	where s.""UserId""='{UserId}' and s.""StartTime""::TIMESTAMP::Date= '{date}'::TIMESTAMP::Date and NS.""CompanyId""='{_repo.UserContext.CompanyId}' and NS.""IsDeleted"" = false";


			var resuly = await _queryAssessment.ExecuteQuerySingle<SuccessionPlanningAssessmentViewModel>(query, null);
			return resuly;
		}





		public async Task<List<CompetencyFeedbackUserViewModel>> GetTopFeedbackUser(string Subordinateid)
		{

			var query = $@"SELECT U.""Id"",U.""Name"",U.""PhotoId"" as PhotoName, 
ROUND(Sum(cast(RLOV.""Name"" as int)) / (Count(*) * 5)::numeric, 2) as Rating
from public.""NtsTask"" t inner join public.""NtsNote"" N on N.""Id""=t.""UdfNoteId"" and N.""CompanyId""='{_repo.UserContext.CompanyId}' and N.""IsDeleted"" = false
inner join  cms.""N_TAS Task_CompetencyFeedback"" F on N.""Id""=F.""NtsNoteId"" and F.""IsDeleted""=false and F.""CompanyId""='{_repo.UserContext.CompanyId}'
inner join public.""LOV"" RLOV on RLOV.""Id""=F.""Rating"" and RLOV.""IsDeleted""=false and RLOV.""CompanyId""='{_repo.UserContext.CompanyId}' 
inner join public.""User"" U on U.""Id""=t.""AssignedToUserId"" and U.""IsDeleted""=false and U.""CompanyId""='{_repo.UserContext.CompanyId}'
inner join public.""LOV"" LOVS on LOVS.""Id""=t.""TaskStatusId"" and LOVS.""Code""='TASK_STATUS_COMPLETE' and LOVS.""CompanyId""='{_repo.UserContext.CompanyId}' and LOVS.""IsDeleted"" = false
where F.""SubordinateId""='{Subordinateid}' and t.""IsDeleted""=false and t.""CompanyId""='{_repo.UserContext.CompanyId}'  group by  U.""Id"", U.""Name"", U.""PhotoId"" order by Rating desc limit 5";
			var resuly = await _queryAssessment.ExecuteQueryList<CompetencyFeedbackUserViewModel>(query, null);
			return resuly;
		}

		public async Task<List<CompetencyFeedbackUserViewModel>> GetCompetencyTopName(string Subordinateid)
		{

			var query = $@"SELECT c.""CompetencyName"" , ROUND(Sum(cast(RLOV.""Name"" as int))/(Count(*)*5)*100::numeric,2) as Rating
from public.""NtsTask"" t inner join public.""NtsNote"" N on N.""Id""=t.""UdfNoteId"" and N.""CompanyId""='{_repo.UserContext.CompanyId}' and N.""IsDeleted"" = false
inner join  cms.""N_TAS Task_CompetencyFeedback"" F on N.""Id""=F.""NtsNoteId"" and F.""IsDeleted""=false and F.""CompanyId""='{_repo.UserContext.CompanyId}' 
inner join public.""LOV"" RLOV on RLOV.""Id""=F.""Rating"" and RLOV.""IsDeleted""=false and RLOV.""CompanyId""='{_repo.UserContext.CompanyId}'
inner join cms.""N_PerformanceDocument_CompetencyMaster"" C on C.""Id""=F.""CompetencyId"" and C.""IsDeleted""=false and C.""CompanyId""='{_repo.UserContext.CompanyId}' 
inner join public.""LOV"" LOVS on LOVS.""Id""=t.""TaskStatusId"" and LOVS.""Code""='TASK_STATUS_COMPLETE' and LOVS.""CompanyId""='{_repo.UserContext.CompanyId}' 
where F.""SubordinateId""='{Subordinateid}' and t.""IsDeleted""=false and t.""CompanyId""='{_repo.UserContext.CompanyId}' and F.""Rating""!=null  group by  c.""CompetencyName"" having ROUND(Sum(cast(RLOV.""Name"" as int))/(Count(*)*5)*100::numeric,2) >=50 order by Rating desc limit 5";
			var resuly = await _queryAssessment.ExecuteQueryList<CompetencyFeedbackUserViewModel>(query, null);
			return resuly;
		}

		public async Task<List<CompetencyFeedbackUserViewModel>> GetAreDevelopmentCompetencyTopName(string Subordinateid)
		{

			var query = $@"SELECT c.""CompetencyName"" , ROUND(Sum(cast(RLOV.""Name"" as int))/(Count(*)*5)*100::numeric,2) as Rating
from public.""NtsTask"" t inner join public.""NtsNote"" N on N.""Id""=t.""UdfNoteId"" and N.""CompanyId""='{_repo.UserContext.CompanyId}' and N.""IsDeleted"" = false
inner join  cms.""N_TAS Task_CompetencyFeedback"" F on N.""Id""=F.""NtsNoteId"" and F.""IsDeleted""=false and F.""CompanyId""='{_repo.UserContext.CompanyId}'
inner join public.""LOV"" RLOV on RLOV.""Id""=F.""Rating"" and RLOV.""IsDeleted""=false and RLOV.""CompanyId""='{_repo.UserContext.CompanyId}' 
inner join cms.""N_PerformanceDocument_CompetencyMaster"" C on C.""Id""=F.""CompetencyId"" and C.""IsDeleted""=false and C.""CompanyId""='{_repo.UserContext.CompanyId}' 
inner join public.""LOV"" LOVS on LOVS.""Id""=t.""TaskStatusId"" and LOVS.""Code""='TASK_STATUS_COMPLETE' and LOVS.""CompanyId""='{_repo.UserContext.CompanyId}' and LOVS.""IsDeleted"" = false
where F.""SubordinateId""='{Subordinateid}' and t.""CompanyId""='{_repo.UserContext.CompanyId}' and t.""IsDeleted"" = false group by  c.""CompetencyName"" having ROUND(Sum(cast(RLOV.""Name"" as int))/(Count(*)*5)*100::numeric,2) <=40 order by Rating desc limit 5";
			var resuly = await _queryAssessment.ExecuteQueryList<CompetencyFeedbackUserViewModel>(query, null);
			return resuly;
		}


		public async Task<List<CompetencyFeedbackUserViewModel>> GetChartList(string Subordinateid)
		{

			var query = $@"select c.""CompetencyName"", T.""Rating"" as SelfCount,O.""OtherRating"" as ""OhersCount"" from cms.""N_PerformanceDocument_CompetencyMaster"" C
inner join cms.""N_TAS Task_CompetencyFeedback"" F on C.""Id"" = F.""CompetencyId""
left join(SELECT c.""CompetencyName"" ,c.""Id"", ROUND(Sum(cast(RLOV.""Name"" as int)) / (Count(*) * 5)::numeric, 2) as ""Rating""
from public.""NtsTask"" t inner join public.""NtsNote"" N on N.""Id""=t.""UdfNoteId"" and N.""CompanyId""='{_repo.UserContext.CompanyId}' and N.""IsDeleted"" = false 
inner join  cms.""N_TAS Task_CompetencyFeedback"" F on N.""Id""=F.""NtsNoteId"" and F.""IsDeleted""=false and F.""CompanyId""='{_repo.UserContext.CompanyId}' 
inner join public.""LOV"" RLOV on RLOV.""Id""=F.""Rating"" and RLOV.""CompanyId""='{_repo.UserContext.CompanyId}' and RLOV.""IsDeleted"" = false
inner join cms.""N_PerformanceDocument_CompetencyMaster"" C on C.""Id""=F.""CompetencyId""  and C.""IsDeleted""=false and C.""CompanyId""='{_repo.UserContext.CompanyId}' 
where F.""SubordinateId""='{Subordinateid}' and F.""Rating""!='' and t.""OwnerUserId""=t.""AssignedToUserId"" and t.""IsDeleted""=false  and t.""CompanyId""='{_repo.UserContext.CompanyId}' 
group by  c.""CompetencyName"", C.""Id""  ) T on t.""Id""=C.""Id""
left join(SELECT c.""CompetencyName"" , c.""Id"", ROUND(Sum(cast(RLOV.""Name"" as int))/(Count(*)*5)::numeric,2) as ""OtherRating""
from public.""NtsTask"" t inner join public.""NtsNote"" N on N.""Id""=t.""UdfNoteId"" and N.""CompanyId""='{_repo.UserContext.CompanyId}' and N.""IsDeleted"" = false
inner join  cms.""N_TAS Task_CompetencyFeedback"" F on N.""Id""=F.""NtsNoteId""  and F.""IsDeleted""=false and F.""CompanyId""='{_repo.UserContext.CompanyId}'
inner join public.""LOV"" RLOV on RLOV.""Id""=F.""Rating"" and RLOV.""CompanyId""='{_repo.UserContext.CompanyId}' and RLOV.""IsDeleted"" = false
inner join cms.""N_PerformanceDocument_CompetencyMaster"" C on C.""Id""=F.""CompetencyId""  and C.""IsDeleted""=false and C.""CompanyId""='{_repo.UserContext.CompanyId}'
where F.""SubordinateId""='{Subordinateid}' and F.""Rating""!='' and t.""OwnerUserId""!=t.""AssignedToUserId""  and t.""IsDeleted""=false  and t.""CompanyId""='{_repo.UserContext.CompanyId}' 
group by  c.""CompetencyName"", C.""Id"" )  O on O.""Id""=C.""Id""
where C.""IsDeleted""=false";
			var resuly = await _queryAssessment.ExecuteQueryList<CompetencyFeedbackUserViewModel>(query, null);
			return resuly;
		}

	}
}
