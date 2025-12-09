using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IlnarApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddPinCodeField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PinCode",
                table: "Users",
                type: "character varying(4)",
                maxLength: 4,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PinCode",
                table: "Users");
        }
    }
}
