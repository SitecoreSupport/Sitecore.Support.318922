using Sitecore.Diagnostics;
using Sitecore.EmailCampaign.Model.Web;
using Sitecore.EmailCampaign.Model.Web.Exceptions;
using Sitecore.EmailCampaign.Server.Contexts;
using Sitecore.EmailCampaign.Server.Responses;
using Sitecore.Modules.EmailCampaign.Core;
using Sitecore.Modules.EmailCampaign.Messages;
using System;
using System.Web.Http;
using Sitecore.Support.EmailCampaign.Server.Filters;
using Sitecore.Services.Core;
using Sitecore.Services.Infrastructure.Web.Http;
using Sitecore.Modules.EmailCampaign.Validators;
using Sitecore.Modules.EmailCampaign.Services;
using Sitecore.EmailCampaign.Model.Web.Settings;
using Sitecore.ExM.Framework.Diagnostics;
using Sitecore.EmailCampaign.Server.Helpers;
using Sitecore.EmailCampaign.Cm.Factories;
using Sitecore.Configuration;
using Sitecore.EmailCampaign.Model.Message;
using Sitecore.Text;
using System.Collections.Generic;
using System.Linq;
using Sitecore.Data.Items;
using Sitecore.Data;
using Sitecore.EmailCampaign.Cm.Dispatch;

namespace Sitecore.Support.EmailCampaign.Server.Controllers.SendQuickTest
{
  [ServicesController("EXM.ExecuteSendQuickTestEx")]
  [SitecoreAuthorize(new string[]
{
  "sitecore\\EXM Advanced Users",
  "sitecore\\EXM Users"
})]
  public class ExecuteSendQuickTestExController: ServicesApiController
  {
    private readonly RegexValidator _emailRegexValidator;

    private readonly IExmCampaignService _exmCampaignService;

    private readonly GlobalSettings _globalSettings;

    private readonly ILogger _logger;

    private readonly RegistryHelper _registryHelper;

    private readonly IAbnTestService _abnTestService;

    private readonly ISendingManagerFactory _sendingManagerFactory;

    public ExecuteSendQuickTestExController(IExmCampaignService exmCampaignService, ILogger logger, IAbnTestService abnTestService, ISendingManagerFactory sendingManagerFactory)
      : this(exmCampaignService, GlobalSettings.Instance, new RegistryHelper(), logger, (RegexValidator)Factory.CreateObject("emailRegexValidator", true), abnTestService, sendingManagerFactory)
    {
    }

    internal ExecuteSendQuickTestExController(IExmCampaignService exmCampaignService, GlobalSettings globalSettings, RegistryHelper registryHelper, ILogger logger, RegexValidator emailRegexValidator, IAbnTestService abnTestService, ISendingManagerFactory sendingManagerFactory)
    {
      Assert.ArgumentNotNull(exmCampaignService, "exmCampaignService");
      Assert.ArgumentNotNull(globalSettings, "globalSettings");
      Assert.ArgumentNotNull(registryHelper, "registryHelper");
      Assert.ArgumentNotNull(logger, "logger");
      Assert.ArgumentNotNull(emailRegexValidator, "emailRegexValidator");
      Assert.ArgumentNotNull(abnTestService, "abnTestService");
      Assert.ArgumentNotNull(sendingManagerFactory, "sendingManagerFactory");
      _exmCampaignService = exmCampaignService;
      _globalSettings = globalSettings;
      _registryHelper = registryHelper;
      _logger = logger;
      _emailRegexValidator = emailRegexValidator;
      _abnTestService = abnTestService;
      _sendingManagerFactory = sendingManagerFactory;
    }

