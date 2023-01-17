using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ERP.Utility;


namespace ERP.Data.GraphModel
{
    public partial class SAL_PaymentPlan: NodeBase
    {

        public string Name { get; set; }

        public string Description { get; set; }
        public string Plan { get; set; }
        public float? Discount { get; set; }
        public float? OnBooking { get; set; }
        public float? In6Months { get; set; }
        public float? In12Months { get; set; }
        public float? AtHandover { get; set; }
        public float? Afteryear1 { get; set; }
        public float? Afteryear2 { get; set; }
        public float? Afteryear3 { get; set; }
        public float? Afteryear4 { get; set; }
        public float? Afteryear5 { get; set; }

        //public string UnitType { get; set; }

        //public string DLDFee { get; set; }

        //public string OkoudFee { get; set; }
        //public string VAT { get; set; }

        //public string TotalCost { get; set; }

        //public string DownPayment { get; set; }     


        //public string PayMode { get; set; }

        //public string PayStatus { get; set; }
        //public class R_PaymentPlan_UnitType : RelationshipBase
        //{

        //}
        public class R_PaymentPlan_Project : RelationshipBase
        {

        }
    }
}
