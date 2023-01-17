using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Text.Json;
using Newtonsoft.Json;
using System.IO;

namespace Synergy.App.Business
{
    public class BreMasterMetadataBusiness : BusinessBase<BreMasterMetadataViewModel, BreMasterTableMetadata>, IBreMasterMetadataBusiness
    {


        //public List<BreMetadataViewMo>
        public BreMasterMetadataBusiness(IRepositoryBase<BreMasterMetadataViewModel, BreMasterTableMetadata> repo, IMapper autoMapper) : base(repo, autoMapper)
        {

        }

        public async Task<List<BreMasterMetadataViewModel>> GetAllBreMasterMetaData()
        {
            var id = 1;
            var result = new List<BreMasterMetadataViewModel>();
            var dbCollections = await _repo.GetListGlobal<DataIntegrationViewModel, DataIntegration>(x => x.CompanyCode == _repo.UserContext.CompanyCode);
            result.Add(new BreMasterMetadataViewModel
            {

                Id = _repo.UserContext.CompanyCode,
                Name = _repo.UserContext.CompanyCode,
                DisplayName = _repo.UserContext.CompanyName,
                DataType = DataTypeEnum.Object,
                ParentId = null,
                HasSubFolders = dbCollections.Any(),
                Expanded = true,
                CreatedDate = DateTime.Now,
                LastUpdatedDate = DateTime.Now
            });
            //  await LoadNTS(result, dbCollections);
            foreach (var item in dbCollections)
            {
                var hasProperties = item.Schema != null && item.Schema.Any();
                result.Add(new BreMasterMetadataViewModel
                {

                    Id = item.CollectionName,
                    Name = item.CollectionName,
                    DisplayName = item.CollectionDisplayName,
                    DataType = DataTypeEnum.Object,
                    ParentId = _repo.UserContext.CompanyCode,
                    HasSubFolders = hasProperties,
                    Expanded = false,
                    CreatedDate = DateTime.Now,
                    LastUpdatedDate = DateTime.Now
                });
                if (hasProperties)
                {
                    var schemaList = JsonConvert.DeserializeObject<Dictionary<string, string>>(item.Schema);
                    result.AddRange(schemaList.Select(x => new BreMasterMetadataViewModel
                    {
                        Id = x.Key,
                        Name = x.Value,
                        DisplayName = x.Key,
                        DataType = GetDataType(x.Value),
                        ParentId = item.CollectionName,
                        HasSubFolders = false,
                        CreatedDate = DateTime.Now,
                        LastUpdatedDate = DateTime.Now

                    }));
                }

            }
            return result;
        }
        public async Task<List<BreMasterMetadataViewModel>> GetBreMasterDataTreeList(string parentId)
        {
            var result = new List<BreMasterMetadataViewModel>();
            if (parentId.IsNullOrEmpty())
            {
                result.Add(new BreMasterMetadataViewModel
                {

                    Id = _repo.UserContext.CompanyCode,
                    Name = _repo.UserContext.CompanyCode,
                    DisplayName = _repo.UserContext.CompanyName,
                    DataType = DataTypeEnum.Object,
                    ParentId = null,
                    HasSubFolders = true,
                    Expanded = true,
                    CreatedDate = DateTime.Now,
                    LastUpdatedDate = DateTime.Now
                });
            }
            else if (parentId == _repo.UserContext.CompanyCode)
            {
                var dbCollections = await _repo.GetListGlobal<DataIntegrationViewModel, DataIntegration>(x => x.CompanyCode == _repo.UserContext.CompanyCode);

                result.AddRange(dbCollections.Select(x => new BreMasterMetadataViewModel
                {

                    Id = x.CollectionName,
                    Name = x.CollectionName,
                    DisplayName = x.CollectionDisplayName,
                    DataType = DataTypeEnum.Object,
                    ParentId = _repo.UserContext.CompanyCode,
                    HasSubFolders = x.Schema != null && x.Schema.Any(),

                }));

            }
            else
            {
                var parentList = await _repo.GetList<DataIntegrationViewModel, DataIntegration>(x => x.CollectionName == parentId);
                var parent = parentList.FirstOrDefault();
                if (parent != null && parent.Schema != null)
                {
                    var schemaList = JsonConvert.DeserializeObject<Dictionary<string, string>>(parent.Schema);
                    result.AddRange(schemaList.Select(x => new BreMasterMetadataViewModel
                    {
                        Id = x.Key,
                        Name = x.Value,
                        DisplayName = x.Key,
                        DataType = GetDataType(x.Value),
                        ParentId = parent.CollectionName,
                        HasSubFolders = false

                    }));
                }

            }

            return result;
        }
        private async Task LoadNTS(List<BreMasterMetadataViewModel> result, List<DataIntegrationViewModel> dbCollections)
        {
            var templateName = $"{_repo.UserContext.CompanyCode}_NtsTemplate";
            var templateFieldName = $"{_repo.UserContext.CompanyCode}_NtsTemplateField";
            templateName = "Data_NtsTemplate";
            templateFieldName = "Data_NtsTemplateField";

            var allTemplates = await _repo.GetList<NtsTemplate, NtsTemplate>(x => x.DocumentStatus == "Published");
            var templateFields = await _repo.GetList<NtsTemplateField, NtsTemplateField>(x => x.TemplateId != null);
            // var templateFields =  _repo.Mongo.GetCollection<NtsTemplateField>(templateFieldName).Find(x => x.TemplateId != null);
            var allTemplateList = allTemplates.ToList();
            var templateFieldList = templateFields.ToList();

            await Task.WhenAll(
               Task.Run(() =>
               {
                   var noteTemplates = allTemplateList.Where(x => x.NtsType == "Note");
                   if (noteTemplates != null && noteTemplates.Any())
                   {
                       result.Add(new BreMasterMetadataViewModel
                       {

                           Id = "NTS_Note",
                           Name = "NTS_Note",
                           DisplayName = "Note",
                           DataType = DataTypeEnum.Object,
                           ParentId = _repo.UserContext.CompanyCode,
                           HasSubFolders = true,
                           Expanded = false,
                           CreatedDate = DateTime.Now,
                           LastUpdatedDate = DateTime.Now
                       });
                       foreach (var item in noteTemplates)
                       {
                           var fields = templateFieldList.Where(x => x.TemplateId == item.TemplateId);
                           result.Add(new BreMasterMetadataViewModel
                           {

                               Id = item.Code,
                               Name = item.Code,
                               DisplayName = item.Name,
                               DataType = DataTypeEnum.Object,
                               ParentId = "NTS_Note",
                               HasSubFolders = fields.Any(),
                               Expanded = false,
                               CreatedDate = DateTime.Now,
                               LastUpdatedDate = DateTime.Now
                           });


                           if (fields.Any())
                           {
                               //result.AddRange(fields.Select(x => new BreMasterMetadataViewModel
                               //{
                               //    Id = $"{item.Code}_{x.FieldName}",
                               //    Name = $"{item.Code}_{x.FieldName}",
                               //    DisplayName = x.LabelDisplayName,
                               //    DataType = GetDataTypeFromPartialView(x.FieldPartialViewName),
                               //    ParentId = item.Code,
                               //    HasSubFolders = false,
                               //    CreatedDate = DateTime.Now,
                               //    LastUpdatedDate = DateTime.Now

                               //}));
                           }

                       }
                   }
               }),
               Task.Run(() =>
               {
                   var taskTemplates = allTemplateList.Where(x => x.NtsType == "Task");
                   if (taskTemplates != null && taskTemplates.Any())
                   {
                       result.Add(new BreMasterMetadataViewModel
                       {

                           Id = "NTS_Task",
                           Name = "NTS_Task",
                           DisplayName = "Task",
                           DataType = DataTypeEnum.Object,
                           ParentId = _repo.UserContext.CompanyCode,
                           HasSubFolders = true,
                           Expanded = false,
                           CreatedDate = DateTime.Now,
                           LastUpdatedDate = DateTime.Now
                       });
                       foreach (var item in taskTemplates)
                       {
                           var fields = templateFieldList.Where(x => x.TemplateId == item.TemplateId);
                           result.Add(new BreMasterMetadataViewModel
                           {

                               Id = item.Code,
                               Name = item.Code,
                               DisplayName = item.Name,
                               DataType = DataTypeEnum.Object,
                               ParentId = "NTS_Task",
                               HasSubFolders = fields.Any(),
                               Expanded = false,
                               CreatedDate = DateTime.Now,
                               LastUpdatedDate = DateTime.Now
                           });


                           if (fields.Any())
                           {
                               //result.AddRange(fields.Select(x => new BreMasterMetadataViewModel
                               //{
                               //    Id = $"{item.Code}_{x.FieldName}",
                               //    Name = $"{item.Code}_{x.FieldName}",
                               //    DisplayName = x.LabelDisplayName,
                               //    DataType = GetDataTypeFromPartialView(x.FieldPartialViewName),
                               //    ParentId = item.Code,
                               //    HasSubFolders = false,
                               //    CreatedDate = DateTime.Now,
                               //    LastUpdatedDate = DateTime.Now

                               //}));
                           }

                       }
                   }
               }),
               Task.Run(() =>
               {
                   var serviceTemplates = allTemplateList.Where(x => x.NtsType == "Service");
                   if (serviceTemplates != null && serviceTemplates.Any())
                   {
                       result.Add(new BreMasterMetadataViewModel
                       {

                           Id = "NTS_Service",
                           Name = "NTS_Service",
                           DisplayName = "Service",
                           DataType = DataTypeEnum.Object,
                           ParentId = _repo.UserContext.CompanyCode,
                           HasSubFolders = true,
                           Expanded = false,
                           CreatedDate = DateTime.Now,
                           LastUpdatedDate = DateTime.Now
                       });
                       foreach (var item in serviceTemplates)
                       {
                           var fields = templateFieldList.Where(x => x.TemplateId == item.TemplateId);
                           result.Add(new BreMasterMetadataViewModel
                           {

                               Id = item.Code,
                               Name = item.Code,
                               DisplayName = item.Name,
                               DataType = DataTypeEnum.Object,
                               ParentId = "NTS_Service",
                               HasSubFolders = fields.Any(),
                               Expanded = false,
                               CreatedDate = DateTime.Now,
                               LastUpdatedDate = DateTime.Now
                           });


                           if (fields.Any())
                           {
                               //result.AddRange(fields.Select(x => new BreMasterMetadataViewModel
                               //{
                               //    Id = $"{item.Code}_{x.FieldName}",
                               //    Name = $"{item.Code}_{x.FieldName}",
                               //    DisplayName = x.LabelDisplayName,
                               //    DataType = GetDataTypeFromPartialView(x.FieldPartialViewName),
                               //    ParentId = item.Code,
                               //    HasSubFolders = false,
                               //    CreatedDate = DateTime.Now,
                               //    LastUpdatedDate = DateTime.Now

                               //}));
                           }

                       }
                   }
               })
           );

        }

