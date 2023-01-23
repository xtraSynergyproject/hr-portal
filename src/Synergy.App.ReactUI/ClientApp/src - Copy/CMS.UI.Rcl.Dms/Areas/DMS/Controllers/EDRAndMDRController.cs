//using System.Collections.Generic;
//using System;
//using System.IO;
//using System.Linq;
//using System.Web;
//using Cms.UI.ViewModel;
//using Kendo.Mvc.UI;
//using Newtonsoft.Json;
//using Microsoft.VisualBasic.FileIO;
//using Newtonsoft.Json.Linq;
//using Kendo.Mvc.Extensions;
//using System.Text.RegularExpressions;
//using CMS.UI.Web;
//using CMS.Business;
//using Microsoft.AspNetCore.Mvc;
//using CMS.Common;

//namespace ERP.UI.Web.Areas.Dms.Controllers
//{
//    public class EDRAndMDRController : ApplicationController
//    {
       
//        private IEDRDataBusiness _edrBusiness;
//        private IFileBusiness _fileBusiness;
//        private INoteBusiness _noteBusiness;
//        private IServiceBusiness _servicebusiness;
//        private ITaskBusiness _taskBusiness;
//        private IListOfValueBusiness _lovBusiness;
//        private IUserBusiness _userBusiness;

//        public EDRAndMDRController(IEDRDataBusiness edrBusiness, IFileBusiness fileBusiness, INoteBusiness noteBusiness,
//            IServiceBusiness servicebusiness, ITaskBusiness taskBusiness, IListOfValueBusiness lovBusiness, IUserBusiness userBusiness)
//        {
//            _edrBusiness = edrBusiness;
//            _fileBusiness = fileBusiness;
//            _noteBusiness = noteBusiness;
//            _servicebusiness = servicebusiness;
//            _taskBusiness = taskBusiness;
//            _lovBusiness = lovBusiness;
//            _userBusiness = userBusiness;
//        }
      
//        public IActionResult Manage()
//        {
//            var model = new EDRDataViewModel();
//            return PartialView("_Manage",model);
//        }
//        public ActionResult SelectWorkspace()
//        {           
//            //ViewBag.Type = Type;
//            var model = new NoteViewModel();
//            //if (Type.EqualsIgnoreCase("DOCUMENT"))
//            //{
//            //    var note = _business.GetDetails(new NoteViewModel { Id = Id });
//            //    model.Subject = note.Subject;
//            //}
//            return PartialView("_SelectWorkspace", model);
//        }
//        [HttpPost]
//        public ActionResult Load(EDRMDRFileTypeEnum type,string revision)
//        {
//            //var path = Server.MapPath("~/Content/web/spreadsheet/products.json");

//            //ViewBag.Sheets = JsonConvert.DeserializeObject<IEnumerable<SpreadsheetSheet>>(System.IO.File.ReadAllText(path));
//            var fileId = _edrBusiness.GetSingle(x => x.FileType == type && x.RevisionNo == revision);
//            if (fileId!=null)
//            {
//                var doc = _fileBusiness.GetSingleById(fileId.FileId);
//                var byteArray = _fileBusiness.DownloadMongoFile(doc.MongoFileId);
//                Stream stream = new MemoryStream(byteArray);
//                var workbook = Workbook.Load(stream, doc.FileExtension);
//                return Content(workbook.ToJson(), Telerik.Web.Spreadsheet.MimeTypes.JSON);
//            }
//            return Json(new { data = "" });
//        }

//        [HttpPost]
//        public ActionResult Upload(HttpPostedFileBase file)
//        {
//            var workbook = Workbook.Load(file.InputStream, Path.GetExtension(file.FileName));
//            return Content(workbook.ToJson(), Telerik.Web.Spreadsheet.MimeTypes.JSON);
//        }

//        [HttpPost]
//        public ActionResult Download(EDRDataViewModel model)
//        {
//            var workbook = Workbook.FromJson(model.Data);
//            using (var stream = new MemoryStream())
//            {
//                workbook.Save(stream, model.Extension);
//                byte[] bytes;
//                var mimeType = Telerik.Web.Spreadsheet.MimeTypes.ByExtension[model.Extension];
//                bytes = stream.ToArray();

//                var fileVM = new FileViewModel
//                {
//                    AttachmentType = AttachmentTypeEnum.File,
//                    FileName = model.FileType.ToString(),
//                    FileExtension = model.Extension,
//                    ContentType = mimeType,
//                    ContentLength = stream.Length,
//                    ContentByte = bytes,

//                };
//                var result = _fileBusiness.Create(fileVM);
//                var edrModel = new EDRDataViewModel { FileId = fileVM.Id,FileType=model.FileType };
//                _edrBusiness.Create(edrModel);

//                //return File(stream.ToArray(), mimeType, type.ToString() + extension);
//                return Json(new { success = true });
//                // return View("_Manage");
//            }
//        }

//        [HttpGet]
//        public ActionResult GetRevision(EDRMDRFileTypeEnum? type)
//        {
//            var list = new List<EDRDataViewModel>();
//            if (type.IsNotNull())
//            {
//                list = _edrBusiness.GetActiveList().Where(x => x.FileType == type).OrderByDescending(x => x.CreatedDate).ToList();
//            }
//                return Json(list, JsonRequestBehavior.AllowGet);
//        }
//        public string GetCellValues(JToken row,int index)
//        {
//            JArray cells = (JArray)row.SelectToken("cells");

            
//            foreach (JToken cell in cells)
//            {
//                var z = (int)cell.SelectToken("index");
//                if (z == index)
//                {
//                    return (string)cell.SelectToken("value");
//                }
                
//            }
//            return string.Empty;
//        }
//        public string GetCellBackground(JToken row, int index)
//        {
//            JArray cells = (JArray)row.SelectToken("cells");


//            foreach (JToken cell in cells)
//            {
//                var z = (int)cell.SelectToken("index");
//                if (z == index)
//                {
//                    return (string)cell.SelectToken("background");
//                }

//            }
//            return string.Empty;
//        }

//        public string GetCellValuesByString(JToken row, string str)
//        {
//            JArray cells = (JArray)row.SelectToken("cells");

                
//            foreach (JToken cell in cells)
//            {
//                var z = (int)cell.SelectToken("index");
             
//                var val = (string)cell.SelectToken("value");
//                if (val.IsNotNullAndNotEmpty())
//                {
//                    if (val.ToLower().StartsWith(str.ToLower()))
//                    {
//                        return (string)cell.SelectToken("value");
//                    }
//                }            
//            }
//            return string.Empty;
//        }
//        [HttpPost]
//        public ActionResult LoadGalfarEngineeringDocuments1(string data,long templateMasterId,string templateMasterName,long workspaceId,long folderId)
//        {
//            var errorList = new List<string>();
//            var udflist = _noteBusiness.DynamicUdfColumns(templateMasterId);
//            JObject jObject = JObject.Parse(data);
//            string sheet1 = (string)jObject.SelectToken("activeSheet");            
//            JArray sheets = (JArray)jObject.SelectToken("sheets");
//            foreach (JToken sheet in sheets)
//            {
//                string sheetname = (string)sheet.SelectToken("name");
//                if (sheetname == sheet1)
//                {
//                    var list = new List<long>();

//                    JArray rows = (JArray)sheet.SelectToken("rows");
//                    foreach (JToken row in rows)
//                    {

//                        var noteno = GetCellValues(row, 0);
//                        if (noteno != null)
//                        {
//                            if (noteno.ToLower().Contains("document") || noteno.ToLower().Contains("rfi no") || noteno.ToLower().Contains("==="))
//                            {
//                                continue;
//                            }
//                            //string pattern = "\\b" + noteno + "\\b";
//                            string pattern = "" + noteno + "";
//                            //Regex regReplace = new Regex(pattern);

//                            var subject = GetCellValues(row, 1);
//                            var revision = GetCellValuesByString(row, "rev-");
//                            var existDoc = new List<NoteViewModel>();
//                            if (revision.IsNotNullAndNotEmpty())
//                            {
//                                existDoc = _noteBusiness.GetDocumentByNoteAndRevision(templateMasterId, noteno, revision).ToList();
//                            }
//                            else
//                            {
//                                var exist = _noteBusiness.GetSingle(x => x.NoteNo == noteno);
//                                if (exist.IsNotNull())
//                                {
//                                    existDoc.Add(exist);
//                                }
                                
//                            }
                             
//                            if (existDoc.Count == 0)
//                            {
//                                //var workspace = _noteBusiness.GetWorkspaceDataForAdmin().Where(x => x.Name.ToLower().Contains("technip workspace")).FirstOrDefault();

//                                try
//                                {
//                                    if (workspaceId > 0)
//                                    {
//                                        var doc = _noteBusiness.GetDetails(new NoteViewModel
//                                        {
//                                            Id = 0,
//                                            TemplateId = 0,
//                                            TemplateMasterId = templateMasterId,
//                                            ActiveUserId = LoggedInUserId,
//                                            RequestedByUserId = LoggedInUserId,
//                                            OwnerUserId = LoggedInUserId,
//                                            Operation = DataOperation.Create,
//                                            ReferenceType = NoteReferenceTypeEnum.Self,
//                                            WorkspaceId = workspaceId,//workspace.Id,

//                                        });
//                                        doc.TemplateAction = NtsActionEnum.Submit;
//                                        doc.Operation = DataOperation.Create;
//                                        doc.NoteNo = noteno;
//                                        doc.Subject = subject;
//                                        var approveType = "";
//                                        //var isLatest = fields[26];
//                                        //var genFile = fields[27];
//                                        //if (genFile.IsNotNullAndNotEmpty())
//                                        //{
//                                        //    var genhead = _fileBusiness.GetList(x => x.FileName == genFile).OrderByDescending(x=>x.Id).FirstOrDefault();
//                                        //    if (genhead != null)
//                                        //        doc.CSVFileIds = genhead.Id + "";
//                                        //}                                    
//                                        foreach (var item in doc.Controls.Where(x => x.FieldPartialViewName != "NTS_Group"))
//                                        {
//                                            for (int j = 0; j <= udflist.Count - 1; j++)
//                                            {
//                                                int k = j + 2;
//                                                var field = GetCellValues(row, k);
//                                                if (item.FieldName == udflist[j].FieldName)
//                                                {
//                                                    if (item.FieldPartialViewName == "NTS_Attachment")
//                                                    {
//                                                        if (field.IsNotNullAndNotEmpty())
//                                                        {
//                                                            var file = _fileBusiness.GetFileByName(field);
//                                                            if (file.IsNotNull())
//                                                            {
//                                                                item.Code = file.Id.ToString();
//                                                                item.Value = file.FileName;
//                                                            }

//                                                        }
//                                                        break;
//                                                    }
//                                                    else if (item.FieldPartialViewName == "NTS_DropDownList" && item.DataSourceControllerName == "ListOfValue")
//                                                    {
//                                                        if (field.IsNotNullAndNotEmpty())
//                                                        {
//                                                            var parentValue = udflist[j].DataSourceHtmlAttributesString;
//                                                            string substr = parentValue.Substring(parentValue.IndexOf(":") + 2);
//                                                            string substr1 = substr.Substring(0, substr.IndexOf("\"}"));
//                                                            var lov = _lovBusiness.GetListOfValueByParentAndVlaue(substr1, field);
//                                                            if (lov != null)
//                                                            {
//                                                                item.Code = lov.Code;
//                                                                item.Value = lov.Name;
//                                                            }
//                                                        }
//                                                        break;
//                                                    }
//                                                    else if (item.FieldPartialViewName == "NTS_DropDownList" && item.DataSourceActionName == "GetEnumIdNameList")
//                                                    {
//                                                        if (field.IsNotNullAndNotEmpty())
//                                                        {
//                                                            var code = field.Replace(" ", string.Empty);
//                                                            if (code != null)
//                                                            {
//                                                                item.Code = code;
//                                                                item.Value = field;
//                                                            }
//                                                            if (item.FieldName.Contains("documentApprovalStatusType"))
//                                                            {

//                                                                if (!field.ToLower().Contains("manual") && !field.ToLower().Contains("not"))
//                                                                {
//                                                                    var item1 = doc.Controls.FirstOrDefault(x => x.FieldName == "documentApprovalStatus");
//                                                                    if (item1.IsNotNull())
//                                                                    {
//                                                                        approveType = DocumentApprovalStatusEnum.ApprovalInProgress.ToString();
//                                                                        item1.Code = DocumentApprovalStatusEnum.ApprovalInProgress.ToString();
//                                                                        item1.Value = DocumentApprovalStatusEnum.ApprovalInProgress.Description().ToString();
//                                                                    }

