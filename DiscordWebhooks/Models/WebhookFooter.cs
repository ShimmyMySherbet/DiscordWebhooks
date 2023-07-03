using Newtonsoft.Json;

namespace ShimmyMySherbet.DiscordWebhooks.Models
{
	public class WebhookFooter
	{
		[JsonProperty("text")]
		public string Text { get; set; }

		[JsonProperty("icon_url")]
		public string IconURL { get; set; }
	}
}
