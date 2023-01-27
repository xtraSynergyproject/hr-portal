using CMS.Business;
using CMS.Common;
using CMS.UI.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CMS.Web.Api.Controllers;
using System.Data;
using System.IO;


namespace CMS.Web.Api.Areas.CMS.Controllers
{
    [Route("cms/query")]
    [ApiController]
    public class QueryController : ApiController
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IFileBusiness _fileBusiness;
        private readonly ITableMetadataBusiness _tableMetadataBusiness;
        private readonly IHttpContextAccessor _accessor;


        private AuthSignInManager<ApplicationIdentityUser> _customUserManager;

        public QueryController(AuthSignInManager<ApplicationIdentityUser> customUserManager, IHttpContextAccessor accessor
            , IServiceProvider serviceProvider, IFileBusiness fileBusiness, ITableMetadataBusiness tableMetadataBusiness) : base(serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _fileBusiness = fileBusiness;
            _tableMetadataBusiness = tableMetadataBusiness;
            _accessor = accessor;

        }
        [HttpGet]
        [Route("Test")]
        public async Task<IActionResult> Test()
        {
            try
            {
                await Authenticate("45bba746-3309-49b7-9c03-b5793369d73c");
                var uc = _serviceProvider.GetService<IUserContext>();
                var columns = new IdNameViewModel { Id = "df", Name = "sd" };
                return Ok(columns);
            }
            catch (Exception)
            {

                throw;
            }

        }
        [HttpGet]
        [Route("Test2")]
        public async Task<IActionResult> Test2()
        {
            var j = _accessor.HttpContext;
            return Ok("test");
        }
        [HttpGet]
        [Route("TableData")]
        public async Task<IActionResult> TableData(string tableName, string columns = null, string filter = null, string orderbyColumns = null, string orderBy = null, string filterKey = null, string filterValue = null)
        {
            try
            {
                //var where = "";
                var split = tableName.Split('.');
                if (split.Length != 2)
                {
                    throw new ArgumentException("Invalid table name");
                }
                //if (filterKey.IsNotNullAndNotEmpty() && filterValue.IsNotNullAndNotEmpty())
                //{
                //    where = $@" and ""{split[1]}"".""{filterKey}""='{filterValue}'";
                //}
                var cmsBusiness = _serviceProvider.GetService<ICmsBusiness>();
                var data = await cmsBusiness.GetData(split[0], split[1], columns, filter, orderbyColumns, OrderByEnum.Ascending);
                if (filterKey.IsNotNullAndNotEmpty() && filterValue.IsNotNullAndNotEmpty())
                {
                    data = data.AsEnumerable().Where(row => row.Field<String>(filterKey) == filterValue).CopyToDataTable();
                }

                return Ok(data);
            }
            catch (Exception)
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
                var columns = await columnMetadataBusiness.GetViewableColumnMetadataList(split[0], split[1], true);
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
        public async Task<IActionResult> GetLOVIdNameList(string lovType, string filterKey = null, string filterValue = null)
        {
            try
            {
                var lovBusiness = _serviceProvider.GetService<ILOVBusiness>();
                var list = await lovBusiness.GetList(x => x.LOVType == lovType);

                var result = list.Select(x => new IdNameViewModel { Id = x.Id, Name = x.Name, Code = x.Code }).ToList();
                if (filterKey == "Id")
                {
                    result = result.Where(x => x.Id == filterValue).ToList();
                }
                else if (filterKey == "Code")
                {
                    result = result.Where(x => x.Code == filterValue).ToList();
                }
                else if (filterKey == "Name")
                {
                    result = result.Where(x => x.Name == filterValue).ToList();
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
        public async Task<IActionResult> GetEnumIdNameList(string enumType, string exculdeItem1 = "", string exculdeItem2 = "", string exculdeItem3 = "", string filterKey = null, string filterValue = null)
        {
            try
            {

                var list = new List<IdNameViewModel>();
                var t = string.Concat("CMS.Common.", enumType, ", CMS.Common");
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
                    if (filterKey == "Id")
                    {
                        list = list.Where(x => x.Id == filterValue).ToList();
                    }
                    else if (filterKey == "EnumId")
                    {
                        list = list.Where(x => x.EnumId == filterValue.ToSafeInt()).ToList();
                    }
                    else if (filterKey == "Name")
                    {
                        list = list.Where(x => x.Name == filterValue).ToList();
                    }
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
                        return Ok(new { success = true, fileId = result.Item.Id, fileName = result, data = result.Item });

                    }
                    else
                    {
                        return Ok(new { success = false });
                    }
                }
            }
            catch (Exception ex)
            {
                return Ok(new { success = false });
            }
            return Ok(new { success = false });
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
        [Route("GetOwnerIdNameList")]
        public async Task<ActionResult> GetOwnerIdNameList(string userId)
        {
            var _business = _serviceProvider.GetService<ITaskBusiness>();
            TaskSearchViewModel search = new TaskSearchViewModel();
            search.UserId = userId;
            var viewModel = await _business.GetSearchResult(search);
            var list1 = viewModel.GroupBy(x => x.OwnerUserName).Select(group => new { Id = group.Select(x => x.OwnerUserId).FirstOrDefault(), Name = group.Select(x => x.OwnerUserName).FirstOrDefault() }).ToList();
            var list = list1.Select(x => new TaskSearchViewModel { Id = x.Id, OwnerName = x.Name }).ToList();
            return Ok(list);
        }

        [HttpGet]
        [Route("GetUserIdNameList")]
        public async Task<ActionResult> GetUserIdNameList()
        {
            var _business = _serviceProvider.GetService<IUserBusiness>();
            var data = await _business.GetUserIdNameList();
            return Ok(data);
        }

        [HttpGet]
        [Route("GetEnumAsTreeList")]
        public ActionResult GetEnumAsTreeList(string id, string enumType)
        {

            var list = new List<TreeViewViewModel>();
            if (id.IsNullOrEmpty())
            {

                list.Add(new TreeViewViewModel
                {
                    id = "Root",
                    Name = "All",
                    DisplayName = "All",
                    ParentId = null,
                    hasChildren = true,
                    expanded = true,
                    Type = "Root"

                });
            }
            if (id == "Root")
            {
                var t = string.Concat("CMS.Common.", enumType, ", CMS.Common");
                Type type = Type.GetType(t);
                if (type.IsEnum)
                {
                    list = Enum.GetValues(type)
                   .Cast<Enum>()
                    //.Where(i => i.ToString() != exculdeItem1 && i.ToString() != exculdeItem2)
                   .Select(e => new TreeViewViewModel()
                   {
                       id = Enum.GetName(type, e),
                       Name = e.Description(),
                       DisplayName = e.Description(),
                       ParentId = id,
                       hasChildren = false,
                       expanded = false,
                       Type = "Child"
                       //Name = e.Description(),
                       //EnumId = Convert.ToInt32(e),
                       //Id = Enum.GetName(type, e)
                   })
                   .ToList();
                    //var enumList = EnumExtension.SelectListFor(typeof(type));
                    //if (enumList != null && enumList.Count() > 0)
                    //{
                    //    list.AddRange(enumList.Select(x => new TreeViewViewModel
                    //    {
                    //        id = x.Value.ToString(),
                    //        Name = x.Text.ToString(),
                    //        DisplayName = x.Text.ToString(),
                    //        ParentId = id,
                    //        hasChildren = false,
                    //        expanded = false,
                    //        Type = "Child"

                    //    }));
                    //}

                }


            }
            return Ok(list.ToList());
        }

        [HttpGet]
        [Route("GetModuleTreeList")]
        public async Task<ActionResult> GetModuleTreeList(string id, string moduleCodes, string portalName, string userId)
        {
            await Authenticate(userId, portalName);
            var _context = _serviceProvider.GetService<IUserContext>();
            var _business = _serviceProvider.GetService<IModuleBusiness>();
            var list = new List<TreeViewViewModel>();
            if (id.IsNullOrEmpty())
            {

                list.Add(new TreeViewViewModel
                {
                    id = "Root",
                    Name = "All",
                    DisplayName = "All",
                    ParentId = null,
                    hasChildren = true,
                    expanded = true,
                    Type = "Root"

                });
            }
            if (id == "Root")
            {
                var moduleslist = await _business.GetList(x => x.PortalId == _context.PortalId);

                if (moduleCodes.IsNotNullAndNotEmpty())
                {
                    var modules = moduleCodes.Split(",");
                    moduleslist = moduleslist.Where(x => x.Code != null && modules.Any(y => y == x.Code)).ToList();

                }
                list = moduleslist.Select(e => new TreeViewViewModel()
                {
                    id = e.Id,
                    Name = e.Name,
                    DisplayName = e.Name,
                    ParentId = id,
                    hasChildren = false,
                    expanded = false,
                    Type = "Child"

                })
                   .ToList();

            }
            return Ok(list.ToList());
        }

        [HttpGet]
        [Route("ReadComponentResultData")]
        public async Task<ActionResult> ReadComponentResultData(string serviceId)
        {

            var _componentResultBusiness = _serviceProvider.GetService<IComponentResultBusiness>();
            var model = await _componentResultBusiness.GetComponentResultList(serviceId);
            return Ok(model);
        }

        [HttpGet]
        [Route("ReadTaskSharedData")]
        public async Task<ActionResult> ReadTaskSharedData(string taskId)
        {
            var _ntsTaskSharedBusiness = _serviceProvider.GetService<INtsTaskSharedBusiness>();
            var result = await _ntsTaskSharedBusiness.GetSearchResult(taskId);
            return Ok(result);
        }

        [HttpGet]
        [Route("ReadNoteSharedData")]
        public async Task<ActionResult> ReadNoteSharedData(string noteId)
        {
            var _ntsNoteSharedBusiness = _serviceProvider.GetService<INtsNoteSharedBusiness>();
            var result = await _ntsNoteSharedBusiness.GetSearchResult(noteId);
            return Ok(result);
        }

        [HttpGet]
        [Route("ReadServiceSharedData")]
        public async Task<ActionResult> ReadServiceSharedData(string serviceId)
        {
            var _ntsServiceSharedBusiness = _serviceProvider.GetService<INtsServiceSharedBusiness>();
            var result = await _ntsServiceSharedBusiness.GetSearchResult(serviceId);
            return Ok(result);
        }

        [HttpGet]
        [Route("DeleteTaskShared")]
        public async Task<ActionResult> DeleteTaskShared(string id)
        {
            var _ntsTaskSharedBusiness = _serviceProvider.GetService<INtsTaskSharedBusiness>();
            await _ntsTaskSharedBusiness.Delete(id);
            return Ok(new { success = true });

        }

        [HttpGet]
        [Route("DeleteNoteShared")]
        public async Task<ActionResult> DeleteNoteShared(string id)
        {
            var _ntsNoteSharedBusiness = _serviceProvider.GetService<INtsNoteSharedBusiness>();
            await _ntsNoteSharedBusiness.Delete(id);
            return Ok(new { success = true });

        }

        [HttpGet]
        [Route("DeleteServiceShared")]
        public async Task<ActionResult> DeleteServiceShared(string id)
        {
            var _ntsServiceSharedBusiness = _serviceProvider.GetService<INtsServiceSharedBusiness>();
            await _ntsServiceSharedBusiness.Delete(id);
            return Ok(new { success = true });

        }

        [HttpGet]
        [Route("ReadTeamData")]
        public async Task<ActionResult> ReadTeamData()
        {
            var _teamBusiness = _serviceProvider.GetService<ITeamBusiness>();
            var model = await _teamBusiness.GetList();
            var data = model.ToList();
            return Ok(data);
        }

        [HttpGet]
        [Route("LoadServiceAdhocTaskData")]
        public async Task<IActionResult> LoadServiceAdhocTaskData(string adhocTaskTemplateIds, string serviceId)
        {
            var _taskBusiness = _serviceProvider.GetService<ITaskBusiness>();
            var data = await _taskBusiness.GetServiceAdhocTaskGridData(null, adhocTaskTemplateIds, serviceId);
            return Ok(data);
        }

        [HttpGet]
        [Route("GetTaskAttachmentList")]
        public async Task<ActionResult> GetTaskAttachmentList(string taskId)
        {
            var list = await _fileBusiness.GetList(x => x.ReferenceTypeId == taskId && x.ReferenceTypeCode == ReferenceTypeEnum.NTS_Task);

            return Ok(list);
        }

        [HttpGet]
        [Route("GetNoteAttachmentList")]
        public async Task<ActionResult> GetNoteAttachmentList(string noteId)
        {
            var list = await _fileBusiness.GetList(x => x.ReferenceTypeId == noteId && x.ReferenceTypeCode == ReferenceTypeEnum.NTS_Note);

            return Ok(list);
        }

        [HttpGet]
        [Route("GetServiceAttachmentList")]
        public async Task<ActionResult> GetServiceAttachmentList(string serviceId)
        {
            var list = await _fileBusiness.GetList(x => x.ReferenceTypeId == serviceId && x.ReferenceTypeCode == ReferenceTypeEnum.NTS_Service);

            return Ok(list);
        }

        [HttpGet]
        [Route("DeleteAttachment")]
        public async Task<ActionResult> DeleteAttachment(string Id)
        {
            await _fileBusiness.Delete(Id);
            return Ok(true);
        }
        [HttpGet]
        [Route("DeleteTaskComment")]
        public async Task<ActionResult> DeleteTaskComment(string Id)
        {
            var _ntsTaskCommentBusiness = _serviceProvider.GetService<INtsTaskCommentBusiness>();
            await _ntsTaskCommentBusiness.Delete(Id);
            return Ok(true);
        }
        [HttpGet]
        [Route("DeleteNoteComment")]
        public async Task<ActionResult> DeleteNoteComment(string Id)
        {
            var _ntsNoteCommentBusiness = _serviceProvider.GetService<INtsNoteCommentBusiness>();
            await _ntsNoteCommentBusiness.Delete(Id);
            return Ok(true);
        }
        [HttpGet]
        [Route("DeleteServiceComment")]
        public async Task<ActionResult> DeleteServiceComment(string Id)
        {
            var _ntsServiceCommentBusiness = _serviceProvider.GetService<INtsServiceCommentBusiness>();
            await _ntsServiceCommentBusiness.Delete(Id);
            return Ok(true);
        }

        [HttpGet]
        [Route("ReadNtsTagData")]
        public async Task<ActionResult> ReadNtsTagData(NtsTypeEnum NtsType, string NtsId)
        {
            var _ntsTagBusiness = _serviceProvider.GetService<INtsTagBusiness>();
            var model = await _ntsTagBusiness.GetNtsTagData(NtsType, NtsId);
            return Ok(model);
        }

        [HttpGet]
        [Route("ReadTagCategoriesWithTag")]
        public async Task<ActionResult> ReadTagCategoriesWithTag(string id, string TemplateId)
        {
            var data = new List<TagCategoryViewModel>();
            var _noteBusiness = _serviceProvider.GetService<INoteBusiness>();
            var categoriesList = await _noteBusiness.GetTagCategoryList(TemplateId);
            foreach (var category in categoriesList)
            {
                TagCategoryViewModel model = new TagCategoryViewModel();
                model.Id = category.Id;
                model.Name = category.Name;
                var tagslist = await _noteBusiness.GetTagListByCategoryId(category.Id);
                model.Tags = new List<TagCategoryViewModel>();
                foreach (var tag in tagslist)
                {
                    TagCategoryViewModel tagmodel = new TagCategoryViewModel();
                    tagmodel.Id = tag.Id;
                    tagmodel.Name = tag.Name;
                    tagmodel.ParentNoteId = category.Id;
                    tagmodel.HasChildren = false;
                    data.Add(tagmodel);
                }
                model.HasChildren = tagslist.Count() > 0 ? true : false;
                data.Add(model);
            }


            var result = data.Where(x => id.IsNotNullAndNotEmpty() ? x.ParentNoteId == id : x.ParentNoteId == null)
           .Select(item => new
           {
               id = item.Id,
               Name = item.Name,
               ParentId = item.ParentNoteId,
               hasChildren = item.HasChildren
           });
            return Ok(result);
        }


        [HttpGet]
        [Route("ReadEmailData")]
        public async Task<ActionResult> ReadEmailData()
        {
            var _taskTemplateBusiness = _serviceProvider.GetService<IRecTaskTemplateBusiness>();
            var model = await _taskTemplateBusiness.GetEmailSetting();

            return Ok(model);
        }

        [HttpGet]
        [Route("GetLegalEntityNameList")]
        public async Task<ActionResult> GetLegalEntityNameList()
        {
            var _legalEntityBusiness = _serviceProvider.GetService<ILegalEntityBusiness>();
            var data = await _legalEntityBusiness.GetList();

            return Ok(data);
        }
        [HttpGet]
        [Route("GetLogstashSyncDetails")]
        public async Task<ActionResult> GetLogstashSyncDetails()
        {
            var _noteBusiness = _serviceProvider.GetService<INoteBusiness>();
            var data = await _noteBusiness.GetScheduleSyncData();
            return Ok(data);
        }
    }
}
