using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Hotel_Management_API.Migrations
{
    /// <inheritdoc />
    public partial class AddAdminAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AppUsers",
                columns: new[] { "Id", "FullName", "IsActive", "PasswordHash", "Role", "Username" },
                values: new object[] { 999, "Quan tri vien 1", true, "$2a$12$ANVS8jHjMjiekgW9gwHjAeR2dCKRP20cx4QUcgY8tmhCx2RCSdUxe", "Admin", "Admin1" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AppUsers",
                keyColumn: "Id",
                keyValue: 999);
        }
    }
}
