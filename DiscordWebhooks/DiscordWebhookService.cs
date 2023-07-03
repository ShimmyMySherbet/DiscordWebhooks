using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using ShimmyMySherbet.DiscordWebhooks.Embeded;
using System.Threading.Tasks;

namespace ShimmyMySherbet.DiscordWebhooks
{
	public static class DiscordWebhookService
	{
		public static void PostMessage(string WebhookURL, WebhookMessage message)
		{
			HttpWebRequest request = WebRequest.CreateHttp(WebhookURL);
			request.Method = "POST";
			request.ContentType = "application/json";

			string Payload = JsonConvert.SerializeObject(message);
			byte[] Buffer = Encoding.UTF8.GetBytes(Payload);

			request.ContentLength = Buffer.Length;

			using (Stream write = request.GetRequestStream())
			{
				write.Write(Buffer, 0, Buffer.Length);
				write.Flush();
			}

			var resp = (HttpWebResponse)request.GetResponse();
		}

		public static async Task PostMessageAsync(string WebhookURL, WebhookMessage message)
		{
			HttpWebRequest request = WebRequest.CreateHttp(WebhookURL);
			request.Method = "POST";
			request.ContentType = "application/json";

			string Payload = JsonConvert.SerializeObject(message);
			byte[] Buffer = Encoding.UTF8.GetBytes(Payload);

			request.ContentLength = Buffer.Length;

			using (Stream write = (await request.GetRequestStreamAsync()))
			{
				await write.WriteAsync(Buffer, 0, Buffer.Length);
				await write.FlushAsync();
			}

			_ = (HttpWebResponse)(await request.GetResponseAsync());
		}
	}
}
