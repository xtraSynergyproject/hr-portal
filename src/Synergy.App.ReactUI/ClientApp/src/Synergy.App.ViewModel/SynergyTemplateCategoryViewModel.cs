using Synergy.App.Common;
using Synergy.App.DataModel;
using System;
using System.Collections.Generic;

namespace Synergy.App.ViewModel
{
    public class SynergyTemplateCategoryViewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public NtsTypeEnum NtsType { get; set; }
        public string Description { get; set; }        
        public NtsClassificationEnum NtsClassification { get; set; }
        public DataOperation? Operation { get; set; }
       // public List<SynergyTemplateCategoryViewModel> CategoriesList { get; set; }
    }
}
