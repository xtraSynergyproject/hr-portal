using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.ViewModel;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public interface IQRCodeBusiness : IBusinessBase<QRCodeDataViewModel, QRCodeData>
    {
        Task<Tuple<string, string, MemoryStream>> GenerateBarCode(string data, QRCodeDataTypeEnum dataType = QRCodeDataTypeEnum.Url, QRCodeTypeEnum codeType = QRCodeTypeEnum.QR_CODE, ReferenceTypeEnum? referenceType = null, string referenceId = null, bool isPopup = false);
    }
}
