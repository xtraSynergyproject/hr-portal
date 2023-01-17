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
    public class PageContentBusiness : BusinessBase<PageContentViewModel,PageContent>, IPageContentBusiness
    {
        private readonly IStyleBusiness _styleBusiness;
        public PageContentBusiness(IRepositoryBase<PageContentViewModel, PageContent> repo, IMapper autoMapper, IStyleBusiness styleBusiness) : base(repo, autoMapper)
        {
            _styleBusiness = styleBusiness;
         }
        public async override Task<CommandResult<PageContentViewModel>> Create(PageContentViewModel model, bool autoCommit = true)
        {
            //var data = _autoMapper.Map<PageContentViewModel>(model);
            //var style = await _styleBusiness.GetStyleBySourceId(model.Id);
            //if (style != null)
            //{
            //    model.Style = BusinessExtension.ToStyleText(style);
            //}
            var result = await base.Create(model,autoCommit);
            return CommandResult<PageContentViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<PageContentViewModel>> Edit(PageContentViewModel model, bool autoCommit = true)
        {
            //var data = _autoMapper.Map<PageContentViewModel>(model);
            //var style = await _styleBusiness.GetStyleBySourceId(model.Id);
            //if (style != null)
            //{
            //    data.Style = BusinessExtension.ToStyleText(style);
            //}
            var result = await base.Edit(model,autoCommit);
            return CommandResult<PageContentViewModel>.Instance(model, result.IsSuccess, result.Messages);
        } 
    }
}
