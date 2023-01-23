using Synergy.App.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;


namespace Synergy.App.DataModel
{

    public class DocumentESign : DataModelBase
    {
        public string DocumentFileId { get; set; }
        public string DocumentReferenceNo { get; set; }
        public string Key { get; set; }
        public string Transaction { get; set; }
        public string ESignUrl { get; set; }
        public string ReferenceId { get; set; }
        public ReferenceTypeEnum ReferenceType { get; set; }
        public string SignedDocumentFileId { get; set; }

    }
    [Table("DocumentESignLog", Schema = "log")]
    public class DocumentESignLog : DocumentESign
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
