using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieCatalogBackend.Migrations
{
    public partial class AddRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "Role",
                table: "User",
                type: "NUMBER(3)",
                nullable: false,
                defaultValue: (byte)0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "User");
        }
    }
}
