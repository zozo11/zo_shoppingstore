using System;

namespace ZoOnlineStore.Models;

public class User
{
        public int ID { get; set; }
        public string UUID { get; set; } = Guid.NewGuid().ToString();
        public required string Username { get; set; }
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }
        public required string Role { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public ICollection<Order>? Orders { get; set; }
        public ICollection<Token>? Tokens { get; set; }
}
