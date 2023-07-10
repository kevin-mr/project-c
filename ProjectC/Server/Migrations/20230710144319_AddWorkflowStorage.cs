using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectC.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddWorkflowStorage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowAction_Workflow_WorkFlowId",
                table: "WorkflowAction");

            migrationBuilder.RenameColumn(
                name: "WorkFlowId",
                table: "WorkflowAction",
                newName: "WorkflowId");

            migrationBuilder.RenameIndex(
                name: "IX_WorkflowAction_WorkFlowId",
                table: "WorkflowAction",
                newName: "IX_WorkflowAction_WorkflowId");

            migrationBuilder.CreateTable(
                name: "WorkflowStorage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PropertyIdentifier = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Data = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WorkflowId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowStorage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkflowStorage_Workflow_WorkflowId",
                        column: x => x.WorkflowId,
                        principalTable: "Workflow",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowStorage_WorkflowId",
                table: "WorkflowStorage",
                column: "WorkflowId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowAction_Workflow_WorkflowId",
                table: "WorkflowAction",
                column: "WorkflowId",
                principalTable: "Workflow",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowAction_Workflow_WorkflowId",
                table: "WorkflowAction");

            migrationBuilder.DropTable(
                name: "WorkflowStorage");

            migrationBuilder.RenameColumn(
                name: "WorkflowId",
                table: "WorkflowAction",
                newName: "WorkFlowId");

            migrationBuilder.RenameIndex(
                name: "IX_WorkflowAction_WorkflowId",
                table: "WorkflowAction",
                newName: "IX_WorkflowAction_WorkFlowId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowAction_Workflow_WorkFlowId",
                table: "WorkflowAction",
                column: "WorkFlowId",
                principalTable: "Workflow",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
