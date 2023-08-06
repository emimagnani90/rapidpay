namespace RapidPay.ConfigurationSections
{
    public class JwtSettings
    {
        public string Sub { get; set; }
        public string Issuer { get;set; }
        public string Audience { get; set; }
        public string Key { get; set; }
    }
}
