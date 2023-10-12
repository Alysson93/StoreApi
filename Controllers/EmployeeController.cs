using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace StoreApi.Controllers;

[ApiController]
[Route("[controller]")]
public class EmployeeController : ControllerBase
{

    private readonly ILogger<EmployeeController> _logger;
    private readonly UserManager<IdentityUser> userManager;

    public EmployeeController(ILogger<EmployeeController> logger, UserManager<IdentityUser> userManager)
    {
        _logger = logger;
        this.userManager = userManager;
    }

    // [HttpGet]
    // public IResult Get()
    // {
    //     var categories = Context.Categories.ToList();
    //     var response = categories.Select(c => new CategoryResponse
    //     {
    //         Id = c.Id,
    //         Name = c.Name,
    //         Active = c.Active
    //     });
    //     return Results.Ok(response);
    // }


    // [HttpGet("{id}")]
    // public IResult GetById([FromRoute] Guid id)
    // {
    //     var category = Context.Categories.Where(c => c.Id == id).FirstOrDefault();
    //     if (category == null) return Results.NotFound();
    //     var response = new CategoryResponse
    //     {
    //         Id = category.Id,
    //         Name = category.Name,
    //         Active = category.Active
    //     };
    //     return Results.Ok(response);
    // }


    [HttpPost]
    public IResult Post(EmployeeRequest request)
    {
        var user = new IdentityUser
        {
            UserName = request.Email,
            Email = request.Email
        };
        var result = this.userManager.CreateAsync(user, request.Password).Result;
        if (!result.Succeeded)
            return Results.BadRequest(result.Errors.First());
        
        var claimResult = this.userManager.AddClaimAsync(user, new Claim("Code", request.Code)).Result;
        if (!claimResult.Succeeded)
            return Results.BadRequest(result.Errors.First());

        claimResult = this.userManager.AddClaimAsync(user, new Claim("Name", request.Name)).Result;
        if (!claimResult.Succeeded)
            return Results.BadRequest(result.Errors.First());        

        return Results.Created($"/employee/{user.Id}", user.Id);   
    }


    // [HttpPut("{id}")]
    // public IResult Put(CategoryRequest request, [FromRoute] Guid id)
    // {
    //     var category = Context.Categories.Where(c => c.Id == id).FirstOrDefault();
    //     if (category == null) return Results.NotFound();
    //     category.EditInfo(request.Name, request.Active);
    //     if (!category.IsValid)
    //         return Results.ValidationProblem(category.Notifications.ConvertToProblemDetails());
    //     Context.SaveChanges();
    //     return Results.Ok();
    // }


    // [HttpDelete("{id}")]
    // public IResult Delete([FromRoute] Guid id)
    // {
    //     var category = Context.Categories.Where(c => c.Id == id).FirstOrDefault();
    //     if (category == null) return Results.NotFound();
    //     Context.Categories.Remove(category);
    //     Context.SaveChanges();
    //     return Results.Ok();
    // }


}
