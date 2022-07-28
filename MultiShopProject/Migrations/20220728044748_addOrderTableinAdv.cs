using Microsoft.EntityFrameworkCore.Migrations;

namespace MultiShopProject.Migrations
{
    public partial class addOrderTableinAdv : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "Order",
                table: "Advertisements",
                nullable: false,
                defaultValue: (byte)0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Order",
                table: "Advertisements");
        }
    }
}
