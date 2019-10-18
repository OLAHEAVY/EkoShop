using Microsoft.EntityFrameworkCore.Migrations;

namespace EkoShop.DataAccess.Migrations
{
    public partial class AddedCouponToTheDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Brand",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Coupon",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    CouponType = table.Column<string>(nullable: false),
                    Discount = table.Column<double>(nullable: false),
                    MinimumAmount = table.Column<double>(nullable: false),
                    ImageUrl = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coupon", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Coupon");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Brand");
        }
    }
}
