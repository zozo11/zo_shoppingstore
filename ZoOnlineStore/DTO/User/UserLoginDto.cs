using System;
namespace ZoOnlineStore.DTO.User;

public class UserLoginDto
{
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? Token { get; set; }
    public string Device{ get; set; } = string.Empty;
    public string IPAddress { get; set; } = string.Empty;
}