    [ActionName("DefaultAction")]
    public Response ExecuteSendQuickTest(SendQuickTestContext data)
    {
      Assert.ArgumentNotNull(data, "requestArgs");
      Assert.IsNotNull(data.TestEmails, "TestEmails must be supplied");
      try
      {
        MessageItem messageItem = _exmCampaignService.GetMessageItem(Guid.Parse(data.MessageId), data.Language);
        if (messageItem != null)
        {
          if (!_globalSettings.GetNoSend())
          {
            if (messageItem.State != MessageState.Sent)
            {
              string text = CheckAddressValidation(data.TestEmails);
              if (string.IsNullOrEmpty(text))
              {
                AbnTest abnTest = _abnTestService.GetAbnTest(messageItem);
                if (abnTest != null && abnTest.TestCandidates.Count > 1 && data.VariantIds.Length == 0)
                {
                  return new Response
                  {
                    ErrorMessage = EcmTexts.Localize("The quick test cannot be sent. Select one or more variants and try again.", Array.Empty<object>()),
                    Error = true
                  };
                }
                _registryHelper.LastTestEmail = data.TestEmails;
                try
                {
                  var sentData = RunQuickTest(data.TestEmails, data.VariantIds, messageItem, abnTest);

                  if (sentData.Any(sd => sd.Errors.Any()))
                  {
                    return new Response
                    {
                      Error = true,
                      ErrorMessage = string.Join("\n", sentData.Select(sd => sd.Errors))
                    };


                  }
                }
                catch (EmailCampaignException ex)
                {
                  return new Response
                  {
                    ErrorMessage = ex.LocalizedMessage,
                    Error = true
                  };
                }
                return new StringResponse
                {
                  Value = EcmTexts.Localize("The message has been sent.", Array.Empty<object>())
                };
              }
              return new Response
              {
                ErrorMessage = text,
                Error = true
              };
            }
            return new Response
            {
              ErrorMessage = EcmTexts.Localize("Unable to send the quick test as the message has been sent.", Array.Empty<object>()),
              Error = true
            };
          }
          return new Response
          {
            ErrorMessage = EcmTexts.Localize("Message dispatch is disabled by the system. Change this in the Global Settings menu in EXM or contact your administrator.", Array.Empty<object>()),
            Error = true
          };
        }
        return new Response
        {
          ErrorMessage = EcmTexts.Localize("Edited message could not be found. It may have been moved or deleted by another user.", Array.Empty<object>()),
          Error = true
        };
      }
      catch (Exception ex2)
      {
        _logger.LogError(ex2.Message, ex2);
        return new Response
        {
          ErrorMessage = EcmTexts.Localize("A serious error occurred please contact the administrator", Array.Empty<object>()),
          Error = true
        };
      }
    }

    private string CheckAddressValidation(string emailAddresses)
    {
      ListString listString = new ListString(emailAddresses, ',');
      if (listString.Count == 0)
      {
        return EcmTexts.Localize("'{0}' is not a valid email address. To send the quick test, please enter a valid email address and try again.", string.Empty);
      }
      using (IEnumerator<string> enumerator = (from email in listString
                                               where !_emailRegexValidator.IsValid(email.Trim())
                                               select email).GetEnumerator())
      {
        if (enumerator.MoveNext())
        {
          string current = enumerator.Current;
          return EcmTexts.Localize("'{0}' is not a valid email address. To send the quick test, please enter a valid email address and try again.", current.Trim());
        }
      }
      return string.Empty;
    }

    private List<SendingProcessData> RunQuickTest(string recipientEmailAddresses, IEnumerable<string> variantIds, MessageItem messageItem, AbnTest abnTest)
    {
      List<SendingProcessData> sentData = new List<SendingProcessData>();
      if (abnTest == null || abnTest.TestCandidates.Count < 2)
      {
       sentData.Add(SendMessage(recipientEmailAddresses, messageItem, null));
      }
      else
      {
        foreach (string variantId in variantIds)
        {
          Item item = abnTest.TestCandidates.FirstOrDefault((Item x) => x.ID == new ID(variantId));
          if (item != null)
          {
            sentData.Add(SendMessage(recipientEmailAddresses, messageItem, item));
          }
        }
      }

      return sentData;
    }

    private SendingProcessData SendMessage(string recipientEmailAddresses, MessageItem messageItem, Item variantTargetItem)
    {
      MessageItem messageItem2 = messageItem.Clone() as MessageItem;
      Assert.IsNotNull(messageItem2, "Cannot clone messageItem");
     
        WebPageMail webPageMail;
        if (variantTargetItem != null && (webPageMail = (messageItem2 as WebPageMail)) != null)
        {
          webPageMail.TargetItem = variantTargetItem;
        }

       return _sendingManagerFactory.GetSendingManager(messageItem2).SendTestMessage(recipientEmailAddresses, 1);
      
    }
  }
}