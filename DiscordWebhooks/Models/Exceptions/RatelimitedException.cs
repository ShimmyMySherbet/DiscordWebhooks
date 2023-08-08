namespace ShimmyMySherbet.DiscordWebhooks.Models.Exceptions
{
    /// <summary>
    /// Raises when trying to synchronously send a message to Discord, and Discord responds with a rate-limit
    /// </summary>
    /// <remarks>
    /// The Async APIs do not raise this method, and instead automatically try to re-send after the cooldown period has expired
    /// </remarks>
    public class RatelimitedException : DiscordException
    {
        public RatelimitedException(DiscordStatusMessage content) : base(content)
        {
        }
    }
}
