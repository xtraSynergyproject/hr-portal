﻿using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class OrganizationDocumentsViewModel: OrganizationDocument
    {
        public string Ids { get; set; }
        public string FileName { get; set; }
    }
}