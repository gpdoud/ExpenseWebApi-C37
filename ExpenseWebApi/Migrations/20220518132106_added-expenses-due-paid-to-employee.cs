using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseWebApi.Migrations
{
    public partial class addedexpensesduepaidtoemployee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ExpensesDue",
                table: "Employees",
                type: "decimal(9,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ExpensesPaid",
                table: "Employees",
                type: "decimal(9,2)",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpensesDue",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "ExpensesPaid",
                table: "Employees");
        }
    }
}
