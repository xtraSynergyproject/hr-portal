using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class UserRoleViewModel :  ViewModelBase
    {
        public long UserRoleId { get; set; }
        [Required]
        [StringLength(200)]
        public string Name { get; set; }

        [StringLength(2000)]
        public string Description { get; set; }

        public virtual string Rules { get; set; }

        [Display(Name = "Geo Location")]
        public string GeoLocations { get; set; }
        [Display(Name="Grade")]
        public string Grades { get; set; }
        [Display(Name = "Organization")]
        public string Orgs { get; set; }
        [Display(Name = "Job")]
        public string Jobs { get; set; }
        [Display(Name = "Position")]
        public string Positions{ get; set; }
        [Display(Name = "Person")]
        public string Persons { get; set; }

        [Display(Name = "Worklist Templates")]
        public string WorklistTemplateMasters { get; set; }
        [Display(Name = "Team Templates")]
        public string TemplateTeams { get; set; }

        public string Module { get; set; }
        public string Submodules { get; set; }
        public string Screens { get; set; }
        //public string Tabs { get; set; }
        //public string Blocks { get; set; }
        public string Actions { get; set; }
        public string Fields { get; set; }
        public string FieldsEditable { get; set; }
        public long[] GeoLocationData { get; set; }
        public long[] GradeData { get; set; }
        public long[] OrgData { get; set; }
        public long[] JobData { get; set; }
        public long[] PosData { get; set; }
        public long[] PerData { get; set; }
        public long[] WorklistTemplateMasterData { get; set; }
        public long[] TemplateTeamData { get; set; }

        public IEnumerable<ModuleViewModel> Modules { get; set; }

        public string Code { get; set; }

    }
}
