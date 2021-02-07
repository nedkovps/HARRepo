using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HARRepo.FileManager.Data.Migrations
{
    public partial class AddLastActivityToRepo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastActivityOn",
                table: "Repositories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastActivityOn",
                table: "Repositories");
        }
    }
}
