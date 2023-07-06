using System.Net;
using System.Threading;
using System.Threading.Tasks;
using ShimmyMySherbet.DiscordWebhooks.Helpers;
using ShimmyMySherbet.DiscordWebhooks.Models;
using ShimmyMySherbet.DiscordWebhooks.Models.Exceptions;

namespace ShimmyMySherbet.DiscordWebhooks
{
    /// <summary>
    /// Static tool to post messages to discord with a specified webhook URL.
    /// </summary>
    /// <remarks>
    /// This class provides the core interfaces for this library. You can interface with them directly, though it is recommended to use <see cref="DiscordWebhookChannel"/> instead.
    /// </remarks>
    public static class DiscordWebhookService
	{
		/// <summary>
		/// Synchronously posts a new message to discord. Does not wait for the discord API to respond to the message, which is the default behaviour of the discord webhook API.
		/// </summary>
		/// <remarks>
		/// Does not provide any feedback from the API. A 'fire and forget' method.
		/// </remarks>
		/// <param name="message">message to post</param>
		/// <param name="webhookUrl">The discord webhook URL to post messages to</param>
		/// <param name="threadID">The ID of the thread to post the message to, if applicable</param>
		public static void FireMessage(string webhookUrl, WebhookMessage message, ulong threadID = 0)
		{
			var request = WebRequest.CreateHttp(webhookUrl + $"{(threadID != 0 ? $"?thread_id={threadID}" : "")}");

			request.PostJson(message);

			try
			{
				request.GetResponse();
			}
			catch (WebException ex)
			{
				if (ex.Response?.ContentLength > 0)
				{
					var statusMessage = ex.Response.ReadResponse<DiscordStatusMessage>();
					statusMessage.Status = ((HttpWebResponse)ex.Response).StatusDescription;

					throw new DiscordException(statusMessage);
				}
				throw;
			}
		}

		/// <summary>
		/// Asynchronously posts a new message to discord. Does not wait for the discord API to respond to the message, which is the default behaviour of the discord webhook API.
		/// </summary>
		/// <remarks>
		/// Does not provide any feedback from the API. A 'fire and forget' method.
		/// </remarks>
		/// <param name="message">message to post</param>
		/// <param name="webhookUrl">The discord webhook URL to post messages to</param>
		/// <param name="threadID">The ID of the thread to post the message to, if applicable</param>
		public static async Task FireMessageAsync(string webhookUrl, WebhookMessage message, ulong threadID = 0)
		{
			var request = WebRequest.CreateHttp(webhookUrl + $"{(threadID != 0 ? $"?thread_id={threadID}" : "")}");
			await request.PostJsonAsync(message);

			try
			{
				await request.GetResponseAsync();
			}
			catch (WebException ex)
			{
				if (ex.Response?.ContentLength > 0)
				{
					var statusMessage = ex.Response.ReadResponse<DiscordStatusMessage>();
					statusMessage.Status = ((HttpWebResponse)ex.Response).StatusDescription;

					throw new DiscordException(statusMessage);
				}
				throw;
			}
		}

		/// <summary>
		/// Synchronously posts a new message to discord. Waits for the API to send the message to the channel, providing feedback.
		/// </summary>
		/// <remarks>
		/// Provides feedback from the API, including errors. Allows for editing of messages, and for sending more messages to a newly created thread.
		/// </remarks>
		/// <param name="message">message to post</param>
		/// <param name="webhookUrl">The discord webhook URL to post messages to</param>
		/// <param name="threadID">The ID of the thread to post the message to, if applicable</param>
		public static PostedDiscordMessage PostMessage(string webhookUrl, WebhookMessage message, ulong threadID = 0)
		{
			var request = WebRequest.CreateHttp(webhookUrl + $"?wait=true{(threadID != 0 ? $"&thread_id={threadID}" : "")}");

			// Including thread name when sending to an existing thread throws an api error
			if (threadID != 0)
				message.ThreadName = string.Empty;

			request.PostJson(message);

			try
			{
				var response = request.GetResponse();
				return response.ReadResponse<PostedDiscordMessage>();
			}
			catch (WebException ex)
			{
				if (ex.Response?.ContentLength > 0)
				{
					var statusMessage = ex.Response.ReadResponse<DiscordStatusMessage>();
					statusMessage.Status = ((HttpWebResponse)ex.Response).StatusDescription;

					throw new DiscordException(statusMessage);
				}
				throw;
			}
		}

		/// <summary>
		/// Asynchronously posts a new message to discord. Waits for the API to send the message to the channel, providing feedback.
		/// </summary>
		/// <remarks>
		/// Provides feedback from the API, including errors. Allows for editing of messages, and for sending more messages to a newly created thread.
		/// </remarks>
		/// <param name="message">message to post</param>
		/// <param name="webhookUrl">The discord webhook URL to post messages to</param>
		/// <param name="threadID">The ID of the thread to post the message to, if applicable</param>
		public static async Task<PostedDiscordMessage> PostMessageAsync(string webhookUrl, WebhookMessage message, ulong threadID = 0)
		{
			var request = WebRequest.CreateHttp(webhookUrl + $"?wait=true{(threadID != 0 ? $"&thread_id={threadID}" : "")}");

			// Including thread name when sending to an existing thread throws an api error
			if (threadID != 0)
				message.ThreadName = string.Empty;

			await request.PostJsonAsync(message);

			try
			{
				var response = await request.GetResponseAsync();
				var value = response.ReadResponse<PostedDiscordMessage>();
				value.Channel = new DiscordWebhookChannel(webhookUrl, value.ChannelID);

				return value;

			}
			catch (WebException ex)
			{
				if (ex.Response?.ContentLength > 0)
				{
					var statusMessage = await ex.Response.ReadResponseAsync<DiscordStatusMessage>();
					statusMessage.Status = ((HttpWebResponse)ex.Response).StatusDescription;

					throw new DiscordException(statusMessage);
				}
				throw;
			}
		}


