using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ShimmyMySherbet.DiscordWebhooks.Models.Response;

namespace ShimmyMySherbet.DiscordWebhooks.Models
{
	public class PostedDiscordMessage
	{
		/// <summary>
		/// The channel the message was sent to, allows for sending further messages to the same channel, including to newly created threads.
		/// </summary>
		/// <remarks>
		/// This value is not set by the Discord API, but is instead set by <seealso cref="DiscordWebhookService"/> when a message is posted
		/// </remarks>
		public DiscordWebhookChannel Channel { get; set; }

		/// <summary>
		/// The ID of the created discord message.
		/// </summary>
		[JsonProperty("id")]
		public ulong MessageID { get; set; }

		/// <summary>
		/// Message type flags
		/// </summary>
		[JsonProperty("type")]
		public int Type { get; set; }

		/// <summary>
		/// The text content of the sent message
		/// </summary>
		[JsonProperty("content")]
		public string Content { get; set; }

		/// <summary>
		/// The discord channel ID the message was sent to, or the thread ID if the channel is a forms channel
		/// </summary>
		[JsonProperty("channel_id")]
		public ulong ChannelID { get; set; }

		/// <summary>
		/// Provides info on the webhook user that sent the message
		/// </summary>
		[JsonProperty("author")]
		public ResponseAuthor Author { get; set; }

		/// <summary>
		/// The time the message was last edited
		/// </summary>
		[JsonProperty("edited_timestamp")]
		public DateTime? Edited { get; set; }

		/// <summary>
		/// The time the message was first created
		/// </summary>
		[JsonProperty("timestamp")]
		public DateTime Created { get; set; }

		/// <summary>
		/// A flag specifying if this message was allowed to @everyone or @here
		/// </summary>
		[JsonProperty("mention_everyone")]
		public bool MentionEveryone { get; set; }

		/// <summary>
		/// A flag specifying if the message is pinned
		/// </summary>
		[JsonProperty("pinned")]
		public bool Pinned { get; set; }

		/// <summary>
		/// The index of the message in the channel
		/// </summary>
		[JsonProperty("position")]
		public int Position { get; set; }

		/// <summary>
		/// A flag specifying if the message is tts
		/// </summary>
		public bool TTS { get; set; }

		/// <summary>
		/// The webhook ID
		/// </summary>
		[JsonProperty("webhook_id")]
		public ulong WebhookID { get; set; }

		/// <summary>
		/// A list of the embeds sent to the webhook
		/// </summary>
		[JsonProperty("embeds")]
		public List<WebhookEmbed> Embeds { get; set; }

		/// <summary>
		/// Synchronously edits the sent message, updating this instance.
		/// </summary>
		/// <param name="message">The message to replace this one.</param>
		/// <remarks>
		/// You cannot modify the username, avatar, or thread name. These fields will be ignored when modifying messages.
		/// </remarks>
		public void EditMessage(WebhookMessage message)
		{
			var instance = DiscordWebhookService.EditMessage(Channel.WebhookURL, message, MessageID, Channel.ThreadID);
			CopyFrom(instance);
		}

		/// <summary>
		/// Asynchronously edits the sent message, updating this instance.
		/// </summary>
		/// <param name="message">The message to replace this one.</param>
		/// <remarks>
		/// You cannot modify the username, avatar, or thread name. These fields will be ignored when modifying messages.
		/// </remarks>
		public async Task EditMessageAsync(WebhookMessage message)
		{
			var instance = await DiscordWebhookService.EditMessageAsync(Channel.WebhookURL, message, MessageID, Channel.ThreadID);
			CopyFrom(instance);
		}

		/// <summary>
		/// Fills in the current instance with modified values after editing the discord message
		/// </summary>
		/// <param name="instance">New response from the API to repalce the current values</param>
		private void CopyFrom(PostedDiscordMessage instance)
		{
			Type = instance.Type;
			Content = instance.Content;
			Author = instance.Author;
			Edited = instance.Edited;
			MentionEveryone = instance.MentionEveryone;
			Pinned = instance.Pinned;
			Position = instance.Position;
			TTS = instance.TTS;
			Embeds = instance.Embeds;
		}

		/// <summary>
		/// Synchronously deletes a previously sent message
		/// </summary>
		public void Delete()
		{
			DiscordWebhookService.DeleteMessage(Channel.WebhookURL, MessageID, Channel.ThreadID);
		}

		/// <summary>
		/// Asynchronously deletes a previously sent message
		/// </summary>
		public async Task DeleteAsync()
		{
			await DiscordWebhookService.DeleteMessageAsync(Channel.WebhookURL, MessageID, Channel.ThreadID);
		}
	}
}
