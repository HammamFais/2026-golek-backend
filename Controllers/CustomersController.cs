using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GolekBackend.Data;
using GolekBackend.Models.Entities;
using GolekBackend.Models.DTOs;

namespace GolekBackend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomersController : ControllerBase
{
private readonly AppDbContext _context;
public CustomersController(AppDbContext context) => _context = context;

[HttpGet]
public async Task<ActionResult<IEnumerable<CustomerDto>>> GetCustomers()
{
return await _context.Customers
.Select(c => new CustomerDto { Id = c.Id, FullName = c.FullName, Email = c.Email, PhoneNumber = c.PhoneNumber })
.ToListAsync();
}

[HttpPost]
public async Task<ActionResult<CustomerDto>> CreateCustomer(CreateCustomerDto dto)
{
var customer = new Customer { FullName = dto.FullName, Email = dto.Email, PhoneNumber = dto.PhoneNumber };
_context.Customers.Add(customer);
await _context.SaveChangesAsync();
return Ok(new CustomerDto { Id = customer.Id, FullName = customer.FullName, Email = customer.Email, PhoneNumber = customer.PhoneNumber });
}

// FITUR BARU: Update Customer (PUT)
[HttpPut("{id}")]
public async Task<IActionResult> UpdateCustomer(int id, CreateCustomerDto dto)
{
var customer = await _context.Customers.FindAsync(id);
if (customer == null) return NotFound("Customer nggak ada, King!");

customer.FullName = dto.FullName;
customer.Email = dto.Email;
customer.PhoneNumber = dto.PhoneNumber;

await _context.SaveChangesAsync();
return Ok("Data Customer berhasil diupdate!");
}

// FITUR BARU: Hapus Customer (DELETE)
[HttpDelete("{id}")]
public async Task<IActionResult> DeleteCustomer(int id)
{
var customer = await _context.Customers.FindAsync(id);
if (customer == null) return NotFound("Customer emang nggak ada!");

_context.Customers.Remove(customer);
await _context.SaveChangesAsync();
return Ok("Customer sudah dihapus!");
}
}