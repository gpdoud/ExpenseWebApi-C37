using System;
using Microsoft.EntityFrameworkCore;

namespace ExpenseWebApi.Models {

    public class ExpenseDbContext : DbContext {

        public virtual DbSet<Employee> Employees { get; set; } = null!;
        public virtual DbSet<Expense> Expenses { get; set; } = null!;
        public virtual DbSet<Expenseline> Expenselines { get; set; } = null!;
        public virtual DbSet<Item> Items { get; set; } = null!;

        public ExpenseDbContext(DbContextOptions<ExpenseDbContext> options) : base(options) {
        }
    }
}

