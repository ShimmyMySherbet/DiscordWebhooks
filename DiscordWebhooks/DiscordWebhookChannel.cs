using System.Threading;
using System.Threading.Tasks;
using ShimmyMySherbet.DiscordWebhooks.Models;

namespace ShimmyMySherbet.DiscordWebhooks
{
	/// <summary>
	/// An object representing an endpoint for messages. Supports message channels and forms channels with threads.
	/// </summary>
	/// <remarks>
	/// Provides an instance-based method for sending messages to a channel, as an alternative to the static messages provided by <seealso cref="DiscordWebhookService"/>
	/// </remarks>
	public class DiscordWebhookChannel
	{
		/// <summary>
		/// Raised when Discord rate limits the endpoint
		/// </summary>
		public event OnRatelimitArgs OnRatelimit;

		/// <summary>
		/// The base webhook URL to send messages
		/// </summary>
		public string WebhookURL { get; }

		/// <summary>
		/// The ID of the thread, if applicable
		/// </summary>
		public ulong ThreadID { get; }

		/// <summary>
		/// Flag specifying if this channel is a thread, indicating a forms channel
		/// </summary>
		public bool IsThread => ThreadID != 0;

		/// <summary>
		/// Creates a new webhook channel instance
		/// </summary>
		/// <param name="webhookURL">The base webhook URL</param>
		/// <param name="threadID">Thread ID to send messages to for forum channels</param>
		public DiscordWebhookChannel(string webhookURL, ulong threadID = 0)
		{
			WebhookURL = webhookURL;
			ThreadID = threadID;
		}

		/// <summary>
		/// Synchronously posts a new message to discord. Does not wait for the discord API to respond to the message, which is the default behaviour of the discord webhook API.
		/// </summary>
		/// <remarks>
		/// Does not provide any feedback from the API. A 'fire and forget' method.
		/// </remarks>
		/// <param name="message">message to post</param>
		public void FireMessage(WebhookMessage message) =>
			DiscordWebhookService.FireMessage(WebhookURL, message, ThreadID);

		/// <summary>
		/// Synchronously posts a new message to discord. Does not wait for the discord API to respond to the message, which is the default behaviour of the discord webhook API.
		/// </summary>
		/// <remarks>
		/// Does not provide any feedback from the API. A 'fire and forget' method.
		/// </remarks>
		/// <param name="builder">A delegate that builds the new <seealso cref="WebhookMessage"/></param>
		public void FireMessage(WebhookMessageBuilder builder) =>
			DiscordWebhookService.FireMessage(WebhookURL, builder(new WebhookMessage()), ThreadID);

		/// <summary>
		/// Asynchronously posts a new message to discord. Does not wait for the discord API to respond to the message, which is the default behaviour of the discord webhook API.
		/// </summary>
		/// <remarks>
		/// Does not provide any feedback from the API. A 'fire and forget' method.
		/// </remarks>
		/// <param name="message">message to post</param>
		public async Task FireMessageAsync(WebhookMessage message) =>
			await DiscordWebhookService.PostMessageAsync(WebhookURL, message, ThreadID);

		/// <summary>
		/// Asynchronously posts a new message to discord. Does not wait for the discord API to respond to the message, which is the default behaviour of the discord webhook API.
		/// </summary>
		/// <remarks>
		/// Does not provide any feedback from the API. A 'fire and forget' method.
		/// </remarks>
		/// <param name="builder">A delegate that builds the new <seealso cref="WebhookMessage"/></param>
		public async Task FireMessageAsync(WebhookMessageBuilder builder) =>
			await DiscordWebhookService.PostMessageAsync(WebhookURL, builder(new WebhookMessage()), ThreadID);

		/// <summary>
		/// Synchronously posts a new message to discord. Waits for the API to send the message to the channel, providing feedback.
		/// </summary>
		/// <remarks>
		/// Provides feedback from the API, including errors. Allows for editing of messages, and for sending more messages to a newly created thread.
		/// </remarks>
		/// <param name="message">message to post</param>
		public PostedDiscordMessage PostMessage(WebhookMessage message) =>
			DiscordWebhookService.PostMessage(WebhookURL, message, ThreadID, OnRatelimit);

		/// <summary>
		/// Synchronously posts a new message to discord. Waits for the API to send the message to the channel, providing feedback.
		/// </summary>
		/// <remarks>
		/// Provides feedback from the API, including errors. Allows for editing of messages, and for sending more messages to a newly created thread.
		/// </remarks>
		/// <param name="builder">A delegate that builds the new <seealso cref="WebhookMessage"/></param>
		public PostedDiscordMessage PostMessage(WebhookMessageBuilder builder) =>
			DiscordWebhookService.PostMessage(WebhookURL, builder(new WebhookMessage()), ThreadID, OnRatelimit);

		/// <summary>
		/// Asynchronously posts a new message to discord. Waits for the API to send the message to the channel, providing feedback.
		/// </summary>
		/// <remarks>
		/// Provides feedback from the API, including errors. Allows for editing of messages, and for sending more messages to a newly created thread.
		/// </remarks>
		/// <param name="message">message to post</param>
		/// <param name="token">Cancellation token to cancel waiting for rate-limits</param>
		public async Task<PostedDiscordMessage> PostMessageAsync(WebhookMessage message, CancellationToken token = default) =>
			await DiscordWebhookService.PostMessageAsync(WebhookURL, message, ThreadID, token, OnRatelimit);

		/// <summary>
		/// Asynchronously posts a new message to discord. Waits for the API to send the message to the channel, providing feedback.
		/// </summary>
		/// <remarks>
		/// Provides feedback from the API, including errors. Allows for editing of messages, and for sending more messages to a newly created thread.
		/// </remarks>
		/// <param name="builder">A delegate that builds the new <seealso cref="WebhookMessage"/></param>
		/// <param name="token">Cancellation token to cancel waiting for rate-limits</param>
		public async Task<PostedDiscordMessage> PostMessageAsync(WebhookMessageBuilder builder, CancellationToken token = default) =>
			await DiscordWebhookService.PostMessageAsync(WebhookURL, builder(new WebhookMessage()), ThreadID, token, OnRatelimit);
	}
}
