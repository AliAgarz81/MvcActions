using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcCrud.Data;
using MvcCrud.Models;

namespace MvcCrud.Controllers;

public class ItemController : Controller
{
    private readonly DataContext _context;

    public ItemController(DataContext context)
    {
        _context = context;
    }
    public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString,  int? pageNumber)
    {
        ViewData["CurrentSort"] = sortOrder;
        ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
        ViewData["CurrentFilter"] = searchString;
        if (searchString != null)
        {
            pageNumber = 1;
        }
        else
        {
            searchString = currentFilter;
        }
        var items = from i in _context.Items select i;
        if (!String.IsNullOrEmpty(searchString))
        {
            items = items.Where(i => i.Name.ToLower().Contains(searchString.ToLower()));
        }
        switch (sortOrder)
        {
            case "name_desc":
                items = items.OrderByDescending(i => i.Name);
                break;
            default:
                items = items.OrderBy(i => i.Name);
                break;
        }

        int pageSize = 3;
        return View(await PaginatedList<Item>.CreateAsync(items.AsNoTracking(), pageNumber ?? 1, pageSize));
    }

    public IActionResult Create()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(Item item)
    {
        if (!ModelState.IsValid) return View(item);
        await _context.Items.AddAsync(item);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Details(Guid id)
    {
        var item = await _context.Items.FirstOrDefaultAsync(i => i.Id == id);
        if (item is null)
        {
            return NotFound();
        }
        return View(item);
    }
}