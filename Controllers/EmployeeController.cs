using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Npgsql;
using System.Security.Claims;

namespace StoreApi.Controllers;

[ApiController]
[Route("[controller]")]
public class EmployeeController : ControllerBase
{

    private readonly ILogger<EmployeeController> _logger;
    private readonly UserManager<IdentityUser> userManager;
    private readonly IConfiguration configuration;


    public EmployeeController(ILogger<EmployeeController> logger, UserManager<IdentityUser> userManager, IConfiguration configuration)
    {
        _logger = logger;
        this.userManager = userManager;
        this.configuration = configuration;
    }

    [HttpGet]
    public IResult Get([FromQuery]int page, [FromQuery]int rows)
    {
        var db = new NpgsqlConnection(this.configuration["ConnectionString"]);
        var query =  @"SELECT ""Email"", ""ClaimValue"" as Name
            FROM public.""AspNetUsers"" u 
            INNER JOIN public.""AspNetUserClaims"" c
            ON u.""Id"" = c.""UserId"" AND ""ClaimType"" = 'Name'
            ORDER BY ""name""
            OFFSET @page ROWS
            FETCH NEXT @rows ROWS ONLY;";
        var employees = db.Query<EmployeeResponse>( 
           query, new {page = (page-1)*rows, rows}
        );
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
