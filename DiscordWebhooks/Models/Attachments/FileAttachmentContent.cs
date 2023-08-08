using System;
using System.IO;
using System.Net.Http;
using ShimmyMySherbet.DiscordWebhooks.Models.Interfaces;

namespace ShimmyMySherbet.DiscordWebhooks.Models.Attachments
{
    public class FileAttachmentContent : IAttachmentContent
    {
        public string FileName { get; set; }

        public FileAttachmentContent(string fileName)
        {
            FileName = fileName;
        }

        public HttpContent GetContent()
        {
            if (!File.Exists(FileName))
            {
                throw new FileNotFoundException(FileName);
            }

            // FileStream is disposed on WebResponse dispose
            return new StreamContent(new FileStream(FileName, FileMode.Open, FileAccess.Read));
        }
    }
}
