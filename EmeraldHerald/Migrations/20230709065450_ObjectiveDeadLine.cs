using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmeraldHerald.Migrations
{
    /// <inheritdoc />
    public partial class ObjectiveDeadLine : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Deadline",
                table: "Objectives",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deadline",
                table: "Objectives");
        }
    }
}
