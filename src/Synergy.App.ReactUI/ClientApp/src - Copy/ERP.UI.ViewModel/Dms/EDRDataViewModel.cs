using ERP.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.UI.ViewModel
{
    public class EDRDataViewModel : ViewModelBase
    {
        public EDRMDRFileTypeEnum FileType { get; set; }
        public long FileId { get; set; }
        public string RevisionNo { get; set; }
        public string Data { get; set; }
        public string Extension { get; set; }
        public long? ParentId { get; set; }
        public long? WorkspaceId { get; set; }
        public bool? IsSkipExistingFile { get; set; }

        //public string DocumentNo { get; set; }
        //public string Revision { get; set; }
        //public string Title { get; set; }
        //public string ContractNo { get; set; }
        //public string ProjectNo { get; set; }
        //public DateTime? StatusDate { get; set; }
        //public long? SerialNo { get; set; }
        //public string QpDocumentNo { get; set; }
        //public string TPDDocumentNo { get; set; }
        //public string Description { get; set; }
        //public string TypeOfDoc { get; set; }

        //public string StartPercent { get; set; }
        //public string IDCPercent { get; set; }
        //public string QPCommentsPercent { get; set; }
        //public string ReceiveQPPercent { get; set; }
        //public string QPApprovalPercent { get; set; }
        //public string QPResponsePercent { get; set; }
        //public string AFCPercent { get; set; }
        //public string QPResponseACHPercent { get; set; }
        //public string QPResponseAFCPercent { get; set; }

        //public string P_StartDate { get; set; }
        //public string P_IDCDate { get; set; }
        //public string P_QPCommentsDate { get; set; }
        //public string P_ReceiveQPDate { get; set; }
        //public string P_QPApprovalDate { get; set; }
        //public string P_QPResponseDate { get; set; }
        //public string P_AFCDate { get; set; }
        //public string P_QPResponseACHDate { get; set; }
        //public string P_QPResponseAFCDate { get; set; }

        //public string F_StartDate { get; set; }
        //public string F_IDCDate { get; set; }
        //public string F_QPCommentsDate { get; set; }
        //public string F_ReceiveQPDate { get; set; }
        //public string F_QPApprovalDate { get; set; }
        //public string F_QPResponseDate { get; set; }
        //public string F_AFCDate { get; set; }
        //public string F_QPResponseACHDate { get; set; }
        //public string F_QPResponseAFCDate { get; set; }

        //public string A_StartDate { get; set; }
        //public string A_IDCDate { get; set; }
        //public string A_QPCommentsDate { get; set; }
        //public string A_ReceiveQPDate { get; set; }
        //public string A_QPApprovalDate { get; set; }
        //public string A_QPResponseDate { get; set; }
        //public string A_AFCDate { get; set; }
        //public string A_QPResponseACHDate { get; set; }
        //public string A_QPResponseAFCDate { get; set; }


        //public string Weightage { get; set; }
        //public string Plan { get; set; }
        //public string WeightagePlan { get; set; }
        //public string ActivityProgram { get; set; }
        //public string WeightageActivity { get; set; }
        //public string WeightageVariance { get; set; }
        //public string Remarks { get; set; }

        //public string ProcessWeightage { get; set; }
        //public string ProcessWeightagePlan { get; set; }
        //public string ProcessWeightageActivity { get; set; }
        //public string ProcessWeightageVariance { get; set; }

    }
}
