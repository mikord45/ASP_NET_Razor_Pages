using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASP_NET_Razor_Pages.Migrations
{
    public partial class CreatedRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Movie");

            migrationBuilder.AddColumn<int>(
                name: "RatingId",
                table: "Movie",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Movie_RatingId",
                table: "Movie",
                column: "RatingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Movie_Rating_RatingId",
                table: "Movie",
                column: "RatingId",
                principalTable: "Rating",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movie_Rating_RatingId",
                table: "Movie");

            migrationBuilder.DropIndex(
                name: "IX_Movie_RatingId",
                table: "Movie");

            migrationBuilder.DropColumn(
                name: "RatingId",
                table: "Movie");

            migrationBuilder.AddColumn<string>(
                name: "Rating",
                table: "Movie",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: false,
                defaultValue: "");
        }
    }
}
