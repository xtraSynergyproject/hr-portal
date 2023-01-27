using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json.Linq;
using Synergy.App.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using Newtonsoft.Json;

namespace Synergy.App.WebUtility
{
    public class WebHelper : IWebHelper
    {
        private readonly IConfiguration _configuration;

        public WebHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<T> GetApiAsync<T>(string url)
        {
            using (var client = new HttpClient())
            {
                var baseUrl = _configuration.GetValue<string>("WebApiUrl");
                var address = new Uri($"{baseUrl}{url}");
                var response = await client.GetAsync(address);
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(content);
            }
        }
        public async Task<T> GetGeneralApiAsync<T>(string url)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(content);
            }
        }
        public async Task<List<T>> GetApiListAsync<T>(string url)
        {
            using (var client = new HttpClient())
            {
                var baseUrl = _configuration.GetValue<string>("WebApiUrl");
                var address = new Uri($"{baseUrl}{url}");
                var response = await client.GetAsync(address);
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<T>>(content);
            }
        }

        public string RemoveHost(string json)
        {
            var host = _configuration.GetValue<string>("ApplicationBaseUrl");
            dynamic controls = JObject.Parse(json);
            RemoveIterateJson(controls.components, host);
            //foreach (var component in controls.components)
            //{
            //    RemoveIterateJson(component, host);
            //    //if (component?.data?.url != null && component.data.url != "")
            //    //{
            //    //    string url = component.data.url;
            //    //    component.data.url = url.Replace(host, "");
            //    //}
            //}
            var newJson = controls.ToString();
            return newJson;
        }
        private void RemoveIterateJson(JArray components, string host)
        {
            foreach (dynamic component in components)
            {
                if (component.components != null)
                {
                    RemoveIterateJson(component.components, host);
                }
                if (component.type == "panel")
                {
                    RemoveIterateJson(component.components, host);
                }
                else if (component.type == "columns")
                {
                    RemoveIterateJson(component.columns, host);
                }
                else if (component.type == "table")
                {
                    foreach (var row in component.rows)
                    {
                        if (row != null)
                        {
                            foreach (JToken cell in row.Children())
                            {
                                var comp = (JArray)cell.SelectToken("components");
                                RemoveIterateJson(comp, host);
                            }
                        }
                    }
                }
                else
                {
                    if (component?.data?.url != null && component.data.url != "")
                    {
                        string url = component.data.url;
                        component.data.url = url.Replace(host, "");
                    }
                }


            }
        }
        public string AddHost(string json)
        {
            var host = _configuration.GetValue<string>("ApplicationBaseUrl");
            dynamic controls = JObject.Parse(json);
            AddIterateJson(controls.components, host);
            //foreach (var component in controls.components)
            //{
            //    if (component?.data?.url != null && component.data.url != "")
            //    {
            //        string url = component.data.url;
            //        url = url.Replace("///", "");
            //        if (url.Contains(":"))
            //        {
            //            var usrl = new System.Uri(url);
            //            url = url.Split(usrl.Authority + "/")[1];
            //        }
            //        component.data.url = $"{host}{ url}";
            //    }
            //}
            var newJson = controls.ToString();
            return newJson;
        }
        private void AddIterateJson(JArray components, string host)
        {
            foreach (dynamic component in components)
            {
                if (component.components != null)
                {
                    AddIterateJson(component.components, host);
                }
                if (component.type == "panel")
                {
                    AddIterateJson(component.components, host);
                }
                else if (component.type == "columns")
                {
                    AddIterateJson(component.columns, host);
                }
                else if (component.type == "table")
                {
                    foreach (var row in component.rows)
                    {
                        if (row != null)
                        {
                            foreach (JToken cell in row.Children())
                            {
                                var comp = (JArray)cell.SelectToken("components");
                                AddIterateJson(comp, host);
                            }
                        }
                    }
                }
                else
                {
                    if ((component?.data?.url != null && component.data.url != ""))
                    {
                        string url = component.data.url;
                        url = url.Replace("///", "");
                        if (url.Contains(":"))
                        {
                            var usrl = new System.Uri(url);
                            url = url.Split(usrl.Authority + "/")[1];
                        }
                        component.data.url = $"{host}{ url}";
                    }
                    else if ((component?.url != null && component.url != ""))
                    {
                        string url = component.url;
                        url = url.Replace("///", "");
                        if (url.Contains(":"))
                        {
                            var usrl = new System.Uri(url);
                            url = url.Split(usrl.Authority + "/")[1];
                        }
                        component.url = $"{host}{ url}";
                    }
                }


            }
        }
        public string GetHost()
        {
            var host = _configuration.GetValue<string>("ApplicationBaseUrl");
            return host;
        }
        public string GetCubeJsKey()
        {
            var host = _configuration.GetValue<string>("CubeJSKey");
            return host;
        }
        public string GetCubeJSBaseUrl()
        {
            var host = _configuration.GetSection("CubeJSSettings").GetValue<string>("CubeJSBaseUrl");
            return host;
        }
        public string GetEsdbConnectionUrl()
        {
            var host = _configuration.GetValue<string>("EsdbConnectionUrl");
            return host;
        }

    }
    public interface IWebHelper
    {
        Task<T> GetApiAsync<T>(string url);
        Task<T> GetGeneralApiAsync<T>(string url);
        Task<List<T>> GetApiListAsync<T>(string url);
        string RemoveHost(string json);
        string AddHost(string json);
        string GetHost();
        string GetCubeJsKey();
        string GetCubeJSBaseUrl();
        string GetEsdbConnectionUrl();
    }
}
