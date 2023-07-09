using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectC.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddRequestEventRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RequestRuleId",
                table: "RequestEvent",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WebhookRuleId",
                table: "RequestEvent",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RequestEvent_RequestRuleId",
                table: "RequestEvent",
                column: "RequestRuleId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestEvent_WebhookRuleId",
                table: "RequestEvent",
                column: "WebhookRuleId");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestEvent_RequestRule_RequestRuleId",
                table: "RequestEvent",
                column: "RequestRuleId",
                principalTable: "RequestRule",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestEvent_WebhookRule_WebhookRuleId",
                table: "RequestEvent",
                column: "WebhookRuleId",
                principalTable: "WebhookRule",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestEvent_RequestRule_RequestRuleId",
                table: "RequestEvent");

            migrationBuilder.DropForeignKey(
                name: "FK_RequestEvent_WebhookRule_WebhookRuleId",
                table: "RequestEvent");

            migrationBuilder.DropIndex(
                name: "IX_RequestEvent_RequestRuleId",
                table: "RequestEvent");

            migrationBuilder.DropIndex(
                name: "IX_RequestEvent_WebhookRuleId",
                table: "RequestEvent");

            migrationBuilder.DropColumn(
                name: "RequestRuleId",
                table: "RequestEvent");

            migrationBuilder.DropColumn(
                name: "WebhookRuleId",
                table: "RequestEvent");
        }
    }
}
