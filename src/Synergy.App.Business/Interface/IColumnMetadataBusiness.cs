using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public interface IColumnMetadataBusiness : IBusinessBase<ColumnMetadataViewModel, ColumnMetadata>
    {
        Task<List<ColumnMetadataViewModel>> GetViewableColumnMetadataList(string tableMetadataId, TemplateTypeEnum templateType, bool includeForiegnKeyTableColumns = true);
       // Task<List<ColumnMetadataViewModel>> GetViewableColumnMetadataList(string tableMetadataId, bool includeForiegnKeyTableColumns = true);
        Task<List<ColumnMetadataViewModel>> GetViewableColumnMetadataList(string schemaName, string tableName, bool includeForiegnKeyTableColumns = true);
    }
}
