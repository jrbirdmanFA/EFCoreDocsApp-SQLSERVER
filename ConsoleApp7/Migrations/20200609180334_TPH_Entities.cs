using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ConsoleApp7.Migrations
{
    public partial class TPH_Entities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BlogsTPH",
                columns: table => new
                {
                    BlogId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Url = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogsTPH", x => x.BlogId);
                });

            migrationBuilder.CreateTable(
                name: "WidgetTPH",
                columns: table => new
                {
                    WidgetId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WidgetTPH", x => x.WidgetId);
                });

            migrationBuilder.CreateTable(
                name: "PostsTPH",
                columns: table => new
                {
                    PostId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    BlogId = table.Column<int>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    WidgetId = table.Column<int>(nullable: false),
                    Stupid = table.Column<bool>(nullable: true),
                    VideoTitle = table.Column<string>(nullable: true),
                    ReleaseDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostsTPH", x => x.PostId);
                    table.ForeignKey(
                        name: "FK_PostsTPH_BlogsTPH_BlogId",
                        column: x => x.BlogId,
                        principalTable: "BlogsTPH",
                        principalColumn: "BlogId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostsTPH_WidgetTPH_WidgetId",
                        column: x => x.WidgetId,
                        principalTable: "WidgetTPH",
                        principalColumn: "WidgetId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "PostsLL",
                keyColumn: "FostLLId",
                keyValue: 2,
                column: "ReleaseDate",
                value: new DateTime(2019, 8, 14, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.CreateIndex(
                name: "IX_PostsTPH_BlogId",
                table: "PostsTPH",
                column: "BlogId");

            migrationBuilder.CreateIndex(
                name: "IX_PostsTPH_WidgetId",
                table: "PostsTPH",
                column: "WidgetId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PostsTPH");

            migrationBuilder.DropTable(
                name: "BlogsTPH");

            migrationBuilder.DropTable(
                name: "WidgetTPH");

            migrationBuilder.UpdateData(
                table: "PostsLL",
                keyColumn: "FostLLId",
                keyValue: 2,
                column: "ReleaseDate",
                value: new DateTime(2019, 7, 18, 0, 0, 0, 0, DateTimeKind.Local));
        }
    }
}
