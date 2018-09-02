using Microsoft.EntityFrameworkCore.Migrations;

namespace Blog.Data.Migrations
{
    public partial class CensoredFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsCensored",
                table: "Replies",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsCensored",
                table: "Comments",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "CensoredCommentsCount",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DeletedCommentsCount",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCensored",
                table: "Replies");

            migrationBuilder.DropColumn(
                name: "IsCensored",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "CensoredCommentsCount",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "DeletedCommentsCount",
                table: "AspNetUsers");
        }
    }
}
