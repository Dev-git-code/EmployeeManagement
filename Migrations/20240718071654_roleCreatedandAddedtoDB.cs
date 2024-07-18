using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeManagement.Migrations
{
    public partial class roleCreatedandAddedtoDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "7c42fcc4-e233-4b12-bbe5-b83944cb068b", "a89e9ce0-5071-4fbb-b2c8-961891858fc7", "Administrator", null });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "f35cb023-8742-4ea7-b41b-ed6e404d93ce", "eaf8ef59-aa60-4f1c-bb75-96cdea6661cc", "Employee", null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7c42fcc4-e233-4b12-bbe5-b83944cb068b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f35cb023-8742-4ea7-b41b-ed6e404d93ce");
        }
    }
}
