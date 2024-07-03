using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectC.Server.Migrations
{
    /// <inheritdoc />
    public partial class RenameWorkflowActionToRequestRuleVariant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestEvent_WorkflowAction_WorkflowActionId",
                table: "RequestEvent");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowTrigger_WorkflowAction_WorkflowActionId",
                table: "WorkflowTrigger");

            migrationBuilder.DropTable(
                name: "WorkflowAction");

            migrationBuilder.RenameColumn(
                name: "WorkflowActionId",
                table: "WorkflowTrigger",
                newName: "RequestRuleVariantId");

            migrationBuilder.RenameIndex(
                name: "IX_WorkflowTrigger_WorkflowActionId",
                table: "WorkflowTrigger",
                newName: "IX_WorkflowTrigger_RequestRuleVariantId");

            migrationBuilder.RenameColumn(
                name: "WorkflowActionId",
                table: "RequestEvent",
                newName: "RequestRuleVariantId");

            migrationBuilder.RenameIndex(
                name: "IX_RequestEvent_WorkflowActionId",
                table: "RequestEvent",
                newName: "IX_RequestEvent_RequestRuleVariantId");

            migrationBuilder.CreateTable(
                name: "RequestRuleVariant",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Method = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PathRegex = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResponseStatus = table.Column<int>(type: "int", nullable: false),
                    ResponseDelay = table.Column<int>(type: "int", nullable: false),
                    ResponseHeaders = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResponseBody = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestRuleId = table.Column<int>(type: "int", nullable: true),
                    WorkflowId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestRuleVariant", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequestRuleVariant_RequestRule_RequestRuleId",
                        column: x => x.RequestRuleId,
                        principalTable: "RequestRule",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RequestRuleVariant_Workflow_WorkflowId",
                        column: x => x.WorkflowId,
                        principalTable: "Workflow",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RequestRuleVariant_RequestRuleId",
                table: "RequestRuleVariant",
                column: "RequestRuleId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestRuleVariant_WorkflowId",
                table: "RequestRuleVariant",
                column: "WorkflowId");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestEvent_RequestRuleVariant_RequestRuleVariantId",
                table: "RequestEvent",
                column: "RequestRuleVariantId",
                principalTable: "RequestRuleVariant",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowTrigger_RequestRuleVariant_RequestRuleVariantId",
                table: "WorkflowTrigger",
                column: "RequestRuleVariantId",
                principalTable: "RequestRuleVariant",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestEvent_RequestRuleVariant_RequestRuleVariantId",
                table: "RequestEvent");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowTrigger_RequestRuleVariant_RequestRuleVariantId",
                table: "WorkflowTrigger");

            migrationBuilder.DropTable(
                name: "RequestRuleVariant");

            migrationBuilder.RenameColumn(
                name: "RequestRuleVariantId",
                table: "WorkflowTrigger",
                newName: "WorkflowActionId");

            migrationBuilder.RenameIndex(
                name: "IX_WorkflowTrigger_RequestRuleVariantId",
                table: "WorkflowTrigger",
                newName: "IX_WorkflowTrigger_WorkflowActionId");

            migrationBuilder.RenameColumn(
                name: "RequestRuleVariantId",
                table: "RequestEvent",
                newName: "WorkflowActionId");

            migrationBuilder.RenameIndex(
                name: "IX_RequestEvent_RequestRuleVariantId",
                table: "RequestEvent",
                newName: "IX_RequestEvent_WorkflowActionId");

            migrationBuilder.CreateTable(
                name: "WorkflowAction",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestRuleId = table.Column<int>(type: "int", nullable: true),
                    WorkflowId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Method = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PathRegex = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseBody = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResponseDelay = table.Column<int>(type: "int", nullable: false),
                    ResponseHeaders = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResponseStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowAction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkflowAction_RequestRule_RequestRuleId",
                        column: x => x.RequestRuleId,
                        principalTable: "RequestRule",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WorkflowAction_Workflow_WorkflowId",
                        column: x => x.WorkflowId,
                        principalTable: "Workflow",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowAction_RequestRuleId",
                table: "WorkflowAction",
                column: "RequestRuleId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowAction_WorkflowId",
                table: "WorkflowAction",
                column: "WorkflowId");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestEvent_WorkflowAction_WorkflowActionId",
                table: "RequestEvent",
                column: "WorkflowActionId",
                principalTable: "WorkflowAction",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowTrigger_WorkflowAction_WorkflowActionId",
                table: "WorkflowTrigger",
                column: "WorkflowActionId",
                principalTable: "WorkflowAction",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
