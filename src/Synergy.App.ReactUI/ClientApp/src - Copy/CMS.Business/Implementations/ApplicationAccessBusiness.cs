using AutoMapper;
using CMS.Common;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace CMS.Business
{
    public class ApplicationAccessBusiness : BusinessBase<ApplicationAccessViewModel, ApplicationAccess>, IApplicationAccessBusiness
    {
        IColumnMetadataBusiness _columnMetadataBusiness;
        public ApplicationAccessBusiness(IRepositoryBase<ApplicationAccessViewModel, ApplicationAccess> repo, IMapper autoMapper) :
            base(repo, autoMapper)
        {
        }

        public async Task CreateAccessLog(HttpRequest request, AccessLogTypeEnum type)
        {
            var remoteIp = request.HttpContext.Connection.RemoteIpAddress;

            var avm = new ApplicationAccessViewModel
            {
                UserId = _repo.UserContext.UserId,
                Email = _repo.UserContext.Email,
                UserName = _repo.UserContext.Name,
                AccessType = type,
                LogDate = DateTime.Now,
                ClientIP = remoteIp.ToString()

            };
            await _repo.Create(avm);
        }

        public async Task CreateLogin(HttpRequest request, UserViewModel user)
        {
            var remoteIp = request.HttpContext.Connection.RemoteIpAddress;

            var avm = new ApplicationAccessViewModel
            {
                UserId = user.Id,
                Email = user.Email,
                UserName = user.Name,
                AccessType = AccessLogTypeEnum.Login,
                LogDate = DateTime.Now,
                ClientIP = remoteIp.ToString(),
                Url = request.Path,
                CreatedBy = user.Id,
                LastUpdatedBy = user.Id

            };
            await _repo.Create(avm);
        }
    }
}
