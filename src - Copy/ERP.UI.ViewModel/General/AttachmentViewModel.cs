using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ERP.UI.ViewModel
{
    public class AttachmentViewModel : ViewModelBase
    {
        //public long AttachmentId { get; set; }

        public long? ReferenceId { get; set; }
        public ReferenceTypeEnum? ReferenceTypeCode { get; set; }
        public NodeEnum? ReferenceNode { get; set; }
        public AttachmentTypeEnum? AttachmentType { get; set; }


        public long? VersionNo { get; set; }
        [Display(Name = "File Name")]
        public string FileName { get; set; }
        public FileViewModel SelectedFile { get; set; }

        [Display(Name = "Document Upload")]
        public long FileId { get; set; }
        public long? LinkId { get; set; }

        public string CSVFileIds { get; set; }
        public bool IsAllowed { get; set; }
    }
}
