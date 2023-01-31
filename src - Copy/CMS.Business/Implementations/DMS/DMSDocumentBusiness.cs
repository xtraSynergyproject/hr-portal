using AutoMapper;
using Cms.UI.ViewModel;
using CMS.Business.Interface.DMS;
using CMS.Common;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Business
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
        //IUserContext _userContext;
        public DMSDocumentBusiness(IRepositoryBase<NoteViewModel, NtsNote> repo, ITemplateBusiness templateBusiness,
            ITemplateCategoryBusiness templateCategoryBusiness, INoteBusiness noteBusiness,
            ITableMetadataBusiness tableMetadataBusiness, IRepositoryQueryBase<DataTable> queryRepodt,
            IRepositoryQueryBase<FolderViewModel> queryRepo1, IServiceBusiness serviceBusiness,
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
                cypher = await GetFoldersAndDocumentsQueryNew(documentQueryType, userId, parentId);
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
            var query = $@"select Distinct n.""Id"" as Id, n.""NoteSubject"" as ""Name"", n.""NoteNo"" as ""NoteVersionNo"", lov.""Name"" as ""StatusName"",t.""DisplayName"" as DocumentType,n.""LastUpdatedDate"" as ""UpdatedDate"",
                      case when t.""Code"" = 'GENERAL_FOLDER' then 'GENERAL_FOLDER' else case when t.""Code"" = 'WORKSPACE_GENERAL' then 'WORKSPACE_GENERAL' else ''end end as ""FolderCode"",u.""Name"" as ""UpdatedByUser"",u.""PhotoId"" as PhotoId,'' as ""FolderPath"",n.""Id"" as ""NoteId"",n.""Id"",n.""IsArchived"" as IsArchived,
					  
					  case when tp.""Code"" = 'GENERAL_FOLDER' then 'GENERAL_FOLDER' else case when tp.""Code"" = 'WORKSPACE_GENERAL' then 'WORKSPACE_GENERAL' else ''end end as ""FileName"",
                      t.""Code"" as TemplateMasterCode,n.""ParentNoteId"" as ParentId,pn.""NoteSubject"" as ParentName,
					  up.""Name"" as LastUpdateby,n.""LastUpdatedDate"",t.""Code"",tp.""DisplayName"" as ""DocumentName""
                      from Public.""TemplateCategory"" tc inner join public.""Template"" as t on tc.""Id""=t.""TemplateCategoryId"" and t.""IsDeleted""=false and t.""CompanyId""='{_repo.UserContext.CompanyId}'
							Join public.""NtsNote"" as n on n.""TemplateId""=t.""Id"" and n.""IsDeleted""=false and ""IsArchived""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}'
							Left Join public.""User"" as u on n.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
							Left Join public.""User"" as up on up.""Id""=n.""LastUpdatedBy"" and up.""IsDeleted""=false and up.""CompanyId""='{_repo.UserContext.CompanyId}'							
                             Left Join public.""LOV"" as lov on n.""NoteStatusId""=lov.""Id"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_repo.UserContext.CompanyId}'
                inner join public.""DocumentPermission"" as np on np.""NoteId""=n.""Id"" and np.""IsDeleted""=false and np.""CompanyId""='{_repo.UserContext.CompanyId}'
                inner join public.""User"" as npu on npu.""Id""=np.""PermittedUserId"" and npu.""Id""='{userId}' and npu.""IsDeleted""=false and npu.""CompanyId""='{_repo.UserContext.CompanyId}'

                left join public.""NtsNote"" as pn on pn.""Id""=n.""ParentNoteId"" and pn.""IsDeleted""=false and pn.""CompanyId""='{_repo.UserContext.CompanyId}'
				left join Public.""Template"" as  tp on tp.""Id""=pn.""TemplateId""  and tp.""IsDeleted""=false and tp.""CompanyId""='{_repo.UserContext.CompanyId}'
                       where tc.""Code"" in ('GENERAL_DOCUMENT','GENERAL_FOLDER') and tc.""IsDeleted""=false and tc.""CompanyId""='{_repo.UserContext.CompanyId}'
					   UNION
                       select Distinct n.""Id"" as Id, n.""NoteSubject"" as ""Name"", n.""NoteNo"" as ""NoteVersionNo"", lov.""Name"" as ""StatusName"", t.""DisplayName"" as DocumentType, n.""LastUpdatedDate"" as ""UpdatedDate"",
                      case when t.""Code"" = 'GENERAL_FOLDER' then 'GENERAL_FOLDER' else case when t.""Code"" = 'WORKSPACE_GENERAL' then 'WORKSPACE_GENERAL' else ''end end as ""FolderCode"", u.""Name"" as ""UpdatedByUser"", u.""PhotoId"" as PhotoId,'' as ""FolderPath"", n.""Id"" as ""NoteId"", n.""Id"", n.""IsArchived"" as IsArchived,
					  
					  case when tp.""Code"" = 'GENERAL_FOLDER' then 'GENERAL_FOLDER' else case when tp.""Code"" = 'WORKSPACE_GENERAL' then 'WORKSPACE_GENERAL' else ''end end as ""FileName"",
                      t.""Code"" as TemplateMasterCode, n.""ParentNoteId"" as ParentId, pn.""NoteSubject"" as ParentName,
                      up.""Name"" as LastUpdateby, n.""LastUpdatedDate"", t.""Code"", tp.""DisplayName"" as ""DocumentName""
                      from Public.""TemplateCategory"" tc inner join public.""Template"" as t on tc.""Id""=t.""TemplateCategoryId""  and t.""IsDeleted""=false and t.""CompanyId""='{_repo.UserContext.CompanyId}'
							Join public.""NtsNote"" as n on n.""TemplateId""=t.""Id"" and n.""IsDeleted""=false and ""IsArchived""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}'
							Left Join public.""User"" as u on n.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
							Left Join public.""User"" as up on up.""Id""=n.""LastUpdatedBy"" and up.""IsDeleted""=false and up.""CompanyId""='{_repo.UserContext.CompanyId}'							
                             Left Join public.""LOV"" as lov on n.""NoteStatusId""=lov.""Id"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join public.""DocumentPermission"" as np2 on np2.""NoteId""=n.""Id"" and np2.""IsDeleted""=false and np2.""CompanyId""='{_repo.UserContext.CompanyId}'

                inner join public.""UserGroupUser"" as npwg on npwg.""UserGroupId""=np2.""PermittedUserGroupId"" and npwg.""IsDeleted""=false and npwg.""CompanyId""='{_repo.UserContext.CompanyId}'
				inner join public.""User"" as npwgu on npwgu.""Id""=npwg.""UserId""	and npwgu.""Id""='{userId}'	and npwgu.""IsDeleted""=false and npwgu.""CompanyId""='{_repo.UserContext.CompanyId}'		

                left join public.""NtsNote"" as pn on pn.""Id""=n.""ParentNoteId"" and pn.""IsDeleted""=false and pn.""CompanyId""='{_repo.UserContext.CompanyId}'
				left join Public.""Template"" as  tp on tp.""Id""=pn.""TemplateId"" and tp.""IsDeleted""=false and tp.""CompanyId""='{_repo.UserContext.CompanyId}'
                       where tc.""Code"" in ('GENERAL_DOCUMENT','GENERAL_FOLDER') and tc.""IsDeleted""=false and tc.""CompanyId""='{_repo.UserContext.CompanyId}' order by ""LastUpdatedDate"" desc";
            var list = await _queryRepo1.ExecuteQueryList<FolderAndDocumentViewModel>(query, null);

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

            var query = $@"with recursive Document as(
(select n.""Id"" as id,n.""NoteSubject"" as NoteSubject,n.""OwnerUserId"",n.""ParentNoteId"" as ParentNoteId ,n.""CreatedDate"" as dateCreated,n.""TemplateCode"" as TemplateCode, 'Document' as Type
from public.""NtsNote"" as n
join public.""User"" as u on n.""OwnerUserId""<>u.""Id"" and u.""Id""='{userId}' and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""Template"" as t on t.""Id""=n.""TemplateId"" and t.""IsDeleted""=false and t.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""TemplateCategory"" as tc on tc.""Id""=t.""TemplateCategoryId"" and tc.""IsDeleted""=false and tc.""CompanyId""='{_repo.UserContext.CompanyId}' and tc.""Code""='GENERAL_DOCUMENT'
JOIN ""DocumentPermission"" dp ON n.""Id"" = dp.""NoteId"" AND dp.""PermissionType"" = 0 and (dp.""PermittedUserId""='{userId}' and dp.""IsDeleted""=false and dp.""CompanyId""='{_repo.UserContext.CompanyId}'
or dp.""PermittedUserGroupId"" in (select ""UserGroupId"" from public.""UserGroupUser"" where ""UserId""='{userId}' and ""IsDeleted""=false and ""CompanyId""='{_repo.UserContext.CompanyId}'))
where n.""IsArchived""=false and n.""ParentNoteId"" is not null and n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}'
and  (n.""IsArchived"" <>true or n.""IsArchived"" is null) #DOCSEARCH#
	union
    select n.""Id"" as id,n.""NoteSubject"" as NoteSubject,n.""OwnerUserId"",n.""ParentNoteId"" as ParentNoteId ,n.""CreatedDate"" as dateCreated,n.""TemplateCode"" as TemplateCode, 'Document' as Type
from public.""NtsNote"" as n
join public.""User"" as u on n.""OwnerUserId""=u.""Id"" and u.""Id""='{userId}'and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""Template"" as t on t.""Id""=n.""TemplateId"" and t.""IsDeleted""=false and t.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""TemplateCategory"" as tc on tc.""Id""=t.""TemplateCategoryId"" and tc.""IsDeleted""=false and tc.""CompanyId""='{_repo.UserContext.CompanyId}' and tc.""Code""='GENERAL_DOCUMENT'
left join public.""DocumentPermission"" as np on np.""NoteId""=n.""Id"" and np.""IsDeleted""=false and np.""CompanyId""='{_repo.UserContext.CompanyId}' and np.""PermittedUserId""='{userId}'
                left join public.""User"" as npu on npu.""Id""=np.""PermittedUserId"" and npu.""Id""=u.""Id"" and npu.""IsDeleted""=false and npu.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join public.""DocumentPermission"" as np2 on np2.""NoteId""=n.""Id"" and np2.""IsDeleted""=false and np2.""CompanyId""='{_repo.UserContext.CompanyId}'	
				left join public.""UserGroupUser"" as npwg on npwg.""UserGroupId""=np2.""PermittedUserGroupId"" and npwg.""IsDeleted""=false and npwg.""CompanyId""='{_repo.UserContext.CompanyId}'	
				left join public.""User"" as npwgu on npwgu.""Id""=npwg.""UserId"" and npwgu.""Id""=u.""Id"" and npwgu.""IsDeleted""=false and npwgu.""CompanyId""='{_repo.UserContext.CompanyId}'
where n.""IsArchived""=false and n.""ParentNoteId"" is not null and n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}'
and  (n.""IsArchived"" <>true or n.""IsArchived"" is null) #DOCSEARCH#)

    union all

    select n.""Id"" as id, n.""NoteSubject"" as name, n.""OwnerUserId"", n.""ParentNoteId"" as parentId, n.""CreatedDate"" as dateCreated, n.""TemplateCode"" as TemplateCode, 'Folder' as Type
from public.""NtsNote"" as n
     join Document as d on n.""Id""=d.ParentNoteId
	)
	select distinct * from Document order by Type";

            var docSearch = "";
            if (search.IsNotNullAndNotEmpty())
            {
                docSearch = $@" and lower(n.""NoteSubject"") like lower('%{search}%') COLLATE ""tr-TR-x-icu""";
            }
            query = query.Replace("#DOCSEARCH#", docSearch);

            var result = await _queryWorkSpace.ExecuteQueryList(query, null);
            return result;
        }
        private async Task<string> GetDocumentsQueryByParentFolderIdNew(string parentId, string UserId,string id)
        {
            var udfs = await _noteBusiness.GetUdfQuery(null, "GENERAL_DOCUMENT", null, null, "attachment,DocumentId", "documentApprovalStatusType,documentApprovalStatusType", "code,code", "issueCodes,issueCodes"
               , "documentApprovalStatus,documentApprovalStatus", "stageStatus,stageStatus", "discipline,discipline", "OutgoingIssueCodes,OutgoingIssueCodes", "vendorList,vendorList", "projectFolder,projectFolder", "ChangeRequestId,ChangeRequestServiceId");
            var query= $@"select f.""Id"" as Id,f.""NoteSubject"" as Name
 --,tm.FileId as FileId
 ,tm.""Code"" as FolderCode,null as WorkspaceId
                ,'false' as IsSelfWorkspace,false as IsWorkspaceAdmin,false as IsAssignedWorkspace,'Document' as FolderType
                ,'0' as PermissionType,'2' as Access
				,'' as InheritedFrom
				,'3' as AppliesTo
                ,u.""Id"" as OwnerUserId,true as IsOwner,pn.""Id"" as ParentId,pn.""NoteSubject"" as ParentName
				,f.""IsArchived"" as IsArchived
				,0 as DocCount
                ,f.""DisablePermissionInheritance"" as DisablePermissionInheritance
                ,f.""NoteSubject"" as DocumentName,f.""NoteDescription"" as Description,f.""CreatedDate"" as CreatedDate
                ,f.""VersionNo"" as NoteVersionNo
				,f.""LockStatus"" as LockStatus
				,f.""LastUpdatedDate"" as UpdatedDate
                ,f.""StartDate"" as Start,coalesce(f.""NoteSubject"", f.""NoteDescription"", '') as Title
                --,coalesce(f.""StartDate"", '"", DateTime.Now.Date.ToYYY_MM_DD(), @""') as End
               -- ,coalesce(f.""ExpiryDate"", '"", DateTime.Now.LastDayOfYear().ToYYY_MM_DD(), @""') as DueDate
                ,tm.""Name"" as DocumentType,tm.""Code"" as TemplateMasterCode,tm.""Id"" as TemplateMasterId
                --,tm.""Color"" as Color
				,tm.""Id"" as DocumentTypeId
                --, case when udf.""DocumentId"" is not null then udf.""DocumentId""::int else 0 end as DocumentId
                ,udf.""DocumentId"" as DocumentId
                ,gen.""FileName"" as FileName
                ,lov.""Name"" as StatusName,lov.""Code"" as NoteStatus
                ,null as tag
				, dast.""Name"" as DocumentApprovalStatusType
                , udf.""documentApprovalStatus"" as DocumentsApprovalStatus,udf.""stageStatus"" as StageStatus,
                udf.""discipline"" as Discipline,coalesce(udf.""code"",udf.""OutgoingIssueCodes"",udf.""issueCodes"") as IssueCode
                ,udf.""vendorList"" as Vendor,udf.""projectFolder"" as ProjectFolder
				,u.""Name"" as OwnerUser, u.""Name"" as CreatedByUser, u.""Name"" as UpdatedByUser
                ,s.""Id"" as ServiceId,s.""ServiceNo"" as WorkflowNo,s.""ServiceSubject"" as WorkflowName
                ,null as WorkflowStatus, ws.""Id"" as WorkflowTemplateId,ws.""Code"" as WorkflowCode
                ,'N' as NtsId,true as IsAllDay, gen.""ContentLength"" as FileSize,0 as PhotoId
                ,ntg.""TagId"" as TagIds,tg.""NoteSubject"" as TagNames
                ,null as EnableDocumentChangeRequest, null as EnableLock
                ,ntg.""TagCategoryId"" as TagCategoryIds,tgc.""NoteSubject"" as TagCategoryNames 
                , f.""SequenceOrder"" as SequenceNo,
				'' as ChangeRequestServiceId,f.""NoteNo"" as NoteNo,wfs.""Id"" as WorkflowServiceId,wfss.""Code"" as WorkflowServiceStatus
				--, t.""DefaultView"" as DefaultView,t.""TemplateOwner"" as TemplateOwner
from 
 public.""User"" as u
				 join public.""NtsNote"" as f on f.""OwnerUserId""=u.""Id"" and u.""Id""='{UserId}' and f.""IsDeleted""=false and f.""CompanyId""='{_repo.UserContext.CompanyId}' and  (f.""IsArchived"" <> true or f.""IsArchived"" is null)
				 join public.""LOV"" as lov on lov.""Id""=f.""NoteStatusId"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_repo.UserContext.CompanyId}'
				 left join (" + udfs + $@") as udf on udf.""NtsNoteId""=f.""Id"" 
				 
				 join public.""Template"" as tm on tm.""Id""=f.""TemplateId"" and tm.""IsDeleted""=false and tm.""CompanyId""='{_repo.UserContext.CompanyId}'--and tm.""Code""='GENERAL_FOLDER'
				 join public.""TemplateCategory"" as tc on tc.""Id""=tm.""TemplateCategoryId""  and tc.""IsDeleted""=false and tc.""CompanyId""='{_repo.UserContext.CompanyId}' and tc.""Code""='GENERAL_DOCUMENT'
				 join public.""NtsNote"" as pn on pn.""Id""=f.""ParentNoteId"" and pn.""IsDeleted""=false and pn.""CompanyId""='{_repo.UserContext.CompanyId}'
 left join public.""LOV"" as dast on dast.""Id""=udf.""documentApprovalStatusType"" and dast.""IsDeleted""=false and dast.""CompanyId""='{_repo.UserContext.CompanyId}'
				 left join public.""NtsService"" as s on s.""Id""=f.""ParentServiceId"" and s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}'
				 left join public.""NtsService"" as wfs on wfs.""ReferenceId""=f.""Id"" and wfs.""IsDeleted""=false and wfs.""CompanyId""='{_repo.UserContext.CompanyId}'
left join public.""LOV"" as wfss on wfss.""Id""=wfs.""ServiceStatusId"" and wfss.""IsDeleted""=false and wfss.""CompanyId""='{_repo.UserContext.CompanyId}'
			  -- left join public.""NtsService"" as cs on cs.""Id""=udf.""ChangeRequestServiceId"" and cs.""IsDeleted""=false and cs.""CompanyId""='{_repo.UserContext.CompanyId}'
				left join public.""File"" as gen on gen.""Id""=udf.""DocumentId"" and gen.""IsDeleted""=false and gen.""CompanyId""='{_repo.UserContext.CompanyId}'
				left join public.""Template"" as ws on ws.""Id""=tm.""WorkFlowTemplateId"" and ws.""IsDeleted""=false and ws.""CompanyId""='{_repo.UserContext.CompanyId}'
				left join public.""NtsTag"" as ntg on ntg.""NtsId""=f.""Id"" and ntg.""IsDeleted""=false and ntg.""CompanyId""='{_repo.UserContext.CompanyId}'
				left join public.""NtsNote"" as tg on tg.""Id""=ntg.""TagId"" and tg.""IsDeleted""=false and tg.""CompanyId""='{_repo.UserContext.CompanyId}'
				left join public.""NtsNote"" as tgc on tgc.""Id""=tg.""ParentNoteId"" and tgc.""IsDeleted""=false and tgc.""CompanyId""='{_repo.UserContext.CompanyId}'
				where 1=1 and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'#WHERE# 
				union
Select f.""Id"" as Id,f.""NoteSubject"" as Name
--,tm.FileId as FileId
,tm.""Code"" as FolderCode,null as WorkspaceId
                ,'false' as IsSelfWorkspace,false as IsWorkspaceAdmin,false as IsAssignedWorkspace,'Document' as FolderType
                ,np.""PermissionType"" as PermissionType,np.""Access"" as Access
				,'' as InheritedFrom
				,np.""AppliesTo"" as AppliesTo
                ,u.""Id"" as OwnerUserId,true as IsOwner,pn.""Id"" as ParentId,pn.""NoteSubject"" as ParentName
				,f.""IsArchived"" as IsArchived
				,0 as DocCount
                ,f.""DisablePermissionInheritance"" as DisablePermissionInheritance
                ,f.""NoteSubject"" as DocumentName,f.""NoteDescription"" as Description,f.""CreatedDate"" as CreatedDate
                ,f.""VersionNo"" as NoteVersionNo
				,f.""LockStatus"" as LockStatus
				,f.""LastUpdatedDate"" as UpdatedDate
                ,f.""StartDate"" as Start,coalesce(f.""NoteSubject"", f.""NoteDescription"", '') as Title
                --,coalesce(f.""StartDate"", '"", DateTime.Now.Date.ToYYY_MM_DD(), @""') as End
                --,coalesce(f.""ExpiryDate"", '"", DateTime.Now.LastDayOfYear().ToYYY_MM_DD(), @""') as DueDate
                ,tm.""Name"" as DocumentType,tm.""Code"" as TemplateMasterCode,tm.""Id"" as TemplateMasterId
                --,tm.""Color"" as Color
				,tm.""Id"" as DocumentTypeId
                --, case when udf.""DocumentId"" is not null then udf.""DocumentId""::int else 0 end as DocumentId
                , udf.""DocumentId"" as DocumentId
                ,gen.""FileName"" as FileName
                ,lov.""Name"" as StatusName,lov.""Code"" as NoteStatus
                ,null as tag
				, dast.""Name"" as DocumentApprovalStatusType
                , udf.""documentApprovalStatus"" as DocumentsApprovalStatus,udf.""stageStatus"" as StageStatus,
                udf.""discipline"" as Discipline,coalesce(udf.""code"",udf.""OutgoingIssueCodes"",udf.""issueCodes"") as IssueCode
                ,udf.""vendorList"" as Vendor,udf.""projectFolder"" as ProjectFolder
			   ,u.""Name"" as OwnerUser, u.""Name"" as CreatedByUser, u.""Name"" as UpdatedByUser
                ,s.""Id"" as ServiceId,s.""ServiceNo"" as WorkflowNo,s.""ServiceSubject"" as WorkflowName
                ,null as WorkflowStatus, null as WorkflowTemplateId,ws.""Code"" as WorkflowCode
                ,'N' as NtsId,true as IsAllDay, gen.""ContentLength"" as FileSize,0 as PhotoId
                ,'' as TagIds,'' as TagNames
                ,null as EnableDocumentChangeRequest
				, null as EnableLock
                ,'' as TagCategoryIds
				,'' as TagCategoryNames 
                , f.""SequenceOrder"" as SequenceNo
				,cs.""Id"" as ChangeRequestServiceId
				,f.""NoteNo"" as NoteNo,null as WorkflowServiceId,null as WorkflowServiceStatus
				--, t.""DefaultView"" as DefaultView,t.""TemplateOwner"" as TemplateOwner				
from 
public.""User"" as u
				 join public.""NtsNote"" as f on f.""OwnerUserId""<>u.""Id"" and u.""Id""='{UserId}' and f.""IsDeleted""=false and f.""CompanyId""='{_repo.UserContext.CompanyId}' and  (f.""IsArchived"" <> true or f.""IsArchived"" is null)
				  join public.""User"" as ou on f.""OwnerUserId""=ou.""Id"" and ou.""IsDeleted""=false and ou.""CompanyId""='{_repo.UserContext.CompanyId}'
				 join public.""LOV"" as lov on lov.""Id""=f.""NoteStatusId"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_repo.UserContext.CompanyId}'	
				
				 join public.""Template"" as tm on tm.""Id""=f.""TemplateId"" and tm.""IsDeleted""=false and tm.""CompanyId""='{_repo.UserContext.CompanyId}' --and tm.""Code""='GENERAL_FOLDER'
				 join public.""TemplateCategory"" as tc on tc.""Id""=tm.""TemplateCategoryId""  and tc.""IsDeleted""=false and tc.""CompanyId""='{_repo.UserContext.CompanyId}' and tc.""Code""='GENERAL_DOCUMENT'
				  left join (" + udfs + $@") as udf on udf.""NtsNoteId""=f.""Id""  	
				 
				 join public.""NtsNote"" as pn on pn.""Id""=f.""ParentNoteId"" and pn.""IsDeleted""=false and pn.""CompanyId""='{_repo.UserContext.CompanyId}'
 left join public.""LOV"" as dast on dast.""Id""=udf.""documentApprovalStatusType"" and dast.""IsDeleted""=false and dast.""CompanyId""='{_repo.UserContext.CompanyId}'
		left join(select np1.*,case when npu is null then npwgu.""Id"" else npu.""Id"" end as ""UserId""
from  public.""DocumentPermission"" as np1
                left join public.""User"" as npu on npu.""Id""=np1.""PermittedUserId"" and npu.""Id""='{UserId}' and npu.""IsDeleted""=false and npu.""CompanyId""='{_repo.UserContext.CompanyId}' 


                left join public.""UserGroupUser"" as npwg on npwg.""UserGroupId""=np1.""PermittedUserGroupId""  and npwg.""IsDeleted""=false and npwg.""CompanyId""='{_repo.UserContext.CompanyId}' 

                left join public.""User"" as npwgu on npwgu.""Id""=npwg.""UserId""  and npwgu.""Id""='{UserId}'	 and npwgu.""IsDeleted""=false and npwgu.""CompanyId""='{_repo.UserContext.CompanyId}'

                    where np1.""IsDeleted""=false	  )
				as np on np.""NoteId""=f.""Id"" and np.""UserId""='{UserId}'
                 --left join public.""DocumentPermission"" as np on np.""NoteId""=f.""Id"" and np.""IsDeleted""=false and np.""CompanyId""='{_repo.UserContext.CompanyId}' -- and np.""PermittedUserId""='{UserId}'	
				--left join public.""User"" as npu on npu.""Id""=np.""PermittedUserId""	and npu.""Id""=u.""Id""	and npu.""IsDeleted""=false	and npu.""CompanyId""='{_repo.UserContext.CompanyId}'
				----left join public.""DocumentPermission"" as np2 on np2.""NoteId""=f.""Id"" and np2.""IsDeleted""=false	and np2.""CompanyId""='{_repo.UserContext.CompanyId}'
			-- left join public.""UserGroupUser"" as npwg on npwg.""UserGroupId""=np.""PermittedUserGroupId"" and npwg.""IsDeleted""=false and npwg.""CompanyId""='{_repo.UserContext.CompanyId}'	
				--left join public.""User"" as npwgu on npwgu.""Id""=npwg.""UserId"" and npwgu.""Id""=u.""Id"" and npwgu.""IsDeleted""=false and npwgu.""CompanyId""='{_repo.UserContext.CompanyId}'			
				 left join public.""NtsService"" as s on s.""Id""=f.""ParentServiceId"" and s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}'
				 left join public.""NtsService"" as cs on cs.""Id""=udf.""ChangeRequestServiceId"" and cs.""IsDeleted""=false and cs.""CompanyId""='{_repo.UserContext.CompanyId}'
				left join public.""File"" as gen on gen.""Id""=udf.""DocumentId"" and gen.""IsDeleted""=false and gen.""CompanyId""='{_repo.UserContext.CompanyId}'
				left join public.""Template"" as ws on ws.""Id""=tm.""WorkFlowTemplateId"" and ws.""IsDeleted""=false and ws.""CompanyId""='{_repo.UserContext.CompanyId}'
				where 1=1 and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}' --and  (npu.""Id""='{UserId}' or npwgu.""Id""='{UserId}') 
#WHERE#  ";
            var search = "";
            if (parentId.IsNotNullAndNotEmpty())
            {
                search = $@" and f.""ParentNoteId""='{parentId}'";
            }
            if (id.IsNotNullAndNotEmpty())
            {
                search = $@" and f.""Id""='{id}'";
            }
            query = query.Replace("#WHERE#", search);
            return query;
        }
        private async Task<string> GetFoldersAndDocumentsQueryNew(DocumentQueryTypeEnum documentQueryType, string UserId, string parentId)
        {
            //            var Query = @"select t.""Name"" from public.""Template"" as t
            //join public.""TemplateCategory"" as tc on tc.""Id""=t.""TemplateCategoryId"" and tc.""Code""='GENERAL_DOCUMENT'
            //and t.""IsDeleted""=false and t.""IsDeleted""=false";
            //            var templateNames = await _queryRepo1.ExecuteScalarList<string>(Query, null);
            var udfs = await _noteBusiness.GetUdfQuery(null, "GENERAL_DOCUMENT", null, null, "attachment,DocumentId", "documentApprovalStatusType,documentApprovalStatusType", "code,code", "issueCodes,issueCodes"
                , "documentApprovalStatus,documentApprovalStatus", "stageStatus,stageStatus", "discipline,discipline", "OutgoingIssueCodes,OutgoingIssueCodes", "vendorList,vendorList", "projectFolder,projectFolder", "ChangeRequestId,ChangeRequestServiceId");
            if (documentQueryType == DocumentQueryTypeEnum.Document)
            {
                var query = string.Concat($@" Select ws.""Id"" as Id,n.""NoteSubject"" as Name,ws.""Id"" as WorkspaceId
 --,tm.""FileId"" as FileId
 ,tm.""Code"" as FolderCode
                ,'true' as IsSelfWorkspace,false as IsWorkspaceAdmin,true as IsAssignedWorkspace,'Workspace' as FolderType
                ,0 as PermissionType,2 as Access
				,'' as InheritedFrom
				,2 as AppliesTo
                ,u.""Id"" as OwnerUserId,true as IsOwner,pn.""Id"" as ParentId,pn.""NoteSubject"" as ParentName
				,n.""IsArchived"" as IsArchived
				,0 as DocCount
                ,n.""DisablePermissionInheritance"" as DisablePermissionInheritance
				,n.""NoteSubject"" as DocumentName,n.""NoteDescription"" as Description,ws.""CreatedDate"" as CreatedDate
				,ws.""VersionNo"" as NoteVersionNo
				,n.""LockStatus"" as LockStatus
				,n.""LastUpdatedDate"" as UpdatedDate
				,n.""StartDate"" as Start,coalesce(n.""NoteSubject"",n.""NoteDescription"",'') as Title
				--,coalesce(ws.""StartDate"",'"", DateTime.Now.Date.ToYYY_MM_DD(), @""') as End
				--,coalesce(ws.""ExpiryDate"",'"", DateTime.Now.LastDayOfYear().ToYYY_MM_DD(), @""') as DueDate
				,tm.""Name"" as DocumentType,tm.""Code"" as TemplateMasterCode,tm.""Id"" as TemplateMasterId
				--,tm.""Color"" as Color
				,tm.""Id"" as DocumentTypeId
				, 0 as DocumentId,null as FileName
				,null as StatusName,null as NoteStatus
				,null as tag
				, null as DocumentApprovalStatusType, null as DocumentApprovalStatus,null as StageStatus, 
                null as Discipline,null as IssueCode,null as Vendor,null as ProjectFolder
				,null as OwnerUser, null as CreatedByUser, null as UpdatedByUser
				,null as ServiceId,null as WorkflowNo,null as WorkflowName
				,null as WorkflowStatus, null as WorkflowTemplateId
				,'N' as NtsId,true as IsAllDay, 0 as FileSize,0 as PhotoId
                ,null as TagIds,null as TagNames,null as EnableDocumentChangeRequest, null as EnableLock
                ,null as TagCategoryIds,null as TagCategoryNames, ws.""SequenceOrder"" as SequenceNo,null as ChangeRequestServiceId,n.""NoteNo"" as NoteNo
               -- , t.""DefaultView"" as DefaultView,t.""TemplateOwner"" as TemplateOwner			
				
				from public.""User"" as u
				 join public.""NtsNote"" as n on n.""OwnerUserId""=u.""Id"" and u.""Id""='{UserId}' and  (n.""IsArchived"" <>'true' or n.""IsArchived"" is null) and n.""IsDeleted""=false and n .""CompanyId""='{_repo.UserContext.CompanyId}'
				 join cms.""N_GENERAL_FOLDER_WORKSPACE"" as ws on ws.""NtsNoteId""=n.""Id"" and ws.""IsDeleted""=false and ws.""CompanyId""='{_repo.UserContext.CompanyId}'
				 join public.""LOV"" as lv on lv.""Id""=ws.""TypeId"" and lv.""IsDeleted""=false and lv.""CompanyId""='{_repo.UserContext.CompanyId}' and  lv.""Code"" in ('MY_WORKSPACE')
				 join public.""Template"" as tm on tm.""Id""=n.""TemplateId"" and tm.""IsDeleted""=false and tm.""CompanyId""='{_repo.UserContext.CompanyId}'
				left join public.""TemplateCategory"" as tc on tc.""Id""=tm.""TemplateCategoryId""  and tc.""IsDeleted""=false and tc.""CompanyId""='{_repo.UserContext.CompanyId}' and tc.""Code""='GENERAL_FOLDER'
				left join public.""NtsNote"" as pn on pn.""Id""=n.""ParentNoteId"" and pn.""IsDeleted""=false and pn.""CompanyId""='{_repo.UserContext.CompanyId}'
                 where u.""IsDeleted""=false and and u.""CompanyId""='{_repo.UserContext.CompanyId}'
				
				Union 
select ws.""Id"" as Id,n.""NoteSubject"" as Name
--,tm.FileId as FileId
,tm.""Code"" as FolderCode,ws.""Id"" as WorkspaceId
                ,false as IsSelfWorkspace,true as IsWorkspaceAdmin,true as IsAssignedWorkspace,'Workspace' as FolderType
                ,0 as PermissionType,2 as Access
				,'' as InheritedFrom
				,2 as AppliesTo
                ,null as OwnerUserId,false as IsOwner,pn.""Id"" as ParentId,pn.""NoteSubject"" as ParentName
				,n.""IsArchived"" as IsArchived
				,0 as DocCount
                ,n.""DisablePermissionInheritance"" as DisablePermissionInheritance
				,n.""NoteSubject"" as DocumentName,n.""NoteDescription"" as Description,ws.""CreatedDate"" as CreatedDate
				,ws.""VersionNo"" as NoteVersionNo
				,n.""LockStatus"" as LockStatus
				,n.""LastUpdatedDate"" as UpdatedDate
				,n.""StartDate"" as Start,coalesce(n.""NoteSubject"", n.""NoteDescription"", '') as Title
				--,coalesce(ws.""StartDate"", '"", DateTime.Now.Date.ToYYY_MM_DD(), @""') as End
				--,coalesce(ws.""ExpiryDate"", '"", DateTime.Now.LastDayOfYear().ToYYY_MM_DD(), @""') as DueDate
				,tm.""Name"" as DocumentType,tm.""Code"" as TemplateMasterCode,tm.""Id"" as TemplateMasterId
				--,tm.Color as Color
				,tm.""Id"" as DocumentTypeId
				, 0 as DocumentId,null as FileName
				,null as StatusName,null as NoteStatus
				,null as tag
				, null as DocumentApprovalStatusType, null as DocumentApprovalStatus,null as StageStatus, 
                null as Discipline,null as IssueCode,null as Vendor,null as ProjectFolder
				,null as OwnerUser, null as CreatedByUser, null as UpdatedByUser
				,null as ServiceId,null as WorkflowNo,null as WorkflowName
				,null as WorkflowStatus, null as WorkflowTemplateId
				,'N' as NtsId,true as IsAllDay, 0 as FileSize,0 as PhotoId
                ,null as TagIds,null as TagNames,null as EnableDocumentChangeRequest, null as EnableLock
                ,null as TagCategoryIds,null as TagCategoryNames, ws.""SequenceOrder"" as SequenceNo,null as ChangeRequestServiceId,n.""NoteNo"" as NoteNo
                --, t.DefaultView as DefaultView,t.TemplateOwner as TemplateOwner
				
				from 
			 public.""User"" as u
			 left join public.""DocumentPermission"" as per  on per.""PermittedUserId""=u.""Id"" and u.""Id""='{UserId}' and per.""IsDeleted""=false and per.""CompanyId""='{_repo.UserContext.CompanyId}'
			 left join public.""UserGroupUser"" as ugu on  ugu.""UserId""=u.""Id"" and ugu.""IsDeleted""=false and ugu.""CompanyId""='{_repo.UserContext.CompanyId}'
			 left join public.""UserGroup"" as ug on  ug.""Id""=ugu.""Id"" and ug.""IsDeleted""=false and ug.""CompanyId""='{_repo.UserContext.CompanyId}'
			  left join public.""DocumentPermission"" as per1 on  per1.""PermittedUserGroupId""=ug.""Id"" and per1.""IsDeleted""=false and per1.""CompanyId""='{_repo.UserContext.CompanyId}'
				 join public.""NtsNote"" as n on per1.""NoteId""=n.""Id"" or per.""NoteId""=n.""Id"" and  (n.""IsArchived"" <>'true' or n.""IsArchived"" is null) and n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}'
				 join cms.""N_GENERAL_FOLDER_WORKSPACE"" as ws on ws.""NtsNoteId""=n.""Id""  and ws.""IsDeleted""=false and ws.""CompanyId""='{_repo.UserContext.CompanyId}'
				 join public.""LOV"" as lv on lv.""Id""=ws.""TypeId"" and lv.""IsDeleted""=false and lv.""CompanyId""='{_repo.UserContext.CompanyId}' and  lv.""Code"" not in ('ADMIN_WORKSPACE')
				 join public.""Template"" as tm on tm.""Id""=n.""TemplateId"" and tm.""IsDeleted""=false and tm.""CompanyId""='{_repo.UserContext.CompanyId}'
				left join public.""TemplateCategory"" as tc on tc.""Id""=tm.""TemplateCategoryId""  and tc.""IsDeleted""=false and tc.""CompanyId""='{_repo.UserContext.CompanyId}' and tc.""Code""='GENERAL_FOLDER'
				left join public.""NtsNote"" as pn on pn.""Id""=n.""ParentNoteId"" and pn.""IsDeleted""=false and pn.""CompanyId""='{_repo.UserContext.CompanyId}'
                where u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
Union
				Select ws.""Id"" as Id,n.""NoteSubject"" as Name
				--,tm.FileId as FileId
				,tm.""Code"" as FolderCode,ws.""Id"" as WorkspaceId
                ,'false' as IsSelfWorkspace,false as IsWorkspaceAdmin,true as IsAssignedWorkspace,'Workspace' as FolderType
                ,case when u.""Id"" is not null then np.""PermissionType"" else np2.""PermissionType"" end as PermissionType
                ,case when u.""Id"" is not null then np.""Access"" else np2.""Access"" end as Access
               ,case when u.""Id"" is not null then np.""InheritedFrom"" else np2.""InheritedFrom"" end as InheritedFrom
                ,case when u.""Id"" is not null then np.""AppliesTo"" else np2.""AppliesTo"" end as AppliesTo
                ,null as OwnerUserId,false as IsOwner,pn.""Id"" as ParentId,pn.""NoteSubject"" as ParentName
				,n.""IsArchived"" as IsArchived
				,0 as DocCount
                ,n.""DisablePermissionInheritance"" as DisablePermissionInheritance
                ,n.""NoteSubject"" as DocumentName,n.""NoteDescription"" as Description,ws.""CreatedDate"" as CreatedDate
				,ws.""VersionNo"" as NoteVersionNo
				,n.""LockStatus"" as LockStatus
				,ws.""LastUpdatedDate"" as UpdatedDate
				,n.""StartDate"" as Start,coalesce(n.""NoteSubject"", n.""NoteDescription"", '') as Title
				--,coalesce(ws.""StartDate"", '"", DateTime.Now.Date.ToYYY_MM_DD(), @""') as End
				--,coalesce(ws.""ExpiryDate"", '"", DateTime.Now.LastDayOfYear().ToYYY_MM_DD(), @""') as DueDate
				,tm.""Name"" as DocumentType,tm.""Code"" as TemplateMasterCode,tm.""Id"" as TemplateMasterId
				--,tm.""Color"" as Color
				,tm.""Id"" as DocumentTypeId
				, 0 as DocumentId,null as FileName
				,null as StatusName,null as NoteStatus
				,null as tag
				, null as DocumentApprovalStatusType, null as DocumentApprovalStatus,null as StageStatus,
                null as Discipline,null as IssueCode,null as Vendor,null as ProjectFolder
				,null as OwnerUser, null as CreatedByUser, null as UpdatedByUser
				,null as ServiceId,null as WorkflowNo,null as WorkflowName
				,null as WorkflowStatus, null as WorkflowTemplateId
				,'N' as NtsId,true as IsAllDay, 0 as FileSize,0 as PhotoId
                ,null as TagIds,null as TagNames,null as EnableDocumentChangeRequest, null as EnableLock
                ,null as TagCategoryIds,null as TagCategoryNames, ws.""SequenceOrder"" as SequenceNo,null as ChangeRequestServiceId,n.""NoteNo"" as NoteNo
                --, t.""DefaultView"" as DefaultView,t.""TemplateOwner"" as TemplateOwner
				
				from public.""User"" as u
left join public.""DocumentPermission"" as np on np.""PermittedUserId""=u.""Id""and u.""Id""='{UserId}' and np.""IsDeleted""=false and np.""CompanyId""='{_repo.UserContext.CompanyId}'		
left join public.""UserGroupUser"" as npwg on npwg.""UserId""=u.""Id"" and npwg.""IsDeleted""=false and npwg.""CompanyId""='{_repo.UserContext.CompanyId}'
				left join public.""DocumentPermission"" as np2 on np2.""PermittedUserGroupId""=npwg.""UserGroupId""  and np2.""IsDeleted""=false and np2.""CompanyId""='{_repo.UserContext.CompanyId}'
				 join public.""NtsNote"" as n on n.""Id""=np2.""NoteId"" or n.""Id""=np.""NoteId"" and  (n.""IsArchived"" <>'true' or n.""IsArchived"" is null) and n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}'
				 join cms.""N_GENERAL_FOLDER_WORKSPACE"" as ws on ws.""NtsNoteId""=n.""Id"" and ws.""IsDeleted""=false and ws.""CompanyId""='{_repo.UserContext.CompanyId}'
				 join public.""LOV"" as lv on lv.""Id""=ws.""TypeId"" and lv.""IsDeleted""=false and lv.""Code""<>'ADMIN_WORKSPACE' and lv.""CompanyId""='{_repo.UserContext.CompanyId}'
				 join public.""Template"" as tm on tm.""Id""=n.""TemplateId"" and tm.""IsDeleted""=false and tm.""Code""='WORKSPACE_GENERAL' and tm.""CompanyId""='{_repo.UserContext.CompanyId}'
				 join public.""TemplateCategory"" as tc on tc.""Id""=tm.""TemplateCategoryId""  and tc.""IsDeleted""=false and tc.""CompanyId""='{_repo.UserContext.CompanyId}' and tc.""Code""='GENERAL_FOLDER'
						
				left join public.""NtsNote"" as pn on pn.""Id""=n.""ParentNoteId"" and pn.""IsDeleted""=false and pn.""CompanyId""='{_repo.UserContext.CompanyId}'
                where u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
				union
				select f.""Id"" as Id,f.""NoteSubject"" as Name
				--,tm.""FileId"" as FileId
				,tm.""Code"" as FolderCode,null as WorkspaceId
                ,'false' as IsSelfWorkspace,false as IsWorkspaceAdmin,false as IsAssignedWorkspace,'Folder' as FolderType
                ,0 as PermissionType,2 as Access
				,'' as InheritedFrom
				,2 as AppliesTo
                ,u.""Id"" as OwnerUserId,true as IsOwner,pn.""Id"" as ParentId,pn.""NoteSubject"" as ParentName
				,f.""IsArchived"" as IsArchived
				,0 as DocCount
                ,f.""DisablePermissionInheritance"" as DisablePermissionInheritance
				,f.""NoteSubject"" as DocumentName,f.""NoteDescription"" as Description,f.""CreatedDate"" as CreatedDate
				,f.""VersionNo"" as NoteVersionNo
				,f.""LockStatus"" as LockStatus
				,f.""LastUpdatedDate"" as UpdatedDate
				,f.""StartDate"" as Start,coalesce(f.""NoteSubject"", f.""NoteDescription"", '') as Title
				--,coalesce(f.""StartDate"", '"", DateTime.Now.Date.ToYYY_MM_DD(), @""') as End
				--,coalesce(f.""ExpiryDate"", '"", DateTime.Now.LastDayOfYear().ToYYY_MM_DD(), @""') as DueDate
				,tm.""Name"" as DocumentType,tm.""Code"" as TemplateMasterCode,tm.""Id"" as TemplateMasterId
				--,tm.""Color"" as Color
				,tm.""Id"" as DocumentTypeId
				, 0 as DocumentId,null as FileName
				,null as StatusName,null as NoteStatus
				,null as tag
				, null as DocumentApprovalStatusType, null as DocumentApprovalStatus,null as StageStatus, 
				null as Discipline,null as IssueCode,null as Vendor,null as ProjectFolder
				,null as OwnerUser, null as CreatedByUser, null as UpdatedByUser
				,null as ServiceId,null as WorkflowNo,null as WorkflowName
				,null as WorkflowStatus, null as WorkflowTemplateId
				,'N' as NtsId,true as IsAllDay, 0 as FileSize,0 as PhotoId
                ,null as TagIds,null as TagNames,null as EnableDocumentChangeRequest, null as EnableLock
                ,null as TagCategoryIds,null as TagCategoryNames, f.""SequenceOrder"" as SequenceNo,null as ChangeRequestServiceId,f.""NoteNo"" as NoteNo
                --, t.DefaultView as DefaultView,t.TemplateOwner as TemplateOwner
				from public.""User"" as u
				 join public.""NtsNote"" as f on f.""OwnerUserId""=u.""Id"" and  (f.""IsArchived"" <>'true' or f.""IsArchived"" is null) and f.""IsDeleted""=false and f.""CompanyId""='{_repo.UserContext.CompanyId}' and u.""Id""='{UserId}'
				 join cms.""N_GENERAL_FOLDER_GENERALFOLDER"" as ws on ws.""NtsNoteId""=f.""Id"" and ws.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
				 --left join public.""LOV"" as lv on lv.""Id""=ws.""TypeId"" and lv.""IsDeleted""=false and lv.""CompanyId""='{_repo.UserContext.CompanyId}'
				 join public.""Template"" as tm on tm.""Id""=f.""TemplateId"" and tm.""IsDeleted""=false and tm.""CompanyId""='{_repo.UserContext.CompanyId}' and tm.""Code""='GENERAL_FOLDER'
				 join public.""TemplateCategory"" as tc on tc.""Id""=tm.""TemplateCategoryId""  and tc.""IsDeleted""=false and tc.""CompanyId""='{_repo.UserContext.CompanyId}'  and tc.""Code""='GENERAL_FOLDER'
				left join public.""NtsNote"" as pn on pn.""Id""=f.""ParentNoteId"" and pn.""IsDeleted""=false and pn.""CompanyId""='{_repo.UserContext.CompanyId}'
				where u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
				union
				 select f.""Id"" as Id,f.""NoteSubject"" as Name
				 --,tm.FileId as FileId
				 ,tm.""Code"" as FolderCode,null as WorkspaceId
                ,'false' as IsSelfWorkspace,false as IsWorkspaceAdmin,false as IsAssignedWorkspace,'Folder' as FolderType
                ,case when npu.""Id"" is not null then np.""PermissionType"" else np2.""PermissionType"" end as PermissionType
                ,case when npu.""Id"" is not null then np.""Access"" else np2.""Access"" end as Access
                ,case when npu.""Id"" is not null then np.""InheritedFrom"" else np2.""InheritedFrom"" end as InheritedFrom
                ,case when npu.""Id"" is not null then np.""AppliesTo"" else np2.""AppliesTo"" end as AppliesTo
                ,null as OwnerUserId,false as IsOwner,pn.""Id"" as ParentId,pn.""NoteSubject"" as ParentName
				,f.""IsArchived"" as IsArchived
				,0 as DocCount
                ,f.""DisablePermissionInheritance"" as DisablePermissionInheritance
				,f.""NoteSubject"" as DocumentName,f.""NoteDescription"" as Description,f.""CreatedDate"" as CreatedDate
				,f.""VersionNo"" as NoteVersionNo
				,f.""LockStatus"" as LockStatus
				,f.""LastUpdatedDate"" as UpdatedDate
				,f.""StartDate"" as Start,coalesce(f.""NoteSubject"", f.""NoteDescription"", '') as Title
				--,coalesce(f.""StartDate"", '"", DateTime.Now.Date.ToYYY_MM_DD(), @""') as End
				--,coalesce(f.""ExpiryDate"", '"", DateTime.Now.LastDayOfYear().ToYYY_MM_DD(), @""') as DueDate
				,tm.""Name"" as DocumentType,tm.""Code"" as TemplateMasterCode,tm.""Id"" as TemplateMasterId
				--,tm.Color as Color
				,tm.""Id"" as DocumentTypeId
				, 0 as DocumentId,null as FileName
				,null as StatusName,null as NoteStatus
				,null as tag
				, null as DocumentApprovalStatusType, null as DocumentApprovalStatus,null as StageStatus, 
                null as Discipline,null as IssueCode,null as Vendor,null as ProjectFolder
			   ,null as OwnerUser, null as CreatedByUser, null as UpdatedByUser
				,null as ServiceId,null as WorkflowNo,null as WorkflowName
				,null as WorkflowStatus, null as WorkflowTemplateId
				,'N' as NtsId,true as IsAllDay, 0 as FileSize,0 as PhotoId
                ,null as TagIds,null as TagNames,null as EnableDocumentChangeRequest, null as EnableLock
                ,null as TagCategoryIds,null as TagCategoryNames, f.""SequenceOrder"" as SequenceNo,null as ChangeRequestServiceId,f.""NoteNo"" as NoteNo
                --, t.DefaultView as DefaultView,t.TemplateOwner as TemplateOwner
				from 
				-- public.""User"" as u
				  public.""NtsNote"" as f --on f.""OwnerUserId""<>u.""Id""  and u.""Id""='{UserId}'  
				 join cms.""N_GENERAL_FOLDER_GENERALFOLDER"" as ws on ws.""NtsNoteId""=f.""Id"" and  (f.""IsArchived"" <>'true' or f.""IsArchived"" is null) and ws.""IsDeleted""=false and ws.""CompanyId""='{_repo.UserContext.CompanyId}'
				 --join public.""LOV"" as lv on lv.""Id""=ws.""TypeId"" and lv.""IsDeleted""=false and lv.""CompanyId""='{_repo.UserContext.CompanyId}'--and lv.""Code""<>'ADMIN_WORKSPACE'
				 join public.""Template"" as tm on tm.""Id""=f.""TemplateId"" and tm.""IsDeleted""=false and tm.""CompanyId""='{_repo.UserContext.CompanyId}' and tm.""Code""='GENERAL_FOLDER'
				 join public.""TemplateCategory"" as tc on tc.""Id""=tm.""TemplateCategoryId""  and tc.""IsDeleted""=false and tc.""CompanyId""='{_repo.UserContext.CompanyId}' and tc.""Code""='GENERAL_FOLDER'
				left join public.""DocumentPermission"" as np on np.""NoteId""=f.""Id"" and np.""IsDeleted""=false and np.""IsDeleted""=false and np.""CompanyId""='{_repo.UserContext.CompanyId}'
				left join public.""User"" as npu on npu.""Id""=np.""PermittedUserId"" and npu.""UserId""='{UserId}'	and npu.""IsDeleted""=false	and npu.""CompanyId""='{_repo.UserContext.CompanyId}'
				left join public.""DocumentPermission"" as np2 on np2.""NoteId""=f.""Id"" and np2.""IsDeleted""=false and np2.""CompanyId""='{_repo.UserContext.CompanyId}'
				left join public.""UserGroupUser"" as npwg on npwg.""UserGroupId""=np2.""PermittedUserGroupId"" and npwg.""IsDeleted""=false and npwg.""CompanyId""='{_repo.UserContext.CompanyId}'
				left join public.""User"" as npwgu on npwgu.""Id""=npwg.""UserId""	and npwgu.""UserId""='{UserId}'	and npwgu.""IsDeleted""=false and npwgu.""CompanyId""='{_repo.UserContext.CompanyId}'	
				left join public.""NtsNote"" as pn on pn.""Id""=f.""ParentNoteId"" and pn.""IsDeleted""=false and pn.""CompanyId""='{_repo.UserContext.CompanyId}'
where f.""OwnerUserId""<>'{UserId}'	and f.""IsDeleted""=false and f.""CompanyId""='{_repo.UserContext.CompanyId}'
Union 
 select f.""Id"" as Id,f.""NoteSubject"" as Name
 --,tm.FileId as FileId
 ,tm.""Code"" as FolderCode,null as WorkspaceId
                ,'false' as IsSelfWorkspace,false as IsWorkspaceAdmin,false as IsAssignedWorkspace,'Document' as FolderType
                ,'0' as PermissionType,'2' as Access
				,'' as InheritedFrom
				,'3' as AppliesTo
                ,u.""Id"" as OwnerUserId,true as IsOwner,pn.""Id"" as ParentId,pn.""NoteSubject"" as ParentName
				,f.""IsArchived"" as IsArchived
				,0 as DocCount
                ,f.""DisablePermissionInheritance"" as DisablePermissionInheritance
                ,f.""NoteSubject"" as DocumentName,f.""NoteDescription"" as Description,f.""CreatedDate"" as CreatedDate
                ,f.""VersionNo"" as NoteVersionNo
				,f.""LockStatus"" as LockStatus
				,f.""LastUpdatedDate"" as UpdatedDate
                ,f.""StartDate"" as Start,coalesce(f.""NoteSubject"", f.""NoteDescription"", '') as Title
                --,coalesce(f.""StartDate"", '"", DateTime.Now.Date.ToYYY_MM_DD(), @""') as End
               -- ,coalesce(f.""ExpiryDate"", '"", DateTime.Now.LastDayOfYear().ToYYY_MM_DD(), @""') as DueDate
                ,tm.""Name"" as DocumentType,tm.""Code"" as TemplateMasterCode,tm.""Id"" as TemplateMasterId
                --,tm.""Color"" as Color
				,tm.""Id"" as DocumentTypeId
                , case when udf.""DocumentId"" is not null then udf.""DocumentId""::int else 0 end as DocumentId,gen.""FileName"" as FileName
                ,lov.""Name"" as StatusName,lov.""Code"" as NoteStatus
                ,null as tag
				, udf.""documentApprovalStatusType"" as DocumentApprovalStatusType
                , udf.""documentApprovalStatus"" as DocumentApprovalStatus,udf.""stageStatus"" as StageStatus,
                udf.""discipline"" as Discipline,coalesce(udf.""code"",udf.""OutgoingIssueCodes"",udf.""issueCodes"") as IssueCode
                ,udf.""vendorList"" as Vendor,udf.""projectFolder"" as ProjectFolder
				,u.""Name"" as OwnerUser, u.""Name"" as CreatedByUser, u.""Name"" as UpdatedByUser
                ,s.""Id"" as ServiceId,s.""ServiceNo"" as WorkflowNo,s.""ServiceSubject"" as WorkflowName
                ,null as WorkflowStatus, null as WorkflowTemplateId
                ,'N' as NtsId,true as IsAllDay, gen.""ContentLength"" as FileSize,0 as PhotoId
                ,ntg.""TagId"" as TagIds,tg.""NoteSubject"" as TagNames
                ,null as EnableDocumentChangeRequest, null as EnableLock
                ,ntg.""TagCategoryId"" as TagCategoryIds,tgc.""NoteSubject"" as TagCategoryNames 
                , f.""SequenceOrder"" as SequenceNo,cs.""Id"" as ChangeRequestServiceId,f.""NoteNo"" as NoteNo
				--, t.""DefaultView"" as DefaultView,t.""TemplateOwner"" as TemplateOwner
from 
 public.""User"" as u
				 join public.""NtsNote"" as f on f.""OwnerUserId""=u.""Id"" and u.""Id""='{UserId}' and f.""IsDeleted""=false and f.""IsDeleted""=false and f.""CompanyId""='{_repo.UserContext.CompanyId}' and  (f.""IsArchived"" <>'true' or f.""IsArchived"" is null)
				 join public.""LOV"" as lov on lov.""Id""=f.""NoteStatusId"" and lov.""IsDeleted""=false and lov.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
				 left join (" + udfs + @") as udf on udf.""NtsNoteId""=f.""Id"" 
				 
				 join public.""Template"" as tm on tm.""Id""=f.""TemplateId"" and tm.""IsDeleted""=false and tm.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}' --and tm.""Code""='GENERAL_FOLDER'
				 join public.""TemplateCategory"" as tc on tc.""Id""=tm.""TemplateCategoryId""  and tc.""IsDeleted""=false and  tc.""CompanyId""='{_repo.UserContext.CompanyId}' and tc.""Code""='GENERAL_DOCUMENT'
				 join public.""NtsNote"" as pn on pn.""Id""=f.""ParentNoteId"" and pn.""IsDeleted""=false and pn.""CompanyId""='{_repo.UserContext.CompanyId}'
				 left join public.""NtsService"" as s on s.""Id""=f.""ParentServiceId"" and s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}'
				 left join public.""NtsService"" as cs on cs.""Id""=udf.""ChangeRequestServiceId"" and cs.""IsDeleted""=false and cs.""CompanyId""='{_repo.UserContext.CompanyId}'
				left join public.""File"" as gen on gen.""Id""=udf.""DocumentId"" and gen.""IsDeleted""=false  and gen.""CompanyId""='{_repo.UserContext.CompanyId}'
				left join public.""Template"" as ws on ws.""Id""=tm.""WorkFlowTemplateId"" and ws.""IsDeleted""=false  and ws.""CompanyId""='{_repo.UserContext.CompanyId}'
				left join public.""NtsTag"" as ntg on ntg.""NtsId""=f.""Id"" and ntg.""IsDeleted""=false and ntg.""CompanyId""='{_repo.UserContext.CompanyId}'
				left join public.""NtsNote"" as tg on tg.""Id""=ntg.""TagId"" and tg.""IsDeleted""=false and tg.""CompanyId""='{_repo.UserContext.CompanyId}'
				left join public.""NtsNote"" as tgc on tgc.""Id""=tg.""ParentNoteId"" and tgc.""IsDeleted""=false and tgc.""CompanyId""='{_repo.UserContext.CompanyId}'
				union
Select f.""Id"" as Id,f.""NoteSubject"" as Name
--,tm.FileId as FileId
,tm.""Code"" as FolderCode,null as WorkspaceId
                ,'false' as IsSelfWorkspace,false as IsWorkspaceAdmin,false as IsAssignedWorkspace,'Document' as FolderType
                ,0 as PermissionType,2 as Access
				,'' as InheritedFrom
				,3 as AppliesTo
                ,u.""Id"" as OwnerUserId,true as IsOwner,pn.""Id"" as ParentId,pn.""NoteSubject"" as ParentName
				,f.""IsArchived"" as IsArchived
				,0 as DocCount
                ,f.""DisablePermissionInheritance"" as DisablePermissionInheritance
                ,f.""NoteSubject"" as DocumentName,f.""NoteDescription"" as Description,f.""CreatedDate"" as CreatedDate
                ,f.""VersionNo"" as NoteVersionNo
				,f.""LockStatus"" as LockStatus
				,f.""LastUpdatedDate"" as UpdatedDate
                ,f.""StartDate"" as Start,coalesce(f.""NoteSubject"", f.""NoteDescription"", '') as Title
                --,coalesce(f.""StartDate"", '"", DateTime.Now.Date.ToYYY_MM_DD(), @""') as End
                --,coalesce(f.""ExpiryDate"", '"", DateTime.Now.LastDayOfYear().ToYYY_MM_DD(), @""') as DueDate
                ,tm.""Name"" as DocumentType,tm.""Code"" as TemplateMasterCode,tm.""Id"" as TemplateMasterId
                --,tm.""Color"" as Color
				,tm.""Id"" as DocumentTypeId
                , case when udf.""DocumentId"" is not null then udf.""DocumentId""::int else 0 end as DocumentId,gen.""FileName"" as FileName
                ,lov.""Name"" as StatusName,lov.""Code"" as NoteStatus
                ,null as tag
				, udf.""documentApprovalStatusType"" as DocumentApprovalStatusType
                , udf.""documentApprovalStatus"" as DocumentApprovalStatus,udf.""stageStatus"" as StageStatus,
                udf.""discipline"" as Discipline,coalesce(udf.""code"",udf.""OutgoingIssueCodes"",udf.""issueCodes"") as IssueCode
                ,udf.""vendorList"" as Vendor,udf.""projectFolder"" as ProjectFolder
			   ,u.""Name"" as OwnerUser, u.""Name"" as CreatedByUser, u.""Name"" as UpdatedByUser
                ,s.""Id"" as ServiceId,s.""ServiceNo"" as WorkflowNo,s.""ServiceSubject"" as WorkflowName
                ,null as WorkflowStatus, null as WorkflowTemplateId
                ,'N' as NtsId,true as IsAllDay, gen.""ContentLength"" as FileSize,0 as PhotoId
                ,'' as TagIds,'' as TagNames
                ,null as EnableDocumentChangeRequest
				, null as EnableLock
                ,'' as TagCategoryIds
				,'' as TagCategoryNames 
                , f.""SequenceOrder"" as SequenceNo
				,cs.""Id"" as ChangeRequestServiceId
				,f.""NoteNo"" as NoteNo
				--, t.""DefaultView"" as DefaultView,t.""TemplateOwner"" as TemplateOwner				
from 
public.""User"" as u
				 join public.""NtsNote"" as f on f.""OwnerUserId""<>u.""Id"" and u.""Id""='{UserId}' and  (f.""IsArchived"" <>true or f.""IsArchived"" is null) and f.""IsDeleted""=false	and f.""CompanyId""='{_repo.UserContext.CompanyId}'
				  join public.""User"" as ou on f.""OwnerUserId""=ou.""Id"" and ou.""IsDeleted""=false	and ou.""CompanyId""='{_repo.UserContext.CompanyId}'
				 join public.""LOV"" as lov on lov.""Id""=f.""NoteStatusId"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_repo.UserContext.CompanyId}'	
				
				 join public.""Template"" as tm on tm.""Id""=f.""TemplateId"" and tm.""IsDeleted""=false and tm.""CompanyId""='{_repo.UserContext.CompanyId}'--and tm.""Code""='GENERAL_FOLDER'
				 join public.""TemplateCategory"" as tc on tc.""Id""=tm.""TemplateCategoryId""  and tc.""IsDeleted""=false and tc.""CompanyId""='{_repo.UserContext.CompanyId}' and tc.""Code""='GENERAL_DOCUMENT'
				  left join (" + udfs + @") as udf on udf.""NtsNoteId""=f.""Id""  	
				 
				 join public.""NtsNote"" as pn on pn.""Id""=f.""ParentNoteId"" and pn.""IsDeleted""=false and pn.""CompanyId""='{_repo.UserContext.CompanyId}'	
				 left join public.""DocumentPermission"" as np on np.""NoteId""=f.""Id"" and np.""IsDeleted""=false and np.""IsDeleted""=false and np.""CompanyId""='{_repo.UserContext.CompanyId}'	
				left join public.""User"" as npu on npu.""Id""=np.""PermittedUserId""	and npu.""Id""=u.""Id""	and npu.""IsDeleted""=false	and npu.""CompanyId""='{_repo.UserContext.CompanyId}'
				left join public.""DocumentPermission"" as np2 on np2.""NoteId""=f.""Id"" and np2.""IsDeleted""=false and np2.""CompanyId""='{_repo.UserContext.CompanyId}'	
				left join public.""UserGroupUser"" as npwg on npwg.""UserGroupId""=np2.""PermittedUserGroupId"" and npwg.""IsDeleted""=false and npwg.""CompanyId""='{_repo.UserContext.CompanyId}'	
				left join public.""User"" as npwgu on npwgu.""Id""=npwg.""UserId"" and npwgu.""Id""=u.""Id"" and npwgu.""IsDeleted""=false	and npwgu.""CompanyId""='{_repo.UserContext.CompanyId}'			
				 left join public.""NtsService"" as s on s.""Id""=f.""ParentServiceId"" and s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}'
				 left join public.""NtsService"" as cs on cs.""Id""=udf.""ChangeRequestServiceId"" and cs.""IsDeleted""=false and cs.""CompanyId""='{_repo.UserContext.CompanyId}'
				left join public.""File"" as gen on gen.""Id""=udf.""DocumentId"" and gen.""IsDeleted""=false and gen.""CompanyId""='{_repo.UserContext.CompanyId}'
				left join public.""Template"" as ws on ws.""Id""=tm.""WorkFlowTemplateId"" and ws.""IsDeleted""=false and ws.""CompanyId""='{_repo.UserContext.CompanyId}'
				--left join public.""NtsTag"" as ntg on ntg.""NtsId""=f.""Id"" and ntg.""IsDeleted""=false and ntg.""CompanyId""='{_repo.UserContext.CompanyId}'
				--left join public.""NtsNote"" as tg on tg.""Id""=ntg.""TagId"" and tg.""IsDeleted""=false and tg.""CompanyId""='{_repo.UserContext.CompanyId}'
				--left join public.""NtsNote"" as tgc on tgc.""Id""=tg.""ParentNoteId"" and tgc.""IsDeleted""=false and tgc.""CompanyId""='{_repo.UserContext.CompanyId}'
                   where u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
             
                ");

                return query;
            }
            else if (documentQueryType == DocumentQueryTypeEnum.Folder)
            {
                return $@"select distinct  n.""Id"" as Id,n.""NoteSubject"" as Name
--,tm.FileId as FileId
,tm.""Code"" as FolderCode,n.""Id"" as WorkspaceId
 ,'true' as IsSelfWorkspace,false as IsWorkspaceAdmin,true as IsAssignedWorkspace,'Workspace' as FolderType
 ,'0'::int as PermissionType,'2'::int as Access
,'' as InheritedFrom
 ,'2'::int as AppliesTo
 ,u.""Id"" as OwnerUserId,true as IsOwner,pn.""Id"" as ParentId,pn.""NoteSubject"" as ParentName,n.""IsArchived"" as IsArchived,0 as DocCount
--, t.DefaultView as DefaultView
 ,n.""DisablePermissionInheritance"" as DisablePermissionInheritance, n.""SequenceOrder"" as SequenceNo
 --,ws.""ModifiedStatus"" as ModifiedStatus
 
 from 
 public.""User"" as u
 join public.""NtsNote"" as n on n.""OwnerUserId""=u.""Id"" and  (n.""IsArchived"" <> true or n.""IsArchived"" is null) and n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}'   and u.""Id""='{UserId}'
join cms.""N_GENERAL_FOLDER_WORKSPACE"" as ws on ws.""NtsNoteId""=n.""Id"" and ws.""IsDeleted""=false and ws.""CompanyId""='{_repo.UserContext.CompanyId}' 
 join public.""LOV"" as lv on lv.""Id""=ws.""TypeId"" and lv.""IsDeleted""=false and  lv.""Code"" in ('MY_WORKSPACE')
 join public.""Template"" as tm on tm.""Id""=n.""TemplateId"" and tm.""IsDeleted""=false and tm.""CompanyId""='{_repo.UserContext.CompanyId}' 
left join public.""TemplateCategory"" as tc on tc.""Id""=tm.""TemplateCategoryId""  and tc.""IsDeleted""=false and tc.""Code""='GENERAL_FOLDER' and tc.""CompanyId""='{_repo.UserContext.CompanyId}' 
left join public.""NtsNote"" as pn on pn.""Id""=n.""ParentNoteId"" and pn.""IsDeleted""=false and pn.""CompanyId""='{_repo.UserContext.CompanyId}'
where n.""IsDeleted""=false and ws.""IsDeleted""=false  
and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
union
Select distinct  n.""Id"" as Id,n.""NoteSubject"" as Name
--,tm.FileId as FileId
,tm.""Code"" as FolderCode,ws.""Id"" as WorkspaceId
                ,'false' as IsSelfWorkspace,false as IsWorkspaceAdmin,true as IsAssignedWorkspace,'Workspace' as FolderType
                , np.""PermissionType""  as PermissionType
                , np.""Access""  as Access
                , np.""InheritedFrom""  as InheritedFrom
               ,np.""AppliesTo""  as AppliesTo
                ,n.""OwnerUserId"" as OwnerUserId,false as IsOwner,pn.""Id"" as ParentId,pn.""NoteSubject"" as ParentName,n.""IsArchived"" as IsArchived,0 as DocCount
                ,n.""DisablePermissionInheritance"" as DisablePermissionInheritance, ws.""SequenceOrder"" as SequenceNo
				--, t.""DefaultView"" as DefaultView,ws.""ModifiedStatus"" as ModifiedStatus
				
				from 
				 --public.""User"" as u
				  public.""NtsNote"" as n --on n.""OwnerUserId""=u.""Id"" 
				 join cms.""N_GENERAL_FOLDER_WORKSPACE"" as ws on ws.""NtsNoteId""=n.""Id"" and  (n.""IsArchived"" <> true or n.""IsArchived"" is null) and n.""IsDeleted""=false and ws.""IsDeleted""=false  and n.""CompanyId""='{_repo.UserContext.CompanyId}'
				   
				 join public.""Template"" as tm on tm.""Id""=n.""TemplateId"" and tm.""IsDeleted""=false and tm.""Code""='WORKSPACE_GENERAL' and tm.""CompanyId""='{_repo.UserContext.CompanyId}'
				 join public.""TemplateCategory"" as tc on tc.""Id""=tm.""TemplateCategoryId""  and tc.""IsDeleted""=false and tc.""Code""='GENERAL_FOLDER' and tc.""CompanyId""='{_repo.UserContext.CompanyId}'
				 --left join public.""DocumentPermission"" as np on np.""NoteId""=n.""Id"" and np.""IsDeleted""=false --and np.""PermittedUserId""='{UserId}' and np.""CompanyId""='{_repo.UserContext.CompanyId}' 
               -- left join public.""User"" as npu on npu.""Id""=np.""PermittedUserId"" and npu.""Id""='{UserId}' and npu.""IsDeleted""=false and npu.""CompanyId""='{_repo.UserContext.CompanyId}' 
			
				--left join public.""UserGroupUser"" as npwg on npwg.""UserGroupId""=np.""PermittedUserGroupId""  and npwg.""IsDeleted""=false and npwg.""CompanyId""='{_repo.UserContext.CompanyId}' 
				--left join public.""User"" as npwgu on npwgu.""Id""=npwg.""UserId""  and npwgu.""Id""='{UserId}'	 and npwgu.""IsDeleted""=false and npwgu.""CompanyId""='{_repo.UserContext.CompanyId}'  					
				 left join(select np.*,case when npu is null then npwgu.""Id"" else npu.""Id"" end as ""UserId""
from  public.""DocumentPermission"" as np  
                left join public.""User"" as npu on npu.""Id""=np.""PermittedUserId"" and npu.""Id""='{UserId}' and npu.""IsDeleted""=false and npu.""CompanyId""='{_repo.UserContext.CompanyId}' 
			
				left join public.""UserGroupUser"" as npwg on npwg.""UserGroupId""=np.""PermittedUserGroupId""  and npwg.""IsDeleted""=false and npwg.""CompanyId""='{_repo.UserContext.CompanyId}' 
				left join public.""User"" as npwgu on npwgu.""Id""=npwg.""UserId""  and npwgu.""Id""='{UserId}'	 and npwgu.""IsDeleted""=false and npwgu.""CompanyId""='{_repo.UserContext.CompanyId}'
					where np.""IsDeleted""=false	  )
				as np on np.""NoteId""=n.""Id"" and ""UserId""='{UserId}'
left join public.""NtsNote"" as pn on pn.""Id""=n.""ParentNoteId"" and pn.""IsDeleted""=false and pn.""CompanyId""='{_repo.UserContext.CompanyId}'
				left join public.""LOV"" as lv on lv.""Id""=ws.""TypeId"" and lv.""IsDeleted""=false  and lv.""CompanyId""='{_repo.UserContext.CompanyId}' 
				where  lv.""Code"" <>'MY_WORKSPACE' or lv.""Code"" is null and n.""IsDeleted""=false and ws.""IsDeleted""=false  and n.""CompanyId""='{_repo.UserContext.CompanyId}' 

                union
    Select distinct f.""Id"" as Id,f.""NoteSubject"" as Name
  --,tm.FileId as FileId
  ,tm.""Code"" as FolderCode,ws.""Id"" as WorkspaceId
                ,'false' as IsSelfWorkspace,false as IsWorkspaceAdmin,false as IsAssignedWorkspace,'Folder' as FolderType
                , np.""PermissionType""  as PermissionType
                , np.""Access""  as Access
                , np.""InheritedFrom""  as InheritedFrom
                , np.""AppliesTo""  as AppliesTo
                ,null as OwnerUserId,false as IsOwner,pn.""Id"" as ParentId,pn.""NoteSubject"" as ParentName,f.""IsArchived"" as IsArchived
				,t.""docCount"" as DocCount
                ,f.""DisablePermissionInheritance"" as DisablePermissionInheritance, f.""SequenceOrder"" as SequenceNo
				--, t.DefaultView as DefaultView,f.ModifiedStatus as ModifiedStatus				
from 

				  public.""NtsNote"" as f  
				 join cms.""N_GENERAL_FOLDER_GENERALFOLDER"" as ws1 on ws1.""NtsNoteId""=f.""Id"" and  (f.""IsArchived"" <> true or f.""IsArchived"" is null)	and f.""IsDeleted""=false and ws1.""IsDeleted""=false	and f.""CompanyId""='{_repo.UserContext.CompanyId}'				
				 join public.""Template"" as tm on tm.""Id""=f.""TemplateId"" and tm.""IsDeleted""=false and tm.""Code""='GENERAL_FOLDER' and tm.""CompanyId""='{_repo.UserContext.CompanyId}'
				 join public.""TemplateCategory"" as tc on tc.""Id""=tm.""TemplateCategoryId""  and tc.""IsDeleted""=false and tc.""Code""='GENERAL_FOLDER' and tc.""CompanyId""='{_repo.UserContext.CompanyId}'
				join public.""NtsNote"" as pn on pn.""Id""=f.""ParentNoteId"" and pn.""IsDeleted""=false and pn.""CompanyId""='{_repo.UserContext.CompanyId}'
left join cms.""N_GENERAL_FOLDER_WORKSPACE"" as ws on ws.""Id""=ws1.""WorkspaceId"" and ws.""IsDeleted""=false and ws1.""CompanyId""='{_repo.UserContext.CompanyId}'
				 

	 left join(select np.*,case when npu is null then npwgu.""Id"" else npu.""Id"" end as ""UserId"" 
from  public.""DocumentPermission"" as np 
                left join public.""User"" as npu on npu.""Id""=np.""PermittedUserId"" and npu.""Id""='{UserId}' and npu.""IsDeleted""=false and npu.""CompanyId""='{_repo.UserContext.CompanyId}' 
			
				left join public.""UserGroupUser"" as npwg on npwg.""UserGroupId""=np.""PermittedUserGroupId""  and npwg.""IsDeleted""=false and npwg.""CompanyId""='{_repo.UserContext.CompanyId}' 
				left join public.""User"" as npwgu on npwgu.""Id""=npwg.""UserId""  and npwgu.""Id""='{UserId}'	 and npwgu.""IsDeleted""=false and npwgu.""CompanyId""='{_repo.UserContext.CompanyId}'
					where np.""IsDeleted""=false	  )
				as np on np.""NoteId""=f.""Id"" and ""UserId""='{UserId}'	
--public.""DocumentPermission"" as np on np.""NoteId""=f.""Id"" and np.""IsDeleted""=false  and np.""CompanyId""='{_repo.UserContext.CompanyId}' --and  np.""PermittedUserId""='{UserId}' 
			--	left join public.""User"" as npu on npu.""Id""=np.""PermittedUserId"" and npu.""Id""='{UserId}'	 and npu.""IsDeleted""=false and npu.""CompanyId""='{_repo.UserContext.CompanyId}'	
			--	--left join public.""DocumentPermission"" as np2 on np2.""NoteId""=f.""Id"" and	np2.""IsDeleted""=false 
			--	left join public.""UserGroupUser"" as npwg on npwg.""UserGroupId""=np.""PermittedUserGroupId"" and	npwg.""IsDeleted""=false and npwg.""CompanyId""='{_repo.UserContext.CompanyId}'
			--	left join public.""User"" as npwgu on npwgu.""Id""=npwg.""UserId"" and npwgu.""Id""='{UserId}' and	npwgu.""IsDeleted""=false  and npwgu.""CompanyId""='{_repo.UserContext.CompanyId}'					
			
				left join (with doc as(select distinct f1.* from public.""NtsNote"" as f1 
				 join public.""Template"" as tm1 on tm1.""Id""=f1.""TemplateId"" and tm1.""IsDeleted""=false and f1.""IsDeleted""=false  and tm1.""CompanyId""='{_repo.UserContext.CompanyId}'
				 join public.""TemplateCategory"" as tc1 on tc1.""Id""=tm1.""TemplateCategoryId""  and tc1.""IsDeleted""=false and tc1.""Code""='GENERAL_DOCUMENT' and tc1.""CompanyId""='{_repo.UserContext.CompanyId}'
 --join public.""DocumentPermission"" as np1 on np1.""NoteId""=f1.""Id"" and np1.""IsDeleted""=false and np1.""CompanyId""='{_repo.UserContext.CompanyId}' --and  np1.""PermittedUserId""='{UserId}' 
				--left join public.""User"" as npu1 on npu1.""Id""=np1.""PermittedUserId"" and npu1.""Id""='{UserId}'	and npu1.""IsDeleted""=false and npu1.""CompanyId""='{_repo.UserContext.CompanyId}'	
				----left join public.""DocumentPermission"" as np21 on np21.""NoteId""=f1.""Id"" 	and np21.""IsDeleted""=false and np21.""CompanyId""='{_repo.UserContext.CompanyId}'		
			--	left join public.""UserGroupUser"" as npwg1 on npwg1.""UserGroupId""=np1.""PermittedUserGroupId"" and npwg1.""IsDeleted""=false 	and npwg1.""CompanyId""='{_repo.UserContext.CompanyId}'	
				--left join public.""User"" as npwgu1 on npwgu1.""Id""=npwg1.""UserId"" and npwgu1.""Id""='{UserId}'  and npwgu1.""IsDeleted""=false and npwgu1.""CompanyId""='{_repo.UserContext.CompanyId}'	
				 join(select np1.*,case when npu is null then npwgu.""Id"" else npu.""Id"" end as ""UserId""
from  public.""DocumentPermission"" as np1
                left join public.""User"" as npu on npu.""Id""=np1.""PermittedUserId"" and npu.""Id""='{UserId}' and npu.""IsDeleted""=false and npu.""CompanyId""='{_repo.UserContext.CompanyId}' 
			
				left join public.""UserGroupUser"" as npwg on npwg.""UserGroupId""=np1.""PermittedUserGroupId""  and npwg.""IsDeleted""=false and npwg.""CompanyId""='{_repo.UserContext.CompanyId}' 
				left join public.""User"" as npwgu on npwgu.""Id""=npwg.""UserId""  and npwgu.""Id""='{UserId}'	 and npwgu.""IsDeleted""=false and npwgu.""CompanyId""='{_repo.UserContext.CompanyId}'
					where np1.""IsDeleted""=false	  )
				as np1 on np1.""NoteId""=f1.""Id"" and ""UserId""='{UserId}'

where f1.""IsDeleted""=false and (f1.""IsArchived"" <> true or f1.""IsArchived"" is null)

									  ) select ""ParentNoteId"",count(*) as ""docCount"" from doc group by ""ParentNoteId"")t
				
				on t.""ParentNoteId""=f.""Id"" 			
				where f.""IsDeleted""=false ";

//                var query = $@"select distinct  n.""Id"" as Id,n.""NoteSubject"" as Name
//--,tm.FileId as FileId
//,tm.""Code"" as FolderCode,n.""Id"" as WorkspaceId
// ,'true' as IsSelfWorkspace,false as IsWorkspaceAdmin,true as IsAssignedWorkspace,'Workspace' as FolderType
// ,'0'::int as PermissionType,'2'::int as Access
//,'' as InheritedFrom
// ,'2'::int as AppliesTo
// ,u.""Id"" as OwnerUserId,true as IsOwner,pn.""Id"" as ParentId,pn.""NoteSubject"" as ParentName,n.""IsArchived"" as IsArchived,0 as DocCount
//--, t.DefaultView as DefaultView
// ,n.""DisablePermissionInheritance"" as DisablePermissionInheritance, n.""SequenceOrder"" as SequenceNo
// --,ws.""ModifiedStatus"" as ModifiedStatus
 
// from 
// public.""User"" as u
// join public.""NtsNote"" as n on n.""OwnerUserId""=u.""Id"" and  (n.""IsArchived"" <> true or n.""IsArchived"" is null) and n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}'  and u.""Id""='{UserId}'
//join cms.""N_GENERAL_FOLDER_WORKSPACE"" as ws on ws.""NtsNoteId""=n.""Id"" and ws.""IsDeleted""=false and ws.""CompanyId""='{_repo.UserContext.CompanyId}'
// join public.""LOV"" as lv on lv.""Id""=ws.""TypeId"" and lv.""IsDeleted""=false and lv.""CompanyId""='{_repo.UserContext.CompanyId}' and  lv.""Code"" in ('MY_WORKSPACE')
// join public.""Template"" as tm on tm.""Id""=n.""TemplateId"" and tm.""IsDeleted""=false and tm.""CompanyId""='{_repo.UserContext.CompanyId}'
//left join public.""TemplateCategory"" as tc on tc.""Id""=tm.""TemplateCategoryId""  and tc.""IsDeleted""=false and tc.""CompanyId""='{_repo.UserContext.CompanyId}' and tc.""Code""='GENERAL_FOLDER'
//left join public.""NtsNote"" as pn on pn.""Id""=n.""ParentNoteId"" and pn.""IsDeleted""=false and pn.""CompanyId""='{_repo.UserContext.CompanyId}' 
//where u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'  -- #WHERE#

//union
//Select distinct  n.""Id"" as Id,n.""NoteSubject"" as Name
//--,tm.FileId as FileId
//,tm.""Code"" as FolderCode,ws.""NtsNoteId"" as WorkspaceId
//                ,'false' as IsSelfWorkspace,false as IsWorkspaceAdmin,true as IsAssignedWorkspace,'Workspace' as FolderType
//                , np.""PermissionType""  as PermissionType
//                , np.""Access""  as Access
//                , np.""InheritedFrom""  as InheritedFrom
//               ,np.""AppliesTo""  as AppliesTo
//                ,n.""OwnerUserId"" as OwnerUserId,false as IsOwner,pn.""Id"" as ParentId,pn.""NoteSubject"" as ParentName,n.""IsArchived"" as IsArchived,0 as DocCount
//                ,n.""DisablePermissionInheritance"" as DisablePermissionInheritance, ws.""SequenceOrder"" as SequenceNo
//				--, t.""DefaultView"" as DefaultView,ws.""ModifiedStatus"" as ModifiedStatus
				
//				from 
//				 --public.""User"" as u
//				  public.""NtsNote"" as n --on n.""OwnerUserId""=u.""Id"" and n.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
//				 join cms.""N_GENERAL_FOLDER_WORKSPACE"" as ws on ws.""NtsNoteId""=n.""Id"" and  (n.""IsArchived"" <> true or n.""IsArchived"" is null)  and ws.""IsDeleted""=false and ws.""CompanyId""='{_repo.UserContext.CompanyId}'
				   
//				 join public.""Template"" as tm on tm.""Id""=n.""TemplateId"" and tm.""IsDeleted""=false and tm.""CompanyId""='{_repo.UserContext.CompanyId}' and tm.""Code""='WORKSPACE_GENERAL'
//				 join public.""TemplateCategory"" as tc on tc.""Id""=tm.""TemplateCategoryId""  and tc.""IsDeleted""=false and tc.""CompanyId""='{_repo.UserContext.CompanyId}' and tc.""Code""='GENERAL_FOLDER'
//				 join public.""DocumentPermission"" as np on np.""NoteId""=n.""Id"" and np.""IsDeleted""=false and np.""CompanyId""='{_repo.UserContext.CompanyId}'  --and np.""PermittedUserId""='{UserId}'
//                left join public.""User"" as npu on npu.""Id""=np.""PermittedUserId"" and npu.""Id""='{UserId}' and npu.""IsDeleted""=false and npu.""CompanyId""='{_repo.UserContext.CompanyId}'
			
//				left join public.""UserGroupUser"" as npwg on npwg.""UserGroupId""=np.""PermittedUserGroupId""  and npwg.""IsDeleted""=false and npwg.""CompanyId""='{_repo.UserContext.CompanyId}'
//				left join public.""User"" as npwgu on npwgu.""Id""=npwg.""UserId""  and npwgu.""Id""='{UserId}'	 and npwgu.""IsDeleted""=false and npwgu.""CompanyId""='{_repo.UserContext.CompanyId}'	 					
//				left join public.""NtsNote"" as pn on pn.""Id""=n.""ParentNoteId"" and pn.""IsDeleted""=false and pn.""CompanyId""='{_repo.UserContext.CompanyId}'
//				left join public.""LOV"" as lv on lv.""Id""=ws.""TypeId"" and lv.""IsDeleted""=false and lv.""CompanyId""='{_repo.UserContext.CompanyId}'
//where  lv.""Code"" <>'MY_WORKSPACE' or lv.""Code"" is null and n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}' and ws.""IsDeleted""=false  --#WHERE#
				


//                union
//    Select distinct n.""Id"" as Id,n.""NoteSubject"" as Name
//  --,tm.FileId as FileId
//  ,tm.""Code"" as FolderCode,ws.""NtsNoteId"" as WorkspaceId
//                ,'false' as IsSelfWorkspace,false as IsWorkspaceAdmin,false as IsAssignedWorkspace,'Folder' as FolderType
//                , np.""PermissionType""  as PermissionType
//                , np.""Access""  as Access
//                , np.""InheritedFrom""  as InheritedFrom
//                , np.""AppliesTo""  as AppliesTo
//                ,null as OwnerUserId,false as IsOwner,pn.""Id"" as ParentId,pn.""NoteSubject"" as ParentName,n.""IsArchived"" as IsArchived
//				,case when t.""docCount"" is null then 0 else t.""docCount"" end as DocCount
//                ,n.""DisablePermissionInheritance"" as DisablePermissionInheritance, n.""SequenceOrder"" as SequenceNo
//				--, t.DefaultView as DefaultView,n.ModifiedStatus as ModifiedStatus				
//from 

//				  public.""NtsNote"" as n  
//				 join cms.""N_GENERAL_FOLDER_GENERALFOLDER"" as ws1 on ws1.""NtsNoteId""=n.""Id"" and  (n.""IsArchived"" <> true or n.""IsArchived"" is null)	 and ws1.""IsDeleted""=false and ws1.""CompanyId""='{_repo.UserContext.CompanyId}'					
//				 join public.""Template"" as tm on tm.""Id""=n.""TemplateId"" and tm.""IsDeleted""=false and tm.""CompanyId""='{_repo.UserContext.CompanyId}' and tm.""Code""='GENERAL_FOLDER'
//				 join public.""TemplateCategory"" as tc on tc.""Id""=tm.""TemplateCategoryId""  and tc.""IsDeleted""=false and tc.""CompanyId""='{_repo.UserContext.CompanyId}' and tc.""Code""='GENERAL_FOLDER'
//				join public.""NtsNote"" as pn on pn.""Id""=n.""ParentNoteId"" and pn.""IsDeleted""=false and pn.""CompanyId""='{_repo.UserContext.CompanyId}'
//left join cms.""N_GENERAL_FOLDER_WORKSPACE"" as ws on ws.""Id""=ws1.""WorkspaceId"" and ws.""IsDeleted""=false and ws.""CompanyId""='{_repo.UserContext.CompanyId}'
				 
// join public.""DocumentPermission"" as np on np.""NoteId""=n.""Id"" and np.""IsDeleted""=false and np.""CompanyId""='{_repo.UserContext.CompanyId}' --and  np.""PermittedUserId""='{UserId}'
//				left join public.""User"" as npu on npu.""Id""=np.""PermittedUserId"" and npu.""Id""='{UserId}'	 and npu.""IsDeleted""=false	
//				--left join public.""DocumentPermission"" as np2 on np2.""NoteId""=n.""Id"" and	np2.""IsDeleted""=false
//				left join public.""UserGroupUser"" as npwg on npwg.""UserGroupId""=np.""PermittedUserGroupId"" and	npwg.""IsDeleted""=false and npwg.""CompanyId""='{_repo.UserContext.CompanyId}'
//				left join public.""User"" as npwgu on npwgu.""Id""=npwg.""UserId"" and npwgu.""Id""='{UserId}' and	npwgu.""IsDeleted""=false and npwgu.""CompanyId""='{_repo.UserContext.CompanyId}'					
			
//				left join (with doc as(select distinct f1.* from public.""NtsNote"" as f1 
//				 join public.""Template"" as tm1 on tm1.""Id""=f1.""TemplateId"" and tm1.""IsDeleted""=false and tm1.""CompanyId""='{_repo.UserContext.CompanyId}'
//				 join public.""TemplateCategory"" as tc1 on tc1.""Id""=tm1.""TemplateCategoryId""  and tc1.""IsDeleted""=false and tc1.""CompanyId""='{_repo.UserContext.CompanyId}' and tc1.""Code""='GENERAL_DOCUMENT'
// join public.""DocumentPermission"" as np1 on np1.""NoteId""=f1.""Id"" and np1.""IsDeleted""=false and np1.""CompanyId""='{_repo.UserContext.CompanyId}' --and  np1.""PermittedUserId""='{UserId}'
//				left join public.""User"" as npu1 on npu1.""Id""=np1.""PermittedUserId"" and npu1.""Id""='{UserId}'	and npu1.""IsDeleted""=false and npu1.""CompanyId""='{_repo.UserContext.CompanyId}'	
//				--left join public.""DocumentPermission"" as np21 on np21.""NoteId""=f1.""Id"" 	and np21.""IsDeleted""=false and np21.""CompanyId""='{_repo.UserContext.CompanyId}'	
//				left join public.""UserGroupUser"" as npwg1 on npwg1.""UserGroupId""=np1.""PermittedUserGroupId"" and npwg1.""IsDeleted""=false and npwg1.""CompanyId""='{_repo.UserContext.CompanyId}'	
//				left join public.""User"" as npwgu1 on npwgu1.""Id""=npwg1.""UserId"" and npwgu1.""Id""='{UserId}'  and npwgu1.""IsDeleted""=false and npwgu1.""CompanyId""='{_repo.UserContext.CompanyId}'
				
//where f1.""IsDeleted""=false and (f1.""IsArchived"" <> true or f1.""IsArchived"" is null) --and  (npu1.""Id""='{UserId}' or npwgu1.""Id""='{UserId}')
//									  ) select ""ParentNoteId"",count(*) as ""docCount"" from doc group by ""ParentNoteId"")t
				
//				on t.""ParentNoteId""=n.""Id"" 			
//				where n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}' 
//--and  (npu.""Id""='{UserId}' or npwgu.""Id""='{UserId}')
//--#FWHERE#

             
//                ";
                //var search = $@"and n.""ParentNoteId"" is null";
                //var Foldersearch = $@"";
                //if (parentId.IsNotNullAndNotEmpty())
                //{
                //    search = $@"and (n.""ParentNoteId"" ='{parentId}' or n.""Id""='{parentId}')";
                //    Foldersearch = $@"and (n.""ParentNoteId"" ='{parentId}' or n.""Id""='{parentId}')";
                //}
                //query = query.Replace("#WHERE#", search);
                //query = query.Replace("#FWHERE#", Foldersearch);
                //return query;
            }

            else
            {
                return string.Concat(@"match(u:ADM_User{Id:{UserId}})-[:R_User_MyWorkspace_Note]->(ws:NTS_Note{IsDeleted:0})
                where (ws.IsArchived <>true or ws.IsArchived is null)
                match(ws)-[:R_Note_Template]->(t: NTS_Template{ IsDeleted: 0,Status: 'Active'})
                -[:R_TemplateRoot]->(tm: NTS_TemplateMaster{ IsDeleted: 0,Status: 'Active'})  
                -[:R_TemplateMaster_TemplateCategory]->(tc: NTS_TemplateCategory{IsDeleted:0,Code:'GENERAL_FOLDER'})
                optional match(ws)-[:R_Note_Parent_Note]->(pn:NTS_Note{IsDeleted: 0,Status: 'Active'})  
                return ws.Id as Id,ws.Subject as Name,tm.FileId as FileId,tm.Code as FolderCode,ws.Id as WorkspaceId
                ,true as IsSelfWorkspace,false as IsWorkspaceAdmin,true as IsAssignedWorkspace,'Workspace' as FolderType
                ,'Allow' as PermissionType,'FullAccess' as Access,'' as InheritedFrom,'ThisFolderSubFoldersAndFiles' as AppliesTo
                ,u.Id as OwnerUserId,true as IsOwner,pn.Id as ParentId,pn.Subject as ParentName,ws.IsArchived as IsArchived,0 as DocCount
                ,ws.DisablePermissionInheritance as DisablePermissionInheritance
                ,null as DocumentId,null as DocumentName, null as CreatedByUser
                ,null as StatusName,null as NoteStatus
                ,null as TagId,null as TagName,null as EnableDocumentChangeRequest, null as EnableLock
                ,tm.Name as DocumentType,tm.Id as DocumentTypeId
                ,ws.VersionNo as NoteVersionNo ,null as ServiceId
                ,null as WorkflowNo,null as WorkflowName
				,null as WorkflowStatus, null as WorkflowTemplateId
                ,null as TagCategoryId,null as TagCategoryName
                ,ws.CreatedDate as CreatedDate,ws.CreatedBy as CreatedBy
                ,ws.LastUpdatedDate as LastUpdatedDate,ws.LastUpdatedBy as LastUpdatedBy
                ,ws.LastUpdatedDate as UpdatedDate,0 as FileSize,null as FileName
                ,ws.StartDate as Start,coalesce(ws.Subject,ws.Description,'') as Title
                ,coalesce(ws.StartDate,'", DateTime.Now.Date.ToYYY_MM_DD(), @"') as End
                ,coalesce(ws.ExpiryDate,'", DateTime.Now.LastDayOfYear().ToYYY_MM_DD(), @"') as DueDate
                ,'N' as NtsId,true as IsAllDay, ws.SequenceNo as SequenceNo, t.DefaultView as DefaultView
                union  
                match(u:ADM_User{Id:{UserId}})-[r:R_User_AdminWorkspace_Note]->(ws:NTS_Note{IsDeleted:0}) 
                where (ws.IsArchived <>true or ws.IsArchived is null)
                match(ws)-[:R_Note_Template]->(t: NTS_Template{ IsDeleted: 0,Status: 'Active'})
                -[:R_TemplateRoot]->(tm: NTS_TemplateMaster{ IsDeleted: 0,Status: 'Active'})  
                -[:R_TemplateMaster_TemplateCategory]->(tc: NTS_TemplateCategory{IsDeleted:0,Code:'GENERAL_FOLDER'})
                optional match(ws)-[:R_Note_Parent_Note]->(pn:NTS_Note{IsDeleted: 0,Status: 'Active'}) 
                return ws.Id as Id,ws.Subject as Name,tm.FileId as FileId,tm.Code as FolderCode,ws.Id as WorkspaceId
                ,false as IsSelfWorkspace,true as IsWorkspaceAdmin,true as IsAssignedWorkspace,'Workspace' as FolderType
                ,'Allow' as PermissionType,'FullAccess' as Access,'' as InheritedFrom,'ThisFolderSubFoldersAndFiles' as AppliesTo
                ,null as OwnerUserId,false as IsOwner,pn.Id as ParentId,pn.Subject as ParentName,ws.IsArchived as IsArchived,0 as DocCount
                ,ws.DisablePermissionInheritance as DisablePermissionInheritance
                ,null as DocumentId,null as DocumentName, null as CreatedByUser
                ,null as StatusName,null as NoteStatus
                ,null as TagId,null as TagName,null as EnableDocumentChangeRequest, null as EnableLock
                ,tm.Name as DocumentType,tm.Id as DocumentTypeId
                ,ws.VersionNo as NoteVersionNo ,null as ServiceId
                ,null as WorkflowNo,null as WorkflowName
			    ,null as WorkflowStatus, null as WorkflowTemplateId
                ,null as TagCategoryId,null as TagCategoryName
                ,ws.CreatedDate as CreatedDate,ws.CreatedBy as CreatedBy
                ,ws.LastUpdatedDate as LastUpdatedDate,ws.LastUpdatedBy as LastUpdatedBy
                ,ws.LastUpdatedDate as UpdatedDate,0 as FileSize,null as FileName
                ,ws.StartDate as Start,coalesce(ws.Subject,ws.Description,'') as Title
                ,coalesce(ws.StartDate,'", DateTime.Now.Date.ToYYY_MM_DD(), @"') as End
                ,coalesce(ws.ExpiryDate,'", DateTime.Now.LastDayOfYear().ToYYY_MM_DD(), @"') as DueDate
                ,'N' as NtsId,true as IsAllDay, ws.SequenceNo as SequenceNo, t.DefaultView as DefaultView
                union
                match(u:ADM_User{Id:{UserId}})
				match (ws:NTS_Note{IsDeleted:0}) 
                where (ws.IsArchived <>true or ws.IsArchived is null)
			    and not (ws)<-[:R_User_AdminWorkspace_Note]-(u)
                match(ws)-[:R_Note_Template]->(t: NTS_Template{ IsDeleted: 0,Status: 'Active'})
                -[:R_TemplateRoot]->(tm: NTS_TemplateMaster{ IsDeleted: 0,Status: 'Active',Code:'WORKSPACE_GENERAL'})
                -[:R_TemplateMaster_TemplateCategory]->(tc: NTS_TemplateCategory{IsDeleted:0,Code:'GENERAL_FOLDER'})
                optional match(ws)<-[:R_NotePermission_Note]-(np:NTS_NotePermission{IsDeleted: 0})
                -[:R_NotePermission_User]->(npu:ADM_User{Id:u.Id}) 
                optional match(ws)<-[:R_NotePermission_Note]-(np2:NTS_NotePermission{IsDeleted: 0})
                -[:R_NotePermission_WorkspacePermissionGroup]->(npwg:ADM_WorkspacePermissionGroup{IsDeleted: 0})  
                <-[:R_User_UserPermissionGroup]-(npwgu:ADM_User{Id:u.Id})
                optional match(ws)-[:R_Note_Parent_Note]->(pn:NTS_Note{IsDeleted: 0,Status: 'Active'}) 
                with u,ws,npu,np,np2,tm,pn,t
                where npu is not null or npwgu is not null
                return ws.Id as Id,ws.Subject as Name,tm.FileId as FileId,tm.Code as FolderCode,ws.Id as WorkspaceId
                ,false as IsSelfWorkspace,false as IsWorkspaceAdmin,true as IsAssignedWorkspace,'Workspace' as FolderType
                ,case when npu.Id is not null then np.PermissionType else np2.PermissionType end as PermissionType
                ,case when npu.Id is not null then np.Access else np2.Access end as Access
                ,case when npu.Id is not null then np.InheritedFrom else np2.InheritedFrom end as InheritedFrom
                ,case when npu.Id is not null then np.AppliesTo else np2.AppliesTo end as AppliesTo
                ,null as OwnerUserId,false as IsOwner,pn.Id as ParentId,pn.Subject as ParentName,ws.IsArchived as IsArchived,0 as DocCount
                ,ws.DisablePermissionInheritance as DisablePermissionInheritance
                ,null as DocumentId,null as DocumentName, null as CreatedByUser
                ,null as StatusName,null as NoteStatus
                ,null as TagId,null as TagName,null as EnableDocumentChangeRequest, null as EnableLock
                ,tm.Name as DocumentType,tm.Id as DocumentTypeId
                ,ws.VersionNo as NoteVersionNo,null as ServiceId
                ,null as WorkflowNo,null as WorkflowName 
                ,null as WorkflowStatus, null as WorkflowTemplateId
                ,null as TagCategoryId,null as TagCategoryName
                ,ws.CreatedDate as CreatedDate,ws.CreatedBy as CreatedBy
                ,ws.LastUpdatedDate as LastUpdatedDate,ws.LastUpdatedBy as LastUpdatedBy
                ,ws.LastUpdatedDate as UpdatedDate,0 as FileSize,null as FileName
                ,ws.StartDate as Start,coalesce(ws.Subject,ws.Description,'') as Title
                ,coalesce(ws.StartDate,'", DateTime.Now.Date.ToYYY_MM_DD(), @"') as End
                ,coalesce(ws.ExpiryDate,'", DateTime.Now.LastDayOfYear().ToYYY_MM_DD(), @"') as DueDate
                ,'N' as NtsId,true as IsAllDay, ws.SequenceNo as SequenceNo, t.DefaultView as DefaultView
                union
                match(u:ADM_User{Id:{UserId}})<-[:R_Note_Owner_User]-(f:NTS_Note{IsDeleted:0})
                where (f.IsArchived <>true or f.IsArchived is null)
                match(f)-[:R_Note_Template]->(t: NTS_Template{ IsDeleted: 0,Status: 'Active'})
                -[:R_TemplateRoot]->(tm: NTS_TemplateMaster{ IsDeleted: 0,Status: 'Active',Code:'GENERAL_FOLDER'})  
                -[:R_TemplateMaster_TemplateCategory]->(tc: NTS_TemplateCategory{IsDeleted:0,Code:'GENERAL_FOLDER'})
                match(f)-[:R_Note_Parent_Note]->(pn:NTS_Note{IsDeleted: 0,Status: 'Active'}) 
                return f.Id as Id,f.Subject as Name,tm.FileId as FileId,tm.Code as FolderCode,null as WorkspaceId
                ,false as IsSelfWorkspace,false as IsWorkspaceAdmin,false as IsAssignedWorkspace,'Folder' as FolderType
                ,'Allow' as PermissionType,'FullAccess' as Access,'' as InheritedFrom,'ThisFolderSubFoldersAndFiles' as AppliesTo
                ,u.Id as OwnerUserId,true as IsOwner,pn.Id as ParentId,pn.Subject as ParentName,f.IsArchived as IsArchived,0 as DocCount
                ,f.DisablePermissionInheritance as DisablePermissionInheritance
                ,null as DocumentId,null as DocumentName, null as CreatedByUser
                ,null as StatusName,null as NoteStatus 
                ,null as TagId,null as TagName,null as EnableDocumentChangeRequest, null as EnableLock
                ,tm.Name as DocumentType,tm.Id as DocumentTypeId
                ,f.VersionNo as NoteVersionNo,null as ServiceId
                ,null as WorkflowNo,null as WorkflowName
				,null as WorkflowStatus, null as WorkflowTemplateId
                ,null as TagCategoryId,null as TagCategoryName
                ,f.CreatedDate as CreatedDate,f.CreatedBy as CreatedBy
                ,f.LastUpdatedDate as LastUpdatedDate,f.LastUpdatedBy as LastUpdatedBy
                ,f.LastUpdatedDate as UpdatedDate,0 as FileSize,null as FileName
                ,f.StartDate as Start,coalesce(f.Subject, f.Description, '') as Title
                ,coalesce(f.StartDate, '", DateTime.Now.Date.ToYYY_MM_DD(), @"') as End
                ,coalesce(f.ExpiryDate, '", DateTime.Now.LastDayOfYear().ToYYY_MM_DD(), @"') as DueDate
                ,'N' as NtsId,true as IsAllDay, f.SequenceNo as SequenceNo, t.DefaultView as DefaultView
                union
                match(u:ADM_User{Id:{UserId}})
                match(f:NTS_Note{IsDeleted:0})
                where (f.IsArchived <>true or f.IsArchived is null)
                and not (f)-[:R_Note_Owner_User]->(u)
                match(f)-[:R_Note_Template]->(t: NTS_Template{ IsDeleted: 0,Status: 'Active'})
                -[:R_TemplateRoot]->(tm: NTS_TemplateMaster{ IsDeleted: 0,Status: 'Active',Code:'GENERAL_FOLDER'})  
                -[:R_TemplateMaster_TemplateCategory]->(tc: NTS_TemplateCategory{IsDeleted:0,Code:'GENERAL_FOLDER'})
                match(f)-[:R_Note_Parent_Note]->(pn:NTS_Note{IsDeleted: 0,Status: 'Active'}) 
                optional match(f)<-[:R_NotePermission_Note]-(np:NTS_NotePermission{IsDeleted: 0})
                -[:R_NotePermission_User]->(npu:ADM_User{Id:u.Id}) 
                optional match(f)<-[:R_NotePermission_Note]-(np2:NTS_NotePermission{IsDeleted: 0})
                -[:R_NotePermission_WorkspacePermissionGroup]->(npwg:ADM_WorkspacePermissionGroup{IsDeleted: 0})  
                <-[:R_User_UserPermissionGroup]-(npwgu:ADM_User{Id:u.Id})            
                with u,f,npu,np,np2,tm,pn,t
                where npu is not null or npwgu is not null
                return f.Id as Id,f.Subject as Name,tm.FileId as FileId,tm.Code as FolderCode,null as WorkspaceId
                ,false as IsSelfWorkspace,false as IsWorkspaceAdmin,false as IsAssignedWorkspace,'Folder' as FolderType
                ,case when npu.Id is not null then np.PermissionType else np2.PermissionType end as PermissionType
                ,case when npu.Id is not null then np.Access else np2.Access end as Access
                ,case when npu.Id is not null then np.InheritedFrom else np2.InheritedFrom end as InheritedFrom
                ,case when npu.Id is not null then np.AppliesTo else np2.AppliesTo end as AppliesTo
                ,null as OwnerUserId,false as IsOwner,pn.Id as ParentId,pn.Subject as ParentName,f.IsArchived as IsArchived,0 as DocCount
                ,f.DisablePermissionInheritance as DisablePermissionInheritance
                ,null as DocumentId,null as DocumentName, null as CreatedByUser
                ,null as StatusName,null as NoteStatus
                ,null as TagId,null as TagName,null as EnableDocumentChangeRequest, null as EnableLock
                ,tm.Name as DocumentType,tm.Id as DocumentTypeId
                ,f.VersionNo as NoteVersionNo,null as ServiceId
                ,null as WorkflowNo,null as WorkflowName
				,null as WorkflowStatus, null as WorkflowTemplateId
                ,null as TagCategoryId,null as TagCategoryName
                ,f.CreatedDate as CreatedDate,f.CreatedBy as CreatedBy
                ,f.LastUpdatedDate as LastUpdatedDate,f.LastUpdatedBy as LastUpdatedBy
                ,f.LastUpdatedDate as UpdatedDate,0 as FileSize,null as FileName
                ,f.StartDate as Start,coalesce(f.Subject, f.Description, '') as Title
				,coalesce(f.StartDate, '", DateTime.Now.Date.ToYYY_MM_DD(), @"') as End
				,coalesce(f.ExpiryDate, '", DateTime.Now.LastDayOfYear().ToYYY_MM_DD(), @"') as DueDate
                ,'N' as NtsId,true as IsAllDay, f.SequenceNo as SequenceNo, t.DefaultView as DefaultView
                union
                match(u:ADM_User{Id:{UserId}})<-[:R_Note_Owner_User]-(f:NTS_Note{IsDeleted:0})
                where (f.IsArchived <>true or f.IsArchived is null)
                match(f)-[:R_Note_Template]->(t: NTS_Template{ IsDeleted: 0,Status: 'Active'})
                -[:R_TemplateRoot]->(tm: NTS_TemplateMaster{ IsDeleted: 0,Status: 'Active'})  
                -[:R_TemplateMaster_TemplateCategory]->(tc: NTS_TemplateCategory{IsDeleted:0,Code:'GENERAL_DOCUMENT'})
                match(f)-[:R_Note_Parent_Note]->(pn:NTS_Note{IsDeleted: 0,Status: 'Active'}) 
			    match (f)-[:R_Note_Tag_Note]->(tg:NTS_Note) 
                -[:R_Note_Template]->(: NTS_Template{ IsDeleted: 0,Status: 'Active'})
                -[:R_TemplateRoot]->(tntm: NTS_TemplateMaster{ IsDeleted: 0,Status: 'Active',Code:'DOCUMENT_TAGS'})  
                -[:R_TemplateMaster_TemplateCategory]->(: NTS_TemplateCategory{Code:'TAGSCATEGORY'})
                match (tg)-[:R_Note_Parent_Note]->(tgc:NTS_Note)
                optional match(f)-[:R_Note_Status_ListOfValue]->(lov: GEN_ListOfValue)
                optional match(f)< -[:R_NoteFieldValue_Note] - (nfv: NTS_NoteFieldValue{ IsDeleted: 0})
				-[:R_NoteFieldValue_TemplateField]->(ttf: NTS_TemplateField{ FieldName: 'attachment'}) 
                optional match(gen:GEN_File { Id: toInt(nfv.Code)})
                optional match(t)-[:R_Template_Workflow_ServiceTemplateMaster]->(ws: NTS_TemplateMaster{ IsDeleted: 0})
                optional match(f)-[:R_Note_Reference]->(s: NTS_Service{ IsDeleted: 0})-[:R_Service_Status_ListOfValue]->(sov: GEN_ListOfValue)
                return f.Id as Id,f.Subject as Name,tm.FileId as FileId,tm.Code as FolderCode,null as WorkspaceId
                ,false as IsSelfWorkspace,false as IsWorkspaceAdmin,false as IsAssignedWorkspace,'Document' as FolderType
                ,'Allow' as PermissionType,'FullAccess' as Access,'' as InheritedFrom,'OnlyThisDocument' as AppliesTo
                ,u.Id as OwnerUserId,true as IsOwner,pn.Id as ParentId,pn.Subject as ParentName,f.IsArchived as IsArchived,0 as DocCount
                ,f.DisablePermissionInheritance as DisablePermissionInheritance
                ,f.Id as DocumentId,f.Subject as DocumentName,u.UserName as CreatedByUser
                ,lov.Name as StatusName,lov.Code as NoteStatus
                ,tg.Id as TagId,tg.Subject as TagName,null as EnableDocumentChangeRequest, null as EnableLock
                ,tm.Name as DocumentType,tm.Id as DocumentTypeId
                ,f.VersionNo as NoteVersionNo,s.Id as ServiceId
                ,s.ServiceNo as WorkflowNo,s.Subject as WorkflowName
				,sov.Name as WorkflowStatus, ws.Id as WorkflowTemplateId
                ,tgc.Id as TagCategoryId,tgc.Subject as TagCategoryName
                ,f.CreatedDate as CreatedDate,f.CreatedBy as CreatedBy
                ,f.LastUpdatedDate as LastUpdatedDate,f.LastUpdatedBy as LastUpdatedBy
                ,f.LastUpdatedDate as UpdatedDate, gen.ContentLength as FileSize,nfv.Value as FileName
                ,f.StartDate as Start,coalesce(f.Subject, f.Description, '') as Title
				,coalesce(f.StartDate, '", DateTime.Now.Date.ToYYY_MM_DD(), @"') as End
				,coalesce(f.ExpiryDate, '", DateTime.Now.LastDayOfYear().ToYYY_MM_DD(), @"') as DueDate
                ,'N' as NtsId,true as IsAllDay, f.SequenceNo as SequenceNo, t.DefaultView as DefaultView
                union
                match(u:ADM_User{Id:{UserId}})
                match(f:NTS_Note{IsDeleted:0})
                where (f.IsArchived <>true or f.IsArchived is null)
                and not (f)-[:R_Note_Owner_User]->(u)
                match(f)-[:R_Note_Template]->(t: NTS_Template{ IsDeleted: 0,Status: 'Active'})
                -[:R_TemplateRoot]->(tm: NTS_TemplateMaster{ IsDeleted: 0,Status: 'Active'})  
                -[:R_TemplateMaster_TemplateCategory]->(tc: NTS_TemplateCategory{IsDeleted:0,Code:'GENERAL_DOCUMENT'})
			    match (f)-[:R_Note_Tag_Note]->(tg:NTS_Note) 
                -[:R_Note_Template]->(: NTS_Template{ IsDeleted: 0,Status: 'Active'})
                -[:R_TemplateRoot]->(tntm: NTS_TemplateMaster{ IsDeleted: 0,Status: 'Active',Code:'DOCUMENT_TAGS'})  
                -[:R_TemplateMaster_TemplateCategory]->(: NTS_TemplateCategory{Code:'TAGSCATEGORY'})
                match (tg)-[:R_Note_Parent_Note]->(tgc:NTS_Note)
                optional match(f)<-[:R_NotePermission_Note]-(np:NTS_NotePermission{IsDeleted: 0})
                -[:R_NotePermission_User]->(npu:ADM_User{Id:u.Id}) 
                optional match(f)<-[:R_NotePermission_Note]-(np2:NTS_NotePermission{IsDeleted: 0})
                -[:R_NotePermission_WorkspacePermissionGroup]->(npwg:ADM_WorkspacePermissionGroup{IsDeleted: 0})  
                <-[:R_User_UserPermissionGroup]-(npwgu:ADM_User{Id:u.Id})
                optional match(f)-[:R_Note_Parent_Note]->(pn:NTS_Note{IsDeleted: 0,Status: 'Active'}) 
                optional match(f)< -[:R_NoteFieldValue_Note] - (nfv: NTS_NoteFieldValue{ IsDeleted: 0})
				-[:R_NoteFieldValue_TemplateField]->(ttf: NTS_TemplateField{ FieldName: 'attachment'}) 
                optional match(gen:GEN_File { Id: toInt(nfv.Code)})
                optional match(f)-[:R_Note_Status_ListOfValue]->(lov: GEN_ListOfValue)
                with u,f,npu,np,np2,tm,pn,tg,tgc,nfv,gen,lov,t
                where npu is not null or npwgu is not null
                optional match(t)-[:R_Template_Workflow_ServiceTemplateMaster]->(ws: NTS_TemplateMaster{ IsDeleted: 0})
                optional match(f)-[:R_Note_Reference]->(s: NTS_Service{ IsDeleted: 0})-[:R_Service_Status_ListOfValue]->(sov: GEN_ListOfValue)
                return f.Id as Id,f.Subject as Name,tm.FileId as FileId,tm.Code as FolderCode,null as WorkspaceId
                ,false as IsSelfWorkspace,false as IsWorkspaceAdmin,false as IsAssignedWorkspace,'Document' as FolderType
                ,case when npu.Id is not null then np.PermissionType else np2.PermissionType end as PermissionType
                ,case when npu.Id is not null then np.Access else np2.Access end as Access
                ,case when npu.Id is not null then np.InheritedFrom else np2.InheritedFrom end as InheritedFrom
                ,case when npu.Id is not null then np.AppliesTo else np2.AppliesTo end as AppliesTo
                ,null as OwnerUserId,false as IsOwner,pn.Id as ParentId,pn.Subject as ParentName,f.IsArchived as IsArchived,0 as DocCount
                ,f.DisablePermissionInheritance as DisablePermissionInheritance
                ,f.Id as DocumentId,f.Subject as DocumentName,u.UserName as CreatedByUser
                ,lov.Name as StatusName,lov.Code as NoteStatus
                ,tg.Id as TagId,tg.Subject as TagName,t.EnableDocumentChangeRequest as EnableDocumentChangeRequest, t.EnableLock as EnableLock
                ,tm.Name as DocumentType,tm.Id as DocumentTypeId
                ,f.VersionNo as NoteVersionNo,s.Id as ServiceId
                ,s.ServiceNo as WorkflowNo,s.Subject as WorkflowName
				,sov.Name as WorkflowStatus, ws.Id as WorkflowTemplateId              
                ,tgc.Id as TagCategoryId,tgc.Subject as TagCategoryName
                ,f.CreatedDate as CreatedDate,f.CreatedBy as CreatedBy
                ,f.LastUpdatedDate as LastUpdatedDate,f.LastUpdatedBy as LastUpdatedBy
                ,f.LastUpdatedDate as UpdatedDate, gen.ContentLength as FileSize,nfv.Value as FileName
                ,f.StartDate as Start,coalesce(f.Subject, f.Description, '') as Title
                ,coalesce(f.StartDate, '", DateTime.Now.Date.ToYYY_MM_DD(), @"') as End
                ,coalesce(f.ExpiryDate, '", DateTime.Now.LastDayOfYear().ToYYY_MM_DD(), @"') as DueDate
                ,'N' as NtsId,true as IsAllDay, f.SequenceNo as SequenceNo, t.DefaultView as DefaultView");
            }


        }

        public async Task<List<FolderViewModel>> GetAllByParent(string UserId, string parentId)
        {
            var udfs = await _noteBusiness.GetUdfQuery(null, "GENERAL_DOCUMENT", null, null, "attachment,DocumentId", "documentApprovalStatusType,documentApprovalStatusType", "code,code", "issueCodes,issueCodes"
              , "documentApprovalStatus,documentApprovalStatus", "stageStatus,stageStatus", "discipline,discipline", "OutgoingIssueCodes,OutgoingIssueCodes", "vendorList,vendorList", "projectFolder,projectFolder", "ChangeRequestId,ChangeRequestServiceId", "workspaceId,WorkspaceId");
            var query= $@"select distinct  n.""Id"" as Id,n.""NoteSubject"" as Name
                ,tm.""Code"" as FolderCode,n.""Id"" as WorkspaceId
                ,'true' as IsSelfWorkspace,false as IsWorkspaceAdmin,true as IsAssignedWorkspace,'Workspace' as FolderType
                ,'0'::int as PermissionType
                ,'2'::int as Access
                ,'' as InheritedFrom
                ,'2'::int as AppliesTo
                ,u.""Id"" as OwnerUserId,true as IsOwner,null as ParentId,null as ParentName,n.""IsArchived"" as IsArchived,tags.""Count"" as DocCount
                ,n.""DisablePermissionInheritance"" as DisablePermissionInheritance
                ,null as DocumentName
                ,null as Description
                ,n.""CreatedDate"" as CreatedDate
                ,n.""VersionNo"" as NoteVersionNo
                ,n.""LockStatus"" as LockStatus
                ,n.""LastUpdatedDate"" as LastUpdatedDate
                ,n.""StartDate"" as Start
                ,coalesce(n.""NoteSubject"", n.""NoteDescription"", '') as Title
                ,tm.""Name"" as DocumentType
                ,tm.""Code"" as TemplateMasterCode
                ,tm.""Id"" as TemplateMasterId
                ,tm.""Id"" as DocumentTypeId
                ,null as DocumentId
                ,null as FileName
                ,null as StatusName
                ,null as NoteStatus
                ,null as tag
                ,null as DocumentApprovalStatusType
                ,null as DocumentsApprovalStatus
                ,null as StageStatus
                ,null as Discipline
                ,null as IssueCode
                ,null as Vendor
                ,null as ProjectFolder
                ,null as OwnerUser
                ,u.""Name"" as CreatedByUser
                ,null as UpdatedByUser
                ,null as ServiceId
                ,null as WorkflowNo
                ,null as WorkflowName
                ,null as WorkflowStatus
                ,null as WorkflowTemplateId
                ,null as WorkflowCode
                ,'N' as NtsId
                ,true as IsAllDay
                , 0 as FileSize
                ,0 as PhotoId
                ,null as TagIds
                ,null as TagNames
                ,null as EnableDocumentChangeRequest
                , null as EnableLock
                ,null as TagCategoryIds
                ,null as TagCategoryNames 
                ,0 as SequenceNo
                ,null as ChangeRequestServiceId
                ,n.""NoteNo"" as DocumentNo
                ,null as WorkflowServiceId
                ,null as WorkflowServiceStatus   
                ,null as WorkflowServiceStatusName

 
                from 
                public.""User"" as u
                join public.""NtsNote"" as n on n.""OwnerUserId""=u.""Id"" and n.""Id""='{parentId}' and  (n.""IsArchived"" <> true or n.""IsArchived"" is null) and n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}'   and u.""Id""='{UserId}'
                join cms.""N_GENERAL_FOLDER_WORKSPACE"" as ws on ws.""NtsNoteId""=n.""Id"" and ws.""IsDeleted""=false and ws.""CompanyId""='{_repo.UserContext.CompanyId}' 
                join public.""LOV"" as lv on lv.""Id""=ws.""TypeId"" and lv.""IsDeleted""=false and  lv.""Code"" in ('MY_WORKSPACE')
                join public.""Template"" as tm on tm.""Id""=n.""TemplateId"" and tm.""IsDeleted""=false and tm.""CompanyId""='{_repo.UserContext.CompanyId}' 
                left join public.""TemplateCategory"" as tc on tc.""Id""=tm.""TemplateCategoryId""  and tc.""IsDeleted""=false and tc.""Code""='GENERAL_FOLDER' and tc.""CompanyId""='{_repo.UserContext.CompanyId}' 
                --left join public.""NtsNote"" as pn on pn.""Id""=n.""ParentNoteId"" and pn.""IsDeleted""=false and pn.""CompanyId""='{_repo.UserContext.CompanyId}'
                 left join (select count(*) as ""Count"",""NtsId"" from public.""NtsTag"" as tag
                join public.""NtsNote"" as doc on doc.""Id""=tag.""TagSourceReferenceId"" and (doc.""IsArchived"" <> true or doc.""IsArchived"" is null) and doc.""IsDeleted""=false and doc.""CompanyId""='{_repo.UserContext.CompanyId}' 
                join(select distinct np.""NoteId"",case when npu is null then npwgu.""Id"" else npu.""Id"" end as ""UserId""
                from  public.""DocumentPermission"" as np  
                left join public.""User"" as npu on npu.""Id""=np.""PermittedUserId"" and npu.""Id""='{UserId}' and npu.""IsDeleted""=false and npu.""CompanyId""='{_repo.UserContext.CompanyId}' 			
                left join public.""UserGroupUser"" as npwg on npwg.""UserGroupId""=np.""PermittedUserGroupId""  and npwg.""IsDeleted""=false and npwg.""CompanyId""='{_repo.UserContext.CompanyId}' 
                left join public.""User"" as npwgu on npwgu.""Id""=npwg.""UserId""  and npwgu.""Id""='{UserId}'	 and npwgu.""IsDeleted""=false and npwgu.""CompanyId""='{_repo.UserContext.CompanyId}'
                where np.""IsDeleted""=false	  )
                as np on np.""NoteId""=doc.""Id"" and np.""UserId""='{UserId}' and tag.""IsDeleted""=false and tag.""TagId"" is null and tag.""CompanyId""='{_repo.UserContext.CompanyId}'  group by tag.""NtsId"") as Tags on ""NtsId""=n.""Id""
                where n.""IsDeleted""=false and ws.""IsDeleted""=false  
                and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
                
                UNION
                    
                Select distinct  n.""Id"" as Id,n.""NoteSubject"" as Name
                ,tm.""Code"" as FolderCode,ws.""Id"" as WorkspaceId
                ,'false' as IsSelfWorkspace,false as IsWorkspaceAdmin,true as IsAssignedWorkspace,'Workspace' as FolderType
                , np.""PermissionType""  as PermissionType
                , np.""Access""  as Access
                , np.""InheritedFrom""  as InheritedFrom
               ,np.""AppliesTo""  as AppliesTo
                ,n.""OwnerUserId"" as OwnerUserId,false as IsOwner,pn.""Id"" as ParentId,pn.""NoteSubject"" as ParentName,n.""IsArchived"" as IsArchived,tags.""Count"" as DocCount
                ,n.""DisablePermissionInheritance"" as DisablePermissionInheritance
				,null as DocumentName
                ,null as Description
                ,n.""CreatedDate"" as CreatedDate
                ,n.""VersionNo"" as NoteVersionNo
                ,n.""LockStatus"" as LockStatus
                ,n.""LastUpdatedDate"" as LastUpdatedDate
                ,n.""StartDate"" as Start
                ,coalesce(n.""NoteSubject"", n.""NoteDescription"", '') as Title
                ,tm.""Name"" as DocumentType
                ,tm.""Code"" as TemplateMasterCode
                ,tm.""Id"" as TemplateMasterId
                ,tm.""Id"" as DocumentTypeId
                ,null as DocumentId
                ,null as FileName
                ,null as StatusName
                ,null as NoteStatus
                ,null as tag
                ,null as DocumentApprovalStatusType
                ,null as DocumentsApprovalStatus
                ,null as StageStatus
                ,null as Discipline
                ,null as IssueCode
                ,null as Vendor
                ,null as ProjectFolder
                ,null as OwnerUser
                ,u.""Name"" as CreatedByUser
                ,null as UpdatedByUser
                ,null as ServiceId
                ,null as WorkflowNo
                ,null as WorkflowName
                ,null as WorkflowStatus
                ,null as WorkflowTemplateId
                ,null as WorkflowCode
                ,'N' as NtsId
                ,true as IsAllDay
                , 0 as FileSize
                ,0 as PhotoId
                ,null as TagIds
                ,null as TagNames
                ,null as EnableDocumentChangeRequest
                , null as EnableLock
                ,null as TagCategoryIds
                ,null as TagCategoryNames 
                ,ws.""SequenceOrder"" as SequenceNo
                ,null as ChangeRequestServiceId
                ,n.""NoteNo"" as DocumentNo
                ,null as WorkflowServiceId
                ,null as WorkflowServiceStatus
                ,null as WorkflowServiceStatusName
				
				from
				 public.""NtsNote"" as n 
				 join cms.""N_GENERAL_FOLDER_WORKSPACE"" as ws on ws.""NtsNoteId""=n.""Id"" and n.""ParentNoteId""='{parentId}' and  (n.""IsArchived"" <> true or n.""IsArchived"" is null) and
                n.""IsDeleted""=false and n.""NoteSubject"" <>'My Workspace' and ws.""IsDeleted""=false  and n.""CompanyId""='{_repo.UserContext.CompanyId}'
				   
				 join public.""Template"" as tm on tm.""Id""=n.""TemplateId"" and tm.""IsDeleted""=false and tm.""Code""='WORKSPACE_GENERAL' and tm.""CompanyId""='{_repo.UserContext.CompanyId}'
				 join public.""TemplateCategory"" as tc on tc.""Id""=tm.""TemplateCategoryId""  and tc.""IsDeleted""=false and tc.""Code""='GENERAL_FOLDER' and tc.""CompanyId""='{_repo.UserContext.CompanyId}'
				 left join public.""User"" as u on u.""Id""=n.""OwnerUserId""
				 left join(select np.*,case when npu is null then npwgu.""Id"" else npu.""Id"" end as ""UserId"" 
                from  public.""DocumentPermission"" as np  
                left join public.""User"" as npu on npu.""Id""=np.""PermittedUserId"" and npu.""Id""='{UserId}' and
                npu.""IsDeleted""=false and npu.""CompanyId""='{_repo.UserContext.CompanyId}' 			
				left join public.""UserGroupUser"" as npwg on npwg.""UserGroupId""=np.""PermittedUserGroupId""  and npwg.""IsDeleted""=false and npwg.""CompanyId""='{_repo.UserContext.CompanyId}' 
				left join public.""User"" as npwgu on npwgu.""Id""=npwg.""UserId""  and npwgu.""Id""='{UserId}'	 and npwgu.""IsDeleted""=false and npwgu.""CompanyId""='{_repo.UserContext.CompanyId}'
				where np.""IsDeleted""=false)
				as np on np.""NoteId""=n.""Id"" and np.""UserId""='{UserId}'
                left join public.""NtsNote"" as pn on pn.""Id""=n.""ParentNoteId"" and pn.""IsDeleted""=false and pn.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join (select count(*) as ""Count"",""NtsId"" from public.""NtsTag"" as tag
                join public.""NtsNote"" as doc on doc.""Id""=tag.""TagSourceReferenceId"" and (doc.""IsArchived"" <> true or doc.""IsArchived"" is null) and doc.""IsDeleted""=false and doc.""CompanyId""='{_repo.UserContext.CompanyId}' 
                join(select distinct np.""NoteId"",case when npu is null then npwgu.""Id"" else npu.""Id"" end as ""UserId""
                from  public.""DocumentPermission"" as np  
                left join public.""User"" as npu on npu.""Id""=np.""PermittedUserId"" and npu.""Id""='{UserId}' and npu.""IsDeleted""=false and npu.""CompanyId""='{_repo.UserContext.CompanyId}' 			
                left join public.""UserGroupUser"" as npwg on npwg.""UserGroupId""=np.""PermittedUserGroupId""  and npwg.""IsDeleted""=false and npwg.""CompanyId""='{_repo.UserContext.CompanyId}' 
                left join public.""User"" as npwgu on npwgu.""Id""=npwg.""UserId""  and npwgu.""Id""='{UserId}'	 and npwgu.""IsDeleted""=false and npwgu.""CompanyId""='{_repo.UserContext.CompanyId}'
                where np.""IsDeleted""=false	  )
                as np on np.""NoteId""=doc.""Id"" and np.""UserId""='{UserId}' and tag.""IsDeleted""=false and tag.""TagId"" is null and tag.""CompanyId""='{_repo.UserContext.CompanyId}'  group by tag.""NtsId"") as Tags on ""NtsId""=n.""Id""
				left join public.""LOV"" as lv on lv.""Id""=ws.""TypeId"" and lv.""IsDeleted""=false  and lv.""CompanyId""='{_repo.UserContext.CompanyId}' 
				where  lv.""Code"" <>'MY_WORKSPACE' or lv.""Code"" is null and n.""IsDeleted""=false and ws.""IsDeleted""=false  and n.""CompanyId""='{_repo.UserContext.CompanyId}' 
                UNION
                Select distinct  n.""Id"" as Id,n.""NoteSubject"" as Name
                ,tm.""Code"" as FolderCode,ws.""Id"" as WorkspaceId
                ,'false' as IsSelfWorkspace,false as IsWorkspaceAdmin,true as IsAssignedWorkspace,'Workspace' as FolderType
                , np.""PermissionType""  as PermissionType
                , np.""Access""  as Access
                , np.""InheritedFrom""  as InheritedFrom
               ,np.""AppliesTo""  as AppliesTo
                ,n.""OwnerUserId"" as OwnerUserId,false as IsOwner,pn.""Id"" as ParentId,pn.""NoteSubject"" as ParentName,n.""IsArchived"" as IsArchived,tags.""Count"" as DocCount
                ,n.""DisablePermissionInheritance"" as DisablePermissionInheritance
				,null as DocumentName
                ,null as Description
                ,n.""CreatedDate"" as CreatedDate
                ,n.""VersionNo"" as NoteVersionNo
                ,n.""LockStatus"" as LockStatus
                ,n.""LastUpdatedDate"" as LastUpdatedDate
                ,n.""StartDate"" as Start
                ,coalesce(n.""NoteSubject"", n.""NoteDescription"", '') as Title
                ,tm.""Name"" as DocumentType
                ,tm.""Code"" as TemplateMasterCode
                ,tm.""Id"" as TemplateMasterId
                ,tm.""Id"" as DocumentTypeId
                ,null as DocumentId
                ,null as FileName
                ,null as StatusName
                ,null as NoteStatus
                ,null as tag
                ,null as DocumentApprovalStatusType
                ,null as DocumentsApprovalStatus
                ,null as StageStatus
                ,null as Discipline
                ,null as IssueCode
                ,null as Vendor
                ,null as ProjectFolder
                ,null as OwnerUser
                ,u.""Name"" as CreatedByUser
                ,null as UpdatedByUser
                ,null as ServiceId
                ,null as WorkflowNo
                ,null as WorkflowName
                ,null as WorkflowStatus
                ,null as WorkflowTemplateId
                ,null as WorkflowCode
                ,'N' as NtsId
                ,true as IsAllDay
                , 0 as FileSize
                ,0 as PhotoId
                ,null as TagIds
                ,null as TagNames
                ,null as EnableDocumentChangeRequest
                , null as EnableLock
                ,null as TagCategoryIds
                ,null as TagCategoryNames 
                ,ws.""SequenceOrder"" as SequenceNo
                ,null as ChangeRequestServiceId
                ,n.""NoteNo"" as DocumentNo
                ,null as WorkflowServiceId
                ,null as WorkflowServiceStatus
                ,null as WorkflowServiceStatusName
				
				from
				 public.""NtsNote"" as n 
				 join cms.""N_GENERAL_FOLDER_WORKSPACE"" as ws on ws.""NtsNoteId""=n.""Id"" and n.""Id""='{parentId}' and  (n.""IsArchived"" <> true or n.""IsArchived"" is null) and
                n.""IsDeleted""=false and n.""NoteSubject"" <>'My Workspace' and ws.""IsDeleted""=false  and n.""CompanyId""='{_repo.UserContext.CompanyId}'
				   
				 join public.""Template"" as tm on tm.""Id""=n.""TemplateId"" and tm.""IsDeleted""=false and tm.""Code""='WORKSPACE_GENERAL' and tm.""CompanyId""='{_repo.UserContext.CompanyId}'
				 join public.""TemplateCategory"" as tc on tc.""Id""=tm.""TemplateCategoryId""  and tc.""IsDeleted""=false and tc.""Code""='GENERAL_FOLDER' and tc.""CompanyId""='{_repo.UserContext.CompanyId}'
				 left join public.""User"" as u on u.""Id""=n.""OwnerUserId""
				 left join(select np.*,case when npu is null then npwgu.""Id"" else npu.""Id"" end as ""UserId"" 
                from  public.""DocumentPermission"" as np  
                left join public.""User"" as npu on npu.""Id""=np.""PermittedUserId"" and npu.""Id""='{UserId}' and
                npu.""IsDeleted""=false and npu.""CompanyId""='{_repo.UserContext.CompanyId}' 			
				left join public.""UserGroupUser"" as npwg on npwg.""UserGroupId""=np.""PermittedUserGroupId""  and npwg.""IsDeleted""=false and npwg.""CompanyId""='{_repo.UserContext.CompanyId}' 
				left join public.""User"" as npwgu on npwgu.""Id""=npwg.""UserId""  and npwgu.""Id""='{UserId}'	 and npwgu.""IsDeleted""=false and npwgu.""CompanyId""='{_repo.UserContext.CompanyId}'
				where np.""IsDeleted""=false)
				as np on np.""NoteId""=n.""Id"" and np.""UserId""='{UserId}'
                left join public.""NtsNote"" as pn on pn.""Id""=n.""ParentNoteId"" and pn.""IsDeleted""=false and pn.""CompanyId""='{_repo.UserContext.CompanyId}'
                 left join (select count(*) as ""Count"",""NtsId"" from public.""NtsTag"" as tag
                join public.""NtsNote"" as doc on doc.""Id""=tag.""TagSourceReferenceId"" and (doc.""IsArchived"" <> true or doc.""IsArchived"" is null) and doc.""IsDeleted""=false and doc.""CompanyId""='{_repo.UserContext.CompanyId}' 
                join(select distinct np.""NoteId"",case when npu is null then npwgu.""Id"" else npu.""Id"" end as ""UserId""
                from  public.""DocumentPermission"" as np  
                left join public.""User"" as npu on npu.""Id""=np.""PermittedUserId"" and npu.""Id""='{UserId}' and npu.""IsDeleted""=false and npu.""CompanyId""='{_repo.UserContext.CompanyId}' 			
                left join public.""UserGroupUser"" as npwg on npwg.""UserGroupId""=np.""PermittedUserGroupId""  and npwg.""IsDeleted""=false and npwg.""CompanyId""='{_repo.UserContext.CompanyId}' 
                left join public.""User"" as npwgu on npwgu.""Id""=npwg.""UserId""  and npwgu.""Id""='{UserId}'	 and npwgu.""IsDeleted""=false and npwgu.""CompanyId""='{_repo.UserContext.CompanyId}'
                where np.""IsDeleted""=false	  )
                as np on np.""NoteId""=doc.""Id"" and np.""UserId""='{UserId}' and tag.""IsDeleted""=false and tag.""TagId"" is null and tag.""CompanyId""='{_repo.UserContext.CompanyId}'  group by tag.""NtsId"") as Tags on ""NtsId""=n.""Id""
				left join public.""LOV"" as lv on lv.""Id""=ws.""TypeId"" and lv.""IsDeleted""=false  and lv.""CompanyId""='{_repo.UserContext.CompanyId}' 
				where  lv.""Code"" <>'MY_WORKSPACE' or lv.""Code"" is null and n.""IsDeleted""=false and ws.""IsDeleted""=false  and n.""CompanyId""='{_repo.UserContext.CompanyId}' 

                union
                Select distinct f.""Id"" as Id,f.""NoteSubject"" as Name                
                ,tm.""Code"" as FolderCode,ws.""Id"" as WorkspaceId
                ,case when wn.""NoteSubject""='My Workspace' then 'true' else 'false' end as IsSelfWorkspace,false as IsWorkspaceAdmin,false as IsAssignedWorkspace,'Folder' as FolderType
                , np.""PermissionType""  as PermissionType
                , np.""Access""  as Access
                , np.""InheritedFrom""  as InheritedFrom
                , np.""AppliesTo""  as AppliesTo
                ,null as OwnerUserId,false as IsOwner,pn.""Id"" as ParentId,pn.""NoteSubject"" as ParentName,f.""IsArchived"" as IsArchived
				,tags.""Count"" as DocCount
                ,f.""DisablePermissionInheritance"" as DisablePermissionInheritance
                ,null as DocumentName
                ,null as Description
                ,f.""CreatedDate"" as CreatedDate
                ,f.""VersionNo"" as NoteVersionNo
                ,f.""LockStatus"" as LockStatus
                ,f.""LastUpdatedDate"" as LastUpdatedDate
                ,f.""StartDate"" as Start
                ,coalesce(f.""NoteSubject"", f.""NoteDescription"", '') as Title
                ,tm.""Name"" as DocumentType
                ,tm.""Code"" as TemplateMasterCode
                ,tm.""Id"" as TemplateMasterId
                ,tm.""Id"" as DocumentTypeId
                ,null as DocumentId
                ,null as FileName
                ,null as StatusName
                ,null as NoteStatus
                ,null as tag
                ,null as DocumentApprovalStatusType
                ,null as DocumentsApprovalStatus
                ,null as StageStatus
                ,null as Discipline
                ,null as IssueCode
                ,null as Vendor
                ,null as ProjectFolder
                ,null as OwnerUser
                ,u.""Name"" as CreatedByUser
                ,null as UpdatedByUser
                ,null as ServiceId
                ,null as WorkflowNo
                ,null as WorkflowName
                ,null as WorkflowStatus
                ,null as WorkflowTemplateId
                ,null as WorkflowCode
                ,'N' as NtsId
                ,true as IsAllDay
                , 0 as FileSize
                ,0 as PhotoId
                ,null as TagIds
                ,null as TagNames
                ,null as EnableDocumentChangeRequest
                , null as EnableLock
                ,null as TagCategoryIds
                ,null as TagCategoryNames 
                ,f.""SequenceOrder"" as SequenceNo
                ,null as ChangeRequestServiceId
                ,f.""NoteNo"" as DocumentNo
                ,null as WorkflowServiceId
                ,null as WorkflowServiceStatus
                ,null as WorkflowServiceStatusName
								
                from 

				public.""NtsNote"" as f  
				join cms.""N_GENERAL_FOLDER_GENERALFOLDER"" as ws1 on ws1.""NtsNoteId""=f.""Id"" and f.""ParentNoteId""='{parentId}' and  (f.""IsArchived"" <> true or f.""IsArchived"" is null)	and f.""IsDeleted""=false and ws1.""IsDeleted""=false	and f.""CompanyId""='{_repo.UserContext.CompanyId}'				
				join public.""Template"" as tm on tm.""Id""=f.""TemplateId"" and tm.""IsDeleted""=false and tm.""Code""='GENERAL_FOLDER' and tm.""CompanyId""='{_repo.UserContext.CompanyId}'
				join public.""TemplateCategory"" as tc on tc.""Id""=tm.""TemplateCategoryId""  and tc.""IsDeleted""=false and tc.""Code""='GENERAL_FOLDER' and tc.""CompanyId""='{_repo.UserContext.CompanyId}'
				join public.""NtsNote"" as pn on pn.""Id""=f.""ParentNoteId"" and pn.""IsDeleted""=false and pn.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join cms.""N_GENERAL_FOLDER_WORKSPACE"" as ws on ws.""Id""=ws1.""WorkspaceId"" and ws.""IsDeleted""=false and ws1.""CompanyId""='{_repo.UserContext.CompanyId}'
				left join public.""NtsNote"" as wn on wn.""Id""=ws.""NtsNoteId"" and wn.""IsDeleted""=false
                left join public.""User"" as u on u.""Id""=f.""OwnerUserId""

	            left join(select np.*,case when npu is null then npwgu.""Id"" else npu.""Id"" end as ""UserId"" 
                from  public.""DocumentPermission"" as np 
                left join public.""User"" as npu on npu.""Id""=np.""PermittedUserId"" and npu.""Id""='{UserId}' and npu.""IsDeleted""=false and npu.""CompanyId""='{_repo.UserContext.CompanyId}' 
			
				left join public.""UserGroupUser"" as npwg on npwg.""UserGroupId""=np.""PermittedUserGroupId""  and npwg.""IsDeleted""=false and npwg.""CompanyId""='{_repo.UserContext.CompanyId}' 
				left join public.""User"" as npwgu on npwgu.""Id""=npwg.""UserId""  and npwgu.""Id""='{UserId}'	 and npwgu.""IsDeleted""=false and npwgu.""CompanyId""='{_repo.UserContext.CompanyId}'
					where np.""IsDeleted""=false)
				as np on np.""NoteId""=f.""Id"" and np.""UserId""='{UserId}'
                 left join (select count(*) as ""Count"",""NtsId"" from public.""NtsTag"" as tag
                join public.""NtsNote"" as doc on doc.""Id""=tag.""TagSourceReferenceId"" and (doc.""IsArchived"" <> true or doc.""IsArchived"" is null) and doc.""IsDeleted""=false and doc.""CompanyId""='{_repo.UserContext.CompanyId}' 
                join(select distinct np.""NoteId"",case when npu is null then npwgu.""Id"" else npu.""Id"" end as ""UserId""
                from  public.""DocumentPermission"" as np  
                left join public.""User"" as npu on npu.""Id""=np.""PermittedUserId"" and npu.""Id""='{UserId}' and npu.""IsDeleted""=false and npu.""CompanyId""='{_repo.UserContext.CompanyId}' 			
                left join public.""UserGroupUser"" as npwg on npwg.""UserGroupId""=np.""PermittedUserGroupId""  and npwg.""IsDeleted""=false and npwg.""CompanyId""='{_repo.UserContext.CompanyId}' 
                left join public.""User"" as npwgu on npwgu.""Id""=npwg.""UserId""  and npwgu.""Id""='{UserId}'	 and npwgu.""IsDeleted""=false and npwgu.""CompanyId""='{_repo.UserContext.CompanyId}'
                where np.""IsDeleted""=false	  )
                as np on np.""NoteId""=doc.""Id"" and np.""UserId""='{UserId}' and tag.""IsDeleted""=false and tag.""TagId"" is null and tag.""CompanyId""='{_repo.UserContext.CompanyId}'  group by tag.""NtsId"") as Tags on ""NtsId""=f.""Id""
                UNION
                Select distinct f.""Id"" as Id,f.""NoteSubject"" as Name                
                ,tm.""Code"" as FolderCode,ws.""Id"" as WorkspaceId
                ,case when wn.""NoteSubject""='My Workspace' then 'true' else 'false' end as IsSelfWorkspace,false as IsWorkspaceAdmin,false as IsAssignedWorkspace,'Folder' as FolderType
                , np.""PermissionType""  as PermissionType
                , np.""Access""  as Access
                , np.""InheritedFrom""  as InheritedFrom
                , np.""AppliesTo""  as AppliesTo
                ,null as OwnerUserId,false as IsOwner,pn.""Id"" as ParentId,pn.""NoteSubject"" as ParentName,f.""IsArchived"" as IsArchived
				,tags.""Count"" as DocCount
                ,f.""DisablePermissionInheritance"" as DisablePermissionInheritance
				,null as DocumentName
                ,null as Description
                ,f.""CreatedDate"" as CreatedDate
                ,f.""VersionNo"" as NoteVersionNo
                ,f.""LockStatus"" as LockStatus
                ,f.""LastUpdatedDate"" as LastUpdatedDate
                ,f.""StartDate"" as Start
                ,coalesce(f.""NoteSubject"", f.""NoteDescription"", '') as Title
                ,tm.""Name"" as DocumentType
                ,tm.""Code"" as TemplateMasterCode
                ,tm.""Id"" as TemplateMasterId
                ,tm.""Id"" as DocumentTypeId
                ,null as DocumentId
                ,null as FileName
                ,null as StatusName
                ,null as NoteStatus
                ,null as tag
                ,null as DocumentApprovalStatusType
                ,null as DocumentsApprovalStatus
                ,null as StageStatus
                ,null as Discipline
                ,null as IssueCode
                ,null as Vendor
                ,null as ProjectFolder
                ,null as OwnerUser
                ,u.""Name"" as CreatedByUser
                ,null as UpdatedByUser
                ,null as ServiceId
                ,null as WorkflowNo
                ,null as WorkflowName
                ,null as WorkflowStatus
                ,null as WorkflowTemplateId
                ,null as WorkflowCode
                ,'N' as NtsId
                ,true as IsAllDay
                , 0 as FileSize
                ,0 as PhotoId
                ,null as TagIds
                ,null as TagNames
                ,null as EnableDocumentChangeRequest
                , null as EnableLock
                ,null as TagCategoryIds
                ,null as TagCategoryNames 
                ,f.""SequenceOrder"" as SequenceNo
                ,null as ChangeRequestServiceId
                ,f.""NoteNo"" as DocumentNo
                ,null as WorkflowServiceId
                ,null as WorkflowServiceStatus
                ,null as WorkflowServiceStatusName

                from 

				public.""NtsNote"" as f  
				join cms.""N_GENERAL_FOLDER_GENERALFOLDER"" as ws1 on ws1.""NtsNoteId""=f.""Id"" and f.""Id""='{parentId}' and  (f.""IsArchived"" <> true or f.""IsArchived"" is null)	and f.""IsDeleted""=false and ws1.""IsDeleted""=false	and f.""CompanyId""='{_repo.UserContext.CompanyId}'				
				join public.""Template"" as tm on tm.""Id""=f.""TemplateId"" and tm.""IsDeleted""=false and tm.""Code""='GENERAL_FOLDER' and tm.""CompanyId""='{_repo.UserContext.CompanyId}'
				join public.""TemplateCategory"" as tc on tc.""Id""=tm.""TemplateCategoryId""  and tc.""IsDeleted""=false and tc.""Code""='GENERAL_FOLDER' and tc.""CompanyId""='{_repo.UserContext.CompanyId}'
				join public.""NtsNote"" as pn on pn.""Id""=f.""ParentNoteId"" and pn.""IsDeleted""=false and pn.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join cms.""N_GENERAL_FOLDER_WORKSPACE"" as ws on ws.""Id""=ws1.""WorkspaceId"" and ws.""IsDeleted""=false and ws1.""CompanyId""='{_repo.UserContext.CompanyId}'
				left join public.""NtsNote"" as wn on wn.""Id""=ws.""NtsNoteId"" and wn.""IsDeleted""=false
                left join public.""User"" as u on u.""Id""=f.""OwnerUserId""


	            left join(select np.*,case when npu is null then npwgu.""Id"" else npu.""Id"" end as ""UserId"" 
                from  public.""DocumentPermission"" as np 
                left join public.""User"" as npu on npu.""Id""=np.""PermittedUserId"" and npu.""Id""='{UserId}' and npu.""IsDeleted""=false and npu.""CompanyId""='{_repo.UserContext.CompanyId}' 
			
				left join public.""UserGroupUser"" as npwg on npwg.""UserGroupId""=np.""PermittedUserGroupId""  and npwg.""IsDeleted""=false and npwg.""CompanyId""='{_repo.UserContext.CompanyId}' 
				left join public.""User"" as npwgu on npwgu.""Id""=npwg.""UserId""  and npwgu.""Id""='{UserId}'	 and npwgu.""IsDeleted""=false and npwgu.""CompanyId""='{_repo.UserContext.CompanyId}'
					where np.""IsDeleted""=false)
				as np on np.""NoteId""=f.""Id"" and np.""UserId""='{UserId}'
                 left join (select count(*) as ""Count"",""NtsId"" from public.""NtsTag"" as tag
                join public.""NtsNote"" as doc on doc.""Id""=tag.""TagSourceReferenceId"" and (doc.""IsArchived"" <> true or doc.""IsArchived"" is null) and doc.""IsDeleted""=false and doc.""CompanyId""='{_repo.UserContext.CompanyId}' 
                join(select distinct np.""NoteId"",case when npu is null then npwgu.""Id"" else npu.""Id"" end as ""UserId""
                from  public.""DocumentPermission"" as np  
                left join public.""User"" as npu on npu.""Id""=np.""PermittedUserId"" and npu.""Id""='{UserId}' and npu.""IsDeleted""=false and npu.""CompanyId""='{_repo.UserContext.CompanyId}' 			
                left join public.""UserGroupUser"" as npwg on npwg.""UserGroupId""=np.""PermittedUserGroupId""  and npwg.""IsDeleted""=false and npwg.""CompanyId""='{_repo.UserContext.CompanyId}' 
                left join public.""User"" as npwgu on npwgu.""Id""=npwg.""UserId""  and npwgu.""Id""='{UserId}'	 and npwgu.""IsDeleted""=false and npwgu.""CompanyId""='{_repo.UserContext.CompanyId}'
                where np.""IsDeleted""=false	  )
                as np on np.""NoteId""=doc.""Id"" and np.""UserId""='{UserId}' and tag.""IsDeleted""=false and tag.""TagId"" is null and tag.""CompanyId""='{_repo.UserContext.CompanyId}'  group by tag.""NtsId"") as Tags on ""NtsId""=f.""Id""
                UNION
            
                select f.""Id"" as Id,f.""NoteSubject"" as Name,tm.""Code"" as FolderCode,null as WorkspaceId
                ,case when wn.""NoteSubject""='My Workspace' then 'true' else 'false' end as IsSelfWorkspace,false as IsWorkspaceAdmin,false as IsAssignedWorkspace,'Document' as FolderType
                ,0 as PermissionType,2 as Access
                ,'' as InheritedFrom
                ,3 as AppliesTo
                ,u.""Id"" as OwnerUserId,true as IsOwner,pn.""Id"" as ParentId,pn.""NoteSubject"" as ParentName
                ,f.""IsArchived"" as IsArchived
                ,0 as DocCount
                ,f.""DisablePermissionInheritance"" as DisablePermissionInheritance
                ,f.""NoteSubject"" as DocumentName,f.""NoteDescription"" as Description,f.""CreatedDate"" as CreatedDate
                ,f.""VersionNo"" as NoteVersionNo
                ,f.""LockStatus"" as LockStatus
                ,f.""LastUpdatedDate"" as LastUpdatedDate
                ,f.""StartDate"" as Start,coalesce(f.""NoteSubject"", f.""NoteDescription"", '') as Title
                ,tm.""Name"" as DocumentType,tm.""Code"" as TemplateMasterCode,tm.""Id"" as TemplateMasterId
                ,tm.""Id"" as DocumentTypeId
                ,udf.""DocumentId"" as DocumentId
                ,gen.""FileName"" as FileName
                ,lov.""Name"" as StatusName,lov.""Code"" as NoteStatus
                ,null as tag
                , dast.""Name"" as DocumentApprovalStatusType
                , udf.""documentApprovalStatus"" as DocumentsApprovalStatus,udf.""stageStatus"" as StageStatus,
                udf.""discipline"" as Discipline,coalesce(udf.""code"",udf.""OutgoingIssueCodes"",udf.""issueCodes"") as IssueCode
                ,udf.""vendorList"" as Vendor,udf.""projectFolder"" as ProjectFolder
                ,u.""Name"" as OwnerUser, u.""Name"" as CreatedByUser, u.""Name"" as UpdatedByUser
                ,s.""Id"" as ServiceId,s.""ServiceNo"" as WorkflowNo,s.""ServiceSubject"" as WorkflowName
                ,null as WorkflowStatus, st.""Id"" as WorkflowTemplateId,st.""Code"" as WorkflowCode
                ,'N' as NtsId,true as IsAllDay, gen.""ContentLength"" as FileSize,0 as PhotoId
                ,ntg.""TagId"" as TagIds,tg.""NoteSubject"" as TagNames
                ,null as EnableDocumentChangeRequest, null as EnableLock
                ,ntg.""TagCategoryId"" as TagCategoryIds,tgc.""NoteSubject"" as TagCategoryNames 
                , f.""SequenceOrder"" as SequenceNo,
                '' as ChangeRequestServiceId,f.""NoteNo"" as DocumentNo,wfs.""Id"" as WorkflowServiceId,wfss.""Code"" as WorkflowServiceStatus,wfss.""Name"" as WorkflowServiceStatusName

                from 
                public.""User"" as u
                join public.""NtsNote"" as f on f.""OwnerUserId""=u.""Id"" and u.""Id""='{UserId}' and f.""ParentNoteId""='{parentId}' and f.""IsDeleted""=false and f.""CompanyId""='{_repo.UserContext.CompanyId}' and  (f.""IsArchived"" <> true or f.""IsArchived"" is null) and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
                join public.""LOV"" as lov on lov.""Id""=f.""NoteStatusId"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join (" + udfs + $@") as udf on udf.""NtsNoteId""=f.""Id"" 

                join public.""Template"" as tm on tm.""Id""=f.""TemplateId"" and tm.""IsDeleted""=false and tm.""CompanyId""='{_repo.UserContext.CompanyId}'--and tm.""Code""='GENERAL_FOLDER'
                join public.""TemplateCategory"" as tc on tc.""Id""=tm.""TemplateCategoryId""  and tc.""IsDeleted""=false and tc.""CompanyId""='{_repo.UserContext.CompanyId}' and tc.""Code""='GENERAL_DOCUMENT'
                join public.""NtsNote"" as pn on pn.""Id""=f.""ParentNoteId"" and pn.""IsDeleted""=false and pn.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join public.""LOV"" as dast on dast.""Id""=udf.""documentApprovalStatusType"" and dast.""IsDeleted""=false and dast.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join public.""NtsService"" as s on s.""Id""=f.""ParentServiceId"" and s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join public.""NtsService"" as wfs on wfs.""Id""=f.""ReferenceId"" and wfs.""IsDeleted""=false and wfs.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join public.""LOV"" as wfss on wfss.""Id""=wfs.""ServiceStatusId"" and wfss.""IsDeleted""=false and wfss.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join public.""File"" as gen on gen.""Id""=udf.""DocumentId"" and gen.""IsDeleted""=false and gen.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join public.""Template"" as st on st.""Id""=tm.""WorkFlowTemplateId"" and st.""IsDeleted""=false and st.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join public.""NtsTag"" as ntg on ntg.""NtsId""=f.""Id"" and ntg.""IsDeleted""=false and ntg.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join public.""NtsNote"" as tg on tg.""Id""=ntg.""TagId"" and tg.""IsDeleted""=false and tg.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join public.""NtsNote"" as tgc on tgc.""Id""=tg.""ParentNoteId"" and tgc.""IsDeleted""=false and tgc.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join cms.""N_GENERAL_FOLDER_WORKSPACE"" as ws on ws.""Id""=udf.""WorkspaceId"" and ws.""IsDeleted""=false and ws.""CompanyId""='{_repo.UserContext.CompanyId}'
				left join public.""NtsNote"" as wn on wn.""Id""=ws.""NtsNoteId"" and wn.""IsDeleted""=false
                
                UNION

                select f.""Id"" as Id,f.""NoteSubject"" as Name,tm.""Code"" as FolderCode,null as WorkspaceId
                ,case when wn.""NoteSubject""='My Workspace' then 'true' else 'false' end as IsSelfWorkspace,false as IsWorkspaceAdmin,false as IsAssignedWorkspace,'Document' as FolderType
                ,0 as PermissionType,2 as Access
                ,'' as InheritedFrom
                ,3 as AppliesTo
                ,u.""Id"" as OwnerUserId,true as IsOwner,pn.""Id"" as ParentId,pn.""NoteSubject"" as ParentName
                ,f.""IsArchived"" as IsArchived
                ,0 as DocCount
                ,f.""DisablePermissionInheritance"" as DisablePermissionInheritance
                ,f.""NoteSubject"" as DocumentName,f.""NoteDescription"" as Description,f.""CreatedDate"" as CreatedDate
                ,f.""VersionNo"" as NoteVersionNo
                ,f.""LockStatus"" as LockStatus
                ,f.""LastUpdatedDate"" as LastUpdatedDate
                ,f.""StartDate"" as Start,coalesce(f.""NoteSubject"", f.""NoteDescription"", '') as Title
                ,tm.""Name"" as DocumentType,tm.""Code"" as TemplateMasterCode,tm.""Id"" as TemplateMasterId
                ,tm.""Id"" as DocumentTypeId
                ,udf.""DocumentId"" as DocumentId
                ,gen.""FileName"" as FileName
                ,lov.""Name"" as StatusName,lov.""Code"" as NoteStatus
                ,null as tag
                , dast.""Name"" as DocumentApprovalStatusType
                , udf.""documentApprovalStatus"" as DocumentsApprovalStatus,udf.""stageStatus"" as StageStatus,
                udf.""discipline"" as Discipline,coalesce(udf.""code"",udf.""OutgoingIssueCodes"",udf.""issueCodes"") as IssueCode
                ,udf.""vendorList"" as Vendor,udf.""projectFolder"" as ProjectFolder
                ,u.""Name"" as OwnerUser, u.""Name"" as CreatedByUser, u.""Name"" as UpdatedByUser
                ,s.""Id"" as ServiceId,s.""ServiceNo"" as WorkflowNo,s.""ServiceSubject"" as WorkflowName
                ,null as WorkflowStatus, st.""Id"" as WorkflowTemplateId,st.""Code"" as WorkflowCode
                ,'N' as NtsId,true as IsAllDay, gen.""ContentLength"" as FileSize,0 as PhotoId
                ,ntg.""TagId"" as TagIds,tg.""NoteSubject"" as TagNames
                ,null as EnableDocumentChangeRequest, null as EnableLock
                ,ntg.""TagCategoryId"" as TagCategoryIds,tgc.""NoteSubject"" as TagCategoryNames 
                , f.""SequenceOrder"" as SequenceNo,
                '' as ChangeRequestServiceId,f.""NoteNo"" as DocumentNo,wfs.""Id"" as WorkflowServiceId,wfss.""Code"" as WorkflowServiceStatus,wfss.""Name"" as WorkflowServiceStatusName

                from 
                public.""User"" as u
                join public.""NtsNote"" as f on f.""OwnerUserId""=u.""Id"" and u.""Id""='{UserId}' and f.""Id""='{parentId}' and f.""IsDeleted""=false and f.""CompanyId""='{_repo.UserContext.CompanyId}' and  (f.""IsArchived"" <> true or f.""IsArchived"" is null) and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
                join public.""LOV"" as lov on lov.""Id""=f.""NoteStatusId"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join (" + udfs + $@") as udf on udf.""NtsNoteId""=f.""Id"" 

                join public.""Template"" as tm on tm.""Id""=f.""TemplateId"" and tm.""IsDeleted""=false and tm.""CompanyId""='{_repo.UserContext.CompanyId}'--and tm.""Code""='GENERAL_FOLDER'
                join public.""TemplateCategory"" as tc on tc.""Id""=tm.""TemplateCategoryId""  and tc.""IsDeleted""=false and tc.""CompanyId""='{_repo.UserContext.CompanyId}' and tc.""Code""='GENERAL_DOCUMENT'
                join public.""NtsNote"" as pn on pn.""Id""=f.""ParentNoteId"" and pn.""IsDeleted""=false and pn.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join public.""LOV"" as dast on dast.""Id""=udf.""documentApprovalStatusType"" and dast.""IsDeleted""=false and dast.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join public.""NtsService"" as s on s.""Id""=f.""ParentServiceId"" and s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join public.""NtsService"" as wfs on wfs.""Id""=f.""ReferenceId"" and wfs.""IsDeleted""=false and wfs.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join public.""LOV"" as wfss on wfss.""Id""=wfs.""ServiceStatusId"" and wfss.""IsDeleted""=false and wfss.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join public.""File"" as gen on gen.""Id""=udf.""DocumentId"" and gen.""IsDeleted""=false and gen.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join public.""Template"" as st on st.""Id""=tm.""WorkFlowTemplateId"" and st.""IsDeleted""=false and st.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join public.""NtsTag"" as ntg on ntg.""NtsId""=f.""Id"" and ntg.""IsDeleted""=false and ntg.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join public.""NtsNote"" as tg on tg.""Id""=ntg.""TagId"" and tg.""IsDeleted""=false and tg.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join public.""NtsNote"" as tgc on tgc.""Id""=tg.""ParentNoteId"" and tgc.""IsDeleted""=false and tgc.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join cms.""N_GENERAL_FOLDER_WORKSPACE"" as ws on ws.""Id""=udf.""WorkspaceId"" and ws.""IsDeleted""=false and ws.""CompanyId""='{_repo.UserContext.CompanyId}'
				left join public.""NtsNote"" as wn on wn.""Id""=ws.""NtsNoteId"" and wn.""IsDeleted""=false
                
                UNION

                Select f.""Id"" as Id,f.""NoteSubject"" as Name,tm.""Code"" as FolderCode,null as WorkspaceId
                ,case when wn.""NoteSubject""='My Workspace' then 'true' else 'false' end as IsSelfWorkspace,false as IsWorkspaceAdmin,false as IsAssignedWorkspace,'Document' as FolderType
                ,np.""PermissionType"" as PermissionType,np.""Access"" as Access
                ,'' as InheritedFrom
                ,np.""AppliesTo"" as AppliesTo
                ,u.""Id"" as OwnerUserId,true as IsOwner,pn.""Id"" as ParentId,pn.""NoteSubject"" as ParentName
                ,f.""IsArchived"" as IsArchived
                ,0 as DocCount
                ,f.""DisablePermissionInheritance"" as DisablePermissionInheritance
                ,f.""NoteSubject"" as DocumentName,f.""NoteDescription"" as Description,f.""CreatedDate"" as CreatedDate
                ,f.""VersionNo"" as NoteVersionNo
                ,f.""LockStatus"" as LockStatus
                ,f.""LastUpdatedDate"" as LastUpdatedDate
                ,f.""StartDate"" as Start,coalesce(f.""NoteSubject"", f.""NoteDescription"", '') as Title
                ,tm.""Name"" as DocumentType,tm.""Code"" as TemplateMasterCode,tm.""Id"" as TemplateMasterId
                ,tm.""Id"" as DocumentTypeId
                , udf.""DocumentId"" as DocumentId
                ,gen.""FileName"" as FileName
                ,lov.""Name"" as StatusName,lov.""Code"" as NoteStatus
                ,null as tag
                , dast.""Name"" as DocumentApprovalStatusType
                , udf.""documentApprovalStatus"" as DocumentsApprovalStatus,udf.""stageStatus"" as StageStatus,
                udf.""discipline"" as Discipline,coalesce(udf.""code"",udf.""OutgoingIssueCodes"",udf.""issueCodes"") as IssueCode
                ,udf.""vendorList"" as Vendor,udf.""projectFolder"" as ProjectFolder
                ,u.""Name"" as OwnerUser, u.""Name"" as CreatedByUser, u.""Name"" as UpdatedByUser
                ,s.""Id"" as ServiceId,s.""ServiceNo"" as WorkflowNo,s.""ServiceSubject"" as WorkflowName
                ,null as WorkflowStatus, null as WorkflowTemplateId,st.""Code"" as WorkflowCode
                ,'N' as NtsId,true as IsAllDay, gen.""ContentLength"" as FileSize,0 as PhotoId
                ,'' as TagIds,'' as TagNames
                ,null as EnableDocumentChangeRequest
                , null as EnableLock
                ,'' as TagCategoryIds
                ,'' as TagCategoryNames 
                , f.""SequenceOrder"" as SequenceNo
                ,cs.""Id"" as ChangeRequestServiceId
                ,f.""NoteNo"" as DocumentNo,null as WorkflowServiceId,null as WorkflowServiceStatus,null as WorkflowServiceStatusName
				
                from 
                public.""User"" as u
                join public.""NtsNote"" as f on f.""OwnerUserId""<>u.""Id"" and u.""Id""='{UserId}' and f.""ParentNoteId""='{parentId}' and f.""IsDeleted""=false and f.""CompanyId""='{_repo.UserContext.CompanyId}' and  (f.""IsArchived"" <> true or f.""IsArchived"" is null)
                and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}' 
                join public.""User"" as ou on f.""OwnerUserId""=ou.""Id"" and ou.""IsDeleted""=false and ou.""CompanyId""='{_repo.UserContext.CompanyId}'
                join public.""LOV"" as lov on lov.""Id""=f.""NoteStatusId"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_repo.UserContext.CompanyId}'	

                join public.""Template"" as tm on tm.""Id""=f.""TemplateId"" and tm.""IsDeleted""=false and tm.""CompanyId""='{_repo.UserContext.CompanyId}' --and tm.""Code""='GENERAL_FOLDER'
                join public.""TemplateCategory"" as tc on tc.""Id""=tm.""TemplateCategoryId""  and tc.""IsDeleted""=false and tc.""CompanyId""='{_repo.UserContext.CompanyId}' and tc.""Code""='GENERAL_DOCUMENT'
                left join (" + udfs + $@") as udf on udf.""NtsNoteId""=f.""Id""  	

                join public.""NtsNote"" as pn on pn.""Id""=f.""ParentNoteId"" and pn.""IsDeleted""=false and pn.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join public.""LOV"" as dast on dast.""Id""=udf.""documentApprovalStatusType"" and dast.""IsDeleted""=false and dast.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join(select np1.*,case when npu is null then npwgu.""Id"" else npu.""Id"" end as ""UserId""
                from  public.""DocumentPermission"" as np1
                left join public.""User"" as npu on npu.""Id""=np1.""PermittedUserId"" and npu.""Id""='{UserId}' and npu.""IsDeleted""=false and npu.""CompanyId""='{_repo.UserContext.CompanyId}' 


                left join public.""UserGroupUser"" as npwg on npwg.""UserGroupId""=np1.""PermittedUserGroupId""  and npwg.""IsDeleted""=false and npwg.""CompanyId""='{_repo.UserContext.CompanyId}' 

                left join public.""User"" as npwgu on npwgu.""Id""=npwg.""UserId""  and npwgu.""Id""='{UserId}'	 and npwgu.""IsDeleted""=false and npwgu.""CompanyId""='{_repo.UserContext.CompanyId}'

                where np1.""IsDeleted""=false	  )
                as np on np.""NoteId""=f.""Id"" and np.""UserId""='{UserId}'			
                left join public.""NtsService"" as s on s.""Id""=f.""ParentServiceId"" and s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join public.""NtsService"" as cs on cs.""Id""=udf.""ChangeRequestServiceId"" and cs.""IsDeleted""=false and cs.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join public.""File"" as gen on gen.""Id""=udf.""DocumentId"" and gen.""IsDeleted""=false and gen.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join public.""Template"" as st on st.""Id""=tm.""WorkFlowTemplateId"" and st.""IsDeleted""=false and st.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join cms.""N_GENERAL_FOLDER_WORKSPACE"" as ws on ws.""Id""=udf.""WorkspaceId"" and ws.""IsDeleted""=false and ws.""CompanyId""='{_repo.UserContext.CompanyId}'
				left join public.""NtsNote"" as wn on wn.""Id""=ws.""NtsNoteId"" and wn.""IsDeleted""=false

                UNION

                Select f.""Id"" as Id,f.""NoteSubject"" as Name,tm.""Code"" as FolderCode,null as WorkspaceId
                ,case when wn.""NoteSubject""='My Workspace' then 'true' else 'false' end as IsSelfWorkspace,false as IsWorkspaceAdmin,false as IsAssignedWorkspace,'Document' as FolderType
                ,np.""PermissionType"" as PermissionType,np.""Access"" as Access
                ,'' as InheritedFrom
                ,np.""AppliesTo"" as AppliesTo
                ,u.""Id"" as OwnerUserId,true as IsOwner,pn.""Id"" as ParentId,pn.""NoteSubject"" as ParentName
                ,f.""IsArchived"" as IsArchived
                ,0 as DocCount
                ,f.""DisablePermissionInheritance"" as DisablePermissionInheritance
                ,f.""NoteSubject"" as DocumentName,f.""NoteDescription"" as Description,f.""CreatedDate"" as CreatedDate
                ,f.""VersionNo"" as NoteVersionNo
                ,f.""LockStatus"" as LockStatus
                ,f.""LastUpdatedDate"" as LastUpdatedDate
                ,f.""StartDate"" as Start,coalesce(f.""NoteSubject"", f.""NoteDescription"", '') as Title
                ,tm.""Name"" as DocumentType,tm.""Code"" as TemplateMasterCode,tm.""Id"" as TemplateMasterId
                ,tm.""Id"" as DocumentTypeId
                , udf.""DocumentId"" as DocumentId
                ,gen.""FileName"" as FileName
                ,lov.""Name"" as StatusName,lov.""Code"" as NoteStatus
                ,null as tag
                , dast.""Name"" as DocumentApprovalStatusType
                , udf.""documentApprovalStatus"" as DocumentsApprovalStatus,udf.""stageStatus"" as StageStatus,
                udf.""discipline"" as Discipline,coalesce(udf.""code"",udf.""OutgoingIssueCodes"",udf.""issueCodes"") as IssueCode
                ,udf.""vendorList"" as Vendor,udf.""projectFolder"" as ProjectFolder
                ,u.""Name"" as OwnerUser, u.""Name"" as CreatedByUser, u.""Name"" as UpdatedByUser
                ,s.""Id"" as ServiceId,s.""ServiceNo"" as WorkflowNo,s.""ServiceSubject"" as WorkflowName
                ,null as WorkflowStatus, null as WorkflowTemplateId,st.""Code"" as WorkflowCode
                ,'N' as NtsId,true as IsAllDay, gen.""ContentLength"" as FileSize,0 as PhotoId
                ,'' as TagIds,'' as TagNames
                ,null as EnableDocumentChangeRequest
                , null as EnableLock
                ,'' as TagCategoryIds
                ,'' as TagCategoryNames 
                , f.""SequenceOrder"" as SequenceNo
                ,cs.""Id"" as ChangeRequestServiceId
                ,f.""NoteNo"" as DocumentNo,null as WorkflowServiceId,null as WorkflowServiceStatus,null as WorkflowServiceStatusName
				
                from 
                public.""User"" as u
                join public.""NtsNote"" as f on f.""OwnerUserId""<>u.""Id"" and u.""Id""='{UserId}' and f.""Id""='{parentId}' and f.""IsDeleted""=false and f.""CompanyId""='{_repo.UserContext.CompanyId}' and  (f.""IsArchived"" <> true or f.""IsArchived"" is null)
                and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}' 
                join public.""User"" as ou on f.""OwnerUserId""=ou.""Id"" and ou.""IsDeleted""=false and ou.""CompanyId""='{_repo.UserContext.CompanyId}'
                join public.""LOV"" as lov on lov.""Id""=f.""NoteStatusId"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_repo.UserContext.CompanyId}'	

                join public.""Template"" as tm on tm.""Id""=f.""TemplateId"" and tm.""IsDeleted""=false and tm.""CompanyId""='{_repo.UserContext.CompanyId}' --and tm.""Code""='GENERAL_FOLDER'
                join public.""TemplateCategory"" as tc on tc.""Id""=tm.""TemplateCategoryId""  and tc.""IsDeleted""=false and tc.""CompanyId""='{_repo.UserContext.CompanyId}' and tc.""Code""='GENERAL_DOCUMENT'
                left join (" + udfs + $@") as udf on udf.""NtsNoteId""=f.""Id""  	

                join public.""NtsNote"" as pn on pn.""Id""=f.""ParentNoteId"" and pn.""IsDeleted""=false and pn.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join public.""LOV"" as dast on dast.""Id""=udf.""documentApprovalStatusType"" and dast.""IsDeleted""=false and dast.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join(select np1.*,case when npu is null then npwgu.""Id"" else npu.""Id"" end as ""UserId""
                from  public.""DocumentPermission"" as np1
                left join public.""User"" as npu on npu.""Id""=np1.""PermittedUserId"" and npu.""Id""='{UserId}' and npu.""IsDeleted""=false and npu.""CompanyId""='{_repo.UserContext.CompanyId}' 


                left join public.""UserGroupUser"" as npwg on npwg.""UserGroupId""=np1.""PermittedUserGroupId""  and npwg.""IsDeleted""=false and npwg.""CompanyId""='{_repo.UserContext.CompanyId}' 

                left join public.""User"" as npwgu on npwgu.""Id""=npwg.""UserId""  and npwgu.""Id""='{UserId}'	 and npwgu.""IsDeleted""=false and npwgu.""CompanyId""='{_repo.UserContext.CompanyId}'

                where np1.""IsDeleted""=false	  )
                as np on np.""NoteId""=f.""Id"" and np.""UserId""='{UserId}'			
                left join public.""NtsService"" as s on s.""Id""=f.""ParentServiceId"" and s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join public.""NtsService"" as cs on cs.""Id""=udf.""ChangeRequestServiceId"" and cs.""IsDeleted""=false and cs.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join public.""File"" as gen on gen.""Id""=udf.""DocumentId"" and gen.""IsDeleted""=false and gen.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join public.""Template"" as st on st.""Id""=tm.""WorkFlowTemplateId"" and st.""IsDeleted""=false and st.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join cms.""N_GENERAL_FOLDER_WORKSPACE"" as ws on ws.""Id""=udf.""WorkspaceId"" and ws.""IsDeleted""=false and ws.""CompanyId""='{_repo.UserContext.CompanyId}'
				left join public.""NtsNote"" as wn on wn.""Id""=ws.""NtsNoteId"" and wn.""IsDeleted""=false
                ";
            
            var list = await _queryRepo1.ExecuteQueryList(query, null);
            list = list.OrderByDescending(x => x.Access).DistinctBy(x => x.Id).ToList();            
            //list = list.DistinctBy(x => x.Id).ToList();

            ManageParentHierarchyNew(list);
            return list.Where(x => x.PermissionType == DmsPermissionTypeEnum.Allow).ToList();
        }
        public async Task<List<FolderViewModel>> GetFirstLevelWorkspacesByUser(string UserId)
        { 
           
            var query= string.Concat($@"select distinct  n.""Id"" as Id,n.""NoteSubject"" as Name
                ,tm.""Code"" as FolderCode,n.""Id"" as WorkspaceId
                ,'true' as IsSelfWorkspace,false as IsWorkspaceAdmin,true as IsAssignedWorkspace,'Workspace' as FolderType
                ,'0'::int as PermissionType,'2'::int as Access
                ,'' as InheritedFrom
                ,'2'::int as AppliesTo
                ,u.""Id"" as OwnerUserId,true as IsOwner,n.""IsArchived"" as IsArchived,Tags.""Count"" as DocCount
                ,n.""DisablePermissionInheritance"" as DisablePermissionInheritance, 0 as SequenceNo
                ,n.""CreatedDate"" as CreatedDate,n.""LastUpdatedDate"" as LastUpdatedDate,n.""NoteNo"" as DocumentNo,u.""Name"" as CreatedByUser

 
                from 
                public.""User"" as u
                join public.""NtsNote"" as n on n.""OwnerUserId""=u.""Id"" and  (n.""IsArchived"" <> true or n.""IsArchived"" is null) and n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}'   and u.""Id""='{UserId}'
                join cms.""N_GENERAL_FOLDER_WORKSPACE"" as ws on ws.""NtsNoteId""=n.""Id"" and ws.""IsDeleted""=false and ws.""CompanyId""='{_repo.UserContext.CompanyId}' 
                join public.""LOV"" as lv on lv.""Id""=ws.""TypeId"" and lv.""IsDeleted""=false and  lv.""Code"" in ('MY_WORKSPACE')
                join public.""Template"" as tm on tm.""Id""=n.""TemplateId"" and tm.""IsDeleted""=false and tm.""CompanyId""='{_repo.UserContext.CompanyId}' 
                left join public.""TemplateCategory"" as tc on tc.""Id""=tm.""TemplateCategoryId""  and tc.""IsDeleted""=false and tc.""Code""='GENERAL_FOLDER' and tc.""CompanyId""='{_repo.UserContext.CompanyId}' 
                --left join public.""NtsNote"" as pn on pn.""Id""=n.""ParentNoteId"" and pn.""IsDeleted""=false and pn.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join (select count(*) as ""Count"",""NtsId"" from public.""NtsTag"" as tag
                join public.""NtsNote"" as doc on doc.""Id""=tag.""TagSourceReferenceId"" and (doc.""IsArchived"" <> true or doc.""IsArchived"" is null) and doc.""IsDeleted""=false and doc.""CompanyId""='{_repo.UserContext.CompanyId}' 
                join(select distinct np.""NoteId"",case when npu is null then npwgu.""Id"" else npu.""Id"" end as ""UserId""
                from  public.""DocumentPermission"" as np  
                left join public.""User"" as npu on npu.""Id""=np.""PermittedUserId"" and npu.""Id""='{UserId}' and npu.""IsDeleted""=false and npu.""CompanyId""='{_repo.UserContext.CompanyId}' 			
                left join public.""UserGroupUser"" as npwg on npwg.""UserGroupId""=np.""PermittedUserGroupId""  and npwg.""IsDeleted""=false and npwg.""CompanyId""='{_repo.UserContext.CompanyId}' 
                left join public.""User"" as npwgu on npwgu.""Id""=npwg.""UserId""  and npwgu.""Id""='{UserId}'	 and npwgu.""IsDeleted""=false and npwgu.""CompanyId""='{_repo.UserContext.CompanyId}'
                where np.""IsDeleted""=false	  )
                as np on np.""NoteId""=doc.""Id"" and np.""UserId""='{UserId}' and tag.""IsDeleted""=false and tag.""TagId"" is null and tag.""CompanyId""='{_repo.UserContext.CompanyId}'  group by tag.""NtsId"") as Tags on ""NtsId""=n.""Id""
                where n.""IsDeleted""=false and ws.""IsDeleted""=false  
                and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}' 
            
                UNION
                Select distinct  n.""Id"" as Id,n.""NoteSubject"" as Name
                ,tm.""Code"" as FolderCode,ws.""Id"" as WorkspaceId
                ,'false' as IsSelfWorkspace,false as IsWorkspaceAdmin,true as IsAssignedWorkspace,'Workspace' as FolderType
                , np.""PermissionType""  as PermissionType
                , np.""Access""  as Access
                , np.""InheritedFrom""  as InheritedFrom
                ,np.""AppliesTo""  as AppliesTo
                ,n.""OwnerUserId"" as OwnerUserId,false as IsOwner,n.""IsArchived"" as IsArchived,Tags.""Count"" as DocCount
                ,n.""DisablePermissionInheritance"" as DisablePermissionInheritance, ws.""SequenceOrder"" as SequenceNo				
				,n.""CreatedDate"" as CreatedDate,n.""LastUpdatedDate"" as LastUpdatedDate,n.""NoteNo"" as DocumentNo,u.""Name"" as CreatedByUser
                from 
				 
                public.""NtsNote"" as n 
                join cms.""N_GENERAL_FOLDER_WORKSPACE"" as ws on ws.""NtsNoteId""=n.""Id"" and  (n.""IsArchived"" <> true or n.""IsArchived"" is null) and n.""IsDeleted""=false 
                and n.""NoteSubject"" <>'My Workspace' and n.""ParentNoteId"" is null and ws.""IsDeleted""=false  
                and n.""CompanyId""='{_repo.UserContext.CompanyId}'				   
                join public.""Template"" as tm on tm.""Id""=n.""TemplateId"" and tm.""IsDeleted""=false and tm.""Code""='WORKSPACE_GENERAL' and tm.""CompanyId""='{_repo.UserContext.CompanyId}'
                join public.""TemplateCategory"" as tc on tc.""Id""=tm.""TemplateCategoryId""  and tc.""IsDeleted""=false and tc.""Code""='GENERAL_FOLDER' and tc.""CompanyId""='{_repo.UserContext.CompanyId}'
				left join public.""User"" as u on u.""Id""=n.""OwnerUserId""
                left join(select np.*,case when npu is null then npwgu.""Id"" else npu.""Id"" end as ""UserId""
                from  public.""DocumentPermission"" as np  
                left join public.""User"" as npu on npu.""Id""=np.""PermittedUserId"" and npu.""Id""='{UserId}' and npu.""IsDeleted""=false and npu.""CompanyId""='{_repo.UserContext.CompanyId}' 
			
                left join public.""UserGroupUser"" as npwg on npwg.""UserGroupId""=np.""PermittedUserGroupId""  and npwg.""IsDeleted""=false and npwg.""CompanyId""='{_repo.UserContext.CompanyId}' 
                left join public.""User"" as npwgu on npwgu.""Id""=npwg.""UserId""  and npwgu.""Id""='{UserId}'	 and npwgu.""IsDeleted""=false and npwgu.""CompanyId""='{_repo.UserContext.CompanyId}'
                where np.""IsDeleted""=false	  )
                as np on np.""NoteId""=n.""Id"" and np.""UserId""='{UserId}'
                left join (select count(*) as ""Count"",""NtsId"" from public.""NtsTag"" as tag
                join public.""NtsNote"" as doc on doc.""Id""=tag.""TagSourceReferenceId"" and (doc.""IsArchived"" <> true or doc.""IsArchived"" is null) and doc.""IsDeleted""=false and doc.""CompanyId""='{_repo.UserContext.CompanyId}' 
                join(select distinct np.""NoteId"",case when npu is null then npwgu.""Id"" else npu.""Id"" end as ""UserId""
                from  public.""DocumentPermission"" as np  
                left join public.""User"" as npu on npu.""Id""=np.""PermittedUserId"" and npu.""Id""='{UserId}' and npu.""IsDeleted""=false and npu.""CompanyId""='{_repo.UserContext.CompanyId}' 			
                left join public.""UserGroupUser"" as npwg on npwg.""UserGroupId""=np.""PermittedUserGroupId""  and npwg.""IsDeleted""=false and npwg.""CompanyId""='{_repo.UserContext.CompanyId}' 
                left join public.""User"" as npwgu on npwgu.""Id""=npwg.""UserId""  and npwgu.""Id""='{UserId}'	 and npwgu.""IsDeleted""=false and npwgu.""CompanyId""='{_repo.UserContext.CompanyId}'
                where np.""IsDeleted""=false	  )
                as np on np.""NoteId""=doc.""Id"" and np.""UserId""='{UserId}' and tag.""IsDeleted""=false and tag.""TagId"" is null and tag.""CompanyId""='{_repo.UserContext.CompanyId}'  group by tag.""NtsId"") as Tags on ""NtsId""=n.""Id""
                left join public.""LOV"" as lv on lv.""Id""=ws.""TypeId"" and lv.""IsDeleted""=false  and lv.""CompanyId""='{_repo.UserContext.CompanyId}' 
                where  lv.""Code"" <>'MY_WORKSPACE' or lv.""Code"" is null and n.""IsDeleted""=false and ws.""IsDeleted""=false  and n.""CompanyId""='{_repo.UserContext.CompanyId}' and n.""PortalId"" = '{_repo.UserContext.PortalId}' ");
            var list = await _queryRepo1.ExecuteQueryList(query, null);
            list = list.OrderByDescending(x => x.Access).DistinctBy(x => x.Id).ToList();


            ManageParentHierarchyNew(list);
            return list.Where(x => x.PermissionType == DmsPermissionTypeEnum.Allow).ToList();
        }
        public async Task<List<FolderViewModel>> GetAllChildWorkspaceAndFolder(string UserId, string parentId)
        {
            var udfs = await _noteBusiness.GetUdfQuery(null, "GENERAL_DOCUMENT", null, null, "attachment,DocumentId", "fileAttachment,fileAttachmentId", "documentApprovalStatusType,documentApprovalStatusType", "code,code", "issueCodes,issueCodes"
              , "documentApprovalStatus,documentApprovalStatus", "stageStatus,stageStatus", "discipline,discipline", "OutgoingIssueCodes,OutgoingIssueCodes", "vendorList,vendorList", "projectFolder,projectFolder", "ChangeRequestId,ChangeRequestServiceId", "workspaceId,WorkspaceId");
            var query = $@"
                Select distinct  n.""Id"" as Id,n.""NoteSubject"" as Name
                ,tm.""Code"" as FolderCode,ws.""Id"" as WorkspaceId
                ,'false' as IsSelfWorkspace,false as IsWorkspaceAdmin,true as IsAssignedWorkspace,'Workspace' as FolderType
                , np.""PermissionType""  as PermissionType
                , np.""Access""  as Access
                , np.""InheritedFrom""  as InheritedFrom
               ,np.""AppliesTo""  as AppliesTo
                ,n.""OwnerUserId"" as OwnerUserId,false as IsOwner,pn.""Id"" as ParentId,pn.""NoteSubject"" as ParentName,n.""IsArchived"" as IsArchived,tags.""Count"" as DocCount
                ,n.""DisablePermissionInheritance"" as DisablePermissionInheritance
				,null as DocumentName
                ,null as Description
                ,n.""CreatedDate"" as CreatedDate
                ,n.""VersionNo"" as NoteVersionNo
                ,n.""LockStatus"" as LockStatus
                ,n.""LastUpdatedDate"" as LastUpdatedDate
                ,n.""StartDate"" as Start
                ,coalesce(n.""NoteSubject"", n.""NoteDescription"", '') as Title
                ,tm.""Name"" as DocumentType
                ,tm.""Code"" as TemplateMasterCode
                ,tm.""Id"" as TemplateMasterId
                ,tm.""Id"" as DocumentTypeId
                ,null as DocumentId
                ,null as FileName
                ,null as StatusName
                ,null as NoteStatus
                ,null as tag
                ,null as DocumentApprovalStatusType
                ,null as DocumentsApprovalStatus
                ,null as StageStatus
                ,null as Discipline
                ,null as IssueCode
                ,null as Vendor
                ,null as ProjectFolder
                ,null as OwnerUser
                ,u.""Name"" as CreatedByUser
                ,null as UpdatedByUser
                ,null as ServiceId
                ,null as WorkflowNo
                ,null as WorkflowName
                ,null as WorkflowStatus
                ,null as WorkflowTemplateId
                ,null as WorkflowCode
                ,'N' as NtsId
                ,true as IsAllDay
                , 0 as FileSize
                ,0 as PhotoId
                ,null as TagIds
                ,null as TagNames
                ,null as EnableDocumentChangeRequest
                , null as EnableLock
                ,null as TagCategoryIds
                ,null as TagCategoryNames 
                ,ws.""SequenceOrder"" as SequenceNo
                ,null as ChangeRequestServiceId
                ,n.""NoteNo"" as DocumentNo
                ,null as WorkflowServiceId
                ,null as WorkflowServiceStatus
                ,null as WorkflowServiceStatusName
				
				from
				 public.""NtsNote"" as n 
				 join cms.""N_GENERAL_FOLDER_WORKSPACE"" as ws on ws.""NtsNoteId""=n.""Id"" and n.""ParentNoteId""='{parentId}' and  (n.""IsArchived"" <> true or n.""IsArchived"" is null) and
                n.""IsDeleted""=false and n.""NoteSubject"" <>'My Workspace' and ws.""IsDeleted""=false  and n.""CompanyId""='{_repo.UserContext.CompanyId}'
				   
				 join public.""Template"" as tm on tm.""Id""=n.""TemplateId"" and tm.""IsDeleted""=false and tm.""Code""='WORKSPACE_GENERAL' and tm.""CompanyId""='{_repo.UserContext.CompanyId}'
				 join public.""TemplateCategory"" as tc on tc.""Id""=tm.""TemplateCategoryId""  and tc.""IsDeleted""=false and tc.""Code""='GENERAL_FOLDER' and tc.""CompanyId""='{_repo.UserContext.CompanyId}'
				 left join public.""User"" as u on u.""Id""=n.""OwnerUserId""
				 left join(select np.*,case when npu is null then npwgu.""Id"" else npu.""Id"" end as ""UserId"" 
                from  public.""DocumentPermission"" as np  
                left join public.""User"" as npu on npu.""Id""=np.""PermittedUserId"" and npu.""Id""='{UserId}' and
                npu.""IsDeleted""=false and npu.""CompanyId""='{_repo.UserContext.CompanyId}' 			
				left join public.""UserGroupUser"" as npwg on npwg.""UserGroupId""=np.""PermittedUserGroupId""  and npwg.""IsDeleted""=false and npwg.""CompanyId""='{_repo.UserContext.CompanyId}' 
				left join public.""User"" as npwgu on npwgu.""Id""=npwg.""UserId""  and npwgu.""Id""='{UserId}'	 and npwgu.""IsDeleted""=false and npwgu.""CompanyId""='{_repo.UserContext.CompanyId}'
				where np.""IsDeleted""=false)
				as np on np.""NoteId""=n.""Id"" and np.""UserId""='{UserId}'
                left join public.""NtsNote"" as pn on pn.""Id""=n.""ParentNoteId"" and pn.""IsDeleted""=false and pn.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join (select count(*) as ""Count"",""NtsId"" from public.""NtsTag"" as tag
                join public.""NtsNote"" as doc on doc.""Id""=tag.""TagSourceReferenceId"" and (doc.""IsArchived"" <> true or doc.""IsArchived"" is null) and doc.""IsDeleted""=false and doc.""CompanyId""='{_repo.UserContext.CompanyId}' 
                join(select distinct np.""NoteId"",case when npu is null then npwgu.""Id"" else npu.""Id"" end as ""UserId""
                from  public.""DocumentPermission"" as np  
                left join public.""User"" as npu on npu.""Id""=np.""PermittedUserId"" and npu.""Id""='{UserId}' and npu.""IsDeleted""=false and npu.""CompanyId""='{_repo.UserContext.CompanyId}' 			
                left join public.""UserGroupUser"" as npwg on npwg.""UserGroupId""=np.""PermittedUserGroupId""  and npwg.""IsDeleted""=false and npwg.""CompanyId""='{_repo.UserContext.CompanyId}' 
                left join public.""User"" as npwgu on npwgu.""Id""=npwg.""UserId""  and npwgu.""Id""='{UserId}'	 and npwgu.""IsDeleted""=false and npwgu.""CompanyId""='{_repo.UserContext.CompanyId}'
                where np.""IsDeleted""=false	  )
                as np on np.""NoteId""=doc.""Id"" and np.""UserId""='{UserId}' and tag.""IsDeleted""=false and tag.""TagId"" is null and tag.""CompanyId""='{_repo.UserContext.CompanyId}'  group by tag.""NtsId"") as Tags on ""NtsId""=n.""Id""
				left join public.""LOV"" as lv on lv.""Id""=ws.""TypeId"" and lv.""IsDeleted""=false  and lv.""CompanyId""='{_repo.UserContext.CompanyId}' 
				where  lv.""Code"" <>'MY_WORKSPACE' or lv.""Code"" is null and n.""IsDeleted""=false and ws.""IsDeleted""=false  and n.""CompanyId""='{_repo.UserContext.CompanyId}' 
                
                UNION

                Select distinct f.""Id"" as Id,f.""NoteSubject"" as Name                
                ,tm.""Code"" as FolderCode,ws.""Id"" as WorkspaceId
                ,case when wn.""NoteSubject""='My Workspace' then 'true' else 'false' end as IsSelfWorkspace
                ,false as IsWorkspaceAdmin,false as IsAssignedWorkspace,'Folder' as FolderType
                , np.""PermissionType""  as PermissionType
                , np.""Access""  as Access
                , np.""InheritedFrom""  as InheritedFrom
                , np.""AppliesTo""  as AppliesTo
                ,null as OwnerUserId,false as IsOwner,pn.""Id"" as ParentId,pn.""NoteSubject"" as ParentName,f.""IsArchived"" as IsArchived
				,tags.""Count"" as DocCount
                ,f.""DisablePermissionInheritance"" as DisablePermissionInheritance
                ,null as DocumentName
                ,null as Description
                ,f.""CreatedDate"" as CreatedDate
                ,f.""VersionNo"" as NoteVersionNo
                ,f.""LockStatus"" as LockStatus
                ,f.""LastUpdatedDate"" as LastUpdatedDate
                ,f.""StartDate"" as Start
                ,coalesce(f.""NoteSubject"", f.""NoteDescription"", '') as Title
                ,tm.""Name"" as DocumentType
                ,tm.""Code"" as TemplateMasterCode
                ,tm.""Id"" as TemplateMasterId
                ,tm.""Id"" as DocumentTypeId
                ,null as DocumentId
                ,null as FileName
                ,null as StatusName
                ,null as NoteStatus
                ,null as tag
                ,null as DocumentApprovalStatusType
                ,null as DocumentsApprovalStatus
                ,null as StageStatus
                ,null as Discipline
                ,null as IssueCode
                ,null as Vendor
                ,null as ProjectFolder
                ,null as OwnerUser
                ,u.""Name"" as CreatedByUser
                ,null as UpdatedByUser
                ,null as ServiceId
                ,null as WorkflowNo
                ,null as WorkflowName
                ,null as WorkflowStatus
                ,null as WorkflowTemplateId
                ,null as WorkflowCode
                ,'N' as NtsId
                ,true as IsAllDay
                , 0 as FileSize
                ,0 as PhotoId
                ,null as TagIds
                ,null as TagNames
                ,null as EnableDocumentChangeRequest
                , null as EnableLock
                ,null as TagCategoryIds
                ,null as TagCategoryNames 
                ,f.""SequenceOrder"" as SequenceNo
                ,null as ChangeRequestServiceId
                ,f.""NoteNo"" as DocumentNo
                ,null as WorkflowServiceId
                ,null as WorkflowServiceStatus
                ,null as WorkflowServiceStatusName
								
                from 

				public.""NtsNote"" as f  
				join cms.""N_GENERAL_FOLDER_GENERALFOLDER"" as ws1 on ws1.""NtsNoteId""=f.""Id"" and f.""ParentNoteId""='{parentId}' and  (f.""IsArchived"" <> true or f.""IsArchived"" is null)	and f.""IsDeleted""=false and ws1.""IsDeleted""=false	and f.""CompanyId""='{_repo.UserContext.CompanyId}'				
				join public.""Template"" as tm on tm.""Id""=f.""TemplateId"" and tm.""IsDeleted""=false and tm.""Code""='GENERAL_FOLDER' and tm.""CompanyId""='{_repo.UserContext.CompanyId}'
				join public.""TemplateCategory"" as tc on tc.""Id""=tm.""TemplateCategoryId""  and tc.""IsDeleted""=false and tc.""Code""='GENERAL_FOLDER' and tc.""CompanyId""='{_repo.UserContext.CompanyId}'
				join public.""NtsNote"" as pn on pn.""Id""=f.""ParentNoteId"" and pn.""IsDeleted""=false and pn.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join cms.""N_GENERAL_FOLDER_WORKSPACE"" as ws on ws.""Id""=ws1.""WorkspaceId"" and ws.""IsDeleted""=false and ws1.""CompanyId""='{_repo.UserContext.CompanyId}'
				left join public.""NtsNote"" as wn on wn.""Id""=ws.""NtsNoteId"" and wn.""IsDeleted""=false
                left join public.""User"" as u on u.""Id""=f.""OwnerUserId""

	            left join(select np.*,case when npu is null then npwgu.""Id"" else npu.""Id"" end as ""UserId"" 
                from  public.""DocumentPermission"" as np 
                left join public.""User"" as npu on npu.""Id""=np.""PermittedUserId"" and npu.""Id""='{UserId}' and npu.""IsDeleted""=false and npu.""CompanyId""='{_repo.UserContext.CompanyId}' 
			
				left join public.""UserGroupUser"" as npwg on npwg.""UserGroupId""=np.""PermittedUserGroupId""  and npwg.""IsDeleted""=false and npwg.""CompanyId""='{_repo.UserContext.CompanyId}' 
				left join public.""User"" as npwgu on npwgu.""Id""=npwg.""UserId""  and npwgu.""Id""='{UserId}'	 and npwgu.""IsDeleted""=false and npwgu.""CompanyId""='{_repo.UserContext.CompanyId}'
					where np.""IsDeleted""=false)
				as np on np.""NoteId""=f.""Id"" and np.""UserId""='{UserId}'
                 left join (select count(*) as ""Count"",""NtsId"" from public.""NtsTag"" as tag
                join public.""NtsNote"" as doc on doc.""Id""=tag.""TagSourceReferenceId"" and (doc.""IsArchived"" <> true or doc.""IsArchived"" is null) and doc.""IsDeleted""=false and doc.""CompanyId""='{_repo.UserContext.CompanyId}' 
                join(select distinct np.""NoteId"",case when npu is null then npwgu.""Id"" else npu.""Id"" end as ""UserId""
                from  public.""DocumentPermission"" as np  
                left join public.""User"" as npu on npu.""Id""=np.""PermittedUserId"" and npu.""Id""='{UserId}' and npu.""IsDeleted""=false and npu.""CompanyId""='{_repo.UserContext.CompanyId}' 			
                left join public.""UserGroupUser"" as npwg on npwg.""UserGroupId""=np.""PermittedUserGroupId""  and npwg.""IsDeleted""=false and npwg.""CompanyId""='{_repo.UserContext.CompanyId}' 
                left join public.""User"" as npwgu on npwgu.""Id""=npwg.""UserId""  and npwgu.""Id""='{UserId}'	 and npwgu.""IsDeleted""=false and npwgu.""CompanyId""='{_repo.UserContext.CompanyId}'
                where np.""IsDeleted""=false	  )
                as np on np.""NoteId""=doc.""Id"" and np.""UserId""='{UserId}' and tag.""IsDeleted""=false and tag.""TagId"" is null and tag.""CompanyId""='{_repo.UserContext.CompanyId}'  group by tag.""NtsId"") as Tags on ""NtsId""=f.""Id""
                
                
                ";

            var list = await _queryRepo1.ExecuteQueryList(query, null);
            list = list.OrderByDescending(x => x.Access).DistinctBy(x => x.Id).ToList();
            ManageParentHierarchyNew(list);
            return list.Where(x => x.PermissionType == DmsPermissionTypeEnum.Allow).ToList();
        }
        public async Task<List<FolderViewModel>> GetAllChildWorkspaceFolderAndDocument(string UserId, string parentId)
        {
            var udfs = await _noteBusiness.GetUdfQuery(null, "GENERAL_DOCUMENT", null, null, "attachment,DocumentId", "fileAttachment,fileAttachmentId", "documentApprovalStatusType,documentApprovalStatusType", "code,code", "issueCodes,issueCodes"
              , "documentApprovalStatus,documentApprovalStatus", "stageStatus,stageStatus", "discipline,discipline", "OutgoingIssueCodes,OutgoingIssueCodes", "vendorList,vendorList", "projectFolder,projectFolder", "ChangeRequestId,ChangeRequestServiceId", "workspaceId,WorkspaceId");
            var query = $@"
                Select distinct  n.""Id"" as Id,n.""NoteSubject"" as Name
                ,tm.""Code"" as FolderCode,ws.""Id"" as WorkspaceId
                ,'false' as IsSelfWorkspace,false as IsWorkspaceAdmin,true as IsAssignedWorkspace,'Workspace' as FolderType
                , np.""PermissionType""  as PermissionType
                , np.""Access""  as Access
                , np.""InheritedFrom""  as InheritedFrom
               ,np.""AppliesTo""  as AppliesTo
                ,n.""OwnerUserId"" as OwnerUserId,false as IsOwner,pn.""Id"" as ParentId,pn.""NoteSubject"" as ParentName,n.""IsArchived"" as IsArchived,tags.""Count"" as DocCount
                ,n.""DisablePermissionInheritance"" as DisablePermissionInheritance
				,null as DocumentName
                ,null as Description
                ,n.""CreatedDate"" as CreatedDate
                ,n.""VersionNo"" as NoteVersionNo
                ,n.""LockStatus"" as LockStatus
                ,n.""LastUpdatedDate"" as LastUpdatedDate
                ,n.""StartDate"" as Start
                ,coalesce(n.""NoteSubject"", n.""NoteDescription"", '') as Title
                ,tm.""Name"" as DocumentType
                ,tm.""Code"" as TemplateMasterCode
                ,tm.""Id"" as TemplateMasterId
                ,tm.""Id"" as DocumentTypeId
                ,null as DocumentId
                ,null as FileName
                ,null as StatusName
                ,null as NoteStatus
                ,null as tag
                ,null as DocumentApprovalStatusType
                ,null as DocumentsApprovalStatus
                ,null as StageStatus
                ,null as Discipline
                ,null as IssueCode
                ,null as Vendor
                ,null as ProjectFolder
                ,null as OwnerUser
                ,u.""Name"" as CreatedByUser
                ,null as UpdatedByUser
                ,null as ServiceId
                ,null as WorkflowNo
                ,null as WorkflowName
                ,null as WorkflowStatus
                ,null as WorkflowTemplateId
                ,null as WorkflowCode
                ,'N' as NtsId
                ,true as IsAllDay
                , 0 as FileSize
                ,0 as PhotoId
                ,null as TagIds
                ,null as TagNames
                ,null as EnableDocumentChangeRequest
                , null as EnableLock
                ,null as TagCategoryIds
                ,null as TagCategoryNames 
                ,ws.""SequenceOrder"" as SequenceNo
                ,null as ChangeRequestServiceId
                ,n.""NoteNo"" as DocumentNo
                ,null as WorkflowServiceId
                ,null as WorkflowServiceStatus
                ,null as WorkflowServiceStatusName
				
				from
				 public.""NtsNote"" as n 
				 join cms.""N_GENERAL_FOLDER_WORKSPACE"" as ws on ws.""NtsNoteId""=n.""Id"" and n.""ParentNoteId""='{parentId}' and  (n.""IsArchived"" <> true or n.""IsArchived"" is null) and
                n.""IsDeleted""=false and n.""NoteSubject"" <>'My Workspace' and ws.""IsDeleted""=false  and n.""CompanyId""='{_repo.UserContext.CompanyId}'
				   
				 join public.""Template"" as tm on tm.""Id""=n.""TemplateId"" and tm.""IsDeleted""=false and tm.""Code""='WORKSPACE_GENERAL' and tm.""CompanyId""='{_repo.UserContext.CompanyId}'
				 join public.""TemplateCategory"" as tc on tc.""Id""=tm.""TemplateCategoryId""  and tc.""IsDeleted""=false and tc.""Code""='GENERAL_FOLDER' and tc.""CompanyId""='{_repo.UserContext.CompanyId}'
				 left join public.""User"" as u on u.""Id""=n.""OwnerUserId""
				 left join(select np.*,case when npu is null then npwgu.""Id"" else npu.""Id"" end as ""UserId"" 
                from  public.""DocumentPermission"" as np  
                left join public.""User"" as npu on npu.""Id""=np.""PermittedUserId"" and npu.""Id""='{UserId}' and
                npu.""IsDeleted""=false and npu.""CompanyId""='{_repo.UserContext.CompanyId}' 			
				left join public.""UserGroupUser"" as npwg on npwg.""UserGroupId""=np.""PermittedUserGroupId""  and npwg.""IsDeleted""=false and npwg.""CompanyId""='{_repo.UserContext.CompanyId}' 
				left join public.""User"" as npwgu on npwgu.""Id""=npwg.""UserId""  and npwgu.""Id""='{UserId}'	 and npwgu.""IsDeleted""=false and npwgu.""CompanyId""='{_repo.UserContext.CompanyId}'
				where np.""IsDeleted""=false)
				as np on np.""NoteId""=n.""Id"" and np.""UserId""='{UserId}'
                left join public.""NtsNote"" as pn on pn.""Id""=n.""ParentNoteId"" and pn.""IsDeleted""=false and pn.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join (select count(*) as ""Count"",""NtsId"" from public.""NtsTag"" as tag
                join public.""NtsNote"" as doc on doc.""Id""=tag.""TagSourceReferenceId"" and (doc.""IsArchived"" <> true or doc.""IsArchived"" is null) and doc.""IsDeleted""=false and doc.""CompanyId""='{_repo.UserContext.CompanyId}' 
                join(select distinct np.""NoteId"",case when npu is null then npwgu.""Id"" else npu.""Id"" end as ""UserId""
                from  public.""DocumentPermission"" as np  
                left join public.""User"" as npu on npu.""Id""=np.""PermittedUserId"" and npu.""Id""='{UserId}' and npu.""IsDeleted""=false and npu.""CompanyId""='{_repo.UserContext.CompanyId}' 			
                left join public.""UserGroupUser"" as npwg on npwg.""UserGroupId""=np.""PermittedUserGroupId""  and npwg.""IsDeleted""=false and npwg.""CompanyId""='{_repo.UserContext.CompanyId}' 
                left join public.""User"" as npwgu on npwgu.""Id""=npwg.""UserId""  and npwgu.""Id""='{UserId}'	 and npwgu.""IsDeleted""=false and npwgu.""CompanyId""='{_repo.UserContext.CompanyId}'
                where np.""IsDeleted""=false	  )
                as np on np.""NoteId""=doc.""Id"" and np.""UserId""='{UserId}' and tag.""IsDeleted""=false and tag.""TagId"" is null and tag.""CompanyId""='{_repo.UserContext.CompanyId}'  group by tag.""NtsId"") as Tags on ""NtsId""=n.""Id""
				left join public.""LOV"" as lv on lv.""Id""=ws.""TypeId"" and lv.""IsDeleted""=false  and lv.""CompanyId""='{_repo.UserContext.CompanyId}' 
				where  lv.""Code"" <>'MY_WORKSPACE' or lv.""Code"" is null and n.""IsDeleted""=false and ws.""IsDeleted""=false  and n.""CompanyId""='{_repo.UserContext.CompanyId}' 
               
                UNION                
                
                Select distinct f.""Id"" as Id,f.""NoteSubject"" as Name                
                ,tm.""Code"" as FolderCode,ws.""Id"" as WorkspaceId
                ,case when wn.""NoteSubject""='My Workspace' then 'true' else 'false' end as IsSelfWorkspace,false as IsWorkspaceAdmin,false as IsAssignedWorkspace,'Folder' as FolderType
                , np.""PermissionType""  as PermissionType
                , np.""Access""  as Access
                , np.""InheritedFrom""  as InheritedFrom
                , np.""AppliesTo""  as AppliesTo
                ,null as OwnerUserId,false as IsOwner,pn.""Id"" as ParentId,pn.""NoteSubject"" as ParentName,f.""IsArchived"" as IsArchived
				,tags.""Count"" as DocCount
                ,f.""DisablePermissionInheritance"" as DisablePermissionInheritance
                ,null as DocumentName
                ,null as Description
                ,f.""CreatedDate"" as CreatedDate
                ,f.""VersionNo"" as NoteVersionNo
                ,f.""LockStatus"" as LockStatus
                ,f.""LastUpdatedDate"" as LastUpdatedDate
                ,f.""StartDate"" as Start
                ,coalesce(f.""NoteSubject"", f.""NoteDescription"", '') as Title
                ,tm.""Name"" as DocumentType
                ,tm.""Code"" as TemplateMasterCode
                ,tm.""Id"" as TemplateMasterId
                ,tm.""Id"" as DocumentTypeId
                ,null as DocumentId
                ,null as FileName
                ,null as StatusName
                ,null as NoteStatus
                ,null as tag
                ,null as DocumentApprovalStatusType
                ,null as DocumentsApprovalStatus
                ,null as StageStatus
                ,null as Discipline
                ,null as IssueCode
                ,null as Vendor
                ,null as ProjectFolder
                ,null as OwnerUser
                ,u.""Name"" as CreatedByUser
                ,null as UpdatedByUser
                ,null as ServiceId
                ,null as WorkflowNo
                ,null as WorkflowName
                ,null as WorkflowStatus
                ,null as WorkflowTemplateId
                ,null as WorkflowCode
                ,'N' as NtsId
                ,true as IsAllDay
                , 0 as FileSize
                ,0 as PhotoId
                ,null as TagIds
                ,null as TagNames
                ,null as EnableDocumentChangeRequest
                , null as EnableLock
                ,null as TagCategoryIds
                ,null as TagCategoryNames 
                ,f.""SequenceOrder"" as SequenceNo
                ,null as ChangeRequestServiceId
                ,f.""NoteNo"" as DocumentNo
                ,null as WorkflowServiceId
                ,null as WorkflowServiceStatus
                ,null as WorkflowServiceStatusName
								
                from 

				public.""NtsNote"" as f  
				join cms.""N_GENERAL_FOLDER_GENERALFOLDER"" as ws1 on ws1.""NtsNoteId""=f.""Id"" and f.""ParentNoteId""='{parentId}' and  (f.""IsArchived"" <> true or f.""IsArchived"" is null)	and f.""IsDeleted""=false and ws1.""IsDeleted""=false	and f.""CompanyId""='{_repo.UserContext.CompanyId}'				
				join public.""Template"" as tm on tm.""Id""=f.""TemplateId"" and tm.""IsDeleted""=false and tm.""Code""='GENERAL_FOLDER' and tm.""CompanyId""='{_repo.UserContext.CompanyId}'
				join public.""TemplateCategory"" as tc on tc.""Id""=tm.""TemplateCategoryId""  and tc.""IsDeleted""=false and tc.""Code""='GENERAL_FOLDER' and tc.""CompanyId""='{_repo.UserContext.CompanyId}'
				join public.""NtsNote"" as pn on pn.""Id""=f.""ParentNoteId"" and pn.""IsDeleted""=false and pn.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join cms.""N_GENERAL_FOLDER_WORKSPACE"" as ws on ws.""Id""=ws1.""WorkspaceId"" and ws.""IsDeleted""=false and ws1.""CompanyId""='{_repo.UserContext.CompanyId}'
				left join public.""NtsNote"" as wn on wn.""Id""=ws.""NtsNoteId"" and wn.""IsDeleted""=false
                left join public.""User"" as u on u.""Id""=f.""OwnerUserId""

	            left join(select np.*,case when npu is null then npwgu.""Id"" else npu.""Id"" end as ""UserId"" 
                from  public.""DocumentPermission"" as np 
                left join public.""User"" as npu on npu.""Id""=np.""PermittedUserId"" and npu.""Id""='{UserId}' and npu.""IsDeleted""=false and npu.""CompanyId""='{_repo.UserContext.CompanyId}' 
			
				left join public.""UserGroupUser"" as npwg on npwg.""UserGroupId""=np.""PermittedUserGroupId""  and npwg.""IsDeleted""=false and npwg.""CompanyId""='{_repo.UserContext.CompanyId}' 
				left join public.""User"" as npwgu on npwgu.""Id""=npwg.""UserId""  and npwgu.""Id""='{UserId}'	 and npwgu.""IsDeleted""=false and npwgu.""CompanyId""='{_repo.UserContext.CompanyId}'
					where np.""IsDeleted""=false)
				as np on np.""NoteId""=f.""Id"" and np.""UserId""='{UserId}'
                 left join (select count(*) as ""Count"",""NtsId"" from public.""NtsTag"" as tag
                join public.""NtsNote"" as doc on doc.""Id""=tag.""TagSourceReferenceId"" and (doc.""IsArchived"" <> true or doc.""IsArchived"" is null) and doc.""IsDeleted""=false and doc.""CompanyId""='{_repo.UserContext.CompanyId}' 
                join(select distinct np.""NoteId"",case when npu is null then npwgu.""Id"" else npu.""Id"" end as ""UserId""
                from  public.""DocumentPermission"" as np  
                left join public.""User"" as npu on npu.""Id""=np.""PermittedUserId"" and npu.""Id""='{UserId}' and npu.""IsDeleted""=false and npu.""CompanyId""='{_repo.UserContext.CompanyId}' 			
                left join public.""UserGroupUser"" as npwg on npwg.""UserGroupId""=np.""PermittedUserGroupId""  and npwg.""IsDeleted""=false and npwg.""CompanyId""='{_repo.UserContext.CompanyId}' 
                left join public.""User"" as npwgu on npwgu.""Id""=npwg.""UserId""  and npwgu.""Id""='{UserId}'	 and npwgu.""IsDeleted""=false and npwgu.""CompanyId""='{_repo.UserContext.CompanyId}'
                where np.""IsDeleted""=false	  )
                as np on np.""NoteId""=doc.""Id"" and np.""UserId""='{UserId}' and tag.""IsDeleted""=false and tag.""TagId"" is null and tag.""CompanyId""='{_repo.UserContext.CompanyId}'  group by tag.""NtsId"") as Tags on ""NtsId""=f.""Id""
                
                UNION
            
                select f.""Id"" as Id,f.""NoteSubject"" as Name,tm.""Code"" as FolderCode,null as WorkspaceId
                ,case when wn.""NoteSubject""='My Workspace' then 'true' else 'false' end as IsSelfWorkspace,false as IsWorkspaceAdmin,false as IsAssignedWorkspace,'Document' as FolderType
                ,0 as PermissionType,2 as Access
                ,'' as InheritedFrom
                ,3 as AppliesTo
                ,u.""Id"" as OwnerUserId,true as IsOwner,pn.""Id"" as ParentId,pn.""NoteSubject"" as ParentName
                ,f.""IsArchived"" as IsArchived
                ,0 as DocCount
                ,f.""DisablePermissionInheritance"" as DisablePermissionInheritance
                ,f.""NoteSubject"" as DocumentName,f.""NoteDescription"" as Description,f.""CreatedDate"" as CreatedDate
                ,f.""VersionNo"" as NoteVersionNo
                ,f.""LockStatus"" as LockStatus
                ,f.""LastUpdatedDate"" as LastUpdatedDate
                ,f.""StartDate"" as Start,coalesce(f.""NoteSubject"", f.""NoteDescription"", '') as Title
                ,tm.""Name"" as DocumentType,tm.""Code"" as TemplateMasterCode,tm.""Id"" as TemplateMasterId
                ,tm.""Id"" as DocumentTypeId
                ,coalesce(udf.""DocumentId"",udf.""fileAttachmentId"") as DocumentId
                ,gen.""FileName"" as FileName
                ,lov.""Name"" as StatusName,lov.""Code"" as NoteStatus
                ,null as tag
                , dast.""Name"" as DocumentApprovalStatusType
                , udf.""documentApprovalStatus"" as DocumentsApprovalStatus,udf.""stageStatus"" as StageStatus,
                udf.""discipline"" as Discipline,coalesce(udf.""code"",udf.""OutgoingIssueCodes"",udf.""issueCodes"") as IssueCode
                ,udf.""vendorList"" as Vendor,udf.""projectFolder"" as ProjectFolder
                ,u.""Name"" as OwnerUser, u.""Name"" as CreatedByUser, u.""Name"" as UpdatedByUser
                ,s.""Id"" as ServiceId,s.""ServiceNo"" as WorkflowNo,s.""ServiceSubject"" as WorkflowName
                ,null as WorkflowStatus, st.""Id"" as WorkflowTemplateId,st.""Code"" as WorkflowCode
                ,'N' as NtsId,true as IsAllDay, gen.""ContentLength"" as FileSize,0 as PhotoId
                ,ntg.""TagId"" as TagIds,tg.""NoteSubject"" as TagNames
                ,null as EnableDocumentChangeRequest, null as EnableLock
                ,ntg.""TagCategoryId"" as TagCategoryIds,tgc.""NoteSubject"" as TagCategoryNames 
                , f.""SequenceOrder"" as SequenceNo,
                '' as ChangeRequestServiceId,f.""NoteNo"" as DocumentNo,wfs.""Id"" as WorkflowServiceId,wfss.""Code"" as WorkflowServiceStatus,wfss.""Name"" as WorkflowServiceStatusName

                from 
                public.""User"" as u
                join public.""NtsNote"" as f on f.""OwnerUserId""=u.""Id"" and u.""Id""='{UserId}' and f.""ParentNoteId""='{parentId}' and f.""IsDeleted""=false and f.""CompanyId""='{_repo.UserContext.CompanyId}' and  (f.""IsArchived"" <> true or f.""IsArchived"" is null) and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
                join public.""LOV"" as lov on lov.""Id""=f.""NoteStatusId"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join (" + udfs + $@") as udf on udf.""NtsNoteId""=f.""Id"" 

                join public.""Template"" as tm on tm.""Id""=f.""TemplateId"" and tm.""IsDeleted""=false and tm.""CompanyId""='{_repo.UserContext.CompanyId}'--and tm.""Code""='GENERAL_FOLDER'
                join public.""TemplateCategory"" as tc on tc.""Id""=tm.""TemplateCategoryId""  and tc.""IsDeleted""=false and tc.""CompanyId""='{_repo.UserContext.CompanyId}' and tc.""Code""='GENERAL_DOCUMENT'
                join public.""NtsNote"" as pn on pn.""Id""=f.""ParentNoteId"" and pn.""IsDeleted""=false and pn.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join public.""LOV"" as dast on dast.""Id""=udf.""documentApprovalStatusType"" and dast.""IsDeleted""=false and dast.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join public.""NtsService"" as s on s.""Id""=f.""ParentServiceId"" and s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join public.""NtsService"" as wfs on wfs.""Id""=f.""ReferenceId"" and wfs.""IsDeleted""=false and wfs.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join public.""LOV"" as wfss on wfss.""Id""=wfs.""ServiceStatusId"" and wfss.""IsDeleted""=false and wfss.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join public.""File"" as gen on gen.""Id""=coalesce(udf.""DocumentId"",udf.""fileAttachmentId"") and gen.""IsDeleted""=false and gen.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join public.""Template"" as st on st.""Id""=tm.""WorkFlowTemplateId"" and st.""IsDeleted""=false and st.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join public.""NtsTag"" as ntg on ntg.""NtsId""=f.""Id"" and ntg.""IsDeleted""=false and ntg.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join public.""NtsNote"" as tg on tg.""Id""=ntg.""TagId"" and tg.""IsDeleted""=false and tg.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join public.""NtsNote"" as tgc on tgc.""Id""=tg.""ParentNoteId"" and tgc.""IsDeleted""=false and tgc.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join cms.""N_GENERAL_FOLDER_WORKSPACE"" as ws on ws.""Id""=udf.""WorkspaceId"" and ws.""IsDeleted""=false and ws.""CompanyId""='{_repo.UserContext.CompanyId}'
				left join public.""NtsNote"" as wn on wn.""Id""=ws.""NtsNoteId"" and wn.""IsDeleted""=false
                
                
                
                UNION

                Select f.""Id"" as Id,f.""NoteSubject"" as Name,tm.""Code"" as FolderCode,null as WorkspaceId
                ,case when wn.""NoteSubject""='My Workspace' then 'true' else 'false' end as IsSelfWorkspace,false as IsWorkspaceAdmin,false as IsAssignedWorkspace,'Document' as FolderType
                ,np.""PermissionType"" as PermissionType,np.""Access"" as Access
                ,'' as InheritedFrom
                ,np.""AppliesTo"" as AppliesTo
                ,u.""Id"" as OwnerUserId,false as IsOwner,pn.""Id"" as ParentId,pn.""NoteSubject"" as ParentName
                ,f.""IsArchived"" as IsArchived
                ,0 as DocCount
                ,f.""DisablePermissionInheritance"" as DisablePermissionInheritance
                ,f.""NoteSubject"" as DocumentName,f.""NoteDescription"" as Description,f.""CreatedDate"" as CreatedDate
                ,f.""VersionNo"" as NoteVersionNo
                ,f.""LockStatus"" as LockStatus
                ,f.""LastUpdatedDate"" as LastUpdatedDate
                ,f.""StartDate"" as Start,coalesce(f.""NoteSubject"", f.""NoteDescription"", '') as Title
                ,tm.""Name"" as DocumentType,tm.""Code"" as TemplateMasterCode,tm.""Id"" as TemplateMasterId
                ,tm.""Id"" as DocumentTypeId
                ,coalesce(udf.""DocumentId"",udf.""fileAttachmentId"") as DocumentId
                ,gen.""FileName"" as FileName
                ,lov.""Name"" as StatusName,lov.""Code"" as NoteStatus
                ,null as tag
                , dast.""Name"" as DocumentApprovalStatusType
                , udf.""documentApprovalStatus"" as DocumentsApprovalStatus,udf.""stageStatus"" as StageStatus,
                udf.""discipline"" as Discipline,coalesce(udf.""code"",udf.""OutgoingIssueCodes"",udf.""issueCodes"") as IssueCode
                ,udf.""vendorList"" as Vendor,udf.""projectFolder"" as ProjectFolder
                ,ou.""Name"" as OwnerUser, ou.""Name"" as CreatedByUser, u.""Name"" as UpdatedByUser
                ,s.""Id"" as ServiceId,s.""ServiceNo"" as WorkflowNo,s.""ServiceSubject"" as WorkflowName
                ,null as WorkflowStatus, null as WorkflowTemplateId,st.""Code"" as WorkflowCode
                ,'N' as NtsId,true as IsAllDay, gen.""ContentLength"" as FileSize,0 as PhotoId
                ,'' as TagIds,'' as TagNames
                ,null as EnableDocumentChangeRequest
                , null as EnableLock
                ,'' as TagCategoryIds
                ,'' as TagCategoryNames 
                , f.""SequenceOrder"" as SequenceNo
                ,cs.""Id"" as ChangeRequestServiceId
                ,f.""NoteNo"" as DocumentNo,null as WorkflowServiceId,null as WorkflowServiceStatus,null as WorkflowServiceStatusName
				
                from 
                public.""User"" as u
                join public.""NtsNote"" as f on f.""OwnerUserId""<>u.""Id"" and u.""Id""='{UserId}' and f.""ParentNoteId""='{parentId}' and f.""IsDeleted""=false and f.""CompanyId""='{_repo.UserContext.CompanyId}' and  (f.""IsArchived"" <> true or f.""IsArchived"" is null)
                and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}' 
                join public.""User"" as ou on f.""OwnerUserId""=ou.""Id"" and ou.""IsDeleted""=false and ou.""CompanyId""='{_repo.UserContext.CompanyId}'
                join public.""LOV"" as lov on lov.""Id""=f.""NoteStatusId"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_repo.UserContext.CompanyId}'	

                join public.""Template"" as tm on tm.""Id""=f.""TemplateId"" and tm.""IsDeleted""=false and tm.""CompanyId""='{_repo.UserContext.CompanyId}' --and tm.""Code""='GENERAL_FOLDER'
                join public.""TemplateCategory"" as tc on tc.""Id""=tm.""TemplateCategoryId""  and tc.""IsDeleted""=false and tc.""CompanyId""='{_repo.UserContext.CompanyId}' and tc.""Code""='GENERAL_DOCUMENT'
                left join (" + udfs + $@") as udf on udf.""NtsNoteId""=f.""Id""  	

                join public.""NtsNote"" as pn on pn.""Id""=f.""ParentNoteId"" and pn.""IsDeleted""=false and pn.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join public.""LOV"" as dast on dast.""Id""=udf.""documentApprovalStatusType"" and dast.""IsDeleted""=false and dast.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join(select np1.*,case when npu is null then npwgu.""Id"" else npu.""Id"" end as ""UserId""
                from  public.""DocumentPermission"" as np1
                left join public.""User"" as npu on npu.""Id""=np1.""PermittedUserId"" and npu.""Id""='{UserId}' and npu.""IsDeleted""=false and npu.""CompanyId""='{_repo.UserContext.CompanyId}' 


                left join public.""UserGroupUser"" as npwg on npwg.""UserGroupId""=np1.""PermittedUserGroupId""  and npwg.""IsDeleted""=false and npwg.""CompanyId""='{_repo.UserContext.CompanyId}' 

                left join public.""User"" as npwgu on npwgu.""Id""=npwg.""UserId""  and npwgu.""Id""='{UserId}'	 and npwgu.""IsDeleted""=false and npwgu.""CompanyId""='{_repo.UserContext.CompanyId}'

                where np1.""IsDeleted""=false	  )
                as np on np.""NoteId""=f.""Id"" and np.""UserId""='{UserId}'			
                left join public.""NtsService"" as s on s.""Id""=f.""ParentServiceId"" and s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join public.""NtsService"" as cs on cs.""Id""=udf.""ChangeRequestServiceId"" and cs.""IsDeleted""=false and cs.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join public.""File"" as gen on gen.""Id""=coalesce(udf.""DocumentId"",udf.""fileAttachmentId"") and gen.""IsDeleted""=false and gen.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join public.""Template"" as st on st.""Id""=tm.""WorkFlowTemplateId"" and st.""IsDeleted""=false and st.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join cms.""N_GENERAL_FOLDER_WORKSPACE"" as ws on ws.""Id""=udf.""WorkspaceId"" and ws.""IsDeleted""=false and ws.""CompanyId""='{_repo.UserContext.CompanyId}'
				left join public.""NtsNote"" as wn on wn.""Id""=ws.""NtsNoteId"" and wn.""IsDeleted""=false

               
                ";

            var list = await _queryRepo1.ExecuteQueryList(query, null);
            list = list.OrderByDescending(x => x.Access).DistinctBy(x => x.Id).ToList();
            //list = list.DistinctBy(x => x.Id).ToList();

            ManageParentHierarchyNew(list);
            return list.Where(x => x.PermissionType == DmsPermissionTypeEnum.Allow).ToList();
        }
        public async Task<List<FolderViewModel>> GetAllChildWorkspaceFolderAndFiles(string UserId, string parentId)
        {
            var udfs = await _noteBusiness.GetUdfQuery(null, "GENERAL_DOCUMENT", null, null, "attachment,DocumentId", "fileAttachment,fileAttachmentId", "documentApprovalStatusType,documentApprovalStatusType", "code,code", "issueCodes,issueCodes"
              , "documentApprovalStatus,documentApprovalStatus", "stageStatus,stageStatus", "discipline,discipline", "OutgoingIssueCodes,OutgoingIssueCodes", "vendorList,vendorList", "projectFolder,projectFolder", "ChangeRequestId,ChangeRequestServiceId", "workspaceId,WorkspaceId");
            var query = $@"
                Select distinct  n.""Id"" as Id,n.""NoteSubject"" as Name
                ,tm.""Code"" as FolderCode,ws.""Id"" as WorkspaceId
                ,'false' as IsSelfWorkspace,false as IsWorkspaceAdmin,true as IsAssignedWorkspace,'Workspace' as FolderType
                , np.""PermissionType""  as PermissionType
                , np.""Access""  as Access
                , np.""InheritedFrom""  as InheritedFrom
               ,np.""AppliesTo""  as AppliesTo
                ,n.""OwnerUserId"" as OwnerUserId,false as IsOwner,pn.""Id"" as ParentId,pn.""NoteSubject"" as ParentName,n.""IsArchived"" as IsArchived,tags.""Count"" as DocCount
                ,n.""DisablePermissionInheritance"" as DisablePermissionInheritance
				,null as DocumentName
                ,null as Description
                ,n.""CreatedDate"" as CreatedDate
                ,n.""VersionNo"" as NoteVersionNo
                ,n.""LockStatus"" as LockStatus
                ,n.""LastUpdatedDate"" as LastUpdatedDate
                ,n.""StartDate"" as Start
                ,coalesce(n.""NoteSubject"", n.""NoteDescription"", '') as Title
                ,tm.""Name"" as DocumentType
                ,tm.""Code"" as TemplateMasterCode
                ,tm.""Id"" as TemplateMasterId
                ,tm.""Id"" as DocumentTypeId
                ,null as DocumentId
                ,null as FileName
                ,null as StatusName
                ,null as NoteStatus
                ,null as tag
                ,null as DocumentApprovalStatusType
                ,null as DocumentsApprovalStatus
                ,null as StageStatus
                ,null as Discipline
                ,null as IssueCode
                ,null as Vendor
                ,null as ProjectFolder
                ,null as OwnerUser
                ,u.""Name"" as CreatedByUser
                ,null as UpdatedByUser
                ,null as ServiceId
                ,null as WorkflowNo
                ,null as WorkflowName
                ,null as WorkflowStatus
                ,null as WorkflowTemplateId
                ,null as WorkflowCode
                ,'N' as NtsId
                ,true as IsAllDay
                , 0 as FileSize
                ,0 as PhotoId
                ,null as TagIds
                ,null as TagNames
                ,null as EnableDocumentChangeRequest
                , null as EnableLock
                ,null as TagCategoryIds
                ,null as TagCategoryNames 
                ,ws.""SequenceOrder"" as SequenceNo
                ,null as ChangeRequestServiceId
                ,n.""NoteNo"" as DocumentNo
                ,null as WorkflowServiceId
                ,null as WorkflowServiceStatus
                ,null as WorkflowServiceStatusName
				
				from
				 public.""NtsNote"" as n 
				 join cms.""N_GENERAL_FOLDER_WORKSPACE"" as ws on ws.""NtsNoteId""=n.""Id"" and n.""ParentNoteId""='{parentId}' and  (n.""IsArchived"" <> true or n.""IsArchived"" is null) and
                n.""IsDeleted""=false and n.""NoteSubject"" <>'My Workspace' and ws.""IsDeleted""=false  and n.""CompanyId""='{_repo.UserContext.CompanyId}'
				   
				 join public.""Template"" as tm on tm.""Id""=n.""TemplateId"" and tm.""IsDeleted""=false and tm.""Code""='WORKSPACE_GENERAL' and tm.""CompanyId""='{_repo.UserContext.CompanyId}'
				 join public.""TemplateCategory"" as tc on tc.""Id""=tm.""TemplateCategoryId""  and tc.""IsDeleted""=false and tc.""Code""='GENERAL_FOLDER' and tc.""CompanyId""='{_repo.UserContext.CompanyId}'
				 left join public.""User"" as u on u.""Id""=n.""OwnerUserId""
				 left join(select np.*,case when npu is null then npwgu.""Id"" else npu.""Id"" end as ""UserId"" 
                from  public.""DocumentPermission"" as np  
                left join public.""User"" as npu on npu.""Id""=np.""PermittedUserId"" and npu.""Id""='{UserId}' and
                npu.""IsDeleted""=false and npu.""CompanyId""='{_repo.UserContext.CompanyId}' 			
				left join public.""UserGroupUser"" as npwg on npwg.""UserGroupId""=np.""PermittedUserGroupId""  and npwg.""IsDeleted""=false and npwg.""CompanyId""='{_repo.UserContext.CompanyId}' 
				left join public.""User"" as npwgu on npwgu.""Id""=npwg.""UserId""  and npwgu.""Id""='{UserId}'	 and npwgu.""IsDeleted""=false and npwgu.""CompanyId""='{_repo.UserContext.CompanyId}'
				where np.""IsDeleted""=false)
				as np on np.""NoteId""=n.""Id"" and np.""UserId""='{UserId}'
                left join public.""NtsNote"" as pn on pn.""Id""=n.""ParentNoteId"" and pn.""IsDeleted""=false and pn.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join (select count(*) as ""Count"",""NtsId"" from public.""NtsTag"" as tag
                join public.""NtsNote"" as doc on doc.""Id""=tag.""TagSourceReferenceId"" and (doc.""IsArchived"" <> true or doc.""IsArchived"" is null) and doc.""IsDeleted""=false and doc.""CompanyId""='{_repo.UserContext.CompanyId}' 
                join(select distinct np.""NoteId"",case when npu is null then npwgu.""Id"" else npu.""Id"" end as ""UserId""
                from  public.""DocumentPermission"" as np  
                left join public.""User"" as npu on npu.""Id""=np.""PermittedUserId"" and npu.""Id""='{UserId}' and npu.""IsDeleted""=false and npu.""CompanyId""='{_repo.UserContext.CompanyId}' 			
                left join public.""UserGroupUser"" as npwg on npwg.""UserGroupId""=np.""PermittedUserGroupId""  and npwg.""IsDeleted""=false and npwg.""CompanyId""='{_repo.UserContext.CompanyId}' 
                left join public.""User"" as npwgu on npwgu.""Id""=npwg.""UserId""  and npwgu.""Id""='{UserId}'	 and npwgu.""IsDeleted""=false and npwgu.""CompanyId""='{_repo.UserContext.CompanyId}'
                where np.""IsDeleted""=false	  )
                as np on np.""NoteId""=doc.""Id"" and np.""UserId""='{UserId}' and tag.""IsDeleted""=false and tag.""TagId"" is null and tag.""CompanyId""='{_repo.UserContext.CompanyId}'  group by tag.""NtsId"") as Tags on ""NtsId""=n.""Id""
				left join public.""LOV"" as lv on lv.""Id""=ws.""TypeId"" and lv.""IsDeleted""=false  and lv.""CompanyId""='{_repo.UserContext.CompanyId}' 
				where  lv.""Code"" <>'MY_WORKSPACE' or lv.""Code"" is null and n.""IsDeleted""=false and ws.""IsDeleted""=false  and n.""CompanyId""='{_repo.UserContext.CompanyId}' 
               
                UNION                
                
                Select distinct f.""Id"" as Id,f.""NoteSubject"" as Name                
                ,tm.""Code"" as FolderCode,ws.""Id"" as WorkspaceId
                ,case when wn.""NoteSubject""='My Workspace' then 'true' else 'false' end as IsSelfWorkspace,false as IsWorkspaceAdmin
                ,false as IsAssignedWorkspace,'Folder' as FolderType
                , np.""PermissionType""  as PermissionType
                , np.""Access""  as Access
                , np.""InheritedFrom""  as InheritedFrom
                , np.""AppliesTo""  as AppliesTo
                ,null as OwnerUserId,false as IsOwner,pn.""Id"" as ParentId,pn.""NoteSubject"" as ParentName,f.""IsArchived"" as IsArchived
				,tags.""Count"" as DocCount
                ,f.""DisablePermissionInheritance"" as DisablePermissionInheritance
                ,null as DocumentName
                ,null as Description
                ,f.""CreatedDate"" as CreatedDate
                ,f.""VersionNo"" as NoteVersionNo
                ,f.""LockStatus"" as LockStatus
                ,f.""LastUpdatedDate"" as LastUpdatedDate
                ,f.""StartDate"" as Start
                ,coalesce(f.""NoteSubject"", f.""NoteDescription"", '') as Title
                ,tm.""Name"" as DocumentType
                ,tm.""Code"" as TemplateMasterCode
                ,tm.""Id"" as TemplateMasterId
                ,tm.""Id"" as DocumentTypeId
                ,null as DocumentId
                ,null as FileName
                ,null as StatusName
                ,null as NoteStatus
                ,null as tag
                ,null as DocumentApprovalStatusType
                ,null as DocumentsApprovalStatus
                ,null as StageStatus
                ,null as Discipline
                ,null as IssueCode
                ,null as Vendor
                ,null as ProjectFolder
                ,null as OwnerUser
                ,u.""Name"" as CreatedByUser
                ,null as UpdatedByUser
                ,null as ServiceId
                ,null as WorkflowNo
                ,null as WorkflowName
                ,null as WorkflowStatus
                ,null as WorkflowTemplateId
                ,null as WorkflowCode
                ,'N' as NtsId
                ,true as IsAllDay
                , 0 as FileSize
                ,0 as PhotoId
                ,null as TagIds
                ,null as TagNames
                ,null as EnableDocumentChangeRequest
                , null as EnableLock
                ,null as TagCategoryIds
                ,null as TagCategoryNames 
                ,f.""SequenceOrder"" as SequenceNo
                ,null as ChangeRequestServiceId
                ,f.""NoteNo"" as DocumentNo
                ,null as WorkflowServiceId
                ,null as WorkflowServiceStatus
                ,null as WorkflowServiceStatusName
								
                from 

				public.""NtsNote"" as f  
				join cms.""N_GENERAL_FOLDER_GENERALFOLDER"" as ws1 on ws1.""NtsNoteId""=f.""Id"" and f.""ParentNoteId""='{parentId}' and  (f.""IsArchived"" <> true or f.""IsArchived"" is null)	and f.""IsDeleted""=false and ws1.""IsDeleted""=false	and f.""CompanyId""='{_repo.UserContext.CompanyId}'				
				join public.""Template"" as tm on tm.""Id""=f.""TemplateId"" and tm.""IsDeleted""=false and tm.""Code""='GENERAL_FOLDER' and tm.""CompanyId""='{_repo.UserContext.CompanyId}'
				join public.""TemplateCategory"" as tc on tc.""Id""=tm.""TemplateCategoryId""  and tc.""IsDeleted""=false and tc.""Code""='GENERAL_FOLDER' and tc.""CompanyId""='{_repo.UserContext.CompanyId}'
				join public.""NtsNote"" as pn on pn.""Id""=f.""ParentNoteId"" and pn.""IsDeleted""=false and pn.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join cms.""N_GENERAL_FOLDER_WORKSPACE"" as ws on ws.""Id""=ws1.""WorkspaceId"" and ws.""IsDeleted""=false and ws1.""CompanyId""='{_repo.UserContext.CompanyId}'
				left join public.""NtsNote"" as wn on wn.""Id""=ws.""NtsNoteId"" and wn.""IsDeleted""=false
                left join public.""User"" as u on u.""Id""=f.""OwnerUserId""

	            left join(select np.*,case when npu is null then npwgu.""Id"" else npu.""Id"" end as ""UserId"" 
                from  public.""DocumentPermission"" as np 
                left join public.""User"" as npu on npu.""Id""=np.""PermittedUserId"" and npu.""Id""='{UserId}' and npu.""IsDeleted""=false and npu.""CompanyId""='{_repo.UserContext.CompanyId}' 
			
				left join public.""UserGroupUser"" as npwg on npwg.""UserGroupId""=np.""PermittedUserGroupId""  and npwg.""IsDeleted""=false and npwg.""CompanyId""='{_repo.UserContext.CompanyId}' 
				left join public.""User"" as npwgu on npwgu.""Id""=npwg.""UserId""  and npwgu.""Id""='{UserId}'	 and npwgu.""IsDeleted""=false and npwgu.""CompanyId""='{_repo.UserContext.CompanyId}'
					where np.""IsDeleted""=false)
				as np on np.""NoteId""=f.""Id"" and np.""UserId""='{UserId}'
                 left join (select count(*) as ""Count"",""NtsId"" from public.""NtsTag"" as tag
                join public.""NtsNote"" as doc on doc.""Id""=tag.""TagSourceReferenceId"" and (doc.""IsArchived"" <> true or doc.""IsArchived"" is null) and doc.""IsDeleted""=false and doc.""CompanyId""='{_repo.UserContext.CompanyId}' 
                join(select distinct np.""NoteId"",case when npu is null then npwgu.""Id"" else npu.""Id"" end as ""UserId""
                from  public.""DocumentPermission"" as np  
                left join public.""User"" as npu on npu.""Id""=np.""PermittedUserId"" and npu.""Id""='{UserId}' and npu.""IsDeleted""=false and npu.""CompanyId""='{_repo.UserContext.CompanyId}' 			
                left join public.""UserGroupUser"" as npwg on npwg.""UserGroupId""=np.""PermittedUserGroupId""  and npwg.""IsDeleted""=false and npwg.""CompanyId""='{_repo.UserContext.CompanyId}' 
                left join public.""User"" as npwgu on npwgu.""Id""=npwg.""UserId""  and npwgu.""Id""='{UserId}'	 and npwgu.""IsDeleted""=false and npwgu.""CompanyId""='{_repo.UserContext.CompanyId}'
                where np.""IsDeleted""=false	  )
                as np on np.""NoteId""=doc.""Id"" and np.""UserId""='{UserId}' and tag.""IsDeleted""=false and tag.""TagId"" is null and tag.""CompanyId""='{_repo.UserContext.CompanyId}'  group by tag.""NtsId"") as Tags on ""NtsId""=f.""Id""
                
                UNION
            
                select f.""Id"" as Id,coalesce(gen.""FileName"",'No File') as Name,tm.""Code"" as FolderCode,null as WorkspaceId
                ,case when wn.""NoteSubject""='My Workspace' then 'true' else 'false' end as IsSelfWorkspace
                ,false as IsWorkspaceAdmin,false as IsAssignedWorkspace,'File' as FolderType
                ,0 as PermissionType,2 as Access
                ,'' as InheritedFrom
                ,3 as AppliesTo
                ,u.""Id"" as OwnerUserId,true as IsOwner,pn.""Id"" as ParentId,pn.""NoteSubject"" as ParentName
                ,f.""IsArchived"" as IsArchived
                ,0 as DocCount
                ,f.""DisablePermissionInheritance"" as DisablePermissionInheritance
                ,f.""NoteSubject"" as DocumentName,f.""NoteDescription"" as Description,f.""CreatedDate"" as CreatedDate
                ,f.""VersionNo"" as NoteVersionNo
                ,f.""LockStatus"" as LockStatus
                ,f.""LastUpdatedDate"" as LastUpdatedDate
                ,f.""StartDate"" as Start,coalesce(f.""NoteSubject"", f.""NoteDescription"", '') as Title
                ,tm.""Name"" as DocumentType,tm.""Code"" as TemplateMasterCode,tm.""Id"" as TemplateMasterId
                ,tm.""Id"" as DocumentTypeId
                ,coalesce(udf.""DocumentId"",udf.""fileAttachmentId"") as DocumentId
                ,gen.""FileName"" as FileName
                ,lov.""Name"" as StatusName,lov.""Code"" as NoteStatus
                ,null as tag
                , dast.""Name"" as DocumentApprovalStatusType
                , udf.""documentApprovalStatus"" as DocumentsApprovalStatus,udf.""stageStatus"" as StageStatus,
                udf.""discipline"" as Discipline,coalesce(udf.""code"",udf.""OutgoingIssueCodes"",udf.""issueCodes"") as IssueCode
                ,udf.""vendorList"" as Vendor,udf.""projectFolder"" as ProjectFolder
                ,u.""Name"" as OwnerUser, u.""Name"" as CreatedByUser, u.""Name"" as UpdatedByUser
                ,s.""Id"" as ServiceId,s.""ServiceNo"" as WorkflowNo,s.""ServiceSubject"" as WorkflowName
                ,null as WorkflowStatus, st.""Id"" as WorkflowTemplateId,st.""Code"" as WorkflowCode
                ,'N' as NtsId,true as IsAllDay, gen.""ContentLength"" as FileSize,0 as PhotoId
                ,ntg.""TagId"" as TagIds,tg.""NoteSubject"" as TagNames
                ,null as EnableDocumentChangeRequest, null as EnableLock
                ,ntg.""TagCategoryId"" as TagCategoryIds,tgc.""NoteSubject"" as TagCategoryNames 
                , f.""SequenceOrder"" as SequenceNo,
                '' as ChangeRequestServiceId,f.""NoteNo"" as DocumentNo,wfs.""Id"" as WorkflowServiceId,wfss.""Code"" as WorkflowServiceStatus,wfss.""Name"" as WorkflowServiceStatusName

                from 
                public.""User"" as u
                join public.""NtsNote"" as f on f.""OwnerUserId""=u.""Id"" and u.""Id""='{UserId}' and f.""ParentNoteId""='{parentId}' and f.""IsDeleted""=false and f.""CompanyId""='{_repo.UserContext.CompanyId}' and  (f.""IsArchived"" <> true or f.""IsArchived"" is null) and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
                join public.""LOV"" as lov on lov.""Id""=f.""NoteStatusId"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join (" + udfs + $@") as udf on udf.""NtsNoteId""=f.""Id"" 

                join public.""Template"" as tm on tm.""Id""=f.""TemplateId"" and tm.""IsDeleted""=false and tm.""CompanyId""='{_repo.UserContext.CompanyId}'--and tm.""Code""='GENERAL_FOLDER'
                join public.""TemplateCategory"" as tc on tc.""Id""=tm.""TemplateCategoryId""  and tc.""IsDeleted""=false and tc.""CompanyId""='{_repo.UserContext.CompanyId}' and tc.""Code""='GENERAL_DOCUMENT'
                join public.""NtsNote"" as pn on pn.""Id""=f.""ParentNoteId"" and pn.""IsDeleted""=false and pn.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join public.""LOV"" as dast on dast.""Id""=udf.""documentApprovalStatusType"" and dast.""IsDeleted""=false and dast.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join public.""NtsService"" as s on s.""Id""=f.""ParentServiceId"" and s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join public.""NtsService"" as wfs on wfs.""Id""=f.""ReferenceId"" and wfs.""IsDeleted""=false and wfs.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join public.""LOV"" as wfss on wfss.""Id""=wfs.""ServiceStatusId"" and wfss.""IsDeleted""=false and wfss.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join public.""File"" as gen on gen.""Id""=coalesce(udf.""DocumentId"",udf.""fileAttachmentId"") and gen.""IsDeleted""=false and gen.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join public.""Template"" as st on st.""Id""=tm.""WorkFlowTemplateId"" and st.""IsDeleted""=false and st.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join public.""NtsTag"" as ntg on ntg.""NtsId""=f.""Id"" and ntg.""IsDeleted""=false and ntg.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join public.""NtsNote"" as tg on tg.""Id""=ntg.""TagId"" and tg.""IsDeleted""=false and tg.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join public.""NtsNote"" as tgc on tgc.""Id""=tg.""ParentNoteId"" and tgc.""IsDeleted""=false and tgc.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join cms.""N_GENERAL_FOLDER_WORKSPACE"" as ws on ws.""Id""=udf.""WorkspaceId"" and ws.""IsDeleted""=false and ws.""CompanyId""='{_repo.UserContext.CompanyId}'
				left join public.""NtsNote"" as wn on wn.""Id""=ws.""NtsNoteId"" and wn.""IsDeleted""=false
                
                
                
                UNION

                Select f.""Id"" as Id,coalesce(gen.""FileName"",'No File') as Name,tm.""Code"" as FolderCode,null as WorkspaceId
                ,case when wn.""NoteSubject""='My Workspace' then 'true' else 'false' end as IsSelfWorkspace
                ,false as IsWorkspaceAdmin,false as IsAssignedWorkspace,'File' as FolderType
                ,np.""PermissionType"" as PermissionType,np.""Access"" as Access
                ,'' as InheritedFrom
                ,np.""AppliesTo"" as AppliesTo
                ,u.""Id"" as OwnerUserId,false as IsOwner,pn.""Id"" as ParentId,pn.""NoteSubject"" as ParentName
                ,f.""IsArchived"" as IsArchived
                ,0 as DocCount
                ,f.""DisablePermissionInheritance"" as DisablePermissionInheritance
                ,f.""NoteSubject"" as DocumentName,f.""NoteDescription"" as Description,f.""CreatedDate"" as CreatedDate
                ,f.""VersionNo"" as NoteVersionNo
                ,f.""LockStatus"" as LockStatus
                ,f.""LastUpdatedDate"" as LastUpdatedDate
                ,f.""StartDate"" as Start,coalesce(f.""NoteSubject"", f.""NoteDescription"", '') as Title
                ,tm.""Name"" as DocumentType,tm.""Code"" as TemplateMasterCode,tm.""Id"" as TemplateMasterId
                ,tm.""Id"" as DocumentTypeId
                , coalesce(udf.""DocumentId"",udf.""fileAttachmentId"") as DocumentId
                ,gen.""FileName"" as FileName
                ,lov.""Name"" as StatusName,lov.""Code"" as NoteStatus
                ,null as tag
                , dast.""Name"" as DocumentApprovalStatusType
                , udf.""documentApprovalStatus"" as DocumentsApprovalStatus,udf.""stageStatus"" as StageStatus,
                udf.""discipline"" as Discipline,coalesce(udf.""code"",udf.""OutgoingIssueCodes"",udf.""issueCodes"") as IssueCode
                ,udf.""vendorList"" as Vendor,udf.""projectFolder"" as ProjectFolder
                ,ou.""Name"" as OwnerUser, ou.""Name"" as CreatedByUser, ou.""Name"" as UpdatedByUser
                ,s.""Id"" as ServiceId,s.""ServiceNo"" as WorkflowNo,s.""ServiceSubject"" as WorkflowName
                ,null as WorkflowStatus, null as WorkflowTemplateId,st.""Code"" as WorkflowCode
                ,'N' as NtsId,true as IsAllDay, gen.""ContentLength"" as FileSize,0 as PhotoId
                ,'' as TagIds,'' as TagNames
                ,null as EnableDocumentChangeRequest
                , null as EnableLock
                ,'' as TagCategoryIds
                ,'' as TagCategoryNames 
                , f.""SequenceOrder"" as SequenceNo
                ,cs.""Id"" as ChangeRequestServiceId
                ,f.""NoteNo"" as DocumentNo,null as WorkflowServiceId,null as WorkflowServiceStatus,null as WorkflowServiceStatusName
				
                from 
                public.""User"" as u
                join public.""NtsNote"" as f on f.""OwnerUserId""<>u.""Id"" and u.""Id""='{UserId}' and f.""ParentNoteId""='{parentId}' and f.""IsDeleted""=false and f.""CompanyId""='{_repo.UserContext.CompanyId}' and  (f.""IsArchived"" <> true or f.""IsArchived"" is null)
                and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}' 
                join public.""User"" as ou on f.""OwnerUserId""=ou.""Id"" and ou.""IsDeleted""=false and ou.""CompanyId""='{_repo.UserContext.CompanyId}'
                join public.""LOV"" as lov on lov.""Id""=f.""NoteStatusId"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_repo.UserContext.CompanyId}'	

                join public.""Template"" as tm on tm.""Id""=f.""TemplateId"" and tm.""IsDeleted""=false and tm.""CompanyId""='{_repo.UserContext.CompanyId}' --and tm.""Code""='GENERAL_FOLDER'
                join public.""TemplateCategory"" as tc on tc.""Id""=tm.""TemplateCategoryId""  and tc.""IsDeleted""=false and tc.""CompanyId""='{_repo.UserContext.CompanyId}' and tc.""Code""='GENERAL_DOCUMENT'
                left join (" + udfs + $@") as udf on udf.""NtsNoteId""=f.""Id""  	

                join public.""NtsNote"" as pn on pn.""Id""=f.""ParentNoteId"" and pn.""IsDeleted""=false and pn.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join public.""LOV"" as dast on dast.""Id""=udf.""documentApprovalStatusType"" and dast.""IsDeleted""=false and dast.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join(select np1.*,case when npu is null then npwgu.""Id"" else npu.""Id"" end as ""UserId""
                from  public.""DocumentPermission"" as np1
                left join public.""User"" as npu on npu.""Id""=np1.""PermittedUserId"" and npu.""Id""='{UserId}' and npu.""IsDeleted""=false and npu.""CompanyId""='{_repo.UserContext.CompanyId}' 


                left join public.""UserGroupUser"" as npwg on npwg.""UserGroupId""=np1.""PermittedUserGroupId""  and npwg.""IsDeleted""=false and npwg.""CompanyId""='{_repo.UserContext.CompanyId}' 

                left join public.""User"" as npwgu on npwgu.""Id""=npwg.""UserId""  and npwgu.""Id""='{UserId}'	 and npwgu.""IsDeleted""=false and npwgu.""CompanyId""='{_repo.UserContext.CompanyId}'

                where np1.""IsDeleted""=false	  )
                as np on np.""NoteId""=f.""Id"" and np.""UserId""='{UserId}'			
                left join public.""NtsService"" as s on s.""Id""=f.""ParentServiceId"" and s.""IsDeleted""=false and s.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join public.""NtsService"" as cs on cs.""Id""=udf.""ChangeRequestServiceId"" and cs.""IsDeleted""=false and cs.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join public.""File"" as gen on gen.""Id""=coalesce(udf.""DocumentId"",udf.""fileAttachmentId"") and gen.""IsDeleted""=false and gen.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join public.""Template"" as st on st.""Id""=tm.""WorkFlowTemplateId"" and st.""IsDeleted""=false and st.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join cms.""N_GENERAL_FOLDER_WORKSPACE"" as ws on ws.""Id""=udf.""WorkspaceId"" and ws.""IsDeleted""=false and ws.""CompanyId""='{_repo.UserContext.CompanyId}'
				left join public.""NtsNote"" as wn on wn.""Id""=ws.""NtsNoteId"" and wn.""IsDeleted""=false

               
                ";

            var list = await _queryRepo1.ExecuteQueryList(query, null);
            list = list.OrderByDescending(x => x.Access).DistinctBy(x => x.Id).ToList();
            //list = list.DistinctBy(x => x.Id).ToList();

            ManageParentHierarchyNew(list);
            return list.Where(x => x.PermissionType == DmsPermissionTypeEnum.Allow).ToList();
        }
        public async Task<List<FolderViewModel>> GetAllGeneralWorkspaceData()
        {
            var query = string.Concat($@"select distinct  n.""Id"" as Id,n.""NoteSubject"" as Name                
                ,n.""CreatedDate"" as CreatedDate,n.""LastUpdatedDate"" as LastUpdatedDate
                ,n.""NoteNo"" as DocumentNo,u.""Name"" as CreatedByUser ,n.""OwnerUserId"" as OwnerUserId
                from 
                public.""User"" as u
                join public.""NtsNote"" as n on n.""OwnerUserId""=u.""Id"" and  (n.""IsArchived"" <> true or n.""IsArchived"" is null) and n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}'
                join cms.""N_GENERAL_FOLDER_WORKSPACE"" as ws on ws.""NtsNoteId""=n.""Id"" and ws.""IsDeleted""=false and ws.""CompanyId""='{_repo.UserContext.CompanyId}'
                join public.""Template"" as tm on tm.""Id""=n.""TemplateId"" and tm.""IsDeleted""=false and tm.""CompanyId""='{_repo.UserContext.CompanyId}' 
                join public.""TemplateCategory"" as tc on tc.""Id""=tm.""TemplateCategoryId""  and tc.""IsDeleted""=false and tc.""Code""='GENERAL_FOLDER' and tc.""CompanyId""='{_repo.UserContext.CompanyId}' 
                join public.""LOV"" as lv on lv.""Id""=ws.""TypeId"" and lv.""IsDeleted""=false and  lv.""Code"" not in ('MY_WORKSPACE')
                ");
            var list = await _queryRepo1.ExecuteQueryList(query, null);            
            return list.ToList();
        }
        public async Task<List<FolderViewModel>> GetAllChildbyParent(string parentId)
        {
            var query = $@"select  n.""Id"" as Id,n.""NoteSubject"" as Name, p.""Id"" as ParentId,n.""OwnerUserId"" as OwnerUserId
                from public.""NtsNote"" as n 
                join public.""NtsNote"" as p on p.""Id""=n.""ParentNoteId"" and p.""IsDeleted""=false and p.""CompanyId"" = '{_repo.UserContext.CompanyId}'  
                join public.""Template"" as t on t.""Id""=n.""TemplateId"" and t.""IsDeleted""=false and t.""CompanyId"" = '{_repo.UserContext.CompanyId}'
                join public.""TemplateCategory"" as tc on t.""TemplateCategoryId""=tc.""Id"" and tc.""Code""='GENERAL_FOLDER' and tc.""IsDeleted""=false and tc.""CompanyId"" = '{_repo.UserContext.CompanyId}'
                where p.""Id""='{parentId}' and n.""IsDeleted""=false and n.""CompanyId"" = '{_repo.UserContext.CompanyId}'
                ";
            var list = await _queryRepo1.ExecuteQueryList(query, null);
            return list.ToList();
        }
        public async Task<List<FolderViewModel>> CheckDocumentExist(string parentId)
        {
            var query = $@"select  n.""Id"" as Id,n.""NoteSubject"" as Name,n.""NoteNo"" as DocumentNo, p.""Id"" as ParentId
                from public.""NtsNote"" as n 
                join public.""NtsNote"" as p on p.""Id""=n.""ParentNoteId"" and p.""IsDeleted""=false and p.""CompanyId"" = '{_repo.UserContext.CompanyId}'  
                join public.""Template"" as t on t.""Id""=n.""TemplateId"" and t.""IsDeleted""=false and t.""CompanyId"" = '{_repo.UserContext.CompanyId}'
                join public.""TemplateCategory"" as tc on t.""TemplateCategoryId""=tc.""Id"" and tc.""Code""='GENERAL_DOCUMENT' and tc.""IsDeleted""=false and tc.""CompanyId"" = '{_repo.UserContext.CompanyId}'
                where p.""Id""='{parentId}' and n.""IsDeleted""=false and n.""CompanyId"" = '{_repo.UserContext.CompanyId}'
                ";
            var list = await _queryRepo1.ExecuteQueryList(query, null);
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
            var cypher = string.Concat($@"select n.""Id"" as Id
from public.""NtsNote"" as n
join public.""LOV"" as lv on lv.""Id""=n.""NoteStatusId"" and lv.""IsDeleted""=false and lv.""CompanyId""='{_repo.UserContext.CompanyId}' 
join public.""Template"" as t on t.""Id""=n.""TemplateId"" and t.""IsDeleted""=false and t.""CompanyId""='{_repo.UserContext.CompanyId}' 
join public.""TemplateCategory"" as c on c.""Id""=t.""TemplateCategoryId"" and c.""IsDeleted""=false and c.""CompanyId""='{_repo.UserContext.CompanyId}' 
left join public.""DocumentPermission"" as dp on dp.""NoteId""=n.""Id"" and dp.""PermissionType""=0  and dp.""PermittedUserId""='{userId}' and dp.""IsDeleted""=false and dp.""CompanyId""='{_repo.UserContext.CompanyId}'   
left join public.""UserGroup"" as ug on ug.""Id""=dp.""PermittedUserGroupId"" and ug.""IsDeleted""=false and ug.""CompanyId""='{_repo.UserContext.CompanyId}' 
left join public.""UserGroupUser"" as ugu on ugu.""UserGroupId""=ug.""Id"" and ugu.""UserId""='{userId}' and ugu.""IsDeleted""=false and ugu.""CompanyId""='{_repo.UserContext.CompanyId}' 
where lv.""Code""!='NOTE_STATUS_DRAFT' 
 
and n.""OwnerUserId""='{userId}' and n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}' 
and c.""Code""='GENERAL_DOCUMENT' and (n.""IsArchived""!=true or n.""IsArchived"" is null)");

            var list = await _queryRepo1.ExecuteScalarList<string>(cypher, null);
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
                var cypher1 = string.Concat($@"select n.""Id"" as DocumentId,n.""NoteNo"" as NoteNo ,n.""NoteSubject"" as DocumentName,n.""CreatedDate"" as CreatedDate,
 u.""Name"" as TemplateOwner,
 LOV_revision.""Name"" as Revision,LOV_revision.""Code"" as RevisionCode,
                    LOV_discipline.""Name"" as Discipline,LOV_discipline.""Code"" as DisciplineCode,
                    coalesce(LOV_stageStatus.""Name"",LOV_stageStatus.""Code"") as StageStatus,                     
                    coalesce(LOV_code.""Name"",LOV_code.""Code"") as IssueCode,      
 udfValue.""galfarTransmittalNumber"" as TransmittalNo,
 udfValue.""qpDueDate"" as DueDate,
udfValue.""dateOfSubmission""::date as SubmittedDate  

from 
public.""NtsNote"" as n 
join public.""LOV"" as lv on lv.""Id""=n.""NoteStatusId"" and lv.""IsDeleted""=false and lv.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""User"" as u on u.""Id""=n.""OwnerUserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""Template"" as tm on tm.""Id""=n.""TemplateId"" and tm.""Code""='PROJECT_DOCUMENTS' and tm.""IsDeleted""=false and tm.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_GENERAL_DOCUMENT_ProjectDocuments"" as udfValue on udfValue.""NtsNoteId""=n.""Id"" and udfValue.""IsDeleted""=false and udfValue.""CompanyId""='{_repo.UserContext.CompanyId}'
left join public.""LOV"" as LOV_revision on LOV_revision.""Id""=udfValue.""revision"" and LOV_revision.""IsDeleted""=false and LOV_revision.""CompanyId""='{_repo.UserContext.CompanyId}'
left join public.""LOV"" as LOV_discipline on LOV_discipline.""Id""=udfValue.""discipline"" and LOV_discipline.""IsDeleted""=false and LOV_discipline.""CompanyId""='{_repo.UserContext.CompanyId}'
left join public.""LOV"" as LOV_stageStatus on LOV_stageStatus.""Id""=udfValue.""stageStatus"" and LOV_stageStatus.""IsDeleted""=false and LOV_stageStatus.""CompanyId""='{_repo.UserContext.CompanyId}'
left join public.""LOV"" as LOV_code on LOV_code.""Id""=udfValue.""outgoingIssueCodes"" and LOV_code.""IsDeleted""=false and LOV_code.""CompanyId""='{_repo.UserContext.CompanyId}'
 where 1=1 and n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}' and  n.""Id"" in ('" + documentIds + $@"') and lv.""Code"" <> 'NOTE_STATUS_DRAFT' #DISCIPLINE# #REVESION#
                                     
                    union
                    select n.""Id"" as DocumentId,n.""NoteNo"" as NoteNo ,n.""NoteSubject"" as DocumentName,n.""CreatedDate"" as CreatedDate,
 u.""Name"" as TemplateOwner,
LOV_revision.""Name"" as Revision,LOV_revision.""Code"" as RevisionCode,
                    LOV_discipline.""Name"" as Discipline,LOV_discipline.""Code"" as DisciplineCode,
                    coalesce(LOV_stageStatus.""Name"",LOV_stageStatus.""Code"") as StageStatus,                     
                    coalesce(LOV_code.""Name"",LOV_code.""Code"") as IssueCode,      
 udfValue.""galfarTransmittalNumber"" as TransmittalNo,
 udfValue.""qpDueDate"" as DueDate,
udfValue.""dateOfSubmission""::date as SubmittedDate  

from 
public.""NtsNote"" as n 
join public.""LOV"" as lv on lv.""Id""=n.""NoteStatusId"" and lv.""IsDeleted""=false and  lv.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""User"" as u on u.""Id""=n.""OwnerUserId"" and u.""IsDeleted""=false and  lv.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""Template"" as tm on tm.""Id""=n.""TemplateId"" and tm.""Code""='ENGINEERING_SUBCONTRACT' and tm.""IsDeleted""=false and  lv.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_GENERAL_DOCUMENT_ENGSUBCONTRACT"" as udfValue on udfValue.""NtsNoteId""=n.""Id"" and udfValue.""IsDeleted""=false and  lv.""CompanyId""='{_repo.UserContext.CompanyId}'
left join public.""LOV"" as LOV_revision on LOV_revision.""Id""=udfValue.""revision"" and LOV_revision.""IsDeleted""=false and  lv.""CompanyId""='{_repo.UserContext.CompanyId}'
left join public.""LOV"" as LOV_discipline on LOV_discipline.""Id""=udfValue.""discipline"" and LOV_discipline.""IsDeleted""=false and  lv.""CompanyId""='{_repo.UserContext.CompanyId}'
left join public.""LOV"" as LOV_stageStatus on LOV_stageStatus.""Id""=udfValue.""stageStatus"" and LOV_stageStatus.""IsDeleted""=false and  lv.""CompanyId""='{_repo.UserContext.CompanyId}'
left join public.""LOV"" as LOV_code on LOV_code.""Id""=udfValue.""outgoingIssueCodes"" and LOV_code.""IsDeleted""=false and  lv.""CompanyId""='{_repo.UserContext.CompanyId}'
 where 1=1 and  n.""Id"" in ('" + documentIds + $@"') and lv.""Code"" <> 'NOTE_STATUS_DRAFT' #DISCIPLINE# #REVESION#
                    union
                                        select n.""Id"" as DocumentId,n.""NoteNo"" as NoteNo ,n.""NoteSubject"" as DocumentName,n.""CreatedDate"" as CreatedDate,
 u.""Name"" as TemplateOwner,
 LOV_revision.""Name"" as Revision,LOV_revision.""Code"" as RevisionCode,
                    LOV_discipline.""Name"" as Discipline,LOV_discipline.""Code"" as DisciplineCode,
                    coalesce(LOV_stageStatus.""Name"",LOV_stageStatus.""Code"") as StageStatus,                     
                    coalesce(LOV_code.""Name"",LOV_code.""Code"") as IssueCode,      
 udfValue.""galfarTransmittalNumber"" as TransmittalNo,
 udfValue.""qpDueDate"" as DueDate,
udfValue.""dateOfSubmission""::date as SubmittedDate  

from 
public.""NtsNote"" as n 
join public.""LOV"" as lv on lv.""Id""=n.""NoteStatusId"" and lv.""IsDeleted""=false and  lv.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""User"" as u on u.""Id""=n.""OwnerUserId"" and u.""IsDeleted""=false and  u.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""Template"" as tm on tm.""Id""=n.""TemplateId"" and tm.""Code""='GALFAR_VENDOR' and tm.""IsDeleted""=false and  tm.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_GENERAL_DOCUMENT_GALFARVENDOR"" as udfValue on udfValue.""NtsNoteId""=n.""Id"" and udfValue.""IsDeleted""=false and  udfValue.""CompanyId""='{_repo.UserContext.CompanyId}'
left join public.""LOV"" as LOV_revision on LOV_revision.""Id""=udfValue.""revision"" and LOV_revision.""IsDeleted""=false and  LOV_revision.""CompanyId""='{_repo.UserContext.CompanyId}'
left join public.""LOV"" as LOV_discipline on LOV_discipline.""Id""=udfValue.""discipline"" and LOV_discipline.""IsDeleted""=false and  LOV_discipline.""CompanyId""='{_repo.UserContext.CompanyId}'
left join public.""LOV"" as LOV_stageStatus on LOV_stageStatus.""Id""=udfValue.""stageStatus"" and LOV_stageStatus.""IsDeleted""=false and  LOV_stageStatus.""CompanyId""='{_repo.UserContext.CompanyId}'
left join public.""LOV"" as LOV_code on LOV_code.""Id""=udfValue.""outgoingIssueCodes"" and LOV_code.""IsDeleted""=false and  LOV_code.""CompanyId""='{_repo.UserContext.CompanyId}'
 where 1=1 and n.""IsDeleted""=false and  n.""CompanyId"" = '{_repo.UserContext.CompanyId}'and  n.""Id"" in ('" + documentIds + @"') and lv.""Code"" <> 'NOTE_STATUS_DRAFT' #DISCIPLINE# #REVESION#");
                var searchDiscipline = "";
                if (discipline.IsNotNullAndNotEmpty())
                {
                    searchDiscipline = $@"and LOV_discipline.""Code""='{discipline}'";
                }
                cypher1 = cypher1.Replace("#DISCIPLINE#", searchDiscipline);
                var searchRevesion = "";
                if (revesion.IsNotNullAndNotEmpty())
                {
                    searchRevesion = $@"and LOV_revision.""Code""='{revesion}'";
                }
                cypher1 = cypher1.Replace("#REVESION#", searchRevesion);
                //var prms1 = new Dictionary<string, object>
                //{
                //    { "CompanyId", CompanyId },
                //    { "Status", StatusEnum.Active.ToString() },
                //    { "Discipline", discipline },
                //    { "Revesion", revesion },
                //};
                var result2 = await _queryRepo1.ExecuteQueryList<DocumentListViewModel>(cypher1, null)/*.DistinctBy(e => new { e.NoteNo, e.Revision }).ToList()*/;

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
                var cypher1 = string.Concat($@"select n.""Id"" as DocumentId,n.""NoteNo"" as NoteNo ,n.""NoteSubject"" as DocumentName,n.""CreatedDate"" as CreatedDate,u.""Name"" as TemplateOwner,
                    LOV_revision.""Name"" as Revision,LOV_revision.""Code"" as RevisionCode,
                    LOV_discipline.""Name"" as Discipline,LOV_discipline.""Code"" as DisciplineCode,
                    coalesce(LOV_stageStatus.""Name"",LOV_stageStatus.""Code"") as StageStatus,                     
                    coalesce(LOV_code.""Name"",LOV_code.""Code"") as IssueCode,                    
                    udfValue.""incomingTransmittalNumber"" as TransmittalNo,
                    udfValue.""qpDueDate"" as DueDate,
                    udfValue.""incomingTransmittalDate""::date as SubmittedDate    

from 
public.""NtsNote"" as n 
join public.""LOV"" as lv on lv.""Id""=n.""NoteStatusId"" and lv.""IsDeleted""=false and  lv.""CompanyId"" = '{_repo.UserContext.CompanyId}'
join public.""User"" as u on u.""Id""=n.""OwnerUserId"" and u.""IsDeleted""=false and  u.""CompanyId"" = '{_repo.UserContext.CompanyId}'
join public.""Template"" as tm on tm.""Id""=n.""TemplateId"" and tm.""Code""<>'PROJECT_DOCUMENTS' and tm.""IsDeleted""=false and  tm.""CompanyId"" = '{_repo.UserContext.CompanyId}'
join (" + udfs + $@") as udfValue on udfValue.""NtsNoteId""=n.""Id"" --and udfValue.""IsDeleted""=false
left join public.""LOV"" as LOV_revision on LOV_revision.""Id""=udfValue.""revision"" and LOV_revision.""IsDeleted""=false and  LOV_revision.""CompanyId"" = '{_repo.UserContext.CompanyId}'
left join public.""LOV"" as LOV_discipline on LOV_discipline.""Id""=udfValue.""discipline"" and LOV_discipline.""IsDeleted""=false and  LOV_discipline.""CompanyId"" = '{_repo.UserContext.CompanyId}'
left join public.""LOV"" as LOV_stageStatus on LOV_stageStatus.""Id""=udfValue.""stageStatus"" and LOV_stageStatus.""IsDeleted""=false and  LOV_stageStatus.""CompanyId"" = '{_repo.UserContext.CompanyId}'
left join public.""LOV"" as LOV_code on LOV_code.""Id""=udfValue.""issueCodes"" and LOV_code.""IsDeleted""=false and  LOV_code.""CompanyId"" = '{_repo.UserContext.CompanyId}'
 where 1=1 and n.""IsDeleted""=false and  n.""CompanyId"" = '{_repo.UserContext.CompanyId}' and  n.""Id"" in ('" + documentIds + @"') and lv.""Code"" <> 'NOTE_STATUS_DRAFT' #DISCIPLINE# #REVESION#
                    ");

                var searchDiscipline = "";
                if (discipline.IsNotNullAndNotEmpty())
                {
                    searchDiscipline = $@"and LOV_discipline.""Code""='{discipline}'";
                }
                cypher1 = cypher1.Replace("#DISCIPLINE#", searchDiscipline);
                var searchRevesion = "";
                if (revesion.IsNotNullAndNotEmpty())
                {
                    searchRevesion = $@"and LOV_revision.""Code""='{revesion}'";
                }
                cypher1 = cypher1.Replace("#REVESION#", searchRevesion);
                //var prms1 = new Dictionary<string, object>
                //{
                //    { "CompanyId", CompanyId },
                //    { "Status", StatusEnum.Active.ToString() },
                //    { "Discipline", discipline },
                //    { "Revesion", revesion },
                //};
                var result2 = await _queryRepo1.ExecuteQueryList<DocumentListViewModel>(cypher1, null)/*.DistinctBy(e => new { e.NoteNo, e.Revision }).ToList()*/;

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
                var cypher1 = string.Concat($@"select n.""Id"" as DocumentId,n.""NoteNo"" as NoteNo ,n.""NoteSubject"" as DocumentName,
n.""CreatedDate"" as CreatedDate,u.""Name"" as TemplateOwner,
                    LOV_revision.""Name"" as Revision,LOV_revision.""Code"" as RevisionCode,
                    LOV_discipline.""Name"" as Discipline,LOV_discipline.""Code"" as DisciplineCode,
                    coalesce(LOV_stageStatus.""Name"",LOV_stageStatus.""Code"") as StageStatus,                     
                    coalesce(LOV_OutgoingIssueCodes.""Name"",LOV_OutgoingIssueCodes.""Code"") as IssueCode,                    
                    pd.""galfarTransmittalNumber"" as TransmittalNo,
                    pd.""qpDueDate"" as DueDate,
                    pd.""dateOfSubmission""::date as SubmittedDate
from public.""NtsNote"" as n
join cms.""N_GENERAL_DOCUMENT_ProjectDocuments"" as pd on pd.""NtsNoteId""=n.""Id"" and pd.""IsDeleted""=false and  n.""CompanyId"" = '{_repo.UserContext.CompanyId}'
join public.""LOV"" as lv on lv.""Id""=n.""NoteStatusId"" and lv.""IsDeleted""=false and  lv.""CompanyId"" = '{_repo.UserContext.CompanyId}'
join public.""User"" as u on u.""Id""=n.""OwnerUserId"" and u.""IsDeleted""=false and  u.""CompanyId"" = '{_repo.UserContext.CompanyId}'
left join public.""LOV"" as LOV_projectFolder on pd.""projectFolder""=LOV_projectFolder.""Id"" and LOV_projectFolder.""IsDeleted""=false and  LOV_projectFolder.""CompanyId"" = '{_repo.UserContext.CompanyId}'
left join public.""LOV"" as LOV_projectSubFolder on pd.""projectSubFolder""=LOV_projectSubFolder.""Id"" and LOV_projectSubFolder.""IsDeleted""=false and  LOV_projectSubFolder.""CompanyId"" = '{_repo.UserContext.CompanyId}'
left join public.""LOV"" as LOV_discipline on pd.""discipline""=LOV_discipline.""Id"" and LOV_discipline.""IsDeleted""=false and  LOV_discipline.""CompanyId"" = '{_repo.UserContext.CompanyId}'
left join public.""LOV"" as LOV_revision on pd.""revision""=LOV_revision.""Id"" and LOV_revision.""IsDeleted""=false and  LOV_revision.""CompanyId"" = '{_repo.UserContext.CompanyId}'
left join public.""LOV"" as LOV_OutgoingIssueCodes on pd.""outgoingIssueCodes""=LOV_OutgoingIssueCodes.""Id"" and LOV_OutgoingIssueCodes.""IsDeleted""=false and  LOV_OutgoingIssueCodes.""CompanyId"" = '{_repo.UserContext.CompanyId}'
left join public.""LOV"" as LOV_code on pd.""code""=LOV_code.""Id"" and LOV_code.""IsDeleted""=false and  LOV_code.""CompanyId"" = '{_repo.UserContext.CompanyId}'
left join public.""LOV"" as LOV_stageStatus on pd.""stageStatus""=LOV_stageStatus.""Id"" and LOV_stageStatus.""IsDeleted""=false and  LOV_stageStatus.""CompanyId"" = '{_repo.UserContext.CompanyId}'
left join public.""LOV"" as LOV_documentApprovalStatusType on pd.""documentApprovalStatusType""=LOV_documentApprovalStatusType.""Id"" and LOV_documentApprovalStatusType.""IsDeleted""=false and  LOV_documentApprovalStatusType.""CompanyId"" = '{_repo.UserContext.CompanyId}'
where lv.""Code""!='NOTE_STATUS_DRAFT' and n.""IsDeleted""=false and  n.""CompanyId"" = '{_repo.UserContext.CompanyId}'and n.""Id"" in ('" + documentIds + @"') #DISCIPLINE# #REVESION#
                    union
                     #UNION#");


                var udfs = await _noteBusiness.GetUdfQuery(null, "GENERAL_DOCUMENT", null, "PROJECT_DOCUMENTS", "incomingTransmittalNumber,incomingTransmittalNumber", "issueCodes,issueCodes"
               , "stageStatus,stageStatus", "discipline,discipline", "revision,revision", "qpDueDate,qpDueDate", "incomingTransmittalDate,incomingTransmittalDate");
                var cypher2 = string.Concat($@"select n.""Id"" as DocumentId,n.""NoteNo"" as NoteNo ,n.""NoteSubject"" as DocumentName,n.""CreatedDate"" as CreatedDate,u.""Name"" as TemplateOwner,
                    LOV_revision.""Name"" as Revision,LOV_revision.""Code"" as RevisionCode,
                    LOV_discipline.""Name"" as Discipline,LOV_discipline.""Code"" as DisciplineCode,
                    coalesce(LOV_stageStatus.""Name"",LOV_stageStatus.""Code"") as StageStatus,                     
                    coalesce(LOV_code.""Name"",LOV_code.""Code"") as IssueCode,                
                    udfValue.""incomingTransmittalNumber"" as TransmittalNo,
                    udfValue.""qpDueDate"" as DueDate,
                    udfValue.""incomingTransmittalDate""::date as SubmittedDate    

from 
public.""NtsNote"" as n 
join public.""LOV"" as lv on lv.""Id""=n.""NoteStatusId""  and lv.""IsDeleted""=false and  lv.""CompanyId"" = '{_repo.UserContext.CompanyId}'
join public.""User"" as u on u.""Id""=n.""OwnerUserId"" and u.""IsDeleted""=false and  u.""CompanyId"" = '{_repo.UserContext.CompanyId}'
join public.""Template"" as tm on tm.""Id""=n.""TemplateId"" and tm.""Code""<>'PROJECT_DOCUMENTS' and tm.""IsDeleted""=false and  tm.""CompanyId"" = '{_repo.UserContext.CompanyId}'
join (" + udfs + $@") as udfValue on udfValue.""NtsNoteId""=n.""Id"" --and udfValue.""IsDeleted""=false
left join public.""LOV"" as LOV_revision on LOV_revision.""Id""=udfValue.""revision"" and LOV_revision.""IsDeleted""=false and  LOV_revision.""CompanyId"" = '{_repo.UserContext.CompanyId}'
left join public.""LOV"" as LOV_discipline on LOV_discipline.""Id""=udfValue.""discipline"" and LOV_discipline.""IsDeleted""=false and  LOV_discipline.""CompanyId"" = '{_repo.UserContext.CompanyId}'
left join public.""LOV"" as LOV_stageStatus on LOV_stageStatus.""Id""=udfValue.""stageStatus"" and LOV_stageStatus.""IsDeleted""=false and  LOV_stageStatus.""CompanyId"" = '{_repo.UserContext.CompanyId}'
left join public.""LOV"" as LOV_code on LOV_code.""Id""=udfValue.""issueCodes"" and LOV_code.""IsDeleted""=false and  LOV_code.""CompanyId"" = '{_repo.UserContext.CompanyId}'
 where 1=1 and n.""IsDeleted""=false and  n.""CompanyId"" = '{_repo.UserContext.CompanyId}' and  n.""Id"" in ('" + documentIds + @"') and lv.""Code"" <> 'NOTE_STATUS_DRAFT' #DISCIPLINE# #REVESION#
                    ");

                var searchDiscipline = "";
                if (discipline.IsNotNullAndNotEmpty())
                {
                    searchDiscipline = $@"and LOV_discipline.""Code""='{discipline}'";
                }
                cypher1 = cypher1.Replace("#UNION#", cypher2);
                var searchRevesion = "";
                if (revesion.IsNotNullAndNotEmpty())
                {
                    searchRevesion = $@"and LOV_revision.""Code""='{revesion}'";
                }
                cypher1 = cypher1.Replace("#REVESION#", searchRevesion);
                cypher1 = cypher1.Replace("#DISCIPLINE#", searchDiscipline);
                //var prms1 = new Dictionary<string, object>
                //{
                //    { "CompanyId", CompanyId },
                //    { "Status", StatusEnum.Active.ToString() },
                //    { "Discipline", discipline },
                //    { "Revesion", revesion },
                //};
                var result2 = await _queryRepo1.ExecuteQueryList<DocumentListViewModel>(cypher1, null)/*.DistinctBy(e => new { e.NoteNo, e.Revision }).ToList()*/;

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
                var cypher1 = string.Concat($@"select n.""Id"" as DocumentId, n.""NoteNo"" as NoteNo, n.""NoteSubject"" as DocumentName,
n.""CreatedDate"" as CreatedDate, --u.""Name"" as TemplateOwner,
                    LOV_revision.""Name"" as Revision,LOV_revision.""Code"" as RevisionCode,
                    LOV_discipline.""Name"" as Discipline, LOV_discipline.""Code"" as DisciplineCode,
                    coalesce(LOV_stageStatus.""Name"", LOV_stageStatus.""Code"") as StageStatus,
                    coalesce(LOV_code.""Name"", LOV_code.""Code"") as IssueCode,
                    pd.""incomingTransmittalNumber"" as TransmittalNo,
                    pd.""incomingTransmittalDate""::date as SubmittedDate
from public.""NtsNote"" as n
join cms.""N_GENERAL_DOCUMENT_ENGSUBCONTRACT"" as pd on pd.""NtsNoteId""=n.""Id"" and pd.""IsDeleted""=false and  pd.""CompanyId"" = '{_repo.UserContext.CompanyId}'
join public.""LOV"" as lv on lv.""Id""=n.""NoteStatusId"" and lv.""IsDeleted""=false and  lv.""CompanyId"" = '{_repo.UserContext.CompanyId}'
join public.""User"" as u on u.""Id""=n.""OwnerUserId"" and u.""IsDeleted""=false and  u.""CompanyId"" = '{_repo.UserContext.CompanyId}'
left join public.""LOV"" as LOV_discipline on pd.""discipline""=LOV_discipline.""Id"" and LOV_discipline.""IsDeleted""=false and  LOV_discipline.""CompanyId"" = '{_repo.UserContext.CompanyId}'
left join public.""LOV"" as LOV_revision on pd.""revision""=LOV_revision.""Id"" and LOV_revision.""IsDeleted""=false and  LOV_revision.""CompanyId"" = '{_repo.UserContext.CompanyId}'
left join public.""LOV"" as LOV_code on pd.""issueCodes""=LOV_code.""Id"" and LOV_code.""IsDeleted""=false and  LOV_code.""CompanyId"" = '{_repo.UserContext.CompanyId}'
left join public.""LOV"" as LOV_stageStatus on pd.""stageStatus""=LOV_stageStatus.""Id"" and LOV_stageStatus.""IsDeleted""=false and  LOV_stageStatus.""CompanyId"" = '{_repo.UserContext.CompanyId}'
left join public.""LOV"" as LOV_documentApprovalStatusType on pd.""documentApprovalStatusType""=LOV_documentApprovalStatusType.""Id"" and LOV_documentApprovalStatusType.""IsDeleted""=false and  LOV_documentApprovalStatusType.""CompanyId"" = '{_repo.UserContext.CompanyId}'
where n.""IsDeleted""=false and  n.""CompanyId"" = '{_repo.UserContext.CompanyId}' and lv.""Code""!='NOTE_STATUS_DRAFT' and n.""Id"" in ('" + documentIds + @"')   #DISCIPLINE#     #REVESION#    
                    ");
                var searchDiscipline = "";
                if (discipline.IsNotNullAndNotEmpty())
                {
                    searchDiscipline = $@"and LOV_discipline.""Code""='{discipline}'";
                }
                cypher1 = cypher1.Replace("#DISCIPLINE#", searchDiscipline);
                var searchRevesion = "";
                if (revesion.IsNotNullAndNotEmpty())
                {
                    searchRevesion = $@"and LOV_revision.""Code""='{revesion}'";
                }
                cypher1 = cypher1.Replace("#REVESION#", searchRevesion);
                //var prms1 = new Dictionary<string, object>
                //{
                //    { "CompanyId", CompanyId },
                //    { "Status", StatusEnum.Active.ToString() },
                //    { "Discipline", discipline },
                //    { "Revesion", revesion },
                //};
                var result2 = await _queryRepo1.ExecuteQueryList<DocumentListViewModel>(cypher1, null);
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
                var cypher1 = string.Concat($@"select n.""Id"" as DocumentId,n.""NoteNo"" as NoteNo ,n.""NoteSubject"" as DocumentName,
n.""CreatedDate"" as CreatedDate,u.""Name"" as TemplateOwner,
                    LOV_revision.""Name"" as Revision,LOV_revision.""Code"" as RevisionCode,
                    LOV_discipline.""Name"" as Discipline,LOV_discipline.""Code"" as DisciplineCode,
                    coalesce(LOV_stageStatus.""Name"",LOV_stageStatus.""Code"") as StageStatus,                     
                    coalesce(LOV_OutgoingIssueCodes.""Name"",LOV_OutgoingIssueCodes.""Code"") as IssueCode,                    
                    pd.""galfarTransmittalNumber"" as TransmittalNo,
                    pd.""qpDueDate"" as DueDate,
                    pd.""dateOfSubmission""::date as SubmittedDate
from public.""NtsNote"" as n
join cms.""N_GENERAL_DOCUMENT_ProjectDocuments"" as pd on pd.""NtsNoteId""=n.""Id"" and pd.""IsDeleted""=false and  pd.""CompanyId"" = '{_repo.UserContext.CompanyId}'
join public.""LOV"" as lv on lv.""Id""=n.""NoteStatusId"" and lv.""IsDeleted""=false and  lv.""CompanyId"" = '{_repo.UserContext.CompanyId}'
left join public.""User"" as u on u.""Id""=n.""OwnerUserId"" and u.""IsDeleted""=false and  u.""CompanyId"" = '{_repo.UserContext.CompanyId}'
left join public.""LOV"" as LOV_projectFolder on pd.""projectFolder""=LOV_projectFolder.""Id"" and LOV_projectFolder.""IsDeleted""=false and  LOV_projectFolder.""CompanyId"" = '{_repo.UserContext.CompanyId}'
left join public.""LOV"" as LOV_projectSubFolder on pd.""projectSubFolder""=LOV_projectSubFolder.""Id"" and LOV_projectSubFolder.""IsDeleted""=false and  LOV_projectSubFolder.""CompanyId"" = '{_repo.UserContext.CompanyId}'
left join public.""LOV"" as LOV_discipline on pd.""discipline""=LOV_discipline.""Id"" and LOV_discipline.""IsDeleted""=false and  LOV_discipline.""CompanyId"" = '{_repo.UserContext.CompanyId}'
left join public.""LOV"" as LOV_revision on pd.""revision""=LOV_revision.""Id"" and LOV_revision.""IsDeleted""=false and  LOV_revision.""CompanyId"" = '{_repo.UserContext.CompanyId}'
left join public.""LOV"" as LOV_OutgoingIssueCodes on pd.""outgoingIssueCodes""=LOV_OutgoingIssueCodes.""Id"" and LOV_OutgoingIssueCodes.""IsDeleted""=false and  LOV_OutgoingIssueCodes.""CompanyId"" = '{_repo.UserContext.CompanyId}'
left join public.""LOV"" as LOV_code on pd.""code""=LOV_code.""Id"" and LOV_code.""IsDeleted""=false and  LOV_code.""CompanyId"" = '{_repo.UserContext.CompanyId}'
left join public.""LOV"" as LOV_stageStatus on pd.""stageStatus""=LOV_stageStatus.""Id"" and LOV_stageStatus.""IsDeleted""=false and  LOV_stageStatus.""CompanyId"" = '{_repo.UserContext.CompanyId}'
left join public.""LOV"" as LOV_documentApprovalStatusType on pd.""documentApprovalStatusType""=LOV_documentApprovalStatusType.""Id"" and LOV_documentApprovalStatusType.""IsDeleted""=false and  LOV_documentApprovalStatusType.""CompanyId"" = '{_repo.UserContext.CompanyId}'
where n.""IsDeleted"" and  n.""CompanyId"" = '{_repo.UserContext.CompanyId}' and lv.""Code""!='NOTE_STATUS_DRAFT' and n.""Id"" in ('" + documentIds + @"')  #DISCIPLINE#     #REVESION#    
                    ");

                var searchDiscipline = "";
                if (discipline.IsNotNullAndNotEmpty())
                {
                    searchDiscipline = $@"and LOV_discipline.""Code""='{discipline}'";
                }
                cypher1 = cypher1.Replace("#DISCIPLINE#", searchDiscipline);
                var searchRevesion = "";
                if (revesion.IsNotNullAndNotEmpty())
                {
                    searchRevesion = $@"and LOV_revision.""Code""='{revesion}'";
                }
                cypher1 = cypher1.Replace("#REVESION#", searchRevesion);
                //var prms1 = new Dictionary<string, object>
                //{
                //    { "CompanyId", CompanyId },
                //    { "Status", StatusEnum.Active.ToString() },
                //    { "Discipline", discipline },
                //    { "Revesion", revesion },
                //};
                var result2 = await _queryRepo1.ExecuteQueryList<DocumentListViewModel>(cypher1, null);
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
                var cypher1 = string.Concat($@"select n.""Id"" as DocumentId,n.""NoteNo"" as NoteNo ,n.""NoteSubject"" as DocumentName,
n.""CreatedDate"" as CreatedDate,u.""Name"" as TemplateOwner,
                    LOV_revision.""Name"" as Revision,LOV_revision.""Code"" as RevisionCode,
                    LOV_discipline.""Name"" as Discipline,LOV_discipline.""Code"" as DisciplineCode,
                    coalesce(LOV_stageStatus.""Name"",LOV_stageStatus.""Code"") as StageStatus,                     
                    coalesce(LOV_code.""Name"",LOV_code.""Code"") as IssueCode,                    
                    pd.""incomingTransmittalNumber"" as TransmittalNo,
                    pd.""qpDueDate"" as DueDate,
                    pd.""incomingTransmittalDate""::date as SubmittedDate,
					LOV_vendorList.""Name"" as Vendor
from public.""NtsNote"" as n
join cms.""N_GENERAL_DOCUMENT_GALFARVENDOR"" as pd on pd.""NtsNoteId""=n.""Id"" and pd.""IsDeleted""=false and  pd.""CompanyId"" = '{_repo.UserContext.CompanyId}'
join public.""LOV"" as lv on lv.""Id""=n.""NoteStatusId"" and lv.""IsDeleted""=false and  lv.""CompanyId"" = '{_repo.UserContext.CompanyId}'
join public.""User"" as u on u.""Id""=n.""OwnerUserId"" and u.""IsDeleted""=false and  u.""CompanyId"" = '{_repo.UserContext.CompanyId}'
left join public.""LOV"" as LOV_discipline on pd.""discipline""=LOV_discipline.""Id"" and LOV_discipline.""IsDeleted""=false and  LOV_discipline.""CompanyId"" = '{_repo.UserContext.CompanyId}'
left join public.""LOV"" as LOV_revision on pd.""revision""=LOV_revision.""Id"" and LOV_revision.""IsDeleted""=false and  LOV_revision.""CompanyId"" = '{_repo.UserContext.CompanyId}'
left join public.""LOV"" as LOV_code on pd.""code""=LOV_code.""Id"" and LOV_code.""IsDeleted""=false and  LOV_code.""CompanyId"" = '{_repo.UserContext.CompanyId}'
left join public.""LOV"" as LOV_vendorList on pd.""vendorList""=LOV_vendorList.""Id"" and LOV_vendorList.""IsDeleted""=false and  LOV_vendorList.""CompanyId"" = '{_repo.UserContext.CompanyId}'
left join public.""LOV"" as LOV_stageStatus on pd.""stageStatus""=LOV_stageStatus.""Id"" and LOV_stageStatus.""IsDeleted""=false and  LOV_stageStatus.""CompanyId"" = '{_repo.UserContext.CompanyId}'
left join public.""LOV"" as LOV_documentApprovalStatusType on pd.""documentApprovalStatusType""=LOV_documentApprovalStatusType.""Id"" and LOV_documentApprovalStatusType.""IsDeleted""=false and  LOV_documentApprovalStatusType.""CompanyId"" = '{_repo.UserContext.CompanyId}'
where n.""IsDeleted""=false and  n.""CompanyId"" = '{_repo.UserContext.CompanyId}' and lv.""Code""!='NOTE_STATUS_DRAFT' and n.""Id"" in ('" + documentIds + @"') #DISCIPLINE# #REVESION#
                    ");
                var searchDiscipline = "";
                if (discipline.IsNotNullAndNotEmpty())
                {
                    searchDiscipline = $@"and LOV_discipline.""Code""='{discipline}'";
                }
                cypher1 = cypher1.Replace("#DISCIPLINE#", searchDiscipline);
                var searchRevesion = "";
                if (revesion.IsNotNullAndNotEmpty())
                {
                    searchRevesion = $@"and LOV_revision.""Code""='{revesion}'";
                }
                cypher1 = cypher1.Replace("#REVESION#", searchRevesion);
                //var prms1 = new Dictionary<string, object>
                //{
                //    { "CompanyId", CompanyId },
                //    { "Status", StatusEnum.Active.ToString() },
                //    { "Discipline", discipline },
                //    { "Revesion", revesion },
                //};
                var result2 = await _queryRepo1.ExecuteQueryList<DocumentListViewModel>(cypher1, null);
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
                var cypher1 = string.Concat(@$"select n.""Id"" as DocumentId,n.""NoteNo"" as NoteNo ,n.""NoteSubject"" as DocumentName,
n.""CreatedDate"" as CreatedDate,u.""Name"" as TemplateOwner,
                    LOV_revision.""Name"" as Revision,LOV_revision.""Code"" as RevisionCode,
                    LOV_discipline.""Name"" as Discipline,LOV_discipline.""Code"" as DisciplineCode,
                    coalesce(LOV_stageStatus.""Name"",LOV_stageStatus.""Code"") as StageStatus,                     
                    coalesce(LOV_code.""Name"",LOV_code.""Code"") as IssueCode,                    
                    pd.""incomingTransmittalNumber"" as TransmittalNo,
                    pd.""qpDueDate"" as DueDate,
                    pd.""incomingTransmittalDate""::date as SubmittedDate,
					LOV_vendorList.""Name"" as Vendor
from public.""NtsNote"" as n
join cms.""N_GENERAL_DOCUMENT_GALFARVENDOR"" as pd on pd.""NtsNoteId""=n.""Id"" and pd.""IsDeleted""=false and  pd.""CompanyId"" = '{_repo.UserContext.CompanyId}'
join public.""LOV"" as lv on lv.""Id""=n.""NoteStatusId"" and lv.""IsDeleted""=false and  lv.""CompanyId"" = '{_repo.UserContext.CompanyId}'
join public.""User"" as u on u.""Id""=n.""OwnerUserId"" and u.""IsDeleted""=false and  u.""CompanyId"" = '{_repo.UserContext.CompanyId}'
left join public.""LOV"" as LOV_discipline on pd.""discipline""=LOV_discipline.""Id"" and LOV_discipline.""IsDeleted""=false and  LOV_discipline.""CompanyId"" = '{_repo.UserContext.CompanyId}'
left join public.""LOV"" as LOV_revision on pd.""revision""=LOV_revision.""Id"" and LOV_revision.""IsDeleted""=false and  LOV_revision.""CompanyId"" = '{_repo.UserContext.CompanyId}'
left join public.""LOV"" as LOV_code on pd.""code""=LOV_code.""Id"" and LOV_code.""IsDeleted""=false and  LOV_code.""CompanyId"" = '{_repo.UserContext.CompanyId}'
left join public.""LOV"" as LOV_vendorList on pd.""vendorList""=LOV_vendorList.""Id"" and LOV_vendorList.""IsDeleted""=false and  LOV_vendorList.""CompanyId"" = '{_repo.UserContext.CompanyId}'
left join public.""LOV"" as LOV_stageStatus on pd.""stageStatus""=LOV_stageStatus.""Id"" and LOV_stageStatus.""IsDeleted""=false and  LOV_stageStatus.""CompanyId"" = '{_repo.UserContext.CompanyId}'
left join public.""LOV"" as LOV_documentApprovalStatusType on pd.""documentApprovalStatusType""=LOV_documentApprovalStatusType.""Id"" and LOV_documentApprovalStatusType.""IsDeleted""=false and  LOV_documentApprovalStatusType.""CompanyId"" = '{_repo.UserContext.CompanyId}'
where n.""IsDeleted"" and  n.""CompanyId"" = '{_repo.UserContext.CompanyId}' and lv.""Code""!='NOTE_STATUS_DRAFT' and n.""Id"" in ('" + documentIds + $@"') #DISCIPLINE# #REVESION#

union

select n.""Id"" as DocumentId,n.""NoteNo"" as NoteNo ,n.""NoteSubject"" as DocumentName,
n.""CreatedDate"" as CreatedDate,u.""Name"" as TemplateOwner,
                    LOV_revision.""Name"" as Revision,LOV_revision.""Code"" as RevisionCode,
                    LOV_discipline.""Name"" as Discipline,LOV_discipline.""Code"" as DisciplineCode,
                    coalesce(LOV_stageStatus.""Name"",LOV_stageStatus.""Code"") as StageStatus,                     
                    coalesce(LOV_OutgoingIssueCodes.""Name"",LOV_OutgoingIssueCodes.""Code"") as IssueCode,                    
                    pd.""galfarTransmittalNumber"" as TransmittalNo,
                    pd.""qpDueDate"" as DueDate,
                    pd.""dateOfSubmission""::date as SubmittedDate,
					null as Vendor
from public.""NtsNote"" as n
join cms.""N_GENERAL_DOCUMENT_ProjectDocuments"" as pd on pd.""NtsNoteId""=n.""Id"" and pd.""IsDeleted""=false and  pd.""CompanyId"" = '{_repo.UserContext.CompanyId}'
join public.""LOV"" as lv on lv.""Id""=n.""NoteStatusId"" and lv.""IsDeleted""=false and  lv.""CompanyId"" = '{_repo.UserContext.CompanyId}'
left join public.""User"" as u on u.""Id""=n.""OwnerUserId"" and u.""IsDeleted""=false and  u.""CompanyId"" = '{_repo.UserContext.CompanyId}'
left join public.""LOV"" as LOV_projectFolder on pd.""projectFolder""=LOV_projectFolder.""Id"" and LOV_projectFolder.""IsDeleted""=false and  LOV_projectFolder.""CompanyId"" = '{_repo.UserContext.CompanyId}'
left join public.""LOV"" as LOV_projectSubFolder on pd.""projectSubFolder""=LOV_projectSubFolder.""Id"" and LOV_projectSubFolder.""IsDeleted""=false and  LOV_projectSubFolder.""CompanyId"" = '{_repo.UserContext.CompanyId}'
left join public.""LOV"" as LOV_discipline on pd.""discipline""=LOV_discipline.""Id"" and LOV_discipline.""IsDeleted""=false and  LOV_discipline.""CompanyId"" = '{_repo.UserContext.CompanyId}'
left join public.""LOV"" as LOV_revision on pd.""revision""=LOV_revision.""Id"" and LOV_revision.""IsDeleted""=false and  LOV_revision.""CompanyId"" = '{_repo.UserContext.CompanyId}'
left join public.""LOV"" as LOV_OutgoingIssueCodes on pd.""outgoingIssueCodes""=LOV_OutgoingIssueCodes.""Id"" and LOV_OutgoingIssueCodes.""IsDeleted""=false and  LOV_OutgoingIssueCodes.""CompanyId"" = '{_repo.UserContext.CompanyId}'
left join public.""LOV"" as LOV_code on pd.""code""=LOV_code.""Id"" and LOV_code.""IsDeleted""=false and  LOV_code.""CompanyId"" = '{_repo.UserContext.CompanyId}'
left join public.""LOV"" as LOV_stageStatus on pd.""stageStatus""=LOV_stageStatus.""Id"" and LOV_stageStatus.""IsDeleted""=false and  LOV_stageStatus.""CompanyId"" = '{_repo.UserContext.CompanyId}'
left join public.""LOV"" as LOV_documentApprovalStatusType on pd.""documentApprovalStatusType""=LOV_documentApprovalStatusType.""Id"" and LOV_documentApprovalStatusType.""IsDeleted""=false and  LOV_documentApprovalStatusType.""CompanyId"" = '{_repo.UserContext.CompanyId}'
where n.""IsDeleted""=false and  n.""CompanyId"" = '{_repo.UserContext.CompanyId}' and lv.""Code""!='NOTE_STATUS_DRAFT' and n.""Id"" in ('" + documentIds + $@"') #DISCIPLINE# #REVESION#

union

select n.""Id"" as DocumentId,n.""NoteNo"" as NoteNo ,n.""NoteSubject"" as DocumentName,
n.""CreatedDate"" as CreatedDate,u.""Name"" as TemplateOwner,
                    LOV_revision.""Name"" as Revision,LOV_revision.""Code"" as RevisionCode,
                    LOV_discipline.""Name"" as Discipline,LOV_discipline.""Code"" as DisciplineCode,
                    coalesce(LOV_stageStatus.""Name"",LOV_stageStatus.""Code"") as StageStatus,                     
                    coalesce(LOV_code.""Name"",LOV_code.""Code"") as IssueCode,                    
                    pd.""incomingTransmittalNumber"" as TransmittalNo,
                    null as DueDate,
                    pd.""incomingTransmittalDate""::date as SubmittedDate,
					null as Vendor
from public.""NtsNote"" as n
join cms.""N_GENERAL_DOCUMENT_ENGSUBCONTRACT"" as pd on pd.""NtsNoteId""=n.""Id"" and pd.""IsDeleted""=false and  pd.""CompanyId"" = '{_repo.UserContext.CompanyId}'
join public.""LOV"" as lv on lv.""Id""=n.""NoteStatusId"" and lv.""IsDeleted""=false and  lv.""CompanyId"" = '{_repo.UserContext.CompanyId}'
join public.""User"" as u on u.""Id""=n.""OwnerUserId"" and u.""IsDeleted""=false and  u.""CompanyId"" = '{_repo.UserContext.CompanyId}'
left join public.""LOV"" as LOV_discipline on pd.""discipline""=LOV_discipline.""Id"" and LOV_discipline.""IsDeleted""=false and  LOV_discipline.""CompanyId"" = '{_repo.UserContext.CompanyId}'
left join public.""LOV"" as LOV_revision on pd.""revision""=LOV_revision.""Id"" and LOV_revision.""IsDeleted""=false and  LOV_revision.""CompanyId"" = '{_repo.UserContext.CompanyId}'
left join public.""LOV"" as LOV_code on pd.""issueCodes""=LOV_code.""Id"" and LOV_code.""IsDeleted""=false and  LOV_code.""CompanyId"" = '{_repo.UserContext.CompanyId}'
left join public.""LOV"" as LOV_stageStatus on pd.""stageStatus""=LOV_stageStatus.""Id"" and LOV_stageStatus.""IsDeleted""=false and  LOV_stageStatus.""CompanyId"" = '{_repo.UserContext.CompanyId}'
left join public.""LOV"" as LOV_documentApprovalStatusType on pd.""documentApprovalStatusType""=LOV_documentApprovalStatusType.""Id"" and LOV_documentApprovalStatusType.""IsDeleted""=false and  n.""CompanyId"" = '{_repo.UserContext.CompanyId}'
where n.""IsDeleted""=false and  n.""CompanyId"" = '{_repo.UserContext.CompanyId}' and lv.""Code""!='NOTE_STATUS_DRAFT' and n.""Id"" in ('" + documentIds + @"') #DISCIPLINE# #REVESION# ");

                var searchDiscipline = "";
                if (discipline.IsNotNullAndNotEmpty())
                {
                    searchDiscipline = $@"and LOV_discipline.""Code""='{discipline}'";
                }
                cypher1 = cypher1.Replace("#DISCIPLINE#", searchDiscipline);
                var searchRevesion = "";
                if (revesion.IsNotNullAndNotEmpty())
                {
                    searchRevesion = $@"and LOV_revision.""Code""='{revesion}'";
                }
                cypher1 = cypher1.Replace("#REVESION#", searchRevesion);
                //var prms1 = new Dictionary<string, object>
                //{
                //    { "CompanyId", CompanyId },
                //    { "Status", StatusEnum.Active.ToString() },
                //    { "Discipline", discipline },
                //    { "Revesion", revesion },
                //};
                var result2 = await _queryRepo1.ExecuteQueryList<DocumentListViewModel>(cypher1, null);
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
            var selectQry = "";
            var i = 1;
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

                foreach (var item in templateList.Where(x => x.TableMetadataId != null))
                {
                    var tableMeta = await _tableMetadataBusiness.GetSingle(x => x.Id == item.TableMetadataId);
                    if (item.Code == "GENERAL_DOCUMENT" || item.Code == "HALUL_REQUEST_FOR_INSPECTION")
                    {

                    }

                    else
                    {
                        if (i != 1)
                        {
                            selectQry += " union ";
                        }
                        if (item.Code == "GALFAR_REQUEST_FOR_INSPECTION")
                        {
                            selectQry = @$"{ selectQry}
                            select n.""Id"" as DocumentId,n.""NoteNo"" as NoteNo,n.""NoteSubject"" as DocumentName,n.""CreatedDate"",r.""Name"" as Revision,d.""Name"" as Discipline,
                            ss.""Name"" as StageStatus,c.""Name"" as IssueCode,null as TransmittalNo,null as DueDate,ou.""Name"" as TemplateOwner,
                            p.""dateOfSubmission"" as SubmittedDate
                            FROM public.""NtsNote"" n
                            JOIN public.""Template"" as t ON n.""TemplateId"" = t.""Id"" AND t.""IsDeleted"" = false and  t.""CompanyId"" = '{_repo.UserContext.CompanyId}' --AND t.""Code"" = 'PROJECT_DOCUMENTS' 
                            JOIN cms.""{tableMeta.Name}"" as p ON n.""Id"" = p.""NtsNoteId"" and p.""IsDeleted"" = false and  p.""CompanyId"" = '{_repo.UserContext.CompanyId}' #OVERDUE#
                            Join public.""LOV"" as r on p.""revision""=r.""Id"" AND r.""IsDeleted"" = false and  r.""CompanyId"" = '{_repo.UserContext.CompanyId}'
	                        Join public.""LOV"" as d on p.""discipline""=d.""Id"" AND d.""IsDeleted"" = false and  d.""CompanyId"" = '{_repo.UserContext.CompanyId}' #DISCIPLINE#
	                        Join public.""LOV"" as ss on p.""stageStatus""=ss.""Id"" and ss.""Code""='QP_STATUS' and ss.""IsDeleted""=false and  ss.""CompanyId"" = '{_repo.UserContext.CompanyId}'
	                        left Join public.""LOV"" as c on p.""OutgoingIssueCodes""=c.""Id"" and c.""IsDeleted""=false and  c.""CompanyId"" = '{_repo.UserContext.CompanyId}'
                            LEFT JOIN public.""DocumentPermission"" as dp ON n.""Id"" = dp.""NoteId"" AND dp.""IsDeleted"" = false and  dp.""CompanyId"" = '{_repo.UserContext.CompanyId}'
                            left Join public.""User"" as ou on n.""OwnerUserId""=ou.""Id"" and ou.""Id""='{userId}' and ou.""IsDeleted"" = false and  ou.""CompanyId"" = '{_repo.UserContext.CompanyId}'	                       
                            left Join public.""User"" as u on dp.""PermittedUserId""=u.""Id"" and u.""Id""='{userId}' AND u.""IsDeleted"" = false and  u.""CompanyId"" = '{_repo.UserContext.CompanyId}'
                            LEFT JOIN public.""DocumentPermission"" as dup ON n.""Id"" = dup.""NoteId"" AND dup.""IsDeleted"" = false and  dup.""CompanyId"" = '{_repo.UserContext.CompanyId}'
                            LEFT JOIN public.""UserGroupUser"" as uu ON dup.""PermittedUserGroupId"" = uu.""UserGroupId"" and uu.""Id""='{userId}' AND uu.""IsDeleted"" = false and  uu.""CompanyId"" = '{_repo.UserContext.CompanyId}'
                            left join public.""NtsNote"" as dn on dn.""Id""=dup.""NoteId"" or dn.""Id""=dp.""NoteId"" AND dn.""IsDeleted"" = false and  dn.""CompanyId"" = '{_repo.UserContext.CompanyId}'
                            left join cms.""N_GENERAL_FOLDER_WORKSPACE"" as ws on ws.""NtsNoteId""=dn.""Id"" AND ws.""IsDeleted"" = false and  ws.""CompanyId"" = '{_repo.UserContext.CompanyId}'
                            where n.""IsDeleted""=false and  n.""CompanyId"" = '{_repo.UserContext.CompanyId}'";
                        }
                        else if (item.Code == "PROJECT_DOCUMENTS")
                        {
                            selectQry = @$"{ selectQry}
                            select n.""Id"" as DocumentId,n.""NoteNo"" as NoteNo,n.""NoteSubject"" as DocumentName,n.""CreatedDate"",r.""Name"" as Revision,d.""Name"" as Discipline,
                            ss.""Name"" as StageStatus,c.""Name"" as IssueCode,p.""galfarTransmittalNumber"" as TransmittalNo,p.""qpDueDate"" as DueDate,
                          ou.""Name"" as TemplateOwner,  p.""dateOfSubmission"" as SubmittedDate
                            FROM public.""NtsNote"" n
                            JOIN public.""Template"" t ON n.""TemplateId"" = t.""Id"" AND t.""IsDeleted"" = false and  t.""CompanyId"" = '{_repo.UserContext.CompanyId}'--AND t.""Code"" = 'PROJECT_DOCUMENTS' 
                            JOIN cms.""{tableMeta.Name}"" p ON n.""Id"" = p.""NtsNoteId"" and p.""IsDeleted"" = false and  p.""CompanyId"" = '{_repo.UserContext.CompanyId}' #OVERDUE#
                            Join public.""LOV"" as r on p.""revision""=r.""Id"" AND r.""IsDeleted"" = false and  r.""CompanyId"" = '{_repo.UserContext.CompanyId}'
	                        Join public.""LOV"" as d on p.""discipline""=d.""Id"" AND d.""IsDeleted"" = false and  d.""CompanyId"" = '{_repo.UserContext.CompanyId}' #DISCIPLINE#
	                        Join public.""LOV"" as ss on p.""stageStatus""=ss.""Id"" and ss.""Code""='QP_STATUS' and ss.""IsDeleted""=false and  ss.""CompanyId"" = '{_repo.UserContext.CompanyId}'
	                        left Join public.""LOV"" as c on p.""outgoingIssueCodes""=c.""Id"" and c.""IsDeleted""=false and  c.""CompanyId"" = '{_repo.UserContext.CompanyId}'
                            LEFT JOIN public.""DocumentPermission"" as dp ON n.""Id"" = dp.""NoteId"" AND dp.""IsDeleted"" = false and  dp.""CompanyId"" = '{_repo.UserContext.CompanyId}'
	                       left Join public.""User"" as ou on n.""OwnerUserId""=ou.""Id"" and ou.""Id""='{userId}' and ou.""IsDeleted"" = false and  ou.""CompanyId"" = '{_repo.UserContext.CompanyId}'
                            left Join public.""User"" as u on dp.""PermittedUserId""=u.""Id"" and u.""Id""='{userId}' AND u.""IsDeleted"" = false and  u.""CompanyId"" = '{_repo.UserContext.CompanyId}'
                            LEFT JOIN public.""DocumentPermission"" as dup ON n.""Id"" = dup.""NoteId"" AND dup.""IsDeleted"" = false and  dup.""CompanyId"" = '{_repo.UserContext.CompanyId}'
                            LEFT JOIN public.""UserGroupUser"" uu ON dup.""PermittedUserGroupId"" = uu.""UserGroupId"" and uu.""Id""='{userId}' AND uu.""IsDeleted"" = false and  uu.""CompanyId"" = '{_repo.UserContext.CompanyId}'
                            left join public.""NtsNote"" as dn on dn.""Id""=dup.""NoteId"" or dn.""Id""=dp.""NoteId"" AND dn.""IsDeleted"" = false and  dn.""CompanyId"" = '{_repo.UserContext.CompanyId}'
                            left join cms.""N_GENERAL_FOLDER_WORKSPACE"" as ws on ws.""NtsNoteId""=dn.""Id"" AND ws.""IsDeleted"" = false and  ws.""CompanyId"" = '{_repo.UserContext.CompanyId}'
                            where  n.""IsDeleted"" = false and  n.""CompanyId"" = '{_repo.UserContext.CompanyId}' ";
                        }
                        else
                        {
                            selectQry = @$"{ selectQry}
                            select n.""Id"" as DocumentId,n.""NoteNo"" as NoteNo,n.""NoteSubject"" as DocumentName,n.""CreatedDate"",r.""Name"" as Revision,d.""Name"" as Discipline,
                            ss.""Name"" as StageStatus,c.""Name"" as IssueCode,p.""galfarTransmittalNumber"" as TransmittalNo,p.""qpDueDate"" as DueDate,
                            p.""dateOfSubmission"" as SubmittedDate,ou.""Name"" as TemplateOwner
                            FROM public.""NtsNote"" n
                            JOIN public.""Template"" t ON n.""TemplateId"" = t.""Id"" AND t.""IsDeleted"" = false and t.""CompanyId"" = '{_repo.UserContext.CompanyId}' --AND t.""Code"" = 'PROJECT_DOCUMENTS' 
                            JOIN cms.""{tableMeta.Name}"" p ON n.""Id"" = p.""NtsNoteId"" and p.""IsDeleted"" = false and  p.""CompanyId"" = '{_repo.UserContext.CompanyId}' #OVERDUE#
                            Join public.""LOV"" as r on p.""revision""=r.""Id"" AND r.""IsDeleted"" = false and  r.""CompanyId"" = '{_repo.UserContext.CompanyId}'
	                        Join public.""LOV"" as d on p.""discipline""=d.""Id"" AND d.""IsDeleted"" = false and  d.""CompanyId"" = '{_repo.UserContext.CompanyId}' #DISCIPLINE#
	                        Join public.""LOV"" as ss on p.""stageStatus""=ss.""Id"" and ss.""Code""='QP_STATUS' and ss.""IsDeleted""=false and  ss.""CompanyId"" = '{_repo.UserContext.CompanyId}'
	                        left Join public.""LOV"" as c on p.""outgoingIssueCodes""=c.""Id"" and c.""IsDeleted""=false and  c.""CompanyId"" = '{_repo.UserContext.CompanyId}'
                            LEFT JOIN public.""DocumentPermission"" as dp ON n.""Id"" = dp.""NoteId"" AND dp.""IsDeleted"" = false and  dp.""CompanyId"" = '{_repo.UserContext.CompanyId}'
                            left Join public.""User"" as ou on n.""OwnerUserId""=ou.""Id"" and ou.""Id""='{userId}' and ou.""IsDeleted"" = false and  ou.""CompanyId"" = '{_repo.UserContext.CompanyId}'	                       
                            left Join public.""User"" as u on dp.""PermittedUserId""=u.""Id"" and u.""Id""='{userId}' AND u.""IsDeleted"" = false  and  u.""CompanyId"" = '{_repo.UserContext.CompanyId}'
                            LEFT JOIN public.""DocumentPermission"" as dup ON n.""Id"" = dup.""NoteId"" AND dup.""IsDeleted"" = false  and  dup.""CompanyId"" = '{_repo.UserContext.CompanyId}'
                            LEFT JOIN public.""UserGroupUser"" uu ON dup.""PermittedUserGroupId"" = uu.""UserGroupId"" and uu.""Id""='{userId}' AND uu.""IsDeleted"" = false  and  uu.""CompanyId"" = '{_repo.UserContext.CompanyId}'
                            left join public.""NtsNote"" as dn on dn.""Id""=dup.""NoteId"" or dn.""Id""=dp.""NoteId"" AND dn.""IsDeleted"" = false  and  dn.""CompanyId"" = '{_repo.UserContext.CompanyId}'
                            left join cms.""N_GENERAL_FOLDER_WORKSPACE"" as ws on ws.""NtsNoteId""=dn.""Id"" AND ws.""IsDeleted"" = false  and  ws.""CompanyId"" = '{_repo.UserContext.CompanyId}'
                            where n.""IsDeleted""=false  and  n.""CompanyId"" = '{_repo.UserContext.CompanyId}' ";
                        }


                        i++;
                    }
                }

                var searchDiscipline = "";
                if (discipline.IsNotNullAndNotEmpty())
                {
                    searchDiscipline = $@" and d.""Code""='{discipline}'";
                }
                selectQry = selectQry.Replace("#DISCIPLINE#", searchDiscipline);

                var searchOverdue = "";
                if (IsOverdue == true)
                {
                    searchOverdue = $@" and p.""QpDueDate""::TIMESTAMP::DATE < '{DateTime.Now.Date}'::TIMESTAMP::DATE ";
                }
                selectQry = selectQry.Replace("#OVERDUE#", searchOverdue);

                var result2 = await _querydocList.ExecuteQueryList(selectQry, null);

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

                foreach (var item in templateList.Where(x => x.TableMetadataId != null))
                {
                    var tableMeta = await _tableMetadataBusiness.GetSingle(x => x.Id == item.TableMetadataId);
                    if (item.Code == "GENERAL_DOCUMENT" || item.Code == "HALUL_REQUEST_FOR_INSPECTION")
                    {

                    }
                    else
                    {
                        if (i != 1)
                        {
                            selectQry += " union ";
                        }

                        if (item.Code == "PROJECT_DOCUMENTS")
                        {
                            selectQry = @$"{ selectQry}
                            select n.""Id"" as DocumentId,n.""NoteNo"" as NoteNo,n.""NoteSubject"" as DocumentName,n.""CreatedDate"",r.""Name"" as Revision,d.""Name"" as Discipline,
                            ss.""Name"" as StageStatus,c.""Name"" as IssueCode,null as TransmittalNo,null as DueDate,ou.""Name"" as TemplateOwner,
                            null as SubmittedDate
     FROM public.""NtsNote"" n
     JOIN public.""Template"" t ON n.""TemplateId"" = t.""Id""  AND t.""IsDeleted"" = false and  t.""CompanyId"" = '{_repo.UserContext.CompanyId}' --AND t.""Code"" = 'PROJECT_DOCUMENTS'
     JOIN cms.""{tableMeta.Name}"" p ON n.""Id"" = p.""NtsNoteId"" and p.""IsDeleted"" = false and  p.""CompanyId"" = '{_repo.UserContext.CompanyId}' #OVERDUE#
      Join public.""LOV"" as r on p.""revision""=r.""Id"" and r.""IsDeleted"" = false and  r.""CompanyId"" = '{_repo.UserContext.CompanyId}'
	  Join public.""LOV"" as d on p.""discipline""=d.""Id"" and d.""IsDeleted"" = false and  d.""CompanyId"" = '{_repo.UserContext.CompanyId}' #DISCIPLINE#
	  Join public.""LOV"" as ss on p.""stageStatus""=ss.""Id"" and ss.""Code""='TECHNIP_STATUS' and ss.""IsDeleted""=false and  ss.""CompanyId"" = '{_repo.UserContext.CompanyId}'
      left Join public.""LOV"" as c on p.""outgoingTechnipIssueCodes""=c.""Id"" and c.""IsDeleted""=false and  c.""CompanyId"" = '{_repo.UserContext.CompanyId}'
     LEFT JOIN public.""DocumentPermission"" as dp ON n.""Id"" = dp.""NoteId"" and dp.""IsDeleted"" = false and  dp.""CompanyId"" = '{_repo.UserContext.CompanyId}'
left Join public.""User"" as ou on n.""OwnerUserId""=ou.""Id"" and ou.""Id""='{userId}' and ou.""IsDeleted"" = false and  ou.""CompanyId"" = '{_repo.UserContext.CompanyId}'	
left Join public.""User"" as u on dp.""PermittedUserId""=u.""Id"" and u.""Id""='{userId}' and u.""IsDeleted"" = false and  u.""CompanyId"" = '{_repo.UserContext.CompanyId}'
     LEFT JOIN public.""DocumentPermission"" as dup ON n.""Id"" = dup.""NoteId"" and dup.""IsDeleted"" = false and  dup.""CompanyId"" = '{_repo.UserContext.CompanyId}'
     LEFT JOIN public.""UserGroupUser"" uu ON dup.""PermittedUserGroupId"" = uu.""UserGroupId"" and uu.""Id""='{userId}' and uu.""IsDeleted"" = false and  uu.""CompanyId"" = '{_repo.UserContext.CompanyId}'
     left join public.""NtsNote"" as dn on dn.""Id""=dup.""NoteId"" or dn.""Id""=dp.""NoteId"" and dn.""IsDeleted"" = false and  dn.""CompanyId"" = '{_repo.UserContext.CompanyId}'
     left join cms.""N_GENERAL_FOLDER_WORKSPACE"" as ws on ws.""NtsNoteId""=dn.""Id"" and ws.""IsDeleted"" = false and  ws.""CompanyId"" = '{_repo.UserContext.CompanyId}'
     where n.""IsDeleted""=false and  n.""CompanyId"" = '{_repo.UserContext.CompanyId}' ";

                        }
                        else if (item.Code == "GALFAR_REQUEST_FOR_INSPECTION")
                        {
                            selectQry = @$"{ selectQry}
                            select n.""Id"" as DocumentId,n.""NoteNo"" as NoteNo,n.""NoteSubject"" as DocumentName,n.""CreatedDate"",r.""Name"" as Revision,d.""Name"" as Discipline,
                            ss.""Name"" as StageStatus,null as IssueCode,null as TransmittalNo,null as DueDate,ou.""Name"" as TemplateOwner,
                            null as SubmittedDate
     FROM public.""NtsNote"" n
     JOIN public.""Template"" t ON n.""TemplateId"" = t.""Id""  AND t.""IsDeleted"" = false and  t.""CompanyId"" = '{_repo.UserContext.CompanyId}' --AND t.""Code"" = 'PROJECT_DOCUMENTS'
     JOIN cms.""{tableMeta.Name}"" p ON n.""Id"" = p.""NtsNoteId"" and p.""IsDeleted"" = false and  p.""CompanyId"" = '{_repo.UserContext.CompanyId}' #OVERDUE#
      Join public.""LOV"" as r on p.""revision""=r.""Id"" and r.""IsDeleted"" = false and  r.""CompanyId"" = '{_repo.UserContext.CompanyId}'
	  Join public.""LOV"" as d on p.""discipline""=d.""Id"" and d.""IsDeleted"" = false and  d.""CompanyId"" = '{_repo.UserContext.CompanyId}' #DISCIPLINE#
	  Join public.""LOV"" as ss on p.""stageStatus""=ss.""Id"" and ss.""Code""='TECHNIP_STATUS' and ss.""IsDeleted""=false and  ss.""CompanyId"" = '{_repo.UserContext.CompanyId}'
      --left Join public.""LOV"" as c on p.""OutgoingTechnipIssueCodes""=c.""Id"" and c.""IsDeleted""=false and  c.""CompanyId"" = '{_repo.UserContext.CompanyId}'
     LEFT JOIN public.""DocumentPermission"" as dp ON n.""Id"" = dp.""NoteId"" and dp.""IsDeleted"" = false and  dp.""CompanyId"" = '{_repo.UserContext.CompanyId}'
left Join public.""User"" as ou on n.""OwnerUserId""=ou.""Id"" and ou.""Id""='{userId}' and ou.""IsDeleted"" = false and  ou.""CompanyId"" = '{_repo.UserContext.CompanyId}'	 
left Join public.""User"" as u on dp.""PermittedUserId""=u.""Id"" and u.""Id""='{userId}' and u.""IsDeleted"" = false and  u.""CompanyId"" = '{_repo.UserContext.CompanyId}'
     LEFT JOIN public.""DocumentPermission"" as dup ON n.""Id"" = dup.""NoteId"" and dup.""IsDeleted"" = false and  dup.""CompanyId"" = '{_repo.UserContext.CompanyId}'
     LEFT JOIN public.""UserGroupUser"" uu ON dup.""PermittedUserGroupId"" = uu.""UserGroupId"" and uu.""Id""='{userId}'and uu.""IsDeleted"" = false and  uu.""CompanyId"" = '{_repo.UserContext.CompanyId}'
     left join public.""NtsNote"" as dn on dn.""Id""=dup.""NoteId"" or dn.""Id""=dp.""NoteId"" and dn.""IsDeleted"" = false and  dn.""CompanyId"" = '{_repo.UserContext.CompanyId}'
     left join cms.""N_GENERAL_FOLDER_WORKSPACE"" as ws on ws.""NtsNoteId""=dn.""Id"" and ws.""IsDeleted"" = false and  ws.""CompanyId"" = '{_repo.UserContext.CompanyId}'
     where n.""IsDeleted""=false and  n.""CompanyId"" = '{_repo.UserContext.CompanyId}' ";

                        }

                        else
                        {
                            selectQry = @$"{ selectQry}
                            select n.""Id"" as DocumentId,n.""NoteNo"" as NoteNo,n.""NoteSubject"" as DocumentName,n.""CreatedDate"",r.""Name"" as Revision,d.""Name"" as Discipline,
                            ss.""Name"" as StageStatus,c.""Name"" as IssueCode,p.""technipTransmittalNumber"" as TransmittalNo,p.""technipDueDate"" as DueDate,
                        ou.""Name"" as TemplateOwner,    p.""outgoingTransmittalDate"" as SubmittedDate
     FROM public.""NtsNote"" n
     JOIN public.""Template"" t ON n.""TemplateId"" = t.""Id"" AND t.""IsDeleted"" = false and  t.""CompanyId"" = '{_repo.UserContext.CompanyId}' --AND t.""Code"" = 'PROJECT_DOCUMENTS' 
     JOIN cms.""{tableMeta.Name}"" p ON n.""Id"" = p.""NtsNoteId"" and p.""IsDeleted"" = false and  p.""CompanyId"" = '{_repo.UserContext.CompanyId}' #OVERDUE#
      Join public.""LOV"" as r on p.""revision""=r.""Id"" and r.""IsDeleted"" = false and  r.""CompanyId"" = '{_repo.UserContext.CompanyId}'
	  Join public.""LOV"" as d on p.""discipline""=d.""Id"" and d.""IsDeleted"" = false and  d.""CompanyId"" = '{_repo.UserContext.CompanyId}'#DISCIPLINE#
	  Join public.""LOV"" as ss on p.""stageStatus""=ss.""Id"" and ss.""Code""='TECHNIP_STATUS' and ss.""IsDeleted""=false and  ss.""CompanyId"" = '{_repo.UserContext.CompanyId}'
      left Join public.""LOV"" as c on p.""outgoingTechnipIssueCodes""=c.""Id"" and c.""IsDeleted""=false and  c.""CompanyId"" = '{_repo.UserContext.CompanyId}'
     LEFT JOIN public.""DocumentPermission"" as dp ON n.""Id"" = dp.""NoteId"" and dp.""IsDeleted"" = false and  dp.""CompanyId"" = '{_repo.UserContext.CompanyId}'
left Join public.""User"" as ou on n.""OwnerUserId""=ou.""Id"" and ou.""Id""='{userId}' and ou.""IsDeleted"" = false and  ou.""CompanyId"" = '{_repo.UserContext.CompanyId}'	
left Join public.""User"" as u on dp.""PermittedUserId""=u.""Id"" and u.""Id""='{userId}' and u.""IsDeleted"" = false and  u.""CompanyId"" = '{_repo.UserContext.CompanyId}'
     LEFT JOIN public.""DocumentPermission"" as dup ON n.""Id"" = dup.""NoteId"" and dup.""IsDeleted"" = false and  dup.""CompanyId"" = '{_repo.UserContext.CompanyId}'
     LEFT JOIN public.""UserGroupUser"" uu ON dup.""PermittedUserGroupId"" = uu.""UserGroupId"" and uu.""Id""='{userId}' and uu.""IsDeleted"" = false and  uu.""CompanyId"" = '{_repo.UserContext.CompanyId}'
     left join public.""NtsNote"" as dn on dn.""Id""=dup.""NoteId"" or dn.""Id""=dp.""NoteId"" and dn.""IsDeleted"" = false and  dn.""CompanyId"" = '{_repo.UserContext.CompanyId}'
     left join cms.""N_GENERAL_FOLDER_WORKSPACE"" as ws on ws.""NtsNoteId""=dn.""Id"" and ws.""IsDeleted"" = false and  ws.""CompanyId"" = '{_repo.UserContext.CompanyId}'
     where n.""IsDeleted""=false and  n.""CompanyId"" = '{_repo.UserContext.CompanyId}' ";
                        }



                        i++;
                    }
                }

                var searchDiscipline = "";
                if (discipline.IsNotNullAndNotEmpty())
                {
                    searchDiscipline = $@" and d.""Code""='{discipline}'";
                }
                selectQry = selectQry.Replace("#DISCIPLINE#", searchDiscipline);

                var searchOverdue = "";
                if (IsOverdue == true)
                {
                    searchOverdue = $@" and p.""TechnipDueDate""::TIMESTAMP::DATE < '{DateTime.Now.Date}'::TIMESTAMP::DATE ";
                }
                selectQry = selectQry.Replace("#OVERDUE#", searchOverdue);

                var result2 = await _querydocList.ExecuteQueryList(selectQry, null);

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

                foreach (var item in templateList.Where(x => x.TableMetadataId != null))
                {
                    var tableMeta = await _tableMetadataBusiness.GetSingle(x => x.Id == item.TableMetadataId);
                    if (item.Code == "GENERAL_DOCUMENT" || item.Code == "HALUL_REQUEST_FOR_INSPECTION")
                    {

                    }
                    else
                    {
                        if (i != 1)
                        {
                            selectQry += " union ";
                        }
                        if (item.Code == "PROJECT_DOCUMENTS")
                        {
                            selectQry = @$"{ selectQry}
                            select n.""Id"" as DocumentId,n.""NoteNo"" as NoteNo,n.""NoteSubject"" as DocumentName,n.""CreatedDate"",r.""Name"" as Revision,d.""Name"" as Discipline,
                            ss.""Name"" as StageStatus,coalesce(cd.""Name"", c.""Name"") as IssueCode,p.""qPTransmittalNumber"" as TransmittalNo,p.""galfarDueDate"" as DueDate,
                       ou.""Name"" as TemplateOwner,p.""dateOfReturn"" as SubmittedDate
     FROM public.""NtsNote"" n
     JOIN public.""Template"" t ON n.""TemplateId"" = t.""Id""  AND t.""IsDeleted"" = false and  t.""CompanyId"" = '{_repo.UserContext.CompanyId}'
     JOIN cms.""{tableMeta.Name}"" p ON n.""Id"" = p.""NtsNoteId"" and p.""IsDeleted"" = false and  p.""CompanyId"" = '{_repo.UserContext.CompanyId}' #OVERDUE#
      Join public.""LOV"" as r on p.""revision""=r.""Id"" and r.""IsDeleted"" = false and  r.""CompanyId"" = '{_repo.UserContext.CompanyId}'
	  Join public.""LOV"" as d on p.""discipline""=d.""Id"" and d.""IsDeleted"" = false and  d.""CompanyId"" = '{_repo.UserContext.CompanyId}' #DISCIPLINE#
	  Join public.""LOV"" as ss on p.""stageStatus""=ss.""Id"" and ss.""Code""='GAL_STATUS' and ss.""IsDeleted""=false and  ss.""CompanyId"" = '{_repo.UserContext.CompanyId}'
left Join public.""LOV"" as c on p.""outgoingIssueCodes""=c.""Id"" and c.""IsDeleted""=false and  c.""CompanyId"" = '{_repo.UserContext.CompanyId}'
left Join public.""LOV"" as cd on p.""code""=cd.""Id"" and cd.""IsDeleted""=false and  cd.""CompanyId"" = '{_repo.UserContext.CompanyId}'
     LEFT JOIN public.""DocumentPermission"" as dp ON n.""Id"" = dp.""NoteId"" and dp.""IsDeleted"" = false and  dp.""CompanyId"" = '{_repo.UserContext.CompanyId}'
left Join public.""User"" as ou on n.""OwnerUserId""=ou.""Id"" and ou.""Id""='{userId}' and ou.""IsDeleted"" = false and  ou.""CompanyId"" = '{_repo.UserContext.CompanyId}'	
left Join public.""User"" as u on dp.""PermittedUserId""=u.""Id"" and u.""Id""='{userId}' and u.""IsDeleted"" = false and  u.""CompanyId"" = '{_repo.UserContext.CompanyId}'
     LEFT JOIN public.""DocumentPermission"" as dup ON n.""Id"" = dup.""NoteId"" and dup.""IsDeleted"" = false and  dup.""CompanyId"" = '{_repo.UserContext.CompanyId}'
     LEFT JOIN public.""UserGroupUser"" uu ON dup.""PermittedUserGroupId"" = uu.""UserGroupId"" and uu.""Id""='{userId}' and uu.""IsDeleted"" = false and  uu.""CompanyId"" = '{_repo.UserContext.CompanyId}'
     left join public.""NtsNote"" as dn on dn.""Id""=dup.""NoteId"" or dn.""Id""=dp.""NoteId"" and dn.""IsDeleted"" = false and  dn.""CompanyId"" = '{_repo.UserContext.CompanyId}'
     left join cms.""N_GENERAL_FOLDER_WORKSPACE"" as ws on ws.""NtsNoteId""=dn.""Id"" and ws.""IsDeleted"" = false and  ws.""CompanyId"" = '{_repo.UserContext.CompanyId}'
     where n.""IsDeleted""=false and  n.""CompanyId"" = '{_repo.UserContext.CompanyId}' ";

                        }
                        else if (item.Code == "GALFAR_REQUEST_FOR_INSPECTION")
                        {
                            selectQry = @$"{ selectQry}
                            select n.""Id"" as DocumentId,n.""NoteNo"" as NoteNo,n.""NoteSubject"" as DocumentName,n.""CreatedDate"",r.""Name"" as Revision,d.""Name"" as Discipline,
                            ss.""Name"" as StageStatus,coalesce(cd.""Name"", c.""Name"") as IssueCode,null as TransmittalNo,null as DueDate,
                        ou.""Name"" as TemplateOwner,    p.""dateOfReturn"" as SubmittedDate
     FROM public.""NtsNote"" n
     JOIN public.""Template"" t ON n.""TemplateId"" = t.""Id""  AND t.""IsDeleted"" = false and  t.""CompanyId"" = '{_repo.UserContext.CompanyId}' --AND t.""Code"" = 'PROJECT_DOCUMENTS'
     JOIN cms.""{tableMeta.Name}"" p ON n.""Id"" = p.""NtsNoteId"" and p.""IsDeleted"" = false and  p.""CompanyId"" = '{_repo.UserContext.CompanyId}' #OVERDUE#
      Join public.""LOV"" as r on p.""revision""=r.""Id"" and r.""IsDeleted"" = false and  r.""CompanyId"" = '{_repo.UserContext.CompanyId}'
	  Join public.""LOV"" as d on p.""discipline""=d.""Id"" and d.""IsDeleted"" = false and  d.""CompanyId"" = '{_repo.UserContext.CompanyId}' #DISCIPLINE#
	  Join public.""LOV"" as ss on p.""stageStatus""=ss.""Id"" and ss.""Code""='GAL_STATUS' and ss.""IsDeleted""=false and  ss.""CompanyId"" = '{_repo.UserContext.CompanyId}'
left Join public.""LOV"" as c on p.""OutgoingIssueCodes""=c.""Id"" and c.""IsDeleted""=false and  c.""CompanyId"" = '{_repo.UserContext.CompanyId}'
left Join public.""LOV"" as cd on p.""code""=cd.""Id"" and cd.""IsDeleted""=false and  cd.""CompanyId"" = '{_repo.UserContext.CompanyId}'
     LEFT JOIN public.""DocumentPermission"" as dp ON n.""Id"" = dp.""NoteId"" and dp.""IsDeleted"" = false and  dp.""CompanyId"" = '{_repo.UserContext.CompanyId}'
left Join public.""User"" as ou on n.""OwnerUserId""=ou.""Id"" and ou.""Id""='{userId}' and ou.""IsDeleted"" = false and  ou.""CompanyId"" = '{_repo.UserContext.CompanyId}'	
left Join public.""User"" as u on dp.""PermittedUserId""=u.""Id"" and u.""Id""='{userId}' and u.""IsDeleted"" = false and  u.""CompanyId"" = '{_repo.UserContext.CompanyId}'
     LEFT JOIN public.""DocumentPermission"" as dup ON n.""Id"" = dup.""NoteId"" and dup.""IsDeleted"" = false and  dup.""CompanyId"" = '{_repo.UserContext.CompanyId}'
     LEFT JOIN public.""UserGroupUser"" uu ON dup.""PermittedUserGroupId"" = uu.""UserGroupId"" and uu.""Id""='{userId}' and uu.""IsDeleted"" = false and  uu.""CompanyId"" = '{_repo.UserContext.CompanyId}'
     left join public.""NtsNote"" as dn on dn.""Id""=dup.""NoteId"" or dn.""Id""=dp.""NoteId"" and dn.""IsDeleted"" = false and  dn.""CompanyId"" = '{_repo.UserContext.CompanyId}'
     left join cms.""N_GENERAL_FOLDER_WORKSPACE"" as ws on ws.""NtsNoteId""=dn.""Id"" and ws.""IsDeleted"" = false and  ws.""CompanyId"" = '{_repo.UserContext.CompanyId}'
     where n.""IsDeleted""=false and  n.""CompanyId"" = '{_repo.UserContext.CompanyId}'  ";

                        }
                        else
                        {
                            selectQry = @$"{ selectQry}
                            select n.""Id"" as DocumentId,n.""NoteNo"" as NoteNo,n.""NoteSubject"" as DocumentName,n.""CreatedDate"",r.""Name"" as Revision,d.""Name"" as Discipline,
                            ss.""Name"" as StageStatus,coalesce(cd.""Name"", c.""Name"") as IssueCode,coalesce(p.""qPTransmittalNumber"",p.""incomingTransmittalNumber"") as TransmittalNo,p.""galfarDueDate"" as DueDate,
                     ou.""Name"" as TemplateOwner,coalesce(p.""dateOfReturn"",p.""incomingTransmittalDate"") as SubmittedDate
     FROM public.""NtsNote"" n
     JOIN public.""Template"" t ON n.""TemplateId"" = t.""Id""  AND t.""IsDeleted"" = false and  t.""CompanyId"" = '{_repo.UserContext.CompanyId}'
     JOIN cms.""{tableMeta.Name}"" p ON n.""Id"" = p.""NtsNoteId"" and p.""IsDeleted"" = false and  p.""CompanyId"" = '{_repo.UserContext.CompanyId}' #OVERDUE#
      Join public.""LOV"" as r on p.""revision""=r.""Id"" and r.""IsDeleted"" = false and  r.""CompanyId"" = '{_repo.UserContext.CompanyId}'
	  Join public.""LOV"" as d on p.""discipline""=d.""Id"" and d.""IsDeleted"" = false and  d.""CompanyId"" = '{_repo.UserContext.CompanyId}' #DISCIPLINE#
	  Join public.""LOV"" as ss on p.""stageStatus""=ss.""Id"" and ss.""Code""='GAL_STATUS' and ss.""IsDeleted""=false and  ss.""CompanyId"" = '{_repo.UserContext.CompanyId}'
left Join public.""LOV"" as c on p.""outgoingIssueCodes""=c.""Id"" and c.""IsDeleted""=false and  c.""CompanyId"" = '{_repo.UserContext.CompanyId}'
left Join public.""LOV"" as cd on p.""code""=cd.""Id"" and cd.""IsDeleted""=false and  cd.""CompanyId"" = '{_repo.UserContext.CompanyId}'
     LEFT JOIN public.""DocumentPermission"" as dp ON n.""Id"" = dp.""NoteId"" and dp.""IsDeleted"" = false and  dp.""CompanyId"" = '{_repo.UserContext.CompanyId}'
left Join public.""User"" as ou on n.""OwnerUserId""=ou.""Id"" and ou.""Id""='{userId}' and ou.""IsDeleted"" = false and  ou.""CompanyId"" = '{_repo.UserContext.CompanyId}'	 
left Join public.""User"" as u on dp.""PermittedUserId""=u.""Id"" and u.""Id""='{userId}' and u.""IsDeleted"" = false and  u.""CompanyId"" = '{_repo.UserContext.CompanyId}'
     LEFT JOIN public.""DocumentPermission"" as dup ON n.""Id"" = dup.""NoteId"" and dup.""IsDeleted"" = false and  dup.""CompanyId"" = '{_repo.UserContext.CompanyId}'
     LEFT JOIN public.""UserGroupUser"" uu ON dup.""PermittedUserGroupId"" = uu.""UserGroupId"" and uu.""Id""='{userId}' and uu.""IsDeleted"" = false and  uu.""CompanyId"" = '{_repo.UserContext.CompanyId}'
     left join public.""NtsNote"" as dn on dn.""Id""=dup.""NoteId"" or dn.""Id""=dp.""NoteId"" and dn.""IsDeleted"" = false and  dn.""CompanyId"" = '{_repo.UserContext.CompanyId}'
     left join cms.""N_GENERAL_FOLDER_WORKSPACE"" as ws on ws.""NtsNoteId""=dn.""Id"" and ws.""IsDeleted"" = false and  ws.""CompanyId"" = '{_repo.UserContext.CompanyId}'
     where n.""IsDeleted""=false and  n.""CompanyId"" = '{_repo.UserContext.CompanyId}'   ";
                        }


                        i++;
                    }
                }

                var searchDiscipline = "";
                if (discipline.IsNotNullAndNotEmpty())
                {
                    searchDiscipline = $@" and d.""Code""='{discipline}'";
                }
                selectQry = selectQry.Replace("#DISCIPLINE#", searchDiscipline);

                var searchOverdue = "";
                if (IsOverdue == true)
                {
                    searchOverdue = $@" and p.""galfarDueDate""::TIMESTAMP::DATE < '{DateTime.Now.Date}'::TIMESTAMP::DATE ";
                }
                selectQry = selectQry.Replace("#OVERDUE#", searchOverdue);

                var result2 = await _querydocList.ExecuteQueryList(selectQry, null);

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

                foreach (var item in templateList.Where(x => x.TableMetadataId != null))
                {
                    var tableMeta = await _tableMetadataBusiness.GetSingle(x => x.Id == item.TableMetadataId);
                    if (item.Code == "GENERAL_DOCUMENT" || item.Code == "HALUL_REQUEST_FOR_INSPECTION")
                    {

                    }
                    else
                    {
                        if (i != 1)
                        {
                            selectQry += " union ";
                        }
                        if (item.Code == "PROJECT_DOCUMENTS")
                        {
                            selectQry = @$"{ selectQry}
                            select n.""Id"" as DocumentId,n.""NoteNo"" as NoteNo,n.""NoteSubject"" as DocumentName,n.""CreatedDate"",r.""Name"" as Revision,d.""Name"" as Discipline,
                            ss.""Name"" as StageStatus,c.""Name"" as IssueCode,null as TransmittalNo,null as DueDate,ou.""Name"" as TemplateOwner,
                            p.""outgoingTransmittalDate"" as SubmittedDate
     FROM public.""NtsNote"" n
     JOIN public.""Template"" t ON n.""TemplateId"" = t.""Id""  AND t.""IsDeleted"" = false and  t.""CompanyId"" = '{_repo.UserContext.CompanyId}' --AND t.""Code"" = 'PROJECT_DOCUMENTS'
     JOIN cms.""{tableMeta.Name}"" p ON n.""Id"" = p.""NtsNoteId"" and p.""IsDeleted"" = false and  p.""CompanyId"" = '{_repo.UserContext.CompanyId}' #OVERDUE#
      Join public.""LOV"" as r on p.""revision""=r.""Id"" and r.""IsDeleted"" = false and  r.""CompanyId"" = '{_repo.UserContext.CompanyId}'
	  Join public.""LOV"" as d on p.""discipline""=d.""Id"" and d.""IsDeleted"" = false and  d.""CompanyId"" = '{_repo.UserContext.CompanyId}' #DISCIPLINE#
	  Join public.""LOV"" as ss on p.""stageStatus""=ss.""Id"" and ss.""Code""='Vendor_Status' and ss.""IsDeleted""=false and  ss.""CompanyId"" = '{_repo.UserContext.CompanyId}'
left Join public.""LOV"" as c on p.""outgoingTechnipIssueCodes""=c.""Id"" and c.""IsDeleted""=false and  c.""CompanyId"" = '{_repo.UserContext.CompanyId}'
     LEFT JOIN public.""DocumentPermission"" as dp ON n.""Id"" = dp.""NoteId"" and dp.""IsDeleted"" = false and  dp.""CompanyId"" = '{_repo.UserContext.CompanyId}'
left Join public.""User"" as ou on n.""OwnerUserId""=ou.""Id"" and ou.""Id""='{userId}' and ou.""IsDeleted"" = false and  ou.""CompanyId"" = '{_repo.UserContext.CompanyId}'	
left Join public.""User"" as u on dp.""PermittedUserId""=u.""Id"" and u.""Id""='{userId}' and u.""IsDeleted"" = false and  u.""CompanyId"" = '{_repo.UserContext.CompanyId}'
     LEFT JOIN public.""DocumentPermission"" as dup ON n.""Id"" = dup.""NoteId"" and dup.""IsDeleted"" = false and  dup.""CompanyId"" = '{_repo.UserContext.CompanyId}'
     LEFT JOIN public.""UserGroupUser"" uu ON dup.""PermittedUserGroupId"" = uu.""UserGroupId"" and uu.""Id""='{userId}' and uu.""IsDeleted"" = false and  uu.""CompanyId"" = '{_repo.UserContext.CompanyId}'
     left join public.""NtsNote"" as dn on dn.""Id""=dup.""NoteId"" or dn.""Id""=dp.""NoteId"" and dn.""IsDeleted"" = false and  dn.""CompanyId"" = '{_repo.UserContext.CompanyId}'
     left join cms.""N_GENERAL_FOLDER_WORKSPACE"" as ws on ws.""NtsNoteId""=dn.""Id"" and ws.""IsDeleted"" = false and  ws.""CompanyId"" = '{_repo.UserContext.CompanyId}'
     where n.""IsDeleted""=false and  n.""CompanyId"" = '{_repo.UserContext.CompanyId}' ";

                        }
                        else if (item.Code == "GALFAR_REQUEST_FOR_INSPECTION")
                        {
                            selectQry = @$"{ selectQry}
                            select n.""Id"" as DocumentId,n.""NoteNo"" as NoteNo,n.""NoteSubject"" as DocumentName,n.""CreatedDate"",r.""Name"" as Revision,d.""Name"" as Discipline,
                            ss.""Name"" as StageStatus,c.""Name"" as IssueCode,null as TransmittalNo,null as DueDate,ou.""Name"" as TemplateOwner,
                            null as SubmittedDate
     FROM public.""NtsNote"" n
     JOIN public.""Template"" t ON n.""TemplateId"" = t.""Id""  AND t.""IsDeleted"" = false and  n.""CompanyId"" = '{_repo.UserContext.CompanyId}' --AND t.""Code"" = 'PROJECT_DOCUMENTS'
     JOIN cms.""{tableMeta.Name}"" p ON n.""Id"" = p.""NtsNoteId"" and p.""IsDeleted"" = false and  p.""CompanyId"" = '{_repo.UserContext.CompanyId}' #OVERDUE#
      Join public.""LOV"" as r on p.""revision""=r.""Id"" and r.""IsDeleted"" = false and  r.""CompanyId"" = '{_repo.UserContext.CompanyId}'
	  Join public.""LOV"" as d on p.""discipline""=d.""Id"" and d.""IsDeleted"" = false and  d.""CompanyId"" = '{_repo.UserContext.CompanyId}' #DISCIPLINE#
	  Join public.""LOV"" as ss on p.""stageStatus""=ss.""Id"" and ss.""Code""='Vendor_Status' and ss.""IsDeleted""=false and  ss.""CompanyId"" = '{_repo.UserContext.CompanyId}'
left Join public.""LOV"" as c on p.""outgoingTechnipIssueCodes""=c.""Id"" and c.""IsDeleted""=false and  c.""CompanyId"" = '{_repo.UserContext.CompanyId}'
     LEFT JOIN public.""DocumentPermission"" as dp ON n.""Id"" = dp.""NoteId"" and dp.""IsDeleted"" = false and  dp.""CompanyId"" = '{_repo.UserContext.CompanyId}'
left Join public.""User"" as ou on n.""OwnerUserId""=ou.""Id"" and ou.""Id""='{userId}' and ou.""IsDeleted"" = false and  ou.""CompanyId"" = '{_repo.UserContext.CompanyId}'	
left Join public.""User"" as u on dp.""PermittedUserId""=u.""Id"" and u.""Id""='{userId}' and u.""IsDeleted"" = false and  u.""CompanyId"" = '{_repo.UserContext.CompanyId}'
     LEFT JOIN public.""DocumentPermission"" as dup ON n.""Id"" = dup.""NoteId"" and dup.""IsDeleted"" = false and  dup.""CompanyId"" = '{_repo.UserContext.CompanyId}'
     LEFT JOIN public.""UserGroupUser"" uu ON dup.""PermittedUserGroupId"" = uu.""UserGroupId"" and uu.""Id""='{userId}' and uu.""IsDeleted"" = false and  uu.""CompanyId"" = '{_repo.UserContext.CompanyId}'
     left join public.""NtsNote"" as dn on dn.""Id""=dup.""NoteId"" or dn.""Id""=dp.""NoteId"" and dn.""IsDeleted"" = false and  dn.""CompanyId"" = '{_repo.UserContext.CompanyId}'
     left join cms.""N_GENERAL_FOLDER_WORKSPACE"" as ws on ws.""NtsNoteId""=dn.""Id"" and ws.""IsDeleted"" = false and  ws.""CompanyId"" = '{_repo.UserContext.CompanyId}'
     where n.""IsDeleted""=false and  n.""CompanyId"" = '{_repo.UserContext.CompanyId}'  ";

                        }
                        else
                        {
                            selectQry = @$"{ selectQry}
                            select n.""Id"" as DocumentId,n.""NoteNo"" as NoteNo,n.""NoteSubject"" as DocumentName,n.""CreatedDate"",r.""Name"" as Revision,d.""Name"" as Discipline,
                            ss.""Name"" as StageStatus,c.""Name"" as IssueCode,p.""technipTransmittalNumber"" as TransmittalNo,p.""technipDueDate"" as DueDate,
                            p.""outgoingTransmittalDate"" as SubmittedDate,ou.""Name"" as TemplateOwner
     FROM public.""NtsNote"" n
     JOIN public.""Template"" t ON n.""TemplateId"" = t.""Id""  AND t.""IsDeleted"" = false and  t.""CompanyId"" = '{_repo.UserContext.CompanyId}'  --AND t.""Code"" = 'PROJECT_DOCUMENTS'
     JOIN cms.""{tableMeta.Name}"" p ON n.""Id"" = p.""NtsNoteId"" and p.""IsDeleted"" = false and  p.""CompanyId"" = '{_repo.UserContext.CompanyId}'  #OVERDUE#
      Join public.""LOV"" as r on p.""revision""=r.""Id"" and r.""IsDeleted"" = false and  r.""CompanyId"" = '{_repo.UserContext.CompanyId}' 
	  Join public.""LOV"" as d on p.""discipline""=d.""Id"" and d.""IsDeleted"" = false and  d.""CompanyId"" = '{_repo.UserContext.CompanyId}'  #DISCIPLINE#
	  Join public.""LOV"" as ss on p.""stageStatus""=ss.""Id"" and ss.""Code""='Vendor_Status' and ss.""IsDeleted""=false and  ss.""CompanyId"" = '{_repo.UserContext.CompanyId}' 
left Join public.""LOV"" as c on p.""outgoingTechnipIssueCodes""=c.""Id"" and c.""IsDeleted""=false and  c.""CompanyId"" = '{_repo.UserContext.CompanyId}' 
     LEFT JOIN public.""DocumentPermission"" dp ON n.""Id"" = dp.""NoteId"" and dp.""IsDeleted"" = false and  dp.""CompanyId"" = '{_repo.UserContext.CompanyId}' 
left Join public.""User"" as ou on n.""OwnerUserId""=ou.""Id"" and ou.""Id""='{userId}' and ou.""IsDeleted"" = false and  ou.""CompanyId"" = '{_repo.UserContext.CompanyId}'	
left Join public.""User"" as u on dp.""PermittedUserId""=u.""Id"" and u.""Id""='{userId}' and u.""IsDeleted"" = false and  u.""CompanyId"" = '{_repo.UserContext.CompanyId}' 
     LEFT JOIN public.""DocumentPermission"" as dup ON n.""Id"" = dup.""NoteId"" and dup.""IsDeleted"" = false and  dup.""CompanyId"" = '{_repo.UserContext.CompanyId}' 
     LEFT JOIN public.""UserGroupUser"" uu ON dup.""PermittedUserGroupId"" = uu.""UserGroupId"" and uu.""Id""='{userId}' and uu.""IsDeleted"" = false and  uu.""CompanyId"" = '{_repo.UserContext.CompanyId}' 
     left join public.""NtsNote"" as dn on dn.""Id""=dup.""NoteId"" or dn.""Id""=dp.""NoteId"" and dn.""IsDeleted"" = false and  dn.""CompanyId"" = '{_repo.UserContext.CompanyId}' 
     left join cms.""N_GENERAL_FOLDER_WORKSPACE"" as ws on ws.""NtsNoteId""=dn.""Id"" and ws.""IsDeleted"" = false and  ws.""CompanyId"" = '{_repo.UserContext.CompanyId}' 
     where n.""IsDeleted""=false and  n.""CompanyId"" = '{_repo.UserContext.CompanyId}'   ";
                        }

                        i++;
                    }
                }

                var searchDiscipline = "";
                if (discipline.IsNotNullAndNotEmpty())
                {
                    searchDiscipline = $@" and d.""Code""='{discipline}'";
                }
                selectQry = selectQry.Replace("#DISCIPLINE#", searchDiscipline);

                var searchOverdue = "";
                if (IsOverdue == true)
                {
                    searchOverdue = $@" and p.""TechnipDueDate""::TIMESTAMP::DATE < '{DateTime.Now.Date}'::TIMESTAMP::DATE ";
                }
                selectQry = selectQry.Replace("#OVERDUE#", searchOverdue);

                var result2 = await _querydocList.ExecuteQueryList(selectQry, null);

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

                foreach (var item in templateList)
                {
                    if(item.TableMetadataId != null)
                    {
                        var tableMeta = await _tableMetadataBusiness.GetSingle(x => x.Id == item.TableMetadataId);
                        if (item.Code == "GENERAL_DOCUMENT" || item.Code == "HALUL_REQUEST_FOR_INSPECTION")
                        {

                        }
                        else
                        {
                            if (i != 1)
                            {
                                selectQry += " union ";
                            }

                            if (item.Code == "PROJECT_DOCUMENTS")
                            {
                                selectQry = @$"{ selectQry}
                            select distinct n.""Id"" as DocumentId,n.""NoteNo"" as NoteNo,n.""NoteSubject"" as DocumentName,n.""CreatedDate"",r.""Name"" as Revision,d.""Name"" as Discipline,
                            ss.""Name"" as StageStatus,ou.""Name"" as TemplateOwner,
case 
 when ss.""Code""='QP_STATUS' then c.""Name""
 when ss.""Code"" = 'TECHNIP_STATUS' then ct.""Name""
 when ss.""Code"" = 'GAL_STATUS' then c.""Name""
 when ss.""Code"" = 'Vendor_Status' then ct.""Name"" else null end as IssueCode,
case 
 when ss.""Code""='QP_STATUS' then p.""galfarTransmittalNumber""
 when ss.""Code"" = 'TECHNIP_STATUS' then null
 when ss.""Code"" = 'GAL_STATUS' then p.""qPTransmittalNumber""
 when ss.""Code"" = 'Vendor_Status' then null else null end as TransmittalNo,
case 
 when ss.""Code""='QP_STATUS' then p.""qpDueDate""
 when ss.""Code"" = 'TECHNIP_STATUS' then null
 when ss.""Code"" = 'GAL_STATUS' then p.""galfarDueDate""
 when ss.""Code"" = 'Vendor_Status' then null else null end as DueDate,
case 
 when ss.""Code""='QP_STATUS' then p.""dateOfSubmission""
 when ss.""Code"" = 'TECHNIP_STATUS' then null
 when ss.""Code"" = 'GAL_STATUS' then p.""dateOfReturn""
 when ss.""Code"" = 'Vendor_Status' then null else null end as SubmittedDate
     FROM public.""NtsNote"" n
     JOIN public.""Template"" t ON n.""TemplateId"" = t.""Id""  AND t.""IsDeleted"" = false and  t.""CompanyId"" = '{_repo.UserContext.CompanyId}'
     JOIN cms.""{tableMeta.Name}"" p ON n.""Id"" = p.""NtsNoteId"" and p.""IsDeleted"" = false and  p.""CompanyId"" = '{_repo.UserContext.CompanyId}'
      Join public.""LOV"" as r on p.""revision""=r.""Id"" and r.""IsDeleted"" = false and  r.""CompanyId"" = '{_repo.UserContext.CompanyId}'
	  Join public.""LOV"" as d on p.""discipline""=d.""Id"" and d.""IsDeleted"" = false and  d.""CompanyId"" = '{_repo.UserContext.CompanyId}' #DISCIPLINE#
	  Join public.""LOV"" as ss on p.""stageStatus""=ss.""Id"" and ss.""Code"" in ('QP_STATUS','TECHNIP_STATUS','GAL_STATUS','Vendor_Status' ) and ss.""IsDeleted""=false and  ss.""CompanyId"" = '{_repo.UserContext.CompanyId}'
left Join public.""LOV"" as c on p.""outgoingIssueCodes""=c.""Id"" and c.""IsDeleted""=false and  c.""CompanyId"" = '{_repo.UserContext.CompanyId}'
left Join public.""LOV"" as ct on p.""outgoingTechnipIssueCodes""=ct.""Id"" and ct.""IsDeleted""=false and  ct.""CompanyId"" = '{_repo.UserContext.CompanyId}'
--left Join public.""LOV"" as ci on p.""issueCodes""=ci.""Id"" and ci.""IsDeleted""=false and  ci.""CompanyId"" = '{_repo.UserContext.CompanyId}'
left Join public.""LOV"" as cd on p.""code""=cd.""Id"" and cd.""IsDeleted""=false and  cd.""CompanyId"" = '{_repo.UserContext.CompanyId}'   
LEFT JOIN public.""DocumentPermission"" as dp ON n.""Id"" = dp.""NoteId"" and dp.""IsDeleted"" = false and  dp.""CompanyId"" = '{_repo.UserContext.CompanyId}' 
left Join public.""User"" as ou on n.""OwnerUserId""=ou.""Id"" and ou.""Id""='{userId}' and ou.""IsDeleted"" = false and  ou.""CompanyId"" = '{_repo.UserContext.CompanyId}'	
left Join public.""User"" as u on dp.""PermittedUserId""=u.""Id"" and u.""Id""='{userId}' and u.""IsDeleted"" = false and  u.""CompanyId"" = '{_repo.UserContext.CompanyId}'
     LEFT JOIN public.""DocumentPermission"" as dup ON n.""Id"" = dup.""NoteId"" and dup.""IsDeleted"" = false and  dup.""CompanyId"" = '{_repo.UserContext.CompanyId}'
     LEFT JOIN public.""UserGroupUser"" uu ON dup.""PermittedUserGroupId"" = uu.""UserGroupId"" and uu.""Id""='{userId}' and uu.""IsDeleted"" = false and  uu.""CompanyId"" = '{_repo.UserContext.CompanyId}'
     left join public.""NtsNote"" as dn on dn.""Id""=dup.""NoteId"" or dn.""Id""=dp.""NoteId"" and dn.""IsDeleted"" = false and  dn.""CompanyId"" = '{_repo.UserContext.CompanyId}' 
     left join cms.""N_GENERAL_FOLDER_WORKSPACE"" as ws on ws.""NtsNoteId""=dn.""Id"" and ws.""IsDeleted"" = false and  ws.""CompanyId"" = '{_repo.UserContext.CompanyId}'
     where n.""IsDeleted""=false and  n.""CompanyId"" = '{_repo.UserContext.CompanyId}' ";

                            }
                            else if (item.Code == "GALFAR_REQUEST_FOR_INSPECTION")
                            {
                                selectQry = @$"{ selectQry}
                            select distinct n.""Id"" as DocumentId,n.""NoteNo"" as NoteNo,n.""NoteSubject"" as DocumentName,n.""CreatedDate"",r.""Name"" as Revision,d.""Name"" as Discipline,
                            ss.""Name"" as StageStatus,ou.""Name"" as TemplateOwner,
case 
 when ss.""Code""='QP_STATUS' then c.""Name""
 when ss.""Code"" = 'TECHNIP_STATUS' then null
 when ss.""Code"" = 'GAL_STATUS' then c.""Name""
 when ss.""Code"" = 'Vendor_Status' then null else null end as IssueCode,
 null as TransmittalNo,
 null as DueDate,
case 
 when ss.""Code""='QP_STATUS' then p.""dateOfSubmission""
 when ss.""Code"" = 'TECHNIP_STATUS' then null
 when ss.""Code"" = 'GAL_STATUS' then p.""dateOfReturn""
 when ss.""Code"" = 'Vendor_Status' then null else null end as SubmittedDate
     FROM public.""NtsNote"" n
     JOIN public.""Template"" t ON n.""TemplateId"" = t.""Id""  AND t.""IsDeleted"" = false and  t.""CompanyId"" = '{_repo.UserContext.CompanyId}'
     JOIN cms.""{tableMeta.Name}"" p ON n.""Id"" = p.""NtsNoteId"" and p.""IsDeleted"" = false and  p.""CompanyId"" = '{_repo.UserContext.CompanyId}'
      Join public.""LOV"" as r on p.""revision""=r.""Id"" and r.""IsDeleted"" = false and  r.""CompanyId"" = '{_repo.UserContext.CompanyId}'
	  Join public.""LOV"" as d on p.""discipline""=d.""Id"" and d.""IsDeleted"" = false and  d.""CompanyId"" = '{_repo.UserContext.CompanyId}' #DISCIPLINE#
	  Join public.""LOV"" as ss on p.""stageStatus""=ss.""Id"" and ss.""Code"" in ('QP_STATUS','TECHNIP_STATUS','GAL_STATUS','Vendor_Status' ) and ss.""IsDeleted""=false and  ss.""CompanyId"" = '{_repo.UserContext.CompanyId}'
left Join public.""LOV"" as c on p.""OutgoingIssueCodes""=c.""Id"" and c.""IsDeleted""=false and  c.""CompanyId"" = '{_repo.UserContext.CompanyId}'
--left Join public.""LOV"" as ct on p.""outgoingTechnipIssueCodes""=ct.""Id"" and ct.""IsDeleted""=false and  ct.""CompanyId"" = '{_repo.UserContext.CompanyId}'
--left Join public.""LOV"" as ci on p.""issueCodes""=ci.""Id"" and ci.""IsDeleted""=false and  ci.""CompanyId"" = '{_repo.UserContext.CompanyId}'
left Join public.""LOV"" as cd on p.""code""=cd.""Id"" and cd.""IsDeleted""=false and  cd.""CompanyId"" = '{_repo.UserContext.CompanyId}'   
LEFT JOIN public.""DocumentPermission"" as dp ON n.""Id"" = dp.""NoteId"" and dp.""IsDeleted"" = false and  dp.""CompanyId"" = '{_repo.UserContext.CompanyId}'
	 left Join public.""User"" as u on dp.""PermittedUserId""=u.""Id"" and u.""Id""='{userId}' and u.""IsDeleted"" = false and  u.""CompanyId"" = '{_repo.UserContext.CompanyId}'
left Join public.""User"" as ou on n.""OwnerUserId""=ou.""Id"" and ou.""Id""='{userId}' and ou.""IsDeleted"" = false and  ou.""CompanyId"" = '{_repo.UserContext.CompanyId}'     
LEFT JOIN public.""DocumentPermission"" dup ON n.""Id"" = dup.""NoteId"" and dup.""IsDeleted"" = false and  dup.""CompanyId"" = '{_repo.UserContext.CompanyId}' 
     LEFT JOIN public.""UserGroupUser"" uu ON dup.""PermittedUserGroupId"" = uu.""UserGroupId"" and uu.""Id""='{userId}' and uu.""IsDeleted"" = false and  uu.""CompanyId"" = '{_repo.UserContext.CompanyId}'
     left join public.""NtsNote"" as dn on dn.""Id""=dup.""NoteId"" or dn.""Id""=dp.""NoteId"" and dn.""IsDeleted"" = false and  dn.""CompanyId"" = '{_repo.UserContext.CompanyId}'
     left join cms.""N_GENERAL_FOLDER_WORKSPACE"" as ws on ws.""NtsNoteId""=dn.""Id"" and ws.""IsDeleted"" = false and  ws.""CompanyId"" = '{_repo.UserContext.CompanyId}' 
     where n.""IsDeleted""=false and  n.""CompanyId"" = '{_repo.UserContext.CompanyId}' ";

                            }
                            else
                            {
                                selectQry = @$"{ selectQry}
                            select distinct n.""Id"" as DocumentId,n.""NoteNo"" as NoteNo,n.""NoteSubject"" as DocumentName,n.""CreatedDate"",r.""Name"" as Revision,d.""Name"" as Discipline,
                            ss.""Name"" as StageStatus,ou.""Name"" as TemplateOwner,
case 
 when ss.""Code""='QP_STATUS' then c.""Name""
 when ss.""Code"" = 'TECHNIP_STATUS' then ct.""Name""
 when ss.""Code"" = 'GAL_STATUS' then coalesce(c.""Name"",ci.""Name"" )
 when ss.""Code"" = 'Vendor_Status' then ct.""Name"" else null end as IssueCode,
case 
 when ss.""Code""='QP_STATUS' then p.""galfarTransmittalNumber""
 when ss.""Code"" = 'TECHNIP_STATUS' then p.""technipTransmittalNumber""
 when ss.""Code"" = 'GAL_STATUS' then coalesce(p.""qPTransmittalNumber"",p.""incomingTransmittalNumber"" )
 when ss.""Code"" = 'Vendor_Status' then p.""technipTransmittalNumber"" else null end as TransmittalNo,
case 
 when ss.""Code""='QP_STATUS' then p.""qpDueDate""
 when ss.""Code"" = 'TECHNIP_STATUS' then p.""technipDueDate""
 when ss.""Code"" = 'GAL_STATUS' then p.""galfarDueDate""
 when ss.""Code"" = 'Vendor_Status' then p.""technipDueDate"" else null end as DueDate,
case 
 when ss.""Code""='QP_STATUS' then p.""dateOfSubmission""
 when ss.""Code"" = 'TECHNIP_STATUS' then p.""outgoingTransmittalDate""
 when ss.""Code"" = 'GAL_STATUS' then coalesce(p.""dateOfReturn"",p.""incomingTransmittalDate"")
 when ss.""Code"" = 'Vendor_Status' then p.""outgoingTransmittalDate"" else null end as SubmittedDate
     FROM public.""NtsNote"" n
     JOIN public.""Template"" t ON n.""TemplateId"" = t.""Id""  AND t.""IsDeleted"" = false and  t.""CompanyId"" = '{_repo.UserContext.CompanyId}'
     JOIN cms.""{tableMeta.Name}"" p ON n.""Id"" = p.""NtsNoteId"" and p.""IsDeleted"" = false and  p.""CompanyId"" = '{_repo.UserContext.CompanyId}'
      Join public.""LOV"" as r on p.""revision""=r.""Id"" and r.""IsDeleted"" = false and  r.""CompanyId"" = '{_repo.UserContext.CompanyId}'
	  Join public.""LOV"" as d on p.""discipline""=d.""Id"" and d.""IsDeleted"" = false and  d.""CompanyId"" = '{_repo.UserContext.CompanyId}'  #DISCIPLINE#
	  Join public.""LOV"" as ss on p.""stageStatus""=ss.""Id"" and ss.""Code"" in ('QP_STATUS','TECHNIP_STATUS','GAL_STATUS','Vendor_Status' ) and ss.""IsDeleted""=false and  ss.""CompanyId"" = '{_repo.UserContext.CompanyId}'
left Join public.""LOV"" as c on p.""outgoingIssueCodes""=c.""Id"" and c.""IsDeleted""=false and  c.""CompanyId"" = '{_repo.UserContext.CompanyId}'
left Join public.""LOV"" as ct on p.""outgoingTechnipIssueCodes""=ct.""Id"" and ct.""IsDeleted""=false and  ct.""CompanyId"" = '{_repo.UserContext.CompanyId}'
left Join public.""LOV"" as ci on p.""issueCodes""=ci.""Id"" and ci.""IsDeleted""=false  and  ci.""CompanyId"" = '{_repo.UserContext.CompanyId}'
left Join public.""LOV"" as cd on p.""code""=cd.""Id"" and cd.""IsDeleted""=false  and  cd.""CompanyId"" = '{_repo.UserContext.CompanyId}'   
LEFT JOIN public.""DocumentPermission"" as dp ON n.""Id"" = dp.""NoteId"" and dp.""IsDeleted"" = false and  dp.""CompanyId"" = '{_repo.UserContext.CompanyId}'
	 left Join public.""User"" as u on dp.""PermittedUserId""=u.""Id"" and u.""Id""='{userId}' and u.""IsDeleted"" = false and  u.""CompanyId"" = '{_repo.UserContext.CompanyId}'
 left Join public.""User"" as ou on n.""OwnerUserId""=ou.""Id"" and ou.""Id""='{userId}' and ou.""IsDeleted"" = false and  ou.""CompanyId"" = '{_repo.UserContext.CompanyId}'
     LEFT JOIN public.""DocumentPermission"" as dup ON n.""Id"" = dup.""NoteId"" and dup.""IsDeleted"" = false and  dup.""CompanyId"" = '{_repo.UserContext.CompanyId}'
     LEFT JOIN public.""UserGroupUser"" uu ON dup.""PermittedUserGroupId"" = uu.""UserGroupId"" and uu.""Id""='{userId}' and uu.""IsDeleted"" = false and  uu.""CompanyId"" = '{_repo.UserContext.CompanyId}'
     left join public.""NtsNote"" as dn on dn.""Id""=dup.""NoteId"" or dn.""Id""=dp.""NoteId"" and dn.""IsDeleted"" = false and  dn.""CompanyId"" = '{_repo.UserContext.CompanyId}'
     left join cms.""N_GENERAL_FOLDER_WORKSPACE"" as ws on ws.""NtsNoteId""=dn.""Id""and ws.""IsDeleted"" = false and  ws.""CompanyId"" = '{_repo.UserContext.CompanyId}'
     where n.""IsDeleted""=false and  n.""CompanyId"" = '{_repo.UserContext.CompanyId}' ";
                            }



                            i++;
                        }
                    }
                    
                }

                var searchDiscipline = "";
                if (discipline.IsNotNullAndNotEmpty())
                {
                    searchDiscipline = $@" and d.""Code""='{discipline}'";
                }
                selectQry = selectQry.Replace("#DISCIPLINE#", searchDiscipline);


                var result2 = await _querydocList.ExecuteQueryList(selectQry, null);

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
            //var cypher = string.Concat(@"match(f: NTS_Note{ IsDeleted: 0})
            //where(f.IsArchived <> true or f.IsArchived is null)           
            //match(f) -[:R_Note_Template]->(t: NTS_Template{IsDeleted: 0,Status: 'Active'})
            //-[:R_TemplateRoot]->(tm: NTS_TemplateMaster{IsDeleted: 0,Status: 'Active'})  
            //-[:R_TemplateMaster_TemplateCategory]->(tc: NTS_TemplateCategory{ IsDeleted: 0,Code: 'GENERAL_DOCUMENT'})            
            //optional match(f)< -[:R_NotePermission_Note] - (np: NTS_NotePermission{ IsDeleted: 0})
            //-[:R_NotePermission_User]->(npu: ADM_User{ Id: {UserId}}) 
            //optional match(f)< -[:R_NotePermission_Note] - (np2: NTS_NotePermission{ IsDeleted: 0})
            //-[:R_NotePermission_WorkspacePermissionGroup]->(npwg: ADM_WorkspacePermissionGroup{ IsDeleted: 0})  
            //< -[:R_User_UserPermissionGroup] - (npwgu: ADM_User{ Id: {UserId}})
            //with f,tm,t,tc,case when npu is not null then np else np2 end as pmn 
            //where (npu is not null or npwgu is not null) and pmn.PermissionType='Allow'
            //WITH  f order by f.CreatedDate desc
            //with f.NoteNo as NoteNo,collect (f) as node
            //with node[0].Id as PerId
            //return distinct PerId");

            var query = $@"select distinct n.""Id""
                            from public.""NtsNote"" as n
                            JOIN public.""Template"" t ON n.""TemplateId"" = t.""Id"" AND t.""IsDeleted"" = false and  t.""CompanyId"" = '{_repo.UserContext.CompanyId}'
                            JOIN public.""TemplateCategory"" tc ON t.""TemplateCategoryId"" = tc.""Id"" AND tc.""Code"" = 'GENERAL_DOCUMENT' AND tc.""IsDeleted"" = false and  tc.""CompanyId"" = '{_repo.UserContext.CompanyId}'   
                            LEFT JOIN public.""DocumentPermission"" dp ON n.""Id"" = dp.""NoteId"" AND dp.""IsDeleted"" = false and  dp.""CompanyId"" = '{_repo.UserContext.CompanyId}'
                            Join public.""User"" as u on dp.""PermittedUserId""=u.""Id"" and u.""Id""='{userId}' AND u.""IsDeleted"" = false and  u.""CompanyId"" = '{_repo.UserContext.CompanyId}'
                            LEFT JOIN public.""DocumentPermission"" as dup ON n.""Id"" = dp.""NoteId"" AND dup.""IsDeleted"" = false and  dup.""CompanyId"" = '{_repo.UserContext.CompanyId}'
                            LEFT JOIN public.""UserGroupUser"" uu ON dup.""PermittedUserGroupId"" = uu.""UserGroupId"" and uu.""Id""=u.""Id"" AND uu.""IsDeleted"" = false and  uu.""CompanyId"" = '{_repo.UserContext.CompanyId}'
                            join public.""NtsNote"" as dn on dn.""Id""=dup.""NoteId"" or dn.""Id""=dp.""NoteId"" AND dn.""IsDeleted"" = false and  dn.""CompanyId"" = '{_repo.UserContext.CompanyId}'
                            join cms.""N_DMS_Workspace"" as ws on ws.""NtsNoteId""=dn.""Id"" AND ws.""IsDeleted"" = false and  ws.""CompanyId"" = '{_repo.UserContext.CompanyId}'
                            where n.""IsDeleted""=false and  n.""CompanyId"" = '{_repo.UserContext.CompanyId}' and (n.""IsArchived"" <> true or n.""IsArchived"" is null)";

            var list = await _querydocList.ExecuteQueryList(query, null);
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
            var selectQry = "";
            var i = 1;
            var templateList = new List<TemplateViewModel>();

            var tempCategory = await _templateCategoryBusiness.GetList(x => x.Code == "GENERAL_DOCUMENT" && x.TemplateType == TemplateTypeEnum.Note);

            foreach (var item in tempCategory)
            {
                templateList = await _templateBusiness.GetList(x => x.TemplateCategoryId == item.Id && x.PortalId == _repo.UserContext.PortalId);
            }

            foreach (var item in templateList.Where(x => x.TableMetadataId != null))
            {
                var tableMeta = await _tableMetadataBusiness.GetSingle(x => x.Id == item.TableMetadataId);
                if (item.Code == "GENERAL_DOCUMENT" || item.Code == "HALUL_REQUEST_FOR_INSPECTION" || item.Code == "GALFAR_REQUEST_FOR_INSPECTION")
                {

                }
                else
                {
                    if (i != 1)
                    {
                        selectQry += " union ";
                    }

                    //selectQry = @$"{ selectQry}
                    //    select n.""Id"" as DocumentId,n.""NoteNo"" as NoteNo,n.""NoteSubject"" as DocumentName,n.""CreatedDate"",r.""Name"" as Revision,d.""Name"" as Discipline,
                    //    ss.""Name"" as StageStatus,p.""outgoingIssueCodes"" as IssueCode,p.""qPTransmittalNumber"" as TransmittalNo,p.""galfarDueDate"" as DueDate,
                    //    p.""dateOfReturn"" as SubmittedDate, t.""Id"" as TemplateId
                    //    FROM public.""NtsNote"" n
                    //    JOIN public.""Template"" t ON n.""TemplateId"" = t.""Id"" AND t.""IsDeleted"" = false
                    //    JOIN cms.""{tableMeta.Name}"" p ON n.""Id"" = p.""NtsNoteId"" and p.""IsDeleted"" = false
                    //    Join public.""LOV"" as ts on n.""NoteStatusId""=ts.""Id"" and ts.""Code""!='NOTE_STATUS_DRAFT'
                    //    Join public.""LOV"" as r on p.""revision""=r.""Id"" #REVISION#
                    // Join public.""LOV"" as d on p.""discipline""=d.""Id"" #DISCIPLINE#
                    // Join public.""LOV"" as ss on p.""stageStatus""=ss.""Id"" and ss.""IsDeleted""=false
                    //    LEFT JOIN public.""DocumentPermission"" dp ON n.""Id"" = dp.""NoteId"" 
                    // left Join public.""User"" as u on dp.""PermittedUserId""=u.""Id"" and u.""Id""='{userId}'
                    //    LEFT JOIN public.""DocumentPermission"" dup ON n.""Id"" = dp.""NoteId"" 
                    //    LEFT JOIN public.""UserGroupUser"" uu ON dup.""PermittedUserGroupId"" = uu.""UserGroupId"" and uu.""Id""='{userId}'
                    //    left join public.""NtsNote"" as dn on dn.""Id""=dup.""NoteId"" or dn.""Id""=dp.""NoteId"" 
                    //    left join cms.""N_GENERAL_FOLDER_WORKSPACE"" as ws on ws.""NtsNoteId""=dn.""Id""
                    //    where n.""IsDeleted""=false ";

                    selectQry = $@"{ selectQry}
                                select n.""Id"" as DocumentId,n.""NoteNo"" as NoteNo,n.""NoteSubject"" as DocumentName,n.""CreatedDate"",r.""Name"" as Revision,d.""Name"" as Discipline,
                    ss.""Name"" as StageStatus,p.""qPTransmittalNumber"" as TransmittalNo,p.""galfarDueDate"" as DueDate,
                    p.""dateOfReturn"" as SubmittedDate, tm.""Id"" as TemplateId ,coalesce(LOV_code.""Name"",LOV_code.""Code"") as IssueCode 
from 
public.""NtsNote"" as n 
join public.""LOV"" as lv on lv.""Id""=n.""NoteStatusId"" and lv.""IsDeleted""=false and  lv.""CompanyId"" = '{_repo.UserContext.CompanyId}'
join public.""User"" as u on u.""Id""=n.""OwnerUserId"" and u.""IsDeleted""=false and  u.""CompanyId"" = '{_repo.UserContext.CompanyId}'
join public.""Template"" as tm on tm.""Id""=n.""TemplateId"" --and tm.""Code""='PROJECT_DOCUMENTS' 
and tm.""IsDeleted""=false and  tm.""CompanyId"" = '{_repo.UserContext.CompanyId}'
join cms.""{tableMeta.Name}"" as p on p.""NtsNoteId""=n.""Id"" and p.""IsDeleted""=false and  p.""CompanyId"" = '{_repo.UserContext.CompanyId}'
left join public.""LOV"" as r on r.""Id""=p.""revision"" and r.""IsDeleted""=false and  r.""CompanyId"" = '{_repo.UserContext.CompanyId}'
left join public.""LOV"" as d on d.""Id""=p.""discipline"" and d.""IsDeleted""=false and  d.""CompanyId"" = '{_repo.UserContext.CompanyId}'
left join public.""LOV"" as ss on ss.""Id""=p.""stageStatus"" and ss.""IsDeleted""=false and  ss.""CompanyId"" = '{_repo.UserContext.CompanyId}'
left join public.""LOV"" as LOV_code on LOV_code.""Id""=p.""code"" and LOV_code.""IsDeleted""=false and  LOV_code.""CompanyId"" = '{_repo.UserContext.CompanyId}'
 where 1=1 and n.""IsDeleted""=false and  n.""CompanyId"" = '{_repo.UserContext.CompanyId}'and  n.""Id"" in ('" + documentIds + @"') and lv.""Code"" <> 'NOTE_STATUS_DRAFT' #DISCIPLINE# #REVISION#";

                    i++;
                }
            }

            var searchDiscipline = "";
            if (discipline.IsNotNullAndNotEmpty())
            {
                searchDiscipline = $@" and d.""Code""='{discipline}'";
            }
            selectQry = selectQry.Replace("#DISCIPLINE#", searchDiscipline);

            var searchRevision = "";
            if (revesion.IsNotNullAndNotEmpty())
            {
                searchRevision = $@" and r.""Code""='{revesion}'";
            }
            selectQry = selectQry.Replace("#REVISION#", searchRevision);


            var result2 = await _querydocList.ExecuteQueryList(selectQry, null);

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
                    var Query = $@"select  max( split_part(n.""NoteNo""  COLLATE ""tr-TR-x-icu"", '-', 5)) as maxNumber
                     from Public.""TemplateCategory"" tc inner join
            
                             public.""Template"" as t on tc.""Id""=t.""TemplateCategoryId"" and t.""IsDeleted""=false and  t.""CompanyId"" = '{_repo.UserContext.CompanyId}'
            				Join public.""NtsNote"" as n on n.""TemplateId""=t.""Id"" and n.""IsDeleted""=false and  n.""CompanyId"" = '{_repo.UserContext.CompanyId}'
            				Left Join public.""User"" as u on n.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and  u.""CompanyId"" = '{_repo.UserContext.CompanyId}'
                            Left Join public.""LOV"" as lov on n.""NoteStatusId""=lov.""Id"" and lov.""IsDeleted""=false and  lov.""CompanyId"" = '{_repo.UserContext.CompanyId}'
            				 where tc.""Code"" in ('GENERAL_DOCUMENT') and tc.""IsDeleted""=false and  tc.""CompanyId"" = '{_repo.UserContext.CompanyId}' and t.""Code""='GALFAR_REQUEST_FOR_INSPECTION'
                         and n.""NoteNo""  like '{startstr}%' COLLATE ""tr-TR-x-icu""";


                    var maxNumber = await _queryRepo1.ExecuteScalar<string>(Query, null);

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
                    var Query = $@"select  max( split_part(n.""NoteNo""  COLLATE ""tr-TR-x-icu"", '-', 4)) as maxNumber
                     from Public.""TemplateCategory"" tc inner join
                             public.""Template"" as t on tc.""Id""=t.""TemplateCategoryId"" and t.""IsDeleted""=false and  t.""CompanyId"" = '{_repo.UserContext.CompanyId}'
            				Join public.""NtsNote"" as n on n.""TemplateId""=t.""Id"" and n.""IsDeleted""=false and  n.""CompanyId"" = '{_repo.UserContext.CompanyId}'
							join cms.""N_GENERAL_DOCUMENT_HALULREQFORINSPECTION"" H on n.""Id""=H.""NtsNoteId"" and H.""IsDeleted""=false and  H.""CompanyId"" = '{_repo.UserContext.CompanyId}'

                            Left Join public.""User"" as u on n.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and  u.""CompanyId"" = '{_repo.UserContext.CompanyId}'
                            Left Join public.""LOV"" as lov on n.""NoteStatusId""=lov.""Id"" and lov.""IsDeleted""=false and  lov.""CompanyId"" = '{_repo.UserContext.CompanyId}'
							left Join public.""LOV"" as lovd on H.""discipline""=lovd.""Id"" and lovd.""IsDeleted""=false and  lovd.""CompanyId"" = '{_repo.UserContext.CompanyId}'
            			     where tc.""Code"" in ('GENERAL_DOCUMENT') and tc.""IsDeleted""=false and  tc.""CompanyId"" = '{_repo.UserContext.CompanyId}' and t.""Code""='HALUL_REQUEST_FOR_INSPECTION'
                            and lovd.""Code""='{discipline}'";


                    var maxNumber = await _queryRepo1.ExecuteScalar<string>(Query, null);

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
            var cypher = $@"select  max( split_part(n.""NoteNo""  COLLATE ""tr-TR-x-icu"", '-', 5)) as maxNumber
                     from Public.""TemplateCategory"" tc inner join
            
                             public.""Template"" as t on tc.""Id""=t.""TemplateCategoryId"" and t.""IsDeleted""=false and  t.""CompanyId"" = '{_repo.UserContext.CompanyId}'
            				Join public.""NtsNote"" as n on n.""TemplateId""=t.""Id"" and n.""IsDeleted""=false and  n.""CompanyId"" = '{_repo.UserContext.CompanyId}'
            				Left Join public.""User"" as u on n.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and  u.""CompanyId"" = '{_repo.UserContext.CompanyId}'
                            Left Join public.""LOV"" as lov on n.""NoteStatusId""=lov.""Id"" and lov.""IsDeleted""=false and  lov.""CompanyId"" = '{_repo.UserContext.CompanyId}'
            				 where tc.""Code"" in ('GENERAL_DOCUMENT') and tc.""IsDeleted""=false and  tc.""CompanyId"" = '{_repo.UserContext.CompanyId}' and t.""Code""='GALFAR_REQUEST_FOR_INSPECTION' 
                         and n.""NoteNo""  like '{_firstStr}%' COLLATE ""tr-TR-x-icu"" and  n.""NoteNo""  like '%{_lastStr}' COLLATE ""tr-TR-x-icu"" 
                                  ";

            var maxNumber = await _queryRepo1.ExecuteScalar<string>(cypher, null);
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
            var Query = $@"select  max( split_part(n.""NoteNo""  COLLATE ""tr-TR-x-icu"", '-', 4)) as maxNumber
                     from Public.""TemplateCategory"" tc inner join
                             public.""Template"" as t on tc.""Id""=t.""TemplateCategoryId"" and t.""IsDeleted""=false and  t.""CompanyId"" = '{_repo.UserContext.CompanyId}'
            				Join public.""NtsNote"" as n on n.""TemplateId""=t.""Id"" and n.""IsDeleted""=false and  n.""CompanyId"" = '{_repo.UserContext.CompanyId}'
							join cms.""N_GENERAL_DOCUMENT_HALULREQFORINSPECTION"" H on n.""Id""=H.""NtsNoteId"" and H.""IsDeleted""=false and  H.""CompanyId"" = '{_repo.UserContext.CompanyId}'

                            Left Join public.""User"" as u on n.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and  u.""CompanyId"" = '{_repo.UserContext.CompanyId}'
                            Left Join public.""LOV"" as lov on n.""NoteStatusId""=lov.""Id"" and lov.""IsDeleted""=false and  lov.""CompanyId"" = '{_repo.UserContext.CompanyId}'
							left Join public.""LOV"" as lovd on H.""discipline""=lovd.""Id"" and lovd.""IsDeleted""=false and  lovd.""CompanyId"" = '{_repo.UserContext.CompanyId}'
            			     where tc.""Code"" in ('GENERAL_DOCUMENT') and tc.""IsDeleted""=false and  tc.""CompanyId"" = '{_repo.UserContext.CompanyId}' and t.""Code""='HALUL_REQUEST_FOR_INSPECTION' 
                            and lovd.""Code""='{discipline}' and  n.""NoteNo""  like '%{_lastStr}' COLLATE ""tr-TR-x-icu"" ";

            var maxNumber = await _queryRepo1.ExecuteScalar<string>(Query, null);
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
            var cypher = $@"select n.* from 
public.""TemplateCategory"" as c 
join public.""Template"" as tr on tr.""TemplateCategoryId""=c.""Id"" and tr.""IsDeleted""=false and  tr.""CompanyId"" = '{_repo.UserContext.CompanyId}' and c.""Code"" in ('GENERAL_DOCUMENT')
join public.""NtsNote"" as n on n.""TemplateId""=tr.""Id"" and n.""IsDeleted""=false and n.""IsArchived""=false and  n.""CompanyId"" = '{_repo.UserContext.CompanyId}'
join public.""NtsNote"" as p on n.""ParentNoteId""=p.""Id"" and p.""IsDeleted""=false and p.""IsArchived""=false and  p.""CompanyId"" = '{_repo.UserContext.CompanyId}' 
where p.""Id""='{ParentId}' and c.""IsDeleted""=false and n.""IsArchived""=false and  c.""CompanyId"" = '{_repo.UserContext.CompanyId}'
";




            var value = await _queryRepo1.ExecuteQueryList<NoteViewModel>(cypher, null);


            return value;
        }
        public async Task<IList<FolderViewModel>> GetAllParentByChildId(string ChildId, List<FolderViewModel> ParentList)
        {
            var cypher = $@"select  p.*,t.""Code"" as FolderCode,tc.""Code"" as TemaplateMasterCatCode,p.""NoteSubject"" as Name, p.""ParentNoteId"" as ParentId
from public.""NtsNote"" as p 
join public.""NtsNote"" as n on p.""Id""=n.""ParentNoteId"" and p.""IsDeleted""=false and p.""IsArchived""=false and p.""CompanyId"" = '{_repo.UserContext.CompanyId}'  
join public.""Template"" as t on t.""Id""=p.""TemplateId"" and t.""IsDeleted""=false and t.""CompanyId"" = '{_repo.UserContext.CompanyId}'
join public.""TemplateCategory"" as tc on t.""TemplateCategoryId""=tc.""Id"" and tc.""IsDeleted""=false and tc.""CompanyId"" = '{_repo.UserContext.CompanyId}'
where n.""Id""='{ChildId}' and n.""IsDeleted""=false and n.""IsArchived""=false and n.""CompanyId"" = '{_repo.UserContext.CompanyId}' 
";
           
             ParentList = await _queryRepo1.ExecuteQueryList<FolderViewModel>(cypher, null);
            //foreach (var folder in list)
            //{
            //    ParentList.Add(folder);
            //    await GetAllParentByChildId(folder.Id, ParentList);
            //}
            return ParentList;
        }
        public async Task<IList<FolderViewModel>> GetAllChildByParentId(string ParentId, List<FolderViewModel> FolderList)
        {
            var cypher = $@"select  n.*,t.""Code"" as FolderCode,tc.""Code"" as TemaplateMasterCatCode,n.""NoteSubject"" as Name, p.""Id"" as ParentId
from public.""NtsNote"" as n 
join public.""NtsNote"" as p on p.""Id""=n.""ParentNoteId"" and p.""IsDeleted""=false and p.""IsArchived""=false and p.""CompanyId"" = '{_repo.UserContext.CompanyId}'  
join public.""Template"" as t on t.""Id""=n.""TemplateId"" and t.""IsDeleted""=false and t.""CompanyId"" = '{_repo.UserContext.CompanyId}'
join public.""TemplateCategory"" as tc on t.""TemplateCategoryId""=tc.""Id"" and tc.""IsDeleted""=false and tc.""CompanyId"" = '{_repo.UserContext.CompanyId}'
where p.""Id""='{ParentId}' and n.""IsDeleted""=false and n.""IsArchived""=false and n.""CompanyId"" = '{_repo.UserContext.CompanyId}'
";
            //var cypher = @"match (n: NTS_Note{ IsDeleted: 0,Status: 'Active'})
            //                     -[:R_Note_Parent_Note]->(p:NTS_Note{ IsDeleted: 0,Status: 'Active',Id:{ParentId}})                                                           
            //                    return n,n.Subject as Name, p.Id as ParentId";
            var list = await _queryRepo1.ExecuteQueryList<FolderViewModel>(cypher, null);
            foreach (var folder in list)
            {
                FolderList.Add(folder);
                await GetAllChildByParentId(folder.Id, FolderList);
            }
            return FolderList;
        }
        public async Task<IList<FolderViewModel>> GetAllPermissionChildByParentId(string ParentId, List<FolderViewModel> FolderList)
        {
            var cypher = $@"WITH RECURSIVE Folder AS(
            select  n.*,t.""Code"" as FolderCode,tc.""Code"" as TemaplateMasterCatCode,n.""NoteSubject"" as Name,n.""ParentNoteId"" as ParentId
            from public.""NtsNote"" as n 
            --join public.""NtsNote"" as p on p.""Id""=n.""ParentNoteId"" and p.""IsDeleted""=false and p.""IsArchived""=false and p.""CompanyId"" = '{_repo.UserContext.CompanyId}'  
            join public.""Template"" as t on t.""Id""=n.""TemplateId"" and t.""IsDeleted""=false and t.""CompanyId"" = '{_repo.UserContext.CompanyId}'
            join public.""TemplateCategory"" as tc on t.""TemplateCategoryId""=tc.""Id"" and tc.""Code""='GENERAL_FOLDER' and tc.""IsDeleted""=false and tc.""CompanyId"" = '{_repo.UserContext.CompanyId}'
            where n.""ParentNoteId""='{ParentId}' and n.""IsDeleted""=false and n.""IsArchived""=false and n.""CompanyId"" = '{_repo.UserContext.CompanyId}'
            union all
            select n.*,t.""Code"" as FolderCode,tc.""Code"" as TemaplateMasterCatCode,n.""NoteSubject"" as Name, n.""ParentNoteId"" as ParentId
            from public.""NtsNote"" as n 
            join Folder as f on n.""ParentNoteId""=f.""Id""
            join public.""Template"" as t on t.""Id""=n.""TemplateId"" and t.""IsDeleted""=false and t.""CompanyId"" = '{_repo.UserContext.CompanyId}'
            join public.""TemplateCategory"" as tc on t.""TemplateCategoryId""=tc.""Id"" and tc.""Code""='GENERAL_FOLDER' and tc.""IsDeleted""=false and tc.""CompanyId"" = '{_repo.UserContext.CompanyId}'
            where n.""IsDeleted""=false and n.""IsArchived""=false and n.""CompanyId"" = '{_repo.UserContext.CompanyId}' 
            )
            select * from Folder
            ";
            //var cypher = @"match (n: NTS_Note{ IsDeleted: 0,Status: 'Active'})
            //                     -[:R_Note_Parent_Note]->(p:NTS_Note{ IsDeleted: 0,Status: 'Active',Id:{ParentId}})                                                           
            //                    return n,n.Subject as Name, p.Id as ParentId";
            var list = await _queryRepo1.ExecuteQueryList<FolderViewModel>(cypher, null);

            if(list.Count>0)
            {
                FolderList.AddRange(list);
            }

            var cypher1 = $@"select distinct doc.*,doc.""NoteSubject"" as Name from public.""NtsTag"" as tag
                join public.""NtsNote"" as doc on doc.""Id""=tag.""TagSourceReferenceId"" and doc.""IsDeleted""=false and doc.""IsArchived""=false 

                where ""NtsId""='{ParentId}'
            ";
            //var cypher = @"match (n: NTS_Note{ IsDeleted: 0,Status: 'Active'})
            //                     -[:R_Note_Parent_Note]->(p:NTS_Note{ IsDeleted: 0,Status: 'Active',Id:{ParentId}})                                                           
            //                    return n,n.Subject as Name, p.Id as ParentId";
            var list1 = await _queryRepo1.ExecuteQueryList<FolderViewModel>(cypher1, null);
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
            var cypher = $@"WITH RECURSIVE Folder AS(
            select  n.*,t.""Code"" as FolderCode,tc.""Code"" as TemaplateMasterCatCode,n.""NoteSubject"" as Name,n.""ParentNoteId"" as ParentId
            from public.""NtsNote"" as n 
            --join public.""NtsNote"" as p on p.""Id""=n.""ParentNoteId"" and p.""IsDeleted""=false and p.""IsArchived""=false and p.""CompanyId"" = '{_repo.UserContext.CompanyId}'  
            join public.""Template"" as t on t.""Id""=n.""TemplateId"" and t.""IsDeleted""=false and t.""CompanyId"" = '{_repo.UserContext.CompanyId}'
            join public.""TemplateCategory"" as tc on t.""TemplateCategoryId""=tc.""Id"" and tc.""Code""='GENERAL_FOLDER' and tc.""IsDeleted""=false and tc.""CompanyId"" = '{_repo.UserContext.CompanyId}'
            left join(select np.*,case when npu is null then npwgu.""Id"" else npu.""Id"" end as ""UserId"" 
            from  public.""DocumentPermission"" as np  
            left join public.""User"" as npu on npu.""Id""=np.""PermittedUserId"" and npu.""Id""='{UserId}' and
            npu.""IsDeleted""=false and npu.""CompanyId""='{_repo.UserContext.CompanyId}' 			
			left join public.""UserGroupUser"" as npwg on npwg.""UserGroupId""=np.""PermittedUserGroupId""  and npwg.""IsDeleted""=false and npwg.""CompanyId""='{_repo.UserContext.CompanyId}' 
			left join public.""User"" as npwgu on npwgu.""Id""=npwg.""UserId""  and npwgu.""Id""='{UserId}'	 and npwgu.""IsDeleted""=false and npwgu.""CompanyId""='{_repo.UserContext.CompanyId}'
			where np.""IsDeleted""=false)
			as np on np.""NoteId""=n.""Id"" and np.""UserId""='{UserId}'
            where n.""ParentNoteId""='{ParentId}' and n.""IsDeleted""=false and n.""IsArchived""=false and n.""CompanyId"" = '{_repo.UserContext.CompanyId}'
            union all
            select n.*,t.""Code"" as FolderCode,tc.""Code"" as TemaplateMasterCatCode,n.""NoteSubject"" as Name, n.""ParentNoteId"" as ParentId
            from public.""NtsNote"" as n 
            join Folder as f on n.""ParentNoteId""=f.""Id""
            join public.""Template"" as t on t.""Id""=n.""TemplateId"" and t.""IsDeleted""=false and t.""CompanyId"" = '{_repo.UserContext.CompanyId}'
            join public.""TemplateCategory"" as tc on t.""TemplateCategoryId""=tc.""Id"" and tc.""Code""='GENERAL_FOLDER' and tc.""IsDeleted""=false and tc.""CompanyId"" = '{_repo.UserContext.CompanyId}'
            left join(select np.*,case when npu is null then npwgu.""Id"" else npu.""Id"" end as ""UserId"" 
            from  public.""DocumentPermission"" as np  
            left join public.""User"" as npu on npu.""Id""=np.""PermittedUserId"" and npu.""Id""='{UserId}' and
            npu.""IsDeleted""=false and npu.""CompanyId""='{_repo.UserContext.CompanyId}' 			
			left join public.""UserGroupUser"" as npwg on npwg.""UserGroupId""=np.""PermittedUserGroupId""  and npwg.""IsDeleted""=false and npwg.""CompanyId""='{_repo.UserContext.CompanyId}' 
			left join public.""User"" as npwgu on npwgu.""Id""=npwg.""UserId""  and npwgu.""Id""='{UserId}'	 and npwgu.""IsDeleted""=false and npwgu.""CompanyId""='{_repo.UserContext.CompanyId}'
			where np.""IsDeleted""=false)
			as np on np.""NoteId""=n.""Id"" and np.""UserId""='{UserId}'
            where n.""IsDeleted""=false and n.""IsArchived""=false and n.""CompanyId"" = '{_repo.UserContext.CompanyId}' 
            )
            select * from Folder
            ";            
            var list = await _queryRepo1.ExecuteQueryList<FolderViewModel>(cypher, null);
            var cypher1 = $@"select distinct doc.*,doc.""NoteSubject"" as Name from public.""NtsTag"" as tag
                join public.""NtsNote"" as doc on doc.""Id""=tag.""TagSourceReferenceId"" and doc.""IsDeleted""=false and doc.""IsArchived""=false 
                left join(select np.*,case when npu is null then npwgu.""Id"" else npu.""Id"" end as ""UserId"" 
                from  public.""DocumentPermission"" as np  
                left join public.""User"" as npu on npu.""Id""=np.""PermittedUserId"" and npu.""Id""='{UserId}' and
                npu.""IsDeleted""=false and npu.""CompanyId""='{_repo.UserContext.CompanyId}' 			
			    left join public.""UserGroupUser"" as npwg on npwg.""UserGroupId""=np.""PermittedUserGroupId""  and npwg.""IsDeleted""=false and npwg.""CompanyId""='{_repo.UserContext.CompanyId}' 
			    left join public.""User"" as npwgu on npwgu.""Id""=npwg.""UserId""  and npwgu.""Id""='{UserId}'	 and npwgu.""IsDeleted""=false and npwgu.""CompanyId""='{_repo.UserContext.CompanyId}'
			    where np.""IsDeleted""=false)
			    as np on np.""NoteId""=doc.""Id"" and np.""UserId""='{UserId}'
                where ""NtsId""='{ParentId}'
            ";            
            var list1 = await _queryRepo1.ExecuteQueryList<FolderViewModel>(cypher1, null);
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
                var cypher = $@"
            Select m.""PermissionType"" as PermissionType, m.""Access"" as Access, m.""AppliesTo"" as AppliesTo, m.""Id"" as Id, a.""Id"" as PermittedUserGroupId,m.""InheritedFrom"" as InheritedFrom,
            case when u.""Id"" is not null then u.""Name"" else a.""Name"" end as Principal,m.""Isowner"" as Isowner,m.""Isowner"" as IsOwner,u.""Id"" as PermittedUserId,n.""DisablePermissionInheritance"" as DisablePermissionInheritance,m.""IsInherited"" as IsInherited,
            m.""IsInheritedFromChild"" as IsInheritedFromChild,m.""NoteId"" as NoteId
            from 
            public.""DocumentPermission"" as m 
            join public.""NtsNote"" as n on n.""Id""=m.""NoteId""  and n.""IsDeleted""=false and n.""CompanyId"" = '{_repo.UserContext.CompanyId}' 
            left join public.""User"" as u on u.""Id""=m.""PermittedUserId"" and u.""IsDeleted""=false and u.""CompanyId"" = '{_repo.UserContext.CompanyId}'
            left join public.""UserGroup"" as a on a.""Id""=m.""PermittedUserGroupId"" and a.""IsDeleted""=false and a.""CompanyId"" = '{_repo.UserContext.CompanyId}'
            where m.""IsDeleted""=false and m.""NoteId"" in ('{id}') and m.""CompanyId"" = '{_repo.UserContext.CompanyId}'
          
            ";

               list = await _queryRepo1.ExecuteQueryList<DocumentPermissionViewModel>(cypher, null);
            }

            return list;
        }
        public async Task<IList<FolderViewModel>> GetAllChildDocumentByParentId(string ParentId, List<FolderViewModel> FolderList)
        {
            var cypher = $@"select  n.*,tc.""Code"" as TemaplateMasterCatCode,n.""NoteSubject"" as Name, p.""Id"" as ParentId
from public.""NtsNote"" as n 
join public.""NtsNote"" as p on p.""Id""=n.""ParentNoteId"" and  p.""IsDeleted""=false and p.""IsArchived""=false and p.""CompanyId"" = '{_repo.UserContext.CompanyId}'
join public.""Template"" as t on t.""Id""=n.""TemplateId"" and t.""IsDeleted""=false  and t.""CompanyId"" = '{_repo.UserContext.CompanyId}'
join public.""TemplateCategory"" as tc on t.""TemplateCategoryId""=tc.""Id"" and tc.""IsDeleted""=false and tc.""CompanyId"" = '{_repo.UserContext.CompanyId}'
where p.""Id""='{ParentId}' and  n.""IsDeleted""=false and n.""IsArchived""=false and n.""CompanyId"" = '{_repo.UserContext.CompanyId}' and  (n.""IsArchived"" <> true or n.""IsArchived"" is null)
";
            //var cypher = @"match (n: NTS_Note{ IsDeleted: 0,Status: 'Active'})
            //                     -[:R_Note_Parent_Note]->(p:NTS_Note{ IsDeleted: 0,Status: 'Active',Id:{ParentId}})                                                           
            //                    return n,n.Subject as Name, p.Id as ParentId";
            var list = await _queryRepo1.ExecuteQueryList<FolderViewModel>(cypher, null);
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
            var query = $@"select distinct n.""Id"" as Id from public.""DocumentPermission"" as dp 
                            join public.""NtsNote"" as n on n.""Id"" = dp.""NoteId"" and n.""IsDeleted"" = false and n.""CompanyId"" = '{_repo.UserContext.CompanyId}'
                            join public.""Template"" as t on t.""Id"" = n.""TemplateId"" and t.""IsDeleted"" = false and t.""CompanyId"" = '{_repo.UserContext.CompanyId}'
                            join public.""TemplateCategory"" as tc on tc.""Id"" = t.""TemplateCategoryId"" and tc.""IsDeleted"" = false and tc.""CompanyId"" = '{_repo.UserContext.CompanyId}'
                            where dp.""PermittedUserId"" = '{userId}'
                            and tc.""Code"" ='GENERAL_DOCUMENT' and dp.""IsDeleted"" = false and dp.""CompanyId"" = '{_repo.UserContext.CompanyId}'";
            var result = await _queryRepo1.ExecuteQueryList<NoteTemplateViewModel>(query, null);
            return result;
        }

        public async Task<List<AttachmentViewModel>> GetUserAttachments(string userId, string portalId)
        {
            var attachments = new List<AttachmentViewModel>();

            //var result = await _fileBusiness.GetList( x => x.ReferenceTypeCode == ReferenceTypeEnum.NTS_Note || x.ReferenceTypeCode == ReferenceTypeEnum.NTS_Service
            //    || x.ReferenceTypeCode == ReferenceTypeEnum.NTS_Task || x.ReferenceTypeCode == ReferenceTypeEnum.Page || x.ReferenceTypeCode == ReferenceTypeEnum.Form);


            var query = $@"select f.""Id"" as Id, f.""FileName"" as FullName,f.""ReferenceTypeId""  as ReferenceId, 
                        f.""SnapshotMongoId"" as SnapshotMongoId, f.""ContentLength"" as ContentLength, f.""CreatedDate"" as CreatedDate, n.""TemplateCode"" as TemplateCode
                        from public.""File"" as f
                        join public.""NtsNote"" as n on n.""Id"" = f.""ReferenceTypeId"" and n.""IsDeleted"" = false and n.""CompanyId"" = '{_repo.UserContext.CompanyId}'
                        join public.""DocumentPermission"" as dp on dp.""NoteId"" = n.""Id"" and dp.""IsDeleted"" = false and dp.""CompanyId"" = '{_repo.UserContext.CompanyId}'
                        join public.""User"" as u on u.""Id"" = n.""OwnerUserId"" and u.""IsDeleted"" = false and u.""CompanyId"" = '{_repo.UserContext.CompanyId}'
                        where f.""ReferenceTypeCode"" = 0 and u.""Id"" = '{userId}' and f.""PortalId"" = '{portalId}' and f.""IsDeleted"" = false and f.""CompanyId"" = '{_repo.UserContext.CompanyId}'
                        union
                        select f.""Id"" as Id, f.""FileName"" as FullName, f.""ReferenceTypeId""  as ReferenceId,
                        f.""SnapshotMongoId"" as SnapshotMongoId, f.""ContentLength"" as ContentLength, f.""CreatedDate"" as CreatedDate, n.""TemplateCode"" as TemplateCode
                        from public.""File"" as f
                        join public.""NtsNote"" as n on n.""Id"" = f.""ReferenceTypeId"" and n.""IsDeleted"" = false and n.""CompanyId"" = '{_repo.UserContext.CompanyId}'
                        join public.""DocumentPermission"" as dp on dp.""NoteId"" = n.""Id"" and dp.""IsDeleted"" = false and dp.""CompanyId"" = '{_repo.UserContext.CompanyId}'
                        join public.""User"" as u on u.""Id"" = n.""OwnerUserId"" and u.""IsDeleted"" = false and u.""CompanyId"" = '{_repo.UserContext.CompanyId}'
                        where f.""ReferenceTypeCode"" = 1 and u.""Id"" = '{userId}' and f.""PortalId"" = '{portalId}' and f.""IsDeleted"" = false and f.""CompanyId"" = '{_repo.UserContext.CompanyId}'
                        union
                        select f.""Id"" as Id, f.""FileName"" as FullName, f.""ReferenceTypeId""  as ReferenceId,
                        f.""SnapshotMongoId"" as SnapshotMongoId, f.""ContentLength"" as ContentLength, f.""CreatedDate"" as CreatedDate, n.""TemplateCode"" as TemplateCode
                        from public.""File"" as f
                        join public.""NtsNote"" as n on n.""Id"" = f.""ReferenceTypeId"" and n.""IsDeleted"" = false and n.""CompanyId"" = '{_repo.UserContext.CompanyId}'
                        join public.""DocumentPermission"" as dp on dp.""NoteId"" = n.""Id"" and dp.""IsDeleted"" = false and dp.""CompanyId"" = '{_repo.UserContext.CompanyId}'
                        join public.""User"" as u on u.""Id"" = n.""OwnerUserId"" and u.""IsDeleted"" = false and u.""CompanyId"" = '{_repo.UserContext.CompanyId}'
                        where f.""ReferenceTypeCode"" = 2 and u.""Id"" = '{userId}' and f.""PortalId"" = '{portalId}' and f.""IsDeleted"" = false and f.""CompanyId"" = '{_repo.UserContext.CompanyId}'
                        union
                        select f.""Id"" as Id, f.""FileName"" as FullName, f.""ReferenceTypeId""  as ReferenceId,
                        f.""SnapshotMongoId"" as SnapshotMongoId, f.""ContentLength"" as ContentLength, f.""CreatedDate"" as CreatedDate, n.""TemplateCode"" as TemplateCode
                        from public.""File"" as f
                        join public.""NtsNote"" as n on n.""Id"" = f.""ReferenceTypeId"" and n.""IsDeleted"" = false and n.""CompanyId"" = '{_repo.UserContext.CompanyId}'
                        join public.""DocumentPermission"" as dp on dp.""NoteId"" = n.""Id"" and dp.""IsDeleted"" = false and dp.""CompanyId"" = '{_repo.UserContext.CompanyId}'
                        join public.""User"" as u on u.""Id"" = n.""OwnerUserId"" and u.""IsDeleted"" = false and u.""CompanyId"" = '{_repo.UserContext.CompanyId}'
                        where f.""ReferenceTypeCode"" = 83  and u.""Id"" = '{userId}' and f.""PortalId"" = '{portalId}' and f.""IsDeleted"" = false and f.""CompanyId"" = '{_repo.UserContext.CompanyId}'
                        union
                        select f.""Id"" as Id, f.""FileName"" as FullName, f.""ReferenceTypeId""  as ReferenceId,
                        f.""SnapshotMongoId"" as SnapshotMongoId, f.""ContentLength"" as ContentLength, f.""CreatedDate"" as CreatedDate, n.""TemplateCode"" as TemplateCode
                        from public.""File"" as f
                        join public.""NtsNote"" as n on n.""Id"" = f.""ReferenceTypeId"" and n.""IsDeleted"" = false and n.""CompanyId"" = '{_repo.UserContext.CompanyId}'
                        join public.""DocumentPermission"" as dp on dp.""NoteId"" = n.""Id"" and dp.""IsDeleted"" = false and dp.""CompanyId"" = '{_repo.UserContext.CompanyId}'
                        join public.""User"" as u on u.""Id"" = n.""OwnerUserId"" and u.""IsDeleted"" = false and u.""CompanyId"" = '{_repo.UserContext.CompanyId}'
                        where f.""ReferenceTypeCode"" = 84  and u.""Id"" = '{userId}' and f.""PortalId"" = '{portalId}' and f.""IsDeleted"" = false and f.""CompanyId"" = '{_repo.UserContext.CompanyId}'";
            var result = await _queryRepo1.ExecuteQueryList<AttachmentViewModel>(query, null);
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
            var data = await _queryRepo1.ExecuteQuerySingle<ServiceViewModel>(query, null);
            return data;
        }

        public async Task<IList<FolderViewModel>> GetFolderByParent(string parentId,string noteId)
        {
            var folderlist = new List<FolderViewModel>();
          
            var cypher = $@" With Recursive NtsNote as (
	select n.""Id"" as Id, n.""NoteSubject"" as Name,n.""ParentNoteId"" as ParentId,1 as Level
	 
	from public.""Template"" as t
join public.""TemplateCategory"" as tc on tc.""Id""=t.""TemplateCategoryId"" and tc.""IsDeleted""=false and tc.""Code""='GENERAL_FOLDER'  and tc.""CompanyId""='{_repo.UserContext.CompanyId}' 
join public.""NtsNote"" as n on n.""TemplateId""=t.""Id"" and n.""IsDeleted""=false 
and  (n.""IsArchived"" <> true or n.""IsArchived"" is null) and n.""CompanyId""='{_repo.UserContext.CompanyId}' 
join public.""User"" as u on u.""Id""=n.""OwnerUserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}' 
	 where n.""Id""='{parentId}'
	 union 
	 select n.""Id"" as Id, n.""NoteSubject"" as Name,n.""ParentNoteId"" as ParentId,(pn.""level""+1) as Level
		from public.""Template"" as t
join public.""TemplateCategory"" as tc on tc.""Id""=t.""TemplateCategoryId"" and tc.""IsDeleted""=false and tc.""Code""='GENERAL_FOLDER'  and tc.""CompanyId""='{_repo.UserContext.CompanyId}' 
join public.""NtsNote"" as n on n.""TemplateId""=t.""Id"" and n.""IsDeleted""=false 
and  (n.""IsArchived"" <> true or n.""IsArchived"" is null) and n.""CompanyId""='{_repo.UserContext.CompanyId}' 
	join NtsNote as pn on pn.""parentid""=n.""Id""  and n.""IsDeleted""=false
	 
) select * from NtsNote 
union
select n.""Id"" as Id, n.""NoteSubject"" as Name,n.""ParentNoteId"" as ParentId,0 as Level
from public.""NtsNote"" as n where  (n.""IsArchived"" <> true or n.""IsArchived"" is null) and n.""CompanyId""='{_repo.UserContext.CompanyId}' 
and n.""Id""='{noteId}'
";



            var result =await _queryRepo1.ExecuteQueryList<FolderViewModel>(cypher, null);
            result = result.OrderByDescending(x => x.Level).ToList();
            return result;
        }


        public async Task <List<NoteLinkShareViewModel>> GetDocumentLinksData(long id)
        {

            var query = $@"select N.""Id"", S.""To"",S.""Link"",N.""ExpiryDate"" as ExpiryDate, U.""Name"" as ""From""
from public.""NtsNote""  as N inner join cms.""N_DMS_DocumentShareLink""  as S on N.""Id""=S.""NtsNoteId"" and S.""IsDeleted""=false and S.""CompanyId""='{_repo.UserContext.CompanyId}'
inner join public.""User"" as U on U.""Id""=N.""OwnerUserId"" and U.""IsDeleted""=false and U.""CompanyId""='{_repo.UserContext.CompanyId}'
where N.""IsDeleted""=false and N.""CompanyId""='{_repo.UserContext.CompanyId}' and N.""Id""='{id}'";


            var result = await _QueryShareLink.ExecuteQueryList<NoteLinkShareViewModel>(query, null);
            
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
            var cypher = $@"Select n.""Id"" from
public.""Template"" as tr 
join public.""TemplateCategory"" as tc on tc.""Id""=tr.""TemplateCategoryId"" and tc.""Code""='GENERAL_DOCUMENT' and tc.""IsDeleted""=false
join public.""NtsNote"" as n on n.""TemplateId""=tr.""Id"" and n.""IsDeleted""=false
join public.""NtsNote"" as p on p.""Id""=n.""ParentNoteId"" and p.""Id""='{ParentId}' where n.""NoteNo""='{code}' and tr.""IsDeleted""=false and p.""IsDeleted""=false ";
            return await _queryRepo1.ExecuteScalar<string>(cypher, null);

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
            var cypher = $@"Select n.* from 
public.""Template"" as tr 
join public.""TemplateCategory"" as tc on tc.""Id""=tr.""TemplateCategoryId"" and tc.""Code""='GENERAL_FOLDER' and tc.""IsDeleted""=false
join public.""NtsNote"" as n on n.""TemplateId""=tr.""Id"" and n.""IsDeleted""=false
join public.""NtsNote"" as p on p.""Id""=n.""ParentNoteId"" and p.""Id""='{ParentId}' where n.""NoteSubject""='{code}' and tr.""IsDeleted""=false and p.""IsDeleted""=false
";



            var value =await  _queryRepo1.ExecuteQueryList<NoteViewModel>(cypher, null);
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
            var cypher = $@"Select n.* from 
public.""Template"" as tr 
join public.""TemplateCategory"" as tc on tc.""Id""=tr.""TemplateCategoryId"" and tc.""Code""='GENERAL_DOCUMENT' and tc.""IsDeleted""=false
join public.""NtsNote"" as n on n.""TemplateId""=tr.""Id"" and n.""IsDeleted""=false
join public.""NtsNote"" as p on p.""Id""=n.""ParentNoteId"" and p.""Id""='{ParentId}' where n.""NoteSubject""='{code}' and tr.""IsDeleted""=false and p.""IsDeleted""=false
";

            var value =await  _querydocList.ExecuteQueryList<NoteViewModel>(cypher, null);
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
            var cypher = $@" With Recursive NtsNote as (
                select n.""Id"" as ""Id"", n.""NoteSubject"" as ""Name"",n.""ParentNoteId"" as ""ParentId"",1 as ""Level""
	 
                from public.""Template"" as t
                join public.""TemplateCategory"" as tc on tc.""Id""=t.""TemplateCategoryId"" and tc.""IsDeleted""=false  and tc.""CompanyId""='{_repo.UserContext.CompanyId}' 
                join public.""NtsNote"" as n on n.""TemplateId""=t.""Id"" and n.""IsDeleted""=false 
                and  (n.""IsArchived"" <> true or n.""IsArchived"" is null) and n.""CompanyId""='{_repo.UserContext.CompanyId}' 
                join public.""User"" as u on u.""Id""=n.""OwnerUserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}' 
                where n.""Id""='{noteId}'
                union 
                select n.""Id"" as ""Id"", n.""NoteSubject"" as ""Name"",n.""ParentNoteId"" as ""ParentId"",(pn.""Level""+1) as ""Level""
                from public.""Template"" as t
                join public.""TemplateCategory"" as tc on tc.""Id""=t.""TemplateCategoryId"" and tc.""IsDeleted""=false and tc.""Code""='GENERAL_FOLDER'  and tc.""CompanyId""='{_repo.UserContext.CompanyId}' 
                join public.""NtsNote"" as n on n.""TemplateId""=t.""Id"" and n.""IsDeleted""=false 
                and  (n.""IsArchived"" <> true or n.""IsArchived"" is null) and n.""CompanyId""='{_repo.UserContext.CompanyId}' 
                join NtsNote as pn on pn.""ParentId""=n.""Id""  and n.""IsDeleted""=false
	 
                ) select * from NtsNote where ""Id""<>'{noteId}'
                ";
            var result = await _queryRepo1.ExecuteQueryList<FolderViewModel>(cypher, null);
            result = result.OrderByDescending(x => x.Level).ToList();
            return result;
        }
        public async Task<IList<FolderViewModel>> GetAllDocuments()
        {
            var cypher = $@" select n.""Id"" as ""Id"", n.""NoteSubject"" as ""Name"",n.""ParentNoteId"" as ""ParentId""	 
                from public.""Template"" as t
                join public.""TemplateCategory"" as tc on tc.""Id""=t.""TemplateCategoryId"" and tc.""Code""='GENERAL_DOCUMENT' and tc.""IsDeleted""=false  and tc.""CompanyId""='{_repo.UserContext.CompanyId}' 
                join public.""NtsNote"" as n on n.""TemplateId""=t.""Id"" and n.""IsDeleted""=false order by n.""CreatedDate"" desc 
               ";
            var result = await _queryRepo1.ExecuteQueryList<FolderViewModel>(cypher, null);            
            return result;
        }
        public async Task<bool> UpdateTagsByDocumentIds(string ids)
        {
            var cypher = $@" update public.""NtsTag"" set ""IsDeleted""=true
            where ""Id"" in ({ids}) ";
            var result = await _queryRepo1.ExecuteQuerySingle(cypher, null);
            return true;
        }
        public async Task<bool> DeleteNotesbyNoteIds(string ids)
        {
            var cypher = $@" update public.""NtsNote"" set ""IsDeleted""=true
            where ""Id"" in ({ids}) ";
            var result = await _queryRepo1.ExecuteQuerySingle(cypher, null);
            return true;
        }
        public async Task<bool> ArchiveNotesbyNoteIds(string ids)
        {
            var cypher = $@" update public.""NtsNote"" set ""IsArchived""=true
            where ""Id"" in ({ids}) ";
            var result = await _queryRepo1.ExecuteQuerySingle(cypher, null);
            return true;
        }


        public async Task<bool> CheckMyWorkspaceExist(string UserId)
        {
            var Query = $@"select n.* from
public.""NtsNote"" as n 
				 join cms.""N_GENERAL_FOLDER_WORKSPACE"" as ws on ws.""NtsNoteId""=n.""Id"" and ws.""IsDeleted""=false and ws.""CompanyId""='{_repo.UserContext.CompanyId}'
				 join public.""LOV"" as lv on lv.""Id""=ws.""TypeId"" and lv.""IsDeleted""=false and lv.""CompanyId""='{_repo.UserContext.CompanyId}' and  lv.""Code"" in ('MY_WORKSPACE')
				 join public.""Template"" as tm on tm.""Id""=n.""TemplateId"" and tm.""IsDeleted""=false and tm.""CompanyId""='{_repo.UserContext.CompanyId}'
				 join public.""TemplateCategory"" as tc on tc.""Id""=tm.""TemplateCategoryId""  and tc.""IsDeleted""=false and tc.""CompanyId""='{_repo.UserContext.CompanyId}' and tc.""Code""='GENERAL_FOLDER'
				
                 where 
                  n.""OwnerUserId""='{UserId}' and  (n.""IsArchived"" <>'true' or n.""IsArchived"" is null) and n.""IsDeleted""=false and n .""CompanyId""='{_repo.UserContext.CompanyId}'";
            var data = await _queryRepo1.ExecuteQuerySingle(Query, null);
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

            var query = $@"Select b.* from cms.""N_GENERAL_DOCUMENT_HREmployeeBook"" as b
join public.""NtsNote"" as n on b.""NtsNoteId""=n.""Id"" and n.""IsDeleted""=false and n.""IsArchived""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""LOV"" as bs on n.""NoteStatusId""=bs.""Id"" and bs.""IsDeleted""=false and bs.""CompanyId""='{_repo.UserContext.CompanyId}'
where b.""EmployeeId""='{empId}' and b.""IsDeleted""=false and b.""CompanyId""='{_repo.UserContext.CompanyId}' and bs.""Code""!='NOTE_STATUS_EXPIRE' ";

            var result = await _queryRepo1.ExecuteQuerySingle<NoteTemplateViewModel>(query, null);
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
            var udfs = await _noteBusiness.GetUdfQuery(null, "GENERAL_DOCUMENT", null, null, "attachment,DocumentId", "fileAttachment,fileAttachmentId");
            var query = $@"
                Select f.""Id"" as Id
                ,coalesce(gen.""FileName"",'No File') as Name
                ,np.""PermissionType"" as PermissionType
                ,np.""Access"" as Access
                ,'' as InheritedFrom
                ,np.""AppliesTo"" as AppliesTo                
                ,f.""IsArchived"" as IsArchived
                ,0 as DocCount
                ,f.""DisablePermissionInheritance"" as DisablePermissionInheritance
                ,f.""NoteSubject"" as DocumentName
                ,f.""NoteDescription"" as Description
                ,f.""CreatedDate"" as CreatedDate
                ,f.""VersionNo"" as NoteVersionNo
                ,f.""LockStatus"" as LockStatus
                ,f.""LastUpdatedDate"" as LastUpdatedDate
                ,f.""StartDate"" as Start
                ,coalesce(f.""NoteSubject"", f.""NoteDescription"", '') as Title                
                , coalesce(udf.""DocumentId"",udf.""fileAttachmentId"") as DocumentId
                ,gen.""FileName"" as FileName
                , gen.""ContentLength"" as FileSize                
                ,f.""NoteNo"" as DocumentNo                
				
                from 
                public.""NtsTag"" as t
                join public.""NtsNote"" as f on f.""Id""=t.""TagSourceReferenceId"" and t.""NtsId""='{parentId}' and t.""TagId"" is null and t.""IsDeleted""=false
                and f.""IsDeleted""=false and f.""CompanyId""='{_repo.UserContext.CompanyId}' and  (f.""IsArchived"" <> true or f.""IsArchived"" is null)
                and t.""CompanyId""='{_repo.UserContext.CompanyId}'
                left join (" + udfs + $@") as udf on udf.""NtsNoteId""=f.""Id""
                left join(select np1.*,case when npu is null then npwgu.""Id"" else npu.""Id"" end as ""UserId""
                from  public.""DocumentPermission"" as np1
                left join public.""User"" as npu on npu.""Id""=np1.""PermittedUserId"" and npu.""Id""='{UserId}' and npu.""IsDeleted""=false and npu.""CompanyId""='{_repo.UserContext.CompanyId}' 
                left join public.""UserGroupUser"" as npwg on npwg.""UserGroupId""=np1.""PermittedUserGroupId""  and npwg.""IsDeleted""=false and npwg.""CompanyId""='{_repo.UserContext.CompanyId}' 
                left join public.""User"" as npwgu on npwgu.""Id""=npwg.""UserId""  and npwgu.""Id""='{UserId}'	 and npwgu.""IsDeleted""=false and npwgu.""CompanyId""='{_repo.UserContext.CompanyId}'
                where np1.""IsDeleted""=false	  )
                as np on np.""NoteId""=f.""Id"" and np.""UserId""='{UserId}'
                left join public.""File"" as gen on gen.""Id""=coalesce(udf.""DocumentId"",udf.""fileAttachmentId"") and gen.""IsDeleted""=false and gen.""CompanyId""='{_repo.UserContext.CompanyId}'
                ";

            var list = await _queryRepo1.ExecuteQueryList(query, null);
            list = list.DistinctBy(x => x.Id).ToList();            
            return list.Where(x => x.PermissionType == DmsPermissionTypeEnum.Allow).ToList();
        }
        public async Task<List<FolderViewModel>> GetAllPermittedWorkspaceFolderAndDocument(string UserId)
        {            
            var query = $@"
                Select distinct  n.*, np.""PermissionType""  as PermissionType
				from
				public.""NtsNote"" as n 
				join cms.""N_GENERAL_FOLDER_WORKSPACE"" as ws on ws.""NtsNoteId""=n.""Id"" and  (n.""IsArchived"" <> true or n.""IsArchived"" is null) and
                n.""IsDeleted""=false and ws.""IsDeleted""=false  and n.""CompanyId""='{_repo.UserContext.CompanyId}'				   
				join public.""Template"" as tm on tm.""Id""=n.""TemplateId"" and tm.""IsDeleted""=false and tm.""Code""='WORKSPACE_GENERAL' and tm.""CompanyId""='{_repo.UserContext.CompanyId}'
				join public.""TemplateCategory"" as tc on tc.""Id""=tm.""TemplateCategoryId""  and tc.""IsDeleted""=false and tc.""Code""='GENERAL_FOLDER' and tc.""CompanyId""='{_repo.UserContext.CompanyId}'				
				left join(select np.*,case when npu is null then npwgu.""Id"" else npu.""Id"" end as ""UserId"" 
                from  public.""DocumentPermission"" as np  
                left join public.""User"" as npu on npu.""Id""=np.""PermittedUserId"" and npu.""Id""='{UserId}' and
                npu.""IsDeleted""=false and npu.""CompanyId""='{_repo.UserContext.CompanyId}' 			
				left join public.""UserGroupUser"" as npwg on npwg.""UserGroupId""=np.""PermittedUserGroupId""  and npwg.""IsDeleted""=false and npwg.""CompanyId""='{_repo.UserContext.CompanyId}' 
				left join public.""User"" as npwgu on npwgu.""Id""=npwg.""UserId""  and npwgu.""Id""='{UserId}'	 and npwgu.""IsDeleted""=false and npwgu.""CompanyId""='{_repo.UserContext.CompanyId}'
				where np.""IsDeleted""=false)
				as np on np.""NoteId""=n.""Id"" and np.""UserId""='{UserId}'                
				where n.""IsDeleted""=false and ws.""IsDeleted""=false  and n.""CompanyId""='{_repo.UserContext.CompanyId}' 
               
                UNION                
                
                Select distinct f.*, np.""PermissionType""  as PermissionType								
                from
				public.""NtsNote"" as f  
				join cms.""N_GENERAL_FOLDER_GENERALFOLDER"" as ws1 on ws1.""NtsNoteId""=f.""Id"" and  (f.""IsArchived"" <> true or f.""IsArchived"" is null)	and f.""IsDeleted""=false and ws1.""IsDeleted""=false	and f.""CompanyId""='{_repo.UserContext.CompanyId}'				
				join public.""Template"" as tm on tm.""Id""=f.""TemplateId"" and tm.""IsDeleted""=false and tm.""Code""='GENERAL_FOLDER' and tm.""CompanyId""='{_repo.UserContext.CompanyId}'
				join public.""TemplateCategory"" as tc on tc.""Id""=tm.""TemplateCategoryId""  and tc.""IsDeleted""=false and tc.""Code""='GENERAL_FOLDER' and tc.""CompanyId""='{_repo.UserContext.CompanyId}'
				
	            left join(select np.*,case when npu is null then npwgu.""Id"" else npu.""Id"" end as ""UserId"" 
                from  public.""DocumentPermission"" as np 
                left join public.""User"" as npu on npu.""Id""=np.""PermittedUserId"" and npu.""Id""='{UserId}' and npu.""IsDeleted""=false and npu.""CompanyId""='{_repo.UserContext.CompanyId}' 
			
				left join public.""UserGroupUser"" as npwg on npwg.""UserGroupId""=np.""PermittedUserGroupId""  and npwg.""IsDeleted""=false and npwg.""CompanyId""='{_repo.UserContext.CompanyId}' 
				left join public.""User"" as npwgu on npwgu.""Id""=npwg.""UserId""  and npwgu.""Id""='{UserId}'	 and npwgu.""IsDeleted""=false and npwgu.""CompanyId""='{_repo.UserContext.CompanyId}'
					where np.""IsDeleted""=false)
				as np on np.""NoteId""=f.""Id"" and np.""UserId""='{UserId}'

                UNION

                Select distinct f.*, np.""PermissionType""  as PermissionType
                from public.""NtsNote"" as f
                join public.""Template"" as tm on tm.""Id""=f.""TemplateId"" and tm.""IsDeleted""=false and tm.""CompanyId""='{_repo.UserContext.CompanyId}' --and tm.""Code""='GENERAL_FOLDER'
                join public.""TemplateCategory"" as tc on tc.""Id""=tm.""TemplateCategoryId""  and tc.""IsDeleted""=false and tc.""CompanyId""='{_repo.UserContext.CompanyId}' and tc.""Code""='GENERAL_DOCUMENT'
                left join(select np1.*,case when npu is null then npwgu.""Id"" else npu.""Id"" end as ""UserId""
                from  public.""DocumentPermission"" as np1
                left join public.""User"" as npu on npu.""Id""=np1.""PermittedUserId"" and npu.""Id""='{UserId}' and npu.""IsDeleted""=false and npu.""CompanyId""='{_repo.UserContext.CompanyId}' 


                left join public.""UserGroupUser"" as npwg on npwg.""UserGroupId""=np1.""PermittedUserGroupId""  and npwg.""IsDeleted""=false and npwg.""CompanyId""='{_repo.UserContext.CompanyId}' 

                left join public.""User"" as npwgu on npwgu.""Id""=npwg.""UserId""  and npwgu.""Id""='{UserId}'	 and npwgu.""IsDeleted""=false and npwgu.""CompanyId""='{_repo.UserContext.CompanyId}'

                where np1.""IsDeleted""=false	  )
                as np on np.""NoteId""=f.""Id"" and np.""UserId""='{UserId}'			
                where f.""IsDeleted""=false and f.""CompanyId""='{_repo.UserContext.CompanyId}' and  (f.""IsArchived"" <> true or f.""IsArchived"" is null)
                ";

            var list = await _queryRepo1.ExecuteQueryList(query, null);            
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
        public async Task<bool> CreateDocumentByUpload(string path,string fileName,string ext, string parentId,string userId,string templateId, string batchId)
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
                    return true;
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
            return false;
        }
        
    }
}
