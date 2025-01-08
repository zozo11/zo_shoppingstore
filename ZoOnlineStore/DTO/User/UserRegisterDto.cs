using System;
using System.Reflection.Metadata;

namespace ZoOnlineStore.DTO.User;

public class UserRegisterDto
{
    public required string Username{ get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string Role { get; set;}
}
