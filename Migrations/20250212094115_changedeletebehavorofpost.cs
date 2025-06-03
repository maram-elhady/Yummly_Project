using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyRecipeApp.Migrations
{
    /// <inheritdoc />
    public partial class changedeletebehavorofpost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostComments_Posts_PostId",
                table: "PostComments");

            migrationBuilder.AddForeignKey(
                name: "FK_PostComments_Posts_PostId",
                table: "PostComments",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.DropForeignKey(
               name: "FK_PostLikes_Posts_PostId",
               table: "PostLikes");

            migrationBuilder.AddForeignKey(
                name: "FK_PostLikes_Posts_PostId",
                table: "PostLikes",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostComments_Posts_PostId",
                table: "PostComments");


            migrationBuilder.DropForeignKey(
               name: "FK_PostLikes_Posts_PostId",
               table: "PostLikes");

            migrationBuilder.AddForeignKey(
                name: "FK_PostComments_Posts_PostId",
                table: "PostComments",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
               name: "FK_PostLikes_Posts_PostId",
               table: "PostLikes",
               column: "PostId",
               principalTable: "Posts",
               principalColumn: "Id",
               onDelete: ReferentialAction.Restrict);

        }
    }
}
