using AutoMapper;
using Cms.UI.ViewModel;
using CMS.Business.Interface;
using CMS.Business.Interface.DMS;
using CMS.Common;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CMS.Business
{
    public class DocumentPermissionBusiness: BusinessBase<DocumentPermissionViewModel, DocumentPermission>, IDocumentPermissionBusiness

    {

        private readonly IRepositoryQueryBase<TemplateViewModel> _queryRepo;
        private readonly IServiceProvider _sp;
        private readonly IRepositoryQueryBase<DocumentPermissionViewModel> _querytag;
        private readonly IRepositoryQueryBase<DMSDocumentViewModel> _queryDMSDocument;
        ITemplateCategoryBusiness _templateCategoryBusiness;
        ITemplateBusiness _templateBusiness;
        ITableMetadataBusiness _tableMetadataBusiness;
        private readonly IRepositoryQueryBase<WorkspaceViewModel> _queryworkspace;
        private readonly IRepositoryQueryBase<DMSCalenderViewModel> _querycalndarDocument;
        private readonly IUserContext _userContext;
        private IRepositoryQueryBase<NoteLinkShareViewModel> _QueryShareLink;
        INoteBusiness _noteBusiness;
        IUserGroupUserBusiness _userGroupBusiness;
        //INoteBusiness _note


        // public DocumentPermissionBusiness(IRepositoryBase<DocumentPermissionViewModel, DocumentPermission> repo, IRepositoryQueryBase<DocumentPermissionViewModel> querytag, IRepositoryQueryBase<TemplateViewModel> queryRepo, IMapper autoMapper, IRepositoryQueryBase<DMSDocumentViewModel> queryDMSDocument, ITemplateCategoryBusiness templateCategoryBusiness, ITemplateBusiness templateBusiness, ITableMetadataBusiness tableMetadataBusiness) : base(repo, autoMapper)
        public DocumentPermissionBusiness(IRepositoryBase<DocumentPermissionViewModel, DocumentPermission> repo,IRepositoryQueryBase<WorkspaceViewModel> queryworkspace, IRepositoryQueryBase<DocumentPermissionViewModel> querytag, IRepositoryQueryBase<TemplateViewModel> queryRepo, IMapper autoMapper, IRepositoryQueryBase<DMSDocumentViewModel> queryDMSDocument, ITemplateCategoryBusiness templateCategoryBusiness, ITemplateBusiness templateBusiness, ITableMetadataBusiness tableMetadataBusiness
            , IServiceProvider sp, IUserContext userContext, IRepositoryQueryBase<DMSCalenderViewModel> querycalndarDocument, IRepositoryQueryBase<NoteLinkShareViewModel> QueryShareLink, INoteBusiness noteBusiness, IUserGroupUserBusiness userGroupBusiness) : base(repo, autoMapper)
            
        {
            _querytag = querytag;
            _queryRepo = queryRepo;
            _queryworkspace = queryworkspace;

            _queryDMSDocument = queryDMSDocument;
            _templateCategoryBusiness = templateCategoryBusiness;
            _templateBusiness = templateBusiness;
            _tableMetadataBusiness = tableMetadataBusiness;
            _sp = sp;
            _userContext = userContext;
            _querycalndarDocument = querycalndarDocument;
            _QueryShareLink = QueryShareLink;
            _noteBusiness = noteBusiness;
            _userGroupBusiness = userGroupBusiness;
        }



        public async override Task<CommandResult<DocumentPermissionViewModel>> Create(DocumentPermissionViewModel model)
        {      
            
           
            var result = await base.Create(model);
            if(result.IsSuccess && !model.DisablePermittedNotification)
            {
                await SendNotification(model);
            }
           
            return CommandResult<DocumentPermissionViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        public async Task SendNotification(DocumentPermissionViewModel model)
        {
            var note = await _noteBusiness.GetNoteDetails(new NoteTemplateViewModel { NoteId = model.NoteId, DataAction = DataActionEnum.Read });
            note.OwnerUserId = _repo.UserContext.UserId;
            var notificationTemplate = await _repo.GetSingle<NotificationTemplate, NotificationTemplate>(x => x.Code == "NOTE_PERMITTED_USER_TEMPLATE" && x.NtsType == NtsTypeEnum.Note);
            if (notificationTemplate != null)
            {
                if (model.PermittedUserId.IsNotNull() && model.PermittedUserId != _repo.UserContext.UserId)
                {
                    await _noteBusiness.SendNotification(note, notificationTemplate, model.PermittedUserId);

                }
                if (model.PermittedUserGroupId.IsNotNull())
                {
                    var users = await _userGroupBusiness.GetList(x => x.UserGroupId == model.PermittedUserGroupId);
                    foreach (var item in users)
                    {
                        await _noteBusiness.SendNotification(note, notificationTemplate, item.UserId);
                    }

                }
            }
        }
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

            var queryData = await _querytag.ExecuteQueryList<DocumentPermissionViewModel>(query, null);
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
           
            var ressult = await _queryDMSDocument.ExecuteQueryList<DMSDocumentViewModel>(Query, null);

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
            var ressult = await _queryDMSDocument.ExecuteQueryList<DMSDocumentViewModel>(Query, null);
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

            var ressult = await _queryDMSDocument.ExecuteQueryList<DMSDocumentViewModel>(Query, null);

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


        public async Task<bool> CheckUserPermission(string NoteId, string UserId)
        {
            var query = $@"select D.""NoteId"" from public.""DocumentPermission"" D
                          inner join  public.""User"" U on U.""Id"" =D.""PermittedUserId"" and u.""IsDeleted""=true and u.""CompanyId"" = '{_repo.UserContext.CompanyId}'
                           where D.""NoteId"" = '{NoteId}' and D.""PermittedUserId"" = '{UserId}' and D.""PermissionType"" = 0 and D.""IsDeleted""=true and D.""CompanyId"" = '{_repo.UserContext.CompanyId}'";
            var ressult = await _queryDMSDocument.ExecuteQueryList<DMSDocumentViewModel>(query, null);
            if (ressult.Count > 0)
            {
                return true;
            }
            else
            {

                query = $@"select D.""NoteId"" from public.""DocumentPermission"" D
inner join public.""UserGroupUser"" G on G.""UserGroupId""=D.""PermittedUserGroupId"" and G.""IsDeleted""=false and G.""CompanyId"" = '{_repo.UserContext.CompanyId}'
inner join Public.""User"" U on U.""Id""=G.""UserId"" and U.""IsDeleted""=false and u.""CompanyId"" = '{_repo.UserContext.CompanyId}'
where D.""NoteId""='{NoteId}' and U.""Id""='{UserId}' and D.""IsDeleted""=false and D.""CompanyId"" = '{_repo.UserContext.CompanyId}' ";

                var ressult1 = await _queryDMSDocument.ExecuteQueryList<DMSDocumentViewModel>(query, null);
                if (ressult1.Count > 0)
                {
                    return true;
                }
                else { return false; }


            }
        }


        public async Task<bool> DeleteDocument(string NoteId)
        {
            var query = $@"Update public.""NtsNote"" set ""IsDeleted""=True where ""Id""='{NoteId}'";
            await _queryDMSDocument.ExecuteCommand(query, null);
            return true;
        }

        public async Task<CommandResult<WorkspaceViewModel>> ValidateSequenceOrder(WorkspaceViewModel model)
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

            var result = await _queryworkspace.ExecuteQueryList(query,null);
            var exist = result.Where(x => x.SequenceOrder == model.SequenceOrder && x.NoteId != model.NoteId);

            if (exist.Any())
            {
                return CommandResult<WorkspaceViewModel>.Instance(model, false, "Sequence No already exist");
            }

            return CommandResult<WorkspaceViewModel>.Instance(model, true,"");

        }
        public async Task<CommandResult<WorkspaceViewModel>> ValidateWorkspace(WorkspaceViewModel model)
        {
            var query = $@"select w.*,n.*,n.""Id"" as NoteId from public.""NtsNote"" as n
join cms.""N_GENERAL_FOLDER_WORKSPACE"" as w on n.""Id"" = w.""NtsNoteId"" and w.""IsDeleted"" = false and w.""CompanyId"" = '{_repo.UserContext.CompanyId}'
where  n.""IsDeleted"" = false and n.""CompanyId"" = '{_repo.UserContext.CompanyId}' and n.""IsArchived"" = false";

          var result = await _queryworkspace.ExecuteQueryList(query, null);
            var exist = result.Where(x => x.NoteSubject == model.WorkspaceName && x.NoteId != model.NoteId);

            if (exist.Any())
            {
                return CommandResult<WorkspaceViewModel>.Instance(model, false, "Workspace Name already exist");
            }
            if (model.Code.IsNotNullAndNotEmpty() && result.Any(x => x.Code == model.Code && x.NoteId != model.NoteId))
            {
                return CommandResult<WorkspaceViewModel>.Instance(model, false, "Code already exist");
            }
            return CommandResult<WorkspaceViewModel>.Instance(model, true, "");

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

            var list = await _queryworkspace.ExecuteQueryList<WorkspaceViewModel>(query, null);
            
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

            


            var queryData = await _queryworkspace.ExecuteQuerySingle(query, null);
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


            var list = await _queryworkspace.ExecuteQueryList<WorkspaceViewModel>(query, null);
            //var taskList = list.Where(x => x.TemplateType == TemplateTypeEnum.Task && x.TaskType != TaskTypeEnum.StepTask);
            return list;
        }



        public async Task<bool> RestoreBinDocument(string NoteId)
        {
            var query = $@"Update public.""NtsNote"" set ""IsDeleted""=false where ""Id""='{NoteId}'";
            await _queryDMSDocument.ExecuteCommand(query, null);
            return true;
        }


        public async Task<bool> RestoreArchiveDocument(string Id,string TableMetadataid)
        {

            //var tableMeta = await _tableMetadataBusiness.GetSingle(x => x.Id == TableMetadataid);
            //var query = $@"Update cms.""{ tableMeta.Name}""  set ""IsArchived""=false where ""Id""='{Id}'";


            var query = $@"Update public.""NtsNote"" set ""IsArchived""=false where ""Id""='{Id}'";
            await _queryDMSDocument.ExecuteCommand(query, null);
            return true;
        }
        public async Task<List<DocumentPermissionViewModel>> ViewPermissionList(string NoteId)
        {
            var query = @$"select D.""Id"",D.""PermissionType"", D.""Access"" ,
u.""Name"" as ""UserPermissionGroup"",D.""AppliesTo"",D.""InheritedFrom"",w.""NtsNoteId""
from public.""DocumentPermission"" as D
left join public.""UserGroup"" as u on u.""Id""=D.""PermittedUserGroupId"" and u.""IsDeleted""=false and u.""CompanyId"" = '{_repo.UserContext.CompanyId}'
left join cms.""N_GENERAL_FOLDER_WORKSPACE"" AS w on w.""NtsNoteId""=D.""NoteId"" and w.""IsDeleted""=false and w.""CompanyId"" = '{_repo.UserContext.CompanyId}'
where w.""NtsNoteId""='{NoteId}' and D.""IsDeleted""=false and D.""CompanyId"" = '{_repo.UserContext.CompanyId}'";


            var list = await _querytag.ExecuteQueryList<DocumentPermissionViewModel>(query, null);
            //var taskList = list.Where(x => x.TemplateType == TemplateTypeEnum.Task && x.TaskType != TaskTypeEnum.StepTask);
            return list;
        }
        public async Task<bool> DeleteWorkspace(string NoteId)
        {
            var note = await _repo.GetSingleById(NoteId);
            if (note != null)
            {
                var query = $@"update  cms.""N_GENERAL_FOLDER_WORKSPACE"" set ""IsDeleted""=true where ""NtsNoteId""='{NoteId}'";
                await _queryRepo.ExecuteCommand(query, null);

                await Delete(NoteId);
                return true;
            }
            return false;
        }

        public async Task ManageInheritedPermission(string noteId, string parentId)
        {
            
            var ParentPermissionList =await GetNotePermissionData(parentId);
            var notePermissions = await GetNotePermissionData(noteId);
            if (ParentPermissionList != null && ParentPermissionList.Count() > 0)
            {
                foreach (var permission in ParentPermissionList.Where(x => x.IsInheritedFromChild == false))
                {
                    if (permission.DisablePermissionInheritance == null || permission.DisablePermissionInheritance == false)
                    {
                        if (permission.AppliesTo == DmsAppliesToEnum.ThisFolderSubFoldersAndFiles && permission.Isowner != true)
                        {
                            if (notePermissions.Any(e => e.PermissionType == permission.PermissionType && e.Principal == permission.Principal && e.Access == permission.Access && e.AppliesTo == permission.AppliesTo))
                            {
                                // Do not create or edit the permission  as it already have such permission
                            }
                            else 
                            {
                                var permissionData = new DocumentPermissionViewModel
                                {
                                    DataAction = DataActionEnum.Create,
                                    PermissionType = permission.PermissionType,
                                    Access = permission.Access,
                                    AppliesTo = permission.AppliesTo,
                                    InheritedFrom =permission.IsInherited==true ? permission.InheritedFrom:permission.Id,
                                    PermittedUserId = permission.PermittedUserId,
                                    PermittedUserGroupId = permission.PermittedUserGroupId,
                                    NoteId = noteId,
                                    Isowner = permission.Isowner,
                                    IsInherited = true,
                                };
                                await Create(permissionData);
                            }
                            
                        }
                    }
                }

            }
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
        public async Task<DocumentPermissionViewModel> GetNotePermissionDataPermissionId(string noteId,string permissionId)
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

        public async Task ManageChildPermissions(string noteId)
        {
            var _documentBusiness = _sp.GetService<IDMSDocumentBusiness>();
            var documents = await _documentBusiness.GetAllFiles(noteId, null, null);
            var docids = documents.Select(x => x.Id);
            var parentPermissions = await GetNotePermissionData(noteId);          
            var allPermision = new List<DocumentPermissionViewModel>();
            List<FolderViewModel> subfoldersList = new List<FolderViewModel>();
            var subfolders = await _documentBusiness.GetAllPermissionChildByParentId(noteId, subfoldersList);
            var ids = subfolders.Select(x => x.Id);
            var noteids = string.Join(",", ids);
            var notePermissions = await _documentBusiness.GetAllNotePermissionByParentId(noteids);
            foreach (var doc in documents)
            {               
                // Create or Edit the Permission for the documents
                var notePermissionscheck = notePermissions.Where(x => x.NoteId == doc.Id).ToList();
                if (doc.DisablePermissionInheritance == null || doc.DisablePermissionInheritance == false)
                {
                    foreach (var permission in parentPermissions.Where(x => x.IsInheritedFromChild == false))
                    {
                        if (permission.AppliesTo != DmsAppliesToEnum.OnlyThisFolder)
                        {
                            if (notePermissionscheck.Any(e => e.PermissionType == permission.PermissionType && e.Principal == permission.Principal && e.Access == permission.Access && e.AppliesTo == permission.AppliesTo 
                            && (permission.IsInherited == true ? e.InheritedFrom == permission.InheritedFrom : true)))
                            {
                                // Do not create or edit the permission  as it already have such permission
                            }
                            else
                            {
                                var existpermission = await GetSingleById(permission.Id);
                                var permissionData = new DocumentPermissionViewModel
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    PermissionType = existpermission.PermissionType,
                                    Access = existpermission.Access,
                                    AppliesTo = existpermission.AppliesTo,
                                    IsInherited = true,
                                    PermittedUserId = existpermission.PermittedUserId,
                                    PermittedUserGroupId = existpermission.PermittedUserGroupId,
                                    NoteId = doc.Id,
                                    InheritedFrom = existpermission.IsInherited==true ? existpermission.InheritedFrom:existpermission.Id,
                                    Isowner = existpermission.Isowner,
                                    DisablePermittedNotification = true,
                                    CompanyId = _userContext.CompanyId,
                                    IsDeleted = false,
                                    CreatedBy = _userContext.UserId,
                                    LastUpdatedBy = _userContext.UserId,
                                    CreatedDate = DateTime.Now,
                                    LastUpdatedDate = DateTime.Now,
                                    LegalEntityId = _userContext.LegalEntityId,
                                    PortalId = _userContext.PortalId,
                                    Status = StatusEnum.Active,
                                    VersionNo = 0,
                                };
                                allPermision.Add(permissionData);                           
                             }
                        }
                    }
                }
            }
            subfolders = subfolders.Where(x => !docids.Contains(x.Id)).ToList();            
            foreach (var folder in subfolders)
            {
                // Create or edit new permission for the subfolder                
                var notePermissionscheck = notePermissions.Where(x => x.NoteId == folder.Id).ToList();
                if (folder.DisablePermissionInheritance == null || folder.DisablePermissionInheritance == false)
                {
                    foreach (var permission in parentPermissions.Where(x => x.IsInheritedFromChild == false))
                    {
                        if (permission.AppliesTo != DmsAppliesToEnum.OnlyThisFolder && permission.AppliesTo != DmsAppliesToEnum.ThisFolderAndFiles)
                        {
                            if (notePermissionscheck.Any(e => e.PermissionType == permission.PermissionType && e.Principal == permission.Principal && e.Access == permission.Access && e.AppliesTo == permission.AppliesTo 
                            && (permission.IsInherited == true ? e.InheritedFrom == permission.InheritedFrom : true)))
                            {
                                // Do not create as it already have such permission
                            }
                            else
                            {
                                var existpermission = permission;
                                var permissionData = new DocumentPermissionViewModel
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    PermissionType = existpermission.PermissionType,
                                    Access = existpermission.Access,
                                    AppliesTo = existpermission.AppliesTo,
                                    IsInherited = true,
                                    PermittedUserId = existpermission.PermittedUserId,
                                    PermittedUserGroupId = existpermission.PermittedUserGroupId,
                                    NoteId = folder.Id,
                                    InheritedFrom = existpermission.IsInherited == true ? existpermission.InheritedFrom : existpermission.Id,
                                    Isowner = existpermission.Isowner,
                                    DisablePermittedNotification = true,
                                    CompanyId = _userContext.CompanyId,
                                    IsDeleted = false,
                                    CreatedBy = _userContext.UserId,
                                    LastUpdatedBy =_userContext.UserId,
                                    CreatedDate = DateTime.Now,
                                    LastUpdatedDate = DateTime.Now,
                                    LegalEntityId = _userContext.LegalEntityId,
                                    PortalId = _userContext.PortalId,
                                    Status = StatusEnum.Active,
                                    VersionNo =0,
                                };
                            allPermision.Add(permissionData);                           
                            }
                        }
                    }
                }               
            }

            await CreateBulkPermission(allPermision);
        }
        public async Task ManageParentPermissions(string noteId,string PermissionId=null)
        {
            var _documentBusiness = _sp.GetService<IDMSDocumentBusiness>();           
            var parents = await _documentBusiness.GetAllParentByNoteId(noteId);
            var allPermision = new List<DocumentPermissionViewModel>();
            var sourcePermissions = new List<DocumentPermissionViewModel>();
            if (PermissionId.IsNotNullAndNotEmpty())
            {
                var permission = await GetNotePermissionDataPermissionId(noteId, PermissionId);
                sourcePermissions.Add(permission);
            }
            else
            {
                var permission = await GetNotePermissionDataExceptDeafultPermission(noteId);
                sourcePermissions.AddRange(permission);
            }
            var ids = parents.Select(x => x.Id);
            var noteids = string.Join(",", ids);
            var parentnotePermissions = await _documentBusiness.GetAllNotePermissionByParentId(noteids);
            foreach (var folder in parents)
            {                               
                var parentnotePermissionscheck = parentnotePermissions.Where(x => x.NoteId == folder.Id);                
                foreach (var permission in sourcePermissions)
                {
                    if (!parentnotePermissionscheck.Any(e => e.PermissionType == permission.PermissionType && e.Principal == permission.Principal && e.Access == permission.Access && e.AppliesTo == permission.AppliesTo 
                    && (permission.IsInheritedFromChild==true?e.InheritedFrom == permission.InheritedFrom:e.InheritedFrom == permission.Id)))
                        {
                            var existpermission = sourcePermissions.Where(x=>x.Id==permission.Id).FirstOrDefault();
                            var permissionData = new DocumentPermissionViewModel
                            {
                                Id = Guid.NewGuid().ToString(),
                                PermissionType = existpermission.PermissionType,
                                Access = DmsAccessEnum.ReadOnly,
                                AppliesTo = DmsAppliesToEnum.OnlyThisFolder,
                                IsInherited = false,
                                IsInheritedFromChild = true,
                                PermittedUserId = existpermission.PermittedUserId,
                                PermittedUserGroupId = existpermission.PermittedUserGroupId,
                                NoteId = folder.Id,
                                InheritedFrom = existpermission.IsInheritedFromChild == true ? existpermission.InheritedFrom : existpermission.Id,
                                Isowner = existpermission.Isowner,
                                DisablePermittedNotification = true,
                                CompanyId = _userContext.CompanyId,
                                IsDeleted = false,
                                CreatedBy = _userContext.UserId,
                                LastUpdatedBy = _userContext.UserId,
                                CreatedDate = DateTime.Now,
                                LastUpdatedDate = DateTime.Now,
                                LegalEntityId = _userContext.LegalEntityId,
                                PortalId = _userContext.PortalId,
                                Status = StatusEnum.Active,
                                VersionNo = 0,
                            };                        
                        allPermision.Add(permissionData);
                    } 
                }                
            }
            await CreateBulkPermission(allPermision);
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
        public async Task DeleteInheritedPermissionFromChildOnPermissionDelete(string noteId, string PermissionId)
        {
            var _documentBusiness = _sp.GetService<IDMSDocumentBusiness>();
            var documents = await _documentBusiness.GetAllFiles(noteId, null, null);
            foreach (var doc in documents)
            {
                var notePermissions = await GetNotePermissionData(doc.Id);
                //  Delete Inherited Permission
                if (doc.DisablePermissionInheritance == null || doc.DisablePermissionInheritance == false)
                {
                    foreach (var permission in notePermissions.Where(x => x.InheritedFrom == PermissionId))
                    {
                       
                        await Delete(permission.Id);
                    }
                }
            }
            List<FolderViewModel> subfoldersList = new List<FolderViewModel>();
            var subfolders = await _documentBusiness.GetAllChildByParentId(noteId, subfoldersList);

            foreach (var folder in subfolders)
            {
                var notePermissions = await GetNotePermissionData(folder.Id);
                if (folder.DisablePermissionInheritance == null || folder.DisablePermissionInheritance == false)
                {
                    var permissions = notePermissions.Where(x => x.InheritedFrom == PermissionId).ToList();
                    foreach (var permission in permissions)
                    {
                        await Delete(permission.Id);
                        await DeleteInheritedPermissionFromChildOnPermissionDelete(folder.Id, permission.Id);
                    }
                 
                }
                //await DeleteInheritedPermissionFromChildOnPermissionDelete(folder.Id, PermissionId);
            }
        }
        public async Task DeleteInheritedPermissionFromParentOnPermissionDelete(string noteId, string PermissionId)
        {
            var _documentBusiness = _sp.GetService<IDMSDocumentBusiness>();           
            List<FolderViewModel> subfoldersList = new List<FolderViewModel>();
            var subfolders = await _documentBusiness.GetAllParentByChildId(noteId, subfoldersList);

            foreach (var folder in subfolders)
            {
                var notePermissions = await GetNotePermissionData(folder.Id);
               // if (folder.DisablePermissionInheritance == null || folder.DisablePermissionInheritance == false)
               // {
                    var permissions = notePermissions.Where(x => x.InheritedFrom == PermissionId && x.IsInheritedFromChild == true).ToList();
                    foreach (var permission in permissions)
                    {
                        await Delete(permission.Id);
                        await DeleteInheritedPermissionFromParentOnPermissionDelete(folder.Id, PermissionId);
                    }

                //}
                //await DeleteInheritedPermissionFromChildOnPermissionDelete(folder.Id, PermissionId);
            }
        }
        //public async Task ManageChildPermissionsOnEdit(string noteId,string PermissionId)
        //{
        //    var _documentBusiness = _sp.GetService<IDMSDocumentBusiness>();
        //    var documents = await _documentBusiness.GetAllFiles(noteId, null, null);
        //    foreach (var doc in documents)
        //    {
        //        var parentPermission = await GetSingleById(PermissionId);
        //        //var parentPermissions = await GetNotePermissionData(noteId);
        //        var notePermissions = await GetNotePermissionData(doc.Id);
        //        //  Edit the Permission for the documents
        //        if (doc.DisablePermissionInheritance == null || doc.DisablePermissionInheritance == false)
        //        {
        //            foreach (var permission in notePermissions.Where(x=>x.InheritedFrom== PermissionId))
        //            {
        //                var notePermission = await GetSingleById(permission.Id);
        //                notePermission.PermissionType = parentPermission.PermissionType;
        //                notePermission.Access = parentPermission.Access;
        //                notePermission.AppliesTo = parentPermission.AppliesTo;
        //                notePermission.PermittedUserId = parentPermission.PermittedUserId;
        //                notePermission.PermittedUserGroupId = parentPermission.PermittedUserGroupId;
        //                notePermission.InheritedFrom = parentPermission.Id;                       
        //                await Edit(notePermission);
        //                //bulk update
        //            }                   
        //        }
        //    }
        //    List<FolderViewModel> subfoldersList = new List<FolderViewModel>();
        //    var subfolders = await _documentBusiness.GetAllChildByParentId(noteId, subfoldersList);
        //   //get all folder and permission in bulk
        //    foreach (var folder in subfolders)
        //    {
        //        // edit new permission for the subfolder 
        //        var parentPermission = await GetSingleById(PermissionId);
        //        //var parentPermissions = await GetNotePermissionData(noteId);
        //        var notePermissions = await GetNotePermissionData(folder.Id);
        //        if (folder.DisablePermissionInheritance == null || folder.DisablePermissionInheritance == false)
        //        {                    
        //            foreach (var permission in notePermissions.Where(x => x.InheritedFrom == PermissionId))
        //            {                        
        //                var notePermission = await GetSingleById(permission.Id);
        //                notePermission.PermissionType = parentPermission.PermissionType;
        //                notePermission.Access = parentPermission.Access;
        //                notePermission.AppliesTo = parentPermission.AppliesTo;
        //                notePermission.PermittedUserId = parentPermission.PermittedUserId;
        //                notePermission.PermittedUserGroupId = parentPermission.PermittedUserGroupId;
        //                notePermission.InheritedFrom = parentPermission.Id;
        //                await Edit(notePermission);
        //                await ManageChildPermissionsOnEdit(folder.Id, permission.Id);
        //            }
        //            var folderPermission = await GetSingle(x => x.InheritedFrom == PermissionId);
        //            if (folderPermission==null) 
        //            {
        //                await ManageChildPermissions(folder.ParentId);
        //                await ManageParentPermissions(folder.ParentId,PermissionId);
        //            }
        //        }
               
        //    }
        //}
        public async Task ManageChildPermissionsOnEdit(string noteId, string PermissionId)
        {
            var _documentBusiness = _sp.GetService<IDMSDocumentBusiness>();            
            var allPermision = new List<DocumentPermissionViewModel>();            
            var allChildFoldersAndDocuments = await _documentBusiness.GetAllPermissionChildByParentId(noteId, new List<FolderViewModel>());
            var ids = allChildFoldersAndDocuments.Select(x => x.Id).ToList();
            ids.Add(noteId);
            var noteids = string.Join(",", ids);
            var notePermissions = await _documentBusiness.GetAllNotePermissionByParentId(noteids);
            var parentPermission = notePermissions.ToList().Where(x => x.Id == PermissionId).FirstOrDefault();
            var childnotePermissions = notePermissions.ToList().Where(x => x.InheritedFrom == PermissionId).ToList(); 
            foreach (var notePermission in childnotePermissions)
            {  
                notePermission.PermissionType = parentPermission.PermissionType;
                notePermission.Access = parentPermission.Access;
                notePermission.AppliesTo = parentPermission.AppliesTo;
                notePermission.PermittedUserId = parentPermission.PermittedUserId;
                notePermission.PermittedUserGroupId = parentPermission.PermittedUserGroupId;
                notePermission.InheritedFrom = parentPermission.Id;
                notePermission.LastUpdatedBy = _userContext.UserId;
                notePermission.LastUpdatedDate = DateTime.Now;
                allPermision.Add(notePermission);
            }
            if (allPermision.Count > 0)
            {
                await UpdateBulkPermission(allPermision);
            }
        }
        public async Task ManageParentPermissionsOnEdit(string noteId, string PermissionId)
        {
            var _documentBusiness = _sp.GetService<IDMSDocumentBusiness>();
            var parents = await _documentBusiness.GetAllParentByNoteId(noteId);
            var allPermision = new List<DocumentPermissionViewModel>();
            var ids = parents.Select(x => x.Id).ToList();
            ids.Add(noteId);
            var noteIds = string.Join(",", ids);
            var notePermissions = await _documentBusiness.GetAllNotePermissionByParentId(noteIds);
            var childPermission = notePermissions.ToList().Where(x => x.Id == PermissionId).FirstOrDefault();
            var childnotePermissions = notePermissions.ToList().Where(x => x.InheritedFrom == PermissionId && x.IsInheritedFromChild==true).ToList();            
            foreach (var permission in childnotePermissions)
            {
                var notePermission = permission;
                notePermission.PermissionType = childPermission.PermissionType;
                notePermission.Access = DmsAccessEnum.ReadOnly;
                notePermission.AppliesTo = DmsAppliesToEnum.OnlyThisFolder;
                notePermission.PermittedUserId = childPermission.PermittedUserId;
                notePermission.PermittedUserGroupId = childPermission.PermittedUserGroupId;                
                notePermission.InheritedFrom = childPermission.Id;
                notePermission.LastUpdatedBy = _userContext.UserId;
                notePermission.LastUpdatedDate = DateTime.Now;
                allPermision.Add(notePermission);                
            }
            if (allPermision.Count > 0)
            {
                await UpdateBulkPermission(allPermision);
            }
        }
        public async Task DeleteChildPermissions(string noteId, IList<DocumentPermissionViewModel> parentnotePermissions, string PermissionId=null)
        {
            var _documentBusiness = _sp.GetService<IDMSDocumentBusiness>();
            var allPermision = new List<DocumentPermissionViewModel>();
            var allChildFoldersAndDocuments = await _documentBusiness.GetAllPermissionChildByParentId(noteId, new List<FolderViewModel>());
            var ids = allChildFoldersAndDocuments.Select(x => x.Id).ToList();
            ids.Add(noteId);
            var noteids = string.Join(",", ids);
            var notePermissions = await _documentBusiness.GetAllNotePermissionByParentId(noteids);
            if (PermissionId.IsNotNullAndNotEmpty())
            {
                var childnotePermissions = notePermissions.ToList().Where(x => x.InheritedFrom == PermissionId).ToList();
                foreach (var notePermission in childnotePermissions)
                {
                    notePermission.IsDeleted = true;
                    notePermission.LastUpdatedBy = _userContext.UserId;
                    notePermission.LastUpdatedDate = DateTime.Now;
                    allPermision.Add(notePermission);
                }
            }
            else
            {
                //delete existing parent permission
                foreach (var parentnotePermission in parentnotePermissions.Where(x => x.IsInheritedFromChild == true))
                {
                    if (notePermissions.Any(x => x.Id == parentnotePermission.InheritedFrom))
                    {
                        parentnotePermission.IsDeleted = true;
                        parentnotePermission.LastUpdatedBy = _userContext.UserId;
                        parentnotePermission.LastUpdatedDate = DateTime.Now;
                        allPermision.Add(parentnotePermission);
                    }

                }
                //delete child permission inherited from source
                var sourceInheritedPermission= notePermissions.ToList().Where(x => x.IsInherited == true && x.NoteId==noteId).ToList();
                var sourcePermissionInheritedFromIds = sourceInheritedPermission.Select(x => x.InheritedFrom).ToList();
                var childnotePermissions = notePermissions.ToList().Where(x => x.IsInherited == true && sourcePermissionInheritedFromIds.Contains(x.InheritedFrom)).ToList();
                foreach (var notePermission in childnotePermissions)
                {
                    notePermission.IsDeleted = true;
                    notePermission.LastUpdatedBy = _userContext.UserId;
                    notePermission.LastUpdatedDate = DateTime.Now;
                    allPermision.Add(notePermission);
                }
            }            
            if (allPermision.Count > 0)
            {
                await UpdateBulkPermission(allPermision);
            }
        }
        public async Task DeleteParentPermissions(string noteId, string PermissionId)
        {
            var _documentBusiness = _sp.GetService<IDMSDocumentBusiness>();
            var parents = await _documentBusiness.GetAllParentByNoteId(noteId);
            var allPermision = new List<DocumentPermissionViewModel>();
            var ids = parents.Select(x => x.Id).ToList();
            ids.Add(noteId);
            var noteIds = string.Join(",", ids);
            var notePermissions = await _documentBusiness.GetAllNotePermissionByParentId(noteIds);            
            var childnotePermissions = notePermissions.ToList().Where(x => x.InheritedFrom == PermissionId && x.IsInheritedFromChild == true).ToList();
            foreach (var permission in childnotePermissions)
            {
                var notePermission = permission;
                notePermission.IsDeleted = true;
                notePermission.LastUpdatedBy = _userContext.UserId;
                notePermission.LastUpdatedDate = DateTime.Now;
                allPermision.Add(notePermission);
            }
            if (allPermision.Count > 0)
            {
                await UpdateBulkPermission(allPermision);
            }
        }
        //public async Task ManageParentPermissionsOnEdit(string noteId, string PermissionId)
        //{
        //    var _documentBusiness = _sp.GetService<IDMSDocumentBusiness>();

        //    List<FolderViewModel> subfoldersList = new List<FolderViewModel>();
        //    var subfolders = await _documentBusiness.GetAllParentByChildId(noteId, subfoldersList);

        //    foreach (var folder in subfolders)
        //    {
        //        // edit new permission for the parent 
        //        var childPermission = await GetSingleById(PermissionId);              
        //        var parentPermissions = await GetNotePermissionData(folder.Id);
        //       // if (folder.DisablePermissionInheritance == null || folder.DisablePermissionInheritance == false)
        //       // {
        //            foreach (var permission in parentPermissions.Where(x => x.InheritedFrom == PermissionId && x.IsInheritedFromChild==true))
        //            {
        //                var notePermission = await GetSingleById(permission.Id);
        //                notePermission.PermissionType = childPermission.PermissionType;
        //                notePermission.Access = DmsAccessEnum.ReadOnly;//parentPermission.Access;
        //                notePermission.AppliesTo = DmsAppliesToEnum.OnlyThisFolder;
        //                notePermission.PermittedUserId = childPermission.PermittedUserId;
        //                notePermission.PermittedUserGroupId = childPermission.PermittedUserGroupId;
        //                notePermission.IsInheritedFromChild = true;
        //                notePermission.InheritedFrom = childPermission.Id;
        //            await Edit(notePermission);
        //            }
        //        //var newPermissionId = parentPermissions.Where(x => x.InheritedFrom == PermissionId && x.IsInheritedFromChild == true).Select(x => x.Id).FirstOrDefault();
        //       // }
        //        await ManageParentPermissionsOnEdit(folder.Id, PermissionId);
        //    }
        //}
        public async Task DeleteOldParentPermission(string noteId)
        {
            var _documentBusiness = _sp.GetService<IDMSDocumentBusiness>();
            // get all inherited permission of folder and delete
            var notePermissions = await GetNotePermissionData(noteId);
            foreach (var permission in notePermissions)
            {
                if (permission.IsInherited == true)
                {
                    // Delete Permission
                    await Delete(permission.Id);
                }

            }
            // get all files and delete the inherited permissions
            //var documents = await _documentBusiness.GetAllFiles(noteId, null, null);
            //foreach (var doc in documents)
            //{
            //    var docPermissions = await GetNotePermissionData(doc.Id);                
            //    foreach (var permission in docPermissions)
            //    {
            //        if (permission.IsInherited == true)
            //        {
            //            await Delete(permission.Id);
            //            //_repository.RemoveRelationShip<NTS_NotePermission, R_NotePermission_Note, NTS_Note>(permission.Id, doc.Id, false);
            //            //_repository.Commit();
            //        }
            //    }
            //}
            //// get list of sub folders and delete inherited permission
            //List<FolderViewModel> subfoldersList = new List<FolderViewModel>();
            //var subfolders =await  _documentBusiness.GetAllChildByParentId(noteId, subfoldersList);
            ////var subfolders = _notebusiness.GetAllFolderByParentId(noteId, subfoldersList);
            //foreach (var folder in subfolders)
            //{
            //    await DeleteOldParentPermission(folder.Id);
            //}
        }
        //public async Task RemoveChildPermission(string noteId,string removedNoteId)
        //{
        //    var _documentBusiness = _sp.GetService<IDMSDocumentBusiness>();
        //    // get all  permission of removed child
        //    var childPermissions = await GetNotePermissionData(noteId);
        //    // get list of direct parent folders or workspace and delete inherited child permission
        //    List<FolderViewModel> subfoldersList = new List<FolderViewModel>();
        //    var subfolders = await _documentBusiness.GetAllParentByChildId(noteId, subfoldersList);           
        //    foreach (var folder in subfolders)
        //    {
        //        var notePermissions = await GetNotePermissionData(folder.Id);
        //        foreach (var permission in notePermissions)
        //        {
        //            // check if the permission inherited from child is present in parent or not
        //            if (permission.IsInheritedFromChild == true && childPermissions.Any(x=>x.Id== permission.InheritedFrom))
        //            {
        //                // Delete Permission
        //                await Delete(permission.Id);
        //            }
        //        }
        //        await RemoveChildPermission(folder.Id);
        //    }
        //}
        public async Task RemoveChildPermission(string noteId, string removedNoteId)
        {
            var _documentBusiness = _sp.GetService<IDMSDocumentBusiness>();
            // get all  permission of removed child
            var childPermissions = await GetNotePermissionData(noteId);
            var notePermissions = await GetNotePermissionData(removedNoteId);
            foreach (var permission in childPermissions)
            {
                // check if the permission inherited from child is present in parent or not
                if (permission.IsInheritedFromChild == true && notePermissions.Any(x => x.Id == permission.InheritedFrom))
                {
                    // Delete Permission
                    await Delete(permission.Id);
                }
            }
            // get list of direct parent folders or workspace and delete inherited child permission
            List<FolderViewModel> subfoldersList = new List<FolderViewModel>();
            var subfolders = await _documentBusiness.GetAllParentByChildId(noteId, subfoldersList);
            foreach (var folder in subfolders)
            {
                //var notePermissions = await GetNotePermissionData(folder.Id);
             
                await RemoveChildPermission(folder.Id, removedNoteId);
            }
        }
        public async Task<IList<DocumentPermissionViewModel>> GetParentPermissionData(string noteId)
        {
            IList<DocumentPermissionViewModel> PermissionList = new List<DocumentPermissionViewModel>();
            List<FolderViewModel> list = new List<FolderViewModel>();
            var parentLIst=await GetAllParents(noteId, list);
            //var cypher = string.Concat(@"match (n:NTS_Note {Id: {noteId}})-[:R_Note_Parent_Note*1..]->(p:NTS_Note) return n.Id as Id, n.Subject as Name,p.Id as ParentId,p.Subject as ParentName, p.DisablePermissionInheritance as DisablePermissionInheritance");
            // var prms = new Dictionary<string, object> { { "noteId", noteId } };
            //var parentLIst =await _queryRepo.ExecuteQueryList<FolderViewModel>(cypher, null);
            if (parentLIst != null && parentLIst.Count() > 0)
            {
                foreach (var parent in parentLIst)
                {
                  
                    IList<DocumentPermissionViewModel> ParentPermissionList = new List<DocumentPermissionViewModel>();
                    ParentPermissionList =await GetNotePermissionData(parent.ParentId);
                    if (ParentPermissionList != null && ParentPermissionList.Count() > 0)
                    {
                        foreach (var permission in ParentPermissionList)
                        {                            
                            PermissionList.Add(permission);
                        }
                    }
                    if (parent.DisablePermissionInheritance.HasValue && parent.DisablePermissionInheritance.Value == true)
                    {
                        break;
                    }
                }
            }
            return PermissionList;
        }
        public async Task DisableParentPermissions(string noteId, string InheritanceStatus)
        {
            var _noteBusiness = _sp.GetService<INoteBusiness>();
            var _documentBusiness = _sp.GetService<IDMSDocumentBusiness>();
            var allPermision = new List<DocumentPermissionViewModel>();
            var alldocPermision = new List<DocumentPermissionViewModel>();
            var Note = await _noteBusiness.GetSingleById(noteId);
            //direct level child documents
            var documents = await _documentBusiness.GetAllFiles(noteId, null, null);
            var docids = documents.Select(x => x.Id).ToList();
            docids.Add(noteId);
            var noteids = string.Join(",", docids);
            var notePermissions = await _documentBusiness.GetAllNotePermissionByParentId(noteids);
            if (InheritanceStatus.ToSafeBool() == false)
            { 
                if (Note.ParentNoteId.IsNotNull())
                {                    
                    var parentPermissions =await GetNotePermissionData(Note.ParentNoteId);                                      
                    // Create or Edit the Permission for the documents
                    var notePermissionscheck = notePermissions.Where(x => x.NoteId == noteId).ToList();
                    foreach (var permission in parentPermissions.Where(x=>x.IsInheritedFromChild==false))
                    {
                        if (permission.AppliesTo != DmsAppliesToEnum.OnlyThisFolder)
                        {                            
                            if (!notePermissionscheck.Any(e => e.PermissionType == permission.PermissionType && e.Principal == permission.Principal && e.Access == permission.Access && e.AppliesTo == permission.AppliesTo))
                            {
                                var existpermission = permission;
                                //var permissionData = new DocumentPermissionViewModel
                                //{

                                //    PermissionType = existpermission.PermissionType,
                                //    Access = existpermission.Access,
                                //    AppliesTo = existpermission.AppliesTo,
                                //    IsInherited = true,
                                //    PermittedUserId = existpermission.PermittedUserId,
                                //    NoteId = noteId,
                                //    InheritedFrom = existpermission.InheritedFrom == null ? existpermission.Id : existpermission.InheritedFrom,
                                //    Isowner = existpermission.Isowner,
                                //};
                                //await Create(permissionData);
                                var permissionData = new DocumentPermissionViewModel
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    PermissionType = existpermission.PermissionType,
                                    Access = existpermission.Access,
                                    AppliesTo = existpermission.AppliesTo,
                                    IsInherited = true,
                                    PermittedUserId = existpermission.PermittedUserId,
                                    PermittedUserGroupId = existpermission.PermittedUserGroupId,
                                    NoteId = noteId,
                                    InheritedFrom = existpermission.IsInherited == true ? existpermission.InheritedFrom : existpermission.Id,
                                    Isowner = existpermission.Isowner,
                                    DisablePermittedNotification = true,
                                    CompanyId = _userContext.CompanyId,
                                    IsDeleted = false,
                                    CreatedBy = _userContext.UserId,
                                    LastUpdatedBy = _userContext.UserId,
                                    CreatedDate = DateTime.Now,
                                    LastUpdatedDate = DateTime.Now,
                                    LegalEntityId = _userContext.LegalEntityId,
                                    PortalId = _userContext.PortalId,
                                    Status = StatusEnum.Active,
                                    VersionNo = 0,
                                };
                                allPermision.Add(permissionData);
                            }
                       
                        }
                    }
                    if (allPermision.Count > 0)
                    {
                        await CreateBulkPermission(allPermision);
                    }
                    // Create or Edit the Permission for direct level child documents
                    var parentPermissionsfordoc = await GetNotePermissionData(noteId);
                    foreach (var doc in documents)
                    {                        
                        var docPermissionscheck = notePermissions.Where(x => x.NoteId == doc.Id).ToList();
                        if (doc.DisablePermissionInheritance == null || doc.DisablePermissionInheritance == false)
                        {
                            foreach (var permission in parentPermissionsfordoc.Where(x => x.IsInheritedFromChild == false))
                            {
                                if (permission.AppliesTo != DmsAppliesToEnum.OnlyThisFolder)
                                {
                                    if (!docPermissionscheck.Any(e => e.PermissionType == permission.PermissionType && e.Principal == permission.Principal && e.Access == permission.Access && e.AppliesTo == permission.AppliesTo))
                                    {
                                        //var existpermission = await GetSingleById(permission.Id);
                                        var existpermission = permission;
                                        var permissionData = new DocumentPermissionViewModel
                                        {
                                            Id = Guid.NewGuid().ToString(),
                                            PermissionType = existpermission.PermissionType,
                                            Access = existpermission.Access,
                                            AppliesTo = existpermission.AppliesTo,
                                            IsInherited = true,
                                            PermittedUserId = existpermission.PermittedUserId,
                                            PermittedUserGroupId = existpermission.PermittedUserGroupId,
                                            NoteId = doc.Id,
                                            InheritedFrom = existpermission.IsInherited == true ? existpermission.InheritedFrom : existpermission.Id,
                                            Isowner = existpermission.Isowner,
                                            DisablePermittedNotification = true,
                                            CompanyId = _userContext.CompanyId,
                                            IsDeleted = false,
                                            CreatedBy = _userContext.UserId,
                                            LastUpdatedBy = _userContext.UserId,
                                            CreatedDate = DateTime.Now,
                                            LastUpdatedDate = DateTime.Now,
                                            LegalEntityId = _userContext.LegalEntityId,
                                            PortalId = _userContext.PortalId,
                                            Status = StatusEnum.Active,
                                            VersionNo = 0,
                                        };
                                        alldocPermision.Add(permissionData);
                                    }
                                }
                            }
                        }
                    }
                    if (alldocPermision.Count>0)
                    {
                        await CreateBulkPermission(alldocPermision);
                    }
                    Note.DisablePermissionInheritance = false;
                    Note.LastUpdatedBy = _userContext.UserId;
                    Note.LastUpdatedDate = DateTime.Now;
                    await _noteBusiness.Edit(Note);
                }
                else
                {
                    Note.DisablePermissionInheritance = false;
                    Note.LastUpdatedBy = _userContext.UserId;
                    Note.LastUpdatedDate = DateTime.Now;
                    await _noteBusiness.Edit(Note);
                }
            }
            else
            {               
                var inheritedPermissions = notePermissions.Where(x => x.IsInherited == true).ToList();
                foreach (var permission in inheritedPermissions)
                {
                    permission.IsDeleted = true;
                    permission.LastUpdatedBy = _userContext.UserId;
                    permission.LastUpdatedDate = DateTime.Now;
                    allPermision.Add(permission);
                }
                if (allPermision.Count > 0)
                {
                    await UpdateBulkPermission(allPermision);
                }
                Note.DisablePermissionInheritance = true;
                Note.LastUpdatedBy = _userContext.UserId;
                Note.LastUpdatedDate = DateTime.Now;
                await _noteBusiness.Edit(Note);
              
            }

        }
        public async Task<IList<FolderViewModel>> GetAllParents(string noteId, List<FolderViewModel> parentList)
        {
            var cypher = string.Concat($@"select n.""Id"" as Id, n.""NoteSubject"" as Name,p.""Id"" as ParentId,p.""NoteSubject"" as ParentName, p.""DisablePermissionInheritance"" as DisablePermissionInheritance
from public.""NtsNote"" as n 
join public.""NtsNote"" as p on p.""Id""=n.""ParentNoteId""  and p.""IsDeleted""=false and p.""CompanyId"" = '{_repo.UserContext.CompanyId}'
where n.""Id""='{noteId}' and n.""IsDeleted""=false and n.""CompanyId"" = '{_repo.UserContext.CompanyId}'

");
           
            var list =await _queryRepo.ExecuteQuerySingle<FolderViewModel>(cypher, null);
            if (list!=null && list.ParentId!=null) 
            {
                parentList.Add(list);
                await GetAllParents(list.ParentId, parentList);
            }
            return parentList;
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
            

            var resuly = await _querycalndarDocument.ExecuteQueryList<DMSCalenderViewModel>(query, null);
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


            var result = await _QueryShareLink.ExecuteQueryList<NoteLinkShareViewModel>(query, null);

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


            var result = await _QueryShareLink.ExecuteQuerySingle<NoteLinkShareViewModel>(query, null);

            return result;
        }

        public async Task<DMSDocumentViewModel> GetFileId(string Id)
        {
             var selectQry = "";
             var i = 1;
            
             var tempCategory = await _templateCategoryBusiness.GetList(x => x.Code == "GENERAL_DOCUMENT"  && x.TemplateType==TemplateTypeEnum.Note);
            
             foreach (var item1 in tempCategory)
             {
                 var templateList = await _templateBusiness.GetList(x => x.TemplateCategoryId == item1.Id);
            
            
                 foreach (var item in templateList.Where(x => x.TableMetadataId != null))
                 {
                     var tableMeta = await _tableMetadataBusiness.GetSingle(x => x.Id == item.TableMetadataId);
                     if (item.Code == "GENERAL_DOCUMENT" || item.Code== "ENGINEERING_SUBCONTRACT")
            
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

            var result = await _queryDMSDocument.ExecuteQuerySingle(selectQry, null);

            return result;
        }
        

        
        public async Task<bool> DeleteLink(string Id)
        {

            var Query = $@"update cms.""N_DMS_DocumentShareLink"" set ""IsDeleted""=true where ""NtsNoteId""='{Id}'";

            await _queryDMSDocument.ExecuteCommand(Query, null);

            return true;

        }
        public async Task<WorkspaceViewModel> GetLegalEntity(string parentId)
        {
            var query = "";

            query = @$"select w.""LegalEntityId"" as ""LegalEntityId""

                                from cms.""N_GENERAL_FOLDER_WORKSPACE"" as w
                       Join public.""NtsNote"" as n on n.""Id""=w.""NtsNoteId"" and n.""IsDeleted""=false
						          where n.""Id""='{parentId}'and w.""IsDeleted"" = false and w.""CompanyId"" = '{_repo.UserContext.CompanyId}'";
            var queryData = await _queryworkspace.ExecuteQuerySingle(query, null);
            var list = queryData;
            return list;
        }
        public async Task<bool> DeletePermissionByDocumentIds(string ids)
        {
            var cypher = $@" update public.""DocumentPermission"" set ""IsDeleted""=true
            where ""Id"" in ({ids}) ";
            var result = await _queryRepo.ExecuteQuerySingle(cypher, null);
            return true;
        }
    }
    }
