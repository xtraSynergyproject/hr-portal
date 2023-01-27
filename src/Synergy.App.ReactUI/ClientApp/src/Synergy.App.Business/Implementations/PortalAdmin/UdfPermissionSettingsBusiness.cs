using AutoMapper;
using Synergy.App.Business.Interface.PortalAdmin;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.App.Business.Implementations.PortalAdmin
{
   public class UdfPermissionSettingsBusiness: BusinessBase<UdfPermissionSettingsViewModel, UdfPermissionHeader>, IUdfPermissionSettingsBusiness
    {
        public UdfPermissionSettingsBusiness(IRepositoryBase<UdfPermissionSettingsViewModel, UdfPermissionHeader> repo, IMapper autoMapper
           ) : base(repo, autoMapper)
        {
            
        }
    }
}
