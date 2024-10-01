using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Diagnostics;
using AeroDynasty.Enums;
using System.Text.Json.Serialization;

namespace AeroDynasty.Services
{
    public static class JSONDataLoader
    {
        public static List<T> LoadFromFile<T>(string filePath)
        {
            try
            {
                // Read the JSON data from the file
                string jsonData = File.ReadAllText(filePath);

                // Create JsonSerializerOptions and add custom converters if necessary
                var options = new JsonSerializerOptions
                {
                    Converters = { new ContinentConverter() } // Add converter for Continent
                };

                // Deserialize the JSON data to a list of the specified type (T)
                return JsonSerializer.Deserialize<List<T>>(jsonData, options);
            }
            catch (Exception ex)
            {
                // Handle exceptions like file not found or incorrect format
                Trace.WriteLine($"Error loading data from {filePath}: {ex.Message}");
                return new List<T>();
            }
        }
    }

    // Custom converter for the Continent enum
    public class ContinentConverter : JsonConverter<Continent>
    {
        public override Continent Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var continentString = reader.GetString();

            // Try to parse the string to the Continent enum
            if (Enum.TryParse<Continent>(continentString, true, out var continent))
            {
                return continent;
            }

            throw new JsonException($"Unable to convert \"{continentString}\" to {nameof(Continent)}.");
        }

        public override void Write(Utf8JsonWriter writer, Continent value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
