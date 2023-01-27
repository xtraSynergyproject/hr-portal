using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace Synergy.App.Business
{
    public class CaseManagementBusiness : BusinessBase<ServiceViewModel, NtsService>, ICaseManagementBusiness
    {
        private readonly IRepositoryQueryBase<ServiceViewModel> _queryRepo;
        private readonly ICmsQueryBusiness _cmsQueryBusiness;
        public CaseManagementBusiness(IRepositoryBase<ServiceViewModel, NtsService> repo, IMapper autoMapper,
            IRepositoryQueryBase<ServiceViewModel> queryRepo, ICmsQueryBusiness cmsQueryBusiness)
            : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;
            _cmsQueryBusiness = cmsQueryBusiness;
        }

        public async Task<List<PositionChartIndexViewModel>> GetCMDBHierarchyData(string parentId, int levelUpto)
        {
            var queryData = await _cmsQueryBusiness.GetCMDBHierarchyData(parentId, levelUpto);
            var list = queryData;
            return list;
        }
    }
}
