using AutoMapper;
using CMS.Common;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace CMS.Business
{
    public class NtsTagBusiness : BusinessBase<NtsTagViewModel, NtsTag>, INtsTagBusiness
    {
        private readonly IRepositoryQueryBase<NtsTagViewModel> _queryRepo;
        private readonly INoteBusiness _noteBusiness;
        private readonly IUserContext _userContext;
        public NtsTagBusiness(IRepositoryBase<NtsTagViewModel, NtsTag> repo,IMapper autoMapper,
            IRepositoryQueryBase<NtsTagViewModel> queryRepo,
            INoteBusiness noteBusiness, IUserContext userContext) : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;
            _noteBusiness = noteBusiness;
            _userContext = userContext;
        }

      

        public async override Task<CommandResult<NtsTagViewModel>> Create(NtsTagViewModel model)
        {
          
            var result = await base.Create(model);
           
            return CommandResult<NtsTagViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<NtsTagViewModel>> Edit(NtsTagViewModel model)
        {
            var result = await base.Edit(model);
            
            return CommandResult<NtsTagViewModel>.Instance(model);
        }
        public async Task<List<TagCategoryViewModel>> GetNtsTagData(NtsTypeEnum ntstype,string ntsId)
        {
            var query = @$"  select tc.""TagCategoryName"" as Name,nts.""Id"" as Id,n.""NoteSubject"" as TagName,
nts.""CreatedDate"" as CreatedDate,u.""Name"" as CreatedByName,nts.""TagId"" as TagId,p.""Id"" as ParentNoteId,
nts.""LastUpdatedDate"" as LastUpdatedDate,lu.""Name"" as LastUpdatedByName
from public.""NtsTag"" as nts
join public.""User"" as u on u.""Id""=nts.""CreatedBy"" and nts.""IsDeleted""=false and u.""IsDeleted""=false and u.""CompanyId""='{_repo.UserContext.CompanyId}' and nts.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""User"" as lu on lu.""Id""=nts.""LastUpdatedBy"" and lu.""IsDeleted""=false and lu.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""NtsNote"" as p on p.""Id"" = nts.""TagCategoryId"" and p.""IsDeleted""=false and p.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_General_TagCategory"" as tc on tc.""NtsNoteId""=p.""Id"" and tc.""IsDeleted""=false  and tc.""CompanyId""='{_repo.UserContext.CompanyId}'
join public.""NtsNote"" as n on n.""Id"" = nts.""TagId"" and n.""IsDeleted""=false and n.""CompanyId""='{_repo.UserContext.CompanyId}'
join cms.""N_General_Tag"" as t on t.""NtsNoteId""=n.""Id""  and t.""IsDeleted""=false and t.""CompanyId""='{_repo.UserContext.CompanyId}'
where nts.""NtsType""='{(int)((NtsTypeEnum)Enum.Parse(typeof(NtsTypeEnum), ntstype.ToString()))}' and nts.""NtsId""='{ntsId}' and nts.""IsDeleted""=false ";
            var querydata = await _queryRepo.ExecuteQueryList<TagCategoryViewModel>(query, null);
            return querydata;
        }

        public async Task<TagCategoryViewModel> GetTagCategoryDataById(string categoryId)
        {
            var query = @$"  select *,""NtsNoteId"" as NoteId from  cms.""N_General_TagCategory"" where ""NtsNoteId""='{categoryId}' and ""IsDeleted""=false";
            var querydata = await _queryRepo.ExecuteQuerySingle<TagCategoryViewModel>(query, null);
            return querydata;
        }
        public async Task<TagCategoryViewModel> GetTagCategoryDataByNoteId(string NoteId)
        {
            var query = @$"  select *,""NtsNoteId"" as NoteId from  cms.""N_General_TagCategory"" where ""NtsNoteId""='{NoteId}' and ""IsDeleted""=false";
            var querydata = await _queryRepo.ExecuteQuerySingle<TagCategoryViewModel>(query, null);
            return querydata;
        }
        public async Task<TagViewModel> GetTagByNoteId(string NoteId)
        {
            var query = @$"  select *,""NtsNoteId"" as NoteId from  cms.""N_General_Tag"" where ""NtsNoteId""='{NoteId}' and ""IsDeleted""=false and ""CompanyId""='{_repo.UserContext.CompanyId}' ";
            var querydata = await _queryRepo.ExecuteQuerySingle<TagViewModel>(query, null);
            return querydata;
        }
        public async Task<List<TagViewModel>> TagListByCategoryNoteId(string NoteId)
        {
            var query = @$"  select tag.*,c.""NoteSubject"" as NoteSubject,tag.""NtsNoteId"" as NoteId,tc.""NtsNoteId"" as ParentNoteId from 
public.""NtsNote"" as p join 
cms.""N_General_TagCategory"" as tc on tc.""NtsNoteId""=p.""Id"" and p.""Id""='{NoteId}' and tc.""IsDeleted""=false
join public.""NtsNote"" as c on c.""ParentNoteId""= p.""Id""
join cms.""N_General_Tag"" as tag on tag.""NtsNoteId""=c.""Id""
and tag.""IsDeleted""=false";
            var querydata = await _queryRepo.ExecuteQueryList<TagViewModel>(query, null);
            return querydata;
        }
        public async Task GenerateTagsForCategory(string categoryId)
        {
            var model = await GetTagCategoryDataById(categoryId);
            if (model.IsNotNull())
            {                
                var querydata = await _queryRepo.ExecuteQueryList<IdNameViewModel>(model.TextQueryCode, null);
                if (querydata!=null && querydata.Count()>0) 
                {
                    foreach (var data in querydata) 
                    {
                        var tagList = await TagListByCategoryNoteId(model.NoteId);
                        if (tagList.Any(x => x.TagSourceReferenceId == data.Id && x.ParentNoteId ==model.NoteId))
                        {
                            foreach (var rec in tagList.Where(x => x.TagSourceReferenceId == data.Id && x.ParentNoteId == model.NoteId)) 
                            {
                                if (!rec.NoteSubject.Equals(data.Name,StringComparison.InvariantCultureIgnoreCase))
                                {
                                    var noteTempModel = new NoteTemplateViewModel();
                                    noteTempModel.DataAction = DataActionEnum.Edit;
                                    noteTempModel.ParentNoteId = model.NoteId;
                                    noteTempModel.ActiveUserId = model.LastUpdatedBy;
                                    noteTempModel.NoteId = rec.NoteId;
                                    var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                                    notemodel.NoteSubject = data.Name;
                                    var tag=await GetTagByNoteId(rec.NoteId);
                                    tag.TagSourceReferenceId = data.Id;
                                    notemodel.Json = JsonConvert.SerializeObject(tag);
                                    var result = await _noteBusiness.ManageNote(notemodel);
                                }   
                            }
                            
                        }
                        else 
                        {
                            var noteTempModel = new NoteTemplateViewModel();
                            noteTempModel.DataAction =DataActionEnum.Create;
                            noteTempModel.ActiveUserId = model.LastUpdatedBy;                           
                            noteTempModel.TemplateCode = "TAG";
                            noteTempModel.ParentNoteId = model.NoteId;
                            var notemodel = await _noteBusiness.GetNoteDetails(noteTempModel);
                            notemodel.NoteSubject = data.Name;
                            TagViewModel tagModel = new TagViewModel();
                            tagModel.TagSourceReferenceId = data.Id;
                            notemodel.Json = JsonConvert.SerializeObject(tagModel);
                            notemodel.NoteStatusCode = "NOTE_STATUS_COMPLETE";
                            var result = await _noteBusiness.ManageNote(notemodel);
                        }
                       
                    }
                   
                }
            }

        }


    }
}
