using Microsoft.AspNetCore.Mvc;
using ResultViewer.Server.Context;
using System.Data.SqlClient;
using Dapper;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ResultViewer.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly DbContext _context;

        public ProductController(DbContext context) => _context = context;

        //The splitOn argument tells Dapper to split the data on the CategoryId column. Anything up to that column maps to the first parameter Product, and anything else from that column onward should be mapped to the second input parameter Category.

        [HttpGet("splitononetomany")]
        public async Task<IResult> GetProductsUsingSplitOnOneToMany()
        {
            var sql = @"SELECT ProductID, ProductName, p.CategoryID, CategoryName
                FROM Products p 
                INNER JOIN Categories c ON p.CategoryID = c.CategoryID";

            using (var connection = _context.CreateConnection())
            {
                var products = await connection.QueryAsync<Product, Category, Product>(sql, (product, category) => {
                    product.Category = category;
                    return product;
                },
                splitOn: "CategoryId");

                //products.ToList().ForEach(product => Console.WriteLine($"Product: {product.ProductName}, Category: {product.Category.CategoryName}"));

                return Results.Ok(products);
            }
        }

        [HttpGet("splitonmanytomany")]
        public async Task<IResult> GetOrdersUsingSplitOnManyToMany()
        {
            var sql = @"SELECT o.OrderId, o.OrderDate, od.ProductId, p.ProductName
                FROM Orders o 
                INNER JOIN OrderDetails od ON od.OrderId = o.OrderId
                INNER JOIN Products p ON p.ProductId = od.ProductId";

            using (var connection = _context.CreateConnection())
            {
                var orders = await connection.QueryAsync<Order, Product, Order>(sql, (order, product) =>
                {
                    order.OrderDetails.Add(new OrderDetail
                    {
                        Product = product
                    });
                    return order;
                }, splitOn: "ProductId");

                var result = orders.GroupBy(o => o.OrderID).Select(g =>
                {
                    var groupedOrder = g.First();
                    groupedOrder.OrderDetails = g.Select(o => o.OrderDetails.Single()).ToList();
                    return groupedOrder;
                });

                //foreach (var order in result)
                //{
                //    Console.WriteLine($"Order Id: {order.OrderID}, Order Date: {order.OrderDate}");

                //    foreach (var orderDetail in order.OrderDetails)
                //    {
                //        Console.WriteLine($"\tProduct Name: {orderDetail.Product.ProductName}");
                //    }
                //}

                return Results.Ok(result);
            }
        }

        [HttpGet("splitonmanytomany")]
        public async Task<IResult> GetProductsUsingSplitOnManyToMany()
        {
            var sql = @"SELECT p.ProductID, p.ProductName, c.CategoryID, c.CategoryName
                FROM Products p 
                INNER JOIN ProductCategories pc ON pc.ProductID = p.ProductID
                INNER JOIN Categories c ON c.CategoryID = pc.CategoryID";

            using (var connection = _context.CreateConnection())
            {
                var products = await connection.QueryAsync<Product, Category, Product>(sql, (product, category) => {
                    product.Categories.Add(category);
                    return product;
                },
                splitOn: "CategoryID");

                //products.ToList().ForEach(product => Console.WriteLine($"Product: {product.ProductName}, Categories: {string.Join(", ", product.Categories.Select(c => c.CategoryName))}"));

                return Results.Ok(products);
            }
        }

        [HttpGet("multiplerelationships")]
        public async Task<IResult> GetProductsUsingMultipleRelationships()
        {
            var sql = @"SELECT p.ProductID, p.ProductName, c.CategoryID, c.CategoryName, s.SupplierID, s.CompanyName
                FROM Products p 
                INNER JOIN Categories c ON c.CategoryID = p.CategoryID
                INNER JOIN Suppliers s ON s.SupplierID = p.SupplierID";

            using (var connection = _context.CreateConnection())
            {
                var products = await connection.QueryAsync<Product, Category, Supplier, Product>(sql, (product, category, supplier) => {
                    product.Category = category;
                    product.Supplier = supplier;
                    return product;
                },
                splitOn: "CategoryID,SupplierID");

                //products.ToList().ForEach(product => Console.WriteLine($"Product: {product.ProductName}, Category: {product.Category.CategoryName}, Supplier: {product.Supplier.CompanyName}"));

                return Results.Ok(products);
            }
        }
    }

    public class Product
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int? SupplierID { get; set; }
        public int? CategoryID { get; set; }
        public string QuantityPerUnit { get; set; }
        public decimal? UnitPrice { get; set; }
        public short? UnitsInStock { get; set; }
        public short? UnitsOnOrder { get; set; }
        public short? ReorderLevel { get; set; }
        public bool Discontinued { get; set; }
        public Category Category { get; set; }
        public Supplier Supplier { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
        //public List<Category> Categories { get; set; } = new List<Category>();
    }

    public class Category
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public byte[] Picture { get; set; }
        public List<Product> Products { get; set; }
    }

    //public class ProductCategories
    //{
    //    public int ProductID { get; set; }
    //    public int CategoryID { get; set; }
    //}

    public class Order
    {
        public int OrderID { get; set; }
        public string CustomerID { get; set; }
        public int? EmployeeID { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? RequiredDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public int? ShipVia { get; set; }
        public decimal? Freight { get; set; }
        public string ShipName { get; set; }
        public string ShipAddress { get; set; }
        public string ShipCity { get; set; }
        public string ShipRegion { get; set; }
        public string ShipPostalCode { get; set; }
        public string ShipCountry { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
    }

    public class OrderDetail
    {
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public decimal UnitPrice { get; set; }
        public short Quantity { get; set; }
        public float Discount { get; set; }
        public Order Order { get; set; }
        public Product Product { get; set; }
    }

    public class Supplier
    {
        public int SupplierID { get; set; }
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public string ContactTitle { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string HomePage { get; set; }
    }
}
