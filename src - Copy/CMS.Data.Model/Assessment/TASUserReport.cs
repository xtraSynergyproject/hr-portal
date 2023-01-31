using CMS.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMS.Data.Model
{
    public class TASUserReport : DataModelBase
    {
        public string UserId { get; set; }
        public string OrganizationCode { get; set; }
        public string Entity { get; set; }
        public string NewSide { get; set; }
        public string NationalityCategory { get; set; }
        public string Category { get; set; }
        public string Sex { get; set; }
        public string Age { get; set; }
        public string Position { get; set; }
        public string TypeOfOrganizationalUnit { get; set; }
        public string EmployeeNumber { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string ManagerNumber { get; set; }

        public string ManagerName { get; set; }
        public string Grievances2018 { get; set; }
        public string Grievances2019 { get; set; }
        public string SubordinateGrievances2018 { get; set; }
        public string SubordinateGrievances2019 { get; set; }
        public string RewardsAndIncentives2018 { get; set; }
        public string RewardsAndIncentives2019 { get; set; }
        public string SubordinateRewardsAndIncentives2018 { get; set; }
        public string SubordinateRewardsAndIncentives2019 { get; set; }
        public string Promotions2017 { get; set; }
        public string Promotions2018 { get; set; }
        public string Promotions2019 { get; set; }

        public string SubordinatePromotions2017 { get; set; }
        public string SubordinatePromotions2018 { get; set; }
        public string SubordinatePromotions2019 { get; set; }
        public string Promotions { get; set; }
        public string JobPerformance2017 { get; set; }
        public string JobPerformance2018 { get; set; }
        public string JobPerformance2019 { get; set; }
        public double? AverageJobPerformanceForThreeYears { get; set; }
        public double? FunctionalPerformanceResult { get; set; }
        public string Violations2018 { get; set; }
        public string Violations2019 { get; set; }
        public string SubordinateViolations2018 { get; set; }
        public string SubordinateViolations2019 { get; set; }
        public string Qualification { get; set; }
        public double? ResultOfEducationalQualification { get; set; }
        public string NumberOfTrainingHours2019 { get; set; }
        public string NumberOfSubordinateTrainingHours2019 { get; set; }
        public string UserFullName { get; set; }
        public string MinistryName { get; set; }

        public string Internshipfor2019 { get; set; }


        public string IdNumber { get; set; }
        public string Nationality { get; set; }
        public string DateOfBirth { get; set; }
        public string SocialStatus { get; set; }
        public string DateOfHiring { get; set; }
        public string NumberOfYearsOfService { get; set; }
        public double? ResultOfTheNumberOfYearsOfService { get; set; }
        public string Class { get; set; }
        public string AppointmentStatus { get; set; }
        public string ClassClass { get; set; }
        public string OrganizationalUnit { get; set; }
        public string EmirateOfTheWorkSite { get; set; }
        public string UserJobTitle { get; set; }
        public string FunctionalFamily { get; set; }
        public string FunctionalFamilySubFamily { get; set; }
        public string TypeOfContract { get; set; }
        public string EducationalLevel { get; set; }
        public string CabinetRatingPmo { get; set; }
        public string FunctionalClass { get; set; }
        public string Org1 { get; set; }
        public string Org2 { get; set; }
        public string Org3 { get; set; }
        public string Org4 { get; set; }
        public string Org5 { get; set; }
        public string Org6 { get; set; }
        public string Org7 { get; set; }
        public string Org8 { get; set; }
        public string DatePoint { get; set; }
        public string LastChangeInPosition { get; set; }



        public string OrganizationCodeAr { get; set; }
        public string EntityAr { get; set; }
        public string NewSideAr { get; set; }
        public string NationalityCategoryAr { get; set; }
        public string SexAr { get; set; }
        public string AgeAr { get; set; }
        public string PositionAr { get; set; }
        public string TypeOfOrganizationalUnitAr { get; set; }
        public string EmployeeNumberAr { get; set; }
        public string TheNameOfTheLineManagerAr { get; set; }
        public string TheEmployeeNumberForTheLineManagerAr { get; set; }
        public string MobileNumberAr { get; set; }
        public string EMailAr { get; set; }
        public string Grievances2018Ar { get; set; }
        public string RewardsAndIncentives2018Ar { get; set; }
        public string RewardsAndIncentives2019Ar { get; set; }
        public string Promotions2017Ar { get; set; }
        public string Promotions2018Ar { get; set; }
        public string Promotions2019Ar { get; set; }
        public string JobPerformance2017Ar { get; set; }
        public string JobPerformance2018Ar { get; set; }
        public string JobPerformance2019Ar { get; set; }
        public double? AverageJobPerformanceForThreeYearsAr { get; set; }
        public double? FunctionalPerformanceResultAr { get; set; }
        public string Violations2018Ar { get; set; }
        public string Violations2019Ar { get; set; }
        public string QualificationAr { get; set; }
        public string ResultOfEducationalQualificationAr { get; set; }
        public string NumberOfTrainingHours2019Ar { get; set; }
        public string UserFullNameAr { get; set; }
        public string IdNumberAr { get; set; }
        public string NationalityAr { get; set; }
        public string DateOfBirthAr { get; set; }
        public string SocialStatusAr { get; set; }
        public string DateOfHiringAr { get; set; }
        public string NumberOfYearsOfServiceAr { get; set; }
        public string ResultOfTheNumberOfYearsOfServiceAr { get; set; }

        public string NumberOfYearsOfServiceInSameField { get; set; }
        public string ClassAr { get; set; }
        public string AppointmentStatusAr { get; set; }
        public string ClassClassAr { get; set; }
        public string OrganizationalUnitAr { get; set; }
        public string EmirateOfTheWorkSiteAr { get; set; }
        public string JobTitleAr { get; set; }
        public string FunctionalFamilyAr { get; set; }
        public string FunctionalFamilySubFamilyAr { get; set; }
        public string TypeOfContractAr { get; set; }
        public string EducationalLevelAr { get; set; }
        public string CabinetRatingPmoAr { get; set; }
        public string FunctionalClassAr { get; set; }
        public string Org1Ar { get; set; }
        public string Org2Ar { get; set; }
        public string Org3Ar { get; set; }
        public string Org4Ar { get; set; }
        public string Org5Ar { get; set; }
        public string Org6Ar { get; set; }
        public string Org7Ar { get; set; }
        public string Org8Ar { get; set; }
        public string DatePointAr { get; set; }

        public string MonthYear { get; set; }
        public string GovermentWork { get; set; }
        public string Classification { get; set; }
        public string RegularityUnit { get; set; }
        public string TotalNoOfExperience { get; set; }
        public string TotalNoOfExperienceInSameJob { get; set; }
        public double? FinalResult { get; set; }
        public string EvaluationResultCompareToPeers1 { get; set; }
        public string EvaluationResultCompareToPeers2 { get; set; }
        public double? TotalTechnicalAssessmentResult { get; set; }
        public double? TotalPshycologicalAssessmentResult { get; set; }
        public string ScientificAndSpecializedCertificate { get; set; }
        public string ScientificAndSpecializedTechnicalInterview { get; set; }
        public string GovAbilityToProvideService { get; set; }
        public string ResultOfTechnicalAssessment { get; set; }
        public string ResultOfCaseStudy { get; set; }
        public string FinalPshycologicalResult { get; set; }
        public string PerformanceTrainingHoursIn2019 { get; set; }
        public string PerformanceRewardsAndIncentiveIn2019And2018 { get; set; }
        public string PerformanceSubordinatesTrainingHoursIn2019 { get; set; }
        public string PerformanceSubordinatesRewardsAndIncentiveIn2019And2018 { get; set; }
        public string ResultOfEmployeePerformance { get; set; }
        public string ResultOfScientificPerformnanceGoverment { get; set; }
        public string ResultOfTechnicalAssessmentInSameField { get; set; }

        public string NoOfSubordinateEmployeePromotion2017 { get; set; }
        public string NoOfSubordinateEmployeePromotion2018 { get; set; }
        public string NoOfSubordinateEmployeePromotion2019 { get; set; }





        public string MonthAr { get; set; }
        public string YearAr { get; set; }
        public string TheSideAr { get; set; }
        public string GovermentWorkAr { get; set; }
        public string ClassificationAr { get; set; }
        public string RegularityUnitAr { get; set; }
        public string TotalNoOfExperienceAr { get; set; }
        public string TotalNoOfExperienceInSameJobAr { get; set; }
        public string FinalResultAr { get; set; }
        public string EvaluationResultCompareToPeers1Ar { get; set; }
        public string EvaluationResultCompareToPeers2Ar { get; set; }
        public string TotalTechnicalAssessmentResultAr { get; set; }
        public string TotalPshycologicalAssessmentResultAr { get; set; }
        public string ScientificAndSpecializedCertificateAr { get; set; }
        public string ScientificAndSpecializedTechnicalInterviewAr { get; set; }
        public string GovAbilityToProvideServiceAr { get; set; }
        public string ResultOfTechnicalAssessmentAr { get; set; }
        public string ResultOfCaseStudyAr { get; set; }
        public string FinalPshycologicalResultAr { get; set; }
        public string PerformanceTrainingHoursIn2019Ar { get; set; }
        public string PerformanceRewardsAndIncentiveIn2019And2018Ar { get; set; }
        public string PerformanceSubordinatesTrainingHoursIn2019Ar { get; set; }
        public string PerformanceSubordinatesRewardsAndIncentiveIn2019And2018Ar { get; set; }
        public string ResultOfEmployeePerformanceAr { get; set; }
        public string ResultOfScientificPerformnanceGovermentAr { get; set; }
        public string ResultOfTechnicalAssessmentInSameFieldAr { get; set; }
        public string ListRequired102 { get; set; }


        public string ListRequired102Ar { get; set; }

        public double? TotalScore { get; set; }

        public double? LeadershipScore { get; set; }

        public double? MentalAbilityScore { get; set; }

        public double? TechnicalScore { get; set; }

        public double? CaseStudyScore { get; set; }

        public double? InterviewScore { get; set; }
        public double? ResultOfTechnicalInterview { get; set; }

        public string ILM1 { get; set; }
        public string ILM2 { get; set; }
        public string ILM3 { get; set; }
        public string ILM4 { get; set; }
        public string ILM5 { get; set; }
        public string ILM6 { get; set; }
        public string ILM7 { get; set; }
        public string ILM8 { get; set; }
        public string ILM9 { get; set; }

        public string Control { get; set; }
        public string Commitment { get; set; }
        public string TheChallenge { get; set; }
        public string Altafah { get; set; }
        public long? DepartmentEmployeeCount { get; set; }

        public string ResultComparedEmp1 { get; set; }
        public string ResultComparedEmp2 { get; set; }
        public string ResultComparedEmp3 { get; set; }
        public string ResultComparedEmp4 { get; set; }

        public double? ResultComparedEmpValue1 { get; set; }
        public double? ResultComparedEmpValue2 { get; set; }
        public double? ResultComparedEmpValue3 { get; set; }
        public double? ResultComparedEmpValue4 { get; set; }

        public string IsSubordinateReport { get; set; }
    }
    
}
