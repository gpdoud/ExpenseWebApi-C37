using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseWebApi.Models {

    public class Expense {

        public int Id { get; set; } = 0;
        [StringLength(30)]
        public string Desc { get; set; } = string.Empty;
        [StringLength(30)]
        public string Status { get; set; } = "New";
        [Column(TypeName = "decimal(9,2)")]
        public decimal Total { get; set; } = 0;

        public int EmployeeId { get; set; } = 0;
        public virtual Employee? Employee { get; set; } = null;

        public virtual ICollection<Expenseline>? Expenselines { get; set; }
    }
}

