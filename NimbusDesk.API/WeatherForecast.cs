namespace NimbusDesk.API
{
    /// <summary>
    /// Represents weather forecast data for a specific date.
    /// This is a sample class included in the default ASP.NET Core template.
    /// </summary>
    public class WeatherForecast
    {
        /// <summary>Gets or sets the date of the weather forecast.</summary>
        public DateOnly Date { get; set; }

        /// <summary>Gets or sets the temperature in Celsius.</summary>
        public int TemperatureC { get; set; }

        /// <summary>
        /// Gets the temperature in Fahrenheit, calculated from the Celsius temperature.
        /// </summary>
        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        /// <summary>Gets or sets a summary description of the weather forecast.</summary>
        public string? Summary { get; set; }
    }
}
