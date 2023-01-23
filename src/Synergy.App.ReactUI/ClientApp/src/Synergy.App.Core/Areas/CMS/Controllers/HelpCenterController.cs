using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Synergy.App.Core.Areas.CMS.Controllers
{
    [Area("Cms")]
    public class HelpCenterController : Controller
    {
        private IMapper _autoMapper;
        private readonly IUserContext _userContext;
        private readonly IHRCoreBusiness _hrCoreBusiness;
        private readonly IUserBusiness _userBusiness;
        private readonly IServiceBusiness _serviceBusiness;
        private readonly INoteBusiness _noteBusiness;
        private readonly ITeamBusiness _teamBusiness;
        private readonly INtsNoteUserReactionBusiness _ntsNoteUserReactionBusiness;
        public HelpCenterController(IUserContext userContext, IServiceBusiness serviceBusiness, IUserBusiness userBusiness,
             IHRCoreBusiness hrCoreBusiness, IMapper autoMapper, INoteBusiness noteBusiness, ITeamBusiness teamBusiness,
             INtsNoteUserReactionBusiness ntsNoteUserReactionBusiness)
        {
            _userContext = userContext;
            _serviceBusiness = serviceBusiness;
            _userBusiness = userBusiness;
            _hrCoreBusiness = hrCoreBusiness;
            _autoMapper = autoMapper;
            _noteBusiness = noteBusiness;
            _teamBusiness = teamBusiness;
            _ntsNoteUserReactionBusiness = ntsNoteUserReactionBusiness;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ReadUserGuideData()
        {
            var res = await _hrCoreBusiness.GetUserGuideData();
            return Json(res);
        }

        public async Task<IActionResult> NewPost(string noteId, string templateMasterId, string templateMasterCode = null, string userId = null, string layoutMode = null, NoteReferenceTypeEnum tagtotype = NoteReferenceTypeEnum.Self, AssignToTypeEnum? ownerType = null, string teamId = null, string orgId = null, string sourcePost = "COMPANY", ModuleEnum? moduleName = null, bool isUserGuide = false, bool isHelp = false)
        {
            var viewModel = new PostMessageViewModel();
            var model = new NoteTemplateViewModel
            {
                TemplateCode = templateMasterCode,
                TemplateId = templateMasterId,
                Id = noteId,
                DataAction = noteId.IsNullOrEmpty() ? DataActionEnum.Create : DataActionEnum.Read,
                OwnerUserId = userId ?? _userContext.UserId,
                ActiveUserId = _userContext.UserId,
                RequestedByUserId = _userContext.UserId,
            };
            var newmodel = await _noteBusiness.GetNoteDetails(model);
            viewModel = _autoMapper.Map<NoteTemplateViewModel, PostMessageViewModel>(newmodel, viewModel);
            if (viewModel.TeamId != null)
            {
                var team = await _teamBusiness.GetSingleById(viewModel.TeamId);
                viewModel.TeamName = team.Name;
            }
            viewModel.ReferenceTo = teamId;
            viewModel.ReferenceType = tagtotype;
            if (orgId.IsNotNullAndNotEmpty())
            {
                viewModel.ReferenceTo = orgId;
                viewModel.ReferenceType = NoteReferenceTypeEnum.Organization;

            }
            viewModel.EnableBroadcast = true;
            viewModel.SourcePost = sourcePost;
            viewModel.IsUserGuide = true;
            viewModel.StartDate = DateTime.Today;
            ViewBag.IsHelp = isHelp;
            return View(viewModel);
        }



        [HttpPost]
        public async Task<IActionResult> ManagePost(PostMessageViewModel model)
        {
            if (model.DataAction == DataActionEnum.Create)
            {
                if (model.IsUserGuide == true && model.SequenceOrder.IsNotNull())
                {
                    var exist = await _hrCoreBusiness.ValidateUserGuideSequenceNo(model);
                    if (exist)
                    {
                        return Json(new { success = false, error = "Sequence No already exist" });
                    }
                }
                model.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                model.NoteSubject = model.NoteDescription;
                // model.NoteDescription = model.NoteDescription;
                model.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                var result = await _noteBusiness.ManageNote(model);
                if (result.IsSuccess)
                {
                    return Json(new { success = true, note = result });
                }
                return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
            }
            if (model.DataAction == DataActionEnum.Edit)
            {
                if (model.IsUserGuide == true && model.SequenceOrder.IsNotNull())
                {
                    var exist = await _hrCoreBusiness.ValidatePostMsgSequenceNo(model);
                    if (exist)
                    {
                        return Json(new { success = false, error = "Sequence No already exist" });
                    }
                }

                model.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                model.NoteSubject = model.NoteDescription;
                //model.NoteDescription = model.NoteDescription;
                model.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                var result = await _noteBusiness.ManageNote(model);
                if (result.IsSuccess)
                {
                    return Json(new { success = true, note = result });
                }
                return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
            }
            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
        }

        public IActionResult LikedUser(string noteId)
        {
            var model = new NoteTemplateViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> LikeDislikePost(NtsNoteUserReactionViewModel model)
        {
            if (model.DataAction == DataActionEnum.Create)
            {
                var result = await _ntsNoteUserReactionBusiness.Create(model);
                if (result.IsSuccess)
                {
                    return Json(new { success = true, note = result });
                }
            }
            else
            {
                var result = await _ntsNoteUserReactionBusiness.Edit(model);
                if (result.IsSuccess)
                {
                    return Json(new { success = true, note = result });
                }
            }
            return Json(new { success = false, error = ModelState.SerializeErrors().ToHtmlError() });
        }
    }
}
