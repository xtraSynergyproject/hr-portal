//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using ERP.Utility;
using System;

namespace ERP.Data.GraphModel
{
    public partial class ADM_LicenseRoot : RootNodeBase
    {

    }
    public partial class ADM_License : NodeDatedBase
    {
        public string LicenseKey { get; set; }
        public string ServerIpAddress { get; set; }
        public LicenseTypeEnum LicenseType { get; set; }
        public long? NumberOfUsers { get; set; }
    }

    public class R_LicenseRoot : RelationshipDatedBase
    {

    }
    public class R_License_Customer : RelationshipDatedBase
    {

    }
}
