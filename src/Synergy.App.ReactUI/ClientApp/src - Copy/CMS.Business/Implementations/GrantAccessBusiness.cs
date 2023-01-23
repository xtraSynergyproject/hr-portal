using AutoMapper;
using CMS.Common;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace CMS.Business
{
    public class GrantAccessBusiness : BusinessBase<GrantAccessViewModel, GrantAccess>, IGrantAccessBusiness
    {
        private readonly IRepositoryQueryBase<GrantAccessViewModel> _queryRepo;
        public GrantAccessBusiness(IRepositoryBase<GrantAccessViewModel, GrantAccess> repo,
            IRepositoryQueryBase<GrantAccessViewModel> queryRepo,
            IMapper autoMapper) : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;
        }

        public async override Task<CommandResult<GrantAccessViewModel>> Create(GrantAccessViewModel model)
        {

            var data = _autoMapper.Map<GrantAccessViewModel>(model);           
            var result = await base.Create(data);

            return CommandResult<GrantAccessViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<GrantAccessViewModel>> Edit(GrantAccessViewModel model)
        {

            var data = _autoMapper.Map<GrantAccessViewModel>(model);           
            var result = await base.Edit(data);

            return CommandResult<GrantAccessViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        public async Task<IList<GrantAccessViewModel>> GetGrantAccessList(string userId)
        {
            string query = @$"SELECT ga.*,gu.""Name"" as UserName
                            FROM public.""GrantAccess"" as ga                            
                            inner join public.""User"" as gu on gu.""Id""=ga.""UserId"" and gu.""IsDeleted""=false
                            inner join public.""User"" as u on u.""Id""=ga.""CreatedBy"" and u.""IsDeleted""=false
                            where u.""Id""='{userId}' and ga.""IsDeleted""=false";
            var list = await _queryRepo.ExecuteQueryList(query, null);
            return list;
        }

    }
}
