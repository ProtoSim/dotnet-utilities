using System.Text.RegularExpressions;

namespace ProtoSim.DotNetUtilities {
    public static class Strings {
        /// <summary>
        /// Checks the given string to determine if it is a valid Guid value
        /// </summary>
        /// <remarks>Null string objects will return False</remarks>
        /// <param name="guid">The string value to check</param>
        /// <returns>returns True if valid or False if invalid</returns>
        public static bool IsValidGuid(this string guid) {
            if (string.IsNullOrEmpty(guid))
                return false;

            return Regex.IsMatch(guid, @"\w{8}-(\w{4}-){3}\w{12}");
        }
    }
}