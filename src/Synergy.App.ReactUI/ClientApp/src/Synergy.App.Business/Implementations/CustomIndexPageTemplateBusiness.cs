using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using System.Linq;
using System.Threading.Tasks;


namespace Synergy.App.Business
{
    public class CustomIndexPageTemplateBusiness : BusinessBase<CustomIndexPageTemplateViewModel, CustomIndexPageTemplate>, ICustomIndexPageTemplateBusiness
    {
        private ICustomIndexPageColumnBusiness _indexPageColumnBusiness;
        public CustomIndexPageTemplateBusiness(IRepositoryBase<CustomIndexPageTemplateViewModel, CustomIndexPageTemplate> repo, ICustomIndexPageColumnBusiness indexPageColumnBusiness, IMapper autoMapper) : base(repo, autoMapper)
        {
            _indexPageColumnBusiness = indexPageColumnBusiness;
        }

        public async override Task<CommandResult<CustomIndexPageTemplateViewModel>> Create(CustomIndexPageTemplateViewModel model, bool autoCommit = true)
        {
            var result = await base.Create(model, autoCommit);
            if (result.IsSuccess)
            {
                var indexPageTemplateId = result.Item.Id;
                //var serviceTemplate = await _repo.GetSingle<CustomTemplateViewModel, CustomTemplate>(x => x.TemplateId == model.TemplateId);
                //if (serviceTemplate != null)
                //{
                //    serviceTemplate.CustomIndexPageTemplateId = indexPageTemplateId;
                //    serviceTemplate.EnableIndexPage = true;
                //    await _repo.Edit<CustomTemplateViewModel, CustomTemplate>(serviceTemplate);
                //}
                foreach (var col in model.SelectedTableRows)
                {
                    col.Id = null;
                    col.CustomIndexPageTemplateId = indexPageTemplateId;
                    await _indexPageColumnBusiness.Create(col);
                }
            }
            return CommandResult<CustomIndexPageTemplateViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<CustomIndexPageTemplateViewModel>> Edit(CustomIndexPageTemplateViewModel model, bool autoCommit = true)
        {
            //var data = _autoMapper.Map<IndexPageTemplateViewModel>(model);
            var result = await base.Edit(model, autoCommit);
            if (result.IsSuccess)
            {
                var pagecol = await _indexPageColumnBusiness.GetList(x => x.CustomIndexPageTemplateId == model.Id);
                var existingIds = pagecol.Select(x => x.Id);
                var newIds = model.SelectedTableRows.Select(x => x.Id);
                var ToDelete = existingIds.Except(newIds).ToList(); //pagecol.Select(x=>x.Id).ToList().Except(model.SelectedTableRows).ToList();
                var ToAdd = newIds.Except(existingIds).ToList();//model.SelectedTableRows.Except(pagecol).ToList();
                var ToEdit = newIds.Intersect(existingIds).ToList();//model.SelectedTableRows.Intersect(pagecol).ToList();
                                                                    //foreach (var col in ToAdd)
                                                                    //{
                foreach (var col1 in model.SelectedTableRows)
                {
                    if (ToAdd.Any(x => x == col1.Id))
                    {
                        col1.CustomIndexPageTemplateId = model.Id;
                        col1.Id = null;
                        await _indexPageColumnBusiness.Create(col1);
                    }

                }
                foreach (var col1 in model.SelectedTableRows)
                {
                    if (ToEdit.Any(x => x == col1.Id))
                    {

                        await _indexPageColumnBusiness.Edit(col1);
                    }
                }
                foreach (var col in ToDelete)
                {
                    await _indexPageColumnBusiness.Delete(col);
                }
            }
            //return CommandResult<IndexPageTemplateViewModel>.Instance(model);
            return CommandResult<CustomIndexPageTemplateViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }


    }
}
