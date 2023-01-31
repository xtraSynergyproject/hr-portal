using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.ViewModel;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.UI.Web.Areas.Recruitment.Controllers
{
    [Area("Recruitment")]
    public class UploadCandidateController : ApplicationController
    {
        private readonly IWebHostEnvironment _HostEnvironment;
        private readonly IListOfValueBusiness _lovBusiness;
        private ICandidateProfileBusiness _candidateProfileBusiness;
        private ICandidateEducationalBusiness _candidateEducationalBusiness;
        private ICandidateExperienceBusiness _candidateExperienceBusiness;
        private IDocumentBusiness _documentBusiness;
        private IJobAdvertisementBusiness _jobAdvertisementBusiness;
        private IApplicationBusiness _applicationBusiness;
        private IApplicationEducationalBusiness _applicationEducationalBusiness;
        private IApplicationExperienceBusiness _applicationExperienceBusiness;
        private IFileBusiness _fileBusiness;
        private ICandidateExperienceByCountryBusiness _candidateExperienceByCountryBusiness;
        private ICandidateExperienceByOtherBusiness _candidateExperienceByOtherBusiness;
        private IApplicationExperienceByCountryBusiness _applicationExperienceByCountryBusiness;
        private IApplicationExperienceByOtherBusiness _applicationExperienceByOtherBusiness;
        private readonly IUserContext _userContext;
        private readonly IUserBusiness _userBusiness;
        private readonly IJobCriteriaBusiness _jobCriteriaBusiness;

        private IMasterBusiness _masterBusiness;
        public UploadCandidateController(IWebHostEnvironment HostEnvironment, IListOfValueBusiness lovBusiness,
             ICandidateProfileBusiness candidateProfileBusiness, ICandidateEducationalBusiness candidateEducationalBusiness,
            ICandidateExperienceBusiness candidateExperienceBusiness, IMasterBusiness masterBusiness,
            IDocumentBusiness documentBusiness, IJobAdvertisementBusiness jobAdvertisementBusiness,
             IApplicationBusiness applicationBusiness, IApplicationEducationalBusiness applicationEducationalBusiness,
             IApplicationExperienceBusiness applicationExperienceBusiness, IFileBusiness fileBusiness,
             ICandidateExperienceByCountryBusiness candidateExperienceByCountryBusiness,
             ICandidateExperienceByOtherBusiness candidateExperienceByOtherBusiness, IUserContext userContext,
             IUserBusiness userBusiness, IApplicationExperienceByCountryBusiness applicationExperienceByCountryBusiness,
             IApplicationExperienceByOtherBusiness applicationExperienceByOtherBusiness, IJobCriteriaBusiness jobCriteriaBusiness)
        {
            _HostEnvironment = HostEnvironment;
            _lovBusiness = lovBusiness;
            _candidateProfileBusiness = candidateProfileBusiness;
            _candidateEducationalBusiness = candidateEducationalBusiness;
            _candidateExperienceBusiness = candidateExperienceBusiness;
            _masterBusiness = masterBusiness;
            _documentBusiness = documentBusiness;
            _jobAdvertisementBusiness = jobAdvertisementBusiness;
            _applicationBusiness = applicationBusiness;
            _applicationEducationalBusiness = applicationEducationalBusiness;
            _applicationExperienceBusiness = applicationExperienceBusiness;
            _fileBusiness = fileBusiness;
            _candidateExperienceByCountryBusiness = candidateExperienceByCountryBusiness;
            _candidateExperienceByOtherBusiness = candidateExperienceByOtherBusiness;
            _userContext = userContext;
            _userBusiness = userBusiness;
            _applicationExperienceByCountryBusiness = applicationExperienceByCountryBusiness;
            _applicationExperienceByOtherBusiness = applicationExperienceByOtherBusiness;
            _jobCriteriaBusiness = jobCriteriaBusiness;

        }




        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Save(IList<IFormFile> files)
        {
            try
            {
                foreach (var file in files)
                {
                    var ms = new MemoryStream();
                    file.OpenReadStream().CopyTo(ms);

                    var fileName = "";
                    var physicalPath = "";
                    foreach (var f in files)
                    {
                        string webRootPath = _HostEnvironment.WebRootPath;
                        string contentRootPath = _HostEnvironment.ContentRootPath;

                        string path = "";
                        path = Path.Combine(contentRootPath, "Content");


                        fileName = Path.GetFileName(f.FileName);
                        physicalPath = Path.Combine(path, fileName);
                        using Stream fileStream = new FileStream(physicalPath, FileMode.Create);
                        await file.CopyToAsync(fileStream);
                    }
                    var result = await ManageCandidate(physicalPath); //DuplicateRecords(physicalPath); ManageCandidate(physicalPath);
                    if (System.IO.File.Exists(physicalPath))
                    {
                        System.IO.File.Delete(physicalPath);
                    }

                    //if (result.IsSuccess)
                    //{
                    //    Response.Headers.Add("fileId", result.Item.Id);
                    //    Response.Headers.Add("fileName", result.Item.Name);
                    //    return Json(new { success = true, fileId = result.Item.Id, filename = result.Item.Name });
                    //}
                    //else
                    //{
                    //    Response.Headers.Add("status", "false");
                    //    return Content("");
                    //}



                }
            }
            catch (Exception ex)
            {

            }
            return Content("");
        }



        public async Task<List<string>> ManageCandidate(string physicalPath)
        {
            var errorList = new List<string>();
            try
            {
                if (System.IO.File.Exists(physicalPath))
                {
                    using (TextFieldParser parser = new TextFieldParser(physicalPath))
                    {
                        parser.TextFieldType = FieldType.Delimited;
                        parser.SetDelimiters(",");
                        parser.ReadFields();
                        var i = 1;
                        while (!parser.EndOfData)
                        {
                            var fields = parser.ReadFields().ToList();
                            try
                            {
                                var model = new UploadCandidateViewModel();
                                model.Date = fields[1];
                                model.Position = fields[2];
                                model.PositionOther = fields[3];
                                model.HeardAboutUsFrom = fields[4];
                                model.FullName = fields[5];
                                model.PresentAddress = fields[6];
                                model.Telephone = fields[7];
                                model.Mobile = fields[8];
                                model.PermanentAddress = fields[9];
                                model.Email = fields[10];
                                model.DOB = fields[11];
                                model.Gender = fields[12];
                                model.PassportNo = fields[13];
                                model.PassportExpiry = fields[14];
                                model.Visa = fields[15];
                                model.VisaType = fields[16];
                                model.VisaExpiry = fields[17];
                                model.OtherVisa = fields[18];
                                model.OtherCountryVisa = fields[19];
                                model.OtherCountryVisaType = fields[20];
                                model.OtherCountryVisaExpiry = fields[21];
                                model.QatarNocAvailable = fields[22];
                                model.NoQatarNocReason = fields[23];
                                model.DrivingLicenseCountry = fields[24];
                                model.DrivingLicenseType = fields[25];
                                model.DrivingLicenseExpiry = fields[26];
                                model.TotalWorkExperience = fields[27];
                                model.TotalQatarExperience = fields[28];
                                model.TotalGCCExperience = fields[29];
                                model.TotalOtherExperience = fields[30];
                                model.Education = fields[31];
                                model.EducationPassingYear = fields[32];
                                model.EducationInstitute = fields[33];
                                model.EducationCountry = fields[34];
                                model.EducationMarksPercentage = fields[35];
                                model.EducationCourseDetails = fields[36];
                                model.EducationOthers = fields[37];
                                model.EducationOthersDetails = fields[38];
                                model.PresentWorkingCompany = fields[39];
                                model.PresentCompanyDesignation = fields[40];
                                model.PresentCompanyDuties = fields[41];
                                model.PresentCompanyPeriodFrom = fields[42];
                                model.MonthlyGrossSalary = fields[43];
                                model.NoticePeriod = fields[44];
                                model.OtherPositionInterested = fields[45];
                                model.PreviousCompanyWorkingDetails = fields[46];

                                var splitedLink = "https://www.galfarqatar.com.qa/components/com_chronoforms5/chronoforms/uploads/Career/";
                                model.CoveringLetterLink = fields[47].Split(splitedLink)[1];
                                model.ResumeLink = fields[48].Split(splitedLink)[1];
                                model.CerificateLink = fields[49].Split(splitedLink)[1];

                                if (model.PassportNo.IsNotNull() && model.PassportNo != string.Empty)
                                {
                                    var isExist = await _candidateProfileBusiness.GetSingle(x => x.PassportNumber == model.PassportNo);

                                    if (!isExist.IsNotNull())
                                    {

                                        var candidateprofile = new CandidateProfileViewModel
                                        {
                                            FirstName = model.FullName
                                        };

                                        //upload links if Exist
                                        if (model.CoveringLetterLink.IsNotNull() && model.CoveringLetterLink != string.Empty)
                                        {
                                            var res = UploadCandidateFile(model.CoveringLetterLink);
                                            if (res.IsNotNull())
                                            {
                                                candidateprofile.CoverLetterId = res.Result;
                                            }
                                        }

                                        if (model.ResumeLink.IsNotNull() && model.ResumeLink != string.Empty)
                                        {
                                            var res = UploadCandidateFile(model.ResumeLink);
                                            if (res.IsNotNull())
                                            {
                                                candidateprofile.ResumeId = res.Result;
                                            }
                                        }

                                        if (model.CerificateLink.IsNotNull() && model.CerificateLink != string.Empty)
                                        {
                                            var res = UploadCandidateFile(model.CerificateLink);
                                            if (res.IsNotNull())
                                            {
                                                candidateprofile.AcademicCertificateId = res.Result;
                                            }
                                        }

                                        if (model.Gender.IsNotNull())
                                        {
                                            var gen = await _lovBusiness.GetSingle(x => x.Code == model.Gender && x.ListOfValueType == "LOV_GENDER");
                                            if (gen.IsNotNull())
                                            {
                                                candidateprofile.Gender = gen.Id;
                                            }
                                        }
                                        if (model.HeardAboutUsFrom.IsNotNull())
                                        {
                                            var about = await _lovBusiness.GetSingle(x => x.Code == model.HeardAboutUsFrom && x.ListOfValueType == "HEARD_ABOUT_ US_FROM");
                                            if (about.IsNotNull())
                                            {
                                                candidateprofile.HeardAboutUsFrom = about.Id;
                                            }

                                        }
                                        candidateprofile.Designation = model.Position;
                                        candidateprofile.CurrentAddress = model.PresentAddress;
                                        candidateprofile.PermanentAddress = model.PermanentAddress;
                                        candidateprofile.ContactPhoneHome = model.Telephone;
                                        candidateprofile.ContactPhoneLocal = model.Mobile;
                                        candidateprofile.Email = model.Email;
                                        //candidateprofile.AgencyId = _userContext.UserId;
                                        candidateprofile.SourceFrom = SourceTypeEnum.Migrated.ToString();


                                        if (model.DOB.IsNotNull() && model.DOB != string.Empty)
                                        {
                                            candidateprofile.BirthDate = Convert.ToDateTime(model.DOB);
                                        }

                                        candidateprofile.PassportNumber = model.PassportNo;
                                        if (model.PassportExpiry.IsNotNull() && model.PassportExpiry != string.Empty)
                                        {
                                            candidateprofile.PassportExpiryDate = Convert.ToDateTime(model.PassportExpiry);
                                        }
                                        if (model.Visa.IsNotNull() && model.Visa != string.Empty)
                                        {
                                            var countryList = await _masterBusiness.GetIdNameList("Country");
                                            if (countryList.IsNotNull())
                                            {
                                                var selectedCountry = countryList.Where(x => x.Name == model.Visa).FirstOrDefault();
                                                if (selectedCountry.IsNotNull())
                                                {
                                                    candidateprofile.VisaCountry = selectedCountry.Id;
                                                }

                                            }
                                        }
                                        if (model.VisaExpiry.IsNotNull() && model.VisaExpiry != string.Empty)
                                        {
                                            candidateprofile.VisaExpiry = Convert.ToDateTime(model.VisaExpiry);
                                        }
                                        if (model.VisaType.IsNotNull())
                                        {
                                            var vt = await _lovBusiness.GetSingle(x => x.Name == model.VisaType && x.ListOfValueType == "LOV_VISATYPE");
                                            if (vt.IsNotNull())
                                            {
                                                candidateprofile.VisaType = vt.Id;
                                            }
                                        }

                                        if (model.OtherCountryVisa.IsNotNull() && model.OtherCountryVisa != string.Empty)
                                        {
                                            var countryList = await _masterBusiness.GetIdNameList("Country");
                                            if (countryList.IsNotNull())
                                            {
                                                var selectedCountry = countryList.Where(x => x.Name == model.OtherCountryVisa).FirstOrDefault();
                                                if (selectedCountry.IsNotNull())
                                                {
                                                    candidateprofile.VisaCountry = selectedCountry.Id;
                                                }

                                            }
                                        }
                                        if (model.OtherCountryVisaExpiry.IsNotNull() && model.OtherCountryVisaExpiry != string.Empty)
                                        {
                                            candidateprofile.OtherCountryVisaExpiry = Convert.ToDateTime(model.OtherCountryVisaExpiry);
                                        }
                                        if (model.OtherCountryVisaType.IsNotNull())
                                        {
                                            var vt = await _lovBusiness.GetSingle(x => x.Name == model.OtherCountryVisaType && x.ListOfValueType == "LOV_VISATYPE");
                                            if (vt.IsNotNull())
                                            {
                                                candidateprofile.OtherCountryVisaType = vt.Id;
                                            }
                                        }
                                        candidateprofile.QatarNocAvailable = model.QatarNocAvailable;
                                        candidateprofile.QatarNocNotAvailableReason = model.NoQatarNocReason;
                                        candidateprofile.TotalWorkExperience = Convert.ToDouble(model.TotalWorkExperience);
                                        //candidateprofile.TotalQatarExperience = model.TotalQatarExperience;
                                        //candidateprofile.TotalOtherExperience = model.TotalOtherExperience;
                                        //candidateprofile.TotalGCCExperience = model.TotalGCCExperience;
                                        candidateprofile.NetSalary = model.MonthlyGrossSalary;
                                        candidateprofile.NoticePeriod = model.NoticePeriod;
                                        candidateprofile.OptionForAnotherPosition = model.OtherPositionInterested;

                                        var userViewModel = new UserViewModel();
                                        if (candidateprofile.Email.IsNotNull())
                                        {
                                            var random = new Random();
                                            var Pass = Convert.ToString(random.Next(10000000, 99999999));
                                            userViewModel.Email = candidateprofile.Email;
                                            userViewModel.Name = candidateprofile.FirstName;
                                            userViewModel.CreatedBy = _userContext.UserId;
                                            userViewModel.CreatedDate = DateTime.Now;
                                            userViewModel.Status = StatusEnum.Active;
                                            userViewModel.Password = Pass;
                                            userViewModel.PortalName = "CareerPortal";
                                            userViewModel.UserType = UserTypeEnum.CANDIDATE;
                                            userViewModel.UserName = candidateprofile.FirstName;
                                            userViewModel.ConfirmPassword = Pass;
                                        }
                                        var userResult = await _userBusiness.Create(userViewModel);
                                        if (userResult.IsNotNull())
                                        {
                                            candidateprofile.UserId = userResult.Item.Id;
                                        }

                                        var resultCandidateProfile = await _candidateProfileBusiness.CreateCandidate(candidateprofile);



                                        if (resultCandidateProfile.IsSuccess)
                                        {
                                            var countryId = string.Empty;
                                            if (model.EducationCountry.IsNotNull() && model.EducationCountry != string.Empty)
                                            {
                                                var countryList = await _masterBusiness.GetIdNameList("Country");
                                                if (countryList.IsNotNull())
                                                {
                                                    var selectedCountry = countryList.Where(x => x.Name == model.EducationCountry).FirstOrDefault();
                                                    if (selectedCountry.IsNotNull())
                                                    {
                                                        countryId = selectedCountry.Id;
                                                    }

                                                }
                                            }

                                            var qualification = "";
                                            if (model.Education.IsNotNull())
                                            {
                                                model.Education = model.Education.Replace("\"", "");
                                                var vt = await _lovBusiness.GetSingle(x => x.Name == model.Education && x.ListOfValueType == "LOV_QUALIFICATION");
                                                if (vt.IsNotNull())
                                                {
                                                    qualification = vt.Id;
                                                }
                                            }

                                            var candidateEducation = await _candidateEducationalBusiness.Create(new CandidateEducationalViewModel
                                            {
                                                CandidateProfileId = resultCandidateProfile.Item.Id.ToString(),
                                                QualificationId = qualification, // List of Value - QualifictaionId
                                                QualificationType = QualificationTypeEnum.Educational,
                                                Specialization = model.EducationCourseDetails,
                                                PassingYear = model.EducationPassingYear,
                                                Institute = model.EducationInstitute,
                                                CountryName = model.EducationCountry,
                                                CountryId = countryId,
                                                Marks = model.EducationMarksPercentage,
                                            });

                                            var qatarCountryId = string.Empty;

                                            var countryListData = await _masterBusiness.GetIdNameList("Country");
                                            if (countryListData.IsNotNull())
                                            {
                                                var selectedCountry = countryListData.Where(x => x.Code == "Qatar").FirstOrDefault();
                                                if (selectedCountry.IsNotNull())
                                                {
                                                    qatarCountryId = selectedCountry.Id;
                                                }

                                            }
                                            var candidateExpCountry = await _candidateExperienceByCountryBusiness.Create(new CandidateExperienceByCountryViewModel
                                            {
                                                CountryId = qatarCountryId,
                                                CandidateProfileId = resultCandidateProfile.Item.Id.ToString(),
                                                NoOfYear = Convert.ToDouble(model.TotalQatarExperience)
                                            });



                                            var candidateExperience = await _candidateExperienceBusiness.Create(new CandidateExperienceViewModel
                                            {
                                                CandidateProfileId = resultCandidateProfile.Item.Id.ToString(),
                                                Employer = model.PresentWorkingCompany,
                                                JobTitle = model.PresentCompanyDesignation,
                                                Responsibilities = model.PresentCompanyDuties,
                                                From = (DateTime.TryParse(model.PresentCompanyPeriodFrom, out DateTime Temp) == true) ? Convert.ToDateTime(model.PresentCompanyPeriodFrom) : null,
                                            });

                                            var GCCExp = await _lovBusiness.GetSingle(x => x.Code == "Gulf" && x.ListOfValueType == "LOV_Country");
                                            if (GCCExp.IsNotNull())
                                            {
                                                var candidateExpOther = await _candidateExperienceByOtherBusiness.Create(new CandidateExperienceByOtherViewModel
                                                {
                                                    CompanyId = GCCExp.CompanyId,
                                                    NoOfYear = Convert.ToDouble(model.TotalGCCExperience)
                                                }); ;
                                            }

                                            var othetExp = await _lovBusiness.GetSingle(x => x.Code == "Other" && x.ListOfValueType == "LOV_Country");
                                            if (othetExp.IsNotNull())
                                            {
                                                var candidateExpOther = await _candidateExperienceByOtherBusiness.Create(new CandidateExperienceByOtherViewModel
                                                {
                                                    CompanyId = othetExp.CompanyId,
                                                    NoOfYear = Convert.ToDouble(model.TotalOtherExperience)
                                                }); ;
                                            }



                                        }
                                    }
                                }

                                i++;
                            }
                            catch (Exception ex)
                            {
                                var path = @"D:\\Sumbul\\Project\\Document\\Migration_CMS\\error1.txt";
                                System.IO.File.AppendAllText(path, ex.Message + Environment.NewLine);
                            }
                        }
                    }
                }
                return errorList;
            }
            catch (Exception ex)
            {
                var path = @"D:\\Sumbul\\Project\\Document\\Migration_CMS\\error1.txt";

                System.IO.File.AppendAllText(path, ex.Message + Environment.NewLine);
                return null;
            }
        }

        private async Task<string> UploadCandidateFile(string FileName)
        {
            try
            {
                //var fileName = "D:\\Upload\\abcd.txt";
                var fileName = "D:\\Sumbul\\Project\\Document\\Migration_CMS\\" + FileName;

                //var takeActionLink = _configuration.GetValue<string>(
                //"ApplicationBaseUrl") + "/CMS/Index?taskId=" + model.Id + "&assignTo=" + model.AssignedToUserId + "&teamId=" + model.AssignedToTeamId;


                using FileStream uploadFileStream = System.IO.File.OpenRead(fileName);
                if (uploadFileStream.IsNotNull())
                {
                    try
                    {
                        //foreach (var file in files)
                        //{
                        var ms = new MemoryStream();
                        uploadFileStream.CopyTo(ms);

                        byte[] fileBytes = new byte[uploadFileStream.Length];


                        var result = await _fileBusiness.Create(new FileViewModel
                        {
                            ContentByte = ms.ToArray(),
                            ContentType = "application/octet-stream",//file.ContentType,
                            ContentLength = uploadFileStream.Length,
                            FileName = FileName,
                            FileExtension = Path.GetExtension(FileName)

                        });

                        if (result.IsSuccess)
                        {
                            return result.Item.Id;
                        }
                        else
                        {
                            return null;
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                var path = @"D:\\Sumbul\\Project\\Document\\Migration_CMS\\error.txt";

                System.IO.File.AppendAllText(path, ex.Message + Environment.NewLine);
                return null;
            }
        }

        public async Task<List<string>> DuplicateRecords(string physicalPath)
        {
            var errorList = new List<string>();
            try
            {
                if (System.IO.File.Exists(physicalPath))
                {
                    using (TextFieldParser parser = new TextFieldParser(physicalPath))
                    {
                        parser.TextFieldType = FieldType.Delimited;
                        parser.SetDelimiters(",");
                        parser.ReadFields();
                        var i = 1;
                        while (!parser.EndOfData)
                        {
                            var fields = parser.ReadFields().ToList();
                            try
                            {
                                var model = new UploadCandidateViewModel();

                                model.PassportNo = fields[13];

                                var isExist = await _candidateProfileBusiness.GetSingle(x => x.PassportNumber == model.PassportNo);

                                if (isExist.IsNotNull())
                                {
                                    var path = @"D:\\Sumbul\\Project\\Document\\Migration_CMS\\duplicate.txt";

                                    System.IO.File.AppendAllText(path, i++ + " " + model.PassportNo + " " + Environment.NewLine);
                                }
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }
                }
                return errorList;
            }
            catch (Exception ex)
            {
                var path = @"D:\\Sumbul\\Project\\Document\\Migration_CMS\\error1.txt";

                System.IO.File.AppendAllText(path, ex.Message + Environment.NewLine);
                return null;
            }
        }

        public IActionResult StaffCandidateSheetView(string jobAdvId)
        {
            ViewBag.JobAdvId = jobAdvId;
            return View();
        }
        public async Task<IActionResult> ReadStaffDetails([DataSourceRequest] DataSourceRequest request)
        {
            var result = new List<StaffCandidateViewModel>();
            //var candidateProfile = await _candidateProfileBusiness.GetStaffList(_userContext.UserId);
            //if (candidateProfile.IsNotNull() && candidateProfile.Count() > 0)
            //{
            //    foreach (var can in candidateProfile)
            //    {
            //        var res = new StaffCandidateViewModel
            //        {
            //            CandidateName = can.FirstName,
            //            PassportNumber = can.PassportNumber,
            //            Nationality = can.NationalityId,
            //            ContactNumber = can.ContactPhoneHome,
            //            EmailId = can.Email,
            //            TotalExperience = can.TotalWorkExperience.ToString(),
            //            PresentSalary = can.NetSalary,
            //            //Currency
            //            // res.ExpectedSalary = can.Expecte
            //            NoticePeriod = can.NoticePeriod,
            //            CurrentLocation = can.CurrentAddress,
            //            ResumeLink = can.ResumeAttachmentName,
            //            CVSendDate = can.CreatedDate.ToDatabaseDateFormat(),
            //            JobAdvertisement = can.JobAdvertisement,
            //            ExpectedSalary = can.ExpectedSalary
            //        };

            //        if (res.Nationality.IsNotNullAndNotEmpty())
            //        {
            //            var nation = await _masterBusiness.GetIdNameList("Nationality");
            //            if (nation.IsNotNull())
            //            {
            //                var re = nation.Where(x => x.Id == res.Nationality).FirstOrDefault();
            //                if (re.IsNotNull())
            //                {
            //                    res.Nationality = re.Name + "_" + re.Id;
            //                }
            //            }
            //        }

            //        //if (res.JobAdvertisement.IsNotNullAndNotEmpty())
            //        //{
            //        //    var jd = await _jobAdvertisementBusiness.GetJobIdNameList();
            //        //    if (jd.IsNotNull())
            //        //    {
            //        //        var re = jd.Where(x => x.Id == res.JobAdvertisement).FirstOrDefault();
            //        //        if (re.IsNotNull())
            //        //        {
            //        //            res.JobAdvertisement = re.JobName + "_" + re.Id;
            //        //        }
            //        //    }
            //        //}


            //        var jd = await _masterBusiness.GetJobNameById(res.JobAdvertisement.Split("_")[^1]);
            //        if (jd.IsNotNull())
            //        {
            //            res.JobAdvertisement = jd.Name + "_" + res.JobAdvertisement.Split("_")[^1];
            //        }


            //        var candidateQualification = await _candidateEducationalBusiness.GetSingle(x => x.CandidateProfileId == can.Id);
            //        if (candidateQualification.IsNotNull())
            //        {
            //            res.Qualification = candidateQualification.OtherQualification;
            //        }

            //        var candidateExperience = await _candidateExperienceBusiness.GetSingle(x => x.CandidateProfileId == can.Id);
            //        if (candidateExperience.IsNotNull())
            //        {
            //            res.PresentEmployer = candidateExperience.Employer;
            //            res.Designation = candidateExperience.JobTitle;
            //        }

            //        var resume = await _fileBusiness.GetSingle(x => x.Id == can.ResumeId);
            //        if (resume.IsNotNull())
            //        {
            //            res.ResumeLink = resume.FileName + "_" + resume.Id;
            //        }

            //        var pass = await _fileBusiness.GetSingle(x => x.Id == can.PassportAttachmentId);
            //        if (pass.IsNotNull())
            //        {
            //            res.PassportLink = pass.FileName + "_" + pass.Id;
            //        }

            //        var candidateExp = await _candidateExperienceBusiness.GetSingle(x => x.CandidateProfileId == can.Id);
            //        if (candidateExp.IsNotNull())
            //        {
            //            var a = await _fileBusiness.GetSingle(x => x.Id == candidateExp.AttachmentId);
            //            if (a.IsNotNull())
            //            {
            //                res.ExperienceLetterLink = a.FileName + "_" + a.Id;
            //            }
            //        }

            //        var candidateEducation = await _candidateEducationalBusiness.GetSingle(x => x.CandidateProfileId == can.Id);
            //        if (candidateEducation.IsNotNull())
            //        {

            //            var a = await _fileBusiness.GetSingle(x => x.Id == candidateEducation.AttachmentId);
            //            if (a.IsNotNull())
            //            {
            //                res.EducationLink = a.FileName + "_" + a.Id;
            //            }
            //        }
            //        var other = await _fileBusiness.GetSingle(x => x.Id == can.OtherCertificateId);
            //        if (other.IsNotNull())
            //        {
            //            res.DocumentLink = other.FileName + "_" + other.Id;
            //        }
            //        result.Add(res);

            //    }



            //    return Json(result.ToDataSourceResult(request));

            //}
            //else
            //{
            result.Add(new StaffCandidateViewModel
            {
                CandidateName = "",
                PassportNumber = ""
            });
            return Json(result.ToDataSourceResult(request));
        }

        [HttpPost]
        public async Task<IActionResult> ManageStaffDetails(StaffCandidateSubmitViewModel model)
        {
            var result = new StaffCandidateSubmitViewModel()
            {
                Created = new List<StaffCandidateViewModel>(),
                Updated = new List<StaffCandidateViewModel>(),
                Destroyed = new List<StaffCandidateViewModel>()
            };

            if (model.IsNotNull())
            {
                foreach (var can in model.Created)
                {
                    if (can.PassportNumber.IsNotNull() && can.Nationality.IsNotNull())
                    {
                        var nation = can.Nationality.Split("_")[^1];
                        var data = await _candidateProfileBusiness.GetList(x => x.PassportNumber == can.PassportNumber);
                        if (data.Count > 0)
                        {
                            return Json(new { success = false, error = "Passport No - "+ can.PassportNumber +" Already Exist" });
                        }
                    }
                }
                var res = await _applicationBusiness.CreateStaffCandidateAndApplication(model);
                if (res)
                {
                    return Json(new { success = true });
                }
               
            }

            return Json(new { success = false });
        }

        public async Task<ActionResult> GetJobAdvertisment(string jobAdvId)
        {
            var result = await _jobAdvertisementBusiness.GetJobIdNameList();
            if (jobAdvId.IsNotNull())
            {
                result = result.Where(x => x.JobAdvId == jobAdvId).ToList();
            }
            var name = new List<string>();
            if (result.IsNotNull())
            {
                foreach (var i in result.Where(x => x.ManpowerTypeName == "Staff"))
                {
                    var str = i.JobName + "_" + i.JobAdvId;
                    name.Add(str);
                }
                string res = string.Join(",", name);
                return Json(res);
            }
            return Json("");
        }

        public async Task<ActionResult> GetNationality()
        {
            var nationality = await _masterBusiness.GetIdNameList("Nationality");
            var name = new List<string>();
            if (nationality.IsNotNull())
            {
                foreach (var i in nationality)
                {
                    var str = i.Name + "_" + i.Id;
                    name.Add(str);
                }
                string res = string.Join(",", name);
                return Json(res);
            }
            return Json("");
        }


        public async Task<ActionResult> GetCountry()
        {
            var nationality = await _masterBusiness.GetIdNameList("Country");
            var name = new List<string>();
            if (nationality.IsNotNull())
            {
                foreach (var i in nationality)
                {
                    var str = i.Name + "_" + i.Id;
                    name.Add(str);
                }
                string res = string.Join(",", name);
                return Json(res);
            }
            return Json("");
        }
        public async Task<ActionResult> GetPassportStatus()
        {
            var data = await _lovBusiness.GetList(x => x.ListOfValueType == "LOV_PASSPORTSTATUS");
            var name = new List<string>();
            if (data.IsNotNull())
            {
                foreach (var i in data)
                {
                    var str = i.Name + "_" + i.Id;
                    name.Add(str);
                }
                string res = string.Join(",", name);
                return Json(res);
            }
            return Json("");
        }
        public async Task<ActionResult> GetVisaType()
        {
            var data = await _lovBusiness.GetList(x => x.ListOfValueType == "LOV_VISATYPE");
            var name = new List<string>();
            if (data.IsNotNull())
            {
                foreach (var i in data)
                {
                    var str = i.Name + "_" + i.Id;
                    name.Add(str);
                }
                string res = string.Join(",", name);
                return Json(res);
            }
            return Json("");
        }
        public async Task<ActionResult> GetMaritalStatus()
        {
            var data = await _lovBusiness.GetList(x => x.ListOfValueType == "LOV_MARITALSTATUS");
            var name = new List<string>();
            if (data.IsNotNull())
            {
                foreach (var i in data)
                {
                    var str = i.Name + "_" + i.Id;
                    name.Add(str);
                }
                string res = string.Join(",", name);
                return Json(res);
            }
            return Json("");
        }
        public IActionResult WorkerCandidateSheetView(string jobAdvId)
        {
            ViewBag.JobAdvId = jobAdvId;
            return View();
        }

        public async Task<IActionResult> ReadWorkerDetails([DataSourceRequest] DataSourceRequest request)
        {
            var result = new List<WorkerCandidateViewModel>();
            //var candidateProfile = await _candidateProfileBusiness.GetWorkerList(_userContext.UserId);
            //if (candidateProfile.IsNotNull() && candidateProfile.Count() > 0)
            //{
            //    foreach (var can in candidateProfile)
            //    {
            //        var res = new WorkerCandidateViewModel
            //        {
            //            CandidateName = can.FirstName,
            //            DOB = can.BirthDate.ToString(),
            //            Mobile = can.ContactPhoneHome,
            //            PassportCountry = can.PassportIssueCountryId,
            //            PassportExpiry = can.PassportExpiryDate.ToString(),
            //            PassportNumber = can.PassportNumber,
            //            PassportStatus = can.PassportStatusId,
            //            Remarks = can.Remarks,
            //            Salary_QAR = can.NetSalary,
            //            Age = can.Age.ToString(),
            //            TotalWorkExperience = can.TotalWorkExperience.ToString(),
            //            Position = can.JobAdvertisement
            //        };

            //        if (res.PassportCountry.IsNotNullAndNotEmpty())
            //        {
            //            var nation = await _masterBusiness.GetIdNameList("Country");
            //            if (nation.IsNotNull())
            //            {
            //                var re = nation.Where(x => x.Id == res.PassportCountry).FirstOrDefault();
            //                if (re.IsNotNull())
            //                {
            //                    res.PassportCountry = re.Name + "_" + re.Id;
            //                }
            //            }
            //        }


            //        if (res.Position.IsNotNullAndNotEmpty())
            //        {
            //            //var jd = await _jobAdvertisementBusiness.Get();
            //            var jd = await _masterBusiness.GetJobNameById(res.Position.Split("_")[^1]);
            //            if (jd.IsNotNull())
            //            {
            //                res.Position = jd.Name + "_" + res.Position.Split("_")[^1];
            //            }
            //        }

            //        if (res.PassportStatus.IsNotNullAndNotEmpty())
            //        {
            //            var data = await _lovBusiness.GetList(x => x.ListOfValueType == "LOV_PASSPORTSTATUS");
            //            if (data.IsNotNull())
            //            {
            //                var re = data.Where(x => x.Id == res.PassportStatus).FirstOrDefault();
            //                if (re.IsNotNull())
            //                {
            //                    res.PassportStatus = re.Name + "_" + re.Id;
            //                }
            //            }
            //        }

            //        var resume = await _fileBusiness.GetSingle(x => x.Id == can.OtherCertificateId);
            //        if (resume.IsNotNull())
            //        {
            //            res.DocumentLink = resume.FileName + "_" + resume.Id;
            //        }

            //        //var pass = await _fileBusiness.GetSingle(x => x.Id == can.PassportAttachmentId);
            //        //if (pass.IsNotNull())
            //        //{
            //        //    res.PassportLink = pass.FileName + "_" + pass.Id;
            //        //}

            //        //var candidateExp = await _candidateExperienceBusiness.GetSingle(x => x.CandidateProfileId == can.Id);
            //        //if (candidateExp.IsNotNull())
            //        //{
            //        //    if (candidateExp.AttachmentId.IsNotNull())
            //        //    {
            //        //        var a = await _fileBusiness.GetSingle(x => x.Id == candidateExp.AttachmentId);
            //        //        res.ExperienceLetterLink = a.FileName + "_" + a.Id;
            //        //    }
            //        //}

            //        //var candidateEducation = await _candidateEducationalBusiness.GetSingle(x => x.CandidateProfileId == can.Id);
            //        //if (candidateEducation.IsNotNull())
            //        //{
            //        //    var a = await _fileBusiness.GetSingle(x => x.Id == candidateExp.AttachmentId);
            //        //    res.EducationLink = a.FileName + "_" + a.Id;
            //        //}

            //        var othetExp = await _lovBusiness.GetSingle(x => x.Code == "Other" && x.ListOfValueType == "LOV_Country");
            //        if (othetExp.IsNotNull())
            //        {
            //            var candidateExpOther = await _candidateExperienceByOtherBusiness.GetSingle(x => x.CandidateProfileId == can.Id);
            //            if (candidateExpOther.IsNotNull())
            //            {
            //                res.WorkExperienceAbroad = candidateExpOther.NoOfYear.ToString();
            //            }
            //        }

            //        var countryIndia = string.Empty;

            //        var countryListData = await _masterBusiness.GetIdNameList("Country");
            //        if (countryListData.IsNotNull())
            //        {
            //            var selectedCountry = countryListData.Where(x => x.Code == "India").FirstOrDefault();
            //            if (selectedCountry.IsNotNull())
            //            {
            //                countryIndia = selectedCountry.Id;
            //            }

            //        }
            //        var candidateExpCountry = await _candidateExperienceByCountryBusiness.GetSingle(x => x.CandidateProfileId == can.Id && x.CountryId == countryIndia);
            //        if (candidateExpCountry.IsNotNull())
            //        {
            //            res.WorkExperienceIndia = candidateExpCountry.NoOfYear.ToString();
            //        }


            //        result.Add(res);

            //    }



            //    return Json(result.ToDataSourceResult(request));

            //}
            //else
            //{
            //    result.Add(new WorkerCandidateViewModel
            //    {
            //        CandidateName = "",
            //        PassportNumber = ""
            //    });
            //    return Json(result.ToDataSourceResult(request));
            //}
            result.Add(new WorkerCandidateViewModel
            {
                CandidateName = "",
                PassportNumber = ""
            });
            return Json(result.ToDataSourceResult(request));
        }

        [HttpPost]
        public async Task<IActionResult> ManageWorkerDetails(WorkerCandidateSubmitViewModel model)
        {
            var result = new WorkerCandidateSubmitViewModel()
            {
                Created = new List<WorkerCandidateViewModel>(),
                Updated = new List<WorkerCandidateViewModel>(),
                Destroyed = new List<WorkerCandidateViewModel>()
            };

            if (model.IsNotNull())
            {
                foreach (var can in model.Created)
                {
                    if (can.PassportNumber.IsNotNull())
                    {
                        var country = can.PassportCountry.Split("_")[^1];
                        var data = await _candidateProfileBusiness.GetList(x => x.PassportNumber == can.PassportNumber);
                        if (data.Count > 0)
                        {                          
                          
                           return Json(new { success = false, error = "Passport No - " + can.PassportNumber + " Already Exist" });
                            
                        }
                    }
                }

                var a = await _applicationBusiness.CreateWorkerCandidateAndApplication(model);
                return Json(new { success = true });
            }

            return Json(new { success = false });
        }


        public async Task<ActionResult> GetJobAdvertismentForWorker(string jobAdvId)
        {
            var result = await _jobAdvertisementBusiness.GetJobIdNameList();
            if (jobAdvId.IsNotNull())
            {
                result = result.Where(x => x.JobAdvId == jobAdvId).ToList();
            }
            var name = new List<string>();
            if (result.IsNotNull())
            {
                foreach (var i in result.Where(x => x.ManpowerTypeName != "Staff"))
                {
                    var str = i.JobName + "_" + i.JobAdvId;
                    name.Add(str);
                }
                string res = string.Join(",", name);
                return Json(res);
            }
            return Json("");
        }

        [HttpPost]
        public async Task<FileViewModel> UploadDocument(IFormFile file)
        {

            var uploadFileStream = file.OpenReadStream();
            if (uploadFileStream.IsNotNull())
            {
                try
                {
                    //foreach (var file in files)
                    //{
                    var ms = new MemoryStream();
                    uploadFileStream.CopyTo(ms);

                    byte[] fileBytes = new byte[uploadFileStream.Length];


                    var result = await _fileBusiness.Create(new FileViewModel
                    {
                        ContentByte = ms.ToArray(),
                        ContentType = file.ContentType,// "application/octet-stream",//file.ContentType,
                        ContentLength = uploadFileStream.Length,
                        FileName = file.FileName,
                        FileExtension = Path.GetExtension(file.FileName)

                    });

                    if (result.IsSuccess)
                    {
                        return result.Item;
                    }
                    else
                    {
                        return null;
                    }
                }

                catch (Exception ex)
                {

                }
            }
            return null;

        }

        [HttpPost]
        public async Task<ActionResult> RemoveFile(string passport, string type)
        {
            var user = await _candidateProfileBusiness.GetSingle(x => x.PassportNumber == passport);
            if (user.IsNotNull())
            {
                var userId = user.Id;
                if (type == "resume")
                {
                    if (user.IsNotNull())
                    {
                        user.ResumeId = null;
                        var res = await _candidateProfileBusiness.EditCandidate(user);
                        var app = await _applicationBusiness.GetSingle(x => x.CandidateProfileId == userId);
                        if (app.IsNotNull())
                        {
                            app.ResumeId = null;
                            var appres = _applicationBusiness.Edit(app);
                        }
                        return Json(res);
                    }
                }
                else if (type == "passport")
                {
                    if (user.IsNotNull())
                    {
                        user.PassportAttachmentId = null;
                        var res = await _candidateProfileBusiness.EditCandidate(user);
                        var app = await _applicationBusiness.GetSingle(x => x.CandidateProfileId == userId);
                        if (app.IsNotNull())
                        {
                            app.PassportAttachmentId = null;
                            var appres = _applicationBusiness.Edit(app);
                        }
                        return Json(res);
                    }
                }
                else if (type == "education")
                {
                    var model = await _candidateEducationalBusiness.GetSingle(x => x.CandidateProfileId == userId);
                    if (model.IsNotNull())
                    {
                        model.AttachmentId = null;
                        var res = await _candidateEducationalBusiness.Edit(model);
                        var app = await _applicationEducationalBusiness.GetSingle(x => x.ApplicationId == res.Item.Id);
                        if (app.IsNotNull())
                        {
                            app.AttachmentId = null;
                            var appres = _applicationEducationalBusiness.Edit(app);
                        }
                        return Json(res);
                    }
                }
                else if (type == "experience")
                {
                    var model = await _candidateExperienceBusiness.GetSingle(x => x.CandidateProfileId == userId);
                    if (model.IsNotNull())
                    {
                        model.AttachmentId = null;
                        var res = await _candidateExperienceBusiness.Edit(model);
                        var app = await _applicationExperienceBusiness.GetSingle(x => x.ApplicationId == res.Item.Id);
                        if (app.IsNotNull())
                        {
                            app.AttachmentId = null;
                            var appres = _applicationExperienceBusiness.Edit(app);
                        }
                        return Json(res);
                    }
                }
                else if (type == "other")
                {
                    if (user.IsNotNull())
                    {
                        user.OtherCertificateId = null;
                        var res = await _candidateProfileBusiness.EditCandidate(user);
                        var app = await _applicationBusiness.GetSingle(x => x.CandidateProfileId == userId);
                        if (app.IsNotNull())
                        {
                            app.OtherCertificateId = null;
                            var appres = _applicationBusiness.Edit(app);
                        }
                        return Json(res);
                    }
                }
            }
            return null;
        }

        public async Task<IActionResult> ReadGridWorkerDetails([DataSourceRequest] DataSourceRequest request)
        {
            var result = new List<WorkerCandidateViewModel>();
            var candidateProfile = await _candidateProfileBusiness.GetWorkerList(_userContext.UserId);
            if (candidateProfile.IsNotNull() && candidateProfile.Count() > 0)
            {
                foreach (var can in candidateProfile)
                {
                    var res = new WorkerCandidateViewModel
                    {
                        CandidateName = can.FirstName,
                        DOB = can.BirthDate.ToDefaultDateFormat(),
                        Mobile = can.ContactPhoneHome,
                        PassportCountry = can.PassportIssueCountryId,
                        PassportExpiry = can.PassportExpiryDate.ToDefaultDateFormat(),
                        PassportNumber = can.PassportNumber,
                        PassportStatus = can.PassportStatusId,
                        Remarks = can.Remarks,
                        Salary_QAR = can.NetSalary,
                        Age = can.Age.ToString(),
                        TotalWorkExperience = can.TotalWorkExperience.ToString(),
                        Position = can.JobAdvertisement,
                        Nationality = can.NationalityId
                        //,VisaType = can.VisaType
                    };

                    var job = await _masterBusiness.GetJobDetailById(can.JobAdvertisement.Split("_")[^1]);
                    var appDetails = await _applicationBusiness.GetCandidateAppDetails(can.Id, job.Id);
                    if (appDetails.IsNotNull())
                    {                       
                        res.ApplicationState = appDetails.ApplicationStateName;
                        res.ApplicationStatus = appDetails.ApplicationStatusName;

                    }

                    if (res.PassportCountry.IsNotNullAndNotEmpty())
                    {
                        var nation = await _masterBusiness.GetIdNameList("Country");
                        if (nation.IsNotNull())
                        {
                            var re = nation.Where(x => x.Id == res.PassportCountry).FirstOrDefault();
                            if (re.IsNotNull())
                            {
                                res.PassportCountry = re.Name;// + "_" + re.Id;
                            }
                        }
                    }
                    if (res.Nationality.IsNotNullAndNotEmpty())
                    {
                        var nationality = await _masterBusiness.GetIdNameList("Nationality");
                        if (nationality.IsNotNull())
                        {
                            var re = nationality.Where(x => x.Id == res.Nationality).FirstOrDefault();
                            if (re.IsNotNull())
                            {
                                res.Nationality = re.Name;// + "_" + re.Id;
                            }
                        }
                    }
                    //if (res.VisaType.IsNotNullAndNotEmpty())
                    //{
                    //    var visatype = await _lovBusiness.GetList(x => x.ListOfValueType == "LOV_VISATYPE");
                    //    if (visatype.IsNotNull())
                    //    {
                    //        var visa = visatype.Where(x => x.Id == res.VisaType).FirstOrDefault();
                    //        if (visa.IsNotNull())
                    //        {
                    //            res.VisaType = visa.Name;// + "_" + re.Id;
                    //        }
                    //    }
                    //}

                    if (res.Position.IsNotNullAndNotEmpty())
                    {
                        //var jd = await _jobAdvertisementBusiness.Get();
                        var jd = await _masterBusiness.GetJobNameById(res.Position.Split("_")[^1]);
                        if (jd.IsNotNull())
                        {
                            res.Position = jd.Name;// + "_" + res.Position.Split("_")[^1];
                        }
                    }

                    if (res.PassportStatus.IsNotNullAndNotEmpty())
                    {
                        var data = await _lovBusiness.GetList(x => x.ListOfValueType == "LOV_PASSPORTSTATUS");
                        if (data.IsNotNull())
                        {
                            var re = data.Where(x => x.Id == res.PassportStatus).FirstOrDefault();
                            if (re.IsNotNull())
                            {
                                res.PassportStatus = re.Name;// + "_" + re.Id;
                            }
                        }
                    }

                    var resume = await _fileBusiness.GetSingle(x => x.Id == can.OtherCertificateId);
                    if (resume.IsNotNull())
                    {
                        res.DocumentLink = resume.FileName + "_" + resume.Id;
                    }

                    var othetExp = await _lovBusiness.GetSingle(x => x.Code == "Other" && x.ListOfValueType == "LOV_Country");
                    if (othetExp.IsNotNull())
                    {
                        var candidateExpOther = await _candidateExperienceByOtherBusiness.GetSingle(x => x.CandidateProfileId == can.Id);
                        if (candidateExpOther.IsNotNull())
                        {
                            res.WorkExperienceAbroad = candidateExpOther.NoOfYear.ToString();
                        }
                    }

                    var countryIndia = string.Empty;

                    var countryListData = await _masterBusiness.GetIdNameList("Country");
                    if (countryListData.IsNotNull())
                    {
                        var selectedCountry = countryListData.Where(x => x.Code == "India").FirstOrDefault();
                        if (selectedCountry.IsNotNull())
                        {
                            countryIndia = selectedCountry.Id;
                        }

                    }
                    var candidateExpCountry = await _candidateExperienceByCountryBusiness.GetSingle(x => x.CandidateProfileId == can.Id && x.CountryId == countryIndia);
                    if (candidateExpCountry.IsNotNull())
                    {
                        res.WorkExperienceIndia = candidateExpCountry.NoOfYear.ToString();
                    }

                    if (res.WorkExperienceIndia.IsNotNull() && res.WorkExperienceAbroad.IsNotNull())
                    {
                        res.TotalWorkExperience = (double.Parse(res.WorkExperienceAbroad) + double.Parse(res.WorkExperienceIndia)).ToString();
                        if (res.TotalWorkExperience.Length >= 3)
                        {
                            res.TotalWorkExperience = res.TotalWorkExperience.Substring(0, 3);

                        }

                    }
                    else if (res.WorkExperienceIndia.IsNotNull())
                    {
                        res.TotalWorkExperience = double.Parse(res.WorkExperienceIndia).ToString();
                        if (res.TotalWorkExperience.Length >= 3)
                        {
                            res.TotalWorkExperience = res.TotalWorkExperience.Substring(0, 3);

                        }
                    }
                    else if (res.WorkExperienceIndia.IsNotNull())
                    {
                        res.TotalWorkExperience = double.Parse(res.WorkExperienceAbroad).ToString();
                        if (res.TotalWorkExperience.Length >= 3)
                        {
                            res.TotalWorkExperience = res.TotalWorkExperience.Substring(0, 3);

                        }
                    }
                    result.Add(res);

                }
            }
            return Json(result.ToDataSourceResult(request));
        }

        public async Task<IActionResult> ReadGridStaffDetails([DataSourceRequest] DataSourceRequest request)
        {
            var result = new List<StaffCandidateViewModel>();
            var candidateProfile = await _candidateProfileBusiness.GetStaffList(_userContext.UserId);
           
            if (candidateProfile.IsNotNull() && candidateProfile.Count() > 0)
            {

                foreach (var can in candidateProfile)
                {
                    var job = await _masterBusiness.GetJobDetailById(can.JobAdvertisement.Split("_")[^1]);
                    var appDetails = await _applicationBusiness.GetCandidateAppDetails(can.Id,job.Id);
                        //GetSingle(x => x.CandidateProfileId == can.Id && x.JobId == job.Id);

                    var res = new StaffCandidateViewModel
                    {
                        CandidateName = can.FirstName,
                        PassportNumber = can.PassportNumber,
                        Nationality = can.NationalityId,
                        ContactNumber = can.ContactPhoneHome,
                        EmailId = can.Email,
                        TotalExperience = can.TotalWorkExperience.ToString(),
                        PresentSalary = can.NetSalary,
                        //Currency
                        // res.ExpectedSalary = can.Expecte
                        NoticePeriod = can.NoticePeriod,
                        CurrentLocation = can.CurrentAddress,
                        ResumeLink = can.ResumeAttachmentName,
                        CVSendDate = can.CreatedDate.ToDatabaseDateFormat(),
                        JobAdvertisement = can.JobAdvertisement,
                        ExpectedSalary = can.ExpectedSalary,
                        PermanentAddressHouse = can.PermanentAddressHouse,
                        PermanentAddressStreet = can.PermanentAddressStreet,
                        PermanentAddressCity = can.PermanentAddressCity,
                        PermanentAddressState = can.PermanentAddressState,
                        PermanentAddressCountry = can.PermanentAddressCountryId,
                        MaritalStatus = can.MaritalStatus,
                        CandidateId = can.Id,
                    };
                    if (appDetails.IsNotNull())
                    {
                        res.CVSendDate = appDetails.AppliedDate.ToDefaultDateFormat();
                        res.ApplicationState = appDetails.ApplicationStateName;
                        res.ApplicationNo = appDetails.ApplicationNo;
                       // res.ApplicationStatus = appDetails.ApplicationStatusName;

                    }
                    //if (res.Nationality.IsNotNullAndNotEmpty())
                    //{
                    //    var nation = await _masterBusiness.GetIdNameList("Nationality");
                    //    if (nation.IsNotNull())
                    //    {
                    //        var re = nation.Where(x => x.Id == res.Nationality).FirstOrDefault();
                    //        if (re.IsNotNull())
                    //        {
                    //           // + "_" + re.Id;
                    //        }
                    //    }
                    //}

                    if (res.Nationality.IsNotNull() && !res.Nationality.Contains("_"))
                    {
                        var nationality = await _masterBusiness.GetIdNameList("Nationality");
                        res.Nationality = nationality.Where(x => x.Id == res.Nationality).FirstOrDefault().Name;
                    }
                    else if (res.Nationality.Contains("_"))
                    {
                        res.Nationality = res.Nationality.Split("_")[0];
                    }
                    //PermanentAddressCountry
                    if (res.PermanentAddressCountry.IsNotNull() && !res.PermanentAddressCountry.Contains("_"))
                    {
                        var permanentAddressCountry = await _masterBusiness.GetIdNameList("Country");
                        res.PermanentAddressCountry = permanentAddressCountry.Where(x => x.Id == res.PermanentAddressCountry).FirstOrDefault().Name;
                    }
                    else if (res.PermanentAddressCountry.IsNotNull() && res.PermanentAddressCountry.Contains("_"))
                    {
                        res.PermanentAddressCountry = res.PermanentAddressCountry.Split("_")[0];
                    }
                    //MaritalStatus
                    if (res.MaritalStatus.IsNotNull() && !res.MaritalStatus.Contains("_"))
                    {
                        var maritalStatus = await _lovBusiness.GetList(x => x.ListOfValueType == "LOV_MARITALSTATUS");
                        res.MaritalStatus = maritalStatus.Where(x => x.Id == res.MaritalStatus).FirstOrDefault().Name;
                    }
                    else if (res.MaritalStatus.IsNotNull() && res.MaritalStatus.Contains("_"))
                    {
                        res.MaritalStatus = res.MaritalStatus.Split("_")[0];
                    }

                    var jd = await _masterBusiness.GetJobNameById(res.JobAdvertisement.Split("_")[^1]);
                    if (jd.IsNotNull())
                    {
                        res.JobAdvertisement = jd.Name; //+ "_" + res.JobAdvertisement.Split("_")[^1];
                    }


                    var candidateQualification = await _candidateEducationalBusiness.GetSingle(x => x.CandidateProfileId == can.Id);
                    if (candidateQualification.IsNotNull())
                    {
                        res.Qualification = candidateQualification.OtherQualification;
                    }

                    var candidateExperience = await _candidateExperienceBusiness.GetSingle(x => x.CandidateProfileId == can.Id);
                    if (candidateExperience.IsNotNull())
                    {
                        res.PresentEmployer = candidateExperience.Employer;
                        res.Designation = candidateExperience.JobTitle;
                    }

                    var resume = await _fileBusiness.GetSingle(x => x.Id == can.ResumeId);
                    if (resume.IsNotNull())
                    {
                        res.ResumeLink = resume.FileName + "_" + resume.Id;
                    }

                    var pass = await _fileBusiness.GetSingle(x => x.Id == can.PassportAttachmentId);
                    if (pass.IsNotNull())
                    {
                        res.PassportLink = pass.FileName + "_" + pass.Id;
                    }

                    var candidateExp = await _candidateExperienceBusiness.GetSingle(x => x.CandidateProfileId == can.Id);
                    if (candidateExp.IsNotNull())
                    {
                        var a = await _fileBusiness.GetSingle(x => x.Id == candidateExp.AttachmentId);
                        if (a.IsNotNull())
                        {
                            res.ExperienceLetterLink = a.FileName + "_" + a.Id;
                        }
                    }

                    var candidateEducation = await _candidateEducationalBusiness.GetSingle(x => x.CandidateProfileId == can.Id);
                    if (candidateEducation.IsNotNull())
                    {

                        var a = await _fileBusiness.GetSingle(x => x.Id == candidateEducation.AttachmentId);
                        if (a.IsNotNull())
                        {
                            res.EducationLink = a.FileName + "_" + a.Id;
                        }
                    }
                    var other = await _fileBusiness.GetSingle(x => x.Id == can.OtherCertificateId);
                    if (other.IsNotNull())
                    {
                        res.DocumentLink = other.FileName + "_" + other.Id;
                    }
                    result.Add(res);

                }

            }

            return Json(result.ToDataSourceResult(request));
        }

        public async Task<IActionResult> AddSkills(string jobId)
        {
            var candmodel = new CandidateProfileViewModel();
            candmodel.JobAdvertisementId = jobId;
            candmodel.Criterias = _jobCriteriaBusiness.GetApplicationJobCriteriaByJobAndType(jobId, "Criteria").Result.ToList();
            candmodel.CriteriasList = JsonConvert.SerializeObject(candmodel.Criterias);
            candmodel.Skills = _jobCriteriaBusiness.GetApplicationJobCriteriaByJobAndType(jobId, "Skills").Result.ToList();
            candmodel.SkillsList = JsonConvert.SerializeObject(candmodel.Skills);
            candmodel.OtherInformations = _jobCriteriaBusiness.GetApplicationJobCriteriaByJobAndType(jobId, "OtherInformation").Result.ToList();
            candmodel.OtherInformationsList = JsonConvert.SerializeObject(candmodel.OtherInformations);
            candmodel.SignatureDate = System.DateTime.Now;
            return View(candmodel);
        }
        public async Task<IActionResult> EditStaffCandidateAgency(string candidateId)
        {
            var candidateProfile = await _candidateProfileBusiness.GetStaffList(_userContext.UserId);
            var model = candidateProfile.Where(x=>x.Id==candidateId).FirstOrDefault();
            var job = await _masterBusiness.GetJobDetailById(model.JobAdvertisement.Split("_")[^1]);
            var appDetails = await _applicationBusiness.GetCandidateAppDetails(model.Id, job.Id);
            model.AppliedDate = appDetails.AppliedDate;
            if (model != null)
            {
                model.DataAction = DataActionEnum.Edit;
            }
            var candidateQualification = await _candidateEducationalBusiness.GetSingle(x => x.CandidateProfileId == candidateId);
            if (candidateQualification.IsNotNull())
            {
                model.Qualification = candidateQualification.OtherQualification;
                var a = await _fileBusiness.GetSingle(x => x.Id == candidateQualification.AttachmentId);
                if (a.IsNotNull())
                {
                    model.AcademicCertificateId = a.Id;
                }
            }
            var candidateExperience = await _candidateExperienceBusiness.GetSingle(x => x.CandidateProfileId == candidateId);
            if (candidateExperience.IsNotNull())
            {
                model.PresentEmployer = candidateExperience.Employer;
                model.Designation = candidateExperience.JobTitle;
                var a = await _fileBusiness.GetSingle(x => x.Id == candidateExperience.AttachmentId);
                if (a.IsNotNull())
                {
                    model.CoverLetterId = a.Id;
                }
            }
            model.JobAdvertisement = model.JobAdvertisement.Split("_")[0];
            model.PresentSalary = model.NetSalary;
            model.CurrentLocation = model.CurrentAddress;
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ManageStaffCandidateAgency(CandidateProfileViewModel canModel)
        {

            var data = await _candidateProfileBusiness.GetSingleById(canModel.Id);
            if (data.IsNotNull())
            {              
                data.ContactPhoneHome = canModel.ContactPhoneHome;
                data.TotalWorkExperience = canModel.TotalWorkExperience;             
                data.CurrentAddress = canModel.CurrentLocation;

                data.PermanentAddressHouse = canModel.PermanentAddressHouse;
                data.PermanentAddressStreet = canModel.PermanentAddressStreet;
                data.PermanentAddressCity = canModel.PermanentAddressCity;
                data.PermanentAddressState = canModel.PermanentAddressState;

                data.NationalityId = canModel.NationalityId;
                data.PermanentAddressCountryId = canModel.PermanentAddressCountryId;
                data.MaritalStatus = canModel.MaritalStatus;
                
                data.ResumeId = canModel.ResumeId;
                data.PassportAttachmentId = canModel.PassportAttachmentId;
                data.OtherCertificateId = canModel.OtherCertificateId;

                var res = await _candidateProfileBusiness.EditCandidate(data);
                
                var dataCandEdu = await _candidateEducationalBusiness.GetSingle(x => x.CandidateProfileId == canModel.Id);
                if (dataCandEdu!=null)
                {
                    dataCandEdu.AttachmentId = canModel.AcademicCertificateId;
                    var resCandEdu = await _candidateEducationalBusiness.Edit(dataCandEdu);
                }
                var dataCandExp = await _candidateExperienceBusiness.GetSingle(x => x.CandidateProfileId == canModel.Id);
                if (dataCandExp != null)
                {
                    dataCandExp.AttachmentId = canModel.CoverLetterId;
                    var resCandExp = await _candidateExperienceBusiness.Edit(dataCandExp);
                }
                var applist = await _applicationBusiness.GetList(x => x.CandidateProfileId == canModel.Id);
                foreach (var app in applist)
                {
                    var appEdit = await _applicationBusiness.GetSingleById(app.Id);
                    appEdit.ContactPhoneHome = canModel.ContactPhoneHome;
                    appEdit.TotalWorkExperience = canModel.TotalWorkExperience;
                    appEdit.CurrentAddress = canModel.CurrentLocation;

                    appEdit.PermanentAddressHouse = canModel.PermanentAddressHouse;
                    appEdit.PermanentAddressStreet = canModel.PermanentAddressStreet;
                    appEdit.PermanentAddressCity = canModel.PermanentAddressCity;
                    appEdit.PermanentAddressState = canModel.PermanentAddressState;

                    appEdit.NationalityId = canModel.NationalityId;
                    appEdit.PermanentAddressCountryId = canModel.PermanentAddressCountryId;
                    appEdit.MaritalStatus = canModel.MaritalStatus;

                    appEdit.ResumeId = canModel.ResumeId;
                    appEdit.PassportAttachmentId = canModel.PassportAttachmentId;
                    appEdit.OtherCertificateId = canModel.OtherCertificateId;

                    await _applicationBusiness.Edit(appEdit);

                    var dataAppEdu = await _applicationEducationalBusiness.GetSingle(x => x.ApplicationId == app.Id);
                    if (dataAppEdu != null)
                    {
                        dataAppEdu.AttachmentId = canModel.AcademicCertificateId;
                        var resAppEdu = await _applicationEducationalBusiness.Edit(dataAppEdu);
                    }
                    var dataAppExp = await _applicationExperienceBusiness.GetSingle(x => x.ApplicationId == app.Id);
                    if (dataAppExp != null)
                    {
                        dataAppExp.AttachmentId = canModel.CoverLetterId;
                        var resAppExp = await _applicationExperienceBusiness.Edit(dataAppExp);
                    }


                }
            }
       
                
            return Json(new { success = true });
        }
    }

}