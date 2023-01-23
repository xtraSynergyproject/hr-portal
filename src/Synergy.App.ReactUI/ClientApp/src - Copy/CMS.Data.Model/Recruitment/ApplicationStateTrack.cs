using CMS.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.Model
{
    [Table("ApplicationStateTrack", Schema = "rec")]
    public class ApplicationStateTrack : DataModelBase
    {
        [ForeignKey("Application")]
        public string ApplicationId { get; set; }
        //public Application Application { get; set; }
        [ForeignKey("ApplicationState")]
        public string ApplicationStateId { get; set; }
        public ApplicationState ApplicationState { get; set; }
        [ForeignKey("ApplicationStatus")]
        public string ApplicationStatusId { get; set; }
        public string ApplicationStatusCode { get; set; }
        public ApplicationStatus ApplicationStatus { get; set; }
        [ForeignKey("User")]
        public string ChangedBy { get; set; }
        public User User { get; set; }
        public DateTime? ChangedDate { get; set; }
        public string TaskReferenceId { get; set; }
    }
}
