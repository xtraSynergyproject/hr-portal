using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CMS.UI.ViewModel;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using ERP.Data.GraphModel;
using CMS.Data.Model;
using CMS.Business;
using AutoMapper;
using CMS.Common;
using System.Net.Http.Headers;

namespace CMS.Web.Scheduler
{
    public class DmsMigration
    {
        private List<ADM_User> SourceUserList;
        private List<User> TargetUserList;
        private string WebApiUrl = "http://178.238.236.213:3001/";
        //private string WebApiUrl = "http://95.111.235.64:91/";
        //private string WebApiUrl = "http://localhost:50276/";
        private IServiceProvider _serviceProvider;
        //private IUserBusiness _userBusiness;
        private IMapper _autoMapper;
        private List<ADM_Team> SourceTeamList;
        private List<TeamViewModel> TargetTeamList;
        private List<GEN_ListOfValue> SourceLOVList;
        private List<LOV> TargetLOVList;
        private List<ERP.UI.ViewModel.ListOfValueViewModel> SVMLOVList;
        private List<GEN_File> SourceFileList;
        private List<File> TargetFileList;
        private List<ERP.UI.ViewModel.FileViewModel> SVMFileList;
        private List<ADM_UserRole> SourceRoleList;
        private List<UserRoleViewModel> TargetRoleList;
        private List<ADM_WorkspacePermissionGroup> SourceUserGroupList;
        private List<UserGroup> TargetUserGroupList;
        private List<ERP.UI.ViewModel.NotePermissionViewModel> SourcePermissionList;
        private List<DocumentPermission> TargetPermissionList;
        private List<ERP.UI.ViewModel.NotePermissionViewModel> SVMPermissionList;
        private List<ERP.UI.ViewModel.NoteViewModel> noteList;
        private List<NoteTemplateViewModel> TargetFolderList;

        public DmsMigration(IMapper autoMapper, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            //_userBusiness = userBusiness;
            _autoMapper = autoMapper;

        }
        public async Task MigrateDMS()
        {
           //await MigrateUser();
           // await MigrateTeam();
            //await MigrateLOV();
            // await MigrateUserRole();
            // await MigrateUserGroup();
          // await MigratePermission();
            await MigrateVendor();

        }
        public async Task MigrateUser()
        {
            await ExtractUser();
            await TransformUser();
            await LoadUser();
        }
        public async Task MigrateTeam()
        {
            await ExtractTeam();
            await TransformTeam();
            await LoadTeam();
        }
        public async Task MigrateLOV()
        {
            await ExtractLOV();
            await TransformLOV();
            await LoadLOV();
        }
        public async Task MigrateFile()
        {
            await ExtractFile();
            await TransformFile();
            await LoadFile();
        }
        public async Task MigrateUserRole()
        {
            await ExtractUserRole();
            await TransformUserRole();
            await LoadUserRole();
        }
        public async Task MigrateUserGroup()
        {
            await ExtractUserGroup();
            await TransformUserGroup();
            await LoadUserGroup();
        }
        public async Task MigratePermission()
        {
            await ExtractPermission();
            await TransformPermission();
            await LoadPermission();
        }
        public async Task MigrateWorkspace()
        {
            await ExtractWorkSpace();
            await TransformWorkSpace();
            await LoadFolder();
        }
        public async Task MigrateFolder()
        {
            await ExtractFolder();
            await TransformFolder();
            await LoadFolder();
        }
        public async Task MigrateProject()
        {
            await ExtractProjectDocument();
            await TransformProjectDocument();
            await LoadFolder();
        }
        public async Task MigrateVendor()
        {
            await ExtractVendorDocument();
            await TransformVendorDocument();
            await LoadFolder();
        }
        public async Task MigrateEngineer()
        {
            await ExtractEngineerDocument();
            await TransformEngineerDocument();
            await LoadFolder();
        }
        public async Task MigrateRFI()
        {
            await ExtractRequestForInspectionDocument();
            await TransformRequestForInspectionDocument();
            await LoadFolder();
        }
        public async Task MigrateRFIHalul()
        {
            await ExtractRequestForInspectionHalulDocument();
            await TransformRequestForInspectionHalulDocument();
            await LoadFolder();
        }
        public async Task MigrateGeneral()
        {
            await ExtractGeneralDocument();
            await TransformGeneralDocument();
            await LoadFolder();
        }
        //public async Task Extract()
        //{
        //    await ExtractUser();
        //    await ExtractTeam();
        //    await ExtractLOV();
        //    await ExtractFile();
        //    await ExtractUserRole();
        //    await ExtractUserGroup();
        //    await ExtractPermission();
        //    await ExtractWorkSpace();
        //    await ExtractFolder();
        //    await ExtractGeneralDocument();
        //    await ExtractEngineerDocument();
        //    await ExtractProjectDocument();
        //    await ExtractRequestForInspectionDocument();
        //    await ExtractVendorDocument();

        //}
        //public async Task Transform()
        //{
        //    await TransformUser();
        //    await TransformTeam();
        //    await TransformLOV();
        //    await TransformFile();
        //    await TransformUserRole();
        //    await TransformUserGroup();
        //    await TransformPermission();
        //    await TransformWorkSpace();
        //    await TransformFolder();
        //    await TransformGeneralDocument();
        //    await TransformEngineerDocument();
        //    await TransformProjectDocument();
        //    await TransformRequestForInspectionDocument();
        //    await TransformVendorDocument();
        //}
        //public async Task Load()
        //{
        //    await LoadUser();
        //    await LoadTeam();
        //    await LoadLOV();
        //    await LoadFile();
        //    await LoadUserRole();
        //    await LoadUserGroup();
        //    await LoadPermission();
        //    await LoadFolder();
        //    //await LoadWorkSpace();

