using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrasturcture.Data.Migrations
{
    public partial class AddedAmmountPropertyToCreditTransfer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Ammount",
                table: "CreditTransfers",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ammount",
                table: "CreditTransfers");
        }
    }
}
