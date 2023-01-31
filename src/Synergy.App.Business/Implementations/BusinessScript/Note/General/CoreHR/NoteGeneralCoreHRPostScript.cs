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
    public class NoteGeneralCoreHRPostScript : INoteGeneralCoreHRPostScript
    {
        /// <summary>
        /// This method for to Create Position Hierarchy
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="udf"></param>
        /// <param name="uc"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
        public async Task<CommandResult<NoteTemplateViewModel>> CreatePositionHierarchy(NoteTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _business = sp.GetService<IHRCoreBusiness>();
            await _business.CreatePositionHierarchy(viewModel);
            return CommandResult<NoteTemplateViewModel>.Instance(viewModel);
        }

        public async Task<CommandResult<NoteTemplateViewModel>> CreateDepartmentHierarchy(NoteTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _business = sp.GetService<IHRCoreBusiness>();
            await _business.CreateDepartmentHierarchy(viewModel);
            return CommandResult<NoteTemplateViewModel>.Instance(viewModel);
        }
        public async Task<CommandResult<NoteTemplateViewModel>> UpdatePositionNameOnJobChange(NoteTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _business = sp.GetService<IHRCoreBusiness>();
            // Update All position Name
            var positionList = await _business.GetPositionByJobId(udf.NoteId);
            foreach (var data in positionList)
            {
                var department = await _business.GetDepartmentNameById(data.DepartmentId);
                var positionName = department.Name + "_" + udf.JobTitle;
                await _business.UpdatePositionName(positionName, data.Id);
            }
            return CommandResult<NoteTemplateViewModel>.Instance(viewModel);

        }
        public async Task<CommandResult<NoteTemplateViewModel>> UpdatePositionNameOnDepartmentChange(NoteTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _business = sp.GetService<IHRCoreBusiness>();

            // Update All position Name
            var positionList = await _business.GetPositionByDepartmentId(udf.NoteId);
            foreach (var data in positionList)
            {
                var job = await _business.GetJobNameById(data.JobId);
                var positionName = job.Name + "_" + udf.DepartmentName;
                await _business.UpdatePositionName(positionName, data.Id);
            }
            return CommandResult<NoteTemplateViewModel>.Instance(viewModel);

        }
        public async Task<CommandResult<NoteTemplateViewModel>> CreateEmployeeBook(NoteTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _tableMetadataBusiness = sp.GetService<ITableMetadataBusiness>();
            string parentId = "";
            var existing = await _tableMetadataBusiness.GetTableDataByColumn("GENERAL_FOLDER", null, "Code", "EMPLOYEE_BOOKS");

            if (existing != null)
            {
                parentId = Convert.ToString(existing["NtsNoteId"]);
            }
            if (viewModel.DataAction == DataActionEnum.Create)
            {
                var _noteBusiness = sp.GetService<INoteBusiness>();
                var noteTempModel = new NoteTemplateViewModel();
                noteTempModel.ActiveUserId = uc.UserId;
                noteTempModel.TemplateCode = "HR_EMPLOYEE_BOOK";
                noteTempModel.OwnerUserId = uc.UserId;
                noteTempModel.ParentNoteId = parentId;
                var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                notemodel.DataAction = DataActionEnum.Create;
                dynamic exo = new System.Dynamic.ExpandoObject();

                if (viewModel.UdfNoteTableId.IsNotNull())
                {
                    ((IDictionary<String, Object>)exo).Add("EmployeeId", viewModel.UdfNoteTableId);
                }
                notemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                var udftabledata = await _tableMetadataBusiness.GetTableDataByColumn("HRPerson", null, "Id", viewModel.UdfNoteTableId);
                if (udftabledata != null)
                {
                    notemodel.NoteSubject = Convert.ToString(udftabledata["PersonFullName"]);
                }

                notemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";

                var result = await _noteBusiness.ManageNote(notemodel);
            }

            return CommandResult<NoteTemplateViewModel>.Instance(viewModel);
        }
        public async Task<CommandResult<NoteTemplateViewModel>> EmployeeBookPostScript(NoteTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _tableMetadataBusiness = sp.GetService<ITableMetadataBusiness>();
            var EmployeeId = "";

            var existing = await _tableMetadataBusiness.GetTableDataByColumn("HR_EMPLOYEE_BOOK", null, "NtsNoteId", viewModel.NoteId);

            if (existing != null)
            {
                EmployeeId = Convert.ToString(existing["EmployeeId"]);
            }
            if (viewModel.NoteStatusCode == "NOTE_STATUS_INPROGRESS")
            {
                var _noteBusiness = sp.GetService<INoteBusiness>();
                var templateModel = new NoteTemplateViewModel();
                templateModel.ActiveUserId = uc.UserId;

                templateModel.TemplateCode = "EMP_PROFILE_BOOK";
                templateModel.ParentNoteId = viewModel.NoteId;
                var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
                newmodel.DataAction = DataActionEnum.Create;
                newmodel.NotePlusId = viewModel.NoteId;
                newmodel.NoteSubject = "Employee Profile";
                dynamic exo = new System.Dynamic.ExpandoObject();
                newmodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                if (EmployeeId.IsNotNullAndNotEmpty())
                {
                    ((IDictionary<String, Object>)exo).Add("EmployeeId", EmployeeId);
                }
                newmodel.SequenceOrder = 1;
                newmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                newmodel.SequenceOrder = 1;
                var result = await _noteBusiness.ManageNote(newmodel);

                var templateModel1 = new NoteTemplateViewModel();
                templateModel1.ActiveUserId = uc.UserId;

                templateModel1.TemplateCode = "EMP_DOCUMENT_BOOK";
                templateModel1.ParentNoteId = viewModel.NoteId;
                var newmodel1 = await _noteBusiness.GetNoteDetails(templateModel1);
                newmodel1.DataAction = DataActionEnum.Create;
                newmodel1.NotePlusId = viewModel.NoteId;
                newmodel1.NoteSubject = "Employee Document";
                newmodel1.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                newmodel1.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                newmodel1.SequenceOrder = 2;
                var result1 = await _noteBusiness.ManageNote(newmodel1);

                var templateModel2 = new NoteTemplateViewModel();
                templateModel2.ActiveUserId = uc.UserId;

                templateModel2.TemplateCode = "EMP_ATTENDANCE_BOOK";
                templateModel2.ParentNoteId = viewModel.NoteId;
                var newmodel2 = await _noteBusiness.GetNoteDetails(templateModel2);
                newmodel2.DataAction = DataActionEnum.Create;
                newmodel2.NotePlusId = viewModel.NoteId;
                newmodel2.NoteSubject = "Employee Attendance";
                newmodel2.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                newmodel2.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                newmodel2.SequenceOrder = 3;
                var result2 = await _noteBusiness.ManageNote(newmodel2);

                var templateModel3 = new NoteTemplateViewModel();
                templateModel3.ActiveUserId = uc.UserId;
                templateModel3.ParentNoteId = viewModel.NoteId;
                templateModel3.TemplateCode = "EMP_BENEFITS_BOOK";
                var newmodel3 = await _noteBusiness.GetNoteDetails(templateModel3);
                newmodel3.DataAction = DataActionEnum.Create;
                newmodel3.NotePlusId = viewModel.NoteId;
                newmodel3.NoteSubject = "Employee Benefits";
                newmodel3.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                newmodel3.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                newmodel3.SequenceOrder = 4;
                var result3 = await _noteBusiness.ManageNote(newmodel3);



                var templateModel4 = new NoteTemplateViewModel();
                templateModel4.ActiveUserId = uc.UserId;

                templateModel4.TemplateCode = "EMP_ASSIGNMENT_BOOK";

                templateModel4.ParentNoteId = viewModel.NoteId;
                var newmodel4 = await _noteBusiness.GetNoteDetails(templateModel4);
                newmodel4.DataAction = DataActionEnum.Create;
                newmodel4.NotePlusId = viewModel.NoteId;
                newmodel4.NoteSubject = "Employee Assignment";
                newmodel4.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                newmodel4.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                newmodel4.SequenceOrder = 5;
                var result4 = await _noteBusiness.ManageNote(newmodel4);

                var templateModel5 = new NoteTemplateViewModel();
                templateModel5.ActiveUserId = uc.UserId;

                templateModel5.ParentNoteId = viewModel.NoteId;
                templateModel5.TemplateCode = "EMP_PAYSLIP_BOOK";
                var newmodel5 = await _noteBusiness.GetNoteDetails(templateModel5);
                newmodel5.DataAction = DataActionEnum.Create;
                newmodel5.NotePlusId = viewModel.NoteId;
                newmodel5.NoteSubject = "Employee PaySlip";
                newmodel5.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                newmodel5.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                newmodel5.SequenceOrder = 6;
                var result5 = await _noteBusiness.ManageNote(newmodel5);
            }

            return CommandResult<NoteTemplateViewModel>.Instance(viewModel);
        }

        public async Task<CommandResult<NoteTemplateViewModel>> CreateHybridHierarchy(NoteTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _hierarchyMasterbusiness = sp.GetService<IHierarchyMasterBusiness>();
            var _noteBusiness = sp.GetService<INoteBusiness>();
            var _hybridHierarchyBusiness = sp.GetService<IHybridHierarchyBusiness>();
            var model = await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel()
            {
                NoteId = viewModel.NoteId,
                SetUdfValue = true
            });
            var rowData1 = model.ColumnList.ToDictionary(x => x.Name, x => x.Value);
            var BusinessHierarchyParentId = rowData1.ContainsKey("BusinessHierarchyParentId") ? Convert.ToString(rowData1["BusinessHierarchyParentId"]) : "";

            if (BusinessHierarchyParentId.IsNotNullAndNotEmpty())
            {
                var hybridHierarchy = await _hybridHierarchyBusiness.GetSingle(x => x.Id == BusinessHierarchyParentId);
                if (hybridHierarchy == null)
                {
                    // Create New Hybrid Hierarchy
                    var hierarchy = await _hierarchyMasterbusiness.GetSingle(x => x.Code == "BUSINESS_HIERARCHY");
                    hybridHierarchy = new HybridHierarchyViewModel();
                    // Hierarchy Master code="BUSINESS_HIERARCHY";
                    // LevelId=0;
                    //ReferenceId=null;ReferenceType=Root;Name= Hierarchy Master Name
                    if (hierarchy.IsNotNull())
                    {
                        hybridHierarchy.ParentId = BusinessHierarchyParentId;
                        hybridHierarchy.LevelId = 0;
                        hybridHierarchy.Name = hierarchy.Name;
                        hybridHierarchy.HierarchyMasterId = hierarchy.Id;
                        hybridHierarchy.ReferenceId = viewModel.UdfNoteTableId;
                        hybridHierarchy.ReferenceType = "ROOT";
                        // hybridHierarchy.DataAction = DataActionEnum.Create;
                        // await _hybridHierarchyBusiness.Create(hybridHierarchy);
                    }
                }
                else if (hybridHierarchy.ReferenceType == "POSITION")
                {
                    hybridHierarchy.ReferenceType = "EMPLOYEE";
                    hybridHierarchy.ReferenceId = viewModel.UdfNoteTableId;                    
                    await _hybridHierarchyBusiness.Edit(hybridHierarchy);
                }
                else
                {
                    var model1 = new HybridHierarchyViewModel();
                    model1.ParentId = BusinessHierarchyParentId;
                    model1.LevelId = hybridHierarchy.LevelId + 1;
                    model1.HierarchyMasterId = hybridHierarchy.HierarchyMasterId;
                    model1.ReferenceId = viewModel.UdfNoteTableId;
                    // model1.Name = hybridHierarchy.Name;
                    model1.ReferenceType = Convert.ToString(rowData1["BusinessHierarchyReferenceType"]);
                    //model1.ReferenceType = BusinessExtension.GetHybridHierarchyReferenceType(hybridHierarchy.ReferenceType, viewModel.TemplateCode);
                    model1.DataAction = DataActionEnum.Create;
                    await _hybridHierarchyBusiness.Create(model1);
                }                

            }
            return CommandResult<NoteTemplateViewModel>.Instance(viewModel);
        }
        public async Task<CommandResult<NoteTemplateViewModel>> TriggerNewPersonRequestService(NoteTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            if (udf.BulkRequestId == null || udf.BulkRequestId == "")
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
                    return CommandResult<NoteTemplateViewModel>.Instance(viewModel, false, errorList);
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
                        var perModel = new PersonViewModel
                        {
                            UserId = userid,
                            TitleId = udf.TitleId,
                            FirstName = udf.FirstName,
                            LastName = udf.LastName,
                            GenderId = udf.GenderId,
                            DateOfBirth = Convert.ToDateTime(udf.DateOfBirth),
                            MaritalStatusId = udf.MaritalStatusId,
                            NationalityId = udf.NationalityId,
                            ReligionId = udf.ReligionId,
                            DateOfJoin = Convert.ToDateTime(udf.DateOfJoin),
                            BusinessHierarchyParentId = udf.BusinessHierarchyParentId
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
                                OrgLevel1Id = udf.OrgLevel1Id,
                                OrgLevel2Id = udf.OrgLevel2Id,
                                OrgLevel3Id = udf.OrgLevel3Id,
                                OrgLevel4Id = udf.OrgLevel4Id,
                                BrandId = udf.BrandId,
                                MarketId = udf.MarketId,
                                ProvinceId = udf.ProvinceId,
                                CareerLevelId = udf.CareerLevelId,
                                DateOfJoin = udf.DateOfJoin,
                                AssignmentStatusId = assignstatusid
                            };
                            assignnotemodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(assignModel);
                            assignnotemodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                            var assignResult = await _noteBusiness.ManageNote(assignnotemodel);
                            //var assignid = assignResult.Item.UdfNoteTableId;
                            if (assignResult.IsSuccess)
                            {

                            }
                            else
                            {
                                errorList.Add("Error", assignResult.Message);
                                return CommandResult<NoteTemplateViewModel>.Instance(viewModel, false, errorList);
                            }
                        }
                        else
                        {
                            errorList.Add("Error", perResult.Message);
                            return CommandResult<NoteTemplateViewModel>.Instance(viewModel, false, errorList);
                        }

                    }
                    else
                    {
                        errorList.Add("Error", userResult.Message);
                        return CommandResult<NoteTemplateViewModel>.Instance(viewModel, false, errorList);
                    }
                }
            }
            return CommandResult<NoteTemplateViewModel>.Instance(viewModel);
        }

        public async Task<CommandResult<NoteTemplateViewModel>> UpdateDepartmentNameRequest(NoteTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _hrCoreBusiness = sp.GetService<IHRCoreBusiness>();

            if (viewModel.NoteStatusCode == "NOTE_STATUS_INPROGRESS")
            {
                if (udf.BulkRequestId == null || udf.BulkRequestId == "")
                {
                    await _hrCoreBusiness.UpdateDepartmentName(udf);
                }
            }

            return CommandResult<NoteTemplateViewModel>.Instance(viewModel);
        }
        public async Task<CommandResult<NoteTemplateViewModel>> UpdateJobNameOnRequest(NoteTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _hrCoreBusiness = sp.GetService<IHRCoreBusiness>();

            if (viewModel.NoteStatusCode == "NOTE_STATUS_INPROGRESS")
            {
                if (udf.BulkRequestId == null || udf.BulkRequestId == "")
                {
                    await _hrCoreBusiness.UpdateJobName(udf);
                }
            }

            return CommandResult<NoteTemplateViewModel>.Instance(viewModel);
        }

        public async Task<CommandResult<NoteTemplateViewModel>> UpdatePersonDepartment(NoteTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            if (udf.BulkRequestId == null || udf.BulkRequestId == "")
            {
                var _hrCorebusiness = sp.GetService<IHRCoreBusiness>();
                await _hrCorebusiness.UpdatePersonDepartment(viewModel);
            }
            return CommandResult<NoteTemplateViewModel>.Instance(viewModel);
        }
        public async Task<CommandResult<NoteTemplateViewModel>> UpdatePersonJob(NoteTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            if (udf.BulkRequestId == null || udf.BulkRequestId == "")
            {
                var _hrCorebusiness = sp.GetService<IHRCoreBusiness>();
                await _hrCorebusiness.UpdatePersonJob(viewModel);
            }
            return CommandResult<NoteTemplateViewModel>.Instance(viewModel);
        }
        public async Task<CommandResult<NoteTemplateViewModel>> UpdateEmployeeDetailOnTerminate(NoteTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _hrCoreBusiness = sp.GetService<IHRCoreBusiness>();

            if (viewModel.NoteStatusCode == "NOTE_STATUS_INPROGRESS")
            {
                await _hrCoreBusiness.UpdatePersonType(udf);
                await _hrCoreBusiness.UpdateAssignmentStatus(udf);
                await _hrCoreBusiness.UpdateUserStatus(udf);
                await _hrCoreBusiness.UpdateContract(udf);


            }

            return CommandResult<NoteTemplateViewModel>.Instance(viewModel);
        }
        public async Task<CommandResult<NoteTemplateViewModel>> CreatePositionOnNewPositionRequest(NoteTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            if (udf.BulkRequestId == null || udf.BulkRequestId == "")
            {

                var _hrCorebusiness = sp.GetService<IHRCoreBusiness>();
                await _hrCorebusiness.CreateNewPosition(viewModel);

            }
            return CommandResult<NoteTemplateViewModel>.Instance(viewModel);
        }
        public async Task<CommandResult<NoteTemplateViewModel>> CreateDepartmentOnNewDepartment(NoteTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            if (udf.BulkRequestId == null || udf.BulkRequestId == "")
            {
                var _hrCorebusiness = sp.GetService<IHRCoreBusiness>();
                await _hrCorebusiness.CreateDepartment(viewModel);
            }
            return CommandResult<NoteTemplateViewModel>.Instance(viewModel);
        }
        public async Task<CommandResult<NoteTemplateViewModel>> CreateCareerLevelOnNewCareerLevel(NoteTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            if (udf.BulkRequestId == null || udf.BulkRequestId == "")
            {
                var _hrCorebusiness = sp.GetService<IHRCoreBusiness>();
                await _hrCorebusiness.CreateNewCareerLevel(viewModel);
            }
            return CommandResult<NoteTemplateViewModel>.Instance(viewModel);
        }
        public async Task<CommandResult<NoteTemplateViewModel>> CreateJobOnNewJob(NoteTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            if (udf.BulkRequestId == null || udf.BulkRequestId == "")
            {
                var _hrCorebusiness = sp.GetService<IHRCoreBusiness>();
                await _hrCorebusiness.CreateNewJob(viewModel);
            }
            return CommandResult<NoteTemplateViewModel>.Instance(viewModel);
        }

        public async Task<CommandResult<NoteTemplateViewModel>> CreatePersonOnNewPerson(NoteTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            if (udf.BulkRequestId == null || udf.BulkRequestId == "")
            {
                var _hrCorebusiness = sp.GetService<IHRCoreBusiness>();
                await _hrCorebusiness.CreateNewPerson(viewModel);
            }
            return CommandResult<NoteTemplateViewModel>.Instance(viewModel);
        }
    }
}
