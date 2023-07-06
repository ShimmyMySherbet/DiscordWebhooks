using Newtonsoft.Json;

namespace ShimmyMySherbet.DiscordWebhooks.Models
{
	/// <summary>
	/// Represents a status message from discord on an error
	/// </summary>
	public class DiscordStatusMessage
	{
		/// <summary>
		/// Error code for this status
		/// </summary>
		[JsonProperty("CODE")]
		public int Code { get; set; }

		/// <summary>
		/// The message attached to this response
		/// </summary>
		[JsonProperty("message")]
		public string Message { get; set; }

		/// <summary>
		/// The HTTP status message provided with this response
		/// </summary>
		[JsonIgnore]
		public string Status { get; set; }
	}
}