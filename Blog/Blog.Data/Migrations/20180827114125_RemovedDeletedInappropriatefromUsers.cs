using Microsoft.EntityFrameworkCore.Migrations;

namespace Blog.Data.Migrations
{
    public partial class RemovedDeletedInappropriatefromUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedInappropriateCommentsCount",
                table: "AspNetUsers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DeletedInappropriateCommentsCount",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);
        }
    }
}
