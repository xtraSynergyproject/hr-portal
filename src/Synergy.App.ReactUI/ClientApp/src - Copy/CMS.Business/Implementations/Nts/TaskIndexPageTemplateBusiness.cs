using AutoMapper;
using CMS.Common;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
using System.Linq;
using System.Threading.Tasks;


namespace CMS.Business
{
    public class TaskIndexPageTemplateBusiness : BusinessBase<TaskIndexPageTemplateViewModel, TaskIndexPageTemplate>, ITaskIndexPageTemplateBusiness
    {
        private ITaskIndexPageColumnBusiness _indexPageColumnBusiness;
        public TaskIndexPageTemplateBusiness(IRepositoryBase<TaskIndexPageTemplateViewModel, TaskIndexPageTemplate> repo, ITaskIndexPageColumnBusiness indexPageColumnBusiness, IMapper autoMapper) : base(repo, autoMapper)
        {
            _indexPageColumnBusiness = indexPageColumnBusiness;
        }

        public async override Task<CommandResult<TaskIndexPageTemplateViewModel>> Create(TaskIndexPageTemplateViewModel model)
        {
            var result = await base.Create(model);
            if (result.IsSuccess)
            {
                var indexPageTemplateId = result.Item.Id;
                var task = await _repo.GetSingle<TaskTemplateViewModel, TaskTemplate>(x => x.TemplateId == model.TemplateId);
                if (task != null)
                {
                    task.TaskIndexPageTemplateId = indexPageTemplateId;
                    task.EnableIndexPage = true;
                    await _repo.Edit<TaskTemplateViewModel, TaskTemplate>(task);
                }
                foreach (var col in model.SelectedTableRows)
                {
                    col.Id = null;
                    col.TaskIndexPageTemplateId = indexPageTemplateId;
                    await _indexPageColumnBusiness.Create(col);
                }
            }
            return CommandResult<TaskIndexPageTemplateViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<TaskIndexPageTemplateViewModel>> Edit(TaskIndexPageTemplateViewModel model)
        {
            //var data = _autoMapper.Map<IndexPageTemplateViewModel>(model);
            var result = await base.Edit(model);
            if (result.IsSuccess)
            {
                var pagecol = await _indexPageColumnBusiness.GetList(x => x.TaskIndexPageTemplateId == model.Id);
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
                        col1.TaskIndexPageTemplateId = model.Id;
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
            return CommandResult<TaskIndexPageTemplateViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }


    }
}
