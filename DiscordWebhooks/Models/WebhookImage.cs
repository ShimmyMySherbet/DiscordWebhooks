using Newtonsoft.Json;

namespace ShimmyMySherbet.DiscordWebhooks.Models
{
	/// <summary>
	/// Represents an embedded image in a Discord Embed
	/// </summary>
	public class WebhookImage
	{
		/// <summary>
		/// Image URI to display
		/// </summary>
		[JsonProperty("url")]
		public string ImageURL { get; set; }
	}
}
