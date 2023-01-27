
namespace ERP.Data.GraphModel
{
    using ERP.Utility;
    using System;
    using System.Linq;

    public partial class NTS_Setting : NTSBase
    {
        public ulong SettingId { get; set; }
        public ulong DefaultUserId { get; set; }
        public string DefaultEmailTemplate { get; set; }
    }
}
