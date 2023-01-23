using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Synergy.App.Common
{
    public class PagedList<T>
    {
        public long Total { get; set; }
        public long ItemIndex { get; set; }
        public List<T> Data { get; set; }
        public static PagedList<T> Instance(long total,List<T> data)
        {
            return new PagedList<T>(total, data);
        }
        private PagedList(long total, List<T> data)
        {
            Data = data;
            Total = total;
        }
        public static PagedList<T> Instance(long itemIndex)
        {
            return new PagedList<T>(itemIndex);
        }
        public static PagedList<T> Instance()
        {
            return new PagedList<T>();
        }
        private PagedList(long itemIndex)
        {
            ItemIndex = itemIndex;
           
        }
        private PagedList()
        {
           
        }
    }
    
}
