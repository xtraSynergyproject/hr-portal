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
    public class ListOfValueBusiness : BusinessBase<ListOfValueViewModel, ListOfValue>, IListOfValueBusiness
    {
        private readonly IRepositoryQueryBase<ListOfValueViewModel> _queryRepo;
        public ListOfValueBusiness(IRepositoryBase<ListOfValueViewModel, ListOfValue> repo, IMapper autoMapper,
            IRepositoryQueryBase<ListOfValueViewModel> queryRepo
            ) : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;
        }

        public async override Task<CommandResult<ListOfValueViewModel>> Create(ListOfValueViewModel model, bool autoCommit = true)
        {           
            //var validateName = await IsNameExists(data);
            //if (!validateName.IsSuccess)
            //{
            //    return CommandResult<ListOfValueViewModel>.Instance(model, false, validateName.Messages);
            //}
            var result = await base.Create(model,autoCommit);

            return CommandResult<ListOfValueViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<ListOfValueViewModel>> Edit(ListOfValueViewModel model, bool autoCommit = true)
        {
            //var data = _autoMapper.Map<ListOfValueViewModel>(model);
            //var validateName = await IsNameExists(data);
            //if (!validateName.IsSuccess)
            //{
            //    return CommandResult<ListOfValueViewModel>.Instance(model, false, validateName.Messages);
            //}
            var result = await base.Edit(model,autoCommit);

            return CommandResult<ListOfValueViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        
        private async Task<CommandResult<ListOfValueViewModel>> IsNameExists(ListOfValueViewModel model)
        {
                        
            return CommandResult<ListOfValueViewModel>.Instance();
        }

        public async Task<List<ListOfValueViewModel>> GetTreeList(string id)
        {
            var query = $@"
                     WITH RECURSIVE ListofValue AS(
                 SELECT s.* FROM rec.""ListOfValue"" as s
                     where ""ParentId""='{id}'   and ""IsDeleted""=false
					 
                        union all
             SELECT s.* FROM rec.""ListOfValue"" as s
                        join ListofValue ns on s.""ParentId""=ns.""Id""
                        where s.""IsDeleted""=false
                 )
                 SELECT * from ListofValue
                        ";
            var list = await _queryRepo.ExecuteQueryList(query, null);
            return list;
        }

        public async Task<List<ListOfValueViewModel>> GetListOfValueByParentAndValue(string type, string value)
        {
            var query = $@"select * from  public.""LOV"" as LOV1 where   (LOV1.""Name"" = '{value}' and LOV1.""LOVType"" = '{type}')";
            var list = await _queryRepo.ExecuteQueryList(query, null);
            return list;
        }
    }
}
