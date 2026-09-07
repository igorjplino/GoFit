using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoFit.Infrastructure.Contexts.GoFitDb.Migrations
{
    /// <inheritdoc />
    public partial class AddAthleteIdAndCancelledDateToWorkoutTracking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AthleteId",
                table: "WorkoutsTracking",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CancelledDate",
                table: "WorkoutsTracking",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutsTracking_AthleteId",
                table: "WorkoutsTracking",
                column: "AthleteId",
                unique: true,
                filter: "[EndWorkoutDate] IS NULL AND [CancelledDate] IS NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutsTracking_Athletes_AthleteId",
                table: "WorkoutsTracking",
                column: "AthleteId",
                principalTable: "Athletes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutsTracking_Athletes_AthleteId",
                table: "WorkoutsTracking");

            migrationBuilder.DropIndex(
                name: "IX_WorkoutsTracking_AthleteId",
                table: "WorkoutsTracking");

            migrationBuilder.DropColumn(
                name: "AthleteId",
                table: "WorkoutsTracking");

            migrationBuilder.DropColumn(
                name: "CancelledDate",
                table: "WorkoutsTracking");
        }
    }
}
