using ERP.Utility;
using System;
using System.ComponentModel.DataAnnotations;

namespace ERP.UI.ViewModel
{
    public class TrainingViewModel : ViewModelBase
    {
        public string Name { get; set; }
        [Display(Name= "Training Category")]
        public SpsTrainingCategoryEnum TrainingCategory { get; set; }
    }
}
