using System;
using System.Globalization;

namespace ShimmyMySherbet.DiscordWebhook.Helpers
{
	/// <summary>
	/// Provides a few helper methods for interfacing with discord
	/// </summary>
	public static class DiscordHelpers
	{
		/// <summary>
		/// Formats a <see cref="DateTime"/> to the ISO DateTime format.
		/// </summary>
		/// <param name="dateTime">DateTime object to format</param>
		/// <returns>ISO Date Time string</returns>
		/// <remarks>
		/// Note: This is a direct conversion, ensure the <seealso cref="DateTime"/> object is in UTC time
		/// </remarks>
		public static string DateTimeToISO(DateTime dateTime)
		{
			return DateTimeToISO(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second);
		}

		/// <summary>
		/// Creates an ISO date time string
		/// </summary>
		/// <param name="year">Year</param>
		/// <param name="month">Month (1-12)</param>
		/// <param name="day">Day of the month (1-31)</param>
		/// <param name="hour">Hour of day (1-12)</param>
		/// <param name="minute">Minute of the hour (0-59)</param>
		/// <param name="second">Second of the minute (0-59)</param>
		/// <returns>DateTime string</returns>
		public static string DateTimeToISO(int year, int month, int day, int hour, int minute, int second)
		{
			return new DateTime(year, month, day, hour, minute, second, 0, DateTimeKind.Local)
				.ToString("yyyy-MM-dd'T'HH:mm:ss.fffK", CultureInfo.InvariantCulture);
		}
	}
}
