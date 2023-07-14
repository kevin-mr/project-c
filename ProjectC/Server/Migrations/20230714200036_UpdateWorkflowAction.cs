using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectC.Server.Migrations
{
    /// <inheritdoc />
    public partial class UpdateWorkflowAction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "WorkflowAction",
                newName: "Description");

            migrationBuilder.AddColumn<string>(
                name: "Method",
                table: "WorkflowAction",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "WorkflowAction",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PathRegex",
                table: "WorkflowAction",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Method",
                table: "WorkflowAction");

            migrationBuilder.DropColumn(
                name: "Path",
                table: "WorkflowAction");

            migrationBuilder.DropColumn(
                name: "PathRegex",
                table: "WorkflowAction");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "WorkflowAction",
                newName: "Name");
        }
    }
}
