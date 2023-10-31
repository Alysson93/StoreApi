using Dapper;
using Npgsql;

public class QueryAllUsersWithClaimName
{

	private readonly IConfiguration configuration;

	public QueryAllUsersWithClaimName(IConfiguration configuration)
	{
		this.configuration = configuration;
	}

	public IEnumerable<EmployeeResponse> Execute(int? page, int? rows)
	{
      if (page == null) page = 1;
      if (rows == null) rows = 6;
      var db = new NpgsqlConnection(this.configuration["ConnectionString"]);
      var query =  @"SELECT ""Email"", ""ClaimValue"" as Name
         FROM public.""AspNetUsers"" u 
         INNER JOIN public.""AspNetUserClaims"" c
         ON u.""Id"" = c.""UserId"" AND ""ClaimType"" = 'Name'
         ORDER BY ""name""
         OFFSET @page ROWS
         FETCH NEXT @rows ROWS ONLY;";
      return db.Query<EmployeeResponse>( 
         query, new {page = (page-1)*rows, rows}
      );
	}

}