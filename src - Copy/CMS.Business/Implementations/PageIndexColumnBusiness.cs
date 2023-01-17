using AutoMapper;
using CMS.Common;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace CMS.Business
{
    public class PageIndexColumnBusiness : BusinessBase<PageIndexColumnViewModel, PageIndexColumn>, IPageIndexColumnBusiness
    {
        private readonly IPageContentBusiness _pageContentBusiness;
        private readonly IPublishPageBusiness _publishPageBusiness;
        private readonly IPublishPageContentBusiness _publishPageContentBusiness;
        private readonly IStyleBusiness _styleBusiness;
        public PageIndexColumnBusiness(IRepositoryBase<PageIndexColumnViewModel, PageIndexColumn> repo, IPublishPageContentBusiness publishPageContentBusiness, IPublishPageBusiness publishPageBusiness, IMapper autoMapper, IPageContentBusiness pageContentBusiness, IStyleBusiness styleBusiness) : base(repo, autoMapper)
        {
            _pageContentBusiness = pageContentBusiness;
            _styleBusiness = styleBusiness;
            _publishPageBusiness = publishPageBusiness;

            _publishPageContentBusiness = publishPageContentBusiness;
        }

      

        public async override Task<CommandResult<PageIndexColumnViewModel>> Create(PageIndexColumnViewModel model)
        {
            var data = _autoMapper.Map<PageIndexColumnViewModel>(model);
            var result = await base.Create(data);
            if (result.IsSuccess)
            {
               
            }
            return CommandResult<PageIndexColumnViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<PageIndexColumnViewModel>> Edit(PageIndexColumnViewModel model)
        {
            var data = _autoMapper.Map<PageIndexColumnViewModel>(model);
           
       
            var result = await base.Edit(data);
            if (result.IsSuccess)
            {
               
            }
            return CommandResult<PageIndexColumnViewModel>.Instance(model);
        }
        public async Task SavePageContent(List<PageContentViewModel> model)
        {
            foreach (var content in model)
            {
                if (content.DataAction == DataActionEnum.Create)
                {
                    var result1 = await _pageContentBusiness.Create(content);
                }
                else if (content.DataAction == DataActionEnum.Edit)
                {
                    var contentdata = await _pageContentBusiness.GetSingleById(content.Id);
                    if (contentdata != null)
                    {
                        contentdata.Title = content.Title;
                        contentdata.PageId = content.PageId;
                        contentdata.ParentId = content.ParentId;
                        contentdata.Content = content.Content;
                        contentdata.Style = content.Style;
                        contentdata.CssClass = content.CssClass;
                        contentdata.PageContentType = content.PageContentType;
                        contentdata.ComponentType = content.ComponentType;
                        contentdata.PageRowType = content.PageRowType;
                        var result1 = await _pageContentBusiness.Edit(contentdata);
                    }
                }
            }
        }


        

      
    }
}
