using ERP.Utility;
using System;
using System.Dynamic;
using System.Linq;

namespace ERP.Data.GraphModel
{
  
    public partial class NTS_NoteLog
    {
        public long Id { get; set; }
        public string NoteNo { get; set; }
        public long? VersionNo { get; set; }        
        public string Description { get; set; }
        public string NoteStatusCode { get; set; }
        public NoteReferenceTypeEnum? ReferenceType { get; set; }
        public long? ReferenceTo { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public DateTime? ReminderDate { get; set; }
        public DataOperationEvent? DataOperationEvent { get; set; }
        public string PermissionType { get; set; }
        public string Access { get; set; }
        public string AppliesTo { get; set; }
        public string AssignTo { get; set; }
        public string TagName { get; set; }
    }



}
