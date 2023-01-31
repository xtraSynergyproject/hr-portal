using Synergy.App.Common;
using Synergy.App.DataModel;
using System;

namespace Synergy.App.ViewModel
{
    public class FileUploadViewModel : ViewModelBase
    {
        public FileUploadViewModel()
        {
            MaxFileSize = 20000000;
            PostUrl = "/Cms/Document/SaveFile";
            IsExtMessageVisible = false;
        }
        public string Property { get; set; }
        public string Value { get; set; }
        public long MaxFileSize { get; set; }
        public string AllowedExtensions { get; set; }
        public string CallbackMethod { get; set; }
        public string PostUrl { get; set; }
        public bool IsExtMessageVisible { get; set; }
        public string ImageHeight { get; set; }
        public string ImageWidth { get; set; }
        public string ReferenceId { get; set; }
    }
}
