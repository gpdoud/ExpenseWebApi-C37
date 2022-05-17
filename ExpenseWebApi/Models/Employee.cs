using System;
using System.ComponentModel.DataAnnotations;

namespace ExpenseWebApi.Models {

    public class Employee {

        public int Id { get; set; } = 0;
        [StringLength(30)]
        public string Name { get; set; } = string.Empty;
        [StringLength(30)]
        public string Emil { get; set; } = string.Empty;
        [StringLength(30)]
        public string Password { get; set; } = string.Empty;
        public bool Admin { get; set; } = false;
    }
}

