using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieCatalogBackend.Migrations
{
    public partial class InitialRedo2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Genre",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    Name = table.Column<string>(type: "NVARCHAR2(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genre", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Movie",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    Name = table.Column<string>(type: "NVARCHAR2(256)", maxLength: 256, nullable: true),
                    Poster = table.Column<string>(type: "NCLOB", maxLength: 8000, nullable: true),
                    Description = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    Year = table.Column<short>(type: "NUMBER(5)", nullable: false),
                    Country = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    Time = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    TagLine = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    Director = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    Budget = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    Fees = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    AgeLimit = table.Column<short>(type: "NUMBER(5)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movie", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    Role = table.Column<byte>(type: "NUMBER(3)", nullable: false),
                    Username = table.Column<string>(type: "NVARCHAR2(64)", maxLength: 64, nullable: false),
                    Name = table.Column<string>(type: "NVARCHAR2(64)", maxLength: 64, nullable: false),
                    BirthDate = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    Email = table.Column<string>(type: "NVARCHAR2(64)", maxLength: 64, nullable: false),
                    Password = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    Gender = table.Column<byte>(type: "NUMBER(3)", nullable: true),
                    Avatar = table.Column<string>(type: "NCLOB", maxLength: 8000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GenreMovie",
                columns: table => new
                {
                    GenresId = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    MoviesId = table.Column<Guid>(type: "RAW(16)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenreMovie", x => new { x.GenresId, x.MoviesId });
                    table.ForeignKey(
                        name: "FK_GenreMovie_Genre_GenresId",
                        column: x => x.GenresId,
                        principalTable: "Genre",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GenreMovie_Movie_MoviesId",
                        column: x => x.MoviesId,
                        principalTable: "Movie",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MovieUser",
                columns: table => new
                {
                    FavoritesId = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    UsersFavoredId = table.Column<Guid>(type: "RAW(16)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovieUser", x => new { x.FavoritesId, x.UsersFavoredId });
                    table.ForeignKey(
                        name: "FK_MovieUser_Movie_FavoritesId",
                        column: x => x.FavoritesId,
                        principalTable: "Movie",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MovieUser_User_UsersFavoredId",
                        column: x => x.UsersFavoredId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Review",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    TargetMovieId = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    CreatorId = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    ReviewText = table.Column<string>(type: "NVARCHAR2(2000)", nullable: false),
                    Rating = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    IsAnonymous = table.Column<bool>(type: "NUMBER(1)", nullable: false),
                    CreateDateTime = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Review", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Review_Movie_TargetMovieId",
                        column: x => x.TargetMovieId,
                        principalTable: "Movie",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Review_User_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GenreMovie_MoviesId",
                table: "GenreMovie",
                column: "MoviesId");

            migrationBuilder.CreateIndex(
                name: "IX_MovieUser_UsersFavoredId",
                table: "MovieUser",
                column: "UsersFavoredId");

            migrationBuilder.CreateIndex(
                name: "IX_Review_CreatorId",
                table: "Review",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Review_TargetMovieId",
                table: "Review",
                column: "TargetMovieId");

            migrationBuilder.CreateIndex(
                name: "IX_User_Email",
                table: "User",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_Username",
                table: "User",
                column: "Username",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GenreMovie");

            migrationBuilder.DropTable(
                name: "MovieUser");

            migrationBuilder.DropTable(
                name: "Review");

            migrationBuilder.DropTable(
                name: "Genre");

            migrationBuilder.DropTable(
                name: "Movie");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
