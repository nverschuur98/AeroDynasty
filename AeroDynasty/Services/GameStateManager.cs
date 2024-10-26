using AeroDynasty.Models.RouteModels;
using AeroDynasty.Models.AirlineModels;
using AeroDynasty.Models.AirlinerModels;
using AeroDynasty.Models.AirportModels;
using AeroDynasty.Models.Core;
using System.IO;
using System.Text.Json;
using System.Collections.ObjectModel;
using System.Text.Json.Serialization;
using AeroDynasty.Models.AircraftModels;
using AeroDynasty.Enums;
using AeroDynasty.Views;

namespace AeroDynasty.Services
{
    public class GameStateManager
    {
        // The GameState model to hold data for saving/loading
        public class GameState
        {
            public DateTime CurrentDate { get; set; }
            public UserData UserData { get; set; }
            public List<Airline> Airlines { get; set; }
            public List<Airliner> Airliners { get; set; }
            public List<Route> Routes { get; set; }

            public GameState()
            {
                Airliners = new List<Airliner>(GameData.Instance.Airliners);
                Airlines = new List<Airline>(GameData.Instance.Airlines);
                Routes = new List<Route>(GameData.Instance.Routes);
                UserData = GameData.Instance.UserData;
                CurrentDate = GameData.Instance.CurrentDate;
            }
        }

        // Save the current game state to a JSON file
        public static async Task SaveGame(GameData gameData, string filePath)
        {
            var gameState = new GameState();
            string json;

            var options = new JsonSerializerOptions
            {
                WriteIndented = true, // For easier reading if you open the JSON
                Converters =
                {
                    new AirlinerConverter(),
                    new CountryConverter(),
                    new AirportConverter(),
                    new RouteConverter(),
                    new AirlineConverter(),
                    new UserDataConverter()
                }
            };

            try
            {
                json = JsonSerializer.Serialize(gameState, options);
                await File.WriteAllTextAsync(filePath, json);
            }
            catch (Exception ex)
            {

                throw new Exception($"Error while creating save game: {ex.Message}");
            }

        }

        // Load the game state from a JSON file and apply it to the GameViewModel
        public static async Task LoadGame(GameData gameData, string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("Save file not found.");

            string json = await File.ReadAllTextAsync(filePath);

            // Deserialize JSON into GameState object
            JsonDocument SaveFile = JsonDocument.Parse(json);
            JsonElement gameState;

            if (SaveFile != null)
            {
                gameState = SaveFile.RootElement;

                try
                {
                    // Assign deserialized date
                    GameData.Instance.CurrentDate = Convert.ToDateTime(gameState.GetProperty("CurrentDate").GetDateTime());
                }
                catch (Exception e)
                {

                    throw new Exception(e.Message);
                }

                //Get the user airline
                try
                {
                    GameData.Instance.UserData.Airline = GameData.Instance.Airlines.Where(a => a.Name == gameState.GetProperty("UserData").GetProperty("Airline").ToString()).FirstOrDefault();
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }

                //Get the airline data
                try
                {
                    foreach(JsonElement airline in gameState.GetProperty("Airlines").EnumerateArray())
                    {
                        Airline air = GameData.Instance.Airlines.Where(a => a.Name == airline.GetProperty("Name").ToString()).First();
                        air.CashBalance = Convert.ToDouble(airline.GetProperty("CashBalance").ToString());
                        air.Reputation = Convert.ToDouble(airline.GetProperty("Reputation").ToString());
                    }
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }

                //Get the airliner data
                try
                {
                    foreach (JsonElement airliner in gameState.GetProperty("Airliners").EnumerateArray())
                    {
                        Airliner air = new Airliner();
                        air.Owner = GameData.Instance.Airlines.Where(a => a.Name == airliner.GetProperty("Owner").ToString()).First();
                        air.Model = GameData.Instance.AircraftModels.Where(ac => ac.Name == airliner.GetProperty("Model").ToString()).First();
                        air.Registration = new Registration(air.Owner.Country, Convert.ToInt32(airliner.GetProperty("RegistrationNumber").ToString()));

                        GameData.Instance.Airliners.Add(air);
                    }
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }

                //Get the routes
                try
                {
                    foreach(JsonElement route in gameState.GetProperty("Routes").EnumerateArray())
                    {
                        Route R = new Route();
                        R.Origin = GameData.Instance.Airports.Where(air => air.ICAO == route.GetProperty("Origin_ICAO").ToString()).First();
                        R.Destination = GameData.Instance.Airports.Where(air => air.ICAO == route.GetProperty("Destination_ICAO").ToString()).First();
                        R.routeOwner = GameData.Instance.Airlines.Where(air => air.Name == route.GetProperty("Owner").ToString()).First();
                        R.ticketPrice = Convert.ToDouble(route.GetProperty("TicketPrice").ToString());
                        R.flightFrequency = Convert.ToInt32(route.GetProperty("FlightFrequency").ToString());

                        GameData.Instance.Routes.Add(R);
                    }
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }

            }
        }
    }

