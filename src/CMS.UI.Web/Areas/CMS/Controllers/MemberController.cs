using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Synergy.App.Business;
using Synergy.App.Common;
using Synergy.App.WebUtility;
using Synergy.App.ViewModel;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace CMS.UI.Web.Areas.CMS.Controllers
{
    [Area("Cms")]
    public class MemberController : ApplicationController
    {
        private readonly IMemberBusiness _memberBusiness;
        private readonly IPortalBusiness _portalBusiness;
        private readonly IMemberGroupBusiness _groupBusiness;
        private readonly IConfiguration _configuration;
        public MemberController(IMemberBusiness memberBusiness, IPortalBusiness portalBusiness, IMemberGroupBusiness groupBusiness, IConfiguration configuration)
        {
            _memberBusiness = memberBusiness;
            _portalBusiness = portalBusiness;
            _groupBusiness = groupBusiness;
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Memberlist()
        {
            return View();
        }
        public IActionResult CreateMember()
        {
            return View("ManageMember", new MemberViewModel
            {
                DataAction = DataActionEnum.Create,
                Id = MongoDB.Bson.ObjectId.GenerateNewId().ToString(),
            });
        }
        public async Task<IActionResult> EditMember(string Id)
        {
            var member = await _memberBusiness.GetSingleById(Id);

            if (member != null)
            {
                //if (member.MemberStatus == StatusEnum.Active)
                //{
                //    member.Status = true;
                //}
                member.DataAction = DataActionEnum.Edit;
                return View("ManageMember", member);
            }
            return View("ManageMember", new MemberViewModel());
        }
        //public ActionResult ReadMemberData([DataSourceRequest] DataSourceRequest request)
        //{
        //    var user = new List<SynergyUserViewModel>();
        //    using (var httpClient = new HttpClient())
        //    {               
        //        var address = new Uri(_configuration.GetConnectionString("SynergyWebApi") + "api/user/GetUserList");
        //        var response = httpClient.GetAsync(address).Result;
        //        user = JsonConvert.DeserializeObject<List<SynergyUserViewModel>>(response.Content.ReadAsStringAsync().Result);
        //    }
        //    var model = _memberBusiness.GetList();
        //    var data = model.Result.ToList();

        //    var res = from d in data
        //            join u in user
        //            on d.SynergyUserId equals u.Id
        //            select new MemberViewModel
        //            {
        //                SynergyUserId = d.SynergyUserId,
        //                UserName = u.UserName,
        //                Email = u.Email,
        //                Status = u.Status,
        //                CreatedDate = d.CreatedDate,
        //                LastUpdatedDate = d.LastUpdatedDate,
        //                Id=d.Id
        //            };


        //    var dsResult = res.ToDataSourceResult(request);
        //    return Json(dsResult);
        //}
        public ActionResult ReadMemberData()
        {
            var user = new List<SynergyUserViewModel>();
            using (var httpClient = new HttpClient())
            {
                var address = new Uri(_configuration.GetConnectionString("SynergyWebApi") + "api/user/GetUserList");
                var response = httpClient.GetAsync(address).Result;
                user = JsonConvert.DeserializeObject<List<SynergyUserViewModel>>(response.Content.ReadAsStringAsync().Result);
            }
            var model = _memberBusiness.GetList();
            var data = model.Result.ToList();

            var res = from d in data
                      join u in user
                      on d.SynergyUserId equals u.Id
                      select new MemberViewModel
                      {
                          SynergyUserId = d.SynergyUserId,
                          UserName = u.UserName,
                          Email = u.Email,
                          Status = u.Status,
                          CreatedDate = d.CreatedDate,
                          LastUpdatedDate = d.LastUpdatedDate,
                          Id = d.Id
                      };



            return Json(res);
        }
        [HttpGet]
        public JsonResult GetSynergyUserList()
        {
            var data = new List<SynergyUserViewModel>();
            using (var httpClient = new HttpClient())
            {
                var address = new Uri(_configuration.GetConnectionString("SynergyWebApi") + "api/user/GetUserList");
                var response = httpClient.GetAsync(address).Result;
                data = JsonConvert.DeserializeObject<List<SynergyUserViewModel>>(response.Content.ReadAsStringAsync().Result);
            }
            return Json(data);
        }

        [HttpGet]
        public async Task<JsonResult> GetPortalList()
        {
            var data = await _portalBusiness.GetList();
            return Json(data);
        }

        [HttpGet]
        public async Task<JsonResult> GetMemberGroupList()
        {
            var data = await _groupBusiness.GetList();
            return Json(data);
        }

        [HttpPost]
        public async Task<IActionResult> ManageMember(MemberViewModel model)
        {
            if (ModelState.IsValid)
            {
                //if(model.Status)
                //{
                //    model.MemberStatus = StatusEnum.Active;
                //}
                //else
                //{
                //    model.MemberStatus = StatusEnum.Inactive;
                //}
                if (model.DataAction == DataActionEnum.Create)
                {
                    var result = await _memberBusiness.Create(model);
                    if (result.IsSuccess)
                    {
                        //return RedirectToAction("Index");
                        return PopupRedirect("Member created successfully", true);
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
                else if (model.DataAction == DataActionEnum.Edit)
                {
                    var result = await _memberBusiness.Edit(model);
                    if (result.IsSuccess)
                    {
                        return PopupRedirect("Member edited successfully", true);
                        //return PopupRedirect("Portal edited successfully");
                    }
                    else
                    {
                        ModelState.AddModelErrors(result.Messages);
                    }
                }
            }

            return View("ManageMember", model);
        }
        public async Task<JsonResult> GetMemberTreeList(string id)
        {
            //var result = await _seetingsBusiness.GetDocumentTypeTreeList(id);
            var list = new List<TreeViewViewModel>();
            if (id.IsNullOrEmpty())
            {
                list.Add(new TreeViewViewModel
                {
                    id = "Member",
                    Name = "Member",
                    DisplayName = "Member",
                    ParentId = null,
                    hasChildren = true,
                    expanded = true,
                    Type = NtsTypeEnum.Note.ToString(),
                    children = true,
                    text = "Member",
                    parent = "#",
                    a_attr = new { data_id = "Member", data_name = "Member", data_type = NtsTypeEnum.Note.ToString() },

                });
                list.Add(new TreeViewViewModel
                {
                    id = "MemberGroup",
                    Name = "MemberGroup",
                    DisplayName = "Member Group",
                    ParentId = null,
                    hasChildren = true,
                    expanded = true,
                    Type = NtsTypeEnum.Task.ToString(),
                    children = true,
                    text = "Member Group",
                    parent = "#",
                    a_attr = new { data_id = "MemberGroup", data_name = "MemberGroup", data_type = NtsTypeEnum.Note.ToString() },
                });

            }

            return Json(list.ToList());
        }

    }
}
