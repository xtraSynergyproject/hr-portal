using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using System.Linq;
using System.Threading.Tasks;


namespace Synergy.App.Business
{
    public class NtsLogPageColumnBusiness : BusinessBase<NtsLogPageColumnViewModel, NtsLogPageColumn>, INtsLogPageColumnBusiness
    {
        private ITemplateBusiness _templateBusiness;
        public NtsLogPageColumnBusiness(IRepositoryBase<NtsLogPageColumnViewModel, NtsLogPageColumn> repo, ITemplateBusiness templateBusiness, IMapper autoMapper) : base(repo, autoMapper)
        {
           _templateBusiness = templateBusiness;
        }

        public async override Task<CommandResult<NtsLogPageColumnViewModel>> Create(NtsLogPageColumnViewModel model, bool autoCommit = true)
        {          
            var result = await base.Create(model,autoCommit);
            if (result.IsSuccess)
            {
                //var indexPageTemplateId = result.Item.Id;
                var task = await _templateBusiness.GetSingle(x => x.Id == model.TemplateId);
                if (task != null)
                {
                   // task.Id = indexPageTemplateId;

                    
                    await _templateBusiness.Edit<TemplateViewModel, Template>(task);
                }
                foreach (var col in model.SelectedTableRows)
                {
                    col.Id = null;
                    col.TemplateId = task.Id;
                    await base.Create(col);
                }
            }
            return CommandResult<NtsLogPageColumnViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<NtsLogPageColumnViewModel>> Edit(NtsLogPageColumnViewModel model, bool autoCommit = true)
        {
            //var data = _autoMapper.Map<IndexPageTemplateViewModel>(model);
            var result = await base.Edit(model,autoCommit);
            if (result.IsSuccess)
            {
                var pagecol = await _repo.GetList(x => x.TemplateId == model.TemplateId); 
                var existingIds = pagecol.Select(x => x.Id);
                var newIds = model.SelectedTableRows.Select(x => x.Id);
                var ToDelete = existingIds.Except(newIds).ToList();
                var ToAdd = newIds.Except(existingIds).ToList();
                var ToEdit = newIds.Intersect(existingIds).ToList();
                foreach (var col1 in model.SelectedTableRows)
                {
                    if (ToAdd.Any(x => x == col1.Id))
                    {
                        col1.TemplateId = model.TemplateId;
                        col1.Id = null;
                        await base.Create(col1);
                    }

                }
                foreach (var col1 in model.SelectedTableRows)
                {
                    if (ToEdit.Any(x => x == col1.Id))
                    {

                        await base.Edit(col1);
                    }
                }
                foreach (var col1 in ToDelete)
                {
                    await base.Delete(col1);
                }
            }
            //return CommandResult<IndexPageTemplateViewModel>.Instance(model);
            return CommandResult<NtsLogPageColumnViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }


    }
}
