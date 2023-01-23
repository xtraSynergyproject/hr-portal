using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class DocumentBinderViewModel : NoteSearchViewModel
    {
#pragma warning disable CS0108 // 'DocumentBinderViewModel.BinderCode' hides inherited member 'NoteSearchViewModel.BinderCode'. Use the new keyword if hiding was intended.
        public string BinderCode { get; set; }
#pragma warning restore CS0108 // 'DocumentBinderViewModel.BinderCode' hides inherited member 'NoteSearchViewModel.BinderCode'. Use the new keyword if hiding was intended.
        public string DocumentRangeFrom { get; set; }
        public string DocumentRangeTo { get; set; }
        public long? CountryId { get; set; }
        public string Logo { get; set; }
        public string CountryName { get; set; }
        public string DepartmentName { get; set; }
        public string CityName { get; set; }
    }
}
