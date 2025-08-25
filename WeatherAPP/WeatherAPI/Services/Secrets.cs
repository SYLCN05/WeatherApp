namespace WeatherAPI.Services
{
    public static class Secrets
    {

        private static IConfiguration configuration;

        static Secrets() 
        {
            configuration = new ConfigurationBuilder()
                .AddUserSecrets<Program>()
                .Build();
        }

        public static string OpenWeatherApiKey()
        {
          return  configuration["OpenWeather:API_KEY"] ?? throw new Exception("API key niet gevonden!");
        }
    }
}
