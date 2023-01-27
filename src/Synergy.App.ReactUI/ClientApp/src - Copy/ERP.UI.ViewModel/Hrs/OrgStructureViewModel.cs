using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using ERP.Utility;

namespace ERP.UI.ViewModel
{
    public class OrgStructureViewModel
    {
        private string _Department;
        private string _Division;
        private string _Section;
        private string _Unit;
        public string Group { get; set; }
        public string Department
        {
            get
            {
                return _Department ?? Group;
            }

            set
            {
                _Department = value;
            }
        }
        public string Division
        {
            get
            {
                return _Division ?? Department;
            }

            set
            {
                _Division = value;
            }
        }
        public string Section
        {
            get
            {
                return _Section ?? Division;
            }

            set
            {
                _Section = value;
            }
        }
        public string Unit
        {
            get
            {
                return _Unit ?? Section;
            }

            set
            {
                _Unit = value;
            }
        }

    }
}
