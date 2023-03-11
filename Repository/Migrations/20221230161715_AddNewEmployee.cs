using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CompanyEmployees.Repository.Migrations
{
    public partial class AddNewEmployee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmployeeId", "Age", "CompanyId", "Name", "Position" },
                values: new object[] { new Guid("2095a044-7caa-409e-8402-f90baaaf9cfc"), 39, new Guid("3d490a70-94ce-4d15-9494-5248280c2ce3"), "Mac Jeffery", "Project Manager" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValue: new Guid("2095a044-7caa-409e-8402-f90baaaf9cfc"));
        }
    }
}