//                                                                }
//                                                                else
//                                                                {
//                                                                    var item1 = doc.Controls.FirstOrDefault(x => x.FieldName == "documentApprovalStatus");
//                                                                    if (item1.IsNotNull())
//                                                                    {
//                                                                        item1.Code = DocumentApprovalStatusEnum.ApprovedManually.ToString();
//                                                                        item1.Value = DocumentApprovalStatusEnum.ApprovedManually.Description().ToString();
//                                                                    }
//                                                                }

//                                                            }

//                                                        }
//                                                        break;
//                                                    }
//                                                    else if (item.FieldPartialViewName == "NTS_DropDownList" && item.DataSourceActionName != "ListOfValue" && item.DataSourceActionName != "GetEnumIdNameList")
//                                                    {
//                                                        if (field.IsNotNullAndNotEmpty())
//                                                        {
//                                                            var user = _userBusiness.GetSingle(x => x.UserName == field || x.Email == field);
//                                                            if (user.IsNotNull())
//                                                            {
//                                                                item.Code = user.Id.ToString();
//                                                                item.Value = user.UserName;
//                                                            }
//                                                        }
//                                                        break;
//                                                    }
//                                                    else if (item.FieldPartialViewName == "NTS_DatePicker" || item.FieldPartialViewName == "NTS_DateTimePicker")
//                                                    {
//                                                        if (field.IsNotNullAndNotEmpty())
//                                                        {
//                                                            double d = double.Parse(field);
//                                                            DateTime conv = DateTime.FromOADate(d);
//                                                            item.Code = conv.ToString();
//                                                            item.Value = conv.ToDefaultDateFormat();
//                                                        }
//                                                        break;
//                                                    }
//                                                    else
//                                                    {
//                                                        item.Code = field;
//                                                        item.Value = field;
//                                                        break;
//                                                    }
//                                                }
//                                            }

//                                            if (item.FieldName == "workspaceId")
//                                            {
//                                                item.Code = workspaceId.ToString();
//                                                item.Value = workspaceId.ToString();
//                                            }
//                                        }
//                                        //var folder = _noteBusiness.GetAllChildForWorkspace(workspace.Id).Where(x => x.Subject != null && x.Subject.ToLower().Contains("incoming document")).FirstOrDefault();


//                                        //long folderId = 0;
//                                        //if (folder != null)
//                                        //{
//                                        //    folderId = folder.Id;
//                                        //}
//                                        //else
//                                        //{
//                                        //    var foldermodel = new NoteViewModel
//                                        //    {
//                                        //        TemplateMasterCode = "GENERAL_FOLDER",
//                                        //        Operation = DataOperation.Create,
//                                        //        OwnerUserId = workspace.OwnerUserId,
//                                        //        ActiveUserId = workspace.OwnerUserId,
//                                        //        RequestedByUserId = workspace.OwnerUserId,
//                                        //        TemplateAction = NtsActionEnum.Submit,
//                                        //        ReferenceType = NoteReferenceTypeEnum.Self,
//                                        //        ParentId = workspace.Id,
//                                        //        StartDate = DateTime.Now.ApplicationNow().Date,
//                                        //        WorkspaceId = workspace.Id,
//                                        //    };

//                                        //    var newmodel1 = _noteBusiness.GetDetails(foldermodel);
//                                        //    newmodel1.Subject = "Incoming Document";
//                                        //    newmodel1.TemplateAction = NtsActionEnum.Submit;
//                                        //    _noteBusiness.Manage(newmodel1);
//                                        //    folderId = newmodel1.Id;
//                                        //}
//                                        doc.ParentId = folderId;
//                                        var result = _noteBusiness.Manage(doc);
//                                        if (result.IsSuccess)
//                                        {
//                                            if (approveType == DocumentApprovalStatusEnum.ApprovalInProgress.ToString())
//                                            {
//                                                var workflowId = _noteBusiness.GetServiceWorkflowTemplateId(templateMasterId);
//                                                if (workflowId.IsNotNull() && workflowId != 0)
//                                                {
//                                                    var service = _servicebusiness.GetServiceDetails(new ServiceViewModel
//                                                    {
//                                                        Id = 0,
//                                                        TemplateId = 0,
//                                                        TemplateMasterId = workflowId,
//                                                        ActiveUserId = LoggedInUserId,
//                                                        RequestedByUserId = LoggedInUserId,
//                                                        OwnerUserId = LoggedInUserId,
//                                                        Operation = DataOperation.Create

//                                                    });
//                                                    service.Operation = DataOperation.Create;
//                                                    service.TemplateAction = NtsActionEnum.Submit;
//                                                    var stepTasks = _taskBusiness.GetServiceStepsForAdd(service);
//                                                    stepTasks.ForEach(x => x.SLA = null);
//                                                    service.ServiceTasksList = stepTasks;
//                                                    service.ServiceTasks = new JavaScriptSerializer().Serialize(stepTasks);
//                                                    var regularDocument1 = service.Controls.FirstOrDefault(x => x.FieldName == "RegularDocument1");
//                                                    if (regularDocument1.IsNotNull())
//                                                    {
//                                                        regularDocument1.Code = result.Item.Id.ToString();
//                                                        regularDocument1.Value = result.Item.Subject;
//                                                    }
//                                                    _servicebusiness.Manage(service);
//                                                }

//                                            }
//                                            else
//                                            {
//                                                if (templateMasterName == "Engineering Document")
//                                                {
//                                                    MoveEngineeringDocument(result.Item);
//                                                }
//                                                else if (templateMasterName == "Project Documents")
//                                                {
//                                                    MoveProjectDocument(result.Item);
//                                                }
//                                                else if (templateMasterName == "Vendor Documents")
//                                                {
//                                                    MoveVendorDocument(result.Item);
//                                                }
//                                            }
//                                            var replace1 = noteno + "===Completed";
//                                            data = data.Replace(pattern, replace1);
//                                            //data = regReplace.Replace(data, replace1, 1);
//                                            //data = data.Replace(noteno, noteno + "===Completed");
//                                            var j = Json(new { success = true, errors = errorList, excelData = data });
//                                            j.MaxJsonLength = int.MaxValue;
//                                            return j;
//                                        }
//                                        else
//                                        {
//                                            var replace2 = noteno + "===Error[Mandatory Fields Req.]";
//                                            data = data.Replace(pattern, replace2);
//                                            //data = regReplace.Replace(data, replace2, 1);
//                                            //data =data.Replace(noteno, noteno + " error=" + result.MessageString);
//                                            //errorList.Add(result.MessageString);
//                                            var j = Json(new { success = false, errors = errorList, excelData = data });
//                                            j.MaxJsonLength = int.MaxValue;
//                                            return j;
//                                        }
//                                    }

//                                }
//                                catch (Exception ex)
//                                {
//                                    var replace2 = noteno + "===Error";
//                                    // return Json(new { success = false, errors = ex.ToString() });
//                                    data = data.Replace(pattern, replace2);
//                                    //data = regReplace.Replace(data, replace2, 1);
//                                    //data = data.Replace(noteno, noteno + " error=" + ex.ToString());
//                                    //errorList.Add(result.MessageString);
//                                    var j = Json(new { success = false, errors = errorList, excelData = data });
//                                    j.MaxJsonLength = int.MaxValue;
//                                    return j;
//                                }
//                            }
//                            else
//                            {
//                                try
//                                {
//                                    bool success = false;
//                                    foreach (var document in existDoc)
//                                    {
//                                        var doc = _noteBusiness.GetDetails(new NoteViewModel
//                                        {
//                                            Id = document.Id,
//                                            Operation = DataOperation.Correct,
//                                            ReferenceType = NoteReferenceTypeEnum.Self,

//                                        });
//                                        doc.TemplateAction = NtsActionEnum.EditAsNewVersion;
//                                        doc.Operation = DataOperation.Correct;
//                                        doc.NoteNo = noteno;
//                                        doc.Subject = subject;
//                                        //var isLatest = fields[26];
//                                        //var genFile = fields[27];
//                                        //if (genFile.IsNotNullAndNotEmpty())
//                                        //{
//                                        //    var genhead = _fileBusiness.GetList(x=> x.FileName == genFile).OrderByDescending(x => x.Id).FirstOrDefault();
//                                        //    if (genhead != null)
//                                        //        doc.CSVFileIds = genhead.Id + "";
//                                        //}
//                                        foreach (var item in doc.Controls.Where(x => x.FieldPartialViewName != "NTS_Group"))
//                                        {
//                                            for (int j = 0; j <= udflist.Count - 1; j++)
//                                            {
//                                                int k = j + 2;
//                                                var field = GetCellValues(row, k);
//                                                if (item.FieldName == udflist[j].FieldName)
//                                                {
//                                                    if (item.FieldPartialViewName == "NTS_Attachment")
//                                                    {
//                                                        if (field.IsNotNullAndNotEmpty())
//                                                        {
//                                                            var file = _fileBusiness.GetFileByName(field);
//                                                            if (file.IsNotNull())
//                                                            {
//                                                                item.Code = file.Id.ToString();
//                                                                item.Value = file.FileName;
//                                                            }
//                                                        }
//                                                        break;
//                                                    }
//                                                    else if (item.FieldPartialViewName == "NTS_DropDownList" && item.DataSourceControllerName == "ListOfValue")
//                                                    {
//                                                        if (field.IsNotNullAndNotEmpty())
//                                                        {
//                                                            var parentValue = udflist[j].DataSourceHtmlAttributesString;
//                                                            string substr = parentValue.Substring(parentValue.IndexOf(":") + 2);
//                                                            string substr1 = substr.Substring(0, substr.IndexOf("\"}"));
//                                                            var lov = _lovBusiness.GetListOfValueByParentAndVlaue(substr1, field);
//                                                            if (lov != null)
//                                                            {
//                                                                item.Code = lov.Code;
//                                                                item.Value = lov.Name;
//                                                            }
//                                                        }
//                                                        break;
//                                                    }
//                                                    else if (item.FieldPartialViewName == "NTS_DropDownList" && item.DataSourceActionName == "GetEnumIdNameList")
//                                                    {
//                                                        if (field.IsNotNullAndNotEmpty())
//                                                        {
//                                                            var code = field.Replace(" ", string.Empty);
//                                                            if (code != null)
//                                                            {
//                                                                item.Code = code;
//                                                                item.Value = field;
//                                                            }
//                                                        }
//                                                        break;
//                                                    }
//                                                    else if (item.FieldPartialViewName == "NTS_DropDownList" && item.DataSourceActionName != "ListOfValue" && item.DataSourceActionName != "GetEnumIdNameList")
//                                                    {
//                                                        if (field.IsNotNullAndNotEmpty())
//                                                        {
//                                                            var user = _userBusiness.GetSingle(x => x.UserName == field || x.Email == field);
//                                                            if (user.IsNotNull())
//                                                            {
//                                                                item.Code = user.Id.ToString();
//                                                                item.Value = user.UserName;
//                                                            }
//                                                        }
//                                                        break;
//                                                    }
//                                                    else if (item.FieldPartialViewName == "NTS_DatePicker" || item.FieldPartialViewName == "NTS_DateTimePicker")
//                                                    {
//                                                        if (field.IsNotNullAndNotEmpty())
//                                                        {
//                                                            double d = double.Parse(field);
//                                                            DateTime conv = DateTime.FromOADate(d);
//                                                            item.Code = conv.ToString();
//                                                            item.Value = conv.ToDefaultDateFormat();
//                                                        }
//                                                        break;
//                                                    }
//                                                    else
//                                                    {
//                                                        if (field.IsNotNullAndNotEmpty())
//                                                        {
//                                                            item.Code = field;
//                                                            item.Value = field;
//                                                        }
//                                                        break;
//                                                    }
//                                                }
//                                            }

//                                        }
//                                        var result = _noteBusiness.Manage(doc);
//                                        success = result.IsSuccess;
                                       

//                                    }
//                                    if (!success)
//                                    {
//                                        var replace2 = noteno + "===Error";
//                                        data = data.Replace(pattern, replace2);
//                                        //data = regReplace.Replace(data, replace2, 1);
//                                        //data = data.Replace(noteno, noteno + "===error" + result.MessageString);
//                                        //errorList.Add(result.MessageString);
//                                        var j = Json(new { success = false, errors = errorList, excelData = data });
//                                        j.MaxJsonLength = int.MaxValue;
//                                        return j;
//                                    }
//                                    else
//                                    {
//                                        var replace1 = noteno + "===Completed";
//                                        data = data.Replace(pattern, replace1);
//                                        //data = regReplace.Replace(data, replace1, 1);
//                                        //data = data.Replace(noteno, noteno + "===Completed");
//                                        var j = Json(new { success = true, errors = errorList, excelData = data });
//                                        j.MaxJsonLength = int.MaxValue;
//                                        return j;
//                                    }
//                                }
//                                catch (Exception ex)
//                                {
//                                    var replace2 = noteno + "===Error";
//                                    data = data.Replace(pattern, replace2);
//                                    //data = regReplace.Replace(data, replace2, 1);
//                                    //data = data.Replace(noteno, noteno + "===error" + ex.ToString());
//                                    //errorList.Add(result.MessageString);
//                                    var j = Json(new { success = false, errors = errorList, excelData = data });
//                                    j.MaxJsonLength = int.MaxValue;
//                                    return j;
//                                }


