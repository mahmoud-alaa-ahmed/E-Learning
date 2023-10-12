using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLayer.Migrations
{
    /// <inheritdoc />
    public partial class updates2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_UserSubscribtions_UserSubscribtionId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscribtions_UserSubscribtions_UserSubscribtionId",
                table: "Subscribtions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserSubscribtions",
                table: "UserSubscribtions");

            migrationBuilder.DropIndex(
                name: "IX_Subscribtions_UserSubscribtionId",
                table: "Subscribtions");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_UserSubscribtionId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "UserSubscribtions");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "UserSubscribtions");

            migrationBuilder.DropColumn(
                name: "UserSubscribtionId",
                table: "Subscribtions");

            migrationBuilder.DropColumn(
                name: "UserSubscribtionId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UserSubscribtions",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserSubscribtions",
                table: "UserSubscribtions",
                columns: new[] { "UserId", "SubscribtionId" });

            migrationBuilder.CreateTable(
                name: "UserCourseEnrollments",
                columns: table => new
                {
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCourseEnrollments", x => new { x.CourseId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserCourseEnrollments_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserCourseEnrollments_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserSubscribtions_SubscribtionId",
                table: "UserSubscribtions",
                column: "SubscribtionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCourseEnrollments_UserId",
                table: "UserCourseEnrollments",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSubscribtions_AspNetUsers_UserId",
                table: "UserSubscribtions",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSubscribtions_Subscribtions_SubscribtionId",
                table: "UserSubscribtions",
                column: "SubscribtionId",
                principalTable: "Subscribtions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserSubscribtions_AspNetUsers_UserId",
                table: "UserSubscribtions");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSubscribtions_Subscribtions_SubscribtionId",
                table: "UserSubscribtions");

            migrationBuilder.DropTable(
                name: "UserCourseEnrollments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserSubscribtions",
                table: "UserSubscribtions");

            migrationBuilder.DropIndex(
                name: "IX_UserSubscribtions_SubscribtionId",
                table: "UserSubscribtions");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UserSubscribtions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "UserSubscribtions",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "UserSubscribtions",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

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

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserSubscribtions",
                table: "UserSubscribtions",
                column: "Id");

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
    }
}
