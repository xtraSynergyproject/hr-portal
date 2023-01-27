using System;
using CMS.Common;
using CMS.Data.Model;
using CMS.Data.Repository;
using CMS.UI.ViewModel;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CodeAnalysis.CSharp.Scripting;

namespace CMS.Business
{
    public class DynamicScriptBusiness : IDynamicScriptBusiness
    {
        public async Task<CommandResult<T>> ExecuteScript<T>(string script, T viewModel, TemplateTypeEnum templateType, dynamic inputData, IUserContext uc, IServiceProvider sp)
        {
            try
            {
                if (script.IsNotNullAndNotEmpty())
                {

                    var methodScript = GenerateScript(templateType.ToString(), script);
                    var scriptParam = new ScriptParam<T>();
                    scriptParam.viewModel = viewModel;
                    scriptParam.uc = uc;
                    scriptParam.sp = sp;
                    scriptParam.udf = inputData.Data;


                    var result = await CSharpScript.EvaluateAsync<CommandResult<T>>(script, options:
                        ScriptOptions.Default
                        .WithImports("CMS.UI.ViewModel",
                        "CMS.Business",
                        "CMS.Common",
                        "CMS.Data.Model",
                        "CMS.Data.Repository",
                        "System")
                        .WithReferences(typeof(ServiceViewModel).Assembly,
                        typeof(NtsService).Assembly,
                        typeof(ServiceBusiness).Assembly,
                        typeof(IRepositoryBase<,>).Assembly), globals: scriptParam);
                    return result;


                }
                return CommandResult<T>.Instance(viewModel, false, "Custom script is null");
            }
            catch (Exception e)
            {

                throw;
            }
        }

        private string GenerateScript(string templateType, string script)
        {

            return @$"CommandResult<{templateType}TemplateViewModel> ExecuteScriptFor{templateType}({templateType}TemplateViewModel viewModel, dynamic udf, IUserContext uc, IServiceProvider sp)
                        {{{script} }} return ExecuteScriptFor{templateType}(viewModel,udf,uc,sp);";
        }



    }
    public class ScriptParam<T>
    {
        public T viewModel { get; set; }
        public dynamic udf { get; set; }
        public IUserContext uc { get; set; }
        public IServiceProvider sp { get; set; }
    }
}
