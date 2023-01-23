using CMS.Business.BusinessScript.Task.General.DocumentManagement;
using CMS.Common;
using CMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Business.Implementations.BusinessScript.Task.General.DocumentManagement
{
    public class TaskGeneralDMSPostScript : ITaskGeneralDMSPostScript
    {
        /// <summary>
        /// AddTagging - it will tag task to relevent goal and compentency
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="udf"></param>
        /// <param name="uc"></param>
        /// <param name="sp"></param>
        /// <returns></returns>
      
      
        public async Task<CommandResult<TaskTemplateViewModel>> OverwriteUdfs(TaskTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _udfPermissionBusiness = sp.GetService<IUdfPermissionBusiness>();
            var _columnMetadataBusiness = sp.GetService<IColumnMetadataBusiness>();
            var _serviceBusiness = sp.GetService<IServiceBusiness>();
            var _noteBusiness = sp.GetService<INoteBusiness>();
            var _lovBusiness = sp.GetService<ILOVBusiness>();
            var _tableMetadataBusiness = sp.GetService<ITableMetadataBusiness>();
            var rowData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(viewModel.Json);
            var udfpermissions = await _udfPermissionBusiness.GetList(x => x.TemplateId == viewModel.TemplateId);
            var serviceDetails = await _serviceBusiness.GetSingleById(viewModel.ParentServiceId);
            var documentId = string.Empty;
            var service = await _serviceBusiness.GetServiceDetails(new ServiceTemplateViewModel
            {
                TemplateCode = serviceDetails.TemplateCode,
                ServiceId = serviceDetails.Id,
                SetUdfValue = true,
            });
            if (service.IsNotNull())
            {
                var documentIdCol = service.ColumnList.Where(x => x.Name == "documentId").FirstOrDefault();
                documentId = documentIdCol.Value.ToString();
            }
            
            var columnlist = new Dictionary<string, object>();
            foreach(var item in udfpermissions)
            {
                if (item.EditableBy.Length > 0)
                {
                    var column = await _columnMetadataBusiness.GetSingle(x=>x.Id==item.ColumnMetadataId &&  x.TableMetadataId == viewModel.UdfTableMetadataId);
                    if (column.IsNotNull())
                    {
                        bool exist = columnlist.ContainsKey(column.Alias);
                        var udfColumnValue = Convert.ToString(rowData[column.Alias]);
                        if (udfColumnValue.IsNotNullAndNotEmpty() && !exist)
                        {
                            columnlist.Add(column.Alias, udfColumnValue);

                        }

                    }
                   
                   
                }
            }
            if (documentId.IsNotNullAndNotEmpty())
            {
                var noteDetails = await _noteBusiness.GetSingleById(documentId);
                var noteTempModel = new NoteTemplateViewModel();
                noteTempModel.TemplateCode = noteDetails.TemplateCode;
                noteTempModel.NoteId = noteDetails.Id;
                noteTempModel.SetUdfValue = true;
                var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                var rowData1 = notemodel.ColumnList.ToDictionary(x => x.Name, x => x.Value);
                var attachmentKey = "attachment";
                if (notemodel.TemplateCode == "ENGINEERING_SUBCONTRACT")
                {
                    attachmentKey = "fileAttachment";
                }
                foreach(var x in columnlist)
                {
                    if (x.Key=="File" && x.Value.ToString() != "[]")
                    {
                        rowData1[attachmentKey] = Convert.ToString(x.Value);
                    }
                  
                    else if (rowData1.ContainsKey(x.Key))
                    {
                        rowData1[x.Key] = Convert.ToString(x.Value);
                    }
                    
                }
                

                if (viewModel.TemplateCode == "GALFAR_VENDOR_STEP_TASK4" || viewModel.TemplateCode == "PROJECT_DOC_TASK4" || viewModel.TemplateCode == "ENGINEERING_SUBCONTRACT_STEP_TASK4" )
                {
                    var signedAttachment = rowData.GetValueOrDefault("signedAfcAttachment").ToString();
                    var revisionValue = rowData1.GetValueOrDefault("revision");
                    var codeValue = rowData.GetValueOrDefault("code");
                    var revision = await _lovBusiness.GetSingleById(revisionValue.ToString());
                    var code = await _lovBusiness.GetSingleById(codeValue.ToString());
                  
                    if (signedAttachment.IsNotNullAndNotEmpty() && signedAttachment != "[]" && revision.IsNotNull() && code.IsNotNull() && code.Code=="AFC" && (revision.Code == "Rev-A" || revision.Code == "Rev-B" || revision.Code == "Rev-C" || revision.Code == "Rev-D" || revision.Code == "Rev-E" || revision.Code == "Rev-F" || revision.Code == "Rev-G"))
                    {
                        rowData1[attachmentKey] = Convert.ToString(signedAttachment);
                    }
                   
                }
                if (viewModel.TemplateCode == "INSPECTION_SER_TASK3" || viewModel.TemplateCode == "INSPECTION_SER_TASK3_HALUL")
                {
                    var signedAttachment = rowData.GetValueOrDefault("signedAfcAttachment").ToString();
                    rowData1[attachmentKey] = Convert.ToString(signedAttachment);
                }
                var currentStageStatus= rowData1.GetValueOrDefault("stageStatus").ToString();
                var stageStatusId = await GetStageStatus(viewModel.TemplateCode, rowData, currentStageStatus, sp);
                if (stageStatusId.IsNotNullAndNotEmpty())
                {
                    rowData1["stageStatus"] = stageStatusId;
                }
               

                var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData1);
                var update = await _noteBusiness.EditNoteUdfTable(notemodel, data1, notemodel.UdfNoteTableId);
            }
            return CommandResult<TaskTemplateViewModel>.Instance(viewModel);
        }

        public async Task<CommandResult<TaskTemplateViewModel>> CopySameNoteNoUdfs(TaskTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _udfPermissionBusiness = sp.GetService<IUdfPermissionBusiness>();
            var _columnMetadataBusiness = sp.GetService<IColumnMetadataBusiness>();
            var _serviceBusiness = sp.GetService<IServiceBusiness>();
            var _noteBusiness = sp.GetService<INoteBusiness>();
            var _lovBusiness = sp.GetService<ILOVBusiness>();
            var _tableMetadataBusiness = sp.GetService<ITableMetadataBusiness>();
            var rowData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(viewModel.Json);
            var udfpermissions = await _udfPermissionBusiness.GetList(x => x.TemplateId == viewModel.TemplateId);
            var serviceDetails = await _serviceBusiness.GetSingleById(viewModel.ParentServiceId);
            var documentId = string.Empty;
            var service = await _serviceBusiness.GetServiceDetails(new ServiceTemplateViewModel
            {
                TemplateCode = serviceDetails.TemplateCode,
                ServiceId = serviceDetails.Id,
                SetUdfValue = true,
            });
            if (service.IsNotNull())
            {
                var documentIdCol = service.ColumnList.Where(x => x.Name == "documentId").FirstOrDefault();
                documentId = documentIdCol.Value.ToString();
            }

            var columnlist = new Dictionary<string, object>();
            foreach (var item in udfpermissions)
            {
                if (item.EditableBy.Length > 0)
                {
                    var column = await _columnMetadataBusiness.GetSingle(x => x.Id == item.ColumnMetadataId && x.TableMetadataId == viewModel.UdfTableMetadataId);
                    if (column.IsNotNull())
                    {
                        bool exist = columnlist.ContainsKey(column.Alias);
                        var udfColumnValue = Convert.ToString(rowData[column.Alias]);
                        if (udfColumnValue.IsNotNullAndNotEmpty() && !exist)
                        {
                            columnlist.Add(column.Alias, udfColumnValue);

                        }

                    }


                }
            }
            if (documentId.IsNotNullAndNotEmpty())
            {
                var noteDetails = await _noteBusiness.GetSingleById(documentId);
                var noteTempModel = new NoteTemplateViewModel();
                noteTempModel.TemplateCode = noteDetails.TemplateCode;
                noteTempModel.NoteId = noteDetails.Id;
                noteTempModel.SetUdfValue = true;
                var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                var rowData1 = notemodel.ColumnList.ToDictionary(x => x.Name, x => x.Value);
                var sameNoteNo = await _noteBusiness.GetList(x => x.NoteNo == notemodel.NoteNo);
                var attachmentKey = "attachment";
                if (notemodel.TemplateCode == "ENGINEERING_SUBCONTRACT")
                {
                    attachmentKey = "fileAttachment";
                }
                foreach (var item in sameNoteNo)
                {
                    var revision = rowData1.GetValueOrDefault("revision").ToString();
                    var noteRevision = await _tableMetadataBusiness.GetTableDataByColumn(item.TemplateCode, "", "NtsNoteId", item.Id);
                    if (noteRevision.IsNotNull() && noteRevision["revision"].ToString() == revision)
                    {
                        var noteTempModel1 = new NoteTemplateViewModel();
                        noteTempModel1.TemplateCode = item.TemplateCode;
                        noteTempModel1.NoteId = item.Id;
                        noteTempModel1.SetUdfValue = true;
                        var notemodel1 = await _noteBusiness.GetNoteDetails(noteTempModel1);
                        var rowData2 = notemodel1.ColumnList.ToDictionary(x => x.Name, x => x.Value);
                        var attachmentKey1 = "attachment";
                        if (notemodel1.TemplateCode == "ENGINEERING_SUBCONTRACT")
                        {
                            attachmentKey1 = "fileAttachment";
                        }
                        foreach (var x in columnlist)
                        {
                            if (x.Key == "File" && x.Value.ToString() != "[]")
                            {
                                rowData2[attachmentKey1] = Convert.ToString(x.Value);
                            }

                            else if (rowData2.ContainsKey(x.Key))
                            {
                                rowData2[x.Key] = Convert.ToString(x.Value);
                            }

                        }


                        if (viewModel.TemplateCode == "GALFAR_VENDOR_STEP_TASK4" || viewModel.TemplateCode == "PROJECT_DOC_TASK4" || viewModel.TemplateCode == "ENGINEERING_SUBCONTRACT_STEP_TASK4")
                        {
                            var signedAttachment = rowData.GetValueOrDefault("signedAfcAttachment").ToString();
                            var revisionValue = rowData2.GetValueOrDefault("revision");
                            var codeValue = rowData.GetValueOrDefault("code");
                            var revision1 = await _lovBusiness.GetSingleById(revisionValue.ToString());
                            var code = await _lovBusiness.GetSingleById(codeValue.ToString());

                            if (signedAttachment.IsNotNullAndNotEmpty() && signedAttachment != "[]" && revision1.IsNotNull() && code.IsNotNull() && code.Code == "AFC" && (revision1.Code == "Rev-A" || revision1.Code == "Rev-B" || revision1.Code == "Rev-C" || revision1.Code == "Rev-D" || revision1.Code == "Rev-E" || revision1.Code == "Rev-F" || revision1.Code == "Rev-G"))
                            {
                                rowData2[attachmentKey] = Convert.ToString(signedAttachment);
                            }

                        }
                        if (viewModel.TemplateCode == "INSPECTION_SER_TASK3" || viewModel.TemplateCode == "INSPECTION_SER_TASK3_HALUL")
                        {
                            var signedAttachment = rowData.GetValueOrDefault("signedAfcAttachment").ToString();
                            rowData2[attachmentKey] = Convert.ToString(signedAttachment);
                        }
                        var currentStageStatus1 = rowData2.GetValueOrDefault("stageStatus").ToString();
                        var stageStatusId1 = await GetStageStatus(viewModel.TemplateCode, rowData, currentStageStatus1, sp);
                        if (stageStatusId1.IsNotNullAndNotEmpty())
                        {
                            rowData2["stageStatus"] = stageStatusId1;
                        }


                        var data2 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData2);
                        var update1 = await _noteBusiness.EditNoteUdfTable(notemodel1, data2, notemodel1.UdfNoteTableId);

                    }
                }
            }
            return CommandResult<TaskTemplateViewModel>.Instance(viewModel);
        }
        public async Task<CommandResult<TaskTemplateViewModel>> CopyProjectDocumentToFolderForUploadingToQP(TaskTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _udfPermissionBusiness = sp.GetService<IUdfPermissionBusiness>();
            var _columnMetadataBusiness = sp.GetService<IColumnMetadataBusiness>();
            var _serviceBusiness = sp.GetService<IServiceBusiness>();
            var _noteBusiness = sp.GetService<INoteBusiness>();
            var _lovBusiness = sp.GetService<ILOVBusiness>();
            var _documentBusiness = sp.GetService<IDMSDocumentBusiness>();
            var serviceDetails = await _serviceBusiness.GetSingleById(viewModel.ParentServiceId);            
            var service = await _serviceBusiness.GetServiceDetails(new ServiceTemplateViewModel
            {
                TemplateCode = serviceDetails.TemplateCode,
                ServiceId = serviceDetails.Id,
                SetUdfValue = true,
            });
            if (service.IsNotNull())
            {
                var documentIdCol = service.ColumnList.Where(x => x.Name == "documentId").FirstOrDefault();
                var documentId = documentIdCol.Value.ToString();
                if (documentId.IsNotNullAndNotEmpty())
                {                
                    var noteDetails = await _noteBusiness.GetSingleById(documentId);
                    var noteTempModel = new NoteTemplateViewModel();
                    noteTempModel.TemplateCode = noteDetails.TemplateCode;
                    noteTempModel.NoteId = noteDetails.Id;
                    noteTempModel.SetUdfValue = true;
                    var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                    var udfData = notemodel.ColumnList;
                    var projectFolderCol = udfData.Where(x => x.Name == "projectFolder").FirstOrDefault();
                    var projectFolderLov = await _lovBusiness.GetSingleById(projectFolderCol.Value.ToString());
                    var projectFolder = projectFolderLov.Name;
                    var projectSubFolderCol = udfData.Where(x => x.Name == "projectSubFolder").FirstOrDefault();
                    var projectSubFolderLov = await _lovBusiness.GetSingleById(projectSubFolderCol.Value.ToString());
                    var projectSubFolder = projectSubFolderLov.Name;
                    var disciplineCol = udfData.Where(x => x.Name == "discipline").FirstOrDefault();
                    var disciplineLov = await _lovBusiness.GetSingleById(disciplineCol.Value.ToString());
                    var discipline = disciplineLov.Name;
                    var revisionCol = udfData.Where(x => x.Name == "revision").FirstOrDefault();
                    var revisionLov = await _lovBusiness.GetSingleById(revisionCol.Value.ToString());
                    var revision = revisionLov.Name;
                    var workspaces = await _documentBusiness.GetAllGeneralWorkspaceData();
                    var workspace = workspaces.Where(x => x.Name.ToLower().Contains(projectFolder.ToLower())).FirstOrDefault();
                    if (workspace != null)
                    {
                        if (projectSubFolder != "NA")
                        {
                            var desiplins = await _documentBusiness.GetAllChildbyParent(workspace.Id);
                            var desiplin = desiplins.Where(x => x.Name != null && x.Name.ToLower().Contains(discipline.ToLower())).FirstOrDefault();
                            string desiplinFolderId =string.Empty;
                            if (desiplin != null)
                            {
                                desiplinFolderId = desiplin.Id;

                            }
                            else
                            {                            
                                var result = await CreateFolder(discipline, workspace.Id, workspace.OwnerUserId, sp);
                                desiplinFolderId = result.Item.NoteId;
                            }
                            var statements = await _documentBusiness.GetAllChildbyParent(desiplinFolderId);
                            var statement=  statements.Where(x => x.Name != null && x.Name.ToLower().Contains(projectSubFolder.ToLower())).FirstOrDefault();
                            string statementFolderId = string.Empty;
                            if (statement != null)
                            {
                                statementFolderId = statement.Id;
                            }
                            else
                            {                            
                                var result = await CreateFolder(projectSubFolder, desiplinFolderId, workspace.OwnerUserId, sp);
                                statementFolderId = result.Item.NoteId;
                            }
                            var revs = await _documentBusiness.GetAllChildbyParent(statementFolderId);
                            var rev= revs.Where(x => x.Name != null && x.Name.ToLower().Contains(revision.ToLower())).FirstOrDefault();
                            string revParentId = string.Empty;
                            if (rev != null)
                            {
                                revParentId = rev.Id;
                            }
                            else
                            {                            
                                var result = await CreateFolder(revision, statementFolderId, workspace.OwnerUserId, sp);
                                revParentId = result.Item.NoteId;
                            }
                            var revnote=await CopyDocument(noteDetails, revParentId, workspace.OwnerUserId, sp);                        
                            //_repoNote.CreateOneToOneRelationshipToReferenceType(revnote.Id, new R_Note_Reference(), NodeEnum.NTS_Service, viewModel.ServiceId.Value);


                            if (revision.Contains("Rev"))
                            {
                                var latestrevfolders = await _documentBusiness.GetAllChildbyParent(statementFolderId);
                                var latestrevfolder= latestrevfolders.Where(x => x.Name.ToLower().Contains("latest")).FirstOrDefault();
                                if (latestrevfolder != null)
                                {

                                    var revDoc =await _documentBusiness.CheckDocumentExist(latestrevfolder.Id);
                                    if (revDoc != null)
                                    {
                                        var check = revDoc.Where(x => x.DocumentNo == noteDetails.NoteNo).FirstOrDefault();
                                        if (check != null)
                                        {                                        
                                           await _noteBusiness.Delete(check.Id);
                                        }
                                        var note = await CopyDocument(noteDetails, latestrevfolder.Id, workspace.OwnerUserId, sp);
                                       // _repoNote.CreateOneToOneRelationshipToReferenceType(note.Id, new R_Note_Reference(), NodeEnum.NTS_Service, viewModel.ServiceId.Value);
                                    }

                                }
                                else
                                {                                
                                    var result = await CreateFolder("Latest revision Files", statementFolderId, workspace.OwnerUserId, sp);
                                    var note = await CopyDocument(noteDetails, result.Item.NoteId, workspace.OwnerUserId, sp);
                                    //_repoNote.CreateOneToOneRelationshipToReferenceType(note.Id, new R_Note_Reference(), NodeEnum.NTS_Service, viewModel.ServiceId.Value);
                                }

                            }



                        }
                        else
                        {
                            var desiplins = await _documentBusiness.GetAllChildbyParent(workspace.Id);
                            var desiplin= desiplins.Where(x => x.Name != null && x.Name.ToLower().Contains(discipline.ToLower())).FirstOrDefault();
                            string desiplinFolderId = string.Empty;
                            if (desiplin != null)
                            {
                                desiplinFolderId = desiplin.Id;

                            }
                            else
                            {                            
                                var result = await CreateFolder(discipline, workspace.Id, workspace.OwnerUserId, sp);
                                desiplinFolderId = result.Item.NoteId;
                            }

                            var revs = await _documentBusiness.GetAllChildbyParent(desiplinFolderId);
                            var rev=revs.Where(x => x.Name != null && x.Name.ToLower().Contains(revision.ToLower())).FirstOrDefault();
                            string revParentId = string.Empty;
                            if (rev != null)
                            {
                                revParentId = rev.Id;
                            }
                            else
                            {                           
                                var result = await CreateFolder(revision, desiplinFolderId, workspace.OwnerUserId, sp);
                                revParentId = result.Item.NoteId;                            
                            }
                            var revnote = await CopyDocument(noteDetails, revParentId, workspace.OwnerUserId, sp);
                            //_repoNote.CreateOneToOneRelationshipToReferenceType(revnote.Id, new R_Note_Reference(), NodeEnum.NTS_Service, viewModel.ServiceId.Value);


                            if (revision.Contains("Rev"))
                            {
                                var latestrevfolders = await _documentBusiness.GetAllChildbyParent(desiplinFolderId);
                                var latestrevfolder= latestrevfolders.Where(x => x.Name.ToLower().Contains("latest")).FirstOrDefault();
                                if (latestrevfolder != null)
                                {

                                    var revDoc =await _documentBusiness.CheckDocumentExist(latestrevfolder.Id);
                                    if (revDoc != null)
                                    {
                                        var check = revDoc.Where(x => x.DocumentNo == noteDetails.NoteNo).FirstOrDefault();

                                        if (check != null)
                                        {                                        
                                            await _noteBusiness.Delete(check.Id);
                                        }
                                        var note = await CopyDocument(noteDetails, latestrevfolder.Id, workspace.OwnerUserId, sp);
                                        //_repoNote.CreateOneToOneRelationshipToReferenceType(note.Id, new R_Note_Reference(), NodeEnum.NTS_Service, viewModel.ServiceId.Value);
                                    }

                                }
                                else
                                {                                
                                    var result = await CreateFolder("Latest revision Files", desiplinFolderId, workspace.OwnerUserId, sp);
                                    var note = await CopyDocument(noteDetails, result.Item.NoteId, workspace.OwnerUserId, sp);
                                    //_repoNote.CreateOneToOneRelationshipToReferenceType(note.Id, new R_Note_Reference(), NodeEnum.NTS_Service, viewModel.ServiceId.Value);
                                }

                            }


                        }

                    }
                }
            }
            return CommandResult<TaskTemplateViewModel>.Instance(viewModel);
        }
        public async Task<CommandResult<TaskTemplateViewModel>> CopyProjectDocumentToFolderForReceivingFromQP(TaskTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _udfPermissionBusiness = sp.GetService<IUdfPermissionBusiness>();
            var _columnMetadataBusiness = sp.GetService<IColumnMetadataBusiness>();
            var _serviceBusiness = sp.GetService<IServiceBusiness>();
            var _noteBusiness = sp.GetService<INoteBusiness>();
            var _lovBusiness = sp.GetService<ILOVBusiness>();
            var _documentBusiness = sp.GetService<IDMSDocumentBusiness>();
            var rowData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(viewModel.Json);
            var codeCol = Convert.ToString(rowData["code"]);
            var codeLov = await _lovBusiness.GetSingleById(codeCol);
            var code = codeLov.Name;
            var serviceDetails = await _serviceBusiness.GetSingleById(viewModel.ParentServiceId);
            var service = await _serviceBusiness.GetServiceDetails(new ServiceTemplateViewModel
            {
                TemplateCode = serviceDetails.TemplateCode,
                ServiceId = serviceDetails.Id,
                SetUdfValue = true,
            });
            if (service.IsNotNull())
            {            
                var documentIdCol = service.ColumnList.Where(x => x.Name == "documentId").FirstOrDefault();
                var documentId = documentIdCol.Value.ToString();
                if (documentId.IsNotNullAndNotEmpty())
                {
                    var noteDetails = await _noteBusiness.GetSingleById(documentId);
                    var noteTempModel = new NoteTemplateViewModel();
                    noteTempModel.TemplateCode = noteDetails.TemplateCode;
                    noteTempModel.NoteId = noteDetails.Id;
                    noteTempModel.SetUdfValue = true;
                    var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                    var udfData = notemodel.ColumnList;
                    var projectFolderCol = udfData.Where(x => x.Name == "projectFolder").FirstOrDefault();
                    var projectFolderLov = await _lovBusiness.GetSingleById(projectFolderCol.Value.ToString());
                    var projectFolder = projectFolderLov.Name;
                    var projectSubFolderCol = udfData.Where(x => x.Name == "projectSubFolder").FirstOrDefault();
                    var projectSubFolderLov = await _lovBusiness.GetSingleById(projectSubFolderCol.Value.ToString());
                    var projectSubFolder = projectSubFolderLov.Name;
                    var disciplineCol = udfData.Where(x => x.Name == "discipline").FirstOrDefault();
                    var disciplineLov = await _lovBusiness.GetSingleById(disciplineCol.Value.ToString());
                    var discipline = disciplineLov.Name;
                    var revisionCol = udfData.Where(x => x.Name == "revision").FirstOrDefault();
                    var revisionLov = await _lovBusiness.GetSingleById(revisionCol.Value.ToString());
                    var revision = revisionLov.Name;
                    var workspaces = await _documentBusiness.GetAllGeneralWorkspaceData();
                    var workspace = workspaces.Where(x => x.Name.ToLower().Contains(projectFolder.ToLower())).FirstOrDefault();
                    if (workspace != null)
                    {
                        if (projectSubFolder != "NA")
                        {
                            var desiplins = await _documentBusiness.GetAllChildbyParent(workspace.Id);
                            var desiplin = desiplins.Where(x => x.Name != null && x.Name.ToLower().Contains(discipline.ToLower())).FirstOrDefault();
                            string desiplinFolderId = string.Empty;
                            if (desiplin != null)
                            {
                                desiplinFolderId = desiplin.Id;

                            }
                            else
                            {
                                var result = await CreateFolder(discipline, workspace.Id, workspace.OwnerUserId, sp);
                                desiplinFolderId = result.Item.NoteId;
                            }
                            var statements = await _documentBusiness.GetAllChildbyParent(desiplinFolderId);
                            var statement = statements.Where(x => x.Name != null && x.Name.ToLower().Contains(projectSubFolder.ToLower())).FirstOrDefault();
                            string statementFolderId = string.Empty;
                            if (statement != null)
                            {
                                statementFolderId = statement.Id;
                            }
                            else
                            {
                                var result = await CreateFolder(projectSubFolder, desiplinFolderId, workspace.OwnerUserId, sp);
                                statementFolderId = result.Item.NoteId;
                            }
                            if (code.Contains("AFC"))
                            {

                                var folders =await _documentBusiness.GetAllChildbyParent(statementFolderId);
                                var folder=folders.Where(x => x.Name != null && x.Name.ToLower().Contains("signed")).FirstOrDefault();
                                string parentId = string.Empty;
                                if (folder != null)
                                {
                                    parentId = folder.Id;
                                }
                                else
                                {                               
                                    var result = await CreateFolder("Signed AFC", statementFolderId, workspace.OwnerUserId, sp);
                                    parentId = result.Item.NoteId;
                                }
                                if (parentId.IsNotNullAndNotEmpty())
                                {
                                    var revDoc =await _documentBusiness.CheckDocumentExist(parentId);
                                    if (revDoc != null)
                                    {
                                        var check = revDoc.Where(x => x.DocumentNo == noteDetails.NoteNo).FirstOrDefault();

                                        if (check != null)
                                        {                                       
                                           await _noteBusiness.Delete(check.Id);
                                        }
                                        var note =await CopyDocument(noteDetails, parentId, workspace.OwnerUserId,sp);
                                        //_repoNote.CreateOneToOneRelationshipToReferenceType(note.Id, new R_Note_Reference(), NodeEnum.NTS_Service, viewModel.ServiceId.Value);
                                    }
                                }
                            }
                        }
                        else
                        {
                            var desiplins = await _documentBusiness.GetAllChildbyParent(workspace.Id);
                            var desiplin = desiplins.Where(x => x.Name != null && x.Name.ToLower().Contains(discipline.ToLower())).FirstOrDefault();
                            string desiplinFolderId = string.Empty;
                            if (desiplin != null)
                            {
                                desiplinFolderId = desiplin.Id;

                            }
                            else
                            {
                                var result = await CreateFolder(discipline, workspace.Id, workspace.OwnerUserId, sp);
                                desiplinFolderId = result.Item.NoteId;
                            }

                            if (code.Contains("AFC"))
                            {
                                var folders = await _documentBusiness.GetAllChildbyParent(desiplinFolderId);
                                var folder = folders.Where(x => x.Name != null && x.Name.ToLower().Contains("signed")).FirstOrDefault();
                                string parentId = string.Empty;                            
                                if (folder != null)
                                {
                                    parentId = folder.Id;
                                }
                                else
                                {
                                    var result = await CreateFolder("Signed AFC", desiplinFolderId, workspace.OwnerUserId, sp);
                                    parentId = result.Item.NoteId;
                                }
                                if (parentId.IsNotNullAndNotEmpty())
                                {
                                    var revDoc = await _documentBusiness.CheckDocumentExist(parentId);
                                    if (revDoc != null)
                                    {
                                        var check = revDoc.Where(x => x.DocumentNo == noteDetails.NoteNo).FirstOrDefault();

                                        if (check != null)
                                        {
                                            await _noteBusiness.Delete(check.Id);
                                        }
                                        var note = await CopyDocument(noteDetails, parentId, workspace.OwnerUserId, sp);
                                        //_repoNote.CreateOneToOneRelationshipToReferenceType(note.Id, new R_Note_Reference(), NodeEnum.NTS_Service, viewModel.ServiceId.Value);
                                    }
                                }

                            }


                        }

                    }
                }
            }
            return CommandResult<TaskTemplateViewModel>.Instance(viewModel);
        }
        public async Task<CommandResult<TaskTemplateViewModel>> CopyVendorDocumentToFolderForUploadingToQP(TaskTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _udfPermissionBusiness = sp.GetService<IUdfPermissionBusiness>();
            var _columnMetadataBusiness = sp.GetService<IColumnMetadataBusiness>();
            var _serviceBusiness = sp.GetService<IServiceBusiness>();
            var _noteBusiness = sp.GetService<INoteBusiness>();
            var _lovBusiness = sp.GetService<ILOVBusiness>();
            var _documentBusiness = sp.GetService<IDMSDocumentBusiness>();
            var serviceDetails = await _serviceBusiness.GetSingleById(viewModel.ParentServiceId);
            var service = await _serviceBusiness.GetServiceDetails(new ServiceTemplateViewModel
            {
                TemplateCode = serviceDetails.TemplateCode,
                ServiceId = serviceDetails.Id,
                SetUdfValue = true,
            });
            if (service.IsNotNull())
            {
                var documentIdCol = service.ColumnList.Where(x => x.Name == "documentId").FirstOrDefault();
                var documentId = documentIdCol.Value.ToString();
                if (documentId.IsNotNullAndNotEmpty())
                {
                    var noteDetails = await _noteBusiness.GetSingleById(documentId);
                    var noteTempModel = new NoteTemplateViewModel();
                    noteTempModel.TemplateCode = noteDetails.TemplateCode;
                    noteTempModel.NoteId = noteDetails.Id;
                    noteTempModel.SetUdfValue = true;
                    var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                    var udfData = notemodel.ColumnList;
                    var vendorCol = udfData.Where(x => x.Name == "vendorList").FirstOrDefault();
                    var vendorLov = await _lovBusiness.GetSingleById(vendorCol.Value.ToString());
                    var vendor = vendorLov.Name;
                    var disciplineCol = udfData.Where(x => x.Name == "discipline").FirstOrDefault();
                    var disciplineLov = await _lovBusiness.GetSingleById(disciplineCol.Value.ToString());
                    var discipline = disciplineLov.Name;
                    var revisionCol = udfData.Where(x => x.Name == "revision").FirstOrDefault();
                    var revisionLov = await _lovBusiness.GetSingleById(revisionCol.Value.ToString());
                    var revision = revisionLov.Name;
                    var workspaces = await _documentBusiness.GetAllGeneralWorkspaceData();
                    var workspace = workspaces.Where(x => x.Name.ToLower().Contains("vendor documents")).FirstOrDefault();
                    if (workspace != null)
                    {

                        var vendorFolders = await _documentBusiness.GetAllChildbyParent(workspace.Id);
                        var vendorFolder = vendorFolders.Where(x => x.Name != null && x.Name.ToLower().Contains(vendor.ToLower()) && x.Name.Contains("Vendor Document")).FirstOrDefault();
                        string venFolderId = string.Empty;
                        if (vendorFolder != null)
                        {
                            venFolderId = vendorFolder.Id;

                        }
                        else
                        {
                            var name = vendor + " " + "Vendor Document";
                            var result = await CreateFolder(name, workspace.Id, workspace.OwnerUserId, sp);
                            venFolderId = result.Item.NoteId;
                        }
                        var desiplins = await _documentBusiness.GetAllChildbyParent(venFolderId);
                        var desiplin = desiplins.Where(x => x.Name != null && x.Name.ToLower().Contains(discipline.ToLower())).FirstOrDefault();
                        string desiplinFolderId = string.Empty;
                        if (desiplin != null)
                        {
                            desiplinFolderId = desiplin.Id;
                        }
                        else
                        {
                            var result = await CreateFolder(discipline, venFolderId, workspace.OwnerUserId, sp);
                            desiplinFolderId = result.Item.NoteId;
                        }
                        var revs = await _documentBusiness.GetAllChildbyParent(desiplinFolderId);
                        var rev = revs.Where(x => x.Name != null && x.Name.ToLower().Contains(revision.ToLower())).FirstOrDefault();
                        string revParentId = string.Empty;
                        if (rev != null)
                        {
                            revParentId = rev.Id;
                        }
                        else
                        {
                            var result = await CreateFolder(revision, desiplinFolderId, workspace.OwnerUserId, sp);
                            revParentId = result.Item.NoteId;
                        }
                        var revnote = await CopyDocument(noteDetails, revParentId, workspace.OwnerUserId, sp);
                        //_repoNote.CreateOneToOneRelationshipToReferenceType(revnote.Id, new R_Note_Reference(), NodeEnum.NTS_Service, viewModel.ServiceId.Value);


                        if (revision.Contains("Rev"))
                        {
                            var latestrevfolders = await _documentBusiness.GetAllChildbyParent(desiplinFolderId);
                            var latestrevfolder = latestrevfolders.Where(x => x.Name.ToLower().Contains("latest")).FirstOrDefault();
                            if (latestrevfolder != null)
                            {

                                var revDoc = await _documentBusiness.CheckDocumentExist(latestrevfolder.Id);
                                if (revDoc != null)
                                {
                                    var check = revDoc.Where(x => x.DocumentNo == noteDetails.NoteNo).FirstOrDefault();
                                    if (check != null)
                                    {
                                        await _noteBusiness.Delete(check.Id);
                                    }
                                    var note = await CopyDocument(noteDetails, latestrevfolder.Id, workspace.OwnerUserId, sp);
                                    // _repoNote.CreateOneToOneRelationshipToReferenceType(note.Id, new R_Note_Reference(), NodeEnum.NTS_Service, viewModel.ServiceId.Value);
                                }

                            }
                            else
                            {
                                var result = await CreateFolder("Latest revision Files", desiplinFolderId, workspace.OwnerUserId, sp);
                                var note = await CopyDocument(noteDetails, result.Item.NoteId, workspace.OwnerUserId, sp);
                                //_repoNote.CreateOneToOneRelationshipToReferenceType(note.Id, new R_Note_Reference(), NodeEnum.NTS_Service, viewModel.ServiceId.Value);
                            }

                        }
                    }
                }
            }
            return CommandResult<TaskTemplateViewModel>.Instance(viewModel);
        }
        public async Task<CommandResult<TaskTemplateViewModel>> CopyVendorDocumentToFolderForReceivingFromQP(TaskTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _udfPermissionBusiness = sp.GetService<IUdfPermissionBusiness>();
            var _columnMetadataBusiness = sp.GetService<IColumnMetadataBusiness>();
            var _serviceBusiness = sp.GetService<IServiceBusiness>();
            var _noteBusiness = sp.GetService<INoteBusiness>();
            var _lovBusiness = sp.GetService<ILOVBusiness>();
            var _documentBusiness = sp.GetService<IDMSDocumentBusiness>();
            var rowData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(viewModel.Json);
            var codeCol = Convert.ToString(rowData["code"]);
            var codeLov = await _lovBusiness.GetSingleById(codeCol);
            var code = codeLov.Name;
            var serviceDetails = await _serviceBusiness.GetSingleById(viewModel.ParentServiceId);
            var service = await _serviceBusiness.GetServiceDetails(new ServiceTemplateViewModel
            {
                TemplateCode = serviceDetails.TemplateCode,
                ServiceId = serviceDetails.Id,
                SetUdfValue = true,
            });
            if (service.IsNotNull())
            {
                var documentIdCol = service.ColumnList.Where(x => x.Name == "documentId").FirstOrDefault();
                var documentId = documentIdCol.Value.ToString();
                if (documentId.IsNotNullAndNotEmpty())
                {
                    var noteDetails = await _noteBusiness.GetSingleById(documentId);
                    var noteTempModel = new NoteTemplateViewModel();
                    noteTempModel.TemplateCode = noteDetails.TemplateCode;
                    noteTempModel.NoteId = noteDetails.Id;
                    noteTempModel.SetUdfValue = true;
                    var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                    var udfData = notemodel.ColumnList;
                    var vendorCol = udfData.Where(x => x.Name == "vendorList").FirstOrDefault();
                    var vendorLov = await _lovBusiness.GetSingleById(vendorCol.Value.ToString());
                    var vendor = vendorLov.Name;
                    var disciplineCol = udfData.Where(x => x.Name == "discipline").FirstOrDefault();
                    var disciplineLov = await _lovBusiness.GetSingleById(disciplineCol.Value.ToString());
                    var discipline = disciplineLov.Name;
                    var revisionCol = udfData.Where(x => x.Name == "revision").FirstOrDefault();
                    var revisionLov = await _lovBusiness.GetSingleById(revisionCol.Value.ToString());
                    var revision = revisionLov.Name;
                    var workspaces = await _documentBusiness.GetAllGeneralWorkspaceData();
                    var workspace = workspaces.Where(x => x.Name.ToLower().Contains("vendor documents")).FirstOrDefault();
                    if (workspace != null)
                    {

                        var vendorFolders = await _documentBusiness.GetAllChildbyParent(workspace.Id);
                        var vendorFolder = vendorFolders.Where(x => x.Name != null && x.Name.ToLower().Contains(vendor.ToLower()) && x.Name.Contains("Vendor Document")).FirstOrDefault();
                        string venFolderId = string.Empty;
                        if (vendorFolder != null)
                        {
                            venFolderId = vendorFolder.Id;

                        }
                        else
                        {
                            var name = vendor + " " + "Vendor Document";
                            var result = await CreateFolder(name, workspace.Id, workspace.OwnerUserId, sp);
                            venFolderId = result.Item.NoteId;
                        }
                        var desiplins = await _documentBusiness.GetAllChildbyParent(venFolderId);
                        var desiplin = desiplins.Where(x => x.Name != null && x.Name.ToLower().Contains(discipline.ToLower())).FirstOrDefault();
                        string desiplinFolderId = string.Empty;
                        if (desiplin != null)
                        {
                            desiplinFolderId = desiplin.Id;
                        }
                        else
                        {
                            var result = await CreateFolder(discipline, venFolderId, workspace.OwnerUserId, sp);
                            desiplinFolderId = result.Item.NoteId;
                        }
                        if (code.Contains("AFC"))
                        {
                            var folders = await _documentBusiness.GetAllChildbyParent(desiplinFolderId);
                            var folder = folders.Where(x => x.Name != null && x.Name.ToLower().Contains("signed")).FirstOrDefault();
                            string parentId = string.Empty;
                            if (folder != null)
                            {
                                parentId = folder.Id;
                            }
                            else
                            {
                                var result = await CreateFolder("Signed AFC", desiplinFolderId, workspace.OwnerUserId, sp);
                                parentId = result.Item.NoteId;
                            }
                            if (parentId.IsNotNullAndNotEmpty())
                            {
                                var revDoc = await _documentBusiness.CheckDocumentExist(parentId);
                                if (revDoc != null)
                                {
                                    var check = revDoc.Where(x => x.DocumentNo == noteDetails.NoteNo).FirstOrDefault();

                                    if (check != null)
                                    {
                                        await _noteBusiness.Delete(check.Id);
                                    }
                                    var note = await CopyDocument(noteDetails, parentId, workspace.OwnerUserId, sp);
                                    //_repoNote.CreateOneToOneRelationshipToReferenceType(note.Id, new R_Note_Reference(), NodeEnum.NTS_Service, viewModel.ServiceId.Value);
                                }
                            }

                        }
                    }
                }
            }
            return CommandResult<TaskTemplateViewModel>.Instance(viewModel);
        }
        public async Task<CommandResult<TaskTemplateViewModel>> CopyEngineeringDocumentToFolderForUploadingToQP(TaskTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _udfPermissionBusiness = sp.GetService<IUdfPermissionBusiness>();
            var _columnMetadataBusiness = sp.GetService<IColumnMetadataBusiness>();
            var _serviceBusiness = sp.GetService<IServiceBusiness>();
            var _noteBusiness = sp.GetService<INoteBusiness>();
            var _lovBusiness = sp.GetService<ILOVBusiness>();
            var _documentBusiness = sp.GetService<IDMSDocumentBusiness>();
            var serviceDetails = await _serviceBusiness.GetSingleById(viewModel.ParentServiceId);
            var service = await _serviceBusiness.GetServiceDetails(new ServiceTemplateViewModel
            {
                TemplateCode = serviceDetails.TemplateCode,
                ServiceId = serviceDetails.Id,
                SetUdfValue = true,
            });
            if (service.IsNotNull())
            {
                var documentIdCol = service.ColumnList.Where(x => x.Name == "documentId").FirstOrDefault();
                var documentId = documentIdCol.Value.ToString();
                if (documentId.IsNotNullAndNotEmpty())
                {
                    var noteDetails = await _noteBusiness.GetSingleById(documentId);
                    var noteTempModel = new NoteTemplateViewModel();
                    noteTempModel.TemplateCode = noteDetails.TemplateCode;
                    noteTempModel.NoteId = noteDetails.Id;
                    noteTempModel.SetUdfValue = true;
                    var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                    var udfData = notemodel.ColumnList;
                    var disciplineCol = udfData.Where(x => x.Name == "discipline").FirstOrDefault();
                    var disciplineLov = await _lovBusiness.GetSingleById(disciplineCol.Value.ToString());
                    var discipline = disciplineLov.Name;
                    var revisionCol = udfData.Where(x => x.Name == "revision").FirstOrDefault();
                    var revisionLov = await _lovBusiness.GetSingleById(revisionCol.Value.ToString());
                    var revision = revisionLov.Name;
                    var workspaces = await _documentBusiness.GetAllGeneralWorkspaceData();
                    var workspace = workspaces.Where(x => x.Name.ToLower().Contains("engineering")).FirstOrDefault();
                    if (workspace != null)
                    {
                        var desiplins = await _documentBusiness.GetAllChildbyParent(workspace.Id);
                        var desiplin = desiplins.Where(x => x.Name != null && x.Name.ToLower().Contains(discipline.ToLower())).FirstOrDefault();
                        string desiplinFolderId = string.Empty;
                        if (desiplin != null)
                        {
                            desiplinFolderId = desiplin.Id;
                        }
                        else
                        {
                            var result = await CreateFolder(discipline, workspace.Id, workspace.OwnerUserId, sp);
                            desiplinFolderId = result.Item.NoteId;
                        }
                        var revs = await _documentBusiness.GetAllChildbyParent(desiplinFolderId);
                        var rev = revs.Where(x => x.Name != null && x.Name.ToLower().Contains(revision.ToLower())).FirstOrDefault();
                        string revParentId = string.Empty;
                        if (rev != null)
                        {
                            revParentId = rev.Id;
                        }
                        else
                        {
                            var result = await CreateFolder(revision, desiplinFolderId, workspace.OwnerUserId, sp);
                            revParentId = result.Item.NoteId;
                        }
                        var revnote = await CopyDocument(noteDetails, revParentId, workspace.OwnerUserId, sp);
                        //_repoNote.CreateOneToOneRelationshipToReferenceType(revnote.Id, new R_Note_Reference(), NodeEnum.NTS_Service, viewModel.ServiceId.Value);


                        if (revision.Contains("Rev"))
                        {
                            var latestrevfolders = await _documentBusiness.GetAllChildbyParent(desiplinFolderId);
                            var latestrevfolder = latestrevfolders.Where(x => x.Name.ToLower().Contains("latest")).FirstOrDefault();
                            if (latestrevfolder != null)
                            {

                                var revDoc = await _documentBusiness.CheckDocumentExist(latestrevfolder.Id);
                                if (revDoc != null)
                                {
                                    var check = revDoc.Where(x => x.DocumentNo == noteDetails.NoteNo).FirstOrDefault();
                                    if (check != null)
                                    {
                                        await _noteBusiness.Delete(check.Id);
                                    }
                                    var note = await CopyDocument(noteDetails, latestrevfolder.Id, workspace.OwnerUserId, sp);
                                    // _repoNote.CreateOneToOneRelationshipToReferenceType(note.Id, new R_Note_Reference(), NodeEnum.NTS_Service, viewModel.ServiceId.Value);
                                }

                            }
                            else
                            {
                                var result = await CreateFolder("Latest revision Files", desiplinFolderId, workspace.OwnerUserId, sp);
                                var note = await CopyDocument(noteDetails, result.Item.NoteId, workspace.OwnerUserId, sp);
                                //_repoNote.CreateOneToOneRelationshipToReferenceType(note.Id, new R_Note_Reference(), NodeEnum.NTS_Service, viewModel.ServiceId.Value);
                            }

                        }
                    }
                }
            }
            return CommandResult<TaskTemplateViewModel>.Instance(viewModel);
        }
        public async Task<CommandResult<TaskTemplateViewModel>> CopyEngineeringDocumentToFolderForReceivingFromQP(TaskTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _udfPermissionBusiness = sp.GetService<IUdfPermissionBusiness>();
            var _columnMetadataBusiness = sp.GetService<IColumnMetadataBusiness>();
            var _serviceBusiness = sp.GetService<IServiceBusiness>();
            var _noteBusiness = sp.GetService<INoteBusiness>();
            var _lovBusiness = sp.GetService<ILOVBusiness>();
            var _documentBusiness = sp.GetService<IDMSDocumentBusiness>();
            var rowData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(viewModel.Json);
            var codeCol = Convert.ToString(rowData["code"]);
            var codeLov = await _lovBusiness.GetSingleById(codeCol);
            var code = codeLov.Name;
            var serviceDetails = await _serviceBusiness.GetSingleById(viewModel.ParentServiceId);
            var service = await _serviceBusiness.GetServiceDetails(new ServiceTemplateViewModel
            {
                TemplateCode = serviceDetails.TemplateCode,
                ServiceId = serviceDetails.Id,
                SetUdfValue = true,
            });
            if (service.IsNotNull())
            {
                var documentIdCol = service.ColumnList.Where(x => x.Name == "documentId").FirstOrDefault();
                var documentId = documentIdCol.Value.ToString();
                if (documentId.IsNotNullAndNotEmpty())
                {
                    var noteDetails = await _noteBusiness.GetSingleById(documentId);
                    var noteTempModel = new NoteTemplateViewModel();
                    noteTempModel.TemplateCode = noteDetails.TemplateCode;
                    noteTempModel.NoteId = noteDetails.Id;
                    noteTempModel.SetUdfValue = true;
                    var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                    var udfData = notemodel.ColumnList;                    
                    var disciplineCol = udfData.Where(x => x.Name == "discipline").FirstOrDefault();
                    var disciplineLov = await _lovBusiness.GetSingleById(disciplineCol.Value.ToString());
                    var discipline = disciplineLov.Name;
                    var revisionCol = udfData.Where(x => x.Name == "revision").FirstOrDefault();
                    var revisionLov = await _lovBusiness.GetSingleById(revisionCol.Value.ToString());
                    var revision = revisionLov.Name;
                    var workspaces = await _documentBusiness.GetAllGeneralWorkspaceData();
                    var workspace = workspaces.Where(x => x.Name.ToLower().Contains("engineering")).FirstOrDefault();
                    if (workspace != null)
                    {
                        var desiplins = await _documentBusiness.GetAllChildbyParent(workspace.Id);
                        var desiplin = desiplins.Where(x => x.Name != null && x.Name.ToLower().Contains(discipline.ToLower())).FirstOrDefault();
                        string desiplinFolderId = string.Empty;
                        if (desiplin != null)
                        {
                            desiplinFolderId = desiplin.Id;
                        }
                        else
                        {
                            var result = await CreateFolder(discipline, workspace.Id, workspace.OwnerUserId, sp);
                            desiplinFolderId = result.Item.NoteId;
                        }
                        if (code.Contains("AFC"))
                        {
                            var folders = await _documentBusiness.GetAllChildbyParent(desiplinFolderId);
                            var folder = folders.Where(x => x.Name != null && x.Name.ToLower().Contains("signed")).FirstOrDefault();
                            string parentId = string.Empty;
                            if (folder != null)
                            {
                                parentId = folder.Id;
                            }
                            else
                            {
                                var result = await CreateFolder("Signed AFC", desiplinFolderId, workspace.OwnerUserId, sp);
                                parentId = result.Item.NoteId;
                            }
                            if (parentId.IsNotNullAndNotEmpty())
                            {
                                var revDoc = await _documentBusiness.CheckDocumentExist(parentId);
                                if (revDoc != null)
                                {
                                    var check = revDoc.Where(x => x.DocumentNo == noteDetails.NoteNo).FirstOrDefault();

                                    if (check != null)
                                    {
                                        await _noteBusiness.Delete(check.Id);
                                    }
                                    var note = await CopyDocument(noteDetails, parentId, workspace.OwnerUserId, sp);
                                    //_repoNote.CreateOneToOneRelationshipToReferenceType(note.Id, new R_Note_Reference(), NodeEnum.NTS_Service, viewModel.ServiceId.Value);
                                }
                            }

                        }
                    }
                }
            }
            return CommandResult<TaskTemplateViewModel>.Instance(viewModel);
        }
        private async Task<CommandResult<NoteTemplateViewModel>> CopyDocument(NoteViewModel note,string targetId,string ownerId, IServiceProvider sp)
        {
            var _noteBusiness = sp.GetService<INoteBusiness>();
            var _documentBusiness = sp.GetService<IDMSDocumentBusiness>();
            var templateModel = new NoteTemplateViewModel();
            templateModel.ActiveUserId = ownerId;
            templateModel.DataAction = DataActionEnum.Create;
            templateModel.NoteId = note.Id;
            templateModel.SetUdfValue = true;
            var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
            newmodel.NoteId = null;
            newmodel.ParentNoteId = targetId;
            newmodel.StartDate = System.DateTime.Now;
            newmodel.SetUdfValue = true;
            newmodel.OwnerUserId = ownerId;
            newmodel.ReferenceId = note.ReferenceId;
            newmodel.ReferenceType = note.ReferenceType;
            var result = await _noteBusiness.ManageNote(newmodel);
            return result;
        }
        private async Task<CommandResult<NoteTemplateViewModel>> CreateFolder(string folderName, string parentId, string ownerId, IServiceProvider sp)
        {
            var _noteBusiness = sp.GetService<INoteBusiness>();
            var _documentBusiness = sp.GetService<IDMSDocumentBusiness>();            
            var templateModel = new NoteTemplateViewModel();
            templateModel.ActiveUserId = ownerId;
            templateModel.DataAction = DataActionEnum.Create;
            templateModel.TemplateCode = "GENERAL_FOLDER";
            var newmodel = await _noteBusiness.GetNoteDetails(templateModel);
            newmodel.NoteSubject = folderName;
            newmodel.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
            newmodel.ParentNoteId = parentId;
            newmodel.OwnerUserId = ownerId;
            var result = await _noteBusiness.ManageNote(newmodel);
            return result;
        }

        public async Task<string> GetStageStatus(string templateCode, Dictionary<string, object> rowData, string currentStageStatus, IServiceProvider sp)
        {
            var stageStatusId = "";
            var _lovBusiness = sp.GetService<ILOVBusiness>();
            if (templateCode == "GALFAR_VENDOR_STEP_TASK1" || templateCode == "ENGINEERING_SUBCONTRACT_STEP_TASK1")
            {
                var stageStatus = await _lovBusiness.GetSingle(x => x.Code == "InternalReview_Status");
                if (stageStatus.IsNotNull())
                {
                    stageStatusId = stageStatus.Id;
                }
            }
            else if (templateCode == "PROJECT_DOC_TASK3" || templateCode == "GALFAR_VENDOR_STEP_TASK3" || templateCode == "ENGINEERING_SUBCONTRACT_STEP_TASK3" || templateCode == "INSPECTION_SER_TASK2_HALUL" || templateCode == "INSPECTION_SER_TASK2")
            {
                var stageStatus = await _lovBusiness.GetSingle(x => x.Code == "QP_STATUS");
                if (stageStatus.IsNotNull())
                {
                    stageStatusId = stageStatus.Id;
                }
            }

            else if (templateCode == "GALFAR_VENDOR_STEP_TASK16" || templateCode == "GALFAR_VENDOR_STEP_TASK4" || templateCode == "ENG_DOC_STEP_TASK11" || templateCode == "ENGINEERING_SUBCONTRACT_STEP_TASK4"
                 || templateCode == "INSPECTION_SER_TASK_HALUL" || templateCode == "INSPECTION_SER_TASK3_HALUL" || templateCode == "INSPECTION_SER_TASK" || templateCode == "INSPECTION_SER_TASK3")
            {
                var stageStatus = await _lovBusiness.GetSingle(x => x.Code == "GAL_STATUS");
                if (stageStatus.IsNotNull())
                {
                    stageStatusId = stageStatus.Id;
                }
            }

            else if (templateCode == "PROJECT_DOC_TASK4")
            {
                var codeValue = rowData.GetValueOrDefault("code").ToString();
                var code = await _lovBusiness.GetSingleById(codeValue);
                var statusCode = "ReIssued_Status";
                if (code.IsNotNull() && (code.Code.Equals("6") || code.Code.Equals("10") || code.Code.Equals("AFC")))
                {
                    statusCode = "Closed_status";
                }
                var stageStatus = await _lovBusiness.GetSingle(x => x.Code == statusCode);
                if (stageStatus.IsNotNull())
                {
                    stageStatusId = stageStatus.Id;
                }
            }

            else if (templateCode == "ENGINEERING_SUBCONTRACT_STEP_TASK5")
            {
                var codeValue = rowData.GetValueOrDefault("outgoingTechnipIssueCodes").ToString();
                var code = await _lovBusiness.GetSingleById(codeValue);
                var statusCode = "TECHNIP_STATUS";
                if (code.IsNotNull() && (code.Code.Equals("6") || code.Code.Equals("10") || code.Code.Equals("AFC")))
                {
                    statusCode = "Closed_status";
                }
                var stageStatus = await _lovBusiness.GetSingle(x => x.Code == statusCode);
                if (stageStatus.IsNotNull())
                {
                    stageStatusId = stageStatus.Id;
                }
            }

            else if (templateCode == "GALFAR_VENDOR_STEP_TASK5")
            {
                var codeValue = rowData.GetValueOrDefault("code").ToString();
                var code = await _lovBusiness.GetSingleById(codeValue);
                var statusCode = "Vendor_Status";
                if (code.IsNotNull() && (code.Code.Equals("6") || code.Code.Equals("10") || code.Code.Equals("AFC")))
                {
                    statusCode = "Closed_status";
                }
                var stageStatus = await _lovBusiness.GetSingle(x => x.Code == statusCode);
                if (stageStatus.IsNotNull())
                {
                    stageStatusId = stageStatus.Id;
                }
            }

            else if (templateCode == "GALFAR_VENDOR_STEP_TASK6" || templateCode == "ENGINEERING_SUBCONTRACT_STEP_TASK6")
            {
                var codeValue = currentStageStatus;
                var code = await _lovBusiness.GetSingleById(codeValue);
                if (code.IsNotNull() && code.Code != "Closed_status")
                {
                    var stageStatus = await _lovBusiness.GetSingle(x => x.Code == "ReIssued_Status");
                    if (stageStatus.IsNotNull())
                    {
                        stageStatusId = stageStatus.Id;
                    }
                }

            }


            return stageStatusId;
        }
        
    }
}
