using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.ComponentModel.DataAnnotations;

namespace Synergy.App.ViewModel
{
    public class RecruitmentCandidatePayElementViewModel 
    {
       
        public string ElementName { get; set; }
        public double? Value { get; set; }
        public string PayId { get; set; }
        public string Id { get; set; }
        public string ElementId { get; set; }
        public string ApplicationId { get; set; }
        public string JsonPayElement { get; set; }

        


    }
}
