using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace Synergy.App.Business
{
    public class HierarchyMasterBusiness : BusinessBase<HierarchyMasterViewModel, HierarchyMaster>, IHierarchyMasterBusiness
    {
        // INoteBusiness _noteBusiness;
        private readonly IServiceProvider _serviceProvider;
       // ITableMetadataBusiness _tableMetadataBusiness;
        public HierarchyMasterBusiness(IRepositoryBase<HierarchyMasterViewModel, HierarchyMaster> repo, IMapper autoMapper,
            //INoteBusiness noteBusiness, 
            IServiceProvider serviceProvider
          //  ITableMetadataBusiness tableMetadataBusiness
          ) : base(repo, autoMapper)
        {
            //_noteBusiness = noteBusiness;
            _serviceProvider = serviceProvider;
           // _tableMetadataBusiness = tableMetadataBusiness;
        }

        public async override Task<CommandResult<HierarchyMasterViewModel>> Create(HierarchyMasterViewModel model, bool autoCommit = true)
        {

         
            var validateName = await IsNameExists(model);
            if (!validateName.IsSuccess)
            {
                return CommandResult<HierarchyMasterViewModel>.Instance(model, false, validateName.Messages);
            }
            var result = await base.Create(model,autoCommit);
            if (result.IsSuccess)
            {
                if ( model.RootNodeId.IsNotNullAndNotEmpty() && model.HierarchyType == HierarchyTypeEnum.Position)
                {
                    await CreatePositionHierarchy(DataActionEnum.Create, result.Item.Id, model.RootNodeId);
                }
                else if (model.RootNodeId.IsNotNullAndNotEmpty() && model.HierarchyType == HierarchyTypeEnum.Organization)
                {
                    await CreateOrganisationHierarchy(DataActionEnum.Create, result.Item.Id, model.RootNodeId);
                }
                else if (model.RootNodeId.IsNotNullAndNotEmpty() && model.HierarchyType == HierarchyTypeEnum.User)
                {
                    await CreateUserHierarchy(DataActionEnum.Create, result.Item.Id, model.RootNodeId);
                }
            }
            return CommandResult<HierarchyMasterViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

       

        public async override Task<CommandResult<HierarchyMasterViewModel>> Edit(HierarchyMasterViewModel model, bool autoCommit = true)
        {

          
            var validateName = await IsNameExists(model);
            if (!validateName.IsSuccess)
            {
                return CommandResult<HierarchyMasterViewModel>.Instance(model, false, validateName.Messages);
            }
            var exisitng = await GetSingleById(model.Id);
            var result = await base.Edit(model,autoCommit);
            if (result.IsSuccess)
            {
                if (model.HierarchyType == HierarchyTypeEnum.Position)
                {
                    if (exisitng.RootNodeId.IsNullOrEmpty()&& model.RootNodeId.IsNotNullAndNotEmpty())
                    {
                        await CreatePositionHierarchy(DataActionEnum.Create, result.Item.Id, model.RootNodeId);
                    }
                    else if(exisitng.RootNodeId.IsNotNullAndNotEmpty()&&model.RootNodeId.IsNotNullAndNotEmpty()&& exisitng.RootNodeId != model.RootNodeId)
                    {
                        await CreatePositionHierarchy(DataActionEnum.Edit, result.Item.Id, model.RootNodeId, exisitng.RootNodeId);
                    }
                   
                }
                else if (model.HierarchyType == HierarchyTypeEnum.Organization)
                {
                    if (exisitng.RootNodeId.IsNullOrEmpty() && model.RootNodeId.IsNotNullAndNotEmpty())
                    {
                        await CreateOrganisationHierarchy(DataActionEnum.Create, result.Item.Id, model.RootNodeId);
                    }
                    else if (exisitng.RootNodeId.IsNotNullAndNotEmpty() && model.RootNodeId.IsNotNullAndNotEmpty() && exisitng.RootNodeId != model.RootNodeId)
                    {
                        await CreateOrganisationHierarchy(DataActionEnum.Edit, result.Item.Id, model.RootNodeId, exisitng.RootNodeId);
                    }

                }
                else if (model.HierarchyType == HierarchyTypeEnum.User)
                {
                    if (exisitng.RootNodeId.IsNullOrEmpty() && model.RootNodeId.IsNotNullAndNotEmpty())
                    {
                        await CreateUserHierarchy(DataActionEnum.Create, result.Item.Id, model.RootNodeId);
                    }
                    else if (exisitng.RootNodeId.IsNotNullAndNotEmpty() && model.RootNodeId.IsNotNullAndNotEmpty() && exisitng.RootNodeId != model.RootNodeId)
                    {
                        await CreateUserHierarchy(DataActionEnum.Edit, result.Item.Id, model.RootNodeId, exisitng.RootNodeId);
                    }

                }
            }
            return CommandResult<HierarchyMasterViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        private async Task<CommandResult<HierarchyMasterViewModel>> IsNameExists(HierarchyMasterViewModel model)
        {

            var errorList = new Dictionary<string, string>();

            if (model.Name != null || model.Name != "")
            {
                var name = await _repo.GetSingle(x => x.Name == model.Name && x.Id != model.Id);
                if (name != null)
                {
                    errorList.Add("Name", "Name already exist.");
                }
            }
            if (errorList.Count > 0)
            {
                return CommandResult<HierarchyMasterViewModel>.Instance(model, false, errorList);
            }

            return CommandResult<HierarchyMasterViewModel>.Instance();
        }
        public async Task CreatePositionHierarchy(DataActionEnum DataAction, string hierarchyId, string rootNodeId,string existingRootNodeId=null)
        {
            var _noteBusiness = _serviceProvider.GetService<INoteBusiness>();
            if (DataAction == DataActionEnum.Create)
            {

                var noteTemp = new NoteTemplateViewModel();
                noteTemp.TemplateCode = "PositionHierarchy";
                var note = await _noteBusiness.GetNoteDetails(noteTemp);

                note.OwnerUserId = _repo.UserContext.UserId;
                note.StartDate = DateTime.Now;
                note.Json = "{}";
                note.DataAction = DataActionEnum.Create;

                //var list = new List<System.Dynamic.ExpandoObject>();
                dynamic exo = new System.Dynamic.ExpandoObject();
                ((IDictionary<String, Object>)exo).Add("PositionId", rootNodeId);
                ((IDictionary<String, Object>)exo).Add("HierarchyId", hierarchyId);

                note.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                note.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                var res = await _noteBusiness.ManageNote(note);
            }
            else if (DataAction == DataActionEnum.Edit)
            {
                var _hrBusiness = _serviceProvider.GetService<IHRCoreBusiness>();
                var existingposhierarchy = await _hrBusiness.GetPositionByHierarchyAndPosition(existingRootNodeId,hierarchyId);
                if (existingposhierarchy.IsNotNull())
                {
                    var poshierarchyId = existingposhierarchy.Id;
                    var noteTemp = new NoteTemplateViewModel();
                    noteTemp.TemplateCode = "PositionHierarchy";
                    noteTemp.NoteId = poshierarchyId;
                    var note = await _noteBusiness.GetNoteDetails(noteTemp);

                    note.OwnerUserId = _repo.UserContext.UserId;
                    note.StartDate = DateTime.Now;
                    note.Json = "{}";
                    note.DataAction = DataActionEnum.Edit;
                    dynamic exo = new System.Dynamic.ExpandoObject();
                    ((IDictionary<String, Object>)exo).Add("PositionId", rootNodeId);
                    ((IDictionary<String, Object>)exo).Add("HierarchyId", hierarchyId);
                    note.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                    note.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                    var res = await _noteBusiness.ManageNote(note);
                }

            }


        }

        public async Task CreateOrganisationHierarchy(DataActionEnum DataAction, string hierarchyId, string rootNodeId, string existingRootNodeId = null)
        {
            var _noteBusiness = _serviceProvider.GetService<INoteBusiness>();
            if (DataAction == DataActionEnum.Create)
            {

                var noteTemp = new NoteTemplateViewModel();
                noteTemp.TemplateCode = "HRDepartmentHierarchy";
                var note = await _noteBusiness.GetNoteDetails(noteTemp);

                note.OwnerUserId = _repo.UserContext.UserId;
                note.StartDate = DateTime.Now;
                note.Json = "{}";
                note.DataAction = DataActionEnum.Create;

                //var list = new List<System.Dynamic.ExpandoObject>();
                dynamic exo = new System.Dynamic.ExpandoObject();
                ((IDictionary<String, Object>)exo).Add("DepartmentId", rootNodeId);
                ((IDictionary<String, Object>)exo).Add("HierarchyId", hierarchyId);

                note.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                note.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                var res = await _noteBusiness.ManageNote(note);
            }
            else if (DataAction == DataActionEnum.Edit)
            {
                var _hrBusiness = _serviceProvider.GetService<IHRCoreBusiness>();
                var existingposhierarchy = await _hrBusiness.GetOrgByHierarchyAndOrg(existingRootNodeId, hierarchyId);
                if (existingposhierarchy.IsNotNull())
                {
                    var poshierarchyId = existingposhierarchy.Id;
                    var noteTemp = new NoteTemplateViewModel();
                    noteTemp.TemplateCode = "HRDepartmentHierarchy";
                    noteTemp.NoteId = poshierarchyId;
                    var note = await _noteBusiness.GetNoteDetails(noteTemp);

                    note.OwnerUserId = _repo.UserContext.UserId;
                    note.StartDate = DateTime.Now;
                    note.Json = "{}";
                    note.DataAction = DataActionEnum.Edit;
                    dynamic exo = new System.Dynamic.ExpandoObject();
                    ((IDictionary<String, Object>)exo).Add("DepartmentId", rootNodeId);
                    ((IDictionary<String, Object>)exo).Add("HierarchyId", hierarchyId);
                    note.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                    note.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                    var res = await _noteBusiness.ManageNote(note);
                }

            }


        }

        public async Task CreateUserHierarchy(DataActionEnum DataAction, string hierarchyId, string rootNodeId, string existingRootNodeId = null)
        {
            var _noteBusiness = _serviceProvider.GetService<INoteBusiness>();
            if (DataAction == DataActionEnum.Create)
            {

                var noteTemp = new NoteTemplateViewModel();
                noteTemp.TemplateCode = "USER_HIERARCHY";
                var note = await _noteBusiness.GetNoteDetails(noteTemp);

                note.OwnerUserId = _repo.UserContext.UserId;
                note.StartDate = DateTime.Now;
                note.Json = "{}";
                note.DataAction = DataActionEnum.Create;

                //var list = new List<System.Dynamic.ExpandoObject>();
                dynamic exo = new System.Dynamic.ExpandoObject();
                ((IDictionary<String, Object>)exo).Add("UserId", rootNodeId);
                ((IDictionary<String, Object>)exo).Add("HierarchyId", hierarchyId);

                note.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                note.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                var res = await _noteBusiness.ManageNote(note);
            }
            else if (DataAction == DataActionEnum.Edit)
            {
                var _hrBusiness = _serviceProvider.GetService<IHRCoreBusiness>();
                var existingposhierarchy = await _hrBusiness.GetUserByHierarchyAndUser(existingRootNodeId, hierarchyId);
                if (existingposhierarchy != null)
                {
                    var poshierarchyId = existingposhierarchy.Id;
                    var noteTemp = new NoteTemplateViewModel();
                    noteTemp.TemplateCode = "USER_HIERARCHY";
                    noteTemp.NoteId = poshierarchyId;
                    var note = await _noteBusiness.GetNoteDetails(noteTemp);

                    note.OwnerUserId = _repo.UserContext.UserId;
                    note.StartDate = DateTime.Now;
                    note.Json = "{}";
                    note.DataAction = DataActionEnum.Edit;
                    dynamic exo = new System.Dynamic.ExpandoObject();
                    ((IDictionary<String, Object>)exo).Add("UserId", rootNodeId);
                    ((IDictionary<String, Object>)exo).Add("HierarchyId", hierarchyId);
                    note.Json = Newtonsoft.Json.JsonConvert.SerializeObject(exo);
                    note.NoteStatusCode = "NOTE_STATUS_INPROGRESS";
                    var res = await _noteBusiness.ManageNote(note);
                }

            }


        }


        public async Task<IList<IdNameViewModel>> GetHierarchyMasterLevelById(string Id)
        {

            var obj = await GetSingleById(Id);
            var list = new List<IdNameViewModel>(){
             new IdNameViewModel()
            {
                Name = obj.Level1Name,
                Id="1"
            },
              new IdNameViewModel()
            {
                Name = obj.Level2Name,
                Id="2"
            },
               new IdNameViewModel()
            {
                Name = obj.Level3Name,
                Id="3"
            },
                new IdNameViewModel()
            {
                Name = obj.Level4Name,
                Id="4"
            },
                 new IdNameViewModel()
            {
                Name = obj.Level5Name,
                Id="5"
            }

            };


            return list;
        }

    }
}
