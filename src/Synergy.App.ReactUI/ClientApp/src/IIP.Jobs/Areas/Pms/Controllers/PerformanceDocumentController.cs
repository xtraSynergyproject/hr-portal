using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.ViewModel;
using Synergy.App.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Synergy.App.Api.Areas.Pms.Controllers
{
    [Route("pms/PerformanceDocument")]
    [ApiController]
    public class PerformanceDocumentController : ApiController
    {
        private readonly ITaskBusiness _taskBusiness;
        private readonly IUserRoleBusiness _userRoleBusiness;
        private readonly IServiceBusiness _serviceBusiness;
        private readonly IPerformanceManagementBusiness _pmtBusiness;
        private readonly IUserHierarchyBusiness _userHierarchyBusiness;
        private readonly IHRCoreBusiness _hRCoreBusiness;
        ICmsBusiness _cmsBusiness;
        private AuthSignInManager<ApplicationIdentityUser> _customUserManager;
        private readonly IServiceProvider _serviceProvider;
        public PerformanceDocumentController(AuthSignInManager<ApplicationIdentityUser> customUserManager, IServiceProvider serviceProvider,
            IPerformanceManagementBusiness performanceManagementBusiness, IUserHierarchyBusiness userHierarchyBusiness,
            ITaskBusiness taskBusiness, IUserRoleBusiness userRoleBusiness, IServiceBusiness serviceBusiness,ICmsBusiness cmsBusiness,
            IHRCoreBusiness hRCoreBusiness
            ) : base(serviceProvider)
        {
            _customUserManager = customUserManager;
            _serviceProvider = serviceProvider;
            _pmtBusiness = performanceManagementBusiness;
            _userRoleBusiness = userRoleBusiness;
            _taskBusiness = taskBusiness;
            _serviceBusiness = serviceBusiness;
            _userHierarchyBusiness = userHierarchyBusiness;
            _cmsBusiness= cmsBusiness;
            _hRCoreBusiness = hRCoreBusiness;
        }

        #region Subordinate Objective
        
        [HttpGet]
        [Route("GetPerDocSubordinatesIdNameList")]
        public async Task<IActionResult> GetPerDocSubordinatesIdNameList(string userId,string performanceId)
        {

            await Authenticate(userId, "PerformanceManagement");
            var _userContext = _serviceProvider.GetService<IUserContext>();
            var perDocUsers = await _pmtBusiness.GetPerDocMasMappedUserData(performanceId);

            if (_userContext.UserRoleCodes.Contains("ADMIN"))
            {
                perDocUsers.Insert(0, new IdNameViewModel() { Id = "", Name = "--All--" });
                return Ok(perDocUsers);
            }
            else
            {
                IList<IdNameViewModel> userList = new List<IdNameViewModel>();
                var subordinate = await _userHierarchyBusiness.GetHierarchyUsers("PERFORMANCE_HIERARCHY", _userContext.UserId, 1, 1);
                userList = subordinate.Select(x => new IdNameViewModel { Id = x.Id, Name = x.Name }).ToList();

                IList<IdNameViewModel> CommonList = perDocUsers.Where(p => userList.Any(p2 => p2.Id == p.Id)).ToList();

                CommonList.Insert(0, new IdNameViewModel() { Id = _userContext.UserId, Name = _userContext.Name });

                return Ok(CommonList);
            }
        }

        #endregion

        #region Performance Master

        [HttpGet]
        [Route("ReadPerformanceDocumentData")]
        public async Task<ActionResult> ReadPerformanceDocumentData(PerformanceDocumentViewModel search = null)
        {
            var where = $@" order by ""N_PerformanceDocumentMaster_PerformanceDocumentMaster"".""StartDate"" desc";
            var model = await _cmsBusiness.GetDataListByTemplate("PERFORMANCE_DOCUMENT_MASTER", "", where);
            return Ok(model);
        }

        [HttpGet]
        [Route("CreatePDM")]
        public async Task<IActionResult> CreatePDM(string noteId)
        {
            var model = new PerformanceDocumentViewModel();
            if (noteId.IsNotNullAndNotEmpty())
            {
                model = await _pmtBusiness.GetPerformanceDocumentDetails(noteId);
                model.DataAction = DataActionEnum.Edit;
            }
            else
            {
                model.DataAction = DataActionEnum.Create;
                model.DocumentStatus = PerformanceDocumentStatusEnum.Draft;
            }
            return Ok(model);
        }

        [HttpGet]
        [Route("ReadPerformanceRatingList")]
        public async Task<ActionResult> ReadPerformanceRatingList()
        {
            var list = await _pmtBusiness.GetPerformanceRatingsList();
            return Ok(list);
        }

        [HttpGet]
        [Route("ReadDepartmentList")]
        public async Task<ActionResult> ReadDepartmentList()
        {
            var list = await _pmtBusiness.GetDepartmentList();
            return Ok(list);
        }


        
        [HttpPost]
        [Route("ManagePerformanceDocument")]
        public async Task<IActionResult> ManagePerformanceDocument(PerformanceDocumentViewModel model)
        {
            if (model.DataAction == DataActionEnum.Create)
            {
                var result = await _pmtBusiness.CreatePerDoc(model);
                if (result.IsSuccess)
                {
                    return Ok(new { success = true });
                }
                else
                {
                    return Ok(new { success = false, error = result.HtmlError });
                }
            }
            else
            {
                var result = await _pmtBusiness.EditPerDoc(model);
                if (result.IsSuccess)
                {
                    return Ok(new { success = true });
                }
                else
                {
                    return Ok(new { success = false, error = result.HtmlError });
                }
            }
        }


        //Manage User Mapping
        [HttpGet]
        [Route("ReadMappedUserPerformanceDocumentData")]
        public async Task<ActionResult> ReadMappedUserPerformanceDocumentData(string PmDoc)
        {
            var model = await _pmtBusiness.GetPerformanceDocumentMappedUserData(PmDoc, null);
            return Ok(model);
        }

        [HttpGet]
        [Route("ReadUsers")]
        public async Task<ActionResult> ReadUsers(string deptId = null)
        {
            var model = await _hRCoreBusiness.GetUsersInfo(deptId);
            return Ok(model.OrderBy(x => x.PersonFullName));
        }

        //Manage Grade Rating

        [HttpGet]
        [Route("ReadPerformanceGradeRatingData")]
        public async Task<ActionResult> ReadPerformanceGradeRatingData(string ParentNoteId)
        {
            var model = await _pmtBusiness.GetPerformanceGradeRatingData(ParentNoteId);
            return Ok(model);
        }

        [HttpGet]
        [Route("ReadGradeList")]
        public async Task<ActionResult> ReadGradeList()
        {
            var list = await _cmsBusiness.GetDataListByTemplate("HRGrade", "");
            return Ok(list);
        }

        [HttpGet]
        [Route("ReadPerformanceGradeRatingList")]
        public async Task<ActionResult> ReadPerformanceGradeRatingList(string perRatingId)
        {
            var list = await _pmtBusiness.GetPerformanceGradeRatingList(perRatingId);
            return Ok(list);
        }

        [HttpGet]
        [Route("GetPerformanceDocumentsList")]
        public async Task<ActionResult> GetPerformanceDocumentsList()
        {
            var list = await _pmtBusiness.GetPerformanceDocumentsList();
            return Ok(list);
        }

        [HttpGet]
        [Route("ReadPerformanceDocumentStageData")]
        public async Task<ActionResult> ReadPerformanceDocumentStageData(string ParentNoteId)
        {
            //var model = await _cmsBusiness.GetDataListByTemplate("PERFORMACE_DOCUMENT_MASTER_STAGE", "");
            var model = await _pmtBusiness.GetPerformanceDocumentStageData(ParentNoteId);
            return Ok(model);
        }
        //Publish Document

        [HttpGet]
        [Route("PublishPerformanceDocument")]
        public async Task<IActionResult> PublishPerformanceDocument(string pdmId, string status)
        {
            var model = new PerformanceDocumentViewModel();
            model = await _pmtBusiness.GetPerformanceDocumentDetails(pdmId);
            model.DataAction = DataActionEnum.Create;
            return Ok(model);
        }

        [HttpPost]
        [Route("ManagePublishPerformanceDocument")]
        public async Task<IActionResult> ManagePublishPerformanceDocument(PerformanceDocumentViewModel model)
        {
            //var result = await _pmtBusiness.GeneratePerformanceDocument(model.Id);
            if (model.DocumentStatus == PerformanceDocumentStatusEnum.Draft || model.DocumentStatus == PerformanceDocumentStatusEnum.Active)
            {
                var result = await _pmtBusiness.PublishDocumentMaster(model.NoteId);
                if (result.IsSuccess)
                {
                    return Ok(new { success = true });
                }
            }


            return Ok(new { success = false, message = "Cannot Publish Document" });

        }
        #endregion


        
    }
}
