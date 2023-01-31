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
    public class StyleBusiness : BusinessBase<StyleViewModel, Style>, IStyleBusiness
    {
        
        public StyleBusiness(IRepositoryBase<StyleViewModel, Style> repo, IMapper autoMapper) : base(repo, autoMapper)
        {


        }

        public async override Task<CommandResult<StyleViewModel>> Create(StyleViewModel model)
        {

            var data = _autoMapper.Map<StyleViewModel>(model);
            var result = await base.Create(data);
            if (result.IsSuccess)
            {
                if (model.SourceType == PageContentTypeEnum.Page)
                {
                    //var page = await _pageBusiness.GetSingleById(model.PageId);
                    var page = await _repo.GetSingleById<PageViewModel, Page>(model.PageId);
                    if (page != null)
                    {
                        page.Style = result.Item.ToStyleText();
                        var pageresult = await _repo.Edit<PageViewModel, Page>(page);
                    }
                }
                else
                {
                    //var content = await _pageContentBusiness.GetSingleById(model.PageId);
                    var content = await _repo.GetSingleById<PageContentViewModel, PageContent>(model.SourceId);
                    if (content != null)
                    {
                        content.Style = result.Item.ToStyleText();
                        var contentresult = await _repo.Edit<PageContentViewModel, PageContent>(content);
                    }
                }
            }

            return CommandResult<StyleViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<StyleViewModel>> Edit(StyleViewModel model)
        {

            var data = _autoMapper.Map<StyleViewModel>(model);
            var result = await base.Edit(data);
            if (result.IsSuccess)
            {
                if (model.SourceType == PageContentTypeEnum.Page)
                {
                    //var page = await _pageBusiness.GetSingleById(model.PageId);
                    var page = await _repo.GetSingleById<PageViewModel, Page>(model.PageId);
                    if (page != null)
                    {
                        page.Style = result.Item.ToStyleText(); 
                        var pageresult = await _repo.Edit<PageViewModel, Page>(page);
                    }
                }
                else
                {
                    //var content = await _pageContentBusiness.GetSingleById(model.PageId);
                    var content = await _repo.GetSingleById<PageContentViewModel, PageContent>(model.SourceId);
                    if (content != null)
                    {
                        content.Style = result.Item.ToStyleText();
                        var contentresult = await _repo.Edit<PageContentViewModel, PageContent>(content);
                    }
                }
            }

            return CommandResult<StyleViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async Task<StyleViewModel> GetStyleBySourceId(string sourceId, PageContentTypeEnum sourceType)
        {
            var result = await GetSingle(x => x.SourceId == sourceId);
            if (result == null)
            {
                switch (sourceType)
                {
                    case PageContentTypeEnum.Group:
                        return GetGroupDefaultStyle();
                    case PageContentTypeEnum.Column:
                        return GetColumnDefaultStyle();
                    case PageContentTypeEnum.Cell:
                        return GetCellDefaultStyle();
                    case PageContentTypeEnum.Component:
                        return GetComponentDefaultStyle();
                    case PageContentTypeEnum.Page:
                        return GetPageDefaultStyle();
                    default:
                        return GetDefaultStyle();
                }
            }
            result.DataAction = DataActionEnum.Edit;
            return result;
        }

        private StyleViewModel GetDefaultStyle()
        {
            return new StyleViewModel
            {
                DataAction = DataActionEnum.Create,
                PaddingDefault = "10px",
                BackgroundColor = "transparent",
                BorderColorDefault = "silver",
                BorderStyleDefault = "solid",
                BorderWidthDefault = "1px",
                BorderRadiusDefault = "3px"

            };
        }

        private StyleViewModel GetPageDefaultStyle()
        {
            return GetDefaultStyle();
        }

        private StyleViewModel GetComponentDefaultStyle()
        {
            return GetDefaultStyle();
        }

        private StyleViewModel GetCellDefaultStyle()
        {
            return GetDefaultStyle();
        }

        private StyleViewModel GetColumnDefaultStyle()
        {
            return GetDefaultStyle();
        }

        private StyleViewModel GetGroupDefaultStyle()
        {
            return GetDefaultStyle();
        }

        public async Task<StyleViewModel> GetStyleBySourceId(string sourceId)
        {
            return await GetSingle(x => x.SourceId == sourceId);
        }
    }
}
