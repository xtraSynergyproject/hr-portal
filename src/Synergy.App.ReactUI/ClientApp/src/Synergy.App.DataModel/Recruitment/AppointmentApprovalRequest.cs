using Synergy.App.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.DataModel
{
    [Table("AppointmentApprovalRequest", Schema = "rec")]
    public class AppointmentApprovalRequest : DataModelBase
    {
        public string SelectedCandidate { get; set; }
        public string SelectedSource { get; set; }
        public string InterviewConductedBy { get; set; }
        public DateTime? InterviewDateTime { get; set; }
        public string InterviewVenue { get; set; }
        public string PositionToWhichSelected { get; set; }
        public string SelectedAgainstVacanceOrNewPost { get; set; }
        public string JustificationCommentForNewPost { get; set; }
        public string Comments { get; set; }
        public string AcceptedSalary { get; set; }
        public string FamilyStatus { get; set; }
        public string LeaveCycle { get; set; }
        public string OtherBenefits { get; set; }
        public NtsActionEnum? ApproverAction { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public string RoleId { get; set; }
        public string ApprovedBy { get; set; }

        [ForeignKey("Application")]
        public string ApplicationId { get; set; }
        public Application Application { get; set; }
        public string RemarksByHr { get; set; }

    }
}
