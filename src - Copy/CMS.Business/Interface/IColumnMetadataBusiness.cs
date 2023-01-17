using CMS.Common;
using CMS.Data.Model;
using CMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Business
{
    public interface IColumnMetadataBusiness : IBusinessBase<ColumnMetadataViewModel, ColumnMetadata>
    {
        Task<List<ColumnMetadataViewModel>> GetViewableColumnMetadataList(string tableMetadataId, TemplateTypeEnum templateType, bool includeForiegnKeyTableColumns = true);
       // Task<List<ColumnMetadataViewModel>> GetViewableColumnMetadataList(string tableMetadataId, bool includeForiegnKeyTableColumns = true);
        Task<List<ColumnMetadataViewModel>> GetViewableColumnMetadataList(string schemaName, string tableName, bool includeForiegnKeyTableColumns = true);
    }
}
