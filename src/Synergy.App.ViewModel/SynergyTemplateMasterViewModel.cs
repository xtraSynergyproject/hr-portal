using Synergy.App.Common;
using Synergy.App.DataModel;
using System;

namespace Synergy.App.ViewModel
{
    public class SynergyTemplateMasterViewModel
    {
        public long TemplateMasterId { get; set; }
        public long Id { get; set; }
        public NtsTypeEnum NtsType { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public DataOperation? Operation { get; set; }
        public long TemplateCategoryId { get; set; }

    }
}
