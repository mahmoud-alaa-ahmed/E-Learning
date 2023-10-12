using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class deletesubscription2tables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserSubscribtions");

            migrationBuilder.DropTable(
                name: "Subscribtions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Subscribtions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DurationInDays = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscribtions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserSubscribtions",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SubscribtionId = table.Column<int>(type: "int", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSubscribtions", x => new { x.UserId, x.SubscribtionId });
                    table.ForeignKey(
                        name: "FK_UserSubscribtions_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserSubscribtions_Subscribtions_SubscribtionId",
                        column: x => x.SubscribtionId,
                        principalTable: "Subscribtions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Subscribtions",
                columns: new[] { "Id", "DurationInDays", "Name" },
                values: new object[,]
                {
                    { 1, 30, "Three Months Subscribtion" },
                    { 2, 90, "Three Months Subscribtion" },
                    { 3, 360, "One Year Subscribtion " }
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserSubscribtions_SubscribtionId",
                table: "UserSubscribtions",
                column: "SubscribtionId");
        }
    }
}