        private DataTypeEnum GetDataTypeFromPartialView(string fieldPartialViewName)
        {
            return DataTypeEnum.Object;
        }

        private DataTypeEnum GetDataType(string value)
        {
            DataTypeEnum result;
            if (!Enum.TryParse<DataTypeEnum>(value, out result))
            {
                return DataTypeEnum.Object;
            }
            return result;
        }
        public async Task<List<BreMasterMetadataViewModel>> GetBreMasterMetaData(string bussinessRuleId, string parentId)
        {

            var list = new List<BreMasterMetadataViewModel>();

            var columnMetaData = new List<BreMasterColumnMetadataViewModel>();
            //var tableMetaData = await _repo.GetList<BreMasterTableMetadataViewModel, BreMasterTableMetadata>(x => x.BusinessRuleId == parentId);
            var tableMetaData = await _repo.GetList<BreMasterTableMetadataViewModel, BreMasterTableMetadata>(x => x.BusinessRuleId == bussinessRuleId);
            var tableMeta = await _repo.GetSingle<BreMasterTableMetadataViewModel, BreMasterTableMetadata>(x => x.Id == parentId);
            if (tableMeta.IsNotNull())
            {
                columnMetaData = await _repo.GetList<BreMasterColumnMetadataViewModel, BreMasterColumnMetadata>(x => x.BreMasterTableMetadataId == tableMeta.Id);
            }
            if (parentId.IsNullOrEmpty())
            {
                list.Add(new BreMasterMetadataViewModel
                {
                    Expanded = true,
                    Id = bussinessRuleId,
                    Name = "Master",
                    DisplayName = "Master",
                    ParentId = null,
                    CompanyId = _repo.UserContext.CompanyId,
                    BreInputDataType = BreInputDataTypeEnum.Root,
                    //HasSubFolders = childList != null,
                    HasSubFolders = (tableMetaData != null&&tableMetaData.Count>0),
                    DataType = DataTypeEnum.Object,
                });
                return list;
            }
            else 
            {
                if (parentId==bussinessRuleId)
                {
                    foreach (var item in tableMetaData)
                    {
                        if (item.DataType == DataTypeEnum.Object)
                        {
                            var table = await _repo.GetSingle<TableMetadataViewModel, TableMetadata>(x => x.Id == item.TableMetadataId);
                            var columnList = await _repo.GetList<BreMasterColumnMetadataViewModel, BreMasterColumnMetadata>(x => x.BreMasterTableMetadataId == item.Id);
                            list.Add(new BreMasterMetadataViewModel
                            {
                                Expanded = true,
                                Id = item.Id,
                                CompanyId = item.CompanyId,
                                Name = table.Name,
                                ParentId = item.ParentId,
                                BreInputDataType = item.BreInputDataType,
                                DataType = item.DataType,
                                HasSubFolders = columnList != null
                            });
                        }
                    }
                }
               
                foreach (var item in columnMetaData)
                {

                    var column = await _repo.GetSingle<ColumnMetadataViewModel, ColumnMetadata>(x => x.Id == item.ColumnMetadataId);
                    var table = await _repo.GetSingle<TableMetadataViewModel, TableMetadata>(x => x.Id == column.TableMetadataId);
                    list.Add(new BreMasterMetadataViewModel
                    {
                        Expanded = true,
                        Id = item.Id,
                        CompanyId = item.CompanyId,
                        Name = column.Name,
                        ParentId = item.ParentId,
                        ParentName=table.Alias,
                        BreInputDataType = item.BreInputDataType,
                        DataType = item.DataType,
                        FVDataType = column.DataType,
                    });

                }



            }
            return list.ToList();
        }

