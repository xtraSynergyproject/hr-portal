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
    public class CompanyBusiness : BusinessBase<CompanyViewModel, Company>, ICompanyBusiness
    {
        private readonly IRepositoryQueryBase<CompanyViewModel> _queryRepo;
        private readonly IServiceProvider _serviceProvider;
        public CompanyBusiness(IRepositoryBase<CompanyViewModel, Company> repo, IRepositoryQueryBase<CompanyViewModel> queryRepo, IMapper autoMapper, IServiceProvider serviceProvider) : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;
            _serviceProvider = serviceProvider;
        }

        public async override Task<CommandResult<CompanyViewModel>> Create(CompanyViewModel model, bool autoCommit = true)
        {
            var name = typeof(Company).Name;
            var companyname = model.Name;
            var companycode = model.Code;
            var errorList = new Dictionary<string, string>();
            //var filter = Builders<CompanyViewModel>.Filter.Regex("Name", new BsonRegularExpression(companyname, "i"));
            //var company = await _repo.Mongo.GetCollection<CompanyViewModel>(name).Find(filter).FirstOrDefaultAsync();
            //var filter1 = Builders<CompanyViewModel>.Filter.Regex("Code", new BsonRegularExpression(companycode, "i"));
            //var code = await _repo.Mongo.GetCollection<CompanyViewModel>(name).Find(filter1).FirstOrDefaultAsync();
            //if (company != null && company.Name.ToLower() == model.Name.ToLower())
            //{
            //    errorList.Add("Name", "Company Name Already Exist");
            //}
            //if (code != null && code.Code.ToLower() == model.Code.ToLower())
            //{
            //    errorList.Add("Code", "Company Code Already Exist");
            //}
            if (errorList.Count > 0)
            {
                return CommandResult<CompanyViewModel>.Instance(model, false, errorList);
            }
            else
            {
                var data = _autoMapper.Map<CompanyViewModel>(model);
                data.SmtpPassword = Helper.Encrypt(model.SmtpPassword);
                if (model.SmsPassword.IsNotNullAndNotEmpty()) 
                {
                    data.SmsPassword = Helper.Encrypt(model.SmsPassword);
                }
                
                var result = await base.Create(data, autoCommit);
                if (result.IsSuccess) 
                {
                    var user = await GetSingle<UserViewModel, User>(x=>x.Email==result.Item.Email);
                    if (!user.IsNotNull()) 
                    {
                        UserViewModel usermodel = new UserViewModel();
                        usermodel.Email = result.Item.Email;
                        usermodel.Name = result.Item.SmtpSenderName.IsNotNull()? result.Item.SmtpSenderName: result.Item.Email;
                       var userBusiness= _serviceProvider.GetService<IUserBusiness>();
                        await userBusiness.Create(usermodel);
                    }
                    
                }
                return CommandResult<CompanyViewModel>.Instance(model, result.IsSuccess, result.Messages);
            }

            //return await base.Create(model,autoCommit);
        }

        public async override Task<CommandResult<CompanyViewModel>> Edit(CompanyViewModel model, bool autoCommit = true)
        {
            var errorList = new Dictionary<string, string>();
            var company = await _repo.GetSingle(x => x.Name == model.Name && x.Id !=model.Id);
            var code = await _repo.GetSingle(x => x.Code == model.Code && x.Id != model.Id);
            if (company != null)
            {
                errorList.Add("Name", "Company Name Already Exist");
            }
            if (code != null)
            {
                errorList.Add("Code", "Company Code Already Exist");
            }
            if (model.Name.IsNullOrEmpty())
            {
                errorList.Add("Name", "Company Name Required ");
            }
            if (model.Code.IsNullOrEmpty())
            {
                errorList.Add("Code", "Company Code Required ");
            }
            if (errorList.Count > 0)
            {
                return CommandResult<CompanyViewModel>.Instance(model, false, errorList);
            }
            else
            {
                var data = _autoMapper.Map<CompanyViewModel>(model);
                data.SmtpPassword = Helper.Encrypt(model.SmtpPassword);
                if (model.SmsPassword.IsNotNullAndNotEmpty()) 
                {
                    data.SmsPassword = Helper.Encrypt(model.SmsPassword);
                }
                
                var result = await base.Edit(data,autoCommit);
                if (result.IsSuccess)
                {
                    var user = await GetSingle<UserViewModel,User>(x => x.Email == result.Item.Email);
                    if (!user.IsNotNull())
                    {
                        UserViewModel usermodel = new UserViewModel();
                        usermodel.Email = result.Item.Email;
                        usermodel.Name = result.Item.SmtpSenderName.IsNotNull() ? result.Item.SmtpSenderName : result.Item.Email;
                        var userBusiness = _serviceProvider.GetService<IUserBusiness>();
                        await userBusiness.Create(usermodel);
                    }

                }
                return CommandResult<CompanyViewModel>.Instance(model, result.IsSuccess, result.Messages);
            }

            // return await base.Edit(model,autoCommit);
        }
        public async Task<List<CompanyViewModel>> GetData()
        {
          
            string query = @$"SELECT c.*, co.""Name"" as CountryName
                            FROM public.""Company"" as c                           
                            LEFT JOIN cms.""Country"" as co ON co.""Id"" = c.""CountryId""
                            where c.""IsDeleted"" = false";
            var list = await _queryRepo.ExecuteQueryList(query, null);
            return list;
        }

    }
}
