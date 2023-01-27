using CMS.Common;
using CMS.UI.ViewModel;

namespace Cms.UI.ViewModel
{
    public class EDRDataViewModel : ViewModelBase
    {
        public EDRMDRFileTypeEnum FileType { get; set; }
        public string FileId { get; set; }
        public string RevisionNo { get; set; }
        public string Data { get; set; }
        public string Extension { get; set; }
        public string ParentId { get; set; }
        public string WorkspaceId { get; set; }
        public bool IsSkipExistingFile { get; set; }

    }
}
