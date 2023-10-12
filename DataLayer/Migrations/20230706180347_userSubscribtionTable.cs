using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class userSubscribtionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserSubscribtionId",
                table: "Subscribtions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserSubscribtionId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UserSubscribtions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubscribtionId = table.Column<int>(type: "int", nullable: false),
                    StartedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsSubscribed = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSubscribtions", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Subscribtions",
                keyColumn: "Id",
                keyValue: 1,
                column: "UserSubscribtionId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Subscribtions",
                keyColumn: "Id",
                keyValue: 2,
                column: "UserSubscribtionId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Subscribtions",
                keyColumn: "Id",
                keyValue: 3,
                column: "UserSubscribtionId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_Subscribtions_UserSubscribtionId",
                table: "Subscribtions",
                column: "UserSubscribtionId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_UserSubscribtionId",
                table: "AspNetUsers",
                column: "UserSubscribtionId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_UserSubscribtions_UserSubscribtionId",
                table: "AspNetUsers",
                column: "UserSubscribtionId",
                principalTable: "UserSubscribtions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscribtions_UserSubscribtions_UserSubscribtionId",
                table: "Subscribtions",
                column: "UserSubscribtionId",
                principalTable: "UserSubscribtions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_UserSubscribtions_UserSubscribtionId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscribtions_UserSubscribtions_UserSubscribtionId",
                table: "Subscribtions");

            migrationBuilder.DropTable(
                name: "UserSubscribtions");

            migrationBuilder.DropIndex(
                name: "IX_Subscribtions_UserSubscribtionId",
                table: "Subscribtions");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_UserSubscribtionId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserSubscribtionId",
                table: "Subscribtions");

            migrationBuilder.DropColumn(
                name: "UserSubscribtionId",
                table: "AspNetUsers");
        }
    }
}
