public record EmployeeRequest
(
	string Email,
	string Password,
	string Name,
	string Code
);


public record EmployeeResponse
(
	string Email,
	string Name
);