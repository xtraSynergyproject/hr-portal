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
    public class FormIndexPageTemplateBusiness : BusinessBase<FormIndexPageTemplateViewModel, FormIndexPageTemplate>, IFormIndexPageTemplateBusiness
    {
        private IFormIndexPageColumnBusiness _formIndexPageColumnBusiness;
        public FormIndexPageTemplateBusiness(IRepositoryBase<FormIndexPageTemplateViewModel, FormIndexPageTemplate> repo
            , IFormIndexPageColumnBusiness formIndexPageColumnBusiness, IMapper autoMapper) : base(repo, autoMapper)
        {
            _formIndexPageColumnBusiness = formIndexPageColumnBusiness;
        }
        public async override Task<CommandResult<FormIndexPageTemplateViewModel>> Create(FormIndexPageTemplateViewModel model, bool autoCommit = true)
        {
            var result = await base.Create(model,autoCommit);
            if (result.IsSuccess)
            {
                var indexPageTemplateId = result.Item.Id;
                var form = await _repo.GetSingle<FormTemplateViewModel, FormTemplate>(x => x.TemplateId == model.TemplateId);
                if (form != null)
                {
                    form.IndexPageTemplateId = indexPageTemplateId;
                    form.EnableIndexPage = true;
                    await _repo.Edit<FormTemplateViewModel, FormTemplate>(form);
                }
                //foreach (var col in model.SelectedTableRows)
                //{
                //    col.FormIndexPageTemplateId = indexPageTemplateId;
                //    await _formIndexPageColumnBusiness.Create(col);
                //}
            }
            return CommandResult<FormIndexPageTemplateViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<FormIndexPageTemplateViewModel>> Edit(FormIndexPageTemplateViewModel model, bool autoCommit = true)
        {
            var result = await base.Edit(model,autoCommit);
            if (result.IsSuccess)
            {
                var form = await _repo.GetSingle<FormTemplateViewModel, FormTemplate>(x => x.TemplateId == model.TemplateId);
                if (form != null)
                {
                    form.IndexPageTemplateId = result.Item.Id;
                    form.EnableIndexPage = true;
                    await _repo.Edit<FormTemplateViewModel, FormTemplate>(form);
                }
                var pagecol = await _formIndexPageColumnBusiness.GetList(x => x.FormIndexPageTemplateId == model.Id);
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
                        col1.FormIndexPageTemplateId = model.Id;
                        col1.Id = null;
                        await _formIndexPageColumnBusiness.Create(col1);
                    }

                }

                //}
                //foreach (var col in ToEdit)
                //{
                foreach (var col1 in model.SelectedTableRows)
                {
                    if (ToEdit.Any(x => x == col1.Id))
                    {
                        // var data = _autoMapper.Map<IndexPageColumnViewModel>(col1);
                        // data.IndexPageTemplate = null;
                        // data.IndexPageTemplateId = result.Item.Id;
                        await _formIndexPageColumnBusiness.Edit(col1);
                    }
                }
                //}
                foreach (var col in ToDelete)
                {
                    await _formIndexPageColumnBusiness.Delete(col);
                }
            }
            //return CommandResult<IndexPageTemplateViewModel>.Instance(model);
            return CommandResult<FormIndexPageTemplateViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async Task<CommandResult<FormIndexPageTemplateViewModel>> CopyFormTemplateIndexPageData(FormIndexPageTemplateViewModel model, string newTempId, bool devImport = false, CopyTemplateViewModel copyModel = null)
        {
            var newFormIndexModel = await _repo.GetSingle(x => x.TemplateId == newTempId);
            var newTempModel = await _repo.GetSingleById<TemplateViewModel, Template>(newTempId);
            List<FormIndexPageColumnViewModel> rows = new();

            if (devImport)
            {
                rows = copyModel.FormIndexPageColumn;
            }
            else
            {
                rows = await _formIndexPageColumnBusiness.GetList(x => x.FormIndexPageTemplateId == model.Id);
            }
            var newColumnMetaData = await _repo.GetList<ColumnMetadataViewModel, ColumnMetadata>(x => x.TableMetadataId == newTempModel.TableMetadataId);
            foreach (var i in rows)
            {
                i.DataAction = DataActionEnum.Create;
                i.Id = null;
                i.Select = true;
                i.FormIndexPageTemplateId = newFormIndexModel.Id;
                if(i.IsForeignKeyTableColumn == false)
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

            var existingModel = newFormIndexModel;
            newFormIndexModel = model;

            newFormIndexModel.Id = existingModel.Id;
            newFormIndexModel.TemplateId = existingModel.TemplateId;
            newFormIndexModel.CreatedDate = existingModel.CreatedDate;
            newFormIndexModel.CreatedBy = existingModel.CreatedBy;
            newFormIndexModel.LastUpdatedDate = existingModel.LastUpdatedDate;
            newFormIndexModel.LastUpdatedBy = existingModel.LastUpdatedBy;
            newFormIndexModel.CompanyId = existingModel.CompanyId;
            newFormIndexModel.LegalEntityId = existingModel.LegalEntityId;
            newFormIndexModel.SelectedTableRows = model.SelectedTableRows;

            var result = await Edit(newFormIndexModel);
            return result;
        }
    }
}
