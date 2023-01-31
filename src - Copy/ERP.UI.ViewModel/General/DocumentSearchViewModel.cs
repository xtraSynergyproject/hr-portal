using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;
using ERP.Data.Model;
using ERP.Data.GraphModel;

namespace ERP.UI.ViewModel
{
    public class DocumentSearchViewModel : ViewModelBase
    {
        public long DocumentId { get; set; }
        public long OwnerId { get; set; }
        public string Name { get; set; }
        public string DocumentTypeCode { get; set; }
        public GEN_ListOfValue DocumentType { get; set; }
        public string ReferenceTypeCode { get; set; }
        public GEN_ListOfValue ReferenceType { get; set; }
        public long ReferenceId { get; set; }

        public DateTime? DateOfIssue { get; set; }
        public DateTime? DateOfExpiry { get; set; }
        public string DocumentNo { get; set; }
        public string Description { get; set; }
        public string ApprovedAuthority { get; set; }


    }
}