//                            }
//                        }
//                        else
//                        {
//                            break;
//                        }

//                    }
//                    break;
//                }
                
//            }         
//            return Json(new { success = true });
           
//        }
//        [HttpPost]
//        public ActionResult ValidateGalfarEngineeringDocuments(string data, long templateMasterId, string templateMasterName, long workspaceId, long folderId)
//        {
//            var errorList = new List<string>();
//            var udflist = _noteBusiness.DynamicUdfColumns(templateMasterId);
//            JObject jObject = JObject.Parse(data);
//            string sheet1 = (string)jObject.SelectToken("activeSheet");
//            JArray sheets = (JArray)jObject.SelectToken("sheets");
//            foreach (JToken sheet in sheets)
//            {
//                string sheetname = (string)sheet.SelectToken("name");
//                if (sheetname == sheet1)
//                {
//                    var list = new List<long>();
//                    int z = 1;
//                    JArray rows = (JArray)sheet.SelectToken("rows");
//                    foreach (JToken row in rows)
//                    {


//                        var noteno = GetCellValues(row, 0);
//                        if (noteno != null)
//                        {
//                            if (noteno.ToLower().Contains("document")|| noteno.ToLower().Contains("rfi no") || noteno.ToLower().Contains("==="))
//                            {
//                                z++;
//                                continue;
//                            }
//                            var subject = GetCellValues(row, 1);
//                            var revision = GetCellValuesByString(row, "rev-");
//                            var document = new NoteViewModel();
//                            if (revision.IsNotNullAndNotEmpty())
//                            {
//                                document = _noteBusiness.GetDocumentByNoteAndRevision(templateMasterId, noteno, revision).FirstOrDefault();
//                            }
//                            else
//                            {
//                                document = _noteBusiness.GetSingle(x => x.NoteNo == noteno);
//                            }
//                            if (!document.IsNotNull())
//                            {
//                                //var workspace = _noteBusiness.GetWorkspaceDataForAdmin().Where(x => x.Name.ToLower().Contains("technip workspace")).FirstOrDefault();

//                                try
//                                {
//                                    if (workspaceId > 0)
//                                    {
//                                        var doc = _noteBusiness.GetDetails(new NoteViewModel
//                                        {
//                                            Id = 0,
//                                            TemplateId = 0,
//                                            TemplateMasterId = templateMasterId,
//                                            ActiveUserId = LoggedInUserId,
//                                            RequestedByUserId = LoggedInUserId,
//                                            OwnerUserId = LoggedInUserId,
//                                            Operation = DataOperation.Read,
//                                            ReferenceType = NoteReferenceTypeEnum.Self,
//                                            WorkspaceId = workspaceId,//workspace.Id,

//                                        });
//                                        var approveType = "";
//                                        //var isLatest = fields[26];
//                                        //var genFile = fields[27];
//                                        //if (genFile.IsNotNullAndNotEmpty())
//                                        //{
//                                        //    var genhead = _fileBusiness.GetList(x => x.FileName == genFile).OrderByDescending(x=>x.Id).FirstOrDefault();
//                                        //    if (genhead != null)
//                                        //        doc.CSVFileIds = genhead.Id + "";
//                                        //}                                    
//                                        foreach (var item in doc.Controls.Where(x => x.FieldPartialViewName != "NTS_Group"))
//                                        {
//                                            for (int j = 0; j <= udflist.Count - 1; j++)
//                                            {
//                                                int k = j + 2;
//                                                var field = GetCellValues(row, k);
//                                                if (item.FieldName == udflist[j].FieldName)
//                                                {
//                                                    if (item.FieldPartialViewName == "NTS_Attachment")
//                                                    {
//                                                        if (field.IsNotNullAndNotEmpty())
//                                                        {
//                                                            var file = _fileBusiness.GetFileByName(field);
//                                                            if (!file.IsNotNull())
//                                                            {
//                                                                errorList.Add(string.Concat("At row " + z + "Document No:" + noteno + " ,File does not exist :", udflist[j].LabelDisplayName));

//                                                            }
//                                                        }
//                                                        break;
//                                                    }
//                                                    else if (item.FieldPartialViewName == "NTS_DropDownList" && item.DataSourceControllerName == "ListOfValue")
//                                                    {
//                                                        if (field.IsNotNullAndNotEmpty())
//                                                        {
//                                                            var parentValue = udflist[j].DataSourceHtmlAttributesString;
//                                                            string substr = parentValue.Substring(parentValue.IndexOf(":") + 2);
//                                                            string substr1 = substr.Substring(0, substr.IndexOf("\"}"));
//                                                            var lov = _lovBusiness.GetListOfValueByParentAndVlaue(substr1, field);
//                                                            if (lov == null)
//                                                            {
//                                                                errorList.Add(string.Concat("At row " + z + "Document No:" + noteno + " ,value does not exist :", udflist[j].LabelDisplayName));

//                                                            }
//                                                        }
//                                                        break;
//                                                    }
//                                                    else if (item.FieldPartialViewName == "NTS_DropDownList" && item.DataSourceActionName == "GetEnumIdNameList")
//                                                    {
//                                                        if (field.IsNotNullAndNotEmpty())
//                                                        {
//                                                            var parentValue = udflist[j].DataSourceHtmlAttributesString;
//                                                            string substr = parentValue.Substring(parentValue.IndexOf(":") + 2);
//                                                            string substr1 = substr.Substring(0, substr.IndexOf("\"}"));
//                                                            var code = field.Replace(" ", string.Empty);
//                                                            if (!GetValidateEnumValue(substr1, code))
//                                                            {
//                                                                errorList.Add(string.Concat("At row " + z + "Document No:" + noteno + " ,value does not exist :", udflist[j].LabelDisplayName));
//                                                            }
//                                                        }
//                                                        break;
//                                                    }
//                                                    else if (item.FieldPartialViewName == "NTS_DropDownList" && item.DataSourceActionName != "ListOfValue" && item.DataSourceActionName != "GetEnumIdNameList")
//                                                    {
//                                                        if (field.IsNotNullAndNotEmpty())
//                                                        {
//                                                            var user = _userBusiness.GetSingle(x => x.UserName == field || x.Email == field);
//                                                            if (!user.IsNotNull())
//                                                            {
//                                                                errorList.Add(string.Concat("At row " + z + "Document No:" + noteno + " ,User does not exist :", udflist[j].LabelDisplayName));
//                                                            }
//                                                        }
//                                                        break;
//                                                    }
//                                                    else if (item.FieldPartialViewName == "NTS_DatePicker" || item.FieldPartialViewName == "NTS_DateTimePicker")
//                                                    {
//                                                        if (field.IsNotNullAndNotEmpty())
//                                                        {
//                                                            try
//                                                            {
//                                                                double d = double.Parse(field);
//                                                                DateTime conv = DateTime.FromOADate(d);
//                                                            }
//                                                            catch (Exception)
//                                                            {
//                                                                errorList.Add(string.Concat("At row " + z + "Document No:" + noteno + " ,Incorrect Date Format :", udflist[j].LabelDisplayName));
//                                                            }

//                                                        }
//                                                        break;
//                                                    }
//                                                    else
//                                                    {
//                                                        break;
//                                                    }
//                                                }
//                                            }

//                                        }


//                                    }

//                                }
//                                catch (Exception ex)
//                                {
//                                    //return Json(new { success = false, errors = ex.ToString() });
//                                    errorList.Add(string.Concat("At row " + z + "Document No:" + noteno + " ,Upload fialed :", noteno));
//                                }
//                            }
//                            else
//                            {
//                                try
//                                {
//                                    var doc = _noteBusiness.GetDetails(new NoteViewModel
//                                    {
//                                        Id = document.Id,
//                                        Operation = DataOperation.Correct,
//                                        ReferenceType = NoteReferenceTypeEnum.Self,

//                                    });
//                                    //var isLatest = fields[26];
//                                    //var genFile = fields[27];
//                                    //if (genFile.IsNotNullAndNotEmpty())
//                                    //{
//                                    //    var genhead = _fileBusiness.GetList(x=> x.FileName == genFile).OrderByDescending(x => x.Id).FirstOrDefault();
//                                    //    if (genhead != null)
//                                    //        doc.CSVFileIds = genhead.Id + "";
//                                    //}
//                                    foreach (var item in doc.Controls.Where(x => x.FieldPartialViewName != "NTS_Group"))
//                                    {
//                                        for (int j = 0; j <= udflist.Count - 1; j++)
//                                        {
//                                            int k = j + 2;
//                                            var field = GetCellValues(row, k);
//                                            if (item.FieldName == udflist[j].FieldName)
//                                            {
//                                                if (item.FieldPartialViewName == "NTS_Attachment")
//                                                {
//                                                    if (field.IsNotNullAndNotEmpty())
//                                                    {
//                                                        var file = _fileBusiness.GetFileByName(field);
//                                                        if (!file.IsNotNull())
//                                                        {
//                                                            errorList.Add(string.Concat("At row " + z + "Document No:" + noteno + " ,File does not exist :", udflist[j].LabelDisplayName));

//                                                        }
//                                                    }
//                                                    break;
//                                                }
//                                                else if (item.FieldPartialViewName == "NTS_DropDownList" && item.DataSourceControllerName == "ListOfValue")
//                                                {
//                                                    if (field.IsNotNullAndNotEmpty())
//                                                    {
//                                                        var parentValue = udflist[j].DataSourceHtmlAttributesString;
//                                                        string substr = parentValue.Substring(parentValue.IndexOf(":") + 2);
//                                                        string substr1 = substr.Substring(0, substr.IndexOf("\"}"));
//                                                        var lov = _lovBusiness.GetListOfValueByParentAndVlaue(substr1, field);
//                                                        if (lov == null)
//                                                        {
//                                                            errorList.Add(string.Concat("At row " + z + "Document No:" + noteno + " ,value does not exist :", udflist[j].LabelDisplayName));
//                                                        }
//                                                    }
//                                                    break;
//                                                }
//                                                else if (item.FieldPartialViewName == "NTS_DropDownList" && item.DataSourceActionName == "GetEnumIdNameList")
//                                                {
//                                                    if (field.IsNotNullAndNotEmpty())
//                                                    {
//                                                        var parentValue = udflist[j].DataSourceHtmlAttributesString;
//                                                        string substr = parentValue.Substring(parentValue.IndexOf(":") + 2);
//                                                        string substr1 = substr.Substring(0, substr.IndexOf("\"}"));
//                                                        var code = field.Replace(" ", string.Empty);
//                                                        if (!GetValidateEnumValue(substr1, code))
//                                                        {
//                                                            errorList.Add(string.Concat("At row " + z + "Document No:" + noteno + " ,value does not exist :", udflist[j].LabelDisplayName));
//                                                        }
//                                                    }
//                                                    break;
//                                                }
//                                                else if (item.FieldPartialViewName == "NTS_DropDownList" && item.DataSourceActionName != "ListOfValue" && item.DataSourceActionName != "GetEnumIdNameList")
//                                                {
//                                                    if (field.IsNotNullAndNotEmpty())
//                                                    {
//                                                        var user = _userBusiness.GetSingle(x => x.UserName == field || x.Email == field);
//                                                        if (!user.IsNotNull())
//                                                        {
//                                                            errorList.Add(string.Concat("At row " + z + "Document No:" + noteno + " ,User does not exist :", udflist[j].LabelDisplayName));
//                                                        }
//                                                    }
//                                                    break;
//                                                }
//                                                else if (item.FieldPartialViewName == "NTS_DatePicker" || item.FieldPartialViewName == "NTS_DateTimePicker")
//                                                {
//                                                    if (field.IsNotNullAndNotEmpty())
//                                                    {
//                                                        try
//                                                        {
//                                                            double d = double.Parse(field);
//                                                            DateTime conv = DateTime.FromOADate(d);
//                                                        }
//                                                        catch (Exception)
//                                                        {
//                                                            errorList.Add(string.Concat("At row " + z + "Document No:" + noteno + " ,Incorrect Date Format :", udflist[j].LabelDisplayName));
//                                                        }
//                                                    }
//                                                    break;
//                                                }
//                                                else
//                                                {
//                                                    break;
//                                                }
//                                            }
//                                        }

