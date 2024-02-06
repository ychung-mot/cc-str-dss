using AdvSol.Services.Dtos;
using AdvSol.Utils;
using System.Text.Json;

namespace AdvSol.Services
{
    public interface IAddressService
    {
        Task ValidateAddress(IAddress dto, Dictionary<string, List<string>> errors);
    }

    public class AddressService : IAddressService
    {
        private readonly HttpClient _httpClient;
        private string _geocoderUrl;

        public AddressService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _geocoderUrl = configuration.GetValue<string>("geocoder-url") ?? "https://geocoder.api.gov.bc.ca/addresses.geojson?addressString=";
        }

        public async Task ValidateAddress(IAddress dto, Dictionary<string, List<string>> errors)
        {            
            var address = $"{dto.StreetAddress} {dto.City} {dto.Province}";

            var response = "";
            try
            {
                response = await _httpClient.GetStringAsync($"{_geocoderUrl}{address}");
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
            }

            using (JsonDocument doc = JsonDocument.Parse(response))
            {
                var root = doc.RootElement;

                if (root.ValueKind == JsonValueKind.Object)
                {
                    if (root.TryGetProperty("features", out var features))
                    {
                        var feature = features[0];
                        var geometry = feature.GetProperty("geometry");
                        var coordinates = geometry.GetProperty("coordinates");

                        dto.Latitude = coordinates[0].GetDouble();
                        dto.Longitude = coordinates[1].GetDouble();

                        var properties = feature.GetProperty("properties");
                        var foundFaults = false;

                        if (properties.TryGetProperty("faults", out var faults))
                        {
                            if (faults.EnumerateArray().Count() > 0)
                            {
                                foundFaults = true;
                            }

                            //foreach (var fault in faults.EnumerateArray())
                            //{
                            //    var value = fault.GetProperty("value").GetInt32();
                            //    var element = fault.GetProperty("element").GetString();
                            //    var faultType = fault.GetProperty("fault").GetString();
                            //    var penalty = fault.GetProperty("penalty").GetInt32();

                            //    Console.WriteLine($"Fault: {faultType}, Element: {element}, Value: {value}, Penalty: {penalty}");
                            //}
                        }

                        var score = properties.GetProperty("score").GetInt32();
                        var matchPrecision = properties.GetProperty("matchPrecision").GetString();

                        if (score > 90 && matchPrecision == "CIVIC_NUMBER"|| matchPrecision == "BLOCK")
                        {
                            foundFaults = false;
                        } 

                        if (foundFaults || matchPrecision != "CIVIC_NUMBER")
                        {
                            errors.AddItem("Address", $"Unable to locate the address {address} – please consider removing any unit numbers if present.");
                        }

                    }
                }
            }
        }
    }
}
