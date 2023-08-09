namespace ShimmyMySherbet.DiscordWebhooks.Models
{
    /// <summary>
    /// A delegate definition for a webhook message builder
    /// </summary>
    /// <param name="message">Blank message to mutate</param>
    /// <returns>Same instance of message </returns>
    public delegate WebhookMessage WebhookMessageBuilder(WebhookMessage message);

    /// <summary>
    /// Handler raised when Discord ratelimits a request
    /// </summary>
    /// <param name="message">The related message</param>
    /// <param name="webhookURL">Endpoint webhook URL</param>
    /// <param name="retryAfter">Ratelimit period</param>
    public delegate void OnRatelimitArgs(WebhookMessage message, string webhookURL, float retryAfter);
}
