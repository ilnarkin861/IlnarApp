using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IlnarApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedCreatedAtField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "Note",
                newName: "CreatedAt");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "NoteType",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "NoteType");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Note",
                newName: "CreatedDate");
        }
    }
}
