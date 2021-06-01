using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

/*
 * Author: ShimmyMySherbet#5694
 * Free to distribute and use personally or commercially
 * Just don't try to pass this off as your work.
*/

namespace ShimmyMySherbet.DiscordWebhooks.Embeded
{
    public class DiscordWebhookClient
    {
        public Uri WebhookURL;

        public DiscordWebhookClient(string webhookURL) => WebhookURL = new Uri(webhookURL);

        public void PostMessage(WebhookMessage message)
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

        public async Task PostMessageAsync(WebhookMessage message)
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

    public static class DiscordHelpers
    {
        public static string DateTimeToISO(DateTime dateTime)
        {
            return DateTimeToISO(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second);
        }

        public static string DateTimeToISO(int year, int month, int day, int hour, int minute, int second)
        {
            return new DateTime(year, month, day, hour, minute, second, 0, DateTimeKind.Local)
                .ToString("yyyy-MM-dd'T'HH:mm:ss.fffK", CultureInfo.InvariantCulture);
        }
    }

    public class WebhookAuthor
    {
        public string name;
        public string url;
        public string icon_url;
    }

    public class WebhookEmbed
    {
        [JsonIgnore]
        private WebhookMessage parent;

        internal WebhookEmbed(WebhookMessage parent)
        {
            this.parent = parent;
        }

        public WebhookEmbed()
        {
        }

        public WebhookMessage Finalize()
        {
            if (parent == null)
            {
                parent = new WebhookMessage() { embeds = new List<WebhookEmbed>() { this } };
            }
            return parent;
        }

        public int color;

        public WebhookAuthor author;

        public string title;

        public string url;

        public string description;

        public List<WebhookField> fields = new List<WebhookField>();

        public WebhookImage image;

        public WebhookImage thumbnail;

        public WebhookFooter footer;

        public string timestamp;

        public WebhookEmbed WithTitle(string title)
        {
            this.title = title;
            return this;
        }

        public WebhookEmbed WithURL(string value)
        {
            this.url = value;
            return this;
        }

        public WebhookEmbed WithDescription(string value)
        {
            this.description = value;
            return this;
        }

        public WebhookEmbed WithTimestamp(DateTime value)
        {
            this.timestamp = DiscordHelpers.DateTimeToISO(value);
            return this;
        }

        public WebhookEmbed WithField(string name, string value, bool inline = true)
        {
            this.fields.Add(new WebhookField() { value = value, inline = inline, name = name });
            return this;
        }

        public WebhookEmbed WithImage(string value)
        {
            this.image = new WebhookImage() { url = value };
            return this;
        }

        public WebhookEmbed WithThumbnail(string value)
        {
            this.thumbnail = new WebhookImage() { url = value };
            return this;
        }

        public WebhookEmbed WithAuthor(string name, string url = null, string icon = null)
        {
            this.author = new WebhookAuthor() { name = name, icon_url = icon, url = url };
            return this;
        }

        public WebhookEmbed WithColor(EmbedColor color)
        {
            this.color = BitConverter.ToInt32(new byte[4] { color.B, color.G, color.R, 0 }, 0);
            return this;
        }

        // Unity Support

        //public WebhookEmbed WithColor(UnityEngine.Color color)
        //{
        //    byte r = Clamp(color.r);
        //    byte g = Clamp(color.g);
        //    byte b = Clamp(color.b);

        //    int numeric = BitConverter.ToInt32(new byte[4] { b, g, r, 0 }, 0);
        //    this.color = numeric;
        //    return this;
        //}

        private byte Clamp(float a)
        {
            return (byte)(Math.Round(a * 255, 0));
        }
    }

    public class WebhookField
    {
        public string name;

        public string value;

        public bool inline;
    }

    public class WebhookFooter
    {
        public string text;

        public string icon_url;
    }

    public class WebhookImage
    {
        public string url = "";
    }

    public class WebhookMessage
    {
        public string username;

        public string avatar_url;

        public string content = "";

        public List<WebhookEmbed> embeds = new List<WebhookEmbed>();

        public bool tts { get; set; }

        public WebhookMessage WithEmbed(WebhookEmbed embed)
        {
            embeds.Add(embed);
            return this;
        }

