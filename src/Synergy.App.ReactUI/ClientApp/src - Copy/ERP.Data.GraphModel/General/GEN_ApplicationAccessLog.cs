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
using System.Linq;

namespace ERP.Data.GraphModel
{
  
    public partial class GEN_ApplicationAccessLog : NodeBase
    {
        public long? UserId { get; set; }
        public string Email { get; set; }
        public string IqamahNo { get; set; }
        public string MobileNo { get; set; }
        public string PersonFullName { get; set; }
        public string Url { get; set; }
        public string ClientIP { get; set; }
        public AccessLogTypeEnum AccessType { get; set; }
        public ApplicationEnum? Application { get; set; }
        public ModuleEnum? ModuleName { get; set; }
        public string SessionId { get; set; }
        public DateTime LogDate { get; set; }
    }



}
