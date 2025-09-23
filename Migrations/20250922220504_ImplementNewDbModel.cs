using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotnetAPI.Migrations
{
    /// <inheritdoc />
    public partial class ImplementNewDbModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConfigBess");

            migrationBuilder.DropTable(
                name: "Modules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "users");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "users",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "users",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "users",
                newName: "user_name");

            migrationBuilder.RenameColumn(
                name: "PasswordHash",
                table: "users",
                newName: "password_hash");

            migrationBuilder.RenameIndex(
                name: "IX_Users_Email",
                table: "users",
                newName: "ix_users_email");

            migrationBuilder.RenameIndex(
                name: "IX_Users_UserName",
                table: "users",
                newName: "ix_users_user_name");

            migrationBuilder.AddPrimaryKey(
                name: "pk_users",
                table: "users",
                column: "id");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PCS");

            migrationBuilder.DropTable(
                name: "BATTERY");

            migrationBuilder.DropTable(
                name: "PCS_MODEL");

            migrationBuilder.DropTable(
                name: "BESS");

            migrationBuilder.DropTable(
                name: "operation_modes");

            migrationBuilder.DropPrimaryKey(
                name: "pk_users",
                table: "users");

            migrationBuilder.RenameTable(
                name: "users",
                newName: "Users");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "Users",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Users",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "user_name",
                table: "Users",
                newName: "UserName");

            migrationBuilder.RenameColumn(
                name: "password_hash",
                table: "Users",
                newName: "PasswordHash");

            migrationBuilder.RenameIndex(
                name: "ix_users_email",
                table: "Users",
                newName: "IX_Users_Email");

            migrationBuilder.RenameIndex(
                name: "ix_users_user_name",
                table: "Users",
                newName: "IX_Users_UserName");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Modules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 120, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConfigBess",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ModuleId = table.Column<Guid>(type: "TEXT", nullable: false),
                    MaxDCCurrent = table.Column<int>(type: "INTEGER", nullable: false),
                    MinDCCurrent = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfigBess", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConfigBess_Modules_ModuleId",
                        column: x => x.ModuleId,
                        principalTable: "Modules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConfigBess_ModuleId",
                table: "ConfigBess",
                column: "ModuleId");
        }
    }
}
