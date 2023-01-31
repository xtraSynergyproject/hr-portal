using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class EmployeeBenefitViewModel : ViewModelBase
    {

        public long GradeId { get; set; }
        public MedicalCardTypeEnum MedicalCardType { get; set; }

        public TravelClassEnum TravelClass { get; set; }
        public int TicketAllowanceInterval { get; set; }

    }
}
