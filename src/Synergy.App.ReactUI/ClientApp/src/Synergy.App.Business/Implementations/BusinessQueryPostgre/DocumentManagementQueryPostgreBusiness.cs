using AutoMapper;
using Synergy.App.ViewModel;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Drawing.Charts;

namespace Synergy.App.Business
{
    public class DocumentManagementQueryPostgreBusiness : BusinessBase<NoteViewModel, NtsNote>, IDocumentManagementQueryBusiness
    {
        IUserContext _uc;
        private readonly IRepositoryQueryBase<NoteViewModel> _queryRepo;
        private readonly ITemplateCategoryBusiness _templateCategoryBusiness;
        private readonly ITemplateBusiness _templateBusiness;
        private readonly ITableMetadataBusiness _tableMetadataBusiness;
        private readonly IRepositoryQueryBase<FolderViewModel> _queryRepo1;
        private readonly IRepositoryQueryBase<WorkspaceViewModel> _queryWorkSpace;
        private readonly IRepositoryQueryBase<DocumentListViewModel> _querydocList;
        private IRepositoryQueryBase<NoteLinkShareViewModel> _QueryShareLink;
        private readonly INoteBusiness _noteBusiness;
        public DocumentManagementQueryPostgreBusiness(IRepositoryBase<NoteViewModel, NtsNote> repo
            , IMapper autoMapper
            , IUserContext uc
            , ITemplateCategoryBusiness templateCategoryBusiness
            , ITemplateBusiness templateBusiness
            , ITableMetadataBusiness tableMetadataBusiness
            , IRepositoryQueryBase<FolderViewModel> queryRepo1
            , IRepositoryQueryBase<WorkspaceViewModel> queryWorkSpace
            , IRepositoryQueryBase<DocumentListViewModel> querydocList
            , INoteBusiness noteBusiness
            , IRepositoryQueryBase<NoteLinkShareViewModel> QueryShareLink
            , IRepositoryQueryBase<NoteViewModel> queryRepo) : base(repo, autoMapper)
        {
            _uc = uc;
            _queryRepo = queryRepo;
            _templateCategoryBusiness = templateCategoryBusiness;
            _templateBusiness = templateBusiness;
            _tableMetadataBusiness = tableMetadataBusiness;
            _QueryShareLink = QueryShareLink;
            _noteBusiness = noteBusiness;
            _queryRepo1 = queryRepo1;
            _queryWorkSpace = queryWorkSpace;
            _querydocList = querydocList;
        }

        #region DMSDocumentBusiness

