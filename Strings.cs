using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;
using System.Text.RegularExpressions;

namespace ProtoSim.DotNetUtilities {
    /// <summary>
    /// Provides extension methods for the string Type, adding functionality
    /// </summary>
    public static class Strings {
        /// <summary>
        /// Checks if <paramref name="emailAddress"/> is a valid email address value
        /// </summary>
        /// <param name="emailAddress">The <c>string</c> value to check</param>
        /// <returns><c>true</c> if valid or <c>false</c> if invalid</returns>
        public static bool IsValidEmailAddress([NotNullWhen(true)] this string? emailAddress) {
            if (string.IsNullOrEmpty(emailAddress))
                return false;

            return Regex.IsMatch(emailAddress, @"^[a-z0-9]+([-_.]{1}[a-z0-9]+)*@[a-z0-9]+([-]{1}[a-z0-9]+)*[.][a-z]{2,}");
        }

        /// <summary>
        /// Checks if <paramref name="guid"/> is a valid Guid value
        /// </summary>
        /// <param name="guid">The <c>string</c> value to check</param>
        /// <returns><c>true</c> if valid or <c>false</c> if invalid</returns>
        public static bool IsValidGuid([NotNullWhen(true)] this string? guid) {
            if (string.IsNullOrEmpty(guid))
                return false;

            return Regex.IsMatch(guid, @"\w{8}-(\w{4}-){3}\w{12}");
        }

        /// <summary>
        /// Converts the given <paramref name="item"/> to a plain string, taking into account its <c>Type</c>
        /// </summary>
        /// <remarks>Will return the standard <c>object.ToString()</c> result if unsupported Type</remarks>
        /// <param name="item">The item to convert</param>
        /// <param name="encapsulationCharacters">Custom encapsulation characters to use instead of default</param>
        /// <returns>A <c>string</c> object representing <paramref name="item"/></returns>
        public static string ToPlainString(this object? item, char[]? encapsulationCharacters = null) {
            if (item == null)
                return string.Empty;

            if (item is IList itemList) {
                var plainString = $"{encapsulationCharacters?[0] ?? '['}";

                for (int i = 0; i < itemList.Count; i++) {
                    plainString += itemList[i].ToPlainString();

                    if (i < itemList.Count - 1)
                        plainString += ",";
                }

                return $"{plainString + (encapsulationCharacters?[1] ?? ']')}";
            }

            return item switch {
                Vector3 vector3 => $"{encapsulationCharacters?[0] ?? '<'}{vector3.X},{vector3.Y},{vector3.Z}{encapsulationCharacters?[1] ?? '>'}",
                _ => item.ToString() ?? string.Empty,
            };
        }
    }
}