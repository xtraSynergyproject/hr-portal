using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.ViewModel;
using CMS.UI.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Web.Areas.CMS.Controllers
{
    [Route("cms/query")]
    [ApiController]
    public class QueryController : Controller
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IFileBusiness _fileBusiness;
        private readonly ITableMetadataBusiness _tableMetadataBusiness;
        private readonly ITemplateBusiness _templateBusiness;
        private readonly ITemplateCategoryBusiness _templateCategoryBusiness;
        private readonly ITeamBusiness _teamBusiness;
        private readonly IUserContext _userContext;

        private AuthSignInManager<ApplicationIdentityUser> _customUserManager;

        public QueryController(AuthSignInManager<ApplicationIdentityUser> customUserManager
            , IServiceProvider serviceProvider, IFileBusiness fileBusiness, ITableMetadataBusiness tableMetadataBusiness
            , ITemplateBusiness templateBusiness, ITemplateCategoryBusiness templateCategoryBusiness, ITeamBusiness teamBusiness
            , IUserContext userContext)
        {
            _serviceProvider = serviceProvider;
            _fileBusiness = fileBusiness;
            _tableMetadataBusiness = tableMetadataBusiness;
            _templateBusiness = templateBusiness;
            _templateCategoryBusiness = templateCategoryBusiness;
            _teamBusiness = teamBusiness;
            _userContext = userContext;
        }

        [HttpGet]
        [Route("TableData")]
        public async Task<IActionResult> TableData(string tableName, string columns = null, string filter = null, string orderbyColumns = null, string orderBy = null, string where = null, bool isUnique = false, string uniqueColumns = null, bool ignoreJoins = false, string returnColumns = null, int? limit = null, int? skip = null, bool enableLocalization = false, string lang = null)
        {
            try
            {
                var split = tableName.Split('.');
                if (split.Length != 2)
                {
                    throw new ArgumentException("Invalid table name");
                }
                var cmsBusiness = _serviceProvider.GetService<ICmsBusiness>();
                var data = await cmsBusiness.GetData(split[0], split[1], columns, filter, orderbyColumns, OrderByEnum.Ascending, where, ignoreJoins, returnColumns, limit, skip, enableLocalization);
                //var j = JsonConvert.SerializeObject(data);
                //var result = JsonConvert.DeserializeObject(j);
                if (isUnique)
                {
                    if (uniqueColumns.IsNotNullAndNotEmpty())
                    {
                        var cols = uniqueColumns.Split(',');
                        data = data.DefaultView.ToTable(true, cols);
                    }

                }
                return Ok(data);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        [HttpGet]
        [Route("ColumnIdNameListAlias")]
        public async Task<IActionResult> ColumnIdNameListAlias(string tableName)
        {
            try
            {
                var split = tableName.Split('.');
                if (split.Length != 2)
                {
                    throw new ArgumentException("Invalid table name");
                }
                var columnMetadataBusiness = _serviceProvider.GetService<IColumnMetadataBusiness>();
                if (split[1] == "enum")
                {
                    var enumColunm = new List<IdNameViewModel>
                    {
                        new IdNameViewModel { DataType = DataTypeEnum.String, Id = "Name", Name = "Name" },
                        new IdNameViewModel { DataType = DataTypeEnum.String, Id = "Description", Name = "Description" },
                        new IdNameViewModel { DataType = DataTypeEnum.Int, Id = "EnumId", Name = "EnumId" }
                    };
                    return Ok(enumColunm);
                }

                var columns = new List<ColumnMetadataViewModel>();
                if (tableName.IsNotNullAndNotEmpty())
                {
                    columns = await columnMetadataBusiness.GetViewableColumnMetadataList(split[0], split[1], true);

                }
                return Ok(columns.Select(x => new { Id = x.Alias, Name = x.Alias, DataType = x.DataType.ToString() }));
            }
            catch (Exception)
            {

                throw;
            }

        }

        [HttpGet]
        [Route("ColumnIdNameListAliasById")]
        public async Task<IActionResult> ColumnIdNameListAliasById(string tableMetadataId)
        {
            try
            {
                var columnMetadataBusiness = _serviceProvider.GetService<IColumnMetadataBusiness>();
                var columns = new List<ColumnMetadataViewModel>();
                var tableMetadata = await _tableMetadataBusiness.GetSingleById(tableMetadataId);
                if (tableMetadataId.IsNotNullAndNotEmpty() && tableMetadata.IsNotNull())
                {
                    columns = await columnMetadataBusiness.GetViewableColumnMetadataList(tableMetadataId, tableMetadata.Type, true);
                }
                return Ok(columns.Select(x => new { Id = x.Alias, Name = x.Alias, DataType = x.DataType.ToString() }));
            }
            catch (Exception)
            {
                throw;
            }

        }
        [HttpGet]
        [Route("ColumnIdNameList")]
        public async Task<IActionResult> ColumnIdNameList(string tableName)
        {
            try
            {
                var split = tableName.Split('.');
                if (split.Length != 2)
                {
                    throw new ArgumentException("Invalid table name");
                }
                var columnMetadataBusiness = _serviceProvider.GetService<IColumnMetadataBusiness>();
                var columns = new List<ColumnMetadataViewModel>();
                if (tableName.IsNotNullAndNotEmpty())
                {
                    columns = await columnMetadataBusiness.GetViewableColumnMetadataList(split[0], split[1], true);

                }
                return Ok(columns.Select(x => new { Id = x.Id, Name = x.Name, DataType = x.DataType.ToString() }));
            }
            catch (Exception)
            {

                throw;
            }

        }
        [HttpGet]
        [Route("MultipleColumnIdNameList")]
        public async Task<IActionResult> MultipleColumnIdNameList(string tableName)
        {
            try
            {
                //var split = tableName.Split('.');
                //if (split.Length != 2)
                //{
                //    throw new ArgumentException("Invalid table name");
                //}
                var columnMetadataBusiness = _serviceProvider.GetService<IColumnMetadataBusiness>();
                var columns = new List<ColumnMetadataViewModel>();
                if (tableName.IsNotNullAndNotEmpty())
                {
                    var table = tableName.Split(',');
                    foreach (var item in table)
                    {
                        if (item != "")
                        {
                            var split = item.Split('.');
                            if (split.Length != 2)
                            {
                                throw new ArgumentException("Invalid table name");
                            }
                            var column = await columnMetadataBusiness.GetViewableColumnMetadataList(split[0], split[1], false);
                            column.ForEach(x => x.Name = split[1] + "." + x.Name);
                            columns.AddRange(column);
                        }
                    }

                }
                return Ok(columns.Select(x => new { Id = x.Id, Name = x.Name, DataType = x.DataType.ToString() }));
            }
            catch (Exception)
            {

                throw;
            }

        }
        [HttpGet]
        [Route("ColumnList")]
        public async Task<IActionResult> ColumnList(string tableName)
        {
            try
            {
                var split = tableName.Split('.');
                if (split.Length != 2)
                {
                    throw new ArgumentException("Invalid table name");
                }
                var columnMetadataBusiness = _serviceProvider.GetService<IColumnMetadataBusiness>();
                var columns = await columnMetadataBusiness.GetViewableColumnMetadataList(split[0], split[1], true);
                return Ok(columns);
            }
            catch (Exception)
            {

                throw;
            }

        }
        [HttpGet]
        [Route("Test")]
        public async Task<IActionResult> Test()
        {
            try
            {
                var columns = new IdNameViewModel { Id = "df", Name = "sd" };
                return Ok(columns);
            }
            catch (Exception)
            {

                throw;
            }

        }

        [HttpGet]
        [Route("TestApi/{tableName}")]
        public async Task<IActionResult> TestApi(string tableName)
        {
            try
            {
                var split = tableName.Split('.');
                if (split.Length != 2)
                {
                    throw new ArgumentException("Invalid table name");
                }
                var columnMetadataBusiness = _serviceProvider.GetService<IColumnMetadataBusiness>();
                var columns = await columnMetadataBusiness.GetViewableColumnMetadataList(split[0], split[1], true);
                return Ok(columns);
            }
            catch (Exception)
            {

                throw;
            }

        }
        [HttpGet]
        [Route("TableIdNameList")]
        public async Task<IActionResult> TableIdNameList()
        {
            try
            {
                var tableMetadataBusiness = _serviceProvider.GetService<ITableMetadataBusiness>();
                var tables = await tableMetadataBusiness.GetList();
                return Ok(tables.Select(x => new { Id = x.Id, Name = x.Schema + "." + x.Name }));
            }
            catch (Exception)
            {

                throw;
            }

        }
        [HttpGet]
        [Route("TableList")]
        public async Task<IActionResult> TableList()
        {
            try
            {
                var tableMetadataBusiness = _serviceProvider.GetService<ITableMetadataBusiness>();
                var tables = await tableMetadataBusiness.GetList();
                return Ok(tables);
            }
            catch (Exception)
            {

                throw;
            }

        }
        [HttpGet]
        [Route("GetLOVIdNameList")]
        public async Task<IActionResult> GetLOVIdNameList(string lovType, string parentId, string excludeItem = null)
        {
            try
            {
                var lovBusiness = _serviceProvider.GetService<ILOVBusiness>();
                var list = await lovBusiness.GetList(x => x.LOVType == lovType && x.Status == StatusEnum.Active);
                if (parentId.IsNotNullAndNotEmpty())
                {
                    list = list.Where(x => x.ParentId == parentId).ToList();

                }
                list = list.OrderBy(x => x.SequenceOrder).ThenBy(x => x.Name).ToList();

                var result = list.Select(x => new IdNameViewModel { Id = x.Id, Name = x.Name, Code = x.Code }).ToList();
                if (excludeItem.IsNotNullAndNotEmpty())
                {
                    result = result.Where(x => x.Code != excludeItem).ToList();
                }

                return Ok(result);
            }
            catch (Exception)
            {

                throw;
            }

        }

        [HttpGet]
        [Route("GetCustomIdNameList")]
        public async Task<IActionResult> GetCustomIdNameList(string customType, string prms)
        {
            var result = new List<IdNameViewModel>();
            try
            {
                switch (customType)
                {
                    case "PropertyConsultant":
                        var lovBusiness = _serviceProvider.GetService<ILOVBusiness>();
                        var list = await lovBusiness.GetList();
                        result = list.Select(x => new IdNameViewModel { Id = x.Id, Name = x.Name, Code = x.Code }).ToList();
                        break;
                }
                return Ok(result);
            }
            catch (Exception)
            {

                throw;
            }

        }

        [HttpGet]
        [Route("GetEnumIdNameList")]
        public async Task<IActionResult> GetEnumIdNameList(string enumType, string exculdeItem1 = "", string exculdeItem2 = "", string exculdeItem3 = "")
        {
            try
            {

                var list = new List<IdNameViewModel>();
                var t = string.Concat("Synergy.App.Common.", enumType, ", Synergy.App.Common");
                var type = Type.GetType(t);
                if (type.IsEnum)
                {
                    list = Enum.GetValues(type)
                        .Cast<Enum>()
                         .Where(i => i.ToString() != exculdeItem1)
                        .Select(e => new IdNameViewModel()
                        {
                            Name = e.Description(),
                            EnumId = Convert.ToInt32(e),
                            Id = Enum.GetName(type, e)
                        })
                        .ToList();
                    return await Task.FromResult(Ok(list));
                }
                throw new ArgumentException("Invalid enum type");

            }
            catch (Exception)
            {

                throw;
            }

        }

        [HttpPost]
        [Route("SaveFileInFormIo")]
        public async Task<IActionResult> UploadFormIo(string baseUrl = "", ReferenceTypeEnum referenceType = ReferenceTypeEnum.NTS_Note, string referenceId = "")
        {
            var files = HttpContext.Request.Form.Files;
            try
            {
                foreach (var file in files)
                {
                    var ms = new MemoryStream();
                    file.OpenReadStream().CopyTo(ms);
                    var result = await _fileBusiness.Create(new FileViewModel
                    {
                        ContentByte = ms.ToArray(),
                        ContentType = file.ContentType,
                        ContentLength = file.Length,
                        FileName = file.FileName,
                        FileExtension = Path.GetExtension(file.FileName),
                        ReferenceTypeCode = referenceType,
                        ReferenceTypeId = referenceId
                    }
                    );
                    if (result.IsSuccess)
                    {
                        return Json(new { success = true, fileId = result.Item.Id, fileName = result, data = result.Item });

                    }
                    else
                    {
                        return Json(new { success = false });
                    }
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false });
            }
            return Json(new { success = false });
        }
        [HttpGet]
        [Route("GetDashboardItemIdNameList")]
        public async Task<IActionResult> GetDashboardItemIdNameList()
        {

            try
            {

                var noteBusiness = _serviceProvider.GetService<INoteBusiness>();
                var list = await noteBusiness.GetAllDashboardItemDetailsWithDashboard();
                return Ok(list);
            }
            catch (Exception)
            {

                throw;
            }

        }

        [HttpGet]
        [Route("GetUnAssignedPersonList")]
        public async Task<IActionResult> GetUnAssignedPersonList(string personId)
        {
            try
            {
                var hrBusiness = _serviceProvider.GetService<IHRCoreBusiness>();
                var list = await hrBusiness.GetUnAssignedPersonList(personId);
                return Ok(list);
            }
            catch (Exception)
            {
                throw;
            }

        }

        [HttpGet]
        [Route("GetAssignedPersonList")]
        public async Task<IActionResult> GetAssignedPersonList(string personId)
        {
            try
            {
                var hrBusiness = _serviceProvider.GetService<IHRCoreBusiness>();
                var list = await hrBusiness.GetAssignedPersonList(personId);
                return Ok(list);
            }
            catch (Exception)
            {
                throw;
            }

        }

        [HttpGet]
        [Route("GetJobByPerson")]
        public async Task<IActionResult> GetJobByPerson(string jobId, string personId)
        {
            try
            {
                var hrBusiness = _serviceProvider.GetService<IHRCoreBusiness>();
                var list = await hrBusiness.GetJobByPerson(jobId, personId);
                return Ok(list);
            }
            catch (Exception)
            {
                throw;
            }

        }

        [HttpGet]
        [Route("GetDepartmentByPerson")]
        public async Task<IActionResult> GetDepartmentByPerson(string departmentId, string personId)
        {
            try
            {
                var hrBusiness = _serviceProvider.GetService<IHRCoreBusiness>();
                var list = await hrBusiness.GetDepartmentByPerson(departmentId, personId);
                return Ok(list);
            }
            catch (Exception)
            {
                throw;
            }

        }



        [HttpGet]
        [Route("GetUnAssignedPositionList")]
        public async Task<IActionResult> GetUnAssignedPositionList(string positionId, string departmentId, string jobId)
        {
            try
            {
                var hrBusiness = _serviceProvider.GetService<IHRCoreBusiness>();
                var list = await hrBusiness.GetUnAssignedPositionList(positionId, departmentId, jobId);
                return Ok(list);
            }
            catch (Exception)
            {
                throw;
            }

        }
        [HttpGet]
        [Route("GetUnAssignedUserList")]
        public async Task<IActionResult> GetUnAssignedUserList(string userId)
        {
            try
            {
                var hrBusiness = _serviceProvider.GetService<IHRCoreBusiness>();
                var list = await hrBusiness.GetUnAssignedUserList(userId);
                return Ok(list);
            }
            catch (Exception)
            {
                throw;
            }

        }
        [HttpGet]
        [Route("GetUnAssignedContractPersionList")]
        public async Task<IActionResult> GetUnAssignedContractPersionList(string personId)
        {
            try
            {
                var hrBusiness = _serviceProvider.GetService<IHRCoreBusiness>();
                var list = await hrBusiness.GetUnAssignedContractPersionList(personId);
                return Ok(list);
            }
            catch (Exception)
            {
                throw;
            }

        }

        [HttpGet]
        [Route("GetUnAssignedChildPositionList")]
        public async Task<IActionResult> GetUnAssignedChildPositionList(string parentId, string positionId)
        {
            try
            {
                var hrBusiness = _serviceProvider.GetService<IHRCoreBusiness>();
                var list = await hrBusiness.GetUnAssignedChildPositionList(parentId, positionId);
                return Ok(list);
            }
            catch (Exception)
            {
                throw;
            }

        }

        [HttpGet]
        [Route("GetUnAssignedChildDepartmentList")]
        public async Task<IActionResult> GetUnAssignedChildDepartmentList(string parentId, string departmentId)
        {
            try
            {
                var hrBusiness = _serviceProvider.GetService<IHRCoreBusiness>();
                var list = await hrBusiness.GetUnAssignedChildDepartmentList(parentId, departmentId);
                return Ok(list);
            }
            catch (Exception)
            {
                throw;
            }

        }

        [HttpGet]
        [Route("GetNonExistingPosition")]
        public async Task<IActionResult> GetNonExistingPosition(string hierarchyId, string positionId)
        {
            try
            {
                var hrBusiness = _serviceProvider.GetService<IHRCoreBusiness>();
                var list = await hrBusiness.GetNonExistingPosition(hierarchyId, positionId);
                return Ok(list);
            }
            catch (Exception)
            {
                throw;
            }

        }

        [HttpGet]
        [Route("GetPositionWithParent")]
        public async Task<IActionResult> GetPositionWithParent(string hierarchyId, string positionId)
        {
            try
            {
                var hrBusiness = _serviceProvider.GetService<IHRCoreBusiness>();
                var list = await hrBusiness.GetPositionWithParent(hierarchyId, positionId);
                return Ok(list);
            }
            catch (Exception)
            {
                throw;
            }

        }


        [HttpGet]
        [Route("GetNonExistingDepartment")]
        public async Task<IActionResult> GetNonExistingDepartment(string hierarchyId, string deptId)
        {
            try
            {
                var hrBusiness = _serviceProvider.GetService<IHRCoreBusiness>();
                var list = await hrBusiness.GetNonExistingDepartment(hierarchyId, deptId);
                return Ok(list);
            }
            catch (Exception)
            {
                throw;
            }

        }

        [HttpGet]
        [Route("GetDepartmentWithParent")]
        public async Task<IActionResult> GetDepartmentWithParent(string hierarchyId, string deptId)
        {
            try
            {
                var hrBusiness = _serviceProvider.GetService<IHRCoreBusiness>();
                var list = await hrBusiness.GetDepartmentWithParent(hierarchyId, deptId);
                return Ok(list);
            }
            catch (Exception)
            {
                throw;
            }

        }
        [HttpGet]
        [Route("GetUserWithParent")]
        public async Task<IActionResult> GetUserWithParent(string hierarchyId, string parentUserId, string userId)
        {
            try
            {
                var hrBusiness = _serviceProvider.GetService<IHRCoreBusiness>();
                var list = await hrBusiness.GetUserWithParent(hierarchyId, parentUserId, userId);
                return Ok(list);
            }
            catch (Exception)
            {
                throw;
            }

        }
        [HttpGet]
        [Route("GetTemplatesOnCategory")]
        public async Task<IActionResult> GetTemplatesOnCategory()
        {
            try
            {
                var tempCategoryId = await _templateCategoryBusiness.GetSingle(x => x.Code == "GENERAL_DOCUMENT");
                var templatlist = await _templateBusiness.GetList(x => x.TemplateCategoryId == tempCategoryId.Id && x.Code != "GENERAL");
                return Json(templatlist);

            }
            catch (Exception)
            {
                throw;
            }

        }
        [HttpGet]
        [Route("GetSebiInternalTemplatesOnCategory")]
        public async Task<IActionResult> GetSebiInternalTemplatesOnCategory(string teamId)
        {
            try
            {
                //var tempCategoryId = await _templateCategoryBusiness.GetSingle(x => x.Code == "CMS_SEBI_INT");
                //var templatlist = await _templateBusiness.GetList(x => x.TemplateCategoryId == tempCategoryId.Id); 
                var templatlist = await _templateBusiness.GetTemplateServiceListbyTeam("CMS_SEBI_INT", teamId);
                return Json(templatlist);

            }
            catch (Exception)
            {
                throw;
            }

        }
        [HttpGet]
        [Route("GetAllTeamByPortal")]
        public async Task<IActionResult> GetAllTeamByPortal()
        {
            try
            {
                var userContext = _serviceProvider.GetService<IUserContext>();
                var teamList = await _teamBusiness.GetList(x => x.PortalId == userContext.PortalId);
                var result = teamList.Select(x => new IdNameViewModel { Id = x.Id, Name = x.Name, Code = x.Code }).ToList();
                return Ok(result);

            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet]
        [Route("GetServiceBookReport")]
        public async Task<IActionResult> GetServiceBookReport(string serviceId)
        {
            var _userContext = _serviceProvider.GetService<IUserContext>();
            var _companyBusiness = _serviceProvider.GetService<ICompanyBusiness>();
            var _serviceBusiness = _serviceProvider.GetService<IServiceBusiness>();
            try
            {
                var report = new BookReportViewModel();
                report.LoggedInUserName = _userContext.Name;
                report.CurrentDate = System.DateTime.Today;
                var company = await _companyBusiness.GetSingleById(_userContext.CompanyId);
                if (company.LogoFileId.IsNotNullAndNotEmpty())
                {
                    var bytes = await _fileBusiness.GetFileByte(company.LogoFileId);
                    if (bytes.Length > 0)
                    {
                        report.Logo = Convert.ToBase64String(bytes, 0, bytes.Length);
                    }

                }
                report.NtsItems = await _serviceBusiness.GetServiceBookDetails(serviceId);
                return Ok(report);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet]
        [Route("GetNoteBookReport")]
        public async Task<IActionResult> GetNoteBookReport(string noteId)
        {
            var _userContext = _serviceProvider.GetService<IUserContext>();
            var _companyBusiness = _serviceProvider.GetService<ICompanyBusiness>();
            var _noteBusiness = _serviceProvider.GetService<INoteBusiness>();
            try
            {
                var report = new BookReportViewModel();
                report.LoggedInUserName = _userContext.Name;
                report.CurrentDate = System.DateTime.Today;
                var company = await _companyBusiness.GetSingleById(_userContext.CompanyId);
                if (company.LogoFileId.IsNotNullAndNotEmpty())
                {
                    var bytes = await _fileBusiness.GetFileByte(company.LogoFileId);
                    if (bytes.Length > 0)
                    {
                        report.Logo = Convert.ToBase64String(bytes, 0, bytes.Length);
                    }

                }
                report.NtsItems = await _noteBusiness.GetNoteBookDetails(noteId);
                return Ok(report);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet]
        [Route("GetCMSExternalRequest")]
        public async Task<IActionResult> GetCMSExternalRequest()
        {
            try
            {
                var cmsExternal = _serviceProvider.GetService<IServiceBusiness>();
                var list = await cmsExternal.GetCMSExternalRequest();
                return Ok(list);
            }
            catch (Exception)
            {
                throw;
            }

        }

        [HttpGet]
        [Route("GetTeamUsersByCode")]
        public async Task<IActionResult> GetTeamUsersByCode(string code)
        {
            try
            {
                var teamBusiness = _serviceProvider.GetService<ITeamBusiness>();
                var list = await teamBusiness.GetTeamUsersByCode(code);
                return Ok(list);
            }
            catch (Exception)
            {
                throw;
            }

        }

        [HttpGet]
        [Route("GetSubDivisionByArea")]
        public async Task<IActionResult> GetSubDivisionByArea(string area)
        {
            try
            {
                var lovBusiness = _serviceProvider.GetService<ILOVBusiness>();
                var list = await lovBusiness.GetList(x => x.ParentId == area);
                return Ok(list);
            }
            catch (Exception)
            {
                throw;
            }

        }

        [HttpGet]
        [Route("GetWardByMunicipal")]
        public async Task<IActionResult> GetWardByMunicipal(string municipal)
        {
            try
            {
                var lovBusiness = _serviceProvider.GetService<ILOVBusiness>();
                var list = await lovBusiness.GetList(x => x.ParentId == municipal);
                return Ok(list);
            }
            catch (Exception)
            {
                throw;
            }

        }

        [HttpGet]
        [Route("GetRatingDetailsFromDocumentMaster/{documentMasterId}")]
        public async Task<IActionResult> GetRatingDetailsFromDocumentMaster(string documentMasterId)
        {
            try
            {
                var _pmtBusiness = _serviceProvider.GetService<IPerformanceManagementBusiness>();
                var data = new List<IdNameViewModel>();
                if (documentMasterId.IsNotNullAndNotEmpty())
                {
                    data = await _pmtBusiness.GetRatingDetailsFromDocumentMaster(documentMasterId);
                }
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }

        }

        [HttpGet]
        [Route("GetComplaintSubType")]
        public async Task<IActionResult> GetComplaintSubType(string lovType, string parentId)
        {
            try
            {
                var lovBusiness = _serviceProvider.GetService<ILOVBusiness>();
                var list = await lovBusiness.GetList(x => x.LOVType == lovType && x.ParentId == parentId);

                var result = list.Select(x => new IdNameViewModel { Id = x.Id, Name = x.Name, Code = x.Code }).ToList();
                return Ok(result);
            }
            catch (Exception)
            {

                throw;
            }

        }
        [HttpGet]
        [Route("GetComplaintSubTypeByCode")]
        public async Task<IActionResult> GetComplaintSubTypeByCode(string lovType, string parentCode)
        {
            try
            {
                var lovBusiness = _serviceProvider.GetService<ILOVBusiness>();
                var parentLov = await lovBusiness.GetSingle(x => x.Code == parentCode);
                var parentId = parentLov.Id;
                var list = await lovBusiness.GetList(x => x.LOVType == lovType && x.ParentId == parentId);
                var result = list.Select(x => new IdNameViewModel { Id = x.Id, Name = x.Name, Code = x.Code }).ToList();
                return Ok(result);
            }
            catch (Exception)
            {

                throw;
            }

        }
        [HttpGet]
        [Route("GetBHPermissionListByGroupCode")]
        public async Task<IActionResult> GetBHPermissionListByGroupCode(string lovType, string groupCode, string permissionId)
        {
            try
            {
                var lovBusiness = _serviceProvider.GetService<ILOVBusiness>();
                var _hybridHierarchyBusiness = _serviceProvider.GetService<IHybridHierarchyBusiness>();
                var list = await lovBusiness.GetList(x => x.LOVType == lovType);
                if (groupCode.IsNotNullAndNotEmpty())
                {
                    list = await lovBusiness.GetList(x => x.LOVType == lovType && x.GroupCode == groupCode);
                }
                var bhpList = await _hybridHierarchyBusiness.GetBusinessHierarchyPermissionData(null);
                if (permissionId.IsNotNullAndNotEmpty() && bhpList.Count > 0)
                {
                    bhpList = bhpList.Where(c => c.PermissionId != permissionId).ToList();
                }
                if (list.Count > 0 && bhpList.Count > 0)
                {
                    list = list.Where(p => !bhpList.Any(p2 => p2.PermissionId == p.Id)).ToList();
                }
                var result = list.Select(x => new IdNameViewModel { Id = x.Id, Name = x.Name, Code = x.Code }).ToList();
                return Ok(result);
            }
            catch (Exception)
            {
                throw;
            }

        }

        [HttpGet]
        [Route("GetParentGoalByDepartment")]
        public async Task<IActionResult> GetParentGoalByDepartment(string departmentId)
        {
            try
            {
                var pmsBusiness = _serviceProvider.GetService<IPerformanceManagementBusiness>();
                if (departmentId.IsNullOrEmpty())
                {
                    departmentId = _userContext.OrganizationId;
                }
                var list = await pmsBusiness.GetParentGoalByDepartment(departmentId);

                var result = list.Select(x => new IdNameViewModel { Id = x.Id, Name = x.Name, Code = x.Code }).ToList();
                return Ok(result);
            }
            catch (Exception)
            {

                throw;
            }

        }
        [HttpGet]
        [Route("GetDepartmentGoal")]
        public async Task<IActionResult> GetDepartmentGoal()
        {
            try
            {
                var pmsBusiness = _serviceProvider.GetService<IPerformanceManagementBusiness>();

                var list = await pmsBusiness.GetDepartmentGoal(_userContext.OrganizationId);

                var result = list.Select(x => new IdNameViewModel { Id = x.Id, Name = x.Name, Code = x.Code }).ToList();
                return Ok(result);
            }
            catch (Exception)
            {

                throw;
            }

        }
        [HttpGet]
        [Route("GetPerformanceMasterByDepatment")]
        public async Task<IActionResult> GetPerformanceMasterByDepatment(string departmentId)
        {
            try
            {
                var pmsBusiness = _serviceProvider.GetService<IPerformanceManagementBusiness>();

                var list = await pmsBusiness.GetPerformanceMasterByDepatment(departmentId, null);

                var result = list.Select(x => new IdNameViewModel { Id = x.Id, Name = x.Name, Code = x.Code }).ToList();
                return Ok(result);
            }
            catch (Exception)
            {

                throw;
            }

        }

        [HttpGet]
        [Route("GetDepartmentBasedOnUser")]
        public async Task<IActionResult> GetDepartmentBasedOnUser()
        {
            try
            {
                var pmsBusiness = _serviceProvider.GetService<IPerformanceManagementBusiness>();

                var list = await pmsBusiness.GetDepartmentBasedOnUser();

                var result = list.Select(x => new IdNameViewModel { Id = x.Id, Name = x.Name, Code = x.Code }).ToList();
                return Ok(result);
            }
            catch (Exception)
            {

                throw;
            }

        }


        [HttpGet]
        [Route("ReadPerformanceUsers")]
        public async Task<IActionResult> ReadPerformanceUsers()
        {
            try
            {
                var _business = _serviceProvider.GetService<IHRCoreBusiness>();
                var list = await _business.GetUsersInfo();

                var result = list.Select(x => new IdNameViewModel { Id = x.UserId, Name = x.PersonFullName }).ToList();
                return Ok(result);
            }
            catch (Exception)
            {

                throw;
            }

        }
        [HttpGet]
        [Route("GetParentGoal")]
        public async Task<IActionResult> GetParentGoal(string goalId)
        {
            try
            {
                var pmsBusiness = _serviceProvider.GetService<IPerformanceManagementBusiness>();

                var str = await pmsBusiness.GetParentGoal(goalId);

                var result = str;
                return Ok(result);
            }
            catch (Exception)
            {

                throw;
            }

        }

        [HttpGet]
        [Route("GetDepartmentByType")]
        public async Task<IActionResult> GetDepartmentByType(string type, string level)
        {
            try
            {
                var hrBusiness = _serviceProvider.GetService<IHRCoreBusiness>();
                var list = await hrBusiness.GetDepartmentByType(type, level);
                return Ok(list);
            }
            catch (Exception)
            {
                throw;
            }

        }

        [HttpGet]
        [Route("GetTeamByGroupCode")]
        public async Task<IActionResult> GetTeamByGroupCode(string code)
        {

            try
            {
                var TeamBusiness = _serviceProvider.GetService<ITeamBusiness>();
                var list = await TeamBusiness.GetList(x => x.GroupCode == code);
                return Ok(list);
            }
            catch (Exception)
            {
                throw;
            }

        }

        [HttpGet]
        [Route("GetDeductionPayrollElement")]
        public async Task<IActionResult> GetDeductionPayrollElement()
        {
            try
            {
                var lovBusiness = _serviceProvider.GetService<IPayrollElementBusiness>();
                var list = await lovBusiness.GetPayrollDeductionElement();
                return Ok(list);
            }
            catch (Exception)
            {
                throw;
            }

        }

        [Route("getlicensedetils")]
        [HttpGet]
        public async Task<ActionResult> GetLicenseDetils(string licenseKey, string licensePrivateKey, string machineName)
        {
            try
            {
                var _business = _serviceProvider.GetService<ISalesBusiness>();
                var data = await _business.GetLicenseDetails(licenseKey, licensePrivateKey, machineName);
                return Json(new { success = true, result = data });
            }
            catch (Exception e)
            {
                return Json(new { success = false, errors = e.ToString() });
            }

        }

        [HttpGet]
        [Route("GetPerformanceBookReport")]
        public async Task<IActionResult> GetPerformanceBookReport(string serviceId)
        {
            var _userContext = _serviceProvider.GetService<IUserContext>();
            var _companyBusiness = _serviceProvider.GetService<ICompanyBusiness>();
            var _pmtBusiness = _serviceProvider.GetService<IPerformanceManagementBusiness>();
            try
            {
                var report = new BookReportViewModel();
                report.LoggedInUserName = _userContext.Name;
                report.CurrentDate = System.DateTime.Today;
                var company = await _companyBusiness.GetSingleById(_userContext.CompanyId);
                if (company.LogoFileId.IsNotNullAndNotEmpty())
                {
                    var bytes = await _fileBusiness.GetFileByte(company.LogoFileId);
                    if (bytes.Length > 0)
                    {
                        report.Logo = Convert.ToBase64String(bytes, 0, bytes.Length);
                    }

                }
                report.NtsItems = await _pmtBusiness.GetBookList(serviceId, null);
                return Ok(report);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //[HttpGet]
        //[Route("GetCSCBirthCertificateReport")]
        //public async Task<IActionResult> GetCSCBirthCertificateReport(string serviceId)
        //{
        //    //serviceId = "875bd230-0af3-410a-ac85-5e5ccd4e46f6";
        //    var _userContext = _serviceProvider.GetService<IUserContext>();
        //    var _companyBusiness = _serviceProvider.GetService<ICompanyBusiness>();
        //    var _commonServiceBusiness = _serviceProvider.GetService<ICommonServiceBusiness>();
        //    var _applicationDocumentBusiness = _serviceProvider.GetService<IApplicationDocumentBusiness>();
        //    //var report = new CSCReportViewModel();
        //    try
        //    {
        //        var report = await _commonServiceBusiness.GetCSCBirthCertificateData(serviceId);
        //        if (report==null)
        //        {
        //            report = new CSCReportViewModel();
        //        }
        //        //report.LocalAreaName = "Raipur";
        //        //report.TehasilBlockName = "Raipur";
        //        //report.TehasilBlockName = "Raipur";
        //        //report.DistrictName = "Raipur";
        //        //report.StateName = "Chhattisgarh";
        //        //report.Name = "Ritesh Baghel";
        //        //report.GenderName = "Male";
        //        //report.BirthDate = new DateTime(2022,06,02);
        //        //report.BirthPlace = "Raipur";
        //        //report.MotherName = "Manju Baghel";
        //        //report.FatherName = "Sanjay Baghel";
        //        //report.CurrentAddress = "H No 101, Zone 1, Raipur ";
        //        //report.PermanentAddress = "H No 101, Zone 1, Raipur ";
        //        //report.RegistrationNo = "CG/BRC/2022/06/10/01";
        //        //report.RegistrationDate = new DateTime(2022, 06, 10);
        //        //report.Remarks = "Good Health";
        //        //report.IssueDate = DateTime.Today;
        //        //report.AuthorityAddress = "Admin Office, Raipur, Chhattisgarh";
        //        //report.ServiceId = "01";
        //        //report.ServiceNo = "S-13.06.2022-01";
        //        if (report.BirthDate.IsNotNull())
        //        {
        //            report.BirthDateText = string.Format("{0:dd MMM yyyy}", report.BirthDate);
        //        }
        //        if (report.RegistrationDate.IsNotNull())
        //        {
        //            report.RegistrationDateText = string.Format("{0:dd MMM yyyy}", report.RegistrationDate);
        //        }
        //        if (report.IssueDate.IsNotNull())
        //        {
        //            report.IssueDateText = string.Format("{0:dd MMM yyyy}", report.IssueDate);
        //        }
        //        var birthlogo = await _applicationDocumentBusiness.GetSingle(x => x.Code == "GOVT_BIRTH_DEATH_LOGO");
        //        if (birthlogo.IsNotNull())
        //        {
        //            var birthlogobytes = await _fileBusiness.GetFileByte(birthlogo.DocumentId);
        //            if (birthlogobytes.Length > 0)
        //            {
        //               report.BirthDeathLogo = Convert.ToBase64String(birthlogobytes, 0, birthlogobytes.Length);
        //            }
        //        }

        //        var govtlogo = await _applicationDocumentBusiness.GetSingle(x => x.Code == "GOVT_3LION_LOGO");
        //        if (govtlogo.IsNotNull())
        //        {
        //            var govtlogobytes = await _fileBusiness.GetFileByte(govtlogo.DocumentId);
        //            if (govtlogobytes.Length > 0)
        //            {
        //                report.GovtLogo = Convert.ToBase64String(govtlogobytes, 0, govtlogobytes.Length);
        //            }
        //        }
        //        //var seallogo = await _applicationDocumentBusiness.GetSingle(x => x.Code == "CERTIFIED_LOGO");
        //        var seallogo = await _applicationDocumentBusiness.GetSingle(x => x.Code == "APPROVED_LOGO");
        //        if (seallogo.IsNotNull())
        //        {
        //            var seallogobytes = await _fileBusiness.GetFileByte(seallogo.DocumentId);
        //            if (seallogobytes.Length > 0)
        //            {
        //                report.SealLogo = Convert.ToBase64String(seallogobytes, 0, seallogobytes.Length);
        //            }
        //        }
        //        return Ok(report);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        [HttpGet]
        [Route("GetCSCUserList")]
        public async Task<IActionResult> GetCSCUserList()
        {
            try
            {
                var userBusiness = _serviceProvider.GetService<IUserBusiness>();
                var list = await userBusiness.GetAllCSCUsersList();
                return Ok(list);
            }
            catch (Exception)
            {
                throw;
            }

        }

        [HttpGet]
        [Route("GetCSCOfficeType")]
        public async Task<IActionResult> GetCSCOfficeType(string templateCode)
        {
            try
            {
                var _cmsBusiness = _serviceProvider.GetService<ICmsBusiness>();
                var list = await _cmsBusiness.GetCSCOfficeType(templateCode);
                return Ok(list);
            }
            catch (Exception)
            {
                throw;
            }

        }
        [HttpGet]
        [Route("GetCSCSubOfficeType")]
        public async Task<IActionResult> GetCSCSubOfficeType(string officeId, string districtId)
        {
            try
            {
                var _cmsBusiness = _serviceProvider.GetService<ICmsBusiness>();
                var list = await _cmsBusiness.GetCSCSubfficeType(officeId, districtId);
                return Ok(list);
            }
            catch (Exception)
            {
                throw;
            }

        }
        [HttpGet]
        [Route("GetCSCRevenueVillage")]
        public async Task<IActionResult> GetCSCRevenueVillage(string officeId, string subDistrictId)
        {
            try
            {
                var _cmsBusiness = _serviceProvider.GetService<ICmsBusiness>();
                var list = await _cmsBusiness.GetRevenueVillage(officeId, subDistrictId);
                return Ok(list);
            }
            catch (Exception)
            {
                throw;
            }

        }
        [HttpGet]
        [Route("GetParcelIdNameList")]
        public async Task<IActionResult> GetParcelIdNameList()
        {
            try
            {
                var _smartCityBusiness = _serviceProvider.GetService<ISmartCityBusiness>();
                var list = await _smartCityBusiness.GetParcelIdNameList();
                return Ok(list);
            }
            catch (Exception)
            {
                throw;
            }

        }
        [HttpGet]
        [Route("GetBinCollectorNameList")]
        public async Task<IActionResult> GetBinCollectorNameList()
        {
            try
            {
                var _smartCityBusiness = _serviceProvider.GetService<ISmartCityBusiness>();
                var list = await _smartCityBusiness.GetBinCollectorNameList();
                return Ok(list);
            }
            catch (Exception)
            {
                throw;
            }

        }
        [HttpGet]
        [Route("GetPortalUserIdNameList")]
        public async Task<IActionResult> GetPortalUserIdNameList()
        {
            try
            {
                var _userBusiness = _serviceProvider.GetService<IUserBusiness>();
                var data1 = await _userBusiness.GetAllUsersWithPortalId(_userContext.PortalId);               
                var data = data1.Select(x => new IdNameViewModel()
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToList();               
                return Ok(data);
            }
            catch (Exception)
            {
                throw;
            }
         
        }

        [HttpGet]
        [Route("GetEGovAdminConstituencyIdNameList")]
        public async Task<IActionResult> GetEGovAdminConstituencyIdNameList()
        {
            try
            {
                var _egovernanceBusiness = _serviceProvider.GetService<IEGovernanceBusiness>();
                var list = await _egovernanceBusiness.GetEGovAdminConstituencyIdNameList();
                return Ok(list);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        [Route("GetAssetTypeIdNameList")]
        public async Task<IActionResult> GetAssetTypeIdNameList()
        {
            try
            {
                var _smartCityBusiness = _serviceProvider.GetService<ISmartCityBusiness>();
                var list = await _smartCityBusiness.GetAssetTypeIdNameList();
                return Ok(list);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
