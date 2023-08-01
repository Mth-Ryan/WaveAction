using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WaveActionApi.Migrations
{
    /// <inheritdoc />
    public partial class SlugIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Threads_TitleSlug",
                table: "Threads",
                column: "TitleSlug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_TitleSlug",
                table: "Posts",
                column: "TitleSlug",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Threads_TitleSlug",
                table: "Threads");

            migrationBuilder.DropIndex(
                name: "IX_Posts_TitleSlug",
                table: "Posts");
        }
    }
}
