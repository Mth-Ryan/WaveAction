using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WaveAction.Migrations
{
    /// <inheritdoc />
    public partial class AditionalInfos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ThumbnailUrl",
                table: "Threads",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PublicEmail",
                table: "Profiles",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShortBio",
                table: "Profiles",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ThumbnailUrl",
                table: "Posts",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ThumbnailUrl",
                table: "Threads");

            migrationBuilder.DropColumn(
                name: "PublicEmail",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "ShortBio",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "ThumbnailUrl",
                table: "Posts");
        }
    }
}
