using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using static Hrms.Helpers.ApplicationHelper;
using Hrms.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Contracts;
using Microsoft.AspNetCore.Authorization;
using System.Net;

namespace Hrms.Extensions
{
    public class ActionFilter :ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
        //    throw new NotImplementedException();
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            HrmsContext dbHrmsContext = new HrmsContext();
            bool hasAllowAnonymous = context.ActionDescriptor.EndpointMetadata
                                   .Any(em => em.GetType() == typeof(AllowAnonymousAttribute)); //< -- Here it is
            if (hasAllowAnonymous) return;
            try
            {
                var sessionDataString = context.HttpContext.Session.GetString(SessionUserLogin);

                if (sessionDataString != null)
                {
                    var sessionData = JsonConvert.DeserializeObject<UserSessionData>(sessionDataString);
                    UserAccounts users = dbHrmsContext.UserAccounts.FirstOrDefault(x => x.Id == Convert.ToInt32(sessionData.UserId));
                    if (users != null)
                    {
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception)
            {
                context.Result = new RedirectResult(GetSettingContentByName(dbHrmsContext, "Website Name") + "login");
                return;
            }
            base.OnActionExecuting(context);
        }
    }
}
