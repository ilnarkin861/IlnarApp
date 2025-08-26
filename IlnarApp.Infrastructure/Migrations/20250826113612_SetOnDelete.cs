using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IlnarApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SetOnDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Note_Archive_ArchiveId",
                table: "Note");

            migrationBuilder.AddForeignKey(
                name: "FK_Note_Archive_ArchiveId",
                table: "Note",
                column: "ArchiveId",
                principalTable: "Archive",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Note_Archive_ArchiveId",
                table: "Note");

            migrationBuilder.AddForeignKey(
                name: "FK_Note_Archive_ArchiveId",
                table: "Note",
                column: "ArchiveId",
                principalTable: "Archive",
                principalColumn: "Id");
        }
    }
}
