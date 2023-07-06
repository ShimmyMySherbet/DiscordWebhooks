using System;

namespace ShimmyMySherbet.DiscordWebhooks.Models.Exceptions
{
	/// <summary>
	/// Exception representing an error from Discord's Webhook API
	/// </summary>
	public class DiscordException : Exception
	{
		/// <summary>
		/// The core information returned by the Discord API
		/// </summary>
		public DiscordStatusMessage Content { get; }

		/// <summary>
		/// Exception representing an error from Discord's Webhook API
		/// </summary>
		/// <param name="content">Result from the Discord API</param>
		public DiscordException(DiscordStatusMessage content) : base($"[{content.Status}] {content.Message} ({content.Code})")
		{
			Content = content;
		}
	}
}
