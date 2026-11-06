using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Data;
using OnlineShop.Models;

namespace OnlineShop.Controllers;

public class ProductController : Controller
{
    private readonly AppDbContext _context;

    public ProductController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        [FromForm, Required] string? name,
        [FromForm] string? description,
        [FromForm, Required, Range(typeof(decimal), "0", "9999999999999999.99", ParseLimitsInInvariantCulture = true)] decimal? price)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }

        var product = new Product
        {
            Name = name!.Trim(),
            Description = description ?? string.Empty,
            Price = price!.Value
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        TempData["ProductSaved"] = "Product saved.";
        return RedirectToAction(nameof(Create));
    }
}
