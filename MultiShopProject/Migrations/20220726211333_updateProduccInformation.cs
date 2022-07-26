using Microsoft.EntityFrameworkCore.Migrations;

namespace MultiShopProject.Migrations
{
    public partial class updateProduccInformation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductInformations_ProductInformationId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "ProductInformations");

            migrationBuilder.DropColumn(
                name: "Review",
                table: "ProductInformations");

            migrationBuilder.AlterColumn<int>(
                name: "ProductInformationId",
                table: "Products",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "ProductInformations",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductInformations_ProductInformationId",
                table: "Products",
                column: "ProductInformationId",
                principalTable: "ProductInformations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ProductInformations_ProductInformationId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "ProductInformations");

            migrationBuilder.AlterColumn<int>(
                name: "ProductInformationId",
                table: "Products",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ProductInformations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Review",
                table: "ProductInformations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ProductInformations_ProductInformationId",
                table: "Products",
                column: "ProductInformationId",
                principalTable: "ProductInformations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
