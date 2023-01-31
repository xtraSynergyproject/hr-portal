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
    public class NoteIndexPageTemplateBusiness : BusinessBase<NoteIndexPageTemplateViewModel, NoteIndexPageTemplate>, INoteIndexPageTemplateBusiness
    {
        private INoteIndexPageColumnBusiness _indexPageColumnBusiness;
        public NoteIndexPageTemplateBusiness(IRepositoryBase<NoteIndexPageTemplateViewModel, NoteIndexPageTemplate> repo, INoteIndexPageColumnBusiness indexPageColumnBusiness, IMapper autoMapper) : base(repo, autoMapper)
        {
            _indexPageColumnBusiness = indexPageColumnBusiness;
        }

        public async override Task<CommandResult<NoteIndexPageTemplateViewModel>> Create(NoteIndexPageTemplateViewModel model, bool autoCommit = true)
        {
            var result = await base.Create(model,autoCommit);
            if (result.IsSuccess)
            {
                var indexPageTemplateId = result.Item.Id;
                var task = await _repo.GetSingle<NoteTemplateViewModel, NoteTemplate>(x => x.TemplateId == model.TemplateId);
                if (task != null)
                {
                    task.NoteIndexPageTemplateId = indexPageTemplateId;
                    task.EnableIndexPage = true;
                    await _repo.Edit<NoteTemplateViewModel, NoteTemplate>(task);
                }
                foreach (var col in model.SelectedTableRows)
                {
                    col.Id = null;
                    col.NoteIndexPageTemplateId = indexPageTemplateId;
                    await _indexPageColumnBusiness.Create(col);
                }
            }
            return CommandResult<NoteIndexPageTemplateViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<NoteIndexPageTemplateViewModel>> Edit(NoteIndexPageTemplateViewModel model, bool autoCommit = true)
        {
            //var data = _autoMapper.Map<IndexPageTemplateViewModel>(model);
            var result = await base.Edit(model,autoCommit);
            if (result.IsSuccess)
            {
                var pagecol = await _indexPageColumnBusiness.GetList(x => x.NoteIndexPageTemplateId == model.Id);
                var existingIds = pagecol.Select(x => x.Id);
                
                var newIds = model.SelectedTableRows.IsNotNull() ? model.SelectedTableRows.Select(x => x.Id) : new List<string>();
                var ToDelete = existingIds.Except(newIds).ToList(); //pagecol.Select(x=>x.Id).ToList().Except(model.SelectedTableRows).ToList();
                var ToAdd = newIds.Except(existingIds).ToList();//model.SelectedTableRows.Except(pagecol).ToList();
                var ToEdit = newIds.Intersect(existingIds).ToList();//model.SelectedTableRows.Intersect(pagecol).ToList();

                if (model.SelectedTableRows.IsNotNull())
                {
                    foreach (var col1 in model.SelectedTableRows)
                    {
                        if (ToAdd.Any(x => x == col1.Id))
                        {
                            col1.NoteIndexPageTemplateId = model.Id;
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
                }
                foreach (var col in ToDelete)
                {
                    await _indexPageColumnBusiness.Delete(col);
                }

                //return CommandResult<IndexPageTemplateViewModel>.Instance(model);

            }
            return CommandResult<NoteIndexPageTemplateViewModel>.Instance(model, result.IsSuccess, result.Messages);

        }


        public async Task<CommandResult<NoteIndexPageTemplateViewModel>> CopyNoteTemplateIndexPageData(NoteIndexPageTemplateViewModel model, string newTempId, CopyTemplateViewModel copyModel = null)
        {
            var newTempModel = await _repo.GetSingleById<TemplateViewModel, Template>(newTempId);
            var newNoteIndexModel = await _repo.GetSingle(x => x.TemplateId == newTempId);
            List<NoteIndexPageColumnViewModel> rows = new();
            if(copyModel.IsNotNull())
            {
                rows = copyModel.NoteIndexPageColumn;
            }
            else
            {
                rows = await _indexPageColumnBusiness.GetList(x => x.NoteIndexPageTemplateId == model.Id);
            }
            var newColumnMetaData = await _repo.GetList<ColumnMetadataViewModel, ColumnMetadata>(x => x.TableMetadataId == newTempModel.TableMetadataId);
            foreach (var i in rows)
            {
                i.DataAction = DataActionEnum.Create;
                i.Id = null;
                i.Select = true;
                i.NoteIndexPageTemplateId = newNoteIndexModel.Id;
                if (i.IsForeignKeyTableColumn == false)
                {
                    var column = newColumnMetaData.SingleOrDefault(x => x.Alias == i.HeaderName);
                    i.ColumnMetadataId = column.Id;
                }
                else
                {
                    i.ColumnMetadataId = null;

                }
            }
            model.SelectedTableRows = rows;

            var existingModel = newNoteIndexModel;
            newNoteIndexModel = model;

            newNoteIndexModel.Id = existingModel.Id;
            newNoteIndexModel.TemplateId = existingModel.TemplateId;
            newNoteIndexModel.CreatedDate = existingModel.CreatedDate;
            newNoteIndexModel.CreatedBy = existingModel.CreatedBy;
            newNoteIndexModel.LastUpdatedDate = existingModel.LastUpdatedDate;
            newNoteIndexModel.LastUpdatedBy = existingModel.LastUpdatedBy;
            newNoteIndexModel.CompanyId = existingModel.CompanyId;
            newNoteIndexModel.LegalEntityId = existingModel.LegalEntityId;

            var result = await Edit(newNoteIndexModel);
            return result;

        }

    }
}
