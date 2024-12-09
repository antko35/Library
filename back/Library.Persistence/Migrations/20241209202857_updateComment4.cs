﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class updateComment4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           /* migrationBuilder.AddColumn<DateTime>(
                name: "WritingDate",
                table: "Comments",
                type: "timestamp with time zone",
                nullable: true);*/
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WritingDate",
                table: "Comments");
        }
    }
}
