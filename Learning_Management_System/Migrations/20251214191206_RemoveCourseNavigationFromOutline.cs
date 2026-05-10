using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Learning_Management_System.Migrations
{
    /// <inheritdoc />
    public partial class RemoveCourseNavigationFromOutline : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseOutline_Courses_CourseId",
                table: "CourseOutline");

            migrationBuilder.DropIndex(
                name: "IX_CourseOutline_CourseId",
                table: "CourseOutline");

            migrationBuilder.AddColumn<int>(
                name: "CoursesCourseId",
                table: "CourseOutline",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CourseOutline_CoursesCourseId",
                table: "CourseOutline",
                column: "CoursesCourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseOutline_Courses_CoursesCourseId",
                table: "CourseOutline",
                column: "CoursesCourseId",
                principalTable: "Courses",
                principalColumn: "CourseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseOutline_Courses_CoursesCourseId",
                table: "CourseOutline");

            migrationBuilder.DropIndex(
                name: "IX_CourseOutline_CoursesCourseId",
                table: "CourseOutline");

            migrationBuilder.DropColumn(
                name: "CoursesCourseId",
                table: "CourseOutline");

            migrationBuilder.CreateIndex(
                name: "IX_CourseOutline_CourseId",
                table: "CourseOutline",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseOutline_Courses_CourseId",
                table: "CourseOutline",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