        //    //await LoadGeneralDocument();
        //    //await LoadEngineerDocument();
        //    //await LoadProjectDocument();
        //    //await LoadRequestForInspectionDocument();
        //    //await LoadVendorDocument();
        //}
        private async Task ExtractFolder()
        {
            // noteList = await GetApiListAsync<NoteViewModel>();
            var cypher = @"match(f:NTS_Note{IsDeleted:0})
                //where (f.IsArchived <>true or f.IsArchived is null)
                
                match(f)-[:R_Note_Template]->(t: NTS_Template{ IsDeleted: 0,Status: 'Active'})
                -[:R_TemplateRoot]->(tm: NTS_TemplateMaster{ IsDeleted: 0,Status: 'Active'})  
                -[:R_TemplateMaster_TemplateCategory]->(tc: NTS_TemplateCategory{IsDeleted:0,Code:'GENERAL_FOLDER'}) where tm.Code in ['GENERAL_FOLDER','PUBLIC_FOLDER','LEGALENTITY_FOLDER']
                match(f)-[:R_Note_Status_ListOfValue]->(lov: GEN_ListOfValue)                
                optional match(f)-[:R_Note_Owner_User]->(u: ADM_User) 
                optional match(f)-[:R_Note_Parent_Note]->(pn:NTS_Note{IsDeleted: 0,Status: 'Active'}) 
                optional match(f)-[:R_Note_Workspace_Note]->(ws:NTS_Note{IsDeleted: 0,Status: 'Active'})
                return distinct f,ws.Id as WorkspaceId,pn.Id as ParentId,u.Id as OwnerUserId,lov.Code as NoteStatusCode,tm.Code as TemplateMasterCode order by f.Id";
            noteList = await GetApiListCyherAsync<ERP.UI.ViewModel.NoteViewModel>(cypher);
        }
        private async Task TransformFolder()
        {
            var _noteBusiness = _serviceProvider.GetService<INoteBusiness>();
            var idss = await _noteBusiness.GetList(x => x.TemplateCode == "GENERAL_FOLDER" || x.TemplateCode == "PUBLIC_FOLDER" || x.TemplateCode == "LEGALENTITY_FOLDER");
            var ids = idss.Select(x => x.Id).ToList();
            noteList = noteList.Where(x => !ids.Contains(x.Id.ToString())).ToList();
            if (noteList.Count > 0)
            {
                TargetFolderList = new List<NoteTemplateViewModel>();
                foreach (var folder in noteList)
                {
                    var noteTemp = new NoteTemplateViewModel();
                    noteTemp.TemplateCode = folder.TemplateMasterCode;
                    var note = await _noteBusiness.GetNoteDetails(noteTemp);

                    note.NoteId = folder.Id.ToString();
                    note.NoteNo = folder.NoteNo;
                    note.NoteSubject = folder.Subject;
                    note.NoteDescription = folder.Description;
                    note.StartDate = folder.StartDate;
                    note.ExpiryDate = note.ExpiryDate;
                    if (folder.OwnerUserId.IsNotNull())
                    {
                        note.OwnerUserId = folder.OwnerUserId.ToString();
                    }
                    else
                    {
                        note.OwnerUserId = "1841";
                    }
                    note.RequestedByUserId = note.OwnerUserId;
                    note.Json = "{}";
                    note.DataAction = DataActionEnum.Create;
                    if (folder.ParentId.IsNotNull())
                    {
                        note.ParentNoteId = folder.ParentId.ToString();
                    }
                    note.IsDeleted = folder.IsDeleted == 0 ? false : true;
                    note.Status = folder.Status == ERP.Utility.StatusEnum.Active ? Common.StatusEnum.Active : Common.StatusEnum.Inactive;
                    note.CreatedDate = folder.CreatedDate;
                    note.CreatedBy = folder.CreatedBy.ToString();
                    note.LastUpdatedDate = folder.LastUpdatedDate;
                    note.LastUpdatedBy = folder.LastUpdatedBy.ToString();

                    dynamic exo = new System.Dynamic.ExpandoObject();

                    if (folder.WorkspaceId.IsNotNull())
                    {
                        var workspcaeId = await _noteBusiness.GetWorkspaceId(folder.WorkspaceId.ToString());
                        if (workspcaeId != null)
                        {
                            ((IDictionary<String, Object>)exo).Add("WorkspaceId", workspcaeId.Id);
                            note.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                        }
                    }

                    if (folder.NoteStatusCode == "ACTIVE")
                    {
                        note.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                    }
                    else if (folder.NoteStatusCode == "DRAFT")
                    {
                        note.NoteStatusCode = "NOTE_STATUS_DRAFT";
                    }
                    else if (folder.NoteStatusCode == "EXPIRED")
                    {
                        note.NoteStatusCode = "NOTE_STATUS_EXPIRE";
                    }

                    if (note.NoteStatusCode.IsNotNullAndNotEmpty())
                    {
                        TargetFolderList.Add(note);
                    }
                }
            }
        }
        private async Task LoadFolder()
        {
            var _noteBusiness = _serviceProvider.GetService<INoteBusiness>();

            if (TargetFolderList.Count > 0)
            {
                foreach (var note in TargetFolderList)
                {
                    try
                    {
                        var exist = await _noteBusiness.GetSingleById(note.NoteId);
                        if (exist == null)
                        {
                           var res = await _noteBusiness.ManageNote(note);
                        }
                        else
                        {
                           
                           // await _noteBusiness.EditNote1(exist);
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
        }
        private async Task ExtractWorkSpace()
        {
            // noteList = await GetApiListAsync<NoteViewModel>();
            var cypher = @"match(f:NTS_Note{IsDeleted:0})
                //where (f.IsArchived <>true or f.IsArchived is null)
                
                match(f)-[:R_Note_Template]->(t: NTS_Template{ IsDeleted: 0,Status: 'Active'})
                -[:R_TemplateRoot]->(tm: NTS_TemplateMaster{ IsDeleted: 0,Status: 'Active',Code:'WORKSPACE_GENERAL'})  
                -[:R_TemplateMaster_TemplateCategory]->(tc: NTS_TemplateCategory{IsDeleted:0,Code:'GENERAL_FOLDER'})
                optional match(f)-[:R_Note_Owner_User]->(u: ADM_User)
                optional match(f)-[:R_Note_Status_ListOfValue]->(lov: GEN_ListOfValue)
                optional match(f)-[:R_Note_Parent_Note]->(pn:NTS_Note{IsDeleted: 0,Status: 'Active'}) 
                optional match(f)<-[:R_User_Workspace_Note]-(uw: ADM_User)
                optional match(f)<-[:R_User_AdminWorkspace_Note]-(uaw: ADM_User)
                return distinct f,pn.Id as ParentId,u.Id as OwnerUserId,lov.Code as NoteStatusCode,uw.Id as MyWorkSpaceId,uaw.Id as AdminWorkSpaceId order by f.Id";
            noteList = await GetApiListCyherAsync<ERP.UI.ViewModel.NoteViewModel>(cypher);
        }
        private async Task TransformWorkSpace()
        {
            var _noteBusiness = _serviceProvider.GetService<INoteBusiness>();
            var _lovBusiness = _serviceProvider.GetService<ILOVBusiness>();
            var idss = await _noteBusiness.GetList(x => x.TemplateCode == "WORKSPACE_GENERAL");
            var ids = idss.Select(x => x.Id).ToList();
            noteList = noteList.Where(x => !ids.Contains(x.Id.ToString())).ToList();
            if (noteList.Count > 0)
            {
                TargetFolderList = new List<NoteTemplateViewModel>();
                foreach (var folder in noteList)
                {
                    var noteTemp = new NoteTemplateViewModel();
                    noteTemp.TemplateCode = "WORKSPACE_GENERAL";
                    var note = await _noteBusiness.GetNoteDetails(noteTemp);

                    note.NoteId = folder.Id.ToString();
                    note.NoteNo = folder.NoteNo;
                    note.NoteSubject = folder.Subject;
                    note.NoteDescription = folder.Description;
                    note.StartDate = folder.StartDate;
                    note.ExpiryDate = note.ExpiryDate;
                    if (folder.OwnerUserId.IsNotNull())
                    {
                        note.OwnerUserId = folder.OwnerUserId.ToString();
                    }
                    else
                    {
                        note.OwnerUserId = "1841";
                    }
                    note.RequestedByUserId = note.OwnerUserId;
                    note.Json = "{}";
                    note.DataAction = DataActionEnum.Create;
                    if (folder.ParentId.IsNotNull())
                    {
                        note.ParentNoteId = folder.ParentId.ToString();
                    }
                    note.IsDeleted = folder.IsDeleted == 0 ? false : true;
                    note.Status = folder.Status == ERP.Utility.StatusEnum.Active ? Common.StatusEnum.Active : Common.StatusEnum.Inactive;
                    note.CreatedDate = folder.CreatedDate;
                    note.CreatedBy = folder.CreatedBy.ToString();
                    note.LastUpdatedDate = folder.LastUpdatedDate;
                    note.LastUpdatedBy = folder.LastUpdatedBy.ToString();


                    dynamic exo = new System.Dynamic.ExpandoObject();

                    if (folder.MyWorkSpaceId != null)
                    {
                        var lov = await _lovBusiness.GetSingle(x => x.LOVType == "LOV_WORKSPACETYPE" && x.Code == "MY_WORKSPACE");
                        ((IDictionary<String, Object>)exo).Add("TypeId", lov.Id);

                        note.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                    }
                    else if (folder.AdminWorkSpaceId != null)
                    {
                        var lov = await _lovBusiness.GetSingle(x => x.LOVType == "LOV_WORKSPACETYPE" && x.Code == "ADMIN_WORKSPACE");
                        ((IDictionary<String, Object>)exo).Add("TypeId", lov.Id);

                        note.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                    }

                    if (folder.NoteStatusCode == "ACTIVE")
                    {
                        note.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                    }
                    else if (folder.NoteStatusCode == "DRAFT")
                    {
                        note.NoteStatusCode = "NOTE_STATUS_DRAFT";
                    }
                    else if (folder.NoteStatusCode == "EXPIRED")
                    {
                        note.NoteStatusCode = "NOTE_STATUS_EXPIRE";
                    }

                    if (note.NoteStatusCode.IsNotNullAndNotEmpty())
                    {
                        TargetFolderList.Add(note);
                    }
                }
            }
        }

        private async Task ExtractProjectDocument()
        {
            // noteList = await GetApiListAsync<NoteViewModel>();
            var cypher = @"match(f:NTS_Note{IsDeleted:0})
                where (f.IsArchived <>true or f.IsArchived is null)                
                match(f)-[:R_Note_Template]->(t: NTS_Template{ IsDeleted: 0,Status: 'Active'})
                -[:R_TemplateRoot]->(tm: NTS_TemplateMaster{ IsDeleted: 0,Status: 'Active',Code:'PROJECT_DOCUMENTS'})  
                -[:R_TemplateMaster_TemplateCategory]->(tc: NTS_TemplateCategory{IsDeleted:0,Code:'GENERAL_DOCUMENT'})                
                match(f)-[:R_Note_Status_ListOfValue]->(lov: GEN_ListOfValue)
                optional match(f)-[:R_Note_Owner_User]->(u: ADM_User)
                optional match (f)<-[:R_NoteFieldValue_Note]-(nfv: NTS_NoteFieldValue{ IsDeleted: 0})
                -[:R_NoteFieldValue_TemplateField]->(ttf:NTS_TemplateField)
                with u,f,t,tm,tc,lov,apoc.map.fromPairs(COLLECT([ttf.FieldName,nfv.Code])) as udf 
                optional match(f)-[:R_Note_Parent_Note]->(pn:NTS_Note{IsDeleted: 0,Status: 'Active'}) 
optional match(f)-[:R_Note_Workspace_Note]->(ws:NTS_Note{IsDeleted: 0,Status: 'Active'})
                return distinct f,pn.Id as ParentId,ws.Id as WorkspaceId,u.Id as OwnerUserId,udf.projectFolder as ProjectFolder,udf.projectSubFolder as ProjectSubFolder,udf.revision as Revision,udf.discipline as Discipline,
udf.attachment as Attachment,udf.galfarTransmittalNumber as GalfarTransmittalNumber,udf.galfarToQp as GalfarToQp,udf.outgoingIssueCodes as OutgoingIssueCodes,udf.dateOfSubmission as DateOfSubmission,
udf.qpDueDate as QpDueDate,udf.technipAttachment1 as TechnipAttachment1,udf.qPTransmittalNumber as QPTransmittalNumber,udf.QpToGalfar as QpToGalfar,udf.code as Code,udf.dateOfReturn as DateOfReturn,
udf.galfarDueDate as GalfarDueDate,udf.technipAttachment2 as TechnipAttachment2,udf.stageStatus as StageStatus,udf.documentApprovalStatusType as DocumentApprovalStatusType,udf.nativeFileAttachment as NativeFileAttachment,
udf.documentApprovalStatus as DocumentApprovalStatus,udf.documentStatus as DocumentStatus,udf.lastCheckOutBy as LastCheckOutBy,udf.lastCheckOutDate as LastCheckOutDate,udf.stepFileIds as StepFileIds,
udf.technipTransmittalNumber as TechnipTransmittalNumber,udf.galfarToTechnip as GalfarToTechnip,udf.outgoingTechnipIssueCodes as OutgoingTechnipIssueCodes,udf.outgoingTransmittalDate as OutgoingTransmittalDate,
udf.technipDueDate as TechnipDueDate,udf.technipAttachment3 as TechnipAttachment3,udf.commentAttachment as CommentAttachment,lov.Code as NoteStatusCode order by f.Id";
            noteList = await GetApiListCyherAsync<ERP.UI.ViewModel.NoteViewModel>(cypher);
        }
        private async Task TransformProjectDocument()
        {
            var _noteBusiness = _serviceProvider.GetService<INoteBusiness>();
            var _lovBusiness = _serviceProvider.GetService<ILOVBusiness>();
            var idss = await _noteBusiness.GetList(x => x.TemplateCode == "PROJECT_DOCUMENTS");
            var ids = idss.Select(x => x.Id).ToList();
            noteList = noteList.Where(x => !ids.Contains(x.Id.ToString())).ToList();
            if (noteList.Count > 0)
            {
                TargetFolderList = new List<NoteTemplateViewModel>();
                foreach (var folder in noteList)
                {
                    var noteTemp = new NoteTemplateViewModel();
                    noteTemp.TemplateCode = "PROJECT_DOCUMENTS";
                    var note = await _noteBusiness.GetNoteDetails(noteTemp);

                    note.NoteId = folder.Id.ToString();
                    note.NoteNo = folder.NoteNo;
                    note.NoteSubject = folder.Subject;
                    note.NoteDescription = folder.Description;
                    note.StartDate = folder.StartDate;
                    note.ExpiryDate = note.ExpiryDate;
                    if (folder.OwnerUserId.IsNotNull())
                    {
                        note.OwnerUserId = folder.OwnerUserId.ToString();
                    }
                    else
                    {
                        note.OwnerUserId = "1841";
                    }
                    note.RequestedByUserId = note.OwnerUserId;
                    note.Json = "{}";
                    note.DataAction = DataActionEnum.Create;
                    if (folder.ParentId.IsNotNull())
                    {
                        note.ParentNoteId = folder.ParentId.ToString();
                    }
                    note.IsDeleted = folder.IsDeleted == 0 ? false : true;
                    note.Status = folder.Status == ERP.Utility.StatusEnum.Active ? Common.StatusEnum.Active : Common.StatusEnum.Inactive;
                    note.CreatedDate = folder.CreatedDate;
                    note.CreatedBy = folder.CreatedBy.ToString();
                    note.LastUpdatedDate = folder.LastUpdatedDate;
                    note.LastUpdatedBy = folder.LastUpdatedBy.ToString();

                    //var cypher = @"match(g:NTS_Note{NoteNo:'" + folder.NoteNo + "'}) return max(g.Id) as Id";
                    //var Latest = await GetApiListCyherAsync<ERP.UI.ViewModel.NoteViewModel>(cypher);

                    dynamic exo = new System.Dynamic.ExpandoObject();

                    //if (Latest.Count > 0)
                    //{
                    //    var check = Latest.FirstOrDefault();
                    //    if (check.Id == folder.Id)
                    //    {
                    //        ((IDictionary<String, Object>)exo).Add("isLatestRevision", folder.ProjectFolder);
                    //    }
                    //}
                    if (folder.ProjectFolder.IsNotNullAndNotEmpty())
                    {
                        var lov = await _lovBusiness.GetSingle(x => x.Code == folder.ProjectFolder && x.LOVType == "PROJECT_FOLDER");
                        if (lov != null)
                        {
                            ((IDictionary<String, Object>)exo).Add("projectFolder", lov.Id);
                        }
                    }
                    //((IDictionary<String, Object>)exo).Add("projectFolder", folder.ProjectFolder);
                    if (folder.ProjectSubFolder.IsNotNullAndNotEmpty())
                    {
                        var lov = await _lovBusiness.GetSingle(x => x.Code == folder.ProjectSubFolder && x.LOVType == "PROJECT_SUBFOLDER");
                        if (lov != null)
                        {
                            ((IDictionary<String, Object>)exo).Add("projectSubFolder", lov.Id);
                        }
                    }
                    //((IDictionary<String, Object>)exo).Add("projectSubFolder", folder.ProjectSubFolder);
                    if (folder.Revision.IsNotNullAndNotEmpty())
                    {
                        var lov = await _lovBusiness.GetSingle(x => x.Code == folder.Revision && x.LOVType == "GALFAR_REV");
                        if (lov != null)
                        {
                            ((IDictionary<String, Object>)exo).Add("revision", lov.Id);
                        }
                    }
                    //((IDictionary<String, Object>)exo).Add("revision", folder.Revision);
                    if (folder.Discipline.IsNotNullAndNotEmpty())
                    {
                        var lov = await _lovBusiness.GetSingle(x => x.Code == folder.Discipline && x.LOVType == "DISCIPLINE_DEP");
                        if (lov != null)
                        {
                            ((IDictionary<String, Object>)exo).Add("discipline", lov.Id);
                        }
                    }
                    //((IDictionary<String, Object>)exo).Add("discipline", folder.Discipline);
                    ((IDictionary<String, Object>)exo).Add("attachment", folder.Attachment);
                    ((IDictionary<String, Object>)exo).Add("galfarTransmittalNumber", folder.GalfarTransmittalNumber);
                    ((IDictionary<String, Object>)exo).Add("galfarToQp", folder.GalfarToQp);
                    if (folder.OutgoingIssueCodes.IsNotNullAndNotEmpty())
                    {
                        var lov = await _lovBusiness.GetSingle(x => x.Code == folder.OutgoingIssueCodes && x.LOVType == "GALFAR_CODE");
                        if (lov != null)
                        {
                            ((IDictionary<String, Object>)exo).Add("outgoingIssueCodes", lov.Id);
                        }
                    }
                    //((IDictionary<String, Object>)exo).Add("outgoingIssueCodes", folder.OutgoingIssueCodes);
                    ((IDictionary<String, Object>)exo).Add("dateOfSubmission", folder.DateOfSubmission);
                    ((IDictionary<String, Object>)exo).Add("qpDueDate", folder.QpDueDate);
                    ((IDictionary<String, Object>)exo).Add("technipAttachment1", folder.TechnipAttachment1);
                    ((IDictionary<String, Object>)exo).Add("qPTransmittalNumber", folder.QPTransmittalNumber);
                    ((IDictionary<String, Object>)exo).Add("QpToGalfar", folder.QpToGalfar);
                    if (folder.Code.IsNotNullAndNotEmpty())
                    {
                        var lov = await _lovBusiness.GetSingle(x => x.Code == folder.Code && x.LOVType == "GALFAR_CODE");
                        if (lov != null)
                        {
                            ((IDictionary<String, Object>)exo).Add("code", lov.Id);
                        }
                    }
                    //((IDictionary<String, Object>)exo).Add("code", folder.Code);
                    ((IDictionary<String, Object>)exo).Add("dateOfReturn", folder.DateOfReturn);
                    ((IDictionary<String, Object>)exo).Add("galfarDueDate", folder.GalfarDueDate);
                    ((IDictionary<String, Object>)exo).Add("technipAttachment2", folder.TechnipAttachment2);
                    if (folder.StageStatus.IsNotNullAndNotEmpty())
                    {
                        var lov = await _lovBusiness.GetSingle(x => x.Name == folder.StageStatus && x.LOVType == "PENDINGSTATUS");
                        if (lov != null)
                        {
                            ((IDictionary<String, Object>)exo).Add("stageStatus", lov.Id);
                        }
                    }
                    //((IDictionary<String, Object>)exo).Add("stageStatus", folder.StageStatus);
                    if (folder.DocumentApprovalStatusType.IsNotNull())
                    {
                        var lov = await _lovBusiness.GetSingle(x => x.Code == folder.DocumentApprovalStatusType.ToString() && x.LOVType == "DAST");
                        if (lov != null)
                        {
                            ((IDictionary<String, Object>)exo).Add("documentApprovalStatusType", lov.Id);
                        }
                    }
                    //((IDictionary<String, Object>)exo).Add("documentApprovalStatusType", folder.DocumentApprovalStatusType);
                    ((IDictionary<String, Object>)exo).Add("nativeFileAttachment", folder.NativeFileAttachment);
                    ((IDictionary<String, Object>)exo).Add("documentApprovalStatus", folder.DocumentApprovalStatus);
                    ((IDictionary<String, Object>)exo).Add("documentStatus", folder.DocumentStatus);
                    ((IDictionary<String, Object>)exo).Add("lastCheckOutBy", folder.LastCheckOutBy);
                    ((IDictionary<String, Object>)exo).Add("lastCheckOutDate", folder.LastCheckOutDate);
                    ((IDictionary<String, Object>)exo).Add("stepFileIds", folder.StepFileIds);
                    ((IDictionary<String, Object>)exo).Add("technipTransmittalNumber", folder.TechnipTransmittalNumber);
                    ((IDictionary<String, Object>)exo).Add("galfarToTechnip", folder.GalfarToTechnip);
                    if (folder.OutgoingTechnipIssueCodes.IsNotNullAndNotEmpty())
                    {
                        var lov = await _lovBusiness.GetSingle(x => x.Code == folder.OutgoingTechnipIssueCodes && x.LOVType == "GALFAR_CODE");
                        if (lov != null)
                        {
                            ((IDictionary<String, Object>)exo).Add("outgoingTechnipIssueCodes", lov.Id);
                        }
                    }
                    //((IDictionary<String, Object>)exo).Add("outgoingTechnipIssueCodes", folder.OutgoingTechnipIssueCodes);
                    ((IDictionary<String, Object>)exo).Add("outgoingTransmittalDate", folder.OutgoingTransmittalDate);
                    ((IDictionary<String, Object>)exo).Add("technipDueDate", folder.TechnipDueDate);
                    ((IDictionary<String, Object>)exo).Add("technipAttachment3", folder.TechnipAttachment3);
                    ((IDictionary<String, Object>)exo).Add("commentAttachment", folder.CommentAttachment);

                    if (folder.WorkspaceId.IsNotNull())
                    {
                        var workspcaeId = await _noteBusiness.GetWorkspaceId(folder.WorkspaceId.ToString());
                        if (workspcaeId != null)
                        {
                            ((IDictionary<String, Object>)exo).Add("WorkspaceId", workspcaeId.Id);
                        }
                    }
                    note.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);

                    if (folder.NoteStatusCode == "ACTIVE")
                    {
                        note.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                    }
                    else if (folder.NoteStatusCode == "DRAFT")
                    {
                        note.NoteStatusCode = "NOTE_STATUS_DRAFT";
                    }
                    else if (folder.NoteStatusCode == "EXPIRED")
                    {
                        note.NoteStatusCode = "NOTE_STATUS_EXPIRE";
                    }

                    if (note.NoteStatusCode.IsNotNullAndNotEmpty())
                    {
                        TargetFolderList.Add(note);
                    }
                }
            }
        }

        private async Task ExtractEngineerDocument()
        {
            // noteList = await GetApiListAsync<NoteViewModel>();
            var cypher = @"match(f:NTS_Note{IsDeleted:0})
                where (f.IsArchived <>true or f.IsArchived is null)  and f.Id>50000  and f.Id<=70000             
                match(f)-[:R_Note_Template]->(t: NTS_Template{ IsDeleted: 0,Status: 'Active'})
                -[:R_TemplateRoot]->(tm: NTS_TemplateMaster{ IsDeleted: 0,Status: 'Active',Code:'ENGINEERING_SUBCONTRACT'})  
                -[:R_TemplateMaster_TemplateCategory]->(tc: NTS_TemplateCategory{IsDeleted:0,Code:'GENERAL_DOCUMENT'})
                match(f)-[:R_Note_Owner_User]->(u: ADM_User)
                match(f)-[:R_Note_Status_ListOfValue]->(lov: GEN_ListOfValue)
                optional match (f)<-[:R_NoteFieldValue_Note]-(nfv: NTS_NoteFieldValue{ IsDeleted: 0})
                -[:R_NoteFieldValue_TemplateField]->(ttf:NTS_TemplateField)
                with u,f,t,tm,tc,lov,apoc.map.fromPairs(COLLECT([ttf.FieldName,nfv.Code])) as udf 
                optional match(f)-[:R_Note_Parent_Note]->(pn:NTS_Note{IsDeleted: 0,Status: 'Active'}) 
                optional match(f)-[:R_Note_Workspace_Note]->(ws:NTS_Note{IsDeleted: 0,Status: 'Active'})
                return distinct f,pn.Id as ParentId,ws.Id as WorkspaceId,u.Id as OwnerUserId,udf.attachment as Attachment,udf.revision as Revision,udf.discipline as Discipline,udf.incomingTransmittalNumber as IncomingTransmittalNumber,
udf.technipToGalfar as TechnipToGalfar,udf.issueCodes as IssueCodes,udf.incomingTransmittalDate as IncomingTransmittalDate,udf.technipAttachment as TechnipAttachment,udf.galfarTransmittalNumber as GalfarTransmittalNumber,
udf.galfarToQp as GalfarToQp,udf.outgoingIssueCodes as OutgoingIssueCodes,udf.dateOfSubmission as DateOfSubmission,udf.qpDueDate as QpDueDate,udf.technipAttachment1 as TechnipAttachment1,udf.qPTransmittalNumber as QPTransmittalNumber,
udf.QpToGalfar as QpToGalfar,udf.code as Code,udf.dateOfReturn as DateOfReturn,udf.galfarDueDate as GalfarDueDate,udf.technipAttachment2 as TechnipAttachment2,
udf.technipTransmittalNumber as TechnipTransmittalNumber,udf.galfarToTechnip as GalfarToTechnip,udf.outgoingTechnipIssueCodes as OutgoingTechnipIssueCodes,udf.outgoingTransmittalDate as OutgoingTransmittalDate,udf.technipDueDate as TechnipDueDate,
udf.technipAttachment3 as TechnipAttachment3,udf.commentAttachment as CommentAttachment,udf.stageStatus as StageStatus,udf.documentApprovalStatusType as DocumentApprovalStatusType,
udf.nativeFileAttachment as NativeFileAttachment,udf.documentApprovalStatus as DocumentApprovalStatus,udf.documentStatus as DocumentStatus,udf.lastCheckOutBy as LastCheckOutBy,
udf.lastCheckOutDate as LastCheckOutDate,udf.technipRevisionNo as TechnipRevisionNo,udf.technipDocumentNo as TechnipDocumentNo,udf.stepFileIds as StepFileIds,lov.Code as NoteStatusCode order by f.Id";
            noteList = await GetApiListCyherAsync<ERP.UI.ViewModel.NoteViewModel>(cypher);
        }
        private async Task TransformEngineerDocument()
        {
            var _noteBusiness = _serviceProvider.GetService<INoteBusiness>();
            var _lovBusiness = _serviceProvider.GetService<ILOVBusiness>();
            var idss = await _noteBusiness.GetList(x => x.TemplateCode == "ENGINEERING_SUBCONTRACT");
            var ids = idss.Select(x => x.Id).ToList();
            noteList = noteList.Where(x => !ids.Contains(x.Id.ToString())).ToList();
            if (noteList.Count > 0)
            {
                TargetFolderList = new List<NoteTemplateViewModel>();
                foreach (var folder in noteList)
                {
                    var noteTemp = new NoteTemplateViewModel();
                    noteTemp.TemplateCode = "ENGINEERING_SUBCONTRACT";
                    var note = await _noteBusiness.GetNoteDetails(noteTemp);

                    note.NoteId = folder.Id.ToString();
                    note.NoteNo = folder.NoteNo;
                    note.NoteSubject = folder.Subject;
                    note.NoteDescription = folder.Description;
                    note.StartDate = folder.StartDate;
                    note.ExpiryDate = note.ExpiryDate;
                    if (folder.OwnerUserId.IsNotNull())
                    {
                        note.OwnerUserId = folder.OwnerUserId.ToString();
                    }
                    else
                    {
                        note.OwnerUserId = "1841";
                    }
                    note.RequestedByUserId = note.OwnerUserId;
                    note.Json = "{}";
                    note.DataAction = DataActionEnum.Create;
                    if (folder.ParentId.IsNotNull())
                    {
                        note.ParentNoteId = folder.ParentId.ToString();
                    }
                    note.IsDeleted = folder.IsDeleted == 0 ? false : true;
                    note.Status = folder.Status == ERP.Utility.StatusEnum.Active ? Common.StatusEnum.Active : Common.StatusEnum.Inactive;
                    note.CreatedDate = folder.CreatedDate;
                    note.CreatedBy = folder.CreatedBy.ToString();
                    note.LastUpdatedDate = folder.LastUpdatedDate;
                    note.LastUpdatedBy = folder.LastUpdatedBy.ToString();


                    dynamic exo = new System.Dynamic.ExpandoObject();

                    ((IDictionary<String, Object>)exo).Add("attachment", folder.Attachment);
                    if (folder.Revision.IsNotNullAndNotEmpty())
                    {
                        var lov = await _lovBusiness.GetSingle(x => x.Code == folder.Revision && x.LOVType == "GALFAR_REV");
                        if (lov != null)
                        {
                            ((IDictionary<String, Object>)exo).Add("revision", lov.Id);
                        }
                    }
                    //((IDictionary<String, Object>)exo).Add("revision", folder.Revision);
                    if (folder.Discipline.IsNotNullAndNotEmpty())
                    {
                        var lov = await _lovBusiness.GetSingle(x => x.Code == folder.Discipline && x.LOVType == "DISCIPLINE_DEP");
                        if (lov != null)
                        {
                            ((IDictionary<String, Object>)exo).Add("discipline", lov.Id);
                        }
                    }
                    //((IDictionary<String, Object>)exo).Add("discipline", folder.Discipline);
                    ((IDictionary<String, Object>)exo).Add("incomingTransmittalNumber", folder.IncomingTransmittalNumber);
                    ((IDictionary<String, Object>)exo).Add("technipToGalfar", folder.TechnipToGalfar);
                    if (folder.IssueCodes.IsNotNullAndNotEmpty())
                    {
                        var lov = await _lovBusiness.GetSingle(x => x.Code == folder.IssueCodes && x.LOVType == "GALFAR_CODE");
                        if (lov != null)
                        {
                            ((IDictionary<String, Object>)exo).Add("issueCodes", lov.Id);
                        }
                    }
                   // ((IDictionary<String, Object>)exo).Add("issueCodes", folder.IssueCodes);
                    ((IDictionary<String, Object>)exo).Add("incomingTransmittalDate", folder.IncomingTransmittalDate);
                    ((IDictionary<String, Object>)exo).Add("technipAttachment", folder.TechnipAttachment);
                    ((IDictionary<String, Object>)exo).Add("galfarTransmittalNumber", folder.GalfarTransmittalNumber);
                    ((IDictionary<String, Object>)exo).Add("galfarToQp", folder.GalfarToQp);
                    if (folder.OutgoingIssueCodes.IsNotNullAndNotEmpty())
                    {
                        var lov = await _lovBusiness.GetSingle(x => x.Code == folder.OutgoingIssueCodes && x.LOVType == "GALFAR_CODE");
                        if (lov != null)
                        {
                            ((IDictionary<String, Object>)exo).Add("outgoingIssueCodes", lov.Id);
                        }
                    }
                    //((IDictionary<String, Object>)exo).Add("outgoingIssueCodes", folder.OutgoingIssueCodes);
                    ((IDictionary<String, Object>)exo).Add("dateOfSubmission", folder.DateOfSubmission);
                    ((IDictionary<String, Object>)exo).Add("qpDueDate", folder.QpDueDate);
                    ((IDictionary<String, Object>)exo).Add("technipAttachment1", folder.TechnipAttachment1);
                    ((IDictionary<String, Object>)exo).Add("qPTransmittalNumber", folder.QPTransmittalNumber);
                    ((IDictionary<String, Object>)exo).Add("QpToGalfar", folder.QpToGalfar);
                    if (folder.Code.IsNotNullAndNotEmpty())
                    {
                        var lov = await _lovBusiness.GetSingle(x => x.Code == folder.Code && x.LOVType == "GALFAR_CODE");
                        if (lov != null)
                        {
                            ((IDictionary<String, Object>)exo).Add("code", lov.Id);
                        }
                    }
                    //((IDictionary<String, Object>)exo).Add("code", folder.Code);
                    ((IDictionary<String, Object>)exo).Add("dateOfReturn", folder.DateOfReturn);
                    ((IDictionary<String, Object>)exo).Add("galfarDueDate", folder.GalfarDueDate);
                    ((IDictionary<String, Object>)exo).Add("technipAttachment2", folder.TechnipAttachment2);
                    ((IDictionary<String, Object>)exo).Add("technipTransmittalNumber", folder.TechnipTransmittalNumber);
                    ((IDictionary<String, Object>)exo).Add("galfarToTechnip", folder.GalfarToTechnip);
                    if (folder.OutgoingTechnipIssueCodes.IsNotNullAndNotEmpty())
                    {
                        var lov = await _lovBusiness.GetSingle(x => x.Code == folder.OutgoingTechnipIssueCodes && x.LOVType == "GALFAR_CODE");
                        if (lov != null)
                        {
                            ((IDictionary<String, Object>)exo).Add("outgoingTechnipIssueCodes", lov.Id);
                        }
                    }
                   // ((IDictionary<String, Object>)exo).Add("outgoingTechnipIssueCodes", folder.OutgoingTechnipIssueCodes);
                    ((IDictionary<String, Object>)exo).Add("outgoingTransmittalDate", folder.OutgoingTransmittalDate);
                    ((IDictionary<String, Object>)exo).Add("technipDueDate", folder.TechnipDueDate);
                    ((IDictionary<String, Object>)exo).Add("technipAttachment3", folder.TechnipAttachment3);
                    ((IDictionary<String, Object>)exo).Add("commentAttachment", folder.CommentAttachment);
                    if (folder.StageStatus.IsNotNullAndNotEmpty())
                    {
                        var lov = await _lovBusiness.GetSingle(x => x.Name == folder.StageStatus && x.LOVType == "PENDINGSTATUS");
                        if (lov != null)
                        {
                            ((IDictionary<String, Object>)exo).Add("stageStatus", lov.Id);
                        }
                    }
                    // ((IDictionary<String, Object>)exo).Add("stageStatus", folder.StageStatus);
                    if (folder.DocumentApprovalStatusType.IsNotNull())
                    {
                        var lov = await _lovBusiness.GetSingle(x => x.Code == folder.DocumentApprovalStatusType.ToString() && x.LOVType == "DAST");
                        if (lov != null)
                        {
                            ((IDictionary<String, Object>)exo).Add("documentApprovalStatusType", lov.Id);
                        }
                    }
                    //((IDictionary<String, Object>)exo).Add("documentApprovalStatusType", folder.DocumentApprovalStatusType);
                    ((IDictionary<String, Object>)exo).Add("nativeFileAttachment", folder.NativeFileAttachment);
                    ((IDictionary<String, Object>)exo).Add("documentApprovalStatus", folder.DocumentApprovalStatus);
                    ((IDictionary<String, Object>)exo).Add("documentStatus", folder.DocumentStatus);
                    ((IDictionary<String, Object>)exo).Add("lastCheckOutBy", folder.LastCheckOutBy);
                    ((IDictionary<String, Object>)exo).Add("lastCheckOutDate", folder.LastCheckOutDate);
                    ((IDictionary<String, Object>)exo).Add("technipRevisionNo", folder.TechnipRevisionNo);
                    ((IDictionary<String, Object>)exo).Add("technipDocumentNo", folder.TechnipDocumentNo);
                    ((IDictionary<String, Object>)exo).Add("stepFileIds	", folder.StepFileIds);
                    if (folder.WorkspaceId.IsNotNull())
                    {
                        var workspcaeId = await _noteBusiness.GetWorkspaceId(folder.WorkspaceId.ToString());
                        if (workspcaeId != null)
                        {
                            ((IDictionary<String, Object>)exo).Add("WorkspaceId", workspcaeId.Id);
                        }
                    }

                    note.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);

                    if (folder.NoteStatusCode == "ACTIVE")
                    {
                        note.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                    }
                    else if (folder.NoteStatusCode == "DRAFT")
                    {
                        note.NoteStatusCode = "NOTE_STATUS_DRAFT";
                    }
                    else if (folder.NoteStatusCode == "EXPIRED")
                    {
                        note.NoteStatusCode = "NOTE_STATUS_EXPIRE";
                    }

                    if (note.NoteStatusCode.IsNotNullAndNotEmpty())
                    {
                        TargetFolderList.Add(note);
                    }
                }
            }
        }

        private async Task ExtractVendorDocument()
        {
            // noteList = await GetApiListAsync<NoteViewModel>();
            var cypher = @"match(f:NTS_Note{IsDeleted:0})
                where (f.IsArchived <>true or f.IsArchived is null)                
                match(f)-[:R_Note_Template]->(t: NTS_Template{ IsDeleted: 0,Status: 'Active'})
                -[:R_TemplateRoot]->(tm: NTS_TemplateMaster{ IsDeleted: 0,Status: 'Active',Code:'GALFAR_VENDOR'})  
                -[:R_TemplateMaster_TemplateCategory]->(tc: NTS_TemplateCategory{IsDeleted:0,Code:'GENERAL_DOCUMENT'})
                optional match(f)-[:R_Note_Owner_User]->(u: ADM_User)
                optional match(f)-[:R_Note_Status_ListOfValue]->(lov: GEN_ListOfValue)
                optional match (f)<-[:R_NoteFieldValue_Note]-(nfv: NTS_NoteFieldValue{ IsDeleted: 0})
                -[:R_NoteFieldValue_TemplateField]->(ttf:NTS_TemplateField)
                with u,f,t,tm,tc,lov,apoc.map.fromPairs(COLLECT([ttf.FieldName,nfv.Code])) as udf 
                optional match(f)-[:R_Note_Parent_Note]->(pn:NTS_Note{IsDeleted: 0,Status: 'Active'}) 
                optional match(f)-[:R_Note_Workspace_Note]->(ws:NTS_Note{IsDeleted: 0,Status: 'Active'})
                return distinct f,pn.Id as ParentId,ws.Id as WorkspaceId,u.Id as OwnerUserId,udf.vendorList as VendorList,udf.revision as Revision,udf.discipline as Discipline,udf.attachment as Attachment,
udf.vendorDocumentNo as VendorDocumentNo,udf.technipToGalfar as TechnipToGalfar,udf.vendorRevisionNo as VendorRevisionNo,udf.incomingTransmittalNumber as IncomingTransmittalNumber,udf.issueCodes as IssueCodes,
udf.incomingTransmittalDate as IncomingTransmittalDate,udf.technipAttachment as TechnipAttachment,udf.vendorTransmittalNo as VendorTransmittalNo,udf.galfarCommentsToVendor as GalfarCommentsToVendor,udf.revisionNo as RevisionNo,
udf.vendorOutgoingIssueCode as VendorOutgoingIssueCode,udf.vendorTransmittalAttachment as VendorTransmittalAttachment,
udf.galfarTransmittalNumber as GalfarTransmittalNumber,udf.galfarToQp as GalfarToQp,udf.outgoingIssueCodes as OutgoingIssueCodes,udf.dateOfSubmission as DateOfSubmission,
udf.qpDueDate as QpDueDate,udf.technipAttachment1 as TechnipAttachment1,udf.qPTransmittalNumber as QPTransmittalNumber,udf.QpToGalfar as QpToGalfar,udf.code as Code,
udf.dateOfReturn as DateOfReturn,udf.galfarDueDate as GalfarDueDate,udf.technipAttachment2 as TechnipAttachment2,udf.technipTransmittalNumber as TechnipTransmittalNumber,
udf.galfarToTechnip as GalfarToTechnip,udf.outgoingTechnipIssueCodes as OutgoingTechnipIssueCodes,udf.outgoingTransmittalDate as OutgoingTransmittalDate,udf.technipDueDate as TechnipDueDate,udf.technipAttachment3 as TechnipAttachment3,
udf.commentAttachment as CommentAttachment,udf.stageStatus as StageStatus,udf.documentApprovalStatusType as DocumentApprovalStatusType,udf.documentApprovalStatus as DocumentApprovalStatus,
udf.documentStatus as DocumentStatus,udf.lastCheckOutBy as LastCheckOutBy,udf.lastCheckOutDate as LastCheckOutDate,udf.stepFileIds as StepFileIds,lov.Code as NoteStatusCode order by f.Id";
            noteList = await GetApiListCyherAsync<ERP.UI.ViewModel.NoteViewModel>(cypher);
        }
        private async Task TransformVendorDocument()
        {
            var _noteBusiness = _serviceProvider.GetService<INoteBusiness>();
            var _lovBusiness = _serviceProvider.GetService<ILOVBusiness>();
            var idss = await _noteBusiness.GetList(x => x.TemplateCode == "GALFAR_VENDOR");
            var ids = idss.Select(x => x.Id).ToList();
           // noteList = noteList.Where(x => !ids.Contains(x.Id.ToString())).ToList();
            if (noteList.Count > 0)
            {
                TargetFolderList = new List<NoteTemplateViewModel>();
                foreach (var folder in noteList)
                {
                    var noteTemp = new NoteTemplateViewModel();
                    noteTemp.TemplateCode = "GALFAR_VENDOR";
                    var note = await _noteBusiness.GetNoteDetails(noteTemp);

                    note.NoteId = folder.Id.ToString();
                    note.NoteNo = folder.NoteNo;
                    note.NoteSubject = folder.Subject;
                    note.NoteDescription = folder.Description;
                    note.StartDate = folder.StartDate;
                    note.ExpiryDate = note.ExpiryDate;
                    if (folder.OwnerUserId.IsNotNull())
                    {
                        note.OwnerUserId = folder.OwnerUserId.ToString();
                    }
                    else
                    {
                        note.OwnerUserId = "1841";
                    }
                    note.RequestedByUserId = note.OwnerUserId;
                    note.Json = "{}";
                    note.DataAction = DataActionEnum.Create;
                    if (folder.ParentId.IsNotNull())
                    {
                        note.ParentNoteId = folder.ParentId.ToString();
                    }
                    note.IsDeleted = folder.IsDeleted == 0 ? false : true;
                    note.Status = folder.Status == ERP.Utility.StatusEnum.Active ? Common.StatusEnum.Active : Common.StatusEnum.Inactive;
                    note.CreatedDate = folder.CreatedDate;
                    note.CreatedBy = folder.CreatedBy.ToString();
                    note.LastUpdatedDate = folder.LastUpdatedDate;
                    note.LastUpdatedBy = folder.LastUpdatedBy.ToString();


                    dynamic exo = new System.Dynamic.ExpandoObject();

                    if(folder.VendorList.IsNotNullAndNotEmpty())
                    {
                        var lov = await _lovBusiness.GetSingle(x => x.Code == folder.VendorList && x.LOVType== "VENDOR_LIST");
                        if (lov != null)
                        {
                            ((IDictionary<String, Object>)exo).Add("vendorList", lov.Id);
                        }
                    }
                    if (folder.Revision.IsNotNullAndNotEmpty())
                    {
                        var lov = await _lovBusiness.GetSingle(x => x.Code == folder.Revision && x.LOVType == "REVISION");
                        if (lov != null)
                        {
                            ((IDictionary<String, Object>)exo).Add("revision", lov.Id);
                        }
                    }
                    if (folder.Discipline.IsNotNullAndNotEmpty())
                    {
                        var lov = await _lovBusiness.GetSingle(x => x.Code == folder.Discipline && x.LOVType == "DISCIPLINE");
                        if (lov != null)
                        {
                            ((IDictionary<String, Object>)exo).Add("discipline", lov.Id);
                        }
                    }
                    ((IDictionary<String, Object>)exo).Add("attachment", folder.Attachment);
                    if (folder.VendorDocumentNo.IsNotNullAndNotEmpty())
                    {
                        var vendor = folder.VendorDocumentNo.Replace("'", "");
                        ((IDictionary<String, Object>)exo).Add("vendorDocumentNo", vendor);
                    }
                    ((IDictionary<String, Object>)exo).Add("technipToGalfar", folder.TechnipToGalfar);
                    if (folder.VendorRevisionNo.IsNotNullAndNotEmpty())
                    {
                        var lov = await _lovBusiness.GetSingle(x => x.Code == folder.VendorRevisionNo && x.LOVType == "REVISION");
                        if (lov != null)
                        {
                            ((IDictionary<String, Object>)exo).Add("vendorRevisionNo", lov.Id);
                        }
                    }
                    ((IDictionary<String, Object>)exo).Add("incomingTransmittalNumber", folder.IncomingTransmittalNumber);
                    if (folder.IssueCodes.IsNotNullAndNotEmpty())
                    {
                        var lov = await _lovBusiness.GetSingle(x => x.Code == folder.IssueCodes && x.LOVType == "ISSUE_CODE");
                        if (lov != null)
                        {
                            ((IDictionary<String, Object>)exo).Add("issueCodes", lov.Id);
                        }
                    }
                 
                    ((IDictionary<String, Object>)exo).Add("incomingTransmittalDate", folder.IncomingTransmittalDate);
                    ((IDictionary<String, Object>)exo).Add("technipAttachment", folder.TechnipAttachment);
                    ((IDictionary<String, Object>)exo).Add("vendorTransmittalNo", folder.VendorTransmittalNo);
                    ((IDictionary<String, Object>)exo).Add("galfarCommentsToVendor", folder.GalfarCommentsToVendor);
                    if (folder.RevisionNo.IsNotNullAndNotEmpty())
                    {
                        var lov = await _lovBusiness.GetSingle(x => x.Code == folder.RevisionNo && x.LOVType == "REVISION");
                        if (lov != null)
                        {
                            ((IDictionary<String, Object>)exo).Add("revisionNo", lov.Id);
                        }
                    }
                    //((IDictionary<String, Object>)exo).Add("revisionNo", folder.RevisionNo);
                    if (folder.VendorOutgoingIssueCode.IsNotNullAndNotEmpty())
                    {
                        var lov = await _lovBusiness.GetSingle(x => x.Code == folder.VendorOutgoingIssueCode && x.LOVType == "ISSUE_CODE");
                        if (lov != null)
                        {
                            ((IDictionary<String, Object>)exo).Add("vendorOutgoingIssueCode", lov.Id);
                        }
                    }
                   // ((IDictionary<String, Object>)exo).Add("vendorOutgoingIssueCode", folder.VendorOutgoingIssueCode);
                    ((IDictionary<String, Object>)exo).Add("vendorTransmittalAttachment", folder.VendorTransmittalAttachment);
                    ((IDictionary<String, Object>)exo).Add("galfarTransmittalNumber", folder.GalfarTransmittalNumber);
                    ((IDictionary<String, Object>)exo).Add("galfarToQp", folder.GalfarToQp);
                    if (folder.OutgoingIssueCodes.IsNotNullAndNotEmpty())
                    {
                        var lov = await _lovBusiness.GetSingle(x => x.Code == folder.OutgoingIssueCodes && x.LOVType == "ISSUE_CODE");
                        if (lov != null)
                        {
                            ((IDictionary<String, Object>)exo).Add("outgoingIssueCodes", lov.Id);
                        }
                    }
                    //((IDictionary<String, Object>)exo).Add("outgoingIssueCodes", folder.OutgoingIssueCodes);
                    ((IDictionary<String, Object>)exo).Add("dateOfSubmission", folder.DateOfSubmission);
                    ((IDictionary<String, Object>)exo).Add("qpDueDate", folder.QpDueDate);
                    ((IDictionary<String, Object>)exo).Add("technipAttachment1", folder.TechnipAttachment1);
                    ((IDictionary<String, Object>)exo).Add("qPTransmittalNumber", folder.QPTransmittalNumber);
                    ((IDictionary<String, Object>)exo).Add("QpToGalfar", folder.QpToGalfar);
                    if (folder.Code.IsNotNullAndNotEmpty())
                    {
                        var lov = await _lovBusiness.GetSingle(x => x.Code == folder.Code && x.LOVType == "ISSUE_CODE");
                        if (lov != null)
                        {
                            ((IDictionary<String, Object>)exo).Add("code", lov.Id);
                        }
                    }
                    //((IDictionary<String, Object>)exo).Add("code", folder.Code);
                    ((IDictionary<String, Object>)exo).Add("dateOfReturn", folder.DateOfReturn);
                    ((IDictionary<String, Object>)exo).Add("galfarDueDate", folder.GalfarDueDate);
                    ((IDictionary<String, Object>)exo).Add("technipAttachment2", folder.TechnipAttachment2);
                    ((IDictionary<String, Object>)exo).Add("technipTransmittalNumber", folder.TechnipTransmittalNumber);
                    ((IDictionary<String, Object>)exo).Add("galfarToTechnip", folder.GalfarToTechnip);
                    if (folder.OutgoingTechnipIssueCodes.IsNotNullAndNotEmpty())
                    {
                        var lov = await _lovBusiness.GetSingle(x => x.Code == folder.OutgoingTechnipIssueCodes && x.LOVType == "ISSUE_CODE");
                        if (lov != null)
                        {
                            ((IDictionary<String, Object>)exo).Add("outgoingTechnipIssueCodes", lov.Id);
                        }
                    }
                    //((IDictionary<String, Object>)exo).Add("outgoingTechnipIssueCodes", folder.OutgoingTechnipIssueCodes);
                    ((IDictionary<String, Object>)exo).Add("outgoingTransmittalDate", folder.OutgoingTransmittalDate);
                    ((IDictionary<String, Object>)exo).Add("technipDueDate", folder.TechnipDueDate);
                    ((IDictionary<String, Object>)exo).Add("technipAttachment3", folder.TechnipAttachment3);
                    ((IDictionary<String, Object>)exo).Add("commentAttachment", folder.CommentAttachment);
                    if (folder.StageStatus.IsNotNullAndNotEmpty())
                    {
                        var lov = await _lovBusiness.GetSingle(x => x.Name == folder.StageStatus && x.LOVType == "PENDINGSTATUS");
                        if (lov != null)
                        {
                            ((IDictionary<String, Object>)exo).Add("stageStatus", lov.Id);
                            note.NoteStatusId = lov.Id;
                        }
                       
                    }
                    //((IDictionary<String, Object>)exo).Add("stageStatus", folder.StageStatus);
                    if (folder.DocumentApprovalStatusType.IsNotNull())
                    {
                        var lov = await _lovBusiness.GetSingle(x => x.Code == folder.DocumentApprovalStatusType.ToString() && x.LOVType == "DAST");
                        if (lov != null)
                        {
                            ((IDictionary<String, Object>)exo).Add("documentApprovalStatusType", lov.Id);
                        }
                    }
                    //((IDictionary<String, Object>)exo).Add("documentApprovalStatusType", folder.DocumentApprovalStatusType);
                    ((IDictionary<String, Object>)exo).Add("documentApprovalStatus", folder.DocumentApprovalStatus);
                    ((IDictionary<String, Object>)exo).Add("documentStatus", folder.DocumentStatus);
                    ((IDictionary<String, Object>)exo).Add("lastCheckOutBy", folder.LastCheckOutBy);
                    ((IDictionary<String, Object>)exo).Add("lastCheckOutDate", folder.LastCheckOutDate);
                    ((IDictionary<String, Object>)exo).Add("stepFileIds", folder.StepFileIds);
                    if (folder.WorkspaceId.IsNotNull())
                    {
                        var workspcaeId = await _noteBusiness.GetWorkspaceId(folder.WorkspaceId.ToString());
                        if (workspcaeId != null)
                        {
                            ((IDictionary<String, Object>)exo).Add("WorkspaceId", workspcaeId.Id);
                        }
                    }

                    note.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);

                    if (folder.NoteStatusCode == "ACTIVE")
                    {
                        note.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                    }
                    else if (folder.NoteStatusCode == "DRAFT")
                    {
                        note.NoteStatusCode = "NOTE_STATUS_DRAFT";
                    }
                    else if (folder.NoteStatusCode == "EXPIRED")
                    {
                        note.NoteStatusCode = "NOTE_STATUS_EXPIRE";
                    }

                    if (note.NoteStatusCode.IsNotNullAndNotEmpty())
                    {
                        TargetFolderList.Add(note);
                    }
                }
            }
        }

        private async Task ExtractRequestForInspectionDocument()
        {
            // noteList = await GetApiListAsync<NoteViewModel>();
            var cypher = @"match(f:NTS_Note{IsDeleted:0})
                where (f.IsArchived <>true or f.IsArchived is null)                
                match(f)-[:R_Note_Template]->(t: NTS_Template{ IsDeleted: 0,Status: 'Active'})
                -[:R_TemplateRoot]->(tm: NTS_TemplateMaster{ IsDeleted: 0,Status: 'Active',Code:'GALFAR_REQUEST_FOR_INSPECTION'})  
                -[:R_TemplateMaster_TemplateCategory]->(tc: NTS_TemplateCategory{IsDeleted:0,Code:'GENERAL_DOCUMENT'})
                optional match(f)-[:R_Note_Owner_User]->(u: ADM_User)
                optional match(f)-[:R_Note_Status_ListOfValue]->(lov: GEN_ListOfValue)
                optional match (f)<-[:R_NoteFieldValue_Note]-(nfv: NTS_NoteFieldValue{ IsDeleted: 0})
                -[:R_NoteFieldValue_TemplateField]->(ttf:NTS_TemplateField)
                with u,f,t,tm,tc,lov,apoc.map.fromPairs(COLLECT([ttf.FieldName,nfv.Code])) as udf 
                optional match(f)-[:R_Note_Parent_Note]->(pn:NTS_Note{IsDeleted: 0,Status: 'Active'}) 
                optional match(f)-[:R_Note_Workspace_Note]->(ws:NTS_Note{IsDeleted: 0,Status: 'Active'})
                return distinct f,pn.Id as ParentId,ws.Id as WorkspaceId,u.Id as OwnerUserId,udf.InspectionActivity as InspectionActivity,udf.attachment as Attachment,udf.contractNo as ContractNo,udf.date as Date,
udf.discipline as Discipline,udf.locationOfInspection as LocationOfInspection,udf.system as System,udf.dateOfInspection as DateOfInspection,udf.otherDates as OtherDates,udf.area as Area,
udf.location as Location, udf.drawingNumber as DrawingNumber,udf.tagNumber as TagNumber,udf.chainage as Chainage,udf.itpQcpNo as ItpQcpNo,udf.itpItemNo as ItpItemNo,udf.interventionPoints as InterventionPoints,
udf.subContract as SubContract,udf.contractor as Contractor,udf.qp as Qp, udf.tpi as Tpi,udf.submittedBy as SubmittedBy,udf.subContractorName as SubContractorName,udf.contractorName as ContractorName	,
udf.subContractorMobileNo as SubContractorMobileNo,udf.contractorMobileNo as ContractorMobileNo,udf.subContractDate as SubContractDate,udf.contractDate	 as ContractDate,udf.checkedBy as CheckedBy,
udf.tpiName as TpiName,udf.contractorQaQcName as ContractorQaQcName,udf.tpiMobileNo as TpiMobileNo,udf.contractorQaQcMobileNo as ContractorQaQcMobileNo,udf.tpiContractDate as TpiContractDate,
udf.contractQaQcDate as ContractQaQcDate,udf.galfarToQp as GalfarToQp,udf.dateOfSubmission as DateOfSubmission,udf.QpToGalfar as QpToGalfar,udf.code as Code,udf.dateOfReturn as DateOfReturn,
udf.stageStatus	as StageStatus,udf.documentApprovalStatusType as DocumentApprovalStatusType,udf.finalRemarks as FinalRemarks,udf.galfarTransmittalNumber as GalfarTransmittalNumber,
udf.outgoingIssueCodes as OutgoingIssueCodes,udf.qpDueDate as QpDueDate,udf.technipAttachment1 as TechnipAttachment1,udf.qPTransmittalNumber as QPTransmittalNumber,udf.galfarDueDate as GalfarDueDate,
udf.technipAttachment2 as TechnipAttachment2,udf.documentApprovalStatus as DocumentApprovalStatus,udf.footerDocRefNo as FooterDocRefNo,udf.revision as Revision,lov.Code as NoteStatusCode order by f.Id
 ";
            noteList = await GetApiListCyherAsync<ERP.UI.ViewModel.NoteViewModel>(cypher);
        }
        private async Task TransformRequestForInspectionDocument()
        {
            var _noteBusiness = _serviceProvider.GetService<INoteBusiness>();
            var _lovBusiness = _serviceProvider.GetService<ILOVBusiness>();
            var idss = await _noteBusiness.GetList(x => x.TemplateCode == "GALFAR_REQUEST_FOR_INSPECTION");
            var ids = idss.Select(x=>x.Id).ToList();
            noteList = noteList.Where(x => !ids.Contains(x.Id.ToString())).ToList();
            if (noteList.Count > 0)
            {
                TargetFolderList = new List<NoteTemplateViewModel>();
                foreach (var folder in noteList)
                {
                    var noteTemp = new NoteTemplateViewModel();
                    noteTemp.TemplateCode = "GALFAR_REQUEST_FOR_INSPECTION";
                    var note = await _noteBusiness.GetNoteDetails(noteTemp);

                    note.NoteId = folder.Id.ToString();
                    note.NoteNo = folder.NoteNo;
                    note.NoteSubject = folder.Subject;
                    note.NoteDescription = folder.Description;
                    note.StartDate = folder.StartDate;
                    note.ExpiryDate = note.ExpiryDate;
                    if (folder.OwnerUserId.IsNotNull())
                    {
                        note.OwnerUserId = folder.OwnerUserId.ToString();
                    }
                    else
                    {
                        note.OwnerUserId = "1841";
                    }
                    note.RequestedByUserId = folder.OwnerUserId.ToString();
                    note.Json = "{}";
                    note.DataAction = DataActionEnum.Create;
                    if (folder.ParentId.IsNotNull())
                    {
                        note.ParentNoteId = folder.ParentId.ToString();
                    }
                    note.IsDeleted = folder.IsDeleted == 0 ? false : true;
                    note.Status = folder.Status == ERP.Utility.StatusEnum.Active ? Common.StatusEnum.Active : Common.StatusEnum.Inactive;
                    note.CreatedDate = folder.CreatedDate;
                    note.CreatedBy = folder.CreatedBy.ToString();
                    note.LastUpdatedDate = folder.LastUpdatedDate;
                    note.LastUpdatedBy = folder.LastUpdatedBy.ToString();


                    dynamic exo = new System.Dynamic.ExpandoObject();

                    ((IDictionary<String, Object>)exo).Add("InspectionActivity", folder.InspectionActivity);
                    ((IDictionary<String, Object>)exo).Add("attachment", folder.Attachment);
                    ((IDictionary<String, Object>)exo).Add("contractNo", folder.ContractNo);
                    ((IDictionary<String, Object>)exo).Add("date", folder.Date);
                    if (folder.Discipline.IsNotNullAndNotEmpty())
                    {
                        var lov = await _lovBusiness.GetSingle(x => x.Code == folder.Discipline && x.LOVType == "DISCIPLINE_DEP");
                        if (lov != null)
                        {
                            ((IDictionary<String, Object>)exo).Add("discipline", lov.Id);
                        }
                    }
                    //((IDictionary<String, Object>)exo).Add("discipline", folder.Discipline);
                    if (folder.LocationOfInspection.IsNotNullAndNotEmpty())
                    {
                        var lov = await _lovBusiness.GetSingle(x => x.Code == folder.LocationOfInspection && x.LOVType == "LOCATION_INSPECTION");
                        if (lov != null)
                        {
                            ((IDictionary<String, Object>)exo).Add("locationOfInspection", lov.Id);
                        }
                    }
                    //((IDictionary<String, Object>)exo).Add("locationOfInspection", folder.LocationOfInspection);
                    ((IDictionary<String, Object>)exo).Add("system", folder.System);
                    ((IDictionary<String, Object>)exo).Add("dateOfInspection", folder.DateOfInspection);
                    ((IDictionary<String, Object>)exo).Add("otherDates", folder.OtherDates);
                    if (folder.Area.IsNotNullAndNotEmpty())
                    {
                        var lov = await _lovBusiness.GetSingle(x => x.Code == folder.Area && x.LOVType == "RFIAREA");
                        if (lov != null)
                        {
                            ((IDictionary<String, Object>)exo).Add("area", lov.Id);
                        }
                    }
                    //((IDictionary<String, Object>)exo).Add("area", folder.Area);
                    ((IDictionary<String, Object>)exo).Add("location", folder.Location);
                    ((IDictionary<String, Object>)exo).Add("drawingNumber", folder.DrawingNumber);
                    ((IDictionary<String, Object>)exo).Add("tagNumber", folder.TagNumber);
                    ((IDictionary<String, Object>)exo).Add("chainage", folder.Chainage);
                    ((IDictionary<String, Object>)exo).Add("itpQcpNo", folder.ItpQcpNo);
                    ((IDictionary<String, Object>)exo).Add("itpItemNo", folder.ItpItemNo);
                    ((IDictionary<String, Object>)exo).Add("interventionPoints", folder.InterventionPoints);
                    if (folder.SubContract.IsNotNullAndNotEmpty())
                    {
                        var lov = await _lovBusiness.GetSingle(x => x.Code == folder.SubContract && x.LOVType == "RFIAREA");
                        if (lov != null)
                        {
                            ((IDictionary<String, Object>)exo).Add("subContract", lov.Id);
                        }
                    }
                    //((IDictionary<String, Object>)exo).Add("subContract", folder.SubContract);
                    if (folder.Contractor.IsNotNullAndNotEmpty())
                    {
                        var lov = await _lovBusiness.GetSingle(x => x.Code == folder.Contractor && x.LOVType == "INTERVENTIONPOINT");
                        if (lov != null)
                        {
                            ((IDictionary<String, Object>)exo).Add("contractor", lov.Id);
                        }
                    }
                    //((IDictionary<String, Object>)exo).Add("contractor", folder.Contractor);
                    if (folder.Qp.IsNotNullAndNotEmpty())
                    {
                        var lov = await _lovBusiness.GetSingle(x => x.Code == folder.Qp && x.LOVType == "INTERVENTIONPOINT");
                        if (lov != null)
                        {
                            ((IDictionary<String, Object>)exo).Add("qp", lov.Id);
                        }
                    }
                    //((IDictionary<String, Object>)exo).Add("qp", folder.Qp);
                    if (folder.Tpi.IsNotNullAndNotEmpty())
                    {
                        var lov = await _lovBusiness.GetSingle(x => x.Code == folder.Tpi && x.LOVType == "INTERVENTIONPOINT");
                        if (lov != null)
                        {
                            ((IDictionary<String, Object>)exo).Add("tpi", lov.Id);
                        }
                    }
                    //((IDictionary<String, Object>)exo).Add("tpi", folder.Tpi);
                    ((IDictionary<String, Object>)exo).Add("submittedBy", folder.SubmittedBy);
                    ((IDictionary<String, Object>)exo).Add("subContractorName", folder.SubContractorName);
                    ((IDictionary<String, Object>)exo).Add("contractorName", folder.ContractorName);
                    ((IDictionary<String, Object>)exo).Add("subContractorMobileNo", folder.SubContractorMobileNo);
                    ((IDictionary<String, Object>)exo).Add("contractorMobileNo", folder.ContractorMobileNo);
                    ((IDictionary<String, Object>)exo).Add("subContractDate", folder.SubContractDate);
                    ((IDictionary<String, Object>)exo).Add("contractDate", folder.ContractDate);
                    ((IDictionary<String, Object>)exo).Add("checkedBy", folder.CheckedBy);
                    ((IDictionary<String, Object>)exo).Add("tpiName", folder.TpiName);
                    ((IDictionary<String, Object>)exo).Add("contractorQaQcName", folder.ContractorQaQcName);
                    ((IDictionary<String, Object>)exo).Add("tpiMobileNo", folder.TpiMobileNo);
                    ((IDictionary<String, Object>)exo).Add("contractorQaQcMobileNo", folder.ContractorQaQcMobileNo);
                    ((IDictionary<String, Object>)exo).Add("tpiContractDate", folder.TpiContractDate);
                    ((IDictionary<String, Object>)exo).Add("contractQaQcDate", folder.ContractQaQcDate);
                    ((IDictionary<String, Object>)exo).Add("galfarToQp", folder.GalfarToQp);
                    ((IDictionary<String, Object>)exo).Add("dateOfSubmission", folder.DateOfSubmission);
                    ((IDictionary<String, Object>)exo).Add("QpToGalfar", folder.QpToGalfar);
                    if (folder.Code.IsNotNullAndNotEmpty())
                    {
                        var lov = await _lovBusiness.GetSingle(x => x.Code == folder.Code && x.LOVType == "RFIFinalStage");
                        if (lov != null)
                        {
                            ((IDictionary<String, Object>)exo).Add("code", lov.Id);
                        }
                    }
                    //((IDictionary<String, Object>)exo).Add("code", folder.Code);
                    ((IDictionary<String, Object>)exo).Add("dateOfReturn", folder.DateOfReturn);
                    ((IDictionary<String, Object>)exo).Add("commentAttachment", folder.CommentAttachment);
                    if (folder.StageStatus.IsNotNullAndNotEmpty())
                    {
                        var lov = await _lovBusiness.GetSingle(x => x.Name == folder.StageStatus && x.LOVType == "PENDINGSTATUS");
                        if (lov != null)
                        {
                            ((IDictionary<String, Object>)exo).Add("stageStatus", lov.Id);
                        }
                    }
                    //((IDictionary<String, Object>)exo).Add("stageStatus", folder.StageStatus);
                    if (folder.DocumentApprovalStatusType.IsNotNull())
                    {
                        var lov = await _lovBusiness.GetSingle(x => x.Code == folder.DocumentApprovalStatusType.ToString() && x.LOVType == "DAST");
                        if (lov != null)
                        {
                            ((IDictionary<String, Object>)exo).Add("documentApprovalStatusType", lov.Id);
                        }
                    }
                    //((IDictionary<String, Object>)exo).Add("documentApprovalStatusType", folder.DocumentApprovalStatusType);
                    ((IDictionary<String, Object>)exo).Add("finalRemarks", folder.FinalRemarks);
                    ((IDictionary<String, Object>)exo).Add("galfarTransmittalNumber", folder.GalfarTransmittalNumber);
                    ((IDictionary<String, Object>)exo).Add("outgoingIssueCodes", folder.OutgoingIssueCodes);
                    ((IDictionary<String, Object>)exo).Add("qpDueDate", folder.QpDueDate);
                    ((IDictionary<String, Object>)exo).Add("technipAttachment1", folder.TechnipAttachment1);
                    ((IDictionary<String, Object>)exo).Add("qPTransmittalNumber", folder.QPTransmittalNumber);
                    ((IDictionary<String, Object>)exo).Add("galfarDueDate", folder.GalfarDueDate);
                    ((IDictionary<String, Object>)exo).Add("technipAttachment2", folder.TechnipAttachment2);
                    ((IDictionary<String, Object>)exo).Add("documentApprovalStatus", folder.DocumentApprovalStatus);
                    ((IDictionary<String, Object>)exo).Add("footerDocRefNo", folder.FooterDocRefNo);
                    ((IDictionary<String, Object>)exo).Add("revision", folder.Revision);
                    if (folder.WorkspaceId.IsNotNull())
                    {
                        var workspcaeId = await _noteBusiness.GetWorkspaceId(folder.WorkspaceId.ToString());
                        if (workspcaeId != null)
                        {
                            ((IDictionary<String, Object>)exo).Add("WorkspaceId", workspcaeId.Id);
                        }
                    }

                    note.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);

                    if (folder.NoteStatusCode == "ACTIVE")
                    {
                        note.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                    }
                    else if (folder.NoteStatusCode == "DRAFT")
                    {
                        note.NoteStatusCode = "NOTE_STATUS_DRAFT";
                    }
                    else if (folder.NoteStatusCode == "EXPIRED")
                    {
                        note.NoteStatusCode = "NOTE_STATUS_EXPIRE";
                    }

                    if (note.NoteStatusCode.IsNotNullAndNotEmpty())
                    {
                        TargetFolderList.Add(note);
                    }
                }
            }
        }

