using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmeraldHerald.Migrations
{
    /// <inheritdoc />
    public partial class DeadlineInHours : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DeadlineInHours",
                table: "Objectives",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeadlineInHours",
                table: "Objectives");
        }
    }
}
