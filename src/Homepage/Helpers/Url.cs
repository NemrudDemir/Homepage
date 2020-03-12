using System;
using System.Collections.Generic;
using System.Linq;

namespace Homepage.Helpers
{
    public static class Url
    {
        private static HashSet<char> _invalidChars = new HashSet<char> {
            '\'', '&'
        };

        /// <summary>
        /// returns escaped text for safe use in url
        /// </summary>
        /// <param name="text">Text to escape</param>
        /// <returns>Escaped text for safe use in url</returns>
        public static string Escape(string text)
        {
            return Uri.EscapeUriString(RemoveInvalidChars(text));
        }

        /// <summary>
        /// returns string with only valid chars for safe use in url
        /// </summary>
        /// <param name="text">Text so remove invalid chars</param>
        /// <returns>String with only valid chars for safe use in url</returns>
        private static string RemoveInvalidChars(string text)
        {
            return new string(text.Where(c => !_invalidChars.Contains(c)).ToArray());
        }
    }
}
