using System.Net.Http;

namespace ShimmyMySherbet.DiscordWebhooks.Models.Interfaces
{
    public interface IAttachmentContent
    {
        HttpContent GetContent();
    }
}
