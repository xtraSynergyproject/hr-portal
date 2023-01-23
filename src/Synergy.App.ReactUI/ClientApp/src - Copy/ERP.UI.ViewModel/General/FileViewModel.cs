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
    [Serializable]
    public class FileViewModel : ViewModelBase
    {
        //public long FileId { get; set; }
        public long DocumentId { get; set; }
        public string AnnotationsText { get; set; }
        public string FileTypeCode { get; set; }
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public string ContentType { get; set; }
        public string Path { get; set; }
        public long ContentLength { get; set; }
        public string ContentBase64 { get; set; }
        public byte[] ContentByte { get; set; }
        public string ThumbNailBase64 { get; set; }
        public AttachmentTypeEnum? AttachmentType { get; set; }
        public long? LinkId { get; set; }
        public bool? IsInPhysicalPath { get; set; }
        public string AttachmentDescription { get; set; }
        public string SourceFile { get; set; }
        public string DestinationFolder { get; set; }
        public bool? IsFileViewableFormat { get; set; }
        public bool? IsGridFsStorage { get; set; }
        public bool EnableSnapshot { get; set; }
        public bool? SkipCompression { get; set; }
        public string StreamVideoPath
        {
            get { return AppSettings.FileWebApiBaseUrl + "api/video"; }
        }
        //        {
        //            get
        //            {
        //                if (FileName.ToUpper().Contains("PDF") || FileName.ToUpper().Contains("DOC") || System.Web.MimeMapping.GetMimeMapping(FileName).StartsWith("image/"))
        //                {
        //                    return true;
        //                }
        //                else
        //                {
        //                    return false;
        //                }
        //            }

        //}


        public DirectoryInfo TempDirectory { get; set; }
        public List<long> FileSnapshotIds { get; set; }
        public string MongoFileId { get; set; }
        public string FileExtractedText { get; set; }
    }
}

