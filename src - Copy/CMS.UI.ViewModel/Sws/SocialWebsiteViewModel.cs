using CMS.Common;
using CMS.Data.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;

namespace CMS.UI.ViewModel
{
    public class SocialWebsiteViewModel : NoteTemplateViewModel
    {
        public string socialMediaType { get; set; }       
        public string postType { get; set; }       

    }
}
