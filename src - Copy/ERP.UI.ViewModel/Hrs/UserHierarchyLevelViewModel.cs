
using System;

namespace ERP.UI.ViewModel
{
    public class UserHierarchyLevelViewModel : ViewModelBase
    {         
        public long HierarchyId { get; set; }
        
        public long UserId { get; set; }

        public long? ParentUserId { get; set; }

        public DateTime? EffectiveStartDate { get; set; }
        public DateTime? EffectiveEndDate { get; set; }
        public int HierarchyLevelNo { get; set; }
        public bool IsLatest { get; set; }
        public int OptionNo { get; set; }

    }
}
