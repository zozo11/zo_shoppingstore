using System;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Xunit;
using ZoOnlineStore.Controllers;
using ZoOnlineStore.Data;
using ZoOnlineStore.DTO.User;
using ZoOnlineStore.Models;
using MockQueryable.Moq;

namespace ZoOnlineStore.Tests.Controllers;

public class UsersControllerTests
{
    private readonly UsersController _controller;
    private readonly AppDbContext _context;

    public UsersControllerTests()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;

        //INIT USER
        var mockUsers = new List<User>{
            new User {ID = 1, Email="test1@test.com", PasswordHash="hashedPassword1", Username="Test1", Role="Admin"},
            new User {ID = 2, Email="test2@test.com", PasswordHash="hashedPassword2", Username="Test2", Role="g_membwer"},
        }.AsQueryable().BuildMockDbSet();

        var mockCategory = new List<Category>{
            new Category{ID = 1,Name = "Electronics"},
            new Category{ID = 2,Name = "Smart Phone", ParentID=1},

        }.AsQueryable().BuildMockDbSet();
        
        var mockProduct = new List<Product>{
            new Product {ID = 1, 
                        Name = "iPhone 13", 
                        UUID = Guid.NewGuid().ToString(), 
                        SKU = "PHO-001",  
                        Category = new Category { ID = 1, Name = "Electronics" }, 
                        Inventories = new List<Inventory>()},
            new Product {ID = 2, 
                        Name = "Xiaomi", 
                        UUID = Guid.NewGuid().ToString(), 
                        SKU = "PHO-002",  
                        Category = new Category { ID = 1, Name = "Electronics" }, 
                        Inventories = new List<Inventory>()},

        }.AsQueryable().BuildMockDbSet();
        
        var mockOrder = new List<Order>{
            new Order {ID=1, UUID= Guid.NewGuid().ToString(), OrderNumber="OD-20250102", UserID=1}
        }.AsQueryable().BuildMockDbSet();

        var mockOrderItem = new List<OrderItem>{
            new OrderItem{ ID=1, OrderID=1, Price=1200, ProductID=1, Quantity=1}

        }.AsQueryable().BuildMockDbSet();
        
        var mockInventory = new List<Inventory>{
            

        }.AsQueryable().BuildMockDbSet();

        var mockToken = new List<Token>{}.AsQueryable().BuildMockDbSet();

        _context = new AppDbContext(options)
        {
            Users = mockUsers.Object,
            Categories = mockCategory.Object,
            Products = mockProduct.Object,
            Orders = mockOrder.Object,
            OrderItems = mockOrderItem.Object,
            Inventories = mockInventory.Object,
            Tokens = mockToken.Object,
        };

        var jwtSetting = Options.Create(new JwtSetting{SecretKey = "YourTestSecretKey"});

        _controller = new UsersController(_context, jwtSetting);
    }

    [Fact]
    public async Task Register_ReturnBadRequest_Email_exists()
    {
        var existUsesr = new User{
            Email = "test1@test.com",
            PasswordHash = "passwordTest",
            Username = "Test1",
            Role = "Admin"
        };

        _context.Users.Add(existUsesr);
        await _context.SaveChangesAsync();

        var registerDto = new UserRegisterDto{
            Email = "test1@test.com",
            Password = "passwordTest",
            Username = "Test1",
            Role = "g_member"
        };

        var result = await _controller.Register(registerDto);

        //Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Register_ReturnOk_NewUserAdded(){
        //Arrange
        var registerDto = new UserRegisterDto{
            Username = "Test3",
            Email = "test2@example.com",
            Password = "PasswordTest3",
            Role = "g_member"
        };

        var result = await _controller.Register(registerDto);

        // Assert
        Assert.IsType<OkObjectResult>(result);
    }
}

internal class Mock<T>
{
}