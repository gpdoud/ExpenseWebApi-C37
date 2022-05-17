using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseWebApi.Models {

    public class Item {

        public int Id { get; set; } = 0;
        [StringLength(30)]
        public string Name { get; set; } = string.Empty;
        [Column(TypeName = "decimal(7,2)")]
        public decimal Price { get; set; } = 0;
    }
}

