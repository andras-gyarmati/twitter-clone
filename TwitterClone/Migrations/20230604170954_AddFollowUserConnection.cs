using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwitterClone.Migrations
{
    /// <inheritdoc />
    public partial class AddFollowUserConnection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserUser",
                columns: table => new
                {
                    FollowedId = table.Column<int>(type: "INTEGER", nullable: false),
                    FollowerId = table.Column<int>(type: "INTEGER", nullable: false),
                    FollowDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserUser", x => new { x.FollowedId, x.FollowerId });
                    table.ForeignKey(
                        name: "FK_UserUser_User_FollowedId",
                        column: x => x.FollowedId,
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserUser_User_FollowerId",
                        column: x => x.FollowerId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserUser_FollowerId",
                table: "UserUser",
                column: "FollowerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserUser");
        }
    }
}
