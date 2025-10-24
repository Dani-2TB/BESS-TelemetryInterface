using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotnetAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "operation_modes",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_operation_modes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "PCS_MODEL",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    rated_power = table.Column<int>(type: "INTEGER", nullable: false),
                    voltage_max_dc = table.Column<int>(type: "INTEGER", nullable: false),
                    voltage_min_dc = table.Column<int>(type: "INTEGER", nullable: false),
                    current_max_dc = table.Column<int>(type: "INTEGER", nullable: false),
                    name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_pcs_model", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "TEXT", nullable: false),
                    user_name = table.Column<string>(type: "TEXT", maxLength: 64, nullable: false),
                    email = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    password_hash = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "BESS",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", nullable: false),
                    current_max_ac_in = table.Column<int>(type: "INTEGER", nullable: false),
                    current_max_ac_out = table.Column<int>(type: "INTEGER", nullable: false),
                    OPERATION_MODE_id = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_bess", x => x.id);
                    table.ForeignKey(
                        name: "OPERATION_MODE_BESS",
                        column: x => x.OPERATION_MODE_id,
                        principalTable: "operation_modes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BATTERY",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", nullable: false),
                    soc_max = table.Column<int>(type: "INTEGER", nullable: false),
                    soc_min = table.Column<int>(type: "INTEGER", nullable: false),
                    current_max = table.Column<int>(type: "INTEGER", nullable: false),
                    voltage_max = table.Column<int>(type: "INTEGER", nullable: false),
                    voltage_min = table.Column<int>(type: "INTEGER", nullable: false),
                    votage_absorption = table.Column<int>(type: "INTEGER", nullable: false),
                    current_charging = table.Column<int>(type: "INTEGER", nullable: false),
                    pwr_max = table.Column<int>(type: "INTEGER", nullable: false),
                    BESS_id = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_battery", x => x.id);
                    table.ForeignKey(
                        name: "BESS_BATTERY",
                        column: x => x.BESS_id,
                        principalTable: "BESS",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PCS",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    BATTERY_id = table.Column<int>(type: "INTEGER", nullable: false),
                    PCS_MODEL_id = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_pcs", x => x.id);
                    table.ForeignKey(
                        name: "BATTERY_PCS",
                        column: x => x.BATTERY_id,
                        principalTable: "BATTERY",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "PCS_MODEL",
                        column: x => x.PCS_MODEL_id,
                        principalTable: "PCS_MODEL",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_battery_bess_id",
                table: "BATTERY",
                column: "BESS_id");

            migrationBuilder.CreateIndex(
                name: "ix_bess_operation_mode_id",
                table: "BESS",
                column: "OPERATION_MODE_id");

            migrationBuilder.CreateIndex(
                name: "ix_pcs_battery_id",
                table: "PCS",
                column: "BATTERY_id");

            migrationBuilder.CreateIndex(
                name: "ix_pcs_pcs_model_id",
                table: "PCS",
                column: "PCS_MODEL_id");

            migrationBuilder.CreateIndex(
                name: "ix_users_email",
                table: "Users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_users_user_name",
                table: "Users",
                column: "user_name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PCS");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "BATTERY");

            migrationBuilder.DropTable(
                name: "PCS_MODEL");

            migrationBuilder.DropTable(
                name: "BESS");

            migrationBuilder.DropTable(
                name: "operation_modes");
        }
    }
}
