using Microsoft.AspNetCore.Mvc;

namespace StoreApi.Controllers;

[ApiController]
[Route("[controller]")]
public class CategoryController : ControllerBase
{

    private readonly ILogger<CategoryController> _logger;
    private readonly AppDbContext Context;

    public CategoryController(ILogger<CategoryController> logger, AppDbContext context)
    {
        _logger = logger;
        Context = context;
    }

    [HttpGet]
    public IResult Get()
    {
        var categories = Context.Categories.ToList();
        var response = categories.Select(c => new CategoryResponse
        {
            Id = c.Id,
            Name = c.Name,
            Active = c.Active
        });
        return Results.Ok(response);
    }


    [HttpGet("{id}")]
    public IResult GetById([FromRoute] Guid id)
    {
        var category = Context.Categories.Where(c => c.Id == id).FirstOrDefault();
        if (category == null) return Results.NotFound();
        var response = new CategoryResponse
        {
            Id = category.Id,
            Name = category.Name,
            Active = category.Active
        };
        return Results.Ok(response);
    }


    [HttpPost]
    public IResult Post(CategoryRequest request)
    {
        var category = new Category
        {
            Name = request.Name,
            CreatedBy = "Test",
            CreatedOn = DateTime.UtcNow,
            EditedBy = "Test",
            EditedOn = DateTime.UtcNow
        };
        Context.Categories.Add(category);
        Context.SaveChanges();
        return Results.Created($"/categories/{category.Id}", category.Id);   
    }


    [HttpPut("{id}")]
    public IResult Put(CategoryRequest request, [FromRoute] Guid id)
    {
        var category = Context.Categories.Where(c => c.Id == id).FirstOrDefault();
        if (category == null) return Results.NotFound();
        category.Name = request.Name;
        category.Active = request.Active;
        Context.SaveChanges();
        return Results.Ok();
    }


    [HttpDelete("{id}")]
    public IResult Delete([FromRoute] Guid id)
    {
        var category = Context.Categories.Where(c => c.Id == id).FirstOrDefault();
        if (category == null) return Results.NotFound();
        Context.Categories.Remove(category);
        Context.SaveChanges();
        return Results.Ok();
    }


}
