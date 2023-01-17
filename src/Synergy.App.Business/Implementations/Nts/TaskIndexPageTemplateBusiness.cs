using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Synergy.App.Business
{
    public class TaskIndexPageTemplateBusiness : BusinessBase<TaskIndexPageTemplateViewModel, TaskIndexPageTemplate>, ITaskIndexPageTemplateBusiness
    {
        private ITaskIndexPageColumnBusiness _indexPageColumnBusiness;
        public TaskIndexPageTemplateBusiness(IRepositoryBase<TaskIndexPageTemplateViewModel, TaskIndexPageTemplate> repo, ITaskIndexPageColumnBusiness indexPageColumnBusiness, IMapper autoMapper) : base(repo, autoMapper)
        {
            _indexPageColumnBusiness = indexPageColumnBusiness;
        }

        public async override Task<CommandResult<TaskIndexPageTemplateViewModel>> Create(TaskIndexPageTemplateViewModel model, bool autoCommit = true)
        {
            var result = await base.Create(model,autoCommit);
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

        public async override Task<CommandResult<TaskIndexPageTemplateViewModel>> Edit(TaskIndexPageTemplateViewModel model, bool autoCommit = true)
        {
            //var data = _autoMapper.Map<IndexPageTemplateViewModel>(model);
            var result = await base.Edit(model,autoCommit);
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

        public async Task<CommandResult<TaskIndexPageTemplateViewModel>> CopyTaskTemplateIndexPageData(TaskIndexPageTemplateViewModel model, string newTempId, CopyTemplateViewModel copyModel = null)
        {

            var newTaskIndexModel = await _repo.GetSingle(x => x.TemplateId == newTempId);
            var newTempModel = await _repo.GetSingleById<TemplateViewModel, Template>(newTempId);

            List<TaskIndexPageColumnViewModel> rows = new();

            if (copyModel.IsNotNull())
            {
                rows = copyModel.TaskIndexPageColumn;
            }
            else
            {
                rows = await _indexPageColumnBusiness.GetList(x => x.TaskIndexPageTemplateId == model.Id);
            }
            var newColumnMetaData = await _repo.GetList<ColumnMetadataViewModel, ColumnMetadata>(x => x.TableMetadataId == newTempModel.UdfTableMetadataId);

            
            foreach (var i in rows)
            {
                i.DataAction = DataActionEnum.Create;
                i.Id = null;
                i.Select = true;
                i.TaskIndexPageTemplateId = newTaskIndexModel.Id;
                if (i.IsForeignKeyTableColumn == false)
                {
                    var column = newColumnMetaData.SingleOrDefault(x => x.Alias == i.ColumnName);
                    i.ColumnMetadataId = column.Id;
                }
                else
                {
                    i.ColumnMetadataId = null;

                }
            }
            model.SelectedTableRows = rows;

            var existingModel = newTaskIndexModel;
            newTaskIndexModel = model;

            newTaskIndexModel.Id = existingModel.Id;
            newTaskIndexModel.TemplateId = existingModel.TemplateId;
            newTaskIndexModel.CreatedDate = existingModel.CreatedDate;
            newTaskIndexModel.CreatedBy = existingModel.CreatedBy;
            newTaskIndexModel.LastUpdatedDate = existingModel.LastUpdatedDate;
            newTaskIndexModel.LastUpdatedBy = existingModel.LastUpdatedBy;
            newTaskIndexModel.CompanyId = existingModel.CompanyId;
            newTaskIndexModel.LegalEntityId = existingModel.LegalEntityId;


            var result = await Edit(newTaskIndexModel);
            return result;

        }

    }
}
