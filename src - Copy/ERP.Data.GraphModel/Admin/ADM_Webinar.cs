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
    public partial class ADM_Webinar : NodeBase
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public long? TopBannerId { get; set; }
        public long? LeftBannerId { get; set; }

        public long? BottomBannerId { get; set; }
        public long? BottomRightBannerId { get; set; }

        public long NotificationTemplateId { get; set; }

        public string RegisteredSuccessMessage { get; set; }

    }
  

}