using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoFit.Infrastructure.Contexts.GoFitDb.Migrations
{
    /// <inheritdoc />
    public partial class AddAppUserIdToAthlete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "Athletes",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Athletes");
        }
    }
}
