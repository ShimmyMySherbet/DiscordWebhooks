using System.Threading.Tasks;
using ShimmyMySherbet.DiscordWebhooks.Models;

namespace ShimmyMySherbet.DiscordWebhooks
{
	public class DiscordWebhookClient
	{
		public string WebhookURL { get; set; }


		public DiscordWebhookClient(string webhookURL)
		{
			WebhookURL = webhookURL;
		}

		public void PostMessage(WebhookMessage message) =>
			DiscordWebhookService.PostMessage(WebhookURL, message);

		public async Task PostMessageAsync(WebhookMessage message) =>
			await DiscordWebhookService.PostMessageAsync(WebhookURL, message);
	}
}
