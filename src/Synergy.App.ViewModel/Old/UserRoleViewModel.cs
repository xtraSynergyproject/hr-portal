using Synergy.App.DataModel;
using System;
using System.Collections.Generic;
using Synergy.App.Common;
namespace Synergy.App.ViewModel
{
    public class UserRoleViewModel : UserRole
    {       
        public List<string> UserIds { get; set; }
        public string UserId { get; set; }
        public IList<string> Portal { get; set; }
        public string[] Portals { get; set; }
        public string PortalName { get; set; }
    }
    //public class Permission
    //{
    //    public int Id { get; set; }
    //    public string Code { get; set; }
    //    public string Name { get; set; }
    //    public int? Position { get; set; }
    //}
    public class UserAndRoleViewModel
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }

        public virtual UserViewModel User { get; set; }
        public virtual UserRoleViewModel Role { get; set; }
    }
    public class CredentialType
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int? Position { get; set; }

        public virtual ICollection<Credential> Credentials { get; set; }
    }
    public class Credential
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int CredentialTypeId { get; set; }
        public string Identifier { get; set; }
        public string Secret { get; set; }
        public string Extra { get; set; }

        public virtual User User { get; set; }
        public virtual CredentialType CredentialType { get; set; }

    }
    
}
