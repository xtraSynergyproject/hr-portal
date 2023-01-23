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
    public class UdfPermissionBusiness : BusinessBase<UdfPermissionViewModel, UdfPermission>, IUdfPermissionBusiness
    {


        public UdfPermissionBusiness(IRepositoryBase<UdfPermissionViewModel, UdfPermission> repo, IMapper autoMapper) : base(repo, autoMapper)
        {


        }

        public async override Task<CommandResult<UdfPermissionViewModel>> Create(UdfPermissionViewModel model, bool autoCommit = true)
        {

            if (model.ViewableBy != null)
            {
                var viewableBy = new List<string>();
                int vb = 0;
                foreach (var item in model.ViewableBy)
                {
                    viewableBy.Add(item);
                    //if (int.TryParse(item, out vb))
                    //{
                    //    viewableBy.Add(Convert.ToString(((NtsActiveUserTypeEnum)vb)));
                    //}
                }
                model.ViewableBy = viewableBy.ToArray();
            }
            if (model.EditableBy != null)
            {
                var editableBy = new List<string>();
                int vb = 0;
                foreach (var item in model.EditableBy)
                {
                    editableBy.Add(item);
                    //if (int.TryParse(item, out vb))
                    //{
                    //    editableBy.Add(Convert.ToString(((NtsActiveUserTypeEnum)vb)));
                    //}
                }
                model.EditableBy = editableBy.ToArray();
            }
            var result = await base.Create(model,autoCommit);

            return CommandResult<UdfPermissionViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<UdfPermissionViewModel>> Edit(UdfPermissionViewModel model, bool autoCommit = true)
        {
            if (model.ViewableBy != null)
            {
                var viewableBy = new List<string>();
                //int vb = 0;
                foreach (var item in model.ViewableBy)
                {
                    viewableBy.Add(item);
                    //if (int.TryParse(item, out vb))
                    //{
                    //    viewableBy.Add(Convert.ToString(((NtsActiveUserTypeEnum)vb)));
                    //}
                }
                model.ViewableBy = viewableBy.ToArray();
            }
            if (model.EditableBy != null)
            {
                var editableBy = new List<string>();
               // int vb = 0;
                foreach (var item in model.EditableBy)
                {
                    editableBy.Add(item);
                    //if (int.TryParse(item, out vb))
                    //{
                    //    editableBy.Add(Convert.ToString(((NtsActiveUserTypeEnum)vb)));
                    //}
                }
                model.EditableBy = editableBy.ToArray();
            }

            var result = await base.Edit(model,autoCommit);

            return CommandResult<UdfPermissionViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }



    }
}
