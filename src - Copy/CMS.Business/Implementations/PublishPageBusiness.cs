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
    public class PublishPageBusiness : BusinessBase<PublishPageViewModel, PagePublished>, IPublishPageBusiness
    {
        private readonly IStyleBusiness _styleBusiness;
        public PublishPageBusiness(IRepositoryBase<PublishPageViewModel, PagePublished> repo, IMapper autoMapper, IStyleBusiness styleBusiness) : base(repo, autoMapper)
        {
            _styleBusiness = styleBusiness;
         }
        public async override Task<CommandResult<PublishPageViewModel>> Create(PublishPageViewModel model)
        {
            var data = _autoMapper.Map<PublishPageViewModel>(model);           
            var result = await base.Create(data);
            return CommandResult<PublishPageViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<PublishPageViewModel>> Edit(PublishPageViewModel model)
        {
            var data = _autoMapper.Map<PublishPageViewModel>(model);            
            var result = await base.Edit(data);
            return CommandResult<PublishPageViewModel>.Instance(model, result.IsSuccess, result.Messages);
        } 
    }
}
