using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieCatalogBackend.Migrations
{
    public partial class UniqueReviews : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Review_TargetMovieId",
                table: "Review");

            migrationBuilder.CreateIndex(
                name: "IX_Review_TargetMovieId_CreatorId",
                table: "Review",
                columns: new[] { "TargetMovieId", "CreatorId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Review_TargetMovieId_CreatorId",
                table: "Review");

            migrationBuilder.CreateIndex(
                name: "IX_Review_TargetMovieId",
                table: "Review",
                column: "TargetMovieId");
        }
    }
}
