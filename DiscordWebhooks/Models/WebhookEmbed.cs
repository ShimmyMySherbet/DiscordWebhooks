using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using ShimmyMySherbet.DiscordWebhooks.Embeded;

namespace ShimmyMySherbet.DiscordWebhooks.Models
{
	public class WebhookEmbed
	{
		[JsonProperty("color")]
		public int ColorCode { get; set; }

		[JsonProperty("author")]
		public WebhookAuthor Author { get; set; }

		[JsonProperty("title")]
		public string Title { get; set; }

		[JsonProperty("url")]
		public string Url { get; set; }

		[JsonProperty("description")]
		public string Description { get; set; }

		[JsonProperty("fields")]
		public List<WebhookField> Fields { get; set; } = new List<WebhookField>();

		[JsonProperty("image")]
		public WebhookImage Image { get; set; }

		[JsonProperty("thumbnail")]
		public WebhookImage Thumbnail { get; set; }

		[JsonProperty("footer")]
		public WebhookFooter Footer { get; set; }

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

		public WebhookMessage Finalize()
		{
			if (parent == null)
			{
				parent = new WebhookMessage() { Embeds = new List<WebhookEmbed>() { this } };
			}
			return parent;
		}

		public WebhookEmbed WithTitle(string title)
		{
			Title = title;
			return this;
		}

		public WebhookEmbed WithURL(string value)
		{
			Url = value;
			return this;
		}

		public WebhookEmbed WithDescription(string value)
		{
			Description = value;
			return this;
		}

		public WebhookEmbed WithTimestamp(DateTime value)
		{
			TimestampStr = DiscordHelpers.DateTimeToISO(value.ToLocalTime());
			return this;
		}

		public WebhookEmbed WithField(string name, string value, bool inline = true)
		{
			Fields.Add(new WebhookField() { Value = value, Inline = inline, Name = name });
			return this;
		}

		public WebhookEmbed WithImage(string imageUrl)
		{
			Image = new WebhookImage() { ImageURL = imageUrl };
			return this;
		}

		public WebhookEmbed WithThumbnail(string imageUrl)
		{
			Thumbnail = new WebhookImage() { url = imageUrl };
			return this;
		}

		public WebhookEmbed WithAuthor(string name, string url = null, string icon = null)
		{
			Author = new WebhookAuthor() { Name = name, IconURL = icon, Url = url };
			return this;
		}

		public WebhookEmbed WithColor(EmbedColor color)
		{
			ColorCode = color.ToColorCode();
			return this;
		}
	}
}
