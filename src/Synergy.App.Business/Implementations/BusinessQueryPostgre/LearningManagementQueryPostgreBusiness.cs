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
    public class LearningManagementQueryPostgreBusiness : BusinessBase<NoteViewModel, NtsNote>, ILearningManagementQueryBusiness
    {



		private readonly IRepositoryQueryBase<LearningPlanViewModel> _queryRepo;
		private IUserContext _userContext;
		private IServiceBusiness _serviceBusiness;
		private INoteBusiness _noteBusiness;
		private readonly IServiceProvider _sp;

		public LearningManagementQueryPostgreBusiness(IRepositoryBase<NoteViewModel, NtsNote> repo, IMapper autoMapper
			,  IServiceProvider sp,
			IUserContext userContext, IServiceBusiness serviceBusiness, INoteBusiness noteBusiness,
			IRepositoryQueryBase<LearningPlanViewModel> queryRepo) : base(repo, autoMapper)
		{

			_sp = sp;
			_userContext = userContext;
			_serviceBusiness = serviceBusiness;
			_noteBusiness = noteBusiness;
			_queryRepo = queryRepo;

		}

		public async Task<List<LearningPlanViewModel>> GetLearningPlanData()
		{

			var query1 = $@"
			SELECT c.""CompetencyName"" as CompetencyName,lp.""LearningStartDate""::date as LearningStartDate,lp.""LearningEndDate""::date as LearningEndDate,
lc.""CourseName"" as CourseName,u.""Name"" as OwnerUserName
FROM cms.""N_LearningPlan_LearningPlan"" as lp
join public.""NtsService"" as nts on nts.""UdfNoteTableId""=lp.""Id"" and  nts.""IsDeleted""=false  and nts.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""User"" as u on u.""Id""=nts.""OwnerUserId""  and u.""IsDeleted""=false  and u.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_PerformanceDocument_CompetencyMaster"" as c on c.""Id""=lp.""CompetencyId"" and c.""IsDeleted""=false and c.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_LearningPlan_Courses"" as lc on lc.""Id""=lp.""CourseId"" and lc.""IsDeleted""=false   and lc.""CompanyId""='{_repo.UserContext.CompanyId}'
       where  lp.""IsDeleted""=false  and lp.""CompanyId""='{_repo.UserContext.CompanyId}'
";
			return await _queryRepo.ExecuteQueryList<LearningPlanViewModel>(query1, null);
		}
		public async Task<List<LearningPlanViewModel>> GetCasteDetails(string casteId)
		{

			var query1 = $@"
			SELECT c.""Id"" as CasteName,cc.""CategoryName"" as Category

FROM cms.""F_CSC_CASTE_CasteMaster"" as c
join cms.""F_CSC_CASTE_CasteCategory"" as cc on c.""CasteCategoryId""=cc.""Id"" and  cc.""IsDeleted""=false 

       where  c.""IsDeleted""=false  and c.""Id""='{casteId}'
";
			return await _queryRepo.ExecuteQueryList<LearningPlanViewModel>(query1, null);
		}
	}
}
