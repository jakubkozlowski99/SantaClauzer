using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SantaClauzer.Database.Migrations
{
    /// <inheritdoc />
    public partial class PresentGroupUserModel_AddedInvitationAcceptedParameter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "InvitationAccepted",
                table: "PresentGroupUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InvitationAccepted",
                table: "PresentGroupUsers");
        }
    }
}
