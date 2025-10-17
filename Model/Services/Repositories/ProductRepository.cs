using Microsoft.EntityFrameworkCore;
using Model.DomainModels.ProductAggregates;
using Model.Services.Contracts;
using ResponseFramework;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Model.Services.Repositories;

/// <summary>
/// Repository implementation for <see cref="Product"/> entity.
/// Provides CRUD operations (Insert, Update, Delete, Select) using Entity Framework Core.
/// This repository communicates with the database via <see cref="WebAPIApplicationContext"/>.
/// </summary>
public class ProductRepository : IProductRepository
{
    #region [- Fields -]

    /// <summary>
    /// The database context for the application.
    /// </summary>
    private readonly WebAPIApplicationContext _context;

    #endregion

    #region [- Constructor -]

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductRepository"/> class.
    /// </summary>
    /// <param name="context">The EF Core database context.</param>
    public ProductRepository(WebAPIApplicationContext context)
    {
        _context = context;
    }

    #endregion

    #region Public Methods

    #region [- Insert -]
    /// <summary>
    /// Inserts a new product into the database.
    /// </summary>
    /// <param name="product">The product entity to insert.</param>
    /// <returns>A model of the Response framework that returns a type of bool .
    /// This model indicating success or failure of process .</returns>
    public async Task<IResponse<bool>> Insert(Product product)
    {
        if (product is null)
            return new Response<bool>("product is null .", HttpStatusCode.BadRequest);

        try
        {
            var sqlParameters = new List<SqlParameter>
            {
                new SqlParameter("@ProductName", product.ProductName),
                new SqlParameter("@ProductDescription", product.ProductDescription),
                new SqlParameter("@UnitPrice", product.UnitPrice)
            };
            var rowInserted = await _context.Database.ExecuteSqlRawAsync($"EXEC usp_InsertProduct {sqlParameters}");
            if (rowInserted >= 1)
            {
                return new Response<bool>(true);
            }
            return new Response<bool>($"Error Message : Product not insert .", HttpStatusCode.InternalServerError);
        }
        catch (Exception exp)
        {
            return new Response<bool>($"Error Message : {exp}", HttpStatusCode.InternalServerError);
        }
    }
    #endregion

    #region [- Update -]
    /// <summary>
    /// Updates an existing product in the database.
    /// </summary>
    /// <param name="product">The product entity with updated values.</param>
    /// <returns>A model of the Response framework that returns a type of bool .
    /// This model indicating success or failure of process .</returns>
    public async Task<IResponse<bool>> Update(Product product)
    {
        if (product is null)
            return new Response<bool>("product is null .", HttpStatusCode.BadRequest);

        try
        {
            var sqlParameters = new List<SqlParameter>
            {
                new SqlParameter("@Id", product.Id),
                new SqlParameter("@ProductName", product.ProductName),
                new SqlParameter("@ProductDescription", product.ProductDescription),
                new SqlParameter("@UnitPrice", product.UnitPrice)
            };
            var rowsModified = await _context.Database.ExecuteSqlRawAsync($"EXEC usp_UpdateProduct {sqlParameters}");
            if (rowsModified >= 1)
                return new Response<bool>(true);
            return new Response<bool>($"Error Message : Product not update .", HttpStatusCode.InternalServerError);
        }
        catch (Exception exp)
        {
            return new Response<bool>($"Error Message : {exp}", HttpStatusCode.InternalServerError);
        }
    }
    #endregion

    #region [- Delete -]
    /// <summary>
    /// Deletes a product from the database.
    /// </summary>
    /// <param name="id">The product identifier to delete.</param>
    /// <returns>A model of the Response framework that returns a type of bool .
    /// This model indicating success or failure of process .</returns>
    public async Task<IResponse<bool>> Delete(Guid id)
    {
        if (id == Guid.Empty)
            return new Response<bool>("product not found .", HttpStatusCode.BadRequest);

        try
        {
            var sqlParameter = new SqlParameter("@Id", id);
            var rowsModified = await _context.Database.ExecuteSqlRawAsync($"EXEC usp_DeleteProduct {sqlParameter}");
            if (rowsModified >= 1)
                return new Response<bool>(true);

            return new Response<bool>($"Error Message : Product not update .", HttpStatusCode.InternalServerError);
        }
        catch (Exception exp)
        {
            return new Response<bool>($"Error Message : {exp}", HttpStatusCode.InternalServerError);
        }
    }
    #endregion

    #region [- Select All -]
    /// <summary>
    /// Retrieves all products from the database.
    /// </summary>
    /// <returns>Response containing a list of products, or an error message.</returns>
    public async Task<IResponse<IEnumerable<Product>>> SelectAll()
    {
        try
        {
            var products = await _context.Products.FromSqlRaw("EXEC usp_GetAllProducts").AsNoTracking().ToListAsync();
            if (products is null)
                return new Response<IEnumerable<Product>>("Products not found .Context is incorrect .", HttpStatusCode.NotFound);

            return new Response<IEnumerable<Product>>(products, true, "The process was completed successfully.", "", HttpStatusCode.OK);
        }
        catch (Exception exp)
        {
            return new Response<IEnumerable<Product>>($"Error Message : {exp}", HttpStatusCode.InternalServerError);
        }
    }
    #endregion

    #region [- Select By Id -]
    /// <summary>
    /// Retrieves a single product by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the product.</param>
    /// <returns>Response containing the product, or an error message.</returns>
    public async Task<IResponse<Product>> SelectById(Guid id)
    {
        if (id == Guid.Empty)
            return new Response<Product>("Id is empty .", HttpStatusCode.BadRequest);

        try
        {
            var sqlParameter = new SqlParameter("@Id", id);
            var product = await _context.Products.FromSqlRaw($"EXEC usp_GetProductById {id}").FirstOrDefaultAsync();
            if (product is null)
                return new Response<Product>("The product not found .", HttpStatusCode.NotFound);

            return new Response<Product>(product, true, "The process was completed successfully.", "", HttpStatusCode.OK);
        }
        catch (Exception exp)
        {
            return new Response<Product>($"Error Message : {exp}", HttpStatusCode.InternalServerError);
        }
    }
    #endregion

    #endregion
}
