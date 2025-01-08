using System;

namespace ZoOnlineStore.Models;

public class Token
{
    public int ID { get; set; }
    public int UserID { get; set; } // 外键，关联到 User 表
    public required string TokenValue { get; set; } // 存储实际的令牌值
    public DateTime ExpiryDate { get; set; } // 令牌过期时间
    public required string Device { get; set; } // 可选：存储设备信息
    public required string IPAddress { get; set; } // 可选：存储登录时的 IP 地址
    public DateTime CreatedAt { get; set; } // 令牌创建时间

    public required User User { get; set; } // 导航属性
}
