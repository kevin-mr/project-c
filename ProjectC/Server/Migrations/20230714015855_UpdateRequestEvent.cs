using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectC.Server.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRequestEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ForWorkflowAction",
                table: "RequestEvent");

            migrationBuilder.AddColumn<int>(
                name: "WorkflowActionId",
                table: "RequestEvent",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RequestEvent_WorkflowActionId",
                table: "RequestEvent",
                column: "WorkflowActionId");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestEvent_WorkflowAction_WorkflowActionId",
                table: "RequestEvent",
                column: "WorkflowActionId",
                principalTable: "WorkflowAction",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestEvent_WorkflowAction_WorkflowActionId",
                table: "RequestEvent");

            migrationBuilder.DropIndex(
                name: "IX_RequestEvent_WorkflowActionId",
                table: "RequestEvent");

            migrationBuilder.DropColumn(
                name: "WorkflowActionId",
                table: "RequestEvent");

            migrationBuilder.AddColumn<bool>(
                name: "ForWorkflowAction",
                table: "RequestEvent",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
