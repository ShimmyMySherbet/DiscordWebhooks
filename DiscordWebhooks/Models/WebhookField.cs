using Newtonsoft.Json;

namespace ShimmyMySherbet.DiscordWebhooks.Models
{
	public class WebhookField
	{
		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("value")]
		public string Value { get; set; }

		[JsonProperty("inline")]
		public bool Inline { get; set; }
	}
}
