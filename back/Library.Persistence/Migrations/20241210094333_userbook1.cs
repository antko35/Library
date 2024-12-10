using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class userbook1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserBook");
        }
    }
}
