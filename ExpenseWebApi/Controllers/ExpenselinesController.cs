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
    public class ExpenselinesController : ControllerBase {
        private readonly ExpenseDbContext _context;

        public ExpenselinesController(ExpenseDbContext context) {
            _context = context;
        }

        private async Task<IActionResult> RecalcExpenseTotal(int expenseId) {
            var exp = await _context.Expenses.FindAsync(expenseId);
            if(exp is null) {
                throw new Exception("Recalc failed!");
            }
            exp.Total = (from el in _context.Expenselines
                         join i in _context.Items
                             on el.ItemId equals i.Id
                         where el.ExpenseId == expenseId
                         select new {
                             Subtotal = el.Quantity * i.Price
                         }).Sum(x => x.Subtotal);
            await _context.SaveChangesAsync();
            return Ok();
        }

        // GET: api/Expenselines
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Expenseline>>> GetExpenselines() {
            if (_context.Expenselines == null) {
                return NotFound();
            }
            return await _context.Expenselines.ToListAsync();
        }

        // GET: api/Expenselines/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Expenseline>> GetExpenseline(int id) {
            if (_context.Expenselines == null) {
                return NotFound();
            }
            var expenseline = await _context.Expenselines.FindAsync(id);

            if (expenseline == null) {
                return NotFound();
            }

            return expenseline;
        }

        // PUT: api/Expenselines/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExpenseline(int id, Expenseline expenseline) {
            if (id != expenseline.Id) {
                return BadRequest();
            }

            _context.Entry(expenseline).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();
                await RecalcExpenseTotal(expenseline.ExpenseId);
            } catch (DbUpdateConcurrencyException) {
                if (!ExpenselineExists(id)) {
                    return NotFound();
                } else {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Expenselines
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Expenseline>> PostExpenseline(Expenseline expenseline) {
            if (_context.Expenselines == null) {
                return Problem("Entity set 'ExpenseDbContext.Expenselines'  is null.");
            }
            _context.Expenselines.Add(expenseline);
            await _context.SaveChangesAsync();
            await RecalcExpenseTotal(expenseline.ExpenseId);

            return CreatedAtAction("GetExpenseline", new { id = expenseline.Id }, expenseline);
        }

        // DELETE: api/Expenselines/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpenseline(int id) {
            if (_context.Expenselines == null) {
                return NotFound();
            }
            var expenseline = await _context.Expenselines.FindAsync(id);
            if (expenseline == null) {
                return NotFound();
            }

            _context.Expenselines.Remove(expenseline);
            await _context.SaveChangesAsync();
            await RecalcExpenseTotal(expenseline.ExpenseId);

            return NoContent();
        }

        private bool ExpenselineExists(int id) {
            return (_context.Expenselines?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
