using Newtonsoft.Json;

namespace ShimmyMySherbet.DiscordWebhooks.Models
{
	public class WebhookImage
	{
		[JsonProperty("url")]
		public string ImageURL { get; set; }
	}
}
