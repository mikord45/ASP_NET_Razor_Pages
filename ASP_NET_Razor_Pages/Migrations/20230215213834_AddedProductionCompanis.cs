using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASP_NET_Razor_Pages.Migrations
{
    public partial class AddedProductionCompanis : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductionCompanyId",
                table: "Movie",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ProductionCompany",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductionCompany", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Movie_ProductionCompanyId",
                table: "Movie",
                column: "ProductionCompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Movie_ProductionCompany_ProductionCompanyId",
                table: "Movie",
                column: "ProductionCompanyId",
                principalTable: "ProductionCompany",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movie_ProductionCompany_ProductionCompanyId",
                table: "Movie");

            migrationBuilder.DropTable(
                name: "ProductionCompany");

            migrationBuilder.DropIndex(
                name: "IX_Movie_ProductionCompanyId",
                table: "Movie");

            migrationBuilder.DropColumn(
                name: "ProductionCompanyId",
                table: "Movie");
        }
    }
}
