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
    public class NtsGroupTemplateBusiness : BusinessBase<NtsGroupTemplateViewModel, NtsGroupTemplate>, INtsGroupTemplateBusiness
    {

        public NtsGroupTemplateBusiness(IRepositoryBase<NtsGroupTemplateViewModel, NtsGroupTemplate> repo, IMapper autoMapper) : base(repo, autoMapper)
        {


        }

        public async override Task<CommandResult<NtsGroupTemplateViewModel>> Create(NtsGroupTemplateViewModel model)
        {

            // var data = _autoMapper.Map<UserRoleViewModel>(model);

            var result = await base.Create(model);
            return CommandResult<NtsGroupTemplateViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<NtsGroupTemplateViewModel>> Edit(NtsGroupTemplateViewModel model)
        {
            var result = await base.Edit(model);
            return CommandResult<NtsGroupTemplateViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }


    }
}
