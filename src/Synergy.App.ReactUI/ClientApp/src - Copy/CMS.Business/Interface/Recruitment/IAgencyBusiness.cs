﻿using CMS.Data.Model;
using CMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Business
{
    public interface IAgencyBusiness : IBusinessBase<AgencyViewModel, Agency>
    {


        //Task<List<AgencyViewModel>> GetUserIdList();

        Task<IList<AgencyViewModel>> GetAgencyList();
    }
}
