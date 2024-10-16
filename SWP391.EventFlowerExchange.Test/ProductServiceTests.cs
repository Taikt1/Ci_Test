using NUnit.Framework;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using SWP391.EventFlowerExchange.Application;
using SWP391.EventFlowerExchange.Domain.Entities;
using SWP391.EventFlowerExchange.Domain.ObjectValues;
using SWP391.EventFlowerExchange.Infrastructure;
using Microsoft.IdentityModel.Tokens;

[TestFixture]
//public class ProductServiceTests
//{
//    private Mock<IProductRepository> _mockRepo;
//    private ProductService _productService;

//    [SetUp]
//    public void Setup()
//    {
//        _mockRepo = new Mock<IProductRepository>();
//        _productService = new ProductService(_mockRepo.Object);
//    }
//    [Test]
//    public async Task GetEnableProductListFromAPIAsync_ReturnsProductList()
//    {
//        var products = new List<Product> { new Product { ProductId = 1, ProductName = "Flower" } };
//        _mockRepo.Setup(repo => repo.GetEnableProductListAsync()).ReturnsAsync(products);

//        var result = await _productService.GetEnableProductListFromAPIAsync();

//        if (result == null || result.Count == 0)
//        {
//            Console.WriteLine("Result is empty.");
//        }
//        else
//        {
//            Console.WriteLine($"Result contains {result.Count} product(s):");
//            foreach (var product in result)
//            {
//                Console.WriteLine($"Product ID: {product.ProductId}, Name: {product.ProductName}");
//            }
//        }
//    }

//    [Test]
//    public async Task GetEnableProductListFromAPIAsync_NoProducts_ReturnsEmptyList()
//    {
//        _mockRepo.Setup(repo => repo.GetEnableProductListAsync()).ReturnsAsync(new List<Product>());

//        var result = await _productService.GetEnableProductListFromAPIAsync();

//        if (result == null || result.Count == 0)
//        {
//            Console.WriteLine("Result is empty.");
//        }
//        else
//        {
//            Console.WriteLine($"Result contains {result.Count} product(s).");
//        }
//    }

//    [Test]
//    public async Task AddProductAsync_ValidProduct_ReturnsSuccess()
//    {
//        CreateProduct product = new CreateProduct { ProductName = "Rose", SellerId = "ididsj" };
//        _mockRepo.Setup(repo => repo.CreateNewProductAsync(product)).ReturnsAsync(true);

//        var result = await _productService.CreateNewProductFromAPIAsync(product);

//        Console.WriteLine($"AddProductAsync result: {result}");
//    }


//    [Test]
//    public async Task DeleteProductAsync_ValidProductId_ReturnsSuccess()
//    {

//        Product product = new Product() { ProductId = 1 };
//        _mockRepo.Setup(repo => repo.RemoveProductAsync(product)).ReturnsAsync(true);

//        var result = await _productService.RemoveProductFromAPIAsync(product);

//        Console.WriteLine($"DeleteProductAsync result: {result}");
//    }
//}

public class ProductServiceTests
{
    private Mock<IProductRepository> _mockRepo;
    private ProductService _productService;

    [SetUp]
    public void Setup()
    {
        _mockRepo = new Mock<IProductRepository>();
        _productService = new ProductService(_mockRepo.Object);
    }

    [Test]
    public async Task GetEnableProductListFromAPIAsync_ReturnsProductList()
    {
        var products = new List<Product> { new Product { ProductName = "Flower" } };
        _mockRepo.Setup(repo => repo.GetEnableProductListAsync()).ReturnsAsync(products);

        var result = await _productService.GetEnableProductListFromAPIAsync();

        if (result == null || result.Count == 0)
        {
            Console.WriteLine("Result is empty.");
        }
        else
        {
            Console.WriteLine($"Result contains {result.Count} product(s):");
            foreach (var product in result)
            {
                Console.WriteLine($"Product Name: {product.ProductName}");
            }
        }
    }

    [Test]
    public async Task GetEnableProductListFromAPIAsync_NoProducts_ReturnsEmptyList()
    {
        _mockRepo.Setup(repo => repo.GetEnableProductListAsync()).ReturnsAsync(new List<Product>());

        var result = await _productService.GetEnableProductListFromAPIAsync();

        if (result == null || result.Count == 0)
        {
            Console.WriteLine("Result is empty.");
        }
        else
        {
            Console.WriteLine($"Result contains {result.Count} product(s).");
        }
    }

    [Test]
    public async Task CreateNewProductFromAPIAsync_ValidProduct_ReturnsSuccess()
    {
        CreateProduct product = new CreateProduct { ProductName = "Rose", SellerId = "ididsj" };
        _mockRepo.Setup(repo => repo.CreateNewProductAsync(product)).ReturnsAsync(true);

        var result = await _productService.CreateNewProductFromAPIAsync(product);

        Console.WriteLine($"CreateNewProductFromAPIAsync result: {result}");
    }

    [Test]
    public async Task DeleteProductAsync_ValidProductId_ReturnsSuccess()
    {
        Product product = new Product() { ProductName = "Flower" }; // ID is auto-generated
        _mockRepo.Setup(repo => repo.RemoveProductAsync(product)).ReturnsAsync(true);

        var result = await _productService.RemoveProductFromAPIAsync(product);

        Console.WriteLine($"DeleteProductAsync result: {result}");
    }
}