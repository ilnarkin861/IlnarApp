using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IlnarApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemovedNoteImageModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NoteNoteImages");

            migrationBuilder.DropTable(
                name: "NoteImage");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NoteImage",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Deleted = table.Column<bool>(type: "boolean", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NoteImage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NoteNoteImages",
                columns: table => new
                {
                    NoteImagesId = table.Column<Guid>(type: "uuid", nullable: false),
                    NotesId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NoteNoteImages", x => new { x.NoteImagesId, x.NotesId });
                    table.ForeignKey(
                        name: "FK_NoteNoteImages_NoteImage_NoteImagesId",
                        column: x => x.NoteImagesId,
                        principalTable: "NoteImage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NoteNoteImages_Note_NotesId",
                        column: x => x.NotesId,
                        principalTable: "Note",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NoteNoteImages_NotesId",
                table: "NoteNoteImages",
                column: "NotesId");
        }
    }
}
