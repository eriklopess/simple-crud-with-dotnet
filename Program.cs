using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/{code}", ([FromRoute] string code) => {
    Product p = ProductRepository.GetBy(code);
    if (p != null)
        return Results.Ok(p);
    return Results.NotFound();
    });
app.MapPost("/", (Product product) => {
    ProductRepository.Add(product);
    return Results.Created($"/products/{product.Code}", product);
});
app.MapPut("/", (Product product) => {
    ProductRepository.UpdateProduct(product);
    return Results.Ok();
});
app.MapDelete("/{code}", ([FromRoute] string code) => {
    Product product = ProductRepository.GetBy(code);
    ProductRepository.RemoveProduct(product);
    return Results.Ok();
});

app.Run();

public static class ProductRepository {
    public static List<Product> Products { get; set; }

    public static void Add(Product product) {
        if(Products == null)
            Products = new List<Product>();
        
        Products.Add(product);
    }

    public static Product GetBy(string code) {
        return Products.FirstOrDefault(p => p.Code == code);
    }

    public static Product UpdateProduct(Product product) {
        Product p = ProductRepository.GetBy(product.Code);
        p.Name = product.Name;
        return p;
    }

    public static void RemoveProduct(Product product) {
        Products.Remove(product);
    }
}

public class Product {
    public string Code { get; set; }
    public string Name { get; set; }
}