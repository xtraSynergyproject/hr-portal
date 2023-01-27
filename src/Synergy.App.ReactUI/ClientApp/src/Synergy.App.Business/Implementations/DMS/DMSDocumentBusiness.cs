using AutoMapper;
using Synergy.App.ViewModel;
using Synergy.App.Business.Interface.DMS;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Synergy.App.Business
{
    public class DMSDocumentBusiness : BusinessBase<NoteViewModel, NtsNote>, IDMSDocumentBusiness
    {
        private readonly IRepositoryQueryBase<FolderViewModel> _queryRepo1;
        private readonly IRepositoryQueryBase<DataTable> _queryRepodt;
        private ITemplateBusiness _templateBusiness;
        private ITemplateCategoryBusiness _templateCategoryBusiness;
        private INoteBusiness _noteBusiness;
        private IServiceBusiness _serviceBusiness;
        private ITableMetadataBusiness _tableMetadataBusiness;
        private readonly IRepositoryQueryBase<DocumentListViewModel> _querydocList;
        private ILOVBusiness _LOV;
        private readonly IRepositoryQueryBase<WorkspaceViewModel> _queryWorkSpace;
        private IFileBusiness _fileBusiness;
        private INtsStagingBusiness _stagingBusiness;
        private IRepositoryQueryBase<NoteLinkShareViewModel> _QueryShareLink;
        private IDocumentManagementQueryBusiness _documentManagementQueryBusiness;
        //IUserContext _userContext;
        public DMSDocumentBusiness(IRepositoryBase<NoteViewModel, NtsNote> repo, ITemplateBusiness templateBusiness,
            ITemplateCategoryBusiness templateCategoryBusiness, INoteBusiness noteBusiness,
            ITableMetadataBusiness tableMetadataBusiness, IRepositoryQueryBase<DataTable> queryRepodt,
            IRepositoryQueryBase<FolderViewModel> queryRepo1, IServiceBusiness serviceBusiness,
            IDocumentManagementQueryBusiness documentManagementQueryBusiness,
             IMapper autoMapper, IRepositoryQueryBase<DocumentListViewModel> querydocList, ILOVBusiness LOV,
             IRepositoryQueryBase<WorkspaceViewModel> queryWorkSpace, IFileBusiness fileBusiness, INtsStagingBusiness stagingBusiness
            , IUserContext userContext, IRepositoryQueryBase<NoteLinkShareViewModel> QueryShareLink) : base(repo, autoMapper)
        {
            _queryRepo1 = queryRepo1;
            _templateBusiness = templateBusiness;
            _templateCategoryBusiness = templateCategoryBusiness;
            _noteBusiness = noteBusiness;
            _serviceBusiness = serviceBusiness;
            _tableMetadataBusiness = tableMetadataBusiness;
            _querydocList = querydocList;
            _queryRepodt = queryRepodt;
            _LOV = LOV;
            _queryWorkSpace = queryWorkSpace;
            _fileBusiness = fileBusiness;
            _QueryShareLink = QueryShareLink;
            _stagingBusiness = stagingBusiness;
            _documentManagementQueryBusiness = documentManagementQueryBusiness;
        }
        public async Task<IList<FolderViewModel>> GetUserWorkspaceTreeDataNew(string userId, string parentId)
        {
            var result = await GetFoldersAndDocumentsNew(userId, DocumentQueryTypeEnum.Folder, parentId);

            return result.Where(x => x.FolderType != FolderTypeEnum.Document /*&& x.AppliesTo != DmsAppliesToEnum.AllDocuments*/).ToList();
        }
        public async Task<List<FolderViewModel>> GetFoldersAndDocumentsNew(string userId, DocumentQueryTypeEnum documentQueryType = DocumentQueryTypeEnum.Folder, string parentId = null,string id=null)
        {
            var cypher = "";
            if (documentQueryType == DocumentQueryTypeEnum.Folder)
            {
                cypher = await _documentManagementQueryBusiness.GetFoldersAndDocumentsQueryNew(documentQueryType, userId, parentId);
            }
            else if (documentQueryType == DocumentQueryTypeEnum.Document)
            {
                cypher = await GetDocumentsQueryByParentFolderIdNew(parentId, userId,id);
            }
            // var cypher = await GetFoldersAndDocumentsQueryNew(documentQueryType, userId);


            var list = await _queryRepo1.ExecuteQueryList(cypher, new Dictionary<string, object> { { "Status", StatusEnum.Active }, { "UserId", userId }/*, { "Companyd", CompanyId }*/ });
            list = list.DistinctBy(x => x.Id).ToList();


            ManageParentHierarchyNew(list);
            return list.Where(x => x.PermissionType == DmsPermissionTypeEnum.Allow).ToList();
        }
        public async Task<List<FolderAndDocumentViewModel>> GetAllFolderAndDocumentByUserId(string userId)
        {
            //            var udfs = await _noteBusiness.GetUdfQuery(null, "GENERAL_DOCUMENT", null, null, "attachment,DocumentId", "documentApprovalStatusType,documentApprovalStatusType", "code,code", "issueCodes,issueCodes"
            //                , "documentApprovalStatus,documentApprovalStatus", "stageStatus,stageStatus", "discipline,discipline", "OutgoingIssueCodes,OutgoingIssueCodes", "vendorList,vendorList", "projectFolder,projectFolder", "ChangeRequestId,ChangeRequestServiceId");
            //            var cypher = string.Concat(@" Select ws.""Id"" as Id
            //                ,n.""NoteSubject"" as Name                 
            //                ,tm.""Code"" as FolderCode
            //                ,ws.""Id"" as WorkspaceId
            //                ,'true' as IsSelfWorkspace
            //                ,false as IsWorkspaceAdmin
            //                ,true as IsAssignedWorkspace
            //                ,'Workspace' as FolderType
            //                ,0 as PermissionType
            //                ,2 as Access
            //				,'' as InheritedFrom
            //				,2 as AppliesTo
            //                ,u.""Id"" as OwnerUserId
            //                ,true as IsOwner
            //                ,pn.""Id"" as ParentId
            //                ,pn.""NoteSubject"" as ParentName
            //				,n.""IsArchived"" as IsArchived
            //				,0 as DocCount
            //                ,n.""DisablePermissionInheritance"" as DisablePermissionInheritance
            //				,n.""NoteSubject"" as DocumentName
            //                ,n.""NoteDescription"" as Description
            //                ,ws.""CreatedDate"" as CreatedDate
            //				,ws.""VersionNo"" as NoteVersionNo
            //				,n.""LockStatus"" as LockStatus
            //				,n.""LastUpdatedDate"" as LastUpdatedDate
            //				,coalesce(n.""NoteSubject"",n.""NoteDescription"",'') as Title				
            //				,tm.""Name"" as DocumentType
            //                ,tm.""Code"" as TemplateMasterCode
            //                ,tm.""Id"" as TemplateMasterId				
            //				,tm.""Id"" as DocumentTypeId
            //				,null as DocumentId,null as FileName
            //				,null as StatusName,null as NoteStatus				
            //				,null as OwnerUser, null as CreatedByUser, null as UpdatedByUser
            //				,null as ServiceId,null as WorkflowNo,null as WorkflowName
            //				,null as WorkflowStatus, null as WorkflowTemplateId
            //				, ws.""SequenceOrder"" as SequenceNo,n.""NoteNo"" as NoteNo


            //				from public.""User"" as u
            //				 join public.""NtsNote"" as n on n.""OwnerUserId""=u.""Id"" and u.""Id""='{UserId}' and  (n.""IsArchived"" <>'true' or n.""IsArchived"" is null) and n.""IsDeleted""=false
            //				 join cms.""N_GENERAL_FOLDER_WORKSPACE"" as ws on ws.""NtsNoteId""=n.""Id"" and ws.""IsDeleted""=false
            //				 join public.""LOV"" as lv on lv.""Id""=ws.""TypeId"" and lv.""IsDeleted""=false and  lv.""Code"" in ('MY_WORKSPACE')
            //				 join public.""Template"" as tm on tm.""Id""=n.""TemplateId"" and tm.""IsDeleted""=false
            //				left join public.""TemplateCategory"" as tc on tc.""Id""=tm.""TemplateCategoryId""  and tc.""IsDeleted""=false and tc.""Code""='GENERAL_FOLDER'
            //				left join public.""NtsNote"" as pn on pn.""Id""=n.""ParentNoteId"" and pn.""IsDeleted""=false

            //				Union 
            //                select ws.""Id"" as Id
            //                ,n.""NoteSubject"" as Name                
            //                ,tm.""Code"" as FolderCode,
            //                ws.""Id"" as WorkspaceId
            //                ,false as IsSelfWorkspace
            //                ,true as IsWorkspaceAdmin
            //                ,true as IsAssignedWorkspace
            //                ,'Workspace' as FolderType
            //                ,0 as PermissionType
            //                ,2 as Access
            //				,'' as InheritedFrom
            //				,2 as AppliesTo
            //                ,null as OwnerUserId
            //                ,false as IsOwner
            //                ,pn.""Id"" as ParentId
            //                ,pn.""NoteSubject"" as ParentName
            //				,n.""IsArchived"" as IsArchived
            //				,0 as DocCount
            //                ,n.""DisablePermissionInheritance"" as DisablePermissionInheritance
            //				,n.""NoteSubject"" as DocumentName
            //                ,n.""NoteDescription"" as Description
            //                ,ws.""CreatedDate"" as CreatedDate
            //				,ws.""VersionNo"" as NoteVersionNo
            //				,n.""LockStatus"" as LockStatus
            //				,n.""LastUpdatedDate"" as LastUpdatedDate
            //				,coalesce(n.""NoteSubject"", n.""NoteDescription"", '') as Title				
            //				,tm.""Name"" as DocumentType
            //                ,tm.""Code"" as TemplateMasterCode
            //                ,tm.""Id"" as TemplateMasterId				
            //				,tm.""Id"" as DocumentTypeId
            //				,null as DocumentId
            //                ,null as FileName
            //				,null as StatusName
            //                ,null as NoteStatus
            //				,null as OwnerUser
            //                ,null as CreatedByUser
            //                ,null as UpdatedByUser
            //				,null as ServiceId
            //                ,null as WorkflowNo
            //                ,null as WorkflowName
            //				,null as WorkflowStatus
            //                , null as WorkflowTemplateId
            //				, ws.""SequenceOrder"" as SequenceNo
            //                ,n.""NoteNo"" as NoteNo
            //				from 
            //			    public.""User"" as u
            //			    left join public.""DocumentPermission"" as per  on per.""PermittedUserId""=u.""Id"" and u.""Id""='{UserId}' and per.""IsDeleted""=false
            //			    left join public.""UserGroupUser"" as ugu on  ugu.""UserId""=u.""Id"" and ugu.""IsDeleted""=false
            //			    left join public.""UserGroup"" as ug on  ug.""Id""=ugu.""Id"" and ug.""IsDeleted""=false
            //			    left join public.""DocumentPermission"" as per1 on  per1.""PermittedUserGroupId""=ug.""Id"" and per1.""IsDeleted""=false
            //				join public.""NtsNote"" as n on per1.""NoteId""=n.""Id"" or per.""NoteId""=n.""Id"" and  (n.""IsArchived"" <>'true' or n.""IsArchived"" is null) and n.""IsDeleted""=false
            //				join cms.""N_GENERAL_FOLDER_WORKSPACE"" as ws on ws.""NtsNoteId""=n.""Id""  and ws.""IsDeleted""=false
            //				join public.""LOV"" as lv on lv.""Id""=ws.""TypeId"" and lv.""IsDeleted""=false and  lv.""Code"" not in ('ADMIN_WORKSPACE')
            //				join public.""Template"" as tm on tm.""Id""=n.""TemplateId"" and tm.""IsDeleted""=false
            //				left join public.""TemplateCategory"" as tc on tc.""Id""=tm.""TemplateCategoryId""  and tc.""IsDeleted""=false and tc.""Code""='GENERAL_FOLDER'
            //				left join public.""NtsNote"" as pn on pn.""Id""=n.""ParentNoteId"" and pn.""IsDeleted""=false
            //Union
            //				Select ws.""Id"" as Id,n.""NoteSubject"" as Name

            //				,tm.""Code"" as FolderCode,ws.""Id"" as WorkspaceId
            //                ,'false' as IsSelfWorkspace,false as IsWorkspaceAdmin,true as IsAssignedWorkspace,'Workspace' as FolderType
            //                ,case when u.""Id"" is not null then np.""PermissionType"" else np2.""PermissionType"" end as PermissionType
            //                ,case when u.""Id"" is not null then np.""Access"" else np2.""Access"" end as Access
            //               ,case when u.""Id"" is not null then np.""InheritedFrom"" else np2.""InheritedFrom"" end as InheritedFrom
            //                ,case when u.""Id"" is not null then np.""AppliesTo"" else np2.""AppliesTo"" end as AppliesTo
            //                ,null as OwnerUserId,false as IsOwner,pn.""Id"" as ParentId,pn.""NoteSubject"" as ParentName
            //				,n.""IsArchived"" as IsArchived
            //				,0 as DocCount
            //                ,n.""DisablePermissionInheritance"" as DisablePermissionInheritance
            //                ,n.""NoteSubject"" as DocumentName,n.""NoteDescription"" as Description,ws.""CreatedDate"" as CreatedDate
            //				,ws.""VersionNo"" as NoteVersionNo
            //				,n.""LockStatus"" as LockStatus
            //				,ws.""LastUpdatedDate"" as LastUpdatedDate	
            //				,coalesce(n.""NoteSubject"", n.""NoteDescription"", '') as Title				
            //				,tm.""Name"" as DocumentType,tm.""Code"" as TemplateMasterCode,tm.""Id"" as TemplateMasterId

            //				,tm.""Id"" as DocumentTypeId
            //				, null as DocumentId,null as FileName
            //				,null as StatusName,null as NoteStatus

            //				,null as OwnerUser, null as CreatedByUser, null as UpdatedByUser
            //				,null as ServiceId,null as WorkflowNo,null as WorkflowName
            //				,null as WorkflowStatus, null as WorkflowTemplateId
            //				, ws.""SequenceOrder"" as SequenceNo,n.""NoteNo"" as NoteNo


            //				from public.""User"" as u
            //                left join public.""DocumentPermission"" as np on np.""PermittedUserId""=u.""Id"" and np.""IsDeleted""=false	and u.""Id""='{UserId}' and np.""IsDeleted""=false			
            //                left join public.""UserGroupUser"" as npwg on npwg.""UserId""=u.""Id"" and npwg.""IsDeleted""=false
            //				left join public.""DocumentPermission"" as np2 on np2.""PermittedUserGroupId""=npwg.""UserGroupId""  and np2.""IsDeleted""=false
            //				join public.""NtsNote"" as n on n.""Id""=np2.""NoteId"" or n.""Id""=np.""NoteId"" and  (n.""IsArchived"" <>'true' or n.""IsArchived"" is null) and n.""IsDeleted""=false
            //				join cms.""N_GENERAL_FOLDER_WORKSPACE"" as ws on ws.""NtsNoteId""=n.""Id"" and ws.""IsDeleted""=false
            //				join public.""LOV"" as lv on lv.""Id""=ws.""TypeId"" and lv.""IsDeleted""=false and lv.""Code""<>'ADMIN_WORKSPACE'
            //				join public.""Template"" as tm on tm.""Id""=n.""TemplateId"" and tm.""IsDeleted""=false and tm.""Code""='WORKSPACE_GENERAL'
            //				join public.""TemplateCategory"" as tc on tc.""Id""=tm.""TemplateCategoryId""  and tc.""IsDeleted""=false and tc.""Code""='GENERAL_FOLDER'

            //				left join public.""NtsNote"" as pn on pn.""Id""=n.""ParentNoteId"" and pn.""IsDeleted""=false
            //				union
            //				select f.""Id"" as Id,f.""NoteSubject"" as Name

            //				,tm.""Code"" as FolderCode,null as WorkspaceId
            //                ,'false' as IsSelfWorkspace,false as IsWorkspaceAdmin,false as IsAssignedWorkspace,'Folder' as FolderType
            //                ,0 as PermissionType,2 as Access
            //				,'' as InheritedFrom
            //				,2 as AppliesTo
            //                ,u.""Id"" as OwnerUserId,true as IsOwner,pn.""Id"" as ParentId,pn.""NoteSubject"" as ParentName
            //				,f.""IsArchived"" as IsArchived
            //				,0 as DocCount
            //                ,f.""DisablePermissionInheritance"" as DisablePermissionInheritance
            //				,f.""NoteSubject"" as DocumentName,f.""NoteDescription"" as Description,f.""CreatedDate"" as CreatedDate
            //				,f.""VersionNo"" as NoteVersionNo
            //				,f.""LockStatus"" as LockStatus
            //				,f.""LastUpdatedDate"" as LastUpdatedDate
            //				,coalesce(f.""NoteSubject"", f.""NoteDescription"", '') as Title

            //				,tm.""Name"" as DocumentType,tm.""Code"" as TemplateMasterCode,tm.""Id"" as TemplateMasterId

            //				,tm.""Id"" as DocumentTypeId
            //				, null as DocumentId,null as FileName
            //				,null as StatusName,null as NoteStatus


            //				,null as OwnerUser, null as CreatedByUser, null as UpdatedByUser
            //				,null as ServiceId,null as WorkflowNo,null as WorkflowName
            //				,null as WorkflowStatus, null as WorkflowTemplateId
            //				, f.""SequenceOrder"" as SequenceNo,f.""NoteNo"" as NoteNo

            //				from public.""User"" as u
            //				 join public.""NtsNote"" as f on f.""OwnerUserId""=u.""Id"" and  (f.""IsArchived"" <>'true' or f.""IsArchived"" is null) and f.""IsDeleted""=false and u.""Id""='{UserId}'
            //				 join cms.""N_GENERAL_FOLDER_GENERALFOLDER"" as ws on ws.""NtsNoteId""=f.""Id"" and ws.""IsDeleted""=false 
            //				 --left join public.""LOV"" as lv on lv.""Id""=ws.""TypeId"" and lv.""IsDeleted""=false
            //				 join public.""Template"" as tm on tm.""Id""=f.""TemplateId"" and tm.""IsDeleted""=false and tm.""Code""='GENERAL_FOLDER'
            //				 join public.""TemplateCategory"" as tc on tc.""Id""=tm.""TemplateCategoryId""  and tc.""IsDeleted""=false and tc.""Code""='GENERAL_FOLDER'
            //				left join public.""NtsNote"" as pn on pn.""Id""=f.""ParentNoteId"" and pn.""IsDeleted""=false

            //				union
            //				 select f.""Id"" as Id,f.""NoteSubject"" as Name

            //				 ,tm.""Code"" as FolderCode,null as WorkspaceId
            //                ,'false' as IsSelfWorkspace,false as IsWorkspaceAdmin,false as IsAssignedWorkspace,'Folder' as FolderType
            //                ,case when npu.""Id"" is not null then np.""PermissionType"" else np2.""PermissionType"" end as PermissionType
            //                ,case when npu.""Id"" is not null then np.""Access"" else np2.""Access"" end as Access
            //                ,case when npu.""Id"" is not null then np.""InheritedFrom"" else np2.""InheritedFrom"" end as InheritedFrom
            //                ,case when npu.""Id"" is not null then np.""AppliesTo"" else np2.""AppliesTo"" end as AppliesTo
            //                ,null as OwnerUserId,false as IsOwner,pn.""Id"" as ParentId,pn.""NoteSubject"" as ParentName
            //				,f.""IsArchived"" as IsArchived
            //				,0 as DocCount
            //                ,f.""DisablePermissionInheritance"" as DisablePermissionInheritance
            //				,f.""NoteSubject"" as DocumentName,f.""NoteDescription"" as Description,f.""CreatedDate"" as CreatedDate
            //				,f.""VersionNo"" as NoteVersionNo
            //				,f.""LockStatus"" as LockStatus
            //				,f.""LastUpdatedDate"" as LastUpdatedDate
            //				,coalesce(f.""NoteSubject"", f.""NoteDescription"", '') as Title

            //				,tm.""Name"" as DocumentType,tm.""Code"" as TemplateMasterCode,tm.""Id"" as TemplateMasterId

            //				,tm.""Id"" as DocumentTypeId
            //				, null as DocumentId,null as FileName
            //				,null as StatusName,null as NoteStatus

            //			   ,null as OwnerUser, null as CreatedByUser, null as UpdatedByUser
            //				,null as ServiceId,null as WorkflowNo,null as WorkflowName
            //				,null as WorkflowStatus, null as WorkflowTemplateId
            //				, f.""SequenceOrder"" as SequenceNo,f.""NoteNo"" as NoteNo

            //				from 

            //				  public.""NtsNote"" as f 
            //				 join cms.""N_GENERAL_FOLDER_GENERALFOLDER"" as ws on ws.""NtsNoteId""=f.""Id"" and  (f.""IsArchived"" <>'true' or f.""IsArchived"" is null) and f.""IsDeleted""=false

            //				 join public.""Template"" as tm on tm.""Id""=f.""TemplateId"" and tm.""IsDeleted""=false and tm.""Code""='GENERAL_FOLDER'
            //				 join public.""TemplateCategory"" as tc on tc.""Id""=tm.""TemplateCategoryId""  and tc.""IsDeleted""=false and tc.""Code""='GENERAL_FOLDER'
            //				left join public.""DocumentPermission"" as np on np.""NoteId""=f.""Id"" and np.""IsDeleted""=false and np.""IsDeleted""=false
            //				left join public.""User"" as npu on npu.""Id""=np.""PermittedUserId"" and npu.""UserId""='{UserId}'	and npu.""IsDeleted""=false	
            //				left join public.""DocumentPermission"" as np2 on np2.""NoteId""=f.""Id"" and np2.""IsDeleted""=false
            //				left join public.""UserGroupUser"" as npwg on npwg.""UserGroupId""=np2.""PermittedUserGroupId"" and npwg.""IsDeleted""=false
            //				left join public.""User"" as npwgu on npwgu.""Id""=npwg.""UserId""	and npwgu.""UserId""='{UserId}'	and npwgu.""IsDeleted""=false		
            //				left join public.""NtsNote"" as pn on pn.""Id""=f.""ParentNoteId"" where f.""OwnerUserId""<>'{UserId}'	
            //Union 
            //                select f.""Id"" as Id,f.""NoteSubject"" as Name

            //                ,tm.""Code"" as FolderCode,null as WorkspaceId
            //                ,'false' as IsSelfWorkspace,false as IsWorkspaceAdmin,false as IsAssignedWorkspace,'Document' as FolderType
            //                ,'0' as PermissionType,'2' as Access
            //				,'' as InheritedFrom
            //				,'3' as AppliesTo
            //                ,u.""Id"" as OwnerUserId,true as IsOwner,pn.""Id"" as ParentId,pn.""NoteSubject"" as ParentName
            //				,f.""IsArchived"" as IsArchived
            //				,0 as DocCount
            //                ,f.""DisablePermissionInheritance"" as DisablePermissionInheritance
            //                ,f.""NoteSubject"" as DocumentName,f.""NoteDescription"" as Description,f.""CreatedDate"" as CreatedDate
            //                ,f.""VersionNo"" as NoteVersionNo
            //				,f.""LockStatus"" as LockStatus
            //				,f.""LastUpdatedDate"" as LastUpdatedDate
            //                ,coalesce(f.""NoteSubject"", f.""NoteDescription"", '') as Title

            //                ,tm.""Name"" as DocumentType,tm.""Code"" as TemplateMasterCode,tm.""Id"" as TemplateMasterId

            //				,tm.""Id"" as DocumentTypeId
            //                , udf.""DocumentId"" as DocumentId,gen.""FileName"" as FileName
            //                ,lov.""Name"" as StatusName,lov.""Code"" as NoteStatus

            //				,u.""Name"" as OwnerUser, u.""Name"" as CreatedByUser, u.""Name"" as UpdatedByUser
            //                ,s.""Id"" as ServiceId,s.""ServiceNo"" as WorkflowNo,s.""ServiceSubject"" as WorkflowName
            //                ,null as WorkflowStatus, null as WorkflowTemplateId
            //                , f.""SequenceOrder"" as SequenceNo,f.""NoteNo"" as NoteNo

            //                from 
            //                public.""User"" as u
            //				join public.""NtsNote"" as f on f.""OwnerUserId""=u.""Id"" and u.""Id""='{UserId}' and f.""IsDeleted""=false and  (f.""IsArchived"" <>'true' or f.""IsArchived"" is null)
            //				join public.""LOV"" as lov on lov.""Id""=f.""NoteStatusId"" and lov.""IsDeleted""=false
            //				left join (" + udfs + @") as udf on udf.""NtsNoteId""=f.""Id"" 

            //				join public.""Template"" as tm on tm.""Id""=f.""TemplateId"" and tm.""IsDeleted""=false --and tm.""Code""='GENERAL_FOLDER'
            //				join public.""TemplateCategory"" as tc on tc.""Id""=tm.""TemplateCategoryId""  and tc.""IsDeleted""=false and tc.""Code""='GENERAL_DOCUMENT'
            //				join public.""NtsNote"" as pn on pn.""Id""=f.""ParentNoteId"" and pn.""IsDeleted""=false
            //				left join public.""NtsService"" as s on s.""Id""=f.""ParentServiceId"" and s.""IsDeleted""=false
            //				left join public.""NtsService"" as cs on cs.""Id""=udf.""ChangeRequestServiceId"" and cs.""IsDeleted""=false
            //				left join public.""File"" as gen on gen.""Id""=udf.""DocumentId"" and gen.""IsDeleted""=false
            //				left join public.""Template"" as ws on ws.""Id""=tm.""WorkFlowTemplateId"" and ws.""IsDeleted""=false
            //				left join public.""NtsTag"" as ntg on ntg.""NtsId""=f.""Id"" and ntg.""IsDeleted""=false
            //				left join public.""NtsNote"" as tg on tg.""Id""=ntg.""TagId"" and tg.""IsDeleted""=false
            //				left join public.""NtsNote"" as tgc on tgc.""Id""=tg.""ParentNoteId"" and tgc.""IsDeleted""=false
            //				union
            //                Select f.""Id"" as Id,f.""NoteSubject"" as Name

            //                ,tm.""Code"" as FolderCode,null as WorkspaceId
            //                ,'false' as IsSelfWorkspace,false as IsWorkspaceAdmin,false as IsAssignedWorkspace,'Document' as FolderType
            //                ,0 as PermissionType,2 as Access
            //				,'' as InheritedFrom
            //				,3 as AppliesTo
            //                ,u.""Id"" as OwnerUserId,true as IsOwner,pn.""Id"" as ParentId,pn.""NoteSubject"" as ParentName
            //				,f.""IsArchived"" as IsArchived
            //				,0 as DocCount
            //                ,f.""DisablePermissionInheritance"" as DisablePermissionInheritance
            //                ,f.""NoteSubject"" as DocumentName,f.""NoteDescription"" as Description,f.""CreatedDate"" as CreatedDate
            //                ,f.""VersionNo"" as NoteVersionNo
            //				,f.""LockStatus"" as LockStatus
            //				,f.""LastUpdatedDate"" as LastUpdatedDate
            //                ,coalesce(f.""NoteSubject"", f.""NoteDescription"", '') as Title

            //                ,tm.""Name"" as DocumentType,tm.""Code"" as TemplateMasterCode,tm.""Id"" as TemplateMasterId

            //				,tm.""Id"" as DocumentTypeId
            //                , udf.""DocumentId"" as DocumentId,gen.""FileName"" as FileName
            //                ,lov.""Name"" as StatusName,lov.""Code"" as NoteStatus

            //			   ,u.""Name"" as OwnerUser, u.""Name"" as CreatedByUser, u.""Name"" as UpdatedByUser
            //                ,s.""Id"" as ServiceId,s.""ServiceNo"" as WorkflowNo,s.""ServiceSubject"" as WorkflowName
            //                ,null as WorkflowStatus, null as WorkflowTemplateId

            //                , f.""SequenceOrder"" as SequenceNo

            //				,f.""NoteNo"" as NoteNo

            //                from 
            //                public.""User"" as u
            //				join public.""NtsNote"" as f on f.""OwnerUserId""<>u.""Id"" and u.""Id""='{UserId}' and  (f.""IsArchived"" <>true or f.""IsArchived"" is null)
            //				join public.""User"" as ou on f.""OwnerUserId""=ou.""Id"" and ou.""IsDeleted""=false	
            //				join public.""LOV"" as lov on lov.""Id""=f.""NoteStatusId"" and lov.""IsDeleted""=false	

            //				join public.""Template"" as tm on tm.""Id""=f.""TemplateId"" and tm.""IsDeleted""=false --and tm.""Code""='GENERAL_FOLDER'
            //				join public.""TemplateCategory"" as tc on tc.""Id""=tm.""TemplateCategoryId""  and tc.""IsDeleted""=false and tc.""Code""='GENERAL_DOCUMENT'
            //				left join (" + udfs + @") as udf on udf.""NtsNoteId""=f.""Id""  	

            //				join public.""NtsNote"" as pn on pn.""Id""=f.""ParentNoteId"" and pn.""IsDeleted""=false	
            //				left join public.""DocumentPermission"" as np on np.""NoteId""=f.""Id"" and np.""IsDeleted""=false and np.""IsDeleted""=false	
            //				left join public.""User"" as npu on npu.""Id""=np.""PermittedUserId""	and npu.""Id""=u.""Id""	and np.""IsDeleted""=false	
            //				left join public.""DocumentPermission"" as np2 on np2.""NoteId""=f.""Id"" and np2.""IsDeleted""=false	
            //				left join public.""UserGroupUser"" as npwg on npwg.""UserGroupId""=np2.""PermittedUserGroupId"" and npwg.""IsDeleted""=false	
            //				left join public.""User"" as npwgu on npwgu.""Id""=npwg.""UserId"" and npwgu.""Id""=u.""Id"" and npwgu.""IsDeleted""=false				
            //				 left join public.""NtsService"" as s on s.""Id""=f.""ParentServiceId"" and s.""IsDeleted""=false
            //				 left join public.""NtsService"" as cs on cs.""Id""=udf.""ChangeRequestServiceId"" and cs.""IsDeleted""=false
            //				left join public.""File"" as gen on gen.""Id""=udf.""DocumentId"" and gen.""IsDeleted""=false
            //				left join public.""Template"" as ws on ws.""Id""=tm.""WorkFlowTemplateId"" and ws.""IsDeleted""=false


            //                ");
            //            var list = await _queryRepo1.ExecuteQueryList<FolderAndDocumentViewModel>(cypher, new Dictionary<string, object> { { "Status", StatusEnum.Active }, { "UserId", userId } });
            //            list = list.DistinctBy(x => x.Id).ToList();


            //ManageParentHierarchyNew(list);
   
            var list = await _documentManagementQueryBusiness.GetAllFolderAndDocumentByUserId( userId);

            return list;//.Where(x => x.PermissionType == DmsPermissionTypeEnum.Allow).ToList();
        }
        private void ManageParentHierarchyNew(List<FolderViewModel> parents)
        {
            var denyFoldersWithDoc = parents.Where(x => x.DocCount > 0 || (x.ParentId.IsNotNull() && x.PermissionType == DmsPermissionTypeEnum.Allow && x.FolderCode != "GENERAL_DOCUMENT")).ToList();
            UpdateAllParentPermission(denyFoldersWithDoc, parents);
        }
        private void UpdateAllParentPermission(List<FolderViewModel> denyFoldersWithDoc, List<FolderViewModel> parents)
        {
            foreach (var item in denyFoldersWithDoc)
            {
                if (item.PermissionType == null || item.PermissionType == DmsPermissionTypeEnum.Deny)
                {
                    item.PermissionType = DmsPermissionTypeEnum.Allow;
                    item.Access = DmsAccessEnum.ReadOnly;
                    item.AppliesTo = DmsAppliesToEnum.OnlyThisFolder;
                }
                var parentWithoutPerm = parents.Where(x => x.Id == item.ParentId).ToList();
                if (parentWithoutPerm.IsNotNull() && parentWithoutPerm.Count > 0)
                {
                    UpdateAllParentPermission(parentWithoutPerm, parents);
                }
            }
        }

        public async Task<List<WorkspaceViewModel>> GetDocuments(string userId, string search)
        {
            //            var query = $@"with recursive Document as(
            //select n.""Id"" as id,n.""NoteSubject"" as NoteSubject,n.""ParentNoteId"" as ParentNoteId ,n.""CreatedDate"" as dateCreated,n.""TemplateCode"" as TemplateCode, 'Document' as Type
            //from public.""NtsNote"" as n
            //join public.""Template"" as t on t.""Id""=n.""TemplateId"" and n.""IsDeleted""=false  and t.""IsDeleted""=false
            //join public.""TemplateCategory"" as tc on tc.""Id""=t.""TemplateCategoryId"" and tc.""IsDeleted""=false and tc.""Code""='GENERAL_DOCUMENT'
            //where n.""IsArchived""=false and n.""ParentNoteId"" is not null #DOCSEARCH#

            //    union all

            //    select n.""Id"" as id, n.""NoteSubject"" as name, n.""ParentNoteId"" as parentId, n.""CreatedDate"" as dateCreated, n.""TemplateCode"" as TemplateCode, 'Folder' as Type
            //from public.""NtsNote"" as n
            //     join Document as d on n.""Id""=d.ParentNoteId
            //	)
            //	select distinct * from Document order by Type ";




            var result = await _documentManagementQueryBusiness.GetDocuments( userId,  search);
            return result;
        }
        private async Task<string> GetDocumentsQueryByParentFolderIdNew(string parentId, string UserId,string id)
        {
            var udfs = await _noteBusiness.GetUdfQuery(null, "GENERAL_DOCUMENT", null, null, "attachment,DocumentId", "documentApprovalStatusType,documentApprovalStatusType", "code,code", "issueCodes,issueCodes"
               , "documentApprovalStatus,documentApprovalStatus", "stageStatus,stageStatus", "discipline,discipline", "OutgoingIssueCodes,OutgoingIssueCodes", "vendorList,vendorList", "projectFolder,projectFolder", "ChangeRequestId,ChangeRequestServiceId");
            var query = await _documentManagementQueryBusiness.GetDocumentsQueryByParentFolderIdNew( parentId,  UserId,  id,  udfs);
            return query;
        }
       

        public async Task<List<FolderViewModel>> GetAllByParent(string UserId, string parentId)
        {
            var udfs = await _noteBusiness.GetUdfQuery(null, "GENERAL_DOCUMENT", null, null, "attachment,DocumentId", "documentApprovalStatusType,documentApprovalStatusType", "code,code", "issueCodes,issueCodes"
              , "documentApprovalStatus,documentApprovalStatus", "stageStatus,stageStatus", "discipline,discipline", "OutgoingIssueCodes,OutgoingIssueCodes", "vendorList,vendorList", "projectFolder,projectFolder", "ChangeRequestId,ChangeRequestServiceId", "workspaceId,WorkspaceId");
            
            
            var list = await _documentManagementQueryBusiness.GetAllByParent( UserId,  parentId,  udfs);

            list = list.OrderByDescending(x => x.Access).DistinctBy(x => x.Id).ToList();            
            //list = list.DistinctBy(x => x.Id).ToList();

            ManageParentHierarchyNew(list);
            return list.Where(x => x.PermissionType == DmsPermissionTypeEnum.Allow).ToList();
        }
        public async Task<List<FolderViewModel>> GetFirstLevelWorkspacesByUser(string UserId)
        {

            var list = await _documentManagementQueryBusiness.GetFirstLevelWorkspacesByUser(UserId);
            list = list.OrderByDescending(x => x.Access).DistinctBy(x => x.Id).ToList();


            ManageParentHierarchyNew(list);
            return list.Where(x => x.PermissionType == DmsPermissionTypeEnum.Allow).ToList();
        }

        public async Task<List<FolderViewModel>> GetAllChildWorkspaceAndFolder(string UserId, string parentId)
        {
            var udfs = await _noteBusiness.GetUdfQuery(null, "GENERAL_DOCUMENT", null, null, "attachment,DocumentId", "fileAttachment,fileAttachmentId", "documentApprovalStatusType,documentApprovalStatusType", "code,code", "issueCodes,issueCodes"
              , "documentApprovalStatus,documentApprovalStatus", "stageStatus,stageStatus", "discipline,discipline", "OutgoingIssueCodes,OutgoingIssueCodes", "vendorList,vendorList", "projectFolder,projectFolder", "ChangeRequestId,ChangeRequestServiceId", "workspaceId,WorkspaceId");


            var list = await _documentManagementQueryBusiness.GetAllChildWorkspaceAndFolder( UserId,  parentId,  udfs);
            list = list.OrderByDescending(x => x.Access).DistinctBy(x => x.Id).ToList();
            ManageParentHierarchyNew(list);
            return list.Where(x => x.PermissionType == DmsPermissionTypeEnum.Allow).ToList();
        }
        public async Task<List<FolderViewModel>> GetAllChildWorkspaceFolderAndDocument(string UserId, string parentId)
        {
            var udfs = await _noteBusiness.GetUdfQuery(null, "GENERAL_DOCUMENT", null, null, "attachment,DocumentId", "fileAttachment,fileAttachmentId", "documentApprovalStatusType,documentApprovalStatusType", "code,code", "issueCodes,issueCodes"
              , "documentApprovalStatus,documentApprovalStatus", "stageStatus,stageStatus", "discipline,discipline", "OutgoingIssueCodes,OutgoingIssueCodes", "vendorList,vendorList", "projectFolder,projectFolder", "ChangeRequestId,ChangeRequestServiceId", "workspaceId,WorkspaceId");


            var list = await _documentManagementQueryBusiness.GetAllChildWorkspaceFolderAndDocument( UserId,  parentId,  udfs);
            list = list.OrderByDescending(x => x.Access).DistinctBy(x => x.Id).ToList();
            //list = list.DistinctBy(x => x.Id).ToList();

            ManageParentHierarchyNew(list);
            return list.Where(x => x.PermissionType == DmsPermissionTypeEnum.Allow).ToList();
        }
        public async Task<List<FolderViewModel>> GetAllChildWorkspaceFolderAndFiles(string UserId, string parentId)
        {
            var udfs = await _noteBusiness.GetUdfQuery(null, "GENERAL_DOCUMENT", null, null, "attachment,DocumentId", "fileAttachment,fileAttachmentId", "documentApprovalStatusType,documentApprovalStatusType", "code,code", "issueCodes,issueCodes"
              , "documentApprovalStatus,documentApprovalStatus", "stageStatus,stageStatus", "discipline,discipline", "OutgoingIssueCodes,OutgoingIssueCodes", "vendorList,vendorList", "projectFolder,projectFolder", "ChangeRequestId,ChangeRequestServiceId", "workspaceId,WorkspaceId");
      

            var list = await _documentManagementQueryBusiness.GetAllChildWorkspaceFolderAndFiles( UserId,  parentId,  udfs);
            list = list.OrderByDescending(x => x.Access).DistinctBy(x => x.Id).ToList();
            //list = list.DistinctBy(x => x.Id).ToList();

            ManageParentHierarchyNew(list);
            return list.Where(x => x.PermissionType == DmsPermissionTypeEnum.Allow).ToList();
        }
        public async Task<List<FolderViewModel>> GetAllGeneralWorkspaceData()
        {

            var list = await _documentManagementQueryBusiness.GetAllGeneralWorkspaceData();
            return list.ToList();
        }
        public async Task<List<FolderViewModel>> GetDocumentVersions(string noteId)
        {

            var list = await _documentManagementQueryBusiness.GetDocumentVersions(noteId);
            return list.ToList();
        }
        public async Task<List<FolderViewModel>> GetAllChildbyParent(string parentId)
        {

            var list = await _documentManagementQueryBusiness.GetAllChildbyParent(parentId);
            return list.ToList();
        }
        public async Task<List<FolderViewModel>> CheckDocumentExist(string parentId)
        {

            var list = await _documentManagementQueryBusiness.CheckDocumentExist(parentId);
            return list.ToList();
        }
        public async Task<DataTable> DocumentReportDataWithFilter(string templateId, string noteNo, string projectNo, string docDesc)
        {
            var list = new DataTable();
            var templatlist = await _templateBusiness.GetSingleById(templateId);
            if (templatlist.IsNotNull())
            {
                var tablemetadata = await _tableMetadataBusiness.GetSingleById(templatlist.TableMetadataId);
                var where = $@" and ""{tablemetadata.Name}"".""isLatestRevision""='True' ";
                if (noteNo.IsNotNullAndNotEmpty())
                {
                    where += $@" and ""NtsNote"".""NoteNo""='{noteNo}'";
                }
                if (docDesc.IsNotNullAndNotEmpty())
                {
                    where += $@" and ""NtsNote"".""NoteDescription""='{docDesc}'";
                }
                var newquery = await _noteBusiness.GetSelectQuery(tablemetadata, where, null, null);
                var res = await _queryRepodt.ExecuteQueryDataTable(newquery, null);
                return res;
            }
            return list;

        }

        public async Task<DataTable> DocumentReportDetailDataWithFilter(string templateId, string noteNo)
        {
            var list = new DataTable();
            var templatlist = await _templateBusiness.GetSingleById(templateId);
            if (templatlist.IsNotNull())
            {
                var tablemetadata = await _tableMetadataBusiness.GetSingleById(templatlist.TableMetadataId);
                var where = $@" and ""{tablemetadata.Name}"".""isLatestRevision""='False' and ""NtsNote"".""NoteNo""='{noteNo}'";
                var newquery = await _noteBusiness.GetSelectQuery(tablemetadata, where, null, null);
                var res = await _queryRepodt.ExecuteQueryDataTable(newquery, null);
                return res;
            }
            return list;

        }
        private async Task<List<string>> GetAllDocumentsDMSReportByRevesion(string userId)
        {


            var list = await _documentManagementQueryBusiness.GetAllDocumentsDMSReportByRevesion(userId);
            return list;
        }
        public async Task<IList<DocumentListViewModel>> DocumentSubmittedReportData(string userId, string stageStatus, string discipline, string revesion, DateTime? fromDate, DateTime? toDate, int skip = 0, int take = 0)
        {
            var result = new List<DocumentListViewModel>();
            var Ids = await GetAllDocumentsDMSReportByRevesion(userId);

            var documentIds = string.Join(",", Ids);
            documentIds = documentIds.Replace(",", "','");
            if (stageStatus.IsNotNullAndNotEmpty() && stageStatus.ToLower() == "qp")
            {
 
                var result2 = await _documentManagementQueryBusiness.DocumentSubmittedReportData( documentIds,  discipline,  revesion);/*.DistinctBy(e => new { e.NoteNo, e.Revision }).ToList()*/;

                if (fromDate.IsNotNull() && toDate.IsNotNull())
                {
                    result2 = result2.Where(x => x.SubmittedDate >= fromDate.Value.Date && x.SubmittedDate <= toDate.Value.Date).ToList();
                }

                return result2;
            }
            else if (stageStatus.IsNotNullAndNotEmpty() && stageStatus.ToLower() == "technip")
            {

                return new List<DocumentListViewModel>();
            }
            else if (stageStatus.IsNotNullAndNotEmpty() && stageStatus.ToLower() == "galfar")
            {
                var udfs = await _noteBusiness.GetUdfQuery(null, "GENERAL_DOCUMENT", null, "PROJECT_DOCUMENTS", "incomingTransmittalNumber,incomingTransmittalNumber", "issueCodes,issueCodes"
               , "stageStatus,stageStatus", "discipline,discipline", "revision,revision", "qpDueDate,qpDueDate", "incomingTransmittalDate,incomingTransmittalDate");

                var result2 = await _documentManagementQueryBusiness.DocumentSubmittedReportData( documentIds,  discipline,  revesion,  udfs)/*.DistinctBy(e => new { e.NoteNo, e.Revision }).ToList()*/;

                if (fromDate.IsNotNull() && toDate.IsNotNull())
                {
                    result2 = result2.Where(x => x.SubmittedDate >= fromDate.Value.Date && x.SubmittedDate <= toDate.Value.Date).ToList();
                }

                return result2;
            }
            else if (stageStatus.IsNotNullAndNotEmpty() && stageStatus.ToLower() != "all" && stageStatus.ToLower() != "galfar" && stageStatus.ToLower() != "technip" && stageStatus.ToLower() != "qp")
            {

                return new List<DocumentListViewModel>();
            }
            else
            {



                var udfs = await _noteBusiness.GetUdfQuery(null, "GENERAL_DOCUMENT", null, "PROJECT_DOCUMENTS", "incomingTransmittalNumber,incomingTransmittalNumber", "issueCodes,issueCodes"
               , "stageStatus,stageStatus", "discipline,discipline", "revision,revision", "qpDueDate,qpDueDate", "incomingTransmittalDate,incomingTransmittalDate");

                var result2 = await _documentManagementQueryBusiness.DocumentSubmittedReport( documentIds,  discipline,  revesion,  udfs);/*.DistinctBy(e => new { e.NoteNo, e.Revision }).ToList()*/

                if (fromDate.IsNotNull() && toDate.IsNotNull())
                {
                    result2 = result2.Where(x => x.SubmittedDate >= fromDate.Value.Date && x.SubmittedDate <= toDate.Value.Date).ToList();
                }

                return result2;
            }

        }

        public async Task<List<DocumentListViewModel>> DocumentReceivedReportData(string userId, string stageStatus, string discipline, string revesion, DateTime? fromDate, DateTime? toDate, int skip = 0, int take = 0)
        {
            var result = new List<DocumentListViewModel>();
            var Ids = await this.GetAllDocumentsDMSReportByRevesion(userId);//.OrderByDescending(e => e.CreatedDate).GroupBy(e => e.NoteNo).Select(x => x.FirstOrDefault()).ToList();
            //var documents = result1.Select(x => x.Id);
            var documentIds = string.Join(",", Ids);
            documentIds = documentIds.Replace(",", "','");
            if (stageStatus.IsNotNullAndNotEmpty() && stageStatus.ToLower() == "qp")
            {

                return new List<DocumentListViewModel>();
            }
            else if (stageStatus.IsNotNullAndNotEmpty() && stageStatus.ToLower() == "technip")
            {

                var result2 = await _documentManagementQueryBusiness.DocumentReceived( documentIds,  discipline,  revesion);
                //if (discipline.IsNotNullAndNotEmpty())
                //{

                //    result2 = result2.Where(x => x.DisciplineCode.ToLower().Contains(discipline.ToLower())).ToList();
                //}
                //if (revesion.IsNotNullAndNotEmpty())
                //{

                //    result2 = result2.Where(x => x.RevisionCode.ToLower().Contains(revesion.ToLower())).ToList();
                //}
                if (fromDate.IsNotNull() && toDate.IsNotNull())
                {
                    result2 = result2.Where(x => x.SubmittedDate >= fromDate.Value.Date && x.SubmittedDate <= toDate.Value.Date).ToList();
                }
                //foreach (var item in result2)
                //{
                //    if (item.SubmittedDate.HasValue)
                //    {
                //        var days = (item.SubmittedDate.Value - System.DateTime.Now).TotalDays;
                //        item.PendingDays = Math.Round(days);
                //    }

                //}
                return result2;
            }
            else if (stageStatus.IsNotNullAndNotEmpty() && stageStatus.ToLower() == "galfar")
            {

                var result2 = await _documentManagementQueryBusiness.DocumentReceivedData( documentIds,  discipline,  revesion);
                //if (discipline.IsNotNullAndNotEmpty())
                //{

                //    result2 = result2.Where(x => x.DisciplineCode.ToLower().Contains(discipline.ToLower())).ToList();
                //}
                //if (revesion.IsNotNullAndNotEmpty())
                //{

                //    result2 = result2.Where(x => x.RevisionCode.ToLower().Contains(revesion.ToLower())).ToList();
                //}
                if (fromDate.IsNotNull() && toDate.IsNotNull())
                {
                    result2 = result2.Where(x => x.SubmittedDate >= fromDate.Value.Date && x.SubmittedDate <= toDate.Value.Date).ToList();
                }
                //foreach (var item in result2)
                //{
                //    if (item.SubmittedDate.HasValue)
                //    {
                //        var days = (item.SubmittedDate.Value - System.DateTime.Now).TotalDays;
                //        item.PendingDays = Math.Round(days);
                //    }

                //}
                return result2;
            }
            else if (stageStatus.IsNotNullAndNotEmpty() && stageStatus.ToLower() != "all" && stageStatus.ToLower() != "galfar" && stageStatus.ToLower() != "technip" && stageStatus.ToLower() != "qp")
            {

                var result2 = await _documentManagementQueryBusiness.DocumentReceivedReport(documentIds, discipline, revesion);
                //if (discipline.IsNotNullAndNotEmpty())
                //{

                //    result2 = result2.Where(x => x.DisciplineCode.ToLower().Contains(discipline.ToLower())).ToList();
                //}
                //if (revesion.IsNotNullAndNotEmpty())
                //{

                //    result2 = result2.Where(x => x.RevisionCode.ToLower().Contains(revesion.ToLower())).ToList();
                //}
                if (fromDate.IsNotNull() && toDate.IsNotNull())
                {
                    result2 = result2.Where(x => x.SubmittedDate >= fromDate.Value.Date && x.SubmittedDate <= toDate.Value.Date).ToList();
                }
                //foreach (var item in result2)
                //{
                //    if (item.SubmittedDate.HasValue)
                //    {
                //        var days = (item.SubmittedDate.Value - System.DateTime.Now).TotalDays;
                //        item.PendingDays = Math.Round(days);
                //    }

                //}
                return result2;
            }
            else
            {
                var result2 = await _documentManagementQueryBusiness.DocumentReceivedReportData( documentIds,  discipline,  revesion);
                //if (discipline.IsNotNullAndNotEmpty())
                //{

                //    result2 = result2.Where(x => x.DisciplineCode.ToLower().Contains(discipline.ToLower())).ToList();
                //}
                //if (revesion.IsNotNullAndNotEmpty())
                //{

                //    result2 = result2.Where(x => x.RevisionCode.ToLower().Contains(revesion.ToLower())).ToList();
                //}
                if (fromDate.IsNotNull() && toDate.IsNotNull())
                {
                    result2 = result2.Where(x => x.SubmittedDate >= fromDate.Value.Date && x.SubmittedDate <= toDate.Value.Date).ToList();
                }
                //foreach (var item in result2)
                //{
                //    if (item.SubmittedDate.HasValue)
                //    {
                //        var days = (item.SubmittedDate.Value - System.DateTime.Now).TotalDays;
                //        item.PendingDays = Math.Round(days);
                //    }

                //}
                return result2;
            }

        }

        public async Task<IList<DocumentListViewModel>> DocumentStageReportData(string userId, string stageStatus, string discipline, bool IsOverdue, int skip = 0, int take = 0)
        {
            var result = new List<DocumentListViewModel>();
           
            IList<TemplateViewModel> templateList = new List<TemplateViewModel>();

            var tempCategory = await _templateCategoryBusiness.GetList(x => x.Code == "GENERAL_DOCUMENT" && x.TemplateType == TemplateTypeEnum.Note);

            foreach (var item in tempCategory)
            {
                templateList = await _templateBusiness.GetList(x => x.TemplateCategoryId == item.Id && x.PortalId == _repo.UserContext.PortalId);
            }
            //var Ids = await GetAllDocumentsDMSReport(userId);
            //var documentIds = string.Join(",", Ids);
            //documentIds = documentIds.Trim(',');
            if (stageStatus.IsNotNullAndNotEmpty() && stageStatus == "QP_STATUS")
            {
                //var cypher1 = string.Concat(@"match (n:NTS_Note) where n.Id in[" + documentIds + @"] and n.TemplateAction<>'Draft'
                //    match(n) -[:R_Note_Template]->(t: NTS_Template{ IsDeleted: 0,Status: 'Active'})                    
                //    optional match (n)<-[:R_NoteFieldValue_Note]-(nfv:NTS_NoteFieldValue{IsDeleted:0})-[:R_NoteFieldValue_TemplateField]->(ttf:NTS_TemplateField)
                //    with n,t,apoc.map.fromPairs(COLLECT([ttf.FieldName,nfv.Code])) as udfCode,apoc.map.fromPairs(COLLECT([ttf.FieldName,nfv.Value])) as udfValue
                //    where udfCode.stageStatus='QP' #DISCIPLINE# #OVERDUE#
                //    return n.Id as DocumentId,n.NoteNo as NoteNo ,n.Subject as DocumentName,n.CreatedDate as CreatedDate,t.TemplateOwner as TemplateOwner,
                //    udfValue.revision as Revision,udfCode.revision as RevisionCode,
                //    udfValue.discipline as Discipline,udfCode.discipline as DisciplineCode,
                //    coalesce(udfValue.stageStatus,udfCode.stageStatus)as StageStatus,
                //    coalesce(udfValue.outgoingIssueCodes, udfCode.outgoingIssueCodes) as IssueCode,                    
                //    udfCode.galfarTransmittalNumber as TransmittalNo,
                //    udfCode.qpDueDate as DueDate,
                //    udfCode.dateOfSubmission as SubmittedDate                    
                //    //,duration.inDays(datetime(nfv7.Code),datetime()).days as PendingDays
                //    ");

               

                var result2 = await _documentManagementQueryBusiness.DocumentStage(templateList,  discipline,  IsOverdue,  userId);

                result2 = result2.OrderByDescending(x => x.CreatedDate).ToList();

                foreach (var item in result2)
                {
                    if (item.SubmittedDate.HasValue)
                    {
                        var days = (DateTime.Now - item.SubmittedDate.Value).TotalDays;
                        item.PendingDays = Math.Round(days);
                    }
                }
                return result2;
            }
            else if (stageStatus.IsNotNullAndNotEmpty() && stageStatus == "TECHNIP_STATUS")
            {
                //var cypher1 = string.Concat(@"
                //    match (n:NTS_Note) where n.Id in[" + documentIds + @"] and n.TemplateAction<>'Draft'
                //    match(n) -[:R_Note_Template]->(t: NTS_Template{ IsDeleted: 0,Status: 'Active'})                    
                //    optional match (n)<-[:R_NoteFieldValue_Note]-(nfv:NTS_NoteFieldValue{IsDeleted:0})-[:R_NoteFieldValue_TemplateField]->(ttf:NTS_TemplateField)
                //    with n,t,apoc.map.fromPairs(COLLECT([ttf.FieldName,nfv.Code])) as udfCode,apoc.map.fromPairs(COLLECT([ttf.FieldName,nfv.Value])) as udfValue
                //    where udfCode.stageStatus='Technip' #DISCIPLINE# #OVERDUE#
                //    return n.Id as DocumentId,n.NoteNo as NoteNo ,n.Subject as DocumentName,n.CreatedDate as CreatedDate,t.TemplateOwner as TemplateOwner,
                //    udfValue.revision as Revision,udfCode.revision as RevisionCode,
                //    udfValue.discipline as Discipline,udfCode.discipline as DisciplineCode,
                //    coalesce(udfValue.stageStatus,udfCode.stageStatus)as StageStatus,
                //    coalesce(udfValue.outgoingTechnipIssueCodes, udfCode.outgoingTechnipIssueCodes) as IssueCode,                    
                //    udfCode.technipTransmittalNumber as TransmittalNo,
                //    udfCode.technipDueDate as DueDate,
                //    udfCode.outgoingTransmittalDate as SubmittedDate                    
                //    //,duration.inDays(datetime(nfv7.Code),datetime()).days as PendingDays
                //    ");



                var result2 = await _documentManagementQueryBusiness.DocumentStageData(templateList, discipline, IsOverdue, userId);

                result2 = result2.OrderByDescending(x => x.CreatedDate).ToList();

                foreach (var item in result2)
                {
                    if (item.SubmittedDate.HasValue)
                    {
                        var days = (DateTime.Now - item.SubmittedDate.Value).TotalDays;
                        item.PendingDays = Math.Round(days);
                    }
                }
                return result2;

            }
            else if (stageStatus.IsNotNullAndNotEmpty() && stageStatus == "GAL_STATUS")
            {
                //var cypher1 = string.Concat(@"
                //    match (n:NTS_Note) where n.Id in[" + documentIds + @"] and n.TemplateAction<>'Draft'
                //    match(n) -[:R_Note_Template]->(t: NTS_Template{ IsDeleted: 0,Status: 'Active'})                    
                //    optional match (n)<-[:R_NoteFieldValue_Note]-(nfv:NTS_NoteFieldValue{IsDeleted:0})-[:R_NoteFieldValue_TemplateField]->(ttf:NTS_TemplateField)
                //    with n,t,apoc.map.fromPairs(COLLECT([ttf.FieldName,nfv.Code])) as udfCode,apoc.map.fromPairs(COLLECT([ttf.FieldName,nfv.Value])) as udfValue
                //    where udfCode.stageStatus='Galfar' #DISCIPLINE# #OVERDUE#
                //    return n.Id as DocumentId,n.NoteNo as NoteNo ,n.Subject as DocumentName,n.CreatedDate as CreatedDate,t.TemplateOwner as TemplateOwner,
                //    udfValue.revision as Revision,udfCode.revision as RevisionCode,
                //    udfValue.discipline as Discipline,udfCode.discipline as DisciplineCode,
                //    coalesce(udfValue.stageStatus,udfCode.stageStatus)as StageStatus,
                //    coalesce(udfValue.code, udfValue.issueCodes,udfCode.issueCodes) as IssueCode,
                //    //case when udfValue.code is null then udfValue.issueCodes else udfValue.code end as IssueCode,
                //    coalesce(udfCode.qPTransmittalNumber,udfCode.incomingTransmittalNumber) as TransmittalNo,
                //    //case when udfCode.qPTransmittalNumber is null then udfCode.incomingTransmittalNumber else udfCode.qPTransmittalNumber end as TransmittalNo,
                //    udfCode.galfarDueDate as DueDate,
                //    coalesce(udfCode.dateOfReturn,udfCode.incomingTransmittalDate) as SubmittedDate
                //    //case when udfCode.dateOfReturn is null then udfCode.incomingTransmittalDate else udfCode.dateOfReturn end as SubmittedDate                    
                //    //,duration.inDays(datetime(case when nfv10.Code is null then nfv6.Code else nfv10.Code end),datetime()).days as PendingDays
                //    ");


                var result2 = await _documentManagementQueryBusiness.DocumentStageReport(templateList, discipline, IsOverdue, userId);

                result2 = result2.OrderByDescending(x => x.CreatedDate).ToList();

                foreach (var item in result2)
                {
                    if (item.SubmittedDate.HasValue)
                    {
                        var days = (DateTime.Now - item.SubmittedDate.Value).TotalDays;
                        item.PendingDays = Math.Round(days);
                    }
                }
                return result2;

            }
            else if (stageStatus.IsNotNullAndNotEmpty() && stageStatus.ToLower() != "all" && stageStatus != "GAL_STATUS" && stageStatus != "TECHNIP_STATUS" && stageStatus != "QP_STATUS")
            {
                //var cypher1 = string.Concat(@"
                //    match (n:NTS_Note) where n.Id in[" + documentIds + @"] and n.TemplateAction<>'Draft'
                //    match(n) -[:R_Note_Template]->(t: NTS_Template{ IsDeleted: 0,Status: 'Active'})                    
                //    optional match (n)<-[:R_NoteFieldValue_Note]-(nfv:NTS_NoteFieldValue{IsDeleted:0})-[:R_NoteFieldValue_TemplateField]->(ttf:NTS_TemplateField)
                //    with n,t,apoc.map.fromPairs(COLLECT([ttf.FieldName,nfv.Code])) as udfCode,apoc.map.fromPairs(COLLECT([ttf.FieldName,nfv.Value])) as udfValue
                //    where udfCode.stageStatus='Vendor' and udfValue.vendorList={Vendor} #DISCIPLINE# #OVERDUE#
                //    return n.Id as DocumentId,n.NoteNo as NoteNo ,n.Subject as DocumentName,n.CreatedDate as CreatedDate,t.TemplateOwner as TemplateOwner,
                //    udfValue.revision as Revision,udfCode.revision as RevisionCode,
                //    udfValue.discipline as Discipline,udfCode.discipline as DisciplineCode,
                //    coalesce(udfValue.stageStatus,udfCode.stageStatus) as StageStatus,
                //    coalesce(udfValue.outgoingTechnipIssueCodes,udfCode.outgoingTechnipIssueCodes) as IssueCode, 
                //    udfCode.technipTransmittalNumber as TransmittalNo,
                //    udfCode.technipDueDate as DueDate,
                //    udfCode.outgoingTransmittalDate as SubmittedDate,
                //    udfValue.vendorList as Vendor
                //    //,duration.inDays(datetime(nfv7.Code),datetime()).days as PendingDays
                //    ");


                var result2 = await _documentManagementQueryBusiness.DocumentDataStageReportData(templateList, discipline, IsOverdue, userId);

                result2 = result2.OrderByDescending(x => x.CreatedDate).ToList();

                foreach (var item in result2)
                {
                    if (item.SubmittedDate.HasValue)
                    {
                        var days = (DateTime.Now - item.SubmittedDate.Value).TotalDays;
                        item.PendingDays = Math.Round(days);
                    }
                }
                return result2;

            }
            else
            {
                //var cypher1 = string.Concat(@"match (n:NTS_Note) where n.Id in[" + documentIds + @"] and n.TemplateAction<>'Draft'
                //    match(n) -[:R_Note_Template]->(t: NTS_Template{ IsDeleted: 0,Status: 'Active'})                    
                //    optional match (n)<-[:R_NoteFieldValue_Note]-(nfv:NTS_NoteFieldValue{IsDeleted:0})-[:R_NoteFieldValue_TemplateField]->(ttf:NTS_TemplateField)
                //    with n,t,apoc.map.fromPairs(COLLECT([ttf.FieldName,nfv.Code])) as udfCode,apoc.map.fromPairs(COLLECT([ttf.FieldName,nfv.Value])) as udfValue
                //    where udfCode.stageStatus='QP' #DISCIPLINE# 
                //    return n.Id as DocumentId,n.NoteNo as NoteNo ,n.Subject as DocumentName,n.CreatedDate as CreatedDate,t.TemplateOwner as TemplateOwner,
                //    udfValue.revision as Revision,udfCode.revision as RevisionCode,
                //    udfValue.discipline as Discipline,udfCode.discipline as DisciplineCode,
                //    coalesce(udfValue.stageStatus,udfCode.stageStatus) as StageStatus,
                //    coalesce(udfValue.outgoingIssueCodes,udfCode.outgoingIssueCodes) as IssueCode,                    
                //    udfCode.galfarTransmittalNumber as TransmittalNo,
                //    udfCode.qpDueDate as DueDate,
                //    udfCode.dateOfSubmission as SubmittedDate,
                //    null as Vendor
                //    union 
                //    match (n:NTS_Note) where n.Id in[" + documentIds + @"] and n.TemplateAction<>'Draft'
                //    match(n) -[:R_Note_Template]->(t: NTS_Template{ IsDeleted: 0,Status: 'Active'})
                //    optional match (n)<-[:R_NoteFieldValue_Note]-(nfv:NTS_NoteFieldValue{IsDeleted:0})-[:R_NoteFieldValue_TemplateField]->(ttf:NTS_TemplateField)
                //    with n,t,apoc.map.fromPairs(COLLECT([ttf.FieldName,nfv.Code])) as udfCode,apoc.map.fromPairs(COLLECT([ttf.FieldName,nfv.Value])) as udfValue
                //    where udfCode.stageStatus='Technip' #DISCIPLINE# 
                //    return n.Id as DocumentId,n.NoteNo as NoteNo ,n.Subject as DocumentName,n.CreatedDate as CreatedDate,t.TemplateOwner as TemplateOwner,
                //    udfValue.revision as Revision,udfCode.revision as RevisionCode,
                //    udfValue.discipline as Discipline,udfCode.discipline as DisciplineCode,
                //    coalesce(udfValue.stageStatus,udfCode.stageStatus) as StageStatus,
                //    coalesce(udfValue.outgoingTechnipIssueCodes,udfCode.outgoingTechnipIssueCodes) as IssueCode,                    
                //    udfCode.technipTransmittalNumber as TransmittalNo,
                //    udfCode.technipDueDate as DueDate,
                //    udfCode.outgoingTransmittalDate as SubmittedDate,
                //    null as Vendor
                //    union
                //    match (n:NTS_Note) where n.Id in[" + documentIds + @"] and n.TemplateAction<>'Draft'
                //    match(n) -[:R_Note_Template]->(t: NTS_Template{ IsDeleted: 0,Status: 'Active'})
                //    optional match (n)<-[:R_NoteFieldValue_Note]-(nfv:NTS_NoteFieldValue{IsDeleted:0})-[:R_NoteFieldValue_TemplateField]->(ttf:NTS_TemplateField)
                //    with n,t,apoc.map.fromPairs(COLLECT([ttf.FieldName,nfv.Code])) as udfCode,apoc.map.fromPairs(COLLECT([ttf.FieldName,nfv.Value])) as udfValue
                //    where udfCode.stageStatus='Galfar' #DISCIPLINE# 
                //    return n.Id as DocumentId,n.NoteNo as NoteNo ,n.Subject as DocumentName,n.CreatedDate as CreatedDate,t.TemplateOwner as TemplateOwner,
                //    udfValue.revision as Revision,udfCode.revision as RevisionCode,
                //    udfValue.discipline as Discipline,udfCode.discipline as DisciplineCode,
                //    coalesce(udfValue.stageStatus,udfCode.stageStatus) as StageStatus,
                //    coalesce(udfValue.code, udfValue.issueCodes,udfCode.issueCodes) as IssueCode,
                //    coalesce(udfCode.qPTransmittalNumber,udfCode.incomingTransmittalNumber) as TransmittalNo,
                //    //case when udfValue.code is null then udfValue.issueCodes else udfValue.code end as IssueCode,
                //    //case when udfCode.qPTransmittalNumber is null then udfCode.incomingTransmittalNumber else udfCode.qPTransmittalNumber end as TransmittalNo,
                //    udfCode.galfarDueDate as DueDate,
                //    coalesce(udfCode.dateOfReturn,udfCode.incomingTransmittalDate) as SubmittedDate,
                //    //case when udfCode.dateOfReturn is null then udfCode.incomingTransmittalDate else udfCode.dateOfReturn end as SubmittedDate,
                //    null as Vendor
                //    union
                //    match (n:NTS_Note) where n.Id in[" + documentIds + @"] and n.TemplateAction<>'Draft'
                //    match(n) -[:R_Note_Template]->(t: NTS_Template{ IsDeleted: 0,Status: 'Active'})
                //    optional match (n)<-[:R_NoteFieldValue_Note]-(nfv:NTS_NoteFieldValue{IsDeleted:0})-[:R_NoteFieldValue_TemplateField]->(ttf:NTS_TemplateField)
                //    with n,t,apoc.map.fromPairs(COLLECT([ttf.FieldName,nfv.Code])) as udfCode,apoc.map.fromPairs(COLLECT([ttf.FieldName,nfv.Value])) as udfValue
                //    where udfCode.stageStatus='Vendor' #DISCIPLINE# 
                //    return n.Id as DocumentId,n.NoteNo as NoteNo ,n.Subject as DocumentName,n.CreatedDate as CreatedDate,t.TemplateOwner as TemplateOwner,
                //    udfValue.revision as Revision,udfCode.revision as RevisionCode,
                //    udfValue.discipline as Discipline,udfCode.discipline as DisciplineCode,
                //    coalesce(udfValue.stageStatus,udfCode.stageStatus) as StageStatus,
                //    coalesce(udfValue.outgoingTechnipIssueCodes,udfCode.outgoingTechnipIssueCodes) as IssueCode, 
                //    udfCode.technipTransmittalNumber as TransmittalNo,
                //    udfCode.technipDueDate as DueDate,
                //    udfCode.outgoingTransmittalDate as SubmittedDate,
                //    udfValue.vendorList as Vendor");




                var result2 = await _documentManagementQueryBusiness.DocumentStageReportData(templateList, discipline, IsOverdue, userId);

                if (IsOverdue == true)
                {
                    result2 = result2.Where(x => x.DueDate < DateTime.Now).ToList();
                }

                result2 = result2.OrderByDescending(x => x.CreatedDate).ToList();

                foreach (var item in result2)
                {
                    if (item.SubmittedDate.HasValue)
                    {
                        var days = (DateTime.Now - item.SubmittedDate.Value).TotalDays;
                        item.PendingDays = Math.Round(days);
                    }
                }
                return result2;
            }


        }

        public async Task<List<DocumentListViewModel>> GetAllDocumentsDMSReport(string userId)
        {

            var list = await _documentManagementQueryBusiness.GetAllDocumentsDMSReport(userId);
        
                return list;
        }

        public async Task<IList<DocumentListViewModel>> DocumentReceivedCommentsReportData(string userId, string templateId, string discipline, string revesion, DateTime fromDate, DateTime toDate, int skip = 0, int take = 0)
        {
            var result = new List<DocumentListViewModel>();
            var Ids = await GetAllDocumentsDMSReportByRevesion(userId);

            var documentIds = string.Join(",", Ids);
            documentIds = documentIds.Replace(",", "','");
            //var cypher1 = string.Concat(@"match (n:NTS_Note) where n.Id in[" + documentIds + @"] and n.TemplateAction<>'Draft'
            //        match(n) -[:R_Note_Template]->(t: NTS_Template{ IsDeleted: 0,Status: 'Active'})-[:R_TemplateRoot]->(tm:NTS_TemplateMaster{IsDeleted: 0,Status:'Active',Code:'PROJECT_DOCUMENTS'})
            //        optional match (n)<-[:R_NoteFieldValue_Note]-(nfv:NTS_NoteFieldValue{IsDeleted:0})-[:R_NoteFieldValue_TemplateField]->(ttf:NTS_TemplateField)
            //        with n,t,tm,apoc.map.fromPairs(COLLECT([ttf.FieldName,nfv.Code])) as udfCode,apoc.map.fromPairs(COLLECT([ttf.FieldName,nfv.Value])) as udfValue
            //        where tm.Code = 'PROJECT_DOCUMENTS' #DISCIPLINE# #REVESION#
            //        return n.Id as DocumentId,n.NoteNo as NoteNo ,n.Subject as DocumentName,n.CreatedDate as CreatedDate,t.TemplateOwner as TemplateOwner,tm.Id as TemplateId,
            //        udfValue.revision as Revision,udfCode.revision as RevisionCode,
            //        udfValue.discipline as Discipline,udfCode.discipline as DisciplineCode,
            //        coalesce(udfValue.stageStatus,udfCode.stageStatus)as StageStatus,
            //        coalesce(udfValue.code, udfCode.code) as IssueCode,                    
            //        udfCode.qPTransmittalNumber as TransmittalNo,
            //        udfCode.galfarDueDate as DueDate,
            //        udfCode.dateOfReturn as SubmittedDate                    
            //        union
            //        match (n:NTS_Note) where n.Id in[" + documentIds + @"] and n.TemplateAction<>'Draft'
            //        match(n) -[:R_Note_Template]->(t: NTS_Template{ IsDeleted: 0,Status: 'Active'})-[:R_TemplateRoot]->(tm:NTS_TemplateMaster{IsDeleted: 0,Status:'Active',Code:'ENGINEERING_SUBCONTRACT'})
            //        optional match (n)<-[:R_NoteFieldValue_Note]-(nfv:NTS_NoteFieldValue{IsDeleted:0})-[:R_NoteFieldValue_TemplateField]->(ttf:NTS_TemplateField)
            //        with n,t,tm,apoc.map.fromPairs(COLLECT([ttf.FieldName,nfv.Code])) as udfCode,apoc.map.fromPairs(COLLECT([ttf.FieldName,nfv.Value])) as udfValue
            //        where tm.Code = 'ENGINEERING_SUBCONTRACT' #DISCIPLINE# #REVESION#
            //        return n.Id as DocumentId,n.NoteNo as NoteNo ,n.Subject as DocumentName,n.CreatedDate as CreatedDate,t.TemplateOwner as TemplateOwner,tm.Id as TemplateId,
            //        udfValue.revision as Revision,udfCode.revision as RevisionCode,
            //        udfValue.discipline as Discipline,udfCode.discipline as DisciplineCode,
            //        coalesce(udfValue.stageStatus,udfCode.stageStatus)as StageStatus,
            //        coalesce(udfValue.code, udfCode.code) as IssueCode, 
            //        udfCode.qPTransmittalNumber as TransmittalNo,
            //        udfCode.galfarDueDate as DueDate,
            //        udfCode.dateOfReturn as SubmittedDate
            //        union
            //         match (n:NTS_Note) where n.Id in[" + documentIds + @"] and n.TemplateAction<>'Draft'
            //        match(n) -[:R_Note_Template]->(t: NTS_Template{ IsDeleted: 0,Status: 'Active'})-[:R_TemplateRoot]->(tm:NTS_TemplateMaster{IsDeleted: 0,Status:'Active',Code:'GALFAR_VENDOR'})
            //        optional match (n)<-[:R_NoteFieldValue_Note]-(nfv:NTS_NoteFieldValue{IsDeleted:0})-[:R_NoteFieldValue_TemplateField]->(ttf:NTS_TemplateField)
            //        with n,t,tm,apoc.map.fromPairs(COLLECT([ttf.FieldName,nfv.Code])) as udfCode,apoc.map.fromPairs(COLLECT([ttf.FieldName,nfv.Value])) as udfValue
            //        where tm.Code = 'GALFAR_VENDOR' #DISCIPLINE# #REVESION#
            //        return n.Id as DocumentId,n.NoteNo as NoteNo ,n.Subject as DocumentName,n.CreatedDate as CreatedDate,t.TemplateOwner as TemplateOwner,tm.Id as TemplateId,
            //        udfValue.revision as Revision,udfCode.revision as RevisionCode,
            //        udfValue.discipline as Discipline,udfCode.discipline as DisciplineCode,
            //        coalesce(udfValue.stageStatus,udfCode.stageStatus)as StageStatus,
            //        coalesce(udfValue.code, udfCode.code) as IssueCode, 
            //        udfCode.qPTransmittalNumber as TransmittalNo,
            //        udfCode.galfarDueDate as DueDate,
            //        udfCode.dateOfReturn as SubmittedDate");

            //var result = new List<DocumentListViewModel>();
            
            var templateList = new List<TemplateViewModel>();

            var tempCategory = await _templateCategoryBusiness.GetList(x => x.Code == "GENERAL_DOCUMENT" && x.TemplateType == TemplateTypeEnum.Note);

            foreach (var item in tempCategory)
            {
                templateList = await _templateBusiness.GetList(x => x.TemplateCategoryId == item.Id && x.PortalId == _repo.UserContext.PortalId);
            }


            var result2 = await _documentManagementQueryBusiness.DocumentReceivedCommentsReportData(templateList,  documentIds,  discipline,  revesion);

            result2 = result2.DistinctBy(e => new { e.NoteNo, e.Revision }).OrderByDescending(x => x.CreatedDate).ToList();

            //result2 = result2.OrderByDescending(x => x.CreatedDate).ToList();

            foreach (var item in result2)
            {
                if (item.SubmittedDate.HasValue)
                {
                    var days = (DateTime.Now - item.SubmittedDate.Value).TotalDays;
                    item.PendingDays = Math.Round(days);
                }
            }

            if (templateId.IsNotNull())
            {
                result2 = result2.Where(x => x.TemplateId == templateId).ToList();
            }

            if (fromDate.IsNotNull() && toDate.IsNotNull())
            {
                result2 = result2.Where(x => x.SubmittedDate >= fromDate && x.SubmittedDate <= toDate).ToList();
            }

            return result2;


        }

        /// <summary>
        /// pre script for Document
        /// </summary>
        /// <param name="viewModel"></param>
        public async Task<bool> ValidateRequestForInspection(NoteTemplateViewModel viewModel, dynamic udf, Dictionary<string, string> errorList)
        {


            if (viewModel.DataAction == DataActionEnum.Create)
            {
                //var rowData = JsonConvert.DeserializeObject<Dictionary<string, object>>(viewModel.Json);
                //var userId = Convert.ToString(rowData.GetValueOrDefault("UserId"))
                //;
                string locationOfInspection = Convert.ToString(udf.locationOfInspection);
                string diciplineval = Convert.ToString(udf.discipline);
                DateTime date = Convert.ToDateTime(udf.date);
                DateTime dateOfInspection = Convert.ToDateTime(udf.dateOfInspection);
                if (date.IsNotNull() && dateOfInspection.IsNotNull())
                {
                    var date1 = date;
                    var date2 = dateOfInspection;
                    if (date2 < date1)
                    {
                        errorList.Add("Validate", "Date Of Inspection Should be greater than or equal to Date");
                        return false;
                    }
                }


                if (viewModel.VersionNo == 1 && viewModel.NoteNo.IsNotNull())
                {

                    var diciplineCode = await _LOV.GetSingle(x => x.Id == diciplineval);
                    var discipline = diciplineCode.Code;

                    var dcode = 0;
                    if (discipline == "D_General")
                    {
                        dcode = 0;
                    }
                    else if (discipline == "D_Civil" || discipline == "D_Structural")
                    {
                        dcode = 1;
                    }
                    else if (discipline == "D_Electrical")
                    {
                        dcode = 2;
                    }
                    else if (discipline == "D_LossPrevention")
                    {
                        dcode = 3;
                    }
                    else if (discipline == "D_Instrumentation")
                    {
                        dcode = 4;
                    }
                    else if (discipline == "D_Mechanical" || discipline == "D_Piping")
                    {
                        dcode = 5;
                    }
                    else if (discipline == "D_Process")
                    {
                        dcode = 6;
                    }
                    else if (discipline == "D_Pipeline")
                    {
                        dcode = 7;
                    }
                    else if (discipline == "D_Telecom")
                    {
                        dcode = 8;
                    }
                    else if (discipline == "D_BuildingService")
                    {
                        dcode = 9;
                    }
                    var lCode = "";
                    if (locationOfInspection.IsNotNull())
                    {


                        var inspection = await _LOV.GetSingle(x => x.Id == locationOfInspection);
                        locationOfInspection = inspection.Code;
                        int charLocation = locationOfInspection.IndexOf("-", StringComparison.Ordinal);
                        if (charLocation > 0)
                        {
                            lCode = locationOfInspection.Substring(0, charLocation);
                        }
                    }

                    string startstr = string.Concat("4172-", dcode.ToString());



                    var maxNumber = await _documentManagementQueryBusiness.ValidateRequestForInspection(startstr);

                    if (maxNumber.IsNotNull())
                    {
                        long number = 0;
                        number = Convert.ToInt64(maxNumber) + 1;
                        var numstr = String.Format("{0:0000}", number);
                        var lastNumStr = await ValidateCustomeNotNoRFI(string.Concat("4172-", dcode.ToString()), numstr);
                        var location = string.Concat(lCode, "-", lastNumStr);
                        var rfiNo = string.Concat("4172-", dcode.ToString(), "-RFI-", location);
                        //var lastNoteNo= ValidateCustomeNotNo(rfiNo);                        
                        viewModel.NoteNo = rfiNo;
                        return true;
                    }
                    else
                    {
                        long number = 1;
                        var numstr = String.Format("{0:0000}", number);
                        var lastNumStr = await ValidateCustomeNotNoRFI(string.Concat("4172-", dcode.ToString()), numstr);
                        var location = string.Concat(lCode, "-", lastNumStr);
                        var rfiNo = string.Concat("4172-", dcode.ToString(), "-RFI-", location);
                        viewModel.NoteNo = rfiNo;
                        return true;
                    }

                }
                else
                {
                    //errorList.Add("Validate", "Date Of Inspection Should be greater than or equal to Date");
                    return false;
                }


            }
            else
            {

                return false;
            }

        }

        /// <summary>
        /// For Halul Pre Script
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="errorList"></param>

        public async Task<bool> ValidateRequestForInspectionHalul(NoteTemplateViewModel viewModel, dynamic udf, Dictionary<string, string> errorList)
        {


            if (viewModel.DataAction == DataActionEnum.Create)
            {
                // var rowData = JsonConvert.DeserializeObject<Dictionary<string, object>>(viewModel.Json);
                // var userId = Convert.ToString(rowData.GetValueOrDefault("UserId"));
                DateTime date = Convert.ToDateTime(udf.date);
                DateTime dateOfInspection = Convert.ToDateTime(udf.dateOfInspection);
                string diciplineval = Convert.ToString(udf.discipline);
                string type = Convert.ToString(udf.type);
                if (date.IsNotNull() && dateOfInspection.IsNotNull())
                {
                    var date1 = date;
                    var date2 = dateOfInspection;
                    if (date2 < date1)
                    {
                        errorList.Add("dateOfInspection", "Date Of Inspection Should be greater than or equal to Date");
                        return false;
                    }
                }
                else { return false; }
                if (viewModel.VersionNo == 1 && viewModel.NoteNo.IsNotNull())
                {
                    //var discipline = Convert.ToString(rowData.GetValueOrDefault("discipline"));

                    var diciplineCode = await _LOV.GetSingle(x => x.Id == diciplineval);
                    var discipline = diciplineCode.Code;


                    var typeCode = await _LOV.GetSingle(x => x.Id == type);
                    type = typeCode.Code;

                    //string startstr = string.Concat("4172-", dcode.ToString());



                    var maxNumber = await _documentManagementQueryBusiness.ValidateRequestForInspectionHalul( discipline);

                    if (maxNumber.IsNotNull())
                    {
                        long number = 0;
                        number = Convert.ToInt64(maxNumber) + 1;
                        var numstr = String.Format("{0:0000}", number);
                        var lastNumstr = await ValidateCustomeNotNoRFIHalul(numstr, discipline);
                        //var location = string.Concat(lCode, "-", numstr);
                        var rfiNo = string.Concat("4108-", type, "-", discipline, "-", lastNumstr);
                        //var lastNoteNo = ValidateCustomeNotNo(rfiNo);
                        viewModel.NoteNo = rfiNo;
                        return true;
                    }
                    else
                    {
                        long number = 1;
                        var numstr = String.Format("{0:0000}", number);
                        //var location = string.Concat(lCode, "-", numstr);
                        var rfiNo = string.Concat("4108-", type, "-", discipline, "-", numstr);
                        viewModel.NoteNo = rfiNo;

                        return true;
                    }

                }
                else { return false; }


            }
            else { return false; }

        }

        private async Task<string> ValidateCustomeNotNoRFI(string _firstStr, string _lastStr)
        {

            var lastStr = _lastStr;


            var maxNumber = await _documentManagementQueryBusiness.ValidateCustomeNotNoRFI( _firstStr,  _lastStr);
            if (maxNumber.IsNotNullAndNotEmpty())
            {
                long number = 0;
                number = Convert.ToInt64(maxNumber) + 1;
                var numstr = String.Format("{0:0000}", number);
                return await ValidateCustomeNotNoRFI(_firstStr, numstr);
            }
            else
            {
                return lastStr;
            }

        }

        private async Task<string> ValidateCustomeNotNoRFIHalul(string _lastStr, string discipline)
        {

            var lastStr = _lastStr;
    

            var maxNumber = await _documentManagementQueryBusiness.ValidateCustomeNotNoRFIHalul( _lastStr,  discipline);
            if (maxNumber.IsNotNullAndNotEmpty())
            {
                long number = 0;
                number = Convert.ToInt64(maxNumber) + 1;
                var numstr = String.Format("{0:0000}", number);
                return await ValidateCustomeNotNoRFIHalul(numstr, discipline);
            }
            else
            {
                return lastStr;
            }

        }

        public async Task<IList<NoteViewModel>> GetAllFiles(string ParentId, string code, string Id)
        {

            var value = await _documentManagementQueryBusiness.GetAllFiles(ParentId, code, Id);


            return value;
        }
        public async Task<IList<FolderViewModel>> GetAllParentByChildId(string ChildId, List<FolderViewModel> ParentList)
        {


            ParentList = await _documentManagementQueryBusiness.GetAllParentByChildId(ChildId, ParentList);
            //foreach (var folder in list)
            //{
            //    ParentList.Add(folder);
            //    await GetAllParentByChildId(folder.Id, ParentList);
            //}
            return ParentList;
        }
        public async Task<IList<FolderViewModel>> GetAllChildByParentId(string ParentId, List<FolderViewModel> FolderList)
        {

            var list = await _documentManagementQueryBusiness.GetAllChildByParentId(ParentId, FolderList);
            foreach (var folder in list)
            {
                FolderList.Add(folder);
                await GetAllChildByParentId(folder.Id, FolderList);
            }
            return FolderList;
        }
        public async Task<IList<FolderViewModel>> GetAllPermissionChildByParentId(string ParentId, List<FolderViewModel> FolderList)
        {

            var list = await _documentManagementQueryBusiness.GetAllPermissionChildByParentId( ParentId);
            if (list.Count>0)
            {
                FolderList.AddRange(list);
            }

 
            //var cypher = @"match (n: NTS_Note{ IsDeleted: 0,Status: 'Active'})
            //                     -[:R_Note_Parent_Note]->(p:NTS_Note{ IsDeleted: 0,Status: 'Active',Id:{ParentId}})                                                           
            //                    return n,n.Subject as Name, p.Id as ParentId";
            var list1 = await _documentManagementQueryBusiness.GetAllPermissionChildByParentIdData(ParentId);
            if (list1.Count > 0)
            {
                FolderList.AddRange(list1);
            }

            //foreach (var folder in list)
            //{
            //    FolderList.Add(folder);
            //    await GetAllChildByParentId(folder.Id, FolderList);
            //}
            return FolderList;
        }
        public async Task<IList<FolderViewModel>> GetAllPermittedChildByParentId(string UserId, string ParentId)
        {           
            var list = await _documentManagementQueryBusiness.GetAllPermittedChildByParentId( UserId, ParentId);
           
            var list1 = await _documentManagementQueryBusiness.GetAllPermittedChildByParentIdData( UserId,  ParentId);
            if (list1.Count > 0)
            {
                list.AddRange(list1);
            }
            return list;
        }
        public async Task<IList<DocumentPermissionViewModel>> GetAllNotePermissionByParentId(string ids)
        {
            var list = new List<DocumentPermissionViewModel>();
            if (ids.IsNotNullAndNotEmpty())
            {
                var id = ids.Replace(",", "','");


                list = await _documentManagementQueryBusiness.GetAllNotePermissionByParentId(id);
            }

            return list;
        }
        public async Task<IList<FolderViewModel>> GetAllChildDocumentByParentId(string ParentId, List<FolderViewModel> FolderList)
        {

            //var cypher = @"match (n: NTS_Note{ IsDeleted: 0,Status: 'Active'})
            //                     -[:R_Note_Parent_Note]->(p:NTS_Note{ IsDeleted: 0,Status: 'Active',Id:{ParentId}})                                                           
            //                    return n,n.Subject as Name, p.Id as ParentId";
            var list = await _documentManagementQueryBusiness.GetAllChildDocumentByParentId( ParentId);
            foreach (var folder in list)
            {
                FolderList.Add(folder);
                if (folder.TemaplateMasterCatCode != "GENERAL_DOCUMENT")
                {
                    await GetAllChildDocumentByParentId(folder.Id, FolderList);
                }

            }
            return FolderList;
        }

        public async Task<CommandResult<ServiceTemplateViewModel>> CreateWorkflowService(TemplateViewModel viewModel, string docId,Dictionary<string,object> rowData)
        {
            var serviceTemplate = new ServiceTemplateViewModel();
            serviceTemplate.ActiveUserId = _repo.UserContext.UserId;
            serviceTemplate.TemplateCode = viewModel.Code;
            var service = await _serviceBusiness.GetServiceDetails(serviceTemplate);

            service.ServiceSubject = "Workflow Service";
            service.OwnerUserId = _repo.UserContext.UserId;
            service.StartDate = DateTime.Now;
            service.DueDate = DateTime.Now.AddDays(10);
            service.DataAction = DataActionEnum.Create;
            service.ServiceStatusCode = "SERVICE_STATUS_INPROGRESS";
            var attachmentValue =rowData.ContainsKey("attachment")?rowData["attachment"].ToString() : rowData.ContainsKey("fileAttachment") ? rowData["fileAttachment"].ToString() :"";

            //service.udf
            // service.UdfNoteId = docId;
            dynamic exo = new System.Dynamic.ExpandoObject();
            ((IDictionary<String, Object>)exo).Add("documentId", docId);
            ((IDictionary<String, Object>)exo).Add("File", attachmentValue);
           
            //// Added ReferenceId in Note for one to many relationship by Arshad
            //service.ReferenceId = docId;
            //service.ReferenceType = ReferenceTypeEnum.NTS_Note;
            service.Json = JsonConvert.SerializeObject(exo);
            var res = await _serviceBusiness.ManageService(service);
            return res;
        }
        public async Task<CommandResult<NoteTemplateViewModel>> ManageUploadedFiles(NoteTemplateViewModel model)
        {
            List<UploadedFileResult> uploadedFiles = JsonConvert.DeserializeObject<List<UploadedFileResult>>(model.UploadedContent);
            List<UploadedFileResult> createdFolders = new List<UploadedFileResult>();
            if (uploadedFiles != null && uploadedFiles.Count > 0)
            {
                uploadedFiles.ForEach(x => x.Folders = x.RelativePath.Split('/').ToList());
                // var folderList = uploadedFiles.Select(x => x.RelativePath.Split('/')).ToList();
                // var max = folderList.Max(x => x.Length);

                foreach (var item in uploadedFiles)
                {
                    for (int i = 0; i < item.Folders.Count; i++)
                    {
                        if (i == 0)
                        {
                            if (!createdFolders.Any(x => x.FolderName == item.Folders[i] && x.ParentFolderName == "" /*&& x.RelativePath == item.RelativePath*/))
                            {

                                //   FolderNAme exists under parent id:viewModel.ParentId. if exist throw duplicate folder message
                                var resultItem = await CreateFolder(item.Folders[i], model.ParentNoteId);
                                if (resultItem.IsSuccess)
                                {
                                    createdFolders.Add(new UploadedFileResult { Id = resultItem.Item.NoteId, FolderName = item.Folders[i], ParentFolderName = "", RelativePath = "" });
                                }
                                else
                                {
                                    return CommandResult<NoteTemplateViewModel>.Instance(resultItem.Item, resultItem.IsSuccess, resultItem.Messages);
                                }
                            }

                        }
                        else if (i == item.Folders.Count - 1)
                        {
                            var parentName = item.Folders[i - 1];
                            var rp = string.Join("//", item.Folders.Take(i));
                            if (!createdFolders.Any(x => x.FolderName == item.Folders[i] && x.ParentFolderName == parentName && x.RelativePath == rp))
                            {
                                var prp = "";

                                if (i <= 1)
                                {
                                    prp = "";
                                }

                                else
                                {
                                    prp = string.Join("//", item.Folders.Take(i - 1));
                                }
                                var parent = createdFolders.FirstOrDefault(x => x.FolderName == parentName && x.RelativePath == prp);
                                if (parent != null)
                                {
                                    var fileItem = await CreateFile(item.Folders[i], parent.Id, item.FileId);

                                    if (fileItem.IsSuccess)
                                    {
                                        createdFolders.Add(new UploadedFileResult { Id = fileItem.Item.NoteId, FolderName = item.Folders[i], ParentFolderName = parentName, RelativePath = rp });
                                    }
                                    else
                                    {
                                        return CommandResult<NoteTemplateViewModel>.Instance(fileItem.Item, fileItem.IsSuccess, fileItem.Messages);
                                    }

                                }
                            }
                        }
                        else
                        {
                            var parentName = item.Folders[i - 1];
                            var rp = string.Join("//", item.Folders.Take(i));
                            if (!createdFolders.Any(x => x.FolderName == item.Folders[i] && x.ParentFolderName == parentName && x.RelativePath == rp))
                            {
                                var prp = "";

                                if (i <= 1)
                                {
                                    prp = "";
                                }
                                else
                                {
                                    prp = string.Join("//", item.Folders.Take(i - 1));
                                }
                                var parent = createdFolders.FirstOrDefault(x => x.FolderName == parentName && x.RelativePath == prp);
                                if (parent != null)
                                {
                                    var Folderitem = await CreateFolder(item.Folders[i], parent.Id);
                                    if (Folderitem.IsSuccess)
                                    {
                                        createdFolders.Add(new UploadedFileResult { Id = Folderitem.Item.NoteId, FolderName = item.Folders[i], ParentFolderName = parentName, RelativePath = rp });
                                    }
                                    else
                                    {
                                        return CommandResult<NoteTemplateViewModel>.Instance(Folderitem.Item, Folderitem.IsSuccess, Folderitem.Messages);
                                    }

                                }
                            }
                        }
                    }
                }
            }

            return CommandResult<NoteTemplateViewModel>.Instance(model);
        }
        public async Task<CommandResult<NoteTemplateViewModel>> CreateFolder(string FolderName, string ParentId)
        {
            var model = new NoteTemplateViewModel
            {
                TemplateCode = "GENERAL_FOLDER",
                DataAction = DataActionEnum.Create,
                OwnerUserId = _repo.UserContext.UserId,
                ActiveUserId = _repo.UserContext.UserId,
                RequestedByUserId = _repo.UserContext.UserId,

                //ReferenceType = NoteReferenceTypeEnum.Self,
                ParentNoteId = ParentId,
                StartDate = DateTime.Now.ApplicationNow().Date,
            };
            var newmodel = await _noteBusiness.GetNoteDetails(model);
            newmodel.NoteSubject = FolderName;
            newmodel.Description = FolderName;
            newmodel.NoteStatusCode = "NOTE_STATUS_DRAFT";
            var result = await _noteBusiness.ManageNote(newmodel);
            return result;
        }
        private async Task<CommandResult<NoteTemplateViewModel>> CreateFile(string FileName, string ParentId, string FileId,string userId=null,string templateId=null)
        {
            var templateCode = "GENERAL_DOCUMENT";
            if (templateId.IsNotNullAndNotEmpty())
            {
                var template = await _templateBusiness.GetSingleById(templateId);
                templateCode = template.Code;
            }
            
            var model = new NoteTemplateViewModel
            {
                TemplateCode = templateCode,
                DataAction = DataActionEnum.Create,
                OwnerUserId = userId.IsNotNullAndNotEmpty()? userId:_repo.UserContext.UserId,
                ActiveUserId = userId.IsNotNullAndNotEmpty() ? userId : _repo.UserContext.UserId,
                RequestedByUserId = userId.IsNotNullAndNotEmpty() ? userId : _repo.UserContext.UserId,
                //ReferenceType = NoteReferenceTypeEnum.Self,
                ParentNoteId = ParentId,
                StartDate = DateTime.Now.ApplicationNow().Date,


            };
            var newmodel = await _noteBusiness.GetNoteDetails(model);
            newmodel.NoteSubject = FileName;
            newmodel.Description = FileName;            
            newmodel.NoteStatusCode = "NOTE_STATUS_DRAFT";
            newmodel.Json = "{}";
            dynamic exo = new System.Dynamic.ExpandoObject();
            if (FileId.IsNotNullAndNotEmpty())
            {
                if(templateCode == "GENERAL_DOCUMENT" || templateCode == "ENGINEERING_SUBCONTRACT")
                {
                    ((IDictionary<String, Object>)exo).Add("fileAttachment", FileId);
                }
                else
                {
                    ((IDictionary<String, Object>)exo).Add("attachment", FileId);
                }
                
                newmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
            }
            var result = await _noteBusiness.ManageNote(newmodel);
            return result;
        }
        public async Task<CommandResult<NoteTemplateViewModel>> AddUploadedFiles(NoteTemplateViewModel model)
        {
            //List<UploadedFileResult> uploadedFiles = JsonConvert.DeserializeObject<List<UploadedFileResult>>(model.UploadedContent);
            //List<UploadedFileResult> createdFolders = new List<UploadedFileResult>();
            if (model.FileIds.IsNotNullAndNotEmpty())
            {
                var fileIds = model.FileIds.Split(',');
                if (fileIds.Count() > 0)
                {
                    foreach (var item in fileIds)
                    {
                        if (item != "")
                        {
                            var fileDetails = await _fileBusiness.GetSingleById(item);
                            var exist = await _noteBusiness.GetList(x => x.NoteSubject == fileDetails.FileName && x.ParentNoteId == model.ParentNoteId);
                            if (exist.Any())
                            {
                                if (exist.Count == 1)
                                {
                                    fileDetails.FileName = fileDetails.FileName + " - Copy";
                                }
                                else
                                {
                                    fileDetails.FileName = fileDetails.FileName + " - Copy "+"("+exist.Count+")";
                                }
                                                            
                            }
                            var fileItem = await CreateFile(fileDetails.FileName, model.ParentNoteId, item);
                            if (fileItem.IsSuccess)
                            {
                                //createdFolders.Add(new UploadedFileResult { Id = fileItem.Item.Id, FolderName = item.Folders[i], ParentFolderName = parentName, RelativePath = rp });
                            }
                            else
                            {
                                return CommandResult<NoteTemplateViewModel>.Instance(fileItem.Item, fileItem.IsSuccess, fileItem.Messages);
                            }
                        }
                    }
                }
            }
            return CommandResult<NoteTemplateViewModel>.Instance(model);
        }
        public async Task<List<NoteTemplateViewModel>> GetAllPermittedDocumentOfLoggedInUser(string userId)
        {

            var result = await _documentManagementQueryBusiness.GetAllPermittedDocumentOfLoggedInUser(userId);
            return result;
        }

        public async Task<List<AttachmentViewModel>> GetUserAttachments(string userId, string portalId)
        {
            var attachments = new List<AttachmentViewModel>();

            //var result = await _fileBusiness.GetList( x => x.ReferenceTypeCode == ReferenceTypeEnum.NTS_Note || x.ReferenceTypeCode == ReferenceTypeEnum.NTS_Service
            //    || x.ReferenceTypeCode == ReferenceTypeEnum.NTS_Task || x.ReferenceTypeCode == ReferenceTypeEnum.Page || x.ReferenceTypeCode == ReferenceTypeEnum.Form);


            var result = await _documentManagementQueryBusiness.GetUserAttachments( userId,  portalId);
            foreach (var i in result)
            {
                attachments.Add(new AttachmentViewModel
                {

                    Id = i.Id,
                    FullName = i.FullName,
                    DisplayName = (i.FullName.Length > 25) ? i.FullName.Substring(0, 25) : i.FullName,
                    ReferenceId = i.ReferenceId,
                    SnapshotMongoId = i.SnapshotMongoId,
                    ContentLength = i.ContentLength,
                    CreatedDateDisplay =  i.CreatedDate.ToString("MMMM dd yyyy"),
                    TemplateCode = i.TemplateCode
                });
            }
            return attachments;
        }

        public Task<FolderViewModel> GetNoteWithItsPermission(string noteId)
        {
            throw new NotImplementedException();
        }
       public async Task<ServiceViewModel> GetWorflowDetailByDocument(string noteId)
        {
            var udfs = await _noteBusiness.GetUdfQuery(null, "DMS_GALGARSERVICE", null, null, "documentId,documentId");
            var query = $@"select s.*,lv.""Code"" as ServiceStatusCode from public.""NtsService"" as s 
join public.""LOV"" as lv on lv.""Id""=s.""ServiceStatusId"" and lv.""IsDeleted""=false and lv.""CompanyId"" = '{_repo.UserContext.CompanyId}'
join (" + udfs + $@") as udf on udf.""NtsNoteId""=s.""UdfNoteId"" 
where udf.""documentId""='{noteId}' and s.""IsDeleted""=false and s.""CompanyId"" = '{_repo.UserContext.CompanyId}'
";
            var data = await _documentManagementQueryBusiness.GetWorflowDetailByDocument(noteId, udfs);
            return data;
        }

        public async Task<IList<FolderViewModel>> GetFolderByParent(string parentId,string noteId)
        {
            var folderlist = new List<FolderViewModel>();

            var result = await _documentManagementQueryBusiness.GetFolderByParent(parentId, noteId);
            result = result.OrderByDescending(x => x.Level).ToList();
            return result;
        }


        public async Task <List<NoteLinkShareViewModel>> GetDocumentLinksData(long id)
        {

            var result = await _documentManagementQueryBusiness.GetDocumentLinksData(id);


            return result;
        }
        public async Task<string> IsUniqueGeneralDocumentByNo(string ParentId, string code, string Id)
        {
            //var prms = Helper.GenerateCypherParameter("CompanyId", CompanyId, "Code", code, "ParentId", ParentId);
            //var cypher = @"match(c:NTS_TemplateCategory{IsDeleted:0,Status: 'Active'}) where c.Code in ['GENERAL_DOCUMENT']
            //                match (c)<-[:R_TemplateMaster_TemplateCategory]- (tr:NTS_TemplateMaster{IsDeleted:0,Status: 'Active'})
            //                match(tr) < -[:R_TemplateRoot] - (t: NTS_Template{ IsDeleted: 0,Status: 'Active'})
            //                match(t) < -[:R_Note_Template] - (n: NTS_Note{ IsDeleted: 0,Status: 'Active',CompanyId: {CompanyId}})
            //                -[:R_Note_Parent_Note]->(p:NTS_Note{ IsDeleted: 0,Status: 'Active',Id:{ParentId}})
            //                where   n.NoteNo={Code}
            //                return n.Id";

            return await _documentManagementQueryBusiness.IsUniqueGeneralDocumentByNo(ParentId, code, Id);

        }

        public async Task<bool> IsUniqueDocumentFolder(string ParentId, string code, string Id)
        {
            var result = false;
            // var prms = Helper.GenerateCypherParameter("CompanyId", CompanyId, "Code", code, "ParentId", ParentId, "Id", Id);
            //           var cypher = @"match (tr:NTS_TemplateMaster{IsDeleted:0,Status: 'Active'})  
            //                               -[:R_TemplateMaster_TemplateCategory]->(tc:NTS_TemplateCategory{Code:'GENERAL_FOLDER'})
            //                               match(tr) < -[:R_TemplateRoot] - (t: NTS_Template{ IsDeleted: 0,Status: 'Active'})  
            //                               match(t) < -[:R_Note_Template] - (n: NTS_Note{ IsDeleted: 0,Status: 'Active',CompanyId: {CompanyId}})
            //                                -[:R_Note_Parent_Note]->(p:NTS_Note{ IsDeleted: 0,Status: 'Active',Id:{ParentId}})
            //                               where n.Subject={Code} 
            //return n";

            var value =await  _documentManagementQueryBusiness.IsUniqueDocumentFolder( ParentId,code);
            if (Id.IsNotNullAndNotEmpty())
            {
                if (value != null && value.Count() > 0)
                {
                    foreach (var folder in value)
                    {
                        if (folder.Id != Id)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }

                }
                else
                {
                    return true;
                }
                return true;
            }
            else
            {
                if (value != null && value.Count() > 0)
                {

                    return false;
                }
                else
                {
                    return true;
                }
            }

            //return true;

            //if (value == 0)
            //{
            //    result = true;
            //}
            //return result;
        }
        public async Task<bool> IsUniqueGeneralDocument(string ParentId, string code, string Id)
        {
            var result = false;
            //var prms = Helper.GenerateCypherParameter("CompanyId", CompanyId, "Code", code, "ParentId", ParentId);
            //           var cypher = @"match(c:NTS_TemplateCategory{IsDeleted:0,Status: 'Active'}) where c.Code in ['GENERAL_DOCUMENT']
            //                           match (c)<-[:R_TemplateMaster_TemplateCategory]- (tr:NTS_TemplateMaster{IsDeleted:0,Status: 'Active'})
            //                           match(tr) < -[:R_TemplateRoot] - (t: NTS_Template{ IsDeleted: 0,Status: 'Active'})
            //                           match(t) < -[:R_Note_Template] - (n: NTS_Note{ IsDeleted: 0,Status: 'Active',CompanyId: {CompanyId}})
            //                           -[:R_Note_Parent_Note]->(p:NTS_Note{ IsDeleted: 0,Status: 'Active',Id:{ParentId}})
            //                           where   n.Subject={Code}
            //return n";

            var value =await _documentManagementQueryBusiness.IsUniqueGeneralDocument( ParentId,code);
            if (Id.IsNotNullAndNotEmpty())
            {
                if (value != null && value.Count() > 0)
                {
                    foreach (var doc in value)
                    {
                        if (doc.Id != Id)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }

                }
                else
                {
                    return true;
                }
                return true;
            }
            else
            {
                if (value != null && value.Count() > 0)
                {

                    return false;
                }
                else
                {
                    return true;
                }
            }
          
        }
        public async Task<IList<FolderViewModel>> GetAllParentByNoteId(string noteId)
        {

            var result = await _documentManagementQueryBusiness.GetAllParentByNoteId(noteId);
            result = result.OrderByDescending(x => x.Level).ToList();
            return result;
        }
        public async Task<IList<FolderViewModel>> GetAllDocuments()
        {
            var result = await _documentManagementQueryBusiness.GetAllDocuments();
            return result;
        }
        public async Task<bool> UpdateTagsByDocumentIds(string ids)
        {

            var result = await _documentManagementQueryBusiness.UpdateTagsByDocumentIds(ids);
            return true;
        }
        public async Task<bool> DeleteNotesbyNoteIds(string ids)
        {
            var result = await _documentManagementQueryBusiness.DeleteNotesbyNoteIds(ids);
            return true;
        }
        public async Task<bool> ArchiveNotesbyNoteIds(string ids)
        {
            var result = await _documentManagementQueryBusiness.ArchiveNotesbyNoteIds(ids);
            return true;
        }


        public async Task<bool> CheckMyWorkspaceExist(string UserId)
        {
            var data = await _documentManagementQueryBusiness.CheckMyWorkspaceExist(UserId);
            if (data!=null)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> CheckEmployeeBook(NoteTemplateViewModel model)
        {
            var rowData = JsonConvert.DeserializeObject<Dictionary<string, object>>(model.Json);
            var empId = Convert.ToString(rowData.GetValueOrDefault("EmployeeId"));

            var result = await _documentManagementQueryBusiness.CheckEmployeeBook(empId);
            if (result.IsNotNull())
            {
                return true;
            }else
            {
                return false;
            }

        }
        public async Task<List<FolderViewModel>> GetAllChildFiles(string UserId, string parentId)
        {
            var udfs = await _noteBusiness.GetUdfQuery(null, "GENERAL_DOCUMENT", null, null, "fileAttachment,fileAttachmentId");

            var list = await _documentManagementQueryBusiness.GetAllChildFiles(UserId, parentId, udfs);
            list = list.DistinctBy(x => x.Id).ToList();            
            return list.Where(x => x.PermissionType == DmsPermissionTypeEnum.Allow).ToList();
        }
        public async Task<List<FolderViewModel>> GetAllPermittedWorkspaceFolderAndDocument(string UserId)
        {

            var list = await _documentManagementQueryBusiness.GetAllPermittedWorkspaceFolderAndDocument(UserId);
            return list.Where(x => x.PermissionType == DmsPermissionTypeEnum.Allow).ToList();
        }

        public async Task<List<UserHierarchyChartViewModel>> GetDMSBookHierarchy(string parentId, int levelUpto, string hierarchyId, string permissions)
        {
            List<UserHierarchyChartViewModel> list = new List<UserHierarchyChartViewModel>();

            if (levelUpto <= 0)
            {
                var workspaces = await GetFirstLevelWorkspacesByUser(_repo.UserContext.UserId);
                list.AddRange(workspaces.Select(x => new UserHierarchyChartViewModel()
                {
                    Id=x.Id,
                    ParentId="-1",
                   Name=x.Name,
                   NodeType="Workspace",
                   CreatedDate= x.CreatedDate.ToString(),
                    DirectChildCount = x.DocCount
                }));
               

                var model = new UserHierarchyChartViewModel()
                {
                    Id = "-1",
                    Name = "Books",
                    DirectChildCount = workspaces.Count()
                };
                list.Insert(0, model);
            }
            else if (levelUpto == 1)
            {
            }
            else if (levelUpto == 2)
            {
                
            }
            foreach (var x in list)
            {
                x.Count = list.Where(x => x.ParentId == x.Id).Count();
            }
            return list;
        }
        public async Task<List<DMSTagViewModel>> GetDMSTagData()
        {
            var result = await _documentManagementQueryBusiness.GetDMSTagData();
            return result;
        }
        public async Task<List<IdNameViewModel>> GetDMSTagIdNameList()
        {
            var result = await _documentManagementQueryBusiness.GetDMSTagIdNameList();
            return result;
        }
        public async Task<DMSTagViewModel> GetDMSTagDetails(string tagId)
        {
            var result = await _documentManagementQueryBusiness.GetDMSTagDetails(tagId);
            return result;
        }
        public async Task DeleteDMSTag(string tagId)
        {
            await _documentManagementQueryBusiness.DeleteDMSTag(tagId);
        }
        public async Task<string> CreateDocumentByUpload(string path,string fileName,string ext, string parentId,string userId,string templateId, string batchId,bool isSingleUpload=false)
        {
            byte[] bytes = System.IO.File.ReadAllBytes(path);
            var fileModel = new FileViewModel
            {
                FileName = fileName,
                FileExtension = ext,
                //ContentType = bytes.ContentType,
                ContentLength = bytes.Length,
                ContentByte = bytes,
            };
            var res = await _fileBusiness.Create(fileModel);
            if (res.IsSuccess)
            {
                if (isSingleUpload)
                {
                    var model = new NoteTemplateViewModel
                    {
                        TemplateCode = "GENERAL_DOCUMENT",
                        DataAction = DataActionEnum.Create,
                        OwnerUserId = userId,
                        ActiveUserId = userId,
                        RequestedByUserId = userId,
                        ParentNoteId = parentId,
                        StartDate = DateTime.Now.ApplicationNow().Date,

                    };
                    var newmodel = await _noteBusiness.GetNoteDetails(model);
                    newmodel.NoteSubject = fileName;
                    newmodel.Description = fileName;
                    newmodel.NoteStatusCode = "NOTE_STATUS_DRAFT";
                    newmodel.Json = "{}";
                    dynamic exo = new System.Dynamic.ExpandoObject();
                    if (res.Item.Id.IsNotNullAndNotEmpty())
                    {
                        
                        ((IDictionary<String, Object>)exo).Add("fileAttachment", res.Item.Id);                      

                        newmodel.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                    }
                    var noteResult = await _noteBusiness.ManageNote(newmodel);
                    if (noteResult.IsSuccess)
                    {
                        return noteResult.Item.NoteId;
                    }
                    //create document here using params
                    //return noteid
                }
                var staging = new NtsStagingViewModel
                {
                    BatchId = batchId,
                    ReferenceId = parentId,
                    ReferenceType = ReferenceTypeEnum.NTS_Note,
                    UserId = userId,
                    TemplateId = templateId,
                    FileId = res.Item.Id,
                    StageStatus=NtsStagingEnum.Inprogress
                };
                var result = await _stagingBusiness.Create(staging);
                if (result.IsSuccess)
                {
                    return "success";
                }
                //var exist = await _noteBusiness.GetList(x => x.NoteSubject == fileName && x.ParentNoteId == parentId);
                //if (exist.Any())
                //{
                //    if (exist.Count == 1)
                //    {
                //        fileName = fileName + " - Copy";
                //    }
                //    else
                //    {
                //        fileName = fileName + " - Copy " + "(" + exist.Count + ")";
                //    }

                //}
                //var result = await CreateFile(fileName, parentId, res.Item.Id,userId,templateId);
                //if (result.IsSuccess)
                //{
                //    return true;
                //}
            }
            return "failed";
        }
        public async Task<IList<DocumentSearchViewModel>> GetAllWorkspaceFolderDocuments(DateTime? lastUpdatedDate)
        {
            var result = await _documentManagementQueryBusiness.GetAllWorkspaceFolderDocuments(lastUpdatedDate);
            return result;
        }
        public async Task<List<dynamic>> GetAllDocumentUdfDataByTableName(string tableName, string ids)
        {
            var result = await _documentManagementQueryBusiness.GetAllDocumentUdfDataByTableName(tableName,ids);
            return result;
        }

        public async Task<string> GetWorkspaceByDocumentId(string documentId)
        {
            var note = await _noteBusiness.GetSingleById(documentId);
            if (note.TemplateCode == "WORKSPACE_GENERAL")
            {
                return note.Id;
            }
            else
            {
                return await GetWorkspaceByDocumentId(note.ParentNoteId);

            }
           // return note.Id;
        }
        public async Task<List<DocumentSearchViewModel>> GetAllDmsDocumentsWithUdf()
        {
            var udfs = await _noteBusiness.GetUdfQuery(null, "GENERAL_DOCUMENT", null, null, "fileAttachment,Attachment");
            var list = await _documentManagementQueryBusiness.GetAllDocumentsWithUdf(udfs);
            return list;
        }
        public async Task<List<DocumentSearchViewModel>> GetAllDmsDocumentsWithUdf1()
        {
            var udfs = await _noteBusiness.GetUdfQuery(null, "GENERAL_DOCUMENT", null, null, "fileAttachment,Attachment");
            var list = await _documentManagementQueryBusiness.GetAllDocumentsWithUdf1(udfs);
            return list;
        }
        public async Task<List<DocumentSearchViewModel>> GetRecentDocumentWithAttachment()
        {
            var udfs = await _noteBusiness.GetUdfQuery(null, "GENERAL_DOCUMENT", null, null, "fileAttachment,Attachment");
            var list = await _documentManagementQueryBusiness.GetRecentDocumentWithAttachment(udfs);
            return list;
        }
        public async Task<List<DashboardDocumentViewModel>> GetTopPendingDocuments(string userId)
        {            
            var list = await _documentManagementQueryBusiness.GetTopPendingDocuments(userId);
            return list;
        }
        public async Task<List<IdNameViewModel>> GetAllDocumentSummary(string userId)
        {            
            var list = await _documentManagementQueryBusiness.GetAllDocumentSummary(userId);
            return list;
        }
        public async Task<List<IdNameViewModel>> GetAllDocumentAnalysis(string userId)
        {            
            var list = await _documentManagementQueryBusiness.GetAllDocumentAnalysis(userId);
            return list;
        }
        public async Task<List<DashboardDocumentViewModel>> GetTopRecentActivities(string userId)
        {
            var udfs = await _noteBusiness.GetUdfQuery(null, "GENERAL_DOCUMENT", null, null, "fileAttachment,Attachment");
            var list = await _documentManagementQueryBusiness.GetTopRecentActivities(udfs,userId);
            return list;
        }
        public async Task<List<DashboardDocumentViewModel>> GetTopRecentDocuments(string userId)
        {
            var udfs = await _noteBusiness.GetUdfQuery(null, "GENERAL_DOCUMENT", null, null, "fileAttachment,Attachment");
            var list = await _documentManagementQueryBusiness.GetTopRecentDocuments(udfs, userId);
            return list;
        }
        public async Task<int> GetDocumentTypeCount()
        {            
            var count = await _documentManagementQueryBusiness.GetDocumentTypeCount();
            return count;
        }
        public async Task<int> GetDocumentCount()
        {            
            var count = await _documentManagementQueryBusiness.GetDocumentCount();
            return count;
        }

        public async Task<List<WorkspaceDMSTree>> GetWorkspacebyUser(string userId)
        {
            var list = await _documentManagementQueryBusiness.GetWorkspacebyUser(userId);
            return list;
        }

        public async Task<List<DMSDocument>> GetDocumentbyUser(string userId)
        {
            var list = await _documentManagementQueryBusiness.GetDocumentbyUser(userId);
            return list;
        }
    }

}
