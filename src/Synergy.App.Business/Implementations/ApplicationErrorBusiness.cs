using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Synergy.App.Business
{
    public class ApplicationErrorBusiness : BusinessBase<ApplicationErrorViewModel, ApplicationError>, IApplicationErrorBusiness
    {
        IRepositoryQueryBase<ApplicationErrorViewModel> _repoQuery;
        private readonly ICmsQueryBusiness _cmsQueryBusiness;
        public ApplicationErrorBusiness(IRepositoryBase<ApplicationErrorViewModel, ApplicationError> repo, IMapper autoMapper
            , IRepositoryQueryBase<ApplicationErrorViewModel> repoQuery, ICmsQueryBusiness cmsQueryBusiness) : base(repo, autoMapper)
        {
            _repoQuery = repoQuery;
            _cmsQueryBusiness = cmsQueryBusiness;
        }



        public async Task<List<ApplicationErrorViewModel>> GetApplicationErrorList()
        {

            return await _cmsQueryBusiness.GetApplicationErrorListData();
        }

    }
}
