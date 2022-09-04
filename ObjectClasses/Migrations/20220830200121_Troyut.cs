using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ObjectClasses.Migrations
{
    public partial class Troyut : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    Level = table.Column<string>(nullable: true),
                    MealType = table.Column<string>(nullable: true),
                    Department = table.Column<string>(nullable: true),
                    IsExpired = table.Column<bool>(nullable: false),
                    IsEligible = table.Column<bool>(nullable: false),
                    MealsClaimed = table.Column<int>(nullable: false),
                    LastClaimed = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Employees");
        }
    }
}