//                                    }

//                                }
//                                catch (Exception ex)
//                                {

//                                    //return Json(new { success = false, errors = ex.ToString() });
//                                    errorList.Add(string.Concat("At row " + z + "Document No:" + noteno + " ,Upload fialed :", noteno));
//                                }


//                            }
//                            z++;
//                        }
//                        else
//                        {
//                            break;
//                        }

//                    }
//                    break;
//                }
               
//            }

//            if (errorList.Count > 0)
//            {
//                return Json(new { success = false, errors = errorList });
//            }
//            else
//            {
//                return LoadGalfarEngineeringDocuments1(data, templateMasterId, templateMasterName, workspaceId, folderId);
//            }


//        }
//        public ActionResult MoveEngineeringDocument(NoteViewModel doc)
//        {
//            string desciplin = doc.Controls.FirstOrDefault(x=>x.FieldName== "discipline").Value;
//            string revision = doc.Controls.FirstOrDefault(x => x.FieldName == "revision").Value;
//            var code = doc.Controls.FirstOrDefault(x => x.FieldName == "code");
//            var workspace = _noteBusiness.GetWorkspaceDataForAdmin().Where(x => x.Name.ToLower().Contains("engineering")).FirstOrDefault();

//            if (workspace != null)
//            {

//                var desiplin = _noteBusiness.GetAllChildForWorkspace(workspace.Id).Where(x => x.Subject != null && x.Subject.ToLower().Contains(desciplin.ToLower())).FirstOrDefault();
//                long desiplinFolderId = 0;
//                if (desiplin != null)
//                {
//                    desiplinFolderId = desiplin.Id;

//                }
//                else
//                {
//                    var foldermodel = new NoteViewModel
//                    {
//                        TemplateMasterCode = "GENERAL_FOLDER",
//                        Operation = DataOperation.Create,
//                        OwnerUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
//                        ActiveUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
//                        RequestedByUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
//                        TemplateAction = NtsActionEnum.Submit,
//                        ReferenceType = NoteReferenceTypeEnum.Self,
//                        ParentId = workspace.Id,
//                        StartDate = DateTime.Now.ApplicationNow().Date,
//                    };

//                    var newmodel1 = _noteBusiness.GetDetails(foldermodel);
//                    newmodel1.Subject = desciplin;
//                    newmodel1.TemplateAction = NtsActionEnum.Submit;
//                    _noteBusiness.Manage(newmodel1);
//                    desiplinFolderId = newmodel1.Id;
//                }

//                var rev = _noteBusiness.GetAllChildForWorkspace(desiplinFolderId).Where(x => x.Subject != null && x.Subject.ToLower().Contains(revision.ToLower())).FirstOrDefault();
//                long revParentId = 0;
//                if (rev != null)
//                {
//                    revParentId = rev.Id;
//                }
//                else
//                {
//                    var foldermodel = new NoteViewModel
//                    {
//                        TemplateMasterCode = "GENERAL_FOLDER",
//                        Operation = DataOperation.Create,
//                        OwnerUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
//                        ActiveUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
//                        RequestedByUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
//                        TemplateAction = NtsActionEnum.Submit,
//                        ReferenceType = NoteReferenceTypeEnum.Self,
//                        ParentId = desiplinFolderId,
//                        StartDate = DateTime.Now.ApplicationNow().Date,
//                    };

//                    var newmodel1 = _noteBusiness.GetDetails(foldermodel);
//                    newmodel1.Subject = revision;
//                    newmodel1.TemplateAction = NtsActionEnum.Submit;
//                    _noteBusiness.Manage(newmodel1);
//                    revParentId = newmodel1.Id;
//                }
//                var revnote = _noteBusiness.CopyDocument(doc.Id, revParentId, false, workspace.Id, workspace.Id, workspace.OwnerUserId);
//                //_repoNote.CreateOneToOneRelationshipToReferenceType(revnote.Id, new R_Note_Reference(), NodeEnum.NTS_Service, viewModel.ServiceId.Value);


//                if (revision.Contains("Rev"))
//                {
//                    var latestrevfolder = _noteBusiness.GetAllChildForWorkspace(desiplinFolderId).Where(x => x.Subject.ToLower().Contains("latest")).FirstOrDefault();
//                    if (latestrevfolder != null)
//                    {

//                        var revDoc = _noteBusiness.CheckDocumentNoExist(latestrevfolder.Id);
//                        if (revDoc != null)
//                        {
//                            var check = revDoc.Where(x => x.Name == doc.NoteNo).FirstOrDefault();

//                            if (check != null)
//                            {
//                                var doc1 = new NoteViewModel
//                                {
//                                    Id = check.Id.Value,
//                                    Operation = DataOperation.Correct,
//                                };
//                                _noteBusiness.Delete(doc1);
//                            }
//                            var note = _noteBusiness.CopyDocument(doc.Id, latestrevfolder.Id, false, workspace.Id, workspace.Id, workspace.OwnerUserId);
//                            //_repoNote.CreateOneToOneRelationshipToReferenceType(note.Id, new R_Note_Reference(), NodeEnum.NTS_Service, viewModel.ServiceId.Value);
//                        }

//                    }
//                    else
//                    {
//                        var foldermodel = new NoteViewModel
//                        {
//                            TemplateMasterCode = "GENERAL_FOLDER",
//                            Operation = DataOperation.Create,
//                            OwnerUserId = workspace.OwnerUserId,
//                            ActiveUserId = workspace.OwnerUserId,
//                            RequestedByUserId = workspace.OwnerUserId,
//                            TemplateAction = NtsActionEnum.Submit,
//                            ReferenceType = NoteReferenceTypeEnum.Self,
//                            ParentId = desiplinFolderId,
//                            StartDate = DateTime.Now.ApplicationNow().Date,
//                        };

//                        var newmodel1 = _noteBusiness.GetDetails(foldermodel);
//                        newmodel1.Subject = "Latest revision Files";
//                        newmodel1.TemplateAction = NtsActionEnum.Submit;
//                        _noteBusiness.Manage(newmodel1);
//                        var note = _noteBusiness.CopyDocument(doc.Id, newmodel1.Id, false, workspace.Id, workspace.Id, workspace.OwnerUserId);
//                        //_repoNote.CreateOneToOneRelationshipToReferenceType(note.Id, new R_Note_Reference(), NodeEnum.NTS_Service, viewModel.ServiceId.Value);
//                    }

//                }
//                if (code.IsNotNull() && code.Value.IsNotNull() && code.Value.Contains("AFC"))
//                {

//                    long parentId = 0;
//                    var folder = _noteBusiness.GetAllChildForWorkspace(desiplin.Id).Where(x => x.Subject != null && x.Subject.ToLower().Contains("signed")).FirstOrDefault();
//                    if (folder != null)
//                    {
//                        parentId = folder.Id;
//                    }
//                    else
//                    {
//                        var foldermodel = new NoteViewModel
//                        {
//                            TemplateMasterCode = "GENERAL_FOLDER",
//                            Operation = DataOperation.Create,
//                            OwnerUserId = workspace.OwnerUserId,
//                            ActiveUserId = workspace.OwnerUserId,
//                            RequestedByUserId = workspace.OwnerUserId,
//                            TemplateAction = NtsActionEnum.Submit,
//                            ReferenceType = NoteReferenceTypeEnum.Self,
//                            ParentId = desiplin.Id,
//                            StartDate = DateTime.Now.ApplicationNow().Date,
//                            WorkspaceId = workspace.Id,
//                        };

//                        var newmodel = _noteBusiness.GetDetails(foldermodel);
//                        newmodel.Subject = "Signed AFC";
//                        newmodel.TemplateAction = NtsActionEnum.Submit;
//                        newmodel.ParentId = desiplin.Id;
//                        _noteBusiness.Manage(newmodel);
//                        parentId = newmodel.Id;
//                    }

//                    if (parentId != 0)
//                    {
//                        var revDoc = _noteBusiness.CheckDocumentNoExist(parentId);
//                        if (revDoc != null)
//                        {
//                            var check = revDoc.Where(x => x.Name == doc.NoteNo).FirstOrDefault();

//                            if (check != null)
//                            {
//                                var doc1 = new NoteViewModel
//                                {
//                                    Id = check.Id.Value,
//                                    Operation = DataOperation.Correct,
//                                };
//                                _noteBusiness.Delete(doc1);
//                            }
//                            var note = _noteBusiness.CopyDocument(doc.Id, parentId, false, workspace.Id, workspace.Id, workspace.OwnerUserId);
//                        }
//                    }

//                }

//            }
//            return null;
//        }
//        public ActionResult MoveProjectDocument(NoteViewModel doc)
//        {
//            var desciplin = doc.Controls.FirstOrDefault(x => x.FieldName == "discipline");
//            var revision = doc.Controls.FirstOrDefault(x => x.FieldName == "revision");
//            var projectFolder = doc.Controls.FirstOrDefault(x => x.FieldName == "projectFolder");
//            var projectSubFolder = doc.Controls.FirstOrDefault(x => x.FieldName == "projectSubFolder");
//            var code = doc.Controls.FirstOrDefault(x => x.FieldName == "code");
//            var workspace = _noteBusiness.GetWorkspaceDataForAdmin().Where(x => x.Name.ToLower().Contains(projectFolder.Value.ToLower())).FirstOrDefault();

//            if (workspace != null)
//            {
//                if (projectSubFolder.Code != "PSF_NA")
//                {
//                    var desiplin = _noteBusiness.GetAllChildForWorkspace(workspace.Id).Where(x => x.Subject != null && x.Subject.ToLower().Contains(desciplin.Value.ToLower())).FirstOrDefault();
//                    long desiplinFolderId = 0;
//                    if (desiplin != null)
//                    {
//                        desiplinFolderId = desiplin.Id;

//                    }
//                    else
//                    {
//                        var foldermodel = new NoteViewModel
//                        {
//                            TemplateMasterCode = "GENERAL_FOLDER",
//                            Operation = DataOperation.Create,
//                            OwnerUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
//                            ActiveUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
//                            RequestedByUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
//                            TemplateAction = NtsActionEnum.Submit,
//                            ReferenceType = NoteReferenceTypeEnum.Self,
//                            ParentId = workspace.Id,
//                            StartDate = DateTime.Now.ApplicationNow().Date,
//                        };

//                        var newmodel1 = _noteBusiness.GetDetails(foldermodel);
//                        newmodel1.Subject = desciplin.Value;
//                        newmodel1.TemplateAction = NtsActionEnum.Submit;
//                        _noteBusiness.Manage(newmodel1);
//                        desiplinFolderId = newmodel1.Id;
//                    }
//                    var statement = _noteBusiness.GetAllChildForWorkspace(desiplinFolderId).Where(x => x.Subject != null && x.Subject.ToLower().Contains(projectSubFolder.Value.ToLower())).FirstOrDefault();
//                    long statementFolderId = 0;
//                    if (statement != null)
//                    {
//                        statementFolderId = statement.Id;
//                    }
//                    else
//                    {
//                        var foldermodel = new NoteViewModel
//                        {
//                            TemplateMasterCode = "GENERAL_FOLDER",
//                            Operation = DataOperation.Create,
//                            OwnerUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
//                            ActiveUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
//                            RequestedByUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
//                            TemplateAction = NtsActionEnum.Submit,
//                            ReferenceType = NoteReferenceTypeEnum.Self,
//                            ParentId = desiplinFolderId,
//                            StartDate = DateTime.Now.ApplicationNow().Date,
//                        };

//                        var newmodel1 = _noteBusiness.GetDetails(foldermodel);
//                        newmodel1.Subject = projectSubFolder.Value;
//                        newmodel1.TemplateAction = NtsActionEnum.Submit;
//                        _noteBusiness.Manage(newmodel1);
//                        statementFolderId = newmodel1.Id;
//                    }
//                    var rev = _noteBusiness.GetAllChildForWorkspace(statementFolderId).Where(x => x.Subject != null && x.Subject.ToLower().Contains(revision.Value.ToLower())).FirstOrDefault();
//                    long revParentId = 0;
//                    if (rev != null)
//                    {
//                        revParentId = rev.Id;
//                    }
//                    else
//                    {
//                        var foldermodel = new NoteViewModel
//                        {
//                            TemplateMasterCode = "GENERAL_FOLDER",
//                            Operation = DataOperation.Create,
//                            OwnerUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
//                            ActiveUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
//                            RequestedByUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
//                            TemplateAction = NtsActionEnum.Submit,
//                            ReferenceType = NoteReferenceTypeEnum.Self,
//                            ParentId = statementFolderId,
//                            StartDate = DateTime.Now.ApplicationNow().Date,
//                        };

