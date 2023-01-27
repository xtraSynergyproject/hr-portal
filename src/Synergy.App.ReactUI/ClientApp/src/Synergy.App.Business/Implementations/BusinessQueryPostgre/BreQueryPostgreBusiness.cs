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
    public class BreQueryPostgreBusiness : BusinessBase<NoteViewModel, NtsNote>, IBreQueryBusiness
    {
        IUserContext _uc;
        private readonly IRepositoryQueryBase<NoteViewModel> _queryRepo;
        public BreQueryPostgreBusiness(IRepositoryBase<NoteViewModel, NtsNote> repo
            , IMapper autoMapper
            , IUserContext uc
            , IRepositoryQueryBase<NoteViewModel> queryRepo) : base(repo, autoMapper)
        {
            _uc = uc;
            _queryRepo = queryRepo;
        }

        public async Task<List<BusinessRuleViewModel>> GetBusinessRuleActionListData(string templateId, int actionType)
        {
            var query = @$"select br.""Id"",br.""Name"", lov.""Name"" as ActionName from public.""BusinessRule"" as br
                        join public.""LOV"" as lov on lov.""Id"" = br.""ActionId""
                        where br.""TemplateId""='{templateId}' and br.""BusinessLogicExecutionType""='{actionType}' and br.""IsDeleted""=false";

            var queryData = await _queryRepo.ExecuteQueryList<BusinessRuleViewModel>(query, null);
            return queryData;

        }

    }
}
