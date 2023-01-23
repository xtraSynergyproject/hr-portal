using Synergy.App.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class TrendingLocationViewModel : NoteTemplateViewModel
    {
        public string latitude { get; set; }
        public string longitude { get; set; }       
        public SocialMediaTypeEnum? socialMediaType { get; set; }

    }
   
}
