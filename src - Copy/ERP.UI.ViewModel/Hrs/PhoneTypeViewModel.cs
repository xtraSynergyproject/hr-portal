﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class PhoneTypeViewModel : BaseViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "Phone Type Name")]
        public string Name { get; set; }

        [StringLength(2000)]
        public string Description { get; set; }

        public int? SequenceNo { get; set; }

    }
}