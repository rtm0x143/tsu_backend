using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieCatalogBackend.Migrations.TokenList
{
    public partial class TokenListInitial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tokens",
                columns: table => new
                {
                    Token = table.Column<string>(type: "NVARCHAR2(450)", nullable: false),
                    Expiretion = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tokens", x => x.Token);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tokens");
        }
    }
}
