using System.Collections.Generic;
using Newtonsoft.Json;

namespace ShimmyMySherbet.DiscordWebhooks.Models
{
	/// <summary>
	/// Represents what users/roles a message is allowed to ping
	/// </summary>
	public class WebhookAllowedMentions
	{
		/// <summary>
		/// Ping-able category permissions
		/// </summary>
		[JsonProperty("parse")]
		public List<string> ParseFlags { get; set; } = new List<string>();

		/// <summary>
		/// List of role IDs for ping-able roles
		/// </summary>
		[JsonProperty("roles")]
		public List<ulong> Roles { get; set; } = new List<ulong>();

		/// <summary>
		/// List of user IDs for ping-able users
		/// </summary>
		[JsonProperty("users")]
		public List<ulong> Users { get; set; } = new List<ulong>();

		/// <summary>
		/// Flag specifying of the replied user is allowed to be pinged
		/// </summary>
		[JsonProperty("replied_user")]
		public bool RepliedUser { get; set; } = false;
	}
}
