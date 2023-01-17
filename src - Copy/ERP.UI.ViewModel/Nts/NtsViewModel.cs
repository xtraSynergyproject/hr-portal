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
    public class NtsViewModel 
    {
        public long? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ReferenceNo { get; set; }      
        public long? OwnerUserId { get; set; }
        public string OwnerUserName { get; set; }
        public long? AssigneeUserId { get; set; }
        public string AssigneeUserName { get; set; }
        public string ModuleName { get; set; }
        public long? TemplateMasterId { get; set; }
        public string TemplateMasterName { get; set; }
        public string Status { get; set; }
        [Display(Name = "ExpiryDate", ResourceType = typeof(ERP.Translation.Nts.Note))]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = Constant.Annotation.DateTimeFormat)]
        public DateTime? ExpiryDate { get; set; }
        public string ExpiryDateDisplay
        {
            get
            {
                var d = Humanizer.DateHumanizeExtensions.Humanize(ExpiryDate);
                return d;

            }
        }
        public string NoteOwnerFirstLetter
        {
            get { return (OwnerUserName.First().ToString() ); }
        }
        public string OwnerFirstLetter
        {
            get { return (OwnerUserName != null && OwnerUserName != "") ? OwnerUserName.First().ToString() : ((OwnerUserName != null && OwnerUserName != "") ? OwnerUserName.First().ToString() : ""); }
        }
    }
}
