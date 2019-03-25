using Sitecore.Diagnostics;
using Sitecore.EmailCampaign.Server.Filters;
using Sitecore.Security.Accounts;
using Sitecore.Web.Authentication;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Sitecore.Support.EmailCampaign.Server.Filters
{
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
  internal sealed class SitecoreAuthorizeAttribute : AuthorizeAttribute
  {
    internal interface ITicketManager
    {
      bool IsCurrentTicketValid();
    }

    private class TicketManagerWrapper : ITicketManager
    {
      public bool IsCurrentTicketValid()
      {
        return Sitecore.Web.Authentication.TicketManager.IsCurrentTicketValid();
      }
    }

    private static readonly ITicketManager TicketManager = new TicketManagerWrapper();

    public bool AdminsOnly
    {
      get;
      set;
    }

    public SitecoreAuthorizeAttribute(params string[] roles)
    {
      base.Roles = string.Join(",", roles);
    }

    protected override bool IsAuthorized(HttpActionContext actionContext)
    {
      Assert.ArgumentNotNull(actionContext, "actionContext");
      bool num = base.IsAuthorized(actionContext) && !AdminsOnly;
      User user = actionContext.ControllerContext.RequestContext.Principal as User;
      bool flag = (Account)user != (Account)null && user.IsAdministrator;
      if (num | flag)
      {
        return TicketManager.IsCurrentTicketValid();
      }
      return false;
    }
  }
}