using CMS.Common;
using CMS.Data.Model;
using System;

namespace CMS.UI.ViewModel
{
    public class FileUploadViewModel : ViewModelBase
    {
        public FileUploadViewModel()
        {
            MaxFileSize = 20000000;
           PostUrl = "/Cms/Document/SaveFile";
        }
        public string Property { get; set; }
        public string Value { get; set; }
        public long MaxFileSize { get; set; }
        public string AllowedExtensions { get; set; }
        public string CallbackMethod { get; set; }
        public string PostUrl { get; set; }
    }
}
