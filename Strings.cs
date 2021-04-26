using System.Text.RegularExpressions;

namespace ProtoSim.DotNetUtilities {
    public static class Strings {
        public static bool IsValidGuid(this string guid) {
            if (string.IsNullOrEmpty(guid))
                return false;

            return Regex.IsMatch(guid, @"\w{8}-(\w{4}-){3}\w{12}");
        }
    }
}