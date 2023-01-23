using AutoMapper;
using CMS.Common;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace CMS.Business
{
    public class AgencyBusiness : BusinessBase<AgencyViewModel, Agency>, IAgencyBusiness
    {
        IUserBusiness _userBusiness;
        private readonly IRepositoryQueryBase<AgencyViewModel> _queryRepo;
        public AgencyBusiness(IRepositoryBase<AgencyViewModel, Agency> repo, IMapper autoMapper, IUserBusiness userBusiness,
            IRepositoryQueryBase<AgencyViewModel> queryRepo) : base(repo, autoMapper)
        {
            _userBusiness = userBusiness;
            _queryRepo = queryRepo;

    }

        public async override Task<CommandResult<AgencyViewModel>> Create(AgencyViewModel model)
        {
            var data = _autoMapper.Map<AgencyViewModel>(model);
            var validateEmail = await IsExists(data);
            if (!validateEmail.IsSuccess)
            {
                return CommandResult<AgencyViewModel>.Instance(model, false, validateEmail.Messages);
            }
            else 
            { 
            var useremail = await _userBusiness.GetSingle(x => x.Email == data.ContactEmail);
            if(useremail !=null)
            {
                data.UserId = useremail.Id;
            }
                else
                {
                    var random = new Random();
                    var Pass = Convert.ToString(random.Next(10000000, 99999999));
                    var usermodel = new UserViewModel
                    {
                        Name = data.ContactPersonName,
                        Email = data.ContactEmail, 
                        UserType = UserTypeEnum.AGENCY, 
                        Password = Pass,
                        PortalName = "CareerPortal",
                        ConfirmPassword = Pass,
                        SendWelcomeEmail = true,
                    };
                    var userResult = await _userBusiness.Create(usermodel);
                    data.UserId = userResult.Item.Id;
                }
            }
            var result = await base.Create(data);

            return CommandResult<AgencyViewModel>.Instance(data, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<AgencyViewModel>> Edit(AgencyViewModel model)
        {
            var data = _autoMapper.Map<AgencyViewModel>(model);
            var validateEmail = await IsExists(data);
            if (!validateEmail.IsSuccess)
            {
                return CommandResult<AgencyViewModel>.Instance(model, false, validateEmail.Messages);
            }
            //var usermodel = await _userBusiness.GetSingleById(model.UserId);
            //usermodel.Name = data.ContactPersonName;
            //usermodel.Email = data.ContactEmail;
            //var usermodel = new UserViewModel
            //{
            //    Name = data.ContactPersonName,
            //    Email = data.ContactEmail,
            //    Id = data.UserId,
            //};
            //var userResult = await _userBusiness.EditUser(usermodel);
            //if (userResult.IsSuccess)
            //{
                var result = await base.Edit(model);
                return CommandResult<AgencyViewModel>.Instance(model, result.IsSuccess, result.Messages);
            //}
            //else
            //{
            //    return CommandResult<AgencyViewModel>.Instance(model, false, validateEmail.Messages);
            //}
            
        }
        
        private async Task<CommandResult<AgencyViewModel>> IsExists(AgencyViewModel model)
        {
            var errorList = new Dictionary<string, string>();
            if (model.ContactEmail == null)
            {
                errorList.Add("Email", "Email is required.");
            }
            else
            {
                var email = await _repo.GetSingle(x => x.ContactEmail == model.ContactEmail && x.Id != model.Id && x.IsDeleted == false);
                if (email != null)
                {
                    errorList.Add("Email", "Email Id already exist.");
                }
            }
            if (errorList.Count > 0)
            {
                return CommandResult<AgencyViewModel>.Instance(model, false, errorList);
            }

            return CommandResult<AgencyViewModel>.Instance();
        }

        public async Task<IList<AgencyViewModel>> GetAgencyList()
        {
            string query = @$"SELECT a.*, c.""Name"" as CountryName
                                FROM rec.""Agency"" as a
                                LEFT JOIN cms.""Country"" as c ON c.""Id"" = a.""CountryId""                                
                                WHERE a.""IsDeleted"" = false";

            var queryData = await _queryRepo.ExecuteQueryList(query, null);
            var list = queryData.ToList();
            return list;
        }        

    }
}
