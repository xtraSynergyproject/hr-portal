using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class ManageItemOrganizationViewModel : BaseViewModel
    {

        public int Id { get; set; }

       
        [Display(Name = "Created by me")]
        public string NoteCreatedbyme { get; set; }

        [Display(Name = "Shared by me")]
        public string NoteSharedbyme { get; set; }

        [Display(Name = "Shared with me")]
        public string NoteSharedwithme { get; set; }


        [Display(Name = "Assigned to me")]
        public string TaskAssignedtome { get; set; }

        [Display(Name = "Assigned by me")]
        public string TaskAssignedbyme { get; set; }

        [Display(Name = "Shared with me")]
        public string TaskSharedwithme { get; set; }

        [Display(Name = "Shared by me")]
        public string TaskSharedbyme { get; set; }

        [Display(Name = "Assigned to me")]
        public string ServiceAssignedtome { get; set; }

        [Display(Name = "Requested by me")]
        public string ServiceRequestedbyme { get; set; }

        [Display(Name = "Shared with me")]
        public string ServiceSharedwithme { get; set; }

        [Display(Name = "Shared by me")]
        public string ServiceSharedbyme { get; set; }

        [Display(Name = "Delegated to me")]
        public string Delegatedtome { get; set; }

        [Display(Name = "Delegated by me")]
        public string Delegatedbyme { get; set; }

        [Display(Name = "One to One Task(s) Count")]
        public string OnetoOneTask { get; set; }

        [Display(Name = "One to One Note(s) Count")]
        public string OnetoOneNote { get; set; }


    }
}
