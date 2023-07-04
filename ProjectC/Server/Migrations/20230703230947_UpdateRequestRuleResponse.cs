using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectC.Server.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRequestRuleResponse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResponseMethod",
                table: "RequestRule");

            migrationBuilder.AddColumn<int>(
                name: "ResponseStatus",
                table: "RequestRule",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResponseStatus",
                table: "RequestRule");

            migrationBuilder.AddColumn<string>(
                name: "ResponseMethod",
                table: "RequestRule",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
