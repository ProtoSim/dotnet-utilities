using System.Collections;
using System.Numerics;
using System.Text.RegularExpressions;

namespace ProtoSim.DotNetUtilities {
    /// <summary>
    /// Contains extension methods for the string Type, adding functionality
    /// </summary>
    public static class Strings {
        /// <summary>
        /// Checks if <paramref name="emailAddress"/> is a valid email address value
        /// </summary>
        /// <param name="emailAddress">The <c>string</c> value to check</param>
        /// <returns><c>true</c> if valid or <c>false</c> if invalid</returns>
        public static bool IsValidEmailAddress(this string emailAddress) {
            if (string.IsNullOrEmpty(emailAddress))
                return false;

            return Regex.IsMatch(emailAddress, @"^[a-z0-9]+([-_.]{1}[a-z0-9]+)*@[a-z0-9]+([-]{1}[a-z0-9]+)*[.][a-z]{2,}");
        }

        /// <summary>
        /// Checks if <paramref name="guid"/> is a valid Guid value
        /// </summary>
        /// <param name="guid">The <c>string</c> value to check</param>
        /// <returns><c>true</c> if valid or <c>false</c> if invalid</returns>
        public static bool IsValidGuid(this string guid) {
            if (string.IsNullOrEmpty(guid))
                return false;

            return Regex.IsMatch(guid, @"\w{8}-(\w{4}-){3}\w{12}");
        }

        /// <summary>
        /// Converts the given <paramref name="item"/> to a plain string, taking into account its <c>Type</c>
        /// </summary>
        /// <remarks>Will return the standard <c>object.ToString()</c> result if unsupported Type</remarks>
        /// <param name="item">The item to convert</param>
        /// <param name="parenthesesInsteadOfCarets">Use parentheses instead of carets for applicable Types</param>
        /// <returns>A <c>string</c> object representing <paramref name="item"/></returns>
        public static string ToPlainString(this object item, bool parenthesesInsteadOfCarets = false) {
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

            switch (item) {
                case Vector3 vector3:
                    return $"{(parenthesesInsteadOfCarets ? "(" : "<")}{vector3.X},{vector3.Y},{vector3.Z}{(parenthesesInsteadOfCarets ? ")" : ">")}";

                default:
                    return item.ToString();
            }
        }
    }
}