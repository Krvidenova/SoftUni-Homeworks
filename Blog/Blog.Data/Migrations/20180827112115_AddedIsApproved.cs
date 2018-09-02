using Microsoft.EntityFrameworkCore.Migrations;

namespace Blog.Data.Migrations
{
    public partial class AddedIsApproved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "Replies",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "Comments",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "Replies");

            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "Comments");
        }
    }
}
