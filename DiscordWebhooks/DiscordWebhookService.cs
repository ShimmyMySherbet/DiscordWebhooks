using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
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
        /// Synchronously sends a new message to discord. Does not wait for the discord API to respond to the message, which is the default behaviour of the discord webhook API.
        /// </summary>
        /// <remarks>
        /// Does not provide any feedback from the API. A 'fire and forget' method.
        /// </remarks>
        /// <param name="message">message to post</param>
        /// <param name="webhookUrl">The discord webhook URL to post messages to</param>
        /// <param name="threadID">The ID of the thread to post the message to, if applicable</param>
        public static void FireMessage(string webhookUrl, WebhookMessage message, ulong threadID = 0)
        {
            var messageUrl = webhookUrl + $"{(threadID != 0 ? $"?thread_id={threadID}" : "")}";

            // Including thread name when sending to an existing thread throws an api error
            if (threadID != 0)
                message.ThreadName = string.Empty;

            var jsonPayload = JsonConvert.SerializeObject(message);

            var formData = new MultipartFormDataContent
            {
                { new StringContent(jsonPayload), "payload_json" }
            };

            foreach (var attachment in message.Attachments)
            {
                formData.Add(attachment.FormatContent());
            }

            using (var client = new HttpClient())
            using (var response = client.NetPost(messageUrl, formData))
            using (var responseStream = response.Content.NetReadAsStream())
            using (var responseReader = new StreamReader(responseStream))
            {
                response.EnsureSuccessStatusCode();
            }
        }

        /// <summary>
        /// Asynchronously sends a new message to discord. Does not wait for the discord API to respond to the message, which is the default behaviour of the discord webhook API.
        /// </summary>
        /// <remarks>
        /// Does not provide any feedback from the API. A 'fire and forget' method.
        /// </remarks>
        /// <param name="message">message to post</param>
        /// <param name="webhookUrl">The discord webhook URL to post messages to</param>
        /// <param name="threadID">The ID of the thread to post the message to, if applicable</param>
        public static async Task FireMessageAsync(string webhookUrl, WebhookMessage message, ulong threadID = 0)
        {
            var messageUrl = webhookUrl + $"{(threadID != 0 ? $"?thread_id={threadID}" : "")}";

            // Including thread name when sending to an existing thread throws an api error
            if (threadID != 0)
                message.ThreadName = string.Empty;

            var jsonPayload = JsonConvert.SerializeObject(message);

            var formData = new MultipartFormDataContent
            {
                { new StringContent(jsonPayload), "payload_json" }
            };

            foreach (var attachment in message.Attachments)
            {
                formData.Add(attachment.FormatContent());
            }

            using (var client = new HttpClient())
            using (var response = await client.PostAsync(messageUrl, formData))
            using (var responseStream = await response.Content.ReadAsStreamAsync())
            using (var responseReader = new StreamReader(responseStream))
            {
                response.EnsureSuccessStatusCode();
            }
        }

        /// <summary>
        /// Synchronously posts a new message to discord. Waits for the API to send the message to the channel. Throws an error if rate-limited by Discord
        /// </summary>
        /// <remarks>
        /// Provides feedback from the API, including errors. Allows for editing of messages, and for sending more messages to a newly created thread.
        /// </remarks>
        /// <param name="message">message to post</param>
        /// <param name="webhookUrl">The discord webhook URL to post messages to</param>
        /// <param name="threadID">The ID of the thread to post the message to, if applicable</param>
        /// <param name="onRatelimit">Callback raised when Discord ratelimits the endpoint</param>
        /// <exception cref="WebException">Raised when Discord returns a generic failure status</exception>
        /// <exception cref="RatelimitedException">Raised when Discord ratelimits the endpoint</exception>
        public static PostedDiscordMessage PostMessage(string webhookUrl, WebhookMessage message, ulong threadID = 0, OnRatelimitArgs onRatelimit = null)
        {
            var messageUrl = webhookUrl + $"?wait=true{(threadID != 0 ? $"&thread_id={threadID}" : "")}";

            // Including thread name when sending to an existing thread throws an api error
            if (threadID != 0)
                message.ThreadName = string.Empty;

            var jsonPayload = JsonConvert.SerializeObject(message);

            var formData = new MultipartFormDataContent
            {
                { new StringContent(jsonPayload), "payload_json" }
            };

            foreach (var attachment in message.Attachments)
            {
                formData.Add(attachment.FormatContent());
            }

            using (var client = new HttpClient())
            using (var response = client.NetPost(messageUrl, formData))
            using (var responseStream = response.Content.NetReadAsStream())
            using (var responseReader = new StreamReader(responseStream))
            {
                var responseJson = responseReader.ReadToEnd();

                if (!response.IsSuccessStatusCode)
                {
                    var statusMessage = JsonConvert.DeserializeObject<DiscordStatusMessage>(responseJson);
                    if (statusMessage.RetryAfter > 0)
                    {
                        onRatelimit.Invoke(message, webhookUrl, statusMessage.RetryAfter);
                        throw new RatelimitedException(statusMessage);
                    }

                    if (statusMessage.Message != null)
                        throw new DiscordException(statusMessage);

                    response.EnsureSuccessStatusCode();
                }

                var value = JsonConvert.DeserializeObject<PostedDiscordMessage>(responseJson);
                value.Channel = new DiscordWebhookChannel(webhookUrl, value.ChannelID);

                return value;
            }
        }

        /// <summary>
        /// Asynchronously posts a new message to discord. Waits for the API to send the message to the channel. Automatically waits and retries on rate-limit.
        /// </summary>
        /// <remarks>
        /// Provides feedback from the API, including errors. Allows for editing of messages, and for sending more messages to a newly created thread.
        /// </remarks>
        /// <param name="message">message to post</param>
        /// <param name="webhookUrl">The discord webhook URL to post messages to</param>
        /// <param name="threadID">The ID of the thread to post the message to, if applicable</param>
        /// <param name="token">Cancellation token to cancel waiting for rate-limit cooldown</param>
        /// <exception cref="DiscordException">Raised When Discord returns an error with a status message</exception>
        /// <exception cref="WebException">Raised when Discord returns a generic failure status code</exception>
        /// <exception cref="TaskCanceledException">Raised when <paramref name="token"/> has been cancelled</exception>
        /// <exception cref="RatelimitedException">Raised when Discord ratelimits the endpoint</exception>
        public static async Task<PostedDiscordMessage> PostMessageAsync(string webhookUrl, WebhookMessage message, ulong threadID = 0, CancellationToken token = default, OnRatelimitArgs onRatelimit = null)
        {
            var messageUrl = webhookUrl + $"?wait=true{(threadID != 0 ? $"&thread_id={threadID}" : "")}";

            // Including thread name when sending to an existing thread throws an api error
            if (threadID != 0)
                message.ThreadName = string.Empty;

            var jsonPayload = JsonConvert.SerializeObject(message);

            var formData = new MultipartFormDataContent
            {
                { new StringContent(jsonPayload), "payload_json" }
            };

            foreach (var attachment in message.Attachments)
            {
                formData.Add(attachment.FormatContent());
            }

            using (var client = new HttpClient())
            using (var response = await client.PostAsync(messageUrl, formData))
            using (var responseStream = await response.Content.ReadAsStreamAsync())
            using (var responseReader = new StreamReader(responseStream))
            {
                var responseJson = await responseReader.ReadToEndAsync();

                if (!response.IsSuccessStatusCode)
                {
                    var statusMessage = JsonConvert.DeserializeObject<DiscordStatusMessage>(responseJson);

                    if (statusMessage.RetryAfter > 0)
                    {
                        onRatelimit.Invoke(message, webhookUrl, statusMessage.RetryAfter);

                        await Task.Delay((int)Math.Ceiling(statusMessage.RetryAfter * 1000) + 1000, token);

                        token.ThrowIfCancellationRequested();

                        return await PostMessageAsync(webhookUrl, message, threadID);
                    }

                    if (statusMessage.Message != null)
                        throw new DiscordException(statusMessage);

                    response.EnsureSuccessStatusCode();
                }

                var value = JsonConvert.DeserializeObject<PostedDiscordMessage>(responseJson);
                value.Channel = new DiscordWebhookChannel(webhookUrl, value.ChannelID);

                return value;
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
        /// <exception cref="RatelimitedException">Raised when Discord ratelimits the endpoint</exception>
        public static PostedDiscordMessage EditMessage(string webhookUrl, WebhookMessage message, ulong messageID, ulong threadID = 0, OnRatelimitArgs onRatelimit = null)
        {
            var messageUrl = webhookUrl + $"/messages/{messageID}?wait=true{(threadID != 0 ? $"&thread_id={threadID}" : string.Empty)}";

            // Including thread name when sending to an existing thread throws an api error
            if (threadID != 0)
                message.ThreadName = string.Empty;

            var jsonPayload = JsonConvert.SerializeObject(message);

            var formData = new MultipartFormDataContent
            {
                { new StringContent(jsonPayload), "payload_json" }
            };

            foreach (var attachment in message.Attachments)
            {
                formData.Add(attachment.FormatContent());
            }


            using (var client = new HttpClient())
            using (var response = client.NetPatch(messageUrl, formData))
            using (var responseStream = response.Content.NetReadAsStream())
            using (var responseReader = new StreamReader(responseStream))
            {
                var responseJson = responseReader.ReadToEnd();

                if (!response.IsSuccessStatusCode)
                {
                    var statusMessage = JsonConvert.DeserializeObject<DiscordStatusMessage>(responseJson);

                    if (statusMessage.RetryAfter > 0)
                    {
                        onRatelimit.Invoke(message, webhookUrl, statusMessage.RetryAfter);
                        throw new RatelimitedException(statusMessage);
                    }

                    if (statusMessage.Message != null)
                        throw new DiscordException(statusMessage);

                    response.EnsureSuccessStatusCode();
                }

                var value = JsonConvert.DeserializeObject<PostedDiscordMessage>(responseJson);
                value.Channel = new DiscordWebhookChannel(webhookUrl, value.ChannelID);
                return value;
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
        /// <param name="token">Cancellation token to cancel waiting for rate-limit cooldown</param>
        /// <exception cref="DiscordException">Raised When Discord returns an error with a status message</exception>
        /// <exception cref="WebException">Raised when Discord returns a generic failure status code</exception>
        /// <exception cref="TaskCanceledException">Raised when <paramref name="token"/> has been cancelled</exception>
        /// <exception cref="RatelimitedException">Raised when Discord ratelimits the endpoint</exception>
        public static async Task<PostedDiscordMessage> EditMessageAsync(string webhookUrl, WebhookMessage message, ulong messageID, ulong threadID = 0, OnRatelimitArgs onRatelimit = null, CancellationToken token = default)
        {
            var messageUrl = webhookUrl + $"/messages/{messageID}?wait=true{(threadID != 0 ? $"&thread_id={threadID}" : string.Empty)}";

            // Including thread name when sending to an existing thread throws an api error
            if (threadID != 0)
                message.ThreadName = string.Empty;

            var jsonPayload = JsonConvert.SerializeObject(message);

            var formData = new MultipartFormDataContent
            {
                { new StringContent(jsonPayload), "payload_json" }
            };

            foreach (var attachment in message.Attachments)
            {
                formData.Add(attachment.FormatContent());
            }


            using (var client = new HttpClient())
            using (var response = await client.NetPatchAsync(messageUrl, formData))
            using (var responseStream = await response.Content.ReadAsStreamAsync())
            using (var responseReader = new StreamReader(responseStream))
            {
                var responseJson = await responseReader.ReadToEndAsync();

                if (!response.IsSuccessStatusCode)
                {
                    var statusMessage = JsonConvert.DeserializeObject<DiscordStatusMessage>(responseJson);

                    if (statusMessage.RetryAfter > 0)
                    {
                        onRatelimit.Invoke(message, webhookUrl, statusMessage.RetryAfter);

                        await Task.Delay((int)Math.Ceiling(statusMessage.RetryAfter * 1000) + 1000, token);

                        token.ThrowIfCancellationRequested();

                        return await PostMessageAsync(webhookUrl, message, threadID);
                    }

                    if (statusMessage.Message != null)
                        throw new DiscordException(statusMessage);

                    response.EnsureSuccessStatusCode();
                }

                var value = JsonConvert.DeserializeObject<PostedDiscordMessage>(responseJson);
                value.Channel = new DiscordWebhookChannel(webhookUrl, value.ChannelID);
                return value;
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
            using (var client = new HttpClient())
            using (var response = client.NetDelete(webhookURL + $"/messages/{messageID}{(threadID != 0 ? $"?thread_id={threadID}" : string.Empty)}"))
            {
                response.EnsureSuccessStatusCode();
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
            using (var client = new HttpClient())
            using (var response = await client.DeleteAsync(webhookURL + $"/messages/{messageID}{(threadID != 0 ? $"?thread_id={threadID}" : string.Empty)}"))
            {
                response.EnsureSuccessStatusCode();
            }
        }
    }
}
