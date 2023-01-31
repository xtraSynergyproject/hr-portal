
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;
using Synergy.App.Common;
////using Kendo.Mvc.UI;

namespace Synergy.App.ViewModel
{
    public class DataTableRequest
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int draw { get; set; }
        public int start { get; set; }
        public int length { get; set; }
        public long recordsTotal { get; set; }
        public long recordsFiltered { get; set; }
        public string error { get; set; }

        public List<Columns> columns { get; set; }
        public List<Order> order { get; set; }
        public Search search { get; set; }
        public dynamic data { get; set; }

    }
    public class Columns
    {
        public string data { get; set; }
        public string name { get; set; }
        public bool searchable { get; set; }
        public bool orderable { get; set; }
        public Search search { get; set; }

    }
    public class Order
    {
        public int column { get; set; }
        public string dir { get; set; }
    }
    public class Search
    {
        public string value { get; set; }
        public bool regex { get; set; }
    }
}
