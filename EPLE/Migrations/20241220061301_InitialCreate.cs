using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EPLE.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Data",
                columns: table => new
                {
                    Name = table.Column<string>(type: "varchar(255)", nullable: false),
                    Module = table.Column<string>(type: "longchar", nullable: true),
                    Group = table.Column<string>(type: "longchar", nullable: true),
                    Type = table.Column<string>(type: "longchar", nullable: false),
                    DeviceName = table.Column<string>(type: "longchar", nullable: false),
                    Direction = table.Column<string>(type: "longchar", nullable: false),
                    Command = table.Column<string>(type: "longchar", nullable: false),
                    PollingTime = table.Column<int>(type: "integer", nullable: false),
                    DataResetTimeout = table.Column<int>(type: "integer", nullable: false),
                    Use = table.Column<bool>(type: "smallint", nullable: false),
                    Description = table.Column<string>(type: "longchar", nullable: true),
                    DefaultValue = table.Column<string>(type: "longchar", nullable: true),
                    UpdateTime = table.Column<string>(type: "longchar", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Data", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "Device",
                columns: table => new
                {
                    DeviceName = table.Column<string>(type: "varchar(255)", nullable: false),
                    DeviceType = table.Column<string>(type: "longchar", nullable: false),
                    InstanceName = table.Column<string>(type: "longchar", nullable: false),
                    FileName = table.Column<string>(type: "longchar", nullable: false),
                    Use = table.Column<bool>(type: "smallint", nullable: false),
                    Args = table.Column<string>(type: "longchar", nullable: false),
                    Description = table.Column<string>(type: "longchar", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Device", x => x.DeviceName);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Data_Name",
                table: "Data",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Data");

            migrationBuilder.DropTable(
                name: "Device");
        }
    }
}
