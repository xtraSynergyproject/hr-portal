using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using System.Linq;
using System.Threading.Tasks;


namespace Synergy.App.Business
{
    public class ServiceIndexPageTemplateBusiness : BusinessBase<ServiceIndexPageTemplateViewModel, ServiceIndexPageTemplate>, IServiceIndexPageTemplateBusiness
    {
        private IServiceIndexPageColumnBusiness _indexPageColumnBusiness;
        public ServiceIndexPageTemplateBusiness(IRepositoryBase<ServiceIndexPageTemplateViewModel, ServiceIndexPageTemplate> repo, IServiceIndexPageColumnBusiness indexPageColumnBusiness, IMapper autoMapper) : base(repo, autoMapper)
        {
            _indexPageColumnBusiness = indexPageColumnBusiness;
        }

        public async override Task<CommandResult<ServiceIndexPageTemplateViewModel>> Create(ServiceIndexPageTemplateViewModel model, bool autoCommit = true)
        {          
            var result = await base.Create(model,autoCommit);
            if (result.IsSuccess)
            {
                var indexPageTemplateId = result.Item.Id;
                var serviceTemplate = await _repo.GetSingle<ServiceTemplateViewModel, ServiceTemplate>(x => x.TemplateId == model.TemplateId);
                if (serviceTemplate != null)
                {
                    serviceTemplate.ServiceIndexPageTemplateId = indexPageTemplateId;
                    serviceTemplate.EnableIndexPage = true;
                    await _repo.Edit<ServiceTemplateViewModel, ServiceTemplate>(serviceTemplate);
                }
                //foreach (var col in model.SelectedTableRows)
                //{
                //    col.Id = null;
                //    col.ServiceIndexPageTemplateId = indexPageTemplateId;
                //    await _indexPageColumnBusiness.Create(col);
                //}
            }
            return CommandResult<ServiceIndexPageTemplateViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<ServiceIndexPageTemplateViewModel>> Edit(ServiceIndexPageTemplateViewModel model, bool autoCommit = true)
        {
            //var data = _autoMapper.Map<IndexPageTemplateViewModel>(model);
            var result = await base.Edit(model,autoCommit);
            if (result.IsSuccess)
            {
                var pagecol = await _indexPageColumnBusiness.GetList(x => x.ServiceIndexPageTemplateId == model.Id);
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
                        col1.ServiceIndexPageTemplateId = model.Id;
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
            return CommandResult<ServiceIndexPageTemplateViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }


    }
}
