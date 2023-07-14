using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectC.Server.Migrations
{
    /// <inheritdoc />
    public partial class ChangeWorkflowActionRequestRuleToNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowAction_RequestRule_RequestRuleId",
                table: "WorkflowAction");

            migrationBuilder.AlterColumn<int>(
                name: "RequestRuleId",
                table: "WorkflowAction",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowAction_RequestRule_RequestRuleId",
                table: "WorkflowAction",
                column: "RequestRuleId",
                principalTable: "RequestRule",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowAction_RequestRule_RequestRuleId",
                table: "WorkflowAction");

            migrationBuilder.AlterColumn<int>(
                name: "RequestRuleId",
                table: "WorkflowAction",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowAction_RequestRule_RequestRuleId",
                table: "WorkflowAction",
                column: "RequestRuleId",
                principalTable: "RequestRule",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
