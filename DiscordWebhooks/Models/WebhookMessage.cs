using System.Collections.Generic;
using Newtonsoft.Json;

namespace ShimmyMySherbet.DiscordWebhooks.Models
{
	public class WebhookMessage
	{
		[JsonProperty("username")]
		public string Username { get; set; }

		[JsonProperty("avatar_url")]
		public string AvatarURL { get; set; }

		[JsonProperty("content")]
		public string Content { get; set; }

		[JsonProperty("embeds")]
		public List<WebhookEmbed> Embeds { get; set; } = new List<WebhookEmbed>();

		[JsonProperty("tts")]
		public bool TTS { get; set; }

		public WebhookMessage WithEmbed(WebhookEmbed embed)
		{
			Embeds.Add(embed);
			return this;
		}

		public WebhookEmbed PassEmbed()
		{
			var embed = new WebhookEmbed(this);
			Embeds.Add(embed);
			return embed;
		}

		public WebhookMessage WithUsername(string un)
		{
			Username = un;
			return this;
		}

		public WebhookMessage WithAvatar(string avatar)
		{
			AvatarURL = avatar;
			return this;
		}

		public WebhookMessage WithContent(string message)
		{
			Content = message;
			return this;
		}

		public WebhookMessage WithTTS()
		{
			TTS = true;
			return this;
		}
	}
}
