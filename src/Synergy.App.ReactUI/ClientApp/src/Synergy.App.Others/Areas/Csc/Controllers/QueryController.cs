using Microsoft.AspNetCore.Mvc;
using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMS.UI.Web.Areas.Csc.Controllers
{
    [Route("CSC/query")]
    [ApiController]
    public class QueryController : Controller
    {
        private readonly IServiceProvider _serviceProvider;
        public QueryController(AuthSignInManager<ApplicationIdentityUser> customUserManager
            , IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

        }
        [HttpGet]
        [Route("GetCSCBirthCertificateReport")]
        public async Task<IActionResult> GetCSCBirthCertificateReport(string serviceId)
        {
            //serviceId = "875bd230-0af3-410a-ac85-5e5ccd4e46f6";
            var _userContext = _serviceProvider.GetService<IUserContext>();
            var _companyBusiness = _serviceProvider.GetService<ICompanyBusiness>();
            var _commonServiceBusiness = _serviceProvider.GetService<ICommonServiceBusiness>();
            var _applicationDocumentBusiness = _serviceProvider.GetService<IApplicationDocumentBusiness>();
            var _fileBusiness = _serviceProvider.GetService<IFileBusiness>();
            //var report = new CSCReportViewModel();
            try
            {
                var report = await _commonServiceBusiness.GetCSCBirthCertificateData(serviceId);
                if (report == null)
                {
                    report = new CSCReportViewModel();
                }
                //report.LocalAreaName = "Raipur";
                //report.TehasilBlockName = "Raipur";
                //report.TehasilBlockName = "Raipur";
                //report.DistrictName = "Raipur";
                //report.StateName = "Chhattisgarh";
                //report.Name = "Ritesh Baghel";
                //report.GenderName = "Male";
                //report.BirthDate = new DateTime(2022,06,02);
                //report.BirthPlace = "Raipur";
                //report.MotherName = "Manju Baghel";
                //report.FatherName = "Sanjay Baghel";
                //report.CurrentAddress = "H No 101, Zone 1, Raipur ";
                //report.PermanentAddress = "H No 101, Zone 1, Raipur ";
                //report.RegistrationNo = "CG/BRC/2022/06/10/01";
                //report.RegistrationDate = new DateTime(2022, 06, 10);
                //report.Remarks = "Good Health";
                //report.IssueDate = DateTime.Today;
                //report.AuthorityAddress = "Admin Office, Raipur, Chhattisgarh";
                //report.ServiceId = "01";
                //report.ServiceNo = "S-13.06.2022-01";
                if (report.BirthDate.IsNotNull())
                {
                    report.BirthDateText = string.Format("{0:dd MMM yyyy}", report.BirthDate);
                }
                if (report.RegistrationDate.IsNotNull())
                {
                    report.RegistrationDateText = string.Format("{0:dd MMM yyyy}", report.RegistrationDate);
                }
                if (report.IssueDate.IsNotNull())
                {
                    report.IssueDateText = string.Format("{0:dd MMM yyyy}", report.IssueDate);
                }
                var birthlogo = await _applicationDocumentBusiness.GetSingle(x => x.Code == "GOVT_BIRTH_DEATH_LOGO");
                if (birthlogo.IsNotNull())
                {
                    var birthlogobytes = await _fileBusiness.GetFileByte(birthlogo.DocumentId);
                    if (birthlogobytes.Length > 0)
                    {
                        report.BirthDeathLogo = Convert.ToBase64String(birthlogobytes, 0, birthlogobytes.Length);
                    }
                }

                var govtlogo = await _applicationDocumentBusiness.GetSingle(x => x.Code == "GOVT_3LION_LOGO");
                if (govtlogo.IsNotNull())
                {
                    var govtlogobytes = await _fileBusiness.GetFileByte(govtlogo.DocumentId);
                    if (govtlogobytes.Length > 0)
                    {
                        report.GovtLogo = Convert.ToBase64String(govtlogobytes, 0, govtlogobytes.Length);
                    }
                }
                //var seallogo = await _applicationDocumentBusiness.GetSingle(x => x.Code == "CERTIFIED_LOGO");
                var seallogo = await _applicationDocumentBusiness.GetSingle(x => x.Code == "APPROVED_LOGO");
                if (seallogo.IsNotNull())
                {
                    var seallogobytes = await _fileBusiness.GetFileByte(seallogo.DocumentId);
                    if (seallogobytes.Length > 0)
                    {
                        report.SealLogo = Convert.ToBase64String(seallogobytes, 0, seallogobytes.Length);
                    }
                }
                return Ok(report);
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        [HttpGet]
        [Route("GetCSCAcknowledgementMarriageEnglishReport")]
        public async Task<IActionResult> GetCSCAcknowledgementMarriageEnglishReport(string serviceId)
        {
            //serviceId = "875bd230-0af3-410a-ac85-5e5ccd4e46f6";
            var _userContext = _serviceProvider.GetService<IUserContext>();
            var _companyBusiness = _serviceProvider.GetService<ICompanyBusiness>();
            var _commonServiceBusiness = _serviceProvider.GetService<ICommonServiceBusiness>();
            var _applicationDocumentBusiness = _serviceProvider.GetService<IApplicationDocumentBusiness>();
            var _fileBusiness = _serviceProvider.GetService<IFileBusiness>();
            var report = new CSCReportAcknowledgementViewModel();
            try
            {
                report = await _commonServiceBusiness.GetCSCAcknowledgementData(serviceId);
                if (report == null)
                {
                    report = new CSCReportAcknowledgementViewModel();
                }
                //var sercharge = new List<ServiceChargeViewModel>();
                //sercharge.Add(new ServiceChargeViewModel { ChargeName = "Scanning", Amount = 25 });
                //sercharge.Add(new ServiceChargeViewModel { ChargeName = "Application Charge", Amount = 30 });
                //report.ServiceChargeData = sercharge;
                report.ServiceChargeData = await _commonServiceBusiness.GetServiceChargeData(serviceId);

                //report.ApplicationReferenceNumber = "0705011715002466";
                //report.DateOfApplication = new DateTime(2017, 07, 27);
                //report.ApplicationServiceName = "OBC Certificate";
                ////report.ServiceChargeName = "Scanning charges";
                ////report.ServiceChargeAmount = 25.00;
                ////report.ServiceChargeKioskName = "Service Charges of the Kiosk Operator per unit";
                ////report.ServiceChargeKioskAmount = 30.00;
                //report.TotalFeesPaid = 55.00;
                //report.PaymentDetails = "";
                //report.DeliveryDateOfService = new DateTime(2022, 07, 25);
                //report.DeliverableDetails = "Certificate";

                //report.OfficeType = "Sub-District/Revenue Tehsil/Revenue SubTehsil";
                //report.Distict = "Sarguja";
                //report.Tehsil = "BATAULI 2";
                //report.RevenueVillage = "MAHESHPUR";
                //report.NagarPanchayatName = "Nagar Panchayat PATAN";
                //report.NagarPanchayatNameHi = "नगर पंचायत पाटन";
                //report.MunicipalityName = "NAGAR PALIKA BALRAMPUR";
                //report.MunicipalityNameHi = "नगर पालिका बलरामपुर";
                //report.ZoneName = "ZONE - 1";
                //report.ZoneNameHi = "जोन 1";
                //report.ApplicantName = "रामभजन";
                //report.ApplicantAddress = "महेशपुर";
                //report.ApplicantDistict = "Sarguja";
                //report.ApplicantState = "Chhattisgarh";
                //report.ApplicantEmail = "ramniwas.painkra65@gmail.com";
                //report.ApplicantMobile = "9174772742";

                //report.OfficerName = "Rajesh Verma";
                //report.OfficerAddress = "लोक सेवा केंद्र";
                //report.OfficerMobile = "9752542311";

                if (report.DateOfApplication.IsNotNull())
                {
                    report.DateOfApplicationText = string.Format("{0:dd MMM yyyy}", report.DateOfApplication);
                }
                if (report.DeliveryDateOfService.IsNotNull())
                {
                    report.DeliveryDateOfServiceText = string.Format("{0:dd MMM yyyy}", report.DeliveryDateOfService);
                }

                var chipslogo = await _applicationDocumentBusiness.GetSingle(x => x.Code == "CHHATTISGARH_CHIPS");
                if (chipslogo.IsNotNull())
                {
                    var chipslogobytes = await _fileBusiness.GetFileByte(chipslogo.DocumentId);
                    if (chipslogobytes.Length > 0)
                    {
                        report.ChipsLogo = Convert.ToBase64String(chipslogobytes, 0, chipslogobytes.Length);
                    }
                }

                var govtlogo = await _applicationDocumentBusiness.GetSingle(x => x.Code == "CHHATTISGARH_LOK_SEWA");
                if (govtlogo.IsNotNull())
                {
                    var govtlogobytes = await _fileBusiness.GetFileByte(govtlogo.DocumentId);
                    if (govtlogobytes.Length > 0)
                    {
                        report.GovtLogo = Convert.ToBase64String(govtlogobytes, 0, govtlogobytes.Length);
                    }
                }
                //var seallogo = await _applicationDocumentBusiness.GetSingle(x => x.Code == "CERTIFIED_LOGO");
                var seallogo = await _applicationDocumentBusiness.GetSingle(x => x.Code == "APPROVED_LOGO");
                if (seallogo.IsNotNull())
                {
                    var seallogobytes = await _fileBusiness.GetFileByte(seallogo.DocumentId);
                    if (seallogobytes.Length > 0)
                    {
                        report.SealLogo = Convert.ToBase64String(seallogobytes, 0, seallogobytes.Length);
                    }
                }
                return Ok(report);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        [Route("GetCSCAcknowledgementEnglishReport")]
        public async Task<IActionResult> GetCSCAcknowledgementEnglishReport(string serviceId)
        {
            //serviceId = "875bd230-0af3-410a-ac85-5e5ccd4e46f6";
            var _userContext = _serviceProvider.GetService<IUserContext>();
            var _companyBusiness = _serviceProvider.GetService<ICompanyBusiness>();
            var _commonServiceBusiness = _serviceProvider.GetService<ICommonServiceBusiness>();
            var _applicationDocumentBusiness = _serviceProvider.GetService<IApplicationDocumentBusiness>();
            var _fileBusiness = _serviceProvider.GetService<IFileBusiness>();
            var report = new CSCReportAcknowledgementViewModel();
            try
            {
                report = await _commonServiceBusiness.GetCSCAcknowledgementData(serviceId);
                if (report == null)
                {
                    report = new CSCReportAcknowledgementViewModel();
                }
                //var sercharge = new List<ServiceChargeViewModel>();
                //sercharge.Add(new ServiceChargeViewModel { ChargeName = "Scanning", Amount = 25 });
                //sercharge.Add(new ServiceChargeViewModel { ChargeName = "Application Charge", Amount = 30 });
                //report.ServiceChargeData = sercharge;
                report.ServiceChargeData = await _commonServiceBusiness.GetServiceChargeData(serviceId);

                //report.ApplicationReferenceNumber = "0705011715002466";
                //report.DateOfApplication = new DateTime(2017,07,27);
                //report.ApplicationServiceName = "OBC Certificate";
                //report.ServiceChargeName = "Scanning charges";
                //report.ServiceChargeAmount = 25.00;
                //report.ServiceChargeKioskName = "Service Charges of the Kiosk Operator per unit";
                //report.ServiceChargeKioskAmount = 30.00;
                //report.TotalFeesPaid = 55.00;
                //report.PaymentDetails = "";
                //report.DeliveryDateOfService = new DateTime(2022, 07, 25);
                //report.DeliverableDetails = "Certificate";

                //report.OfficeType = "Sub-District/Revenue Tehsil/Revenue SubTehsil";
                //report.Distict = "Sarguja";
                //report.Tehsil = "BATAULI 2";
                //report.RevenueVillage = "MAHESHPUR";

                //report.ApplicantName = "रामभजन";
                //report.ApplicantAddress = "महेशपुर";
                //report.ApplicantDistict = "Sarguja";
                //report.ApplicantState = "Chhattisgarh";
                //report.ApplicantEmail = "ramniwas.painkra65@gmail.com";
                //report.ApplicantMobile = "9174772742";

                //report.OfficerName = "Rajesh Verma";
                //report.OfficerAddress = "लोक सेवा केंद्र";
                //report.OfficerMobile = "9752542311";

                if (report.DateOfApplication.IsNotNull())
                {
                    report.DateOfApplicationText = string.Format("{0:dd MMM yyyy}", report.DateOfApplication);
                }
                if (report.DeliveryDateOfService.IsNotNull())
                {
                    report.DeliveryDateOfServiceText = string.Format("{0:dd MMM yyyy}", report.DeliveryDateOfService);
                }

                var chipslogo = await _applicationDocumentBusiness.GetSingle(x => x.Code == "CHHATTISGARH_CHIPS");
                if (chipslogo.IsNotNull())
                {
                    var chipslogobytes = await _fileBusiness.GetFileByte(chipslogo.DocumentId);
                    if (chipslogobytes.Length > 0)
                    {
                        report.ChipsLogo = Convert.ToBase64String(chipslogobytes, 0, chipslogobytes.Length);
                    }
                }

                var govtlogo = await _applicationDocumentBusiness.GetSingle(x => x.Code == "CHHATTISGARH_LOK_SEWA");
                if (govtlogo.IsNotNull())
                {
                    var govtlogobytes = await _fileBusiness.GetFileByte(govtlogo.DocumentId);
                    if (govtlogobytes.Length > 0)
                    {
                        report.GovtLogo = Convert.ToBase64String(govtlogobytes, 0, govtlogobytes.Length);
                    }
                }
                //var seallogo = await _applicationDocumentBusiness.GetSingle(x => x.Code == "CERTIFIED_LOGO");
                var seallogo = await _applicationDocumentBusiness.GetSingle(x => x.Code == "APPROVED_LOGO");
                if (seallogo.IsNotNull())
                {
                    var seallogobytes = await _fileBusiness.GetFileByte(seallogo.DocumentId);
                    if (seallogobytes.Length > 0)
                    {
                        report.SealLogo = Convert.ToBase64String(seallogobytes, 0, seallogobytes.Length);
                    }
                }
                return Ok(report);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        [Route("GetCSCAcknowledgementHindiReport")]
        public async Task<IActionResult> GetCSCAcknowledgementHindiReport(string serviceId)
        {
            //serviceId = "875bd230-0af3-410a-ac85-5e5ccd4e46f6";
            var _userContext = _serviceProvider.GetService<IUserContext>();
            var _companyBusiness = _serviceProvider.GetService<ICompanyBusiness>();
            var _commonServiceBusiness = _serviceProvider.GetService<ICommonServiceBusiness>();
            var _applicationDocumentBusiness = _serviceProvider.GetService<IApplicationDocumentBusiness>();
            var _fileBusiness = _serviceProvider.GetService<IFileBusiness>();
            var report = new CSCReportAcknowledgementViewModel();
            try
            {
                report = await _commonServiceBusiness.GetCSCAcknowledgementData(serviceId);
                if (report == null)
                {
                    report = new CSCReportAcknowledgementViewModel();
                }
                //var sercharge = new List<ServiceChargeViewModel>();
                //sercharge.Add(new ServiceChargeViewModel { ChargeName = "स्कैनिंग शुल्क", Amount = 25 });
                //sercharge.Add(new ServiceChargeViewModel { ChargeName = "लोकसेवा केंद्र / सीएससी / कीओस्क के लिए सेवा शुल्क", Amount = 30 });
                //report.ServiceChargeData = sercharge;
                report.ServiceChargeData = await _commonServiceBusiness.GetServiceChargeData(serviceId);

                //report.ApplicationReferenceNumber = "0705011715002466";
                //report.DateOfApplication = new DateTime(2017, 07, 27);
                //report.ApplicationServiceName = "अन्य पिछड़ा वर्ग प्रमाण पत्र";
                //report.ServiceChargeName = "स्कैनिंग शुल्क";
                //report.ServiceChargeAmount = 25.00;
                //report.ServiceChargeKioskName = "लोकसेवा केंद्र / सीएससी / कीओस्क के लिए सेवा शुल्क";
                //report.ServiceChargeKioskAmount = 30.00;
                //report.TotalFeesPaid = 55.00;
                //report.PaymentDetails = "";
                //report.DeliveryDateOfService = new DateTime(2022, 07, 25);
                //report.DeliverableDetails = "प्रमाण पत्र";

                //report.OfficeType = "उप-जिला / राजस्व तहसील / राजस्व उप-तहसील";
                //report.Distict = "सरगुजा";
                //report.Tehsil = "बतौली 2";
                //report.RevenueVillage = "महेशपुर";

                //report.ApplicantName = "रामभजन";
                //report.ApplicantAddress = "ग्राम महेशपुर";
                //report.ApplicantDistict = "सरगुजा";
                //report.ApplicantState = "छत्तीसगढ";
                //report.ApplicantEmail = "ramniwas.painkra65@gmail.com";
                //report.ApplicantMobile = "9174772742";

                //report.OfficerName = "राजेश कुमार वर्मा";
                //report.OfficerAddress = "लोक सेवा केंद्र";
                //report.OfficerMobile = "9752542311";

                if (report.DateOfApplication.IsNotNull())
                {
                    report.DateOfApplicationText = string.Format("{0:dd MMM yyyy}", report.DateOfApplication);
                }
                if (report.DeliveryDateOfService.IsNotNull())
                {
                    report.DeliveryDateOfServiceText = string.Format("{0:dd MMM yyyy}", report.DeliveryDateOfService);
                }

                var chipslogo = await _applicationDocumentBusiness.GetSingle(x => x.Code == "CHHATTISGARH_CHIPS");
                if (chipslogo.IsNotNull())
                {
                    var chipslogobytes = await _fileBusiness.GetFileByte(chipslogo.DocumentId);
                    if (chipslogobytes.Length > 0)
                    {
                        report.ChipsLogo = Convert.ToBase64String(chipslogobytes, 0, chipslogobytes.Length);
                    }
                }

                var govtlogo = await _applicationDocumentBusiness.GetSingle(x => x.Code == "CHHATTISGARH_LOK_SEWA");
                if (govtlogo.IsNotNull())
                {
                    var govtlogobytes = await _fileBusiness.GetFileByte(govtlogo.DocumentId);
                    if (govtlogobytes.Length > 0)
                    {
                        report.GovtLogo = Convert.ToBase64String(govtlogobytes, 0, govtlogobytes.Length);
                    }
                }
                //var seallogo = await _applicationDocumentBusiness.GetSingle(x => x.Code == "CERTIFIED_LOGO");
                var seallogo = await _applicationDocumentBusiness.GetSingle(x => x.Code == "APPROVED_LOGO");
                if (seallogo.IsNotNull())
                {
                    var seallogobytes = await _fileBusiness.GetFileByte(seallogo.DocumentId);
                    if (seallogobytes.Length > 0)
                    {
                        report.SealLogo = Convert.ToBase64String(seallogobytes, 0, seallogobytes.Length);
                    }
                }
                return Ok(report);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet]
        [Route("GetCSCOBCCertificateReport")]
        public async Task<IActionResult> GetCSCOBCCertificateReport(string serviceId)
        {
            //serviceId = "875bd230-0af3-410a-ac85-5e5ccd4e46f6";
            var _userContext = _serviceProvider.GetService<IUserContext>();
            var _companyBusiness = _serviceProvider.GetService<ICompanyBusiness>();
            var _commonServiceBusiness = _serviceProvider.GetService<ICommonServiceBusiness>();
            var _applicationDocumentBusiness = _serviceProvider.GetService<IApplicationDocumentBusiness>();
            var _fileBusiness = _serviceProvider.GetService<IFileBusiness>();
            var report = new CSCReportOBCCertificateViewModel();
            try
            {
                report = await _commonServiceBusiness.GetCSCOBCCertificateData(serviceId);
                if (report == null)
                {
                    report = new CSCReportOBCCertificateViewModel();
                }
                report.AuthorityName = report.FinalAuthorityName;
                //report.ElectronicApplicationDate = new DateTime(2022,01,03);
                //report.ApplicationReferenceNumber = "0705012201000067";
                //report.ChoiceCenter = "लोकसेवा केंद्र";
                //report.NetAmount = 5.00;
                //report.RegistrationNumber = "2022-01-000637";

                //report.ApplicantNameHi = "मोक्ष देवांगन";
                //report.ApplicantFatherNameHi = "प्रकाश कुमार देवांगन";
                //report.ApplicantResidentHi = "छड़िया";
                //report.ApplicantTehasilHi = "खरोरा उप-तहसील";
                //report.ApplicantDistrictHi = "रायपुर";
                //report.ApplicantCasteHi = "कोष्टा";

                //report.ApplicantName = "MOKSH DEWANGAN";
                //report.ApplicantFatherName = "PRAKASH KUMAR DEWANGAN";
                //report.ApplicantResident = "CHHADIYA";
                //report.ApplicantTehasil = "KHARORA SUB-TEHSIL";
                //report.ApplicantDistrict = "RAIPUR";
                //report.ApplicantCaste = "KOSTA";

                //report.AuthorityName = "प्रकाश कुमार टंडन";
                //report.IssueDate = new DateTime(2022,01,08);

                var nameHi = string.Empty;
                var fatherNameHi = string.Empty;
                var residentHi = string.Empty;
                var tehasilHi = string.Empty;
                var districtHi = string.Empty;
                var casteHi = string.Empty;

                var name = string.Empty;
                var fatherName = string.Empty;
                var resident = string.Empty;
                var tehasil = string.Empty;
                var district = string.Empty;
                var caste = string.Empty;

                nameHi = report.ApplicantNameHi;
                fatherNameHi = report.ApplicantFatherNameHi;
                residentHi = report.ApplicantResidentHi;
                tehasilHi = report.ApplicantTehasilHi;
                districtHi = report.ApplicantDistrictHi;
                casteHi = report.ApplicantCasteHi;

                name = report.ApplicantName;
                fatherName = report.ApplicantFatherName;
                resident = report.ApplicantResident;
                tehasil = report.ApplicantTehasil;
                district = report.ApplicantDistrict;
                caste = report.ApplicantCaste;

                var code1Hi = @"<div style='text-align:justify;'> प्रमाणित किया जाता है कि <strong>{{ApplicantNameHi}}</strong> पिता <strong>{{ApplicantFatherNameHi}}</strong> निवासी <strong>{{ApplicantResidentHi}}</strong> तहसील <strong>{{ApplicantTehasilHi}}</strong>
                    जिला <strong>{{ApplicantDistrictHi}}</strong> राज्य छत्तीसगढ़ के निवासी हैं, जो अन्य पिछड़ा वर्ग <strong>{{ApplicantCasteHi}}</strong> 
                    जाति का/कि हैं। जिसे पिछड़ा वर्ग रूप में छत्तीसगढ़ शासन, आदिम जाति, अनुसुचित जाति एवं पिछड़ा वर्ग कल्याण विभाग की अधिसूचना क्र.एफ-8-5/25/4/84, तिथि 26 दिसंबर, 1984
                    द्वारा अधिमान्य किया गया है तथा यह जाति छत्तीसगढ़ राज्य के अन्य पिछड़ा वर्ग की सूची के अनुक्रमांक 25 पर अंकित है। <strong>{{ApplicantNameHi}}</strong> और/या उनका
                    परिवार सामान्यतः छत्तीसगढ़ के जिला <strong>{{ApplicantDistrictHi}}</strong> तहसील <strong>{{ApplicantTehasilHi}}</strong> में निवास करता है। </div>";
                code1Hi = code1Hi.Replace("{{ApplicantNameHi}}", nameHi);
                code1Hi = code1Hi.Replace("{{ApplicantFatherNameHi}}", fatherNameHi);
                code1Hi = code1Hi.Replace("{{ApplicantResidentHi}}", residentHi);
                code1Hi = code1Hi.Replace("{{ApplicantTehasilHi}}", tehasilHi);
                code1Hi = code1Hi.Replace("{{ApplicantDistrictHi}}", districtHi);
                code1Hi = code1Hi.Replace("{{ApplicantCasteHi}}", casteHi);
                report.OBCSection1Hi = code1Hi;
                
                var code2Hi = @"<div style='text-align:justify;'> यह भी प्रमाणित किया जाता है कि <strong>{{ApplicantNameHi}}</strong> क्रीमीलेयर (संपन्न वर्ग) व्यक्तियों/वर्गो की श्रेणी मैं नहीं आते हैं, जिसका उल्लेख भारत सरकार कार्मिक एव प्रशिक्षण विभाग
                    के परिपत्र संख्या 36012/22/93/ईएसटीटी(एससीटी) दिनांक 08/09/1993 यथा संशोधित परिपत्र संख्या 36033/3/2004-ईएसटीटी(आरक्षण) दिनांक 14/10/2008 तथा सामान्य प्रशासन विभाग के परिपत्र क्रमांक
                    एफ-9-3/2001/1-3 दिनांक 24/06/2009 के कॉलम 3 में किया गया है| </div>";
                code2Hi = code2Hi.Replace("{{ApplicantNameHi}}", nameHi);
                report.OBCSection2Hi = code2Hi;

                var code3Hi = @"<div style='text-align:justify;'> यह प्रमाण पत्र इन उपबंधो के अधीन जारी किया गया है कि यदि जिला स्तरीय प्रमाणपत्र सत्यापन समिति अथवा उच्च स्तरीय प्रमाणीकरण छानबीन समिति के द्वारा 
                    अपनी जांच/निरीक्षण में आवेदक श्री/श्रीमती/सुश्री <strong>{{ApplicantNameHi}}</strong> के द्वारा प्रस्तुत सामाजिक प्रास्थिति का दावा असत्य अथवा कटपूर्ण पाया जाता 
                     है तो यह प्रमाण पत्र तत्काल प्रभाव से निरस्त माना जावेगा तथा तत् समय प्रवृत किसी अन्य विधि में अंतविष्ट किसी बात के होते हुए भी छत्तीसगढ़ अनुसुचित जाति, अनुसुचित जनजाति
                     और अन्य पिछड़ा वर्ग (सामाजिक प्रास्थिति के प्रमाणीकरण का विनियमन) अधिनियम,2013 की धारा 8 से 13 के अधीन कार्यवाही की जाएगी | </div>";
                code3Hi = code3Hi.Replace("{{ApplicantNameHi}}", nameHi);
                report.OBCSection3Hi = code3Hi;

                var code1 = @"<div style='text-align:justify;'> It is hereby certified that Mr/Mrs/Ms <strong>{{ApplicantName}}</strong> Father <strong>{{ApplicantFatherName}}</strong> resident of
                    <strong>{{ApplicantResident}}</strong> village tehsil <strong>{{ApplicantTehasil}}</strong> District <strong>{{ApplicantDistrict}}</strong> belongs to Other Backward Class <strong>{{ApplicantCaste}}</strong> caste. This
                    caste has been recognized as Other Backward Classes vide Notification no. F-8-5/25/4/84 dated 26th December 1984, as
                    amended from time to time by subsequent notifications, and this caste mentioned at serial no 25 as Other Backward
                    Class of Chhattisgarh state. Therefore, MR/Mrs/Ms <strong>{{ApplicantName}}</strong> Father <strong>{{ApplicantFatherName}}</strong> belongs to
                    Other Backward Class. </div>";
                code1 = code1.Replace("{{ApplicantName}}", name);
                code1 = code1.Replace("{{ApplicantFatherName}}", fatherName);
                code1 = code1.Replace("{{ApplicantResident}}", resident);
                code1 = code1.Replace("{{ApplicantTehasil}}", tehasil);
                code1 = code1.Replace("{{ApplicantDistrict}}", district);
                code1 = code1.Replace("{{ApplicantCaste}}", caste);
                report.OBCSection1 = code1;

                var code2 = @"<div style='text-align:justify;'> 2.This is also certified that Mr/Mrs/Ms <strong>{{ApplicantName}}</strong> does not belong to the category of creamy layer persons/
                    classes which have been mentioned in the circular no 36012/22/93/ESTT(SCT) dated 8/09/1993 as amended by notification
                    on 36033/3/2004-ESTT (Reservation) dated 14/10/2008 of Government of India, department of personnel and training and in
                    column 3 of circular number f-9-3/2001/1-3 dated 24/06/2009 of General Administration Department. </div>";
                code2 = code2.Replace("{{ApplicantName}}", name);
                report.OBCSection2 = code2;

                var code3 = @"<div style='text-align:justify;'> 3.This certificate has been issued under the provisions that in case the district level certificates verification committee or
                    high power certification scrutiny committee during its inquiry /inspection finds the claim submitted by Mr/Mrs/Ms <strong>{{ApplicantName}}</strong> 
                    is false or fraudulent, the certificate shall be deemed to be cancelled with immediate effect and
                    notwithstanding contained anything under any other law for the time being in force, action shall be initiated under
                    section 8-13 of the Chhattisgarh Scheduled Castes, Scheduled Tribes and Other Backward Classes (Regulation of social status certification). </div>";
                code3 = code3.Replace("{{ApplicantName}}", name);
                report.OBCSection3 = code3;


                if (report.ElectronicApplicationDate.IsNotNull())
                {
                    report.ElectronicApplicationDateText = string.Format("{0:dd MMM yyyy}", report.ElectronicApplicationDate);
                }
                if (report.IssueDate.IsNotNull())
                {
                    report.IssueDateText = string.Format("{0:dd MMM yyyy}", report.IssueDate);
                }

                var govtlogo = await _applicationDocumentBusiness.GetSingle(x => x.Code == "CHHATTISGARH_LOK_SEWA");
                if (govtlogo.IsNotNull())
                {
                    var govtlogobytes = await _fileBusiness.GetFileByte(govtlogo.DocumentId);
                    if (govtlogobytes.Length > 0)
                    {
                        report.GovtLogo = Convert.ToBase64String(govtlogobytes, 0, govtlogobytes.Length);
                    }
                }
                //var seallogo = await _applicationDocumentBusiness.GetSingle(x => x.Code == "CERTIFIED_LOGO");
                var seallogo = await _applicationDocumentBusiness.GetSingle(x => x.Code == "APPROVED_LOGO");
                if (seallogo.IsNotNull())
                {
                    var seallogobytes = await _fileBusiness.GetFileByte(seallogo.DocumentId);
                    if (seallogobytes.Length > 0)
                    {
                        report.SealLogo = Convert.ToBase64String(seallogobytes, 0, seallogobytes.Length);
                    }
                }
                return Ok(report);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet]
        [Route("GetCSCOBCCertificateTempReport")]
        public async Task<IActionResult> GetCSCOBCCertificateTempReport(string serviceId)
        {
            //serviceId = "875bd230-0af3-410a-ac85-5e5ccd4e46f6";
            var _userContext = _serviceProvider.GetService<IUserContext>();
            var _companyBusiness = _serviceProvider.GetService<ICompanyBusiness>();
            var _commonServiceBusiness = _serviceProvider.GetService<ICommonServiceBusiness>();
            var _applicationDocumentBusiness = _serviceProvider.GetService<IApplicationDocumentBusiness>();
            var _fileBusiness = _serviceProvider.GetService<IFileBusiness>();
            var report = new CSCReportOBCCertificateViewModel();
            try
            {
                report = await _commonServiceBusiness.GetCSCOBCCertificateData(serviceId);
                if (report == null)
                {
                    report = new CSCReportOBCCertificateViewModel();
                }
                report.AuthorityName = report.ProvisionalAuthorityName;
                //report.ElectronicApplicationDate = new DateTime(2017, 12, 15);
                //report.ApplicationReferenceNumber = "0705011701023687";
                //report.ChoiceCenter = "लोक सेवा केंद्र";
                //report.NetAmount = 5.00;
                //report.RegistrationNumber = "2018-01-001460";

                //report.ApplicantNameHi = "कोमल नायक";
                //report.ApplicantFatherNameHi = "पुनित राम";
                //report.ApplicantResidentHi = "खरोरा";
                //report.ApplicantTehasilHi = "खरोरा उप-तहसील";
                //report.ApplicantDistrictHi = "रायपुर";
                //report.ApplicantCasteHi = "बंजारा";

                //report.ApplicantName = "KOMAL NAYAK";
                //report.ApplicantFatherName = "PUNIT RAM";
                //report.ApplicantResident = "KHARORA";
                //report.ApplicantTehasil = "KHARORA SUB-TEHSIL";
                //report.ApplicantDistrict = "RAIPUR";
                //report.ApplicantCaste = "BANJARA";

                //report.AuthorityName = "विष्णु सिंह ठाकुर";
                //report.IssueDate = new DateTime(2018, 01, 18);

                var nameHi = string.Empty;
                var fatherNameHi = string.Empty;
                var residentHi = string.Empty;
                var tehasilHi = string.Empty;
                var districtHi = string.Empty;
                var casteHi = string.Empty;

                var name = string.Empty;
                var fatherName = string.Empty;
                var resident = string.Empty;
                var tehasil = string.Empty;
                var district = string.Empty;
                var caste = string.Empty;

                nameHi = report.ApplicantNameHi;
                fatherNameHi = report.ApplicantFatherNameHi;
                residentHi = report.ApplicantResidentHi;
                tehasilHi = report.ApplicantTehasilHi;
                districtHi = report.ApplicantDistrictHi;
                casteHi = report.ApplicantCasteHi;

                name = report.ApplicantName;
                fatherName = report.ApplicantFatherName;
                resident = report.ApplicantResident;
                tehasil = report.ApplicantTehasil;
                district = report.ApplicantDistrict;
                caste = report.ApplicantCaste;

                var code1Hi = @"<div style='text-align:justify;'> प्रमाणित किया जाता है कि <strong>{{ApplicantNameHi}}</strong> पिता <strong>{{ApplicantFatherNameHi}}</strong> निवासी <strong>{{ApplicantResidentHi}}</strong> तहसील <strong>{{ApplicantTehasilHi}}</strong>
                    जिला <strong>{{ApplicantDistrictHi}}</strong> राज्य छत्तीसगढ़ के निवासी हैं, जो अन्य पिछड़ा वर्ग <strong>{{ApplicantCasteHi}}</strong> 
                    जाति का/कि हैं। जिसे पिछड़ा वर्ग रूप में छत्तीसगढ़ शासन, आदिम जाति, अनुसूचित जाति एवं पिछड़ा वर्ग कल्याण विभाग की अधिसूचना क्र.एफ-8-5/25/4/84, तिथि 26 दिसंबर, 1984
                    द्वारा अधिमान्य किया गया है तथा यह जाति छत्तीसगढ़ राज्य के अन्य पिछड़ा वर्ग की सूची के अनुक्रमांक 4 पर अंकित है। <strong>{{ApplicantNameHi}}</strong> और/या उनका
                    परिवार सामान्यतः छत्तीसगढ़ के जिला <strong>{{ApplicantDistrictHi}}</strong> तहसील <strong>{{ApplicantTehasilHi}}</strong> में निवास करता है।<br> 
                    जारी करने की तिथि (अनुमोदन तिथि) से छ: माह के लिये वेध । </div>";

                code1Hi = code1Hi.Replace("{{ApplicantNameHi}}", nameHi);
                code1Hi = code1Hi.Replace("{{ApplicantFatherNameHi}}", fatherNameHi);
                code1Hi = code1Hi.Replace("{{ApplicantResidentHi}}", residentHi);
                code1Hi = code1Hi.Replace("{{ApplicantTehasilHi}}", tehasilHi);
                code1Hi = code1Hi.Replace("{{ApplicantDistrictHi}}", districtHi);
                code1Hi = code1Hi.Replace("{{ApplicantCasteHi}}", casteHi);
                report.OBCSection1Hi = code1Hi;

                var code2Hi = @"<div style='text-align:justify;'> यह भी प्रमाणित किया जाता है कि <strong>{{ApplicantNameHi}}</strong> क्रीमीलेयर (संपन्न वर्ग) व्यक्तियों/वर्गो की श्रेणी मैं नहीं आते हैं, जिसका उल्लेख भारत सरकार कार्मिक एव प्रशिक्षण विभाग
                    के परिपत्र संख्या 36012/22/93/ईएसटीटी(एससीटी) दिनांक 08/09/1993 यथा संशोधित परिपत्र संख्या 36033/3/2004-ईएसटीटी(आरक्षण) दिनांक 14/10/2008 तथा सामान्य प्रशासन विभाग के परिपत्र क्रमांक
                    एफ-9-3/2001/1-3 दिनांक 24/06/2009 के कॉलम 3 में किया गया है| </div>";
                code2Hi = code2Hi.Replace("{{ApplicantNameHi}}", nameHi);
                report.OBCSection2Hi = code2Hi;

                var code3Hi = @"<div style='text-align:justify;'> यह प्रमाण पत्र इन उपबंधो के अधीन जारी किया गया है कि यदि जिला स्तरीय प्रमाणपत्र सत्यापन समिति अथवा उच्च स्तरीय प्रमाणीकरण छानबीन समिति के द्वारा 
                    अपनी जांच/निरीक्षण में आवेदक श्री/श्रीमती/सुश्री <strong>{{ApplicantNameHi}}</strong> के द्वारा प्रस्तुत सामाजिक प्रास्थिति का दावा असत्य अथवा कटपूर्ण पाया जाता 
                     है तो यह प्रमाण पत्र तत्काल प्रभाव से निरस्त माना जावेगा तथा तत् समय प्रवृत किसी अन्य विधि में अंतविष्ट किसी बात के होते हुए भी छत्तीसगढ़ अनुसुचित जाति, अनुसुचित जनजाति
                     और अन्य पिछड़ा वर्ग (सामाजिक प्रास्थिति के प्रमाणीकरण का विनियमन) अधिनियम,2013 की धारा 8 से 13 के अधीन कार्यवाही की जाएगी | </div>";
                code3Hi = code3Hi.Replace("{{ApplicantNameHi}}", nameHi);
                report.OBCSection3Hi = code3Hi;

                var code1 = @"<div style='text-align:justify;'> It is hereby certified that Mr/Mrs/Ms <strong>{{ApplicantName}}</strong> Father <strong>{{ApplicantFatherName}}</strong> resident of
                    <strong>{{ApplicantResident}}</strong> village tehsil <strong>{{ApplicantTehasil}}</strong> District <strong>{{ApplicantDistrict}}</strong> belongs to Other Backward Class <strong>{{ApplicantCaste}}</strong> caste. This
                    caste has been recognized as Other Backward Classes vide Notification no. F-8-5/25/4/84 dated 26th December 1984, as
                    amended from time to time by subsequent notifications, and this caste mentioned at serial no 4 as Other Backward
                    Class of Chhattisgarh state. Therefore, MR/Mrs/Ms <strong>{{ApplicantName}}</strong> Father <strong>{{ApplicantFatherName}}</strong> belongs to
                    Other Backward Class. </div>";
                code1 = code1.Replace("{{ApplicantName}}", name);
                code1 = code1.Replace("{{ApplicantFatherName}}", fatherName);
                code1 = code1.Replace("{{ApplicantResident}}", resident);
                code1 = code1.Replace("{{ApplicantTehasil}}", tehasil);
                code1 = code1.Replace("{{ApplicantDistrict}}", district);
                code1 = code1.Replace("{{ApplicantCaste}}", caste);
                report.OBCSection1 = code1;

                var code2 = @"<div style='text-align:justify;'> 2.This is also certified that Mr/Mrs/Ms <strong>{{ApplicantName}}</strong> does not belong to the category of creamy layer persons/
                    classes which have been mentioned in the circular no 36012/22/93/ESTT(SCT) dated 8/09/1993 as amended by notification
                    on 36033/3/2004-ESTT (Reservation) dated 14/10/2008 of Government of India, department of personnel and training and in
                    column 3 of circular number f-9-3/2001/1-3 dated 24/06/2009 of General Administration Department. </div>";
                code2 = code2.Replace("{{ApplicantName}}", name);
                report.OBCSection2 = code2;

                var code3 = @"<div style='text-align:justify;'> 3.This certificate has been issued under the provisions that in case the district level certificates verification committee or
                    high power certification scrutiny committee during its inquiry /inspection finds the claim submitted by Mr/Mrs/Ms <strong>{{ApplicantName}}</strong> 
                    is false or fraudulent, the certificate shall be deemed to be cancelled with immediate effect and
                    notwithstanding contained anything under any other law for the time being in force, action shall be initiated under
                    section 8-13 of the Chhattisgarh Scheduled Castes, Scheduled Tribes and Other Backward Classes (Regulation of social status certification). </div>";
                code3 = code3.Replace("{{ApplicantName}}", name);
                report.OBCSection3 = code3;


                if (report.ElectronicApplicationDate.IsNotNull())
                {
                    report.ElectronicApplicationDateText = string.Format("{0:dd MMM yyyy}", report.ElectronicApplicationDate);
                }
                if (report.IssueDate.IsNotNull())
                {
                    report.IssueDateText = string.Format("{0:dd MMM yyyy}", report.IssueDate);
                }

                var govtlogo = await _applicationDocumentBusiness.GetSingle(x => x.Code == "CHHATTISGARH_LOK_SEWA");
                if (govtlogo.IsNotNull())
                {
                    var govtlogobytes = await _fileBusiness.GetFileByte(govtlogo.DocumentId);
                    if (govtlogobytes.Length > 0)
                    {
                        report.GovtLogo = Convert.ToBase64String(govtlogobytes, 0, govtlogobytes.Length);
                    }
                }
                //var seallogo = await _applicationDocumentBusiness.GetSingle(x => x.Code == "CERTIFIED_LOGO");
                var seallogo = await _applicationDocumentBusiness.GetSingle(x => x.Code == "APPROVED_LOGO");
                if (seallogo.IsNotNull())
                {
                    var seallogobytes = await _fileBusiness.GetFileByte(seallogo.DocumentId);
                    if (seallogobytes.Length > 0)
                    {
                        report.SealLogo = Convert.ToBase64String(seallogobytes, 0, seallogobytes.Length);
                    }
                }
                return Ok(report);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet]
        [Route("GetCSCMarriageCertificateReport")]
        public async Task<IActionResult> GetCSCMarriageCertificateReport(string serviceId)
        {
            var _userContext = _serviceProvider.GetService<IUserContext>();
            var _companyBusiness = _serviceProvider.GetService<ICompanyBusiness>();
            var _commonServiceBusiness = _serviceProvider.GetService<ICommonServiceBusiness>();
            var _applicationDocumentBusiness = _serviceProvider.GetService<IApplicationDocumentBusiness>();
            var _fileBusiness = _serviceProvider.GetService<IFileBusiness>();
            var report = new CSCReportMarriageCertificateViewModel();
            try
            {
                report = await _commonServiceBusiness.GetCSCMarriageCertificateData(serviceId);
                if (report == null)
                {
                    report = new CSCReportMarriageCertificateViewModel();
                }

                //report.ElectronicApplicationDate = new DateTime(2022,06,28);
                //report.ApplicationReferenceNumber = "2303012226000258";
                //report.ChoiceCenter = "लोकसेवा केंद्र";
                //report.NetAmount = 5.00;
                //report.RegistrationNumber = "2022-26-000261";

                //report.GroomName = "Groom Name";
                //report.GroomFatherName = "GroomFatherName";
                //report.GroomAge = 25;
                //report.GroomAddress = "GroomAddress";
                //report.GroomNameHi = "दूल्हे का नाम";
                //report.GroomFatherNameHi = "दूल्हे के पिता का नाम";
                //report.GroomAddressHi = "दूल्हे का पता";

                //report.BrideName = "BrideName";
                //report.BrideFatherName = "BrideFatherName";
                //report.BrideAge = 25;
                //report.BrideAddress = "BrideAddress";
                //report.BrideNameHi = "दुल्हन का नाम";
                //report.BrideFatherNameHi = "दुल्हन के पिता का नाम";
                //report.BrideAddressHi = "दुल्हन का पता";

                //report.MarriageDate = new DateTime(2022, 05, 20);
                //report.ApprovedDate = new DateTime(2022,06,30);
                //report.MunicipalOfficeName = "KONDAGAON MUNICIPAL COUNCIL";
                //report.MunicipalOfficeNameHi = "कोण्डागांव नगर पालिका परिषद";
                //report.AuthorityName = "XYZ";

                var groomName = string.Empty;
                var groomFatherName = string.Empty;
                int groomAge = 0;
                var groomAddress = string.Empty;
                var groomNameHi = string.Empty;
                var groomFatherNameHi = string.Empty;
                var groomAddressHi = string.Empty;

                var brideName = string.Empty;
                var brideFatherName = string.Empty;
                int brideAge = 0;
                var brideAddress = string.Empty;
                var brideNameHi = string.Empty;
                var brideFatherNameHi = string.Empty;
                var brideAddressHi = string.Empty;

                groomName = report.GroomName;
                groomFatherName = report.GroomFatherName;
                groomAge = report.GroomAge;
                groomAddress = report.GroomAddress;
                groomNameHi = report.GroomNameHi;
                groomFatherNameHi = report.GroomFatherNameHi;
                groomAddressHi = report.GroomAddressHi;

                brideName = report.BrideName;
                brideFatherName = report.BrideFatherName;
                brideAge = report.BrideAge;
                brideAddress = report.BrideAddress;
                brideNameHi = report.BrideNameHi;
                brideFatherNameHi = report.BrideFatherNameHi;
                brideAddressHi = report.BrideAddressHi;
                
                if (report.MarriageDate.IsNotNull())
                {
                    report.MarriageDateText = string.Format("{0:dd MMM yyyy}", report.MarriageDate);
                }

                var code1Hi = @"<div style='text-align:justify;line-height: 1.6;'> प्रमाणित किया जाता है कि श्री <strong>{{GroomNameHi}}</strong> पुत्र श्री <strong>{{GroomFatherNameHi}}</strong> आयु <strong>{{GroomAge}}</strong> वर्ष 
                                निवासी <strong>{{GroomAddressHi}}</strong> एवं श्रीमती <strong>{{BrideNameHi}}</strong> पुत्री श्री <strong>{{BrideFatherNameHi}}</strong> आयु <strong>{{BrideAge}}</strong> वर्ष 
                                निवासी <strong>{{BrideAddressHi}}</strong> दिनाकं <strong>{{MarriageDate}}</strong> को विवाह बंधन में बंधे | </div>";
                
                code1Hi = code1Hi.Replace("{{GroomNameHi}}", groomNameHi);
                code1Hi = code1Hi.Replace("{{GroomFatherNameHi}}", groomFatherNameHi);
                code1Hi = code1Hi.Replace("{{GroomAge}}", groomAge.ToString());
                code1Hi = code1Hi.Replace("{{GroomAddressHi}}", groomAddressHi);
                code1Hi = code1Hi.Replace("{{BrideNameHi}}", brideNameHi);
                code1Hi = code1Hi.Replace("{{BrideFatherNameHi}}", brideFatherNameHi);
                code1Hi = code1Hi.Replace("{{BrideAge}}", brideAge.ToString());
                code1Hi = code1Hi.Replace("{{BrideAddressHi}}", brideAddressHi);
                code1Hi = code1Hi.Replace("{{MarriageDate}}", report.MarriageDateText);
                report.Section1Hi = code1Hi;


                var code1 = @"<div style='text-align:justify;line-height: 1.6;'> It is here by certified that Mr. <strong>{{GroomName}}</strong> S/o Mr. <strong>{{GroomFatherName}}</strong> aged <strong>{{GroomAge}}</strong> years
                                resident of <strong>{{GroomAddress}}</strong> and Mrs <strong>{{BrideName}}</strong> D/o Mr. <strong>{{BrideFatherName}}</strong> aged <strong>{{BrideAge}}</strong> years 
                                resident of <strong>{{BrideAddress}}</strong> tied in the knot of marriage on Date <strong>{{MarriageDate}}</strong>. </div>";
                code1 = code1.Replace("{{GroomName}}", groomName);
                code1 = code1.Replace("{{GroomFatherName}}", groomFatherName);
                code1 = code1.Replace("{{GroomAge}}", groomAge.ToString());
                code1 = code1.Replace("{{GroomAddress}}", groomAddress);
                code1 = code1.Replace("{{BrideName}}", brideName);
                code1 = code1.Replace("{{BrideFatherName}}", brideFatherName);
                code1 = code1.Replace("{{BrideAge}}", brideAge.ToString());
                code1 = code1.Replace("{{BrideAddress}}", brideAddress);
                code1 = code1.Replace("{{MarriageDate}}", report.MarriageDateText);
                report.Section1 = code1;

                if (report.ElectronicApplicationDate.IsNotNull())
                {
                    report.ElectronicApplicationDateText = string.Format("{0:dd MMM yyyy}", report.ElectronicApplicationDate);
                }
                if (report.ApprovedDate.IsNotNull())
                {
                    report.ApprovedDateText = string.Format("{0:dd MMM yyyy}", report.ApprovedDate);
                }
                report.PrintDate = DateTime.Today;
                if (report.PrintDate.IsNotNull())
                {
                    report.PrintDateText = string.Format("{0:dd MMM yyyy}", report.PrintDate);
                }
                if (report.GroomImageId.IsNotNullAndNotEmpty())
                {
                    var groomimagebytes = await _fileBusiness.GetFileByte(report.GroomImageId);
                    if (groomimagebytes.Length > 0)
                    {
                        report.GroomImage = Convert.ToBase64String(groomimagebytes, 0, groomimagebytes.Length);
                    }
                }
                else
                {
                    var groomlogo = await _applicationDocumentBusiness.GetSingle(x => x.Code == "CSC_GROOM_LOGO");
                    if (groomlogo.IsNotNull())
                    {
                        var groomimagebytes = await _fileBusiness.GetFileByte(groomlogo.DocumentId);
                        if (groomimagebytes.Length > 0)
                        {
                            report.GroomImage = Convert.ToBase64String(groomimagebytes, 0, groomimagebytes.Length);
                        }
                    }
                }
                if (report.BrideImageId.IsNotNullAndNotEmpty())
                {
                    var brideimagebytes = await _fileBusiness.GetFileByte(report.BrideImageId);
                    if (brideimagebytes.Length > 0)
                    {
                        report.BrideImage = Convert.ToBase64String(brideimagebytes, 0, brideimagebytes.Length);
                    }
                }
                else
                {
                    var bridelogo = await _applicationDocumentBusiness.GetSingle(x => x.Code == "CSC_BRIDE_LOGO");
                    if (bridelogo.IsNotNull())
                    {
                        var brideimagebytes = await _fileBusiness.GetFileByte(bridelogo.DocumentId);
                        if (brideimagebytes.Length > 0)
                        {
                            report.BrideImage = Convert.ToBase64String(brideimagebytes, 0, brideimagebytes.Length);
                        }
                    }
                }
                if (report.GroomBrideImageId.IsNotNullAndNotEmpty())
                {
                    var groombrideimagebytes = await _fileBusiness.GetFileByte(report.GroomBrideImageId);
                    if (groombrideimagebytes.Length > 0)
                    {
                        report.GroomBrideImage = Convert.ToBase64String(groombrideimagebytes, 0, groombrideimagebytes.Length);
                    }
                }
                var govtlogo = await _applicationDocumentBusiness.GetSingle(x => x.Code == "CHHATTISGARH_LOK_SEWA");
                if (govtlogo.IsNotNull())
                {
                    var govtlogobytes = await _fileBusiness.GetFileByte(govtlogo.DocumentId);
                    if (govtlogobytes.Length > 0)
                    {
                        report.GovtLogo = Convert.ToBase64String(govtlogobytes, 0, govtlogobytes.Length);
                    }
                }
                //var seallogo = await _applicationDocumentBusiness.GetSingle(x => x.Code == "CERTIFIED_LOGO");
                var seallogo = await _applicationDocumentBusiness.GetSingle(x => x.Code == "APPROVED_LOGO");
                if (seallogo.IsNotNull())
                {
                    var seallogobytes = await _fileBusiness.GetFileByte(seallogo.DocumentId);
                    if (seallogobytes.Length > 0)
                    {
                        report.SealLogo = Convert.ToBase64String(seallogobytes, 0, seallogobytes.Length);
                    }
                }
                return Ok(report);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
