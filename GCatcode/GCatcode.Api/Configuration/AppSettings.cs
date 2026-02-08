namespace GCatcode.Api.Configuration
{
    public class AppSettings
    {
        public ConnectionStrings DB { get; set; } = new();
        public JwtSettings JwtSettings { get; set; } = new();

        public string BackendName { get; set; } = string.Empty;
        public string Environment { get; set; } = string.Empty;
        public bool IsTesting { get; set; } = false;
        public string FRONT_PUBLIC_ORIGIN { get; set; } = string.Empty;
    }

    public class ConnectionStrings
    {
        public string DefaultConnection { get; set; } = string.Empty;
        public string LogsConnection { get; set; } = string.Empty;
    }

    public class JwtSettings
    {
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public string Key { get; set; } = string.Empty;
        public int AccessTokenExpirationMinutes { get; set; } = 60;
        public int RefreshTokenExpirationDays { get; set; } = 7;
    }
}