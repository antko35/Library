using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class upd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserRole_Roles_RoleEntityId",
                table: "UserRole");

            migrationBuilder.DropForeignKey(
                name: "FK_UserRole_Users_UserEntityId",
                table: "UserRole");

            migrationBuilder.DropIndex(
                name: "IX_UserRole_RoleEntityId",
                table: "UserRole");

            migrationBuilder.DropIndex(
                name: "IX_UserRole_UserEntityId",
                table: "UserRole");

            migrationBuilder.DropColumn(
                name: "RoleEntityId",
                table: "UserRole");

            migrationBuilder.DropColumn(
                name: "UserEntityId",
                table: "UserRole");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RoleEntityId",
                table: "UserRole",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserEntityId",
                table: "UserRole",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_RoleEntityId",
                table: "UserRole",
                column: "RoleEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_UserEntityId",
                table: "UserRole",
                column: "UserEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRole_Roles_RoleEntityId",
                table: "UserRole",
                column: "RoleEntityId",
                principalTable: "Roles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserRole_Users_UserEntityId",
                table: "UserRole",
                column: "UserEntityId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
