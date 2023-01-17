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
    public interface IOCRMappingBusiness : IBusinessBase<OCRMappingViewModel, OCRMapping>
    {
        Task<OCRMappingViewModel> GetExistingOCRMapping(OCRMappingViewModel model);
        Task<Dictionary<string, object>> GetExtractedData(string fileId, List<OCRMappingViewModel> ocrmodel, Dictionary<string, object> rowdata);
    }
}
