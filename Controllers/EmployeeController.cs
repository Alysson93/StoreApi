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
    private readonly QueryAllUsersWithClaimName query;

    public EmployeeController(ILogger<EmployeeController> logger, UserManager<IdentityUser> userManager, QueryAllUsersWithClaimName query)
    {
        _logger = logger;
        this.userManager = userManager;
        this.query = query;
    }

    [HttpGet]
    public IResult Get([FromQuery]int? page, [FromQuery]int? rows)
    {
        var employees = query.Execute(page, rows);
        return Results.Ok(employees);
    }


    [HttpPost]
    public IResult Post([FromBody]EmployeeRequest request)
    {
        var user = new IdentityUser
        {
            UserName = request.Email,
            Email = request.Email
        };
        var result = this.userManager.CreateAsync(user, request.Password).Result;
        if (!result.Succeeded)
            return Results.BadRequest(result.Errors.First());
        
        var userClaims = new List<Claim>
        {
            new Claim("Code", request.Code),
            new Claim("Name", request.Name)
        };
        var claimResult = userManager.AddClaimsAsync(user, userClaims).Result;     
        if (!claimResult.Succeeded)
            return Results.ValidationProblem(result.Errors.ConvertToProblemDetails());

        return Results.Created($"/employee/{user.Id}", user.Id);   
    }

}
