using Synergy.App.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class JSCAssetViewModel : ServiceTemplateViewModel
    {
        public string AssetName { get; set; }
        public string AssetTypeId { get; set; }
        public string AssetTypeName { get; set; }
        public string AssetDescription { get; set; }
        public string WardId { get; set; }
        public string WardName { get; set; }
        public string AssetPhotoId { get; set; }
        public string specificLocation { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }   
        public string DDNNO { get; set; }
        public int? OTP { get; set; }
    }
}
