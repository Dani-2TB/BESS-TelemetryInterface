using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotnetAPI.Migrations
{
    /// <inheritdoc />
    public partial class CreatedAuditLogTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "modbus_id",
                table: "PCS",
                newName: "can_id");

            migrationBuilder.RenameIndex(
                name: "ix_pcs_modbus_id",
                table: "PCS",
                newName: "ix_pcs_can_id");

            migrationBuilder.CreateTable(
                name: "AUDIT_LOG",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    timestamp = table.Column<DateTimeOffset>(type: "TEXT", nullable: false, defaultValueSql: "SYSDATETIMEOFFSET()"),
                    log_level = table.Column<string>(type: "TEXT", nullable: false),
                    category = table.Column<string>(type: "TEXT", nullable: false),
                    message = table.Column<string>(type: "TEXT", nullable: false),
                    entity_type = table.Column<string>(type: "TEXT", nullable: true),
                    entity_id = table.Column<string>(type: "TEXT", nullable: true),
                    changes = table.Column<string>(type: "TEXT", nullable: true),
                    detail = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_audit_log", x => x.id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AUDIT_LOG");

            migrationBuilder.RenameColumn(
                name: "can_id",
                table: "PCS",
                newName: "modbus_id");

            migrationBuilder.RenameIndex(
                name: "ix_pcs_can_id",
                table: "PCS",
                newName: "ix_pcs_modbus_id");
        }
    }
}
