using System;

namespace CMS.Data.Model
{
    public class Document : DataModelBase
    {
        public string Name { get; set; }
        public byte[] Content { get; set; }
        public long Length { get; set; }
        public string Extension { get; set; }
        public string ContentType { get; set; }
    }
}