        // put dms doc business queries
        public async Task<List<FolderAndDocumentViewModel>> GetAllFolderAndDocumentByUserId(string userId)
        {
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

        public async Task<string> GetDocumentsQueryByParentFolderIdNew(string parentId, string UserId, string id, string udfs)
        {

            var query = $@"select f.""Id"" as Id,f.""NoteSubject"" as Name
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

        public async Task<string> GetFoldersAndDocumentsQueryNew(DocumentQueryTypeEnum documentQueryType, string UserId, string parentId)
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
        public async Task<List<FolderViewModel>> GetAllByParent(string UserId, string parentId, string udfs)
        {
         
            var query = $@"select distinct  n.""Id"" as Id,n.""NoteSubject"" as Name
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
            return list;
        }

        public async Task<List<FolderViewModel>> GetFirstLevelWorkspacesByUser(string UserId)
        {

            var query = string.Concat($@"select distinct  n.""Id"" as Id,n.""NoteSubject"" as Name
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
           return list;
        }

        public async Task<List<FolderViewModel>> GetAllChildWorkspaceAndFolder(string UserId, string parentId, string udfs)
        {
        
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
           return list;
        }

       public async Task<List<FolderViewModel>> GetAllChildWorkspaceFolderAndDocument(string UserId, string parentId, string udfs)
        {
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
                ,null as FileName,null as SnapshotMongoId
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
                ,null as FileName,null as SnapshotMongoId
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
                ,gen.""FileName"" as FileName,gen.""SnapshotMongoId"" as SnapshotMongoId
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
                ,gen.""FileName"" as FileName,gen.""SnapshotMongoId"" as SnapshotMongoId
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
            return list;
        }

        public async Task<List<FolderViewModel>> GetAllChildWorkspaceFolderAndFiles(string UserId, string parentId, string udfs)
        {
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
            return list;    
        }

        public async Task<List<FolderViewModel>> GetDocumentVersions(string noteId)
        {
            var query = $@"
                SELECT n.""Id"" as Id,n.""NoteSubject""  as Name,n.""NoteNo"" as DocumentNo,n.""VersionNo"" as VersionNo,u.""Name"" as CreatedByUser,t.""Code"" as TemplateMasterCode, '{noteId}' as ParentId,
                n.""CreatedDate"" as CreatedDate,n.""LastUpdatedDate"" as LastUpdatedDate,wf.""Id"" as WorkflowServiceId,wf.""TemplateCode"" as WorkflowCode,
                lov.""Code"" as WorkflowServiceStatus,lov.""Name"" as WorkflowServiceStatusName,n.""DmsAttachmentId"" as DocumentId
                FROM log.""NtsNoteLog"" n
                JOIN ""Template"" t ON n.""TemplateId"" = t.""Id"" AND t.""IsDeleted"" = false and t.""CompanyId""='{_repo.UserContext.CompanyId}'
                JOIN ""TemplateCategory"" tc ON t.""TemplateCategoryId"" = tc.""Id"" AND tc.""Code"" = 'GENERAL_DOCUMENT' AND tc.""IsDeleted"" = false and tc.""CompanyId""='{_repo.UserContext.CompanyId}' 
                LEFT JOIN ""NtsService"" wf ON wf.""Id"" = n.""ReferenceId"" AND wf.""ReferenceType"" = 0 and wf.""CompanyId""='{_repo.UserContext.CompanyId}'
                LEFT JOIN ""LOV"" lov ON lov.""Id"" = wf.""ServiceStatusId"" and lov.""CompanyId""='{_repo.UserContext.CompanyId}'
                LEFT JOIN public.""User"" as u on n.""CreatedBy""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
                where n.""IsDeleted""=false and n.""RecordId""='{noteId}' order by n.""LastUpdatedDate"" desc
                ";

            var list = await _queryRepo1.ExecuteQueryList(query, null);
            return list;
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
        public async Task<List<DocumentSearchViewModel>> GetAllWorkspaceFolderDocuments(DateTime? lastUpdatedDate)
        {
            if (lastUpdatedDate == null)
            {
                var query = string.Concat($@"Select   n.""Id"" as Id,n.""NoteSubject"" as NoteSubject,n.""NoteDescription"" as NoteDescription ,n.""NoteNo"" as NoteNo,tm.""Name"" as UdfTableName              
                ,n.""CreatedDate"" as CreatedDate,n.""LastUpdatedDate"" as LastUpdatedDate ,f.""Id"" as FileId,f.""FileName"" as FileName,f.""FileExtension"" as FileExtension,f.""FileExtractedText"" as FileExtractedText
                from public.""NtsNote"" as n 			   
                join public.""Template"" as tm on tm.""Id""=n.""TemplateId"" and tm.""IsDeleted""=false 
                join public.""TemplateCategory"" as tc on tc.""Id""=tm.""TemplateCategoryId""  and tc.""IsDeleted""=false and tc.""Code"" in ('GENERAL_FOLDER','GENERAL_DOCUMENT')
                left join public.""File"" as f on f.""Id""=n.""DmsAttachmentId"" and f.""IsDeleted""=false
                where n.""IsDeleted""=false order by n.""LastUpdatedDate"" limit 500
                ");
                var list = await _queryRepo1.ExecuteQueryList<DocumentSearchViewModel>(query, null);
                return list.ToList();
            }
            else
            {
                var query = string.Concat($@"Select   n.""Id"" as Id,n.""NoteSubject"" as NoteSubject,n.""NoteDescription"" as NoteDescription ,n.""NoteNo"" as NoteNo,tm.""Name"" as UdfTableName              
                ,n.""CreatedDate"" as CreatedDate,n.""LastUpdatedDate"" as LastUpdatedDate ,f.""Id"" as FileId,f.""FileName"" as FileName,f.""FileExtension"" as FileExtension,f.""FileExtractedText"" as FileExtractedText
                from public.""NtsNote"" as n 			   
                join public.""Template"" as tm on tm.""Id""=n.""TemplateId"" and tm.""IsDeleted""=false 
                join public.""TemplateCategory"" as tc on tc.""Id""=tm.""TemplateCategoryId""  and tc.""IsDeleted""=false and tc.""Code"" in ('GENERAL_FOLDER','GENERAL_DOCUMENT')
                left join public.""File"" as f on f.""Id""=n.""DmsAttachmentId"" and f.""IsDeleted""=false
                where n.""IsDeleted""=false and n.""LastUpdatedDate"" > '{lastUpdatedDate}' order by n.""LastUpdatedDate"" limit 500
                ");
                var list = await _queryRepo1.ExecuteQueryList<DocumentSearchViewModel>(query, null);
                return list.ToList();

            }
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

        public async Task<List<DocumentListViewModel>> DocumentStage(IList<TemplateViewModel> templateList,  string discipline,  bool IsOverdue, string userId)
        {
            var selectQry = "";
            var i = 1;
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
            return result2;
        }

        public async Task<List<DocumentListViewModel>> DocumentStageData(IList<TemplateViewModel> templateList, string discipline, bool IsOverdue, string userId)
        {
            var selectQry = "";
            var i = 1;
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
            return result2;
        }

        public async Task<List<DocumentListViewModel>> DocumentStageReport(IList<TemplateViewModel> templateList, string discipline, bool IsOverdue, string userId)
        {
            var selectQry = "";
            var i = 1;
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
return result2;
        }
        public async Task<List<DocumentListViewModel>> DocumentDataStageReportData(IList<TemplateViewModel> templateList, string discipline, bool IsOverdue, string userId)
        {
            var selectQry = "";
            var i = 1;
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
            return result2;
        }

        public async Task<List<DocumentListViewModel>> DocumentStageReportData(IList<TemplateViewModel> templateList, string discipline, bool IsOverdue, string userId)
        {
            var selectQry = "";
            var i = 1;

            foreach (var item in templateList)
            {
                if (item.TableMetadataId != null)
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
            return result2;
        }

        public async Task<List<string>> GetAllDocumentsDMSReportByRevesion(string userId)
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

        public async Task<IList<DocumentListViewModel>> DocumentSubmittedReportData(string documentIds, string discipline, string revesion)
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
            return result2;

        }
       public async Task<IList<DocumentListViewModel>> DocumentSubmittedReportData(string documentIds, string discipline, string revesion, string udfs)
        {
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
            var result2 = await _queryRepo1.ExecuteQueryList<DocumentListViewModel>(cypher1, null);
            return result2;
        }

       public async Task<IList<DocumentListViewModel>> DocumentSubmittedReport(string documentIds, string discipline, string revesion, string udfs)
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
            var result2 = await _queryRepo1.ExecuteQueryList<DocumentListViewModel>(cypher1, null);
            return result2;
        }

        public async Task<List<DocumentListViewModel>> DocumentReceived(string documentIds, string discipline, string revesion)
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
            return result2;
        }

        public async Task<List<DocumentListViewModel>> DocumentReceivedData(string documentIds, string discipline, string revesion)
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
            return result2; 
        }


        public async Task<List<DocumentListViewModel>> DocumentReceivedReport(string documentIds, string discipline, string revesion)
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
            return result2;
        }
        public async Task<List<DocumentListViewModel>> DocumentReceivedReportData(string documentIds, string discipline, string revesion)
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
            return result2;
        }
        //public async Task<IList<DocumentListViewModel>> DocumentStage(string userId)
        //{ 
        //}
        public async Task<List<DocumentListViewModel>> GetAllDocumentsDMSReport(string userId)
        {
            

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

        public async Task<List<DocumentListViewModel>> DocumentReceivedCommentsReportData(List<TemplateViewModel> templateList, string documentIds, string discipline, string revesion)
        {
            var selectQry = "";
            var i = 1;
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

            return result2;
        }

        public async Task<string> ValidateRequestForInspection(string startstr)
        {
            var Query = $@"select  max( split_part(n.""NoteNo""  COLLATE ""tr-TR-x-icu"", '-', 5)) as maxNumber
                     from Public.""TemplateCategory"" tc inner join
            
                             public.""Template"" as t on tc.""Id""=t.""TemplateCategoryId"" and t.""IsDeleted""=false and  t.""CompanyId"" = '{_repo.UserContext.CompanyId}'
            				Join public.""NtsNote"" as n on n.""TemplateId""=t.""Id"" and n.""IsDeleted""=false and  n.""CompanyId"" = '{_repo.UserContext.CompanyId}'
            				Left Join public.""User"" as u on n.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and  u.""CompanyId"" = '{_repo.UserContext.CompanyId}'
                            Left Join public.""LOV"" as lov on n.""NoteStatusId""=lov.""Id"" and lov.""IsDeleted""=false and  lov.""CompanyId"" = '{_repo.UserContext.CompanyId}'
            				 where tc.""Code"" in ('GENERAL_DOCUMENT') and tc.""IsDeleted""=false and  tc.""CompanyId"" = '{_repo.UserContext.CompanyId}' and t.""Code""='GALFAR_REQUEST_FOR_INSPECTION'
                         and n.""NoteNo""  like '{startstr}%' COLLATE ""tr-TR-x-icu""";


            var maxNumber = await _queryRepo1.ExecuteScalar<string>(Query, null);
            return maxNumber;
        }

        public async Task<string> ValidateRequestForInspectionHalul(string discipline)
        {
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
            return maxNumber;
        }

        public  async Task<string> ValidateCustomeNotNoRFI(string _firstStr, string _lastStr)
        {
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
            return maxNumber;
        }
        public async Task<string> ValidateCustomeNotNoRFIHalul(string _lastStr, string discipline)
        {
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
            return maxNumber;
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

        public async Task<List<FolderViewModel>> GetAllParentByChildId(string ChildId, List<FolderViewModel> ParentList)
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

        public async Task<List<FolderViewModel>> GetAllChildByParentId(string ParentId, List<FolderViewModel> FolderList)
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
           return list;
        }

        public async Task<IList<FolderViewModel>> GetAllPermissionChildByParentId(string ParentId)
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
            return list;
        }

        public async Task<IList<FolderViewModel>> GetAllPermissionChildByParentIdData(string ParentId)
        {
            var cypher1 = $@"select distinct doc.*,doc.""NoteSubject"" as Name from public.""NtsTag"" as tag
                join public.""NtsNote"" as doc on doc.""Id""=tag.""TagSourceReferenceId"" and doc.""IsDeleted""=false and doc.""IsArchived""=false 

                where ""NtsId""='{ParentId}' and tag.""IsDeleted""=false
            ";
            //var cypher = @"match (n: NTS_Note{ IsDeleted: 0,Status: 'Active'})
            //                     -[:R_Note_Parent_Note]->(p:NTS_Note{ IsDeleted: 0,Status: 'Active',Id:{ParentId}})                                                           
            //                    return n,n.Subject as Name, p.Id as ParentId";
            var list1 = await _queryRepo1.ExecuteQueryList<FolderViewModel>(cypher1, null);
            return list1;
        }

        public async Task<List<FolderViewModel>> GetAllPermittedChildByParentId(string UserId, string ParentId)
        {
            var cypher = $@"WITH RECURSIVE Folder AS(
            select distinct  n.*,t.""Code"" as FolderCode,tc.""Code"" as TemaplateMasterCatCode,n.""NoteSubject"" as Name,n.""ParentNoteId"" as ParentId
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
            return list;

        }

        public async Task<List<FolderViewModel>> GetAllPermittedChildByParentIdData(string UserId, string ParentId)
        {
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
                where tag.""NtsId""='{ParentId}' and tag.""IsDeleted""=false
            ";
            var list1 = await _queryRepo1.ExecuteQueryList<FolderViewModel>(cypher1, null);
            return list1;
        }


        public async Task<List<DocumentPermissionViewModel>> GetAllNotePermissionByParentId(string id)
        {
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

            var list = await _queryRepo1.ExecuteQueryList<DocumentPermissionViewModel>(cypher, null);
            return list;
        }

        public async Task<List<FolderViewModel>> GetAllChildDocumentByParentId(string ParentId)
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
            return list;
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
            return result;


        }

        public async Task<ServiceViewModel> GetWorflowDetailByDocument(string noteId, string udfs)
        {
            var query = $@"select s.*,lv.""Code"" as ServiceStatusCode from public.""NtsService"" as s 
join public.""LOV"" as lv on lv.""Id""=s.""ServiceStatusId"" and lv.""IsDeleted""=false and lv.""CompanyId"" = '{_repo.UserContext.CompanyId}'
join (" + udfs + $@") as udf on udf.""NtsNoteId""=s.""UdfNoteId"" 
where udf.""documentId""='{noteId}' and s.""IsDeleted""=false and s.""CompanyId"" = '{_repo.UserContext.CompanyId}'
";
            var data = await _queryRepo1.ExecuteQuerySingle<ServiceViewModel>(query, null);
            return data;

        }

        public async Task<IList<FolderViewModel>> GetFolderByParent(string parentId, string noteId)
        {
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



            var result = await _queryRepo1.ExecuteQueryList<FolderViewModel>(cypher, null);
            return result;
        }

        public async Task<List<NoteLinkShareViewModel>> GetDocumentLinksData(long id)
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
        public async Task<List<NoteViewModel>> IsUniqueDocumentFolder(string ParentId, string code)
        {
            var cypher = $@"Select n.* from 
public.""Template"" as tr 
join public.""TemplateCategory"" as tc on tc.""Id""=tr.""TemplateCategoryId"" and tc.""Code""='GENERAL_FOLDER' and tc.""IsDeleted""=false
join public.""NtsNote"" as n on n.""TemplateId""=tr.""Id"" and n.""IsDeleted""=false
join public.""NtsNote"" as p on p.""Id""=n.""ParentNoteId"" and p.""Id""='{ParentId}' where n.""NoteSubject""='{code}' and tr.""IsDeleted""=false and p.""IsDeleted""=false
";

            var value = await _queryRepo1.ExecuteQueryList<NoteViewModel>(cypher, null);
            return value;
        }
        public async Task<List<NoteViewModel>> IsUniqueGeneralDocument(string ParentId, string code)
        {
            var cypher = $@"Select n.* from 
public.""Template"" as tr 
join public.""TemplateCategory"" as tc on tc.""Id""=tr.""TemplateCategoryId"" and tc.""Code""='GENERAL_DOCUMENT' and tc.""IsDeleted""=false
join public.""NtsNote"" as n on n.""TemplateId""=tr.""Id"" and n.""IsDeleted""=false
join public.""NtsNote"" as p on p.""Id""=n.""ParentNoteId"" and p.""Id""='{ParentId}' where n.""NoteSubject""='{code}' and tr.""IsDeleted""=false and p.""IsDeleted""=false
";

            var value = await _querydocList.ExecuteQueryList<NoteViewModel>(cypher, null);
            return value;
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

          public async Task<FolderViewModel> UpdateTagsByDocumentIds(string ids)
        {
            var cypher = $@" update public.""NtsTag"" set ""IsDeleted""=true
            where ""Id"" in ({ids}) ";
            var result = await _queryRepo1.ExecuteQuerySingle(cypher, null);
            return result;
        }
        public async Task<FolderViewModel> DeleteNotesbyNoteIds(string ids)
        {
            var cypher = $@" update public.""NtsNote"" set ""IsDeleted""=true
            where ""Id"" in ({ids}) ";
            var result = await _queryRepo1.ExecuteQuerySingle(cypher, null);
            return result;
        }
        public async Task<FolderViewModel> ArchiveNotesbyNoteIds(string ids)
        {
            var cypher = $@" update public.""NtsNote"" set ""IsArchived""=true
            where ""Id"" in ({ids}) ";
            var result = await _queryRepo1.ExecuteQuerySingle(cypher, null);
            return result;
        }

        public async Task<FolderViewModel> CheckMyWorkspaceExist(string UserId)
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
            return data;
        }

        public async Task<NoteTemplateViewModel> CheckEmployeeBook(string empId)
        {

            var query = $@"Select b.* from cms.""N_GENERAL_DOCUMENT_HREmployeeBook"" as b
join public.""NtsNote"" as n on b.""NtsNoteId""=n.""Id"" and n.""IsDeleted""=false and n.""IsArchived""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""LOV"" as bs on n.""NoteStatusId""=bs.""Id"" and bs.""IsDeleted""=false and bs.""CompanyId""='{_repo.UserContext.CompanyId}'
where b.""EmployeeId""='{empId}' and b.""IsDeleted""=false and b.""CompanyId""='{_repo.UserContext.CompanyId}' and bs.""Code""!='NOTE_STATUS_EXPIRE' ";

            var result = await _queryRepo1.ExecuteQuerySingle<NoteTemplateViewModel>(query, null);
            return result;



        }

        public async Task<List<FolderViewModel>> GetAllChildFiles(string UserId, string parentId, string udfs)
        {
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
                ,udf.""fileAttachmentId"" as DocumentId
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
                left join public.""File"" as gen on gen.""Id""=udf.""fileAttachmentId"" and gen.""IsDeleted""=false and gen.""CompanyId""='{_repo.UserContext.CompanyId}'
                ";

            var list = await _queryRepo1.ExecuteQueryList(query, null);
            return list;
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
            return list;
        }


        #endregion


        #region DocumentPermissionBusiness

        // put doc permission business queries

        public async Task<List<DocumentPermissionViewModel>> GetPermissionList(string noteId)
        {
            var query = $@"SELECT D.""Id"",case when U.""Name"" IS NULL then UG.""Name"" else U.""Name"" end as PermittedUserId,
                        D.""PermissionType"", D.""Access"", D.""AppliesTo"", D.""NoteId"",  D.""PermittedUserGroupId"",
                        n.""Id"" as InheritedFromId,n.""NoteSubject"" as InheritedFrom,D.""IsInherited"" as IsInherited,D.""IsInheritedFromChild"" as IsInheritedFromChild
                        FROM public.""DocumentPermission"" as  D left join public.""User"" as U on U.""Id""=D.""PermittedUserId""
	                    left join public.""UserGroup"" UG on UG.""Id""=D.""PermittedUserGroupId"" and UG.""IsDeleted""=false and UG.""CompanyId"" = '{_repo.UserContext.CompanyId}'
                         left join public.""DocumentPermission"" as  IH on IH.""Id""=D.""InheritedFrom"" and IH.""IsDeleted""=false and IH.""CompanyId"" = '{_repo.UserContext.CompanyId}'
                         left join public.""NtsNote"" as  n on n.""Id""=IH.""NoteId""  and n.""IsDeleted""=false and n.""CompanyId"" = '{_repo.UserContext.CompanyId}'
	                    where D.""NoteId""='{noteId}' and D.""IsInheritedFromChild""=false and ((n.""Id"" is not null and D.""IsInherited""=true) or (D.""IsInherited""=false)) and D.""IsDeleted""= 'false' and D.""CompanyId"" = '{_repo.UserContext.CompanyId}'";

            var queryData = await _queryRepo.ExecuteQueryList<DocumentPermissionViewModel>(query, null);
            return queryData;
        }
        public async Task<List<TemplateViewModel>> GetTemplateList(string parentId)
        {
            var query = @$"select t.""Id"",t.""DisplayName"", t.""Description"", t.""Code"", t.""TemplateType"", tc.""Code"" as CategoryCode, tt.""IconFileId"", tt.""NoteTemplateType"" as NoteType
                        from public.""Template"" as t
                        join public.""TemplateCategory"" as tc on tc.""Id""=t.""TemplateCategoryId"" and tc.""IsDeleted""= false and tc.""CompanyId"" = '{_repo.UserContext.CompanyId}' --and tc.""TemplateCategoryType""=1
                        join (select dt.""DocumentTypeId"", pn.""Id"", w.""NtsNoteId""
from cms.""N_DMS_WorkspaceDocType"" as dt
join public.""NtsNote"" as n on n.""Id""=dt.""NtsNoteId"" and n.""IsDeleted""= false and n.""CompanyId"" = '{_repo.UserContext.CompanyId}'
join public.""NtsNote"" as pn on pn.""Id""=n.""ParentNoteId"" and pn.""IsDeleted""= false and pn.""CompanyId"" = '{_repo.UserContext.CompanyId}'
join cms.""N_GENERAL_FOLDER_WORKSPACE"" as w on w.""NtsNoteId""=pn.""Id"" and w.""IsDeleted""= false and w.""CompanyId"" = '{_repo.UserContext.CompanyId}'
join cms.""N_GENERAL_FOLDER_GENERALFOLDER"" as f on f.""WorkspaceId"" = w.""Id""
where f.""NtsNoteId"" = '{parentId}' and dt.""IsDeleted""=false)as wdt on wdt.""DocumentTypeId""=t.""Id"" 
--where w.""NtsNoteId""='{parentId}')as wdt on wdt.""DocumentTypeId""=t.""Id"" 
						left join public.""NoteTemplate"" as tt on tt.""TemplateId""=t.""Id"" and tt.""IsDeleted""= false and tt.""CompanyId"" = '{_repo.UserContext.CompanyId}'
						
						-- left join Cms.""N_GENERAL_FOLDER_WORKSPACE"" as w on w.""=
                        where t.""IsDeleted""=false and t.""CompanyId"" = '{_repo.UserContext.CompanyId}'
						and tc.""Code""='GENERAL_DOCUMENT' and t.""DisplayName"" not in ('Folder','Workspace')";


            var list = await _queryRepo.ExecuteQueryList<TemplateViewModel>(query, null);
            //var taskList = list.Where(x => x.TemplateType == TemplateTypeEnum.Task && x.TaskType != TaskTypeEnum.StepTask);

            return list;


        }

        public async Task<List<DMSDocumentViewModel>> GetArchive(string UserID)
        {
            // var selectQry = "";
            // var i = 1;
            //
            // var tempCategory = await _templateCategoryBusiness.GetList(x => x.Code == "GENERAL_DOCUMENT" || x.Code== "GENERAL_FOLDER" && x.TemplateType==TemplateTypeEnum.Note);
            //
            // foreach (var item1 in tempCategory)
            // {
            //
            //
            //     var templateList = await _templateBusiness.GetList(x => x.TemplateCategoryId == item1.Id);
            //
            //
            //     foreach (var item in templateList.Where(x => x.TableMetadataId != null))
            //     {
            //         var tableMeta = await _tableMetadataBusiness.GetSingle(x => x.Id == item.TableMetadataId);
            //         if (item.Code == "VENDOR_ENGINEER" || item.Code == "REQUEST FOR INSPECTION")
            //
            //         {
            //
            //         }
            //         else
            //         {
            //             if (i != 1)
            //             {
            //                 selectQry += " union ";
            //             }
            //
            //                        selectQry = @$"{ selectQry}
            //                    select n.""NoteSubject"" as ""DocumentName"",'{item.TableMetadataId}' as TableMetadataId, n.""NoteNo"" as ""NoteVersionNo"", lov.""Name"" as ""StatusName"",t.""DisplayName"" as DocumentType,n.""LastUpdatedDate"" as ""UpdatedDate"",
            //case when tc.""Code"" = 'GENERAL_FOLDER' then 'Folder' else 'File' end as ""UploadType"",u.""Name"" as ""UpdatedByUser"",u.""PhotoId"" as PhotoId,'' as ""FolderPath"",n.""Id"" as ""NoteId"",udf.""Id"", udf.""IsArchived"" as IsArchived,
            //t.""Code"" as templatecode
            //from cms.""{ tableMeta.Name}"" as udf
            //Join public.""NtsNote"" as n on n.""Id""=udf.""NtsNoteId"" and n.""IsDeleted""=false
            //inner join public.""Template"" as t on t.""Id""= n.""TemplateId""
            //inner join Public.""TemplateCategory"" tc on tc.""Id""=t.""TemplateCategoryId""
            //                           Left Join public.""User"" as u on n.""OwnerUserId""=u.""Id"" 
            //                           Left Join public.""LOV"" as lov on n.""NoteStatusId""=lov.""Id""
            //						 where udf.""IsDeleted""=false  and n.""OwnerUserId""='{UserID}'";
            //
            //
            //
            //
            //
            //                        i++;
            //
            //
            //                    }
            //                }
            //            }
            //






            var Query = $@"select Distinct n.""NoteSubject"" as ""DocumentName"", n.""VersionNo"" as ""NoteVersionNo"", lov.""Name"" as ""StatusName"",t.""DisplayName"" as DocumentType,n.""LastUpdatedDate"" as ""UpdatedDate"",
                      case when tc.""Code"" = 'GENERAL_FOLDER' then 'Folder' else 'File' end as ""UploadType"",u.""Name"" as ""UpdatedByUser"",u.""PhotoId"" as PhotoId,'' as ""FolderPath"",n.""Id"" as ""NoteId"",n.""Id"",n.""IsArchived"" as IsArchived,
                      t.""Code"" as templatecode,n.""ParentNoteId"" as ParentId,n.""CreatedDate"" as  CreatedDate
                      from Public.""TemplateCategory"" tc inner join public.""Template"" as t on tc.""Id""=t.""TemplateCategoryId"" and t.""IsDeleted""=false and t.""CompanyId"" = '{_repo.UserContext.CompanyId}'
							Join public.""NtsNote"" as n on n.""TemplateId""=t.""Id"" and n.""IsDeleted""=false and n.""CompanyId"" = '{_repo.UserContext.CompanyId}'
							Left Join public.""User"" as u on n.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId"" = '{_repo.UserContext.CompanyId}'
                             Left Join public.""LOV"" as lov on n.""NoteStatusId""=lov.""Id"" and lov.""IsDeleted""=false and lov.""CompanyId"" = '{_repo.UserContext.CompanyId}'
                inner join public.""DocumentPermission"" as np on np.""NoteId""=n.""Id"" and np.""IsDeleted""=false and np.""CompanyId"" = '{_repo.UserContext.CompanyId}'
                inner join public.""User"" as npu on npu.""Id""=np.""PermittedUserId"" and npu.""Id""='{UserID}'	and npu.""IsDeleted""=false and npu.""CompanyId"" = '{_repo.UserContext.CompanyId}'
                       where tc.""Code"" in ('GENERAL_DOCUMENT','GENERAL_FOLDER') and tc.""IsDeleted""=false and tc.""CompanyId"" = '{_repo.UserContext.CompanyId}' and n.""IsArchived""=True and n.""IsDeleted""=false
                      UNION
select Distinct n.""NoteSubject"" as ""DocumentName"", n.""VersionNo"" as ""NoteVersionNo"", lov.""Name"" as ""StatusName"",t.""DisplayName"" as DocumentType,n.""LastUpdatedDate"" as ""UpdatedDate"",
                      case when tc.""Code"" = 'GENERAL_FOLDER' then 'Folder' else 'File' end as ""UploadType"",u.""Name"" as ""UpdatedByUser"",u.""PhotoId"" as PhotoId,'' as ""FolderPath"",n.""Id"" as ""NoteId"",n.""Id"",n.""IsArchived"" as IsArchived,
                      t.""Code"" as templatecode,n.""ParentNoteId"" as ParentId,n.""CreatedDate"" as  CreatedDate
                      from Public.""TemplateCategory"" tc inner join public.""Template"" as t on tc.""Id""=t.""TemplateCategoryId"" and t.""IsDeleted""=false and t.""CompanyId"" = '{_repo.UserContext.CompanyId}'
							Join public.""NtsNote"" as n on n.""TemplateId""=t.""Id"" and n.""IsDeleted""=false and n.""CompanyId"" = '{_repo.UserContext.CompanyId}'
							Left Join public.""User"" as u on n.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId"" = '{_repo.UserContext.CompanyId}'
                             Left Join public.""LOV"" as lov on n.""NoteStatusId""=lov.""Id"" and lov.""IsDeleted""=false and lov.""CompanyId"" = '{_repo.UserContext.CompanyId}'
   left join public.""DocumentPermission"" as np2 on np2.""NoteId""=n.""Id"" and np2.""IsDeleted""=false and np2.""CompanyId"" = '{_repo.UserContext.CompanyId}'

                inner join public.""UserGroupUser"" as npwg on npwg.""UserGroupId""=np2.""PermittedUserGroupId"" and npwg.""IsDeleted""=false and npwg.""CompanyId"" = '{_repo.UserContext.CompanyId}'
				inner join public.""User"" as npwgu on npwgu.""Id""=npwg.""UserId""	and npwgu.""Id""='{UserID}'	and npwgu.""IsDeleted""=false and npwgu.""CompanyId"" = '{_repo.UserContext.CompanyId}'		
                 where tc.""Code"" in ('GENERAL_DOCUMENT','GENERAL_FOLDER') and tc.""IsDeleted""=false  and tc.""CompanyId"" = '{_repo.UserContext.CompanyId}' and n.""IsArchived""=True order by  ""UpdatedDate"" desc

";

            var ressult = await _queryRepo.ExecuteQueryList<DMSDocumentViewModel>(Query, null);

            //foreach (var item in ressult)
            //{
            //    item.Permission = await CheckUserPermission(item.NoteId, UserID);
            //
            //
            //}

            //  var data = ressult.Where(x => x.Permission == true).ToList();

            //foreach (var item in ressult)
            //{
            //    var folderpath = "";
            //    if (item.ParentId.IsNotNull())
            //    {
            //        var plist = await GetFolderByParent(item.ParentId);
            //        foreach (var i in plist)
            //        {
            //            folderpath += " " + i.Name + " >";
            //        }
            //    }
            //    item.FolderPath = folderpath;
            //}

            return ressult;
        }



        public async Task<List<DMSDocumentViewModel>> GetFolderByParent(string ParentId)
        {

            var Query = $@"select n.""NoteSubject"" as ""Name"", n.""Id"" as Id
                      from Public.""TemplateCategory"" tc inner join
                            public.""Template"" as t on tc.""Id""=t.""TemplateCategoryId"" and t.""IsDeleted""=false  and t.""CompanyId"" = '{_repo.UserContext.CompanyId}'
							Join public.""NtsNote"" as n on n.""TemplateId""=t.""Id"" and n.""IsDeleted""=false  and n.""CompanyId"" = '{_repo.UserContext.CompanyId}'
							Left Join public.""User"" as u on n.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false  and u.""CompanyId"" = '{_repo.UserContext.CompanyId}'
                            Left Join public.""LOV"" as lov on n.""NoteStatusId""=lov.""Id"" and lov.""IsDeleted""=false  and lov.""CompanyId"" = '{_repo.UserContext.CompanyId}'
							 where tc.""Code"" in ('GENERAL_FOLDER') and tc.""IsDeleted""=false  and tc.""CompanyId"" = '{_repo.UserContext.CompanyId}' and n.""Id""='{ParentId}' and n.""IsArchived"" <>true or n.""IsArchived"" is null
                            union
select pn.""NoteSubject"" as ""Name"", pn.""Id"" as Id
                      from Public.""TemplateCategory"" tc inner join
                            public.""Template"" as t on tc.""Id""=t.""TemplateCategoryId"" and t.""IsDeleted""=false  and t.""CompanyId"" = '{_repo.UserContext.CompanyId}'
							Join public.""NtsNote"" as n on n.""TemplateId""=t.""Id"" and n.""IsDeleted""=false  and n.""CompanyId"" = '{_repo.UserContext.CompanyId}'
							Left Join public.""User"" as u on n.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false  and u.""CompanyId"" = '{_repo.UserContext.CompanyId}'
                            Left Join public.""LOV"" as lov on n.""NoteStatusId""=lov.""Id"" and lov.""IsDeleted""=false  and lov.""CompanyId"" = '{_repo.UserContext.CompanyId}'
                           Join public.""NtsNote""  as pn on pn.""Id""=n.""ParentNoteId"" and pn.""IsDeleted""=false  and pn.""CompanyId"" = '{_repo.UserContext.CompanyId}'
							 where tc.""Code"" in ('GENERAL_FOLDER') and tc.""IsDeleted""=false  and tc.""CompanyId"" = '{_repo.UserContext.CompanyId}'  and n.""Id""='{ParentId}' and n.""IsArchived"" <>true or n.""IsArchived"" is null

";
            var ressult = await _queryRepo.ExecuteQueryList<DMSDocumentViewModel>(Query, null);
            return ressult;

        }

        public async Task<List<DMSDocumentViewModel>> GetBinDocumentData(string UserID)
        {

            //var selectQry = "";
            //var i = 1;
            //
            //var tempCategory = await _templateCategoryBusiness.GetList(x => x.Code == "GENERAL_DOCUMENT" || x.Code == "GENERAL_FOLDER" && x.TemplateType == TemplateTypeEnum.Note);
            //
            //foreach (var item1 in tempCategory)
            //{
            //
            //
            //    var templateList = await _templateBusiness.GetList(x => x.TemplateCategoryId == item1.Id);
            //
            //
            //    foreach (var item in templateList.Where(x => x.TableMetadataId != null))
            //    {
            //        var tableMeta = await _tableMetadataBusiness.GetSingle(x => x.Id == item.TableMetadataId);
            //
            //        if (i != 1)
            //        {
            //            selectQry += " union ";
            //        }

            //selectQry = @$"{ selectQry}
            //select n.""NoteSubject"" as ""DocumentName"",'{item.TableMetadataId}' as TableMetadataId, n.""NoteNo"" as ""NoteVersionNo"", lov.""Name"" as ""StatusName"",t.""DisplayName"" as DocumentType,n.""LastUpdatedDate"" as ""UpdatedDate"",
            //case when tc.""Code"" = 'GENERAL_FOLDER' then 'Folder' else 'File' end as ""UploadType"",u.""Name"" as ""UpdatedByUser"",u.""PhotoId"" as PhotoId,'' as ""FolderPath"",n.""Id"" as ""NoteId"",udf.""Id"",True as IsArchived,
            //t.""Code"" as templatecode
            //from cms.""{ tableMeta.Name}"" as udf
            //Join public.""NtsNote"" as n on n.""Id""=udf.""NtsNoteId"" 
            //inner join public.""Template"" as t on t.""Id""= n.""TemplateId""
            //inner join Public.""TemplateCategory"" tc on tc.""Id""=t.""TemplateCategoryId""
            //Left Join public.""User"" as u on n.""OwnerUserId""=u.""Id"" 
            //Left Join public.""LOV"" as lov on n.""NoteStatusId""=lov.""Id""
            //where  n.""IsDeleted""=True  and n.""OwnerUserId""='{UserID}'";





            //i++;


            //}
            //}



            var Query = $@"select Distinct n.""NoteSubject"" as ""DocumentName"", n.""VersionNo"" as ""NoteVersionNo"", lov.""Name"" as ""StatusName"",t.""DisplayName"" as DocumentType,n.""LastUpdatedDate"" as ""UpdatedDate"",
                      case when tc.""Code"" = 'GENERAL_FOLDER' then 'Folder' else 'File' end as ""UploadType"",u.""Name"" as ""UpdatedByUser"",u.""PhotoId"" as PhotoId,'' as ""FolderPath"",n.""Id"" as ""NoteId"",n.""Id"",n.""IsArchived"" as IsArchived,
                      t.""Code"" as templatecode,n.""ParentNoteId"" as ParentId,n.""CreatedDate"" as  CreatedDate
                      from Public.""TemplateCategory"" tc inner join public.""Template"" as t on tc.""Id""=t.""TemplateCategoryId"" and t.""IsDeleted""=false  and t.""CompanyId"" = '{_repo.UserContext.CompanyId}'
							Join public.""NtsNote"" as n on n.""TemplateId""=t.""Id"" and n.""IsDeleted""=true  and n.""CompanyId"" = '{_repo.UserContext.CompanyId}'
							Left Join public.""User"" as u on n.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false  and u.""CompanyId"" = '{_repo.UserContext.CompanyId}' 
                             Left Join public.""LOV"" as lov on n.""NoteStatusId""=lov.""Id"" and lov.""IsDeleted""=false  and lov.""CompanyId"" = '{_repo.UserContext.CompanyId}'
                inner join public.""DocumentPermission"" as np on np.""NoteId""=n.""Id"" and np.""IsDeleted""=false  and np.""CompanyId"" = '{_repo.UserContext.CompanyId}' 
                inner join public.""User"" as npu on npu.""Id""=np.""PermittedUserId"" and npu.""Id""='{UserID}'	and npu.""IsDeleted""=false  and npu.""CompanyId"" = '{_repo.UserContext.CompanyId}'
                             where tc.""Code"" in ('GENERAL_DOCUMENT','GENERAL_FOLDER') and tc.""IsDeleted""=false  and tc.""CompanyId"" = '{_repo.UserContext.CompanyId}'
                      UNION
select Distinct n.""NoteSubject"" as ""DocumentName"", n.""VersionNo"" as ""NoteVersionNo"", lov.""Name"" as ""StatusName"",t.""DisplayName"" as DocumentType,n.""LastUpdatedDate"" as ""UpdatedDate"",
                      case when tc.""Code"" = 'GENERAL_FOLDER' then 'Folder' else 'File' end as ""UploadType"",u.""Name"" as ""UpdatedByUser"",u.""PhotoId"" as PhotoId,'' as ""FolderPath"",n.""Id"" as ""NoteId"",n.""Id"",n.""IsArchived"" as IsArchived,
                      t.""Code"" as templatecode,n.""ParentNoteId"" as ParentId,n.""CreatedDate"" as  CreatedDate
                      from Public.""TemplateCategory"" tc inner join public.""Template"" as t on tc.""Id""=t.""TemplateCategoryId"" and t.""IsDeleted""=false  and t.""CompanyId"" = '{_repo.UserContext.CompanyId}'
							Join public.""NtsNote"" as n on n.""TemplateId""=t.""Id"" and n.""IsDeleted""=true  and n.""CompanyId"" = '{_repo.UserContext.CompanyId}'
							Left Join public.""User"" as u on n.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false  and u.""CompanyId"" = '{_repo.UserContext.CompanyId}'
                             Left Join public.""LOV"" as lov on n.""NoteStatusId""=lov.""Id"" and lov.""IsDeleted""=false  and lov.""CompanyId"" = '{_repo.UserContext.CompanyId}'
   left join public.""DocumentPermission"" as np2 on np2.""NoteId""=n.""Id"" and np2.""IsDeleted""=false  and np2.""CompanyId"" = '{_repo.UserContext.CompanyId}' 

                inner join public.""UserGroupUser"" as npwg on npwg.""UserGroupId""=np2.""PermittedUserGroupId"" and npwg.""IsDeleted""=false and npwg.""CompanyId"" = '{_repo.UserContext.CompanyId}' 
				inner join public.""User"" as npwgu on npwgu.""Id""=npwg.""UserId""	and npwgu.""Id""='{UserID}'	and npwgu.""IsDeleted""=false and npwgu.""CompanyId"" = '{_repo.UserContext.CompanyId}' 	
                             where tc.""Code"" in ('GENERAL_DOCUMENT','GENERAL_FOLDER') and tc.""IsDeleted""=false and tc.""CompanyId"" = '{_repo.UserContext.CompanyId}'  order by  ""UpdatedDate"" desc
";

            var ressult = await _queryRepo.ExecuteQueryList<DMSDocumentViewModel>(Query, null);

            // foreach (var item in ressult)
            // {
            //     item.Permission = await CheckUserPermission(item.NoteId, UserID);
            //
            //
            // }
            //
            // var data = ressult.Where(x => x.Permission == true).ToList();
            //  foreach (var item in ressult)
            //  {
            //      var folderpath = "";
            //      if (item.ParentId.IsNotNull())
            //      {
            //          var plist = await GetFolderByParent(item.ParentId);
            //          foreach (var i in plist)
            //          {
            //              folderpath += " " + i.Name + " >";
            //          }
            //      }
            //      item.FolderPath = folderpath;
            //  }


            return ressult;
        }


        public async Task DeleteDocument(string NoteId)
        {
            var query = $@"Update public.""NtsNote"" set ""IsDeleted""=True where ""Id""='{NoteId}'";
            await _queryRepo.ExecuteCommand(query, null);
            
        }

        public async Task<List<WorkspaceViewModel>> ValidateSequenceOrder(WorkspaceViewModel model)
        {
            var query = $@"select w.*,n.*,n.""Id"" as NoteId from public.""NtsNote"" as n
join cms.""N_GENERAL_FOLDER_WORKSPACE"" as w on n.""Id"" = w.""NtsNoteId"" and w.""IsDeleted"" = false and w.""CompanyId"" = '{_repo.UserContext.CompanyId}'
where #WHERE# and n.""IsDeleted"" = false and n.""CompanyId"" = '{_repo.UserContext.CompanyId}' and n.""IsArchived"" = false";

            var where = "";
            if (model.ParentNoteId.IsNotNullAndNotEmpty())
            {
                where = $@"n.""ParentNoteId"" = '{model.ParentNoteId}'";
            }
            else
            {
                where = $@"n.""ParentNoteId"" is null";
            }
            query = query.Replace("#WHERE#", where);

            var result = await _queryRepo.ExecuteQueryList<WorkspaceViewModel>(query, null);
            return result;
            

        }

        public async Task<List<WorkspaceViewModel>> ValidateWorkspace(WorkspaceViewModel model)
        {
            var query = $@"select w.*,n.*,n.""Id"" as NoteId from public.""NtsNote"" as n
join cms.""N_GENERAL_FOLDER_WORKSPACE"" as w on n.""Id"" = w.""NtsNoteId"" and w.""IsDeleted"" = false and w.""CompanyId"" = '{_repo.UserContext.CompanyId}'
where  n.""IsDeleted"" = false and n.""CompanyId"" = '{_repo.UserContext.CompanyId}' and n.""IsArchived"" = false";

            var result = await _queryRepo.ExecuteQueryList<WorkspaceViewModel>(query, null);
            return result;

        }

        public async Task<List<WorkspaceViewModel>> GetWorkspaceList()
        {
            var query = "";

            query = @$"select w.*,nts.*, w.""Id"" ,l.""Name"" as ""LegalEntityName"",u.""Name"" as ""CreatedbyName"",nts.""NoteSubject"" as ""WorkspaceName"",
                            w.""NtsNoteId"" as ""NoteId"",u.""Id"" as UserId,nts1.""NoteSubject"" as ""ParentName"",
                            nts.""SequenceOrder"" as ""SequenceOrder"",nts1.""Id"" as ""ParentNoteId""
                        From cms.""N_GENERAL_FOLDER_WORKSPACE"" as w
                        left join public.""NtsNote"" as nts on nts.""Id""=w.""NtsNoteId"" and nts.""IsDeleted"" = false and nts.""CompanyId"" = '{_repo.UserContext.CompanyId}'
						left join public.""LegalEntity"" as l on w.""LegalEntityId"" = l.""Id"" and l.""IsDeleted"" = false and l.""CompanyId"" = '{_repo.UserContext.CompanyId}'
						left join public.""User"" as u on w.""CreatedBy""=u.""Id"" and u.""IsDeleted"" = false and u.""CompanyId"" = '{_repo.UserContext.CompanyId}'
						left join public.""NtsNote"" as nts1 on nts.""ParentNoteId""=nts1.""Id"" and nts1.""IsDeleted"" = false and nts1.""CompanyId"" = '{_repo.UserContext.CompanyId}'
                        left join public.""LOV"" as wt on w.""TypeId""=wt.""Id"" and wt.""IsDeleted""=false and wt.""CompanyId""='{_repo.UserContext.CompanyId}'								 
                        Where w.""IsDeleted"" = false and (wt.""Code""!='MY_WORKSPACE' or wt.""Code"" is null)
                        and w.""CompanyId"" = '{_repo.UserContext.CompanyId}'  and nts.""PortalId"" = '{_repo.UserContext.PortalId}' order by w.""CreatedDate"" desc ";

            var list = await _queryRepo.ExecuteQueryList<WorkspaceViewModel>(query, null);

            return list;
        }
        public async Task<WorkspaceViewModel> GetWorkspaceEdit(string workspaceId)
        {
            var query = "";

            query = @$"select nts.""Id"" as ""NoteId"",nts.""Id"" as ""Id"",nts.""NoteSubject"" as ""WorkspaceName"",
 nts.""SequenceOrder"" as ""SequenceOrder"",nts1.""Id"" as ""ParentNoteId"" ,w.""LegalEntityId"" as ""LegalEntityId"",w.""Code"" as Code

                                from cms.""N_GENERAL_FOLDER_WORKSPACE"" as w

                                left join public.""NtsNote"" as nts on nts.""Id""=w.""NtsNoteId"" and nts.""IsDeleted"" = false and nts.""CompanyId"" = '{_repo.UserContext.CompanyId}'
								-- left join public.""LegalEntity"" as l on w.""LegalEntityId"" = l.""Id"" 
								--  left join public.""User"" as u on w.""CreatedBy""=u.""Id""
								left join public.""NtsNote"" as nts1 on nts.""ParentNoteId""=nts1.""Id"" and nts1.""IsDeleted"" = false and nts1.""CompanyId"" = '{_repo.UserContext.CompanyId}'
						          where nts.""Id""='{workspaceId}'and w.""IsDeleted"" = false and w.""CompanyId"" = '{_repo.UserContext.CompanyId}' order by w.""CreatedDate"" desc ";




            var queryData = await _queryRepo.ExecuteQuerySingle<WorkspaceViewModel>(query, null);
            var list = queryData;
            return list;
        }
        public async Task<List<WorkspaceViewModel>> DocumentTemplateList(string workspaceId)
        {
            //  var query = @$"select d.*,d.""DocumentTypeId"" ,w.""Id"" ,t.""DisplayName"" from Cms.""N_DMS_WorkspaceDocType"" as d

            ////                          left join public.""NtsNote"" as nts on nts.""Id""=d.""NtsNoteId""
            //// left join public.""NtsNote"" as nts1 on nts1.""Id""=nts.""ParentNoteId"" and nts1.""Id""='{NoteId}'
            ////left join cms.""N_GENERAL_FOLDER_WORKSPACE"" AS w on w.""NtsNoteId""=nts1.""Id"" 
            ////                     left join public.""Template"" as t on t.""Id""=d.""DocumentTypeId"""
            var query = @$"select d.""DocumentTypeId"" as DocumentTypeIds,d.""NtsNoteId"" as ""DocumentTypeNoteId"" from
cms.""N_GENERAL_FOLDER_WORKSPACE"" AS w
join public.""NtsNote"" as p on p.""Id""=w.""NtsNoteId"" and p.""IsDeleted"" = false and p.""CompanyId"" = '{_repo.UserContext.CompanyId}'
join public.""NtsNote"" as c on c.""ParentNoteId""=p.""Id"" and c.""IsDeleted"" = false and c.""CompanyId"" = '{_repo.UserContext.CompanyId}'
join cms.""N_DMS_WorkspaceDocType"" as d on d.""NtsNoteId""=c.""Id"" and d.""IsDeleted"" = false and d.""CompanyId"" = '{_repo.UserContext.CompanyId}'
where p.""Id""='{workspaceId}' and w.""IsDeleted"" = false and w.""CompanyId"" = '{_repo.UserContext.CompanyId}'";


            var list = await _queryRepo.ExecuteQueryList<WorkspaceViewModel>(query, null);
            //var taskList = list.Where(x => x.TemplateType == TemplateTypeEnum.Task && x.TaskType != TaskTypeEnum.StepTask);
            return list;
        }



        public async Task RestoreBinDocument(string NoteId)
        {
            var query = $@"Update public.""NtsNote"" set ""IsDeleted""=false where ""Id""='{NoteId}'";
            await _queryRepo.ExecuteCommand(query, null);
            
        }


        public async Task RestoreArchiveDocument(string Id, string TableMetadataid)
        {

            //var tableMeta = await _tableMetadataBusiness.GetSingle(x => x.Id == TableMetadataid);
            //var query = $@"Update cms.""{ tableMeta.Name}""  set ""IsArchived""=false where ""Id""='{Id}'";


            var query = $@"Update public.""NtsNote"" set ""IsArchived""=false where ""Id""='{Id}'";
            await _queryRepo.ExecuteCommand(query, null);
            
        }
        public async Task<List<DocumentPermissionViewModel>> ViewPermissionList(string NoteId)
        {
            var query = @$"select D.""Id"",D.""PermissionType"", D.""Access"" ,
u.""Name"" as ""UserPermissionGroup"",D.""AppliesTo"",D.""InheritedFrom"",w.""NtsNoteId""
from public.""DocumentPermission"" as D
left join public.""UserGroup"" as u on u.""Id""=D.""PermittedUserGroupId"" and u.""IsDeleted""=false and u.""CompanyId"" = '{_repo.UserContext.CompanyId}'
left join cms.""N_GENERAL_FOLDER_WORKSPACE"" AS w on w.""NtsNoteId""=D.""NoteId"" and w.""IsDeleted""=false and w.""CompanyId"" = '{_repo.UserContext.CompanyId}'
where w.""NtsNoteId""='{NoteId}' and D.""IsDeleted""=false and D.""CompanyId"" = '{_repo.UserContext.CompanyId}'";


            var list = await _queryRepo.ExecuteQueryList<DocumentPermissionViewModel>(query, null);
            //var taskList = list.Where(x => x.TemplateType == TemplateTypeEnum.Task && x.TaskType != TaskTypeEnum.StepTask);
            return list;
        }
        public async Task DeleteWorkspace(string NoteId)
        {
            var query = $@"update  cms.""N_GENERAL_FOLDER_WORKSPACE"" set ""IsDeleted""=true where ""NtsNoteId""='{NoteId}'";
            await _queryRepo.ExecuteCommand(query, null);

        }

        public async Task<IList<DocumentPermissionViewModel>> GetNotePermissionDataForUser(string noteId)
        {
            var cypher = string.Concat($@"Select m.""PermissionType"" as PermissionType, m.""Access"" as Access, m.""AppliesTo"" as AppliesTo, m.""Id"" as Id, a.""Id"" as PermittedUserGroupId,m.""InheritedFrom"" as InheritedFrom,
                                        case when u.""Id"" is not null then u.""Name"" else a.""Name"" end as Principal,m.""Isowner"" as Isowner,u.""Id"" as PermittedUserId,n.""DisablePermissionInheritance"" as DisablePermissionInheritance,m.""IsInherited"" as IsInherited
from 
public.""DocumentPermission"" as m 
join public.""NtsNote"" as n on n.""Id""=m.""NoteId""  and n.""IsDeleted""=false and n.""CompanyId"" = '{_repo.UserContext.CompanyId}' and n.""Id""='{noteId}'
left join public.""User"" as u on u.""Id""=m.""PermittedUserId"" and u.""IsDeleted""=false and u.""CompanyId"" = '{_repo.UserContext.CompanyId}' 
left join public.""UserGroup"" as a on a.""Id""=m.""PermittedUserGroupId"" and a.""IsDeleted""=false and a.""CompanyId"" = '{_repo.UserContext.CompanyId}'
left join public.""UserGroupUser"" as ug on ug.""UserGroupId""=a.""Id"" and ug.""UserId""=u.""Id"" and a.""IsDeleted""=false and a.""CompanyId"" = '{_repo.UserContext.CompanyId}' and ug.""IsDeleted""=false
where m.""IsDeleted""=false and m.""CompanyId"" = '{_repo.UserContext.CompanyId}' and u.""Id"" = '{_repo.UserContext.UserId}'
");

            return await _queryRepo.ExecuteQueryList<DocumentPermissionViewModel>(cypher, null);
        }
        public async Task<IList<DocumentPermissionViewModel>> GetNotePermissionData(string noteId)
        {
            var cypher = string.Concat($@"Select m.""PermissionType"" as PermissionType, m.""Access"" as Access, m.""AppliesTo"" as AppliesTo, m.""Id"" as Id, a.""Id"" as PermittedUserGroupId,m.""InheritedFrom"" as InheritedFrom,
                                        case when u.""Id"" is not null then u.""Name"" else a.""Name"" end as Principal,m.""Isowner"" as Isowner,m.""Isowner"" as IsOwner,u.""Id"" as PermittedUserId,n.""DisablePermissionInheritance"" as DisablePermissionInheritance,m.""IsInherited"" as IsInherited,
m.""IsInheritedFromChild"" as IsInheritedFromChild
from 
public.""DocumentPermission"" as m 
join public.""NtsNote"" as n on n.""Id""=m.""NoteId""  and n.""IsDeleted""=false and n.""CompanyId"" = '{_repo.UserContext.CompanyId}' and n.""Id""='{noteId}'
left join public.""User"" as u on u.""Id""=m.""PermittedUserId"" and u.""IsDeleted""=false and u.""CompanyId"" = '{_repo.UserContext.CompanyId}'
left join public.""UserGroup"" as a on a.""Id""=m.""PermittedUserGroupId"" and a.""IsDeleted""=false and a.""CompanyId"" = '{_repo.UserContext.CompanyId}'
where m.""IsDeleted""=false and m.""CompanyId"" = '{_repo.UserContext.CompanyId}'
");

            return await _queryRepo.ExecuteQueryList<DocumentPermissionViewModel>(cypher, null);
        }
        public async Task<IList<DocumentPermissionViewModel>> GetNotePermissionDataExceptDeafultPermission(string noteId)
        {
            var cypher = string.Concat($@"Select m.""PermissionType"" as PermissionType, m.""Access"" as Access, m.""AppliesTo"" as AppliesTo, m.""Id"" as Id, a.""Id"" as PermittedUserGroupId,m.""InheritedFrom"" as InheritedFrom,
                                        case when u.""Id"" is not null then u.""Name"" else a.""Name"" end as Principal,m.""Isowner"" as Isowner,u.""Id"" as PermittedUserId,n.""DisablePermissionInheritance"" as DisablePermissionInheritance,m.""IsInherited"" as IsInherited,
m.""IsInheritedFromChild"" as IsInheritedFromChild
from 
public.""DocumentPermission"" as m 
join public.""NtsNote"" as n on n.""Id""=m.""NoteId""  and n.""IsDeleted""=false and n.""CompanyId"" = '{_repo.UserContext.CompanyId}' and n.""Id""='{noteId}'
left join public.""User"" as u on u.""Id""=m.""PermittedUserId"" and u.""IsDeleted""=false and u.""CompanyId"" = '{_repo.UserContext.CompanyId}'
left join public.""UserGroup"" as a on a.""Id""=m.""PermittedUserGroupId"" and a.""IsDeleted""=false and a.""CompanyId"" = '{_repo.UserContext.CompanyId}'
where m.""IsDeleted""=false and m.""CompanyId"" = '{_repo.UserContext.CompanyId}' and m.""Isowner""=false
");

            return await _queryRepo.ExecuteQueryList<DocumentPermissionViewModel>(cypher, null);
        }
        public async Task<DocumentPermissionViewModel> GetNotePermissionDataPermissionId(string noteId, string permissionId)
        {
            var cypher = string.Concat($@"Select m.""PermissionType"" as PermissionType, m.""Access"" as Access, m.""AppliesTo"" as AppliesTo,
                        m.""Id"" as Id, a.""Id"" as PermittedUserGroupId,m.""InheritedFrom"" as InheritedFrom,
                        case when u.""Id"" is not null then u.""Name"" else a.""Name"" end as Principal,
                        m.""Isowner"" as Isowner,u.""Id"" as PermittedUserId,n.""DisablePermissionInheritance"" as DisablePermissionInheritance,
                        m.""IsInherited"" as IsInherited,m.""IsInheritedFromChild"" as IsInheritedFromChild
                        from public.""DocumentPermission"" as m 
                        join public.""NtsNote"" as n on n.""Id""=m.""NoteId""  and n.""IsDeleted""=false and n.""CompanyId"" = '{_repo.UserContext.CompanyId}' and n.""Id""='{noteId}'
                        left join public.""User"" as u on u.""Id""=m.""PermittedUserId"" and u.""IsDeleted""=false and u.""CompanyId"" = '{_repo.UserContext.CompanyId}'
                        left join public.""UserGroup"" as a on a.""Id""=m.""PermittedUserGroupId"" and a.""IsDeleted""=false and a.""CompanyId"" = '{_repo.UserContext.CompanyId}'
                        where m.""Id""='{permissionId}' and m.""IsDeleted""=false and m.""CompanyId"" = '{_repo.UserContext.CompanyId}' and m.""Isowner""=false
                        ");

            return await _queryRepo.ExecuteQuerySingle<DocumentPermissionViewModel>(cypher, null);
        }

        public async Task<FolderViewModel> GetAllParents(string noteId, List<FolderViewModel> parentList)
        {
            var cypher = string.Concat($@"select n.""Id"" as Id, n.""NoteSubject"" as Name,p.""Id"" as ParentId,p.""NoteSubject"" as ParentName, p.""DisablePermissionInheritance"" as DisablePermissionInheritance
                from public.""NtsNote"" as n 
                join public.""NtsNote"" as p on p.""Id""=n.""ParentNoteId""  and p.""IsDeleted""=false and p.""CompanyId"" = '{_repo.UserContext.CompanyId}'
                where n.""Id""='{noteId}' and n.""IsDeleted""=false and n.""CompanyId"" = '{_repo.UserContext.CompanyId}'

                ");

            var list = await _queryRepo.ExecuteQuerySingle<FolderViewModel>(cypher, null);
            return list;
        }

        public async Task<List<DMSCalenderViewModel>> GetCalenderDetails(string UserdId)
        {
            var query = $@"select Distinct t.""DisplayName"" as TemplateName,t.""Code"" as templatecode, n.""NoteSubject"" as ""Title"", n.""CreatedDate""  as ""Start"",n.""CreatedDate""  as ""End"",n.""CreatedDate""  as ""DueDate"", n.""ExpiryDate"" as ""DueDate"",lov.""Name"" as StatusName,n.""Id"",n.""Id"" as ""NtsId"",lov.""Name""  as NoteStatus,true as IsAllDay,
                      n.""Id"" as ""NoteId"",n.""Id"",n.""IsArchived"" as IsArchived,
                      t.""Code"" as templatecode,n.""ParentNoteId"" as ParentId
                      from Public.""TemplateCategory"" tc 
                           inner join public.""Template"" as t on tc.""Id""=t.""TemplateCategoryId"" and t.""IsDeleted""=false and t.""CompanyId"" = '{_repo.UserContext.CompanyId}'
							Join public.""NtsNote"" as n on n.""TemplateId""=t.""Id"" and n.""IsDeleted""=false and n.""CompanyId"" = '{_repo.UserContext.CompanyId}'
							Left Join public.""User"" as u on n.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId"" = '{_repo.UserContext.CompanyId}'
                             Left Join public.""LOV"" as lov on n.""NoteStatusId""=lov.""Id"" and lov.""IsDeleted""=false and lov.""CompanyId"" = '{_repo.UserContext.CompanyId}'
                inner join public.""DocumentPermission"" as np on np.""NoteId""=n.""Id"" and np.""IsDeleted""=false and np.""CompanyId"" = '{_repo.UserContext.CompanyId}'
                inner join public.""User"" as npu on npu.""Id""=np.""PermittedUserId""  and npu.""Id""='{UserdId}' and npu.""Id""=u.""Id""	and npu.""IsDeleted""=false and npu.""CompanyId"" = '{_repo.UserContext.CompanyId}'
               
                       where tc.""Code"" in ('GENERAL_DOCUMENT') and tc.""IsDeleted""=false and tc.""CompanyId"" = '{_repo.UserContext.CompanyId}'

union
select Distinct t.""DisplayName"" as TemplateName,t.""Code"" as templatecode, n.""NoteSubject"" as ""Title"", n.""CreatedDate""  as ""Start"",n.""CreatedDate""  as ""End"",n.""CreatedDate""  as ""DueDate"", n.""ExpiryDate"" as ""DueDate"",lov.""Name"" as StatusName,n.""Id"",n.""Id"" as ""NtsId"",lov.""Name""  as NoteStatus,true as IsAllDay,
                      n.""Id"" as ""NoteId"",n.""Id"",n.""IsArchived"" as IsArchived,
                      t.""Code"" as templatecode,n.""ParentNoteId"" as ParentId
                      from Public.""TemplateCategory"" tc 
                       inner join public.""Template"" as t on tc.""Id""=t.""TemplateCategoryId"" and t.""IsDeleted""=false and t.""CompanyId"" = '{_repo.UserContext.CompanyId}'
							Join public.""NtsNote"" as n on n.""TemplateId""=t.""Id"" and n.""IsDeleted""=false and n.""CompanyId"" = '{_repo.UserContext.CompanyId}'
							Left Join public.""User"" as u on n.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId"" = '{_repo.UserContext.CompanyId}'
                             Left Join public.""LOV"" as lov on n.""NoteStatusId""=lov.""Id"" and lov.""IsDeleted""=false and lov.""CompanyId"" = '{_repo.UserContext.CompanyId}'
   left join public.""DocumentPermission"" as np2 on np2.""NoteId""=n.""Id"" and np2.""IsDeleted""=false and np2.""CompanyId"" = '{_repo.UserContext.CompanyId}'

                inner join public.""UserGroupUser"" as npwg on npwg.""UserGroupId""=np2.""PermittedUserGroupId"" and npwg.""IsDeleted""=false and npwg.""CompanyId"" = '{_repo.UserContext.CompanyId}'
				inner join public.""User"" as npwgu on npwgu.""Id""=npwg.""UserId""	and npwgu.""Id""='{UserdId}'	and npwgu.""IsDeleted""=false and npwgu.""CompanyId"" = '{_repo.UserContext.CompanyId}'	
                 where tc.""Code"" in ('GENERAL_DOCUMENT') and tc.""IsDeleted""=false and tc.""CompanyId"" = '{_repo.UserContext.CompanyId}' and n.""IsArchived""=True


";


            var resuly = await _queryRepo.ExecuteQueryList<DMSCalenderViewModel>(query, null);
            // foreach (var item in resuly)
            // {
            //
            //     item.End =DateTime.Now;
            //     item.DueDate =DateTime.Now;
            //         
            //
            // }

            return resuly;


        }



        public async Task<List<NoteLinkShareViewModel>> GetDocumentLinksData(string id)
        {

            var query = $@"select N.""Id"", S.""To"",S.""Link"",N.""ExpiryDate"" as ExpiryDate, U.""Name"" as ""From"",RN.""Id"" as NoteId
from public.""NtsNote""  as N inner join cms.""N_DMS_DocumentShareLink""  as S on N.""Id""=S.""NtsNoteId"" and S.""IsDeleted""=false and S.""CompanyId""='{_repo.UserContext.CompanyId}'
inner join public.""User"" as U on U.""Id""=N.""OwnerUserId"" and U.""IsDeleted""=false and U.""CompanyId""='{_repo.UserContext.CompanyId}'
left join public.""NtsNote"" as RN on RN.""Id""=N.""ParentNoteId""  and RN.""IsDeleted""=false and RN.""CompanyId""='{_repo.UserContext.CompanyId}'
where N.""IsDeleted""=false and N.""CompanyId""='{_repo.UserContext.CompanyId}' and RN.""Id""='{id}'";


            var result = await _queryRepo.ExecuteQueryList<NoteLinkShareViewModel>(query, null);

            return result;
        }


        public async Task<NoteLinkShareViewModel> GetNoteDocumentByKey(string Key)
        {

            var query = $@"select N.""Id"", S.""To"",S.""Link"",N.""ExpiryDate"" as ExpiryDate, U.""Name"" as ""From"",RN.""Id"" as NoteId, N.""ReferenceId"" , N.""ReferenceType"",N.""ParentNoteId"",p.""LogoId""
from public.""NtsNote""  as N inner join cms.""N_DMS_DocumentShareLink""  as S on N.""Id""=S.""NtsNoteId"" and S.""IsDeleted""=false and S.""CompanyId""='{_repo.UserContext.CompanyId}'
inner join public.""User"" as U on U.""Id""=N.""OwnerUserId"" and U.""IsDeleted""=false and U.""CompanyId""='{_repo.UserContext.CompanyId}'
left join public.""NtsNote"" as RN on RN.""Id""=N.""ParentNoteId"" and RN.""IsDeleted""=false and RN.""CompanyId""='{_repo.UserContext.CompanyId}'
left join public.""Portal"" as p on p.""Id""=RN.""PortalId"" and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
where N.""IsDeleted""=false and N.""CompanyId""='{_repo.UserContext.CompanyId}' and S.""Key""='{Key}'";


            var result = await _queryRepo.ExecuteQuerySingle<NoteLinkShareViewModel>(query, null);

            return result;
        }

        
        public async Task DeleteLink(string Id)
        {

            var Query = $@"update cms.""N_DMS_DocumentShareLink"" set ""IsDeleted""=true where ""NtsNoteId""='{Id}'";

            await _queryRepo.ExecuteCommand(Query, null);

            

        }
        public async Task<WorkspaceViewModel> GetLegalEntity(string parentId)
        {
            var query = "";

            query = @$"select w.""LegalEntityId"" as ""LegalEntityId""

                                from cms.""N_GENERAL_FOLDER_WORKSPACE"" as w
                       Join public.""NtsNote"" as n on n.""Id""=w.""NtsNoteId"" and n.""IsDeleted""=false
						          where n.""Id""='{parentId}'and w.""IsDeleted"" = false and w.""CompanyId"" = '{_repo.UserContext.CompanyId}'";
            var queryData = await _queryRepo.ExecuteQuerySingle<WorkspaceViewModel>(query, null);
            var list = queryData;
            return list;
        }
        public async Task<TemplateViewModel> DeletePermissionByDocumentIds(string ids)
        {
            var cypher = $@" update public.""DocumentPermission"" set ""IsDeleted""=true
            where ""Id"" in ({ids}) ";
            var result = await _queryRepo.ExecuteQuerySingle<TemplateViewModel>(cypher, null);
            return result;
        }

        public async Task CreateBulkPermission(List<DocumentPermissionViewModel> permissions)
        {
            List<string> queryList = new List<string>();
            foreach (var item in permissions)
            {
                queryList.Add(@$"Insert into public.""DocumentPermission"" (""Id"",""PermissionType"",""Access"",""AppliesTo"",""NoteId"",""PermittedUserId"",""PermittedUserGroupId"",""CreatedDate"",""CreatedBy"",""LastUpdatedDate"",""LastUpdatedBy"",""IsDeleted"",""SequenceOrder"",""CompanyId"",""LegalEntityId"",""Status"",""VersionNo"",""PortalId"",""InheritedFrom"",""Isowner"",""IsInherited"",""IsInheritedFromChild"")
                Values('{item.Id}',{(int)item.PermissionType},{(int)item.Access},{(int)item.AppliesTo},'{item.NoteId}',NULLIF('{item.PermittedUserId}', '')
               ,NULLIF('{item.PermittedUserGroupId}',''),'{item.CreatedDate}','{item.CreatedBy}','{item.LastUpdatedDate}','{item.LastUpdatedBy}',{item.IsDeleted},0,'{item.CompanyId}','{item.LegalEntityId}',{(int)item.Status},{item.VersionNo},'{item.PortalId}','{item.InheritedFrom}',{item.Isowner},{item.IsInherited},{item.IsInheritedFromChild})");
            }
            if (queryList.Count > 0)
            {
                var query = String.Join(";", queryList);
                await _queryRepo.ExecuteCommand(query, null);
            }
        }


        public async Task UpdateBulkPermission(List<DocumentPermissionViewModel> permissions)
        {
            List<string> queryList = new List<string>();
            foreach (var item in permissions)
            {
                queryList.Add(@$"Update public.""DocumentPermission"" set ""PermissionType""={(int)item.PermissionType},""Access""={(int)item.Access},
                ""AppliesTo""={(int)item.AppliesTo},""PermittedUserId""=NULLIF('{item.PermittedUserId}', ''),
                ""PermittedUserGroupId""=NULLIF('{item.PermittedUserGroupId}',''),""LastUpdatedDate""='{item.LastUpdatedDate}',
                ""LastUpdatedBy""='{item.LastUpdatedBy}',""IsDeleted""={item.IsDeleted}
                where ""Id""='{item.Id}'");
            }
            if (queryList.Count > 0)
            {
                var query = String.Join(";", queryList);
                await _queryRepo.ExecuteCommand(query, null);
            }
        }


        public async Task<DMSDocumentViewModel> GetFileId(string Id)
        {
            var selectQry = "";
            var i = 1;

            var tempCategory = await _templateCategoryBusiness.GetList(x => x.Code == "GENERAL_DOCUMENT" && x.TemplateType == TemplateTypeEnum.Note);

            foreach (var item1 in tempCategory)
            {
                var templateList = await _templateBusiness.GetList(x => x.TemplateCategoryId == item1.Id);


                foreach (var item in templateList.Where(x => x.TableMetadataId != null))
                {
                    var tableMeta = await _tableMetadataBusiness.GetSingle(x => x.Id == item.TableMetadataId);
                    if (item.Code == "GENERAL_DOCUMENT" || item.Code == "ENGINEERING_SUBCONTRACT")

                    {

                        if (i != 1)
                        {
                            selectQry += " union ";
                        }

                        selectQry = @$"{ selectQry}
                                select udf.""fileAttachment"" as ""DocumentId"",'{item.TableMetadataId}' as TableMetadataId, n.""NoteNo"" as ""NoteVersionNo"", lov.""Name"" as ""StatusName"",t.""DisplayName"" as DocumentType,n.""LastUpdatedDate"" as ""UpdatedDate""
                                ,u.""Name"" as ""UpdatedByUser"",u.""PhotoId"" as PhotoId,
                                t.""Code"" as templatecode
                                from cms.""{ tableMeta.Name}"" as udf
                                Join public.""NtsNote"" as n on n.""Id""=udf.""NtsNoteId"" and n.""IsDeleted""=false
                                inner join public.""Template"" as t on t.""Id""= n.""TemplateId""
                                inner join Public.""TemplateCategory"" tc on tc.""Id""=t.""TemplateCategoryId""
                                       Left Join public.""User"" as u on n.""OwnerUserId""=u.""Id"" 
                                       Left Join public.""LOV"" as lov on n.""NoteStatusId""=lov.""Id""
            						 where udf.""IsDeleted""=false  and n.""Id""='{Id}'";

                        i++;
                    }
                    else
                    {
                        if (i != 1)
                        {
                            selectQry += " union ";
                        }

                        selectQry = @$"{ selectQry}
                                select udf.""attachment"" as ""DocumentId"",'{item.TableMetadataId}' as TableMetadataId, n.""NoteNo"" as ""NoteVersionNo"", lov.""Name"" as ""StatusName"",t.""DisplayName"" as DocumentType,n.""LastUpdatedDate"" as ""UpdatedDate""
            ,u.""Name"" as ""UpdatedByUser"",u.""PhotoId"" as PhotoId,
            t.""Code"" as templatecode
            from cms.""{ tableMeta.Name}"" as udf
            Join public.""NtsNote"" as n on n.""Id""=udf.""NtsNoteId"" and n.""IsDeleted""=false
            inner join public.""Template"" as t on t.""Id""= n.""TemplateId""
            inner join Public.""TemplateCategory"" tc on tc.""Id""=t.""TemplateCategoryId""
                                       Left Join public.""User"" as u on n.""OwnerUserId""=u.""Id"" 
                                       Left Join public.""LOV"" as lov on n.""NoteStatusId""=lov.""Id""
            						 where udf.""IsDeleted""=false  and n.""Id""='{Id}'";





                        i++;


                    }
                }
            }

            var result = await _queryRepo.ExecuteQuerySingle<DMSDocumentViewModel>(selectQry, null);

            return result;
        }

        public async Task<List<DMSDocumentViewModel>> getDocData(string NoteId, string UserId)
        {
            var query = $@"select D.""NoteId"" from public.""DocumentPermission"" D
                          inner join  public.""User"" U on U.""Id"" =D.""PermittedUserId"" and u.""IsDeleted""=true and u.""CompanyId"" = '{_repo.UserContext.CompanyId}'
                           where D.""NoteId"" = '{NoteId}' and D.""PermittedUserId"" = '{UserId}' and D.""PermissionType"" = 0 and D.""IsDeleted""=true and D.""CompanyId"" = '{_repo.UserContext.CompanyId}'";
            var ressult = await _queryRepo.ExecuteQueryList<DMSDocumentViewModel>(query, null);
            return ressult;
        }
        public async Task<List<DMSTagViewModel>> GetDMSTagData()
        {
            var query = $@"select t.*,n.""Id"" as NoteId,tp.""TagName"" as TagParentName
                            from cms.""N_DMS_Tags"" as t
                            inner join public.""NtsNote"" as n on n.""Id""=t.""NtsNoteId"" and n.""IsDeleted""=false
                            left join cms.""N_DMS_Tags"" as tp on tp.""Id""=t.""TagParentId"" and tp.""IsDeleted""=false
                            where t.""IsDeleted""=false ";
            var queryData = await _queryRepo.ExecuteQueryList<DMSTagViewModel>(query, null);
            return queryData;
        }
        public async Task<List<IdNameViewModel>> GetDMSTagIdNameList()
        {
            var query = $@"select t.""Id"" as Id,t.""TagName"" as Name
                            from cms.""N_DMS_Tags"" as t
                            inner join public.""NtsNote"" as n on n.""Id""=t.""NtsNoteId"" and n.""IsDeleted""=false
                            where t.""IsDeleted""=false ";
            var queryData = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return queryData;
        }
        public async Task<DMSTagViewModel> GetDMSTagDetails(string tagId)
        {
            var query = $@"select t.*,n.""Id"" as NoteId,tp.""TagName"" as TagParentName
                            from cms.""N_DMS_Tags"" as t
                            inner join public.""NtsNote"" as n on n.""Id""=t.""NtsNoteId"" and n.""IsDeleted""=false
                            left join cms.""N_DMS_Tags"" as tp on tp.""Id""=t.""TagParentId"" and tp.""IsDeleted""=false
                            where t.""IsDeleted""=false and t.""Id""='{tagId}' ";
            var queryData = await _queryRepo.ExecuteQuerySingle<DMSTagViewModel>(query, null);
            return queryData;
        }
        public async Task DeleteDMSTag(string tagId)
        {
            var query = $@"Update cms.""N_DMS_Tags"" set ""IsDeleted""=true where ""Id""='{tagId}' ";
            await _queryRepo.ExecuteCommand(query, null);
        }
        public async Task<List<DMSDocumentViewModel>> getDocumentPermission(string NoteId, string UserId)
        {
            var query = $@"select D.""NoteId"" from public.""DocumentPermission"" D
                        inner join public.""UserGroupUser"" G on G.""UserGroupId""=D.""PermittedUserGroupId"" and G.""IsDeleted""=false and G.""CompanyId"" = '{_repo.UserContext.CompanyId}'
                        inner join Public.""User"" U on U.""Id""=G.""UserId"" and U.""IsDeleted""=false and u.""CompanyId"" = '{_repo.UserContext.CompanyId}'
                        where D.""NoteId""='{NoteId}' and U.""Id""='{UserId}' and D.""IsDeleted""=false and D.""CompanyId"" = '{_repo.UserContext.CompanyId}' ";

            var ressult1 = await _queryRepo.ExecuteQueryList<DMSDocumentViewModel>(query, null);
            return ressult1;
        }
        public async Task<List<dynamic>> GetAllDocumentUdfDataByTableName(string tableName, string ids)
        {
            var query = string.Concat($@"Select n.""NoteSubject"" as NoteSubject,n.""NoteDescription"" as NoteDescription,n.""NoteNo"" as NoteNo,n.""CreatedDate"" as NoteCreatedDate,n.""LastUpdatedDate"" as NoteLastUpdatedDate, t.* from cms.""{tableName}""  as t 
                        inner join public.""NtsNote"" as n on n.""Id""=t.""NtsNoteId"" and n.""IsDeleted""=false and t.""IsDeleted""=false and n.""Id"" in ('{ids}')");
            var ressult1 = await _queryRepo.ExecuteQueryList<dynamic>(query, null);
            return ressult1;
        }
        public async Task<List<DocumentSearchViewModel>> GetAllDocumentsWithUdf(string udfs)
        {

            var query = $@"select f.""Id"" as Id,f.""NoteSubject"" as NoteSubject,f.""NoteDescription"" as NoteDescription ,f.""NoteNo"" as NoteNo,tm.""Name"" as UdfTableName              
                        ,f.""CreatedDate"" as CreatedDate,f.""LastUpdatedDate"" as LastUpdatedDate,a.""Id"" as FileId,a.""FileName"" as  FileName,a.""FileExtension"" as FileExtension
                        ,a.""MongoFileId"" as MongoFileId ,a.""MongoPreviewFileId"" as MongoPreviewFileId    
                        from  public.""NtsNote"" as f  
                        join public.""Template"" as tm on tm.""Id""=f.""TemplateId"" and tm.""IsDeleted""=false
        				join (" + udfs + $@") as udf on udf.""NtsNoteId""=f.""Id"" 
                        left join public.""File"" as a on a.""Id""=udf.""Attachment""
                        where f.""IsDeleted""=false and  (f.""IsArchived"" <> true or f.""IsArchived"" is null) and udf.""Attachment"" is not null and udf.""Attachment"" <>'[]'
                        and a.""FileExtractedText"" is null and (a.""FileExtension"" = '.pdf' or a.""FileExtension"" = '.Pdf' or a.""FileExtension"" = '.PDF' or a.""MongoPreviewFileId"" is not null) limit 50
        				";
            //return query;
            var ressult = await _queryRepo.ExecuteQueryList<DocumentSearchViewModel>(query, null);
            return ressult;
        }
        public async Task<List<DocumentSearchViewModel>> GetAllDocumentsWithUdf1(string udfs)
        {

            var query = $@"select f.""Id"" as Id,f.""NoteSubject"" as NoteSubject,f.""NoteDescription"" as NoteDescription ,f.""NoteNo"" as NoteNo,tm.""Name"" as UdfTableName              
                        ,f.""CreatedDate"" as CreatedDate,f.""LastUpdatedDate"" as LastUpdatedDate,a.""Id"" as FileId,a.""FileName"" as  FileName,a.""FileExtension"" as FileExtension
                        ,a.""MongoFileId"" as MongoFileId ,a.""MongoPreviewFileId"" as MongoPreviewFileId    
                        from  public.""NtsNote"" as f  
                        join public.""Template"" as tm on tm.""Id""=f.""TemplateId"" and tm.""IsDeleted""=false
        				join (" + udfs + $@") as udf on udf.""NtsNoteId""=f.""Id"" 
                        left join public.""File"" as a on a.""Id""=udf.""Attachment""
                        where f.""IsDeleted""=false and  (f.""IsArchived"" <> true or f.""IsArchived"" is null) and udf.""Attachment"" is not null and udf.""Attachment"" <>'[]'
                        and a.""SnapshotMongoId"" is null and (a.""FileExtension"" = '.pdf' or a.""FileExtension"" = '.Pdf' or a.""FileExtension"" = '.PDF' or a.""MongoPreviewFileId"" is not null) limit 50
        				";
            //return query;
            var ressult = await _queryRepo.ExecuteQueryList<DocumentSearchViewModel>(query, null);
            return ressult;
        }
        public async Task<List<DocumentSearchViewModel>> GetRecentDocumentWithAttachment(string udfs)
        {

            var query = $@"select f.""Id"" as Id,f.""NoteSubject"" as NoteSubject,f.""NoteDescription"" as NoteDescription ,f.""NoteNo"" as NoteNo,tm.""Name"" as UdfTableName              
                        ,f.""CreatedDate"" as CreatedDate,f.""LastUpdatedDate"" as LastUpdatedDate,a.""Id"" as FileId,COALESCE(a.""FileName"",f.""NoteSubject"") as  FileName,a.""FileExtension"" as FileExtension
                        ,a.""MongoFileId"" as MongoFileId ,a.""MongoPreviewFileId"" as MongoPreviewFileId    
                        from  public.""NtsNote"" as f  
                        join public.""Template"" as tm on tm.""Id""=f.""TemplateId"" and tm.""IsDeleted""=false
        				join (" + udfs + $@") as udf on udf.""NtsNoteId""=f.""Id"" 
                        left join public.""File"" as a on a.""Id""=udf.""Attachment""
                        where f.""IsDeleted""=false and  (f.""IsArchived"" <> true or f.""IsArchived"" is null)  order by f.""CreatedDate"" desc limit 5
        				";            
            var ressult = await _queryRepo.ExecuteQueryList<DocumentSearchViewModel>(query, null);
            return ressult;
        }
        public async Task<List<DashboardDocumentViewModel>> GetTopPendingDocuments(string userId)
        {

            var query = $@"SELECT  n.""Id"" AS ""Id"", n.""NoteNo"" AS ""DocumentNo"",n.""NoteSubject"" AS ""DocumentName"",lov.""Name"" AS ""WorkflowStatus"",ou.""Name"" AS ""OwnerName"",t.""DisplayName"" as ""DocumentType"",wf.""Id"" AS ""WorkflowId"",wf.""ServiceSubject"" AS ""WorkflowName"",wf.""TemplateCode"" AS ""WorkflowTemplateCode"",wf.""StartDate"" AS ""StartDate"",wf.""DueDate"" AS ""DueDate""
                        FROM ""NtsNote"" n
                        JOIN ""Template"" t ON n.""TemplateId"" = t.""Id"" AND t.""IsDeleted"" = false and t.""CompanyId""='{_repo.UserContext.CompanyId}' 
                        JOIN ""TemplateCategory"" tc ON t.""TemplateCategoryId"" = tc.""Id"" AND tc.""Code"" = 'GENERAL_DOCUMENT' AND tc.""IsDeleted"" = false and tc.""CompanyId""='{_repo.UserContext.CompanyId}' 
                        JOIN ""NtsService"" wf ON wf.""ReferenceId"" = n.""Id"" AND wf.""ReferenceType"" = 0 and wf.""CompanyId""='{_repo.UserContext.CompanyId}' 
                        JOIN ""LOV"" lov ON lov.""Id"" = wf.""ServiceStatusId"" and lov.""Code"" in ('SERVICE_STATUS_OVERDUE','SERVICE_STATUS_INPROGRESS') and lov.""CompanyId""='{_repo.UserContext.CompanyId}' 
                        left join
                            (select np1.*,case when npu is null then npwgu.""Id"" else npu.""Id"" end as ""userid""
		                    from  public.""DocumentPermission"" as np1
                            left join public.""User"" as npu on npu.""Id""=np1.""PermittedUserId"" and npu.""Id""='{userId}' and npu.""IsDeleted""=false and npu.""CompanyId""='{_repo.UserContext.CompanyId}' 
                            left join public.""UserGroupUser"" as npwg on npwg.""UserGroupId""=np1.""PermittedUserGroupId""  and npwg.""IsDeleted""=false and npwg.""CompanyId""='{_repo.UserContext.CompanyId}'
                            left join public.""User"" as npwgu on npwgu.""Id""=npwg.""UserId"" and npwgu.""IsDeleted""=false and npwgu.""Id""='{userId}' and npwgu.""IsDeleted""=false and npwgu.""CompanyId""='{_repo.UserContext.CompanyId}'
                            where np1.""IsDeleted""=false)
                                as np on np.""NoteId""=n.""Id"" AND np.""PermissionType"" = 0 AND ""userid"" ='{userId}'
                        left join public.""User"" as ou on ou.""Id""=n.""OwnerUserId"" and ou.""IsDeleted""=false and ou.""CompanyId""='{_repo.UserContext.CompanyId}' 
                        WHERE n.""IsDeleted"" = false AND n.""IsArchived"" = false order by n.""CreatedDate"" desc limit 5
        				";
            var ressult = await _queryRepo.ExecuteQueryList<DashboardDocumentViewModel>(query, null);
            return ressult;
        }
        public async Task<List<IdNameViewModel>> GetAllDocumentSummary(string userId)
        {

            var query = $@"SELECT  Count(*) as ""Count"",lov.""Name"" AS ""Name""
                        FROM ""NtsNote"" n
                        JOIN ""Template"" t ON n.""TemplateId"" = t.""Id"" AND t.""IsDeleted"" = false and t.""CompanyId""='{_repo.UserContext.CompanyId}' 
                        JOIN ""TemplateCategory"" tc ON t.""TemplateCategoryId"" = tc.""Id"" AND tc.""Code"" = 'GENERAL_DOCUMENT' AND tc.""IsDeleted"" = false and tc.""CompanyId""='{_repo.UserContext.CompanyId}' 
                        JOIN ""NtsService"" wf ON wf.""ReferenceId"" = n.""Id"" AND wf.""ReferenceType"" = 0 and wf.""CompanyId""='{_repo.UserContext.CompanyId}' 
                        JOIN ""LOV"" lov ON lov.""Id"" = wf.""ServiceStatusId"" and lov.""CompanyId""='{_repo.UserContext.CompanyId}' 
                        left join
                            (select np1.*,case when npu is null then npwgu.""Id"" else npu.""Id"" end as ""userid""
		                    from  public.""DocumentPermission"" as np1
                            left join public.""User"" as npu on npu.""Id""=np1.""PermittedUserId"" and npu.""Id""='{userId}' and npu.""IsDeleted""=false and npu.""CompanyId""='{_repo.UserContext.CompanyId}' 
                            left join public.""UserGroupUser"" as npwg on npwg.""UserGroupId""=np1.""PermittedUserGroupId""  and npwg.""IsDeleted""=false and npwg.""CompanyId""='{_repo.UserContext.CompanyId}'
                            left join public.""User"" as npwgu on npwgu.""Id""=npwg.""UserId"" and npwgu.""IsDeleted""=false and npwgu.""Id""='{userId}' and npwgu.""IsDeleted""=false and npwgu.""CompanyId""='{_repo.UserContext.CompanyId}'
                            where np1.""IsDeleted""=false)
                                as np on np.""NoteId""=n.""Id"" AND np.""PermissionType"" = 0 AND ""userid"" ='{userId}'
                        left join public.""User"" as ou on ou.""Id""=n.""OwnerUserId"" and ou.""IsDeleted""=false and ou.""CompanyId""='{_repo.UserContext.CompanyId}' 
                        WHERE n.""IsDeleted"" = false AND n.""IsArchived"" = false  group by lov.""Name""
                        ";
            var ressult = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return ressult;
        }
        public async Task<List<IdNameViewModel>> GetAllDocumentAnalysis(string userId)
        {

            var query = $@"SELECT  Count(*) as ""Count"",lov.""Name"" AS ""Name"",t.""DisplayName"" as ""Code""
                        FROM ""NtsNote"" n
                        JOIN ""Template"" t ON n.""TemplateId"" = t.""Id"" AND t.""IsDeleted"" = false and t.""CompanyId""='{_repo.UserContext.CompanyId}' 
                        JOIN ""TemplateCategory"" tc ON t.""TemplateCategoryId"" = tc.""Id"" AND tc.""Code"" = 'GENERAL_DOCUMENT' AND tc.""IsDeleted"" = false and tc.""CompanyId""='{_repo.UserContext.CompanyId}' 
                        JOIN ""NtsService"" wf ON wf.""ReferenceId"" = n.""Id"" AND wf.""ReferenceType"" = 0 and wf.""CompanyId""='{_repo.UserContext.CompanyId}' 
                        JOIN ""LOV"" lov ON lov.""Id"" = wf.""ServiceStatusId"" and lov.""CompanyId""='{_repo.UserContext.CompanyId}' 
                        left join
                            (select np1.*,case when npu is null then npwgu.""Id"" else npu.""Id"" end as ""userid""
		                    from  public.""DocumentPermission"" as np1
                            left join public.""User"" as npu on npu.""Id""=np1.""PermittedUserId"" and npu.""Id""='{userId}' and npu.""IsDeleted""=false and npu.""CompanyId""='{_repo.UserContext.CompanyId}' 
                            left join public.""UserGroupUser"" as npwg on npwg.""UserGroupId""=np1.""PermittedUserGroupId""  and npwg.""IsDeleted""=false and npwg.""CompanyId""='{_repo.UserContext.CompanyId}'
                            left join public.""User"" as npwgu on npwgu.""Id""=npwg.""UserId"" and npwgu.""IsDeleted""=false and npwgu.""Id""='{userId}' and npwgu.""IsDeleted""=false and npwgu.""CompanyId""='{_repo.UserContext.CompanyId}'
                            where np1.""IsDeleted""=false)
                                as np on np.""NoteId""=n.""Id"" AND np.""PermissionType"" = 0 AND ""userid"" ='{userId}'
                        left join public.""User"" as ou on ou.""Id""=n.""OwnerUserId"" and ou.""IsDeleted""=false and ou.""CompanyId""='{_repo.UserContext.CompanyId}' 
                        WHERE n.""IsDeleted"" = false AND n.""IsArchived"" = false  group by lov.""Name"",t.""DisplayName""
                        ";
            var ressult = await _queryRepo.ExecuteQueryList<IdNameViewModel>(query, null);
            return ressult;
        }
        public async Task<List<DashboardDocumentViewModel>> GetTopRecentActivities(string udfs,string userId)
        {

            var query = $@"SELECT  n.""Id"" AS ""Id"", n.""NoteNo"" AS ""DocumentNo"",n.""NoteSubject"" AS ""DocumentName"",lov.""Name"" AS ""WorkflowStatus"",ou.""Name"" AS ""OwnerName"",
                        t.""DisplayName"" as ""DocumentType"",wf.""Id"" AS ""WorkflowId"",wf.""ServiceSubject"" AS ""WorkflowName"",wf.""StartDate"" AS ""StartDate"",wf.""DueDate"" AS ""DueDate"",
                        f.""Id"" as FileId,f.""FileName"" as  FileName,f.""ContentLength"" as ContentLength,n.""CreatedDate"" as ""CreatedDate""
                        FROM ""NtsNote"" n
                        JOIN ""Template"" t ON n.""TemplateId"" = t.""Id"" AND t.""IsDeleted"" = false
                        JOIN ""TemplateCategory"" tc ON t.""TemplateCategoryId"" = tc.""Id"" AND tc.""Code"" = 'GENERAL_DOCUMENT' AND tc.""IsDeleted"" = false
                        JOIN ""NtsService"" wf ON wf.""ReferenceId"" = n.""Id"" AND wf.""ReferenceType"" = 0
                        JOIN ""LOV"" lov ON lov.""Id"" = wf.""ServiceStatusId"" 
                        JOIN (" + udfs + $@") as udf on udf.""NtsNoteId""=n.""Id"" 
                        left join
                            (select np1.*,case when npu is null then npwgu.""Id"" else npu.""Id"" end as ""userid""
		                    from  public.""DocumentPermission"" as np1
                            left join public.""User"" as npu on npu.""Id""=np1.""PermittedUserId"" and npu.""Id""='{userId}' and npu.""IsDeleted""=false and npu.""CompanyId""='{_repo.UserContext.CompanyId}' 
                            left join public.""UserGroupUser"" as npwg on npwg.""UserGroupId""=np1.""PermittedUserGroupId""  and npwg.""IsDeleted""=false and npwg.""CompanyId""='{_repo.UserContext.CompanyId}'
                            left join public.""User"" as npwgu on npwgu.""Id""=npwg.""UserId"" and npwgu.""IsDeleted""=false and npwgu.""Id""='{userId}' and npwgu.""IsDeleted""=false and npwgu.""CompanyId""='{_repo.UserContext.CompanyId}'
                            where np1.""IsDeleted""=false)
                        as np on np.""NoteId""=n.""Id"" AND np.""PermissionType"" = 0 AND ""userid"" ='{userId}'
                        left join public.""User"" as ou on ou.""Id""=n.""OwnerUserId"" and ou.""IsDeleted""=false                         
                        left join public.""File"" as f on f.""Id""=udf.""Attachment""
                        WHERE n.""IsDeleted"" = false AND n.""IsArchived"" = false order by n.""CreatedDate"" desc limit 5
        				";
            var ressult = await _queryRepo.ExecuteQueryList<DashboardDocumentViewModel>(query, null);
            return ressult;
        }
        public async Task<List<DashboardDocumentViewModel>> GetTopRecentDocuments(string udfs, string userId)
        {

            var query = $@"SELECT  n.""Id"" AS ""Id"", n.""NoteNo"" AS ""DocumentNo"",n.""NoteSubject"" AS ""DocumentName"",ou.""Name"" AS ""OwnerName"",
                        t.""DisplayName"" as ""DocumentType"",f.""Id"" as FileId,f.""FileName"" as  FileName,f.""FileExtension"" as ""FileExtension"",f.""ContentLength"" as ContentLength,
                        n.""CreatedDate"" as ""CreatedDate""
                        FROM ""NtsNote"" n
                        JOIN ""Template"" t ON n.""TemplateId"" = t.""Id"" AND t.""IsDeleted"" = false
                        JOIN ""TemplateCategory"" tc ON t.""TemplateCategoryId"" = tc.""Id"" AND tc.""Code"" = 'GENERAL_DOCUMENT' AND tc.""IsDeleted"" = false                       
                        JOIN (" + udfs + $@") as udf on udf.""NtsNoteId""=n.""Id"" 
                        left join
                            (select np1.*,case when npu is null then npwgu.""Id"" else npu.""Id"" end as ""userid""
		                    from  public.""DocumentPermission"" as np1
                            left join public.""User"" as npu on npu.""Id""=np1.""PermittedUserId"" and npu.""Id""='{userId}' and npu.""IsDeleted""=false and npu.""CompanyId""='{_repo.UserContext.CompanyId}' 
                            left join public.""UserGroupUser"" as npwg on npwg.""UserGroupId""=np1.""PermittedUserGroupId""  and npwg.""IsDeleted""=false and npwg.""CompanyId""='{_repo.UserContext.CompanyId}'
                            left join public.""User"" as npwgu on npwgu.""Id""=npwg.""UserId"" and npwgu.""IsDeleted""=false and npwgu.""Id""='{userId}' and npwgu.""IsDeleted""=false and npwgu.""CompanyId""='{_repo.UserContext.CompanyId}'
                            where np1.""IsDeleted""=false)
                        as np on np.""NoteId""=n.""Id"" AND np.""PermissionType"" = 0 AND ""userid"" ='{userId}'
                        left join public.""User"" as ou on ou.""Id""=n.""OwnerUserId"" and ou.""IsDeleted""=false                         
                        left join public.""File"" as f on f.""Id""=udf.""Attachment""
                        WHERE n.""IsDeleted"" = false AND n.""IsArchived"" = false order by n.""CreatedDate"" desc limit 5
        				";
            var ressult = await _queryRepo.ExecuteQueryList<DashboardDocumentViewModel>(query, null);
            return ressult;
        }
        public async Task<int> GetDocumentTypeCount()
        {
            var query = $@"SELECT count(t.*)
                        FROM  ""Template"" t   
                        JOIN ""TemplateCategory"" tc ON t.""TemplateCategoryId"" = tc.""Id"" AND tc.""Code"" = 'GENERAL_DOCUMENT' AND tc.""IsDeleted"" = false and tc.""CompanyId""='{_repo.UserContext.CompanyId}'  
                        where t.""IsDeleted"" = false and t.""CompanyId""='{_repo.UserContext.CompanyId}' 
        				";
            var ressult = await _queryRepo.ExecuteScalar<int>(query, null);
            return ressult;
        }
        public async Task<int> GetDocumentCount()
        {
            var query = $@"SELECT count(n.*)
                        FROM ""NtsNote"" n
                        JOIN ""Template"" t ON n.""TemplateId"" = t.""Id"" AND t.""IsDeleted"" = false and t.""CompanyId""='{_repo.UserContext.CompanyId}'
                        JOIN ""TemplateCategory"" tc ON t.""TemplateCategoryId"" = tc.""Id"" AND tc.""Code"" = 'GENERAL_DOCUMENT' AND tc.""IsDeleted"" = false and tc.""CompanyId""='{_repo.UserContext.CompanyId}'
                        where  n.""IsDeleted"" = false and n.""CompanyId""='{_repo.UserContext.CompanyId}'
        				";
            var ressult = await _queryRepo.ExecuteScalar<int>(query, null);
            return ressult;
        }

        public async Task<List<WorkspaceDMSTree>> GetWorkspacebyUser(string userId)
        {
            var query = string.Concat($@"select Distinct  n.""Id"" as WorkSpaceId,n.""NoteSubject"" as WorkSpaceName,n.""ParentNoteId"" as Parent_ID,t.""DisplayName"" as Folder_Type
                      from Public.""TemplateCategory"" tc 
					  inner join public.""Template"" as t on tc.""Id""=t.""TemplateCategoryId"" and t.""IsDeleted""=false and t.""CompanyId""='{_repo.UserContext.CompanyId}'
							Join public.""NtsNote"" as n on n.""TemplateId""=t.""Id"" and n.""IsDeleted""=false and ""IsArchived""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}'
							Left Join public.""User"" as u on n.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
							Left Join public.""User"" as up on up.""Id""=n.""LastUpdatedBy"" and up.""IsDeleted""=false and up.""CompanyId""='{_repo.UserContext.CompanyId}'							
                             Left Join public.""LOV"" as lov on n.""NoteStatusId""=lov.""Id"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_repo.UserContext.CompanyId}'
                inner join public.""DocumentPermission"" as np on np.""NoteId""=n.""Id"" and np.""IsDeleted""=false and np.""CompanyId""='{_repo.UserContext.CompanyId}'
                inner join public.""User"" as npu on npu.""Id""=np.""PermittedUserId"" and npu.""Id""='{userId}' and npu.""IsDeleted""=false and npu.""CompanyId""='{_repo.UserContext.CompanyId}'

                left join public.""NtsNote"" as pn on pn.""Id""=n.""ParentNoteId"" and pn.""IsDeleted""=false and pn.""CompanyId""='{_repo.UserContext.CompanyId}'
				left join Public.""Template"" as  tp on tp.""Id""=pn.""TemplateId""  and tp.""IsDeleted""=false and tp.""CompanyId""='{_repo.UserContext.CompanyId}'
                       where tc.""Code"" in ('GENERAL_FOLDER') and tc.""IsDeleted""=false and tc.""CompanyId""='{_repo.UserContext.CompanyId}'
					   AND np.""PermittedUserId""='{userId}' order by Folder_Type DESC"
                                       );
            var ressult = await _queryRepo.ExecuteQueryList<WorkspaceDMSTree>(query, null);
            return ressult;
           
        }

        public async Task<List<DMSDocument>> GetDocumentbyUser(string userId)
        {
            var query = string.Concat($@"select Distinct  n.""Id"" as DocumentId,n.""NoteSubject"" as DocumentName,n.""ParentNoteId"" as Parent_ID,t.""DisplayName"" as Folder_Type,pn.""NoteSubject"" as ParentName
                      from Public.""TemplateCategory"" tc 
					  inner join public.""Template"" as t on tc.""Id""=t.""TemplateCategoryId"" and t.""IsDeleted""=false and t.""CompanyId""='{_repo.UserContext.CompanyId}'
							Join public.""NtsNote"" as n on n.""TemplateId""=t.""Id"" and n.""IsDeleted""=false and ""IsArchived""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}'
							Left Join public.""User"" as u on n.""OwnerUserId""=u.""Id"" and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}'
							Left Join public.""User"" as up on up.""Id""=n.""LastUpdatedBy"" and up.""IsDeleted""=false and up.""CompanyId""='{_repo.UserContext.CompanyId}'							
                             Left Join public.""LOV"" as lov on n.""NoteStatusId""=lov.""Id"" and lov.""IsDeleted""=false and lov.""CompanyId""='{_repo.UserContext.CompanyId}'
                inner join public.""DocumentPermission"" as np on np.""NoteId""=n.""Id"" and np.""IsDeleted""=false and np.""CompanyId""='{_repo.UserContext.CompanyId}'
                inner join public.""User"" as npu on npu.""Id""=np.""PermittedUserId"" and npu.""Id""='{userId}' and npu.""IsDeleted""=false and npu.""CompanyId""='{_repo.UserContext.CompanyId}'

                left join public.""NtsNote"" as pn on pn.""Id""=n.""ParentNoteId"" and pn.""IsDeleted""=false and pn.""CompanyId""='{_repo.UserContext.CompanyId}'
				left join Public.""Template"" as  tp on tp.""Id""=pn.""TemplateId""  and tp.""IsDeleted""=false and tp.""CompanyId""='{_repo.UserContext.CompanyId}'
                       where tc.""Code"" in ('GENERAL_DOCUMENT') and tc.""IsDeleted""=false and tc.""CompanyId""='{_repo.UserContext.CompanyId}'
					   AND np.""PermittedUserId""='{userId}' order by Folder_Type DESC"
                                       );
            var ressult = await _queryRepo.ExecuteQueryList<DMSDocument>(query, null);
            return ressult;

        }
        #endregion
    }
}
