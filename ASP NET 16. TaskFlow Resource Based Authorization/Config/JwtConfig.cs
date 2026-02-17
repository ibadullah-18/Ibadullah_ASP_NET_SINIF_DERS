namespace ASP_NET_16._TaskFlow_Resource_Based_Authorization.Config;

public class JwtConfig
{
    public const string SectionName = "JwtSettings";
    public string SecretKey { get; set; } = string.Empty;
    public string RefreshTokenSecretKey { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public int ExpiresInMinutes { get; set; } = 15;
    public int RefreshTokenExpiresInDays { get; set; } = 7;
}
