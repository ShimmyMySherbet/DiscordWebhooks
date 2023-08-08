using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using ShimmyMySherbet.DiscordWebhooks.Models.Interfaces;

namespace ShimmyMySherbet.DiscordWebhooks.Models
{
    /// <summary>
    /// Object representing 
    /// </summary>
    public class WebhookAttachment
    {
        /// <summary>
        /// Attachment index. Used as a reference between JSON payload attachment objects, and the attachment's form data.
        /// </summary>
        [JsonProperty("id")]
        public int ID { get; set; }

        /// <summary>
        /// Attachment Filename
        /// </summary>
        /// <remarks>
        /// This is not the attachment file path, this is the display filename of the attachment
        /// </remarks>
        [JsonProperty("filename")]
        public string FileName { get; set; }

        /// <summary>
        /// Optional file description for Screen Readers
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Attachment file data source
        /// </summary>
        [JsonIgnore]
        public IAttachmentContent Content { get; set; }

        /// <summary>
        /// Fetches the attachment content from <seealso cref="Content"/>, and formats its headers for Discord
        /// </summary>
        /// <returns>Formatted attachment content ready to post to Discord</returns>
        public HttpContent FormatContent()
        {
            var content = Content.GetContent();
            content.Headers.ContentType = null;
            content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data");
            content.Headers.ContentDisposition.Name = $"files[{ID}]";
            content.Headers.ContentDisposition.FileName = FileName;
            return content;
        }
    }
}
