using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Synergy.App.Business
{
    public class JobDescriptionBusiness : BusinessBase<JobDescriptionViewModel, JobDescription>, IJobDescriptionBusiness
    {
        IUserBusiness _userBusiness;
        private readonly IRepositoryQueryBase<JobDescriptionViewModel> _queryRepo;
        private readonly IRepositoryQueryBase<JobDescriptionCriteriaViewModel> _queryRepoCri;
        public JobDescriptionBusiness(IRepositoryBase<JobDescriptionViewModel, JobDescription> repo, IMapper autoMapper, IUserBusiness userBusiness,
            IRepositoryQueryBase<JobDescriptionViewModel> queryRepo
            , IRepositoryQueryBase<JobDescriptionCriteriaViewModel> queryRepoCri) : base(repo, autoMapper)
        {
            _userBusiness = userBusiness;
            _queryRepo = queryRepo;
            _queryRepoCri = queryRepoCri;

        }

        public async override Task<CommandResult<JobDescriptionViewModel>> Create(JobDescriptionViewModel model, bool autoCommit = true)
        {
            var data = _autoMapper.Map<JobDescriptionViewModel>(model);

            data.Description = HttpUtility.HtmlDecode(data.Description);
            data.Responsibilities = HttpUtility.HtmlDecode(data.Responsibilities);

            var result = await base.Create(data, autoCommit);

            return CommandResult<JobDescriptionViewModel>.Instance(data, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<JobDescriptionViewModel>> Edit(JobDescriptionViewModel model, bool autoCommit = true)
        {
            var data = _autoMapper.Map<JobDescriptionViewModel>(model);

            data.Description = HttpUtility.HtmlDecode(data.Description);
            data.Responsibilities = HttpUtility.HtmlDecode(data.Responsibilities);

            var result = await base.Edit(data,autoCommit);

            return CommandResult<JobDescriptionViewModel>.Instance(data, result.IsSuccess, result.Messages);
        }

        private async Task<CommandResult<JobDescriptionViewModel>> IsNameExists(JobDescriptionViewModel model)
        {
            return CommandResult<JobDescriptionViewModel>.Instance();
        }

        public async Task<IList<JobDescriptionCriteriaViewModel>> GetJobDescCriteriaList(string type, string jobdescid)
        {
            string query = @$"Select jc.*,case when lov.""Name"" is null then '' else lov.""Name"" end  as CriteriaTypeName,
case when lovother.""Name"" is null then '' else lovother.""Name"" end  as LovTypeName
from rec.""JobDescriptionCriteria"" as jc
left join rec.""ListOfValue"" as lov on lov.""Id"" = jc.""CriteriaType""
left join rec.""ListOfValue"" as lovother on lovother.""Id"" = jc.""ListOfValueTypeId""
where jc.""Type"" = '{type}' and jc.""JobDescriptionId"" = '{jobdescid}' and jc.""IsDeleted""=false ";
            var queryData = await _queryRepoCri.ExecuteQueryList(query, null);
            return queryData;
        }
    }
}
