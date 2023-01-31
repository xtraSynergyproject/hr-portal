using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;

namespace Synergy.App.ViewModel
{
    public class GenerateCaptchaViewModel : ViewModelBase
    {
        public string CaptchaId { get; set; }
        public string ReferenceId { get; set; }
        public string ReferenceType { get; set; }
        public string Layout { get; set; }
        public string Style { get; set; }
        public List<string> Styles { get; set; }
        public string Script { get; set; }
        public string BgColor { get; set; }
        public string SelectedImages { get; set; }
        public string Labels { get; set; }
        public string LabelId { get; set; }
        public List<CaptchaImages> Images { get; set; }

    }
    public class CaptchaImages
    {
        public int Number { get; set; }
        public string Base64 { get; set; }
        public string Id { get; set; }
        public string ClassName { get; set; }
        public List<string> ClassList { get; set; }
        public bool IsDummy { get; set; }

    }
}
