using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class JMCAssetViewModel: ServiceTemplateViewModel
    {
        public string AssetName { get; set; }
        public string AssetTypeId { get; set; }
        public string AssetTypeName { get; set; }
        public string AssetDescription { get; set; }
        public string WardId { get; set; }
        public string WardName { get; set; }
        public string specificLocation { get; set; }
        public string AssetPhotoId { get; set; }
        public string NtsNoteId { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string AssetId { get; set; }
        public DateTime? AllotmentDate { get; set; }
        public DateTime? AllotmentFromDate { get; set; }
        public DateTime? AllotmentToDate { get; set; }
    }
}
