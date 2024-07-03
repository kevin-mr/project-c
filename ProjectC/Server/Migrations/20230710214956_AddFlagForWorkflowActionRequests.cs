using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectC.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddFlagForWorkflowActionRequests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ForWorkflowAction",
                table: "RequestEvent",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ForWorkflowAction",
                table: "RequestEvent");
        }
    }
}
