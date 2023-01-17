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
    public class PageIndexBusiness : BusinessBase<PageIndexViewModel, PageIndex>, IPageIndexBusiness
    {
        private readonly IPageIndexColumnBusiness _pageIndexColumnBusiness;
        private readonly IPublishPageBusiness _publishPageBusiness;
        private readonly IPublishPageContentBusiness _publishPageContentBusiness;
        private readonly IStyleBusiness _styleBusiness;
        public PageIndexBusiness(IRepositoryBase<PageIndexViewModel, PageIndex> repo, IPublishPageContentBusiness publishPageContentBusiness, IPublishPageBusiness publishPageBusiness, IMapper autoMapper, IPageContentBusiness pageContentBusiness, IStyleBusiness styleBusiness, IPageIndexColumnBusiness pageIndexColumnBusiness) : base(repo, autoMapper)
        {
            _pageIndexColumnBusiness = pageIndexColumnBusiness;
            _styleBusiness = styleBusiness;
            _publishPageBusiness = publishPageBusiness;

            _publishPageContentBusiness = publishPageContentBusiness;
        }

      

        public async override Task<CommandResult<PageIndexViewModel>> Create(PageIndexViewModel model)
        {
            var data = _autoMapper.Map<PageIndexViewModel>(model);
            var result = await base.Create(data);
            if (result.IsSuccess)
            {
                // Create pageIndexColumn
                if (model.SelectedTableRows!=null && model.SelectedTableRows.Count()>0) 
                {
                    foreach (var row in model.SelectedTableRows)
                    {
                       await  _pageIndexColumnBusiness.Create(row);
                    }
                }
                
            }
            return CommandResult<PageIndexViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<PageIndexViewModel>> Edit(PageIndexViewModel model)
        {
            var data = _autoMapper.Map<PageIndexViewModel>(model);
           
       
            var result = await base.Edit(data);
            if (result.IsSuccess)
            {
               
            }
            return CommandResult<PageIndexViewModel>.Instance(model);
        }
       

        

      
    }
}
