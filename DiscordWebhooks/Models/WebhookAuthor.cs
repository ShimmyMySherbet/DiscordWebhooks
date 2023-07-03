using Newtonsoft.Json;

namespace ShimmyMySherbet.DiscordWebhooks.Models
{
	/// <summary>
	/// Represents a webhook author field. Seen at the top of embeds
	/// </summary>
	public class WebhookAuthor
	{
		/// <summary>
		/// The username of the user
		/// </summary>
		[JsonProperty("name")]
		public string Name { get; set; }

		/// <summary>
		/// The hyperlink of the user
		/// </summary>
		[JsonProperty("url")]
		public string Url { get; set; }

		/// <summary>
		/// The profile image url for the user
		/// </summary>
		[JsonProperty("icon_url")]
		public string IconURL { get; set; }
	}
}
