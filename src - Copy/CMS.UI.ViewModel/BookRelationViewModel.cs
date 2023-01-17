using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CMS.UI.ViewModel
{
    public class BookRealtionViewModel
    {
        public bool Select { get; set; }
        public string Id { get; set; }
        public NtsTypeEnum SourceTableType { get; set; }
        public NtsTypeEnum TargetTableType { get; set; }
        public string TargetTableId { get; set; }
        public string SourceTableId { get; set; }
     
    }
}
