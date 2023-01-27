using CMS.Business.Interface.BusinessScript.Service.General.CoreHR;
using CMS.Common;
using CMS.UI.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Business.Implementations.BusinessScript.Service.General.CoreHR
{
    public class ServiceGeneralCoreHRPostScript : IServiceGeneralCoreHRPostScript
    {
        public CommandResult<ServiceTemplateViewModel> Test(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            throw new NotImplementedException();
        }
        #region Leave Post scripts
        /// <summary>
        /// Update Leave Balance for Annual Leave        
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="uc"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
        public async Task<CommandResult<ServiceTemplateViewModel>> UpdateLeaveAdjustment(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _lovBusiness = sp.GetService<ILOVBusiness>();
            var lov = await _lovBusiness.GetSingle(x => x.LOVType == "LOV_SERVICE_ACTION" && x.Code == "SERVICE_ACTION_DRAFT");
            if (viewModel.ServiceStatusCode != "SERVICE_STATUS_DRAFT")
            {
                var leaveBalanceBusiness = sp.GetService<ILeaveBalanceSheetBusiness>();
                await leaveBalanceBusiness.UpdateLeaveBalance(DateTime.Today, "ANNUAL_LEAVE", viewModel.OwnerUserId);
            }
            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }
        /// <summary>
        /// Update Accural Leave 
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="uc"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
        public async Task<CommandResult<ServiceTemplateViewModel>> UpdateLeaveAccrual(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _lovBusiness = sp.GetService<ILOVBusiness>();
            var lov = await _lovBusiness.GetSingle(x => x.LOVType == "LOV_SERVICE_ACTION" && x.Code == "SERVICE_ACTION_DRAFT");
            if (viewModel.ServiceStatusCode != "SERVICE_STATUS_DRAFT")
            {
                var leaveBalanceBusiness = sp.GetService<ILeaveBalanceSheetBusiness>();
                await leaveBalanceBusiness.UpdateLeaveBalance(DateTime.Today, "ANNUAL_LEAVE", viewModel.OwnerUserId);
            }
            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }
        /// <summary>
        /// Update Annual Leave Encashment
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="uc"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
        public async Task<CommandResult<ServiceTemplateViewModel>> UpdateAnnualLeaveEncashment(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _lovBusiness = sp.GetService<ILOVBusiness>();
            var leaveBalanceBusiness = sp.GetService<ILeaveBalanceSheetBusiness>();
            var lov = await _lovBusiness.GetList(x => x.LOVType == "LOV_SERVICE_ACTION");
            if (viewModel.ServiceStatusCode == "SERVICE_STATUS_INPROGRESS")
            {
                await leaveBalanceBusiness.UpdateLeaveBalance(DateTime.Today, "ANNUAL_LEAVE_ENCASHMENT", viewModel.OwnerUserId);
            }
            if (viewModel.ServiceStatusCode == "SERVICE_STATUS_CANCEL")
            {
                await leaveBalanceBusiness.UpdateLeaveBalance(DateTime.Today, "ANNUAL_LEAVE_ENCASHMENT", viewModel.OwnerUserId);
            }
            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }

        /// <summary>
        /// Update Leave Request
        /// </summary>
        /// <param name="leaveTypeCode"></param>
        /// <param name="viewModel"></param>
        /// <param name="uc"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
        private async ValueTask UpdateLeaveRequest(string leaveTypeCode, ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _lovBusiness = sp.GetService<ILOVBusiness>();
            var _tableMetadataBusiness = sp.GetService<ITableMetadataBusiness>();
            //DataRow stage = await _tableMetadataBusiness.GetTableDataByHeaderId(viewModel.TemplateId, viewModel.UdfNoteId);
            var lov = await _lovBusiness.GetSingle(x => x.LOVType == "LOV_SERVICE_ACTION" && x.Code == "SERVICE_ACTION_DRAFT");
            if (viewModel.ServiceStatusCode != "SERVICE_STATUS_DRAFT")
            {
                var leaveBalanceBusiness = sp.GetService<ILeaveBalanceSheetBusiness>();
                // var startDate = viewModel.Controls.FirstOrDefault(x => x.FieldName == "LeaveStartDate").Code.ToSafeDateTime().Date;
                //if (stage != null)
                //{
                //var tst = udf.LeaveStartDate;
                var startDate = udf.LeaveStartDate != null ? Convert.ToDateTime(udf.LeaveStartDate) : DateTime.Now; //stage["LeaveStartDate"].ToString();
                if (startDate != null)
                {
                    // LeaveStartDate = Convert.ToDateTime(startDate);
                    await leaveBalanceBusiness.UpdateLeaveBalance(startDate, leaveTypeCode, viewModel.OwnerUserId);
                }
                // Convert.ToDateTime(startDate);

                //}
            }
        }

        /// <summary>
        /// Update Leave Balance Upon Maternity Leave 
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="uc"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
        public async Task<CommandResult<ServiceTemplateViewModel>> UpdateMaternityLeave(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {

            await UpdateLeaveRequest("MATERNITY_LEAVE", viewModel, udf, uc, sp);

            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }
        /// <summary>
        /// Update Leave Balance Upon Maternity Leave 
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="uc"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
        public async Task<CommandResult<ServiceTemplateViewModel>> UpdateMaternityLeaveForCayanUAE(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {

            await UpdateLeaveRequest("MATERNITY_U", viewModel, udf, uc, sp);

            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }
        /// <summary>
        /// Update Leave Balance Upon Paternity Leave 
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="uc"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
        public async Task<CommandResult<ServiceTemplateViewModel>> UpdatePaternityLeave(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {

            await UpdateLeaveRequest("PATERNITY_LEAVE", viewModel, udf, uc, sp);

            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }
        /// <summary>
        /// Update Leave Balance Upon Hajj Leave 
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="uc"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
        public async Task<CommandResult<ServiceTemplateViewModel>> UpdateHajjLeave(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {

            await UpdateLeaveRequest("HAJJ_LEAVE", viewModel, udf, uc, sp);

            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }
        /// <summary>
        /// Update Leave Balance Upon Hajj Leave UAE
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="uc"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
        public async Task<CommandResult<ServiceTemplateViewModel>> UpdateHajjLeaveForCayanUAE(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {

            await UpdateLeaveRequest("HAJJ_LEAVE_U", viewModel, udf, uc, sp);

            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }
        /// <summary>
        /// Update Leave Balance Upon Marrige Leave 
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="uc"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
        public async Task<CommandResult<ServiceTemplateViewModel>> UpdateMarriageLeave(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {

            await UpdateLeaveRequest("MARRIAGE_LEAVE", viewModel, udf, uc, sp);

            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }
        /// <summary>
        /// Update Leave Balance Upon Exceptional Leave 
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="uc"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
        public async Task<CommandResult<ServiceTemplateViewModel>> UpdateExceptionalLeave(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {

            await UpdateLeaveRequest("EXCEPTIONAL_LEAVE", viewModel, udf, uc, sp);

            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }
        /// <summary>
        /// Update Leave Balance Upon Compassionate Leave 
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="uc"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
        public async Task<CommandResult<ServiceTemplateViewModel>> UpdateCompassionateLeave(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {

            await UpdateLeaveRequest("COMPASSIONATE_LEAVE", viewModel, udf, uc, sp);

            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }
        /// <summary>
        /// Update Leave Balance Upon Edda Leave 
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="uc"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
        public async Task<CommandResult<ServiceTemplateViewModel>> UpdateEddaLeave(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {

            await UpdateLeaveRequest("EDDA_LEAVE", viewModel, udf, uc, sp);

            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }
        /// <summary>
        /// Update Leave Balance Upon Edda Leave 
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="uc"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
        public async Task<CommandResult<ServiceTemplateViewModel>> UpdateEddaLeaveForCayanUAE(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {

            await UpdateLeaveRequest("IDDAH_L_UAE", viewModel, udf, uc, sp);

            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }
        /// <summary>
        /// Update Leave Balance Upon Exam Leave 
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="uc"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
        public async Task<CommandResult<ServiceTemplateViewModel>> UpdateExamLeave(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {

            await UpdateLeaveRequest("EXAM_LEAVE", viewModel, udf, uc, sp);

            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }
        /// <summary>
        /// Update Leave Balance Upon Sick Leave 
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="uc"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
        public async Task<CommandResult<ServiceTemplateViewModel>> UpdateSickLeave(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {

            await UpdateLeaveRequest("SICK_LEAVE", viewModel, udf, uc, sp);

            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }
        /// <summary>
        /// Update Leave Balance Upon Sick Leave 
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="uc"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
        public async Task<CommandResult<ServiceTemplateViewModel>> UpdateSickLeaveKSA(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {

            await UpdateLeaveRequest("SICK_L_K", viewModel, udf, uc, sp);

            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }
        /// <summary>
        /// Update Leave Balance Upon UnAuthorized Leave Without Pay
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="uc"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
        public async Task<CommandResult<ServiceTemplateViewModel>> UpdateAuthorizedLeaveWithoutPayForCayanKSA(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            await UpdateLeaveRequest("UNPAID_L", viewModel, udf, uc, sp);
            var _lovBusiness = sp.GetService<ILOVBusiness>();

            var lov = await _lovBusiness.GetList(x => x.LOVType == "LOV_SERVICE_ACTION");

            List<KeyValuePair<string, string>> errorList = new List<KeyValuePair<string, string>>();
            if (viewModel.ServiceStatusCode == "SERVICE_STATUS_COMPLETE")
            {
                await AddAttendanceForKSA(viewModel, udf, errorList, "UNPAID_L", uc, sp);
            }
            if (viewModel.ServiceStatusCode == "SERVICE_STATUS_CANCEL")
            //if (viewModel.TemplateAction == NtsActionEnum.Cancel)
            {
                await CancelLeaveAttendanceForKSA(viewModel, udf, errorList, "UNPAID_L", uc, sp);
            }


            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }
        private async ValueTask AddAttendanceForKSA(ServiceTemplateViewModel viewModel, dynamic udf, List<KeyValuePair<string, string>> errorList, string leaveType, IUserContext uc, IServiceProvider sp)
        {
            // get start date and end date           
            //var _tableMetadataBusiness = sp.GetService<ITableMetadataBusiness>();
            // DataRow stage = await _tableMetadataBusiness.GetTableDataByHeaderId(viewModel.TemplateId, viewModel.UdfNoteId);
            //var startDate = viewModel.Controls.FirstOrDefault(x => x.FieldName == "startDate").Code.ToSafeDateTime().Date;
            // var endDate = viewModel.Controls.FirstOrDefault(x => x.FieldName == "endDate").Code.ToSafeDateTime().Date;

            string startDate = udf.LeaveStartDate;
            string endDate = udf.LeaveEndDate;

            if (startDate.IsNotNullAndNotEmpty() && endDate.IsNotNullAndNotEmpty())
            {
                // loop through dates
                for (var date = Convert.ToDateTime(startDate); date <= Convert.ToDateTime(endDate); date = date.AddDays(1))
                {
                    // Check entry if the attendance available for the date
                    await AddAbsentAttendanceForKSA(date, viewModel, leaveType, uc, sp);
                }
            }


        }
        private async ValueTask AddAbsentAttendanceForKSA(DateTime date, ServiceTemplateViewModel viewModel, String leaveType, IUserContext uc, IServiceProvider sp)
        {
            var leveBalanceSheetBusiness = sp.GetService<ILeaveBalanceSheetBusiness>();
            var hrCoreBusiness = sp.GetService<IHRCoreBusiness>();
            var _lovBusiness = sp.GetService<ILOVBusiness>();
            var attendanceBusiness = sp.GetService<IAttendanceBusiness>();
            if (viewModel.OwnerUserId != null)
            {
                var person = await hrCoreBusiness.GetPersonDetailByUserId(viewModel.OwnerUserId);
                var attendance = await attendanceBusiness.GetAttendanceSingleForPersonandDate(person.Id, date);
                if (attendance.IsNotNull())
                {
                    //correct
                    var lov = await _lovBusiness.GetList(x => x.LOVType == "AttendanceLeaveType");
                    var lov1 = await _lovBusiness.GetList(x => x.LOVType == "AttendanceType");
                    attendance.OverrideAttendanceId = lov1.Where(x => x.Code == "ABSENT").Select(x => x.Id).FirstOrDefault();
                    if (leaveType == "UNPAID_L" || leaveType == "AUTH_LEAVE_WITHOUT_PAY")
                    {
                        attendance.AttendanceLeaveTypeId = lov.Where(x => x.Code == "UNPAID_LEAVE").Select(x => x.Id).FirstOrDefault();
                    }
                    else if (leaveType == "UNDERTIME_REQUEST")
                    {
                        attendance.AttendanceLeaveTypeId = lov.Where(x => x.Code == "UNDER_TIME").Select(x => x.Id).FirstOrDefault();
                        attendance.SystemAttendanceId = lov1.Where(x => x.Code == "PRESENT").Select(x => x.Id).FirstOrDefault();
                        attendance.UnderTimeHours = await leveBalanceSheetBusiness.getUnderTimeHours(viewModel.ServiceId);// get it from udfs
                    }
                    attendance.IsCalculated = true;
                    attendance.PayrollPostedStatus = null;
                    attendance.IsOverridden = true;
                    await attendanceBusiness.CorrectAttendance(attendance);
                }
                else
                {
                    //create
                    var model = new AttendanceViewModel
                    {
                        AttendanceDate = date,
                        SystemAttendance = AttendanceTypeEnum.Absent,
                        IsCalculated = true,
                        PayrollPostedStatus = null,
                        UserId = viewModel.OwnerUserId,
                        CreatedDate = DateTime.Now,
                        LastUpdatedDate = DateTime.Now,
                        CreatedBy = uc.UserId,
                    };
                    var lov = await _lovBusiness.GetList(x => x.LOVType == "AttendanceLeaveType");
                    var lov1 = await _lovBusiness.GetList(x => x.LOVType == "AttendanceType");
                    if (leaveType == "UNPAID_L" || leaveType == "AUTH_LEAVE_WITHOUT_PAY")
                    {
                        model.AttendanceLeaveTypeId = lov.Where(x => x.Code == "UNPAID_LEAVE").Select(x => x.Id).FirstOrDefault();
                    }
                    else if (leaveType == "UNDERTIME_REQUEST")
                    {
                        model.AttendanceLeaveTypeId = lov.Where(x => x.Code == "UNDER_TIME").Select(x => x.Id).FirstOrDefault();
                        model.SystemAttendanceId = lov1.Where(x => x.Code == "PRESENT").Select(x => x.Id).FirstOrDefault();
                        model.UnderTimeHours = await leveBalanceSheetBusiness.getUnderTimeHours(viewModel.ServiceId); // get it from udfs
                    }
                    await attendanceBusiness.CreateAttendance(model);

                }
            }

        }
        private async ValueTask CancelLeaveAttendanceForKSA(ServiceTemplateViewModel viewModel, dynamic udf, List<KeyValuePair<string, string>> errorList, string leaveType, IUserContext uc, IServiceProvider sp)
        {
            // get start date and end date
            //var _tableMetadataBusiness = sp.GetService<ITableMetadataBusiness>();
            //DataRow stage = await _tableMetadataBusiness.GetTableDataByHeaderId(viewModel.TemplateId, viewModel.UdfNoteId);
            //var startDate = viewModel.Controls.FirstOrDefault(x => x.FieldName == "startDate").Code.ToSafeDateTime().Date;
            // var endDate = viewModel.Controls.FirstOrDefault(x => x.FieldName == "endDate").Code.ToSafeDateTime().Date;

            string startDate = udf.LeaveStartDate;
            string endDate = udf.LeaveEndDate;

            if (startDate.IsNotNullAndNotEmpty() && endDate.IsNotNullAndNotEmpty())
            {

                // loop through dates
                for (var date = Convert.ToDateTime(startDate); date <= Convert.ToDateTime(endDate); date = date.AddDays(1))
                {
                    // Check entry if the attendance available for the date
                    await CancelAbsentAttendanceForKSA(date, viewModel, udf, leaveType, uc, sp);
                }
            }

        }
        private async ValueTask CancelAbsentAttendanceForKSA(DateTime date, ServiceTemplateViewModel viewModel, dynamic udf, string leaveType, IUserContext uc, IServiceProvider sp)
        {
            var payTransBusiness = sp.GetService<IPayrollTransactionsBusiness>();
            var hrCoreBusiness = sp.GetService<IHRCoreBusiness>();
            var attendanceBusiness = sp.GetService<IAttendanceBusiness>();
            var _lovBusiness = sp.GetService<ILOVBusiness>();
            var OwnerDetails = await hrCoreBusiness.GetPersonDetailByUserId(viewModel.OwnerUserId);
            // var OwnerDetails = userBusiness.GetPersonByUserId(viewModel.OwnerUserId);
            viewModel.PersonId = OwnerDetails.Id;

            if (viewModel.OwnerUserId != null)
            {
                var person = await hrCoreBusiness.GetPersonDetailByUserId(viewModel.OwnerUserId);
                //var person = userBusiness.GetPersonByUserId(viewModel.OwnerUserId);
                var attendance = await attendanceBusiness.GetAttendanceSingleForPersonandDate(person.Id, date);
                if (attendance.IsNotNull() && attendance.PayrollPostedStatus == null)
                {
                    if (leaveType == "UNDERTIME_REQUEST")
                    {
                        attendance.OverrideUnderTimeHours = TimeSpan.Parse("0");
                    }
                    else
                    {
                        //correct

                        var lov1 = await _lovBusiness.GetList(x => x.LOVType == "AttendanceType");
                        attendance.IsOverridden = true;
                        attendance.OverrideAttendanceId = lov1.Where(x => x.Code == "PRESENT").Select(x => x.Id).FirstOrDefault();
                    }
                    await attendanceBusiness.CorrectAttendance(attendance);
                }
                else if (attendance.IsNotNull() && attendance.PayrollPostedStatus == PayrollPostedStatusEnum.Posted)
                {
                    if (leaveType == "AUTH_LEAVE_WITHOUT_PAY")
                    {
                        var isTransExist = await payTransBusiness.GetPayrollTransationDataByReferenceId(viewModel.ServiceId);
                        if (isTransExist.IsNotNull())
                        {
                            var existTrans = await payTransBusiness.GetPayrollTransactionBasedonElement(viewModel.OwnerUserId, date, "UNPAID_LEAVE");
                            if (existTrans.IsNotNull())
                            {
                                var payModel = await GeneratePayrollTransactionViewModel(Convert.ToDouble(existTrans.FirstOrDefault().DeductionAmount), date, "UNPAID_LEAVE_REV", viewModel, uc, sp);
                                // payTransBusiness.Create(payModel);
                                payModel.DataAction = DataActionEnum.Create;
                                payModel.CreatedBy = uc.UserId;
                                payModel.LastUpdatedBy = uc.UserId;
                                await payTransBusiness.ManagePayrollTransaction(payModel);
                            }
                        }
                    }
                    else if (leaveType == "UNDERTIME_REQUEST")
                    {
                        var _tableMetadataBusiness = sp.GetService<ITableMetadataBusiness>();
                        DataRow stage = await _tableMetadataBusiness.GetTableDataByHeaderId(viewModel.TemplateId, viewModel.UdfNoteId);
                        if (stage != null)
                        {
                            string deductionFrom = udf.DeductionType;
                            // var deductionFrom = viewModel.Controls.FirstOrDefault(x => x.FieldName == "deductionType").Code.ToString();

                            if (deductionFrom == DeductionTypeEnum.DeductFromHourlyRateOfSalary.ToString())
                            {
                                var existTrans = await payTransBusiness.GetPayrollTransactionBasedonElement(viewModel.OwnerUserId, date, "UNDERTIME_DED");
                                if (existTrans.IsNotNull())
                                {
                                    var payModel = await GeneratePayrollTransactionViewModel(Convert.ToDouble(existTrans.FirstOrDefault().DeductionAmount), date, "UNDERTIME_DED_REV", viewModel, uc, sp);
                                    //  payTransBusiness.Create(payModel);
                                    payModel.DataAction = DataActionEnum.Create;
                                    payModel.CreatedBy = uc.UserId;
                                    payModel.LastUpdatedBy = uc.UserId;
                                    await payTransBusiness.ManagePayrollTransaction(payModel);
                                }
                            }
                        }
                        attendance.OverrideUnderTimeHours = TimeSpan.Parse("0");
                    }
                    var lov1 = await _lovBusiness.GetList(x => x.LOVType == "AttendanceType");
                    attendance.IsOverridden = true;
                    attendance.OverrideAttendanceId = lov1.Where(x => x.Code == "PRESENT").Select(x => x.Id).FirstOrDefault();
                    await attendanceBusiness.CorrectAttendance(attendance);
                }
            }

        }
        private async Task<PayrollTransactionViewModel> GeneratePayrollTransactionViewModel(double amount, DateTime effectiveDate, string elementCode, ServiceTemplateViewModel viewModel, IUserContext uc, IServiceProvider sp)
        {
            var _lovBusiness = sp.GetService<ILOVBusiness>();
            var _payrollTransactionBusiness = sp.GetService<IPayrollTransactionsBusiness>();
            var lov = await _lovBusiness.GetSingle(x => x.LOVType == "PAYROLL_PROCESS_STATUS" && x.Code == "NOT_PROCESSED");
            var element = await _payrollTransactionBusiness.GetPayrollElementByCode(elementCode);
            var model = new PayrollTransactionViewModel
            {

                Amount = amount,
                EffectiveDate = effectiveDate,
                ProcessStatusId = lov.Id,
                PostedDate = DateTime.Now,

                ElementId = element.Id,
                PersonId = viewModel.PersonId,
                CreatedDate = DateTime.Now,
                LastUpdatedDate = DateTime.Now,
                //CreatedBy = ,
                //LastUpdatedBy = viewModel.LoggedInUserId,
                ReferenceNode = NodeEnum.NTS_Service,
                ReferenceId = viewModel.Id
            };
            return model;
        }
        /// <summary>
        /// Update Leave Balance Upon Undertime Leave 
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="uc"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
        public async Task<CommandResult<ServiceTemplateViewModel>> UpdateUndertimeLeaveRequest(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            List<KeyValuePair<string, string>> errorList = new List<KeyValuePair<string, string>>();
            await UpdateLeaveRequest("UNDERTIME_REQUEST", viewModel, udf, uc, sp);

            var _lovBusiness = sp.GetService<ILOVBusiness>();

            var lov = await _lovBusiness.GetList(x => x.LOVType == "LOV_SERVICE_ACTION");
            if (viewModel.ServiceStatusCode == "SERVICE_STATUS_COMPLETE")
            {
                await AddAttendanceForKSA(viewModel, udf, errorList, "UNDERTIME_REQUEST", uc, sp);
            }
            if (viewModel.ServiceStatusCode == "SERVICE_STATUS_CANCEL")
            {
                await CancelLeaveAttendanceForKSA(viewModel, udf, errorList, "UNDERTIME_REQUEST", uc, sp);
            }
            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }
        /// <summary>
        /// Update Leave Balance Upon Annual Leave Half Day
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="uc"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
        public async Task<CommandResult<ServiceTemplateViewModel>> UpdateAnnualLeaveHalfDayKSA(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            await UpdateLeaveRequest("ANNUAL_LEAVE_HD", viewModel, udf, uc, sp);
            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }
        public async Task<CommandResult<ServiceTemplateViewModel>> UpdateLeaveCancelForCayanKSA(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _lovBusiness = sp.GetService<ILOVBusiness>();
            var lov = await _lovBusiness.GetSingle(x => x.LOVType == "LOV_SERVICE_ACTION" && x.Code == "SERVICE_ACTION_COMPLETE");

            if (/*viewModel.ParentServiceType == ParentServiceTypeEnum.LeaveCancel*/
            //&& 
            viewModel.ParentServiceId.IsNotNullAndNotEmpty()
            && viewModel.ServiceStatusCode == "SERVICE_STATUS_COMPLETE")
            {
                var serviceBusiness = sp.GetService<IServiceBusiness>();
                var leaveToCancel = await serviceBusiness.GetServiceDetails(new ServiceTemplateViewModel
                {
                    Id = viewModel.ParentServiceId,
                    ActiveUserId = viewModel.ActiveUserId,
                    DataAction = DataActionEnum.Edit
                }
                );
                var reason = udf.CancelReason;// viewModel.Controls.FirstOrDefault(x => x.FieldName == "cancelReason").Code;
                leaveToCancel.ServiceStatusCode = "SERVICE_STATUS_CANCEL";                       //leaveToCancel.TemplateAction = NtsActionEnum.Cancel;
                leaveToCancel.DataAction = DataActionEnum.Edit;
                leaveToCancel.CancelReason = reason;
                await serviceBusiness.ManageService(leaveToCancel);
                var leaveBalanceBusiness = sp.GetService<ILeaveBalanceSheetBusiness>();

                var startDate = DateTime.Today;
                //  await leaveBalanceBusiness.UpdateLeaveBalance(startDate, leaveToCancel.TemplateCode, viewModel.OwnerUserId);
            }

            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }
        public async Task<CommandResult<ServiceTemplateViewModel>> UpdateAnnualLeave(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            await UpdateLeaveRequest("ANNUAL_LEAVE", viewModel, udf, uc, sp);
            var _servicebusiness = sp.GetService<IServiceBusiness>();
            var _lovBusiness = sp.GetService<ILOVBusiness>();
            var _payrollBusiness = sp.GetService<IPayrollTransactionsBusiness>();
            var lov = await _lovBusiness.GetList(x => x.LOVType == "LOV_SERVICE_ACTION");
            if (viewModel.ServiceStatusCode == "SERVICE_STATUS_INPROGRESS")
            {
                var _taskBussiness = sp.GetService<ITaskBusiness>();
                //var AnnualLeaveservice =await _servicebusiness.GetServiceDetails(new ServiceTemplateViewModel
                //{
                //    ServiceId = viewModel.ServiceId,
                //    DataAction = DataActionEnum.Read,
                //    ActiveUserId = viewModel.ActiveUserId
                //});
                var service = await _servicebusiness.GetServiceDetails(new ServiceTemplateViewModel
                {
                    TemplateCode = "LeaveHandoverService",
                    ActiveUserId = viewModel.ActiveUserId,
                    OwnerUserId = viewModel.OwnerUserId,
                    DataAction = DataActionEnum.Create,

                });
                //service.ParentServiceType = ParentServiceTypeEnum.LeaveHandOver;
                service.OwnerUserId = viewModel.OwnerUserId;
                service.RequestedByUserId = viewModel.RequestedByUserId;
                service.ParentServiceId = viewModel.ServiceId;
                service.Subject = service.Description = string.Concat("Leave Handover service");
                service.ServiceStatusCode = "SERVICE_STATUS_INPROGRESS";
                //DateTime AnnualLeaveStartDate = viewModel.Controls.FirstOrDefault(x => x.FieldName == "startDate").Code.ToSafeDateTime().Date;
                string AnnualLeaveStartDate = udf.LeaveStartDate;
                service.StartDate = DateTime.Now.Date;
                if (AnnualLeaveStartDate.IsNotNullAndNotEmpty())
                {
                    if (Convert.ToDateTime(AnnualLeaveStartDate) > DateTime.Now.AddDays(5))// After 5 days
                    {
                        service.DueDate = Convert.ToDateTime(AnnualLeaveStartDate).AddDays(-5);
                    }
                    else// within 5 days
                    {
                        service.DueDate = Convert.ToDateTime(AnnualLeaveStartDate).AddDays(-1);
                    }
                }
                service.DataAction = DataActionEnum.Create;
                var result = await _servicebusiness.ManageService(service);
            }
            if (viewModel.ServiceStatusCode == "SERVICE_STATUS_COMPLETE")
            {
                List<KeyValuePair<string, string>> errorList = new List<KeyValuePair<string, string>>();
                await PostAnnualLeaveTransactionToPayroll(viewModel, udf, uc, sp);
            }
            if (viewModel.ServiceStatusCode == "SERVICE_STATUS_CANCEL")
            {
                // delete the transaction if it is not processed
                await _payrollBusiness.DeleteIfAnyNotProcessedTrnasaction(viewModel);
            }
            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }
        private async ValueTask PostAnnualLeaveTransactionToPayroll(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _serviceBussiness = sp.GetService<IServiceBusiness>();
            var _payrollTransactionBusiness = sp.GetService<IPayrollTransactionsBusiness>();
            //var _elementlBusiness = sp.GetService<IElementBusiness>();


            //ServiceTemplateViewModel AnnualLeaveService = await _serviceBussiness.GetServiceDetails(new ServiceTemplateViewModel
            //{
            //    Id = viewModel.ServiceId,
            //    DataAction = DataActionEnum.Read,
            //});

            string startDate = udf.LeaveStartDate;
            var userBusiness = sp.GetService<IUserBusiness>();




            var OwnerDetails = await userBusiness.GetPersonWithSponsor(viewModel.OwnerUserId);
            viewModel.PersonId = OwnerDetails.Id;

            var isEligibleExitReEntryVisa = udf.IsEligibleExitReEntryVisa; //viewModel.Controls.FirstOrDefault(x => x.FieldName == "IsEligibleExitReEntryVisa").Code;
            var exitReEntryPaymentType = udf.ExitReEntryPaymentType;//viewModel.Controls.FirstOrDefault(x => x.FieldName == "exitReEntryPaymentType").Code;
            var requestForTicket = udf.RequestForTicket;// viewModel.Controls.FirstOrDefault(x => x.FieldName == "requestForTicket").Code;
            var leaveLocation = udf.kingdom;// viewModel.Controls.FirstOrDefault(x => x.FieldName == "kingdom").Code;

            if (isEligibleExitReEntryVisa.EqualsIgnoreCase("Yes") && exitReEntryPaymentType.IsNotNullAndNotEmpty() && leaveLocation == KingdomEnum.OutsideKingdom.ToString())
            {
                if (exitReEntryPaymentType == ExitRentryVisaPaymentTypeEnum.DeductFromNextSalary.ToString())
                {
                    if (startDate.IsNotNullAndNotEmpty())
                    {
                        var effectiveDate = Convert.ToDateTime(startDate).AddMonths(1).FirstDateOfMonth();
                        if (Convert.ToDateTime(startDate).Day > 27)
                        {
                            effectiveDate = Convert.ToDateTime(startDate).FirstDateOfMonth();
                        }
                        var paymodel = await GeneratePayrollTransactionViewModel(200, effectiveDate, "EXIT_ENTRY_VISA_DED", viewModel, uc, sp);
                        await _payrollTransactionBusiness.ManagePayrollTransaction(paymodel);
                    }


                }
            }

            var advancedSalary = udf.RequestAdvanceSalary;
            if (advancedSalary.IsNotNull() && advancedSalary.Code.EqualsIgnoreCase("Yes"))
            {
                var advanceLeaveAmount = udf.AdvanceLeaveAmount;//task.Controls.FirstOrDefault(x => x.FieldName == "advanceLeaveAmount");
                if (advanceLeaveAmount != null && advanceLeaveAmount.Code.ToSafeDouble() > 0)
                {
                    if (startDate.IsNotNullAndNotEmpty())
                    {
                        var effectiveDate = Convert.ToDateTime(startDate).AddMonths(-1).FirstDateOfMonth();
                        if (Convert.ToDateTime(startDate).Day > 27)
                        {
                            effectiveDate = Convert.ToDateTime(startDate).FirstDateOfMonth();
                        }
                        var paymodel = await GeneratePayrollTransactionViewModel(200, effectiveDate, "ADVANCE_SALARY", viewModel, uc, sp);
                        await _payrollTransactionBusiness.ManagePayrollTransaction(paymodel);

                        var advanceSalaryDed = await GeneratePayrollTransactionViewModel(advanceLeaveAmount.Code.ToSafeDouble(), effectiveDate.AddMonths(1).FirstDateOfMonth(), "ADVANCE_SALARY_REV", viewModel, uc, sp);
                        await _payrollTransactionBusiness.ManagePayrollTransaction(advanceSalaryDed);
                    }
                }

            }
            if (requestForTicket.EqualsIgnoreCase("Yes") && leaveLocation == KingdomEnum.OutsideKingdom.ToString())
            {
                var ticketPaymentOption = udf.TicketPaymentOption; //AnnualLeaveService.Controls.FirstOrDefault(x => x.FieldName == "ticketPaymentOption");
                if (ticketPaymentOption.IsNotNull() && ticketPaymentOption.Code.EqualsIgnoreCase("TICKET_AMOUNT_IN_PAYROLL"))
                {
                    var averageTicketCost = udf.AverageTicketCost; //task.Controls.FirstOrDefault(x => x.FieldName == "averageTicketCost");
                    if (averageTicketCost != null && averageTicketCost.Code.ToSafeDouble() > 0)
                    {
                        double ticketCost = averageTicketCost.Code.ToSafeDouble();
                        var dependent1 = udf.Dependent1;//AnnualLeaveService.Controls.FirstOrDefault(x => x.FieldName == "dependent1");
                        if (dependent1.IsNotNull() && dependent1.Code.IsNotNullAndNotEmpty())
                        {
                            ticketCost = ticketCost + averageTicketCost.Code.ToSafeDouble();
                        }
                        var dependent2 = udf.Dependent2; //AnnualLeaveService.Controls.FirstOrDefault(x => x.FieldName == "dependent2");
                        if (dependent2.IsNotNull() && dependent2.Code.IsNotNullAndNotEmpty())
                        {
                            ticketCost = ticketCost + averageTicketCost.Code.ToSafeDouble();
                        }
                        var dependent3 = udf.Dependent3;//AnnualLeaveService.Controls.FirstOrDefault(x => x.FieldName == "dependent3");
                        if (dependent3.IsNotNull() && dependent3.Code.IsNotNullAndNotEmpty())
                        {
                            ticketCost = ticketCost + averageTicketCost.Code.ToSafeDouble();
                        }
                        if (startDate.IsNotNullAndNotEmpty())
                        {
                            var effectiveDate = Convert.ToDateTime(startDate).AddMonths(-1).FirstDateOfMonth();
                            if (Convert.ToDateTime(startDate).Day > 27)
                            {
                                effectiveDate = Convert.ToDateTime(startDate).FirstDateOfMonth();
                            }
                            var tickeAmount = await GeneratePayrollTransactionViewModel(averageTicketCost.Code.ToSafeDouble(), effectiveDate, "ANNUAL_TICKET", viewModel, uc, sp);
                            _payrollTransactionBusiness.ManagePayrollTransaction(tickeAmount);
                        }

                    }
                }
            }
        }
        public async Task<CommandResult<ServiceTemplateViewModel>> UpdatePlannedAuthorizedLeaveWithoutPay(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            await UpdateLeaveRequest("PLANNED_UNPAID_L", viewModel, udf, uc, sp);
            var _lovBusiness = sp.GetService<ILOVBusiness>();
            var lov = await _lovBusiness.GetList(x => x.LOVType == "LOV_SERVICE_ACTION");
            if (viewModel.ServiceStatusCode == "SERVICE_STATUS_COMPLETE")
            {
                List<KeyValuePair<string, string>> errorList = new List<KeyValuePair<string, string>>();
                await AddAttendanceForKSA(viewModel, udf, errorList, "PLANNED_UNPAID_L", uc, sp);
            }
            if (viewModel.ServiceStatusCode == "SERVICE_STATUS_CANCEL")
            {
                List<KeyValuePair<string, string>> errorList = new List<KeyValuePair<string, string>>();
                await CancelLeaveAttendanceForKSA(viewModel, udf, errorList, "PLANNED_UNPAID_L", uc, sp);
            }
            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }
        #endregion
        #region Resignation Termination End Of Service Code
        /// <summary>
        /// Resignation/Termination End Of service Code
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="udf"></param>
        /// <param name="uc"></param>
        /// <param name="sp"></param>
        /// <returns></returns>

        public async Task<CommandResult<ServiceTemplateViewModel>> TriggerEndOfService(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _CoreBusiness = sp.GetService<IHRCoreBusiness>();
            await _CoreBusiness.TriggerEndOfService(viewModel);

            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }

        #endregion


        #region Clearance Form For Resignation Termination
        /// <summary>
        /// Resignation/Termination Clearance Form
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="udf"></param>
        /// <param name="uc"></param>
        /// <param name="sp"></param>
        /// <returns></returns>

        public async Task<CommandResult<ServiceTemplateViewModel>> TriggerClearanceForm(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _CoreBusiness = sp.GetService<IHRCoreBusiness>();
            await _CoreBusiness.TriggerClearanceForm(viewModel);

            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }
        #endregion

        public async Task<CommandResult<ServiceTemplateViewModel>> UpdateAccessLogDetail(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _CoreBusiness = sp.GetService<IHRCoreBusiness>();
            DateTime signintime = Convert.ToDateTime(udf.SignInTime);
            DateTime signouttime = Convert.ToDateTime(udf.SignOutTime);
            string location = Convert.ToString(udf.locationName);

            await _CoreBusiness.UpdateAccessLogDetail(viewModel.OwnerUserId, signintime, PunchingTypeEnum.Checkin, DeviceTypeEnum.RemoteLogin, location);
            await _CoreBusiness.UpdateAccessLogDetail(viewModel.OwnerUserId, signouttime, PunchingTypeEnum.Checkout, DeviceTypeEnum.RemoteLogin, location);

            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }
        #region New Person Request
        /// <summary>
        /// New Person Request
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="udf"></param>
        /// <param name="uc"></param>
        /// <param name="sp"></param>
        /// <returns></returns>

        public async Task<CommandResult<ServiceTemplateViewModel>> TriggerNewPersonRequestService(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {          
            
            var errorList = new Dictionary<string, string>();
            var _userBusiness = sp.GetService<IUserBusiness>();
            var _noteBusiness = sp.GetService<INoteBusiness>();
            var _lovBusiness = sp.GetService<ILOVBusiness>();
            var assignlov = await _lovBusiness.GetSingle(x => x.Code == "ASSIGNMENT_STATUS_ACTIVE");
            var assignstatusid = assignlov.Id;
            var email = udf.EmailId;
            var userData = await _userBusiness.ValidateUser(email);
            if (userData != null)
            {
                errorList.Add("Validate", "Person already exist with given email.");
                return CommandResult<ServiceTemplateViewModel>.Instance(viewModel, false, errorList);
            }
            else
            {
                // Create User
                var userModel = new UserViewModel();
                userModel.DataAction = DataActionEnum.Create;
                userModel.Email = email;
                userModel.Name = udf.FirstName + " " + udf.LastName;
                userModel.LineManagerId = udf.LineManagerId;

                var userResult = await _userBusiness.Create(userModel);
                if (userResult.IsSuccess)
                {
                    // Create Person
                    var userid = userResult.Item.Id;
                    var noteTempModel = new NoteTemplateViewModel();
                    noteTempModel.ActiveUserId = uc.UserId;
                    noteTempModel.TemplateCode = "HRPerson";
                    noteTempModel.OwnerUserId = uc.UserId;
                    var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                    notemodel.DataAction = DataActionEnum.Create;
                    //dynamic exo = new System.Dynamic.ExpandoObject();
                    //((IDictionary<String, Object>)exo).Add("EmployeeId", viewModel.UdfNoteTableId);
                    var perModel = new PersonViewModel {
                        UserId = userid,
                        TitleId = udf.TitleId,
                        FirstName = udf.FirstName,
                        LastName = udf.LastName,
                        ContactPersonalEmail = email,
                        GenderId = udf.GenderId,
                        DateOfBirth = Convert.ToDateTime(udf.DateOfBirth),
                        MaritalStatusId = udf.MaritalStatusId,
                        NationalityId = udf.NationalityId,
                        ReligionId = udf.ReligionId,
                        DateOfJoin = Convert.ToDateTime(udf.DateOfJoin),
                        BusinessHierarchyParentId = udf.BusinessHierarchyParentId,
                       
                    };
                    notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(perModel);
                    notemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                    var perResult = await _noteBusiness.ManageNote(notemodel);
                    //var perid = perResult.Item.Id;
                    var perid = perResult.Item.UdfNoteTableId;
                    if (perResult.IsSuccess)
                    {
                        // Create Assignment
                        //var perid = perResult.Item.Id;
                        var assignnoteTempModel = new NoteTemplateViewModel();
                        assignnoteTempModel.ActiveUserId = uc.UserId;
                        assignnoteTempModel.TemplateCode = "HRAssignment";
                        assignnoteTempModel.OwnerUserId = uc.UserId;
                        var assignnotemodel = await _noteBusiness.GetNoteDetails(assignnoteTempModel);
                        assignnotemodel.DataAction = DataActionEnum.Create;
                        //dynamic exo = new System.Dynamic.ExpandoObject();
                        //((IDictionary<String, Object>)exo).Add("EmployeeId", viewModel.UdfNoteTableId);
                                               

                        var assignModel = new AssignmentViewModel
                        {
                            UserId = userid,
                            EmployeeId = perid,
                            DepartmentId = udf.DepartmentId,
                            JobId = udf.JobId,
                            PositionId = udf.PositionId,
                            LocationId = udf.LocationId,
                            AssignmentGradeId = udf.AssignmentGradeId,
                            AssignmentTypeId = udf.AssignmentTypeId,                            
                            CareerLevelId = udf.CareerLevelId,
                            DateOfJoin = udf.DateOfJoin,
                            AssignmentStatusId= assignstatusid
                        };
                        assignnotemodel.Json = JsonConvert.SerializeObject(assignModel);
                        assignnotemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                        var assignResult = await _noteBusiness.ManageNote(assignnotemodel);
                        //var assignid = assignResult.Item.UdfNoteTableId;
                        if (assignResult.IsSuccess)
                        {

                        }
                        else
                        {
                            errorList.Add("Error", assignResult.Message);
                            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel, false, errorList);
                        }
                    }
                    else
                    {
                        errorList.Add("Error", perResult.Message);
                        return CommandResult<ServiceTemplateViewModel>.Instance(viewModel, false, errorList);
                    }

                }
                else
                {
                    errorList.Add("Error",userResult.Message);
                    return CommandResult<ServiceTemplateViewModel>.Instance(viewModel, false, errorList);
                }
            }

            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }
        #endregion
        public async Task<CommandResult<ServiceTemplateViewModel>> CreateDepartmentOnDepartmentRequest(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _hierarchyMasterbusiness = sp.GetService<IHierarchyMasterBusiness>();
            var _noteBusiness = sp.GetService<INoteBusiness>();           
            var model = await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel()
            {
                NoteId = viewModel.UdfNoteId,
                SetUdfValue = true
            });
            var rowData1 = model.ColumnList.ToDictionary(x => x.Name, x => x.Value);
            var BusinessHierarchyParentId = rowData1.ContainsKey("BusinessHierarchyParentId") ? Convert.ToString(rowData1["BusinessHierarchyParentId"]) : "";
            if (BusinessHierarchyParentId.IsNotNullAndNotEmpty())
            {
                NoteTemplateViewModel model1 = new NoteTemplateViewModel();
                model1.DataAction = DataActionEnum.Create;
                model1.TemplateCode = "HRDepartment";
                model1.ActiveUserId = uc.UserId;
                var notemodel = await _noteBusiness.GetNoteDetails(model1);
                notemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                notemodel.Json =  JsonConvert.SerializeObject(rowData1);
                await _noteBusiness.ManageNote(notemodel);
            }
            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }
        public async Task<CommandResult<ServiceTemplateViewModel>> CreateJOBOnJOBRequest(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _hierarchyMasterbusiness = sp.GetService<IHierarchyMasterBusiness>();
            var _noteBusiness = sp.GetService<INoteBusiness>();
            var model = await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel()
            {
                NoteId = viewModel.UdfNoteId,
                SetUdfValue = true
            });
            var rowData1 = model.ColumnList.ToDictionary(x => x.Name, x => x.Value);
            var BusinessHierarchyParentId = rowData1.ContainsKey("BusinessHierarchyParentId") ? Convert.ToString(rowData1["BusinessHierarchyParentId"]) : "";
            if (BusinessHierarchyParentId.IsNotNullAndNotEmpty())
            {
                NoteTemplateViewModel model1 = new NoteTemplateViewModel();
                model1.DataAction = DataActionEnum.Create;
                model1.TemplateCode = "HRJob";
                model1.ActiveUserId = uc.UserId;
                var notemodel = await _noteBusiness.GetNoteDetails(model1);
                notemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                notemodel.Json = JsonConvert.SerializeObject(rowData1);
                await _noteBusiness.ManageNote(notemodel);
            }
            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }
        public async Task<CommandResult<ServiceTemplateViewModel>> CreatePositionOnNewPositionRequest(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _hierarchyMasterbusiness = sp.GetService<IHierarchyMasterBusiness>();
            var _noteBusiness = sp.GetService<INoteBusiness>();
            var model = await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel()
            {
                NoteId = viewModel.UdfNoteId,
                SetUdfValue = true
            });
            var rowData1 = model.ColumnList.ToDictionary(x => x.Name, x => x.Value);
            var BusinessHierarchyParentId = rowData1.ContainsKey("BusinessHierarchyParentId") ? Convert.ToString(rowData1["BusinessHierarchyParentId"]) : "";
            if (BusinessHierarchyParentId.IsNotNullAndNotEmpty())
            {
                NoteTemplateViewModel model1 = new NoteTemplateViewModel();
                model1.DataAction = DataActionEnum.Create;
                model1.TemplateCode = "HRPosition";
                model1.ActiveUserId = uc.UserId;
                var notemodel = await _noteBusiness.GetNoteDetails(model1);
                notemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                notemodel.Json = JsonConvert.SerializeObject(rowData1);
                await _noteBusiness.ManageNote(notemodel);
            }
            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }
        public async Task<CommandResult<ServiceTemplateViewModel>> CreateCareerLevelOnCareerLevelRequest(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _hierarchyMasterbusiness = sp.GetService<IHierarchyMasterBusiness>();
            var _noteBusiness = sp.GetService<INoteBusiness>();
            var model = await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel()
            {
                NoteId = viewModel.UdfNoteId,
                SetUdfValue = true
            });
            var rowData1 = model.ColumnList.ToDictionary(x => x.Name, x => x.Value);
            var BusinessHierarchyParentId = rowData1.ContainsKey("BusinessHierarchyParentId") ? Convert.ToString(rowData1["BusinessHierarchyParentId"]) : "";
            if (BusinessHierarchyParentId.IsNotNullAndNotEmpty())
            {
                NoteTemplateViewModel model1 = new NoteTemplateViewModel();
                model1.DataAction = DataActionEnum.Create;
                model1.TemplateCode = "Career Level";
                model1.ActiveUserId = uc.UserId;
                var notemodel = await _noteBusiness.GetNoteDetails(model1);
                notemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                notemodel.Json = JsonConvert.SerializeObject(rowData1);
                await _noteBusiness.ManageNote(notemodel);
            }
            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }


        public async Task<CommandResult<ServiceTemplateViewModel>> UpdatePersonDepartment(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            // Get Person Assignment
            var _hrBusiness = sp.GetService<IHRCoreBusiness>();
            var _noteBusiness = sp.GetService<INoteBusiness>();
            string personId = udf.PersonId;
            var assignments=await _hrBusiness.GetAssignmentDetails(personId, null);
            var personDetails = assignments.FirstOrDefault();
            // Create New position with the help of New Department and Existing Job Id
            //NoteTemplateViewModel model1 = new NoteTemplateViewModel();
            //model1.DataAction = DataActionEnum.Create;
            //model1.TemplateCode = "HRPosition";
            //model1.ActiveUserId = uc.UserId;
            //var notemodel = await _noteBusiness.GetNoteDetails(model1);
            //notemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
            //dynamic exo = new System.Dynamic.ExpandoObject();
            //((IDictionary<String, Object>)exo).Add("DepartmentId", udf.NewDepartmentId);
            //if (personDetails != null)
            //{              
            //    ((IDictionary<String, Object>)exo).Add("JobId", personDetails.JobId);
            //}                
            //notemodel.Json = JsonConvert.SerializeObject(exo);
            //var position=await _noteBusiness.ManageNote(notemodel);
            var position = await _hrBusiness.CreatePosition(udf.NewDepartmentId, personDetails.JobId);
            // Update the Assignment with New DepartmentId and NewpositionId
            var model = await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel()
            {
                NoteId = personDetails.NoteAssignmentId,
                SetUdfValue = true
            });
            var rowData1 = model.ColumnList.ToDictionary(x => x.Name, x => x.Value);
            rowData1["DepartmentId"] = udf.NewDepartmentId;
            rowData1["PositionId"] = position.Id;
            var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData1);
            var update = await _noteBusiness.EditNoteUdfTable(model, data1, model.UdfNoteTableId);
            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }
        public async Task<CommandResult<ServiceTemplateViewModel>> UpdatePersonJob(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            // Get Person Assignment
            var _hrBusiness = sp.GetService<IHRCoreBusiness>();
            var _noteBusiness = sp.GetService<INoteBusiness>();
            string personId = udf.PersonId;
            var assignments = await _hrBusiness.GetAssignmentDetails(personId, null);
            var personDetails = assignments.FirstOrDefault();
            // Create New position with the help of New Department and Existing Job Id
            // NoteTemplateViewModel model1 = new NoteTemplateViewModel();
            // model1.DataAction = DataActionEnum.Create;
            // model1.TemplateCode = "HRPosition";
            // model1.ActiveUserId = uc.UserId;
            // var notemodel = await _noteBusiness.GetNoteDetails(model1);
            // notemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
            // dynamic exo = new System.Dynamic.ExpandoObject();
            // ((IDictionary<String, Object>)exo).Add("JobId", udf.NewJobId);
            // if (personDetails != null)
            // {
            //     ((IDictionary<String, Object>)exo).Add("DepartmentId", personDetails.DepartmentId);

            // }    
            // notemodel.Json = JsonConvert.SerializeObject(exo);
            //var position= await _noteBusiness.ManageNote(notemodel);
            var position = await _hrBusiness.CreatePosition(personDetails.DepartmentId, udf.NewJobId);
            // Update the Assignment with New DepartmentId and NewpositionId
            var model = await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel()
            {
                NoteId = personDetails.NoteAssignmentId,
                SetUdfValue = true
            });
            var rowData1 = model.ColumnList.ToDictionary(x => x.Name, x => x.Value);
            rowData1["JobId"] = udf.NewJobId;
           rowData1["PositionId"] = position.Id;
            var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData1);
            var update = await _noteBusiness.EditNoteUdfTable(model, data1, model.UdfNoteTableId);
            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }

        public async Task<CommandResult<ServiceTemplateViewModel>> UpdateDepartmentNameRequest(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _hrCoreBusiness = sp.GetService<IHRCoreBusiness>();
            if (viewModel.ServiceStatusCode == "SERVICE_STATUS_COMPLETE")
            {
                await _hrCoreBusiness.UpdateDepartmentName(udf);
            }

            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }
        public async Task<CommandResult<ServiceTemplateViewModel>> UpdateJobNameOnRequest(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _hrCoreBusiness = sp.GetService<IHRCoreBusiness>();
            if (viewModel.ServiceStatusCode == "SERVICE_STATUS_COMPLETE")
            {
                await _hrCoreBusiness.UpdateJobName(udf);
            }

            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }
        /// <summary>
        /// UpdateMisconductRequest
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="udf"></param>
        /// <param name="uc"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
        public async Task<CommandResult<ServiceTemplateViewModel>> UpdateMisconductRequest(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            //var  errorList = new List<KeyValuePair<string, string>>();
            //PostAmountToPayrollTransactionMisconductRequestUAE(viewModel, "", errorList);

            //New method
                var userBusiness = sp.GetService<IUserBusiness>();
                var _lovBusiness = sp.GetService<ILOVBusiness>();
                var _payrollElementBusiness = sp.GetService<IPayrollElementBusiness>();
                var _payrollTransactionsBusiness = sp.GetService<IPayrollTransactionsBusiness>();

            var elementCode = "";
            if (viewModel.ServiceStatusCode == "SERVICE_STATUS_COMPLETE")
            {
                var misconductDate = Convert.ToDateTime(udf.MisconductDate);
                var OwnerDetails = await userBusiness.GetPersonWithSponsor(udf.UserId);
                if (OwnerDetails.IsNotNull())
                {
                    viewModel.PersonId = OwnerDetails.Id;
                }
                var days = udf.NoOfDays;
                int noOfDays = 0;
                if (days != null)
                {
                    noOfDays = int.Parse(days);
                }
                double amount = 0;
                var fineAmountType = await _lovBusiness.GetSingleById(udf.FineAmountTypeId);

                if (fineAmountType.Code == "Days_From_Basic_Salary")
                {
                    elementCode = "MISCONDUCT_BASIC_DED";
                    amount = (await _payrollElementBusiness.GetBasicSalary(viewModel.OwnerUserId) / 22) * noOfDays;
                }
                else if (fineAmountType.Code == "Days_From_Total_Salary")
                {
                    elementCode = "MISCONDUCT_TOTAL_DED";
                    amount = (await _payrollElementBusiness.GetUserSalary(viewModel.OwnerUserId) / 22) * noOfDays;
                }
                else if (fineAmountType.Code == "Amount")
                {
                    elementCode = "MISCONDUCT_FINE_DED";
                    amount = double.Parse(udf.Amount);
                }
                var paymodel = this.GeneratePayrollTransactionViewModel(amount, misconductDate, elementCode, viewModel,uc,sp);
                paymodel.DataAction = DataActionEnum.Create;
                paymodel.CreatedBy = uc.UserId;
                paymodel.LastUpdatedBy = uc.UserId;
                await _payrollTransactionsBusiness.ManagePayrollTransaction(paymodel);
                //await payTransBusiness.Create(paymodel);

            }

            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }
        private void PostAmountToPayrollTransactionMisconductRequestUAE(ServiceViewModel viewModel, string elementCode, List<KeyValuePair<string, string>> errorList)
        {
            //try
            //{
            //    var payTransBusiness = BusinessHelper.GetInstance<IPayrollTransactionBusiness>();
            //    var salaryElementInfoTransBusiness = BusinessHelper.GetInstance<ISalaryElementInfoBusiness>();
            //    var userBusiness = BusinessHelper.GetInstance<IUserBusiness>();
            //    var calendarBusiness = BusinessHelper.GetInstance<ICalendarBusiness>();

            //    //var leaveStartDate = DateTime.Today;
            //    var misconductDate = Convert.ToDateTime(viewModel.Controls.FirstOrDefault(x => x.FieldName == "misconductDate").Code);
            //    var OwnerDetails = userBusiness.GetPersonWithSponsor(viewModel.OwnerUserId);
            //    viewModel.OwnerEmployeeNo = OwnerDetails.Id.ToSafeString();
            //    var days = viewModel.Controls.FirstOrDefault(x => x.FieldName == "noOfDays").Code;
            //    int noOfDays = 0;
            //    if (days != null)
            //    {
            //        noOfDays = int.Parse(days);
            //    }
            //    double amount = 0;
            //    var fineAmountType = viewModel.Controls.FirstOrDefault(x => x.FieldName == "fineAmountType").Code;
            //    if (fineAmountType == "FINE_BASIC")
            //    {
            //        elementCode = "MISCONDUCT_BASIC_DED";
            //        amount = (salaryElementInfoTransBusiness.GetBasicSalary(viewModel.OwnerUserId) / 22) * noOfDays;
            //        //if(amount<=0)
            //        //{
            //        //    errorList.Add("MisconductErrorBasicSalary", "Problem with Basic Salary while processing your misconduct request, Please contact administrator");
            //        //    return;
            //        //}
            //    }
            //    else if (fineAmountType == "FINE_TOTAL")
            //    {
            //        elementCode = "MISCONDUCT_TOTAL_DED";
            //        amount = (salaryElementInfoTransBusiness.GetUserSalary(viewModel.OwnerUserId) / 22) * noOfDays;

            //    }
            //    else if (fineAmountType == "FINE_AMO")
            //    {
            //        elementCode = "MISCONDUCT_FINE_DED";
            //        amount = double.Parse(viewModel.Controls.FirstOrDefault(x => x.FieldName == "amount").Code);
            //    }
            //    var paymodel = GeneratePayrollTransactionViewModel(amount, misconductDate, elementCode, viewModel);
            //    payTransBusiness.Create(paymodel);
            //}
            //catch (Exception e)
            //{
            //    errorList.Add("Problem while processing your misconduct request, Please contact administrator", e.ToString());
            //}
        }
        /// <summary>
        /// PostBusinessTripClaimToPayroll
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="udf"></param>
        /// <param name="uc"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
        public async Task<CommandResult<ServiceTemplateViewModel>> PostBusinessTripClaimToPayroll(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _payrollTransactionBusiness = sp.GetService<IPayrollTransactionsBusiness>();

            //var _payrollTransactionBusiness = BusinessHelper.GetInstance<IPayrollTransactionBusiness>();

            //var _perDiemCost = viewModel.Controls.FirstOrDefault(x => x.FieldName == "PerDiemCost");
            //var _ticketCost = viewModel.Controls.FirstOrDefault(x => x.FieldName == "TicketCost");
            //var _hotelReservationCost = viewModel.Controls.FirstOrDefault(x => x.FieldName == "HotelReservationCost");
            //var _otherExpenses = viewModel.Controls.FirstOrDefault(x => x.FieldName == "OtherExpenses");

            double perDiemCost = udf.PerDiemCost != null ? Convert.ToDouble(udf.PerDiemCost) : 0.00;
            double ticketCost = udf.TicketCost != null ? Convert.ToDouble(udf.TicketCost) : 0.00;
            double hotelReservationCost = udf.HotelReservationCost != null ? Convert.ToDouble(udf.HotelReservationCost) : 0.00;
            double otherExpenses = udf.OtherExpenses != null ? Convert.ToDouble(udf.OtherExpenses) : 0.00;

            //if (_perDiemCost != null && _perDiemCost.Code != null)
            //{
            //    perDiemCost = _perDiemCost.Code.ToSafeDouble();
            //}
            //if (_ticketCost != null && _ticketCost.Code != null)
            //{
            //    ticketCost = _ticketCost.Code.ToSafeDouble();
            //}
            //if (_hotelReservationCost != null && _hotelReservationCost.Code != null)
            //{
            //    hotelReservationCost = _hotelReservationCost.Code.ToSafeDouble();
            //}
            //if (_otherExpenses != null && _otherExpenses.Code != null)
            //{
            //    otherExpenses = _otherExpenses.Code.ToSafeDouble();
            //}

            if (viewModel.ServiceStatusCode == "SERVICE_STATUS_COMPLETE")
            {
                if (viewModel.OwnerUserId != null)
                {
                    var effectiveDate = DateTime.Today;// viewModel.Co.AddMonths(1).FirstDateOfMonth();
                    if (effectiveDate.Day > 27)
                    {
                        effectiveDate = effectiveDate.AddMonths(1).FirstDateOfMonth();
                    }
                    //PerDiem Posting to Payroll
                    if (perDiemCost > 0)
                    {
                        //var paymodel = GeneratePayrollTransactionViewModel(perDiemCost, effectiveDate, "BT_PER_DIEM", viewModel);
                        //_payrollTransactionBusiness.Create(paymodel);

                        var paymodel = await GeneratePayrollTransactionViewModel(perDiemCost, effectiveDate, "BT_PER_DIEM", viewModel, uc, sp);
                        await _payrollTransactionBusiness.ManagePayrollTransaction(paymodel);
                    }
                    //ticketCost Posting to Payroll
                    if (ticketCost > 0)
                    {
                        //var paymodel = GeneratePayrollTransactionViewModel(ticketCost, effectiveDate, "BT_TICKET_EXP", viewModel);
                        //_payrollTransactionBusiness.Create(paymodel);

                        var paymodel = await GeneratePayrollTransactionViewModel(ticketCost, effectiveDate, "BT_TICKET_EXP", viewModel, uc, sp);
                        await _payrollTransactionBusiness.ManagePayrollTransaction(paymodel);
                    }
                    //hotelReservationCost Posting to Payroll
                    if (hotelReservationCost > 0)
                    {
                        //var paymodel = GeneratePayrollTransactionViewModel(hotelReservationCost, effectiveDate, "BT_HOTEL_EXP", viewModel);
                        //_payrollTransactionBusiness.Create(paymodel);

                        var paymodel = await GeneratePayrollTransactionViewModel(hotelReservationCost, effectiveDate, "BT_HOTEL_EXP", viewModel, uc, sp);
                        await _payrollTransactionBusiness.ManagePayrollTransaction(paymodel);
                    }
                    //otherExpenses Posting to Payroll
                    if (otherExpenses > 0)
                    {
                        //var paymodel = GeneratePayrollTransactionViewModel(otherExpenses, effectiveDate, "BT_OTHER_EXP", viewModel);
                        //_payrollTransactionBusiness.Create(paymodel);

                        var paymodel = await GeneratePayrollTransactionViewModel(otherExpenses, effectiveDate, "BT_OTHER_EXP", viewModel, uc, sp);
                        await _payrollTransactionBusiness.ManagePayrollTransaction(paymodel);
                    }
                }

            }
            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }

        public async Task<CommandResult<ServiceTemplateViewModel>> UpdateLineManager(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {            
            var _hrCoreBusiness = sp.GetService<IHRCoreBusiness>();
            if (viewModel.ServiceStatusCode == "SERVICE_STATUS_COMPLETE")
            {
                await _hrCoreBusiness.UpdateLineManager(udf);
            }

            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }

        public async Task<CommandResult<ServiceTemplateViewModel>> UpdatePersonDetails(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _hrCoreBusiness = sp.GetService<IHRCoreBusiness>();
            if (viewModel.ServiceStatusCode == "SERVICE_STATUS_COMPLETE")
            {
                await _hrCoreBusiness.UpdatePersonDetails(udf);
            }

            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }
        public async Task<CommandResult<ServiceTemplateViewModel>> UpdateAssignment(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _hrCoreBusiness = sp.GetService<IHRCoreBusiness>();
            if (viewModel.ServiceStatusCode == "SERVICE_STATUS_COMPLETE")
            {
                await _hrCoreBusiness.UpdateAssignmentDetails(udf);
            }

            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }

        public async Task<CommandResult<ServiceTemplateViewModel>> BulkRequest(ServiceTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _hrCoreBusiness = sp.GetService<IHRCoreBusiness>();
            var _cmsBusiness = sp.GetService<ICmsBusiness>();
            var _noteBusiness = sp.GetService<INoteBusiness>();
            if (viewModel.ServiceStatusCode == "SERVICE_STATUS_COMPLETE")
            {
                var where = $@" and ""N_CoreHR_NewDepartmentCreate"".""BulkRequestId"" = '{viewModel.ServiceId}'";
                var departments = await _cmsBusiness.GetDataListByTemplate("NEW_DEPARTMENT_CREATE","", where);
                if (departments.IsNotNull()) 
                {
                    // CreateDepartment
                    foreach (DataRow data in departments.Rows)
                    {
                        var noteModel = await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel()
                        {
                            NoteId =Convert.ToString(data["NtsNoteId"]),
                            DataAction=DataActionEnum.Create,
                            SetUdfValue=true
                        }) ;
                        await _hrCoreBusiness.CreateDepartment(noteModel);
                    }
                   
                }
                var where1 = $@" and ""N_CoreHR_NewJobCreate"".""BulkRequestId"" = '{viewModel.ServiceId}'";
                var jobs = await _cmsBusiness.GetDataListByTemplate("NEW_JOB_CREATE", "", where1);
                if (jobs.IsNotNull())
                {
                    //CreateNewJob
                    foreach (DataRow data in jobs.Rows)
                    {
                        var noteModel = await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel()
                        {
                            NoteId = Convert.ToString(data["NtsNoteId"]),
                            DataAction = DataActionEnum.Create,
                            SetUdfValue = true
                        });
                        await _hrCoreBusiness.CreateNewJob(noteModel);
                    }

                }
                var where2 = $@" and ""N_CoreHR_NewCareerLevelCreate"".""BulkRequestId"" = '{viewModel.ServiceId}'";
                var careerLevels = await _cmsBusiness.GetDataListByTemplate("NEW_CAREERLEVEL_CREATE", "", where2);
                if (careerLevels.IsNotNull())
                {
                    //CreateNewCareerLevel
                    foreach (DataRow data in careerLevels.Rows)
                    {
                        var noteModel = await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel()
                        {
                            NoteId = Convert.ToString(data["NtsNoteId"]),
                            DataAction = DataActionEnum.Create,
                            SetUdfValue = true
                        });
                        await _hrCoreBusiness.CreateNewCareerLevel(noteModel);
                    }

                }
                var where3 = $@" and ""N_CoreHR_NewPositionCreate"".""BulkRequestId"" = '{viewModel.ServiceId}'";
                var positions = await _cmsBusiness.GetDataListByTemplate("NEW_POSITION_CREATE", "", where3);
                if (positions.IsNotNull())
                {
                    //CreateNewPosition
                    foreach (DataRow data in positions.Rows)
                    {
                        var noteModel = await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel()
                        {
                            NoteId = Convert.ToString(data["NtsNoteId"]),
                            DataAction = DataActionEnum.Create,
                            SetUdfValue = true
                        });
                        await _hrCoreBusiness.CreateNewPosition(noteModel);
                    }

                }
                var where4 = $@" and ""N_CoreHR_NewPersonCreate"".""BulkRequestId"" = '{viewModel.ServiceId}'";
                var persons = await _cmsBusiness.GetDataListByTemplate("NEW_PERSON_CREATE", "", where4);
                if (persons.IsNotNull())
                {
                    //CreateNewPerson
                    foreach (DataRow data in persons.Rows)
                    {
                        var noteModel = await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel()
                        {
                            NoteId = Convert.ToString(data["NtsNoteId"]),
                            DataAction = DataActionEnum.Create,
                            SetUdfValue = true
                        });                      
                        await _hrCoreBusiness.CreateNewPerson(noteModel);
                    }

                }
                var where9 = $@" and ""N_CoreHR_NewEmployeeCreate"".""BulkRequestId"" = '{viewModel.ServiceId}'";
                var employees = await _cmsBusiness.GetDataListByTemplate("EMPLOYEE_CREATE", "", where9);
                if (employees.IsNotNull())
                {
                    //CreateNewPerson
                    foreach (DataRow data in employees.Rows)
                    {
                        var noteModel = await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel()
                        {
                            NoteId = Convert.ToString(data["NtsNoteId"]),
                            DataAction = DataActionEnum.Create,
                            SetUdfValue = true
                        });
                        await _hrCoreBusiness.CreateNewPerson(noteModel);
                    }

                }
                var where5 = $@" and ""N_CoreHR_RenameDepartment"".""BulkRequestId"" = '{viewModel.ServiceId}'";
                var dep = await _cmsBusiness.GetDataListByTemplate("RENAME_DEPARTMENT", "", where5);
                if (dep.IsNotNull())
                {
                    //CreateNewPerson
                    foreach (DataRow data in dep.Rows)
                    {
                        var noteModel = await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel()
                        {
                            NoteId = Convert.ToString(data["NtsNoteId"]),
                            DataAction = DataActionEnum.Create,
                            SetUdfValue = true
                        });
                        var rowData1 = noteModel.ColumnList.ToDictionary(x => x.Name, x => x.Value);
                        await _hrCoreBusiness.UpdateDepartmentName(rowData1);
                    }

                }
                var where6 = $@" and ""N_CoreHR_RenameJob"".""BulkRequestId"" = '{viewModel.ServiceId}'";
                var job = await _cmsBusiness.GetDataListByTemplate("RENAME_JOB", "", where6);
                if (job.IsNotNull())
                {
                    //CreateNewPerson
                    foreach (DataRow data in job.Rows)
                    {
                        var noteModel = await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel()
                        {
                            NoteId = Convert.ToString(data["NtsNoteId"]),
                            DataAction = DataActionEnum.Create,
                            SetUdfValue = true
                        });
                        var rowData1 = noteModel.ColumnList.ToDictionary(x => x.Name, x => x.Value);
                        await _hrCoreBusiness.UpdateJobName(rowData1);
                    }

                }
                var where7 = $@" and ""N_CoreHR_ChangeDepartment"".""BulkRequestId"" = '{viewModel.ServiceId}'";
                var cdep = await _cmsBusiness.GetDataListByTemplate("Change_Department", "", where7);
                if (cdep.IsNotNull())
                {
                    //CreateNewPerson
                    foreach (DataRow data in cdep.Rows)
                    {
                        var noteModel = await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel()
                        {
                            NoteId = Convert.ToString(data["NtsNoteId"]),
                            DataAction = DataActionEnum.Create,
                            SetUdfValue = true
                        });                        
                        await _hrCoreBusiness.UpdatePersonDepartment(noteModel);
                    }

                }
                var where8 = $@" and ""N_CoreHR_ChangeJob"".""BulkRequestId"" = '{viewModel.ServiceId}'";
                var cjob = await _cmsBusiness.GetDataListByTemplate("Change_Job", "", where8);
                if (cjob.IsNotNull())
                {
                    //CreateNewPerson
                    foreach (DataRow data in cjob.Rows)
                    {
                        var noteModel = await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel()
                        {
                            NoteId = Convert.ToString(data["NtsNoteId"]),
                            DataAction = DataActionEnum.Create,
                            SetUdfValue = true
                        });                       
                        await _hrCoreBusiness.UpdatePersonJob(noteModel);
                    }

                }
            }
            return CommandResult<ServiceTemplateViewModel>.Instance(viewModel);
        }
        }
}
