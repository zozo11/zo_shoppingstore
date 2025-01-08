using System;

namespace ZoOnlineStore.Data;
public class JwtSetting
{
    public required string SecretKey { get; set; }
    public string? Issuer { get; set; }
    public string? Audience { get; set; }
    public int ExpiryInMinutes { get; set; }
}