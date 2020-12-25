using Microsoft.EntityFrameworkCore.Migrations;

namespace MVCBlogMK3.Migrations
{
    public partial class UPdateModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_AspNetUsers_AuthorId",
                table: "Comment");

            migrationBuilder.DropIndex(
                name: "IX_Comment_AuthorId",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Comment");

            migrationBuilder.AddColumn<string>(
                name: "BlogUserId",
                table: "Comment",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comment_BlogUserId",
                table: "Comment",
                column: "BlogUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_AspNetUsers_BlogUserId",
                table: "Comment",
                column: "BlogUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_AspNetUsers_BlogUserId",
                table: "Comment");

            migrationBuilder.DropIndex(
                name: "IX_Comment_BlogUserId",
                table: "Comment");

            migrationBuilder.DropColumn(
                name: "BlogUserId",
                table: "Comment");

            migrationBuilder.AddColumn<string>(
                name: "AuthorId",
                table: "Comment",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Comment_AuthorId",
                table: "Comment",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_AspNetUsers_AuthorId",
                table: "Comment",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
