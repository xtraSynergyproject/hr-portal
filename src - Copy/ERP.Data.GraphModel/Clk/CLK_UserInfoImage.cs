using ERP.Utility;
using System;
using System.Collections.Generic;

namespace ERP.Data.GraphModel
{
    public partial class CLK_UserInfoImage : NodeBase
    {
        public UserInfoImageTypeEnum UserImageType { get; set; }
        public int FingerIndex { get; set; }
        public int IFlag { get; set; }
        public string ImageBase64 { get; set; }
        public int ImageLength { get; set; }
    }
    public class R_UserInfoImage_UserInfo : RelationshipBase
    {

    }
}
