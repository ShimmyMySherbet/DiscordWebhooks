using System;
using System.Globalization;

// namespace required for backward compatibility
namespace ShimmyMySherbet.DiscordWebhooks.Embeded
{

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
}
