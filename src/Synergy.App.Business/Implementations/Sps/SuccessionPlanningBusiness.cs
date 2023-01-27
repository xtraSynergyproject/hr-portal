﻿
using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
   public class SuccessionPlanningBusiness: BusinessBase<NoteViewModel, NtsNote>, ISuccessionPlanningBusiness
    {
        private readonly IRepositoryQueryBase<SuccessionPlaningViewModel> _queryRepo1;
		private readonly IRepositoryQueryBase<SuccessionPlanningAssessmentViewModel> _queryAssessment;
		private readonly IRepositoryQueryBase<CompetencyFeedbackUserViewModel> _CompetencyFeedback;
		private readonly ISuccessionPlanningQueryBusiness _successionPlanningQueryBusiness;


		public SuccessionPlanningBusiness(IRepositoryBase<NoteViewModel, NtsNote> repo, IMapper autoMapper, IRepositoryQueryBase<SuccessionPlaningViewModel> queryRepo1,
			 IRepositoryQueryBase<SuccessionPlanningAssessmentViewModel> queryAssessment,
			 ISuccessionPlanningQueryBusiness successionPlanningQueryBusiness,
			 IRepositoryQueryBase<CompetencyFeedbackUserViewModel> CompetencyFeedback) : base(repo, autoMapper)
        {
			_queryRepo1 = queryRepo1;
			_queryAssessment = queryAssessment;
			_CompetencyFeedback = CompetencyFeedback;
			_successionPlanningQueryBusiness= successionPlanningQueryBusiness;


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


			var result = await _successionPlanningQueryBusiness.GetSuccessionPlanings(Module, Employee, Department, Month, year);
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

			var resuly = await _successionPlanningQueryBusiness.GetAssessmentListofUser(Month, Year);
			return resuly;
		}

		public async Task<List<SuccessionPlaningViewModel>> GetAssessmenSettListofUser(int Month, int Year)
		{

			var resuly = await _successionPlanningQueryBusiness.GetAssessmenSettListofUser(Month, Year);
			return resuly;
		}

		public async Task<SuccessionPlanningAssessmentViewModel> GetAssessmenSetByDateuserid(string UserId,DateTime date)
		{

			var resuly = await _successionPlanningQueryBusiness.GetAssessmenSetByDateuserid(UserId, date);
			return resuly;
		}



		public async Task<SuccessionPlanningAssessmentViewModel> GetAssessmentByDateuserid(string UserId, DateTime date)
		{
			var resuly = await _successionPlanningQueryBusiness.GetAssessmentByDateuserid(UserId, date);
			return resuly;
		}


		public async Task<List<CompetencyFeedbackUserViewModel>> GetTopFeedbackUser(string Subordinateid)
		{
			var resuly = await _successionPlanningQueryBusiness.GetTopFeedbackUser(Subordinateid);
			return resuly;
		}

		public async Task<List<CompetencyFeedbackUserViewModel>> GetCompetencyTopName(string Subordinateid)
		{
			var resuly = await _successionPlanningQueryBusiness.GetCompetencyTopName(Subordinateid);
			return resuly;
		}

		public async Task<List<CompetencyFeedbackUserViewModel>> GetAreDevelopmentCompetencyTopName(string Subordinateid)
		{
			var resuly = await _successionPlanningQueryBusiness.GetAreDevelopmentCompetencyTopName(Subordinateid);
			return resuly;
		}


		public async Task<List<CompetencyFeedbackUserViewModel>> GetChartList(string Subordinateid)
		{
			var resuly = await _successionPlanningQueryBusiness.GetChartList(Subordinateid);
			return resuly;
		}

	}
}