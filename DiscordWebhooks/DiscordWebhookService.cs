using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ShimmyMySherbet.DiscordWebhooks.Models;

namespace ShimmyMySherbet.DiscordWebhooks
{
	/// <summary>
	/// Static tool to post messages to discord with a specified webhook URL
	/// </summary>
	public static class DiscordWebhookService
	{
		/// <summary>
		/// Synchronously posts a new message to discord
		/// </summary>
		/// <param name="message">message to post</param>
		/// <param name="webhookUrl">The discord webhook URL to post messages to</param>
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

		/// <summary>
		/// Asynchronously posts a new message to discord
		/// </summary>
		/// <param name="message">message to post</param>
		/// <param name="webhookUrl">The discord webhook URL to post messages to</param>
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
