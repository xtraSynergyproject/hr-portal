using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class ImageSet
    {
        public string Name { get; set; }
        public string FileType { get; set; }
        public long UserId { get; set; }
        public long NtsId { get; set; }
        public string NtsType { get; set; }
        public string Comment { get; set; }
        [JsonIgnore]
        public List<VideoFile> Images { get; set; }
    }

    public class VideoFile
    {
        public string FileName { get; set; }

        public string MimeType { get; set; }

        public byte[] ImageData { get; set; }

        
    }
}
