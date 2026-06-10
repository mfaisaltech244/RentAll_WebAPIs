using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentAll_WebAPIs.Migrations
{
    /// <inheritdoc />
    public partial class AddNewSeedUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "createdat", "email", "passwordhash", "role", "username" },
                values: new object[] { 5, new DateTime(2026, 6, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "m.faisaltech244@gmail.com", "g11egxQ0CrhSovl5q0zVPplNvjg2avtu7YT+SVe5gMg=", "Owner", "MUhammad Faisal" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "id",
                keyValue: 5);
        }
    }
}
