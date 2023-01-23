using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ERP.UI.ViewModel
{
    public class ValidationViewModel
    {
        public List<string> Message { get; set; }
        //private bool _IsSucceed { get; set; }

        public ValidationViewModel()
        {
            Message = new List<string>();
            //_IsSucceed = false;
        }

        public bool IsSucceed()
        {
            if (Message.Count == 0)
                return true;
            else
                return false;
        }
    }
}
