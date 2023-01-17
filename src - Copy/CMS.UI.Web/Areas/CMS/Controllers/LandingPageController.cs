using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ActiveQueryBuilder.Core;
using ActiveQueryBuilder.Web.Server;
using ActiveQueryBuilder.Web.Server.Services;
using AutoMapper;
using CMS.Business;
using CMS.Common;
using CMS.UI.Utility;
using CMS.UI.ViewModel;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;


namespace CMS.UI.Web.Areas.CMS.Controllers
{
    [Area("Cms")]
    public class LandingPageController : ApplicationController
    {

        private IMapper _autoMapper;
        private readonly IRazorViewEngine _razorViewEngine;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly ITeamBusiness _teamBusiness;
        private readonly IHRCoreBusiness _hrCoreBusiness;
        private readonly IUserBusiness _userBusiness;
        private readonly IServiceBusiness _serviceBusiness;
        private readonly INoteBusiness _noteBusiness;
        private readonly IUserContext _userContext;
        private readonly ILOVBusiness _lovBusiness;
        private INtsNoteCommentBusiness _ntsNoteCommentBusiness;
        private IFileBusiness _fileBusiness;
        private readonly INtsNoteSharedBusiness _ntsNoteSharedBusiness;
        //private IAttendanceBusiness _attendanceBusiness;
        public LandingPageController(IUserContext userContext,
             IServiceBusiness serviceBusiness, IUserBusiness userBusiness,
             IHRCoreBusiness hrCoreBusiness
            , INoteBusiness noteBusiness
            , ITeamBusiness teamBusiness
            , IMapper autoMapper
            , IRazorViewEngine razorViewEngine,
            ITempDataProvider tempDataProvider
            , IHttpContextAccessor contextAccessor, ILOVBusiness lovBusiness
            , INtsNoteCommentBusiness ntsNoteCommentBusiness
            , IFileBusiness fileBusiness
            , INtsNoteSharedBusiness ntsNoteSharedBusiness
            //, IAttendanceBusiness attendanceBusiness
            )
        {    
            _userContext = userContext;           
            _serviceBusiness = serviceBusiness;
            _userBusiness = userBusiness;
            _hrCoreBusiness = hrCoreBusiness;
            _noteBusiness = noteBusiness;
            _teamBusiness = teamBusiness;
            _autoMapper = autoMapper;
            _lovBusiness = lovBusiness;
            _tempDataProvider = tempDataProvider; 
            _razorViewEngine = razorViewEngine;
            _contextAccessor = contextAccessor;
            _ntsNoteCommentBusiness = ntsNoteCommentBusiness;
            _fileBusiness = fileBusiness;
            _ntsNoteSharedBusiness = ntsNoteSharedBusiness;
            // _attendanceBusiness = attendanceBusiness;
        }
      

        public async Task<ActionResult> ManagePost()
        {
            //await _attendanceBusiness.UpdateAttendanceTable(DateTime.Now);
            ViewBag.Title = "Dashboard";// "Dashboard";
            var LoggedInUser = await _userBusiness.GetSingleById(_userContext.UserId);
            var fbDashboard =await _serviceBusiness.GetFBDashboardCount(_userContext.UserId);
            fbDashboard.OwnerDisplayName = LoggedInUser.Name;
          
            var companyOrg = await _hrCoreBusiness.GetCompanyOrganization(_userContext.UserId);
            fbDashboard.DepartmentId = companyOrg != null ? (companyOrg.Id ?? "") : "";
            fbDashboard.DataAction = DataActionEnum.Read;
            ViewBag.SilderBanner = await _hrCoreBusiness.GetSliderBannerData();
            //fbDashboard.PositionId = LoggedInUserPositionId ?? 0;
            return View("ManageLandingPage",fbDashboard);
        }
        public async Task<ActionResult> CreatePost(string noteId, string templateMasterId, string templateMasterCode = null, string userId = null, string layoutMode = null, NoteReferenceTypeEnum tagtotype = NoteReferenceTypeEnum.Self, AssignToTypeEnum? ownerType = null, string teamId=null, string orgId=null, string sourcePost = "COMPANY", ModuleEnum? moduleName = null, bool isUserGuide = false, bool isHelp = false)
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
               // LayoutMode = layoutMode.IsNullOrEmpty() ? LayoutModeEnum.Main : layoutMode.ToEnum<LayoutModeEnum>(),
                                             
