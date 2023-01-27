using CMS.Data.Model;
using CMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Business
{
    public interface IBreMetadataBusiness : IBusinessBase<BreMetadataViewModel,BreMasterTableMetadata>
    {
        public Task<List<BreMetadataViewModel>> GetBreMetaData(string BussinessRuleId,string Id);
      
    }
}
