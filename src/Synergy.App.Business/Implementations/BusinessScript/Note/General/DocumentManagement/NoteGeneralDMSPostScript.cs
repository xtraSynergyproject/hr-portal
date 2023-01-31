using Hangfire;
using Synergy.App.Business.Interface.BusinessScript.Note.General.DocumentManagement;
using Synergy.App.Common;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business.Implementations.BusinessScript.Note.General.DocumentManagement
{
    public class NoteGeneralDMSPostScript : INoteGeneralDMSPostScript
    {
        //private readonly IHangfireScheduler _hangfireScheduler;
        public NoteGeneralDMSPostScript(//IHangfireScheduler hangfireScheduler
            )
        {
           // _hangfireScheduler = hangfireScheduler;
        }
        public async Task<CommandResult<NoteTemplateViewModel>> ManageWorkspacePermission(NoteTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            return CommandResult<NoteTemplateViewModel>.Instance(viewModel);
            var _templateBusiness = sp.GetService<ITemplateBusiness>();
            var _templateCategoryBusiness = sp.GetService<ITemplateCategoryBusiness>();
            var template = await _templateBusiness.GetSingleById(viewModel.TemplateId);
            var templateCategory = await _templateCategoryBusiness.GetSingleById(template.TemplateCategoryId);
            if (viewModel.DataAction == DataActionEnum.Create && viewModel.VersionNo == 1 && templateCategory.Code == "GENERAL_DOCUMENT")
            {
                await UpdateDocumentCount(viewModel, udf, uc, sp);
            }
            if (viewModel.NoteStatusCode == "NOTE_STATUS_INPROGRESS" || viewModel.NoteStatusCode == "NOTE_STATUS_DRAFT")
            {
                if (templateCategory.Code == "GENERAL_FOLDER" || templateCategory.Code == "WORKSPACE_GENERAL" || templateCategory.Code == "GENERAL_DOCUMENT")
                {
                    var _documentpermissionBusiness = sp.GetService<IDocumentPermissionBusiness>();
                    if (viewModel.DataAction == DataActionEnum.Create)
                    {

                        var permissionData = new DocumentPermissionViewModel
                        {
                            DataAction = DataActionEnum.Create,
                            PermissionType = DmsPermissionTypeEnum.Allow,
                            Access = DmsAccessEnum.FullAccess,
                            AppliesTo = DmsAppliesToEnum.ThisFolderSubFoldersAndFiles,
                            IsInherited = false,
                            PermittedUserId = viewModel.OwnerUserId,
                            NoteId = viewModel.NoteId,
                            Isowner = true,
                        };
                        await _documentpermissionBusiness.Create(permissionData);
                        if (viewModel.ParentNoteId.IsNotNull())
                        {
                            await _documentpermissionBusiness.ManageInheritedPermission(viewModel.NoteId, viewModel.ParentNoteId);
                        }
                    }
                }
            }
            if (templateCategory.Code == "GENERAL_DOCUMENT" && viewModel.TemplateCode != "GENERAL_DOCUMENT")
            {
                await UpdateIsLatestRevision(viewModel, udf, uc, sp);
            }
            await UpdateWorkspaceId(viewModel, udf, uc, sp);

            return CommandResult<NoteTemplateViewModel>.Instance(viewModel);
        }

        public async Task<CommandResult<NoteTemplateViewModel>> UpdateIsLatestRevision(NoteTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
           // return CommandResult<NoteTemplateViewModel>.Instance(viewModel);
            var _noteBusiness = sp.GetService<INoteBusiness>();
            var note = await _noteBusiness.GetSingleById(viewModel.NoteId);
            var noteTempModel = new NoteTemplateViewModel();
            noteTempModel.TemplateCode = note.TemplateCode;
            noteTempModel.NoteId = note.Id;
            noteTempModel.SetUdfValue = true;
            var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
            var rowData1 = notemodel.ColumnList.ToDictionary(x => x.Name, x => x.Value);
            // //var rowData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(viewModel.Json);
            var isLatestRevision = rowData1.ContainsKey("isLatestRevision") ? Convert.ToString(rowData1["isLatestRevision"]) : "";
            if (viewModel.NoteStatusCode == "NOTE_STATUS_INPROGRESS" && isLatestRevision != "True")
            {
                rowData1["isLatestRevision"] = "True";
                var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData1);
                var update = await _noteBusiness.EditNoteUdfTable(notemodel, data1, notemodel.UdfNoteTableId);
                // var data= Newtonsoft.Json.JsonConvert.SerializeObject(rowData);
                // var update = await _noteBusiness.EditNoteUdfTable(viewModel, data, viewModel.UdfNoteTableId);
            }
            //await _noteBusiness.UpdateOldIsLatestRevision(viewModel.NoteNo, viewModel.NoteId, viewModel.TemplateId);
            var hangfireScheduler = sp.GetService<IHangfireScheduler>();
            await hangfireScheduler.Enqueue<HangfireScheduler>(x => x.UpdateOldIsLatestRevision(viewModel.NoteSubject, viewModel.NoteId, viewModel.TemplateId, uc.ToIdentityUser()));
            return CommandResult<NoteTemplateViewModel>.Instance(viewModel);
        }

        public async Task<CommandResult<NoteTemplateViewModel>> UpdateWorkspaceId(NoteTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            return CommandResult<NoteTemplateViewModel>.Instance(viewModel);
            var _noteBusiness = sp.GetService<INoteBusiness>();
            var _tableMetadataBusiness = sp.GetService<ITableMetadataBusiness>();
            var noteViewModel = await _noteBusiness.GetSingleById(viewModel.NoteId);
            var workspaceId = await _noteBusiness.UpdateWorkspaceId(noteViewModel);
            if (workspaceId.IsNotNullAndNotEmpty())
            {
                var udftabledata = await _tableMetadataBusiness.GetTableDataByColumn("WORKSPACE_GENERAL", "", "NtsNoteId", workspaceId);
                var noteTempModel = new NoteTemplateViewModel();
                noteTempModel.TemplateCode = viewModel.TemplateCode;
                noteTempModel.NoteId = viewModel.NoteId;
                noteTempModel.SetUdfValue = true;
                var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                var rowData1 = notemodel.ColumnList.ToDictionary(x => x.Name, x => x.Value);
                var k = rowData1.FirstOrDefault(x => x.Key == "WorkspaceId" || x.Key == "workspaceId");
                if (udftabledata.IsNotNull() && k.IsNotNull() && k.Key.IsNotNullAndNotEmpty())
                {
                    rowData1[k.Key] = Convert.ToString(udftabledata["Id"]);
                    // rowData1[workspaceId] = Convert.ToString(udftabledata["Id"]);

                }

                var data1 = Newtonsoft.Json.JsonConvert.SerializeObject(rowData1);
                var update = await _noteBusiness.EditNoteUdfTable(notemodel, data1, notemodel.UdfNoteTableId);
            }


            return CommandResult<NoteTemplateViewModel>.Instance(viewModel);
        }


        public async Task<CommandResult<NoteTemplateViewModel>> CreateWorkflowService(NoteTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            var _noteBusiness = sp.GetService<INoteBusiness>();
            var _lovBusiness = sp.GetService<ILOVBusiness>();
            var _templateBusiness = sp.GetService<ITemplateBusiness>();
            var _dmsBusiness = sp.GetService<IDMSDocumentBusiness>();
            if (viewModel.DataAction == DataActionEnum.Create && viewModel.VersionNo == 1)
            {
                var rowData = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(viewModel.Json);
                var docApprovalStatus = Convert.ToString(rowData["documentApprovalStatusType"]);
                var docApprovalLov = await _lovBusiness.GetSingleById(docApprovalStatus);
                if (docApprovalLov.Code == "ApproveThroughDMS" && viewModel.ReferenceId.IsNullOrEmpty() && viewModel.ReferenceType != ReferenceTypeEnum.NTS_Service)
                {
                    var template = await _templateBusiness.GetSingleById(viewModel.TemplateId);
                    if (template.WorkFlowTemplateId.IsNotNullAndNotEmpty())
                    {
                        var workFlowTemplate = await _templateBusiness.GetSingleById(template.WorkFlowTemplateId);
                        var res = await _dmsBusiness.CreateWorkflowService(workFlowTemplate, viewModel.NoteId, rowData);
                        if (res.IsSuccess)
                        {
                            var note = await _noteBusiness.GetSingleById(viewModel.NoteId);
                            if (note.IsNotNull())
                            {
                                note.ReferenceId = res.Item.ServiceId;
                                note.ReferenceType = ReferenceTypeEnum.NTS_Service;
                                await _noteBusiness.Edit(note);
                            }
                        }
                    }
                }
            }
            return CommandResult<NoteTemplateViewModel>.Instance(viewModel);
        }
        private async Task<CommandResult<NoteTemplateViewModel>> UpdateDocumentCount(NoteTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            if (viewModel.DataAction == DataActionEnum.Create && viewModel.VersionNo == 1)
            {
                var _tagBusiness = sp.GetService<INtsTagBusiness>();
                var _dmsBusiness = sp.GetService<IDMSDocumentBusiness>();
                var parentlist = await _dmsBusiness.GetAllParentByNoteId(viewModel.NoteId);
                foreach (var parent in parentlist)
                {
                    var tag = new NtsTagViewModel { NtsId = parent.Id, NtsType = NtsTypeEnum.Note, TagSourceReferenceId = viewModel.NoteId };
                    var res = await _tagBusiness.Create(tag);
                }
            }
            return CommandResult<NoteTemplateViewModel>.Instance(viewModel);
        }

        public async Task<CommandResult<NoteTemplateViewModel>> RemoveReferenceId(NoteTemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
        {
            if (viewModel.DataAction == DataActionEnum.Edit && viewModel.VersionNo > 1)
            {
                var _noteBusiness = sp.GetService<INoteBusiness>();
                var noteDetails = await _noteBusiness.GetSingleById(viewModel.NoteId);
                if (noteDetails.IsNotNull())
                {
                    noteDetails.ReferenceId = null;
                    noteDetails.DataAction = DataActionEnum.Edit;
                    var res = await _noteBusiness.Edit(noteDetails);
                    if (!res.IsSuccess)
                    {
                        return CommandResult<NoteTemplateViewModel>.Instance(viewModel, false, $"Error in Removing ReferenceId to Document");
                    }
                }

            }
            return CommandResult<NoteTemplateViewModel>.Instance(viewModel);
        }
    }

}
