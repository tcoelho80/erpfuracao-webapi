namespace ERP.Furacao.Domain.Settings
{
    public class JWTSettings
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public double ValidForMinutes { get; set; }
        public double RefreshTokenValidForMinutes { get; set; }

    }
}
