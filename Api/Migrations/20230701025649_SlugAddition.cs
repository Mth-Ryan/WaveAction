using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WaveActionApi.Migrations
{
    /// <inheritdoc />
    public partial class SlugAddition : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TitleSlug",
                table: "Threads",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TitleSlug",
                table: "Posts",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TitleSlug",
                table: "Threads");

            migrationBuilder.DropColumn(
                name: "TitleSlug",
                table: "Posts");
        }
    }
}
