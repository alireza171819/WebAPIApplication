using Microsoft.EntityFrameworkCore;
using Model.DomainModels.ProductAggregates;

namespace Model;

/// <summary>
/// Provides the database context for a web API application.
/// </summary>
/// <remarks>
/// The context is configured via dependency injection and exposes
/// <see cref="DbSet{TEntity}"/> properties for each entity type.
/// </remarks>
public class WebAPIApplicationContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WebAPIApplicationContext"/> class
    /// using the supplied options.
    /// </summary>
    /// <param name="options">The options used to configure the context.</param>
    public WebAPIApplicationContext(DbContextOptions<WebAPIApplicationContext> options)
        : base(options)
    {
    }
    /// <summary>
    /// Represents the collection of <see cref="Product"/> entities in the database.
    /// </summary>
    public DbSet<Product> Products { get; set; }
}
