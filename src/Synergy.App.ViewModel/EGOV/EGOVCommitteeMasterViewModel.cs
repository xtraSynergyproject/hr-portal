using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;

namespace Synergy.App.ViewModel
{
    public class EGOVCommitteeMasterViewModel
    {
        public string CommitteeId { get; set; }
        public string CommitteeTitle { get; set; }
        public string CommitteeCode { get; set; }
        public string CommitteeName { get; set; }
        public string ConstitutedUnder { get; set; }
        public string SSCCommitteeMembers { get; set; }
        public string SSCCommitteeFunctions { get; set; }
        public List<EGOVCommitteeMemberViewModel> CommitteeMemberList { get; set; }
        public List<EGOVCommitteeFunctionViewModel> CommitteeFunctionList { get; set; }

    }
    public class EGOVCommitteeMemberViewModel
    {
        public string MemberName { get; set; }
        public string MemberDesignation { get; set; }
        public long? MemberSequenceNo { get; set; }
    }
    public class EGOVCommitteeFunctionViewModel
    {
        public string CommitteeFunction { get; set; }
        public long? FunctionSequenceNo { get; set; }
    }
}