        public async Task<List<BreMasterMetadataViewModel>> GetBreMasterMetaDataOld(string bussinessRuleId, string parentId)
        {
            //var list = await GetList(x => x.BreMetadataType == BreMetadataTypeEnum.MasterData && x.BusinessRuleId == bussinessRuleId);
            //list.ForEach(x => x.HasSubFolders = list.Any(y => y.ParentId == x.Name));
            //list.Add(new BreMasterMetadataViewModel
            //{
            //    Expanded = true,
            //    Id = bussinessRuleId,
            //    //Name = bussinessRuleId,
            //    Name = "Master Data",
            //    DisplayName = "Master Data",
            //    ParentId = null,
            //    CompanyId = _repo.UserContext.CompanyId,
            //    BreInputDataType = BreInputDataTypeEnum.Root,
            //    HasSubFolders = list.Any()
            //});

            var list = new List<BreMasterMetadataViewModel>();
            var columnMetaData = new List<BreMasterColumnMetadataViewModel>();
            //var data = await GetList(x => x.BusinessRuleId == bussinessRuleId);
            var tableMetaData = await _repo.GetList<BreMasterTableMetadataViewModel, BreMasterTableMetadata>(x => x.BusinessRuleId == parentId);
            var tableMeta = await _repo.GetSingle<BreMasterTableMetadataViewModel, BreMasterTableMetadata>(x => x.Id == parentId);
            if (tableMeta.IsNotNull())
            {
                columnMetaData = await _repo.GetList<BreMasterColumnMetadataViewModel, BreMasterColumnMetadata>(x => x.BreMasterTableMetadataId == tableMeta.TableMetadataId);
            }
            if (parentId.IsNullOrEmpty())
            {
                //var childList = await GetSingle<BreMasterMetadataViewModel, BreMasterTableMetadata>(x => x.ParentId == bussinessRuleId);
                
                list.Add(new BreMasterMetadataViewModel
                {
                    //Expanded = true,
                    //Id = bussinessRuleId,
                    //Name = "Input Data",
                    //ParentId = null,
                    //CompanyId = _repo.UserContext.CompanyId,
                    //BreInputDataType = BreInputDataTypeEnum.Root,
                    //HasSubFolders = childList != null,
                    //DataType = DataTypeEnum.Object,
                    Expanded = true,
                    Id = bussinessRuleId,                   
                    Name = "Master",
                    DisplayName = "Master",
                    ParentId = null,
                    CompanyId = _repo.UserContext.CompanyId,
                    BreInputDataType = BreInputDataTypeEnum.Root,
                    //HasSubFolders = childList != null,
                    HasSubFolders = tableMetaData != null,
                    DataType = DataTypeEnum.Object,
                });               
                return list;
            }
            else
            {
                //if (tableMetaData.Count > 0)
                //{
                //    list.AddRange(tableMetaData.Select(x => new BreMasterMetadataViewModel
                //    {
                //        Expanded = false,
                //        Id = x.TableMetadataId,
                //        Name = table.Name,
                //        ParentId = x.BusinessRuleId,
                //        CompanyId = _repo.UserContext.CompanyId,
                //        BreInputDataType = x.BreInputDataType,
                //        HasSubFolders = true,
                //        DataType = x.DataType,
                //    }));
                //    //var columnList = await GetList<BreMasterMetadataViewModel, BreMasterColumnMetadata>(x => x.BreMasterTableMetadataId == table.TableMetadataId);
                //    //if (columnList.Count > 0)
                //    //{
                //    //    list.AddRange(columnList);

                //    //}
                //}
                //var table = await _repo.GetSingle<TableMetadataViewModel, TableMetadata>(x => x.Id == tableMetaData.TableMetadataId);
                foreach (var item in tableMetaData)
                {
                    if (item.DataType == DataTypeEnum.Object)
                    {
                        var table = await _repo.GetSingle<TableMetadataViewModel, TableMetadata>(x => x.Id == item.TableMetadataId);
                        var columnList = await _repo.GetList<BreMasterColumnMetadataViewModel, BreMasterColumnMetadata>(x => x.BreMasterTableMetadataId == item.TableMetadataId);
                        list.Add(new BreMasterMetadataViewModel
                        {
                            Expanded = true,
                            Id = item.Id,
                            CompanyId = item.CompanyId,
                            Name = table.Name,
                            ParentId = item.ParentId,
                            BreInputDataType = item.BreInputDataType,
                            DataType = item.DataType,
                            HasSubFolders= columnList!=null
                        });
                    }
                }
                foreach (var item in columnMetaData)
                {
                    
                    var column = await _repo.GetSingle<ColumnMetadataViewModel, ColumnMetadata>(x => x.Id == item.ColumnMetadataId);                    
                    list.Add(new BreMasterMetadataViewModel
                    {
                        Expanded = true,
                        Id = item.Id,
                        CompanyId = item.CompanyId,
                        Name = column.Name,
                        ParentId = item.ParentId,
                        BreInputDataType = item.BreInputDataType,
                        DataType = item.DataType,                        
                    });
                    
                }

                //var newList = tableMetaData.Where(x => x.BusinessRuleId == parentId).ToList();
                //list.AddRange(newList.Select(x => new BreMasterMetadataViewModel
                //{
                //    Expanded = true,
                //    Id = x.Id,
                //    CompanyId = x.CompanyId,
                //    Name =x.TableMetadataId,
                //    ParentId = x.ParentId,
                //    BreInputDataType = x.BreInputDataType,
                //    DataType = x.DataType,
                //}));
                //list.ForEach(x => x.HasSubFolders = tableMetaData.Any(y => y.ParentId == x.Id));

            }
            return list.ToList();
        }
        public async Task<List<BreMasterMetadataViewModel>> GetMasterDataCollectionList()
        {
            var dbCollections = await _repo.GetListGlobal<DataIntegrationViewModel, DataIntegration>(x => x.CompanyCode == _repo.UserContext.CompanyCode);
            var list = dbCollections.Select(x => new BreMasterMetadataViewModel
            {
                Id = x.Id,
                Name = x.CollectionName,
                DisplayName = x.CollectionDisplayName,
                CreatedDate = DateTime.Now,
                LastUpdatedDate = DateTime.Now
            }).ToList();

            return list.ToList();
        }       
        public async Task<List<BreMasterMetadataViewModel>> GetPropertiesFromMasterDataCollection(string collectionId)
        {
            var dbCollection = await _repo.GetSingleGlobal<DataIntegrationViewModel, DataIntegration>(x => x.Id == collectionId && x.CompanyCode == _repo.UserContext.CompanyCode);
            var result = new List<BreMasterMetadataViewModel>();
            var hasProperties = dbCollection != null && dbCollection.Schema != null && dbCollection.Schema.Any();
            //if (dbCollection != null)
            //{
            //    result.Add(new BreMasterMetadataViewModel
            //    {
            //        Id = dbCollection.Id,
            //        Name = dbCollection.CollectionName,
            //        DisplayName = dbCollection.CollectionDisplayName,
            //        DataType = DataTypeEnum.Object,
            //        ParentId = null,
            //        HasSubFolders = hasProperties

            //    });
            //}
            if (hasProperties)
            {
                var schemaList = JsonConvert.DeserializeObject<Dictionary<string, string>>(dbCollection.Schema);
                result.AddRange(schemaList.Select(x => new BreMasterMetadataViewModel
                {
                    Id = x.Key,
                    Name = x.Value,
                    DisplayName = x.Key,
                    DataType = GetDataType(x.Value),
                    ParentId = dbCollection.CollectionName,
                    HasSubFolders = false

                }));
            }
            return result;
        }
        

    }
}
