using AutoMapper;
using CMS.Common;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
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


namespace CMS.Business
{
    public class ProjectEmailSetupBusiness : BusinessBase<ProjectEmailSetupViewModel, ProjectEmailSetup>, IProjectEmailSetupBusiness
    {
        private readonly IRepositoryQueryBase<IdNameViewModel> _queryRepo;
        private readonly IRepositoryQueryBase<ProjectEmailSetupViewModel> _queryRepo1;
        IUserContext _userContext;
        IServiceProvider _sp;
        public ProjectEmailSetupBusiness(IRepositoryBase<ProjectEmailSetupViewModel, ProjectEmailSetup> repo, IMapper autoMapper,
            IRepositoryQueryBase<IdNameViewModel> queryRepo, IUserContext userContext, IServiceProvider sp,
            IRepositoryQueryBase<ProjectEmailSetupViewModel> queryRepo1) : base(repo, autoMapper)
        {
            _queryRepo = queryRepo;
            _queryRepo1 = queryRepo1;
            _userContext = userContext;
            _sp = sp;
        }

        public async override Task<CommandResult<ProjectEmailSetupViewModel>> Create(ProjectEmailSetupViewModel model)
        {
            model.Signature = HttpUtility.HtmlDecode(model.Signature);
            model.SmtpPassword = Helper.Encrypt(model.SmtpPassword);
            var data = _autoMapper.Map<ProjectEmailSetupViewModel>(model);
            var result = await base.Create(data);

            return CommandResult<ProjectEmailSetupViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }

        public async override Task<CommandResult<ProjectEmailSetupViewModel>> Edit(ProjectEmailSetupViewModel model)
        {
            model.Signature = HttpUtility.HtmlDecode(model.Signature);
            var data = _autoMapper.Map<ProjectEmailSetupViewModel>(model);
            data.SmtpPassword = Helper.Encrypt(model.SmtpPassword);
            var result = await base.Edit(data);

            return CommandResult<ProjectEmailSetupViewModel>.Instance(model, result.IsSuccess, result.Messages);
        }
        public async Task<IList<IdNameViewModel>> GetProjectsList()
        {
            
          string query = @$"select s.""ServiceSubject"" as Name, s.""Id"" as Id 
                            from public.""NtsService"" as s
                            join public.""Template"" as t on t.""Id""=s.""TemplateId"" and t.""Code""='PROJECT_SUPER_SERVICE' and t.""IsDeleted""=false and t.""CompanyId""='{_userContext.CompanyId}'
                            where s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}' ";
          
            var queryData = await _queryRepo.ExecuteQueryList(query, null);
            var list = queryData.ToList();
            return list;
        }
        public async Task<IList<ProjectEmailSetupViewModel>> GetEmailSetupList()
        {

            string query = @$"select p.*,u.""Name"" as UserName, s.""ServiceSubject"" as ServiceName,p.""Id"" 
                            from public.""ProjectEmailSetup"" as p
                            join public.""User"" as u on u.""Id""=p.""UserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
                            left join public.""NtsService"" as s on s.""Id""=p.""ServiceId"" and s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
							where p.""IsDeleted"" = false and p.""CompanyId""='{_userContext.CompanyId}'";

            var queryData = await _queryRepo1.ExecuteQueryList(query, null);
            var list = queryData.ToList();
            if (_userContext.IsSystemAdmin==false)
            {
                list = list.Where(x => x.UserId == _userContext.UserId).ToList();
            }
            return list;
        }

        public async Task<IList<ProjectEmailSetupViewModel>> GetSmtpEmailId()
        {

            string query = @$"select concat('""',u.""Name"",'"" ','<',p.""SmtpUserId"",'>') as EmailText,p.*,2 as SequenceOrder
                            from public.""ProjectEmailSetup"" as p
                            join public.""User"" as u on u.""Id""=p.""UserId"" and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
                            join public.""NtsService"" as s on s.""Id""=p.""ServiceId"" and s.""IsDeleted""=false and s.""CompanyId""='{_userContext.CompanyId}'
							where p.""IsDeleted"" = false and u.""Id""='{_userContext.UserId}' and p.""CompanyId""='{_userContext.CompanyId}' ";

            var queryData = await _queryRepo1.ExecuteQueryList(query, null);
            var query1 = $@"select concat('""',u.""Name"",'"" ','<',p.""SmtpUserId"",'>') as EmailText,u.""Id"" as UserId,
                            p.""SmtpUserId"" as SmtpUserId
                            from public.""Company"" as p
                            join public.""User"" as u on u.""CompanyId""=p.""Id"" and u.""Id""='{_userContext.UserId}' and u.""IsDeleted""=false and u.""CompanyId""='{_userContext.CompanyId}'
                             where p.""IsDeleted""=false and p.""CompanyId""='{_userContext.CompanyId}' ";
            var companydata= await _queryRepo1.ExecuteQuerySingle(query1, null);
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
