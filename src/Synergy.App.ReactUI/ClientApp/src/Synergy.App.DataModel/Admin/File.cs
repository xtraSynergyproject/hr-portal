using Synergy.App.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using static Synergy.App.Common.ApplicationConstant;

namespace Synergy.App.DataModel
{
    public class File : DataModelBase
    {
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public string ContentType { get; set; }
        public long ContentLength { get; set; }
        public string ContentBase64 { get; set; }
        public string SnapshotMongoId { get; set; }
        public string Path { get; set; }
        public string AttachmentType { get; set; }
        public string LinkId { get; set; }
        public string AttachmentDescription { get; set; }
        public bool? IsFileViewableFormat { get; set; }
        public string MongoFileId { get; set; }        
       // public string MongoThumbnailFileId { get; set; }
        public string AnnotationsText { get; set; }
        public string FileExtractedText { get; set; }
        public ReferenceTypeEnum? ReferenceTypeCode { get; set; }
        public string ReferenceTypeId { get; set; }
        public string MongoPreviewFileId { get; set; }
    }

    [Table("FileLog", Schema = "log")]
    public class FileLog : File
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
