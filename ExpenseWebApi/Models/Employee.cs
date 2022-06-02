using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseWebApi.Models {

    public class Employee {

        public int Id { get; set; } = 0;
        [StringLength(30)]
        public string Name { get; set; } = string.Empty;
        [StringLength(30)]
        public string Email { get; set; } = string.Empty;
        [StringLength(30)]
        public string Password { get; set; } = string.Empty;
        public bool Admin { get; set; } = false;

        [Column(TypeName = "decimal(9,2)")]
        public decimal ExpensesDue { get; set; } = 0;
        [Column(TypeName = "decimal(9,2)")]
        public decimal ExpensesPaid { get; set; } = 0;
    }
}

