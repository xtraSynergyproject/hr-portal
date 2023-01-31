using AutoMapper;
using CMS.Business.Interface;
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
    public class NtsGroupUserGroupBusiness : BusinessBase<NtsGroupUserGroupViewModel, NtsGroupUserGroup>, INtsGroupUserGroupBusiness
    {

        public NtsGroupUserGroupBusiness(IRepositoryBase<NtsGroupUserGroupViewModel, NtsGroupUserGroup> repo, IMapper autoMapper) : base(repo, autoMapper)
        {


        }

        public async override Task<CommandResult<NtsGroupUserGroupViewModel>> Create(NtsGroupUserGroupViewModel model)
        {

            // var data = _autoMapper.Map<UserRoleViewModel>(model);

            var result = await base.Create(model);
            return CommandResult<NtsGroupUserGroupViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<NtsGroupUserGroupViewModel>> Edit(NtsGroupUserGroupViewModel model)
        {
            var result = await base.Edit(model);
            return CommandResult<NtsGroupUserGroupViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }


    }
}
