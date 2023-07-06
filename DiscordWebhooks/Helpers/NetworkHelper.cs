using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ShimmyMySherbet.DiscordWebhooks.Helpers
{
	/// <summary>
	/// Provides a collection of extensions for helping with HTTP networking
	/// </summary>
	public static class NetworkHelper
	{
		/// <summary>
		/// Synchronously posts an object in JSON
		/// </summary>
		/// <param name="request">Pending request</param>
		/// <param name="payload">Payload object</param>
		/// <param name="method">HTTP method</param>
		public static void PostJson(this HttpWebRequest request, object payload, string method = "POST")
		{
			var json = JsonConvert.SerializeObject(payload);
			var buffer = Encoding.UTF8.GetBytes(json);

			request.Method = method;
			request.ContentLength = buffer.Length;
			request.ContentType = "application/json";

			using (var network = request.GetRequestStream())
			{
				network.Write(buffer, 0, buffer.Length);
				network.Flush();
			}
		}

		/// <summary>
		/// Asynchronously posts an object in JSON
		/// </summary>
		/// <param name="request">Pending request</param>
		/// <param name="payload">Payload object</param>
		/// <param name="method">HTTP method</param>
		public static async Task PostJsonAsync(this HttpWebRequest request, object payload, string method = "POST")
		{
			var json = JsonConvert.SerializeObject(payload);
			var buffer = Encoding.UTF8.GetBytes(json);

			request.Method = method;
			request.ContentLength = buffer.Length;
			request.ContentType = "application/json";

			using (var network = await request.GetRequestStreamAsync())
			{
				await network.WriteAsync(buffer, 0, buffer.Length);
				await network.FlushAsync();
			}
		}


		/// <summary>
		/// Synchronously reads the payload response from a request
		/// </summary>
		/// <typeparam name="T">Type to deserialize response into</typeparam>
		/// <param name="response">Web response</param>
		/// <returns>Instance of the payload message</returns>
		public static T ReadResponse<T>(this WebResponse response)
		{
			using (var network = response.GetResponseStream())
			using (var reader = new StreamReader(network))
			{
				var json = reader.ReadToEnd();
				return JsonConvert.DeserializeObject<T>(json);
			}
		}

		/// <summary>
		/// Asynchronously Reads the payload response from a request
		/// </summary>
		/// <typeparam name="T">Type to deserialize response into</typeparam>
		/// <param name="response">Web response</param>
		/// <returns>Instance of the payload message</returns>
		public static async Task<T> ReadResponseAsync<T>(this WebResponse response)
		{
			using (var network = response.GetResponseStream())
			using (var reader = new StreamReader(network))
			{
				var json = await reader.ReadToEndAsync();
				await Console.Out.WriteLineAsync(json);
				return JsonConvert.DeserializeObject<T>(json);
			}
		}
	}
}
