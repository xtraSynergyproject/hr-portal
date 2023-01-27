using AutoMapper;
using CMS.Common;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


namespace CMS.Business
{
    public class DataMigration 
    {
        public async Task MigrateEmailToWBSItem()
        {
            Console.WriteLine("WBS Item Migrate!");
            using (var httpClient = new HttpClient())
            {
                var address = new Uri("http://localhost:50276/api/CreateWbsItem");
                var response = await httpClient.GetAsync(address);
               
            }
        }
    }
}
