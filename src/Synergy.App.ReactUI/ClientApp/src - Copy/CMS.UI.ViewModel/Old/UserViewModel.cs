using CMS.Data.Model;
using System;
using CMS.Common;
using System.Collections.Generic;

namespace CMS.UI.ViewModel
{
    public class UserViewModel : User
    {
        public string CreatedByName { get; set; }
        public string LastUpdatedByName { get; set; }
        public string CompanyName { get; set; }
        public string CompanyCode { get; set; }
        public string ConfirmPassword { get; set; }
        public string UserName { get; set; }
        public string LastName { get; set; }
        public string ReturnUrl { get; set; }
        public string Layout { get; set; }
        public IList<string> Portal { get; set; }
        public string PortalName { get; set; }
        public UserTypeEnum? UserType { get; set; }
        public Tuple<string, bool> Test { get; set; }

        public virtual string UserPortals { get; set; }
        public virtual string LegalEntityCode { get; set; }
        public virtual bool SendWelcomeEmail { get; set; }

       // public virtual string PortalId { get; set; }
        public virtual string  PersonId { get; set; }
        public virtual string PositionId { get; set; }
        public virtual string DepartmentId { get; set; }
        public string EmailText { get; set; }
        public IList<string> UserRole { get; set; }
    }
}
