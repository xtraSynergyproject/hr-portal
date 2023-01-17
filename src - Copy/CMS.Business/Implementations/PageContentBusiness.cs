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
    public class PageContentBusiness : BusinessBase<PageContentViewModel,PageContent>, IPageContentBusiness
    {
        private readonly IStyleBusiness _styleBusiness;
        public PageContentBusiness(IRepositoryBase<PageContentViewModel, PageContent> repo, IMapper autoMapper, IStyleBusiness styleBusiness) : base(repo, autoMapper)
        {
            _styleBusiness = styleBusiness;
         }
        public async override Task<CommandResult<PageContentViewModel>> Create(PageContentViewModel model)
        {
            //var data = _autoMapper.Map<PageContentViewModel>(model);
            //var style = await _styleBusiness.GetStyleBySourceId(model.Id);
            //if (style != null)
            //{
            //    model.Style = BusinessExtension.ToStyleText(style);
            //}
            var result = await base.Create(model);
            return CommandResult<PageContentViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<PageContentViewModel>> Edit(PageContentViewModel model)
        {
            //var data = _autoMapper.Map<PageContentViewModel>(model);
            //var style = await _styleBusiness.GetStyleBySourceId(model.Id);
            //if (style != null)
            //{
            //    data.Style = BusinessExtension.ToStyleText(style);
            //}
            var result = await base.Edit(model);
            return CommandResult<PageContentViewModel>.Instance(model, result.IsSuccess, result.Messages);
        } 
    }
}
