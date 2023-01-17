using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Synergy.App.ViewModel
{
    public class UserPermissionViewModel : UserPermission
    {
        //public List<string> Groups { get; set; }
        public string[] Columns { get; set; }
        public string[] UserRoleColumns { get; set; }
        public TemplateTypeEnum? PageType { get; set; }
        public string UserName { get; set; }
        public string PortalName { get; set; }
        public string PageName { get; set; }      
        public string Permission { get
            {
                if (Permissions.IsNotNull())
                {
                    return string.Join(",", Permissions);
                }
                return null;
            } 
        }
        public string Type { get; set; }
        public string Permission1 { get; set; }      
        public string Permission2 { get; set; }     
        public string Permission3 { get; set; }
        public string Permission4 { get; set; }
        public string Permission5 { get; set; }
        public string Permission6 { get; set; }
        public string Permission7 { get; set; }
        public string Permission8 { get; set; }
        public string Permission9 { get; set; }
        public string Permission10 { get; set; }
        public string Permission11 { get; set; }
        public string Permission12 { get; set; }
        public string Permission13 { get; set; }
        public string Permission14 { get; set; }
        public string Permission15 { get; set; }
        public string Permission16 { get; set; }
        public string Permission17 { get; set; }
        public string Permission18 { get; set; }
        public string Permission19 { get; set; }
        public string Permission20 { get; set; }

        public bool Check1 { get; set; }
        public bool Check2 { get; set; }
        public bool Check3 { get; set; }
        public bool Check4 { get; set; }
        public bool Check5 { get; set; }
        public bool Check6 { get; set; }
        public bool Check7 { get; set; }
        public bool Check8 { get; set; }
        public bool Check9 { get; set; }
        public bool Check10 { get; set; }
        public bool Check11 { get; set; }
        public bool Check12 { get; set; }
        public bool Check13 { get; set; }
        public bool Check14 { get; set; }
        public bool Check15 { get; set; }
        public bool Check16 { get; set; }
        public bool Check17 { get; set; }
        public bool Check18 { get; set; }
        public bool Check19 { get; set; }
        public bool Check20 { get; set; }
    }
}
