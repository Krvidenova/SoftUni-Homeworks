namespace Blog.Data.Migrations
{
    using System;
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class PostLastUpdateDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdateDate",
                table: "Posts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastUpdateDate",
                table: "Posts");
        }
    }
}
