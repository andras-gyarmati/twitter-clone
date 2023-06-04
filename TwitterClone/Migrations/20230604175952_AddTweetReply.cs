using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwitterClone.Migrations
{
    /// <inheritdoc />
    public partial class AddTweetReply : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InReplyToId",
                table: "Tweet",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tweet_InReplyToId",
                table: "Tweet",
                column: "InReplyToId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tweet_Tweet_InReplyToId",
                table: "Tweet",
                column: "InReplyToId",
                principalTable: "Tweet",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tweet_Tweet_InReplyToId",
                table: "Tweet");

            migrationBuilder.DropIndex(
                name: "IX_Tweet_InReplyToId",
                table: "Tweet");

            migrationBuilder.DropColumn(
                name: "InReplyToId",
                table: "Tweet");
        }
    }
}
