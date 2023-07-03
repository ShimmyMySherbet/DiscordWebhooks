using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ShimmyMySherbet.DiscordWebhooks.Models;

namespace ShimmyMySherbet.DiscordWebhooks
{
	public static class DiscordWebhookService
	{
		public static void PostMessage(string webhookUrl, WebhookMessage message)
		{
			var request = WebRequest.CreateHttp(webhookUrl);
			request.Method = "POST";
			request.ContentType = "application/json";

			var payloadJson = JsonConvert.SerializeObject(message);
			var uploadBuffer = Encoding.UTF8.GetBytes(payloadJson);

			request.ContentLength = uploadBuffer.Length;

			using (var network = request.GetRequestStream())
			{
				network.Write(uploadBuffer, 0, uploadBuffer.Length);
				network.Flush();
			}

			request.GetResponse();
		}

		public static async Task PostMessageAsync(string webhookUrl, WebhookMessage message)
		{
			var request = WebRequest.CreateHttp(webhookUrl);
			request.Method = "POST";
			request.ContentType = "application/json";

			var payloadJson = JsonConvert.SerializeObject(message);
			var uploadBuffer = Encoding.UTF8.GetBytes(payloadJson);

			request.ContentLength = uploadBuffer.Length;

			using (var network = request.GetRequestStream())
			{
				await network.WriteAsync(uploadBuffer, 0, uploadBuffer.Length);
				await network.FlushAsync();
			}

			await request.GetResponseAsync();
		}
	}
}
