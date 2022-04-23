using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace OnlineStore.Helper
{
    public class DataConvert : JsonConverter<DateTime>
    {
        private string formatDate = "d-M-yyyy";
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            try
            {
                return DateTime.ParseExact(reader.GetString(), formatDate, CultureInfo.InvariantCulture);

            }
            catch(FormatException) 
            {
                
                return DateTime.MinValue;
            }
        }


        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString(formatDate));
        }
    }
}