//                        var newmodel1 = _noteBusiness.GetDetails(foldermodel);
//                        newmodel1.Subject = revision.Value;
//                        newmodel1.TemplateAction = NtsActionEnum.Submit;
//                        _noteBusiness.Manage(newmodel1);
//                        revParentId = newmodel1.Id;
//                    }
//                    var revnote = _noteBusiness.CopyDocument(doc.Id, revParentId, false, workspace.Id, workspace.Id, workspace.OwnerUserId);
//                    //_repoNote.CreateOneToOneRelationshipToReferenceType(revnote.Id, new R_Note_Reference(), NodeEnum.NTS_Service, viewModel.ServiceId.Value);


//                    if (revision.Value.Contains("Rev"))
//                    {
//                        var latestrevfolder = _noteBusiness.GetAllChildForWorkspace(statementFolderId).Where(x => x.Subject.ToLower().Contains("latest")).FirstOrDefault();
//                        if (latestrevfolder != null)
//                        {

//                            var revDoc = _noteBusiness.CheckDocumentNoExist(latestrevfolder.Id);
//                            if (revDoc != null)
//                            {
//                                var check = revDoc.Where(x => x.Name == doc.NoteNo).FirstOrDefault();

//                                if (check != null)
//                                {
//                                    var doc1 = new NoteViewModel
//                                    {
//                                        Id = check.Id.Value,
//                                        Operation = DataOperation.Correct,
//                                    };
//                                    _noteBusiness.Delete(doc1);
//                                }
//                                var note = _noteBusiness.CopyDocument(doc.Id, latestrevfolder.Id, false, workspace.Id, workspace.Id, workspace.OwnerUserId);
//                                //_repoNote.CreateOneToOneRelationshipToReferenceType(note.Id, new R_Note_Reference(), NodeEnum.NTS_Service, viewModel.ServiceId.Value);
//                            }

//                        }
//                        else
//                        {
//                            var foldermodel = new NoteViewModel
//                            {
//                                TemplateMasterCode = "GENERAL_FOLDER",
//                                Operation = DataOperation.Create,
//                                OwnerUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
//                                ActiveUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
//                                RequestedByUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
//                                TemplateAction = NtsActionEnum.Submit,
//                                ReferenceType = NoteReferenceTypeEnum.Self,
//                                ParentId = statementFolderId,
//                                StartDate = DateTime.Now.ApplicationNow().Date,
//                            };

//                            var newmodel1 = _noteBusiness.GetDetails(foldermodel);
//                            newmodel1.Subject = "Latest revision Files";
//                            newmodel1.TemplateAction = NtsActionEnum.Submit;
//                            _noteBusiness.Manage(newmodel1);
//                            var note = _noteBusiness.CopyDocument(doc.Id, newmodel1.Id, false, workspace.Id, workspace.Id, workspace.OwnerUserId);
//                            //_repoNote.CreateOneToOneRelationshipToReferenceType(note.Id, new R_Note_Reference(), NodeEnum.NTS_Service, viewModel.ServiceId.Value);
//                        }

//                    }
//                    if (code.Value.Contains("AFC"))
//                    {
//                        long parentId = 0;
//                        var folder = _noteBusiness.GetAllChildForWorkspace(statementFolderId).Where(x => x.Subject != null && x.Subject.ToLower().Contains("signed")).FirstOrDefault();
//                        if (folder != null)
//                        {
//                            parentId = folder.Id;
//                        }
//                        else
//                        {
//                            var foldermodel = new NoteViewModel
//                            {
//                                TemplateMasterCode = "GENERAL_FOLDER",
//                                Operation = DataOperation.Create,
//                                OwnerUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
//                                ActiveUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
//                                RequestedByUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
//                                TemplateAction = NtsActionEnum.Submit,
//                                ReferenceType = NoteReferenceTypeEnum.Self,
//                                ParentId = statementFolderId,
//                                StartDate = DateTime.Now.ApplicationNow().Date,
//                            };

//                            var newmodel = _noteBusiness.GetDetails(foldermodel);
//                            newmodel.Subject = "Signed AFC";
//                            newmodel.TemplateAction = NtsActionEnum.Submit;
//                            newmodel.ParentId = statementFolderId;
//                            _noteBusiness.Manage(newmodel);
//                            parentId = newmodel.Id;
//                        }

//                        // doc.Id = 0;
//                        //doc.Operation = DataOperation.Create;
//                        //doc.ParentId = parentId;
//                        //_noteBussiness.Manage(doc);
//                        if (parentId != 0)
//                        {
//                            var revDoc = _noteBusiness.CheckDocumentNoExist(parentId);
//                            if (revDoc != null)
//                            {
//                                var check = revDoc.Where(x => x.Name == doc.NoteNo).FirstOrDefault();

//                                if (check != null)
//                                {
//                                    var doc1 = new NoteViewModel
//                                    {
//                                        Id = check.Id.Value,
//                                        Operation = DataOperation.Correct,
//                                    };
//                                    _noteBusiness.Delete(doc1);
//                                }
//                                var note = _noteBusiness.CopyDocument(doc.Id, parentId, false, workspace.Id, workspace.Id, workspace.OwnerUserId);
//                                //_repoNote.CreateOneToOneRelationshipToReferenceType(note.Id, new R_Note_Reference(), NodeEnum.NTS_Service, viewModel.ServiceId.Value);
//                            }
//                        }

//                    }


//                }
//                else
//                {
//                    var desiplin = _noteBusiness.GetAllChildForWorkspace(workspace.Id).Where(x => x.Subject != null && x.Subject.ToLower().Contains(desciplin.Value.ToLower())).FirstOrDefault();
//                    long desiplinFolderId = 0;
//                    if (desiplin != null)
//                    {
//                        desiplinFolderId = desiplin.Id;

//                    }
//                    else
//                    {
//                        var foldermodel = new NoteViewModel
//                        {
//                            TemplateMasterCode = "GENERAL_FOLDER",
//                            Operation = DataOperation.Create,
//                            OwnerUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
//                            ActiveUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
//                            RequestedByUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
//                            TemplateAction = NtsActionEnum.Submit,
//                            ReferenceType = NoteReferenceTypeEnum.Self,
//                            ParentId = workspace.Id,
//                            StartDate = DateTime.Now.ApplicationNow().Date,
//                        };

//                        var newmodel1 = _noteBusiness.GetDetails(foldermodel);
//                        newmodel1.Subject = desciplin.Value;
//                        newmodel1.TemplateAction = NtsActionEnum.Submit;
//                        _noteBusiness.Manage(newmodel1);
//                        desiplinFolderId = newmodel1.Id;
//                    }

//                    var rev = _noteBusiness.GetAllChildForWorkspace(desiplinFolderId).Where(x => x.Subject != null && x.Subject.ToLower().Contains(revision.Value.ToLower())).FirstOrDefault();
//                    long revParentId = 0;
//                    if (rev != null)
//                    {
//                        revParentId = rev.Id;
//                    }
//                    else
//                    {
//                        var foldermodel = new NoteViewModel
//                        {
//                            TemplateMasterCode = "GENERAL_FOLDER",
//                            Operation = DataOperation.Create,
//                            OwnerUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
//                            ActiveUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
//                            RequestedByUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
//                            TemplateAction = NtsActionEnum.Submit,
//                            ReferenceType = NoteReferenceTypeEnum.Self,
//                            ParentId = desiplinFolderId,
//                            StartDate = DateTime.Now.ApplicationNow().Date,
//                        };

//                        var newmodel1 = _noteBusiness.GetDetails(foldermodel);
//                        newmodel1.Subject = revision.Value;
//                        newmodel1.TemplateAction = NtsActionEnum.Submit;
//                        _noteBusiness.Manage(newmodel1);
//                        revParentId = newmodel1.Id;
//                    }
//                    var revnote = _noteBusiness.CopyDocument(doc.Id, revParentId, false, workspace.Id, workspace.Id, workspace.OwnerUserId);
//                    //_repoNote.CreateOneToOneRelationshipToReferenceType(revnote.Id, new R_Note_Reference(), NodeEnum.NTS_Service, viewModel.ServiceId.Value);


//                    if (revision.Value.Contains("Rev"))
//                    {
//                        var latestrevfolder = _noteBusiness.GetAllChildForWorkspace(desiplinFolderId).Where(x => x.Subject.ToLower().Contains("latest")).FirstOrDefault();
//                        if (latestrevfolder != null)
//                        {

//                            var revDoc = _noteBusiness.CheckDocumentNoExist(latestrevfolder.Id);
//                            if (revDoc != null)
//                            {
//                                var check = revDoc.Where(x => x.Name == doc.NoteNo).FirstOrDefault();

//                                if (check != null)
//                                {
//                                    var doc1 = new NoteViewModel
//                                    {
//                                        Id = check.Id.Value,
//                                        Operation = DataOperation.Correct,
//                                    };
//                                    _noteBusiness.Delete(doc1);
//                                }
//                                var note = _noteBusiness.CopyDocument(doc.Id, latestrevfolder.Id, false, workspace.Id, workspace.Id, workspace.OwnerUserId);
//                                //_repoNote.CreateOneToOneRelationshipToReferenceType(note.Id, new R_Note_Reference(), NodeEnum.NTS_Service, viewModel.ServiceId.Value);
//                            }

//                        }
//                        else
//                        {
//                            var foldermodel = new NoteViewModel
//                            {
//                                TemplateMasterCode = "GENERAL_FOLDER",
//                                Operation = DataOperation.Create,
//                                OwnerUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
//                                ActiveUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
//                                RequestedByUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
//                                TemplateAction = NtsActionEnum.Submit,
//                                ReferenceType = NoteReferenceTypeEnum.Self,
//                                ParentId = desiplinFolderId,
//                                StartDate = DateTime.Now.ApplicationNow().Date,
//                            };

//                            var newmodel1 = _noteBusiness.GetDetails(foldermodel);
//                            newmodel1.Subject = "Latest revision Files";
//                            newmodel1.TemplateAction = NtsActionEnum.Submit;
//                            _noteBusiness.Manage(newmodel1);
//                            var note = _noteBusiness.CopyDocument(doc.Id, newmodel1.Id, false, workspace.Id, workspace.Id, workspace.OwnerUserId);
//                            //_repoNote.CreateOneToOneRelationshipToReferenceType(note.Id, new R_Note_Reference(), NodeEnum.NTS_Service, viewModel.ServiceId.Value);
//                        }

//                    }
//                    if (code.Value.Contains("AFC"))
//                    {
//                        long parentId = 0;
//                        var folder = _noteBusiness.GetAllChildForWorkspace(desiplinFolderId).Where(x => x.Subject != null && x.Subject.ToLower().Contains("signed")).FirstOrDefault();
//                        if (folder != null)
//                        {
//                            parentId = folder.Id;
//                        }
//                        else
//                        {
//                            var foldermodel = new NoteViewModel
//                            {
//                                TemplateMasterCode = "GENERAL_FOLDER",
//                                Operation = DataOperation.Create,
//                                OwnerUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
//                                ActiveUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
//                                RequestedByUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
//                                TemplateAction = NtsActionEnum.Submit,
//                                ReferenceType = NoteReferenceTypeEnum.Self,
//                                ParentId = desiplin.Id,
//                                StartDate = DateTime.Now.ApplicationNow().Date,
//                            };

//                            var newmodel = _noteBusiness.GetDetails(foldermodel);
//                            newmodel.Subject = "Signed AFC";
//                            newmodel.TemplateAction = NtsActionEnum.Submit;
//                            newmodel.ParentId = desiplinFolderId;
//                            _noteBusiness.Manage(newmodel);
//                            parentId = newmodel.Id;
//                        }

//                        // doc.Id = 0;
//                        //doc.Operation = DataOperation.Create;
//                        //doc.ParentId = parentId;
//                        //_noteBussiness.Manage(doc);
//                        if (parentId != 0)
//                        {
//                            var revDoc = _noteBusiness.CheckDocumentNoExist(parentId);
//                            if (revDoc != null)
//                            {
//                                var check = revDoc.Where(x => x.Name == doc.NoteNo).FirstOrDefault();

//                                if (check != null)
//                                {
//                                    var doc1 = new NoteViewModel
//                                    {
//                                        Id = check.Id.Value,
//                                        Operation = DataOperation.Correct,
//                                    };
//                                    _noteBusiness.Delete(doc1);
//                                }
//                                var note = _noteBusiness.CopyDocument(doc.Id, parentId, false, workspace.Id, workspace.Id, workspace.OwnerUserId);
//                                //_repoNote.CreateOneToOneRelationshipToReferenceType(note.Id, new R_Note_Reference(), NodeEnum.NTS_Service, viewModel.ServiceId.Value);
//                            }
//                        }

//                    }

