using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Common.Core.Enumerations
{
    public enum SlackMessageTypeEnum
    {
        [Description("#5cb85c")]
        success = 0,
        [Description("#f0ad4e")]
        warning = 1,
        [Description("#d9534f")]
        danger = 2,
        [Description("#5bc0de")]
        info = 3,
        [Description("#337ab7")]
        primary = 4
    }

    public static class SlackAttachmentColorHelper
    {
        /// <summary>
        /// Gets an attribute on an enum field value
        /// </summary>
        /// <typeparam name="T">The type of the attribute you want to retrieve</typeparam>
        /// <param name="enumVal">The enum value</param>
        /// <returns>The attribute of type T that exists on the enum value</returns>
        /// <example>string desc = myEnumVariable.GetAttributeOfType<DescriptionAttribute>().Description;</example>
        public static T GetAttributeOfType<T>(this Enum enumVal) where T : System.Attribute
        {
            var type = enumVal.GetType();
            var memInfo = type.GetMember(enumVal.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
            return (attributes.Length > 0) ? (T)attributes[0] : null;
        }
    }
}
