using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SantaClauzer.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddedPresentGroupUserRepo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Salt",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PresentGroupUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PresentGroupId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PresentGroupUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PresentGroupUsers_PresentGroups_PresentGroupId",
                        column: x => x.PresentGroupId,
                        principalTable: "PresentGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PresentGroupUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PresentGroupUsers_PresentGroupId",
                table: "PresentGroupUsers",
                column: "PresentGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_PresentGroupUsers_UserId",
                table: "PresentGroupUsers",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PresentGroupUsers");

            migrationBuilder.DropColumn(
                name: "Salt",
                table: "Users");
        }
    }
}
