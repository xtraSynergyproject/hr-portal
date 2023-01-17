using CMS.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.Model
{
    public class NtsStaging : DataModelBase
    {
        public string BatchId { get; set; }
        public string ReferenceId { get; set; }
        public ReferenceTypeEnum ReferenceType { get; set; }
        public string TemplateId { get; set; }
        public string UserId { get; set; }
        public string FileId { get; set; } 
        public NtsStagingEnum StageStatus { get; set; } 
        public string Error { get; set; }         

    }    

}
