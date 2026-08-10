using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SantaClauzer.Database.Migrations
{
    /// <inheritdoc />
    public partial class QuickRelationsFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokens_Users_UserModelId",
                table: "RefreshTokens");

            migrationBuilder.RenameColumn(
                name: "UserModelId",
                table: "RefreshTokens",
                newName: "RoleId");

            migrationBuilder.RenameIndex(
                name: "IX_RefreshTokens_UserModelId",
                table: "RefreshTokens",
                newName: "IX_RefreshTokens_RoleId");

            migrationBuilder.AlterColumn<int>(
                name: "CreatorId",
                table: "PresentGroups",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PresentGroups_CreatorId",
                table: "PresentGroups",
                column: "CreatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_PresentGroups_Users_CreatorId",
                table: "PresentGroups",
                column: "CreatorId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokens_Roles_RoleId",
                table: "RefreshTokens",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokens_Users_UserId",
                table: "RefreshTokens",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PresentGroups_Users_CreatorId",
                table: "PresentGroups");

            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokens_Roles_RoleId",
                table: "RefreshTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokens_Users_UserId",
                table: "RefreshTokens");

            migrationBuilder.DropIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens");

            migrationBuilder.DropIndex(
                name: "IX_PresentGroups_CreatorId",
                table: "PresentGroups");

            migrationBuilder.RenameColumn(
                name: "RoleId",
                table: "RefreshTokens",
                newName: "UserModelId");

            migrationBuilder.RenameIndex(
                name: "IX_RefreshTokens_RoleId",
                table: "RefreshTokens",
                newName: "IX_RefreshTokens_UserModelId");

            migrationBuilder.AlterColumn<int>(
                name: "CreatorId",
                table: "PresentGroups",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokens_Users_UserModelId",
                table: "RefreshTokens",
                column: "UserModelId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
