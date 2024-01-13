using Microsoft.EntityFrameworkCore;
using MvcCrud.Models;

namespace MvcCrud.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        
    }

    public DbSet<Item> Items { get; set; }
}