//                }

//            }
//            return null;
//        }
//        public ActionResult MoveVendorDocument(NoteViewModel doc)
//        {
//            var desciplin = doc.Controls.FirstOrDefault(x => x.FieldName == "discipline");
//            var revision = doc.Controls.FirstOrDefault(x => x.FieldName == "revision");
//            var vendor = doc.Controls.FirstOrDefault(x => x.FieldName == "vendorList");
//            var code = doc.Controls.FirstOrDefault(x => x.FieldName == "code");
//            var workspace = _noteBusiness.GetWorkspaceDataForAdmin().Where(x => x.Name.ToLower().Contains("vendor documents")).FirstOrDefault();

//            if (workspace != null)
//            {
//                var vendorFolder = _noteBusiness.GetAllChildForWorkspace(workspace.Id).Where(x => x.Subject != null && x.Subject.ToLower().Contains(vendor.Value.ToLower()) && x.Subject.Contains("Vendor Document")).FirstOrDefault();
//                long venFolderId = 0;
//                if (vendorFolder != null)
//                {
//                    venFolderId = vendorFolder.Id;
//                }
//                else
//                {
//                    var foldermodel = new NoteViewModel
//                    {
//                        TemplateMasterCode = "GENERAL_FOLDER",
//                        Operation = DataOperation.Create,
//                        OwnerUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
//                        ActiveUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
//                        RequestedByUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
//                        TemplateAction = NtsActionEnum.Submit,
//                        ReferenceType = NoteReferenceTypeEnum.Self,
//                        ParentId = workspace.Id,
//                        StartDate = DateTime.Now.ApplicationNow().Date,
//                    };

//                    var newmodel1 = _noteBusiness.GetDetails(foldermodel);
//                    newmodel1.Subject = vendor.Value + " " + "Vendor Document";
//                    newmodel1.TemplateAction = NtsActionEnum.Submit;
//                    _noteBusiness.Manage(newmodel1);
//                    venFolderId = newmodel1.Id;
//                }
//                var desiplin = _noteBusiness.GetAllChildForWorkspace(venFolderId).Where(x => x.Subject != null && x.Subject.ToLower().Contains(desciplin.Value.ToLower())).FirstOrDefault();
//                long desiplinFolderId = 0;
//                if (desiplin != null)
//                {
//                    desiplinFolderId = desiplin.Id;

//                }
//                else
//                {
//                    var foldermodel = new NoteViewModel
//                    {
//                        TemplateMasterCode = "GENERAL_FOLDER",
//                        Operation = DataOperation.Create,
//                        OwnerUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
//                        ActiveUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
//                        RequestedByUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
//                        TemplateAction = NtsActionEnum.Submit,
//                        ReferenceType = NoteReferenceTypeEnum.Self,
//                        ParentId = venFolderId,
//                        StartDate = DateTime.Now.ApplicationNow().Date,
//                    };

//                    var newmodel1 = _noteBusiness.GetDetails(foldermodel);
//                    newmodel1.Subject = desciplin.Value;
//                    newmodel1.TemplateAction = NtsActionEnum.Submit;
//                    _noteBusiness.Manage(newmodel1);
//                    desiplinFolderId = newmodel1.Id;
//                }
//                var rev = _noteBusiness.GetAllChildForWorkspace(desiplinFolderId).Where(x => x.Subject != null && x.Subject.ToLower().Contains(revision.Value.ToLower())).FirstOrDefault();
//                long revParentId = 0;
//                if (rev != null)
//                {
//                    revParentId = rev.Id;
//                }
//                else
//                {
//                    var foldermodel = new NoteViewModel
//                    {
//                        TemplateMasterCode = "GENERAL_FOLDER",
//                        Operation = DataOperation.Create,
//                        OwnerUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
//                        ActiveUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
//                        RequestedByUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
//                        TemplateAction = NtsActionEnum.Submit,
//                        ReferenceType = NoteReferenceTypeEnum.Self,
//                        ParentId = desiplinFolderId,
//                        StartDate = DateTime.Now.ApplicationNow().Date,
//                    };

//                    var newmodel1 = _noteBusiness.GetDetails(foldermodel);
//                    newmodel1.Subject = revision.Value;
//                    newmodel1.TemplateAction = NtsActionEnum.Submit;
//                    _noteBusiness.Manage(newmodel1);
//                    revParentId = newmodel1.Id;
//                }
//                var revnote = _noteBusiness.CopyDocument(doc.Id, revParentId, false, workspace.Id, workspace.Id, workspace.OwnerUserId);
//                //_repoNote.CreateOneToOneRelationshipToReferenceType(revnote.Id, new R_Note_Reference(), NodeEnum.NTS_Service, viewModel.ServiceId.Value);


//                if (revision.Value.Contains("Rev"))
//                {
//                    var latestrevfolder = _noteBusiness.GetAllChildForWorkspace(desiplinFolderId).Where(x => x.Subject.ToLower().Contains("latest")).FirstOrDefault();
//                    if (latestrevfolder != null)
//                    {

//                        var revDoc = _noteBusiness.CheckDocumentNoExist(latestrevfolder.Id);
//                        if (revDoc != null)
//                        {
//                            var check = revDoc.Where(x => x.Name == doc.NoteNo).FirstOrDefault();

//                            if (check != null)
//                            {
//                                var doc1 = new NoteViewModel
//                                {
//                                    Id = check.Id.Value,
//                                    Operation = DataOperation.Correct,
//                                };
//                                _noteBusiness.Delete(doc1);
//                            }
//                            var note = _noteBusiness.CopyDocument(doc.Id, latestrevfolder.Id, false, workspace.Id, workspace.Id, workspace.OwnerUserId);
//                            //_repoNote.CreateOneToOneRelationshipToReferenceType(note.Id, new R_Note_Reference(), NodeEnum.NTS_Service, viewModel.ServiceId.Value);
//                        }

//                    }
//                    else
//                    {
//                        var foldermodel = new NoteViewModel
//                        {
//                            TemplateMasterCode = "GENERAL_FOLDER",
//                            Operation = DataOperation.Create,
//                            OwnerUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
//                            ActiveUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
//                            RequestedByUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
//                            TemplateAction = NtsActionEnum.Submit,
//                            ReferenceType = NoteReferenceTypeEnum.Self,
//                            ParentId = desiplinFolderId,
//                            StartDate = DateTime.Now.ApplicationNow().Date,
//                        };

//                        var newmodel1 = _noteBusiness.GetDetails(foldermodel);
//                        newmodel1.Subject = "Latest revision Files";
//                        newmodel1.TemplateAction = NtsActionEnum.Submit;
//                        _noteBusiness.Manage(newmodel1);
//                        var note = _noteBusiness.CopyDocument(doc.Id, newmodel1.Id, false, workspace.Id, workspace.Id, workspace.OwnerUserId);
//                        //_repoNote.CreateOneToOneRelationshipToReferenceType(note.Id, new R_Note_Reference(), NodeEnum.NTS_Service, viewModel.ServiceId.Value);
//                    }

//                }
//                if (code.Value.Contains("AFC"))
//                {
//                    long parentId = 0;
//                    var folder = _noteBusiness.GetAllChildForWorkspace(desiplinFolderId).Where(x => x.Subject != null && x.Subject.ToLower().Contains("signed")).FirstOrDefault();
//                    if (folder != null)
//                    {
//                        parentId = folder.Id;
//                    }
//                    else
//                    {
//                        var foldermodel = new NoteViewModel
//                        {
//                            TemplateMasterCode = "GENERAL_FOLDER",
//                            Operation = DataOperation.Create,
//                            OwnerUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
//                            ActiveUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
//                            RequestedByUserId = workspace.OwnerUserId,//viewModel.OwnerUserId,
//                            TemplateAction = NtsActionEnum.Submit,
//                            ReferenceType = NoteReferenceTypeEnum.Self,
//                            ParentId = desiplinFolderId,
//                            StartDate = DateTime.Now.ApplicationNow().Date,
//                        };

//                        var newmodel = _noteBusiness.GetDetails(foldermodel);
//                        newmodel.Subject = "Signed AFC";
//                        newmodel.TemplateAction = NtsActionEnum.Submit;
//                        newmodel.ParentId = desiplinFolderId;
//                        _noteBusiness.Manage(newmodel);
//                        parentId = newmodel.Id;
//                    }

//                    // doc.Id = 0;
//                    //doc.Operation = DataOperation.Create;
//                    //doc.ParentId = parentId;
//                    //_noteBussiness.Manage(doc);
//                    if (parentId != 0)
//                    {
//                        var revDoc = _noteBusiness.CheckDocumentNoExist(parentId);
//                        if (revDoc != null)
//                        {
//                            var check = revDoc.Where(x => x.Name == doc.NoteNo).FirstOrDefault();

//                            if (check != null)
//                            {
//                                var doc1 = new NoteViewModel
//                                {
//                                    Id = check.Id.Value,
//                                    Operation = DataOperation.Correct,
//                                };
//                                _noteBusiness.Delete(doc1);
//                            }
//                            var note = _noteBusiness.CopyDocument(doc.Id, parentId, false, workspace.Id, workspace.Id, workspace.OwnerUserId);
//                            //_repoNote.CreateOneToOneRelationshipToReferenceType(note.Id, new R_Note_Reference(), NodeEnum.NTS_Service, viewModel.ServiceId.Value);
//                        }
//                    }


//                }
//            }
//            return null;
//        }       
        
//        public bool GetValidateEnumValue(string enumName,string code)
//        {

//            if (enumName.Contains("DocumentApprovalStatuTypeEnum"))
//            {
//                if (!Enum.IsDefined(typeof(DocumentApprovalStatuTypeEnum), code))               
//                    return false;
                
                   
//            }
//            else
//            {
//                if (!Enum.IsDefined(typeof(StageStatuTypeEnum), code))               
//                    return false;
                               
//            }
//            return true;
//        }
//        [HttpPost]
//        public ActionResult LoadGalfarEngineeringDocuments2(string data, long templateMasterId, string templateMasterName, long workspaceId, long folderId, List<string> errorMessage=null)
//        {
//            var errorList = new List<string>();
//            var udflist = _noteBusiness.DynamicUdfColumns(templateMasterId);
//            JObject jObject = JObject.Parse(data);
//            string sheet1 = (string)jObject.SelectToken("activeSheet");
//            JArray sheets = (JArray)jObject.SelectToken("sheets");
//            foreach (JToken sheet in sheets)
//            {
//                string sheetname = (string)sheet.SelectToken("name");
//                if (sheetname == sheet1)
//                {
//                    var list = new List<long>();
//                    int z = 1;
//                    JArray rows = (JArray)sheet.SelectToken("rows");
//                    foreach (JToken row in rows)
//                    {

//                        var noteno = GetCellValues(row, 0);
//                        if (noteno != null)
//                        {
//                            if (noteno.ToLower().Contains("document") || noteno.ToLower().Contains("==="))
//                            {
//                                z++;
//                                continue;
//                            }
//                            //string pattern = "\\b" + noteno + "\\b";
//                            string pattern = "" + noteno + "";
//                            //Regex regReplace = new Regex(pattern);

//                            var subject = GetCellValues(row, 1);
//                            var revision = GetCellValuesByString(row, "rev-");
//                            var document1 = _noteBusiness.GetDocumentByNoteAndRevision(templateMasterId, noteno, revision).FirstOrDefault();
//                            if (!document1.IsNotNull())
//                            {
//                                //var workspace = _noteBusiness.GetWorkspaceDataForAdmin().Where(x => x.Name.ToLower().Contains("technip workspace")).FirstOrDefault();

//                                try
//                                {
//                                    if (workspaceId > 0)
//                                    {
//                                        var doc = _noteBusiness.GetDetails(new NoteViewModel
//                                        {
//                                            Id = 0,
//                                            TemplateId = 0,
//                                            TemplateMasterId = templateMasterId,
//                                            ActiveUserId = LoggedInUserId,
//                                            RequestedByUserId = LoggedInUserId,
//                                            OwnerUserId = LoggedInUserId,
//                                            Operation = DataOperation.Read,
//                                            ReferenceType = NoteReferenceTypeEnum.Self,
//                                            WorkspaceId = workspaceId,//workspace.Id,

