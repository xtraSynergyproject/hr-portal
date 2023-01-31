using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;

namespace Synergy.App.ViewModel
{
    public class SocialWebsiteViewModel : NoteTemplateViewModel
    {
        public string socialMediaType { get; set; }       
        public string postType { get; set; }       

    }
}
