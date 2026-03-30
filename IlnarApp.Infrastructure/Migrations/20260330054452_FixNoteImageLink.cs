using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IlnarApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixNoteImageLink : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NoteNoteImage_NoteImage_NoteImagesId",
                table: "NoteNoteImage");

            migrationBuilder.DropForeignKey(
                name: "FK_NoteNoteImage_Note_NotesId",
                table: "NoteNoteImage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NoteNoteImage",
                table: "NoteNoteImage");

            migrationBuilder.RenameTable(
                name: "NoteNoteImage",
                newName: "NoteNoteImages");

            migrationBuilder.RenameIndex(
                name: "IX_NoteNoteImage_NotesId",
                table: "NoteNoteImages",
                newName: "IX_NoteNoteImages_NotesId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NoteNoteImages",
                table: "NoteNoteImages",
                columns: new[] { "NoteImagesId", "NotesId" });

            migrationBuilder.AddForeignKey(
                name: "FK_NoteNoteImages_NoteImage_NoteImagesId",
                table: "NoteNoteImages",
                column: "NoteImagesId",
                principalTable: "NoteImage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NoteNoteImages_Note_NotesId",
                table: "NoteNoteImages",
                column: "NotesId",
                principalTable: "Note",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NoteNoteImages_NoteImage_NoteImagesId",
                table: "NoteNoteImages");

            migrationBuilder.DropForeignKey(
                name: "FK_NoteNoteImages_Note_NotesId",
                table: "NoteNoteImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NoteNoteImages",
                table: "NoteNoteImages");

            migrationBuilder.RenameTable(
                name: "NoteNoteImages",
                newName: "NoteNoteImage");

            migrationBuilder.RenameIndex(
                name: "IX_NoteNoteImages_NotesId",
                table: "NoteNoteImage",
                newName: "IX_NoteNoteImage_NotesId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NoteNoteImage",
                table: "NoteNoteImage",
                columns: new[] { "NoteImagesId", "NotesId" });

            migrationBuilder.AddForeignKey(
                name: "FK_NoteNoteImage_NoteImage_NoteImagesId",
                table: "NoteNoteImage",
                column: "NoteImagesId",
                principalTable: "NoteImage",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NoteNoteImage_Note_NotesId",
                table: "NoteNoteImage",
                column: "NotesId",
                principalTable: "Note",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
