namespace Blog.Data.Migrations
{
    using System;
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddedProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdateDate",
                table: "Replies",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdateDate",
                table: "Comments",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedInappropriateCommentsCount",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsBanned",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastUpdateDate",
                table: "Replies");

            migrationBuilder.DropColumn(
                name: "LastUpdateDate",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "DeletedInappropriateCommentsCount",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsBanned",
                table: "AspNetUsers");
        }
    }
}
