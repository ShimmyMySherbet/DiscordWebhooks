# DiscordWebhooks
A lightweight Discord webhook library. 

Provides Async and non-Async APIs. Currently supports everything the Discord webhook API provides.
## Features
| ☐ | Feature | Backlog |
| ------------- | ------------- | -- |
| ✅  | Posting Messages  | N/A |
| ✅  | Editing Messages  | N/A |
| ✅  | Deleting Messages  | N/A |
| ✅  | Uploading Attachments  | N/A |
| ✅  | Embedding Attachments  | N/A |
| ✅  | Message Embeds | N/A |
| ✅  | Forums Channel Support | N/A |
| ✅  | TTS | N/A |
| ✅  | Ping Settings | N/A |
| ✅  | Embed Suppression | N/A |
| ✅  | Ratelimit Handling | N/A |
| ✅  | Discord Error Handling | N/A |
| ✅  | Markdown Text Utilities | N/A |

And more not listed here

## Platform Support
| ☐ | Platform |
| ------------- | ------------- |
| ✅  | .NET 4.8  |
| ✅  | .NET Standard 2.0  |
 ✅  | .NET Core 3.1  |
| ✅  | .NET 6  |
| ✅  | .NET 7  |



# Installation
Install via Nu-get: **Install-Package ShimmyMySherbet.DiscordWebhook**


# Usage Example:

```cs
// instance-based webhook client with support for forum and text channels.
var channel = new DiscordWebhookChannel(ForumsChannelWebhookURL);

channel.OnRatelimit += LogRatelimitEvent;

var ausRole = 1138469998130634753ul;

var patchNotes = "* New vehicle Added (Kayak)\n" +
                    "* Lag fix\n" +
                    "* Improved enemy AI\n" +
                    "* New aggressive mobs\n" +
                    "* New boss battles\n" +
                    "* Fixed npc bugs\n" +
                    "* Map update";

var message = new WebhookMessage()
                .WithUsername("Announcements")
                .CreateThread("Australia v0.4.2 Patch Notes") // Creates a thread in forums channels
                .WithAvatar(AusFlagURL)
                .PassEmbed() // Creates an embed and returns it
                     .WithTitle("Australia Patch Notes")
                     .WithURL("https://www.youtube.com/watch?v=1CQ8b2WOWW8")
                     .WithDescription("New update just dropped")
                     .WithField("Change notes", patchNotes)
                     .UploadImage(@".\aus.png") // Also supports uploading byte[]
                     .Finalize(); // returns the parent WebhookMessage


var sent = await channel.PostMessageAsync(message);


await Task.Delay(1000);

// Modify message object
message.WithContent(ausRole.PingRole())
          .PingsRole(ausRole); // Allows the message to ping the role

// Edit message with new content
await sent.EditMessageAsync(message);


await Task.Delay(10000);
await sent.DeleteAsync();
```
<img src="https://github.com/ShimmyMySherbet/DiscordWebhooks/blob/master/media/example.png?raw=true">
