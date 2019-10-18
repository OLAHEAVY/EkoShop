using Microsoft.EntityFrameworkCore.Migrations;

namespace EkoShop.DataAccess.Migrations
{
    public partial class AddedIsInStockToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsInStock",
                table: "Product",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsInStock",
                table: "Product");
        }
    }
}
