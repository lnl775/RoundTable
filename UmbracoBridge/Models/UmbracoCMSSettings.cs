namespace UmbracoBridge.Models
{
    public class UmbracoCMSSettings
    {
        public const string SettingNameSection = "UmbracoCMSSettings";
        public string BaseUrl { get; set; }
        public string GrantType { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}
