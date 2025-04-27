using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManagementAPI.Migrations
{
    public partial class InitialCreation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Task",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AssignedUserId = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Task", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Task_Users_AssignedUserId",
                        column: x => x.AssignedUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TaskComment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaskId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskComment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskComment_Task_TaskId",
                        column: x => x.TaskId,
                        principalTable: "Task",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskComment_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "IsActive", "Password", "Role", "UserName" },
                values: new object[] { 1, new DateTime(2025, 4, 27, 11, 8, 19, 860, DateTimeKind.Utc).AddTicks(2865), "john.doe@example.com", true, "Mabc@123", "Manager", "john.doe" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "IsActive", "Password", "Role", "UserName" },
                values: new object[] { 2, new DateTime(2025, 4, 27, 11, 8, 19, 860, DateTimeKind.Utc).AddTicks(2871), "jane.smith@example.com", true, "Abc@123", "User", "jane.smith" });

            migrationBuilder.InsertData(
                table: "Task",
                columns: new[] { "Id", "AssignedUserId", "CreatedAt", "Description", "DueDate", "Status", "Title" },
                values: new object[] { 1, 1, new DateTime(2025, 4, 27, 11, 8, 19, 860, DateTimeKind.Utc).AddTicks(3127), "Setup project structure", new DateTime(2025, 4, 28, 11, 8, 19, 860, DateTimeKind.Utc).AddTicks(3128), "Pending", "Initial Setup" });

            migrationBuilder.InsertData(
                table: "Task",
                columns: new[] { "Id", "AssignedUserId", "CreatedAt", "Description", "DueDate", "Status", "Title" },
                values: new object[] { 2, 2, new DateTime(2025, 4, 27, 11, 8, 19, 860, DateTimeKind.Utc).AddTicks(3137), "Create database and apply migrations", new DateTime(2025, 4, 28, 11, 8, 19, 860, DateTimeKind.Utc).AddTicks(3138), "Pending", "Database Migration" });

            migrationBuilder.InsertData(
                table: "TaskComment",
                columns: new[] { "Id", "Comment", "CreatedAt", "TaskId", "UserId" },
                values: new object[] { 1, "Started working on project setup", new DateTime(2025, 4, 27, 11, 8, 19, 860, DateTimeKind.Utc).AddTicks(3167), 1, 1 });

            migrationBuilder.InsertData(
                table: "TaskComment",
                columns: new[] { "Id", "Comment", "CreatedAt", "TaskId", "UserId" },
                values: new object[] { 2, "Database created successfully", new DateTime(2025, 4, 27, 11, 8, 19, 860, DateTimeKind.Utc).AddTicks(3169), 2, 2 });

            migrationBuilder.CreateIndex(
                name: "IX_Task_AssignedUserId",
                table: "Task",
                column: "AssignedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskComment_TaskId",
                table: "TaskComment",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskComment_UserId",
                table: "TaskComment",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TaskComment");

            migrationBuilder.DropTable(
                name: "Task");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
