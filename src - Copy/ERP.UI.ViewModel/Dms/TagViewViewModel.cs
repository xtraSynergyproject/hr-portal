using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class TagViewViewModel:ViewModelBase
    {
        [Required]
        public string Name { get; set; }
        public bool SetAsDefaultView { get; set; }
        public bool IsAllowed { get; set; }
        public string SelectedValues { get; set; }
        public List<FolderViewModel> SelectedFolder { get; set; }
        public List<long> SelectedItems { get; set; }
        public long? SelectedItemId { get; set; }
        public string SelectedItemName { get; set; }
        public string SelectedItemSequence { get; set; }

    }
}
