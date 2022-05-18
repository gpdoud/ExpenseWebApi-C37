using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseWebApi.Migrations
{
    public partial class fixedcustomeremailname : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Emil",
                table: "Employees",
                newName: "Email");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Email",
                table: "Employees",
                newName: "Emil");
        }
    }
}
