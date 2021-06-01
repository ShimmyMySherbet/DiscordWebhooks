# DiscordWebhooks
A small light weight library for sending webhook messages to discord.

Provides Async and non-async APIs.

# Installation
Install via Nu-get: **Install-Package ShimmyMySherbet.DiscordWebhook**



# Usage Example:

```cs
WebhookMessage testMessage = new WebhookMessage()
    .WithContent("Message Content")
    .WithAvatar("https://cdn.discordapp.com/attachments/368648379556954115/818717925842223114/bojo.png")
    .WithUsername("Test Bot")
    .PassEmbed()
        .WithTitle("Announcement")
        .WithDescription("Los Angeles Carries Out Controlled Burn Of Old-Growth Celebrities To Make Way For New Stars")
        .WithColor(Color.red)
        .WithImage("https://i.kinja-img.com/gawker-media/image/upload/c_fit,f_auto,g_center,pg_1,q_60,w_965/aehqzlkjawktzvzqh22p.jpg")
        .WithURL("https://www.theonion.com/los-angeles-carries-out-controlled-burn-of-old-growth-c-1846433536")
        .WithTimestamp(DateTime.Now)
        .WithField("Category", "News Brief")
        .WithField("Author", "Someone idk")
        .Finalize()
    .PassEmbed()
        .WithTitle("2nd embed title here")
        .WithDescription("Description")
        .WithImage("https://cdn.discordapp.com/attachments/368648379556954115/818717925842223114/bojo.png")
        .WithColor(Color.magenta)
        .WithAuthor("Boris", "https://www.theonion.com/los-angeles-carries-out-controlled-burn-of-old-growth-c-1846433536")
        .Finalize();

await DiscordWebhookService.PostMessageAsync(WebhookURL, testMessage);
```

Result:

<img src="https://i.ibb.co/T1pPj7t/image.png">
