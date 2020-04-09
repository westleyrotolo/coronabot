using Microsoft.EntityFrameworkCore.Migrations;

namespace CovidBot.Migrations
{
    public partial class updatefaq : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FAQIntents",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Intent = table.Column<string>(nullable: true),
                    Answer = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FAQIntents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    State = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FAQQuestions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Question = table.Column<string>(nullable: true),
                    FAQIntentId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FAQQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FAQQuestions_FAQIntents_FAQIntentId",
                        column: x => x.FAQIntentId,
                        principalTable: "FAQIntents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SelfCertifications",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Step = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    DateOfBorn = table.Column<string>(nullable: true),
                    PlaceOfBorn = table.Column<string>(nullable: true),
                    ResidenceCity = table.Column<string>(nullable: true),
                    ResidenceAddress = table.Column<string>(nullable: true),
                    DomicileCity = table.Column<string>(nullable: true),
                    DomicileAddress = table.Column<string>(nullable: true),
                    IdentificationType = table.Column<string>(nullable: true),
                    IdentificationNumber = table.Column<string>(nullable: true),
                    IdentificationReleased = table.Column<string>(nullable: true),
                    IdentificationDate = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    StartPlace = table.Column<string>(nullable: true),
                    EndPlace = table.Column<string>(nullable: true),
                    StartRegion = table.Column<string>(nullable: true),
                    EndRegion = table.Column<string>(nullable: true),
                    Reason = table.Column<string>(nullable: true),
                    Note = table.Column<string>(nullable: true),
                    IsOpen = table.Column<bool>(nullable: false),
                    X1Work = table.Column<bool>(nullable: false),
                    X2Urgency = table.Column<bool>(nullable: false),
                    X3Necessary = table.Column<bool>(nullable: false),
                    X4Health = table.Column<bool>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SelfCertifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SelfCertifications_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FAQIntents_Intent",
                table: "FAQIntents",
                column: "Intent",
                unique: true,
                filter: "[Intent] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_FAQQuestions_FAQIntentId",
                table: "FAQQuestions",
                column: "FAQIntentId");

            migrationBuilder.CreateIndex(
                name: "IX_SelfCertifications_UserId",
                table: "SelfCertifications",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FAQQuestions");

            migrationBuilder.DropTable(
                name: "SelfCertifications");

            migrationBuilder.DropTable(
                name: "FAQIntents");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
