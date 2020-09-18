using Microsoft.EntityFrameworkCore.Migrations;

namespace TrashCollection.Data.Migrations
{
    public partial class employeetable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f41cb9f1-c23a-4761-8269-bbc7befbc412");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "19e1922d-46dd-42c3-89a6-2d7c690f0424", "b7ce4ee9-11d8-4287-9202-45dd4e22d45c", "Admin", "ADMIN" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "19e1922d-46dd-42c3-89a6-2d7c690f0424");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "f41cb9f1-c23a-4761-8269-bbc7befbc412", "81c27b94-a083-497c-9292-4682530e8ac7", "Admin", "ADMIN" });
        }
    }
}