		/// <summary>
		/// Synchronously edits a previously sent message, replacing it with the supplied <seealso cref="WebhookMessage"/>
		/// </summary>
		/// <remarks>
		/// Editing messages replaces them with a new one, so to remove parts of the message, omit them from <paramref name="message"/>. Username, avatar, and thread name cannot be modified and with be ignored.
		/// </remarks>
		/// <param name="message">The replacement message to post</param>
		/// <param name="messageID">The ID of the message to edit</param>
		/// <param name="threadID">The ID of the thread the target message is in, if applicable</param>
		/// <param name="webhookUrl">The base webhook url to post the message to</param>
		public static PostedDiscordMessage EditMessage(string webhookUrl, WebhookMessage message, ulong messageID, ulong threadID = 0)
		{
			var request = WebRequest.CreateHttp(webhookUrl + $"/messages/{messageID}?wait=true{(threadID != 0 ? $"&thread_id={threadID}" : string.Empty)}");

			request.PostJson(message, "PATCH");

			try
			{
				var response = request.GetResponse();
				var value = response.ReadResponse<PostedDiscordMessage>();
				value.Channel = new DiscordWebhookChannel(webhookUrl, threadID);

				return value;
			}
			catch (WebException ex)
			{
				if (ex.Response?.ContentLength > 0)
				{
					var statusMessage = ex.Response.ReadResponse<DiscordStatusMessage>();
					statusMessage.Status = ((HttpWebResponse)ex.Response).StatusDescription;

					throw new DiscordException(statusMessage);
				}
				throw;
			}
		}

		/// <summary>
		/// Asynchronously edits a previously sent message, replacing it with the supplied <seealso cref="WebhookMessage"/>
		/// </summary>
		/// <remarks>
		/// Editing messages replaces them with a new one, so to remove parts of the message, omit them from <paramref name="message"/>. Username, avatar, and thread name cannot be modified and with be ignored.
		/// </remarks>
		/// <param name="message">The replacement message to post</param>
		/// <param name="messageID">The ID of the message to edit</param>
		/// <param name="threadID">The ID of the thread the target message is in, if applicable</param>
		/// <param name="webhookUrl">The base webhook url to post the message to</param>
		public static async Task<PostedDiscordMessage> EditMessageAsync(string webhookUrl, WebhookMessage message, ulong messageID, ulong threadID = 0)
		{
			var request = WebRequest.CreateHttp(webhookUrl + $"/messages/{messageID}?wait=true{(threadID != 0 ? $"&thread_id={threadID}" : string.Empty)}");

			await request.PostJsonAsync(message, "PATCH");

			try
			{
				var response = await request.GetResponseAsync();
				var value = await response.ReadResponseAsync<PostedDiscordMessage>();
				value.Channel = new DiscordWebhookChannel(webhookUrl, threadID);

				return value;

			}
			catch (WebException ex)
			{
				if (ex.Response?.ContentLength > 0)
				{
					var statusMessage = ex.Response.ReadResponse<DiscordStatusMessage>();
					statusMessage.Status = ((HttpWebResponse)ex.Response).StatusDescription;

					throw new DiscordException(statusMessage);
				}
				throw;
			}
		}

		/// <summary>
		/// Synchronously deletes a previously sent message
		/// </summary>
		/// <param name="webhookURL">The base discord webhook url</param>
		/// <param name="messageID">The ID of the message to delete</param>
		/// <param name="threadID">The ID of the thread the message is in if applicable</param>
		public static void DeleteMessage(string webhookURL, ulong messageID, ulong threadID = 0)
		{
			var request = WebRequest.CreateHttp(webhookURL + $"/messages/{messageID}{(threadID != 0 ? $"?thread_id={threadID}" : string.Empty)}");
			request.Method = "DELETE";

			try
			{
				request.GetResponse();
			}
			catch (WebException ex)
			{
				if (ex.Response?.ContentLength > 0)
				{
					var statusMessage = ex.Response.ReadResponse<DiscordStatusMessage>();
					statusMessage.Status = ((HttpWebResponse)ex.Response).StatusDescription;

					throw new DiscordException(statusMessage);
				}
				throw;
			}
		}

		/// <summary>
		/// Asynchronously deletes a previously sent message
		/// </summary>
		/// <param name="webhookURL">The base discord webhook url</param>
		/// <param name="messageID">The ID of the message to delete</param>
		/// <param name="threadID">The ID of the thread the message is in if applicable</param>
		public static async Task DeleteMessageAsync(string webhookURL, ulong messageID, ulong threadID = 0)
		{
			var request = WebRequest.CreateHttp(webhookURL + $"/messages/{messageID}{(threadID != 0 ? $"?thread_id={threadID}" : string.Empty)}");
			request.Method = "DELETE";

			try
			{
				await request.GetResponseAsync();
			}
			catch (WebException ex)
			{
				if (ex.Response?.ContentLength > 0)
				{
					var statusMessage = await ex.Response.ReadResponseAsync<DiscordStatusMessage>();
					statusMessage.Status = ((HttpWebResponse)ex.Response).StatusDescription;

					ex.Response.Dispose();
					throw new DiscordException(statusMessage);
				}
				throw;
			}
		}
	}
}
