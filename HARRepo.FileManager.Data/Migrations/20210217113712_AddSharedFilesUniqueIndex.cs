using Microsoft.EntityFrameworkCore.Migrations;

namespace HARRepo.FileManager.Data.Migrations
{
    public partial class AddSharedFilesUniqueIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SharedFiles_FileId",
                table: "SharedFiles");

            migrationBuilder.CreateIndex(
                name: "IX_SharedFiles_FileId_OwnerId_SharedWithId",
                table: "SharedFiles",
                columns: new[] { "FileId", "OwnerId", "SharedWithId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SharedFiles_FileId_OwnerId_SharedWithId",
                table: "SharedFiles");

            migrationBuilder.CreateIndex(
                name: "IX_SharedFiles_FileId",
                table: "SharedFiles",
                column: "FileId");
        }
    }
}
