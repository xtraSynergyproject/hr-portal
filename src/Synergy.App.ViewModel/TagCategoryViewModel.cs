using Synergy.App.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.ViewModel
{
    public class TagCategoryViewModel: NoteTemplateViewModel
    {
        
        public TagCategoryTypeEnum? TagCategoryType { get; set; }
        
        public string TagCategoryCode { get; set; }
        public string TagCategoryName { get; set; }
        public string EnableAutoTag { get; set; }
        public string TagSourceId { get; set; }
        public string TextQueryCode { get; set; }
        public string TextQueryFilter { get; set; }

        public NtsTypeEnum NtsType { get; set; }
        public string NtsId { get; set; }
        public string TagId { get; set; }
        public string TagCategoryId { get; set; }
        public string TagName { get; set; }
        public string Name { get; set; }       
        public string CreatedByName { get; set; }
        public string LastUpdatedByName { get; set; }
        public List<TagCategoryViewModel> Tags { get; set; }
        public bool HasChildren { get; set; }
    }

    public class TagViewModel : NoteTemplateViewModel {
        public string TagSourceReferenceId { get; set; }
    }
}
