using AutoMapper;
using Synergy.App.Common;
using Synergy.App.Common.Utilities;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace Synergy.App.Business
{
    public class ColumnMetadataBusiness : BusinessBase<ColumnMetadataViewModel, ColumnMetadata>, IColumnMetadataBusiness
    {
        private IRepositoryQueryBase<ColumnMetadataViewModel> _repoQuery;
        private IServiceProvider _serviceProvider;
        private readonly ICmsQueryBusiness _cmsQueryBusiness;
        public ColumnMetadataBusiness(IRepositoryBase<ColumnMetadataViewModel, ColumnMetadata> repo, IMapper autoMapper
          , IRepositoryQueryBase<ColumnMetadataViewModel> repoQuery
            , IServiceProvider serviceProvider, ICmsQueryBusiness cmsQueryBusiness) : base(repo, autoMapper)
        {
            _repoQuery = repoQuery;
            _serviceProvider = serviceProvider;
            _cmsQueryBusiness = cmsQueryBusiness;
        }

        public async override Task<CommandResult<ColumnMetadataViewModel>> Create(ColumnMetadataViewModel model, bool autoCommit = true)
        {
            if (model.IsForeignKey)
            {
                var fkTable = await _repo.GetSingle<TableMetadataViewModel, TableMetadata>(x => x.Name == model.ForeignKeyTableName && x.Schema == model.ForeignKeyTableSchemaName);
                if (fkTable != null)
                {
                    model.ForeignKeyTableId = fkTable.Id;
                    if (model.ForeignKeyColumnName.IsNotNullAndNotEmpty())
                    {
                        var fkColumn = await _repo.GetSingle<ColumnMetadataViewModel, ColumnMetadata>(x => x.TableMetadataId == fkTable.Id &&
                         x.Name == model.ForeignKeyColumnName);
                        if (fkColumn != null)
                        {
                            model.ForeignKeyColumnId = fkColumn.Id;
                        }
                    }
                    if (model.ForeignKeyDisplayColumnName.IsNotNullAndNotEmpty())
                    {
                        var fkColumnDisplay = await _repo.GetSingle<ColumnMetadataViewModel, ColumnMetadata>(x => x.TableMetadataId == fkTable.Id &&
                         x.Name == model.ForeignKeyDisplayColumnName);
                        if (fkColumnDisplay != null)
                        {
                            model.ForeignKeyDisplayColumnId = fkColumnDisplay.Id;
                        }
                    }

                    if (model.ForeignKeyConstraintName.IsNullOrEmpty())
                    {
                        var table = await _repo.GetSingle<TableMetadataViewModel, TableMetadata>(x => x.Id == model.TableMetadataId);
                        if (table != null)
                        {
                            model.ForeignKeyConstraintName = $"FK_{table.Name}_{fkTable.Name}_{model.Name}_{model.ForeignKeyColumnName}";
                        }
                    }

                }
            }
            if (model.ForeignKeyConstraintName != null)
            {
                model.ForeignKeyConstraintName = TruncateForeignKeyContraint(model.ForeignKeyConstraintName);
            }
            var colExists = await _repo.GetSingle(x => x.TableMetadataId == model.TableMetadataId && x.Name == model.Name);
            if (colExists != null)
            {
                model.Id = colExists.Id;
                model.DataAction = DataActionEnum.Edit;
                return await Edit(model);
            }
            var result = await base.Create(model, autoCommit);
            if (result.IsSuccess)
            {
                model.Id = result.Item.Id;
            }
            // await ManageForeignKeyReferenceColumn(model);
            return CommandResult<ColumnMetadataViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<ColumnMetadataViewModel>> Edit(ColumnMetadataViewModel model, bool autoCommit = true)
        {
            var EditableByS = model.EditableBy;
            var EditableContextS = model.EditableContext;
            var ViewableByS = model.ViewableBy;
            var ViewableContextS = model.ViewableContext;
            if (model.IsForeignKey)
            {
                var fkTable = await _repo.GetSingle<TableMetadataViewModel, TableMetadata>(x => x.Name == model.ForeignKeyTableName && x.Schema == model.ForeignKeyTableSchemaName);
                if (fkTable != null)
                {
                    model.ForeignKeyTableId = fkTable.Id;
                    if (model.ForeignKeyColumnName.IsNotNullAndNotEmpty())
                    {
                        var fkColumn = await _repo.GetSingle<ColumnMetadataViewModel, ColumnMetadata>(x => x.TableMetadataId == fkTable.Id &&
                         x.Name == model.ForeignKeyColumnName);
                        if (fkColumn != null)
                        {
                            model.ForeignKeyColumnId = fkColumn.Id;
                        }
                    }
                    if (model.ForeignKeyDisplayColumnName.IsNotNullAndNotEmpty())
                    {
                        var fkColumnDisplay = await _repo.GetSingle<ColumnMetadataViewModel, ColumnMetadata>(x => x.TableMetadataId == fkTable.Id &&
                         x.Name == model.ForeignKeyDisplayColumnName);
                        if (fkColumnDisplay != null)
                        {
                            model.ForeignKeyDisplayColumnId = fkColumnDisplay.Id;
                        }
                    }
                    if (model.ForeignKeyConstraintName.IsNullOrEmpty())
                    {
                        var table = await _repo.GetSingle<TableMetadataViewModel, TableMetadata>(x => x.Id == model.TableMetadataId);
                        if (table != null)
                        {
                            model.ForeignKeyConstraintName = $"FK_{table.Name}_{fkTable.Name}_{model.Name}_{model.ForeignKeyColumnName}";
                        }
                    }

                }
            }
            if (model.IgnorePermission)
            {
                var exist = await _repo.GetSingle<ColumnMetadataViewModel, ColumnMetadata>(x => x.Id == model.Id);

                if (exist != null)
                {
                    model.EditableBy = exist.EditableBy;
                    model.EditableContext = exist.EditableContext;
                    model.ViewableBy = exist.ViewableBy;
                    model.ViewableContext = exist.ViewableContext;
                }
            }
            if (model.ForeignKeyConstraintName != null)
            {
                model.ForeignKeyConstraintName = TruncateForeignKeyContraint(model.ForeignKeyConstraintName);
            }
            var result = await base.Edit(model, autoCommit);
            model.EditableBy = EditableByS;
            model.EditableContext = EditableContextS;
            model.ViewableBy = ViewableByS;
            model.ViewableContext = ViewableContextS;
            // await ManageForeignKeyReferenceColumn(model);

            return CommandResult<ColumnMetadataViewModel>.Instance(model);
        }

        //private async Task ManageForeignKeyReferenceColumn(ColumnMetadataViewModel model)
        //{
        //    if (model.IsForeignKey)
        //    {
        //        if (model.HideForeignKeyTableColumns)
        //        {
        //            var referenceColumn = new ColumnMetadataViewModel
        //            {
        //                Name = model.ForeignKeyDisplayColumnAlias,
        //                LabelName = model.ForeignKeyDisplayColumnLabelName,
        //                Alias = model.ForeignKeyDisplayColumnAlias,
        //                DataType = model.ForeignKeyDisplayColumnDataType,
        //                IsSystemColumn = true,
        //                IsHiddenColumn = false,
        //                IsReferenceColumn = model.IsReferenceColumn,
        //                ReferenceTableName = model.ReferenceTableName,
        //                ReferenceTableSchemaName = model.ReferenceTableSchemaName,
        //                IsForeignKey = false,
        //                IsVirtualColumn = true,
        //                ForeignKeyColumnName = model.ForeignKeyColumnName,
        //                ForeignKeyTableName = model.ForeignKeyTableName,
        //                ForeignKeyTableSchemaName = model.ForeignKeyTableSchemaName,
        //                ForeignKeyDisplayColumnName = model.ForeignKeyDisplayColumnName,
        //                ForeignKeyDisplayColumnAlias = model.ForeignKeyDisplayColumnAlias,
        //                ForeignKeyDisplayColumnLabelName = model.ForeignKeyDisplayColumnLabelName,
        //                IsDeleted = false,
        //                CreatedDate = DateTime.Now,
        //                CreatedBy = _repo.UserContext.UserId,
        //                LastUpdatedDate = DateTime.Now,
        //                LastUpdatedBy = _repo.UserContext.UserId,
        //                Status = StatusEnum.Active,
        //                TableMetadataId = model.TableMetadataId,
        //                ForeignKeyDisplayColumnReferenceId = model.Id
        //            };
        //            var exist = await _repo.GetSingle(x => x.TableMetadataId == model.TableMetadataId && x.Name == model.ForeignKeyDisplayColumnAlias);
        //            if (exist != null)
        //            {
        //                referenceColumn.DataAction = DataActionEnum.Edit;
        //                referenceColumn.Id = exist.Id;
        //                referenceColumn.CreatedDate = exist.CreatedDate;
        //                referenceColumn.CreatedBy = exist.CreatedBy;
        //                await Edit(referenceColumn);
        //            }
        //            else
        //            {
        //                referenceColumn.Id = Guid.NewGuid().ToString();
        //                referenceColumn.DataAction = DataActionEnum.Create;

        //                await Create(referenceColumn);
        //            }

        //        }
        //        else
        //        {
        //            var exist = await _repo.GetSingle(x => x.TableMetadataId == model.TableMetadataId && x.Name == model.ForeignKeyDisplayColumnAlias);
        //            if (exist != null)
        //            {
        //                await _repo.Delete(exist.Id);
        //            }
        //        }
        //    }
        //}
        //public async Task<List<ColumnMetadataViewModel>> GetViewableColumnMetadataList(string tableMetadataId, bool includeForiegnKeyTableColumns = true)
        //{
        //    var query = @$"select t.* from public.""TableMetadata"" ta left join public.""Template"" t on ta.""Id""=t.""TableMetadataId""
        //    where ta.""Id""='{tableMetadataId}'";
        //    var template = await _repoQuery.ExecuteQuerySingle<TemplateViewModel>(query, null);
        //    var templateType = TemplateTypeEnum.FormIndexPage;
        //    if (template != null && template.Id != null)
        //    {
        //        templateType = template.TemplateType;
        //    }

        //    return await GetViewableColumnMetadataList(tableMetadataId, templateType, includeForiegnKeyTableColumns);
        //}
        public async Task<List<ColumnMetadataViewModel>> GetViewableColumnMetadataList(string schemaName, string tableName, bool includeForiegnKeyTableColumns = true)
        {
            var tableMetadata = await _cmsQueryBusiness.GetViewableColumnMetadataListData(schemaName, tableName);
            var templateType = TemplateTypeEnum.FormIndexPage;
            if (tableMetadata != null && tableMetadata.Id != null)
            {
                templateType = tableMetadata.TemplateType;

                return await GetViewableColumnMetadataList(tableMetadata.Id, templateType, includeForiegnKeyTableColumns);
            }

            return await Task.FromResult(default(List<ColumnMetadataViewModel>));
        }
        public async Task<List<ColumnMetadataViewModel>> GetViewableColumnMetadataList(string tableMetadataId, TemplateTypeEnum templateType, bool includeForiegnKeyTableColumns = true)
        {
            var list = new List<ColumnMetadataViewModel>();
            switch (templateType)
            {

                case TemplateTypeEnum.Form:
                    list = await _repo.GetList(x => x.TableMetadataId == tableMetadataId && (x.IsHiddenColumn == false || x.IsPrimaryKey));
                    if (includeForiegnKeyTableColumns)
                    {
                        var fkColumns = await GetViewableForiegnKeyColumnListForForm(tableMetadataId, list);
                        if (fkColumns != null && fkColumns.Count > 0)
                        {
                            list.AddRange(fkColumns);
                        }

                    }
                    break;
                case TemplateTypeEnum.Note:
                    var noteTable = await _repo.GetSingleById<TableMetadataViewModel, TableMetadata>(tableMetadataId);
                    var noteBusiness = _serviceProvider.GetService<INoteBusiness>();
                    list = await noteBusiness.GetViewableColumns(noteTable);
                    break;
                case TemplateTypeEnum.Task:
                    var taskTable = await _repo.GetSingleById<TableMetadataViewModel, TableMetadata>(tableMetadataId);
                    var taskBusiness = _serviceProvider.GetService<ITaskBusiness>();
                    list = await taskBusiness.GetViewableColumns(taskTable);
                    break;
                case TemplateTypeEnum.Service:
                    var serviceTable = await _repo.GetSingleById<TableMetadataViewModel, TableMetadata>(tableMetadataId);
                    var serviceBusiness = _serviceProvider.GetService<IServiceBusiness>();
                    list = await serviceBusiness.GetViewableColumns(serviceTable);
                    //list = await _repo.GetList(x => x.TableMetadataId == tableMetadataId && x.IsUdfColumn == true);
                    //if (includeForiegnKeyTableColumns)
                    //{
                    //    var fkColumns = await GetViewableForiegnKeyColumnListForService(tableMetadataId, list);
                    //    if (fkColumns != null && fkColumns.Count > 0)
                    //    {
                    //        list.AddRange(fkColumns);
                    //    }
                    //}
                    break;
                default:
                    list = await _repo.GetList(x => x.TableMetadataId == tableMetadataId && (x.IsHiddenColumn == false || x.IsPrimaryKey));
                    if (includeForiegnKeyTableColumns)
                    {
                        var fkColumns = await GetViewableForiegnKeyColumnListForForm(tableMetadataId, list);
                        if (fkColumns != null && fkColumns.Count > 0)
                        {
                            list.AddRange(fkColumns);
                        }

                    }
                    break;
            }

            return list.OrderBy(x => x.Name).ToList();
        }
        private string TruncateForeignKeyContraint(string name)
        {
            if (name.IsNullOrEmpty())
            {
                return name;
            }
            if (name.Length > 60)
            {
                return $"{name.Substring(0, 25)}_{Guid.NewGuid().ToString()}";
            }

            return name;
        }
        private async Task<List<ColumnMetadataViewModel>> GetViewableForiegnKeyColumnListForForm(string tableMetadataId, List<ColumnMetadataViewModel> columnList)
        {
            var list = new List<ColumnMetadataViewModel>();
            var fks = columnList.Where(x => x.IsForeignKey && x.ForeignKeyTableId.IsNotNullAndNotEmpty());
            if (fks != null && fks.Any())
            {
                var result = await _cmsQueryBusiness.GetViewableForiegnKeyColumnListForFormData(tableMetadataId);
                if (result != null && result.Any())
                {
                    foreach (var item in result)
                    {
                        item.Name = $"{item.ForeignKeyBaseColumnName}_{item.Name}";
                    }




                    list.AddRange(result);
                }
            }

            return list;
        }

        private async Task<List<ColumnMetadataViewModel>> GetViewableForiegnKeyColumnListForNote(string tableMetadataId, List<ColumnMetadataViewModel> columnList)
        {
            var list = new List<ColumnMetadataViewModel>();
            var fks = columnList.Where(x => x.IsForeignKey && x.ForeignKeyTableId.IsNotNullAndNotEmpty());
            if (fks != null && fks.Any())
            {
                var result = await _cmsQueryBusiness.GetViewableForiegnKeyColumnListForNoteData(tableMetadataId);
                if (result != null && result.Any())
                {
                    foreach (var item in result)
                    {
                        if (item.TableSchemaName == "public" && item.TableName == "NtsNote")
                        {
                            if (item.IsPrimaryKey)
                            {
                                item.Name = $"{item.TableAliasName}_{item.Name}";
                            }
                        }
                        else
                        {
                            item.Name = $"{item.TableAliasName}_{item.Name}";
                        }
                    }
                    list.AddRange(result);
                }
            }
            return list;
        }
        private async Task<List<ColumnMetadataViewModel>> GetViewableForiegnKeyColumnListForTask(string tableMetadataId, List<ColumnMetadataViewModel> columnList)
        {
            var list = new List<ColumnMetadataViewModel>();
            var fks = columnList.Where(x => x.IsForeignKey && x.ForeignKeyTableId.IsNotNullAndNotEmpty());
            if (fks != null && fks.Any())
            {
                var result = await _cmsQueryBusiness.GetViewableForiegnKeyColumnListForTaskData(tableMetadataId);
                if (result != null && result.Any())
                {
                    foreach (var item in result)
                    {
                        item.Name = $"{item.TableAliasName}_{item.Name}";
                    }
                    list.AddRange(result);
                }
            }

            var result1 = await _cmsQueryBusiness.GetViewableForiegnKeyColumnListForTaskData2();
            list.AddRange(result1);
            return list;
        }
        private async Task<List<ColumnMetadataViewModel>> GetViewableForiegnKeyColumnListForService(string tableMetadataId, List<ColumnMetadataViewModel> columnList)
        {
            var list = new List<ColumnMetadataViewModel>();
            var fks = columnList.Where(x => x.IsForeignKey && x.ForeignKeyTableId.IsNotNullAndNotEmpty());
            if (fks != null && fks.Any())
            {
                var result = await _cmsQueryBusiness.GetViewableForiegnKeyColumnListForServiceData(tableMetadataId);
                if (result != null && result.Any())
                {
                    foreach (var item in result)
                    {
                        item.Name = $"{item.TableAliasName}_{item.Name}";
                    }
                    list.AddRange(result);
                }
            }

            var result1 = await _cmsQueryBusiness.GetViewableForiegnKeyColumnListForServiceData1(tableMetadataId);
            list.AddRange(result1);
            return list;
        }
    }
}
