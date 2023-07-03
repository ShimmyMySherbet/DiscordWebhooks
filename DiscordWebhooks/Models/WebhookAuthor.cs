using Newtonsoft.Json;

namespace ShimmyMySherbet.DiscordWebhooks.Models
{
	public class WebhookAuthor
	{
		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("url")]
		public string Url { get; set; }

		[JsonProperty("icon_url")]
		public string IconURL { get; set; }
	}
}