                //TeamId = teamId,
                //OrganizationId = orgId,
                //ReferenceTo = teamId,
                //SendNotification = true,
                //ModuleName = moduleName,
                //IsUserGuide = isUserGuide,
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
            //newmodel.SharingMode = SharingModeEnum.System;
            //newmodel.OwnerDisplayName = LoggedInAsByUserName;

            viewModel.EnableBroadcast = true;
          
            viewModel.SourcePost = sourcePost;
            viewModel.IsUserGuide = isUserGuide;
            viewModel.StartDate = DateTime.Today;
            ViewBag.IsHelp = isHelp;
            return View("_CreatePost", viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> ManagePost(PostMessageViewModel model)
        {
            if (model.DataAction == DataActionEnum.Create)
            {
               if(model.IsUserGuide == true && model.SequenceOrder.IsNotNull())
               {
                    var exist = await _hrCoreBusiness.ValidatePostMsgSequenceNo(model);
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

        public async Task<ActionResult>  ReadAnnouncementBusinessData(string type = "Org", string orgId=null)
        {
            var orgList = new List<string>();
            if (type == "Self")
            {
                List<string> list = new List<string>();
                list.Add(_userContext.OrganizationId);
                orgList = await _hrCoreBusiness.GetParentOrganizationReportingList(_userContext.OrganizationId,list);
                var company = await _hrCoreBusiness.GetCompanyOrganization(_userContext.UserId);
                orgList.Add(company != null ? company.Id ?? "" : "");
                orgList.Add(_userContext.CompanyId);
            }
            else if (type == "Company")
            {
                var company = await _hrCoreBusiness.GetCompanyOrganization(_userContext.UserId);
                //orgList.Add(company != null ? company.Id ?? "" : "");
                orgList.Add(orgId);
            }
            else
            {
                orgList.Add(_userContext.OrganizationId);
            }

            var result =await _hrCoreBusiness.GetAnnouncements(orgList);
            //var j = Json(result.ToDataSourceResult(request));
            var j = Json(result);
            return j;
        }
        [HttpGet]
        public async Task<string> ReadManagePost(EndlessScrollingRequest request)
        {
            if (_userContext.UserId.IsNullOrEmpty())
            {
                request.UserId = _userContext.UserId;
            }
            request.LoggendInUserId = _userContext.UserId;
            var list = await _hrCoreBusiness.GetGroupMessage(request);
            var distinct = list.GroupBy(x => x.Id)
                                  .Select(g => g.First())
                                  .ToList();
            if (request.ModuleName != null && request.HomeType == "UserGuide")
            {
                distinct = distinct.Where(x => x.ModuleName == request.ModuleName).ToList();
            }
            var result = new StringBuilder();
            foreach (var item in distinct)
            {
                var data = await _ntsNoteCommentBusiness.GetSearchResult(item.NoteId);
                var sharedata = await _ntsNoteSharedBusiness.GetSearchResult(item.NoteId);
                var likes = await _hrCoreBusiness.GetLikeCount(item.NoteId);
                var Iliked = await _hrCoreBusiness.GetILikeCount(item.NoteId, _userContext.UserId);
                item.CommentsCount = data.Count();
                item.ShareCount = sharedata.Count();
                item.ILiked = Iliked;
                item.LikesUserCount = likes;
                item.ActiveUserId = _userContext.UserId;
                item.AttachmentList = await _fileBusiness.GetList(x => x.ReferenceTypeId == item.NoteId && x.ReferenceTypeCode == ReferenceTypeEnum.NTS_Note);
                foreach (var file in item.AttachmentList)
                {
                    var doc = await _fileBusiness.GetFileByte(file.Id);
                    if (doc != null)
                    {
                        file.ContentByte = doc;
                        file.ContentBase64 = Convert.ToBase64String(doc, 0, doc.Length);
                    }
                }
                //result.Append(ConvertViewToString("_EditPost", item));
                var viewStr = await RenderViewToStringAsync("_EditPost", item, _contextAccessor, _razorViewEngine, _tempDataProvider);
                result.Append(viewStr);
            }
            ////for (int i = 0; i < 100; i++)
            ////{
            ////    var id = (request.PageSize * (request.PageNo - 1)) + i + 1;

            ////    result.Append(ConvertViewToString("_ManagePostItem", new ScreenViewModel { Id = id, Name = string.Concat("Name", id) }));
            ////}
            return result.ToString();
        }
        public async Task<ActionResult> LoadManagePostPartialView(string homeType = "", string userId=null, String module = null)
        {
            var userdetails = await _userBusiness.GetSingleById(_userContext.UserId);
            var companyOrg = await _hrCoreBusiness.GetCompanyOrganization(_userContext.UserId);
            var model = new EndlessScrollListViewModel { Name = "manageNoteList", UrlActionName = "ReadManagePost", UrlControlName = "LandingPage", UrlAreaName = "Cms", PageSize = 10, OrgId = companyOrg.Id, HomeType = homeType, UserId = _userContext.UserId, ModuleName = module != null ? (EnumExtension.ToEnum<ModuleEnum>(module)) : ModuleEnum.Admin };
            return PartialView("_EndlessScrollList", model);
        }
        public async Task<ActionResult> OrgHome()
        {
            var LoggedInUser = await _userBusiness.GetSingleById(_userContext.UserId);           
            var fbDashboard = await _serviceBusiness.GetFBDashboardCount(_userContext.UserId);
            fbDashboard.OwnerDisplayName = LoggedInUser.Name;

            fbDashboard.DataAction = DataActionEnum.Read;
            fbDashboard.PositionId = _userContext.PositionId ?? "";
            fbDashboard.DepartmentId = _userContext.OrganizationId;
            var dept=await _hrCoreBusiness.GetDepartmentNameById(_userContext.OrganizationId);
            fbDashboard.DepartmentName = dept.Name.IsNotNullAndNotEmpty() ? dept.Name : "No Organization";



            return View(fbDashboard);
        }
        public async Task<ActionResult> EditPost(string noteId = null, string templateMasterId = null, string templateMasterCode = null, long? userId = null, string rtc = null, long? rid = null, string rtn = null, long? prid = null, string ru = null, string layoutMode = null, string templateCode = null, NoteReferenceTypeEnum? tagtotype = null, long? tagtoid = null, long? versionId = null)
        {
            //   var model = new NoteSearchViewModel { Operation = DataOperation.Read, NoteStatus = noteStatus, Mode = mode };
            ViewBag.Title = "IndexTitle";// "Note List";
            //  _business.SetAllFieldPermissions(model, 15);
            var model = new NoteTemplateViewModel();
            //var model = new NoteViewModel
            //{
            //    TemplateMasterCode = templateMasterCode,
            //    TemplateMasterId = templateMasterId,
            //    Id = noteId,
            //    Operation = noteId == 0 ? DataOperation.Create : DataOperation.Read,
            //    OwnerUserId = userId ?? 0,
            //    ActiveUserId = LoggedInUserId,
            //    NoteVersionId = versionId,
            //    RequestedByUserId = LoggedInUserId,
            //    ReferenceTypeCode = rtc.IsNullOrEmpty() ? default(ReferenceTypeEnum?) : rtc.ToEnum<ReferenceTypeEnum>(),
            //    ReferenceId = rid,
            //    ReferenceNode = rtn.IsNullOrEmpty() ? default(NodeEnum?) : rtn.ToEnum<NodeEnum>(),
            //    ReturnUrl = ru,
            //    LayoutMode = layoutMode.IsNullOrEmpty() ? LayoutModeEnum.Main : layoutMode.ToEnum<LayoutModeEnum>(),
            //    ReferenceType = tagtotype,
            //    ReferenceTo = tagtoid

            //};
            var newmodel = await _noteBusiness.GetNoteDetails(model);
            return View("_EditPost", newmodel);
        }
        public async Task<string> ReadOrgManagePost(EndlessScrollingRequest request)
        {
            request.UserId = _userContext.UserId;
            request.OrgId = _userContext.OrganizationId;
            var list =await _hrCoreBusiness.GetOrgGroupMessage(request);
            var distinct = list/*.Where(x => x.IsUserGuide == false)*/.GroupBy(x => x.Id)
                                  .Select(g => g.First())
                                  .ToList();
            var result = new StringBuilder();
            foreach (var item in distinct)
            {
                var data= await _ntsNoteCommentBusiness.GetSearchResult(item.NoteId);
                var sharedata = await _ntsNoteSharedBusiness.GetSearchResult(item.NoteId);
                var likes = await _hrCoreBusiness.GetLikeCount(item.NoteId);
                var Iliked = await _hrCoreBusiness.GetILikeCount(item.NoteId,_userContext.UserId); 
                item.CommentsCount = data.Count();
                item.ShareCount = sharedata.Count();
                item.ILiked = Iliked;
                item.LikesUserCount = likes;
                item.ActiveUserId = _userContext.UserId;
                item.AttachmentList = await _fileBusiness.GetList(x => x.ReferenceTypeId == item.NoteId && x.ReferenceTypeCode == ReferenceTypeEnum.NTS_Note);
                foreach (var file in item.AttachmentList) 
                {
                    var doc = await _fileBusiness.GetFileByte(file.Id);
                    if (doc != null)
                    {
                        file.ContentByte = doc;
                        file.ContentBase64 = Convert.ToBase64String(doc, 0, doc.Length);
                    }
                }
                var viewStr = await RenderViewToStringAsync("_EditPost", item, _contextAccessor, _razorViewEngine, _tempDataProvider);
                result.Append(viewStr);
            }          
            return result.ToString();
        }
        public ActionResult GreetingAnnouncement(string orgId, string ru = "")
        {
            var model = new AnnouncementViewModel
            {
                
                StartDate = DateTime.Now.ApplicationNow(),
                ExpiryDate = DateTime.Now.ApplicationNow().AddDays(1),
                ReturnUrl = ru,
                OrgId = orgId,
                DataAction = DataActionEnum.Create,
            };
            return View(model);
        }
        [HttpPost]
        public async Task<ActionResult> GreetingAnnouncement(AnnouncementViewModel model)
        {

            var org = model.OrgId.IsNotNullAndNotEmpty() ? model.OrgId : _userContext.OrganizationId;
            var templateMasterId =await _hrCoreBusiness.GetAnnouncementTemplateMasterId();

            var note =await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel
            {
                //TemplateMasterCode = tempcode,
                NoteId = model.NoteId,
                TemplateId = templateMasterId,
                ActiveUserId = _userContext.UserId,
                RequestedByUserId = _userContext.UserId,
                OwnerUserId = _userContext.UserId,
                //Operation = DataOperation.Create,
                DataAction = model.DataAction,
                //ReferenceType = NoteReferenceTypeEnum.Organization,
               // ReferenceTo = org,
            });
            note.NoteDescription = model.NoteDescription;
            note.NoteSubject = model.NoteSubject;
            note.ExpiryDate = model.ExpiryDate;
            note.StartDate = model.StartDate;
            model.CreatedDate = Convert.ToDateTime(model.StartDate);
            model.ReferenceType = NoteReferenceTypeEnum.Organization;
            model.ReferenceId = org;
            model.EnableBroadcast = true;
            // model.IsNotifyByEmail = model.IsNotifyByEmail;
            note.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
            note.Json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
            
            //note.CSVFileIds = model.Attachment;            
            //note.SharingMode = SharingModeEnum.System;
            var result =await _noteBusiness.ManageNote(note);
            if (!result.IsSuccess)
            {
                result.Messages.Each(x => ModelState.AddModelError(x.Key, x.Value));
                return Json(new { success = false, errors = ModelState.SerializeErrors() });
            }
            else
            {
                return Json(new { success = true, operation = model.DataAction.ToString(), ru = model.ReturnUrl });
            }

        }

        public async Task<ActionResult> ReadGreetingAnnouncement(string noteId, string ru = "")
        {
            var noteModel = await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel
            { 
                NoteId = noteId
            });
            var announcement = await _hrCoreBusiness.GetAnnouncementByNoteId(noteId);
            //var isnotifybyemail = noteModel.Controls.FirstOrDefault(x => x.FieldName == "IsNotifyByEmail").Code;
            //if (isnotifybyemail == null)
            // isnotifybyemail = "false";
            var model = new AnnouncementViewModel
            {
                Subject = noteModel.Subject,
                Body = noteModel.Description,
                StartDate = noteModel.StartDate,
                ExpiryDate = noteModel.ExpiryDate,
                //IsNotifyByEmail=noteModel.SendNotification,
                IsNotifyByEmail = announcement.IsNotifyByEmail? announcement.IsNotifyByEmail:false,
                DataAction = DataActionEnum.Read,
                ReturnUrl = ru,
            };
            return View("GreetingAnnouncement", model);
        }

        public async Task<ActionResult> EditGreetingAnnouncement(string noteId, string ru = "")
        {
            var noteModel = await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel { NoteId = noteId });
            //var isnotifybyemail = noteModel.Controls.FirstOrDefault(x => x.FieldName == "IsNotifyByEmail").Code;
            // if (isnotifybyemail == null)
            //  isnotifybyemail = "false";
            var announcement = await _hrCoreBusiness.GetAnnouncementByNoteId(noteId);
            var model = new AnnouncementViewModel();
            model = _autoMapper.Map<NoteTemplateViewModel, AnnouncementViewModel>(noteModel, model);

            model.OrgId = announcement.ReferenceId;
            model.NoteId = noteModel.Id;
            model.NoteSubject = noteModel.NoteSubject;
            model.NoteDescription = noteModel.NoteDescription;
            model.StartDate = noteModel.StartDate;
            model.ExpiryDate = noteModel.ExpiryDate;
            //IsNotifyByEmail=noteModel.SendNotification,
            model.IsNotifyByEmail = announcement.IsNotifyByEmail ? announcement.IsNotifyByEmail : false;
            model.DataAction = DataActionEnum.Edit;
            model.ReturnUrl = ru;
            
            
            return View("GreetingAnnouncement", model);
        }
        public async Task<ActionResult>  EditHelpPost(string noteId, string templateMasterId, string templateMasterCode, string userId = null, string layoutMode = null, NoteReferenceTypeEnum tagtotype = NoteReferenceTypeEnum.Self, AssignToTypeEnum? ownerType = null, string teamId = null, string orgId = null, string sourcePost = "COMPANY", ModuleEnum? moduleName = null, bool isUserGuide = false, bool isHelp = false)
        {
            ModelState.Clear();
            var viewModel = new PostMessageViewModel();
            //var attachmentBusiness = DependencyResolver.Current.GetService<IAttachmentBusiness>();
            var model = new NoteTemplateViewModel
            {
                TemplateCode = templateMasterCode,
                TemplateId = templateMasterId,
                NoteId = noteId,
                DataAction = noteId.IsNullOrEmpty() ? DataActionEnum.Create : DataActionEnum.Read,
                OwnerUserId = userId ?? _userContext.UserId,
                ActiveUserId = _userContext.UserId,
                RequestedByUserId = _userContext.UserId,
                LayoutMode = layoutMode.IsNullOrEmpty() ? LayoutModeEnum.Main : layoutMode.ToEnum<LayoutModeEnum>(),
               
            };
            var newmodel = await _noteBusiness.GetNoteDetails(model);
            var post = await _hrCoreBusiness.GetGroupMessageByNoteId(noteId);
            viewModel = _autoMapper.Map<NoteTemplateViewModel, PostMessageViewModel>(newmodel, viewModel);
            if (orgId.IsNotNullAndNotEmpty())
            {
                viewModel.ReferenceTo = orgId;
                viewModel.ReferenceType = NoteReferenceTypeEnum.Organization;
                // model.OwnerType = AssignToTypeEnum.Organization;
            }


            if (teamId != null)
            {
                var team= await _teamBusiness.GetSingleById(teamId);
                viewModel.TeamName = team.Name;
            }           
            // newmodel.SharingMode = SharingModeEnum.System;
            var LoggedInUser = await _userBusiness.GetSingleById(_userContext.UserId);
            viewModel.OwnerUserName = LoggedInUser.Name;
            //newmodel.BroadcastType = orgId != 0 ? NoteReferenceTypeEnum.Organization : NoteReferenceTypeEnum.Team;
            viewModel.EnableBroadcast = true;
            viewModel.SourcePost = post.SourcePost;
            viewModel.ReferenceTo = post.ReferenceTo;
            viewModel.ReferenceType = post.ReferenceType;
            viewModel.IsUserGuide = isUserGuide;
            viewModel.StartDate = DateTime.Today;
            viewModel.SequenceOrder = post.SequenceOrder;
            ViewBag.IsHelp = isHelp;           
            viewModel.DataAction = DataActionEnum.Edit;
            //var param = Helper.GenerateCypherParameter("Id", newmodel.Id);
            //var attachlist = attachmentBusiness.GetAttachmentList(ReferenceTypeEnum.NTS_Note, "n.Id={Id}", param, "").Select(x => x.FileId);
            //if (attachlist != null && attachlist.Count() > 0)
            //{
            //    newmodel.CSVFileIds = string.Join(",", attachlist);
            //}
            return View("_CreatePost", viewModel);
        }
        public async Task<IActionResult> DeletePost(string noteId)
        {
            
            var post = await _hrCoreBusiness.GetGroupMessageByNoteId(noteId);
            if (post!=null) 
            {
                await _hrCoreBusiness.DeleteGroupPost(noteId);
                await _noteBusiness.Delete(noteId);
            }
            return Json(new { success = true });
        }

        public async Task<ActionResult> CompanyHome()
        {
            var fbDashboard = await _serviceBusiness.GetFBDashboardCount(_userContext.UserId);
            fbDashboard.OwnerDisplayName = _userContext.Name;
            //  if (fbDashboard.base64Img == "")
            //  {
            //      fbDashboard.base64Img = GenerateAvatar(LoggedInUserName);
            //  }
            fbDashboard.DataAction = DataActionEnum.Read;
            fbDashboard.PositionId = _userContext.PositionId ?? null;
            //var companyOrg = _userContext.CompanyId _orgBusiness.GetCompanyOrganization(LoggedInUserId);
            fbDashboard.DepartmentId = _userContext?.CompanyId ?? (_userContext.LegalEntityId ?? null);
            fbDashboard.DepartmentName = _userContext?.CompanyName ?? "N/A";
            return View(fbDashboard);
        }
        public async Task<string> ReadCompanyManagePost(EndlessScrollingRequest request)
        {
            request.UserId = _userContext.UserId;
            var list = await _hrCoreBusiness.GetCompanyGroupMessage(request);
            //var distinct = list.Where(x => x.IsUserGuide == false).GroupBy(x => x.Id)
            //                      .Select(g => g.First())
            //                      .ToList();
            var distinct = list.GroupBy(x => x.Id)
                      .Select(g => g.First()).OrderByDescending(x=>x.CreatedDate)
                      .ToList();
            var result = new StringBuilder();
            foreach (var item in distinct)
            {
                var data = await _ntsNoteCommentBusiness.GetSearchResult(item.NoteId);
                var sharedata = await _ntsNoteSharedBusiness.GetSearchResult(item.NoteId);
                var likes = await _hrCoreBusiness.GetLikeCount(item.NoteId);
                var Iliked = await _hrCoreBusiness.GetILikeCount(item.NoteId, _userContext.UserId);
                item.CommentsCount = data.Count();
                item.ShareCount = sharedata.Count();
                item.ILiked = Iliked;
                item.LikesUserCount = likes;
                item.ActiveUserId = _userContext.UserId;
                item.AttachmentList = await _fileBusiness.GetList(x => x.ReferenceTypeId == item.NoteId && x.ReferenceTypeCode == ReferenceTypeEnum.NTS_Note);
                foreach (var file in item.AttachmentList)
                {
                    var doc = await _fileBusiness.GetFileByte(file.Id);
                    if (doc != null)
                    {
                        file.ContentByte = doc;
                        file.ContentBase64 = Convert.ToBase64String(doc, 0, doc.Length);
                    }
                }
                //result.Append(ConvertViewToString("_EditPost", item));
                var viewStr = await RenderViewToStringAsync("_EditPost", item, _contextAccessor, _razorViewEngine, _tempDataProvider);
                result.Append(viewStr);

            }
            return result.ToString();
        }
        //public async Task<ActionResult> GetOrgDocument([DataSourceRequest] DataSourceRequest request, string orgId)
        //{
        //    var model = new NoteSearchViewModel { TagToType = NoteReferenceTypeEnum.Organization, TagTo = orgId, Type = "ORGANIZATION_DOCUMENT" };
        //    var result = await _hrCoreBusiness.GetNoteSummaryDetail(model);
        //    var j = Json(result.ToDataSourceResult(request));
        //    return j;
        //}
        public async Task<ActionResult> LikeUnlikeNote(string likeState, string Id)
        {
            if (likeState == "LIKE")
            {
                var note = await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel()
                {
                    TemplateCode = "PostLike",
                    RequestedByUserId = _userContext.UserId,
                    OwnerUserId = _userContext.UserId,
                    ActiveUserId= _userContext.UserId,
                    ParentNoteId = Id,
                    DataAction = DataActionEnum.Create
                }) ;
                note.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                var result = await _noteBusiness.ManageNote(note);
                var data = await _hrCoreBusiness.GetLikeCount(Id);
                if (result.IsSuccess) 
                {
                    return Json(new { success = true, LikeCount = data });
                }
                return Json(new { success = false, LikeCount = data });

            }
            else 
            {
                
                    await _hrCoreBusiness.UnlikePost(Id, _userContext.UserId);               
                var data = await _hrCoreBusiness.GetLikeCount(Id);                   
                return Json(new { success = true, LikeCount = data });
            }
            
            

        }
    }
}
