using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZoOnlineStore.Data;
using ZoOnlineStore.Models;
using ZoOnlineStore.DTO.Category;
using ZoOnlineStore.DTO.Inventory;
using Azure;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualBasic;

namespace ZoOnlineStore.Controllers;
[Route("api/[controller]")]
[ApiController]
public class ProductController : Controller
{
    private readonly AppDbContext _context;
    public ProductController(AppDbContext context){
        _context = context;
    }

    [HttpPost("getproduct")]
    public async Task<IActionResult> GetAllProducts ([FromBody] ProductDto dto){
        try
        {
            var query = _context.Products
                        .Include(p => p.Category)      //include category
                        .Include(p => p.Inventory)  //include inventory
                        .Include(p => p.Images) //get all images
                        .AsQueryable();
            //get products by category
            if(dto.CategoryId != 0){
                var queryProducts = await query.Where(c => c.CategoryID == dto.CategoryId)
                .Select(p => new {
                    ProductId = p.ID,
                    ProductName = p.Name,
                    CategoryName = p.Category.Name,
                    InventoryQuantity = p.Inventory.Quantity,
                    Price = p.Price,
                    Image = p.Images.FirstOrDefault(img => img.IsPrimary).ImagePath
                })
                .ToListAsync();
                var totalRecord = await query.CountAsync();      
                var productList = queryProducts.Select(p => new {
                    p.ProductId,
                    p.ProductName,
                    p.CategoryName,
                    p.InventoryQuantity,
                    p.Price,
                    Image = p.Image ?? "DefaultImagePath" //return default path for null 
                }).ToList();      
                return Ok(new{
                    TotalProducts = totalRecord,
                    Data = productList
                });
            }

            //get product by pagination
            var AllProducts = await query
                .Skip(((dto.Page ?? 1) - 1) * (dto.PageSize ?? 10))
                .Take(dto.PageSize ?? 10)
                .Select(p => new
                    {
                        ProductId = p.ID,
                        ProductName = p.Name,
                        CategoryName = p.Category.Name,
                        InventoryQuantity = p.Inventory.Quantity,
                        Price = p.Price,
                        Image = p.Images.FirstOrDefault(img => img.IsPrimary).ImagePath
                    })
                .ToListAsync();
                
                var allProductList = AllProducts.Select(p => new {
                    p.ProductId,
                    p.ProductName,
                    p.CategoryName,
                    p.InventoryQuantity,
                    p.Price,
                    Image = p.Image ?? "DefaultImagePath"
                }).ToList(); 

            return Ok(new{
                Page = dto.Page,
                PageSize = dto.PageSize,
                Data = allProductList
            });
        }
        catch(Exception ex)
        {
            return StatusCode(500, new
            {
                Message = "An unexpected error occurred. Please try again later.",
                Error = ex.Message
            });
        }
    }
    
    [HttpPost("getProductById")]
    public async Task<IActionResult>  GetProductById ([FromBody] ProductDto dto){
        try{
            if(dto.ProductID < 0){
                //no id return error 
                return BadRequest("Required product id is required.");
            }
            var product = await _context.Products
                        .Include(p => p.Inventory) 
                        .Include(p => p.Images)                         
                        .Include(p => p.Category)                       
                        .FirstOrDefaultAsync(p => p.ID == dto.ProductID);

            if(product == null){
                return NotFound("Product not found.");
            }

            return Ok(new{
                ProductId = product.ID,
                ProductName = product.Name,
                Category = product.Category?.Name,
                CategoryId = product.Category?.ID,
                InventoryQuantity = product.Inventory?.Quantity,
                ProductImages = product.Images.Select(img => new
                {
                    img.ImagePath,
                    img.IsPrimary,
                    img.Caption
                })
            });

                            
        }catch(Exception e){
            return StatusCode(500, new
                {
                    Message = "An unexpected error occurred. Please try again later.",
                    Error = e.Message
                });
        }
    }

    [HttpPost("addProduct")]
    public async Task<IActionResult> AddProduct ([FromBody] ProductDto dto){
        try{
            if (string.IsNullOrEmpty(dto.Name))
            {
                return BadRequest("Product name is required.");
            }

            if (string.IsNullOrEmpty(dto.SKU))
            {
                return BadRequest("SKU is required.");
            }

            if (dto.Price <= 0)
            {
                return BadRequest("Product price must be greater than 0.");
            }

            Product product;
            //update product detail
            product = new Product
            {
                Name = dto.Name,
                SKU = dto.SKU,
                Price = dto.Price,
                Stock = dto.Quantity,
                CategoryID = dto.CategoryId,
                Description = dto.Description,
                UUID = Guid.NewGuid().ToString()
            };
            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            //update product inventory 
            var inventory = new Inventory
            {
                ProductID = product.ID,
                Quantity = dto.Quantity,
                Action = dto.Action
            };
            _context.Inventories.Add(inventory);
            
            //update product image one image at least 
            if (dto.Images != null && dto.Images.Any())
            {
                foreach (var imageDto in dto.Images)
                {
                    var image = new ProductImage
                    {
                        ImagePath = imageDto.ImagePath,
                        IsPrimary = imageDto.IsPrimary,
                        ProductID = product.ID,
                        Caption =  imageDto.Caption,
                        IsThumbnailImage = imageDto.IsThumbnailImage,
                    };

                    _context.ProductImages.Add(image);
                }
                
            }
            // save Inventory, Images 
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "Product added successfully.",
                ProductId = product.ID
            });
        }catch( Exception e ){
            return StatusCode(500, new{
                Message = "An unexpected error occurred. Please try again later.",
                Error = e.Message
            });
        }
    }

    // [HttpPost("updateProduct")]

    // [HttpPost("deletedProduct")]



}
