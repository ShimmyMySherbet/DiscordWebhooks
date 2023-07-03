using System.Threading.Tasks;
using ShimmyMySherbet.DiscordWebhooks.Models;

namespace ShimmyMySherbet.DiscordWebhooks
{
	/// <summary>
	/// An instance based client to send discord webhooks.
	/// </summary>
	public class DiscordWebhookClient
	{
		/// <summary>
		/// Root webhook url to post messages to
		/// </summary>
		public string WebhookURL { get; set; }

		/// <summary>
		/// Creates a new webhook client with the specified URL
		/// </summary>
		/// <param name="webhookURL">Root webhook URL to post messages to</param>
		public DiscordWebhookClient(string webhookURL)
		{
			WebhookURL = webhookURL;
		}

		/// <summary>
		/// Synchronously posts a new message to discord
		/// </summary>
		/// <param name="message">message to post</param>
		public void PostMessage(WebhookMessage message) =>
			DiscordWebhookService.PostMessage(WebhookURL, message);

		/// <summary>
		/// Asynchronously posts a new message to discord
		/// </summary>
		/// <param name="message">message to post</param>
		public async Task PostMessageAsync(WebhookMessage message) =>
			await DiscordWebhookService.PostMessageAsync(WebhookURL, message);
	}
}
