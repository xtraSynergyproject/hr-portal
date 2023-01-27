using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class JSCParcelViewModel
    {
        public dynamic gid { get; set; }
        public string ParentId { get; set; }
        public string type { get; set; }
        public dynamic geometry { get; set; }
        public string prop_id { get; set; }
        public string res_stat { get; set; }
        public string road_desc { get; set; }
        public string road_type { get; set; }
        public string own_dtls { get; set; }
        public string own_name { get; set; }
        public string tel_no { get; set; }
        public string e_mail_id { get; set; }
        public string aadhar { get; set; }
        public string ward_no { get; set; }
        public string wrd_name { get; set; }
        public string pcl_id { get; set; }
        public string usg_cat_gf { get; set; }
        public string UserId { get; set; }
        public string sub_loc { get; set; }
        public string permt_add { get; set; }
        public string locality { get; set; }
        public string sector { get; set; }
        public string post_off { get; set; }
        public string pin_code { get; set; }
        public string bu_type { get; set; }
        public string building { get; set; }
        public string FeeType { get; set; }
        public string RevenueType { get; set; }
        public string RevenueTypeId { get; set; }
        public string Amount { get; set; }
        public string Error { get; set; }
        public bool IsGarbageCollected { get; set; }
        public string AssetTypeId { get; set; }

        public string btup_ar_bm { get; set; }
        public string btup_ar_gf { get; set; }
        public string btup_ar_ff { get; set; }
        public string btup_ar_sf { get; set; }
        public string btup_ar_tf { get; set; }
        public string btup_ar_4f { get; set; }
        public string btup_ar_5f { get; set; }
        public string btup_ar_6f { get; set; }

        public string type_bm { get; set; }
        public string type_gf { get; set; }
        public string type_ff { get; set; }
        public string type_sf { get; set; }
        public string type_tf { get; set; }
        public string type_4f { get; set; }
        public string type_5f { get; set; }
        public string type_6f { get; set; }

        public string bld_age { get; set; }

        public string selft_bm { get; set; }
        public string selft_gf { get; set; }
        public string occ_st_ff { get; set; }
        public string occ_st_sf { get; set; }
        public string occ_st_tf { get; set; }
        public string occ_st_4f { get; set; }
        public string occ_st_5f { get; set; }
        public string occ_st_6f { get; set; }

        public string ty_comm_bm { get; set; }
        public string ty_comm_gf { get; set; }
        public string ty_comm_ff { get; set; }
        public string ty_comm_sf { get; set; }
        public string ty_comm_tf { get; set; }
        public string ty_comm_4f { get; set; }
        public string ty_comm_5f { get; set; }
        public string ty_comm_6f { get; set; }
        public string mmi_id { get; set; }

        public string goetype { get; set; }
        public string[] coordinates { get; set; }
        public bool IsSameLocation { get; set; }
        public DateTime? RegisteredDate { get; set; }
        public bool IsDDNExist { get; set; }
        public string AutoNumber { get; set; }
        public string ServiceNo { get; set; }
        public string ServiceId { get; set; }
        public long Count { get; set; }
        public long PendingCount { get; set; }
        public long InProgressCount { get; set; }
        public long NotPertainingCount { get; set; }
        public long DisposedCount { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string CollectorId { get; set; }
        public string Color { get; set; }
        public string GarbageTypeName { get; set; }
        public string PropertyTypeName { get; set; }
    }
}
