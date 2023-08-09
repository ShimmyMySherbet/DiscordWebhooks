using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ShimmyMySherbet.DiscordWebhooks.Models.Enums;

namespace ShimmyMySherbet.DiscordWebhooks.Helpers
{
    public static class TextUtils
    {
        /// <summary>
        /// Applies the Discord italic effect on text
        /// </summary>
        /// <param name="text">Text to apply effect on</param>
        /// <returns>Italic text</returns>
        public static string Italics(this string text) => $"*{text}*";

        /// <summary>
        /// Applies Discord bold effect on text
        /// </summary>
        /// <param name="text">Text to apply text to</param>
        /// <returns>Bold text</returns>
        public static string Bold(this string text) => $"**{text}**";

        /// <summary>
        /// Applies Discord underline effect on text
        /// </summary>
        /// <param name="text">Text to apply effect to</param>
        /// <returns>Underlined text</returns>
        public static string Underline(this string text) => $"__{text}__";

        /// <summary>
        /// Applies Discord strikethrough effect on text
        /// </summary>
        /// <param name="text">Text value</param>
        /// <returns>Strikethrough text</returns>
        public static string Strikethrough(this string text) => $"~~{text}~~";

        /// <summary>
        /// Applies a single line markdown code block to the specified text
        /// </summary>
        /// <param name="text">Text to apply effect to</param>
        /// <returns>Markdown text</returns>
        public static string SingleCodeBlock(this string text) => $"`{text}`";

        /// <summary>
        /// Applies a markdown spoiler effect to text
        /// </summary>
        /// <param name="text">Text to apply effect to</param>
        /// <returns>Markdown text</returns>
        public static string Spoiler(this string text) => $"||{text}||";

        /// <summary>
        /// Applies the markdown quote effect to text
        /// </summary>
        /// <param name="text">Text to apply the effect to</param>
        /// <returns>Markdown text</returns>
        public static string Quote(this string text)
        {
            var builder = new StringBuilder();

            foreach(var line in text.Split('\n'))
            {
                builder.AppendLine($"> {line}");
            }

            return builder.ToString();
        }

        /// <summary>
        /// Formats text to ping a Discord user by their account ID
        /// </summary>
        /// <param name="userID">User ID</param>
        /// <returns>Mention text to ping the specified user</returns>
        public static string PingUser(this ulong userID) => $"<@{userID}>";

        /// <summary>
        /// Formats text to ping a Discord role by its ID
        /// </summary>
        /// <param name="roleID">The ID of the role to ping</param>
        /// <returns>Mention text to ping the specified role</returns>
        public static string PingRole(this ulong roleID) => $"<&{roleID}>";

        /// <summary>
        /// Formats a masked link for Discord
        /// </summary>
        /// <param name="url">Target URL</param>
        /// <param name="preview">Preview text</param>
        /// <returns>Markdown masked link</returns>
        public static string Hyperlink(this string url, string preview) => $"[{preview}]({url})";

        /// <summary>
        /// Applies markdown heading style to text for Discord
        /// </summary>
        /// <remarks>
        /// Supports multi-line headers
        /// </remarks>
        /// <param name="text">Text to make a header</param>
        /// <param name="level">Value between 0 and 2 representing depth</param>
        /// <param name="depth">Bullet point depth</param>
        /// <returns>Markdown text</returns>
        /// <exception cref="ArgumentException"></exception>
        public static string Heading(this string text, int level = 0, int depth = 0)
        {
            if (level < 0 || level > 2)
            {
                throw new ArgumentException("level must be greater than 0 and less than 3", "level");
            }

            var builder = new StringBuilder();
            
            var spacer = new string('#', level + 1) + new string(' ', depth * 2);
            
            foreach(var line in text.Split('\n'))
            {
                builder.AppendLine($"{spacer} {line}");
            }

            return builder.ToString();
        }

        /// <summary>
        /// Formats a list of values into a markdown bullet point list for Discord
        /// </summary>
        /// <param name="values">Objects representing bullet items</param>
        /// <param name="style">Bullet point style</param>
        /// <returns>Markdown text</returns>
        public static string BulletPoints(IEnumerable<object> values, EBulletPointStyle style = EBulletPointStyle.Bullet)
        {
            var builder = new StringBuilder();
            var index = 1;

            using(var enumerator  = values.GetEnumerator())
            {
                while(enumerator.MoveNext())
                {
                    builder.AppendLine($"{(style == EBulletPointStyle.Numbered ? $"{index++}." : "*")} {enumerator.Current}");
                }
            }

            return builder.ToString();
        }
    }
}
