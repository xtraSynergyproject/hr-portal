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
    public class ContextVariableBusiness : BusinessBase<ContextVariableViewModel, ContextVariable>, IContextVariableBusiness
    {
        public ContextVariableBusiness(IRepositoryBase<ContextVariableViewModel, ContextVariable> repo, IMapper autoMapper) : base(repo, autoMapper)
        {

        }
        public async override Task<List<ContextVariableViewModel>> GetActiveList()
        {
            return new List<ContextVariableViewModel>
            {
                new ContextVariableViewModel{
                    Id="08aee5e4-c2a2-41b4-9091-f82fcb07f7d0",
                    Name="CurrentDate",
                    DisplayName="Current Date",
                    FullyQualifiedName="Context.CurrentDate",
                    Status=StatusEnum.Active,
                    IsDeleted=false,
                    CompanyId=_repo.UserContext.CompanyId,
                    CreatedBy=_repo.UserContext.UserId,
                    CreatedDate=new DateTime(2021,1,1),
                    LastUpdatedBy=_repo.UserContext.UserId,
                    LastUpdatedDate=new DateTime(2021,1,1),
                    ParentId=null
                },
                new ContextVariableViewModel{
                    Id="6e5b0f71-bcb1-47bf-b43b-d4150d1e7e4d",
                    Name="CurrentDateTime",
                    DisplayName="Current DateTime",
                    FullyQualifiedName="Context.CurrentDateTime",
                    Status=StatusEnum.Active,
                    IsDeleted=false,
                    CompanyId=_repo.UserContext.CompanyId,
                    CreatedBy=_repo.UserContext.UserId,
                    CreatedDate=new DateTime(2021,1,1),
                    LastUpdatedBy=_repo.UserContext.UserId,
                    LastUpdatedDate=new DateTime(2021,1,1),
                    ParentId=null
                },
                // new ContextVariableViewModel{
                //    Id="e99dbefa-49b7-4d34-9626-60a1f4a14ae9",
                //    Name="ProcessFlowId",
                //    DisplayName="Process Flow Id",
                //    FullyQualifiedName="Context.ProcessFlowId",
                //    Status=StatusEnum.Active,
                //    IsDeleted=false,
                //    CompanyId=_repo.UserContext.CompanyId,
                //    CreatedBy=_repo.UserContext.UserId,
                //    CreatedDate=new DateTime(2021,1,1),
                //    LastUpdatedBy=_repo.UserContext.UserId,
                //    LastUpdatedDate=new DateTime(2021,1,1),
                //    ParentId=null
                //},
                //new ContextVariableViewModel{
                //    Id="932b4ee4-66ba-47e0-ae3a-08371e6dc668",
                //    Name="ProcessFlowNumber",
                //    DisplayName="Process Flow Number",
                //    FullyQualifiedName="Context.ProcessFlowNumber",
                //    Status=StatusEnum.Active,
                //    IsDeleted=false,
                //    CompanyId=_repo.UserContext.CompanyId,
                //    CreatedBy=_repo.UserContext.UserId,
                //    CreatedDate=new DateTime(2021,1,1),
                //    LastUpdatedBy=_repo.UserContext.UserId,
                //    LastUpdatedDate=new DateTime(2021,1,1),
                //    ParentId=null
                //},
            };
        }
    }
}
