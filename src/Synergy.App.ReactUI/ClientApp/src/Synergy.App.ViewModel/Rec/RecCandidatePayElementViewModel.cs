using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.ComponentModel.DataAnnotations;

namespace Synergy.App.ViewModel
{
    public class RecCandidatePayElementViewModel : ViewModelBase
    {       
        public string ElementName { get; set; }
        public double? Value { get; set; }
        public string PayId { get; set; }
        public string NoteId { get; set; }
        public string ElementId { get; set; }
        public string ApplicationId { get; set; }
        public string JsonPayElement { get; set; }
        public string Comment { get; set; }
    }
}
