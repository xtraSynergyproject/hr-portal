using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;

namespace Synergy.App.ViewModel
{
    public class EGovProjectProposalResponseViewModel
    {        
        public string Id { get; set; }
        public string ProjectProposalId { get; set; }
        public string ResponseByUserId { get; set; }
        public ProjectPropsalResponseEnum ResponseType { get; set; }        

    }
   
}
