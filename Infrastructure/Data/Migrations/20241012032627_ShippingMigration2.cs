using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class ShippingMigration2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shippings_Address_AddressId",
                table: "Shippings");

            migrationBuilder.DropIndex(
                name: "IX_Shippings_AddressId",
                table: "Shippings");

            migrationBuilder.DropColumn(
                name: "AddressId",
                table: "Shippings");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AddressId",
                table: "Shippings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Shippings_AddressId",
                table: "Shippings",
                column: "AddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_Shippings_Address_AddressId",
                table: "Shippings",
                column: "AddressId",
                principalTable: "Address",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