//                                        });
//                                        var approveType = "";
//                                        //var isLatest = fields[26];
//                                        //var genFile = fields[27];
//                                        //if (genFile.IsNotNullAndNotEmpty())
//                                        //{
//                                        //    var genhead = _fileBusiness.GetList(x => x.FileName == genFile).OrderByDescending(x=>x.Id).FirstOrDefault();
//                                        //    if (genhead != null)
//                                        //        doc.CSVFileIds = genhead.Id + "";
//                                        //}                                    
//                                        foreach (var item in doc.Controls.Where(x => x.FieldPartialViewName != "NTS_Group"))
//                                        {
//                                            for (int j = 0; j <= udflist.Count - 1; j++)
//                                            {
//                                                int k = j + 2;
//                                                var field = GetCellValues(row, k);
//                                                if (item.FieldName == udflist[j].FieldName)
//                                                {
//                                                    if (item.FieldPartialViewName == "NTS_Attachment")
//                                                    {
//                                                        if (field.IsNotNullAndNotEmpty())
//                                                        {
//                                                            var file = _fileBusiness.GetFileByName(field);
//                                                            if (!file.IsNotNull())
//                                                            {
//                                                                errorList.Add(string.Concat("At row " + z + " Document No:" + noteno + " ,File does not exist :", udflist[j].LabelDisplayName));

//                                                            }
//                                                        }
//                                                        break;
//                                                    }
//                                                    else if (item.FieldPartialViewName == "NTS_DropDownList" && item.DataSourceControllerName == "ListOfValue")
//                                                    {
//                                                        if (field.IsNotNullAndNotEmpty())
//                                                        {
//                                                            var parentValue = udflist[j].DataSourceHtmlAttributesString;
//                                                            string substr = parentValue.Substring(parentValue.IndexOf(":") + 2);
//                                                            string substr1 = substr.Substring(0, substr.IndexOf("\"}"));
//                                                            var lov = _lovBusiness.GetListOfValueByParentAndVlaue(substr1, field);
//                                                            if (lov == null)
//                                                            {
//                                                                errorList.Add(string.Concat("At row " + z + " Document No:" + noteno + " ,value does not exist :", udflist[j].LabelDisplayName));

//                                                            }
//                                                        }
//                                                        break;
//                                                    }
//                                                    else if (item.FieldPartialViewName == "NTS_DropDownList" && item.DataSourceActionName == "GetEnumIdNameList")
//                                                    {
//                                                        if (field.IsNotNullAndNotEmpty())
//                                                        {
//                                                            var parentValue = udflist[j].DataSourceHtmlAttributesString;
//                                                            string substr = parentValue.Substring(parentValue.IndexOf(":") + 2);
//                                                            string substr1 = substr.Substring(0, substr.IndexOf("\"}"));
//                                                            var code = field.Replace(" ", string.Empty);
//                                                            if (!GetValidateEnumValue(substr1, code))
//                                                            {
//                                                                errorList.Add(string.Concat("At row " + z + " Document No:" + noteno + " ,value does not exist :", udflist[j].LabelDisplayName));
//                                                            }
//                                                        }
//                                                        break;
//                                                    }
//                                                    else if (item.FieldPartialViewName == "NTS_DatePicker")
//                                                    {
//                                                        if (field.IsNotNullAndNotEmpty())
//                                                        {
//                                                            try
//                                                            {
//                                                                double d = double.Parse(field);
//                                                                DateTime conv = DateTime.FromOADate(d);
//                                                            }
//                                                            catch (Exception)
//                                                            {
//                                                                errorList.Add(string.Concat("At row " + z + " Document No:" + noteno + " ,Incorrect Date Format :", udflist[j].LabelDisplayName));
//                                                            }

//                                                        }
//                                                        break;
//                                                    }                                                }
//                                            }
//                                        }
//                                    }
//                                }
//                                catch (Exception ex)
//                                {
//                                    //return Json(new { success = false, errors = ex.ToString() });
//                                    errorList.Add(string.Concat("At row " + z + " Document No:" + noteno + " ,Upload fialed :", noteno));
//                                }
//                            }
//                            else
//                            {
//                                try
//                                {
//                                    var doc = _noteBusiness.GetDetails(new NoteViewModel
//                                    {
//                                        Id = document1.Id,
//                                        Operation = DataOperation.Correct,
//                                        ReferenceType = NoteReferenceTypeEnum.Self,

//                                    });
//                                    //var isLatest = fields[26];
//                                    //var genFile = fields[27];
//                                    //if (genFile.IsNotNullAndNotEmpty())
//                                    //{
//                                    //    var genhead = _fileBusiness.GetList(x=> x.FileName == genFile).OrderByDescending(x => x.Id).FirstOrDefault();
//                                    //    if (genhead != null)
//                                    //        doc.CSVFileIds = genhead.Id + "";
//                                    //}
//                                    foreach (var item in doc.Controls.Where(x => x.FieldPartialViewName != "NTS_Group"))
//                                    {
//                                        for (int j = 0; j <= udflist.Count - 1; j++)
//                                        {
//                                            int k = j + 2;
//                                            var field = GetCellValues(row, k);
//                                            if (item.FieldName == udflist[j].FieldName)
//                                            {
//                                                if (item.FieldPartialViewName == "NTS_Attachment")
//                                                {
//                                                    if (field.IsNotNullAndNotEmpty())
//                                                    {
//                                                        var file = _fileBusiness.GetFileByName(field);
//                                                        if (!file.IsNotNull())
//                                                        {
//                                                            errorList.Add(string.Concat("At row " + z + " Document No:" + noteno + " ,File does not exist :", udflist[j].LabelDisplayName));

//                                                        }
//                                                    }
//                                                    break;
//                                                }
//                                                else if (item.FieldPartialViewName == "NTS_DropDownList" && item.DataSourceControllerName == "ListOfValue")
//                                                {
//                                                    if (field.IsNotNullAndNotEmpty())
//                                                    {
//                                                        var parentValue = udflist[j].DataSourceHtmlAttributesString;
//                                                        string substr = parentValue.Substring(parentValue.IndexOf(":") + 2);
//                                                        string substr1 = substr.Substring(0, substr.IndexOf("\"}"));
//                                                        var lov = _lovBusiness.GetListOfValueByParentAndVlaue(substr1, field);
//                                                        if (lov == null)
//                                                        {
//                                                            errorList.Add(string.Concat("At row " + z + " Document No:" + noteno + " ,value does not exist :", udflist[j].LabelDisplayName));
//                                                        }
//                                                    }
//                                                    break;
//                                                }
//                                                else if (item.FieldPartialViewName == "NTS_DropDownList" && item.DataSourceActionName == "GetEnumIdNameList")
//                                                {
//                                                    if (field.IsNotNullAndNotEmpty())
//                                                    {
//                                                        var parentValue = udflist[j].DataSourceHtmlAttributesString;
//                                                        string substr = parentValue.Substring(parentValue.IndexOf(":") + 2);
//                                                        string substr1 = substr.Substring(0, substr.IndexOf("\"}"));
//                                                        var code = field.Replace(" ", string.Empty);
//                                                        if (!GetValidateEnumValue(substr1, code))
//                                                        {
//                                                            errorList.Add(string.Concat("At row " + z + " Document No:" + noteno + " ,value does not exist :", udflist[j].LabelDisplayName));
//                                                        }
//                                                    }
//                                                    break;
//                                                }
//                                                else if (item.FieldPartialViewName == "NTS_DatePicker")
//                                                {
//                                                    if (field.IsNotNullAndNotEmpty())
//                                                    {
//                                                        try
//                                                        {
//                                                            double d = double.Parse(field);
//                                                            DateTime conv = DateTime.FromOADate(d);
//                                                        }
//                                                        catch (Exception)
//                                                        {
//                                                            errorList.Add(string.Concat("At row " + z + " Document No:" + noteno + " ,Incorrect Date Format :", udflist[j].LabelDisplayName));
//                                                        }
//                                                    }
//                                                    break;
//                                                }
//                                            }
//                                        }
//                                    }
//                                }
//                                catch (Exception ex)
//                                {

//                                    //return Json(new { success = false, errors = ex.ToString() });
//                                    errorList.Add(string.Concat("At row " + z + " Document No:" + noteno + " ,Upload fialed :", noteno));
//                                }


//                            }
//                            //z++;
//                            if (errorList.Count > 0)
//                            {
//                                var replace2 = noteno + "===Error";
//                                data = data.Replace(pattern, replace2);                                
//                                if (errorMessage.IsNotNull())
//                                    errorList.AddRange(errorMessage);
//                                var j = Json(new { success = false, errors = errorList, excelData = data });
//                                j.MaxJsonLength = int.MaxValue;
//                                return j;
//                                //return Json(new { success = false, errors = errorList, excelData = data });
//                            }
//                            var existDoc = _noteBusiness.GetDocumentByNoteAndRevision(templateMasterId, noteno, revision).ToList();
//                            if (existDoc.Count == 0)
//                            {
//                                //var workspace = _noteBusiness.GetWorkspaceDataForAdmin().Where(x => x.Name.ToLower().Contains("technip workspace")).FirstOrDefault();

//                                try
//                                {
//                                    if (workspaceId > 0)
//                                    {
//                                        var doc = _noteBusiness.GetDetails(new NoteViewModel
//                                        {
//                                            Id = 0,
//                                            TemplateId = 0,
//                                            TemplateMasterId = templateMasterId,
//                                            ActiveUserId = LoggedInUserId,
//                                            RequestedByUserId = LoggedInUserId,
//                                            OwnerUserId = LoggedInUserId,
//                                            Operation = DataOperation.Create,
//                                            ReferenceType = NoteReferenceTypeEnum.Self,
//                                            WorkspaceId = workspaceId,//workspace.Id,

//                                        });
//                                        doc.TemplateAction = NtsActionEnum.Submit;
//                                        doc.Operation = DataOperation.Create;
//                                        doc.NoteNo = noteno;
//                                        doc.Subject = subject;
//                                        var approveType = "";
//                                        //var isLatest = fields[26];
//                                        //var genFile = fields[27];
//                                        //if (genFile.IsNotNullAndNotEmpty())
//                                        //{
//                                        //    var genhead = _fileBusiness.GetList(x => x.FileName == genFile).OrderByDescending(x=>x.Id).FirstOrDefault();
//                                        //    if (genhead != null)
//                                        //        doc.CSVFileIds = genhead.Id + "";
//                                        //}                                    
//                                        foreach (var item in doc.Controls.Where(x => x.FieldPartialViewName != "NTS_Group"))
//                                        {
//                                            for (int j = 0; j <= udflist.Count - 1; j++)
//                                            {
//                                                int k = j + 2;
//                                                var field = GetCellValues(row, k);
//                                                if (item.FieldName == udflist[j].FieldName)
//                                                {
//                                                    if (item.FieldPartialViewName == "NTS_Attachment")
//                                                    {
//                                                        if (field.IsNotNullAndNotEmpty())
//                                                        {
//                                                            var file = _fileBusiness.GetFileByName(field);
//                                                            if (file.IsNotNull())
//                                                            {
//                                                                item.Code = file.Id.ToString();
//                                                                item.Value = file.FileName;
//                                                            }

//                                                        }
//                                                        break;
//                                                    }
//                                                    else if (item.FieldPartialViewName == "NTS_DropDownList" && item.DataSourceControllerName == "ListOfValue")
//                                                    {
//                                                        if (field.IsNotNullAndNotEmpty())
//                                                        {
//                                                            var parentValue = udflist[j].DataSourceHtmlAttributesString;
//                                                            string substr = parentValue.Substring(parentValue.IndexOf(":") + 2);
//                                                            string substr1 = substr.Substring(0, substr.IndexOf("\"}"));
//                                                            var lov = _lovBusiness.GetListOfValueByParentAndVlaue(substr1, field);
//                                                            if (lov != null)
//                                                            {
//                                                                item.Code = lov.Code;
//                                                                item.Value = lov.Name;
//                                                            }
//                                                        }
//                                                        break;
//                                                    }
//                                                    else if (item.FieldPartialViewName == "NTS_DropDownList" && item.DataSourceActionName == "GetEnumIdNameList")
//                                                    {
//                                                        if (field.IsNotNullAndNotEmpty())
//                                                        {
//                                                            var code = field.Replace(" ", string.Empty);
//                                                            if (code != null)
//                                                            {
//                                                                item.Code = code;
//                                                                item.Value = field;
//                                                            }
//                                                            if (item.FieldName.Contains("documentApprovalStatusType"))
//                                                            {

//                                                                if (!field.ToLower().Contains("manual") && !field.ToLower().Contains("not"))
//                                                                {
//                                                                    var item1 = doc.Controls.FirstOrDefault(x => x.FieldName == "documentApprovalStatus");
//                                                                    if (item1.IsNotNull())
//                                                                    {
//                                                                        approveType = DocumentApprovalStatusEnum.ApprovalInProgress.ToString();
//                                                                        item1.Code = DocumentApprovalStatusEnum.ApprovalInProgress.ToString();
//                                                                        item1.Value = DocumentApprovalStatusEnum.ApprovalInProgress.Description().ToString();
//                                                                    }

