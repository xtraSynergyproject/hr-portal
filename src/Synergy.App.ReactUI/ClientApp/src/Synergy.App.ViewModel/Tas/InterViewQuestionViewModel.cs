using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class InterViewQuestionViewModel
    {

        public string Id { get; set; }
        public string SlNo { get; set; }

        public string ProficiencyLevel{ get; set; }
        public string Indicator { get; set; }
        public string Remark { get; set; }
        public string Question { get; set; }
        public string CandidateAnswer { get; set; }
        public string Score { get; set; }
        public string InterviewerComment { get; set; }

        public string NoteSubject { get; set; }
        public string NoteDescription { get; set; }

        public string UserId { get; set; }
        public string ServiceId { get; set; }
        public string NoteId { get; set; }
        public long? SequenceOrder { get; set; }


    }

    public class CandidateIdViewModel { 
    
        public string Id { get; set; } 
        public string UserId { get; set; }
        public string CandidateId { get; set; }
        public string InterviewSheetId { get; set; }


    }
}
