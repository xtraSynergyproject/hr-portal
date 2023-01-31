using AutoMapper;
using CMS.Common;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace CMS.Business
{
    public class CandidateProfileBusiness : BusinessBase<CandidateProfileViewModel, CandidateProfile>, ICandidateProfileBusiness
    {
        private readonly IRepositoryQueryBase<CandidateProfileViewModel> _queryRepo;
        private readonly ICandidateExperienceBusiness _candidateExperienceBusiness;
        private readonly ICandidateEducationalBusiness _candidateEducationalBusiness;
        private readonly IServiceProvider _serviceProvider;
        public CandidateProfileBusiness(IRepositoryBase<CandidateProfileViewModel, CandidateProfile> repo, IMapper autoMapper,
            IRepositoryQueryBase<CandidateProfileViewModel> queryRepo, IServiceProvider serviceProvider,
            ICandidateExperienceBusiness candidateExperienceBusiness,
            ICandidateEducationalBusiness candidateEducationalBusiness) : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;
            _candidateExperienceBusiness = candidateExperienceBusiness;
            _candidateEducationalBusiness = candidateEducationalBusiness;
            _serviceProvider = serviceProvider;
        }

        public async override Task<CommandResult<CandidateProfileViewModel>> Create(CandidateProfileViewModel model)
        {
            var validateName = await IsNameExists(model);
            if (!validateName.IsSuccess)
            {
                return CommandResult<CandidateProfileViewModel>.Instance(model, false, validateName.Messages);
            }
            if (model.UserId.IsNullOrEmpty())
            {
                model.UserId = _repo.UserContext.UserId;
            }

            var result = await base.Create(model);
          
            return CommandResult<CandidateProfileViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async Task<CommandResult<CandidateProfileViewModel>> CreateCandidate(CandidateProfileViewModel model)
        {
            //var validateName = await IsNameExists(model);
            //if (!validateName.IsSuccess)
            //{
            //return CommandResult<CandidateProfileViewModel>.Instance(model, false, "");
            //}
            var result = await base.Create(model);

            return CommandResult<CandidateProfileViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async Task<CommandResult<CandidateProfileViewModel>> EditCandidate(CandidateProfileViewModel model)
        {
            var data = _autoMapper.Map<CandidateProfileViewModel>(model);

            var result = await base.Edit(model);

            return CommandResult<CandidateProfileViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<CandidateProfileViewModel>> Edit(CandidateProfileViewModel model)
        {
            //var data = _autoMapper.Map<CandidateProfileViewModel>(model);
            var validateName = await IsNameExists(model);
            if (!validateName.IsSuccess)
            {
                return CommandResult<CandidateProfileViewModel>.Instance(model, false, validateName.Messages);
            }
            var result = await base.Edit(model);
            if (result.IsSuccess && model.TaskId.IsNotNullAndNotEmpty() && model.CurrentTabInfo == "QualificationInfo")
            {
                var task = await _serviceProvider.GetService<IRecTaskBusiness>().UpdateTaskCandidateId(model.TaskId, result.Item.Id);
            }

            return CommandResult<CandidateProfileViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        private async Task<CommandResult<CandidateProfileViewModel>> IsNameExists(CandidateProfileViewModel model)
        {
            var errorList = new Dictionary<string, string>();
            if (model.CurrentTabInfo == "CandidateInfo")
            {
                if (model.TitleId.IsNullOrEmpty())
                {
                    errorList.Add("Title", "Title field is required.");
                }
                if (model.FirstName.IsNullOrEmpty())
                {
                    errorList.Add("FirstName", "First Name field is required.");
                }
                if (model.LastName.IsNullOrEmpty())
                {
                    errorList.Add("LastName", "Last Name field is required.");
                }
                if (model.Age == null)
                {
                    errorList.Add("Age", "Age field is required.");
                }
                if (!model.BirthDate.IsNotNull())
                {
                    errorList.Add("BirthDate", "Birth Date field is required.");
                }
                if (model.BirthPlace.IsNullOrEmpty())
                {
                    errorList.Add("BirthPlace", "City/Country of Birth field is required.");
                }
                if (model.NationalityId.IsNullOrEmpty())
                {
                    errorList.Add("Nationality", "Nationality field is required.");
                }
                if (model.BloodGroup.IsNullOrEmpty())
                {
                    errorList.Add("BloodGroup", "Blood Group field is required.");
                }
                if (model.Gender.IsNullOrEmpty())
                {
                    errorList.Add("Gender", "Gender field is required.");
                }
                if (model.MaritalStatus.IsNullOrEmpty())
                {
                    errorList.Add("MaritalStatus", "Marital Status field is required.");
                }
                if (model.PassportNumber.IsNullOrEmpty())
                {
                    errorList.Add("PassportNumber", "Passport Number field is required.");
                }
                if (model.PassportIssueCountryId.IsNullOrEmpty())
                {
                    errorList.Add("PassportIssueCountry", "Passport Issue Country field is required.");
                }
                if (!model.PassportExpiryDate.IsNotNull())
                {
                    errorList.Add("PassportExpiryDate", "Passport Expiry Date field is required.");
                }
                if (model.ContactPhoneLocal.IsNullOrEmpty())
                {
                    errorList.Add("ContactPhoneLocal", "Current Phone No field is required.");
                }
                if (model.Email.IsNullOrEmpty())
                {
                    errorList.Add("Email", "Email Address field is required.");
                }
                else
                {
                    var existemail = await _repo.GetSingle(x => x.Email == model.Email && x.Id != model.Id);
                    if (existemail != null)
                    {
                        errorList.Add("Email", "Email already exist.");

                    }
                }
                if (model.Level.IsNullOrZero())
                {
                    model.Level = 1;
                }
            }
            else if (model.CurrentTabInfo == "EmploymentInfo")
            {
                if (model.NetSalary.IsNullOrEmpty())
                {
                    errorList.Add("NetSalary", "Net Salary after Income Tax field is required.");
                }
                if (model.OtherAllowances.IsNullOrEmpty())
                {
                    errorList.Add("OtherAllowances", "Other Allowances field is required..");
                }
                if (!model.TimeRequiredToJoin.IsNotNull())
                {
                    errorList.Add("TimeRequiredToJoin", "Days to join field is required..");
                }
                if (model.Level.IsNotNull() && model.Level<2)
                {
                    model.Level = 2;
                }
            }
            else if (model.CurrentTabInfo == "QualificationInfo")
            {
                if (model.Level.IsNotNull() && model.Level < 3)
                {
                    model.Level = 3;
                }
            }


            if (errorList.Count > 0)
            {
                return CommandResult<CandidateProfileViewModel>.Instance(model, false, errorList);
            }
            return CommandResult<CandidateProfileViewModel>.Instance();
        }

        public async Task<CandidateProfileViewModel> GetCandProfileDetails(string candidateProfileId)
        {
            string query = @$"select pl.*,t.""Name"" as TitleName, n.""Name"" as NationalityName, g.""Name"" as GenderName, m.""Name"" as MaritalStatusName,
                                pp.""Name"" as PassportIssueCountryName, vc.""Name"" as VisaCountryName, vt.""Name"" as VisaTypeName, ocv.""Name"" as OtherCountryVisaName,
                                ocvt.""Name"" as OtherCountryVisaTypeName, cc.""Name"" as CurrentAddressCountryName, pc.""Name"" as PermanentAddressCountryName, sc.""Name"" as SalaryCurrencyName
                                FROM rec.""CandidateProfile"" as pl
                                LEFT JOIN rec.""ListOfValue"" as t ON t.""Id"" = pl.""TitleId""
                                LEFT JOIN cms.""Nationality"" as n ON n.""Id"" = pl.""NationalityId""
                                LEFT JOIN rec.""ListOfValue"" as g ON g.""Id"" = pl.""Gender""
                                LEFT JOIN rec.""ListOfValue"" as m ON m.""Id"" = pl.""MaritalStatus""
                                LEFT JOIN cms.""Country"" as pp ON pp.""Id"" = pl.""PassportIssueCountryId""
                                LEFT JOIN cms.""Country"" as vc ON vc.""Id"" = pl.""VisaCountry""
                                LEFT JOIN rec.""ListOfValue"" as vt ON vt.""Id"" = pl.""VisaType""
                                LEFT JOIN cms.""Country"" as ocv ON ocv.""Id"" = pl.""OtherCountryVisa""
                                LEFT JOIN rec.""ListOfValue"" as ocvt ON ocvt.""Id"" = pl.""OtherCountryVisaType""
                                LEFT JOIN cms.""Country"" as cc ON cc.""Id"" = pl.""CurrentAddressCountryId""
                                LEFT JOIN cms.""Country"" as pc ON pc.""Id"" = pl.""PermanentAddressCountryId""
                                LEFT JOIN cms.""Currency"" as sc ON sc.""Id"" = pl.""NetSalaryCurrency""
                                WHERE pl.""Id"" = '{candidateProfileId}' and pl.""IsDeleted"" = false";

            var queryData = await _queryRepo.ExecuteQuerySingle(query, null);
            return queryData;
        }

        public async Task<CandidateProfileViewModel> GetDocumentsByCandidate(string candidateProfileId)
        {
            string query = @$"select cp.*, pp.""FileName"" as PassportAttachmentName, ac.""FileName"" as AcademicCertificateName, oc.""FileName"" as OtherCertificateName, 
                                cv.""FileName"" as ResumeAttachmentName, cl.""FileName"" as CoverLetterAttachmentName From rec.""CandidateProfile"" as cp
                                Left Join public.""File"" as pp on pp.""Id"" = cp.""PassportAttachmentId""
                                Left Join public.""File"" as ac on ac.""Id"" = cp.""AcademicCertificateId""
                                Left Join public.""File"" as oc on oc.""Id"" = cp.""OtherCertificateId""
                                Left Join public.""File"" as cv on cv.""Id"" = cp.""ResumeId""
                                Left Join public.""File"" as cl on cl.""Id"" = cp.""CoverLetterId""
                                where cp.""Id"" = '{candidateProfileId}'";

            var queryData = await _queryRepo.ExecuteQuerySingle(query, null);
            return queryData;
        }

        public async Task<CandidateProfileViewModel> GetCandidateByUser()
        {
            string query = @$"select cp.* From rec.""CandidateProfile"" as cp
            Join public.""User"" as u on cp.""UserId"" = u.""Id""
            where u.""Id"" = '{_repo.UserContext.UserId}'";
            var result = await _queryRepo.ExecuteQuerySingle(query, null);
            return result;
        }
        public async Task<Tuple<CandidateProfileViewModel, bool>> IsCandidateProfileFilled()
        {
            var result = new Tuple<CandidateProfileViewModel, bool>(null, false);
            string query = @$"select cp.* From rec.""CandidateProfile"" as cp
            Join public.""User"" as u on cp.""UserId"" = u.""Id""
            where u.""Id"" = '{_repo.UserContext.UserId}'";
            var data = await _queryRepo.ExecuteQuerySingle(query, null);
            if (data != null)
            {
                bool IsCandExp = false;
                bool IsCandEdu = false;
                var candExp = await _candidateExperienceBusiness.GetListByCandidate(data.Id);
                if (candExp.IsNotNull() && candExp.Count()>0)
                {
                    IsCandExp = true;
                }
                var candEdu = await _candidateEducationalBusiness.GetListByCandidate(QualificationTypeEnum.Educational, data.Id);
                if (candEdu.IsNotNull() && candEdu.Count() > 0)
                {
                    IsCandEdu = true;
                }
                result = new Tuple<CandidateProfileViewModel, bool>(data, false);
                if (
                    data.FirstName.IsNotNullAndNotEmpty()
                    && data.LastName.IsNotNullAndNotEmpty()
                    && data.Age.IsNotNull()
                    && data.BirthDate != null
                    && data.BirthPlace.IsNotNullAndNotEmpty()
                    && data.BloodGroup.IsNotNullAndNotEmpty()
                    && data.Gender.IsNotNullAndNotEmpty()
                    && data.MaritalStatus.IsNotNullAndNotEmpty()
                    && data.PassportNumber.IsNotNullAndNotEmpty()
                    && data.PassportIssueCountryId.IsNotNullAndNotEmpty()
                    && data.PassportExpiryDate != null
                    && data.ResumeId.IsNotNullAndNotEmpty()
                    && data.ContactPhoneLocal.IsNotNullAndNotEmpty()
                    && data.Email.IsNotNullAndNotEmpty()
                    && IsCandExp==true
                    && data.NetSalary.IsNotNullAndNotEmpty()
                    && data.OtherAllowances.IsNotNullAndNotEmpty()
                    && data.TimeRequiredToJoin!=null
                    && IsCandEdu==true
                    )
                {
                    result = new Tuple<CandidateProfileViewModel, bool>(data, true);
                }
            }
            return result;
        }


        //public async Task<CommandResult<CandidateProfileViewModel>> CreateCandidate(CandidateProfileViewModel model)
        //{            
        //    var result = await base.Create(model);
        //    return CommandResult<CandidateProfileViewModel>.Instance(model, result.IsSuccess, result.Messages);
        //}

        public async Task<List<CandidateProfileViewModel>> GetStaffList(string userId)
        {
            //var query = @"select pl.*,concat_ws('_',j.""Name"", jd.""Id"") as JobAdvertisement FROM rec.""CandidateProfile"" as pl
            //            left join rec.""Application"" as app on app.""CandidateProfileId"" = pl.""Id""
            //            left join rec.""JobAdvertisement"" as jd on jd.""JobId"" = app.""JobId"" and jd.""Status""=1
            //            left join rec.""ListOfValue"" as mt on jd.""ManpowerTypeId"" = mt.""Id""
            //            left join cms.""Job"" as j on j.""Id"" = app.""JobId""
            //            where mt.""Code"" = 'Staff' and pl.""SourceFrom"" = 'Agency' and pl.""AgencyId"" ='" + userId + "'";
            var query = @"select distinct pl.*,concat_ws('_',j.""Name"", app.""JobId"") as JobAdvertisement FROM rec.""CandidateProfile"" as pl
                        left join rec.""Application"" as app on app.""CandidateProfileId"" = pl.""Id""
                        left join rec.""JobAdvertisement"" as jd on jd.""JobId"" = app.""JobId"" and jd.""Status""=1
                        left join cms.""Job"" as j on j.""Id"" = app.""JobId""
                        
                        where j.""ManpowerTypeCode"" = 'Staff' and pl.""SourceFrom"" = 'Agency' and pl.""AgencyId"" ='" + userId + "'";
            var queryData = await _queryRepo.ExecuteQueryList<CandidateProfileViewModel>(query, null);
            return queryData;
        }

        public async Task<List<CandidateProfileViewModel>> GetWorkerList(string userId)
        {
            //var query = @"select pl.*,concat_ws('_',j.""Name"", jd.""Id"") as JobAdvertisement FROM rec.""CandidateProfile"" as pl
            //            left join rec.""Application"" as app on app.""CandidateProfileId"" = pl.""Id""
            //            left join rec.""JobAdvertisement"" as jd on jd.""JobId"" = app.""JobId"" and jd.""Status""=1
            //            left join rec.""ListOfValue"" as mt on jd.""ManpowerTypeId"" = mt.""Id""
            //            left join cms.""Job"" as j on j.""Id"" = app.""JobId""
            //            where mt.""Code"" = 'Worker'and pl.""SourceFrom"" = 'Agency' and pl.""AgencyId"" ='" + userId + "'";
            var query = @"select distinct pl.*,concat_ws('_',j.""Name"", app.""JobId"") as JobAdvertisement FROM rec.""CandidateProfile"" as pl
                        left join rec.""Application"" as app on app.""CandidateProfileId"" = pl.""Id""
                        left join rec.""JobAdvertisement"" as jd on jd.""JobId"" = app.""JobId"" and jd.""Status""=1                        
                        left join cms.""Job"" as j on j.""Id"" = app.""JobId""                      
                        where j.""ManpowerTypeCode"" != 'Staff' and pl.""SourceFrom"" = 'Agency' and pl.""AgencyId"" ='" + userId + "'";
            var queryData = await _queryRepo.ExecuteQueryList<CandidateProfileViewModel>(query, null);
            return queryData;
        }

        public async Task<CandidateProfileViewModel> UpdateCandidateProfileDetails(CandidateProfileViewModel model)
        {
            var query = $@"update rec.""CandidateProfile"" set ""UserId""='{model.UserId}' where ""Id""='{model.Id}'";

            var result = await _queryRepo.ExecuteQuerySingle(query, null);
            return result;
        }
    }
}
