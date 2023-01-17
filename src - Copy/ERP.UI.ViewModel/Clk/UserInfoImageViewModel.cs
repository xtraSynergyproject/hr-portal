using System;
using ERP.Utility;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class UserInfoImageViewModel : ViewModelBase
    {
        public long UserInfoId { get; set; }
        public string BiometricId { get; set; }
        public UserInfoImageTypeEnum UserImageType { get; set; }
        public int FingerIndex { get; set; }
        public int IFlag { get; set; }
        public string ImageBase64 { get; set; }
        public int ImageLength { get; set; }

    }
}
