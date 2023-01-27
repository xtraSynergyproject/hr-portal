using DocumentFormat.OpenXml.Presentation;
using Hangfire;
using Hangfire.Storage.Monitoring;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using Nest;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Synergy.App.Common;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.ServiceModel.Syndication;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public interface IHangfireScheduler
    {
        Task Enqueue<T>(Expression<Func<T, Task>> methodCall) where T : HangfireScheduler;
    }
    public class HangfireScheduler : IHangfireScheduler
    {

        private static IServiceProvider _services;
        private readonly IConfiguration _configuration;
        private readonly IUserContext _userContext;
        private AuthSignInManager<ApplicationIdentityUser> _customUserManager;
        private readonly IHttpContextAccessor _accessor;
        public HangfireScheduler(IServiceProvider services, IConfiguration configuration
            , AuthSignInManager<ApplicationIdentityUser> customUserManager
             , IUserContext userContext
             , IHttpContextAccessor accessor
            )
        {
            _services = services;
            _configuration = configuration;
            _customUserManager = customUserManager;
            _userContext = userContext;
            _accessor = accessor;
        }
        public async Task Enqueue<T>(Expression<Func<T, Task>> methodCall) where T : HangfireScheduler
        {
            var skip = ApplicationConstant.AppSettings.SkipHangfireEnqueue(_configuration);
            if (skip)
            {
                var method = methodCall.Compile();
                await method.Invoke((T)this);
                return;
            }
            BackgroundJob.Enqueue<T>(methodCall);
        }


        [LogFailure]
        [AutomaticRetry(Attempts = 3)]
        public async Task<bool> SendEmailUsingHangfire(string Id)
        {
            try
            {

                if (Id.IsNotNullAndNotEmpty())
                {
                    var env = _configuration.GetValue<string>("ApplicationEnvironment");
                    //if (env == "DEV")
                    //{
                    //}
                    //else
                    //{
                    var _business = (IRecEmailBusiness)_services.GetService(typeof(IRecEmailBusiness));
                    var model = await _business.GetSingleById(Id);
                    if (model != null)
                    {
                        var result = await _business.SendMail(model);
                    }
                    //}

                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                throw;
            }

        }
        private void SetContext(ApplicationIdentityUser user)
        {
            //Workaround

            ApplicationIdentityUserStatic.Id = user.Id;
            ApplicationIdentityUserStatic.IsSystemAdmin = user.IsSystemAdmin;
            ApplicationIdentityUserStatic.Email = user.Email;
            ApplicationIdentityUserStatic.UserName = user.UserName;
            ApplicationIdentityUserStatic.UserUniqueId = user.Email;
            ApplicationIdentityUserStatic.CompanyId = user.CompanyId;
            ApplicationIdentityUserStatic.CompanyCode = user.CompanyCode;
            ApplicationIdentityUserStatic.CompanyName = user.CompanyName;
            ApplicationIdentityUserStatic.JobTitle = user.JobTitle;
            ApplicationIdentityUserStatic.PhotoId = user.PhotoId;
            ApplicationIdentityUserStatic.UserRoleCodes = user.UserRoleCodes;
            ApplicationIdentityUserStatic.UserRoleIds = user.UserRoleIds;
            ApplicationIdentityUserStatic.UserPortals = user.UserPortals;
            ApplicationIdentityUserStatic.PersonId = user.PersonId;
            ApplicationIdentityUserStatic.PositionId = user.PositionId;
            ApplicationIdentityUserStatic.DepartmentId = user.DepartmentId;
            ApplicationIdentityUserStatic.PortalId = user.PortalId;
            ApplicationIdentityUserStatic.PortalName = user.PortalName;
            ApplicationIdentityUserStatic.LegalEntityId = user.LegalEntityId;
            ApplicationIdentityUserStatic.LegalEntityCode = user.LegalEntityCode;
            ApplicationIdentityUserStatic.IsGuestUser = user.IsGuestUser;

        }
        [LogFailure]
        public async Task<bool> UpdateLocationCountUsingHangfire(ApplicationIdentityUser userContext)
        {
            try
            {
                SetContext(userContext);
                var _noteBusiness = (INoteBusiness)_services.GetService(typeof(INoteBusiness));
                var dync = new List<dynamic>();
                //Code for track Pai Chart
                var tracks = await _noteBusiness.GetList(x => x.TemplateCode == "TRACK_MASTER");
                var trackModel = new List<TrackChartViewModel>();

                foreach (var track in tracks)
                {
                    var keywords = await _noteBusiness.GetKeywordListByTrackId(track.Id);
                    if (keywords.Count > 0)
                    {
                        var searchStr = string.Join(" ", keywords);
                        var content = @"{""size"": 1000,""query"":{""bool"":{""must"":{""multi_match"":{""fields"":[ ""message"", ""title"",""pagename"", ""post_message"",
                        ""title"", ""description"",""user"", ""messages"",""hashtags"", ""text"", ""location"",""unityp"", ""unit_status"", ""unique_id"",
                        ""track_personnel"", ""tycod"", ""sub_tycod"", ""station"", ""latitude"", ""location"", ""longitude"", ""eid"", ""dgroup"",
                        ""ag_id"",""srNo"", ""cameraName"", ""locationName"", ""policeStation"", ""longitude"", ""latitude""
                        , ""ipAddress"", ""rtspLink"", ""typeOfCamera"", ""make"",""srNo"", ""cameraName"", ""locationName"", ""policeStation"",
                        ""longitude"", ""latitude"", ""ipAddress"", ""rtspLink"", ""typeOfCamera"", ""make"",""hashtags"", ""caption""  ],
                        ""query"":""#SEARCHWHERE#""}}}}}";
                        content = content.Replace("#SEARCHWHERE#", searchStr);
                        var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                        using (var httpClient = new HttpClient())
                        {
                            var url1 = "http://178.238.236.213:9200/rssfeeds,facebook1,youtube1,whatsapp1,insta,twitter2,dial_test,test_camera1,test_camera2/_search?pretty=true";
                            var address = new Uri(url1);
                            var response = await httpClient.PostAsync(address, stringContent);
                            var jsontrack = await response.Content.ReadAsStringAsync();


                            var trackdata = JToken.Parse(jsontrack);
                            var trackdata1 = trackdata.SelectToken("hits");
                            var total = trackdata1.SelectToken("total");
                            var value = total.SelectToken("value");
                            trackModel.Add(new TrackChartViewModel { Name = track.NoteSubject, Count = value.ToString() });
                            if (trackdata1.IsNotNull())
                            {
                                var hits = trackdata1.SelectToken("hits");
                                foreach (var hitsItem in hits)
                                {
                                    var source = hitsItem.SelectToken("_source");
                                    var souraceJson = JsonConvert.SerializeObject(source);
                                    var result = JsonConvert.DeserializeObject<dynamic>(souraceJson);
                                    //var result = JsonConvert.DeserializeObject<dynamic>(souraceJson);
                                    dync.Add(result);

                                }
                            }
                        }
                    }
                }
                var data = await _noteBusiness.GetList(x => x.TemplateCode == "SM_LOCATION");
                foreach (var d in data)
                {
                    var sub = d.NoteSubject;
                    var i = 0;
                    try
                    {
                        foreach (var obj in dync)
                        {
                            var souraceJson = JsonConvert.SerializeObject(obj);
                            if (souraceJson.ToLower().Contains(sub.ToLower()))
                            {
                                i++;
                            }

                        }
                    }
                    catch (Exception ex)
                    {


                    }
                    await _noteBusiness.UpdateLocation(d.Id, i.ToString());
                }
                return true;


            }
            catch (Exception ex)
            {
                throw;
            }

        }
        [LogFailure]
        public async Task<bool> RssFeedFileGenerateUsingHangfire(ApplicationIdentityUser userContext)
        {
            try
            {
                SetContext(userContext);
                var EsdbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
                var path = ApplicationConstant.AppSettings.LogstashConfigPath(_configuration);
                var _noteBusiness = (INoteBusiness)_services.GetService(typeof(INoteBusiness));
                var syncdatalist = await _noteBusiness.GetScheduleSyncData();

                foreach (var syncdata in syncdatalist)
                {
                    var feeds = await _noteBusiness.GetRssFeedDataForSchedulingByTemplateCode(syncdata.scheduleTemplate);
                    if (!syncdata.trackingDate.IsNotNull() || syncdata.trackingDate < feeds.First().LastUpdatedDate)
                    {

                        //string fileName = @"D:\Filess\RSS_FEED.conf";                                        
                        //string fileName = path + syncdata.scheduleTemplate + ".conf";
                        try
                        {
                            // Check if file already exists. If yes, delete it.     
                            //if (!System.IO.File.Exists(fileName))
                            //{
                            //File.Delete(fileName);
                            // Create a new file 
                            //using (System.IO.File.Create(fileName)) { }
                            var inputContent = "";
                            var filterContent = "";
                            foreach (var feed in feeds)
                            {
                                inputContent += "rss { url => '" + feed.feedUrl + "'      interval => 120     tags => '" + feed.feedName + "'   }";
                                filterContent += "if '" + feed.feedName + "' in [tags] {         mutate {           replace => { 'name' => '" + feed.feedName + "' }         }     }";
                            }
                            inputContent += "  //EOI";
                            filterContent += "  //EOF";
                            var fileContent = "input { //EOI } filter {  //EOF } output {     elasticsearch {         hosts => ['" + EsdbUrl.TrimEnd('/') + "'] 		index => 'rssfeeds'     } }";
                            fileContent = fileContent.Replace("//EOI", inputContent);
                            fileContent = fileContent.Replace("//EOF", filterContent);
                            //StreamWriter writer = new StreamWriter(System.IO.File.OpenWrite(fileName));

                            //writer.Write(fileContent);

                            //writer.Close();
                            var strcontent = fileContent.Replace("'", "\"");
                            await _noteBusiness.UpdateScheduleSyncData(syncdata.Id, System.DateTime.Now, strcontent);
                            //}
                            //else
                            //{
                            //    var inputContent = "";
                            //    var filterContent = "";
                            //    foreach (var feed in feeds.Where(x => x.LastUpdatedDate > syncdata.trackingDate))
                            //    {
                            //        inputContent += "rss { url => '" + feed.feedUrl + "'      interval => 120     tags => '" + feed.feedName + "'   }";
                            //        filterContent += "if '" + feed.feedName + "' in [tags] {         mutate {           replace => { 'name' => '" + feed.feedName + "' }         }     }";
                            //    }
                            //    string fileContent = "";
                            //    using (StreamReader reader = new StreamReader(System.IO.File.OpenRead(fileName)))
                            //    {
                            //        fileContent = reader.ReadToEnd();
                            //        reader.Close();
                            //    }
                            //    if (inputContent.IsNotNullAndNotEmpty())
                            //    {
                            //        inputContent += "  //EOI";
                            //        fileContent = fileContent.Replace("//EOI", inputContent);
                            //    }
                            //    if (filterContent.IsNotNullAndNotEmpty())
                            //    {
                            //        filterContent += "  //EOF";
                            //        fileContent = fileContent.Replace("//EOF", filterContent);
                            //    }
                            //    StreamWriter writer = new StreamWriter(System.IO.File.OpenWrite(fileName));

                            //    writer.Write(fileContent);

                            //    writer.Close();
                            //    var strcontent = fileContent.Replace("'", "\"");
                            //    await _noteBusiness.UpdateScheduleSyncData(syncdata.Id, System.DateTime.Now, strcontent);
                            //}
                        }
                        catch (Exception Ex)
                        {

                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {

                return false;
            }
        }
        [LogFailure]
        public async Task<bool> ExecuteProcessDesignComponent(string componentResultId, ApplicationIdentityUser userContext)
        {
            try
            {
                SetContext(userContext);
                var _business = (IComponentResultBusiness)_services.GetService(typeof(IComponentResultBusiness));
                await _business.ExecuteComponent(componentResultId);
                return true;
            }
            catch (Exception ex)
            {
                //Console.Write(ex.ToString());
                //Console.ReadLine();
                return false;
            }

        }
        [LogFailure]
        [AutomaticRetry(Attempts = 3)]
        public async Task<bool> SendMail(string mailId, ApplicationIdentityUser userContext)
        {
            try
            {
                SetContext(userContext);
                var _business = _services.GetService<IEmailBusiness>();
                var email = await _business.GetSingleById(mailId);
                email.DataAction = DataActionEnum.Edit;
                await _business.SendMail(email);
                return true;
            }
            catch (Exception)
            {
                throw;
            }

        }
        [LogFailure]
        public async Task<bool> ExecuteProcessDesignResult(string templateId, string serviceId, ApplicationIdentityUser userContext)
        {
            try
            {
                SetContext(userContext);
                var _business = (IProcessDesignResultBusiness)_services.GetService(typeof(IProcessDesignResultBusiness));
                await _business.ExecuteProcessDesignResult(templateId, serviceId);
                return true;
            }
            catch (Exception ex)
            {
                //Console.Write(ex.ToString());
                //Console.ReadLine();
                return false;
            }

        }
        [LogFailure]
        public async Task<bool> ManageStepTaskComponent(TaskTemplateViewModel template, ApplicationIdentityUser userContext)
        {
            try
            {
                SetContext(userContext);
                var _business = _services.GetService<IComponentResultBusiness>();
                await _business.ManageStepTaskComponent(template);
                return true;
            }
            catch (Exception)
            {
                throw;
            }

        }
        [LogFailure]
        [Queue("alpha")]
        public async Task<bool> UpdateNtsStatus()
        {
            try
            {
                //var _business = (IRecTaskBusiness)_services.GetService(typeof(IRecTaskBusiness));
                //var model = await _business.UpdateOverdueTaskAndServiceStatus(DateTime.Now);
                var ntsBusiness = (INtsBusiness)_services.GetService(typeof(INtsBusiness));
                await ntsBusiness.UpdateOverdueNts(DateTime.Now);
                await ntsBusiness.UpdateNotStartedNts(DateTime.Now);


                return true;
            }
            catch (Exception ex)
            {
                //Console.Write(ex.ToString());
                //Console.ReadLine();
                return false;
            }

        }

        public async Task<bool> UpdateJobAdvertisementStatus()
        {
            try
            {
                var _business = (IJobAdvertisementBusiness)_services.GetService(typeof(IJobAdvertisementBusiness));
                await _business.UpdateJobAdvertisementStatus();
                return true;
            }
            catch (Exception ex)
            {
                //Console.Write(ex.ToString());
                //Console.ReadLine();
                return false;
            }

        }
        [LogFailure]
        public async Task<bool> SendEmailSummary()
        {
            try
            {
                var _emailbusiness = (IRecEmailBusiness)_services.GetService(typeof(IRecEmailBusiness));
                var _business = (IUserBusiness)_services.GetService(typeof(IUserBusiness));
                var _taskBusiness = (IRecTaskBusiness)_services.GetService(typeof(IRecTaskBusiness));
                var _notificationBusiness = (IPushNotificationBusiness)_services.GetService(typeof(IPushNotificationBusiness));
                var _configuration = (IConfiguration)_services.GetService(typeof(IConfiguration));
                var userList = await _business.GetUserListForEmailSummary();
                foreach (var user in userList)
                {
                    var templateList = await _taskBusiness.GetTasksTemplateCodeByUserId(user.Id);
                    var notificationModel = new Synergy.App.ViewModel.NotificationViewModel();
                    notificationModel.Subject = "Synergy Summary | " + DateTime.Now;
                    notificationModel.From = user.Email;
                    notificationModel.ToUserId = user.Id;
                    notificationModel.To = user.Email;
                    var body = new StringBuilder();
                    body.Append("<div><h5> Hello " + user.Name + "</h5>Below is Synergy Summary Details | " + DateTime.Now + "<br><br></div>");
                    foreach (var template in templateList)
                    {

                        var ids = await _taskBusiness.GetTaskIdsByUserId(user.Id, template.TemplateCode, "INPROGRESS,OVERDUE", null);
                        if (ids.IsNotNullAndNotEmpty())
                        {
                            var list = await _taskBusiness.GetTaskDetailsSummaryList(ids, template.TemplateCode, user.Id);
                            var header = list.Count > 0 ? list.First() : null;
                            if (header.IsNotNull())
                            {
                                body.Append("<div><h4 style='color: dodgerblue;'>" + header.Subject + "</h4></div>");
                                body.Append("<div><table>");
                                body.Append("<tr>");
                                body.Append("<th style='border: 1px solid #dddddd;text-align: left;padding: 8px;' width='100px'>Task No</th>");
                                body.Append("<th style='border: 1px solid #dddddd;text-align: left;padding: 8px;' width='300px'>Subject</th>");
                                body.Append("<th style='border: 1px solid #dddddd;text-align: left;padding: 8px;'>Task Status</th>");
                                if (template.TemplateCode != "JOBDESCRIPTION_HM")
                                {
                                    body.Append("<th style='border: 1px solid #dddddd;text-align: left;padding: 8px;'>Candidate Name</th>");
                                }
                                body.Append("<th style='border: 1px solid #dddddd;text-align: left;padding: 8px;'>Position</th>");
                                body.Append("<th style='border: 1px solid #dddddd;text-align: left;padding: 8px;'>Org Unit</th>");
                                if (template.TemplateCode != "JOBDESCRIPTION_HM")
                                {
                                    body.Append("<th style='border: 1px solid #dddddd;text-align: left;padding: 8px;'>GAEC No</th>");
                                }
                                body.Append("<th style='border: 1px solid #dddddd;text-align: left;padding: 8px;'>Start Date</th>");
                                body.Append("<th style='border: 1px solid #dddddd;text-align: left;padding: 8px;'>Due Date</th>");
                                if (header.TextBoxDisplay1.IsNotNullAndNotEmpty() && header.TextBoxDisplayType1 != NtsFieldType.NTS_HyperLink)
                                {
                                    body.Append("<th style='border: 1px solid #dddddd;text-align: left;padding: 8px;'>" + header.TextBoxDisplay1 + "</th>");
                                }
                                if (header.DropdownDisplay1.IsNotNullAndNotEmpty())
                                {
                                    body.Append("<th style='border: 1px solid #dddddd;text-align: left;padding: 8px;'>" + header.DropdownDisplay1 + "</th>");
                                }
                                if (header.TextBoxDisplay2.IsNotNullAndNotEmpty() && header.TextBoxDisplayType2 != NtsFieldType.NTS_HyperLink)
                                {
                                    body.Append("<th style='border: 1px solid #dddddd;text-align: left;padding: 8px;'>" + header.TextBoxDisplay2 + "</th>");
                                }
                                if (header.DropdownDisplay2.IsNotNullAndNotEmpty())
                                {
                                    body.Append("<th style='border: 1px solid #dddddd;text-align: left;padding: 8px;'>" + header.DropdownDisplay2 + "</th>");
                                }
                                if (header.TextBoxDisplay3.IsNotNullAndNotEmpty() && header.TextBoxDisplayType3 != NtsFieldType.NTS_HyperLink)
                                {
                                    body.Append("<th style='border: 1px solid #dddddd;text-align: left;padding: 8px;'>" + header.TextBoxDisplay3 + "</th>");
                                }
                                if (header.DropdownDisplay3.IsNotNullAndNotEmpty())
                                {
                                    body.Append("<th style='border: 1px solid #dddddd;text-align: left;padding: 8px;'>" + header.DropdownDisplay3 + "</th>");
                                }
                                if (header.TextBoxDisplay4.IsNotNullAndNotEmpty() && header.TextBoxDisplayType4 != NtsFieldType.NTS_HyperLink)
                                {
                                    body.Append("<th style='border: 1px solid #dddddd;text-align: left;padding: 8px;'>" + header.TextBoxDisplay4 + "</th>");
                                }
                                if (header.DropdownDisplay4.IsNotNullAndNotEmpty())
                                {
                                    body.Append("<th style='border: 1px solid #dddddd;text-align: left;padding: 8px;'>" + header.DropdownDisplay4 + "</th>");
                                }
                                if (header.TextBoxDisplay5.IsNotNullAndNotEmpty() && header.TextBoxDisplayType5 != NtsFieldType.NTS_HyperLink)
                                {
                                    body.Append("<th style='border: 1px solid #dddddd;text-align: left;padding: 8px;'>" + header.TextBoxDisplay5 + "</th>");
                                }
                                if (header.DropdownDisplay5.IsNotNullAndNotEmpty())
                                {
                                    body.Append("<th style='border: 1px solid #dddddd;text-align: left;padding: 8px;'>" + header.DropdownDisplay5 + "</th>");
                                }
                                if (header.TextBoxDisplay6.IsNotNullAndNotEmpty() && header.TextBoxDisplayType6 != NtsFieldType.NTS_HyperLink)
                                {
                                    body.Append("<th style='border: 1px solid #dddddd;text-align: left;padding: 8px;'>" + header.TextBoxDisplay6 + "</th>");
                                }
                                if (header.DropdownDisplay6.IsNotNullAndNotEmpty())
                                {
                                    body.Append("<th style='border: 1px solid #dddddd;text-align: left;padding: 8px;'>" + header.DropdownDisplay6 + "</th>");
                                }
                                if (header.TextBoxDisplay7.IsNotNullAndNotEmpty() && header.TextBoxDisplayType7 != NtsFieldType.NTS_HyperLink)
                                {
                                    body.Append("<th style='border: 1px solid #dddddd;text-align: left;padding: 8px;'>" + header.TextBoxDisplay7 + "</th>");
                                }
                                if (header.DropdownDisplay7.IsNotNullAndNotEmpty())
                                {
                                    body.Append("<th style='border: 1px solid #dddddd;text-align: left;padding: 8px;'>" + header.DropdownDisplay7 + "</th>");
                                }
                                if (header.TextBoxDisplay8.IsNotNullAndNotEmpty() && header.TextBoxDisplayType8 != NtsFieldType.NTS_HyperLink)
                                {
                                    body.Append("<th style='border: 1px solid #dddddd;text-align: left;padding: 8px;'>" + header.TextBoxDisplay8 + "</th>");
                                }
                                if (header.DropdownDisplay8.IsNotNullAndNotEmpty())
                                {
                                    body.Append("<th style='border: 1px solid #dddddd;text-align: left;padding: 8px;'>" + header.DropdownDisplay8 + "</th>");
                                }
                                if (header.TextBoxDisplay9.IsNotNullAndNotEmpty() && header.TextBoxDisplayType9 != NtsFieldType.NTS_HyperLink)
                                {
                                    body.Append("<th style='border: 1px solid #dddddd;text-align: left;padding: 8px;'>" + header.TextBoxDisplay9 + "</th>");
                                }
                                if (header.DropdownDisplay9.IsNotNullAndNotEmpty())
                                {
                                    body.Append("<th style='border: 1px solid #dddddd;text-align: left;padding: 8px;'>" + header.DropdownDisplay9 + "</th>");
                                }
                                if (header.TextBoxDisplay10.IsNotNullAndNotEmpty() && header.TextBoxDisplayType10 != NtsFieldType.NTS_HyperLink)
                                {
                                    body.Append("<th style='border: 1px solid #dddddd;text-align: left;padding: 8px;'>" + header.TextBoxDisplay10 + "</th>");
                                }
                                if (header.DropdownDisplay10.IsNotNullAndNotEmpty())
                                {
                                    body.Append("<th style='border: 1px solid #dddddd;text-align: left;padding: 8px;'>" + header.DropdownDisplay10 + "</th>");
                                }
                                body.Append("</tr>");
                                string baseUrl = "ApplicationBaseUrl";
                                foreach (var task in list)
                                {
                                    body.Append("<tr>");
                                    body.Append("<td style='border: 1px solid #dddddd;text-align: left;padding: 8px;' width='100px'>" + task.TaskNo + "</td>");
                                    body.Append("<td style='border: 1px solid #dddddd;text-align: left;padding: 8px;' width='300px'>" + task.Subject + "</td>");
                                    if (task.TaskStatusCode == "INPROGRESS")
                                    {
                                        body.Append("<td style='border: 1px solid #dddddd;text-align: left;padding: 8px;background:#ec971f;color:white;font-weight:bold'>" + task.TaskStatusName + "</td>");

                                    }
                                    else if (task.TaskStatusCode == "OVERDUE")
                                    {
                                        body.Append("<td style='border: 1px solid #dddddd;text-align: left;padding: 8px;background:#c9302c;color:white;font-weight:bold'>" + task.TaskStatusName + "</td>");

                                    }
                                    if (template.TemplateCode != "JOBDESCRIPTION_HM")
                                    {
                                        body.Append("<td style='border: 1px solid #dddddd;text-align: left;padding: 8px;'>" + task.CandidateName + "</td>");
                                    }
                                    body.Append("<td style='border: 1px solid #dddddd;text-align: left;padding: 8px;'>" + task.Position + "</td>");
                                    body.Append("<td style='border: 1px solid #dddddd;text-align: left;padding: 8px;'>" + task.OrgUnitName + "</td>");
                                    if (template.TemplateCode != "JOBDESCRIPTION_HM")
                                    {
                                        body.Append("<td style='border: 1px solid #dddddd;text-align: left;padding: 8px;'>" + task.GaecNo + "</td>");
                                    }
                                    body.Append("<td style='border: 1px solid #dddddd;text-align: left;padding: 8px;'>" + task.StartDate + "</td>");
                                    body.Append("<td style='border: 1px solid #dddddd;text-align: left;padding: 8px;'>" + task.DueDate + "</td>");
                                    if ((task.TextBoxDisplayType1 == NtsFieldType.NTS_TextBox || task.TextBoxDisplayType1 == NtsFieldType.NTS_TextArea) && task.TextBoxDisplay1.IsNotNullAndNotEmpty())
                                    {
                                        body.Append("<td style='border: 1px solid #dddddd;text-align: left;padding: 8px;'>" + task.TextValue1 + "</td>");
                                    }
                                    else if ((task.TextBoxDisplayType1 == NtsFieldType.NTS_DatePicker || task.TextBoxDisplayType1 == NtsFieldType.NTS_DateTimePicker) && task.TextBoxDisplay1.IsNotNullAndNotEmpty())
                                    {
                                        body.Append("<td style='border: 1px solid #dddddd;text-align: left;padding: 8px;'>" + task.DatePickerValue1 + "</td>");
                                    }
                                    else if (task.TextBoxDisplayType1 == NtsFieldType.NTS_Attachment && task.TextBoxDisplay1.IsNotNullAndNotEmpty())
                                    {
                                        body.Append("<td style='border: 1px solid #dddddd;text-align: left;padding: 8px;'>" + task.AttachmentValue1 + "</td>");
                                    }
                                    //else if (task.TextBoxDisplayType1 == NtsFieldType.NTS_HyperLink && task.TextBoxDisplay1.IsNotNullAndNotEmpty())
                                    //{

                                    //    body.Append("<td style='border: 1px solid #dddddd;text-align: left;padding: 8px;'> <a href='" + _configuration.GetValue<string>(baseUrl) + task.TextBoxLink1 + "' target='_blank'>click to open</a></td>");
                                    //}
                                    if (task.DropdownDisplay1.IsNotNullAndNotEmpty())
                                    {
                                        body.Append("<td style='border: 1px solid #dddddd;text-align: left;padding: 8px;'>" + task.DropdownDisplayValue1 + "</td>");
                                    }
                                    if ((task.TextBoxDisplayType2 == NtsFieldType.NTS_TextBox || task.TextBoxDisplayType2 == NtsFieldType.NTS_TextArea) && task.TextBoxDisplay2.IsNotNullAndNotEmpty())
                                    {
                                        body.Append("<td style='border: 1px solid #dddddd;text-align: left;padding: 8px;'>" + task.TextValue2 + "</td>");
                                    }
                                    else if ((task.TextBoxDisplayType2 == NtsFieldType.NTS_DatePicker || task.TextBoxDisplayType2 == NtsFieldType.NTS_DateTimePicker) && task.TextBoxDisplay2.IsNotNullAndNotEmpty())
                                    {
                                        body.Append("<td style='border: 1px solid #dddddd;text-align: left;padding: 8px;'>" + task.DatePickerValue1 + "</td>");
                                    }
                                    else if (task.TextBoxDisplayType2 == NtsFieldType.NTS_Attachment && task.TextBoxDisplay2.IsNotNullAndNotEmpty())
                                    {
                                        body.Append("<td style='border: 1px solid #dddddd;text-align: left;padding: 8px;'>" + task.AttachmentValue2 + "</td>");
                                    }
                                    //else if (task.TextBoxDisplayType2 == NtsFieldType.NTS_HyperLink && task.TextBoxDisplay2.IsNotNullAndNotEmpty())
                                    //{
                                    //    body.Append("<td style='border: 1px solid #dddddd;text-align: left;padding: 8px;'><a href='" + _configuration.GetValue<string>(baseUrl) + task.TextBoxLink2 + "' target='_blank'>click to open</a></td>");
                                    //}
                                    if (task.DropdownDisplay2.IsNotNullAndNotEmpty())
                                    {
                                        body.Append("<td style='border: 1px solid #dddddd;text-align: left;padding: 8px;'>" + task.DropdownDisplayValue2 + "</td>");
                                    }

                                    if ((task.TextBoxDisplayType3 == NtsFieldType.NTS_TextBox || task.TextBoxDisplayType3 == NtsFieldType.NTS_TextArea) && task.TextBoxDisplay3.IsNotNullAndNotEmpty())
                                    {
                                        body.Append("<td style='border: 1px solid #dddddd;text-align: left;padding: 8px;'>" + task.TextValue3 + "</td>");
                                    }
                                    else if ((task.TextBoxDisplayType3 == NtsFieldType.NTS_DatePicker || task.TextBoxDisplayType3 == NtsFieldType.NTS_DateTimePicker) && task.TextBoxDisplay3.IsNotNullAndNotEmpty())
                                    {
                                        body.Append("<td style='border: 1px solid #dddddd;text-align: left;padding: 8px;'>" + task.DatePickerValue3 + "</td>");
                                    }
                                    else if (task.TextBoxDisplayType3 == NtsFieldType.NTS_Attachment && task.TextBoxDisplay3.IsNotNullAndNotEmpty())
                                    {
                                        body.Append("<td style='border: 1px solid #dddddd;text-align: left;padding: 8px;'>" + task.AttachmentValue3 + "</td>");
                                    }
                                    //else if (task.TextBoxDisplayType3 == NtsFieldType.NTS_HyperLink && task.TextBoxDisplay3.IsNotNullAndNotEmpty())
                                    //{
                                    //    body.Append("<td style='border: 1px solid #dddddd;text-align: left;padding: 8px;'><a href='" + _configuration.GetValue<string>(baseUrl) + task.TextBoxLink3 + "' target='_blank'>click to open</a></td>");
                                    //}
                                    if (task.DropdownDisplay3.IsNotNullAndNotEmpty())
                                    {
                                        body.Append("<td style='border: 1px solid #dddddd;text-align: left;padding: 8px;'>" + task.DropdownDisplayValue3 + "</td>");
                                    }

                                    if ((task.TextBoxDisplayType4 == NtsFieldType.NTS_TextBox || task.TextBoxDisplayType4 == NtsFieldType.NTS_TextArea) && task.TextBoxDisplay4.IsNotNullAndNotEmpty())
                                    {
                                        body.Append("<td style='border: 1px solid #dddddd;text-align: left;padding: 8px;'>" + task.TextValue4 + "</td>");
                                    }
                                    else if ((task.TextBoxDisplayType4 == NtsFieldType.NTS_DatePicker || task.TextBoxDisplayType4 == NtsFieldType.NTS_DateTimePicker) && task.TextBoxDisplay4.IsNotNullAndNotEmpty())
                                    {
                                        body.Append("<td style='border: 1px solid #dddddd;text-align: left;padding: 8px;'>" + task.DatePickerValue4 + "</td>");
                                    }
                                    else if (task.TextBoxDisplayType4 == NtsFieldType.NTS_Attachment && task.TextBoxDisplay4.IsNotNullAndNotEmpty())
                                    {
                                        body.Append("<td style='border: 1px solid #dddddd;text-align: left;padding: 8px;'>" + task.AttachmentValue4 + "</td>");
                                    }
                                    //else if (task.TextBoxDisplayType4 == NtsFieldType.NTS_HyperLink && task.TextBoxDisplay4.IsNotNullAndNotEmpty())
                                    //{
                                    //    body.Append("<td style='border: 1px solid #dddddd;text-align: left;padding: 8px;'><a href='" + _configuration.GetValue<string>(baseUrl) + task.TextBoxLink4 + "' target='_blank'>click to open</a></td>");
                                    //}
                                    if (task.DropdownDisplay4.IsNotNullAndNotEmpty())
                                    {
                                        body.Append("<td style='border: 1px solid #dddddd;text-align: left;padding: 8px;'>" + task.DropdownDisplayValue4 + "</td>");
                                    }

                                    if ((task.TextBoxDisplayType5 == NtsFieldType.NTS_TextBox || task.TextBoxDisplayType5 == NtsFieldType.NTS_TextArea) && task.TextBoxDisplay5.IsNotNullAndNotEmpty())
                                    {
                                        body.Append("<td style='border: 1px solid #dddddd;text-align: left;padding: 8px;'>" + task.TextValue5 + "</td>");
                                    }
                                    else if ((task.TextBoxDisplayType5 == NtsFieldType.NTS_DatePicker || task.TextBoxDisplayType5 == NtsFieldType.NTS_DateTimePicker) && task.TextBoxDisplay5.IsNotNullAndNotEmpty())
                                    {
                                        body.Append("<td style='border: 1px solid #dddddd;text-align: left;padding: 8px;'>" + task.DatePickerValue5 + "</td>");
                                    }
                                    else if (task.TextBoxDisplayType5 == NtsFieldType.NTS_Attachment && task.TextBoxDisplay5.IsNotNullAndNotEmpty())
                                    {
                                        body.Append("<td style='border: 1px solid #dddddd;text-align: left;padding: 8px;'>" + task.AttachmentValue5 + "</td>");
                                    }
                                    //else if (task.TextBoxDisplayType5 == NtsFieldType.NTS_HyperLink && task.TextBoxDisplay5.IsNotNullAndNotEmpty())
                                    //{
                                    //    body.Append("<td style='border: 1px solid #dddddd;text-align: left;padding: 8px;'><a href='" + _configuration.GetValue<string>(baseUrl) + task.TextBoxLink5 + "' target='_blank'>click to open</a></td>");
                                    //}
                                    if (task.DropdownDisplay5.IsNotNullAndNotEmpty())
                                    {
                                        body.Append("<td style='border: 1px solid #dddddd;text-align: left;padding: 8px;'>" + task.DropdownDisplayValue5 + "</td>");
                                    }

                                    if ((task.TextBoxDisplayType6 == NtsFieldType.NTS_TextBox || task.TextBoxDisplayType6 == NtsFieldType.NTS_TextArea) && task.TextBoxDisplay6.IsNotNullAndNotEmpty())
                                    {
                                        body.Append("<td style='border: 1px solid #dddddd;text-align: left;padding: 8px;'>" + task.TextValue6 + "</td>");
                                    }
                                    else if ((task.TextBoxDisplayType6 == NtsFieldType.NTS_DatePicker || task.TextBoxDisplayType6 == NtsFieldType.NTS_DateTimePicker) && task.TextBoxDisplay6.IsNotNullAndNotEmpty())
                                    {
                                        body.Append("<td style='border: 1px solid #dddddd;text-align: left;padding: 8px;'>" + task.DatePickerValue6 + "</td>");
                                    }
                                    else if (task.TextBoxDisplayType6 == NtsFieldType.NTS_Attachment && task.TextBoxDisplay6.IsNotNullAndNotEmpty())
                                    {
                                        body.Append("<td style='border: 1px solid #dddddd;text-align: left;padding: 8px;'>" + task.AttachmentValue6 + "</td>");
                                    }
                                    //else if (task.TextBoxDisplayType6 == NtsFieldType.NTS_HyperLink && task.TextBoxDisplay6.IsNotNullAndNotEmpty())
                                    //{
                                    //    body.Append("<td style='border: 1px solid #dddddd;text-align: left;padding: 8px;'><a href='" + _configuration.GetValue<string>(baseUrl) + task.TextBoxLink6 + "' target='_blank'>click to open</a></td>");
                                    //}
                                    if (task.DropdownDisplay6.IsNotNullAndNotEmpty())
                                    {
                                        body.Append("<td style='border: 1px solid #dddddd;text-align: left;padding: 8px;'>" + task.DropdownDisplayValue6 + "</td>");
                                    }

                                    if ((task.TextBoxDisplayType7 == NtsFieldType.NTS_TextBox || task.TextBoxDisplayType7 == NtsFieldType.NTS_TextArea) && task.TextBoxDisplay7.IsNotNullAndNotEmpty())
                                    {
                                        body.Append("<td style='border: 1px solid #dddddd;text-align: left;padding: 8px;'>" + task.TextValue7 + "</td>");
                                    }
                                    else if ((task.TextBoxDisplayType7 == NtsFieldType.NTS_DatePicker || task.TextBoxDisplayType7 == NtsFieldType.NTS_DateTimePicker) && task.TextBoxDisplay7.IsNotNullAndNotEmpty())
                                    {
                                        body.Append("<td style='border: 1px solid #dddddd;text-align: left;padding: 8px;'>" + task.DatePickerValue7 + "</td>");
                                    }
                                    else if (task.TextBoxDisplayType7 == NtsFieldType.NTS_Attachment && task.TextBoxDisplay7.IsNotNullAndNotEmpty())
                                    {
                                        body.Append("<td style='border: 1px solid #dddddd;text-align: left;padding: 8px;'>" + task.AttachmentValue7 + "</td>");
                                    }
                                    //else if (task.TextBoxDisplayType7 == NtsFieldType.NTS_HyperLink && task.TextBoxDisplay7.IsNotNullAndNotEmpty())
                                    //{
                                    //    body.Append("<td style='border: 1px solid #dddddd;text-align: left;padding: 8px;'><a href='" + _configuration.GetValue<string>(baseUrl) + task.TextBoxLink7 + "' target='_blank'>click to open</a></td>");
                                    //}
                                    if (task.DropdownDisplay7.IsNotNullAndNotEmpty())
                                    {
                                        body.Append("<td style='border: 1px solid #dddddd;text-align: left;padding: 8px;'>" + task.DropdownDisplayValue7 + "</td>");
                                    }

                                    if ((task.TextBoxDisplayType8 == NtsFieldType.NTS_TextBox || task.TextBoxDisplayType8 == NtsFieldType.NTS_TextArea) && task.TextBoxDisplay8.IsNotNullAndNotEmpty())
                                    {
                                        body.Append("<td style='border: 1px solid #dddddd;text-align: left;padding: 8px;'>" + task.TextValue8 + "</td>");
                                    }
                                    else if ((task.TextBoxDisplayType8 == NtsFieldType.NTS_DatePicker || task.TextBoxDisplayType8 == NtsFieldType.NTS_DateTimePicker) && task.TextBoxDisplay8.IsNotNullAndNotEmpty())
                                    {
                                        body.Append("<td style='border: 1px solid #dddddd;text-align: left;padding: 8px;'>" + task.DatePickerValue8 + "</td>");
                                    }
                                    else if (task.TextBoxDisplayType8 == NtsFieldType.NTS_Attachment && task.TextBoxDisplay8.IsNotNullAndNotEmpty())
                                    {
                                        body.Append("<td style='border: 1px solid #dddddd;text-align: left;padding: 8px;'>" + task.AttachmentValue8 + "</td>");
                                    }
                                    //else if (task.TextBoxDisplayType8 == NtsFieldType.NTS_HyperLink && task.TextBoxDisplay8.IsNotNullAndNotEmpty())
                                    //{
                                    //    body.Append("<td style='border: 1px solid #dddddd;text-align: left;padding: 8px;'><a href='" + _configuration.GetValue<string>(baseUrl) + task.TextBoxLink8 + "' target='_blank'>click to open</a></td>");
                                    //}
                                    if (task.DropdownDisplay8.IsNotNullAndNotEmpty())
                                    {
                                        body.Append("<td style='border: 1px solid #dddddd;text-align: left;padding: 8px;'>" + task.DropdownDisplayValue8 + "</td>");
                                    }

                                    if ((task.TextBoxDisplayType9 == NtsFieldType.NTS_TextBox || task.TextBoxDisplayType9 == NtsFieldType.NTS_TextArea) && task.TextBoxDisplay9.IsNotNullAndNotEmpty())
                                    {
                                        body.Append("<td style='border: 1px solid #dddddd;text-align: left;padding: 8px;'>" + task.TextValue9 + "</td>");
                                    }
                                    //else if ((task.TextBoxDisplayType9 == NtsFieldType.NTS_DatePicker || task.TextBoxDisplayType9 == NtsFieldType.NTS_DateTimePicker) && task.TextBoxDisplay9.IsNotNullAndNotEmpty())
                                    //{
                                    //    body.Append("<td style='border: 1px solid #dddddd;text-align: left;padding: 8px;'><a href='" + _configuration.GetValue<string>(baseUrl) + task.TextBoxLink9 + "' target='_blank'>click to open</a></td>");
                                    //}
                                    else if (task.TextBoxDisplayType9 == NtsFieldType.NTS_Attachment && task.TextBoxDisplay9.IsNotNullAndNotEmpty())
                                    {
                                        body.Append("<td style='border: 1px solid #dddddd;text-align: left;padding: 8px;'>" + task.AttachmentValue9 + "</td>");
                                    }
                                    else if (task.TextBoxDisplayType9 == NtsFieldType.NTS_HyperLink && task.TextBoxDisplay9.IsNotNullAndNotEmpty())
                                    {
                                        body.Append("<td style='border: 1px solid #dddddd;text-align: left;padding: 8px;'>" + task.TextBoxLink9 + "</td>");
                                    }
                                    if (task.DropdownDisplay9.IsNotNullAndNotEmpty())
                                    {
                                        body.Append("<td style='border: 1px solid #dddddd;text-align: left;padding: 8px;'>" + task.DropdownDisplayValue9 + "</td>");
                                    }

                                    if ((task.TextBoxDisplayType10 == NtsFieldType.NTS_TextBox || task.TextBoxDisplayType10 == NtsFieldType.NTS_TextArea) && task.TextBoxDisplay10.IsNotNullAndNotEmpty())
                                    {
                                        body.Append("<td style='border: 1px solid #dddddd;text-align: left;padding: 8px;'>" + task.TextValue10 + "</td>");
                                    }
                                    else if ((task.TextBoxDisplayType10 == NtsFieldType.NTS_DatePicker || task.TextBoxDisplayType10 == NtsFieldType.NTS_DateTimePicker) && task.TextBoxDisplay10.IsNotNullAndNotEmpty())
                                    {
                                        body.Append("<td style='border: 1px solid #dddddd;text-align: left;padding: 8px;'>" + task.DatePickerValue10 + "</td>");
                                    }
                                    else if (task.TextBoxDisplayType10 == NtsFieldType.NTS_Attachment && task.TextBoxDisplay10.IsNotNullAndNotEmpty())
                                    {
                                        body.Append("<td style='border: 1px solid #dddddd;text-align: left;padding: 8px;'>" + task.AttachmentValue10 + "</td>");
                                    }
                                    //else if (task.TextBoxDisplayType10 == NtsFieldType.NTS_HyperLink && task.TextBoxDisplay10.IsNotNullAndNotEmpty())
                                    //{
                                    //    body.Append("<td style='border: 1px solid #dddddd;text-align: left;padding: 8px;'><a href='" + _configuration.GetValue<string>(baseUrl) + task.TextBoxLink10 + "' target='_blank'>click to open</a></td>");
                                    //}
                                    if (task.DropdownDisplay10.IsNotNullAndNotEmpty())
                                    {
                                        body.Append("<td style='border: 1px solid #dddddd;text-align: left;padding: 8px;'>" + task.DropdownDisplayValue10 + "</td>");
                                    }
                                    body.Append("</tr>");
                                }
                                body.Append("</table></div><br><br>");
                            }
                        }
                    }
                    if (templateList.Count > 0)
                    {
                        // body.Append("<div><h4 style='color: dodgerblue;'>No pending task available</h4></div>");

                        notificationModel.Body = body.ToString();
                        var result = await _notificationBusiness.CreateSummaryMail(notificationModel);
                        if (result.IsSuccess)
                        {
                            try
                            {
                                await SendEmailUsingHangfire(result.Item.EmailUniqueId);
                            }
                            catch (Exception e)
                            {

                            }

                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                //Console.Write(ex.ToString());
                //Console.ReadLine();
                return false;
            }

        }

        [LogFailure]
        public async Task<bool> GeneratePerformanceDocument(string pdmId, ApplicationIdentityUser userContext)
        {
            try
            {
                SetContext(userContext);
                var _business = _services.GetService<IPerformanceManagementBusiness>();
                await _business.GeneratePerformanceDocument(pdmId);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        [LogFailure]
        [Queue("alpha")]
        public async Task<bool> UpdateReceiveEmailForProjectManagement()
        {
            try
            {
                var _business = (IEmailBusiness)_services.GetService(typeof(IEmailBusiness));
                await _business.ReceiveMail();
                return true;
            }
            catch (Exception ex)
            {
                //Console.Write(ex.ToString());
                //Console.ReadLine();
                return false;
            }

        }
        [LogFailure]
        public async Task<bool> GenerateTagsForCategory(string tagCategoryId, ApplicationIdentityUser userContext)
        {
            try
            {
                SetContext(userContext);
                var _business = (INtsTagBusiness)_services.GetService(typeof(INtsTagBusiness));
                await _business.GenerateTagsForCategory(tagCategoryId);
                return true;
            }
            catch (Exception ex)
            {
                //Console.Write(ex.ToString());
                //Console.ReadLine();
                return false;
            }

        }
        [LogFailure]
        public async Task<bool> ExecutePayroll(string payrollRunId, ApplicationIdentityUser userContext)
        {
            try
            {
                SetContext(userContext);
                var _business = (IPayrollRunBusiness)_services.GetService(typeof(IPayrollRunBusiness));
                await _business.ExecutePayroll(payrollRunId);
                return true;
            }
            catch (Exception ex)
            {
                //Console.Write(ex.ToString());
                //Console.ReadLine();
                return false;
            }

        }

        [LogFailure]
        public async Task<bool> UpdateOldIsLatestRevision(string noteNo, string noteId, string templateId, ApplicationIdentityUser userContext)
        {
            try
            {
                SetContext(userContext);
                var _business = (INoteBusiness)_services.GetService(typeof(INoteBusiness));
                await _business.UpdateOldIsLatestRevision(noteNo, noteId, templateId);
                return true;
            }
            catch (Exception ex)
            {
                //Console.Write(ex.ToString());
                //Console.ReadLine();
                return false;
            }

        }
        [LogFailure]
        public async Task<bool> CreateTagsForDocumentCount(ApplicationIdentityUser userContext)
        {
            try
            {
                SetContext(userContext);
                //var _business = (IDMSDocumentBusiness)_services.GetService(typeof(IDMSDocumentBusiness));
                //var _tagBusiness = (INtsTagBusiness)_services.GetService(typeof(INtsTagBusiness));
                //var documents = await _business.GetAllDocuments();
                //foreach (var document in documents)
                //{
                //    var parentlist = await _business.GetAllParentByNoteId(document.Id);
                //    foreach (var parent in parentlist)
                //    {
                //        var tag = new NtsTagViewModel { NtsId = parent.Id, NtsType = NtsTypeEnum.Note, TagSourceReferenceId = document.Id };
                //        var res = await _tagBusiness.Create(tag);
                //    }
                //}
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        [LogFailure]
        public async Task<bool> GenerateDummySurveyDetails(string surveyScheduleId, int count, ApplicationIdentityUser userContext, string userId = null)
        {
            try
            {
                SetContext(userContext);
                var _business = (ITalentAssessmentBusiness)_services.GetService(typeof(ITalentAssessmentBusiness));
                await _business.GenerateDummySurveyDetails(surveyScheduleId, count);

                return true;
            }
            catch (Exception ex)
            {
                //Console.Write(ex.ToString());
                //Console.ReadLine();
                return false;
            }

        }

        [LogFailure]
        public async Task<bool> ExecuteSurveyForUsers(string NoteId, string PortalId, ApplicationIdentityUser userContext)
        {
            try
            {
                SetContext(userContext);
                var _business = (ITalentAssessmentBusiness)_services.GetService(typeof(ITalentAssessmentBusiness));
                await _business.ExecuteSurveyForUsers(NoteId, PortalId);

                return true;
            }
            catch (Exception ex)
            {
                //Console.Write(ex.ToString());
                //Console.ReadLine();
                return false;
            }

        }
        [LogFailure]
        public async Task<bool> MapDepartmentUser(NoteTemplateViewModel model, ApplicationIdentityUser userContext)
        {
            try
            {
                SetContext(userContext);
                var _business = (IPerformanceManagementBusiness)_services.GetService(typeof(IPerformanceManagementBusiness));
                await _business.MapDepartmentUser(model);

                return true;
            }
            catch (Exception ex)
            {
                //Console.Write(ex.ToString());
                //Console.ReadLine();
                return false;
            }

        }


        [LogFailure]
        public async Task<bool> GenerateDummyWorkBoardSections(int NoOfItem, ApplicationIdentityUser userContext)
        {
            try
            {
                SetContext(userContext);
                var _business = (IWorkboardBusiness)_services.GetService(typeof(IWorkboardBusiness));
                await _business.GenerateDummyWorkBoardSections(NoOfItem);

                return true;
            }
            catch (Exception ex)
            {
                //Console.Write(ex.ToString());
                //Console.ReadLine();
                return false;
            }

        }
        [LogFailure]
        public async Task<bool> GenerateSurveyDetails(string surveyScheduleId, ApplicationIdentityUser userContext)
        {
            try
            {
                SetContext(userContext);
                var _business = (ITalentAssessmentBusiness)_services.GetService(typeof(ITalentAssessmentBusiness));
                await _business.GenerateSurveyDetails(surveyScheduleId);

                return true;
            }
            catch (Exception ex)
            {
                //Console.Write(ex.ToString());
                //Console.ReadLine();
                return false;
            }

        }
        [LogFailure]
        public async Task<bool> CreateService(ServiceTemplateViewModel service)
        {
            try
            {
                var _business = (IServiceBusiness)_services.GetService(typeof(IServiceBusiness));
                await _business.ManageService(service);
                return true;
            }
            catch (Exception ex)
            {
                //Console.Write(ex.ToString());
                //Console.ReadLine();
                return false;
            }

        }
        [LogFailure]
        [Queue("default")]
        public async Task<bool> ConvertFileToPdf()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                var argsPrepend = "";
                var shellName = "/bin/bash";
                //var _environment = (IHostingEnvironment)_services.GetService(typeof(IHostingEnvironment));
                var _business = (IFileBusiness)_services.GetService(typeof(IFileBusiness));
                var list = await _business.GetFileList();
                foreach (var item in list)
                {
                    string filename = item.Id + item.FileExtension;
                    string path = System.IO.Path.Combine(System.IO.Path.GetTempPath(), filename);
                    string outpath = System.IO.Path.Combine(System.IO.Path.GetTempPath());
                    byte[] contentByte = await _business.DownloadMongoFileByte(item.MongoFileId);
                    using (System.IO.Stream file = System.IO.File.OpenWrite(path))
                    {
                        file.Write(contentByte, 0, contentByte.Length);
                    }
                    ProcessStartInfo procStartInfo = new ProcessStartInfo();
                    procStartInfo.FileName = shellName;
                    procStartInfo.Arguments = "/usr/bin/soffice " + string.Format("--convert-to pdf --outdir {0} {1}", outpath, path);                    
                    procStartInfo.RedirectStandardOutput = true;
                    procStartInfo.UseShellExecute = false;
                    procStartInfo.CreateNoWindow = true;
                    procStartInfo.WorkingDirectory = Environment.CurrentDirectory;

                    Process process = new Process() { StartInfo = procStartInfo, };
                    process.Start();
                    process.WaitForExit();

                    // Check for failed exit code.
                    if (process.ExitCode == 0)
                    {
                        string newfileName = item.Id + ".pdf";
                        string newpath = System.IO.Path.Combine(outpath, newfileName);
                        byte[] bytes = System.IO.File.ReadAllBytes(newpath);
                        var res = await _business.UploadMongoFileByte(item, bytes);
                        if (res)
                        {
                            if (System.IO.File.Exists(path))
                            {
                                System.IO.File.Delete(path);
                            }
                            if (System.IO.File.Exists(newpath))
                            {
                                System.IO.File.Delete(newpath);
                            }
                        }

                    }

                }
            }


            return true;


        }

        [LogFailure]
        public async Task<bool> ConvertSingleFileToPdf(string id)
        {

            var argsPrepend = "";
            var shellName = "/bin/bash";
            //  if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            //  {
            //      shellName = "cmd";
            //      argsPrepend = "/c ";
            //  }
            //var _environment = (IHostingEnvironment)_services.GetService(typeof(IHostingEnvironment));
            var _business = (IFileBusiness)_services.GetService(typeof(IFileBusiness));
            var model = await _business.GetSingleById(id);
            byte[] contentByte = await _business.DownloadMongoFileByte(model.MongoFileId);
            string filename = model.Id + model.FileExtension;
            string path = System.IO.Path.Combine(System.IO.Path.GetTempPath(), filename);
            string outpath = System.IO.Path.GetTempPath();
            Console.Write("Path " + path.ToString());
            Console.Write("Output Path " + outpath.ToString());
            using (System.IO.Stream file = System.IO.File.OpenWrite(path))
            {
                file.Write(contentByte, 0, contentByte.Length);
            }
            string libreOfficePath = "/usr/bin/soffice";
            //switch (Environment.OSVersion.Platform)
            //{
            //    case PlatformID.Unix:
            //        libreOfficePath = "/usr/bin/soffice";
            //        break;
            //    default:
            //        libreOfficePath = "C:\\Program Files\\LibreOffice\\program\\soffice.exe";
            //        break;
            //}

            //ProcessStartInfo procStartInfo = new ProcessStartInfo(libreOfficePath, string.Format("--convert-to pdf --outdir {0} {1}", outpath, path));
            ProcessStartInfo procStartInfo = new ProcessStartInfo();
            procStartInfo.RedirectStandardOutput = true;
            procStartInfo.FileName = shellName;
            procStartInfo.Arguments = "/usr/bin/soffice " + string.Format("--convert-to pdf --outdir {0} {1}", outpath, path);
            procStartInfo.UseShellExecute = false;
            procStartInfo.CreateNoWindow = true;
            procStartInfo.WorkingDirectory = Environment.CurrentDirectory;

            Process process = new Process() { StartInfo = procStartInfo, };
            process.Start();
            process.WaitForExit();

            // Check for failed exit code.
            if (process.ExitCode == 0)
            {
                Console.Write("Converted Succesfully ");
                string newfileName = model.Id + ".pdf";
                string newpath = System.IO.Path.Combine(outpath, newfileName);
                byte[] bytes = System.IO.File.ReadAllBytes(newpath);
                var res = await _business.UploadMongoFileByte(model, bytes);
                if (res)
                {
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                    if (System.IO.File.Exists(newpath))
                    {
                        System.IO.File.Delete(newpath);
                    }
                }

            }

            return true;
        }
        [LogFailure]
        public async Task<bool> GeneratePerformanceDocumentStages(string stageMasterId, ApplicationIdentityUser userContext)
        {
            try
            {
                SetContext(userContext);
                var _business = (IPerformanceManagementBusiness)_services.GetService(typeof(IPerformanceManagementBusiness));
                await _business.GeneratePerformanceDocumentStages(stageMasterId);
                return true;
            }
            catch (Exception ex)
            {
                //Console.Write(ex.ToString());
                //Console.ReadLine();
                return false;
            }

        }
        [LogFailure]
        [Queue("alpha")]
        public async Task<bool> DailyJob()
        {
            try
            {
                var ntsBusiness = (INtsBusiness)_services.GetService(typeof(INtsBusiness));
                await InitiateReviewTask();
                await ntsBusiness.DisbaleGrievenceReopenService(DateTime.Now);
                await ntsBusiness.SendNotificationForRentServices();
                await ntsBusiness.CancelCommunityHallBookingOnExpired(DateTime.Now);
                await ntsBusiness.UpdateRentalStatusForVacating();
                var compResultBusiness = _services.GetService<IComponentResultBusiness>();
                await compResultBusiness.EscalateTask();
                var smartCityBusiness = _services.GetService<ISmartCityBusiness>();
                //await smartCityBusiness.GenerateAssetBillPayment();
                await GenerateRevenueCollectionBillForJammu();
                return true;
            }
            catch (Exception)
            {
                throw;
            }

        }

        private async Task<bool> InitiateReviewTask()
        {
            try
            {
                var _pmsBusiness = (IPerformanceManagementBusiness)_services.GetService(typeof(IPerformanceManagementBusiness));
                var documentMaster = await _pmsBusiness.GetPerformanceDocumentsList();
                if (documentMaster != null)
                {
                    foreach (var master in documentMaster)
                    {
                        var stages = await _pmsBusiness.GetPerformanceDocumentStageData(master.NoteId);
                        stages = stages.Where(x => x.DocumentStageStatus == PerformanceDocumentStatusEnum.Active && x.EnableReview == true && x.ReviewStartDate.IsNotNull() && x.ReviewStartDate.Value.Date <= DateTime.Now.Date).ToList();
                        if (stages.IsNotNull())
                        {
                            foreach (var stage in stages)
                            {
                                await _pmsBusiness.GeneratePerformanceDocumentStages(stage.Id);


                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        [DisableConcurrentExecution(timeoutInSeconds: 10 * 60)]
        [Queue("alpha")]
        public async Task<bool> IncrementalDBMigration()
        {
            try
            {
                Console.WriteLine("Starting IncrementalDBMigration");
                var _cmsBusiness = (ICmsBusiness)_services.GetService(typeof(ICmsBusiness));
                var lastMigrationScriptName = await _cmsBusiness.GetLatestMigrationScript();
                DirectoryInfo info = new DirectoryInfo(Path.Combine(AppContext.BaseDirectory, "MigrationScript"));
                Console.WriteLine("Directory for Migration ==== " + Path.Combine(AppContext.BaseDirectory, "MigrationScript"));

                var eldbUrl = ApplicationConstant.AppSettings.WebApiDevUrl(_configuration);
                var settings = new ConnectionSettings(new Uri(eldbUrl));
                var client = new ElasticClient(settings);
                var handler = new HttpClientHandler();
                handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                handler.ServerCertificateCustomValidationCallback =
                    (httpRequestMessage, cert, cetChain, policyErrors) =>
                    {
                        return true;
                    };
                using (var httpClient = new HttpClient(handler))
                {
                    var url = eldbUrl + "common/query/GetMigrationScripts?lastMigrationScriptName=" + lastMigrationScriptName;
                    var address = new Uri(url);
                    var response = await httpClient.GetAsync(address);
                    if (response.IsSuccessStatusCode)
                    {
                        bool startExecute = true;
                        var content = await response.Content.ReadAsStringAsync();
                        if (content == "No Pending Migration" || content == "Migration not present")
                        {
                            Console.WriteLine(content);
                            startExecute = false;
                        }

                        if (startExecute)
                        {
                            string scriptData = content;
                            var scriptList = scriptData.Split(';');
                            foreach (var script in scriptList)
                            {
                                if (script.IsNotNullAndNotEmpty())
                                {
                                    var exScript = script.Replace("\r", "").Replace("\n", "");
                                    if (exScript.IsNotNullAndNotEmpty() && exScript != "START TRANSACTION" && exScript != "COMMIT")
                                    {
                                        Console.WriteLine("Script Executing ==== " + exScript);
                                        var scriptResult = await _cmsBusiness.ExecuteMigrationScript(exScript);
                                        Console.WriteLine("Script Error Status ==== " + scriptResult);
                                    }
                                }
                            }
                        }
                    }
                }


                //FileInfo[] files = info.GetFiles().OrderBy(p => p.CreationTime).ToArray();
                //bool startExecute = false;
                //foreach (FileInfo file in files)
                //{
                //    Console.WriteLine("Start Executing ==== " + startExecute);
                //    Console.WriteLine("Migration File Name ==== " + file.FullName);
                //    var fileName = file.Name.Split('.');
                //    if (startExecute)
                //    {
                //        // Execute the file 
                //        string scriptData = System.IO.File.ReadAllText(file.FullName);
                //        var scriptList = scriptData.Split(';');
                //        foreach (var script in scriptList)
                //        {
                //            if (script.IsNotNullAndNotEmpty())
                //            {
                //                var exScript = script.Replace("\r", "").Replace("\n", "");
                //                if (exScript.IsNotNullAndNotEmpty() && exScript != "START TRANSACTION" && exScript != "COMMIT")
                //                {
                //                    Console.WriteLine("Script Executing ==== " + exScript);
                //                    var scriptResult = await _cmsBusiness.ExecuteMigrationScript(exScript);
                //                    Console.WriteLine("Script Error Status ==== " + scriptResult);
                //                }
                //            }
                //        }
                //    }
                //    else if (fileName[0] == lastMigrationScriptName)
                //    {
                //        startExecute = true;
                //    }
                //}
                return true;
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
                //Console.ReadLine();
                return false;
            }

        }
        [LogFailure]
        public async Task PerformApiDataMigrationToElasticDB()
        {
            var _noteBusiness = (INoteBusiness)_services.GetService(typeof(INoteBusiness));
            var template = await _noteBusiness.GetSingle(x => x.TemplateCode == "SOCAIL_SCRAPPING_API");
            if (template.IsNotNull())
            {
                var apilist = await _noteBusiness.GetAllCCTNSApiMethods();
                foreach (var api in apilist)
                {
                    try
                    {
                        if (api.Url.ToLower().Contains(api.NoteSubject))
                        {
                            RecurringJob.RemoveIfExists("MigrationJob-" + api.NoteSubject);
                            RecurringJob.AddOrUpdate<Synergy.App.Business.HangfireScheduler>("MigrationJob-" + api.NoteSubject, x => x.ApiMasterDataMigrationToElasticDB(api), api.ScheduleTime);                            
                        }
                        else
                        {
                            RecurringJob.RemoveIfExists("MigrationJob-" + api.NoteSubject);
                            RecurringJob.AddOrUpdate<Synergy.App.Business.HangfireScheduler>("MigrationJob-" + api.NoteSubject, x => x.Dial100DataMigrationToElasticDB(api), api.ScheduleTime);
                        }
                    }
                    catch (Exception)
                    {


                    }



                }
            }


        }
        [LogFailure]
        public async Task ApiMasterDataMigrationToElasticDB(SocailScrappingApiViewModel api)
        {
            try
            {

                var _noteBusiness = (INoteBusiness)_services.GetService(typeof(INoteBusiness));
                if (api.Url.ToLower().Contains(api.NoteSubject))
                {
                    var monitor = JobStorage.Current.GetMonitoringApi();
                    var enqueueList = monitor.EnqueuedJobs("beta", 0, 2500);
                    var districts = await _noteBusiness.GetAllDistrict();
                    foreach (var district in districts.Where(x => x.Code.IsNotNullAndNotEmpty()))
                    {
                        try
                        {
                            if (enqueueList.Where(x => x.Value.Job.Args[1].ToString() == district.Code && x.Value.Job.Arguments.FirstOrDefault().ToString().Contains(api.NoteSubject)).Any())
                            {

                                continue;
                            }
                            BackgroundJob.Enqueue<Synergy.App.Business.HangfireScheduler>(x => x.ApiDataMigrationByDistrictToElasticDB(api, district.Code));
                            
                        }
                        catch (Exception)
                        {


                        }                       
                    }
                }                

            }
            catch (Exception e)
            {

            }

        }
        [LogFailure]
        [Queue("beta")]
        [AutomaticRetry(Attempts = 0)]
        public async Task<string> ApiDataMigrationByDistrictToElasticDB(SocailScrappingApiViewModel api, string districtCode)
        {
            var _noteBusiness = (INoteBusiness)_services.GetService(typeof(INoteBusiness));
            var _fromdate = "";
            var _todate = "";
            try
            {

                var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
                var settings = new ConnectionSettings(new Uri(eldbUrl));
                var client = new ElasticClient(settings);
                var query = ApplicationConstant.BusinessAnalytics.MaxDateQuery;
                query = query.Replace("#FILTERCOLUMN#", api.FilterColumn);
                var queryContent = new StringContent(query, Encoding.UTF8, "application/json");
                var handler = new HttpClientHandler();
                handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                handler.ServerCertificateCustomValidationCallback =
                    (httpRequestMessage, cert, cetChain, policyErrors) =>
                    {
                        return true;
                    };
                using (var httpClient = new HttpClient(handler))
                {
                    var url = eldbUrl + api.NoteSubject + "/_search?pretty=true";
                    var address = new Uri(url);
                    var response = await httpClient.PostAsync(address, queryContent);
                    if (response.IsSuccessStatusCode)
                    {
                        var log = await _noteBusiness.GetSchedulerLog(api.NoteSubject, districtCode);
                        var _jsondata = await response.Content.ReadAsStringAsync();
                        var _dataToken = JToken.Parse(_jsondata);
                        var _responsedata = _dataToken.SelectToken("aggregations");
                        var _maxdateToken = _responsedata.SelectToken("max_date");
                        var _dateToken = _maxdateToken.Last();
                        var _date = _dateToken.Last();
                        var fromDate = _date.Value<DateTime>();
                        var toDate = fromDate.AddDays(api.BatchDays);
                        api.ToDate = api.ToDate == DateTime.MinValue ? DateTime.Now.AddHours(5).AddMinutes(30) : api.ToDate;
                        var batchToDate = (toDate > api.ToDate) ? ((api.ToDate > DateTime.Now.AddHours(5).AddMinutes(30)) ? DateTime.Now.AddHours(5).AddMinutes(30) : api.ToDate) : ((toDate > DateTime.Now.AddHours(5).AddMinutes(30)) ? DateTime.Now.AddHours(5).AddMinutes(30) : toDate);
                        var parameterIds = api.Parameters.Replace('[', '(').Replace(']', ')').Replace("\"", "'");
                        var parameterList = await _noteBusiness.GetAllCCTNSApiMethodsParameter(parameterIds);
                        var orderedParameterList = parameterList.OrderBy(x => x.SequenceNo).ToList();
                        var content1 = "{";
                        int i = 1;
                        foreach (var parameter in orderedParameterList)
                        {
                            if (i == orderedParameterList.Count)
                            {
                                content1 += "\"" + parameter.ParameterName + "\":\"" + parameter.DefaultValue + "\"";
                            }
                            else
                            {
                                content1 += "\"" + parameter.ParameterName + "\":\"" + parameter.DefaultValue + "\",";
                            }
                            i++;
                        }
                        content1 += "}";
                        var url1 = api.Url;
                        var address1 = new Uri(url1);
                        if (url1.ToLower().Contains(api.NoteSubject))
                        {
                            if (fromDate.Date == DateTime.Now.Date)
                            {
                                fromDate = fromDate.AddDays(-1);
                            }
                            //if (batchToDate.Date == DateTime.Now.Date)
                            //{
                            //    batchToDate = batchToDate.AddDays(-1);
                            //}
                            content1 = content1.Replace("#FROM_DATE#", (log.IsNotNull() && !log.success && log.fromDate.IsNotNullAndNotEmpty()) ? log.fromDate : fromDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
                            content1 = content1.Replace("#To_DATE#", (log.IsNotNull() && !log.success && log.toDate.IsNotNullAndNotEmpty()) ? log.toDate : batchToDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
                            content1 = content1.Replace("#DISTRICT_CODE#", districtCode);
                            _fromdate = (log.IsNotNull() && !log.success && log.fromDate.IsNotNullAndNotEmpty()) ? log.fromDate : fromDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                            _todate = (log.IsNotNull() && !log.success && log.toDate.IsNotNullAndNotEmpty()) ? log.toDate : batchToDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                            var stringContent1 = new StringContent(content1, Encoding.UTF8, "application/json");
                            Console.WriteLine(content1);
                            var response1 = await httpClient.PostAsync(address1, stringContent1);
                            if (response1.IsSuccessStatusCode)
                            {
                                var json = await response1.Content.ReadAsStringAsync();
                                Console.WriteLine(json);
                                var dataToken = JToken.Parse(json);
                                var responsedata = dataToken.SelectToken(api.ResponseToken);
                                Console.WriteLine(responsedata);
                                if (responsedata.IsNotNull() && !json.Contains("Not Data Found!"))
                                {
                                    var objects = JArray.Parse(responsedata.ToString());
                                    BulkDescriptor descriptor = new BulkDescriptor();
                                    foreach (JObject root in objects)
                                    {
                                        dynamic obj = new System.Dynamic.ExpandoObject();
                                        var id = string.Empty;
                                        var filtercolumn = string.Empty;
                                        var idcolumn = string.Empty;
                                        foreach (KeyValuePair<String, JToken> app in root)
                                        {
                                            var key = app.Key;
                                            var value = app.Value;

                                            if (key == api.FilterColumn)
                                            {
                                                var a = value.Value<string>();
                                                filtercolumn = a;
                                                try
                                                {
                                                    DateTime dt = DateTime.Parse(a, null);
                                                    ExpandoAddProperty(obj, key, dt);
                                                }
                                                catch (Exception)
                                                {
                                                    DateTime dt = DateTime.ParseExact(a, api.DateFormat, CultureInfo.InvariantCulture);
                                                    ExpandoAddProperty(obj, key, dt);

                                                }

                                            }
                                            else
                                            {
                                                var a = value.Value<string>();
                                                ExpandoAddProperty(obj, key, a);
                                            }
                                            if (key == api.IdColumn)
                                            {
                                                idcolumn = value.Value<string>();

                                            }

                                        }
                                        if (idcolumn.IsNotNullAndNotEmpty() && filtercolumn.IsNotNullAndNotEmpty())
                                        {
                                            id = filtercolumn + "-" + idcolumn;
                                            id = id.Replace("/", "-").Replace(" ", "");
                                        }
                                        descriptor.Index<object>(i => i
                                            .Index(api.NoteSubject)
                                            .Id((Id)id)
                                            .Document(obj));
                                    }
                                    var bulkResponse = client.Bulk(descriptor);
                                    await _noteBusiness.ManageSchedulerLog(new SchedulerLogViewModel
                                    {
                                        NoteSubject = api.NoteSubject,
                                        fromDate = (log.IsNotNull() && !log.success && log.fromDate.IsNotNullAndNotEmpty()) ? log.fromDate : fromDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                                        toDate = (log.IsNotNull() && !log.success && log.toDate.IsNotNullAndNotEmpty()) ? log.toDate : batchToDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                                        districtCode = districtCode,
                                        response = bulkResponse.ToString(),
                                        error = bulkResponse.Errors.ToString(),
                                        success = true,
                                        ActiveUserId = _userContext.UserId
                                    });
                                    return bulkResponse.ToString();
                                }
                                else
                                {
                                    await _noteBusiness.ManageSchedulerLog(new SchedulerLogViewModel
                                    {
                                        NoteSubject = api.NoteSubject,
                                        fromDate = (log.IsNotNull() && !log.success && log.fromDate.IsNotNullAndNotEmpty()) ? log.fromDate : fromDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                                        toDate = (log.IsNotNull() && !log.success && log.toDate.IsNotNullAndNotEmpty()) ? log.toDate : batchToDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                                        districtCode = districtCode,
                                        response = "[]",
                                        error = "",
                                        success = true,
                                        ActiveUserId = _userContext.UserId
                                    });
                                    return "";
                                }

                            }
                            else
                            {
                                await _noteBusiness.ManageSchedulerLog(new SchedulerLogViewModel
                                {
                                    NoteSubject = api.NoteSubject,
                                    fromDate = (log.IsNotNull() && !log.success && log.fromDate.IsNotNullAndNotEmpty()) ? log.fromDate : fromDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                                    toDate = (log.IsNotNull() && !log.success && log.toDate.IsNotNullAndNotEmpty()) ? log.toDate : batchToDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                                    districtCode = districtCode,
                                    response = await response1.Content.ReadAsStringAsync(),
                                    error = response1.ReasonPhrase.ToString(),
                                    success = false,
                                    ActiveUserId = _userContext.UserId
                                });
                                return response1.IsSuccessStatusCode.ToString();
                            }
                        }
                        return "";

                    }
                    else
                    {
                        var fromDate = api.FromDate;
                        var toDate = fromDate.AddDays(api.BatchDays);
                        api.ToDate = api.ToDate == DateTime.MinValue ? DateTime.Now.AddHours(5).AddMinutes(30) : api.ToDate;
                        var batchToDate = (toDate > api.ToDate) ? ((api.ToDate > DateTime.Now.AddHours(5).AddMinutes(30)) ? DateTime.Now.AddHours(5).AddMinutes(30) : api.ToDate) : ((toDate > DateTime.Now.AddHours(5).AddMinutes(30)) ? DateTime.Now.AddHours(5).AddMinutes(30) : toDate);
                        var parameterIds = api.Parameters.Replace('[', '(').Replace(']', ')').Replace("\"", "'");
                        var parameterList = await _noteBusiness.GetAllCCTNSApiMethodsParameter(parameterIds);
                        var orderedParameterList = parameterList.OrderBy(x => x.SequenceNo).ToList();
                        var content1 = "{";
                        int i = 1;
                        foreach (var parameter in orderedParameterList)
                        {
                            if (i == orderedParameterList.Count)
                            {
                                content1 += "\"" + parameter.ParameterName + "\":\"" + parameter.DefaultValue + "\"";
                            }
                            else
                            {
                                content1 += "\"" + parameter.ParameterName + "\":\"" + parameter.DefaultValue + "\",";
                            }
                            i++;
                        }
                        content1 += "}";
                        var url1 = api.Url;
                        var address1 = new Uri(url1);
                        if (url1.ToLower().Contains(api.NoteSubject))
                        {
                            if (fromDate.Date == DateTime.Now.Date)
                            {
                                fromDate = fromDate.AddDays(-1);
                            }
                            //if(batchToDate.Date== DateTime.Now.Date)
                            //{
                            //    batchToDate = batchToDate.AddDays(-1);
                            //}
                            content1 = content1.Replace("#FROM_DATE#", fromDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
                            content1 = content1.Replace("#To_DATE#", batchToDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
                            content1 = content1.Replace("#DISTRICT_CODE#", districtCode);
                            _fromdate = fromDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                            _todate = batchToDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                            var stringContent1 = new StringContent(content1, Encoding.UTF8, "application/json");
                            Console.WriteLine(content1);
                            var response1 = await httpClient.PostAsync(address1, stringContent1);
                            if (response1.IsSuccessStatusCode)
                            {
                                var json = await response1.Content.ReadAsStringAsync();
                                Console.WriteLine(json);
                                var dataToken = JToken.Parse(json);
                                var responsedata = dataToken.SelectToken(api.ResponseToken);
                                Console.WriteLine(responsedata);
                                if (responsedata.IsNotNull() && !json.Contains("Not Data Found!"))
                                {
                                    var objects = JArray.Parse(responsedata.ToString());
                                    BulkDescriptor descriptor = new BulkDescriptor();
                                    foreach (JObject root in objects)
                                    {
                                        dynamic obj = new System.Dynamic.ExpandoObject();
                                        var filtercolumn = string.Empty;
                                        var idcolumn = string.Empty;
                                        var id = string.Empty;
                                        foreach (KeyValuePair<String, JToken> app in root)
                                        {
                                            var key = app.Key;
                                            var value = app.Value;

                                            if (key == api.FilterColumn)
                                            {
                                                var a = value.Value<string>();
                                                filtercolumn = a;
                                                try
                                                {
                                                    DateTime dt = DateTime.Parse(a, null);
                                                    ExpandoAddProperty(obj, key, dt);
                                                }
                                                catch (Exception)
                                                {
                                                    DateTime dt = DateTime.ParseExact(a, api.DateFormat, CultureInfo.InvariantCulture);
                                                    ExpandoAddProperty(obj, key, dt);

                                                }
                                            }
                                            else
                                            {
                                                var a = value.Value<string>();
                                                ExpandoAddProperty(obj, key, a);
                                            }
                                            if (key == api.IdColumn)
                                            {
                                                idcolumn = value.Value<string>();
                                            }


                                        }
                                        if (idcolumn.IsNotNullAndNotEmpty() && filtercolumn.IsNotNullAndNotEmpty())
                                        {
                                            id = filtercolumn + "-" + idcolumn;
                                            id = id.Replace("/", "-").Replace(" ", "");
                                        }
                                        descriptor.Index<object>(i => i
                                            .Index(api.NoteSubject)
                                            .Id((Id)id)
                                            .Document(obj));
                                    }
                                    var bulkResponse = client.Bulk(descriptor);
                                    await _noteBusiness.ManageSchedulerLog(new SchedulerLogViewModel
                                    {
                                        NoteSubject = api.NoteSubject,
                                        fromDate = fromDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                                        toDate = batchToDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                                        districtCode = districtCode,
                                        response = bulkResponse.ToString(),
                                        error = bulkResponse.Errors.ToString(),
                                        success = true,
                                        ActiveUserId = _userContext.UserId
                                    });
                                    return bulkResponse.ToString();
                                }
                                else
                                {
                                    await _noteBusiness.ManageSchedulerLog(new SchedulerLogViewModel
                                    {
                                        NoteSubject = api.NoteSubject,
                                        fromDate = fromDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                                        toDate = batchToDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                                        districtCode = districtCode,
                                        response = "[]",
                                        error = "",
                                        success = true,
                                        ActiveUserId = _userContext.UserId
                                    });
                                    return "";
                                }

                            }
                            else
                            {
                                await _noteBusiness.ManageSchedulerLog(new SchedulerLogViewModel
                                {
                                    NoteSubject = api.NoteSubject,
                                    fromDate = fromDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                                    toDate = batchToDate.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                                    districtCode = districtCode,
                                    response = await response1.Content.ReadAsStringAsync(),
                                    error = response1.ReasonPhrase.ToString(),
                                    success = false,
                                    ActiveUserId = _userContext.UserId
                                });
                                return response1.IsSuccessStatusCode.ToString();
                            }
                        }
                        return "";
                    }


                }
            }
            catch (Exception e)
            {
                await _noteBusiness.ManageSchedulerLog(new SchedulerLogViewModel
                {
                    NoteSubject = api.NoteSubject,
                    fromDate = _fromdate,
                    toDate = _todate,
                    districtCode = districtCode,
                    response = null,
                    error = e.ToString(),
                    success = false,
                    ActiveUserId = _userContext.UserId
                });
                Console.WriteLine(e.ToString());
                throw;
            }

        }
        [LogFailure]
        [Queue("alpha")]
        [AutomaticRetry(Attempts = 0)]
        public async Task<string> Dial100DataMigrationToElasticDB(SocailScrappingApiViewModel api)
        {
            var _noteBusiness = (INoteBusiness)_services.GetService(typeof(INoteBusiness));
            var _fromdate = "";
            var _todate = "";
            try
            {

                var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
                var settings = new ConnectionSettings(new Uri(eldbUrl));
                var client = new ElasticClient(settings);
                var query = ApplicationConstant.BusinessAnalytics.MaxDateQuery;
                query = query.Replace("#FILTERCOLUMN#", api.FilterColumn);
                var queryContent = new StringContent(query, Encoding.UTF8, "application/json");
                var handler = new HttpClientHandler();
                handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                handler.ServerCertificateCustomValidationCallback =
                    (httpRequestMessage, cert, cetChain, policyErrors) =>
                    {
                        return true;
                    };
                using (var httpClient = new HttpClient(handler))
                {
                    var url = eldbUrl + api.NoteSubject + "/_search?pretty=true";
                    var address = new Uri(url);
                    var response = await httpClient.PostAsync(address, queryContent);
                    if (response.IsSuccessStatusCode)
                    {
                        var log = await _noteBusiness.GetSchedulerLog(api.NoteSubject,null);
                        var _jsondata = await response.Content.ReadAsStringAsync();
                        var _dataToken = JToken.Parse(_jsondata);
                        var _responsedata = _dataToken.SelectToken("aggregations");
                        var _maxdateToken = _responsedata.SelectToken("max_date");
                        var _dateToken = _maxdateToken.Last();
                        var _date = _dateToken.Last();
                        var fromDate = _date.Value<DateTime>();
                        var toDate = fromDate.AddDays(api.BatchDays);
                        api.ToDate = api.ToDate == DateTime.MinValue ? DateTime.Now.AddHours(5).AddMinutes(30) : api.ToDate;
                        var batchToDate = (toDate > api.ToDate) ? ((api.ToDate > DateTime.Now.AddHours(5).AddMinutes(30)) ? DateTime.Now.AddHours(5).AddMinutes(30) : api.ToDate) : ((toDate > DateTime.Now.AddHours(5).AddMinutes(30)) ? DateTime.Now.AddHours(5).AddMinutes(30) : toDate);
                        var parameterIds = api.Parameters.Replace('[', '(').Replace(']', ')').Replace("\"", "'");
                        var parameterList = await _noteBusiness.GetAllCCTNSApiMethodsParameter(parameterIds);
                        var orderedParameterList = parameterList.OrderBy(x => x.SequenceNo).ToList();
                        var content1 = "{";
                        int i = 1;
                        foreach (var parameter in orderedParameterList)
                        {
                            if (i == orderedParameterList.Count)
                            {
                                content1 += "\"" + parameter.ParameterName + "\":\"" + parameter.DefaultValue + "\"";
                            }
                            else
                            {
                                content1 += "\"" + parameter.ParameterName + "\":\"" + parameter.DefaultValue + "\",";
                            }
                            i++;
                        }
                        content1 += "}";
                        var url1 = api.Url;
                        var address1 = new Uri(url1);                     
                       
                        content1 = content1.Replace("#FROM_DATE#", (log.IsNotNull() && !log.success && log.fromDate.IsNotNullAndNotEmpty()) ? log.fromDate : fromDate.ToString("yyyy-MM-dd HH:mm:ss"));
                        content1 = content1.Replace("#To_DATE#", (log.IsNotNull() && !log.success && log.toDate.IsNotNullAndNotEmpty()) ? log.toDate : batchToDate.ToString("yyyy-MM-dd HH:mm:ss"));
                        var stringContent1 = new StringContent(content1, Encoding.UTF8, "application/json");
                        _fromdate = (log.IsNotNull() && !log.success && log.fromDate.IsNotNullAndNotEmpty()) ? log.fromDate : fromDate.ToString("yyyy-MM-dd HH:mm:ss");
                        _todate = (log.IsNotNull() && !log.success && log.toDate.IsNotNullAndNotEmpty()) ? log.toDate : batchToDate.ToString("yyyy-MM-dd HH:mm:ss");
                        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("BASIC", api.ApiAuthorization);
                        var request = new HttpRequestMessage
                        {
                            Method = HttpMethod.Get,
                            RequestUri = address1,
                            Content = stringContent1,
                        };
                        var response1 = await httpClient.SendAsync(request);
                        if (response1.IsSuccessStatusCode)
                        {
                            var json = await response1.Content.ReadAsStringAsync();
                            var dataToken = JToken.Parse(json);
                            var responsedata = json != "[]" ? dataToken.First().SelectToken(api.ResponseToken) : null;
                            if (responsedata.IsNotNull())
                            {
                                var objects = JArray.Parse(responsedata.ToString());
                                BulkDescriptor descriptor = new BulkDescriptor();
                                foreach (JObject root in objects)
                                {
                                    dynamic obj = new System.Dynamic.ExpandoObject();
                                    var id = string.Empty;
                                    foreach (KeyValuePair<String, JToken> app in root)
                                    {
                                        var key = app.Key;
                                        var value = app.Value;

                                        if (key == api.FilterColumn)
                                        {
                                            var a = value.Value<string>();
                                            DateTime dt = DateTime.ParseExact(a, api.DateFormat, CultureInfo.InvariantCulture);
                                            ExpandoAddProperty(obj, key, dt);
                                        }
                                        else
                                        {
                                            var a = value.Value<string>();
                                            ExpandoAddProperty(obj, key, a);
                                        }
                                        if (key == api.IdColumn)
                                        {
                                            id = value.Value<string>();
                                        }


                                    }
                                    var qc = @"{""size"": 1000,""query"":{""bool"":{""must"":[{""simple_query_string"" : {""query"": ""#EVENT_NUMBER#"",""fields"": [""event_number""],""default_operator"": ""and""}}]}}}";
                                    qc = qc.Replace("#EVENT_NUMBER#", id);
                                    var qcContent = new StringContent(qc, Encoding.UTF8, "application/json");
                                    var response3 = await httpClient.PostAsync(address, qcContent);
                                    var jsontrack3 = await response3.Content.ReadAsStringAsync();
                                    var trackdata3 = JToken.Parse(jsontrack3);
                                    if (trackdata3.IsNotNull())
                                    {
                                        var hits3 = trackdata3.SelectToken("hits");
                                        if (hits3.IsNotNull())
                                        {
                                            var _hits3 = hits3.SelectToken("hits");
                                            foreach (var hit in _hits3)
                                            {
                                                var source = hit.SelectToken("_source");
                                                var str = JsonConvert.SerializeObject(source);
                                                var result = JsonConvert.DeserializeObject<Dial100ViewModel>(str);
                                                if (result.IsNotNull() && result.isAlerted.IsNotNull())
                                                {
                                                    ExpandoAddProperty(obj, "isAlerted", result.isAlerted);
                                                }
                                            }
                                        }
                                    }
                                    descriptor.Index<object>(i => i
                                        .Index(api.NoteSubject)
                                        .Id((Id)id)
                                        .Document(obj));
                                }
                                var bulkResponse = client.Bulk(descriptor);
                                await _noteBusiness.ManageSchedulerLog(new SchedulerLogViewModel
                                {
                                    NoteSubject = api.NoteSubject,
                                    fromDate = (log.IsNotNull() && !log.success && log.fromDate.IsNotNullAndNotEmpty()) ? log.fromDate : fromDate.ToString("yyyy-MM-dd HH:mm:ss"),
                                    toDate = (log.IsNotNull() && !log.success && log.toDate.IsNotNullAndNotEmpty()) ? log.toDate : batchToDate.ToString("yyyy-MM-dd HH:mm:ss"),
                                    districtCode = null,
                                    response = bulkResponse.ToString(),
                                    error = bulkResponse.Errors.ToString(),
                                    success = true,
                                    ActiveUserId = _userContext.UserId
                                });
                                return bulkResponse.ToString();
                            }
                            else
                            {
                                await _noteBusiness.ManageSchedulerLog(new SchedulerLogViewModel
                                {
                                    NoteSubject = api.NoteSubject,
                                    fromDate = (log.IsNotNull() && !log.success && log.fromDate.IsNotNullAndNotEmpty()) ? log.fromDate : fromDate.ToString("yyyy-MM-dd HH:mm:ss"),
                                    toDate = (log.IsNotNull() && !log.success && log.toDate.IsNotNullAndNotEmpty()) ? log.toDate : batchToDate.ToString("yyyy-MM-dd HH:mm:ss"),
                                    districtCode = null,
                                    response = "[]",
                                    error = "",
                                    success = true,
                                    ActiveUserId = _userContext.UserId
                                });
                                return "";
                            }

                        }
                        else
                        {
                            await _noteBusiness.ManageSchedulerLog(new SchedulerLogViewModel
                            {
                                NoteSubject = api.NoteSubject,
                                fromDate = (log.IsNotNull() && !log.success && log.fromDate.IsNotNullAndNotEmpty()) ? log.fromDate : fromDate.ToString("yyyy-MM-dd HH:mm:ss"),
                                toDate = (log.IsNotNull() && !log.success && log.toDate.IsNotNullAndNotEmpty()) ? log.toDate : batchToDate.ToString("yyyy-MM-dd HH:mm:ss"),
                                districtCode = null,
                                response = await response1.Content.ReadAsStringAsync(),
                                error = response1.ReasonPhrase.ToString(),
                                success = false,
                                ActiveUserId = _userContext.UserId
                            });
                            return response1.IsSuccessStatusCode.ToString();
                        }
                       

                    }
                    else
                    {
                        var fromDate = api.FromDate;
                        var toDate = fromDate.AddDays(api.BatchDays);
                        api.ToDate = api.ToDate == DateTime.MinValue ? DateTime.Now.AddHours(5).AddMinutes(30) : api.ToDate;
                        var batchToDate = (toDate > api.ToDate) ? ((api.ToDate > DateTime.Now.AddHours(5).AddMinutes(30)) ? DateTime.Now.AddHours(5).AddMinutes(30) : api.ToDate) : ((toDate > DateTime.Now.AddHours(5).AddMinutes(30)) ? DateTime.Now.AddHours(5).AddMinutes(30) : toDate);
                        var parameterIds = api.Parameters.Replace('[', '(').Replace(']', ')').Replace("\"", "'");
                        var parameterList = await _noteBusiness.GetAllCCTNSApiMethodsParameter(parameterIds);
                        var orderedParameterList = parameterList.OrderBy(x => x.SequenceNo).ToList();
                        var content1 = "{";
                        int i = 1;
                        foreach (var parameter in orderedParameterList)
                        {
                            if (i == orderedParameterList.Count)
                            {
                                content1 += "\"" + parameter.ParameterName + "\":\"" + parameter.DefaultValue + "\"";
                            }
                            else
                            {
                                content1 += "\"" + parameter.ParameterName + "\":\"" + parameter.DefaultValue + "\",";
                            }
                            i++;
                        }
                        content1 += "}";
                        var url1 = api.Url;
                        var address1 = new Uri(url1);
                        
                        content1 = content1.Replace("#FROM_DATE#", fromDate.ToString("yyyy-MM-dd HH:mm:ss"));
                        content1 = content1.Replace("#To_DATE#", batchToDate.ToString("yyyy-MM-dd HH:mm:ss"));
                        var stringContent1 = new StringContent(content1, Encoding.UTF8, "application/json");
                        _fromdate = fromDate.ToString("yyyy-MM-dd HH:mm:ss");
                        _todate = batchToDate.ToString("yyyy-MM-dd HH:mm:ss");
                        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("BASIC", api.ApiAuthorization);
                        var request = new HttpRequestMessage
                        {
                            Method = HttpMethod.Get,
                            RequestUri = address1,
                            Content = stringContent1,
                        };
                        var response1 = await httpClient.SendAsync(request);
                        if (response1.IsSuccessStatusCode)
                        {
                            var json = await response1.Content.ReadAsStringAsync();
                            var dataToken = JToken.Parse(json);
                            var responsedata = json != "[]" ? dataToken.First().SelectToken(api.ResponseToken) : null;
                            if (responsedata.IsNotNull())
                            {
                                var objects = JArray.Parse(responsedata.ToString());
                                BulkDescriptor descriptor = new BulkDescriptor();
                                foreach (JObject root in objects)
                                {
                                    dynamic obj = new System.Dynamic.ExpandoObject();
                                    var id = string.Empty;
                                    foreach (KeyValuePair<String, JToken> app in root)
                                    {
                                        var key = app.Key;
                                        var value = app.Value;
                                        if (key == api.FilterColumn)
                                        {
                                            var a = value.Value<string>();
                                            DateTime dt = DateTime.ParseExact(a, api.DateFormat, CultureInfo.InvariantCulture);
                                            ExpandoAddProperty(obj, key, dt);
                                        }
                                        else
                                        {
                                            var a = value.Value<string>();
                                            ExpandoAddProperty(obj, key, a);
                                        }
                                        if (key == api.IdColumn)
                                        {
                                            id = value.Value<string>();
                                        }

                                    }
                                    var qc = @"{""size"": 1000,""query"":{""bool"":{""must"":[{""simple_query_string"" : {""query"": ""#EVENT_NUMBER#"",""fields"": [""event_number""],""default_operator"": ""and""}}]}}}";
                                    qc = qc.Replace("#EVENT_NUMBER#", id);
                                    var qcContent = new StringContent(qc, Encoding.UTF8, "application/json");
                                    var response3 = await httpClient.PostAsync(address, qcContent);
                                    var jsontrack3 = await response3.Content.ReadAsStringAsync();
                                    var trackdata3 = JToken.Parse(jsontrack3);
                                    if (trackdata3.IsNotNull())
                                    {
                                        var hits3 = trackdata3.SelectToken("hits");
                                        if (hits3.IsNotNull())
                                        {
                                            var _hits3 = hits3.SelectToken("hits");
                                            foreach (var hit in _hits3)
                                            {
                                                var source = hit.SelectToken("_source");
                                                var str = JsonConvert.SerializeObject(source);
                                                var result = JsonConvert.DeserializeObject<Dial100ViewModel>(str);
                                                if (result.IsNotNull() && result.isAlerted.IsNotNull())
                                                {
                                                    ExpandoAddProperty(obj, "isAlerted", result.isAlerted);
                                                }
                                            }
                                        }
                                    }
                                    descriptor.Index<object>(i => i
                                        .Index(api.NoteSubject)
                                        .Id((Id)id)
                                        .Document(obj));
                                }
                                var bulkResponse = client.Bulk(descriptor);
                                await _noteBusiness.ManageSchedulerLog(new SchedulerLogViewModel
                                {
                                    NoteSubject = api.NoteSubject,
                                    fromDate = fromDate.ToString("yyyy-MM-dd HH:mm:ss"),
                                    toDate = batchToDate.ToString("yyyy-MM-dd HH:mm:ss"),
                                    districtCode = null,
                                    response = bulkResponse.ToString(),
                                    error = bulkResponse.Errors.ToString(),
                                    success = true,
                                    ActiveUserId = _userContext.UserId
                                });
                                return bulkResponse.ToString();
                            }
                            else
                            {
                                await _noteBusiness.ManageSchedulerLog(new SchedulerLogViewModel
                                {
                                    NoteSubject = api.NoteSubject,
                                    fromDate = fromDate.ToString("yyyy-MM-dd HH:mm:ss"),
                                    toDate = batchToDate.ToString("yyyy-MM-dd HH:mm:ss"),
                                    districtCode = null,
                                    response = "[]",
                                    error = "",
                                    success = true,
                                    ActiveUserId = _userContext.UserId
                                });
                                return "";
                            }

                        }
                        else
                        {
                            await _noteBusiness.ManageSchedulerLog(new SchedulerLogViewModel
                            {
                                NoteSubject = api.NoteSubject,
                                fromDate = fromDate.ToString("yyyy-MM-dd HH:mm:ss"),
                                toDate = batchToDate.ToString("yyyy-MM-dd HH:mm:ss"),
                                districtCode = null,
                                response = await response1.Content.ReadAsStringAsync(),
                                error = response1.ReasonPhrase.ToString(),
                                success = false,
                                ActiveUserId = _userContext.UserId
                            });
                            return response1.IsSuccessStatusCode.ToString();
                        }
                        
                    }

                }
            }
            catch (Exception e)
            {
                await _noteBusiness.ManageSchedulerLog(new SchedulerLogViewModel
                {
                    NoteSubject = api.NoteSubject,
                    fromDate = _fromdate,
                    toDate = _todate,
                    districtCode = null,
                    response = null,
                    error = e.ToString(),
                    success = false,
                    ActiveUserId = _userContext.UserId
                });
                Console.WriteLine(e.ToString());
                throw;
            }

        }
        [LogFailure]
        [Queue("alpha")]
        public async Task<bool> ExecuteAlertRule()
        {
            var _noteBusiness = (INoteBusiness)_services.GetService(typeof(INoteBusiness));
            var template = await _noteBusiness.GetSingle(x => x.TemplateCode == "NOTIFICATION_ALERT");
            if (template.IsNotNull())
            {
                var rules = await _noteBusiness.GetAlertRulelist();
                foreach (var rule in rules.Where(x => x.isReporting == false))
                {
                    try
                    {
                        RecurringJob.RemoveIfExists("AlertJobs-" + rule.NoteSubject);
                        RecurringJob.AddOrUpdate<Synergy.App.Business.HangfireScheduler>("AlertJobs-" + rule.NoteSubject, x => x.GenerateAlert(rule.Id), rule.evaluateTime);
                    }
                    catch (Exception)
                    {

                    }

                }
            }

            return true;
        }

        [LogFailure]
        [DisableConcurrentExecution(timeoutInSeconds: 60)]
        [Queue("alpha")]
        [AutomaticRetry(Attempts = 0)]
        public async Task GenerateAlert(string ruleId)
        {
            var cubeJs = ApplicationConstant.AppSettings.CubeJsUrl(_configuration);
            var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
            var settings = new ConnectionSettings(new Uri(eldbUrl));
            var client = new ElasticClient(settings);
            BulkDescriptor descriptor = new BulkDescriptor();
            var filterList = new List<BuilderFilterViewModel>();
            bool isValid = true;
            var _noteBusiness = (INoteBusiness)_services.GetService(typeof(INoteBusiness));
            var rule = await _noteBusiness.GetNotificationALertDetails(ruleId);
            if (rule.columnReferenceId.IsNotNullAndNotEmpty())
            {
                var content = @"{""measures"": [""#MEASURE#""],""dimensions"": #DIMENSION#,""timeDimensions"": [#TIMEDIMENSIONS#],""limit"": #LIMIT#,""order"": [[""Dial100Data.event_time"",""desc""]],""filters"": [" + rule.cubeJsFilter + "]}";
                content = content.Replace("#MEASURE#", rule.queryTableId);
                content = content.Replace("#DIMENSION#", rule.queryColumns);
                content = content.Replace("#TIMEDIMENSIONS#", rule.timeDimensionFilter);
                content = content.Replace("#LIMIT#", rule.limit.IsNullOrEmpty() ? "10" : rule.limit);
                var tempObj = JsonConvert.DeserializeObject(content);
                var str = JsonConvert.SerializeObject(tempObj);
                var handler = new HttpClientHandler();
                handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                handler.ServerCertificateCustomValidationCallback =
                    (httpRequestMessage, cert, cetChain, policyErrors) =>
                    {
                        return true;
                    };
                using (var httpClient = new HttpClient(handler))
                {
                    var url1 = $@"{cubeJs}cubejs-api/v1/load?query={str}";
                    var address = new Uri(url1);
                    var response = await httpClient.GetAsync(address);
                    if (response.IsSuccessStatusCode)
                    {
                        var _jsondata = await response.Content.ReadAsStringAsync();
                        var _dataToken = JToken.Parse(_jsondata);
                        var _data = _dataToken.SelectToken("data");
                        if (_data.Count() > 0)
                        {
                            var tableName = rule.queryTableId.Split('.')[0] + ".";
                            var jsonStr = _data.ToString().Replace(tableName, "");
                            var datalist = JsonConvert.DeserializeObject<List<Dial100ViewModel>>(jsonStr);
                            if (datalist.Count > rule.conditionValue.ToSafeInt())
                            {
                                if (rule.groupFilters != "[]")
                                {
                                    var filters = JsonConvert.DeserializeObject<dynamic>(rule.groupFilters);
                                    foreach (var item in filters.rules)
                                    {
                                        var f = item["field"].Value;
                                        var o = item["operator"].Value;
                                        var v = item["value"].Value;
                                        var filter = new BuilderFilterViewModel { Field = f.Replace(tableName, ""), Operator = o, Value = v };
                                        filterList.Add(filter);
                                    }
                                }
                                if (rule.groupbyColumns.IsNotNullAndNotEmpty())
                                {
                                    var grpColArr = rule.groupbyColumns.Replace(tableName, "").Split(',');
                                    if (grpColArr.Length == 1)
                                    {
                                        var grpDatalist = datalist.GroupBy(x => x.GetType().GetProperty(grpColArr[0]).GetValue(x, null)).ToList();
                                        var maxCount = 0;
                                        var year = DateTime.Now.Year;
                                        var content3 = @"{""query"": {""range"": {""alert_date"": { ""gte"": ""#YEAR#"",""lte"": ""now"",""format"":""yyyy""}}}}";
                                        content3 = content3.Replace("#YEAR#", year.ToString());
                                        var stringContent3 = new StringContent(content3, Encoding.UTF8, "application/json");
                                        var url3 = eldbUrl + "iip_alert_data/_search?pretty=true";
                                        var address3 = new Uri(url3);
                                        var response3 = await httpClient.PostAsync(address3, stringContent3);
                                        if (response3.IsSuccessStatusCode)
                                        {
                                            var json3 = await response3.Content.ReadAsStringAsync();
                                            var data3 = JToken.Parse(json3);
                                            if (data3.IsNotNull())
                                            {
                                                var hits = data3.SelectToken("hits");
                                                if (hits.IsNotNull())
                                                {
                                                    var total = hits.SelectToken("total");
                                                    if (total.IsNotNull())
                                                    {
                                                        var tt = total.First();
                                                        var countstr = tt.Last().ToString();
                                                        maxCount = countstr.ToSafeInt();

                                                    }
                                                }
                                            }
                                        }
                                        else if (response3.ReasonPhrase == "Not Found")
                                        {
                                            maxCount = 0;
                                        }
                                        else
                                        {
                                            return;
                                        }
                                        foreach (var dataItem in grpDatalist)
                                        {
                                            var dataItemList = dataItem.ToList();
                                            foreach (var filter in filterList)
                                            {

                                                if (filter.Operator == "exist")
                                                {
                                                    var distList = dataItemList.DistinctBy(x => x.GetType().GetProperty(filter.Field).GetValue(x, null)).Select(x => x.GetType().GetProperty(filter.Field).GetValue(x, null)).ToList();
                                                    var isExist = distList.Where(x => x.ToString() == filter.Value).Any();
                                                    if (isExist)
                                                    {

                                                        isValid = true;
                                                    }
                                                    else
                                                    {
                                                        isValid = false;
                                                        break;
                                                    }
                                                }
                                                else if (filter.Operator == "contains")
                                                {

                                                }
                                                else if (filter.Operator == "greater than")
                                                {
                                                    var distCount = dataItemList.DistinctBy(x => x.GetType().GetProperty(filter.Field).GetValue(x, null)).Count();
                                                    if (distCount > filter.Value.ToSafeInt())
                                                    {
                                                        isValid = true;
                                                    }
                                                    else
                                                    {
                                                        isValid = false;
                                                        break;
                                                    }
                                                }
                                                else if (filter.Operator == "less than")
                                                {
                                                    var distCount = dataItemList.DistinctBy(x => x.GetType().GetProperty(filter.Field).GetValue(x, null)).Count();
                                                    if (distCount < filter.Value.ToSafeInt())
                                                    {
                                                        isValid = true;
                                                    }
                                                    else
                                                    {
                                                        isValid = false;
                                                        break;
                                                    }

                                                }
                                            }
                                            if (isValid && dataItemList.Count() > rule.conditionValue.ToSafeInt())
                                            {
                                                dynamic obj = new System.Dynamic.ExpandoObject();
                                                var list = dataItemList;
                                                var evnetTime = list.First().event_time;
                                                if (evnetTime.IsNotNull())
                                                {
                                                    var event_date = evnetTime.ToSafeDateTime();
                                                    ExpandoAddProperty(obj, "event_datetime", event_date);

                                                }
                                                foreach (var property in rule.GetType().GetProperties())
                                                {
                                                    var _key = property.Name;
                                                    var _value = property.GetValue(rule);
                                                    if (_key == "summary" || _key == "colorCode" || _key == "NoteDescription" || _key == "queryTableId")
                                                    {
                                                        ExpandoAddProperty(obj, _key, _value);
                                                    }

                                                }
                                                var ids = new List<string>();
                                                foreach (var item in list)
                                                {
                                                    ids.Add(item.event_number);
                                                }
                                                if (ids.Count > 0)
                                                {
                                                    var filIds = String.Join(",", ids);
                                                    ExpandoAddProperty(obj, "sourceIds", filIds);
                                                }
                                                var alertId = Guid.NewGuid().ToString();
                                                ExpandoAddProperty(obj, "alert_date_utc", DateTime.UtcNow);
                                                ExpandoAddProperty(obj, "alert_date", Convert.ToDateTime(DateTime.UtcNow.AddHours(5.5).ToString()));
                                                ExpandoAddProperty(obj, "alertid", alertId);
                                                ExpandoAddProperty(obj, "isFalseEvent", false);
                                                ExpandoAddProperty(obj, "isRead", false);
                                                ExpandoAddProperty(obj, "isVisible", true);
                                                var exCount = maxCount;
                                                maxCount = await CheckAlertNoExist(maxCount);
                                                if (exCount == maxCount)
                                                {
                                                    return;
                                                }
                                                ExpandoAddProperty(obj, "year", year);
                                                ExpandoAddProperty(obj, "y_count", maxCount);
                                                ExpandoAddProperty(obj, "alert_number", (year + "-" + (maxCount)).ToString());
                                                descriptor.Index<object>(i => i
                                                    .Index("iip_alert_data")
                                                    .Id((Id)alertId)
                                                    .Document(obj));

                                                var bulkResponse = client.Bulk(descriptor);
                                                if (bulkResponse.ApiCall.Success)
                                                {

                                                    var content2 = @"{""size"":10000, ""query"": {""bool"" : {""must"": { ""match_all"" : {}},""filter"" : [{ ""terms"" :{""_id"":#IDS#}}]}},""script"": { ""source"": ""ctx._source['isAlerted'] =true""} }";
                                                    var filIds = "[\"" + String.Join("\",\"", ids) + "\"]";
                                                    content2 = content2.Replace("#IDS#", filIds);
                                                    var stringContent2 = new StringContent(content2, Encoding.UTF8, "application/json");
                                                    var url2 = eldbUrl + "dail100/_update_by_query";
                                                    var address2 = new Uri(url2);
                                                    var response2 = await httpClient.PostAsync(address2, stringContent2);

                                                }
                                            }
                                        }
                                    }
                                    else
                                    {

                                    }

                                }
                                else
                                {
                                    foreach (var filter in filterList)
                                    {

                                        if (filter.Operator == "exist")
                                        {
                                            var distList = datalist.DistinctBy(x => x.GetType().GetProperty(filter.Field).GetValue(x, null)).Select(x => x.GetType().GetProperty(filter.Field).GetValue(x, null)).ToList();
                                            var isExist = distList.Where(x => x.ToString() == filter.Value).Any();
                                            if (isExist)
                                            {

                                                isValid = true;
                                            }
                                            else
                                            {
                                                isValid = false;
                                                break;
                                            }
                                        }
                                        else if (filter.Operator == "contains")
                                        {

                                        }
                                        else if (filter.Operator == "greater than")
                                        {
                                            var distCount = datalist.DistinctBy(x => x.GetType().GetProperty(filter.Field).GetValue(x, null)).Count();
                                            if (distCount > filter.Value.ToSafeInt())
                                            {
                                                isValid = true;
                                            }
                                            else
                                            {
                                                isValid = false;
                                                break;
                                            }
                                        }
                                        else if (filter.Operator == "less than")
                                        {
                                            var distCount = datalist.DistinctBy(x => x.GetType().GetProperty(filter.Field).GetValue(x, null)).Count();
                                            if (distCount < filter.Value.ToSafeInt())
                                            {
                                                isValid = true;
                                            }
                                            else
                                            {
                                                isValid = false;
                                                break;
                                            }

                                        }
                                    }
                                    if (isValid)
                                    {
                                        var maxCount = 0;
                                        var year = DateTime.Now.Year;
                                        var content3 = @"{""query"": {""range"": {""alert_date"": { ""gte"": ""#YEAR#"",""lte"": ""now"",""format"":""yyyy""}}}}";
                                        content3 = content3.Replace("#YEAR#", year.ToString());
                                        var stringContent3 = new StringContent(content3, Encoding.UTF8, "application/json");
                                        var url3 = eldbUrl + "iip_alert_data/_search?pretty=true";
                                        var address3 = new Uri(url3);
                                        var response3 = await httpClient.PostAsync(address3, stringContent3);
                                        if (response3.IsSuccessStatusCode)
                                        {
                                            var json3 = await response3.Content.ReadAsStringAsync();
                                            var data3 = JToken.Parse(json3);
                                            if (data3.IsNotNull())
                                            {
                                                var hits = data3.SelectToken("hits");
                                                if (hits.IsNotNull())
                                                {
                                                    var total = hits.SelectToken("total");
                                                    if (total.IsNotNull())
                                                    {
                                                        var tt = total.First();
                                                        var countstr = tt.Last().ToString();
                                                        maxCount = countstr.ToSafeInt();

                                                    }
                                                }
                                            }
                                        }
                                        else if (response3.ReasonPhrase == "Not Found")
                                        {
                                            maxCount = 0;
                                        }
                                        else
                                        {
                                            return;
                                        }
                                        while (datalist.Count() > 0)
                                        {
                                            dynamic obj = new System.Dynamic.ExpandoObject();
                                            var list = datalist.Take(rule.conditionValue.ToSafeInt() + 1).ToList();
                                            datalist = datalist.Except(list).ToList();
                                            if (list.Count() > rule.conditionValue.ToSafeInt())
                                            {
                                                var evnetTime = list.First().event_time;
                                                if (evnetTime.IsNotNull())
                                                {
                                                    var event_date = evnetTime.ToSafeDateTime();
                                                    ExpandoAddProperty(obj, "event_datetime", event_date);

                                                }
                                                foreach (var property in rule.GetType().GetProperties())
                                                {
                                                    var _key = property.Name;
                                                    var _value = property.GetValue(rule);
                                                    if (_key == "summary" || _key == "colorCode" || _key == "NoteDescription" || _key == "queryTableId")
                                                    {
                                                        ExpandoAddProperty(obj, _key, _value);
                                                    }

                                                }
                                                var ids = new List<string>();
                                                foreach (var item in list)
                                                {
                                                    ids.Add(item.event_number);
                                                }
                                                if (ids.Count > 0)
                                                {
                                                    var filIds = String.Join(",", ids);
                                                    ExpandoAddProperty(obj, "sourceIds", filIds);
                                                }
                                                var alertId = Guid.NewGuid().ToString();
                                                ExpandoAddProperty(obj, "alert_date_utc", DateTime.UtcNow);
                                                ExpandoAddProperty(obj, "alert_date", Convert.ToDateTime(DateTime.UtcNow.AddHours(5.5).ToString()));
                                                ExpandoAddProperty(obj, "alertid", alertId);
                                                ExpandoAddProperty(obj, "isFalseEvent", false);
                                                ExpandoAddProperty(obj, "isRead", false);
                                                ExpandoAddProperty(obj, "isVisible", true);
                                                var exCount = maxCount;
                                                maxCount = await CheckAlertNoExist(maxCount);
                                                if (exCount == maxCount)
                                                {
                                                    return;
                                                }
                                                ExpandoAddProperty(obj, "year", year);
                                                ExpandoAddProperty(obj, "y_count", maxCount);
                                                ExpandoAddProperty(obj, "alert_number", (year + "-" + (maxCount)).ToString());
                                                descriptor.Index<object>(i => i
                                                    .Index("iip_alert_data")
                                                    .Id((Id)alertId)
                                                    .Document(obj));

                                                var bulkResponse = client.Bulk(descriptor);
                                                if (bulkResponse.ApiCall.Success)
                                                {

                                                    var content2 = @"{""size"":10000, ""query"": {""bool"" : {""must"": { ""match_all"" : {}},""filter"" : [{ ""terms"" :{""_id"":#IDS#}}]}},""script"": { ""source"": ""ctx._source['isAlerted'] =true""} }";
                                                    var filIds = "[\"" + String.Join("\",\"", ids) + "\"]";
                                                    content2 = content2.Replace("#IDS#", filIds);
                                                    var stringContent2 = new StringContent(content2, Encoding.UTF8, "application/json");
                                                    var url2 = eldbUrl + "dail100/_update_by_query";
                                                    var address2 = new Uri(url2);
                                                    var response2 = await httpClient.PostAsync(address2, stringContent2);

                                                }
                                            }
                                        }

                                    }
                                }
                            }
                        }


                    }
                }

            }


        }
        private async Task<int> CheckAlertNoExist(int no)
        {
            var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
            var settings = new ConnectionSettings(new Uri(eldbUrl));
            var client = new ElasticClient(settings);
            var year = DateTime.Now.Year;
            var content = @"{""size"": 1000,""query"":{""bool"":{""must"":[{""simple_query_string"" : {""query"": ""#ALERTNO#"",""fields"": [""alert_number""],""default_operator"": ""and""}}]}}}";
            content = content.Replace("#ALERTNO#", (year + "-" + (no + 1)).ToString());
            var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
            var url = eldbUrl + "iip_alert_data/_search?pretty=true";
            var address = new Uri(url);
            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                };
            using (var httpClient = new HttpClient(handler))
            {
                var response = await httpClient.PostAsync(address, stringContent);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var data = JToken.Parse(json);
                    if (data.IsNotNull())
                    {
                        var hits = data.SelectToken("hits");
                        if (hits.IsNotNull())
                        {
                            var total = hits.SelectToken("total");
                            if (total.IsNotNull())
                            {
                                var tt = total.First();
                                var countstr = tt.Last().ToString();
                                var cc = countstr.ToSafeInt();
                                if (cc > 0)
                                {
                                    return await CheckAlertNoExist(no + 1);
                                }
                                else
                                {
                                    return no + 1;
                                }
                            }
                        }
                    }
                }
                else if (response.ReasonPhrase == "Not Found")
                {
                    return no + 1;
                }
                else
                {
                    return no;
                }
            }
            return no;
        }
        [LogFailure]
        public async Task<bool> ExecuteRssFeeds()
        {
            var _noteBusiness = (INoteBusiness)_services.GetService(typeof(INoteBusiness));
            var template = await _noteBusiness.GetSingle(x => x.TemplateCode == "RSS_FEED_MASTER");
            if (template.IsNotNull())
            {
                var feeds = await _noteBusiness.GetRssFeedData();
                foreach (var feed in feeds)
                {
                    await MigrateRssFeed(feed);
                }

            }


            return true;
        }
        [LogFailure]
        [AutomaticRetry(Attempts = 2)]
        public async Task<bool> MigrateRssFeed(RssFeedViewModel _feed)
        {
            var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
            var settings = new ConnectionSettings(new Uri(eldbUrl));
            var client = new ElasticClient(settings);
            BulkDescriptor descriptor = new BulkDescriptor();
            var reader = System.Xml.XmlReader.Create(_feed.feedUrl);
            var feed = SyndicationFeed.Load(reader);
            foreach (var i in feed.Items)
            {
                var _post = new NewsFeedsViewModel
                {
                    title = i.Title.Text,
                    message = i.Summary.Text,
                    link = i.Links.Count > 0 ? i.Links[0].Uri.OriginalString : null,
                    published = i.PublishDate.DateTime,
                    author = i.Authors.Count > 0 ? i.Authors[0].Name : null,
                    name = _feed.NoteDescription,
                    //_index = _feed.feedName
                };
                var _id = i.Id.IsNotNullAndNotEmpty() ? i.Id : i.Links[0].Uri.OriginalString;
                descriptor.Index<object>(z => z
                                .Index("rssfeeds")
                                .Id((Id)_id)
                                .Document(_post));

            }
            var bulkResponse = client.Bulk(descriptor);

            return true;

        }
        private void ExpandoAddProperty(System.Dynamic.ExpandoObject expando, string propertyName, object propertyValue)
        {
            // ExpandoObject supports IDictionary so we can extend it like this
            var expandoDict = expando as IDictionary<string, object>;
            if (expandoDict.ContainsKey(propertyName))
                expandoDict[propertyName] = propertyValue;
            else
                expandoDict.Add(propertyName, propertyValue);
        }
        [LogFailure]
        public async Task TrendingSocialDataMigrationToElasticDB()
        {
            var _noteBusiness = (INoteBusiness)_services.GetService(typeof(INoteBusiness));
            var template = await _noteBusiness.GetSingle(x => x.TemplateCode == "TRENDING_LOCATION");
            if (template.IsNotNull())
            {
                var list = await _noteBusiness.GetAllTrendingLocation();
                foreach (var item in list)
                {
                    //await TrendingSocialDataMigration(item);
                    await TrendingTwitterDataMigration(item.NoteSubject);
                    await TrendingYoutubeDataMigration(item.NoteSubject,item.latitude,item.longitude);
                    await TrendingFacebookDataMigration(item.NoteSubject);
                    //await TrendingInstagramDataMigration(item.NoteSubject);
                }
            }


        }
        [LogFailure]
        public async Task TrendingSocialDataMigrationToElasticDB1()
        {
            var monitor = JobStorage.Current.GetMonitoringApi();
            var enqueueList = monitor.EnqueuedJobs("beta", 0, 2500);
            var _noteBusiness = (INoteBusiness)_services.GetService(typeof(INoteBusiness));
            var template = await _noteBusiness.GetSingle(x => x.TemplateCode == "TRENDING_LOCATION");
            if (template.IsNotNull())
            {
                var list = await _noteBusiness.GetAllTrendingLocation();
                foreach (var item in list)
                {
                    if (enqueueList.Where(x => x.Value.Job.Args[0].ToString() == item.NoteSubject && x.Value.Job.Method.Name == "TrendingFacebookDataMigration").Any())
                    {
                        continue;
                    }
                    else
                    {
                        BackgroundJob.Enqueue<Synergy.App.Business.HangfireScheduler>(x => x.TrendingFacebookDataMigration(item.NoteSubject));

                    }
                    if (enqueueList.Where(x => x.Value.Job.Args[0].ToString() == item.NoteSubject && x.Value.Job.Method.Name == "TrendingInstagramDataMigration").Any())
                    {
                        continue;
                    }
                    else
                    {
                        //BackgroundJob.Enqueue<Synergy.App.Business.HangfireScheduler>(x => x.TrendingInstagramDataMigration(item.NoteSubject));

                    }                    
                    
                }
            }


        }
        [LogFailure]
        public async Task TrendingSocialDataMigrationToElasticDB2()
        {
            var monitor = JobStorage.Current.GetMonitoringApi();
            var enqueueList = monitor.EnqueuedJobs("alpha", 0, 2500);
            var _noteBusiness = (INoteBusiness)_services.GetService(typeof(INoteBusiness));
            var template = await _noteBusiness.GetSingle(x => x.TemplateCode == "TRENDING_LOCATION");
            if (template.IsNotNull())
            {
                var list = await _noteBusiness.GetAllTrendingLocation();
                foreach (var item in list)
                {
                    if (enqueueList.Where(x => x.Value.Job.Args[0].ToString() ==item.NoteSubject && x.Value.Job.Method.Name== "TrendingTwitterDataMigration").Any())
                    {
                        continue;
                    }
                    else
                    {
                        BackgroundJob.Enqueue<Synergy.App.Business.HangfireScheduler>(x => x.TrendingTwitterDataMigration(item.NoteSubject));

                    }
                    if (enqueueList.Where(x => x.Value.Job.Args[0].ToString() == item.NoteSubject && x.Value.Job.Method.Name == "TrendingYoutubeDataMigration").Any())
                    {
                        continue;
                    }
                    else
                    {
                        BackgroundJob.Enqueue<Synergy.App.Business.HangfireScheduler>(x => x.TrendingYoutubeDataMigration(item.NoteSubject,item.latitude,item.longitude));

                    }             
                }
            }


        }
        [LogFailure]
        [AutomaticRetry(Attempts = 2)]
        public async Task TrendingSocialDataMigration(TrendingLocationViewModel api)
        {
            try
            {
                var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
                var settings = new ConnectionSettings(new Uri(eldbUrl));
                var client = new ElasticClient(settings);
                if (api.socialMediaType == SocialMediaTypeEnum.Twitter)
                {
                    var list = new List<TwitterTrendingViewModel>();
                    var socialApiUrl = ApplicationConstant.AppSettings.SocialApiUrl(_configuration);
                    var url = socialApiUrl + "twitter_trendings?location=" + api.NoteSubject;
                    using (var httpClient = new HttpClient())
                    {
                        var address = new Uri(url);
                        var response = await httpClient.GetAsync(address);
                        var jsonStr = await response.Content.ReadAsStringAsync();
                        var json = JToken.Parse(jsonStr);
                        var items = json.SelectToken("data");
                        if (items.IsNotNull())
                        {
                            BulkDescriptor descriptor = new BulkDescriptor();
                            foreach (var item in items)
                            {

                                var dataStr = JsonConvert.SerializeObject(item);
                                var model = JsonConvert.DeserializeObject<TwitterTrendingViewModel>(dataStr);
                                if (model.IsNotNull())
                                {
                                    model.location = api.NoteSubject;
                                    model.created_date = DateTime.Now;
                                    var id = model.name + model.created_date.ToString();
                                    var indexName = "trending_" + api.socialMediaType.ToString().ToLower();
                                    descriptor.Index<object>(i => i
                                            .Index(indexName)
                                            .Id((Id)id)
                                            .Document(model));
                                }

                            }
                            var bulkResponse = client.Bulk(descriptor);
                        }

                    }
                }
                else if (api.socialMediaType == SocialMediaTypeEnum.Youtube)
                {
                    var url = "https://youtube.googleapis.com/youtube/v3/search?part=snippet,id&location=" + api.latitude + "," + api.longitude + "&locationRadius=50km&maxResults=10&q=" + api.NoteSubject + "&type=video%2Clist&key=AIzaSyAVKFSEz4Uk7jTUlA-VRjukTh9nMiz_Y60";
                    using (var httpClient = new HttpClient())
                    {
                        //httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "AAAAAAAAAAAAAAAAAAAAALGQPQEAAAAAO%2F6Bh8iW9lNBaTmsAtY%2BsPYUgjc%3DZVbyg27R3pokVj1OXr9wV7uorkzJRdE3qtF7mKHc8K3whRR8jl");
                        var address = new Uri(url);
                        var response = await httpClient.GetAsync(address);
                        var jsonStr = await response.Content.ReadAsStringAsync();
                        var json = JToken.Parse(jsonStr);
                        var items = json.SelectToken("items");
                        if (items.IsNotNull())
                        {
                            BulkDescriptor descriptor = new BulkDescriptor();
                            foreach (var item in items)
                            {
                                var source = item.SelectToken("id");
                                var id = string.Empty;
                                if (source.IsNotNull())
                                {
                                    var videoid = source.SelectToken("videoId");
                                    if (videoid.IsNotNull())
                                    {
                                        id = videoid.Value<string>();
                                    }

                                }
                                var _snippet = item.SelectToken("snippet");


                                var dataStr = JsonConvert.SerializeObject(_snippet);
                                var model = JsonConvert.DeserializeObject<YoutubeTrendingViewModel>(dataStr);
                                var _statistics = item.SelectToken("statistics");
                                var dataStr1 = JsonConvert.SerializeObject(_statistics);
                                var model1 = JsonConvert.DeserializeObject<YoutubeTrendingViewModel>(dataStr1);
                                if (model.IsNotNull())
                                {
                                    model.videoId = id;
                                    model.location = api.NoteSubject;
                                    if (model1.IsNotNull())
                                    {
                                        model.likeCount = model1.likeCount;
                                        model.viewCount = model1.viewCount;
                                        model.favoriteCount = model1.favoriteCount;
                                        model.commentCount = model1.commentCount;

                                    }
                                    try
                                    {
                                        var _thumbnail = _snippet.SelectToken("thumbnails");
                                        var _default = _thumbnail.SelectToken("default");
                                        var _url = _default.SelectToken("url");
                                        if (_url.IsNotNull())
                                        {
                                            model.thumbnailUrl = _url.Value<string>();
                                        }
                                    }
                                    catch (Exception)
                                    {


                                    }
                                    var indexName = "trending_" + api.socialMediaType.ToString().ToLower();
                                    descriptor.Index<object>(i => i
                                        .Index(indexName)
                                        .Id((Id)id)
                                        .Document(model));
                                }

                            }
                            var bulkResponse = client.Bulk(descriptor);
                        }

                    }
                }


            }
            catch (Exception e)
            {
                throw;
            }

        }
        [LogFailure]
        [AutomaticRetry(Attempts = 2)]
        [Queue("alpha")]
        public async Task TrendingTwitterDataMigration(string location)
        {
            try
            {
                var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
                var settings = new ConnectionSettings(new Uri(eldbUrl));
                var client = new ElasticClient(settings);                
                var list = new List<TwitterTrendingViewModel>();
                var socialApiUrl = ApplicationConstant.AppSettings.SocialApiUrl(_configuration);
                var url = socialApiUrl + "twitter_trendings?location=" + location;
                using (var httpClient = new HttpClient())
                    {
                        var address = new Uri(url);
                        var response = await httpClient.GetAsync(address);
                        var jsonStr = await response.Content.ReadAsStringAsync();
                        var json = JToken.Parse(jsonStr);
                        var items = json.SelectToken("data");
                        if (items.IsNotNull())
                        {
                            BulkDescriptor descriptor = new BulkDescriptor();
                            foreach (var item in items)
                            {

                                var dataStr = JsonConvert.SerializeObject(item);
                                var model = JsonConvert.DeserializeObject<TwitterTrendingViewModel>(dataStr);
                                if (model.IsNotNull())
                                {
                                    model.location = location;
                                    model.created_date = DateTime.Now;
                                    var id = model.name + model.created_date.ToString();
                                    var indexName = "trending_twitter";
                                    descriptor.Index<object>(i => i
                                            .Index(indexName)
                                            .Id((Id)id)
                                            .Document(model));
                                }

                            }
                            var bulkResponse = client.Bulk(descriptor);
                        }

                    }
                
            }
            catch (Exception e)
            {
                throw;
            }

        }
        [LogFailure]
        [AutomaticRetry(Attempts = 2)]
        [Queue("alpha")]
        public async Task TrendingYoutubeDataMigration(string location,string lat,string lon)
        {
            try
            {
                var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
                var settings = new ConnectionSettings(new Uri(eldbUrl));
                var client = new ElasticClient(settings);               
                var url = "https://youtube.googleapis.com/youtube/v3/search?part=snippet,id&location=" + lat + "," + lon + "&locationRadius=50km&maxResults=10&q=" + location + "&type=video%2Clist&key=AIzaSyAVKFSEz4Uk7jTUlA-VRjukTh9nMiz_Y60";
                using (var httpClient = new HttpClient())
                    {
                        //httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "AAAAAAAAAAAAAAAAAAAAALGQPQEAAAAAO%2F6Bh8iW9lNBaTmsAtY%2BsPYUgjc%3DZVbyg27R3pokVj1OXr9wV7uorkzJRdE3qtF7mKHc8K3whRR8jl");
                        var address = new Uri(url);
                        var response = await httpClient.GetAsync(address);
                        var jsonStr = await response.Content.ReadAsStringAsync();
                        var json = JToken.Parse(jsonStr);
                        var items = json.SelectToken("items");
                        if (items.IsNotNull())
                        {
                            BulkDescriptor descriptor = new BulkDescriptor();
                            foreach (var item in items)
                            {
                                var source = item.SelectToken("id");
                                var id = string.Empty;
                                if (source.IsNotNull())
                                {
                                    var videoid = source.SelectToken("videoId");
                                    if (videoid.IsNotNull())
                                    {
                                        id = videoid.Value<string>();
                                    }

                                }
                                var _snippet = item.SelectToken("snippet");


                                var dataStr = JsonConvert.SerializeObject(_snippet);
                                var model = JsonConvert.DeserializeObject<YoutubeTrendingViewModel>(dataStr);
                                var _statistics = item.SelectToken("statistics");
                                var dataStr1 = JsonConvert.SerializeObject(_statistics);
                                var model1 = JsonConvert.DeserializeObject<YoutubeTrendingViewModel>(dataStr1);
                                if (model.IsNotNull())
                                {
                                    model.videoId = id;
                                    model.location = location;
                                    if (model1.IsNotNull())
                                    {
                                        model.likeCount = model1.likeCount;
                                        model.viewCount = model1.viewCount;
                                        model.favoriteCount = model1.favoriteCount;
                                        model.commentCount = model1.commentCount;

                                    }
                                    try
                                    {
                                        var _thumbnail = _snippet.SelectToken("thumbnails");
                                        var _default = _thumbnail.SelectToken("default");
                                        var _url = _default.SelectToken("url");
                                        if (_url.IsNotNull())
                                        {
                                            model.thumbnailUrl = _url.Value<string>();
                                        }
                                    }
                                    catch (Exception)
                                    {


                                    }
                                    var indexName = "trending_youtube";
                                    descriptor.Index<object>(i => i
                                        .Index(indexName)
                                        .Id((Id)id)
                                        .Document(model));
                                }

                            }
                            var bulkResponse = client.Bulk(descriptor);
                        }

                    }
            }
            catch (Exception e)
            {
                throw;
            }

        }
        [LogFailure]
        [AutomaticRetry(Attempts = 2)]
        [Queue("beta")]
        public async Task TrendingFacebookDataMigration(string location)
        {
            try
            {
                var _noteBusiness = (INoteBusiness)_services.GetService(typeof(INoteBusiness));
                var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
                var settings = new ConnectionSettings(new Uri(eldbUrl));
                var client = new ElasticClient(settings);                
                var list = new List<TwitterTrendingViewModel>();
                var socialApiUrl = ApplicationConstant.AppSettings.SocialApiUrl(_configuration);
                var credential = await _noteBusiness.GetFacebookCredential();                
                var url = socialApiUrl + "facebook_keyword_login?keyword=" + location + "&no_of_pages=1&username=" + credential.Name + "&password=" + credential.Code + "&post_keyword=top_posts";
                using (var httpClient = new HttpClient())
                {
                    httpClient.Timeout = TimeSpan.FromSeconds(500);
                    var address = new Uri(url);
                    var response = await httpClient.GetAsync(address);
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonStr = await response.Content.ReadAsStringAsync();
                        var json = JToken.Parse(jsonStr);
                        var items = json.SelectToken("data");                        
                        if (items.IsNotNull())
                        {
                            BulkDescriptor descriptor = new BulkDescriptor();
                            foreach (var item in items)
                            {

                                var dataStr = JsonConvert.SerializeObject(item);
                                var model = JsonConvert.DeserializeObject<FacebookPostViewModel>(dataStr);
                                var _polarity = item.SelectToken("post_msg_polarity");
                                var polarity = JsonConvert.SerializeObject(_polarity);
                                var polarity_model = JsonConvert.DeserializeObject<polairty>(polarity);
                                if (polarity_model.IsNotNull())
                                {
                                    model.pos = polarity_model.pos;
                                    model.neg = polarity_model.neg;
                                    model.neu = polarity_model.neu;
                                    model.compound = polarity_model.compound;
                                }
                                if (model.IsNotNull())
                                {
                                    var id = model.post_url;
                                    var indexName = "trending_facebook";
                                    model.keyword = location;
                                    descriptor.Index<object>(i => i
                                            .Index(indexName)
                                            .Id((Id)id)
                                            .Document(model));
                                }

                            }
                            var bulkResponse = client.Bulk(descriptor);
                        }

                        
                    }


                }

            }
            catch (Exception e)
            {
                throw;
            }

        }
        [LogFailure]
        [AutomaticRetry(Attempts = 2)]
        [Queue("beta")]
        public async Task TrendingInstagramDataMigration(string location)
        {
            try
            {
                var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
                var settings = new ConnectionSettings(new Uri(eldbUrl));
                var client = new ElasticClient(settings);
                var socialApiUrl = ApplicationConstant.AppSettings.SocialApiUrl(_configuration);
                var url = socialApiUrl + "gsearch_keyword?keyword=instagram " + location + "&no_of_pages=1";
                using (var httpClient = new HttpClient())
                {
                    var address = new Uri(url);
                    var response = await httpClient.GetAsync(address);
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonStr = await response.Content.ReadAsStringAsync();
                        var json = JToken.Parse(jsonStr);
                        var data = json.SelectToken("data");
                        if (data.IsNotNull())
                        {

                            BulkDescriptor descriptor = new BulkDescriptor();
                            foreach (var item in data)
                            {

                                var dataStr = JsonConvert.SerializeObject(item);
                                var link = JsonConvert.DeserializeObject<string>(dataStr);
                                if (link.IsNotNull())
                                {
                                    var model = new InstagramPostViewModel();
                                    var id = link;
                                    model.url = link;
                                    model.created_date = DateTime.Now;
                                    model.keyword = location;
                                    var indexName = "trending_instagram";
                                    descriptor.Index<object>(i => i
                                            .Index(indexName)
                                            .Id((Id)id)
                                            .Document(model));
                                }

                            }
                            var bulkResponse = client.Bulk(descriptor);


                        }
                    }


                }

            }
            catch (Exception e)
            {
                throw;
            }

        }
        [LogFailure]
        public async Task SocialMediaKeywordDataMigrationToElasticDB()
        {
            var _noteBusiness = (INoteBusiness)_services.GetService(typeof(INoteBusiness));
            var list = await _noteBusiness.GetAllKeywordForHarvesting();
            if (list.Count>0)
            {                
                foreach (var item in list)
                {
                    await FacebookDataMigration(item);
                    await InstagramDataMigration(item);
                    await TwitterDataMigration(item);
                    await YoutubeDataMigration(item);

                }
            }


        }
        [LogFailure]
        public async Task SocialMediaKeywordDataMigrationToElasticDB1()
        {
            var monitor = JobStorage.Current.GetMonitoringApi();
            var enqueueList = monitor.EnqueuedJobs("beta", 0, 2500);
            var _noteBusiness = (INoteBusiness)_services.GetService(typeof(INoteBusiness));
            var list = await _noteBusiness.GetAllKeywordForHarvesting();
            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    if (enqueueList.Where(x => x.Value.Job.Args[0].ToString() == item && x.Value.Job.Method.Name == "FacebookDataMigration").Any())
                    {
                        continue;
                    }
                    else
                    {
                        BackgroundJob.Enqueue<Synergy.App.Business.HangfireScheduler>(x => x.FacebookDataMigration(item));

                    }
                    if (enqueueList.Where(x => x.Value.Job.Args[0].ToString() == item && x.Value.Job.Method.Name == "InstagramDataMigration").Any())
                    {
                        continue;
                    }
                    else
                    {
                        BackgroundJob.Enqueue<Synergy.App.Business.HangfireScheduler>(x => x.InstagramDataMigration(item));

                    }                   

                }
            }


        }
        public async Task SocialMediaKeywordDataMigrationToElasticDB2()
        {
            var monitor = JobStorage.Current.GetMonitoringApi();
            var enqueueList = monitor.EnqueuedJobs("alpha", 0, 2500);
            var _noteBusiness = (INoteBusiness)_services.GetService(typeof(INoteBusiness));
            var list = await _noteBusiness.GetAllKeywordForHarvesting();
            if (list.Count > 0)
            {
                foreach (var item in list)
                {
                    if (enqueueList.Where(x => x.Value.Job.Args[0].ToString() == item && x.Value.Job.Method.Name == "TwitterDataMigration").Any())
                    {
                        continue;
                    }
                    else
                    {
                        BackgroundJob.Enqueue<Synergy.App.Business.HangfireScheduler>(x => x.TwitterDataMigration(item));

                    }
                    if (enqueueList.Where(x => x.Value.Job.Args[0].ToString() == item && x.Value.Job.Method.Name == "YoutubeDataMigration").Any())
                    {
                        continue;
                    }
                    else
                    {
                        BackgroundJob.Enqueue<Synergy.App.Business.HangfireScheduler>(x => x.YoutubeDataMigration(item));

                    }                  

                }
            }


        }
        [LogFailure]
        [AutomaticRetry(Attempts = 1)]
        [Queue("beta")]
        public async Task FacebookDataMigration(string keyword)
        {
            try
            {
                var _noteBusiness = (INoteBusiness)_services.GetService(typeof(INoteBusiness));
                var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
                var settings = new ConnectionSettings(new Uri(eldbUrl));
                var client = new ElasticClient(settings);
                var socialApiUrl = ApplicationConstant.AppSettings.SocialApiUrl(_configuration);
                var credential = await _noteBusiness.GetFacebookCredential();
                //var url = socialApiUrl + "facebook_keyword?keyword=" + keyword + "&pages=1";
                var url = socialApiUrl + "facebook_keyword_login?keyword=" + keyword + "&no_of_pages=1&username="+ credential.Name + "&password="+ credential.Code + "&post_keyword=recent_posts";
                using (var httpClient = new HttpClient())
                {
                    httpClient.Timeout = TimeSpan.FromSeconds(500);
                    var address = new Uri(url);
                    var response = await httpClient.GetAsync(address);
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonStr = await response.Content.ReadAsStringAsync();
                        var json = JToken.Parse(jsonStr);
                        var items = json.SelectToken("data");
                        if (items.IsNotNull())
                        {
                            BulkDescriptor descriptor = new BulkDescriptor();
                            foreach (var item in items)
                            {
                                var dataStr = JsonConvert.SerializeObject(item);
                                var model = JsonConvert.DeserializeObject<FacebookPostViewModel>(dataStr);
                                var _polarity = item.SelectToken("post_msg_polarity");
                                var polarity = JsonConvert.SerializeObject(_polarity);
                                var polarity_model = JsonConvert.DeserializeObject<polairty>(polarity);
                                if (polarity_model.IsNotNull())
                                {
                                    model.pos = polarity_model.pos;
                                    model.neg = polarity_model.neg;
                                    model.neu = polarity_model.neu;
                                    model.compound = polarity_model.compound;
                                }
                                if (model.IsNotNull())
                                {
                                    var id = model.post_url;
                                    var indexName = "facebook_post";
                                    model.keyword = keyword;
                                    descriptor.Index<object>(i => i
                                            .Index(indexName)
                                            .Id((Id)id)
                                            .Document(model));
                                }

                            }
                            var bulkResponse = client.Bulk(descriptor);
                        }

                       
                    }


                }

            }
            catch (Exception e)
            {
                throw;
            }

        }
        [LogFailure]
        [AutomaticRetry(Attempts = 1)]
        [Queue("beta")]
        public async Task InstagramDataMigration(string keyword)
        {
            try
            {
                var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
                var settings = new ConnectionSettings(new Uri(eldbUrl));
                var client = new ElasticClient(settings);
                var socialApiUrl = ApplicationConstant.AppSettings.SocialApiUrl(_configuration);
                var url = socialApiUrl + "gsearch_keyword?keyword=instagram " + keyword + "&no_of_pages=1";
                using (var httpClient = new HttpClient())
                {
                    var address = new Uri(url);
                    var response = await httpClient.GetAsync(address);
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonStr = await response.Content.ReadAsStringAsync();
                        var json = JToken.Parse(jsonStr);
                        var data = json.SelectToken("data");
                        if (data.IsNotNull())
                        {

                            BulkDescriptor descriptor = new BulkDescriptor();
                            foreach (var item in data)
                            {

                                var dataStr = JsonConvert.SerializeObject(item);
                                var link = JsonConvert.DeserializeObject<string>(dataStr);
                                if (link.IsNotNull())
                                {
                                    var model = new InstagramPostViewModel();
                                    var id = link;
                                    model.url = link;
                                    model.created_date = DateTime.Now;
                                    model.keyword = keyword;
                                    var indexName = "instagram_post";
                                    descriptor.Index<object>(i => i
                                            .Index(indexName)
                                            .Id((Id)id)
                                            .Document(model));
                                }

                            }
                            var bulkResponse = client.Bulk(descriptor);


                        }
                    }


                }

            }
            catch (Exception e)
            {
                throw;
            }

        }
        [LogFailure]
        [AutomaticRetry(Attempts = 1)]
        [Queue("alpha")]
        public async Task TwitterDataMigration(string keyword)
        {
            try
            {
                var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
                var socialApiUrl = ApplicationConstant.AppSettings.SocialApiUrl(_configuration);
                var settings = new ConnectionSettings(new Uri(eldbUrl));
                var client = new ElasticClient(settings);
                //var url = "https://api.twitter.com/2/tweets/search/recent?query=" + keyword + "&max_results=10&expansions=attachments.poll_ids%2Cattachments.media_keys%2Cauthor_id%2Centities.mentions.username%2Cgeo.place_id%2Cin_reply_to_user_id%2Creferenced_tweets.id%2Creferenced_tweets.id.author_id&media.fields=duration_ms%2Cheight%2Cmedia_key%2Cpreview_image_url%2Ctype%2Curl%2Cwidth%2Cpublic_metrics%2Calt_text&place.fields=contained_within%2Ccountry%2Ccountry_code%2Cfull_name%2Cgeo%2Cid%2Cname%2Cplace_type&poll.fields=duration_minutes%2Cend_datetime%2Cid%2Coptions%2Cvoting_status&tweet.fields=attachments%2Cauthor_id%2Ccontext_annotations%2Cconversation_id%2Ccreated_at%2Centities%2Cgeo%2Cid%2Cin_reply_to_user_id%2Clang%2Cpublic_metrics%2Cpossibly_sensitive%2Creferenced_tweets%2Creply_settings%2Csource%2Ctext%2Cwithheld&user.fields=created_at%2Cdescription%2Centities%2Cid%2Clocation%2Cname%2Cpinned_tweet_id%2Cprofile_image_url%2Cprotected%2Cpublic_metrics%2Curl%2Cusername%2Cverified%2Cwithheld";                
                var url = socialApiUrl + "tweet_and_sentiment?keyword=" + keyword;
                using (var httpClient = new HttpClient())
                {
                    //httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "AAAAAAAAAAAAAAAAAAAAALGQPQEAAAAAO%2F6Bh8iW9lNBaTmsAtY%2BsPYUgjc%3DZVbyg27R3pokVj1OXr9wV7uorkzJRdE3qtF7mKHc8K3whRR8jl");
                    var address = new Uri(url);
                    var response = await httpClient.GetAsync(address);
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonStr = await response.Content.ReadAsStringAsync();
                        var json = JToken.Parse(jsonStr);
                        var data = json.SelectToken("data");
                        if (data.IsNotNull())
                        {
                            var items = data.SelectToken("data");
                            if (items.IsNotNull())
                            {
                                BulkDescriptor descriptor = new BulkDescriptor();
                                foreach (var item in items)
                                {

                                    var dataStr = JsonConvert.SerializeObject(item);
                                    var model = JsonConvert.DeserializeObject<Twitter1ViewModel>(dataStr);
                                    if (model.IsNotNull())
                                    {

                                        var _referenced_tweets = item.SelectToken("referenced_tweets");
                                        if (_referenced_tweets.IsNotNull() && _referenced_tweets.Count() > 0)
                                        {
                                            _referenced_tweets = _referenced_tweets.First();
                                            var referenced_tweets = JsonConvert.SerializeObject(_referenced_tweets);
                                            var referenced_tweets_model = JsonConvert.DeserializeObject<referenced_tweets>(referenced_tweets);
                                            if (referenced_tweets_model.IsNotNull())
                                            {
                                                model.type = referenced_tweets_model.type;
                                            }
                                        }
                                        var _public_metrics = item.SelectToken("public_metrics");
                                        var public_metrics = JsonConvert.SerializeObject(_public_metrics);
                                        var public_metricss_model = JsonConvert.DeserializeObject<public_metrics>(public_metrics);
                                        if (public_metricss_model.IsNotNull())
                                        {
                                            model.retweet_count = public_metricss_model.retweet_count;
                                            model.reply_count = public_metricss_model.reply_count;
                                            model.like_count = public_metricss_model.like_count;
                                            model.quote_count = public_metricss_model.quote_count;
                                        }
                                        var _polarity = item.SelectToken("polarity");
                                        var polarity = JsonConvert.SerializeObject(_polarity);
                                        var polarity_model = JsonConvert.DeserializeObject<polairty>(polarity);
                                        if (polarity_model.IsNotNull())
                                        {
                                            model.pos = polarity_model.pos;
                                            model.neg = polarity_model.neg;
                                            model.neu = polarity_model.neu;
                                            model.compound = polarity_model.compound;
                                        }
                                        model.keyword = keyword;
                                        var indexName = "twitter_post";
                                        descriptor.Index<object>(i => i
                                                .Index(indexName)
                                                .Id((Id)model.id)
                                                .Document(model));
                                    }

                                }
                                var bulkResponse = client.Bulk(descriptor);
                            }
                        }
                    }



                }
            }
            catch (Exception e)
            {
                throw;
            }

        }
        [LogFailure]
        [AutomaticRetry(Attempts = 1)]
        [Queue("alpha")]
        public async Task YoutubeDataMigration1(string keyword)
        {
            try
            {
                var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
                var settings = new ConnectionSettings(new Uri(eldbUrl));
                var client = new ElasticClient(settings);
                var list = new List<Youtube1ViewModel>();
                var url = "https://youtube.googleapis.com/youtube/v3/search?part=snippet,id&maxResults=50&q=" + keyword + "&type=video%2Clist&key=AIzaSyAVKFSEz4Uk7jTUlA-VRjukTh9nMiz_Y60";
                using (var httpClient = new HttpClient())
                {
                    var address = new Uri(url);
                    var response = await httpClient.GetAsync(address);
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonStr = await response.Content.ReadAsStringAsync();
                        var json = JToken.Parse(jsonStr);
                        var items = json.SelectToken("items");
                        BulkDescriptor descriptor = new BulkDescriptor();
                        foreach (var item in items)
                        {
                            var source = item.SelectToken("id");
                            var id = string.Empty;
                            if (source.IsNotNull())
                            {
                                var _id = source.SelectToken("videoId");
                                id = _id.Value<string>();
                            }
                            var _snippet = item.SelectToken("snippet");
                            var dataStr = JsonConvert.SerializeObject(_snippet);
                            var model = JsonConvert.DeserializeObject<Youtube1ViewModel>(dataStr);
                            if (model.IsNotNull())
                            {
                                model.Id = id;
                                model.keyword = keyword;
                                list.Add(model);
                                var indexName = "youtube_post";
                                descriptor.Index<object>(i => i
                                        .Index(indexName)
                                        .Id((Id)model.Id)
                                        .Document(model));
                            }

                        }
                        var bulkResponse = client.Bulk(descriptor);
                    }

                }
            }
            catch (Exception)
            {

                throw;
            }
            

            
        }
        [LogFailure]
        [AutomaticRetry(Attempts = 1)]
        [Queue("alpha")]
        public async Task YoutubeDataMigration(string keyword)
        {
            try
            {
                var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
                var settings = new ConnectionSettings(new Uri(eldbUrl));
                var client = new ElasticClient(settings);
                var url = "https://xtranet.aitalkx.com/social/youtube_with_sentiment?keyword="+ keyword + "&no_of_pages=1&sortby=date";
                using (var httpClient = new HttpClient())
                {
                    httpClient.Timeout = TimeSpan.FromSeconds(500);
                    var address = new Uri(url);
                    var response = await httpClient.GetAsync(address);
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonStr = await response.Content.ReadAsStringAsync();
                        var json = JToken.Parse(jsonStr);
                        var data = json.SelectToken("data");
                        if (data.IsNotNull())
                        {
                            var items = data.SelectToken("page_0");
                            BulkDescriptor descriptor = new BulkDescriptor();
                            foreach (var item in items)
                            {
                                var id = string.Empty;
                                if (item.IsNotNull())
                                {
                                    var _id = item.SelectToken("videoid");
                                    id = _id.Value<string>();
                                }
                                var dataStr = JsonConvert.SerializeObject(item);
                                var model = JsonConvert.DeserializeObject<Youtube1ViewModel>(dataStr);
                                var _polarity = item.SelectToken("title_polarity");
                                var polarity = JsonConvert.SerializeObject(_polarity);
                                var polarity_model = JsonConvert.DeserializeObject<polairty>(polarity);
                                if (polarity_model.IsNotNull())
                                {
                                    model.pos = polarity_model.pos;
                                    model.neg = polarity_model.neg;
                                    model.neu = polarity_model.neu;
                                    model.compound = polarity_model.compound;
                                }
                                if (model.IsNotNull())
                                {
                                    model.Id = id;
                                    model.keyword = keyword;
                                    var indexName = "youtube_post";
                                    descriptor.Index<object>(i => i
                                            .Index(indexName)
                                            .Id((Id)model.Id)
                                            .Document(model));
                                }

                            }
                            var bulkResponse = client.Bulk(descriptor);
                        }

                    }

                }
            }
            catch (Exception)
            {

                throw;
            }



        }
        [LogFailure]
        public async Task FacebookUserDataMigrationToElasticDB()
        {
            var _noteBusiness = (INoteBusiness)_services.GetService(typeof(INoteBusiness));
            var template = await _noteBusiness.GetSingle(x => x.TemplateCode == "FACEBOOK_USER");
            if (template.IsNotNull())
            {
                var list = await _noteBusiness.GetAllFacebookUser();
                foreach (var item in list)
                {
                    await FacebookUserDataMigration(item);

                }
            }


        }
        [LogFailure]
        [AutomaticRetry(Attempts = 2)]
        public async Task FacebookUserDataMigration(IdNameViewModel model)
        {
            try
            {
                var _noteBusiness = (INoteBusiness)_services.GetService(typeof(INoteBusiness));
                var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
                var settings = new ConnectionSettings(new Uri(eldbUrl));
                var client = new ElasticClient(settings);
                var socialApiUrl = ApplicationConstant.AppSettings.SocialApiUrl(_configuration);
                var credential = await _noteBusiness.GetFacebookCredential();
                //var url = socialApiUrl + "fb_profile_frnds?frnd_user_id=" + model.Code + "&username=" + credential.Name + "&password=" + credential.Code;
                var url = socialApiUrl + "fb_user_data?fb_user_id=" + model.Code;
                using (var httpClient = new HttpClient())
                {
                    httpClient.Timeout = TimeSpan.FromSeconds(500);
                    var address = new Uri(url);
                    var response = await httpClient.GetAsync(address);
                    if (response.IsSuccessStatusCode)
                    {
                        var jsonStr = await response.Content.ReadAsStringAsync();
                        var json = JToken.Parse(jsonStr);
                        var data1 = json.SelectToken("data");
                        if (data1.IsNotNull())
                        {
                            foreach (var data in data1)
                            {
                                BulkDescriptor descriptor = new BulkDescriptor();
                                var dataStr = JsonConvert.SerializeObject(data);
                                var user = JsonConvert.DeserializeObject<FacebookUserViewModel>(dataStr);
                                var _details = data.SelectToken("introduction");
                                if (_details.IsNotNull())
                                {
                                    var _joined = _details.First();
                                    user.join_date = _joined.ToString();
                                }
                                var id = user.profile_url;
                                var indexName = "facebook_user";
                                descriptor.Index<object>(i => i
                                        .Index(indexName)
                                        .Id((Id)id)
                                        .Document(user));
                                var bulkResponse = client.Bulk(descriptor);
                                if (bulkResponse.IsValid)
                                {
                                    var frnd_list = data.SelectToken("friend_list");
                                    if (frnd_list.IsNotNull())
                                    {
                                        BulkDescriptor descriptor1 = new BulkDescriptor();
                                        foreach (var item in frnd_list)
                                        {

                                            var dataStr1 = JsonConvert.SerializeObject(item);
                                            var friend = JsonConvert.DeserializeObject<FacebookFriendViewModel>(dataStr1);
                                            if (friend.IsNotNull())
                                            {
                                                friend.parent_id = user.profile_url;
                                                var id1 = user.profile_url + friend.friend_profile_url;
                                                var indexName1 = "facebook_friend";
                                                descriptor1.Index<object>(i => i
                                                        .Index(indexName1)
                                                        .Id((Id)id1)
                                                        .Document(friend));
                                            }

                                        }
                                        var bulkResponse1 = client.Bulk(descriptor1);
                                    }
                                    var post_list = data.SelectToken("post_data");
                                    if (post_list.IsNotNull())
                                    {
                                        BulkDescriptor descriptor2 = new BulkDescriptor();
                                        foreach (var item in post_list)
                                        {

                                            var dataStr2 = JsonConvert.SerializeObject(item);
                                            var post = JsonConvert.DeserializeObject<FacebookUserPostViewModel>(dataStr2);
                                            if (post.IsNotNull())
                                            {
                                                var _polarity = item.SelectToken("post_msg_polarity");
                                                if (_polarity.IsNotNull())
                                                {
                                                    var polarity = JsonConvert.SerializeObject(_polarity);
                                                    var polarity_model = JsonConvert.DeserializeObject<polairty>(polarity);
                                                    if (polarity_model.IsNotNull())
                                                    {
                                                        post.pos = polarity_model.pos;
                                                        post.neg = polarity_model.neg;
                                                        post.neu = polarity_model.neu;
                                                        post.compound = polarity_model.compound;
                                                    }
                                                }
                                                post.parent_id = user.profile_url;
                                                var id2 = user.profile_url + post.post_title;
                                                var indexName2 = "facebook_user_post";
                                                descriptor2.Index<object>(i => i
                                                        .Index(indexName2)
                                                        .Id((Id)id2)
                                                        .Document(post));
                                            }

                                        }
                                        var bulkResponse2 = client.Bulk(descriptor2);
                                    }

                                }
                            }


                        }
                    }


                }

            }
            catch (Exception e)
            {
                throw;
            }

        }
        [LogFailure]
        public async Task GetVDPIEvent()
        {
            var _noteBusiness = (INoteBusiness)_services.GetService(typeof(INoteBusiness));
            try
            {
                var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
                var settings = new ConnectionSettings(new Uri(eldbUrl));
                var client = new ElasticClient(settings);
                string anpr_topic = "events/ANPR/#";
                string anpr_server_ip = "10.75.22.197";
                int mqtt_port = 1884;

                // Create a new MQTT client.
                var factory = new MqttFactory();
                var mqttClient = factory.CreateMqttClient();

                var options = new MqttClientOptionsBuilder()
                            .WithClientId(Guid.NewGuid().ToString())
                            .WithTcpServer(anpr_server_ip, mqtt_port)
                            .Build();

                // connect handler
                mqttClient.UseConnectedHandler(async e =>
                {
                    Console.WriteLine("### CONNECTED WITH SERVER ###");
                    // Subscribe to a topic
                    await mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic(anpr_topic).Build());
                    Console.WriteLine("### SUBSCRIBED ###");
                });

                // disconnect handler - tries to reconnect
                mqttClient.UseDisconnectedHandler(async e =>
                {
                    Console.WriteLine("### DISCONNECTED FROM SERVER ###");
                    await Task.Delay(TimeSpan.FromSeconds(5));

                    try
                    {
                        await mqttClient.ConnectAsync(options, System.Threading.CancellationToken.None); // Since 3.0.5 with CancellationToken
                    }
                    catch
                    {
                        Console.WriteLine("### RECONNECTING FAILED ###");
                    }
                });
                var data = string.Empty;
                // handling message reveived on the topic
                mqttClient.UseApplicationMessageReceivedHandler(e =>
                {
                    data = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);

                    Console.WriteLine("### RECEIVED APPLICATION MESSAGE ###");
                    Console.WriteLine($"ANPR Event Received = {Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}");
                    using (var httpClient = new HttpClient())
                    {
                        BulkDescriptor descriptor = new BulkDescriptor();
                        dynamic obj = new System.Dynamic.ExpandoObject();
                        ExpandoAddProperty(obj, "event_data", data);
                        descriptor.Index<object>(i => i
                                .Index("vdpi")
                                .Id((Id)Guid.NewGuid().ToString())
                                .Document(obj));
                        var bulkResponse = client.Bulk(descriptor);

                    }
                    _noteBusiness.ManageSchedulerLog(new SchedulerLogViewModel
                    {
                        NoteSubject = "VDP",
                        fromDate = DateTime.Now.ToString(),
                        toDate = DateTime.Now.ToString(),
                        districtCode = null,
                        response = data,
                        error = null,
                        success = true,
                        ActiveUserId = _userContext.UserId
                    });
                    // Note: you will have to parse received data to json
                    Console.WriteLine();
                });
                await mqttClient.ConnectAsync(options);


            }
            catch (Exception e)
            {
                await _noteBusiness.ManageSchedulerLog(new SchedulerLogViewModel
                {
                    NoteSubject = "VDP",
                    fromDate = DateTime.Now.ToString(),
                    toDate = DateTime.Now.ToString(),
                    districtCode = null,
                    response = null,
                    error = e.Message.ToString(),
                    success = false,
                    ActiveUserId = _userContext.UserId
                });
                Console.WriteLine(e.Message.ToString());

            }




        }


        [LogFailure]
        public async Task CCTNSDataMgrationToCommonIndex()
        {
            var _noteBusiness = (INoteBusiness)_services.GetService(typeof(INoteBusiness));
            var template = await _noteBusiness.GetSingle(x => x.TemplateCode == "SOCAIL_SCRAPPING_API");
            if (template.IsNotNull())
            {
                var apilist = await _noteBusiness.GetAllCCTNSApiMethods();
                foreach (var api in apilist.Where(x => x.Url.ToLower().Contains(x.NoteSubject)))
                {
                    await CCTNSApiDataMgrationByIndex(api);
                }
            }

        }
        [LogFailure]
        [AutomaticRetry(Attempts = 2)]
        public async Task CCTNSApiDataMgrationByIndex(SocailScrappingApiViewModel api)
        {
            try
            {
                var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
                var settings = new ConnectionSettings(new Uri(eldbUrl));
                var client = new ElasticClient(settings);
                var url = eldbUrl + api.NoteSubject.ToLower() + "/_search?pretty=true";
                var handler = new HttpClientHandler();
                handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                handler.ServerCertificateCustomValidationCallback =
                    (httpRequestMessage, cert, cetChain, policyErrors) =>
                    {
                        return true;
                    };
                using (var httpClient = new HttpClient(handler))
                {
                    var query = ApplicationConstant.BusinessAnalytics.CctnsMaxDateQuery;
                    query = query.Replace("#INDEXNAME#", api.NoteSubject.ToLower());
                    var queryContent = new StringContent(query, Encoding.UTF8, "application/json");
                    var main_url = eldbUrl + "cctns_common/_search?pretty=true";
                    var main_address = new Uri(main_url);
                    var main_response = await httpClient.PostAsync(main_address, queryContent);
                    if (main_response.IsSuccessStatusCode)
                    {
                        if (main_response.Content != null)
                        {
                            var _jsondata = await main_response.Content.ReadAsStringAsync();
                            if (_jsondata != null)
                            {
                                var _dataToken = JToken.Parse(_jsondata);
                                if (_dataToken != null)
                                {
                                    var _responsedata = _dataToken.SelectToken("aggregations");
                                    if (_responsedata != null)
                                    {
                                        var _maxdateToken = _responsedata.SelectToken("max_date");
                                        if (_maxdateToken.Count() > 0)
                                        {
                                            var _dateToken = _maxdateToken.Last();
                                            if (_dataToken.Count() > 0)
                                            {
                                                var _date = _dateToken.Last();
                                                if (_date != null && _date.HasValues)
                                                {
                                                    var _startDate = _date.Value<DateTime>();
                                                    var content = ApplicationConstant.BusinessAnalytics.CctnsDateRangeQuery;
                                                    content = content.Replace("#FILTERCOLUMN#", api.FilterColumn);
                                                    content = content.Replace("#STARTDATE#", _startDate.ToString("yyyy-MM-ddTHH:mm:ss"));
                                                    content = content.Replace("#ENDDATE#", DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"));
                                                    var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                                                    var address = new Uri(url);
                                                    var response = await httpClient.PostAsync(address, stringContent);
                                                    var jsontrack = await response.Content.ReadAsStringAsync();
                                                    var trackdata = JToken.Parse(jsontrack);
                                                    if (trackdata.IsNotNull())
                                                    {
                                                        var hits = trackdata.SelectToken("hits");
                                                        if (hits.IsNotNull())
                                                        {
                                                            BulkDescriptor descriptor = new BulkDescriptor();
                                                            var _hits = hits.SelectToken("hits");
                                                            foreach (var hit in _hits)
                                                            {
                                                                var _id = hit.SelectToken("_id");
                                                                var _index = hit.SelectToken("_index");
                                                                var source = hit.SelectToken("_source");
                                                                if (source.IsNotNull())
                                                                {
                                                                    var _reportDate = source.SelectToken(api.FilterColumn);
                                                                    var str = JsonConvert.SerializeObject(source);
                                                                    var result = JsonConvert.DeserializeObject<CctnsKeywordViewModel>(str);
                                                                    if (result.IsNotNull())
                                                                    {
                                                                        var model = new CctnsCommonViewModel
                                                                        {
                                                                            ZoneName = result._ZoneName,
                                                                            RangeName = result._RangeName,
                                                                            District = result._District,
                                                                            PoliceStation = result._PoliceStation,
                                                                            JsonString = str,
                                                                            IndexName = _index.ToString(),
                                                                            ReportDate = ((DateTime)_reportDate)
                                                                        };
                                                                        var id = _id.ToString();
                                                                        var indexName = "cctns_common";
                                                                        descriptor.Index<object>(i => i
                                                                                .Index(indexName)
                                                                                .Id((Id)id)
                                                                                .Document(model));

                                                                    }

                                                                }


                                                            }
                                                            var bulkResponse = client.Bulk(descriptor);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                            }

                        }

                    }
                    else
                    {
                        var content = ApplicationConstant.BusinessAnalytics.CctnsBulkQuery;
                        content = content.Replace("#FILTERCOLUMN#", api.FilterColumn);
                        var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                        var address = new Uri(url);
                        var response = await httpClient.PostAsync(address, stringContent);
                        var jsontrack = await response.Content.ReadAsStringAsync();
                        var trackdata = JToken.Parse(jsontrack);
                        if (trackdata.IsNotNull())
                        {
                            var hits = trackdata.SelectToken("hits");
                            if (hits.IsNotNull())
                            {
                                BulkDescriptor descriptor = new BulkDescriptor();
                                var _hits = hits.SelectToken("hits");
                                foreach (var hit in _hits)
                                {
                                    var _id = hit.SelectToken("_id");
                                    var _index = hit.SelectToken("_index");
                                    var source = hit.SelectToken("_source");
                                    if (source.IsNotNull())
                                    {
                                        var _reportDate = source.SelectToken(api.FilterColumn);
                                        var str = JsonConvert.SerializeObject(source);
                                        var result = JsonConvert.DeserializeObject<CctnsKeywordViewModel>(str);
                                        if (result.IsNotNull())
                                        {
                                            var model = new CctnsCommonViewModel
                                            {
                                                ZoneName = result._ZoneName,
                                                RangeName = result._RangeName,
                                                District = result._District,
                                                PoliceStation = result._PoliceStation,
                                                JsonString = str,
                                                IndexName = _index.ToString(),
                                                ReportDate = ((DateTime)_reportDate)
                                            };
                                            var id = _id.ToString();
                                            var indexName = "cctns_common";
                                            descriptor.Index<object>(i => i
                                                    .Index(indexName)
                                                    .Id((Id)id)
                                                    .Document(model));
                                        }

                                    }


                                }
                                var bulkResponse = client.Bulk(descriptor);

                            }
                        }
                    }



                }
            }
            catch (Exception e)
            {
                throw;
            }



        }
        [LogFailure]
        public async Task PushDial100ForecastData()
        {
            var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
            var settings = new ConnectionSettings(new Uri(eldbUrl));
            var client = new ElasticClient(settings);
            var url = "https://xtranet.aitalkx.com/forecast/dial100_forecast_overall";
            using (var httpClient = new HttpClient())
            {
                BulkDescriptor descriptor = new BulkDescriptor();
                httpClient.Timeout = TimeSpan.FromSeconds(500);
                var address = new Uri(url);
                var response = await httpClient.GetAsync(address);
                if (response.IsSuccessStatusCode)
                {
                    var jsonStr = await response.Content.ReadAsStringAsync();
                    if (jsonStr.IsNotNullAndNotEmpty())
                    {
                        var id = Guid.NewGuid().ToString();
                        var model = new Dial100ForcastViewModel { json = jsonStr, created_at = System.DateTime.Now };
                        var indexName = "dial100_forecast";
                        descriptor.Index<object>(i => i
                                .Index(indexName)
                                .Id((Id)id)
                                .Document(model));
                        var bulkResponse = client.Bulk(descriptor);
                    }
                }


            }

        }
        [LogFailure]
        public async Task PushFacebookUser()
        {
            var socialApiUrl = ApplicationConstant.AppSettings.SocialApiUrl(_configuration);
            var _noteBusiness = (INoteBusiness)_services.GetService(typeof(INoteBusiness));
            var template = await _noteBusiness.GetSingle(x => x.TemplateCode == "FACEBOOK_USER");
            if (template.IsNotNull())
            {
                var credential = await _noteBusiness.GetFacebookCredential();
                var list = await _noteBusiness.GetAllFacebookUser();
                foreach (var item in list)
                {
                    using (var httpClient = new HttpClient())
                    {

                        var content = @"{""fb_user_id"": ""#USERID#"",""username"": ""#USERNAME#"",""password"": ""#PASSWORD#""}";
                        content = content.Replace("#USERID#", item.Code);
                        content = content.Replace("#USERNAME#", credential.Name);
                        content = content.Replace("#PASSWORD#", credential.Code);
                        var stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                        var url = socialApiUrl + "fb_users";
                        var address = new Uri(url);
                        var response = await httpClient.PostAsync(address, stringContent);

                    }

                }
            }


        }
        [Queue("alpha")]
        public async Task<bool> IncrementalDMSMigration()
        {
            try
            {

                var _dmsBusiness = (IDMSDocumentBusiness)_services.GetService(typeof(IDMSDocumentBusiness));
                var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
                var settings = new ConnectionSettings(new Uri(eldbUrl));
                var client = new ElasticClient(settings);
                var query = ApplicationConstant.BusinessAnalytics.MaxDateQuery;
                query = query.Replace("#FILTERCOLUMN#", "lastUpdatedDate");
                var queryContent = new StringContent(query, Encoding.UTF8, "application/json");
                var handler = new HttpClientHandler();
                handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                handler.ServerCertificateCustomValidationCallback =
                    (httpRequestMessage, cert, cetChain, policyErrors) =>
                    {
                        return true;
                    };
                using (var httpClient = new HttpClient(handler))
                {
                    var url = eldbUrl + "dms_data/_search?pretty=true";
                    var address = new Uri(url);
                    var response = await httpClient.PostAsync(address, queryContent);
                    if (response.IsSuccessStatusCode)
                    {
                        Console.Write("IncrementalDMSMigration if section");
                        var _jsondata = await response.Content.ReadAsStringAsync();
                        var _dataToken = JToken.Parse(_jsondata);
                        var _responsedata = _dataToken.SelectToken("aggregations");
                        var _maxdateToken = _responsedata.SelectToken("max_date");
                        var _dateToken = _maxdateToken.Last();
                        var _date = _dateToken.Last();
                        var fromDate = _date.Value<DateTime>();
                        var list = await _dmsBusiness.GetAllWorkspaceFolderDocuments(fromDate);
                        BulkDescriptor descriptor = new BulkDescriptor();
                        foreach (var item in list)
                        {
                            item.NoteSubject = Path.GetFileNameWithoutExtension(item.NoteSubject);
                            var extText = item.FileExtractedText.IsNotNullAndNotEmpty() ? Regex.Replace(item.FileExtractedText, @"[^\w\.@-]", " ", RegexOptions.None, TimeSpan.FromSeconds(1.5)) : null;
                            item.FileExtractedText = extText.IsNotNullAndNotEmpty() ? extText.Length > 10000 ? extText.Substring(0, 10000).Replace("\"", " ").Replace("'", " ") : extText.Replace("\"", " ").Replace("'", " ") : null;
                            var id = item.Id;
                            descriptor.Index<object>(i => i
                                   .Index("dms_data")
                                   .Id((Id)id)
                                   .Document(item));
                        }
                        var bulkResponse = client.Bulk(descriptor);
                        if (bulkResponse.IsValid)
                        {
                            var udfTables = list.GroupBy(x => x.UdfTableName).ToList();
                            foreach (var table in udfTables)
                            {
                                var tableName = table.Key;
                                var idList = table.Select(x => x.Id).ToList();
                                var ids = String.Join("','", idList);
                                var list2 = await _dmsBusiness.GetAllDocumentUdfDataByTableName(tableName, ids);
                                BulkDescriptor descriptor1 = new BulkDescriptor();
                                foreach (var root in list2)
                                {
                                    dynamic obj = new System.Dynamic.ExpandoObject();
                                    var id = string.Empty;
                                    foreach (KeyValuePair<String, Object> app in root)
                                    {
                                        var key = app.Key;
                                        var value = app.Value;
                                        if (key == "NtsNoteId")
                                        {
                                            id = value.ToString();

                                        }
                                        ExpandoAddProperty(obj, key, value);
                                    }
                                    descriptor1.Index<object>(i => i
                                        .Index(tableName.ToLower())
                                        .Id((Id)id)
                                        .Document(obj));
                                }
                                var bulkResponse1 = client.Bulk(descriptor1);
                            }
                            return true;
                        }

                    }
                    else if (response.ReasonPhrase == "Not Found")
                    {
                        Console.Write("IncrementalDMSMigration else section");
                        var list = await _dmsBusiness.GetAllWorkspaceFolderDocuments(null);
                        BulkDescriptor descriptor = new BulkDescriptor();
                        foreach (var item in list)
                        {
                            item.NoteSubject = Path.GetFileNameWithoutExtension(item.NoteSubject);
                            var extText = item.FileExtractedText.IsNotNullAndNotEmpty() ? Regex.Replace(item.FileExtractedText, @"[^\w\.@-]", " ", RegexOptions.None, TimeSpan.FromSeconds(1.5)) : null;
                            item.FileExtractedText = extText.IsNotNullAndNotEmpty() ? extText.Length > 10000 ? extText.Substring(0, 10000).Replace("\"", " ").Replace("'", " ") : extText.Replace("\"", " ").Replace("'", " ") : null;
                            var id = item.Id;
                            descriptor.Index<object>(i => i
                                   .Index("dms_data")
                                   .Id((Id)id)
                                   .Document(item));
                        }
                        var bulkResponse = client.Bulk(descriptor);
                        if (bulkResponse.IsValid)
                        {
                            var udfTables = list.GroupBy(x => x.UdfTableName).ToList();
                            foreach (var table in udfTables)
                            {
                                var tableName = table.Key;
                                var idList = table.Select(x => x.Id).ToList();
                                var ids = String.Join("','", idList);
                                var list2 = await _dmsBusiness.GetAllDocumentUdfDataByTableName(tableName, ids);
                                BulkDescriptor descriptor1 = new BulkDescriptor();
                                foreach (var root in list2)
                                {
                                    dynamic obj = new System.Dynamic.ExpandoObject();
                                    var id = string.Empty;
                                    foreach (KeyValuePair<String, Object> app in root)
                                    {
                                        var key = app.Key;
                                        var value = app.Value;
                                        if (key == "NtsNoteId")
                                        {
                                            id = value.ToString();

                                        }
                                        ExpandoAddProperty(obj, key, value);
                                    }
                                    descriptor1.Index<object>(i => i
                                        .Index(tableName.ToLower())
                                        .Id((Id)id)
                                        .Document(obj));
                                }
                                var bulkResponse1 = client.Bulk(descriptor1);
                            }
                            return true;
                        }
                    }


                }
                return false;
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
                return false;
            }

        }
        [LogFailure]
        public async Task CreateHangfireJobs()
        {
            var _noteBusiness = (INoteBusiness)_services.GetService(typeof(INoteBusiness));
            var list = await _noteBusiness.GetList(x => x.TemplateCode == "HANGFIRE_JOBS");
            foreach (var item in list)
            {

                if (item.IsNotNull() && item.NoteSubject.Contains("PerformApiDataMigrationToElasticDB"))
                {
                    try
                    {
                        await PerformApiDataMigrationToElasticDB();
                    }
                    catch (Exception)
                    {

                    }
                }
                else if (item.IsNotNull() && item.NoteSubject.Contains("ExecuteAlertRule"))
                {
                    try
                    {
                        await ExecuteAlertRule();
                    }
                    catch (Exception)
                    {

                    }
                }
                else if (item.IsNotNull() && item.NoteSubject.Contains("ExecuteRssFeeds"))
                {
                    try
                    {
                        //1hour
                        RecurringJob.AddOrUpdate<Synergy.App.Business.HangfireScheduler>(x => x.ExecuteRssFeeds(), Cron.Hourly(5));

                    }
                    catch (Exception)
                    {

                    }

                }
                else if (item.IsNotNull() && item.NoteSubject.Contains("TrendingSocialDataMigrationToElasticDB"))
                {
                    try
                    {
                        //5hour
                        RecurringJob.AddOrUpdate<Synergy.App.Business.HangfireScheduler>(x => x.TrendingSocialDataMigrationToElasticDB(), Cron.Hourly(5));
                    }
                    catch (Exception)
                    {

                    }

                }
                else if (item.IsNotNull() && item.NoteSubject.Contains("SocialMediaKeywordDataMigrationToElasticDB"))
                {
                    try
                    {
                        //5hour
                        RecurringJob.AddOrUpdate<Synergy.App.Business.HangfireScheduler>(x => x.SocialMediaKeywordDataMigrationToElasticDB(), Cron.Hourly(5));
                    }
                    catch (Exception)
                    {

                    }

                }
                else if (item.IsNotNull() && item.NoteSubject.Contains("FacebookUserDataMigrationToElasticDB"))
                {
                    try
                    {
                        RecurringJob.AddOrUpdate<Synergy.App.Business.HangfireScheduler>(x => x.FacebookUserDataMigrationToElasticDB(), Cron.Daily());
                    }
                    catch (Exception)
                    {

                    }

                }
                else if (item.IsNotNull() && item.NoteSubject.Contains("GetVDPIEvent"))
                {
                    try
                    {

                        //BackgroundJob.Enqueue<Synergy.App.Business.HangfireScheduler>(x => x.GetVDPIEvent());

                    }
                    catch (Exception)
                    {

                    }

                }
                else if (item.IsNotNull() && item.NoteSubject.Contains("CCTNSDataMgrationToCommonIndex"))
                {
                    try
                    {
                        //5hour
                        RecurringJob.AddOrUpdate<Synergy.App.Business.HangfireScheduler>(x => x.CCTNSDataMgrationToCommonIndex(), Cron.Hourly(5));

                    }
                    catch (Exception)
                    {

                    }

                }
                else if (item.IsNotNull() && item.NoteSubject.Contains("PushDial100ForecastData"))
                {
                    try
                    {

                        RecurringJob.AddOrUpdate<Synergy.App.Business.HangfireScheduler>(x => x.PushDial100ForecastData(), Cron.Daily());

                    }
                    catch (Exception)
                    {

                    }

                }
                else if (item.IsNotNull() && item.NoteSubject.Contains("PushFacebookUser"))
                {
                    try
                    {

                        RecurringJob.AddOrUpdate<Synergy.App.Business.HangfireScheduler>(x => x.PushFacebookUser(), Cron.Daily());

                    }
                    catch (Exception)
                    {

                    }

                }
                else if (item.IsNotNull() && item.NoteSubject.Contains("IncrementalDMSMigration"))
                {
                    try
                    {
                        //5 min
                        RecurringJob.AddOrUpdate<Synergy.App.Business.HangfireScheduler>(x => x.IncrementalDMSMigration(), Cron.MinuteInterval(5));

                    }
                    catch (Exception)
                    {

                    }

                }
                else if (item.IsNotNull() && item.NoteSubject.Contains("ExtractTextFromPdfForDMS"))
                {
                    try
                    {
                        //5 min
                        RecurringJob.AddOrUpdate<Synergy.App.Business.HangfireScheduler>(x => x.ExtractTextFromPdfForDMS(), Cron.MinuteInterval(5));
                        RecurringJob.AddOrUpdate<Synergy.App.Business.HangfireScheduler>(x => x.ThumbnailFromPdfForDMS(), Cron.MinuteInterval(5));


                    }
                    catch (Exception)
                    {

                    }

                }
                else if (item.IsNotNull() && item.NoteSubject.Contains("GetRoipTables"))
                {
                    try
                    {
                        //2hour
                        RecurringJob.AddOrUpdate<Synergy.App.Business.HangfireScheduler>(x => x.GetRoipTables(), Cron.Hourly(2));
                        RecurringJob.AddOrUpdate<Synergy.App.Business.HangfireScheduler>(x => x.GetRoipTablesData(), Cron.MinuteInterval(30));

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }

                }
                else if (item.IsNotNull() && item.NoteSubject.Contains("GetRoipTablesData"))
                {
                    try
                    {
                        //2hour
                        

                    }
                    catch (Exception)
                    {

                    }

                }
            }

        }

        private async Task<bool> GenerateRevenueCollectionBillForJammu()
        {
            try
            {
                var userBusiness = (IUserBusiness)_services.GetService(typeof(IUserBusiness));
                var user = await userBusiness.ValidateUser("admin@synergy.com");
                if (user != null)
                {
                    var data = new ApplicationIdentityUser
                    {
                        Id = user.Id,
                        UserName = user.Name,
                        IsSystemAdmin = user.IsSystemAdmin,
                        Email = user.Email,
                        UserUniqueId = user.Email,
                        CompanyId = user.CompanyId,
                        CompanyCode = user.CompanyCode,
                        CompanyName = user.CompanyName,
                        JobTitle = user.JobTitle,
                        PhotoId = user.PhotoId,
                        UserRoleCodes = string.Join(",", user.UserRoles.Select(x => x.Code)),
                        UserRoleIds = string.Join(",", user.UserRoles.Select(x => x.Id)),
                        UserPortals = user.UserPortals,
                        LegalEntityId = user.LegalEntityId,
                        LegalEntityCode = user.LegalEntityCode
                    };
                    SetContext(data);
                }
                var _business = (ISmartCityBusiness)_services.GetService(typeof(ISmartCityBusiness));
                await _business.GenerateRevenueCollectionBillForJammu();
                return true;
            }
            catch (Exception ex)
            {

                return false;
            }

        }
        [LogFailure]
        [Queue("default")]
        public async Task ExtractTextFromPdfForDMS()
        {
            try
            {
                var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
                var _fileBusiness = (IFileBusiness)_services.GetService(typeof(IFileBusiness));
                var _noteBusiness = (INoteBusiness)_services.GetService(typeof(INoteBusiness));
                var _documentBusiness = (IDMSDocumentBusiness)_services.GetService(typeof(IDMSDocumentBusiness));
                var query = ApplicationConstant.Document.GetFilesWitoutExtractedText;
                var queryContent = new StringContent(query, Encoding.UTF8, "application/json");
                var list = await _documentBusiness.GetAllDmsDocumentsWithUdf();
                var handler = new HttpClientHandler();
                handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                handler.ServerCertificateCustomValidationCallback =
                    (httpRequestMessage, cert, cetChain, policyErrors) =>
                    {
                        return true;
                    };
                using (var httpClient = new HttpClient(handler))
                {
                    foreach (var item in list)
                    {
                        try
                        {
                            if (item.FileExtension.ToLower() == ".pdf" || item.MongoPreviewFileId.IsNotNullAndNotEmpty())
                            {
                                byte[] contentByte = await _fileBusiness.DownloadMongoFileByte(item.MongoPreviewFileId.IsNotNullAndNotEmpty() ? item.MongoPreviewFileId : item.MongoFileId);
                                if (contentByte.Length > 0)
                                {
                                    using (var document = UglyToad.PdfPig.PdfDocument.Open(contentByte))
                                    {
                                        var pages = document.GetPages();
                                        foreach (var page in pages)
                                        {
                                            item.FileExtractedText += string.Join(" ", page.GetWords());
                                        }

                                    }
                                    var file = await _fileBusiness.GetSingleById(item.FileId);
                                    if (file.IsNotNull())
                                    {
                                        var extText = Regex.Replace(item.FileExtractedText, @"[^\w\.@-]", " ", RegexOptions.None, TimeSpan.FromSeconds(1.5));
                                        file.FileExtractedText = extText;
                                        await _fileBusiness.Edit(file);
                                        var fileName = Path.GetFileNameWithoutExtension(item.FileName);
                                        var content2 = @"{""size"":10, ""query"": {""bool"" : {""must"": { ""match_all"" : {}},""filter"" : [{ ""terms"" :{""_id"":[""#IDS#""]}}]}},""script"": { ""source"": ""ctx._source['fileId'] ='#FileId#';ctx._source['fileName'] ='#FileName#';ctx._source['fileExtension'] ='#FileExtension#';ctx._source['filePreviewExtension'] ='.pdf';ctx._source['fileExtractedText'] ='#FileExtractedText#';""} }";
                                        content2 = content2.Replace("#IDS#", item.Id);
                                        content2 = content2.Replace("#FileId#", item.FileId);
                                        content2 = content2.Replace("#FileName#", fileName);
                                        content2 = content2.Replace("#FileExtension#", item.FileExtension);
                                        content2 = content2.Replace("#FileExtractedText#", extText.Length > 10000 ? extText.Substring(0, 10000).Replace("\"", " ").Replace("'", " ") : extText.Replace("\"", " ").Replace("'", " "));
                                        var stringContent2 = new StringContent(content2, Encoding.UTF8, "application/json");
                                        var url2 = eldbUrl + "dms_data/_update_by_query";
                                        var address2 = new Uri(url2);
                                        var response2 = await httpClient.PostAsync(address2, stringContent2);
                                        Console.WriteLine("ExtractTextFromPdfForDMS " + item.NoteNo + ":" + response2.ToString());
                                    }
                                }


                            }

                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("ExtractTextFromPdfForDMS " + item.NoteNo + ":" + e.Message);
                            continue;

                        }
                    }
                    var url = eldbUrl + "dms_data/_search?pretty=true";
                    var address = new Uri(url);
                    var response = await httpClient.PostAsync(address, queryContent);
                    if (response.IsSuccessStatusCode)
                    {
                        var _jsondata = await response.Content.ReadAsStringAsync();
                        var _dataToken = JToken.Parse(_jsondata);
                        if (_dataToken.IsNotNull())
                        {
                            var hits = _dataToken.SelectToken("hits");
                            if (hits.IsNotNull())
                            {
                                var _hits = hits.SelectToken("hits");
                                foreach (var hit in _hits)
                                {
                                    var _id = hit.SelectToken("_id");
                                    var id = _id.ToString();
                                    var doc = await _noteBusiness.GetSingleById(id);
                                    if (doc.IsNotNull() && doc.DmsAttachmentId.IsNotNullAndNotEmpty())
                                    {
                                        var file = await _fileBusiness.GetSingleById(doc.DmsAttachmentId);
                                        if (file.IsNotNull())
                                        {
                                            var extText = Regex.Replace(file.FileExtractedText, @"[^\w\.@-]", " ", RegexOptions.None, TimeSpan.FromSeconds(1.5));
                                            file.FileExtractedText = extText;
                                            var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                                            var content2 = @"{""size"":10, ""query"": {""bool"" : {""must"": { ""match_all"" : {}},""filter"" : [{ ""terms"" :{""_id"":[""#IDS#""]}}]}},""script"": { ""source"": ""ctx._source['fileId'] ='#FileId#';ctx._source['fileName'] ='#FileName#';ctx._source['fileExtension'] ='#FileExtension#';ctx._source['filePreviewExtension'] ='.pdf';ctx._source['fileExtractedText'] ='#FileExtractedText#';""} }";
                                            content2 = content2.Replace("#IDS#", id);
                                            content2 = content2.Replace("#FileId#", file.Id);
                                            content2 = content2.Replace("#FileName#", fileName);
                                            content2 = content2.Replace("#FileExtension#", file.FileExtension);
                                            content2 = content2.Replace("#FileExtractedText#", extText.Length > 10000 ? extText.Substring(0, 10000).Replace("\"", " ").Replace("'", " ") : extText.Replace("\"", " ").Replace("'", " "));
                                            var stringContent2 = new StringContent(content2, Encoding.UTF8, "application/json");
                                            var url2 = eldbUrl + "dms_data/_update_by_query";
                                            var address2 = new Uri(url2);
                                            var response2 = await httpClient.PostAsync(address2, stringContent2);
                                            Console.WriteLine("ExtractTextFromPdfForDMS from elastic db filter " + id + ":" + response2.ToString());
                                        }
                                    }

                                }

                            }
                        }
                    }

                }


            }
            catch (Exception)
            {

                throw;
            }


        }
        [LogFailure]
        [Queue("default")]
        public async Task ThumbnailFromPdfForDMS()
        {
            try
            {
                var shellName = "/bin/bash";
                var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
                var _fileBusiness = (IFileBusiness)_services.GetService(typeof(IFileBusiness));
                var _documentBusiness = (IDMSDocumentBusiness)_services.GetService(typeof(IDMSDocumentBusiness));
                var list = await _documentBusiness.GetAllDmsDocumentsWithUdf1();
                var handler = new HttpClientHandler();
                handler.ClientCertificateOptions = ClientCertificateOption.Manual;
                handler.ServerCertificateCustomValidationCallback =
                    (httpRequestMessage, cert, cetChain, policyErrors) =>
                    {
                        return true;
                    };
                using (var httpClient = new HttpClient(handler))
                {
                    foreach (var item in list)
                    {
                        try
                        {
                            if (item.FileExtension.ToLower() == ".pdf" || item.MongoPreviewFileId.IsNotNullAndNotEmpty())
                            {
                                byte[] contentByte = await _fileBusiness.DownloadMongoFileByte(item.MongoPreviewFileId.IsNotNullAndNotEmpty() ? item.MongoPreviewFileId : item.MongoFileId);
                                if (contentByte.Length > 0)
                                {                                   
                                    var file = await _fileBusiness.GetSingleById(item.FileId);
                                    if (file.IsNotNull())
                                    {                                        
                                        var fileName = Path.GetFileNameWithoutExtension(item.FileName);
                                        var fileExtension = Path.GetExtension(item.FileName);
                                        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                                        {

                                            string _filename = file.Id + item.FileExtension;
                                            string path = System.IO.Path.Combine(System.IO.Path.GetTempPath(), _filename);
                                            string outpath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), file.Id);
                                            using (System.IO.Stream pdf = System.IO.File.OpenWrite(path))
                                            {
                                                pdf.Write(contentByte, 0, contentByte.Length);
                                            }
                                            ProcessStartInfo procStartInfo = new ProcessStartInfo();
                                            procStartInfo.FileName = "pdftoppm";
                                            Console.WriteLine("/usr/bin/pdftoppm " + path + " " + outpath + " -png -singlefile");
                                            //procStartInfo.Arguments = "/usr/bin/soffice " + string.Format("--convert-to pdf --outdir {0} {1}", outpath, path);
                                            procStartInfo.Arguments = path + " " + outpath + " -png -singlefile";
                                            procStartInfo.RedirectStandardOutput = true;
                                            procStartInfo.UseShellExecute = false;
                                            procStartInfo.CreateNoWindow = true;
                                            procStartInfo.WorkingDirectory = Environment.CurrentDirectory;
                                            Process process = new Process() { StartInfo = procStartInfo, };
                                            process.Start();
                                            process.WaitForExit();

                                            // Check for failed exit code.
                                            if (process.ExitCode == 0)
                                            {
                                                string newfileName = file.Id + ".png";
                                                string newpath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), newfileName);
                                                byte[] bytes = System.IO.File.ReadAllBytes(newpath);
                                                var res = await _fileBusiness.UploadMongoSnapshotFile(file, bytes); ;
                                                if (res)
                                                {
                                                    Console.WriteLine("Successfully uploaded ==== thumbnail");
                                                    if (System.IO.File.Exists(path))
                                                    {
                                                        System.IO.File.Delete(path);
                                                    }
                                                    if (System.IO.File.Exists(newpath))
                                                    {
                                                        System.IO.File.Delete(newpath);
                                                    }
                                                }

                                            }
                                        }
                                        else
                                        {
                                            byte[] pngByte = Freeware.Pdf2Png.Convert(contentByte, 1);
                                            await _fileBusiness.UploadMongoSnapshotFile(file, pngByte);
                                        }                                        
                                       
                                    }
                                }


                            }

                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("ExtractTextFromPdfForDMS " + item.NoteNo + ":" + e.Message);
                            continue;

                        }
                    }
                }




            }
            catch (Exception)
            {

                throw;
            }


        }
        [LogFailure]
        [Queue("default")]
        public async Task<bool> GetRoipTables()
        {
            var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
            var settings = new ConnectionSettings(new Uri(eldbUrl));
            var client = new ElasticClient(settings);
            using var connection = new MySqlConnector.MySqlConnection("Server=10.10.10.110;User ID=root ;Password=pulsecom;Database=deepvlg");
            await connection.OpenAsync();
            using var command = new MySqlConnector.MySqlCommand("show tables;", connection);
            using var reader = await command.ExecuteReaderAsync();
            BulkDescriptor descriptor = new BulkDescriptor();
            while (reader.Read())
            {
                var table = reader.GetString("Tables_in_deepvlg");
                Console.WriteLine(table);
                dynamic obj = new System.Dynamic.ExpandoObject();
                ExpandoAddProperty(obj, "table_name", table);
                descriptor.Index<object>(i => i
                       .Index("roip_tables")
                       .Id((Id)table)
                       .Document(obj));
            }
            var bulkResponse = client.Bulk(descriptor);
            return true;

        }
        [LogFailure]
        [Queue("default")]
        public async Task<bool> GetRoipTablesData()
        {
            var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
            var settings = new ConnectionSettings(new Uri(eldbUrl));
            var client = new ElasticClient(settings);
            var _date = await GetRoipLastDate();
            if (_date.IsNotNullAndNotEmpty())
            {
                var query = "select * from " + _date +";";
                using var connection = new MySqlConnector.MySqlConnection("Server=10.10.10.110;User ID=root ;Password=pulsecom;Database=deepvlg");
                await connection.OpenAsync();
                using var command = new MySqlConnector.MySqlCommand(query, connection);
                using var reader = await command.ExecuteReaderAsync();
                BulkDescriptor descriptor = new BulkDescriptor();
                while (reader.Read())
                {
                    dynamic obj = new System.Dynamic.ExpandoObject();
                    var date_value = reader.GetDateTime("date_value");
                    ExpandoAddProperty(obj, "date_value", date_value);
                    Console.WriteLine(date_value);
                    var start_time = reader.GetTimeSpan("start_time");                    
                    ExpandoAddProperty(obj, "start_time", start_time.ToString());
                    var start_datetime= date_value.Add(start_time);
                    ExpandoAddProperty(obj, "start_datetime", start_datetime);
                    var end_time = reader.GetTimeSpan("end_time");
                    ExpandoAddProperty(obj, "end_time", end_time.ToString());
                    var end_datetime = date_value.Add(end_time);
                    ExpandoAddProperty(obj, "end_datetime", end_datetime);
                    var dur_time = reader.GetTimeSpan("dur_time");
                    ExpandoAddProperty(obj, "dur_time", dur_time.ToString());
                    var dur_datetime = date_value.Add(dur_time);
                    ExpandoAddProperty(obj, "dur_datetime", dur_datetime);
                    var file_name = reader.GetString("file_name");
                    ExpandoAddProperty(obj, "file_name", file_name);
                    var channel_no = reader.GetInt64("channel_no");
                    ExpandoAddProperty(obj, "channel_no", channel_no);
                    var id = file_name + dur_time.ToString();
                    descriptor.Index<object>(i => i
                           .Index("roip_data")
                           .Id((Id)id)
                           .Document(obj));
                }
                var bulkResponse = client.Bulk(descriptor);
            }
            
            return true;

        }
        private async Task<string> GetRoipLastDate()
        {
            List<string> tables = new List<string>();
            var _noteBusiness = (INoteBusiness)_services.GetService(typeof(INoteBusiness));
            var eldbUrl = ApplicationConstant.AppSettings.EsdbConnectionUrl(_configuration);
            var settings = new ConnectionSettings(new Uri(eldbUrl));
            var client = new ElasticClient(settings);
            var query = ApplicationConstant.BusinessAnalytics.MaxDateQuery;
            query = query.Replace("#FILTERCOLUMN#", "date_value");
            var queryContent = new StringContent(query, Encoding.UTF8, "application/json");
            var handler = new HttpClientHandler();
            handler.ClientCertificateOptions = ClientCertificateOption.Manual;
            handler.ServerCertificateCustomValidationCallback =
                (httpRequestMessage, cert, cetChain, policyErrors) =>
                {
                    return true;
                };
            using (var httpClient = new HttpClient(handler))
            {
                var url = eldbUrl + "roip_data/_search?pretty=true";
                var address = new Uri(url);
                var response = await httpClient.PostAsync(address, queryContent);
                if (response.IsSuccessStatusCode)
                {
                    var _jsondata = await response.Content.ReadAsStringAsync();
                    var _dataToken = JToken.Parse(_jsondata);
                    var _responsedata = _dataToken.SelectToken("aggregations");
                    var _maxdateToken = _responsedata.SelectToken("max_date");
                    var _dateToken = _maxdateToken.Last();
                    var _date = _dateToken.Last();
                    var date = _date.Value<DateTime>();
                    var _newDate = date.AddDays(1);
                    var url1 = eldbUrl + "roip_tables/_search?size=10000";
                    var address1 = new Uri(url1);
                    var response1 = await httpClient.GetAsync(address1);
                    if (response1.IsSuccessStatusCode)
                    {
                        var _jsondata1 = await response1.Content.ReadAsStringAsync();
                        var _dataToken1 = JToken.Parse(_jsondata1);
                        if (_dataToken1.IsNotNull())
                        {
                            var hits = _dataToken1.SelectToken("hits");
                            if (hits.IsNotNull())
                            {
                                var _hits = hits.SelectToken("hits");
                                foreach (var hit in _hits)
                                {
                                    var source = hit.SelectToken("_source");
                                    if (source.IsNotNull())
                                    {
                                        var table_name = source.SelectToken("table_name");
                                        if (table_name.IsNotNull())
                                        {
                                            var _table = table_name.Value<string>();
                                            tables.Add(_table);
                                        }

                                    }



                                }
                            }
                        }
                    }
                    if (date == DateTime.Now.Date || (date.AddDays(1) == DateTime.Now.Date && date.AddDays(1).AddHours(2) < DateTime.Now))
                    {
                        var newDate1 = date.ToString("yy-MM-dd");
                        return newDate1.Replace("-", "_");
                    }
                    var dateStr = await GetRoipDate(tables, _newDate);
                    return dateStr;

                }
                else if(response.ReasonPhrase == "Not Found")
                {
                    return "22_02_25";
                }
            }
            return "";
        }

        private async Task<string> GetRoipDate(List<string> tables, DateTime date)
        {

            if (date == DateTime.Now.Date || date > DateTime.Now.Date)
            {
                var newDate1 = DateTime.Now.ToString("yy-MM-dd");
                return newDate1.Replace("-", "_");
            }
            var newDate = date.ToString("yy-MM-dd");
            var dateStr = newDate.Replace("-", "_");
            if (tables.Where(x => x == dateStr).Any())
            {
                return dateStr;
            }
            else
            {
                return await GetRoipDate(tables, date.AddDays(1));
            }


        }
    }
}
