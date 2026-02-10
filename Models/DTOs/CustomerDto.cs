namespace GolekBackend.Models.DTOs;
public class CustomerDto
{
public int Id { get; set; }
public string FullName { get; set; } = string.Empty;
public string Email { get; set; } = string.Empty;
public string PhoneNumber { get; set; } = string.Empty;
}

public class CreateCustomerDto
{
public string FullName { get; set; } = string.Empty;
public string Email { get; set; } = string.Empty;
public string PhoneNumber { get; set; } = string.Empty;
}