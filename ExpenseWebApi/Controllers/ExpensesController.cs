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
    public class ExpensesController : ControllerBase {
        private readonly ExpenseDbContext _context;
        private const string APPROVED = "Approved";
        private const string REJECTED = "Rejected";
        private const string REVIEW = "Review";

        public ExpensesController(ExpenseDbContext context) {
            _context = context;
        }

        private async Task<IActionResult> UpdateEmployeeExpenseDue(int expenseId, bool reverseExpense = false) {
            var exp = await _context.Expenses.FindAsync(expenseId);
            if (exp is null) {
                throw new Exception("Could not read the Expense");
            }
            var empl = await _context.Employees.FindAsync(exp.EmployeeId);
            if (empl is null) {
                throw new Exception("Could not read the Employee");
            }
            empl.ExpensesDue += (reverseExpense) ? exp.Total * -1 : exp.Total;
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("approved")]
        public async Task<ActionResult<IEnumerable<Expense>>> GetApprovedExpenses() {
            return await _context.Expenses.Where(x => x.Status == APPROVED).ToListAsync();
        }

        // GET: api/Expenses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Expense>>> GetExpenses() {
            if (_context.Expenses == null) {
                return NotFound();
            }
            return await _context.Expenses
                                    .Include(x => x.Employee)
                                    .ToListAsync();
        }

        // GET: api/Expenses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Expense>> GetExpense(int id) {
            if (_context.Expenses == null) {
                return NotFound();
            }
            var expense = await _context.Expenses
                                            .Include(x => x.Employee)
                                            .Include(x => x.Expenselines)
                                                .ThenInclude(x => x.Item)
                                            .SingleOrDefaultAsync(x => x.Id == id);

            if (expense == null) {
                return NotFound();
            }

            return expense;
        }

        [HttpPut("approve/{id}")]
        public async Task<IActionResult> ApproveExpense(int id, Expense expense) {
            if (expense.Status == APPROVED) {
                return BadRequest();
            }
            expense.Status = APPROVED;
            var rc = await PutExpense(id, expense);
            await UpdateEmployeeExpenseDue(expense.Id);
            return rc;
        }

        [HttpPut("reject/{id}")]
        public async Task<IActionResult> RejectExpense(int id, Expense expense) {
            expense.Status = REJECTED;
            var rc = await PutExpense(id, expense);
            return rc;
        }

        [HttpPut("review/{id}")]
        public async Task<IActionResult> ReviewExpense(int id, Expense expense) {
            var prevStatus = expense.Status;
            expense.Status = (expense.Total <= 75) ? APPROVED : REVIEW;
            var rc = await PutExpense(id, expense);
            if (prevStatus == APPROVED && expense.Status != APPROVED) {
                await UpdateEmployeeExpenseDue(expense.Id, reverseExpense: true);
            }
            if (prevStatus != APPROVED && expense.Status == APPROVED) {
                await UpdateEmployeeExpenseDue(expense.Id);
            }
            return rc;
        }

        // PUT: api/Expenses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExpense(int id, Expense expense) {
            if (id != expense.Id) {
                return BadRequest();
            }

            _context.Entry(expense).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
            } catch (DbUpdateConcurrencyException) {
                if (!ExpenseExists(id)) {
                    return NotFound();
                } else {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Expenses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Expense>> PostExpense(Expense expense) {
            if (_context.Expenses == null) {
                return Problem("Entity set 'ExpenseDbContext.Expenses'  is null.");
            }
            _context.Expenses.Add(expense);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetExpense", new { id = expense.Id }, expense);
        }

        // DELETE: api/Expenses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpense(int id) {
            if (_context.Expenses == null) {
                return NotFound();
            }
            var expense = await _context.Expenses.FindAsync(id);
            if (expense == null) {
                return NotFound();
            }

            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ExpenseExists(int id) {
            return (_context.Expenses?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
