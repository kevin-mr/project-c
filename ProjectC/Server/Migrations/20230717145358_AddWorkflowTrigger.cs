using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectC.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddWorkflowTrigger : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WorkflowTrigger",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WorkflowActionId = table.Column<int>(type: "int", nullable: false),
                    WebhookEventId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowTrigger", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkflowTrigger_WebhookEvent_WebhookEventId",
                        column: x => x.WebhookEventId,
                        principalTable: "WebhookEvent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkflowTrigger_WorkflowAction_WorkflowActionId",
                        column: x => x.WorkflowActionId,
                        principalTable: "WorkflowAction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowTrigger_WebhookEventId",
                table: "WorkflowTrigger",
                column: "WebhookEventId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowTrigger_WorkflowActionId",
                table: "WorkflowTrigger",
                column: "WorkflowActionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkflowTrigger");
        }
    }
}
