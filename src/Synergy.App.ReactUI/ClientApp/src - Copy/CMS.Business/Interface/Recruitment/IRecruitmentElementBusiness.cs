using CMS.Data.Model;
using CMS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CMS.Common;

namespace CMS.Business
{
    public interface IRecruitmentElementBusiness : IBusinessBase <RecruitmentCandidateElementInfoViewModel, RecruitmentCandidateElementInfo>
    {
        Task<IList<RecruitmentPayElementViewModel>> GetPayElementIdNameList();
        Task<IList<RecruitmentCandidateElementInfoViewModel>> GetElementData(string appid);
        Task<IList<IdNameViewModel>> GetUserIdNameList();
        Task<IList<IdNameViewModel>> GetGradeIdNameList();
        Task<IdNameViewModel> GetGrade(string code);
        Task<bool> Beneficiarycreate(ApplicationBeneficiaryViewModel model,DataActionEnum action);
        Task<IList<ApplicationBeneficiaryViewModel>> GetBeneficiartData(string appid);

        Task<IList<IdNameViewModel>> GetLocationIdNameList();
        Task<IdNameViewModel> GetAccomadationValue(string id);
        Task<string> GenerateFinalOfferRef(string appno);
        Task<ApplicationBeneficiaryViewModel> GetBeneficiartDataByid(string id);
    }
}
