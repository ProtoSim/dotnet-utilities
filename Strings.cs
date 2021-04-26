using System.Text.RegularExpressions;

namespace ProtoSim.DotNetUtilities {
    public static class Strings {
        /// <summary>
        /// Checks the given string to determine if it is a valid email address value
        /// </summary>
        /// <remarks>A NULL string object will return False</remarks>
        /// <param name="emailAddress">The string value to check</param>
        /// <returns>returns True if valid or False if invalid</returns>
        public static bool IsValidEmailAddress(this string emailAddress) {
            if (string.IsNullOrEmpty(emailAddress))
                return false;

            return Regex.IsMatch(emailAddress, @"^[a-z0-9]+([-_.]{1}[a-z0-9]+)*@[a-z0-9]+([-]{1}[a-z0-9]+)*[.][a-z]{2,}");
        }

        /// <summary>
        /// Checks the given string to determine if it is a valid Guid value
        /// </summary>
        /// <remarks>A NULL string object will return False</remarks>
        /// <param name="guid">The string value to check</param>
        /// <returns>returns True if valid or False if invalid</returns>
        public static bool IsValidGuid(this string guid) {
            if (string.IsNullOrEmpty(guid))
                return false;

            return Regex.IsMatch(guid, @"\w{8}-(\w{4}-){3}\w{12}");
        }
    }
}