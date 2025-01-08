using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZoOnlineStore.Data;
using ZoOnlineStore.Models;
using ZoOnlineStore.DTO.Category;
using Azure;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualBasic;

namespace ZoOnlineStore.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : Controller
{
    private readonly AppDbContext _context;
    public CategoryController (AppDbContext context){
        _context = context;
    }

    [HttpGet("categories")]
    public async Task<IActionResult> GetCategories(){
        var query = _context.Categories.AsQueryable();
        var result = await query.ToListAsync();

        return Ok(result);
    }

    [HttpPost("categories")]
    public async Task<IActionResult> SearchCategories([FromBody] CategoriesDto dto){
        if(dto.Page < 1 || dto.PageSize <1 ){
            return BadRequest("Page and size must greater than 0.");
        }
        var query = _context.Categories.AsQueryable();
        
        if(dto.ID.HasValue){
            query = query.Where(c => c.ID == dto.ID || c.ParentID == dto.ID);
        }

        if(!string.IsNullOrEmpty(dto.Name)){
            query = query.Where(c=> c.Name.Contains(dto.Name));
        }
        int totalRecord = await query.CountAsync();

        var result = await query.Skip((dto.Page-1 * dto.PageSize)).Take(dto.PageSize).ToListAsync();

        return Ok(new{
            TotalRecord = totalRecord,
            Page = dto.Page,
            PageSize = dto.PageSize,
            Data = result
        });
    }

    [HttpPost("addcategory")]
    public async Task<IActionResult> AddCategory([FromBody] CategoriesDto dto){
        // 判断 Name 是否为空
        if (string.IsNullOrEmpty(dto.Name))
        {
            return BadRequest("Category name is required.");
        }

        // 查询是否已存在相同 Name 和 ParentID 的分类
        var existingCategory = await _context.Categories
                                .FirstOrDefaultAsync(c=> c.Name == dto.Name && c.ParentID == dto.ParentID);
        
        if(existingCategory != null){
            return Conflict("Category already exists.");
        }

        //create new category
        var newCategory = new Category{
            Name = dto.Name,
            ParentID = dto.ParentID
        };
        _context.Categories.Add(newCategory);
        await _context.SaveChangesAsync();

        return Ok(new{
            Message =  "Category added successfully.",
            Category = newCategory
        });
    }
}   
