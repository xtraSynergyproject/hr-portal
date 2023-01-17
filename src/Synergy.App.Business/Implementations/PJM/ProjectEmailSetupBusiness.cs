using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;


namespace Synergy.App.Business
{
    public class ProjectEmailSetupBusiness : BusinessBase<ProjectEmailSetupViewModel, ProjectEmailSetup>, IProjectEmailSetupBusiness
    {
        private readonly IRepositoryQueryBase<IdNameViewModel> _queryRepo;
        private readonly IRepositoryQueryBase<ProjectEmailSetupViewModel> _queryRepo1;
        private readonly IProjectManagementQueryBusiness _projectManagementQueryBusiness;
        IUserContext _userContext;
        IServiceProvider _sp;
        public ProjectEmailSetupBusiness(IRepositoryBase<ProjectEmailSetupViewModel, ProjectEmailSetup> repo, IMapper autoMapper,
            IRepositoryQueryBase<IdNameViewModel> queryRepo, IUserContext userContext, IServiceProvider sp, IProjectManagementQueryBusiness projectManagementQueryBusiness,
            IRepositoryQueryBase<ProjectEmailSetupViewModel> queryRepo1) : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;
            _queryRepo1 = queryRepo1;
            _userContext = userContext;
            _sp = sp;
            _projectManagementQueryBusiness = projectManagementQueryBusiness;
        }

        public async override Task<CommandResult<ProjectEmailSetupViewModel>> Create(ProjectEmailSetupViewModel model, bool autoCommit = true)
        {
            model.Signature = HttpUtility.HtmlDecode(model.Signature);
            model.SmtpPassword = Helper.Encrypt(model.SmtpPassword);
            var data = _autoMapper.Map<ProjectEmailSetupViewModel>(model);
            var result = await base.Create(data, autoCommit);

            return CommandResult<ProjectEmailSetupViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<ProjectEmailSetupViewModel>> Edit(ProjectEmailSetupViewModel model, bool autoCommit = true)
        {
            model.Signature = HttpUtility.HtmlDecode(model.Signature);
            var data = _autoMapper.Map<ProjectEmailSetupViewModel>(model);
            data.SmtpPassword = Helper.Encrypt(model.SmtpPassword);
            var result = await base.Edit(data,autoCommit);

            return CommandResult<ProjectEmailSetupViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        public async Task<IList<IdNameViewModel>> GetProjectsList()
        {
            var queryData = await _projectManagementQueryBusiness.GetProjectsListData();
            var list = queryData.ToList();
            return list;
        }
        public async Task<IList<ProjectEmailSetupViewModel>> GetEmailSetupList()
        {
            var queryData = await _projectManagementQueryBusiness.GetEmailSetupListData();
            var list = queryData.ToList();
            if (_userContext.IsSystemAdmin==false)
            {
                list = list.Where(x => x.UserId == _userContext.UserId).ToList();
            }
            return list;
        }

        public async Task<IList<ProjectEmailSetupViewModel>> GetSmtpEmailId()
        {

            var queryData = await _projectManagementQueryBusiness.GetProjectEmailSetupData();

            var companydata = await _projectManagementQueryBusiness.GetSingleProjectEmailSetupData();

            var list = queryData.ToList();
            if (companydata.IsNotNull())
            {
                list.Add(new ProjectEmailSetupViewModel()
                {
                    EmailText = companydata.EmailText,
                    UserId=companydata.UserId,
                    SmtpUserId=companydata.SmtpUserId,
                    SequenceOrder=1
                }) ;
            }
            return list.OrderBy(x=>x.SequenceOrder).ToList();
        }

        public async Task<IList<ProjectEmailSetupViewModel>> ReadEmailSetupUsers()
        {
            var result = await GetList();
            return result;
        }
        public async Task<long> GetEmailSetupTotalCount(string id)
        {            
            var result = await GetSingleById(id);

            return result.Count == null ? 0 : result.Count;

        }

        public async Task<ProjectEmailSetupViewModel> UpdateEmailSetupCount(string id, long count)
        {

            var result = await GetSingleById(id);
            result.Count = count;
            await base.Edit(result);

            return result;
        }

        public async Task<bool> TestEmail(string id)
        {

            var emailsetup = await GetSingleById(id);
            var email = new EmailViewModel();
           // email.From = result.Item.OwnerUserId;
            email.Subject = "Test Synergy Email Setup";
            email.To = emailsetup.SmtpUserId;
            email.SenderEmail = emailsetup.SmtpUserId;
            var _emailBusiness = _sp.GetService<IEmailBusiness>();
            var restsetawait=await _emailBusiness.SendMailTask(email);
            var testmessage = await _emailBusiness.TestEmailSetupMail(id);
            if (testmessage)
            {
                emailsetup.Message ="Succesfull";
            }
            else
            {
                emailsetup.Message = "UnSuccesfull";
            }
            await base.Edit(emailsetup);

            return testmessage;
        }

        public async Task<IList<ProjectEmailSetupViewModel>> ReadEmailSetupByProjectId(string id)
        {           
            var result = await GetList(x=>x.ServiceId==id);
            return result;
        }
    }
}
