using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IMS.Service.WebAPI2.Services
{
    public static class MessageService
    {
        private static string defaultLocale = "en";

        public static string GetMessage(string resourceName, string locale)
        {
            string errorMsg = "";

            errorMsg = string.IsNullOrEmpty(Messages.ResourceManager.GetString(resourceName + locale)) ?
                    Messages.ResourceManager.GetString(resourceName + defaultLocale) :
                    Messages.ResourceManager.GetString(resourceName + locale);

            return string.IsNullOrEmpty(errorMsg) ? "Error" : errorMsg;
        }
    }
}