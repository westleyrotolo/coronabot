using Microsoft.EntityFrameworkCore.Migrations;

namespace CovidBot.Migrations
{
    public partial class categorysub : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "FAQIntents",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubCategory",
                table: "FAQIntents",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "FAQIntents");

            migrationBuilder.DropColumn(
                name: "SubCategory",
                table: "FAQIntents");
        }
    }
}
