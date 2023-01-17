using AutoMapper;
using CMS.Common;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace CMS.Business
{
    public class EditorTypeBusiness : BusinessBase<EditorTypeViewModel, EditorType>, IEditorTypeBusiness
    {
        public EditorTypeBusiness(IRepositoryBase<EditorTypeViewModel, EditorType> repo, IMapper autoMapper) : base(repo, autoMapper)
        {

        }

        public async override Task<CommandResult<EditorTypeViewModel>> Create(EditorTypeViewModel model)
        {
            if (model.Name.IsNullOrEmpty())
            {
                return CommandResult<EditorTypeViewModel>.Instance(model, x => x.Name, "Name is required.");
            }
            else
            {
                //var name = typeof(EditorType).Name;
                //var filter = Builders<EditorTypeViewModel>.Filter.Regex("Name", new BsonRegularExpression(model.Name, "i"));
                //var editordata = await _repo.Mongo.GetCollection<EditorTypeViewModel>(name).Find(filter).FirstOrDefaultAsync();
                //if (editordata != null && editordata.Name.ToLower() == model.Name.ToLower())
                //{
                //    return CommandResult<EditorTypeViewModel>.Instance(model, x => x.Name, "Name already exist");
                //}
            }
             
            if (!model.ControlType.ToString().IsNullOrEmpty())
            {
                var editordata = await _repo.GetListGlobal();
                foreach (var editor in editordata)
                {
                    if(editor.ControlType ==model.ControlType)
                    {
                        return CommandResult<EditorTypeViewModel>.Instance(model, x => x.Name, "Control type already used");
                    }
                }
            }
            var data = _autoMapper.Map<EditorTypeViewModel>(model);
            var result = await base.Create(data);
            //var result = await base.CreateGlobal(data);
            //return result;
            return CommandResult<EditorTypeViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<EditorTypeViewModel>> Edit(EditorTypeViewModel model)
        {
            if (model.Name.IsNullOrEmpty())
            {
                return CommandResult<EditorTypeViewModel>.Instance(model, x => x.Name, "Name is required.");
            }
            else
            {
                //var name = typeof(EditorType).Name;
                //var filter = Builders<EditorTypeViewModel>.Filter.Regex("Name", new BsonRegularExpression(model.Name, "i"));
                //var editordata = await _repo.Mongo.GetCollection<EditorTypeViewModel>(name).Find(filter).FirstOrDefaultAsync();
                //if (editordata != null && editordata.Id.ToLower() != model.Id.ToLower() && editordata.Name.ToLower() == model.Name.ToLower())
                //{
                //    return CommandResult<EditorTypeViewModel>.Instance(model, x => x.Name, "Name already exist");
                //}
            }
            if (!model.ControlType.ToString().IsNullOrEmpty())
            {
                var editordata = await _repo.GetListGlobal();
                foreach (var editor in editordata)
                {
                    if (editor.ControlType == model.ControlType && editor.Id != model.Id)
                    {
                        return CommandResult<EditorTypeViewModel>.Instance(model, x => x.Name, "Control type already used");
                    }
                }
            }
            var data = _autoMapper.Map<EditorTypeViewModel>(model);
            var result = await base.Edit(data);
            //var result = await base.EditGlobal(data);
            //return result;
            return CommandResult<EditorTypeViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
    }
}
