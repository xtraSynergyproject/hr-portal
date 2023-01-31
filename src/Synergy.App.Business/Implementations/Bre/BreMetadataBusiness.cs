using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Synergy.App.Business
{
    public class BreMetadataBusiness : BusinessBase<BreMetadataViewModel, BreMasterTableMetadata>, IBreMetadataBusiness
    {
        //public List<BreMetadataViewMo>
        public BreMetadataBusiness(IRepositoryBase<BreMetadataViewModel, BreMasterTableMetadata> repo, IMapper autoMapper) : base(repo, autoMapper)
        {

        }


        public async Task<List<BreMetadataViewModel>> GetBreMetaData(string bussinessRuleId,string parentId)
        {
            var list = new List<BreMetadataViewModel>();
            var data = await GetList(x=>x.BusinessRuleId == bussinessRuleId);
            if (parentId.IsNullOrEmpty())
            {
                var childList = await GetSingle<BreMetadataViewModel, BreMasterTableMetadata>(x => x.ParentId == bussinessRuleId);
                list.Add(new BreMetadataViewModel
                {
                    Expanded = true,
                    Id = bussinessRuleId,
                    Name = "Input Data",
                    ParentId = null,
                    CompanyId = _repo.UserContext.CompanyId,
                    BreInputDataType = BreInputDataTypeEnum.Root,
                    HasSubFolders = childList != null,
                    DataType = DataTypeEnum.Object,
                });
                return list;
            }
            else 
            {
                var newList  = data.Where(x => x.ParentId == parentId).ToList();
                list.AddRange(newList.Select(x => new BreMetadataViewModel
                    {
                        Expanded = true,
                        Id = x.Id,
                        CompanyId = x.CompanyId,
                        Name = x.Name,
                        ParentId = x.ParentId,
                        BreInputDataType = x.BreInputDataType,
                        DataType = x.DataType,
                    }));
                    list.ForEach(x => x.HasSubFolders = data.Any(y => y.ParentId == x.Id));
                   
            }
               
            //data = data.Where(x => x.BreMetadataType == BreMetadataTypeEnum.InputData && x.BusinessRuleId == bussinessRuleId).ToList();
            //list.AddRange(data.Select(x => new BreMetadataViewModel
            //{
            //    Expanded = true,
            //    Id = x.Id,
            //    CompanyId = x.CompanyId,
            //    Name = x.Name,
            //    ParentId = x.ParentId,
            //    BreInputDataType = x.BreInputDataType,
            //    BreMetadataType = x.BreMetadataType,
            //    DataType = x.DataType,
            //}));
            //list.ForEach(x => x.HasSubFolders = list.Any(y => y.ParentId == x.Id));
            //list.Add(new BreMetadataViewModel
            //{
            //    Expanded = true,
            //    Id = bussinessRuleId,
            //    Name = "Input Data",
            //    ParentId = null,
            //    CompanyId = _repo.UserContext.CompanyId,
            //    BreInputDataType = BreInputDataTypeEnum.Root,
            //    HasSubFolders = data.Any(),
            //    DataType = DataTypeEnum.Object,

            //});
            return list.ToList();
        }
    }
}
