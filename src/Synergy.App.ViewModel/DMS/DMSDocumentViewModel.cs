using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
   public class DMSDocumentViewModel
    {

        public string Id { get; set; }
        public string Name { get; set; }
        public string DocumentName { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsArchived { get; set; }
        public string DocumentId { get; set; }
        public string StatusName { get; set; }
        public string NoteStatus { get; set; }

        public string DocumentType { get; set; }
        public string NoteVersionNo { get; set; }
        public string ParentId { get; set; }

        public DateTime UpdatedDate { get; set; }
        public string UploadType { get; set; }
        public string FolderPath { get; set; }

       
        public string UpdatedByUser { get; set; }
        public string NoteId { get; set; }
        public bool Permission { get; set; }
        public string PhotoId { get; set; }
        public string templatecode { get; set; }
        public string TableMetadataId { get; set; }
        public string maxNumber { get; set; }
    }
}
