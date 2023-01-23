﻿using CMS.Data.Model;
using CMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Business
{
    public interface ICandidateExperienceByOtherBusiness : IBusinessBase<CandidateExperienceByOtherViewModel, CandidateExperienceByOther>
    {
        Task<IList<CandidateExperienceByOtherViewModel>> GetNameByCandidate(string candidateProfileId);
    }
}
