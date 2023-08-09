using System;
using System.Net.Http;
using ShimmyMySherbet.DiscordWebhooks.Models.Interfaces;

namespace ShimmyMySherbet.DiscordWebhooks.Models.Attachments
{
    public class BufferAttachmentContent : IAttachmentContent
    {
        public byte[] Content { get; }

        public BufferAttachmentContent(byte[] content)
        {
            Content = content;
        }

        public HttpContent GetContent()
        {
            return new ByteArrayContent(Content);
        }
    }
}