//                                                                }
//                                                                else
//                                                                {
//                                                                    var item1 = doc.Controls.FirstOrDefault(x => x.FieldName == "documentApprovalStatus");
//                                                                    if (item1.IsNotNull())
//                                                                    {
//                                                                        item1.Code = DocumentApprovalStatusEnum.ApprovedManually.ToString();
//                                                                        item1.Value = DocumentApprovalStatusEnum.ApprovedManually.Description().ToString();
//                                                                    }
//                                                                }

//                                                            }

//                                                        }
//                                                        break;
//                                                    }
//                                                    else if (item.FieldPartialViewName == "NTS_DatePicker")
//                                                    {
//                                                        if (field.IsNotNullAndNotEmpty())
//                                                        {
//                                                            double d = double.Parse(field);
//                                                            DateTime conv = DateTime.FromOADate(d);
//                                                            item.Code = conv.ToString();
//                                                            item.Value = conv.ToDefaultDateFormat();
//                                                        }
//                                                        break;
//                                                    }
//                                                    else
//                                                    {
//                                                        item.Code = field;
//                                                        item.Value = field;
//                                                        break;
//                                                    }
//                                                }
//                                            }

//                                            if (item.FieldName == "workspaceId")
//                                            {
//                                                item.Code = workspaceId.ToString();
//                                                item.Value = workspaceId.ToString();
//                                            }
//                                        }
//                                        //var folder = _noteBusiness.GetAllChildForWorkspace(workspace.Id).Where(x => x.Subject != null && x.Subject.ToLower().Contains("incoming document")).FirstOrDefault();


//                                        //long folderId = 0;
//                                        //if (folder != null)
//                                        //{
//                                        //    folderId = folder.Id;
//                                        //}
//                                        //else
//                                        //{
//                                        //    var foldermodel = new NoteViewModel
//                                        //    {
//                                        //        TemplateMasterCode = "GENERAL_FOLDER",
//                                        //        Operation = DataOperation.Create,
//                                        //        OwnerUserId = workspace.OwnerUserId,
//                                        //        ActiveUserId = workspace.OwnerUserId,
//                                        //        RequestedByUserId = workspace.OwnerUserId,
//                                        //        TemplateAction = NtsActionEnum.Submit,
//                                        //        ReferenceType = NoteReferenceTypeEnum.Self,
//                                        //        ParentId = workspace.Id,
//                                        //        StartDate = DateTime.Now.ApplicationNow().Date,
//                                        //        WorkspaceId = workspace.Id,
//                                        //    };

//                                        //    var newmodel1 = _noteBusiness.GetDetails(foldermodel);
//                                        //    newmodel1.Subject = "Incoming Document";
//                                        //    newmodel1.TemplateAction = NtsActionEnum.Submit;
//                                        //    _noteBusiness.Manage(newmodel1);
//                                        //    folderId = newmodel1.Id;
//                                        //}
//                                        doc.ParentId = folderId;
//                                        var result = _noteBusiness.Manage(doc);
//                                        if (result.IsSuccess)
//                                        {
//                                            if (approveType == DocumentApprovalStatusEnum.ApprovalInProgress.ToString())
//                                            {
//                                                var workflowId = _noteBusiness.GetServiceWorkflowTemplateId(templateMasterId);
//                                                if (workflowId.IsNotNull() && workflowId != 0)
//                                                {
//                                                    var service = _servicebusiness.GetServiceDetails(new ServiceViewModel
//                                                    {
//                                                        Id = 0,
//                                                        TemplateId = 0,
//                                                        TemplateMasterId = workflowId,
//                                                        ActiveUserId = LoggedInUserId,
//                                                        RequestedByUserId = LoggedInUserId,
//                                                        OwnerUserId = LoggedInUserId,
//                                                        Operation = DataOperation.Create

//                                                    });
//                                                    service.Operation = DataOperation.Create;
//                                                    service.TemplateAction = NtsActionEnum.Submit;
//                                                    var stepTasks = _taskBusiness.GetServiceStepsForAdd(service);
//                                                    stepTasks.ForEach(x => x.SLA = null);
//                                                    service.ServiceTasksList = stepTasks;
//                                                    service.ServiceTasks = new JavaScriptSerializer().Serialize(stepTasks);
//                                                    var regularDocument1 = service.Controls.FirstOrDefault(x => x.FieldName == "RegularDocument1");
//                                                    if (regularDocument1.IsNotNull())
//                                                    {
//                                                        regularDocument1.Code = result.Item.Id.ToString();
//                                                        regularDocument1.Value = result.Item.Subject;
//                                                    }
//                                                    _servicebusiness.Manage(service);
//                                                }

//                                            }
//                                            else
//                                            {
//                                                if (templateMasterName == "Engineering Document")
//                                                {
//                                                    MoveEngineeringDocument(result.Item);
//                                                }
//                                                else if (templateMasterName == "Project Documents")
//                                                {
//                                                    MoveProjectDocument(result.Item);
//                                                }
//                                                else if (templateMasterName == "Vendor Documents")
//                                                {
//                                                    MoveVendorDocument(result.Item);
//                                                }
//                                            }
//                                            var replace1 = noteno + "===Completed";
//                                            data = data.Replace(pattern, replace1);
//                                            //data = regReplace.Replace(data, replace1, 1);
//                                            //data = data.Replace(noteno, noteno + "===Completed");
//                                            if (errorMessage.IsNotNull())
//                                                errorList.AddRange(errorMessage);
//                                            var j = Json(new { success = true, errors = errorList, excelData = data });
//                                            j.MaxJsonLength = int.MaxValue;
//                                            return j;
//                                        }
//                                        else
//                                        {
//                                            var replace2 = noteno + "===Error";
//                                            data = data.Replace(pattern, replace2);
//                                            //data = regReplace.Replace(data, replace2, 1);
//                                            //data =data.Replace(noteno, noteno + " error=" + result.MessageString);
//                                            //errorList.Add(result.MessageString);
//                                            if (errorMessage.IsNotNull())
//                                                errorList.AddRange(errorMessage);
//                                            var j = Json(new { success = false, errors = errorList, excelData = data });
//                                            j.MaxJsonLength = int.MaxValue;
//                                            return j;
//                                        }
//                                    }

//                                }
//                                catch (Exception ex)
//                                {
//                                    var replace2 = noteno + "===Error";
//                                    // return Json(new { success = false, errors = ex.ToString() });
//                                    data = data.Replace(pattern, replace2);
//                                    //data = regReplace.Replace(data, replace2, 1);
//                                    //data = data.Replace(noteno, noteno + " error=" + ex.ToString());
//                                    //errorList.Add(result.MessageString);
//                                    if (errorMessage.IsNotNull())
//                                        errorList.AddRange(errorMessage);
//                                    var j = Json(new { success = false, errors = errorList, excelData = data });
//                                    j.MaxJsonLength = int.MaxValue;
//                                    return j;
//                                }
//                            }
//                            else
//                            {
//                                try
//                                {
//                                    bool success = false;
//                                    foreach (var document in existDoc)
//                                    {
//                                        var doc = _noteBusiness.GetDetails(new NoteViewModel
//                                        {
//                                            Id = document.Id,
//                                            Operation = DataOperation.Correct,
//                                            ReferenceType = NoteReferenceTypeEnum.Self,

//                                        });
//                                        doc.TemplateAction = NtsActionEnum.EditAsNewVersion;
//                                        doc.Operation = DataOperation.Correct;
//                                        doc.NoteNo = noteno;
//                                        doc.Subject = subject;
//                                        //var isLatest = fields[26];
//                                        //var genFile = fields[27];
//                                        //if (genFile.IsNotNullAndNotEmpty())
//                                        //{
//                                        //    var genhead = _fileBusiness.GetList(x=> x.FileName == genFile).OrderByDescending(x => x.Id).FirstOrDefault();
//                                        //    if (genhead != null)
//                                        //        doc.CSVFileIds = genhead.Id + "";
//                                        //}
//                                        foreach (var item in doc.Controls.Where(x => x.FieldPartialViewName != "NTS_Group"))
//                                        {
//                                            for (int j = 0; j <= udflist.Count - 1; j++)
//                                            {
//                                                int k = j + 2;
//                                                var field = GetCellValues(row, k);
//                                                if (item.FieldName == udflist[j].FieldName)
//                                                {
//                                                    if (item.FieldPartialViewName == "NTS_Attachment")
//                                                    {
//                                                        if (field.IsNotNullAndNotEmpty())
//                                                        {
//                                                            var file = _fileBusiness.GetFileByName(field);
//                                                            if (file.IsNotNull())
//                                                            {
//                                                                item.Code = file.Id.ToString();
//                                                                item.Value = file.FileName;
//                                                            }
//                                                        }
//                                                        break;
//                                                    }
//                                                    else if (item.FieldPartialViewName == "NTS_DropDownList" && item.DataSourceControllerName == "ListOfValue")
//                                                    {
//                                                        if (field.IsNotNullAndNotEmpty())
//                                                        {
//                                                            var parentValue = udflist[j].DataSourceHtmlAttributesString;
//                                                            string substr = parentValue.Substring(parentValue.IndexOf(":") + 2);
//                                                            string substr1 = substr.Substring(0, substr.IndexOf("\"}"));
//                                                            var lov = _lovBusiness.GetListOfValueByParentAndVlaue(substr1, field);
//                                                            if (lov != null)
//                                                            {
//                                                                item.Code = lov.Code;
//                                                                item.Value = lov.Name;
//                                                            }
//                                                        }
//                                                        break;
//                                                    }
//                                                    else if (item.FieldPartialViewName == "NTS_DropDownList" && item.DataSourceActionName == "GetEnumIdNameList")
//                                                    {
//                                                        if (field.IsNotNullAndNotEmpty())
//                                                        {
//                                                            var code = field.Replace(" ", string.Empty);
//                                                            if (code != null)
//                                                            {
//                                                                item.Code = code;
//                                                                item.Value = field;
//                                                            }
//                                                        }
//                                                        break;
//                                                    }
//                                                    else if (item.FieldPartialViewName == "NTS_DatePicker")
//                                                    {
//                                                        if (field.IsNotNullAndNotEmpty())
//                                                        {
//                                                            double d = double.Parse(field);
//                                                            DateTime conv = DateTime.FromOADate(d);
//                                                            item.Code = conv.ToString();
//                                                            item.Value = conv.ToDefaultDateFormat();
//                                                        }
//                                                        break;
//                                                    }
//                                                    else
//                                                    {
//                                                        if (field.IsNotNullAndNotEmpty())
//                                                        {
//                                                            item.Code = field;
//                                                            item.Value = field;
//                                                        }
//                                                        break;
//                                                    }
//                                                }
//                                            }

//                                        }
//                                        var result = _noteBusiness.Manage(doc);
//                                        success = result.IsSuccess;


//                                    }
//                                    if (!success)
//                                    {
//                                        var replace2 = noteno + "===Error";
//                                        data = data.Replace(pattern, replace2);
//                                        //data = regReplace.Replace(data, replace2, 1);
//                                        //data = data.Replace(noteno, noteno + "===error" + result.MessageString);
//                                        //errorList.Add(result.MessageString);
//                                        if (errorMessage.IsNotNull())
//                                            errorList.AddRange(errorMessage);
//                                        var j = Json(new { success = false, errors = errorList, excelData = data });
//                                        j.MaxJsonLength = int.MaxValue;
//                                        return j;
//                                    }
//                                    else
//                                    {
//                                        var replace1 = noteno + "===Completed";
//                                        data = data.Replace(pattern, replace1);
//                                        //data = regReplace.Replace(data, replace1, 1);
//                                        //data = data.Replace(noteno, noteno + "===Completed");
//                                        if (errorMessage.IsNotNull())
//                                            errorList.AddRange(errorMessage);
//                                        var j = Json(new { success = true, errors = errorList, excelData = data });
//                                        j.MaxJsonLength = int.MaxValue;
//                                        return j;
//                                    }
//                                }
//                                catch (Exception ex)
//                                {
//                                    var replace2 = noteno + "===Error";
//                                    data = data.Replace(pattern, replace2);
//                                    //data = regReplace.Replace(data, replace2, 1);
//                                    //data = data.Replace(noteno, noteno + "===error" + ex.ToString());
//                                    //errorList.Add(result.MessageString);
//                                    if (errorMessage.IsNotNull())
//                                        errorList.AddRange(errorMessage);
//                                    var j = Json(new { success = false, errors = errorList, excelData = data });
//                                    j.MaxJsonLength = int.MaxValue;
//                                    return j;
//                                }


//                            }
//                        }
//                        else
//                        {
//                            break;
//                        }

//                    }
//                    break;
//                }

//            }
//            return Json(new { success = true });

//        }
//    }    
//}