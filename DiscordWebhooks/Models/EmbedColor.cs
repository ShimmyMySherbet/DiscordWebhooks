using System;
using System.Collections.Generic;
using System.Text;

namespace ShimmyMySherbet.DiscordWebhooks.Models
{
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

		public static EmbedColor Transparent => new EmbedColor(255, 255, 255);
		public static EmbedColor AliceBlue => new EmbedColor(240, 248, 255);
		public static EmbedColor AntiqueWhite => new EmbedColor(250, 235, 215);
		public static EmbedColor Aqua => new EmbedColor(0, 255, 255);
		public static EmbedColor Aquamarine => new EmbedColor(127, 255, 212);
		public static EmbedColor Azure => new EmbedColor(240, 255, 255);
		public static EmbedColor Beige => new EmbedColor(245, 245, 220);
		public static EmbedColor Bisque => new EmbedColor(255, 228, 196);
		public static EmbedColor Black => new EmbedColor(0, 0, 0);
		public static EmbedColor BlanchedAlmond => new EmbedColor(255, 235, 205);
		public static EmbedColor Blue => new EmbedColor(0, 0, 255);
		public static EmbedColor BlueViolet => new EmbedColor(138, 43, 226);
		public static EmbedColor Brown => new EmbedColor(165, 42, 42);
		public static EmbedColor BurlyWood => new EmbedColor(222, 184, 135);
		public static EmbedColor CadetBlue => new EmbedColor(95, 158, 160);
		public static EmbedColor Chartreuse => new EmbedColor(127, 255, 0);
		public static EmbedColor Chocolate => new EmbedColor(210, 105, 30);
		public static EmbedColor Coral => new EmbedColor(255, 127, 80);
		public static EmbedColor CornflowerBlue => new EmbedColor(100, 149, 237);
		public static EmbedColor Cornsilk => new EmbedColor(255, 248, 220);
		public static EmbedColor Crimson => new EmbedColor(220, 20, 60);
		public static EmbedColor Cyan => new EmbedColor(0, 255, 255);
		public static EmbedColor DarkBlue => new EmbedColor(0, 0, 139);
		public static EmbedColor DarkCyan => new EmbedColor(0, 139, 139);
		public static EmbedColor DarkGoldenrod => new EmbedColor(184, 134, 11);
		public static EmbedColor DarkGray => new EmbedColor(169, 169, 169);
		public static EmbedColor DarkGreen => new EmbedColor(0, 100, 0);
		public static EmbedColor DarkKhaki => new EmbedColor(189, 183, 107);
		public static EmbedColor DarkMagenta => new EmbedColor(139, 0, 139);
		public static EmbedColor DarkOliveGreen => new EmbedColor(85, 107, 47);
		public static EmbedColor DarkOrange => new EmbedColor(255, 140, 0);
		public static EmbedColor DarkOrchid => new EmbedColor(153, 50, 204);
		public static EmbedColor DarkRed => new EmbedColor(139, 0, 0);
		public static EmbedColor DarkSalmon => new EmbedColor(233, 150, 122);
		public static EmbedColor DarkSeaGreen => new EmbedColor(143, 188, 143);
		public static EmbedColor DarkSlateBlue => new EmbedColor(72, 61, 139);
		public static EmbedColor DarkSlateGray => new EmbedColor(47, 79, 79);
		public static EmbedColor DarkTurquoise => new EmbedColor(0, 206, 209);
		public static EmbedColor DarkViolet => new EmbedColor(148, 0, 211);
		public static EmbedColor DeepPink => new EmbedColor(255, 20, 147);
		public static EmbedColor DeepSkyBlue => new EmbedColor(0, 191, 255);
		public static EmbedColor DimGray => new EmbedColor(105, 105, 105);
		public static EmbedColor DodgerBlue => new EmbedColor(30, 144, 255);
		public static EmbedColor Firebrick => new EmbedColor(178, 34, 34);
		public static EmbedColor FloralWhite => new EmbedColor(255, 250, 240);
		public static EmbedColor ForestGreen => new EmbedColor(34, 139, 34);
		public static EmbedColor Fuchsia => new EmbedColor(255, 0, 255);
		public static EmbedColor Gainsboro => new EmbedColor(220, 220, 220);
		public static EmbedColor GhostWhite => new EmbedColor(248, 248, 255);
		public static EmbedColor Gold => new EmbedColor(255, 215, 0);
		public static EmbedColor Goldenrod => new EmbedColor(218, 165, 32);
		public static EmbedColor Gray => new EmbedColor(128, 128, 128);
		public static EmbedColor Green => new EmbedColor(0, 128, 0);
		public static EmbedColor GreenYellow => new EmbedColor(173, 255, 47);
		public static EmbedColor Honeydew => new EmbedColor(240, 255, 240);
		public static EmbedColor HotPink => new EmbedColor(255, 105, 180);
		public static EmbedColor IndianRed => new EmbedColor(205, 92, 92);
		public static EmbedColor Indigo => new EmbedColor(75, 0, 130);
		public static EmbedColor Ivory => new EmbedColor(255, 255, 240);
		public static EmbedColor Khaki => new EmbedColor(240, 230, 140);
		public static EmbedColor Lavender => new EmbedColor(230, 230, 250);
		public static EmbedColor LavenderBlush => new EmbedColor(255, 240, 245);
		public static EmbedColor LawnGreen => new EmbedColor(124, 252, 0);
		public static EmbedColor LemonChiffon => new EmbedColor(255, 250, 205);
		public static EmbedColor LightBlue => new EmbedColor(173, 216, 230);
		public static EmbedColor LightCoral => new EmbedColor(240, 128, 128);
		public static EmbedColor LightCyan => new EmbedColor(224, 255, 255);
		public static EmbedColor LightGoldenrodYellow => new EmbedColor(250, 250, 210);
		public static EmbedColor LightGreen => new EmbedColor(144, 238, 144);
		public static EmbedColor LightGray => new EmbedColor(211, 211, 211);
		public static EmbedColor LightPink => new EmbedColor(255, 182, 193);
		public static EmbedColor LightSalmon => new EmbedColor(255, 160, 122);
		public static EmbedColor LightSeaGreen => new EmbedColor(32, 178, 170);
		public static EmbedColor LightSkyBlue => new EmbedColor(135, 206, 250);
		public static EmbedColor LightSlateGray => new EmbedColor(119, 136, 153);
		public static EmbedColor LightSteelBlue => new EmbedColor(176, 196, 222);
		public static EmbedColor LightYellow => new EmbedColor(255, 255, 224);
		public static EmbedColor Lime => new EmbedColor(0, 255, 0);
		public static EmbedColor LimeGreen => new EmbedColor(50, 205, 50);
		public static EmbedColor Linen => new EmbedColor(250, 240, 230);
		public static EmbedColor Magenta => new EmbedColor(255, 0, 255);
		public static EmbedColor Maroon => new EmbedColor(128, 0, 0);
		public static EmbedColor MediumAquamarine => new EmbedColor(102, 205, 170);
		public static EmbedColor MediumBlue => new EmbedColor(0, 0, 205);
		public static EmbedColor MediumOrchid => new EmbedColor(186, 85, 211);
		public static EmbedColor MediumPurple => new EmbedColor(147, 112, 219);
		public static EmbedColor MediumSeaGreen => new EmbedColor(60, 179, 113);
		public static EmbedColor MediumSlateBlue => new EmbedColor(123, 104, 238);
		public static EmbedColor MediumSpringGreen => new EmbedColor(0, 250, 154);
		public static EmbedColor MediumTurquoise => new EmbedColor(72, 209, 204);
		public static EmbedColor MediumVioletRed => new EmbedColor(199, 21, 133);
		public static EmbedColor MidnightBlue => new EmbedColor(25, 25, 112);
		public static EmbedColor MintCream => new EmbedColor(245, 255, 250);
		public static EmbedColor MistyRose => new EmbedColor(255, 228, 225);
		public static EmbedColor Moccasin => new EmbedColor(255, 228, 181);
		public static EmbedColor NavajoWhite => new EmbedColor(255, 222, 173);
		public static EmbedColor Navy => new EmbedColor(0, 0, 128);
		public static EmbedColor OldLace => new EmbedColor(253, 245, 230);
		public static EmbedColor Olive => new EmbedColor(128, 128, 0);
		public static EmbedColor OliveDrab => new EmbedColor(107, 142, 35);
		public static EmbedColor Orange => new EmbedColor(255, 165, 0);
		public static EmbedColor OrangeRed => new EmbedColor(255, 69, 0);
		public static EmbedColor Orchid => new EmbedColor(218, 112, 214);
		public static EmbedColor PaleGoldenrod => new EmbedColor(238, 232, 170);
		public static EmbedColor PaleGreen => new EmbedColor(152, 251, 152);
		public static EmbedColor PaleTurquoise => new EmbedColor(175, 238, 238);
		public static EmbedColor PaleVioletRed => new EmbedColor(219, 112, 147);
		public static EmbedColor PapayaWhip => new EmbedColor(255, 239, 213);
		public static EmbedColor PeachPuff => new EmbedColor(255, 218, 185);
		public static EmbedColor Peru => new EmbedColor(205, 133, 63);
		public static EmbedColor Pink => new EmbedColor(255, 192, 203);
		public static EmbedColor Plum => new EmbedColor(221, 160, 221);
		public static EmbedColor PowderBlue => new EmbedColor(176, 224, 230);
		public static EmbedColor Purple => new EmbedColor(128, 0, 128);
		public static EmbedColor RebeccaPurple => new EmbedColor(102, 51, 153);
		public static EmbedColor Red => new EmbedColor(255, 0, 0);
		public static EmbedColor RosyBrown => new EmbedColor(188, 143, 143);
		public static EmbedColor RoyalBlue => new EmbedColor(65, 105, 225);
		public static EmbedColor SaddleBrown => new EmbedColor(139, 69, 19);
		public static EmbedColor Salmon => new EmbedColor(250, 128, 114);
		public static EmbedColor SandyBrown => new EmbedColor(244, 164, 96);
		public static EmbedColor SeaGreen => new EmbedColor(46, 139, 87);
		public static EmbedColor SeaShell => new EmbedColor(255, 245, 238);
		public static EmbedColor Sienna => new EmbedColor(160, 82, 45);
		public static EmbedColor Silver => new EmbedColor(192, 192, 192);
		public static EmbedColor SkyBlue => new EmbedColor(135, 206, 235);
		public static EmbedColor SlateBlue => new EmbedColor(106, 90, 205);
		public static EmbedColor SlateGray => new EmbedColor(112, 128, 144);
		public static EmbedColor Snow => new EmbedColor(255, 250, 250);
		public static EmbedColor SpringGreen => new EmbedColor(0, 255, 127);
		public static EmbedColor SteelBlue => new EmbedColor(70, 130, 180);
		public static EmbedColor Tan => new EmbedColor(210, 180, 140);
		public static EmbedColor Teal => new EmbedColor(0, 128, 128);
		public static EmbedColor Thistle => new EmbedColor(216, 191, 216);
		public static EmbedColor Tomato => new EmbedColor(255, 99, 71);
		public static EmbedColor Turquoise => new EmbedColor(64, 224, 208);
		public static EmbedColor Violet => new EmbedColor(238, 130, 238);
		public static EmbedColor Wheat => new EmbedColor(245, 222, 179);
		public static EmbedColor White => new EmbedColor(255, 255, 255);
		public static EmbedColor WhiteSmoke => new EmbedColor(245, 245, 245);
		public static EmbedColor Yellow => new EmbedColor(255, 255, 0);
		public static EmbedColor YellowGreen => new EmbedColor(154, 205, 50);
	}
}
