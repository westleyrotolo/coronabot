using Microsoft.EntityFrameworkCore.Migrations;

namespace CovidBot.Migrations
{
    public partial class addfilename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Filepath",
                table: "FAQAnswers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Filepath",
                table: "FAQAnswers");
        }
    }
}
