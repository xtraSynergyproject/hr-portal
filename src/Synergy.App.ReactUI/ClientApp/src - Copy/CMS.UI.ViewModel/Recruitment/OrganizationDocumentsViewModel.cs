﻿using CMS.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.UI.ViewModel
{
    public class OrganizationDocumentsViewModel: OrganizationDocument
    {
        public string Ids { get; set; }
        public string FileName { get; set; }
    }
}
