using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotnetAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddedModbusID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "modbus_id",
                table: "PCS",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "modbus_id",
                table: "BATTERY",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "ix_pcs_modbus_id",
                table: "PCS",
                column: "modbus_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_battery_modbus_id",
                table: "BATTERY",
                column: "modbus_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_pcs_modbus_id",
                table: "PCS");

            migrationBuilder.DropIndex(
                name: "ix_battery_modbus_id",
                table: "BATTERY");

            migrationBuilder.DropColumn(
                name: "modbus_id",
                table: "PCS");

            migrationBuilder.DropColumn(
                name: "modbus_id",
                table: "BATTERY");
        }
    }
}
