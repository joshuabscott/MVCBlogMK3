using Microsoft.EntityFrameworkCore.Migrations;

namespace MVCBlogMK3.Migrations
{
    public partial class ImageUtility : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageDataUrl",
                table: "Posts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageDataUrl",
                table: "Posts");
        }
    }
}
