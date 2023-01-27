using Synergy.App.DataModel;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public interface IBreMasterMetadataBusiness : IBusinessBase<BreMasterMetadataViewModel, BreMasterTableMetadata>
    {
        public Task<List<BreMasterMetadataViewModel>> GetMasterDataCollectionList();
        public Task<List<BreMasterMetadataViewModel>> GetPropertiesFromMasterDataCollection(string CollectionName);
        public Task<List<BreMasterMetadataViewModel>> GetAllBreMasterMetaData();
        public Task<List<BreMasterMetadataViewModel>> GetBreMasterMetaData(string BussinessRuleId, string ParentId);
        public Task<List<BreMasterMetadataViewModel>> GetBreMasterDataTreeList(string parentId);

    }
}
