namespace ERP.UI.ViewModel
{
    public class DocumentPermissionViewModel : ViewModelBase
    {
        public long DocumentPermissionId { get; set; }
        public long DocumentId { get; set; }
        public long UserId { get; set; }
        public bool CanEdit { get; set; }
        public bool CanView { get; set; }
        public bool CanDelete { get; set; }
    }
}
