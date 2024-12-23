using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPLE.Migrations
{
    /// <inheritdoc />
    public partial class AddDataValue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Value",
                table: "Data",
                type: "longchar",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Value",
                table: "Data");
        }
    }
}
