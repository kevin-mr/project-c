using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectC.Server.Migrations
{
    /// <inheritdoc />
    public partial class RenameWorkflowTriggerToRequestRuleTrigger : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkflowTrigger");

            migrationBuilder.CreateTable(
                name: "RequestRuleTrigger",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestRuleVariantId = table.Column<int>(type: "int", nullable: false),
                    WebhookEventId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestRuleTrigger", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequestRuleTrigger_RequestRuleVariant_RequestRuleVariantId",
                        column: x => x.RequestRuleVariantId,
                        principalTable: "RequestRuleVariant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RequestRuleTrigger_WebhookEvent_WebhookEventId",
                        column: x => x.WebhookEventId,
                        principalTable: "WebhookEvent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RequestRuleTrigger_RequestRuleVariantId",
                table: "RequestRuleTrigger",
                column: "RequestRuleVariantId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestRuleTrigger_WebhookEventId",
                table: "RequestRuleTrigger",
                column: "WebhookEventId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RequestRuleTrigger");

            migrationBuilder.CreateTable(
                name: "WorkflowTrigger",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestRuleVariantId = table.Column<int>(type: "int", nullable: false),
                    WebhookEventId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowTrigger", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkflowTrigger_RequestRuleVariant_RequestRuleVariantId",
                        column: x => x.RequestRuleVariantId,
                        principalTable: "RequestRuleVariant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkflowTrigger_WebhookEvent_WebhookEventId",
                        column: x => x.WebhookEventId,
                        principalTable: "WebhookEvent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowTrigger_RequestRuleVariantId",
                table: "WorkflowTrigger",
                column: "RequestRuleVariantId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowTrigger_WebhookEventId",
                table: "WorkflowTrigger",
                column: "WebhookEventId");
        }
    }
}
