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
    public interface IOCRMappingBusiness : IBusinessBase<OCRMappingViewModel, OCRMapping>
    {
        Task<OCRMappingViewModel> GetExistingOCRMapping(OCRMappingViewModel model);
        Task<Dictionary<string, object>> GetExtractedData(string fileId, List<OCRMappingViewModel> ocrmodel, Dictionary<string, object> rowdata);
    }
}
