using AutoMapper;
using CMS.Business.Interface.PortalAdmin;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Business.Implementations.PortalAdmin
{
   public class UdfPermissionSettingsBusiness: BusinessBase<UdfPermissionSettingsViewModel, UdfPermissionHeader>, IUdfPermissionSettingsBusiness
    {
        public UdfPermissionSettingsBusiness(IRepositoryBase<UdfPermissionSettingsViewModel, UdfPermissionHeader> repo, IMapper autoMapper
           ) : base(repo, autoMapper)
        {
            
        }
    }
}
