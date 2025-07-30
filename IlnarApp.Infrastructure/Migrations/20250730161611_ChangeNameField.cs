using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IlnarApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeNameField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Tag",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Archive",
                newName: "Title");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Tag",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Archive",
                newName: "Name");
        }
    }
}
