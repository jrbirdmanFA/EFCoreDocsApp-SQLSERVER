using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ConsoleApp7.Migrations
{
    public partial class AddLLEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BlogsLL",
                columns: table => new
                {
                    FlogLLId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Url = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogsLL", x => x.FlogLLId);
                });

            migrationBuilder.CreateTable(
                name: "PostsLL",
                columns: table => new
                {
                    FostLLId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    FlogLLId = table.Column<int>(nullable: false),
                    PostType = table.Column<string>(nullable: false),
                    Stupid = table.Column<bool>(nullable: true),
                    VideoTitle = table.Column<string>(nullable: true),
                    ReleaseDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostsLL", x => x.FostLLId);
                    table.ForeignKey(
                        name: "FK_PostsLL_BlogsLL_FlogLLId",
                        column: x => x.FlogLLId,
                        principalTable: "BlogsLL",
                        principalColumn: "FlogLLId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "BlogsLL",
                columns: new[] { "FlogLLId", "Url" },
                values: new object[] { 1, "http://blogs.msdn.com/adonet" });

            migrationBuilder.InsertData(
                table: "PostsLL",
                columns: new[] { "FostLLId", "Content", "FlogLLId", "PostType", "Title", "Stupid" },
                values: new object[] { 3, "This is as dumb as a post", 1, "Curly", "Hello World, The Silent Picture", true });

            migrationBuilder.InsertData(
                table: "PostsLL",
                columns: new[] { "FostLLId", "Content", "FlogLLId", "PostType", "Title" },
                values: new object[] { 1, "I wrote an app using EF Core!", 1, "Moe", "Hello World" });

            migrationBuilder.InsertData(
                table: "PostsLL",
                columns: new[] { "FostLLId", "Content", "FlogLLId", "PostType", "Title", "ReleaseDate", "VideoTitle" },
                values: new object[] { 2, "Lareum ipsum, alpha, beta, crapper...", 1, "Larry", "Hello World, The Movie", new DateTime(2019, 7, 18, 0, 0, 0, 0, DateTimeKind.Local), "this is the Video Title" });

            migrationBuilder.CreateIndex(
                name: "IX_PostsLL_FlogLLId",
                table: "PostsLL",
                column: "FlogLLId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PostsLL");

            migrationBuilder.DropTable(
                name: "BlogsLL");
        }
    }
}
