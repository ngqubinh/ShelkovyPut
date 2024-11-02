using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitMigration3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Demos_Address_AddressId",
                table: "Demos");

            migrationBuilder.DropIndex(
                name: "IX_Demos_AddressId",
                table: "Demos");

            migrationBuilder.DropColumn(
                name: "AddressId",
                table: "Demos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AddressId",
                table: "Demos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Demos_AddressId",
                table: "Demos",
                column: "AddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_Demos_Address_AddressId",
                table: "Demos",
                column: "AddressId",
                principalTable: "Address",
                principalColumn: "Id");
        }
    }
}
