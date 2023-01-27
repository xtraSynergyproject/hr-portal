using CMS.Common;
using Hangfire;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using CMS.UI.ViewModel;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.IO;
using CMS.Data.Model;
using System.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.SignalR;

namespace CMS.Business
{
    public class HangfireScheduler
    {
        private static IServiceProvider _services;
        private readonly IConfiguration _configuration;
        private readonly IUserContext _userContext;
        //private readonly IHostingEnvironment _environment;
        private AuthSignInManager<ApplicationIdentityUser> _customUserManager;
        private readonly IHttpContextAccessor _accessor;
        public HangfireScheduler(IServiceProvider services, IConfiguration configuration,
           AuthSignInManager<ApplicationIdentityUser> customUserManager
            , IUserContext userContext//, IHostingEnvironment environment
            , IHttpContextAccessor accessor)
        {
            _services = services;
            _configuration = configuration;
            _customUserManager = customUserManager;
            _userContext = userContext;
            //_environment = environment;
            _accessor = accessor;
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
        [AutomaticRetry(Attempts = 5)]
        public async Task<bool> SendMail(string mailId, ApplicationIdentityUser userContext)
        {
            try
            {
                SetContext(userContext);
                var _business = _services.GetService<IEmailBusiness>();
                await _business.SendMail(mailId);
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
                    var notificationModel = new UI.ViewModel.NotificationViewModel();
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
                                BackgroundJob.Enqueue<Business.HangfireScheduler>(x => x.SendEmailUsingHangfire(result.Item.EmailUniqueId));
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
        //[LogFailure]
        //public async Task<bool> ConvertFileToPdf()
        //{
        //    if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        //    {
        //        var argsPrepend = "";
        //        var shellName = "/bin/bash";                 
        //        var folder = Guid.NewGuid().ToString();
        //        var outfolder = Guid.NewGuid().ToString();
        //        string folderpath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), folder);
        //        string folderpathAll = System.IO.Path.Combine(System.IO.Path.GetTempPath(), folder, "*.*");
        //        //string outfolderpath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), outfolder);
        //        bool exists = System.IO.Directory.Exists(folderpath);
        //        if (!exists)
        //            System.IO.Directory.CreateDirectory(folderpath);                
        //        //bool exists1 = System.IO.Directory.Exists(outfolderpath);
        //        //if (!exists1)
        //        //    System.IO.Directory.CreateDirectory(outfolderpath);
        //        var _business = (IFileBusiness)_services.GetService(typeof(IFileBusiness));
        //        var list = await _business.GetFileList();
        //        foreach (var item in list)
        //        {
        //            string filename = item.Id + item.FileExtension;
        //            string path = System.IO.Path.Combine(folderpath, filename);                    
        //            byte[] contentByte = await _business.DownloadMongoFileByte(item.MongoFileId);
        //            using (System.IO.Stream file = System.IO.File.OpenWrite(path))
        //            {
        //                file.Write(contentByte, 0, contentByte.Length);
        //            }
        //        }
        //        ProcessStartInfo procStartInfo = new ProcessStartInfo();
        //        procStartInfo.FileName = shellName;
        //        //procStartInfo.Arguments = "/usr/bin/soffice " + string.Format("--convert-to pdf --outdir {0} {1}", outfolderpath, folderpathAll);
        //        procStartInfo.Arguments = "lowriter " + string.Format("--convert-to pdf {0}", folderpathAll);
        //        procStartInfo.RedirectStandardOutput = true;
        //        procStartInfo.UseShellExecute = false;
        //        procStartInfo.CreateNoWindow = true;
        //        procStartInfo.WorkingDirectory = Environment.CurrentDirectory;
        //        Process process = new Process() { StartInfo = procStartInfo, };
        //        process.Start();
        //        process.WaitForExit();

        //        // Check for failed exit code.
        //        if (process.ExitCode == 0)
        //        {
        //            foreach (var item in list)
        //            {
        //                string newfileName = item.Id + ".pdf";
        //                string newpath = System.IO.Path.Combine(folderpath, newfileName);
        //                byte[] bytes = System.IO.File.ReadAllBytes(newpath);
        //                var res = await _business.UploadMongoFileByte(item, bytes);                        
        //            }
        //            if (Directory.Exists(folderpath))
        //            {
        //                Directory.Delete(folderpath, true);
        //            }
        //        }
        //        else
        //        {
        //            if (Directory.Exists(folderpath))
        //            {
        //                Directory.Delete(folderpath, true);
        //            }
        //        }
        //    }
        //    return true;


        //}
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
            Console.Write("Path " +path.ToString());
            Console.Write("Output Path " +outpath.ToString());
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
        public async Task<bool> GeneratePerformanceDocumentStages(string stageMasterId)
        {
            try
            {
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
        public async Task<bool> DailyJob()
        {
            try
            {
                var ntsBusiness = (INtsBusiness)_services.GetService(typeof(INtsBusiness));
                await InitiateReviewTask();
                await ntsBusiness.DisbaleGrievenceReopenService(DateTime.Now);
                await ntsBusiness.SendNotificationForRentServices();
                // CancelCommunityHallBookingOnExpired(TodayDate-TaskStartDate>2 then reject task; reson-completion time exceed 2 days so rejected automatically)
                await ntsBusiness.CancelCommunityHallBookingOnExpired(DateTime.Now);
                await ntsBusiness.UpdateRentalStatusForVacating();
                return true;
            }
            catch (Exception)
            {
                return false;
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
        public async Task<bool> IncrementalDBMigration()
        {
            try
            {
                Console.WriteLine("Starting IncrementalDBMigration");
                var _cmsBusiness = (ICmsBusiness)_services.GetService(typeof(ICmsBusiness));
                var lastMigrationScriptName = await _cmsBusiness.GetLatestMigrationScript();
                DirectoryInfo info = new DirectoryInfo(Path.Combine(AppContext.BaseDirectory, "MigrationScript"));
                Console.WriteLine("Directory for Migration ==== "+Path.Combine(AppContext.BaseDirectory, "MigrationScript"));
                FileInfo[] files = info.GetFiles().OrderBy(p => p.CreationTime).ToArray();
                bool startExecute = false;
                foreach (FileInfo file in files)
                {
                    var fileName = file.Name.Split('.');
                    if (startExecute)
                    {
                        // Execute the file 
                        string scriptData = System.IO.File.ReadAllText(file.FullName);
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
                    else if (fileName[0] == lastMigrationScriptName)
                    {
                        startExecute = true;
                    }
                }
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
        public async Task PerformBackgroundJob(string jobId)
        {
            try
            {
                var _noteBusiness = (INoteBusiness)_services.GetService(typeof(INoteBusiness));
                var _hubContext = (IHubContext<ServiceHub>)_services.GetService(typeof(IHubContext<ServiceHub>));
                var model = await _noteBusiness.GetSingle(x => x.TemplateCode == "SOCAIL_SCRAPPING_API");
                var json = JsonConvert.SerializeObject(model);
                await _hubContext.Clients.Group(jobId).SendAsync("progress", json);
            }
            catch (Exception ex)
            {
                
                
            }

        }
    }
}
