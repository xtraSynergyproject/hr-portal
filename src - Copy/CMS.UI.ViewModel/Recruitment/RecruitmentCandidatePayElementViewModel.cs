using CMS.Common;
using CMS.Data.Model;
using System;
using System.ComponentModel.DataAnnotations;

namespace CMS.UI.ViewModel
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
