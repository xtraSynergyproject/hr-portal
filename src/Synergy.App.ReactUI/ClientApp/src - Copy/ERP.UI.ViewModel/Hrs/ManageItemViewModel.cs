using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class ManageItemViewModel : BaseViewModel
    {

        public int Id { get; set; }

       
        [Display(Name = "Created by ")]
        public string NoteCreatedbyme { get; set; }

        [Display(Name = "Shared by ")]
        public string NoteSharedbyme { get; set; }

        [Display(Name = "Shared with ")]
        public string NoteSharedwithme { get; set; }


        [Display(Name = "Assigned to ")]
        public string TaskAssignedtome { get; set; }

        [Display(Name = "Assigned by ")]
        public string TaskAssignedbyme { get; set; }

        [Display(Name = "Shared with ")]
        public string TaskSharedwithme { get; set; }

        [Display(Name = "Shared by ")]
        public string TaskSharedbyme { get; set; }

        [Display(Name = "Assigned to ")]
        public string ServiceAssignedtome { get; set; }

        [Display(Name = "Requested by ")]
        public string ServiceRequestedbyme { get; set; }

        [Display(Name = "Shared with ")]
        public string ServiceSharedwithme { get; set; }

        [Display(Name = "Shared by ")]
        public string ServiceSharedbyme { get; set; }

        [Display(Name = "Delegated to ")]
        public string Delegatedtome { get; set; }

        [Display(Name = "Delegated by ")]
        public string Delegatedbyme { get; set; }

        [Display(Name = "Task(s) Count")]
        public string OnetoOneTask { get; set; }

        [Display(Name = "Note(s) Count")]
        public string OnetoOneNote { get; set; }


    }
}
