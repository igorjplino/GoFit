using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoFit.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddWorkoutTracking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WorkoutTracking",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    WorkoutId = table.Column<Guid>(type: "TEXT", nullable: false),
                    StartWorkoutDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndWorkoutDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Note = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutTracking", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkoutTracking_Workout_WorkoutId",
                        column: x => x.WorkoutId,
                        principalTable: "Workout",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkoutSetTracking",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    WorkoutTrackingId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Repetitions = table.Column<int>(type: "INTEGER", nullable: false),
                    Weight = table.Column<float>(type: "REAL", nullable: false),
                    Order = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutSetTracking", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkoutSetTracking_WorkoutTracking_WorkoutTrackingId",
                        column: x => x.WorkoutTrackingId,
                        principalTable: "WorkoutTracking",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutSetTracking_WorkoutTrackingId",
                table: "WorkoutSetTracking",
                column: "WorkoutTrackingId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutTracking_WorkoutId",
                table: "WorkoutTracking",
                column: "WorkoutId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkoutSetTracking");

            migrationBuilder.DropTable(
                name: "WorkoutTracking");
        }
    }
}
