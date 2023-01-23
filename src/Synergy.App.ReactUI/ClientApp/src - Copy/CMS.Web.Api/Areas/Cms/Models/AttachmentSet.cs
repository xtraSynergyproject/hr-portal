using CMS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.Web.Api.Areas.Cms.Models
{

    public class AttachmentSet
    {
        public string Name { get; set; }
        public string FileType { get; set; }
        public string UserId { get; set; }
        public string NtsId { get; set; }
        public ReferenceTypeEnum NtsType { get; set; }
        public string Comment { get; set; }
        public bool IsNtsComments { get; set; }
        public List<VideoFile> Images { get; set; }
    }

    public class VideoFile
    {
        public string FileName { get; set; }
        public string MimeType { get; set; }
        public string StringData { get; set; }
        public byte[] ImageData { get; set; }


    }

}
