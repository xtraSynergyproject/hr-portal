using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Synergy.App.Business
{
    public interface ITableMetadataBusiness : IBusinessBase<TableMetadataViewModel, TableMetadata>
    {
        Task<CommandResult<TableMetadataViewModel>> ManageTemplateTable(TemplateViewModel model,bool ignorePermission, string parentTemplateId);
        Task UpdateStaticTables(string tableName = null);
        Task<List<ColumnMetadataViewModel>> GetTableData(string tableMetadataId, string recordId);
        Task<DataRow> GetTableDataByColumn(string templateCode, string templateId, string udfName, string udfValue);
        Task<DataRow> GetTableDataByHeaderId(string templateId, string headerId);
        Task<DataRow> DeleteTableDataByHeaderId(string templateCode, string templateId, string headerId);
        Task<ColumnMetadataViewModel> GetColumnByTableName(string schema, string tableName, string columnName);
        Task<List<ColumnMetadataViewModel>> GetViewColumnByTableName(string schema, string tableName);
        Task EditTableDataByHeaderId(string templateCode, string templateId, string headerId, Dictionary<string, object> columnsToUpdate);
        public List<ColumnMetadataViewModel> AddBaseColumns(TableMetadataViewModel table, IRepositoryBase<TableMetadataViewModel, TableMetadata> _repo, DataActionEnum dataAction);
        public Task ChildComp(JArray comps, TableMetadataViewModel table, int seqNo);
        Task<MemoryStream> GetExcelForTemplateUdf(string templateId);
        Task<MemoryStream> GetExcelForNoteTemplateUdf(string templateId);
        Task<MemoryStream> GetExcelForNoteTemplateData(string templateId);
        Task<MemoryStream> GetExcelForTemplateData(string templateId);
    }
}
