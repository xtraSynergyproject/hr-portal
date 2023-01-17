using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;
using System.IO;
using static ERP.Utility.Constant;

namespace ERP.UI.ViewModel
{    
    public class FileApiViewModel 
    {
        public long Id { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public string ContentType { get; set; }
        public long ContentLength { get; set; }
        public string ContentBase64 { get; set; }
        public string ThumbNailBase64 { get; set; }
        public string Path { get; set; }
        public AttachmentTypeEnum? AttachmentType { get; set; }
        public long? LinkId { get; set; }
        public bool IsInPhysicalPath { get; set; }
        public string AttachmentDescription { get; set; }
        public bool? IsFileViewableFormat { get; set; }
        public string MongoFileId { get; set; }
        public string AnnotationsText { get; set; }
        public string FileExtractedText { get; set; }
    }
}

