using System.Collections.Generic;

// namespace required for backward compatibility
namespace ShimmyMySherbet.DiscordWebhooks.Embeded
{
	public class WebhookMessage
	{
		public string username;

		public string avatar_url;

		public string content = "";

		public List<WebhookEmbed> embeds = new List<WebhookEmbed>();

		public bool tts { get; set; }

		public WebhookMessage WithEmbed(WebhookEmbed embed)
		{
			embeds.Add(embed);
			return this;
		}

		public WebhookEmbed PassEmbed()
		{
			WebhookEmbed embed = new WebhookEmbed(this);
			embeds.Add(embed);
			return embed;
		}

		public WebhookMessage WithUsername(string un)
		{
			username = un;
			return this;
		}

		public WebhookMessage WithAvatar(string avatar)
		{
			avatar_url = avatar;
			return this;
		}

		public WebhookMessage WithContent(string c)
		{
			content = c;
			return this;
		}

		public WebhookMessage WithTTS()
		{
			tts = true;
			return this;
		}
	}

}
