using AutoMapper;
using Synergy.App.Common;
using Synergy.App.DataModel;
using Synergy.App.Repository;
using Synergy.App.ViewModel;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


namespace Synergy.App.Business
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
