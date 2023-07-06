using System.Collections.Generic;
using Newtonsoft.Json;

namespace ShimmyMySherbet.DiscordWebhooks.Models
{
	/// <summary>
	/// Represents a webhook message to post to Discord.
	/// </summary>
	public class WebhookMessage
	{
		/// <summary>
		/// Webhook display username
		/// </summary>
		[JsonProperty("username")]
		public string Username { get; set; }

		/// <summary>
		/// Webhook display avatar URI
		/// </summary>
		[JsonProperty("avatar_url")]
		public string AvatarURL { get; set; }

		/// <summary>
		/// The message content. Shown before any embeds
		/// </summary>
		[JsonProperty("content")]
		public string Content { get; set; }

		/// <summary>
		/// List of message embeds. Max is 25
		/// </summary>
		[JsonProperty("embeds")]
		public List<WebhookEmbed> Embeds { get; set; } = new List<WebhookEmbed>();

		/// <summary>
		/// Marks the message as Text-To-Speech
		/// </summary>
		[JsonProperty("tts")]
		public bool TTS { get; set; }

		/// <summary>
		/// The name of the thread to create when posting the message. Can only be used in forum channels
		/// </summary>
		[JsonProperty("thread_name")]
		public string ThreadName { get; set; }

		/// <summary>
		/// Specifies what users/roles the webhook message is allowed to ping
		/// </summary>
		[JsonProperty("allowed_mentions")]
		public WebhookAllowedMentions AllowedMentions { get; set; } = new WebhookAllowedMentions();


		/// <summary>
		/// Adds an instance of an embed to this message.
		/// </summary>
		/// <param name="embed">Embed to add</param>
		/// <returns>Current instance</returns>
		public WebhookMessage WithEmbed(WebhookEmbed embed)
		{
			Embeds.Add(embed);
			return this;
		}
		/// <summary>
		/// Creates an embed on this message and returns it.
		/// </summary>
		/// <returns>New <seealso cref="WebhookEmbed"/></returns>
		/// <remarks>
		/// You can call <see cref="WebhookEmbed.Finalize"/> and return to this <seealso cref="WebhookMessage"/> instance when you are finished.
		/// </remarks>
		public WebhookEmbed PassEmbed()
		{
			var embed = new WebhookEmbed(this);
			Embeds.Add(embed);
			return embed;
		}

		/// <summary>
		/// Sets the display username of the webhook user
		/// </summary>
		/// <param name="username">Display username</param>
		/// <returns>Current instance</returns>
		public WebhookMessage WithUsername(string username)
		{
			Username = username;
			return this;
		}

		/// <summary>
		/// Creates a new thread in the forms channel to post the message to
		/// </summary>
		/// <remarks>
		/// Can only be used in forum channels
		/// </remarks>
		/// <param name="threadName">The name of the thread to create</param>
		/// <returns>Current instance</returns>
		public WebhookMessage CreateThread(string threadName)
		{
			ThreadName = threadName;
			return this;
		}

		/// <summary>
		/// Sets the display avatar URI of the webhook user
		/// </summary>
		/// <param name="avatar">Avatar image URI</param>
		/// <returns>Current Instance</returns>
		public WebhookMessage WithAvatar(string avatar)
		{
			AvatarURL = avatar;
			return this;
		}

		/// <summary>
		/// Sets the message content of the webhook message.
		/// </summary>
		/// <param name="message">Message text</param>
		/// <returns>Current Instance</returns>
		public WebhookMessage WithContent(string message)
		{
			Content = message;
			return this;
		}
		/// <summary>
		/// Enables Text-To-Speech on this message
		/// </summary>
		/// <returns>Current Instance</returns>
		public WebhookMessage WithTTS()
		{
			TTS = true;
			return this;
		}

		/// <summary>
		/// Adds a ping permission flag to <seealso cref="AllowedMentions"/>
		/// </summary>
		/// <param name="permission">Permission to grant</param>
		private void AddMentionPermission(string permission)
		{
			if (!AllowedMentions.ParseFlags.Contains(permission))
			{
				AllowedMentions.ParseFlags.Add(permission);
			}
		}

		/// <summary>
		/// Allows for this message to ping @everyone and @here
		/// </summary>
		/// <returns>Current Instance</returns>
		public WebhookMessage PingsEveryone()
		{
			AddMentionPermission("everyone");
			return this;
		}

		/// <summary>
		/// Allows for this message to ping any role
		/// </summary>
		/// <remarks>
		/// If the pinged roles are known, for safety it is better to use <seealso cref="PingsRole(ulong)"/>
		/// </remarks>
		/// <returns>Current Instance</returns>
		public WebhookMessage PingsRoles()
		{
			AddMentionPermission("roles");
			return this;
		}

		/// <summary>
		/// Allows for this message to ping any users
		/// </summary>
		/// <remarks>
		/// If the pinged users are known, for safety it is better to use <seealso cref="PingsUser(ulong)"/>
		/// </remarks>
		/// <returns>Current Instance</returns>
		public WebhookMessage PingsUsers()
		{
			AddMentionPermission("users");
			return this;
		}

		/// <summary>
		/// Allows the webhook message to ping the specified role
		/// </summary>
		/// <param name="roleID">The ID of the role that can be pinged</param>
		/// <returns>Current Instance</returns>
		public WebhookMessage PingsRole(ulong roleID)
		{
			if (!AllowedMentions.Roles.Contains(roleID))
			{
				AllowedMentions.Roles.Add(roleID);
			}
			return this;
		}

		/// <summary>
		/// Allows the webhook message to ping the specified role
		/// </summary>
		/// <param name="userID">The ID of the user that can be pinged</param>
		/// <returns>Current Instance</returns>
		public WebhookMessage PingsUser(ulong userID)
		{
			if (!AllowedMentions.Users.Contains(userID))
			{
				AllowedMentions.Users.Add(userID);
			}

			return this;
		}
	}
}
