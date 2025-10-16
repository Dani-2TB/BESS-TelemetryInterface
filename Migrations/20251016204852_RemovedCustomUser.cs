using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotnetAPI.Migrations
{
    /// <inheritdoc />
    public partial class RemovedCustomUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_users",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "APP-USER");

            migrationBuilder.RenameIndex(
                name: "ix_users_user_name",
                table: "APP-USER",
                newName: "ix_app_user_user_name");

            migrationBuilder.RenameIndex(
                name: "ix_users_email",
                table: "APP-USER",
                newName: "ix_app_user_email");

            migrationBuilder.AddPrimaryKey(
                name: "pk_app_user",
                table: "APP-USER",
                column: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_app_user",
                table: "APP-USER");

            migrationBuilder.RenameTable(
                name: "APP-USER",
                newName: "Users");

            migrationBuilder.RenameIndex(
                name: "ix_app_user_user_name",
                table: "Users",
                newName: "ix_users_user_name");

            migrationBuilder.RenameIndex(
                name: "ix_app_user_email",
                table: "Users",
                newName: "ix_users_email");

            migrationBuilder.AddPrimaryKey(
                name: "pk_users",
                table: "Users",
                column: "id");
        }
    }
}
