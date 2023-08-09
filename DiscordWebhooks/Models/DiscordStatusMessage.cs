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
		/// Specifies how long to wait before attemping to send the message again
		/// </summary>
		[JsonProperty("retry_after")]
		public float RetryAfter { get; set; } = 0f;

		/// <summary>
		/// The HTTP status message provided with this response
		/// </summary>
		[JsonIgnore]
		public string Status { get; set; }
	}
}