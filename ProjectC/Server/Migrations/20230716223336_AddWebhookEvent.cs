using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectC.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddWebhookEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WebhookEvent",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Method = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JsonHeaders = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JsonBody = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WebhookRuleId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebhookEvent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WebhookEvent_WebhookRule_WebhookRuleId",
                        column: x => x.WebhookRuleId,
                        principalTable: "WebhookRule",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_WebhookEvent_WebhookRuleId",
                table: "WebhookEvent",
                column: "WebhookRuleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WebhookEvent");
        }
    }
}
