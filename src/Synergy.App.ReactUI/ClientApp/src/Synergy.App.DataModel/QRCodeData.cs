using Synergy.App.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synergy.App.DataModel

{
    public class QRCodeData : DataModelBase
    {
        public QRCodeDataTypeEnum QRCodeDataType { get; set; }
        public string Data { get; set; }
        public QRCodeTypeEnum QRCodeType { get; set; }
        public string QrCodeUrl { get; set; }
        public ReferenceTypeEnum? ReferenceType { get; set; }
        public string ReferenceTypeId { get; set; }
        public string QRCodeImageId { get; set; }
        public bool IsPopup { get; set; }


    }
    [Table("QRCodeDataLog", Schema = "log")]
    public class QRCodeDataLog : QRCodeData
    {
        public string RecordId { get; set; }
        public long LogVersionNo { get; set; }
        public bool IsLatest { get; set; }
        public DateTime LogStartDate { get; set; }
        public DateTime LogEndDate { get; set; }
        public DateTime LogStartDateTime { get; set; }
       public DateTime LogEndDateTime { get; set; } 
        public bool IsDatedLatest { get; set; }
        public bool IsVersionLatest { get; set; }
    }
}
