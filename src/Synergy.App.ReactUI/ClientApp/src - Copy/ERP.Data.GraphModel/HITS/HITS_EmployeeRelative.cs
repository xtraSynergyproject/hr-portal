using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Data.GraphModel
{
    public partial class HITS_EmployeeRelative : NodeBase
    {
        public long ProfileId { get; set; }
        public long EmployeeId { get; set; }
        public string Name { get; set; }
        public string AName { get; set; }
        public string JobName { get; set; }
        public string JobAName { get; set; }
        public long RelProfileId { get; set; }
        public string PrefixName { get; set; }
        public string PrefixAName { get; set; }
        public string RelName { get; set; }
        public string RelAName { get; set; }
        public string RelationName { get; set; }
        public string RelationAName { get; set; }
        public string RelGenderName { get; set; }
        public string RelGenderAName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string H_BirthDate { get; set; }
        public string RelSocStatusName { get; set; }
        public string RelSocStatusAName { get; set; }
        public string NationId { get; set; }
        public string NationName { get; set; }
        public string NationAName { get; set; }
        public string CityId { get; set; }
        public string CityName { get; set; }
        public string CityAName { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string TeleNo1 { get; set; }
        public string Mobile { get; set; }
        public string Zipcode { get; set; }
        public string PrivateEmail { get; set; }
        public string CompanyEmail { get; set; }
        public string ForPassNo { get; set; }
        public DateTime? ForPasDt { get; set; }
        public string H_ForPasDt { get; set; }
        public DateTime? ForPasExp { get; set; }
        public string H_ForPasExp { get; set; }
        public string ForPasFrm { get; set; }
        public string ForResidnt { get; set; }
        public DateTime? ForResDt { get; set; }
        public string H_ForResDt { get; set; }
        public DateTime? ForResExp { get; set; }
        public string H_ForResExp { get; set; }
        public string ForResFrm { get; set; }
        public string Foreigner { get; set; }
    }
}
