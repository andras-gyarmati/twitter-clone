using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwitterClone.Migrations
{
    /// <inheritdoc />
    public partial class AddLikes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LikeCount",
                table: "Tweet");

            migrationBuilder.CreateTable(
                name: "UserTweetLikes",
                columns: table => new
                {
                    TweetId = table.Column<int>(type: "INTEGER", nullable: false),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTweetLikes", x => new { x.TweetId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserTweetLikes_Tweet_TweetId",
                        column: x => x.TweetId,
                        principalTable: "Tweet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserTweetLikes_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserTweetLikes_UserId",
                table: "UserTweetLikes",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserTweetLikes");

            migrationBuilder.AddColumn<int>(
                name: "LikeCount",
                table: "Tweet",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
