
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
    public class DataSourceResult
    {

        public IEnumerable Data { get; set; }
        public int Total { get; set; }
        public IEnumerable<AggregateResult> AggregateResults { get; set; }
        public object Errors { get; set; }
    }
    public class AggregateResult
    {


        public object Value { get; }
        public string Member { get; }
        public object FormattedValue { get; }
        public int ItemCount { get; set; }
        public string Caption { get; }
        public string FunctionName { get; }
        public string AggregateMethodName { get; }

        // public string Format(string format);
        //public override string ToString();
    }
    public class DataSourceRequestAttribute : ModelBinderAttribute
    {

    }
    public class DataSourceRequest
    {
        //  public DataSourceRequest();

        public int Page { get; set; }
        public int PageSize { get; set; }
        public IList<SortDescriptor> Sorts { get; set; }
        public IList<IFilterDescriptor> Filters { get; set; }
        //public IList<GroupDescriptor> Groups { get; set; }
        //public IList<AggregateDescriptor> Aggregates { get; set; }
        public bool GroupPaging { get; set; }
        public bool IncludeSubGroupCount { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
    }
    public class SortDescriptor : JsonObject, IDescriptor
    {
        //public SortDescriptor(string member, ListSortDirection order);

        public string Member { get; set; }
        public ListSortDirection SortDirection { get; set; }

        public void Deserialize(string source)
        {
            throw new NotImplementedException();
        }

        public string Serialize()
        {
            throw new NotImplementedException();
        }

        protected override void Serialize(IDictionary<string, object> json)
        {
            throw new NotImplementedException();
        }
    }
    public abstract class JsonObject
    {
        //public IDictionary<string, object> ToJson();
        protected abstract void Serialize(IDictionary<string, object> json);
    }
    public interface IDescriptor
    {
        void Deserialize(string source);
        string Serialize();
    }
    public interface IFilterDescriptor
    {
        Expression CreateFilterExpression(Expression instance);
    }
}
