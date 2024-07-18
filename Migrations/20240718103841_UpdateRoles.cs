using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeManagement.Migrations
{
    public partial class UpdateRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0c0e96e8-651d-470a-9c71-55997b7550c7");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "979b9852-5334-496a-a7c7-8859cc2d54e8");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "0c0e96e8-651d-470a-9c71-55997b7550c7", "8113a7a7-c7be-436e-aaad-aa2c9f7c4ee0", "Administrator", null });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "979b9852-5334-496a-a7c7-8859cc2d54e8", "c3ac9656-413b-4ee5-9172-4249868636c4", "Employee", null });
        }
    }
}
