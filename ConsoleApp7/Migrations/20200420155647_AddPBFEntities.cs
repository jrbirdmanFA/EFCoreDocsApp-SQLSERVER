using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ConsoleApp7.Migrations
{
    public partial class AddPBFEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BlogsPBF",
                columns: table => new
                {
                    BlogId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Url = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogsPBF", x => x.BlogId);
                });

            migrationBuilder.CreateTable(
                name: "PostsPBF",
                columns: table => new
                {
                    PostId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    BlogId = table.Column<int>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    Stupid = table.Column<bool>(nullable: true),
                    VideoTitle = table.Column<string>(nullable: true),
                    ReleaseDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostsPBF", x => x.PostId);
                    table.ForeignKey(
                        name: "FK_PostsPBF_BlogsPBF_BlogId",
                        column: x => x.BlogId,
                        principalTable: "BlogsPBF",
                        principalColumn: "BlogId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PostsPBF_BlogId",
                table: "PostsPBF",
                column: "BlogId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PostsPBF");

            migrationBuilder.DropTable(
                name: "BlogsPBF");
        }
    }
}
