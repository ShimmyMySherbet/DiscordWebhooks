using System;
using Newtonsoft.Json;

namespace ShimmyMySherbet.DiscordWebhooks.Models.Response
{
	/// <summary>
	/// Represents the author part of a webhook message response
	/// </summary>
	public class ResponseAuthor
	{
		/// <summary>
		/// The url to the webhook's avatar, or <see langword="null"/>
		/// </summary>
		[JsonProperty("avatar")]
		public string Avatar { get; set; }

		/// <summary>
		/// The user ID attributed to the webhook user
		/// </summary>
		[JsonProperty("id")]
		public long ID { get; set; }

		/// <summary>
		/// The display username of the webhook user.
		/// </summary>
		/// <remarks>
		/// This will either be the username set in the webhook message, or the default username for the webhook.
		/// </remarks>
		[JsonProperty("username")]
		public string Username { get; set; }
	}
}
