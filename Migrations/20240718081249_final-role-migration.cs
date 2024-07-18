using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeManagement.Migrations
{
    public partial class finalrolemigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7c42fcc4-e233-4b12-bbe5-b83944cb068b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f35cb023-8742-4ea7-b41b-ed6e404d93ce");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "0c0e96e8-651d-470a-9c71-55997b7550c7", "8113a7a7-c7be-436e-aaad-aa2c9f7c4ee0", "Administrator", null });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "979b9852-5334-496a-a7c7-8859cc2d54e8", "c3ac9656-413b-4ee5-9172-4249868636c4", "Employee", null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0c0e96e8-651d-470a-9c71-55997b7550c7");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "979b9852-5334-496a-a7c7-8859cc2d54e8");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "7c42fcc4-e233-4b12-bbe5-b83944cb068b", "a89e9ce0-5071-4fbb-b2c8-961891858fc7", "Administrator", null });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "f35cb023-8742-4ea7-b41b-ed6e404d93ce", "eaf8ef59-aa60-4f1c-bb75-96cdea6661cc", "Employee", null });
        }
    }
}