    #region Custom Serializers

    public class AirlineConverter : JsonConverter<Airline>
    {
        //public string Name { get; set; }
        //public double Reputation { get; set; }
        //public double CashBalance

        public override Airline Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Implement custom deserialization logic if necessary
            //return JsonSerializer.Deserialize<Airline>(ref reader, options);
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, Airline value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("Name", value.Name);
            writer.WriteString("Reputation", value.Reputation.ToString());
            writer.WriteString("CashBalance", value.CashBalance.ToString());
            writer.WriteEndObject();
        }

    } 

    public class AirlinerConverter : JsonConverter<Airliner>
    {
        //public Registration Registration {  get; set; }
        //public AircraftModel Model { get; set; }
        //public Airline Owner { get; set; }

        public override Airliner? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, Airliner value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("RegistrationNumber", value.Registration.Number.ToString());
            writer.WriteString("Model", value.Model.Name);
            writer.WriteString("Owner", value.Owner.Name);
            writer.WriteEndObject();
        }
    }

    public class CountryConverter : JsonConverter<Country>
    {
        //public string Name { get; set; }
        //public string ISOCode { get; set; }
        //public Continent Continent { get; set; }

        public override Country? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, Country value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("ISOCode", value.ISOCode.ToString());
            writer.WriteEndObject();
        }
    }

    public class AirportConverter : JsonConverter<Airport>
    {
        //public string Name { get; set; }
        //public string IATA { get; set; }
        //public string ICAO { get; set; }
        //public Country Country { get; set; }
        //public Coordinates Coordinates { get; set; }
        //public double demandFactor { get; set; }

        public override Airport? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, Airport value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("ICAO", value.ICAO.ToString());
            writer.WriteEndObject();
        }
    }

    public class RouteConverter : JsonConverter<Route>
    {
        //public Airport Origin { get; set; }
        //public Airport Destination { get; set; }
        //public Airline routeOwner { get; set; }
        //public Airliner assignedAirliner { get; set; }
        //public double ticketPrice { get; set; }
        //public int flightFrequency { get; set; }

        public override Route? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, Route value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("Origin_ICAO", value.Origin.ICAO);
            writer.WriteString("Destination_ICAO", value.Destination.ICAO);
            writer.WriteString("Owner", value.routeOwner.Name);
            writer.WriteString("TicketPrice", value.ticketPrice.ToString());
            writer.WriteString("FlightFrequency", value.flightFrequency.ToString());
            writer.WriteEndObject();
        }
    }

    public class UserDataConverter : JsonConverter<UserData>
    {
        //public Airline Airline { get; private set; }

        public override UserData? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, UserData value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("Airline", value.Airline.Name);
            writer.WriteEndObject();
        }
    }

    #endregion
}
