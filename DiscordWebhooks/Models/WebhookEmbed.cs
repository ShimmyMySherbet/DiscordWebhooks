using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using ShimmyMySherbet.DiscordWebhook.Helpers;

namespace ShimmyMySherbet.DiscordWebhooks.Models
{
    public class WebhookEmbed
    {
        /// <summary>
        /// The color code for the side of this embed
        /// </summary>
        [JsonProperty("color")]
        public int ColorCode { get; set; }

        /// <summary>
        /// The author property of this embed
        /// </summary>
        [JsonProperty("author")]
        public WebhookAuthor Author { get; set; }

        /// <summary>
        /// The title string
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// The hyperlink target of the embed title. See <seealso cref="Title"/>
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }

        /// <summary>
        /// The description component. Seen below the title and before fields.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// List of fields of the embed
        /// </summary>
        [JsonProperty("fields")]
        public List<WebhookField> Fields { get; set; } = new List<WebhookField>();

        /// <summary>
        /// The major image of the embed
        /// </summary>
        [JsonProperty("image")]
        public WebhookImage Image { get; set; }

        /// <summary>
        /// The thumbnail image, seen at the top-right of the embed
        /// </summary>
        [JsonProperty("thumbnail")]
        public WebhookImage Thumbnail { get; set; }

        /// <summary>
        /// The footer of the embed, seen at the very bottom.
        /// </summary>
        [JsonProperty("footer")]
        public WebhookFooter Footer { get; set; }

        /// <summary>
        /// The timestamp attached to the embed, in ISO DateTime format UTC time.
        /// </summary>
        [JsonProperty("timestamp")]
        public string TimestampStr { get; set; }

        [JsonIgnore]
        private WebhookMessage parent;

        internal WebhookEmbed(WebhookMessage parent)
        {
            this.parent = parent;
        }

        public WebhookEmbed()
        {
        }

        /// <summary>
        /// Returns the parent <see cref="WebhookMessage"/> this embed was created on, or creates one if no parent exists.
        /// </summary>
        /// <returns>Parent <see cref="WebhookMessage"/></returns>
        public WebhookMessage Finalize()
        {
            if (parent == null)
            {
                parent = new WebhookMessage() { Embeds = new List<WebhookEmbed>() { this } };
            }
            return parent;
        }

        /// <summary>
        /// Adds a title string to the embed
        /// </summary>
        /// <param name="title">Title text</param>
        /// <returns>Current instance</returns>
        public WebhookEmbed WithTitle(string title)
        {
            Title = title;
            return this;
        }

        /// <summary>
        /// Adds a hyperlink to the embed title.
        /// </summary>
        /// <param name="url">The url of the link</param>
        /// <returns>Current instance</returns>
        public WebhookEmbed WithURL(string url)
        {
            Url = url;
            return this;
        }

        /// <summary>
        /// Adds a description part to the embed
        /// </summary>
        /// <param name="value">Description text</param>
        /// <returns>Current instance</returns>
        public WebhookEmbed WithDescription(string value)
        {
            Description = value;
            return this;
        }

        /// <summary>
        /// Adds a timestamp to the embed.
        /// </summary>
        /// <param name="value">The date time to attach</param>
        /// <param name="convertToUTC">when <see langword="true"/>, <paramref name="value"/> is converted to UTC time before. Set this to false if your time is already in UTC</param>
        /// <returns>Current Instance</returns>
        public WebhookEmbed WithTimestamp(DateTime value, bool convertToUTC = true)
        {
            TimestampStr = DiscordHelpers.DateTimeToISO(convertToUTC ? value.ToUniversalTime() : value);
            return this;
        }

        /// <summary>
        /// Adds a footer to the embed
        /// </summary>
        /// <param name="value">The text content of the footer</param>
        /// <param name="iconURL">Optional icon URL</param>
        /// <returns>Current Instance</returns>
        public WebhookEmbed WithFooter(string value, string iconURL = null)
        {
            if (Footer == null)
                Footer = new WebhookFooter();

            Footer.Text = value;

            if (iconURL != null)
                Footer.IconURL = iconURL;

            return this;
        }


