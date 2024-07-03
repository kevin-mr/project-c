using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectC.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddRequestEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RequestEvent",
                columns: table =>
                    new
                    {
                        Id = table
                            .Column<int>(type: "int", nullable: false)
                            .Annotation("SqlServer:Identity", "1, 1"),
                        Method = table.Column<string>(type: "nvarchar(max)", nullable: false),
                        JsonHeaders = table.Column<string>(type: "nvarchar(max)", nullable: false),
                        JsonBody = table.Column<string>(type: "nvarchar(max)", nullable: false),
                        ArrivalDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                    },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestEvent", x => x.Id);
                }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "RequestEvent");
        }
    }
}
