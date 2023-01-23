using Synergy.App.Business.Interface.BusinessScript.Note.General.CoreHR;
using Synergy.App.Common;
using Synergy.App.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business.Implementations.BusinessScript.Note.General.CoreHR
{
    public class NoteGeneralCoreHRPreScript : INoteGeneralCoreHRPreScript
    {
        /// <summary>
        /// This method for to Create Position Hierarchy
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="udf"></param>
        /// <param name="uc"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
        public async Task<CommandResult<NoteTemplateViewModel>> ValidatePersonUser(NoteTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _business = sp.GetService<IHRCoreBusiness>();
            var res=await _business.ValidateUserMappingToPerson(viewModel);
            if (res == false)
            {
                var errorList = new Dictionary<string, string>();
                errorList.Add("Validate", "This User is already mapped to a different Person,Please Select different User");
                return CommandResult<NoteTemplateViewModel>.Instance(viewModel,false,errorList);
            }
            return CommandResult<NoteTemplateViewModel>.Instance(viewModel);
        }

        /// <summary>
        /// This method for to validate holiday name
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="udf"></param>
        /// <param name="uc"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
        public async Task<CommandResult<NoteTemplateViewModel>> ValidateCalendarHoliday(NoteTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _business = sp.GetService<IPayrollBusiness>();
            var res = await _business.ValidateHolidayName(viewModel);
            if (res == false)
            {
                var errorList = new Dictionary<string, string>();
                errorList.Add("Validate", "Holiday Name already exist");
                return CommandResult<NoteTemplateViewModel>.Instance(viewModel, false, errorList);
            }
            return CommandResult<NoteTemplateViewModel>.Instance(viewModel);
        }

        /// <summary>
        /// This method for to validate calendar for current legalentity
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="udf"></param>
        /// <param name="uc"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
        public async Task<CommandResult<NoteTemplateViewModel>> ValidateCalendar(NoteTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _business = sp.GetService<IPayrollBusiness>();
            var res = await _business.ValidateCalendar(viewModel);
            if (res == false)
            {
                var errorList = new Dictionary<string, string>();
                errorList.Add("Validate", "Calendar already created");
                return CommandResult<NoteTemplateViewModel>.Instance(viewModel, false, errorList);
            }
            return CommandResult<NoteTemplateViewModel>.Instance(viewModel);
        }
        public async Task<CommandResult<NoteTemplateViewModel>> ValidateEmployeeBook(NoteTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _business = sp.GetService<IDMSDocumentBusiness>();
            var result = await _business.CheckEmployeeBook(viewModel);
            if (result == true)
            {
                var errorList = new Dictionary<string, string>();
                errorList.Add("Validate", "Book already created for the selected employee");
                return CommandResult<NoteTemplateViewModel>.Instance(viewModel, false, errorList);
            }
            return CommandResult<NoteTemplateViewModel>.Instance(viewModel);            
        }
        public async Task<CommandResult<NoteTemplateViewModel>> UpdatePositionUdf(NoteTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _business = sp.GetService<IHRCoreBusiness>();
            var _noteBusiness = sp.GetService<INoteBusiness>();
            var jobName = await _business.GetJobNameById(udf.JobId);
            var OrgName = await _business.GetOrgNameById(udf.DepartmentId);
            if (jobName!=null && OrgName != null)
            {
                var Name = await _business.GenerateNextPositionName(OrgName.Name + "_" + jobName.Name + "_");
                var number = Name.Split('_')[2] ;
                var json = viewModel.Json;
                var data = JsonConvert.DeserializeObject<dynamic>(json);
                data["PositionName"] = Name;
                data["PositionNo"] = number;
                viewModel.Json= JsonConvert.SerializeObject(data);
            }           
            return CommandResult<NoteTemplateViewModel>.Instance(viewModel);
        }

        /// <summary>
        /// This method for to validate Unique Department
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="udf"></param>
        /// <param name="uc"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
        public async Task<CommandResult<NoteTemplateViewModel>> ValidateUniqueDepartment(NoteTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _business = sp.GetService<IHRCoreBusiness>();
            var res = await _business.ValidateUniqueDepartment(viewModel);
            if (res)
            {
                var errorList = new Dictionary<string, string>();
                errorList.Add("Validate", "The given department name already exist. Please enter another department name.");
                return CommandResult<NoteTemplateViewModel>.Instance(viewModel, false, errorList);
            }
            return CommandResult<NoteTemplateViewModel>.Instance(viewModel);
        }
        public async Task<CommandResult<NoteTemplateViewModel>> CalculateAndGenerateChargesForCommunityHallBooking(NoteTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _business = sp.GetService<IHRCoreBusiness>();
            var _noteBusiness = sp.GetService<INoteBusiness>();

            return CommandResult<NoteTemplateViewModel>.Instance(viewModel);
        }
    }
}
