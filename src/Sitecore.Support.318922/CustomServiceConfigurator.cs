using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Sitecore.Services.Infrastructure.Sitecore.DependencyInjection;

namespace Sitecore.Support.EmailCampaign.Server.DependencyInjection
{
  public class CustomServiceConfigurator : IServicesConfigurator
  {
    public void Configure(IServiceCollection serviceCollection)
    {
      Assembly[] assemblies = new Assembly[1] { GetType().Assembly };
      serviceCollection.AddWebApiControllers(assemblies);
    }
  }
}