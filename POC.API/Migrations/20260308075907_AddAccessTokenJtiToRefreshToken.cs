using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace POC.API.Migrations
{
    /// <inheritdoc />
    public partial class AddAccessTokenJtiToRefreshToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AccessTokenJti",
                table: "RefreshTokens",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_AccessTokenJti",
                table: "RefreshTokens",
                column: "AccessTokenJti");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_RefreshTokens_AccessTokenJti",
                table: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "AccessTokenJti",
                table: "RefreshTokens");
        }
    }
}