        /// <summary>
        /// Uploads an image to use as the footer icon
        /// </summary>
        /// <param name="fileName">Path to the image file to upload</param>
        /// <returns>Current Instance</returns>
        public WebhookEmbed UploadFooterIcon(string fileName)
        {
            var parent = Finalize();
            parent.WithAttachment(fileName);

            var name = Path.GetFileName(fileName);

            if (Footer == null)
                Footer = new WebhookFooter();

            Footer.IconURL = $"attachment://{name}";

            return this;
        }


        /// <summary>
        /// Uploads an image to use as the footer icon
        /// </summary>
		/// <remarks>
		/// Attempts to auto-detect image format based on <paramref name="imageData"/>'s contents. To manually specify, use <seealso cref="UploadFooterIcon(byte[], string)"/>
		/// </remarks>
        /// <param name="fileName">Path to the image file to upload</param>
        /// <returns>Current Instance</returns>
        public WebhookEmbed UploadFooterIcon(byte[] imageData)
        {
            var fileType = FileTypeDetector.Detect(imageData);

            if (fileType == Enums.EFileType.Unknown)
                fileType = Enums.EFileType.Png;

            var ext = fileType.ToString().ToLower();

            var parent = Finalize();


            var name = $"icon{parent.Attachments.Count}.{ext}";

            parent.WithAttachment(imageData, name);

            if (Footer == null)
                Footer = new WebhookFooter();

            Footer.IconURL = $"attachment://{name}";

            return this;
        }

        /// <summary>
        /// Uploads an image to use as the footer icon, with the specified suggested file name
        /// </summary>
        /// <param name="imageData">Image data to upload</param>
        /// <param name="fileName">A suggested file name for the uploaded image</param>
        /// <returns>Current Instance</returns>
        public WebhookEmbed UploadFooterIcon(byte[] imageData, string fileName)
        {
            var parent = Finalize();

            parent.WithAttachment(imageData, fileName);

            if (Footer == null)
                Footer = new WebhookFooter();

            Footer.IconURL = $"attachment://{fileName}";

            return this;
        }


        /// <summary>
        /// Adds a field to the embed.
        /// </summary>
        /// <param name="name">Field key/name</param>
        /// <param name="value">Field value</param>
        /// <param name="inline">Field in-lining with the previous field</param>
        /// <returns>Current Instance</returns>
        public WebhookEmbed WithField(string name, object value, bool inline = true)
        {
            Fields.Add(new WebhookField() { Value = value?.ToString() ?? string.Empty, Inline = inline, Name = name });
            return this;
        }

        /// <summary>
        /// Sets the image of the embed
        /// </summary>
        /// <param name="imageUrl">Image URI</param>
        /// <returns>Current instance</returns>
        public WebhookEmbed WithImage(string imageUrl)
        {
            Image = new WebhookImage() { ImageURL = imageUrl };
            return this;
        }

        /// <summary>
        /// Uploads an image to use as the image for this embed.
        /// </summary>
        /// <remarks>
        /// Attempts to auto-detect image format based on <paramref name="imageData"/>'s contents. To manually specify, use <seealso cref="UploadImage(byte[], string)"/>
        /// </remarks>
        /// <param name="fileName">File path to the image to upload. Supports .png, .jpg, .jpeg, .webp, .gif file formats</param>
        /// <returns>Current Instance</returns>
        public WebhookEmbed UploadImage(string fileName)
        {
            var parent = Finalize();

            parent.WithAttachment(fileName);

            Image = new WebhookImage()
            {
                ImageURL = $"attachment://{Path.GetFileName(fileName)}"
            };

            return this;
        }

        /// <summary>
        /// Uploads an image to use as the image for this embed.
        /// </summary>
        /// <remarks>
        /// Attempts to auto-detect image format based on <paramref name="imageData"/>'s contents. To manually specify, use <seealso cref="UploadImage(byte[], string)"/>
        /// </remarks>
        /// <param name="fileName">Path to the image file to upload</param>
        /// <returns>Current Instance</returns>
        public WebhookEmbed UploadImage(byte[] imageData)
        {
            var fileType = FileTypeDetector.Detect(imageData);

            if (fileType == Enums.EFileType.Unknown)
                fileType = Enums.EFileType.Png;

            var ext = fileType.ToString().ToLower();

            var parent = Finalize();


            var name = $"image{parent.Attachments.Count}.{ext}";

            parent.WithAttachment(imageData, name);

            Image = new WebhookImage()
            {
                ImageURL = $"attachment://{name}"
            };

            return this;
        }

