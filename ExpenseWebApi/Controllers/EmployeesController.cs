using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ExpenseWebApi.Models;

namespace ExpenseWebApi.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase {
        private readonly ExpenseDbContext _context;

        public EmployeesController(ExpenseDbContext context) {
            _context = context;
        }

        [HttpGet("{email}/{password}")]
        public async Task<ActionResult<Employee>> Login(string email, string password) {
            var empl = await _context.Employees
                                        .SingleOrDefaultAsync(x => x.Email == email && x.Password == password);
            if (empl is null) {
                return NotFound();
            }
            return empl;
        }

        // GET: api/Employees
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees() {
            if (_context.Employees == null) {
                return NotFound();
            }
            return await _context.Employees.ToListAsync();
        }

        // GET: api/Employees/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id) {
            if (_context.Employees == null) {
                return NotFound();
            }
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null) {
                return NotFound();
            }

            return employee;
        }

        [HttpPut("reset/{id}")]
        public async Task<IActionResult> ResetEmployeeExpenseTotals(int id, Employee employee) {
            employee.ExpensesPaid = 0;
            employee.ExpensesDue = 0;
            return await PutEmployee(id, employee);
        }

        // PUT: api/Employees/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(int id, Employee employee) {
            if (id != employee.Id) {
                return BadRequest();
            }

            _context.Entry(employee).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
            } catch (DbUpdateConcurrencyException) {
                if (!EmployeeExists(id)) {
                    return NotFound();
                } else {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Employees
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Employee>> PostEmployee(Employee employee) {
            if (_context.Employees == null) {
                return Problem("Entity set 'ExpenseDbContext.Employees'  is null.");
            }
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmployee", new { id = employee.Id }, employee);
        }

        // DELETE: api/Employees/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id) {
            if (_context.Employees == null) {
                return NotFound();
            }
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) {
                return NotFound();
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmployeeExists(int id) {
            return (_context.Employees?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
