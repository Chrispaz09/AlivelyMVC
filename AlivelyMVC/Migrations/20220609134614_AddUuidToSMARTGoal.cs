using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AlivelyMVC.Migrations
{
    public partial class AddUuidToSMARTGoal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "Uuid",
                table: "SMARTGoals",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Uuid",
                table: "SMARTGoals");
        }
    }
}
