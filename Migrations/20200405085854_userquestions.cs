using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CovidBot.Migrations
{
    public partial class userquestions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Answer",
                table: "FAQIntents");

            migrationBuilder.AddColumn<int>(
                name: "ResponseType",
                table: "FAQIntents",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "FAQAnswers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Answer = table.Column<string>(nullable: true),
                    Filename = table.Column<string>(nullable: true),
                    FAQIntentId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FAQAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FAQAnswers_FAQIntents_FAQIntentId",
                        column: x => x.FAQIntentId,
                        principalTable: "FAQIntents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserQuestions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Question = table.Column<string>(nullable: true),
                    Intent = table.Column<string>(nullable: true),
                    Score = table.Column<double>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    UpdatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserQuestions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FAQAnswers_FAQIntentId",
                table: "FAQAnswers",
                column: "FAQIntentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FAQAnswers");

            migrationBuilder.DropTable(
                name: "UserQuestions");

            migrationBuilder.DropColumn(
                name: "ResponseType",
                table: "FAQIntents");

            migrationBuilder.AddColumn<string>(
                name: "Answer",
                table: "FAQIntents",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
