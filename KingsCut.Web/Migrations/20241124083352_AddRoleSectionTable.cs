using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KingsCut.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddRoleSectionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RoleServices",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    ServiceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleServices", x => new { x.RoleId, x.ServiceId });
                    table.ForeignKey(
                        name: "FK_RoleServices_KingsCutRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "KingsCutRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleServices_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoleServices_ServiceId",
                table: "RoleServices",
                column: "ServiceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoleServices");
        }
    }
}
