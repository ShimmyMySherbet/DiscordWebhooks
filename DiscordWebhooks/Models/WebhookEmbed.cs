using System;
using System.Collections.Generic;
using Newtonsoft.Json;

// namespace required for backward compatibility
namespace ShimmyMySherbet.DiscordWebhooks.Embeded
{
	public class WebhookEmbed
	{
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
				parent = new WebhookMessage() { embeds = new List<WebhookEmbed>() { this } };
			}
			return parent;
		}

		public int color;

		public WebhookAuthor author;

		public string title;

		public string url;

		public string description;

		public List<WebhookField> fields = new List<WebhookField>();

		public WebhookImage image;

		public WebhookImage thumbnail;

		public WebhookFooter footer;

		public string timestamp;

		public WebhookEmbed WithTitle(string title)
		{
			this.title = title;
			return this;
		}

		public WebhookEmbed WithURL(string value)
		{
			this.url = value;
			return this;
		}

		public WebhookEmbed WithDescription(string value)
		{
			this.description = value;
			return this;
		}

		public WebhookEmbed WithTimestamp(DateTime value)
		{
			timestamp = DiscordHelpers.DateTimeToISO(value.ToLocalTime());
			return this;
		}

		public WebhookEmbed WithField(string name, string value, bool inline = true)
		{
			this.fields.Add(new WebhookField() { value = value, inline = inline, name = name });
			return this;
		}

		public WebhookEmbed WithImage(string value)
		{
			this.image = new WebhookImage() { url = value };
			return this;
		}

		public WebhookEmbed WithThumbnail(string value)
		{
			this.thumbnail = new WebhookImage() { url = value };
			return this;
		}

		public WebhookEmbed WithAuthor(string name, string url = null, string icon = null)
		{
			this.author = new WebhookAuthor() { name = name, icon_url = icon, url = url };
			return this;
		}

		public WebhookEmbed WithColor(EmbedColor color)
		{
			this.color = BitConverter.ToInt32(new byte[4] { color.B, color.G, color.R, 0 }, 0);
			return this;
		}

		// Unity Support

		//public WebhookEmbed WithColor(UnityEngine.Color color)
		//{
		//    byte r = Clamp(color.r);
		//    byte g = Clamp(color.g);
		//    byte b = Clamp(color.b);

		//    int numeric = BitConverter.ToInt32(new byte[4] { b, g, r, 0 }, 0);
		//    this.color = numeric;
		//    return this;
		//}

		private byte Clamp(float a)
		{
			return (byte)(Math.Round(a * 255, 0));
		}
	}
}
