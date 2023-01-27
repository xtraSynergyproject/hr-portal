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
    public class MemberGroupBusiness : BusinessBase<MemberGroupViewModel, MemberGroup>, IMemberGroupBusiness
    {
        public MemberGroupBusiness(IRepositoryBase<MemberGroupViewModel, MemberGroup> repo, IMapper autoMapper) : base(repo, autoMapper)
        {

        }

        public async override Task<CommandResult<MemberGroupViewModel>> Create(MemberGroupViewModel model)
        {

            var data = _autoMapper.Map<MemberGroupViewModel>(model);
            var result = await base.Create(data);

            return CommandResult<MemberGroupViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<MemberGroupViewModel>> Edit(MemberGroupViewModel model)
        {

            var data = _autoMapper.Map<MemberGroupViewModel>(model);
            var result = await base.Edit(data);

            return CommandResult<MemberGroupViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

      
    }
}
