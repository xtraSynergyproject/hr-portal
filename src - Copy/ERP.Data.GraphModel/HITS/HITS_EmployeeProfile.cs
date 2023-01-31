using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.Data.GraphModel
{
    public partial class HITS_EmployeeProfile : NodeBase
    {
        public long EmployeeId { get; set; }
        public string ClockNo { get; set; }
        public string Name { get; set; }
        public string AName { get; set; }
        public Nullable<DateTime> BirthDate { get; set; }
        public string H_BirthDate { get; set; }
        public string TeleNo1 { get; set; }
        public string TeleNo2 { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public string SSGroupName { get; set; }
        public string SSGroupAName { get; set; }
        public Nullable<DateTime> FirstHiredDate { get; set; }
        public string H_FirstHiredDate { get; set; }
        public Nullable<DateTime> HireDate { get; set; }
        public string H_HireDate { get; set; }
        public string HrStatusName { get; set; }
        public string HrStatusAName { get; set; }
        public string SocStatusName { get; set; }
        public string SocStatusAName { get; set; }
        public string NatnlNo { get; set; }
        public string GenderName { get; set; }
        public string GenderAName { get; set; }
        public string HrTaxStatusName { get; set; }
        public string HrTaxStatusAName { get; set; }
        public string MilStatusName { get; set; }
        public string MilStatusAName { get; set; }
        public string ScStatusName { get; set; }
        public string ScStatusAName { get; set; }
        public string PositionName { get; set; }
        public string PositionAName { get; set; }
        public string JobName { get; set; }
        public string JobAName { get; set; }
        public string OrganizationName { get; set; }
        public string OrganizationAName { get; set; }
        public string LocationName { get; set; }
        public string LocationAName { get; set; }
        public string GradeName { get; set; }
        public string GradeAName { get; set; }
        public string ContractName { get; set; }
        public string ContractAName { get; set; }
        public string NationId { get; set; }
        public string NationName { get; set; }
        public string NationAName { get; set; }
        public string HrSsAppl { get; set; }
        public string SInsurName { get; set; }
        public string SInsurAName { get; set; }
        public string SsNum { get; set; }
        public string HrSsFixVal { get; set; }
        public string HrSsVarVal { get; set; }
        public string HrSsBonVal { get; set; }
        public string HrPoints { get; set; }
        public string SsBoxNo { get; set; }
        public string EmployerName { get; set; }
        public string EmployerAName { get; set; }
        public string Insoffice { get; set; }
        public string SsInfo { get; set; }
        public Nullable<DateTime> SsDate { get; set; }
        public string H_SsDate { get; set; }
        public string HrBenefit { get; set; }
        public string HrBasicSal { get; set; }
        public string HrTotalSal { get; set; }
        public string HireSal { get; set; }
        public string ContBasSal { get; set; }
        public string ContBasCurName { get; set; }
        public string ContBasCurAName { get; set; }
        public string HrCurrencyName { get; set; }
        public string HrCurrencyAName { get; set; }
        public string ContInfo { get; set; }
        public Nullable<DateTime> ContStart { get; set; }
        public string H_ContStart { get; set; }
        public string ContEnd { get; set; }
        public string H_ContEnd { get; set; }
        public string ContNet { get; set; }
        public Nullable<DateTime> ProbDate { get; set; }
        public string H_ProbDate { get; set; }
        public Nullable<DateTime> TermDate { get; set; }
        public string H_TermDate { get; set; }
        public string VacPackName { get; set; }
        public string VacPackAName { get; set; }
        public string TRuleDesc { get; set; }
        public string TRuleADesc { get; set; }
        public string IdNo { get; set; }
        public string Privacy { get; set; }
        public string CompanyEmail { get; set; }
    }
}
