using Newtonsoft.Json;

namespace Instagram.api.Utils
{
	using System;

	public static class UnixDateTimeHelper
	{
		private const string InvalidUnixEpochErrorMessage = "Unix epoc starts January 1st, 1970";
		/// <summary>
		///   Convert a long into a DateTime
		/// </summary>
		public static DateTime FromUnixTime(this Int64 self)
		{
			var ret = new DateTime(1970, 1, 1);
			return ret.AddSeconds(self);
		}

		/// <summary>
		///   Convert a DateTime into a long
		/// </summary>
		public static Int64 ToUnixTime(this DateTime self)
		{

			if (self == DateTime.MinValue)
			{
				return 0;
			}

			var epoc = new DateTime(1970, 1, 1);
			var delta = self - epoc;

			if (delta.TotalSeconds < 0) throw new ArgumentOutOfRangeException(InvalidUnixEpochErrorMessage);

			return (long)delta.TotalSeconds;
		}
	}

	/// <summary>
	/// Converts date strings returned by the Twitter API into <see cref="System.DateTime"/>
	/// </summary>
	public class InstagramDateConverter : Newtonsoft.Json.Converters.DateTimeConverterBase
	{
		/// <summary>
		/// Reads the json.
		/// </summary>
		/// <param name="reader">The reader.</param>
		/// <param name="objectType">Type of the object.</param>
		/// <param name="existingValue">The existing value.</param>
		/// <param name="serializer">The serializer.</param>
		/// <returns>The parsed value as a DateTime, or null.</returns>
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			if (reader.TokenType != JsonToken.String)
				throw new Exception("Wrong Token Type");

			var ticks = (long)Double.Parse((string) reader.Value);
			return ticks.FromUnixTime();
		}

		/// <summary>
		/// Writes the json.
		/// </summary>
		/// <param name="writer">The writer.</param>
		/// <param name="value">The value.</param>
		/// <param name="serializer">The serializer.</param>
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			if (value.GetType() != typeof(DateTime))
				throw new ArgumentOutOfRangeException("value", "The value provided was not the expected data type.");

			writer.WriteValue(((DateTime)value).ToUnixTime());
		}
	}
}
