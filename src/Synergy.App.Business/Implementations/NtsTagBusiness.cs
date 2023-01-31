using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
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


namespace Synergy.App.Business
{
    public class NtsTagBusiness : BusinessBase<NtsTagViewModel, NtsTag>, INtsTagBusiness
    {
        private readonly IRepositoryQueryBase<NtsTagViewModel> _queryRepo;
        private readonly INoteBusiness _noteBusiness;
        private readonly IUserContext _userContext;
        private readonly ICmsQueryBusiness _cmsQueryBusiness;
        public NtsTagBusiness(IRepositoryBase<NtsTagViewModel, NtsTag> repo,IMapper autoMapper,
            IRepositoryQueryBase<NtsTagViewModel> queryRepo,
            INoteBusiness noteBusiness, IUserContext userContext, ICmsQueryBusiness cmsQueryBusiness) : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;
            _noteBusiness = noteBusiness;
            _userContext = userContext;
            _cmsQueryBusiness = cmsQueryBusiness;
        }

      

        public async override Task<CommandResult<NtsTagViewModel>> Create(NtsTagViewModel model, bool autoCommit = true)
        {
          
            var result = await base.Create(model,autoCommit);
           
            return CommandResult<NtsTagViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<NtsTagViewModel>> Edit(NtsTagViewModel model, bool autoCommit = true)
        {
            var result = await base.Edit(model,autoCommit);
            
            return CommandResult<NtsTagViewModel>.Instance(model);
        }
        public async Task<List<TagCategoryViewModel>> GetNtsTagData(NtsTypeEnum ntstype,string ntsId)
        {
            var querydata = await _cmsQueryBusiness.GetNtsTagData(ntstype, ntsId);
            return querydata;
        }

        public async Task<TagCategoryViewModel> GetTagCategoryDataById(string categoryId)
        {
            var querydata = await _cmsQueryBusiness.GetTagCategoryDataByIdData(categoryId);
            return querydata;
        }
        public async Task<TagCategoryViewModel> GetTagCategoryDataByNoteId(string NoteId)
        {
            var querydata = await _cmsQueryBusiness.GetTagCategoryDataByNoteIdData(NoteId);
            return querydata;
        }
        public async Task<TagViewModel> GetTagByNoteId(string NoteId)
        {
            var querydata = await _cmsQueryBusiness.GetTagByNoteIdData(NoteId);
            return querydata;
        }
        public async Task<List<TagViewModel>> TagListByCategoryNoteId(string NoteId)
        {
            var querydata = await _cmsQueryBusiness.TagListByCategoryNoteIdData(NoteId);
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
