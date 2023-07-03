using Newtonsoft.Json;

namespace ShimmyMySherbet.DiscordWebhooks.Models
{
	/// <summary>
	/// Represents a field of a Discord message embed
	/// </summary>
	public class WebhookField
	{
		/// <summary>
		/// The field name/key
		/// </summary>
		[JsonProperty("name")]
		public string Name { get; set; }

		/// <summary>
		/// The field value
		/// </summary>
		[JsonProperty("value")]
		public string Value { get; set; }

		/// <summary>
		/// Sets in-lining with the previous field
		/// </summary>
		[JsonProperty("inline")]
		public bool Inline { get; set; }
	}
}
