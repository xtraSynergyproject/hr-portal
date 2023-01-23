using AutoMapper;
using CMS.Common;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace CMS.Business
{
    public class ApplicationErrorBusiness : BusinessBase<ApplicationErrorViewModel, ApplicationError>, IApplicationErrorBusiness
    {
        IRepositoryQueryBase<ApplicationErrorViewModel> _repoQuery;
        public ApplicationErrorBusiness(IRepositoryBase<ApplicationErrorViewModel, ApplicationError> repo, IMapper autoMapper
            , IRepositoryQueryBase<ApplicationErrorViewModel> repoQuery) : base(repo, autoMapper)
        {
            _repoQuery = repoQuery;
        }



        public async Task<List<ApplicationErrorViewModel>> GetApplicationErrorList()
        {
            var query = @$"select ad.*,f.""FileName"" as DocumentFileName
            from public.""ApplicationDocument"" as ad
            left join public.""File"" as f on ad.""DocumentId""=f.""Id"" and f.""IsDeleted""=false            
            where ad.""IsDeleted""=false";
            return await _repoQuery.ExecuteQueryList<ApplicationErrorViewModel>(query, null);
        }

    }
}