        /// <summary>
        /// Uploads an image to use as the image for this embed
        /// </summary>
        /// <param name="imageData">Image data to upload. Supports png, jpg, webp, and gif formats</param>
        /// <param name="fileName">A suggested file name for the uploaded image</param>
        /// <returns>Current Instance</returns>
        public WebhookEmbed UploadImage(byte[] imageData, string fileName)
        {
            var parent = Finalize();

            parent.WithAttachment(imageData, fileName);

            Image = new WebhookImage()
            {
                ImageURL = $"attachment://{fileName}"
            };

            return this;
        }

        /// <summary>
        /// Sets the thumbnail of the embed
        /// </summary>
        /// <param name="imageUrl">Image URI</param>
        /// <returns>Current instance</returns>
        public WebhookEmbed WithThumbnail(string imageUrl)
        {
            Thumbnail = new WebhookImage() { ImageURL = imageUrl };
            return this;
        }

        /// <summary>
        /// Uploads an image to use as the thumbnail image for this embed.
        /// </summary>
        /// <remarks>
        /// Attempts to auto-detect image format based on <paramref name="imageData"/>'s contents. To manually specify, use <seealso cref="UploadImage(byte[], string)"/>
        /// </remarks>
        /// <param name="fileName">File path to the image to upload. Supports .png, .jpg, .jpeg, .webp, .gif file formats</param>
        /// <returns>Current Instance</returns>
        public WebhookEmbed UploadThumbnail(string fileName)
        {
            var parent = Finalize();

            parent.WithAttachment(fileName);

            Thumbnail = new WebhookImage()
            {
                ImageURL = $"attachment://{Path.GetFileName(fileName)}"
            };

            return this;
        }


        /// <summary>
        /// Uploads an image to use as the thumbnail image for this embed.
        /// </summary>
        /// <remarks>
        /// Attempts to auto-detect image format based on <paramref name="imageData"/>'s contents. To manually specify, use <seealso cref="UploadImage(byte[], string)"/>
        /// </remarks>
        /// <param name="fileName">Path to the image file to upload</param>
        /// <returns>Current Instance</returns>
        public WebhookEmbed UploadThumbnail(byte[] imageData)
        {
            var fileType = FileTypeDetector.Detect(imageData);

            if (fileType == Enums.EFileType.Unknown)
                fileType = Enums.EFileType.Png;

            var ext = fileType.ToString().ToLower();

            var parent = Finalize();


            var name = $"icon{parent.Attachments.Count}.{ext}";

            parent.WithAttachment(imageData, name);

            Thumbnail = new WebhookImage()
            {
                ImageURL = $"attachment://{name}"
            };

            return this;
        }


        /// <summary>
        /// Uploads an image to use as the thumbnail image for this embed
        /// </summary>
        /// <param name="imageData">Image data to upload. Supports png, jpg, webp, and gif formats</param>
        /// <param name="fileName">A suggested file name for the uploaded image</param>
        /// <returns>Current Instance</returns>
        public WebhookEmbed UploadThumbnail(byte[] imageData, string fileName)
        {
            var parent = Finalize();

            parent.WithAttachment(imageData, fileName);

            Thumbnail = new WebhookImage()
            {
                ImageURL = $"attachment://{fileName}"
            };

            return this;
        }


        /// <summary>
        /// Adds an author field to the embed
        /// </summary>
        /// <param name="name">User display name</param>
        /// <param name="url">Field hyperlink URL</param>
        /// <param name="icon">User avatar URI</param>
        /// <returns>Current instance</returns>
        public WebhookEmbed WithAuthor(string name, string url = null, string icon = null)
        {
            Author = new WebhookAuthor() { Name = name, IconURL = icon, Url = url };
            return this;
        }

        /// <summary>
        /// Sets the side color of the embed
        /// </summary>
        /// <param name="color"><seealso cref="EmbedColor"/> representing the embed color</param>
        /// <returns>current instance</returns>
        public WebhookEmbed WithColor(EmbedColor color)
        {
            ColorCode = color.ToColorCode();
            return this;
        }
    }
}
