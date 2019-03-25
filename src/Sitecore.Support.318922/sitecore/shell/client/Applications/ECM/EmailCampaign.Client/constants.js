define([], function () {
  return {
    MessageStates: {
      DRAFT: 0,
      DISPATCHSCHEDULED: 1,
      SENDING: 2,
      SENT: 3,
      INACTIVE: 4,
      ACTIVATIONSCHEDULED: 5,
      ACTIVE: 6,
      QUEUING: 7
    },
    MessageTypes: {
      AUTOMATED: "Automated"
    },
    SendingStates: {
      UNDEFINED: 0,
      INITIALIZATION: 1,
      QUEUING: 2,
      SENDING: 3,
      PAUSED: 4,
      FINISHING: 5,
      FINISHED: 6
    },
    JsResourcePrefixes: {
        EXM: "/-/speak/v1/ecm/"
    },
    ServerRequestPrefix: {
        EXM: "/sitecore/api/ssc/"
    },
    ServerRequests: {
        ADD_MESSAGE_VARIANT: "EXM/AddMessageVariant",
        ADD_PRE_EXISTING_PAGE: "EXM/AddPreExistingPage",
        ADD_RECIPIENT_LIST: "EXM/AddRecipientList",
        CAN_CREATE_NEW_MESSAGE: "EXM/CanCreateNewMessage",
        CAN_DELETE_FOLDER: "EXM/CanDeleteFolder",
        CAN_DELETE_MESSAGE: "EXM/CanDeleteMessage",
        CAN_IMPORT_HTML: "EXM/CanImportHtml",
        CAN_SAVE_DEFAULT_SETTINGS: "EXM/CanSaveDefaultSettings",
        CAN_SAVE_SUBSCRIPTION_TEMPLATE: "EXM/CanSaveSubscriptionTemplate",
        CHECK_PERMISSIONS: "EXM/CheckPermissions",
        CHECK_RECIPIENT_LISTS: "EXM/CheckRecipientLists",
        COPY_TO_DRAFT: "EXM/CopyToDraft",
        CREATE_NEW_MESSAGE: "EXM/CreateNewMessage",
        CREATE_REPORT_REPEATER_ITEM: "EXM/CreateReportRepeaterItem",
        CURRENT_STATE: "EXM/CurrentState",
        DELETE_FOLDER: "EXM/DeleteFolder",
        DELETE_MESSAGE: "EXM/DeleteMessage",
        DUPLICATE_MESSAGE_VARIANT: "EXM/DuplicateMessageVariant",
        EMAIL_CHANNEL_PERFORMANCE_REPORT_KEY: "EXM/EmailChannelPerformanceReportKey",
        ENGAGEMENT_PLAN_URL: "EXM/EngagementPlanUrl",
        EXPERIENCE_ANALYTICS_KEY: 'EXM/ExperienceAnalyticsKey',
        EXECUTE_SEND_QUICK_TEST: "EXM/ExecuteSendQuickTest",
        FIRST_USAGE: "EXM/FirstUsage",
        IMPORT_HTML: "EXM/ImportHtml",
        LOAD_DEFAULT_SETTINGS: "EXM/LoadDefaultSettings",
        LIST_MANAGEMENT_CONTACT_LIST: "ListManagement/ContactList",
        LIST_MANAGEMENT_LIST: "ListManagement/List",
        MESSAGE_INFO: "EXM/MessageInfo",
        MESSAGE_PREVIEW_URL: "EXM/MessagePreviewUrl",
        MESSAGE_URL: "EXM/MessageUrl",
        PUBLISH_STATISTICS: 'EXM/PublishStatistics',
        RECIPIENT_LISTS: "EXM/RecipientLists",
        REMOVE_ATTACHMENT: "EXM/RemoveAttachment",
        REMOVE_BOUNCED_CONTACTS: "EXM/RemoveBouncedContacts",
        REMOVE_COMPLAINED_CONTACTS: "EXM/RemoveComplainedContacts",
        REMOVE_MESSAGE_VARIANT: "EXM/RemoveMessageVariant",
        REMOVE_REPORT_REPEATER_ITEM: "EXM/RemoveReportRepeaterItem",
        REMOVE_UNSUBSCRIBED_CONTACTS: "EXM/RemoveUnsubscribedContacts",
        REPORT_KEY: "EXM/ReportKey",
        REVOME_RECIPIENT_LIST: "EXM/RemoveRecipientList",
        SAVE_DEFAULT_SETTINGS: "EXM/SaveDefaultSettings",
        SAVE_MESSAGE: "EXM/SaveMessage",
        SWITCH_LANGUAGE: "EXM/SwitchLanguage",
        VALIDATE_DEFAULT_SETTINGS: "EXM/ValidateDefaultSettings",
        VALIDATE_PAGE_PATH: "EXM/ValidatePagePath",
        VALIDATE_SELECTED_CLIENTS: "EXM/ValidateSelectedClients",
        VALIDATE_SENDER: "EXM/ValidateSender",
        VALIDATE_EMAIL_ADDRESS: 'EXM/ValidateEmailAddress',
        VALIDATE_FROM_REPLY_TO_EMAIL_ADDRESS: 'EXM/ValidateFromReplyToEmailAddress',
        VALIDATE_BROKEN_LINKS: 'EXM/ValidateBrokenLinks'
    },
    Reporting: {
        Events: {
            OPEN: 1,
            CLICK: 4,
            FIRST_CLICK: 8,
            UNSUBSCRIBE: 16,
            BOUNCE: 32,
            SENT: 64,
            SPAM: 128,
            FAILED: 256
        }
    },
    URLs: {
        MessagesRegular: "/sitecore/client/Applications/ECM/Pages/Messages/Regular",
        MessagesAutomated: "/sitecore/client/Applications/ECM/Pages/Messages/Automated",
        MessagesSubscription: "/sitecore/client/Applications/ECM/Pages/Messages/Subscription",
        MessageReport: '/sitecore/client/Applications/ECM/Pages/CampaignReport',
        Dashboard: "/sitecore/client/Applications/ECM/Pages/Dashboard",
        EdsDomains: '/sitecore/client/Applications/EDS/Pages/Domains'
    }
  }
});