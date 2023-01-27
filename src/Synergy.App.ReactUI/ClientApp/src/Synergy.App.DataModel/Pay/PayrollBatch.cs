using Synergy.App.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Synergy.App.DataModel
{
    public class PayrollBatch : DataModelBase
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public PayrollRunTypeEnum RunType { get; set; }

        public int? YearMonth { get; set; }
        public DateTime PayrollStartDate { get; set; }
        public DateTime PayrollEndDate { get; set; }

        public DateTime? AttendanceStartDate { get; set; }
        public DateTime? AttendanceEndDate { get; set; }


        public PayrollStatusEnum PayrollStatus { get; set; }


        public double TotalEarning { get; set; }
        public double TotalDeduction { get; set; }
        public double NetAmount { get; set; }


        public int ExecutePayrollTotal { get; set; }
        public int ExecutePayrollError { get; set; }

        public int PayslipTotal { get; set; }
        public int PayslipError { get; set; }

        public int BankLetterTotal { get; set; }
        public int BankLetterError { get; set; }
        public string PayrollGroupId { get; set; }
    }
    [Table("PayrollBatchLog", Schema = "log")]
    public class PayrollBatchLog : PayrollBatch
    {
        public string RecordId { get; set; }
        public long LogVersionNo { get; set; }
        public bool IsLatest { get; set; }
        public DateTime LogStartDate { get; set; }
        public DateTime LogEndDate { get; set; }
        public DateTime LogStartDateTime { get; set; }
       public DateTime LogEndDateTime { get; set; } 
        public bool IsDatedLatest { get; set; } 
        public bool IsVersionLatest { get; set; }
    }
}
