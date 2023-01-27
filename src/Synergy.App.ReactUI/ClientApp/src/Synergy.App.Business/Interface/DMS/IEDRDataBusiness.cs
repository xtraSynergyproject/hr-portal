using Synergy.App.ViewModel;
using Synergy.App.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business.Interface.DMS
{
    public interface IEDRDataBusiness //: IBusinessBase<EDRDataViewModel, PMT_EDRData>
    {
        long? GetFileId(EDRMDRFileTypeEnum type);
    }
}
