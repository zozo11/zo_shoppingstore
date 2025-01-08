using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZoOnlineStore.Data;
using ZoOnlineStore.Models;
using ZoOnlineStore.DTO.User;
using BCrypt.Net;
using System.Data;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.DataProtection;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;

namespace ZoOnlineStore.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly JwtSetting _jwtSettings;
    public UsersController (AppDbContext context, IOptions<JwtSetting> jwtSetting){
        _context = context;
        _jwtSettings = jwtSetting.Value;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegisterDto dto){
        if(await _context.Users.AnyAsync(u => u.Email == dto.Email)){
            return BadRequest ("Email already exist.");
        }

        var user = new User
        {
            Username = dto.Username,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = "ge_member",
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok("User registered successfully.");
    }

    [HttpPost("auth")]
    public async Task<IActionResult> Auth([FromBody] UserLoginDto dto){
        if(!string.IsNullOrEmpty(dto.Token)){
            var token = await _context.Tokens
                        .Where(t => t.TokenValue == dto.Token 
                                && t.ExpiryDate > DateTime.UtcNow
                                && t.Device == dto.Device)
                        .OrderByDescending(t => t.CreatedAt)
                        .FirstOrDefaultAsync();

            if(token != null){
                return Ok(new{
                    Message = "Token is Valid",
                    UserName = token.User.Username,
                    Email = token.User.Email
                });
            }else{
                return Unauthorized(new{
                    Message = "Invalid token. Please log in again.",
                    RedirectUrl = "/login"
                });
            }
        }
        return BadRequest(" Invalid token missing credentials.");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDto dto){    
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
        if(user == null){
            return BadRequest("Invalid email or password.");
        }

        if(!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash) ){
            return BadRequest("Invalid email or password.");
        }   

        //new token 
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var tokenid = new JwtSecurityToken(
            issuer: "local_issuer",
            audience: "local_audience",
            expires: DateTime.Now.AddHours(1),
            signingCredentials : credentials
        );
        var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenid);

        //create new token record
        var token = new Token{
            UserID = user.ID,
            TokenValue = tokenString,
            ExpiryDate = DateTime.UtcNow.AddHours(1),
            Device = dto.Device ?? "Unknown",
            IPAddress = dto.IPAddress ?? "Unknown",
            User = user,
        };

        _context.Tokens.Add(token);
        await _context.SaveChangesAsync();

        return Ok(new{
            Message = "Login successful.", 
            Token = token.TokenValue 
        });
    }
}
