using System;
using System.Text.Json.Serialization;

namespace ExpenseWebApi.Models {

    public class Expenseline {

        public int Id { get; set; } = 0;
        public int Quantity { get; set; } = 1;

        public int ExpenseId { get; set; } = 0;
        [JsonIgnore]
        public virtual Expense? Expense { get; set; } = null;

        public int ItemId { get; set; } = 0;
        public virtual Item? Item { get; set; } = null;

    }
}

