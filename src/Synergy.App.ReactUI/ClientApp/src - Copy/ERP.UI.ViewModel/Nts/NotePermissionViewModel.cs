using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class NotePermissionViewModel : ViewModelBase
    {
        public long NoteId { get; set; }
        public long? WorkspaceId { get; set; }
        public long? ParentId { get; set; }
        public string NoteType { get; set; }
        [Display(Name = "Permission Type")]
        public DmsPermissionTypeEnum PermissionType { get; set; }
        public DmsAccessEnum Access { get; set; }
        [Display(Name = "Inherited From")]
        public string InheritedFrom { get; set; }
        [Display(Name = "Applies To")]
        public DmsAppliesToEnum AppliesTo { get; set; }
        public long? WorkspacePermissionGroupId { get; set; }
        public long? UserId { get; set; }
        [Display(Name = "User/Permission Group")]
        public string Principal { get; set; }
        public bool? IsInherited { get; set; }
        public bool? Iswoner { get; set; }
        public bool? DisablePermissionInheritance { get; set; }
        public string PermissionTypeName
        {
            get { return PermissionType.Description(); }
        }
        
        public string AccessName
        {
            get { return Access.Description(); }
        }
        
        public string AppliesToName
        {
            get { return AppliesTo.Description(); }
        }
        public long? LegalEntityId { get; set; }
        public bool? IsDocument { get; set; }

        [Display(Name = "ExpiryDate", ResourceType = typeof(ERP.Translation.Nts.Note))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DateTimeFormat)]
        public DateTime? ExpiryDate { get; set; }

        public long? ReferenceId { get; set; }

        public string DocumentName { get; set; }
    }
}
