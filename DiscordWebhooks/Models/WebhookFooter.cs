using Newtonsoft.Json;

namespace ShimmyMySherbet.DiscordWebhooks.Models
{
	/// <summary>
	/// Represents a Discord embed footer
	/// </summary>
	public class WebhookFooter
	{
		/// <summary>
		/// Display text of the footer
		/// </summary>
		[JsonProperty("text")]
		public string Text { get; set; }

		/// <summary>
		/// Icon URI to display in the footer
		/// </summary>
		[JsonProperty("icon_url")]
		public string IconURL { get; set; }
	}
}
