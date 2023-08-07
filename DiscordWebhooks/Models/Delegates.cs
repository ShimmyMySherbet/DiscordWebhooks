namespace ShimmyMySherbet.DiscordWebhooks.Models
{
	public delegate WebhookMessage WebhookMessageBuilder(WebhookMessage message);


	public delegate void OnRatelimitArgs(WebhookMessage message, string webhookURL, float retryAfter);
}