        public WebhookEmbed PassEmbed()
        {
            WebhookEmbed embed = new WebhookEmbed(this);
            embeds.Add(embed);
            return embed;
        }

        public WebhookMessage WithUsername(string un)
        {
            username = un;
            return this;
        }

        public WebhookMessage WithAvatar(string avatar)
        {
            avatar_url = avatar;
            return this;
        }

        public WebhookMessage WithContent(string c)
        {
            content = c;
            return this;
        }

        public WebhookMessage WithTTS()
        {
            tts = true;
            return this;
        }
    }

    public struct EmbedColor
    {
        public byte R;
        public byte G;
        public byte B;

        public EmbedColor(byte r, byte g, byte b)
        {
            R = r;
            G = g;
            B = b;
        }
        public override bool Equals(object obj)
        {
            if (obj is EmbedColor col)
            {
                return col.R == R && col.G == G && col.B == B;
            }
            return false;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public static EmbedColor AliceBlue => new EmbedColor(255, 240, 248);
        public static EmbedColor AntiqueWhite => new EmbedColor(255, 250, 235);
        public static EmbedColor Aqua => new EmbedColor(255, 0, 255);
        public static EmbedColor Aquamarine => new EmbedColor(255, 127, 255);
        public static EmbedColor Azure => new EmbedColor(255, 240, 255);
        public static EmbedColor Beige => new EmbedColor(255, 245, 245);
        public static EmbedColor Bisque => new EmbedColor(255, 255, 228);
        public static EmbedColor Black => new EmbedColor(255, 0, 0);
        public static EmbedColor BlanchedAlmond => new EmbedColor(255, 255, 235);
        public static EmbedColor Blue => new EmbedColor(255, 0, 0);
        public static EmbedColor BlueViolet => new EmbedColor(255, 138, 43);
        public static EmbedColor Brown => new EmbedColor(255, 165, 42);
        public static EmbedColor BurlyWood => new EmbedColor(255, 222, 184);
        public static EmbedColor CadetBlue => new EmbedColor(255, 95, 158);
        public static EmbedColor Chartreuse => new EmbedColor(255, 127, 255);
        public static EmbedColor Chocolate => new EmbedColor(255, 210, 105);
        public static EmbedColor Coral => new EmbedColor(255, 255, 127);
        public static EmbedColor CornflowerBlue => new EmbedColor(255, 100, 149);
        public static EmbedColor Cornsilk => new EmbedColor(255, 255, 248);
        public static EmbedColor Crimson => new EmbedColor(255, 220, 20);
        public static EmbedColor Cyan => new EmbedColor(255, 0, 255);
        public static EmbedColor DarkBlue => new EmbedColor(255, 0, 0);
        public static EmbedColor DarkCyan => new EmbedColor(255, 0, 139);
        public static EmbedColor DarkGoldenrod => new EmbedColor(255, 184, 134);
        public static EmbedColor DarkGray => new EmbedColor(255, 169, 169);
        public static EmbedColor DarkGreen => new EmbedColor(255, 0, 100);
        public static EmbedColor DarkKhaki => new EmbedColor(255, 189, 183);
        public static EmbedColor DarkMagenta => new EmbedColor(255, 139, 0);
        public static EmbedColor DarkOliveGreen => new EmbedColor(255, 85, 107);
        public static EmbedColor DarkOrange => new EmbedColor(255, 255, 140);
        public static EmbedColor DarkOrchid => new EmbedColor(255, 153, 50);
        public static EmbedColor DarkRed => new EmbedColor(255, 139, 0);
        public static EmbedColor DarkSalmon => new EmbedColor(255, 233, 150);
        public static EmbedColor DarkSeaGreen => new EmbedColor(255, 143, 188);
        public static EmbedColor DarkSlateBlue => new EmbedColor(255, 72, 61);
        public static EmbedColor DarkSlateGray => new EmbedColor(255, 47, 79);
        public static EmbedColor DarkTurquoise => new EmbedColor(255, 0, 206);
        public static EmbedColor DarkViolet => new EmbedColor(255, 148, 0);
        public static EmbedColor DeepPink => new EmbedColor(255, 255, 20);
        public static EmbedColor DeepSkyBlue => new EmbedColor(255, 0, 191);
        public static EmbedColor DimGray => new EmbedColor(255, 105, 105);
        public static EmbedColor DodgerBlue => new EmbedColor(255, 30, 144);
        public static EmbedColor Firebrick => new EmbedColor(255, 178, 34);
        public static EmbedColor FloralWhite => new EmbedColor(255, 255, 250);
        public static EmbedColor ForestGreen => new EmbedColor(255, 34, 139);
        public static EmbedColor Fuchsia => new EmbedColor(255, 255, 0);
        public static EmbedColor Gainsboro => new EmbedColor(255, 220, 220);
        public static EmbedColor GhostWhite => new EmbedColor(255, 248, 248);
        public static EmbedColor Gold => new EmbedColor(255, 255, 215);
        public static EmbedColor Goldenrod => new EmbedColor(255, 218, 165);
        public static EmbedColor Gray => new EmbedColor(255, 128, 128);
        public static EmbedColor Green => new EmbedColor(255, 0, 128);
        public static EmbedColor GreenYellow => new EmbedColor(255, 173, 255);
        public static EmbedColor Honeydew => new EmbedColor(255, 240, 255);
        public static EmbedColor HotPink => new EmbedColor(255, 255, 105);
        public static EmbedColor IndianRed => new EmbedColor(255, 205, 92);
        public static EmbedColor Indigo => new EmbedColor(255, 75, 0);
        public static EmbedColor Ivory => new EmbedColor(255, 255, 255);
        public static EmbedColor Khaki => new EmbedColor(255, 240, 230);
        public static EmbedColor Lavender => new EmbedColor(255, 230, 230);
        public static EmbedColor LavenderBlush => new EmbedColor(255, 255, 240);
        public static EmbedColor LawnGreen => new EmbedColor(255, 124, 252);
        public static EmbedColor LemonChiffon => new EmbedColor(255, 255, 250);
        public static EmbedColor LightBlue => new EmbedColor(255, 173, 216);
        public static EmbedColor LightCoral => new EmbedColor(255, 240, 128);
        public static EmbedColor LightCyan => new EmbedColor(255, 224, 255);
        public static EmbedColor LightGoldenrodYellow => new EmbedColor(255, 250, 250);
        public static EmbedColor LightGray => new EmbedColor(255, 211, 211);
        public static EmbedColor LightGreen => new EmbedColor(255, 144, 238);
        public static EmbedColor LightPink => new EmbedColor(255, 255, 182);
        public static EmbedColor LightSalmon => new EmbedColor(255, 255, 160);
        public static EmbedColor LightSeaGreen => new EmbedColor(255, 32, 178);
        public static EmbedColor LightSkyBlue => new EmbedColor(255, 135, 206);
        public static EmbedColor LightSlateGray => new EmbedColor(255, 119, 136);
        public static EmbedColor LightSteelBlue => new EmbedColor(255, 176, 196);
        public static EmbedColor LightYellow => new EmbedColor(255, 255, 255);
        public static EmbedColor Lime => new EmbedColor(255, 0, 255);
        public static EmbedColor LimeGreen => new EmbedColor(255, 50, 205);
        public static EmbedColor Linen => new EmbedColor(255, 250, 240);
        public static EmbedColor Magenta => new EmbedColor(255, 255, 0);
        public static EmbedColor Maroon => new EmbedColor(255, 128, 0);
        public static EmbedColor MediumAquamarine => new EmbedColor(255, 102, 205);
        public static EmbedColor MediumBlue => new EmbedColor(255, 0, 0);
        public static EmbedColor MediumOrchid => new EmbedColor(255, 186, 85);
        public static EmbedColor MediumPurple => new EmbedColor(255, 147, 112);
        public static EmbedColor MediumSeaGreen => new EmbedColor(255, 60, 179);
        public static EmbedColor MediumSlateBlue => new EmbedColor(255, 123, 104);
        public static EmbedColor MediumSpringGreen => new EmbedColor(255, 0, 250);
        public static EmbedColor MediumTurquoise => new EmbedColor(255, 72, 209);
        public static EmbedColor MediumVioletRed => new EmbedColor(255, 199, 21);
        public static EmbedColor MidnightBlue => new EmbedColor(255, 25, 25);
        public static EmbedColor MintCream => new EmbedColor(255, 245, 255);
        public static EmbedColor MistyRose => new EmbedColor(255, 255, 228);
        public static EmbedColor Moccasin => new EmbedColor(255, 255, 228);
        public static EmbedColor NavajoWhite => new EmbedColor(255, 255, 222);
        public static EmbedColor Navy => new EmbedColor(255, 0, 0);
        public static EmbedColor OldLace => new EmbedColor(255, 253, 245);
        public static EmbedColor Olive => new EmbedColor(255, 128, 128);
        public static EmbedColor OliveDrab => new EmbedColor(255, 107, 142);
        public static EmbedColor Orange => new EmbedColor(255, 255, 165);
        public static EmbedColor OrangeRed => new EmbedColor(255, 255, 69);
        public static EmbedColor Orchid => new EmbedColor(255, 218, 112);
        public static EmbedColor PaleGoldenrod => new EmbedColor(255, 238, 232);
        public static EmbedColor PaleGreen => new EmbedColor(255, 152, 251);
        public static EmbedColor PaleTurquoise => new EmbedColor(255, 175, 238);
        public static EmbedColor PaleVioletRed => new EmbedColor(255, 219, 112);
        public static EmbedColor PapayaWhip => new EmbedColor(255, 255, 239);
        public static EmbedColor PeachPuff => new EmbedColor(255, 255, 218);
        public static EmbedColor Peru => new EmbedColor(255, 205, 133);
        public static EmbedColor Pink => new EmbedColor(255, 255, 192);
        public static EmbedColor Plum => new EmbedColor(255, 221, 160);
        public static EmbedColor PowderBlue => new EmbedColor(255, 176, 224);
        public static EmbedColor Purple => new EmbedColor(255, 128, 0);
        public static EmbedColor Red => new EmbedColor(255, 255, 0);
        public static EmbedColor RosyBrown => new EmbedColor(255, 188, 143);
        public static EmbedColor RoyalBlue => new EmbedColor(255, 65, 105);
        public static EmbedColor SaddleBrown => new EmbedColor(255, 139, 69);
        public static EmbedColor Salmon => new EmbedColor(255, 250, 128);
        public static EmbedColor SandyBrown => new EmbedColor(255, 244, 164);
        public static EmbedColor SeaGreen => new EmbedColor(255, 46, 139);
        public static EmbedColor SeaShell => new EmbedColor(255, 255, 245);
        public static EmbedColor Sienna => new EmbedColor(255, 160, 82);
        public static EmbedColor Silver => new EmbedColor(255, 192, 192);
        public static EmbedColor SkyBlue => new EmbedColor(255, 135, 206);
        public static EmbedColor SlateBlue => new EmbedColor(255, 106, 90);
        public static EmbedColor SlateGray => new EmbedColor(255, 112, 128);
        public static EmbedColor Snow => new EmbedColor(255, 255, 250);
        public static EmbedColor SpringGreen => new EmbedColor(255, 0, 255);
        public static EmbedColor SteelBlue => new EmbedColor(255, 70, 130);
        public static EmbedColor Tan => new EmbedColor(255, 210, 180);
        public static EmbedColor Teal => new EmbedColor(255, 0, 128);
        public static EmbedColor Thistle => new EmbedColor(255, 216, 191);
        public static EmbedColor Tomato => new EmbedColor(255, 255, 99);
        public static EmbedColor Transparent => new EmbedColor(0, 255, 255);
        public static EmbedColor Turquoise => new EmbedColor(255, 64, 224);
        public static EmbedColor Violet => new EmbedColor(255, 238, 130);
        public static EmbedColor Wheat => new EmbedColor(255, 245, 222);
        public static EmbedColor White => new EmbedColor(255, 255, 255);
        public static EmbedColor WhiteSmoke => new EmbedColor(255, 245, 245);
        public static EmbedColor Yellow => new EmbedColor(255, 255, 255);
        public static EmbedColor YellowGreen => new EmbedColor(255, 154, 205);

    }
}