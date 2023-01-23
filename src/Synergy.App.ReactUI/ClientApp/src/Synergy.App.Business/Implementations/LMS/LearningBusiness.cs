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
	public class LearningBusiness : BusinessBase<NoteViewModel, NtsNote>, ILearningBusiness
	{



		private readonly IRepositoryQueryBase<LearningPlanViewModel> _queryRepo;
		private IUserContext _userContext;
		private IServiceBusiness _serviceBusiness;
		private INoteBusiness _noteBusiness;
		private readonly IServiceProvider _sp;
		private readonly ILearningManagementQueryBusiness _learningManagementQueryBusiness;

		public LearningBusiness(IRepositoryBase<NoteViewModel, NtsNote> repo, IMapper autoMapper
			, IServiceProvider sp,
			IUserContext userContext, IServiceBusiness serviceBusiness, INoteBusiness noteBusiness,
			ILearningManagementQueryBusiness learningManagementQueryBusiness,
			IRepositoryQueryBase<LearningPlanViewModel> queryRepo) : base(repo, autoMapper)
		{

			_sp = sp;
			_userContext = userContext;
			_serviceBusiness = serviceBusiness;
			_noteBusiness = noteBusiness;
			_queryRepo = queryRepo;
			_learningManagementQueryBusiness = learningManagementQueryBusiness;

		}

		public async Task<List<LearningPlanViewModel>> GetLearningPlanData()
		{
			return await _learningManagementQueryBusiness.GetLearningPlanData();

		}
		public async Task<List<LearningPlanViewModel>> GetCasteDetails(string casteId)
		{
			return await _learningManagementQueryBusiness.GetCasteDetails(casteId);
		}
	}
}
