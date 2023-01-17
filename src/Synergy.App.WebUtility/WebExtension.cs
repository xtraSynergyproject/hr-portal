
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Synergy.App.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Synergy.App.WebUtility
{
    public static class WebExtension
    {
        public static void AddModelErrors(this ModelStateDictionary modelState, Dictionary<string, string> messages)
        {
            foreach (var item in messages)
            {
                modelState.AddModelError(item.Key, item.Value);
            }
        }
        public static string GetCultureName(HttpContext context)
        {

            if (context != null && context.User != null && context.User.HasClaim(x => x.Type == ClaimTypes.WindowsDeviceClaim))
            {
                return context.User.FindFirst(ClaimTypes.WindowsDeviceClaim).Value;
            }
            return "en-US";
        }
        public static void AddMyRclServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
        }
        public static string GetMetatdata(string pageName)
        {
            pageName = pageName.ToLower();
            string meta = "";
            switch (pageName)
            {
                case "egovernance":
                case "egovernanceind":
                    meta = "We are introducing the flexible path for modern digital experiences for government agencies and citizens.Visit for details!";
                    break;
                case "home":
                    meta = "Synergy accelerates and reduces the cost of digital transformation by streamlining processes, identifying inefficiencies, and providing insights. Schedule demo today!";
                    break;
                case "overview":
                case "synergyplatformoverview":
                    meta = "The Synergy low code digital transformation platform is perfect for enterprises that want modules to be customised to match their individual specifications and changing demands.";
                    break;
                case "technologiesused":
                    meta = "Synergy used the technology which is designed to simplify complicated corporate processes, handle unstructured data, and increase consumer involvement in response to changing needs.";
                    break;
                case "businessprocessmanagement":
                    meta = "Business Process Management and Automation (BPM) Software by Synergy drives end-to-end process automation and enables continuous improvement. Visit now!";
                    break;
                case "mobileapp":
                    meta = "Synergy low code digital transformation platform has innovative mobile application platform. It provide collaborative and customer centric mobile platform. Visit now!";
                    break;
                case "apiconnector":
                    meta = "Synergy can connect to practically any external API using the API Connector. It is a robust, user-friendly tool for instantly importing data from any API into a Google Sheets. Visit now.";
                    break;
                case "analyticsdashboard":
                    meta = "Analytics and dashboards from Synergy are a strong tool set for businesses to absorb, organise, identify, and analyse data in order to provide actionable insights. Visit us for more information.";
                    break;
                case "servicedesk":
                    meta = "Synergy’s service desk empowers teams to deliver great service experiences and ensures your employees and customers can get help quickly. Visit us for details!";
                    break;
                case "salesmarketing":
                    meta = "Synergy’s sales and marketing solution helps in better engagement and coordination between teams and improve lead generation revenue. Visit us for details!";
                    break;
                case "recruitmentassessment":
                    meta = "Synergy’s Recruitment assessment solution makes it easier for recruiters to find, track, assess, and hire the best prospects quickly and make the informed decision. Visit us now.";
                    break;
                case "projectmanagementnew":
                    meta = "Synergy’s Project management solution helps every business to plan and manage the project successfully and complete its listed goals and deliverables. Visit us for details!";
                    break;
                case "synergyeye":
                    meta = "Synergy eye protect you by offering security, safety and sanctuary for home, business or office. Let the eye of vigilance never be closed. Contact us for details.";
                    break;
                case "citizenmobileapp":
                    meta = "Synergy provides citizen mobile applications services. It helps for e-governance, smart city services, public grievances and more. Visit us for details!";
                    break;
                case "documentmanagement":
                    meta = "Synergy’s eDMS handles documents by electronically storing, organizing, indexing and filing. It can be retrieved as and when required at the click of a button. Visit for details!";
                    break;
                case "humancapitalmanagement":
                    meta = "Synergy’s HCMS is a comprehensive solution to all HR needs, all integrated into one, to meet the management\'s requirements. It is a browser-based application, simple to use and does not require any special training.";
                    break;
                case "financialmanagement":
                    meta = "Synergy’s financial management manages income, expenses, and assets that company use. It also improves short terms and long terms business performance. Visit for details!";
                    break;
                case "intelligencepolicing":
                    meta = "Synergy\'s intelligent policing offers a variety of AI tools and approaches that aid in the policing process, such as decision assistance and optimization strategies. Visit now!";
                    break;
                case "socialmediaanalytics":
                    meta = "Synergy\'s social media analytics collects and analyses audience data from social networks and can be utilized to improve strategic business decisions.Schedule demo today.";
                    break;
                case "successstory":
                    meta = "Know more about Synergy’s success story, how we help our clients achieve the digital transformation goal.";
                    break;
                case "ourteam":
                    meta = "Know about the incredible team members of Synergy. Here we nurture and develop the unique business ideas.";
                    break;
                case "synergycareer":
                    meta = "Add your magic to ours. You\'ve come here because you want to shape the future.Find work as a software developer, business analyst, or in a variety of other fields.";
                    break;
                case "aboutus":
                    meta = "Synergy is a pioneer in low-code digital transformation. Discover our company\'s goal, vision, and culture. For details visit us.";
                    break;
                case "company":
                    meta = "Synergy low code digital transformation platform is used by successful businesses to design and deploy complex, content - driven, customer - engaging business apps.";
                    break;
                case "contactus":
                case "contactusindia":
                    meta = "Contact us for details about Synergy. We are employee friendly organization and believe that employees are the greatest assets. We encourage our employees to provide their input and ideas to create a better environment.";
                    break;
                case "requestconsultation":
                    meta = "Our team is happy to answer your queries. Please complete the form below. We\'ll get back to you as soon as possible!";
                    break;
                case "termsofuse":
                    meta = "Know more about the terms of use of Synergy low code digital transformation. Terms of use are the rules, specifications, and requirements for the use of a product or service.";
                    break;
                case "privacypolicy":
                    meta = "This Privacy Policy is meant to help you understand what information we collect, why we collect it and how you can update, manage, export, and delete your information.";
                    break;
                case "casemanagement":
                case "facedetection":
                case "predictivemaintenance":
                case "numberplatedetection":
                    meta = "Read about digital transformation use cases of Synergy’s customers in the various industry and sectors like financial services, public sector, manufacturing and healthcare.";
                    break;
                case "contentmanagement":
                    meta = "Synergy's content management solution manages content digitally for businesses. It enables its users to create, modify, store and publish content digitally. Visit us for details!";
                    break;
                default:
                    break;
            }
            if (meta.IsNotNullAndNotEmpty())
            {
                return $"<meta name=\"description\" content=\"{meta}\" />";
            }
            return meta;
        }
    }
}
