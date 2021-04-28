﻿using System.Collections;
using System.Text.RegularExpressions;

namespace ProtoSim.DotNetUtilities {
    /// <summary>
    /// Contains extension methods for the string Type, adding functionality
    /// </summary>
    public static class Strings {
        /// <summary>
        /// Checks if <paramref name="emailAddress"/> is a valid email address value
        /// </summary>
        /// <remarks>A NULL string object will return False</remarks>
        /// <param name="emailAddress">The string value to check</param>
        /// <returns>True if valid or False if invalid</returns>
        public static bool IsValidEmailAddress(this string emailAddress) {
            if (string.IsNullOrEmpty(emailAddress))
                return false;

            return Regex.IsMatch(emailAddress, @"^[a-z0-9]+([-_.]{1}[a-z0-9]+)*@[a-z0-9]+([-]{1}[a-z0-9]+)*[.][a-z]{2,}");
        }

        /// <summary>
        /// Checks if <paramref name="guid"/> is a valid Guid value
        /// </summary>
        /// <remarks>A NULL string object will return False</remarks>
        /// <param name="guid">The string value to check</param>
        /// <returns>True if valid or False if invalid</returns>
        public static bool IsValidGuid(this string guid) {
            if (string.IsNullOrEmpty(guid))
                return false;

            return Regex.IsMatch(guid, @"\w{8}-(\w{4}-){3}\w{12}");
        }

        /// <summary>
        /// Converts the given <paramref name="item"/> to a plain string, taking into account its Type
        /// </summary>
        /// <remarks>Will return the standard object.ToString() result if unsupported Type</remarks>
        /// <param name="item">The item to convert</param>
        /// <returns>A string object representing <paramref name="item"/></returns>
        public static string ToPlainString(this object item) {
            if (item == null)
                return null;

            if (item is IList) {
                var itemList = item as IList;
                var plainString = "[";

                for (int i = 0; i < itemList.Count; i++) {
                    plainString += itemList[i].ToPlainString();

                    if (i < itemList.Count - 1)
                        plainString += ",";
                }

                return plainString + "]";
            }

            switch (item.GetType()) {
                default:
                    return item.ToString();
            }
        }
    }
}