        private async Task ExtractRequestForInspectionHalulDocument()
        {
            // noteList = await GetApiListAsync<NoteViewModel>();
            var cypher = @"match(f:NTS_Note{IsDeleted:0})
                where (f.IsArchived <>true or f.IsArchived is null)                
                match(f)-[:R_Note_Template]->(t: NTS_Template{ IsDeleted: 0,Status: 'Active'})
                -[:R_TemplateRoot]->(tm: NTS_TemplateMaster{ IsDeleted: 0,Status: 'Active',Code:'HALUL_REQUEST_FOR_INSPECTION'})  
                -[:R_TemplateMaster_TemplateCategory]->(tc: NTS_TemplateCategory{IsDeleted:0,Code:'GENERAL_DOCUMENT'})
                optional match(f)-[:R_Note_Owner_User]->(u: ADM_User)
                optional match(f)-[:R_Note_Status_ListOfValue]->(lov: GEN_ListOfValue)
                optional match (f)<-[:R_NoteFieldValue_Note]-(nfv: NTS_NoteFieldValue{ IsDeleted: 0})
                -[:R_NoteFieldValue_TemplateField]->(ttf:NTS_TemplateField)
                with u,f,t,tm,tc,lov,apoc.map.fromPairs(COLLECT([ttf.FieldName,nfv.Code])) as udf 
                optional match(f)-[:R_Note_Parent_Note]->(pn:NTS_Note{IsDeleted: 0,Status: 'Active'}) 
                optional match(f)-[:R_Note_Workspace_Note]->(ws:NTS_Note{IsDeleted: 0,Status: 'Active'})
                return distinct f,pn.Id as ParentId,ws.Id as WorkspaceId,u.Id as OwnerUserId,udf.InspectionActivity as InspectionActivity,udf.attachment as Attachment,udf.projectNo as ProjectNo,udf.contractNo as ContractNo,udf.date as Date,
udf.type as Type,udf.discipline as Discipline,udf.locationOfInspection as LocationOfInspection,udf.system as System,udf.dateOfInspection as DateOfInspection,udf.otherDates as OtherDates,udf.area as Area,
udf.location as location, udf.drawingNumber as drawingNumber, udf.drawingAttachment as drawingAttachment,udf.tagNumber as tagNumber,udf.itpQcpNo as itpQcpNo,udf.itpItemNo as itpItemNo,udf.interventionPoints as interventionPoints,
udf.subContract as SubContract,udf.contractor as Contractor,udf.qp as Qp, udf.tpi as Tpi,udf.submittedBy as SubmittedBy,udf.subContractorName as SubContractorName,udf.contractorName as ContractorName	,
udf.subContractorMobileNo as SubContractorMobileNo,udf.contractorMobileNo as ContractorMobileNo,udf.subContractDate as SubContractDate,udf.contractDate	 as ContractDate,udf.checkedBy as CheckedBy,
udf.tpiName as TpiName,udf.contractorQaQcName as ContractorQaQcName,udf.tpiMobileNo as TpiMobileNo,udf.contractorQaQcMobileNo as ContractorQaQcMobileNo,udf.tpiContractDate as TpiContractDate,
udf.contractQaQcDate as ContractQaQcDate,udf.galfarToQp as GalfarToQp,udf.dateOfSubmission as DateOfSubmission,udf.QpToGalfar as QpToGalfar,udf.code as Code,udf.dateOfReturn as DateOfReturn,
udf.stageStatus	as StageStatus,udf.documentApprovalStatusType as DocumentApprovalStatusType,udf.finalRemarks as FinalRemarks,udf.galfarTransmittalNumber as GalfarTransmittalNumber,
udf.outgoingIssueCodes as OutgoingIssueCodes,udf.qpDueDate as QpDueDate,udf.technipAttachment1 as TechnipAttachment1,udf.qPTransmittalNumber as QPTransmittalNumber,udf.galfarDueDate as GalfarDueDate,
udf.technipAttachment2 as TechnipAttachment2,udf.documentApprovalStatus as DocumentApprovalStatus,udf.footerDocRefNo as FooterDocRefNo,udf.revision as Revision,lov.Code as NoteStatusCode order by f.Id
 ";
            noteList = await GetApiListCyherAsync<ERP.UI.ViewModel.NoteViewModel>(cypher);
        }
        private async Task TransformRequestForInspectionHalulDocument()
        {
            var _noteBusiness = _serviceProvider.GetService<INoteBusiness>();
            var _lovBusiness = _serviceProvider.GetService<ILOVBusiness>();
            var idss = await _noteBusiness.GetList(x => x.TemplateCode == "HALUL_REQUEST_FOR_INSPECTION");
            var ids = idss.Select(x => x.Id).ToList();
            noteList = noteList.Where(x => !ids.Contains(x.Id.ToString())).ToList();
            if (noteList.Count > 0)
            {
                TargetFolderList = new List<NoteTemplateViewModel>();
                foreach (var folder in noteList)
                {
                    var noteTemp = new NoteTemplateViewModel();
                    noteTemp.TemplateCode = "HALUL_REQUEST_FOR_INSPECTION";
                    var note = await _noteBusiness.GetNoteDetails(noteTemp);

                    note.NoteId = folder.Id.ToString();
                    note.NoteNo = folder.NoteNo;
                    note.NoteSubject = folder.Subject;
                    note.NoteDescription = folder.Description;
                    note.StartDate = folder.StartDate;
                    note.ExpiryDate = note.ExpiryDate;
                    if (folder.OwnerUserId.IsNotNull())
                    {
                        note.OwnerUserId = folder.OwnerUserId.ToString();
                    }
                    else
                    {
                        note.OwnerUserId = "1841";
                    }
                    note.RequestedByUserId = folder.OwnerUserId.ToString();
                    note.Json = "{}";
                    note.DataAction = DataActionEnum.Create;
                    if (folder.ParentId.IsNotNull())
                    {
                        note.ParentNoteId = folder.ParentId.ToString();
                    }
                    note.IsDeleted = folder.IsDeleted == 0 ? false : true;
                    note.Status = folder.Status == ERP.Utility.StatusEnum.Active ? Common.StatusEnum.Active : Common.StatusEnum.Inactive;
                    note.CreatedDate = folder.CreatedDate;
                    note.CreatedBy = folder.CreatedBy.ToString();
                    note.LastUpdatedDate = folder.LastUpdatedDate;
                    note.LastUpdatedBy = folder.LastUpdatedBy.ToString();


                    dynamic exo = new System.Dynamic.ExpandoObject();

                    ((IDictionary<String, Object>)exo).Add("InspectionActivity", folder.InspectionActivity);
                    ((IDictionary<String, Object>)exo).Add("attachment", folder.Attachment);
                    ((IDictionary<String, Object>)exo).Add("projectNo", folder.ProjectNo);
                    ((IDictionary<String, Object>)exo).Add("contractNo", folder.ContractNo);
                    ((IDictionary<String, Object>)exo).Add("date", folder.Date);
                    if (folder.Type.IsNotNullAndNotEmpty())
                    {
                        var lov = await _lovBusiness.GetSingle(x => x.Code == folder.Type && x.LOVType == "RFI_TYPE");
                        if (lov != null)
                        {
                            ((IDictionary<String, Object>)exo).Add("type", lov.Id);
                        }
                    }
                    //((IDictionary<String, Object>)exo).Add("type", folder.Type);
                    if (folder.Discipline.IsNotNullAndNotEmpty())
                    {
                        var lov = await _lovBusiness.GetSingle(x => x.Code == folder.Discipline && x.LOVType == "RFI_HALUL_DISCIPLINE");
                        if (lov != null)
                        {
                            ((IDictionary<String, Object>)exo).Add("discipline", lov.Id);
                        }
                    }
                    //((IDictionary<String, Object>)exo).Add("discipline", folder.Discipline);
                    ((IDictionary<String, Object>)exo).Add("locationOfInspection", folder.LocationOfInspection);
                    if (folder.Revision.IsNotNullAndNotEmpty())
                    {
                        var lov = await _lovBusiness.GetSingle(x => x.Code == folder.Revision && x.LOVType == "GALFAR_REV");
                        if (lov != null)
                        {
                            ((IDictionary<String, Object>)exo).Add("revision", lov.Id);
                        }
                    }
                    //((IDictionary<String, Object>)exo).Add("revision", folder.Revision);
                    ((IDictionary<String, Object>)exo).Add("system", folder.System);
                    ((IDictionary<String, Object>)exo).Add("dateOfInspection", folder.DateOfInspection);
                    ((IDictionary<String, Object>)exo).Add("otherDates", folder.OtherDates);
                    if (folder.Area.IsNotNullAndNotEmpty())
                    {
                        var lov = await _lovBusiness.GetSingle(x => x.Code == folder.Area && x.LOVType == "RFI_HALUL_AREA");
                        if (lov != null)
                        {
                            ((IDictionary<String, Object>)exo).Add("area", lov.Id);
                        }
                    }
                    //((IDictionary<String, Object>)exo).Add("area", folder.Area);
                    ((IDictionary<String, Object>)exo).Add("location", folder.Location);
                    ((IDictionary<String, Object>)exo).Add("drawingNumber", folder.DrawingNumber);
                    ((IDictionary<String, Object>)exo).Add("drawingAttachment", folder.DrawingAttachment);
                    ((IDictionary<String, Object>)exo).Add("tagNumber", folder.TagNumber);
                    ((IDictionary<String, Object>)exo).Add("itpQcpNo", folder.ItpQcpNo);
                    ((IDictionary<String, Object>)exo).Add("itpItemNo", folder.ItpItemNo);
                    ((IDictionary<String, Object>)exo).Add("interventionPoints", folder.InterventionPoints);
                    if (folder.SubContract.IsNotNullAndNotEmpty())
                    {
                        var lov = await _lovBusiness.GetSingle(x => x.Code == folder.SubContract && x.LOVType == "INTERVENTIONPOINT");
                        if (lov != null)
                        {
                            ((IDictionary<String, Object>)exo).Add("subContract", lov.Id);
                        }
                    }
                    //((IDictionary<String, Object>)exo).Add("subContract", folder.SubContract);
                    if (folder.Contractor.IsNotNullAndNotEmpty())
                    {
                        var lov = await _lovBusiness.GetSingle(x => x.Code == folder.Contractor && x.LOVType == "INTERVENTIONPOINT");
                        if (lov != null)
                        {
                            ((IDictionary<String, Object>)exo).Add("contractor", lov.Id);
                        }
                    }
                    //((IDictionary<String, Object>)exo).Add("contractor", folder.Contractor);
                    if (folder.Qp.IsNotNullAndNotEmpty())
                    {
                        var lov = await _lovBusiness.GetSingle(x => x.Code == folder.Qp && x.LOVType == "INTERVENTIONPOINT");
                        if (lov != null)
                        {
                            ((IDictionary<String, Object>)exo).Add("qp", lov.Id);
                        }
                    }
                    //((IDictionary<String, Object>)exo).Add("qp", folder.Qp);
                    if (folder.Tpi.IsNotNullAndNotEmpty())
                    {
                        var lov = await _lovBusiness.GetSingle(x => x.Code == folder.Tpi && x.LOVType == "INTERVENTIONPOINT");
                        if (lov != null)
                        {
                            ((IDictionary<String, Object>)exo).Add("tpi", lov.Id);
                        }
                    }
                 
                    ((IDictionary<String, Object>)exo).Add("submittedBy", folder.SubmittedBy);
                    ((IDictionary<String, Object>)exo).Add("subContractorName", folder.SubContractorName);
                    ((IDictionary<String, Object>)exo).Add("contractorName", folder.ContractorName);
                    ((IDictionary<String, Object>)exo).Add("subContractorMobileNo", folder.SubContractorMobileNo);
                    ((IDictionary<String, Object>)exo).Add("contractorMobileNo", folder.ContractorMobileNo);
                    ((IDictionary<String, Object>)exo).Add("subContractDate", folder.SubContractDate);
                    ((IDictionary<String, Object>)exo).Add("contractDate", folder.ContractDate);
                    ((IDictionary<String, Object>)exo).Add("checkedBy", folder.CheckedBy);
                    ((IDictionary<String, Object>)exo).Add("tpiName", folder.TpiName);
                    ((IDictionary<String, Object>)exo).Add("contractorQaQcName", folder.ContractorQaQcName);
                    ((IDictionary<String, Object>)exo).Add("tpiMobileNo", folder.TpiMobileNo);
                    ((IDictionary<String, Object>)exo).Add("contractorQaQcMobileNo", folder.ContractorQaQcMobileNo);
                    ((IDictionary<String, Object>)exo).Add("tpiContractDate", folder.TpiContractDate);
                    ((IDictionary<String, Object>)exo).Add("contractQaQcDate", folder.ContractQaQcDate);
                    ((IDictionary<String, Object>)exo).Add("galfarToQp", folder.GalfarToQp);
                    ((IDictionary<String, Object>)exo).Add("dateOfSubmission", folder.DateOfSubmission);
                    ((IDictionary<String, Object>)exo).Add("QpToGalfar", folder.QpToGalfar);
                    if (folder.Code.IsNotNullAndNotEmpty())
                    {
                        var lov = await _lovBusiness.GetSingle(x => x.Code == folder.Code && x.LOVType == "RFIFinalStage");
                        if (lov != null)
                        {
                            ((IDictionary<String, Object>)exo).Add("code", lov.Id);
                        }
                    }
                    ((IDictionary<String, Object>)exo).Add("dateOfReturn", folder.DateOfReturn);
                    if (folder.StageStatus.IsNotNullAndNotEmpty())
                    {
                        var lov = await _lovBusiness.GetSingle(x => x.Name == folder.StageStatus && x.LOVType == "PENDINGSTATUS");
                        if (lov != null)
                        {
                            ((IDictionary<String, Object>)exo).Add("stageStatus", lov.Id);
                        }
                    }
                    //((IDictionary<String, Object>)exo).Add("stageStatus", folder.StageStatus);
                    if (folder.DocumentApprovalStatusType.IsNotNull())
                    {
                        var lov = await _lovBusiness.GetSingle(x => x.Code == folder.DocumentApprovalStatusType.ToString() && x.LOVType == "DAST");
                        if (lov != null)
                        {
                            ((IDictionary<String, Object>)exo).Add("documentApprovalStatusType", lov.Id);
                        }
                    }
                    
                    ((IDictionary<String, Object>)exo).Add("finalRemarks", folder.FinalRemarks);
                    ((IDictionary<String, Object>)exo).Add("galfarTransmittalNumber", folder.GalfarTransmittalNumber);
                    ((IDictionary<String, Object>)exo).Add("outgoingIssueCodes", folder.OutgoingIssueCodes);
                    ((IDictionary<String, Object>)exo).Add("qpDueDate", folder.QpDueDate);
                    ((IDictionary<String, Object>)exo).Add("technipAttachment1", folder.TechnipAttachment1);
                    ((IDictionary<String, Object>)exo).Add("qPTransmittalNumber", folder.QPTransmittalNumber);
                    ((IDictionary<String, Object>)exo).Add("galfarDueDate", folder.GalfarDueDate);
                    ((IDictionary<String, Object>)exo).Add("technipAttachment2", folder.TechnipAttachment2);
                    ((IDictionary<String, Object>)exo).Add("documentApprovalStatus", folder.DocumentApprovalStatus);
                    ((IDictionary<String, Object>)exo).Add("footerDocRefNo", folder.FooterDocRefNo);

                    if (folder.WorkspaceId.IsNotNull())
                    {
                        var workspcaeId = await _noteBusiness.GetWorkspaceId(folder.WorkspaceId.ToString());
                        if (workspcaeId != null)
                        {
                            ((IDictionary<String, Object>)exo).Add("WorkspaceId", workspcaeId.Id);
                        }
                    }

                    note.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);

                    if (folder.NoteStatusCode == "ACTIVE")
                    {
                        note.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                    }
                    else if (folder.NoteStatusCode == "DRAFT")
                    {
                        note.NoteStatusCode = "NOTE_STATUS_DRAFT";
                    }
                    else if (folder.NoteStatusCode == "EXPIRED")
                    {
                        note.NoteStatusCode = "NOTE_STATUS_EXPIRE";
                    }

                    if (note.NoteStatusCode.IsNotNullAndNotEmpty())
                    {
                        TargetFolderList.Add(note);
                    }
                }
            }
        }
        private async Task ExtractGeneralDocument()
        {
            // noteList = await GetApiListAsync<NoteViewModel>();
            var cypher = @"match(f:NTS_Note{IsDeleted:0})
                where (f.IsArchived <>true or f.IsArchived is null)                
                match(f)-[:R_Note_Template]->(t: NTS_Template{ IsDeleted: 0,Status: 'Active'})
                -[:R_TemplateRoot]->(tm: NTS_TemplateMaster{ IsDeleted: 0,Status: 'Active',Code:'GENERAL_DOCUMENT'})  
                -[:R_TemplateMaster_TemplateCategory]->(tc: NTS_TemplateCategory{IsDeleted:0,Code:'GENERAL_DOCUMENT'})
                match(f)-[:R_Note_Owner_User]->(u: ADM_User)
                match(f)-[:R_Note_Status_ListOfValue]->(lov: GEN_ListOfValue)
                optional match (f)<-[:R_NoteFieldValue_Note]-(nfv: NTS_NoteFieldValue{ IsDeleted: 0})
                -[:R_NoteFieldValue_TemplateField]->(ttf:NTS_TemplateField)
                with u,f,t,tm,tc,lov,apoc.map.fromPairs(COLLECT([ttf.FieldName,nfv.Code])) as udf 
                optional match(f)-[:R_Note_Parent_Note]->(pn:NTS_Note{IsDeleted: 0,Status: 'Active'}) 
                optional match(f)-[:R_Note_Workspace_Note]->(ws:NTS_Note{IsDeleted: 0,Status: 'Active'})
                return distinct f,pn.Id as ParentId,ws.Id as WorkspaceId,u.Id as OwnerUserId,udf.documentApprovalStatusType as DocumentApprovalStatusType,udf.documentApprovalStatus as DocumentApprovalStatus,
udf.documentStatus as DocumentStatus,udf.lastCheckOutBy as LastCheckOutBy,udf.lastCheckOutDate as LastCheckOutDate,udf.attachment as Attachment,udf.qpReferenceNo as QpReferenceNo,lov.Code as NoteStatusCode order by f.Id	
 ";
            noteList = await GetApiListCyherAsync<ERP.UI.ViewModel.NoteViewModel>(cypher);
        }
        private async Task TransformGeneralDocument()
        {
            var _noteBusiness = _serviceProvider.GetService<INoteBusiness>();
            var _lovBusiness = _serviceProvider.GetService<ILOVBusiness>();
            var idss = await _noteBusiness.GetList(x => x.TemplateCode == "GENERAL_DOCUMENT");
            var ids = idss.Select(x => x.Id).ToList();
            noteList = noteList.Where(x => !ids.Contains(x.Id.ToString())).ToList();
            if (noteList.Count > 0)
            {
                TargetFolderList = new List<NoteTemplateViewModel>();
                foreach (var folder in noteList)
                {
                    var noteTemp = new NoteTemplateViewModel();
                    noteTemp.TemplateCode = "GENERAL_DOCUMENT";
                    var note = await _noteBusiness.GetNoteDetails(noteTemp);

                    note.NoteId = folder.Id.ToString();
                    note.NoteNo = folder.NoteNo;
                    note.NoteSubject = folder.Subject;
                    note.NoteDescription = folder.Description;

                    note.StartDate = folder.StartDate;
                    note.ExpiryDate = note.ExpiryDate;
                    if (folder.OwnerUserId.IsNotNull())
                    {
                        note.OwnerUserId = folder.OwnerUserId.ToString();
                    }
                    else
                    {
                        note.OwnerUserId = "1841";
                    }
                    note.Json = "{}";
                    note.DataAction = DataActionEnum.Create;
                    if (folder.ParentId.IsNotNull())
                    {
                        note.ParentNoteId = folder.ParentId.ToString();
                    }
                    note.IsDeleted = folder.IsDeleted == 0 ? false : true;
                    note.Status = folder.Status == ERP.Utility.StatusEnum.Active ? Common.StatusEnum.Active : Common.StatusEnum.Inactive;
                    note.CreatedDate = folder.CreatedDate;
                    note.CreatedBy = folder.CreatedBy.ToString();
                    note.LastUpdatedDate = folder.LastUpdatedDate;
                    note.LastUpdatedBy = folder.LastUpdatedBy.ToString();


                    dynamic exo = new System.Dynamic.ExpandoObject();

                    if (folder.DocumentApprovalStatusType.IsNotNull())
                    {
                        var lov = await _lovBusiness.GetSingle(x => x.Code == folder.DocumentApprovalStatusType.ToString() && x.LOVType == "DAST");
                        if (lov != null)
                        {
                            ((IDictionary<String, Object>)exo).Add("documentApprovalStatusType", lov.Id);
                        }
                    }
                    //((IDictionary<String, Object>)exo).Add("documentApprovalStatusType", folder.DocumentApprovalStatusType);
                    ((IDictionary<String, Object>)exo).Add("documentApprovalStatus", folder.DocumentApprovalStatus);
                    ((IDictionary<String, Object>)exo).Add("documentStatus", folder.DocumentStatus);
                    ((IDictionary<String, Object>)exo).Add("lastCheckOutBy", folder.LastCheckOutBy);
                    ((IDictionary<String, Object>)exo).Add("lastCheckOutDate", folder.LastCheckOutDate);
                    ((IDictionary<String, Object>)exo).Add("attachment", folder.Attachment);
                    ((IDictionary<String, Object>)exo).Add("qpReferenceNo", folder.QpReferenceNo);
                    if (folder.WorkspaceId.IsNotNull())
                    {
                        var workspcaeId = await _noteBusiness.GetWorkspaceId(folder.WorkspaceId.ToString());
                        if (workspcaeId != null)
                        {
                            ((IDictionary<String, Object>)exo).Add("WorkspaceId", workspcaeId.Id);
                        }
                    }

                    note.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);

                    if (folder.NoteStatusCode == "ACTIVE")
                    {
                        note.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                    }
                    else if (folder.NoteStatusCode == "DRAFT")
                    {
                        note.NoteStatusCode = "NOTE_STATUS_DRAFT";
                    }
                    else if (folder.NoteStatusCode == "EXPIRED")
                    {
                        note.NoteStatusCode = "NOTE_STATUS_EXPIRE";
                    }

                    if (note.NoteStatusCode.IsNotNullAndNotEmpty())
                    {
                        TargetFolderList.Add(note);
                    }
                }
            }
        }
        private async Task ExtractUser()
        {
            SourceUserList = await GetApiListAsync<ADM_User>();
        }
        private async Task TransformUser()
        {
            //throw new NotImplementedException();
            if (SourceUserList.Count > 0)
            {
                TargetUserList = new List<User>();
                foreach (var su in SourceUserList)
                {
                    var tu = new User
                    {
                        Id = su.Id.ToString(),
                        UserId = su.IqamahNo,
                        Name = su.UserName,
                        Email = su.Email,
                        JobTitle = su.JobTitle,
                        Password = Helper.Encrypt(su.Password),                       
                        //PhotoId=,
                        Mobile = su.MobileNo,
                        //ForgotPasswordOTP=,
                        PasswordChanged = true,
                        IsSystemAdmin = su.IsAdmin,
                        //IsGuestUser=,
                        //UserRoles=,
                        //UserPermissions=,
                        //SignatureId=,
                        EnableRegularEmail = su.EnableRegularEmail,
                        EnableSummaryEmail = su.EnableEmailSummary,
                        //LineManagerId = ,
                        //ActivationCode=,
                        //LegalEntityIds=,
                        IsDeleted = su.IsDeleted == 0 ? false : true,
                        Status = su.Status == ERP.Utility.StatusEnum.Active ? Common.StatusEnum.Active : Common.StatusEnum.Inactive,
                        CreatedDate = su.CreatedDate,
                        CreatedBy = su.CreatedBy.ToString(),
                        LastUpdatedDate = su.LastUpdatedDate,
                        LastUpdatedBy = su.LastUpdatedBy.ToString()

                    };
                    TargetUserList.Add(tu);
                }
            }
        }
        private async Task LoadUser()
        {
            var _userBusiness = _serviceProvider.GetService<IUserBusiness>();
            //throw new NotImplementedException();
            if (TargetUserList.Count > 0)
            {
                foreach (var tu in TargetUserList)
                {
                    try
                    {
                        var user = _autoMapper.Map<User, UserViewModel>(tu);
                        var isUser = await _userBusiness.GetSingle(x => x.Id == tu.Id && x.Email == tu.Email);
                        if (isUser == null)
                        {
                            user.ConfirmPassword = user.Password;
                            await _userBusiness.Create(user);
                        }
                        else
                        {
                            user.ConfirmPassword = user.Password;
                            await _userBusiness.Edit(user);
                        }
                    }
                    catch(Exception ex)
                    {

                    }
                }
            }
        }
        private async Task ExtractTeam()
        {
            SourceTeamList = await GetApiListAsync<ADM_Team>();
        }
        private async Task TransformTeam()
        {
            var cypher = @"MATCH (t:ADM_Team)-[r:R_Team_User]->(u:ADM_User) return  t,u.Id as UserId,r.IsTeamOwner as IsTeamOwner";
            var teammember = await GetApiListCyherAsync<ERP.UI.ViewModel.TeamViewModel>(cypher);
            if (SourceTeamList.Count > 0)
            {
                TargetTeamList = new List<TeamViewModel>();
                
                foreach (var s in SourceTeamList)
                {
                    var member = teammember.Where(x => x.Id == s.Id).ToList();
                    var t = new TeamViewModel
                    {
                        Id = s.Id.ToString(),
                        Name = s.Name,
                        Description = s.Description,
                        Code = s.Code,
                        //LogoId =,
                        GroupCode = s.GroupCode,
                        TeamWorkAssignmentType = (WorkAssignmentTypeEnum)s.TeamWorkAssignmentType,
                        TeamType = s.TeamType != null ? (TeamTypeEnum)s.TeamType : TeamTypeEnum.General,
                        IsDeleted = s.IsDeleted == 0 ? false : true,
                        Status = s.Status == ERP.Utility.StatusEnum.Active ? Common.StatusEnum.Active : Common.StatusEnum.Inactive,
                        CreatedDate = s.CreatedDate,
                        CreatedBy = s.CreatedBy.ToString(),
                        LastUpdatedDate = s.LastUpdatedDate,
                        LastUpdatedBy = s.LastUpdatedBy.ToString()

                    };
                    if(member.Count>0)
                    {
                        t.UserIds = member.Select(x => x.UserId.ToString()).ToList();
                        var owner = member.Where(x => x.IsTeamOwner == true).Select(x => x.UserId.ToString()).FirstOrDefault();
                        t.TeamOwnerId = owner;
                    }
                    TargetTeamList.Add(t);
                }
            }
        }
        private async Task LoadTeam()
        {
            var _teamBusiness = _serviceProvider.GetService<ITeamBusiness>();
            if (TargetTeamList.Count > 0)
            {
                foreach (var datavm in TargetTeamList)
                {
                    //var datavm = _autoMapper.Map<Team, TeamViewModel>(t);
                    var isData = await _teamBusiness.GetSingle(x => x.Id == datavm.Id && x.Code == datavm.Code);
                    if (isData == null)
                    {
                        await _teamBusiness.Create(datavm);
                    }
                    else
                    {
                        await _teamBusiness.Edit(datavm);
                    }
                }
            }
        }
        private async Task ExtractLOV()
        {
            SourceLOVList = await GetApiListAsync<GEN_ListOfValue>();
            var cypher = @"match(lov:GEN_ListOfValue)
                        optional match(lov)-[:R_ListOfValue_ListOfValueType]->(t:GEN_ListOfValue)
                        optional match(lov)-[:R_ListOfValue_Parent]->(p)
                        return lov,t.Code as LOVTypeCode,p.Id as ParentId";
            SVMLOVList = await GetApiListCyherAsync<ERP.UI.ViewModel.ListOfValueViewModel>(cypher);
        }
        private async Task TransformLOV()
        {
            if (SourceLOVList.Count > 0)
            {
                TargetLOVList = new List<LOV>();
                foreach (var s in SourceLOVList)
                {
                    var svm = SVMLOVList.Where(x => x.Id == s.Id).FirstOrDefault();
                    var lovtype = string.Empty;
                    var parentid = string.Empty;
                    if (svm.IsNotNull())
                    {
                        lovtype = svm.LOVTypeCode.IsNotNullAndNotEmpty() ? svm.LOVTypeCode : null;
                        parentid = svm.ParentId.IsNotNull() ? svm.ParentId.ToString() : null;
                    }
                    var t = new LOV
                    {
                        Id = s.Id.ToString(),
                        LOVType = lovtype,
                        Name = s.Name,
                        Code = s.Code,
                        GroupCode = s.Groupcode,
                        ParentId = parentid,
                        //ImageId =,
                        //IconCss =,
                        Description = s.Description,
                        SequenceOrder = (long?)s.SequenceNo,
                        IsDeleted = s.IsDeleted == 0 ? false : true,
                        Status = s.Status == ERP.Utility.StatusEnum.Active ? Common.StatusEnum.Active : Common.StatusEnum.Inactive,
                        CreatedDate = s.CreatedDate,
                        CreatedBy = s.CreatedBy.ToString(),
                        LastUpdatedDate = s.LastUpdatedDate,
                        LastUpdatedBy = s.LastUpdatedBy.ToString()

                    };
                    TargetLOVList.Add(t);
                }
            }
        }
        private async Task LoadLOV()
        {
            //throw new NotImplementedException();
            var _lovBusiness = _serviceProvider.GetService<ILOVBusiness>();
            if (TargetLOVList.Count > 0)
            {
                foreach (var t in TargetLOVList)
                {
                    var datavm = _autoMapper.Map<LOV, LOVViewModel>(t);
                    var isData = await _lovBusiness.GetSingle(x => x.Id == datavm.Id && x.Code == datavm.Code);
                    if (isData == null)
                    {
                        await _lovBusiness.Create(datavm);
                    }
                    else
                    {
                        await _lovBusiness.Edit(datavm);
                    }
                }
            }
        }
        private async Task ExtractFile()
        {
            //SourceFileList = await GetApiListAsync<GEN_File>();
            var cypher = @"match(f:GEN_File) where f.Id>90000 and f.Id<=100000 and f.IsDeleted=0 return f order by f.Id";
            SVMFileList = await GetApiListCyherAsync<ERP.UI.ViewModel.FileViewModel>(cypher);
        }
        private async Task TransformFile()
        {
            //SourceFileList.Count > 0
            if (SVMFileList.Count > 0)
            {
                TargetFileList = new List<File>();
                foreach (var s in SVMFileList)
                {
                    var t = new File
                    {
                        Id = s.Id.ToString(),
                        FileName = s.FileName,
                        FileExtension = s.FileExtension,
                        ContentType = s.ContentType,
                        ContentLength = s.ContentLength,
                        ContentBase64 = s.ContentBase64,
                        //=s.ThumbNailBase64,
                        //SnapshotMongoId=,
                        Path = s.Path,
                        AttachmentType = s.AttachmentType.ToString(),
                        LinkId = s.LinkId.ToString(),
                        //=s.IsInPhysicalPath,
                        AttachmentDescription = s.AttachmentDescription,
                        IsFileViewableFormat = s.IsFileViewableFormat,
                        MongoFileId = s.MongoFileId,
                        AnnotationsText = s.AnnotationsText,
                        //FileExtractedText = s.FileExtractedText,
                        //ReferenceTypeCode=,
                        //ReferenceTypeId=,

                        IsDeleted = s.IsDeleted == 0 ? false : true,
                        Status = s.Status == ERP.Utility.StatusEnum.Active ? Common.StatusEnum.Active : Common.StatusEnum.Inactive,
                        CreatedDate = s.CreatedDate,
                        CreatedBy = s.CreatedBy.ToString(),
                        LastUpdatedDate = s.LastUpdatedDate,
                        LastUpdatedBy = s.LastUpdatedBy.ToString()

                    };
                    TargetFileList.Add(t);
                }
            }
        }
        private async Task LoadFile()
        {
            var _fileBusiness = _serviceProvider.GetService<IFileBusiness>();
            if (TargetFileList.Count > 0)
            {
                foreach (var t in TargetFileList)
                {
                    var datavm = _autoMapper.Map<File, FileViewModel>(t);
                    var isData = await _fileBusiness.GetSingle(x => x.Id == datavm.Id);
                    if (isData == null)
                    {
                        await _fileBusiness.Create(datavm);
                    }
                    else
                    {
                        await _fileBusiness.Edit(datavm);
                    }
                }
            }
        }
        private async Task ExtractUserRole()
        {
            SourceRoleList = await GetApiListAsync<ADM_UserRole>();
        }
        private async Task TransformUserRole()
        {
            var cypher = @"MATCH (t:ADM_UserRole)<-[r:R_User_UserRole]-(u:ADM_User) return  t.Id as UserRoleId,u.Id as UserId";
            var teammember = await GetApiListCyherAsync<ERP.UI.ViewModel.UserViewModel>(cypher);
            if (SourceRoleList.Count > 0)
            {
                TargetRoleList = new List<UserRoleViewModel>();
                foreach (var s in SourceRoleList)
                {
                    var member = teammember.Where(x => x.UserRoleId == s.Id).Select(x=>x.UserId.ToString()).ToList();
                    var t = new UserRoleViewModel
                    {
                        Id = s.Id.ToString(),
                        Name = s.Name,
                        Code = s.Code,
                        //=s.Description,
                        //Users=,
                        //UserRolePermissions=,

                        IsDeleted = s.IsDeleted == 0 ? false : true,
                        Status = s.Status == ERP.Utility.StatusEnum.Active ? Common.StatusEnum.Active : Common.StatusEnum.Inactive,
                        CreatedDate = s.CreatedDate,
                        CreatedBy = s.CreatedBy.ToString(),
                        LastUpdatedDate = s.LastUpdatedDate,
                        LastUpdatedBy = s.LastUpdatedBy.ToString(),
                        UserIds = member
                    };
                    TargetRoleList.Add(t);
                }
            }
        }
        private async Task LoadUserRole()
        {
            var _roleBusiness = _serviceProvider.GetService<IUserRoleBusiness>();
            if (TargetRoleList.Count > 0)
            {
                foreach (var datavm in TargetRoleList)
                {
                    //var datavm = _autoMapper.Map<UserRole, UserRoleViewModel>(t);
                    var isData = await _roleBusiness.GetSingle(x => x.Id == datavm.Id && x.Code == datavm.Code);
                    if (isData == null)
                    {
                        await _roleBusiness.Create(datavm);
                    }
                    else
                    {
                        await _roleBusiness.Edit(datavm);
                    }
                }
            }
        }

        private async Task ExtractUserGroup()
        {
            SourceUserGroupList = await GetApiListAsync<ADM_WorkspacePermissionGroup>();

        }
        private async Task TransformUserGroup()
        {            
            if (SourceUserGroupList.Count > 0)
            {
                TargetUserGroupList = new List<UserGroup>();
                foreach (var s in SourceUserGroupList)
                {
                    var t = new UserGroup
                    {
                        Id = s.Id.ToString(),
                        Name = s.Name,
                        Code = s.Name,
                        Description = s.Description,

                        IsDeleted = s.IsDeleted == 0 ? false : true,
                        Status = s.Status == ERP.Utility.StatusEnum.Active ? Common.StatusEnum.Active : Common.StatusEnum.Inactive,
                        CreatedDate = s.CreatedDate,
                        CreatedBy = s.CreatedBy.ToString(),
                        LastUpdatedDate = s.LastUpdatedDate,
                        LastUpdatedBy = s.LastUpdatedBy.ToString()

                    };
                    TargetUserGroupList.Add(t);
                }
            }

        }
        private async Task LoadUserGroup()
        {
            var _groupBusiness = _serviceProvider.GetService<IUserGroupBusiness>();
            if (TargetUserGroupList.Count > 0)
            {
                foreach (var t in TargetUserGroupList)
                {
                    var datavm = _autoMapper.Map<UserGroup, UserGroupViewModel>(t);
                    var cypher = @"match(g:ADM_WorkspacePermissionGroup{Id:" + datavm.Id + "})<-[:R_User_UserPermissionGroup]-(u:ADM_User) return u";
                    var SVMUserList = await GetApiListCyherAsync<ERP.UI.ViewModel.UserViewModel>(cypher);
                    if (SVMUserList != null && SVMUserList.Count() > 0)
                    {
                        var users = SVMUserList.Select(x => x.Id).ToList();
                        var userIdList = new List<string>();
                        foreach (var u in users)
                        {
                            userIdList.Add(u.ToString());
                        }
                        if (userIdList.Count > 0)
                        {
                            datavm.UserIds = userIdList;
                        }
                    }
                    var isData = await _groupBusiness.GetSingle(x => x.Id == datavm.Id && x.Code == datavm.Code);
                    if (isData == null)
                    {
                        await _groupBusiness.Create(datavm);
                    }
                    else
                    {
                        await _groupBusiness.Edit(datavm);
                    }
                }
            }
        }
        private async Task ExtractPermission()
        {
            //           var cypher1 = @"match(np:NTS_NotePermission) 
            //                           match(np)-[npn:R_NotePermission_Note]->(n:NTS_Note{IsDeleted:0})
            //match(n)-[:R_Note_Template]->(t: NTS_Template{ IsDeleted: 0,Status: 'Active'})
            //               -[:R_TemplateRoot]->(tm: NTS_TemplateMaster{ IsDeleted: 0,Status: 'Active'})  where tm.Code in ['WORKSPACE_GENERAL','GENERAL_FOLDER','PUBLIC_FOLDER','LEGALENTITY_FOLDER']
            //                           return distinct np";
            //           SourcePermissionList = await GetApiListCyherAsync<ERP.UI.ViewModel.NotePermissionViewModel>(cypher1);
            var cypher = @"match(np:NTS_NotePermission)
                           match(np)-[npn:R_NotePermission_Note]->(n:NTS_Note{IsDeleted:0})
match(n)-[:R_Note_Template]->(t: NTS_Template{ IsDeleted: 0,Status: 'Active'})
                -[:R_TemplateRoot]->(tm: NTS_TemplateMaster{ IsDeleted: 0,Status: 'Active'})  where tm.Code in ['GALFAR_VENDOR']
                            optional match(np)-[:R_NotePermission_WorkspacePermissionGroup]->(g:ADM_WorkspacePermissionGroup)
                            optional match(np)-[:R_NotePermission_User]->(u:ADM_User)
                            return np, n.Id as NoteId, npn.IsInherited as IsInherited, g.Id as WorkspacePermissionGroupId, u.Id as UserId";
            SVMPermissionList = await GetApiListCyherAsync<ERP.UI.ViewModel.NotePermissionViewModel>(cypher);
        }
        private async Task TransformPermission()
        {
            var _permissionBusiness = _serviceProvider.GetService<IDocumentPermissionBusiness>();
            var idss = await _permissionBusiness.GetList();
            var ids = idss.Select(x => x.Id).ToList();
           // SourcePermissionList = SourcePermissionList.Where(x => !ids.Contains(x.Id.ToString())).ToList();
            
            if (SVMPermissionList.Count > 0)
            {
                TargetPermissionList = new List<DocumentPermission>();
                foreach (var svm in SVMPermissionList)
                {
                    var noteid = string.Empty;
                    var userid = string.Empty;
                    var groupid = string.Empty;
                    bool isinherited = false;
                    //var svm = SVMPermissionList.Where(x => x.Id == s.Id).FirstOrDefault();
                    if (svm != null)
                    {
                        noteid = svm.NoteId > 0 ? svm.NoteId.ToString() : null;
                        userid = svm.UserId.HasValue ? svm.UserId.ToString() : null;
                        groupid = svm.WorkspacePermissionGroupId.HasValue ? svm.WorkspacePermissionGroupId.ToString() : null;
                        isinherited = svm.IsInherited.HasValue ? (svm.IsInherited.IsTrue() ? true : false) : false;

                        var t = new DocumentPermission
                        {
                            //Id = svm.Id.ToString(),
                            PermissionType = (DmsPermissionTypeEnum)svm.PermissionType,
                            Access = (DmsAccessEnum)svm.Access,
                            AppliesTo = (DmsAppliesToEnum)svm.AppliesTo,
                            NoteId = noteid,
                            PermittedUserId = userid,
                            PermittedUserGroupId = groupid,
                            InheritedFrom = svm.InheritedFrom,
                            IsInherited = isinherited,
                            Isowner = svm.Iswoner.HasValue ? (svm.Iswoner.IsTrue() ? true : false) : false,

                            IsDeleted = svm.IsDeleted == 0 ? false : true,
                            Status = svm.Status == ERP.Utility.StatusEnum.Active ? Common.StatusEnum.Active : Common.StatusEnum.Inactive,
                            CreatedDate = svm.CreatedDate,
                            CreatedBy = svm.CreatedBy.ToString(),
                            LastUpdatedDate = svm.LastUpdatedDate,
                            LastUpdatedBy = svm.LastUpdatedBy.ToString()

                        };
                        TargetPermissionList.Add(t);
                    }
                }
            }
        }
        private async Task LoadPermission()
        {
            var _docPermissionBusiness = _serviceProvider.GetService<IDocumentPermissionBusiness>();
            if (TargetPermissionList.Count > 0)
            {
                foreach (var t in TargetPermissionList)
                {
                    try
                    {
                        var datavm = _autoMapper.Map<DocumentPermission, DocumentPermissionViewModel>(t);
                        await _docPermissionBusiness.Create(datavm);
                        //var isData = await _docPermissionBusiness.GetSingle(x => x.Id == datavm.Id);
                        //if (isData == null)
                        //{
                        //    await _docPermissionBusiness.Create(datavm);
                        //}
                        //else
                        //{
                        //    await _docPermissionBusiness.Edit(datavm);
                        //}
                    }
                    catch(Exception ex)
                    {

                    }
                }
            }
        }
        public async Task<T> GetApiAsync<T>()
        {
            using (var client = new HttpClient())
            {
                var address = new Uri($"{WebApiUrl}api/getlist?type={nameof(T)}");
                var response = await client.GetAsync(address);
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(content);
            }
        }
        public async Task<List<T>> GetApiListAsync<T>()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var address = new Uri($"{WebApiUrl}api/getlist?type={typeof(T).Name}");
                    var response = await client.GetAsync(address);
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<T>>(content);
                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        public async Task<List<T>> GetApiListCyherAsync<T>(string cypher)
        {
            try
            {
                var myContent = JsonConvert.SerializeObject(new { Type = typeof(T).Name, Cyhper = cypher });
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                using (var client = new HttpClient())
                {
                    var address = new Uri($"{WebApiUrl}api/GetListCypher");
                    var response = await client.PostAsync(address, byteContent);
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<T>>(content);
                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }
    }
}
